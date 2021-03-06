// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Data.Common;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Data.Entity.Relational.FunctionalTests;
using Microsoft.Data.SQLite;

namespace Microsoft.Data.Entity.SQLite.FunctionalTests
{
    public class SQLiteTestDatabase : RelationalTestStore
    {
        private readonly Data.SQLite.SQLiteConnection _connection;
        private SQLiteTransaction _transaction;
        private static int _scratchCount;

        public SQLiteTestDatabase(string connectionString)
        {
            _connection = new Data.SQLite.SQLiteConnection(connectionString);
        }

        public override DbConnection Connection
        {
            get { return _connection; }
        }

        private Task<SQLiteTestDatabase> CreateShared()
        {
            _connection.Open();
            _transaction = _connection.BeginTransaction();

            return Task.FromResult(this);
        }

        private Task<SQLiteTestDatabase> CreateScratch()
        {
            _connection.Open();

            return Task.FromResult(this);
        }

        /// <summary>
        ///     A non-transactional, transient, isolated test database. Use this in the case
        ///     where transactions are not appropriate.
        /// </summary>
        public static Task<SQLiteTestDatabase> Scratch()
        {
            var scratchName = "Scratch_" + Interlocked.Increment(ref _scratchCount);

            var connectionStringBuilder = new SQLiteConnectionStringBuilder();
            connectionStringBuilder.Filename = "file:" + scratchName + "?mode=memory&cache=shared";
            connectionStringBuilder.Uri = true;
            return new SQLiteTestDatabase(connectionStringBuilder.ConnectionString).CreateScratch();
        }

        public static Task<SQLiteTestDatabase> Northwind()
        {
            return new SQLiteTestDatabase("Filename=northwind.db").CreateShared();
        }

        public override void Dispose()
        {
            if (_transaction != null)
            {
                _transaction.Dispose();
            }
            _connection.Dispose();
        }
    }
}
