// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Linq;
using System.Text;
using Microsoft.Data.Entity.ChangeTracking;
using Microsoft.Data.Entity.Metadata;
using Microsoft.Data.Entity.Relational.Update;
using Moq;
using Xunit;

namespace Microsoft.Data.Entity.Relational.Tests
{
    public abstract class SqlGeneratorTestBase
    {
        [Fact]
        public void AppendDeleteOperation_creates_full_delete_command_text()
        {
            var stringBuilder = new StringBuilder();
            var operations = CreateDeleteOperations(concurrencyToken: false);

            CreateSqlGenerator().AppendDeleteOperation(stringBuilder, new SchemaQualifiedName("Ducks", "dbo"), operations);

            Assert.Equal(
                "DELETE FROM " + OpenDelimeter + "dbo" + CloseDelimeter + "." + OpenDelimeter + "Ducks" + CloseDelimeter + "" + Environment.NewLine +
                "WHERE " + OpenDelimeter + "Id" + CloseDelimeter + " = @p0;" + Environment.NewLine +
                "SELECT " + RowsAffected + ";" + Environment.NewLine,
                stringBuilder.ToString());
        }

        [Fact]
        public void AppendDeleteOperation_creates_full_delete_command_text_with_concurrency_check()
        {
            var stringBuilder = new StringBuilder();
            var operations = CreateDeleteOperations(concurrencyToken: true);

            CreateSqlGenerator().AppendDeleteOperation(stringBuilder, "Ducks", operations);

            Assert.Equal(
                "DELETE FROM " + OpenDelimeter + "Ducks" + CloseDelimeter + "" + Environment.NewLine +
                "WHERE " + OpenDelimeter + "Id" + CloseDelimeter + " = @p0 AND " + OpenDelimeter + "ConcurrencyToken" + CloseDelimeter + " = @p1;" + Environment.NewLine +
                "SELECT " + RowsAffected + ";" + Environment.NewLine,
                stringBuilder.ToString());
        }

        [Fact]
        public void AppendInsertOperation_appends_insert_and_select_and_where_if_store_generated_columns_exist()
        {
            var stringBuilder = new StringBuilder();
            var operations = CreateInsertOperations(identityKey: true, computedProperty: true);

            CreateSqlGenerator().AppendInsertOperation(stringBuilder, new SchemaQualifiedName("Ducks", "dbo"), operations);

            AppendInsertOperation_appends_insert_and_select_and_where_if_store_generated_columns_exist_verification(stringBuilder);
        }

        protected virtual void AppendInsertOperation_appends_insert_and_select_and_where_if_store_generated_columns_exist_verification(StringBuilder stringBuilder)
        {
            Assert.Equal(
                "INSERT INTO " + OpenDelimeter + "dbo" + CloseDelimeter + "." + OpenDelimeter + "Ducks" + CloseDelimeter + " (" + OpenDelimeter + "Name" + CloseDelimeter + ", " + OpenDelimeter + "Quacks" + CloseDelimeter + ", " + OpenDelimeter + "ConcurrencyToken" + CloseDelimeter + ")" + Environment.NewLine +
                "VALUES (@p0, @p1, @p2);" + Environment.NewLine +
                "SELECT " + OpenDelimeter + "Id" + CloseDelimeter + ", " + OpenDelimeter + "Computed" + CloseDelimeter + "" + Environment.NewLine +
                "FROM " + OpenDelimeter + "dbo" + CloseDelimeter + "." + OpenDelimeter + "Ducks" + CloseDelimeter + "" + Environment.NewLine +
                "WHERE " + RowsAffected + " = 1 AND " + OpenDelimeter + "Id" + CloseDelimeter + " = " + Identity + ";" + Environment.NewLine,
                stringBuilder.ToString());
        }

        [Fact]
        public void AppendInsertOperation_appends_insert_and_select_rowcount_if_no_store_generated_columns_exist_or_conditions_exist()
        {
            var stringBuilder = new StringBuilder();
            var operations = CreateInsertOperations(identityKey: false, computedProperty: false);

            CreateSqlGenerator().AppendInsertOperation(stringBuilder, "Ducks", operations);

            Assert.Equal(
                "INSERT INTO " + OpenDelimeter + "Ducks" + CloseDelimeter + " (" + OpenDelimeter + "Id" + CloseDelimeter + ", " + OpenDelimeter + "Name" + CloseDelimeter + ", " + OpenDelimeter + "Quacks" + CloseDelimeter + ", " + OpenDelimeter + "ConcurrencyToken" + CloseDelimeter + ")" + Environment.NewLine +
                "VALUES (@p0, @p1, @p2, @p3);" + Environment.NewLine +
                "SELECT " + RowsAffected + ";" + Environment.NewLine,
                stringBuilder.ToString());
        }

