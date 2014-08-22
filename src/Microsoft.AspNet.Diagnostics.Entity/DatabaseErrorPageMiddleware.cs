// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using JetBrains.Annotations;
using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Diagnostics.Entity.Utilities;
using Microsoft.AspNet.Diagnostics.Entity.Views;
using Microsoft.AspNet.Http;
using Microsoft.Data.Entity;
using Microsoft.Data.Entity.Migrations;
using Microsoft.Data.Entity.Migrations.Infrastructure;
using Microsoft.Data.Entity.Relational;
using Microsoft.Data.Entity.Storage;
using Microsoft.Framework.DependencyInjection;
using Microsoft.Framework.Logging;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace Microsoft.AspNet.Diagnostics.Entity
{
    public class DatabaseErrorPageMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly DatabaseErrorPageOptions _options;
        private readonly DataStoreErrorLoggerProvider _loggerProvider;

        public DatabaseErrorPageMiddleware([NotNull] RequestDelegate next, [NotNull] ILoggerFactory loggerFactory, [NotNull] DatabaseErrorPageOptions options, bool isDevMode)
        {
            Check.NotNull(next, "next");
            Check.NotNull(options, "options");

            if (isDevMode)
            {
                options.SetDefaultVisibility(isVisible: true);
            }

            _next = next;
            _options = options;

            _loggerProvider = new DataStoreErrorLoggerProvider();
            loggerFactory.AddProvider(_loggerProvider);
        }

        public virtual async Task Invoke([NotNull] HttpContext context)
        {
            Check.NotNull(context, "context");

            _loggerProvider.Logger = new DataStoreErrorLogger();

            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                if (_loggerProvider.Logger.LastError == ex)
                {
                    var dbContext = (DbContext)context.RequestServices.GetService(_loggerProvider.Logger.LastErrorContextType);

                    if (dbContext.Database is RelationalDatabase)
                    {
                        var databaseExists = dbContext.Database.AsRelational().Exists();

                        var serviceProvider = dbContext.Configuration.Services.ServiceProvider;

                        var migrator = serviceProvider.GetService<DbMigrator>();

                        var pendingMigrations = databaseExists
                            ? migrator.GetPendingMigrations().Select(m => m.MigrationId)
                            : migrator.GetLocalMigrations().Select(m => m.MigrationId);

                        var pendingModelChanges = true;
                        var differ = serviceProvider.GetService<ModelDiffer>();
                        var migrationsAssembly = serviceProvider.GetService<MigrationAssembly>();
                        var snapshot = migrationsAssembly.Model;
                        if (snapshot != null)
                        {
                            pendingModelChanges = differ.Diff(snapshot, dbContext.Model).Any();
                        }

                        if ((!databaseExists && pendingMigrations.Any()) || pendingMigrations.Any() || pendingModelChanges)
                        {
                            var page = new DatabaseErrorPage();
                            page.Model = new DatabaseErrorPageModel
                            {
                                Options = _options,
                                Exception = ex,
                                ContextType = dbContext.GetType(),
                                DatabaseExists = databaseExists,
                                PendingMigrations = pendingMigrations,
                                PendingModelChanges = pendingModelChanges
                            };

                            // TODO Building in VS2013 prevents await in catch block
                            //      swap to await once we move to just VS14
                            page.ExecuteAsync(context).Wait();
                            return;
                        }
                    }
                }

                throw;
            }
        }
    }
}
