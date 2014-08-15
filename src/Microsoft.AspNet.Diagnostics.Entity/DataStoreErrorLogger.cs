// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Microsoft.Data.Entity.Storage;
using Microsoft.Framework.Logging;
using System;

namespace Microsoft.AspNet.Diagnostics.Entity
{
    public class DataStoreErrorLogger : ILogger
    {
        public Type LastErrorContextType { get; set; }
        public Exception LastError { get; set; }

        public IDisposable BeginScope(object state)
        {
            return null;
        }

        public bool WriteCore(TraceType eventType, int eventId, object state, Exception exception, Func<object, Exception, string> formatter)
        {
            var errorState = state as DataStoreErrorLogState;
            if (errorState != null)
            {
                LastError = exception;
                LastErrorContextType = errorState.ContextType;
            }

            return true;
        }
    }
}