        [Fact]
        public void AppendInsertOperation_appends_insert_and_select_store_generated_columns_but_no_identity()
        {
            var stringBuilder = new StringBuilder();
            var operations = CreateInsertOperations(identityKey: false, computedProperty: true);

            CreateSqlGenerator().AppendInsertOperation(stringBuilder, "Ducks", operations);

            AppendInsertOperation_appends_insert_and_select_store_generated_columns_but_no_identity_verification(stringBuilder);
        }

        protected virtual void AppendInsertOperation_appends_insert_and_select_store_generated_columns_but_no_identity_verification(StringBuilder stringBuilder)
        {
            Assert.Equal(
                "INSERT INTO " + OpenDelimeter + "Ducks" + CloseDelimeter + " (" + OpenDelimeter + "Id" + CloseDelimeter + ", " + OpenDelimeter + "Name" + CloseDelimeter + ", " + OpenDelimeter + "Quacks" + CloseDelimeter + ", " + OpenDelimeter + "ConcurrencyToken" + CloseDelimeter + ")" + Environment.NewLine +
                "VALUES (@p0, @p1, @p2, @p3);" + Environment.NewLine +
                "SELECT " + OpenDelimeter + "Computed" + CloseDelimeter + "" + Environment.NewLine +
                "FROM " + OpenDelimeter + "Ducks" + CloseDelimeter + "" + Environment.NewLine +
                "WHERE " + RowsAffected + " = 1 AND " + OpenDelimeter + "Id" + CloseDelimeter + " = @p0;" + Environment.NewLine,
                stringBuilder.ToString());
        }

        [Fact]
        public void AppendInsertOperation_appends_insert_and_select_for_only_identity()
        {
            var stringBuilder = new StringBuilder();
            var operations = CreateInsertOperations(identityKey: true, computedProperty: false);

            CreateSqlGenerator().AppendInsertOperation(stringBuilder, "Ducks", operations);

            AppendInsertOperation_appends_insert_and_select_for_only_identity_verification(stringBuilder);
        }

        protected virtual void AppendInsertOperation_appends_insert_and_select_for_only_identity_verification(StringBuilder stringBuilder)
        {
            Assert.Equal(
                "INSERT INTO " + OpenDelimeter + "Ducks" + CloseDelimeter + " (" + OpenDelimeter + "Name" + CloseDelimeter + ", " + OpenDelimeter + "Quacks" + CloseDelimeter + ", " + OpenDelimeter + "ConcurrencyToken" + CloseDelimeter + ")" + Environment.NewLine +
                "VALUES (@p0, @p1, @p2);" + Environment.NewLine +
                "SELECT " + OpenDelimeter + "Id" + CloseDelimeter + "" + Environment.NewLine +
                "FROM " + OpenDelimeter + "Ducks" + CloseDelimeter + "" + Environment.NewLine +
                "WHERE " + RowsAffected + " = 1 AND " + OpenDelimeter + "Id" + CloseDelimeter + " = " + Identity + ";" + Environment.NewLine,
                stringBuilder.ToString());
        }

        [Fact]
        public void AppendInsertOperation_appends_insert_and_select_for_all_store_generated_columns()
        {
            var stringBuilder = new StringBuilder();
            var operations = CreateInsertOperations(identityKey: true, computedProperty: true).Where(p => !p.IsWrite).ToArray();

            CreateSqlGenerator().AppendInsertOperation(stringBuilder, "Ducks", operations);

            AppendInsertOperation_appends_insert_and_select_for_all_store_generated_columns_verification(stringBuilder);
        }

        protected virtual void AppendInsertOperation_appends_insert_and_select_for_all_store_generated_columns_verification(StringBuilder stringBuilder)
        {
            Assert.Equal(
                "INSERT INTO " + OpenDelimeter + "Ducks" + CloseDelimeter + "" + Environment.NewLine +
                "DEFAULT VALUES;" + Environment.NewLine +
                "SELECT " + OpenDelimeter + "Id" + CloseDelimeter + ", " + OpenDelimeter + "Computed" + CloseDelimeter + "" + Environment.NewLine +
                "FROM " + OpenDelimeter + "Ducks" + CloseDelimeter + "" + Environment.NewLine +
                "WHERE " + RowsAffected + " = 1 AND " + OpenDelimeter + "Id" + CloseDelimeter + " = " + Identity + ";" + Environment.NewLine,
                stringBuilder.ToString());
        }

        [Fact]
        public void AppendInsertOperation_appends_insert_and_select_for_only_single_identity_columns()
        {
            var stringBuilder = new StringBuilder();
            var operations = CreateInsertOperations(identityKey: true, computedProperty: false).Where(p => !p.IsWrite).ToArray();

            CreateSqlGenerator().AppendInsertOperation(stringBuilder, "Ducks", operations);

            AppendInsertOperation_appends_insert_and_select_for_only_single_identity_columns_verification(stringBuilder);
        }

