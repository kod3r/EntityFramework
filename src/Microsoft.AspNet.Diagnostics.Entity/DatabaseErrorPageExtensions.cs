// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using JetBrains.Annotations;
using Microsoft.AspNet.Diagnostics.Entity;
using Microsoft.AspNet.Diagnostics.Entity.Utilities;
using Microsoft.AspNet.Builder;
using System;

namespace Microsoft.AspNet.Builder
{
    public static class DatabaseErrorPageExtensions
    {
        public static IBuilder UseDatabaseErrorPage([NotNull] this IBuilder builder)
        {
            Check.NotNull(builder, "builder");

            return builder.UseDatabaseErrorPage(new DatabaseErrorPageOptions());
        }

        public static IBuilder UseDatabaseErrorPage([NotNull] this IBuilder builder, [NotNull] DatabaseErrorPageOptions options)
        {
            Check.NotNull(builder, "builder");
            Check.NotNull(options, "options");

            /* TODO: Development, Staging, or Production
            string appMode = new AppProperties(builder.Properties).Get<string>(Constants.HostAppMode);
            bool isDevMode = string.Equals(Constants.DevMode, appMode, StringComparison.Ordinal);*/
            bool isDevMode = true;
            return builder.Use(next => new DatabaseErrorPageMiddleware(next, options, isDevMode).Invoke);
        }
    }
}