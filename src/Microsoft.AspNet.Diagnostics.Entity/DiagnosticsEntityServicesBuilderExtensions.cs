// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using JetBrains.Annotations;
using Microsoft.AspNet.Diagnostics.Entity.Utilities;
using Microsoft.Data.Entity;
using Microsoft.Framework.DependencyInjection;
using Microsoft.Framework.Logging;

namespace Microsoft.AspNet.Diagnostics.Entity
{
    public static class DiagnosticsEntityServicesBuilderExtensions
    {
        public static EntityServicesBuilder AddDiagnostics([NotNull] this EntityServicesBuilder builder)
        {
            Check.NotNull(builder, "builder");

            builder.ServiceCollection.AddScoped<ILoggerFactory, DataStoreErrorLoggerFactory>();

            return builder;
        }
    }
}