        protected virtual void AppendInsertOperation_appends_insert_and_select_for_only_single_identity_columns_verification(StringBuilder stringBuilder)
        {
            Assert.Equal(
                "INSERT INTO " + OpenDelimeter + "Ducks" + CloseDelimeter + "" + Environment.NewLine +
                "DEFAULT VALUES;" + Environment.NewLine +
                "SELECT " + OpenDelimeter + "Id" + CloseDelimeter + "" + Environment.NewLine +
                "FROM " + OpenDelimeter + "Ducks" + CloseDelimeter + "" + Environment.NewLine +
                "WHERE " + RowsAffected + " = 1 AND " + OpenDelimeter + "Id" + CloseDelimeter + " = " + Identity + ";" + Environment.NewLine,
                stringBuilder.ToString());
        }

        [Fact]
        public void AppendUpdateOperation_appends_update_and_select_if_store_generated_columns_exist()
        {
            var stringBuilder = new StringBuilder();
            var operations = CreateUpdateOperations(computedProperty: true, concurrencyToken: true);

            CreateSqlGenerator().AppendUpdateOperation(stringBuilder, "Ducks", operations);

            AppendUpdateOperation_appends_update_and_select_if_store_generated_columns_exist_verification(stringBuilder);
        }

        protected virtual void AppendUpdateOperation_appends_update_and_select_if_store_generated_columns_exist_verification(StringBuilder stringBuilder)
        {
            Assert.Equal(
                "UPDATE " + OpenDelimeter + "Ducks" + CloseDelimeter + " SET " + OpenDelimeter + "Name" + CloseDelimeter + " = @p0, " + OpenDelimeter + "Quacks" + CloseDelimeter + " = @p1, " + OpenDelimeter + "ConcurrencyToken" + CloseDelimeter + " = @p2" + Environment.NewLine +
                "WHERE " + OpenDelimeter + "Id" + CloseDelimeter + " = @p3 AND " + OpenDelimeter + "ConcurrencyToken" + CloseDelimeter + " = @p4;" + Environment.NewLine +
                "SELECT " + OpenDelimeter + "Computed" + CloseDelimeter + "" + Environment.NewLine +
                "FROM " + OpenDelimeter + "Ducks" + CloseDelimeter + "" + Environment.NewLine +
                "WHERE " + RowsAffected + " = 1 AND " + OpenDelimeter + "Id" + CloseDelimeter + " = @p3;" + Environment.NewLine,
                stringBuilder.ToString());
        }

        [Fact]
        public void AppendUpdateOperation_appends_update_and_select_rowcount_if_store_generated_columns_dont_exist()
        {
            var stringBuilder = new StringBuilder();
            var operations = CreateUpdateOperations(computedProperty: false, concurrencyToken: false);

            CreateSqlGenerator().AppendUpdateOperation(stringBuilder, new SchemaQualifiedName("Ducks", "dbo"), operations);

            Assert.Equal(
                "UPDATE " + OpenDelimeter + "dbo" + CloseDelimeter + "." + OpenDelimeter + "Ducks" + CloseDelimeter + " SET " + OpenDelimeter + "Name" + CloseDelimeter + " = @p0, " + OpenDelimeter + "Quacks" + CloseDelimeter + " = @p1, " + OpenDelimeter + "ConcurrencyToken" + CloseDelimeter + " = @p2" + Environment.NewLine +
                "WHERE " + OpenDelimeter + "Id" + CloseDelimeter + " = @p3;" + Environment.NewLine +
                "SELECT " + RowsAffected + ";" + Environment.NewLine,
                stringBuilder.ToString());
        }

        [Fact]
        public void AppendUpdateOperation_appends_where_for_concurrency_token()
        {
            var stringBuilder = new StringBuilder();
            var operations = CreateUpdateOperations(computedProperty: false, concurrencyToken: true);

            CreateSqlGenerator().AppendUpdateOperation(stringBuilder, "Ducks", operations);

            Assert.Equal(
                "UPDATE " + OpenDelimeter + "Ducks" + CloseDelimeter + " SET " + OpenDelimeter + "Name" + CloseDelimeter + " = @p0, " + OpenDelimeter + "Quacks" + CloseDelimeter + " = @p1, " + OpenDelimeter + "ConcurrencyToken" + CloseDelimeter + " = @p2" + Environment.NewLine +
                "WHERE " + OpenDelimeter + "Id" + CloseDelimeter + " = @p3 AND " + OpenDelimeter + "ConcurrencyToken" + CloseDelimeter + " = @p4;" + Environment.NewLine +
                "SELECT " + RowsAffected + ";" + Environment.NewLine,
                stringBuilder.ToString());
        }

