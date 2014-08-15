// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using JetBrains.Annotations;
using Microsoft.Data.Entity.Utilities;

namespace Microsoft.Data.Entity.Storage
{
    public class DataStoreErrorLogState
    {
        private Type _contextType;

        public virtual Type ContextType
        {
            get { return _contextType; }
            set
            {
                Check.NotNull(value, "value");
                _contextType = value;
            }
        }
    }
}
