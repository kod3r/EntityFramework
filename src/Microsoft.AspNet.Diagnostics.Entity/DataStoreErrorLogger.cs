// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Microsoft.Data.Entity.Storage;
using Microsoft.Framework.Logging;
using System;

namespace Microsoft.AspNet.Diagnostics.Entity
{
    public class DataStoreErrorLogger : ILogger
    {
        private Type _lastErrorContextType;
        private Exception _lastError;

        public Type LastErrorContextType
        {
            get { return _lastErrorContextType; }
        }

        public Exception LastError
        {
            get { return _lastError; }
        }

        public IDisposable BeginScope(object state)
        {
            return NullScope.Instance; 
        }

        public bool WriteCore(TraceType eventType, int eventId, object state, Exception exception, Func<object, Exception, string> formatter)
        {
            var errorState = state as DataStoreErrorLogState;
            if (errorState != null)
            {
                _lastError = exception;
                _lastErrorContextType = errorState.ContextType;
            }

            return true;
        }

        private class NullScope : IDisposable
        {
            public static NullScope Instance = new NullScope();

            public void Dispose()
            {
                // intentionally does nothing 
            }
        }

    }
}