        [Fact]
        public void AppendUpdateOperation_appends_select_for_computed_property()
        {
            var stringBuilder = new StringBuilder();
            var operations = CreateUpdateOperations(computedProperty: true, concurrencyToken: false);

            CreateSqlGenerator().AppendUpdateOperation(stringBuilder, "Ducks", operations);

            AppendUpdateOperation_appends_select_for_computed_property_verification(stringBuilder);
        }

        protected virtual void AppendUpdateOperation_appends_select_for_computed_property_verification(StringBuilder stringBuilder)
        {
            Assert.Equal(
                "UPDATE " + OpenDelimeter + "Ducks" + CloseDelimeter + " SET " + OpenDelimeter + "Name" + CloseDelimeter + " = @p0, " + OpenDelimeter + "Quacks" + CloseDelimeter + " = @p1, " + OpenDelimeter + "ConcurrencyToken" + CloseDelimeter + " = @p2" + Environment.NewLine +
                "WHERE " + OpenDelimeter + "Id" + CloseDelimeter + " = @p3;" + Environment.NewLine +
                "SELECT " + OpenDelimeter + "Computed" + CloseDelimeter + "" + Environment.NewLine +
                "FROM " + OpenDelimeter + "Ducks" + CloseDelimeter + "" + Environment.NewLine +
                "WHERE " + RowsAffected + " = 1 AND " + OpenDelimeter + "Id" + CloseDelimeter + " = @p3;" + Environment.NewLine,
                stringBuilder.ToString());
        }

        [Fact]
        public void Default_BatchCommandSeparator_is_semicolon()
        {
            Assert.Equal(";", CreateSqlGenerator().BatchCommandSeparator);
        }

        protected abstract SqlGenerator CreateSqlGenerator();

        protected abstract string RowsAffected { get; }

        protected abstract string Identity { get; }

        protected IProperty CreateMockProperty(string name)
        {
            var propertyMock = new Mock<IProperty>();
            propertyMock.Setup(m => m.Name).Returns(name);
            return propertyMock.Object;
        }

        protected virtual string OpenDelimeter
        {
            get { return "\""; }
        }

        protected virtual string CloseDelimeter
        {
            get { return "\""; }
        }

        protected ColumnModification[] CreateInsertOperations(bool identityKey = true, bool computedProperty = true)
        {
            var entry = new Mock<StateEntry>().Object;
            var generator = new ParameterNameGenerator();

            return new[]
                {
                    new ColumnModification(
                        entry, CreateMockProperty("Id"), generator, identityKey, !identityKey, isKey: true, isCondition: false),
                    new ColumnModification(
                        entry, CreateMockProperty("Name"), generator,
                        isRead: false, isWrite: true, isKey: false, isCondition: false),
                    new ColumnModification(
                        entry, CreateMockProperty("Quacks"), generator,
                        isRead: false, isWrite: true, isKey: false, isCondition: false),
                    new ColumnModification(
                        entry, CreateMockProperty("Computed"), generator, computedProperty, isWrite: false, isKey: false, isCondition: false),
                    new ColumnModification(
                        entry, CreateMockProperty("ConcurrencyToken"), generator,
                        isRead: false, isWrite: true, isKey: false, isCondition: false)
                };
        }

        protected ColumnModification[] CreateUpdateOperations(bool computedProperty = true, bool concurrencyToken = true)
        {
            var entry = new Mock<StateEntry>().Object;
            var generator = new ParameterNameGenerator();

            return new[]
                {
                    new ColumnModification(
                        entry, CreateMockProperty("Id"), generator,
                        isRead: false, isWrite: false, isKey: true, isCondition: true),
                    new ColumnModification(
                        entry, CreateMockProperty("Name"), generator,
                        isRead: false, isWrite: true, isKey: false, isCondition: false),
                    new ColumnModification(
                        entry, CreateMockProperty("Quacks"), generator,
                        isRead: false, isWrite: true, isKey: false, isCondition: false),
                    new ColumnModification(
                        entry, CreateMockProperty("Computed"), generator, computedProperty, isWrite: false, isKey: false, isCondition: false),
                    new ColumnModification(
                        entry, CreateMockProperty("ConcurrencyToken"), generator, false, true, false, concurrencyToken)
                };
        }

        protected ColumnModification[] CreateDeleteOperations(bool concurrencyToken = true)
        {
            var entry = new Mock<StateEntry>().Object;
            var generator = new ParameterNameGenerator();

            return new[]
                {
                    new ColumnModification(
                        entry, CreateMockProperty("Id"), generator,
                        isRead: false, isWrite: false, isKey: true, isCondition: true),
                    new ColumnModification(
                        entry, CreateMockProperty("ConcurrencyToken"), generator, false, false, false, concurrencyToken)
                };
        }
    }
}
