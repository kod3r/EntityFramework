// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Microsoft.Framework.Logging;
#if K10
using System.Threading;
#else
using System.Runtime.Remoting.Messaging;
#endif

namespace Microsoft.AspNet.Diagnostics.Entity
{
    public class DataStoreErrorLoggerProvider : ILoggerProvider
    {
#if K10
        private readonly AsyncLocal<SqlLogger> _logger = new AsyncLocal<SqlLogger>(); 
#else
        private const string ContextName = "__DataStoreErrorLogger";
#endif

        public ILogger Create(string name)
        {
            // TODO Consider returning NullLogger for classes we don't care about
            return Logger;
        }

        public DataStoreErrorLogger Logger
        {
            get
            {
#if K10
                 return _logger.Value; 
#else
                return (DataStoreErrorLogger)CallContext.LogicalGetData(ContextName);
#endif
            }
            set
            {
#if K10
                _logger.Value = value; 
#else
                CallContext.LogicalSetData(ContextName, value);
#endif
            }
        }
    }
}