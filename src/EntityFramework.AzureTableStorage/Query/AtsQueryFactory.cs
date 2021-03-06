﻿// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using JetBrains.Annotations;
using Microsoft.Data.Entity.AzureTableStorage.Utilities;
using Microsoft.Data.Entity.ChangeTracking;
using Microsoft.Data.Entity.Metadata;
using Microsoft.Framework.Logging;

namespace Microsoft.Data.Entity.AzureTableStorage.Query
{
    public class AtsQueryFactory
    {
        private readonly AtsValueReaderFactory _valueReaderFactory;

        public AtsQueryFactory([NotNull] AtsValueReaderFactory valueReaderFactory)
        {
            Check.NotNull(valueReaderFactory, "valueReaderFactory");
            _valueReaderFactory = valueReaderFactory;
        }

        public virtual AtsQueryCompilationContext MakeCompilationContext([NotNull] IModel model)
        {
            return new AtsQueryCompilationContext(model);
        }

        public virtual AtsQueryContext MakeQueryContext([NotNull] IModel model,
            [NotNull] ILogger logger,
            [NotNull] StateManager stateManager,
            [NotNull] AtsConnection connection)
        {
            return new AtsQueryContext(model, logger, stateManager, connection, _valueReaderFactory);
        }
    }
}
