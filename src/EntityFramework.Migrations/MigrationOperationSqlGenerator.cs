// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using JetBrains.Annotations;
using Microsoft.Data.Entity.Migrations.Model;
using Microsoft.Data.Entity.Migrations.Utilities;
using Microsoft.Data.Entity.Relational;
using Microsoft.Data.Entity.Relational.Model;
using Microsoft.Data.Entity.Utilities;

namespace Microsoft.Data.Entity.Migrations
{
    /// <summary>
    ///     Default migration operation SQL generator, outputs best-effort ANSI-99 compliant SQL.
    /// </summary>
    public abstract class MigrationOperationSqlGenerator
    {
        // TODO: Check whether the following formats ar SqlServer specific or not and move
        // to SqlServer provider if they are.
        internal const string DateTimeFormat = "yyyy-MM-ddTHH:mm:ss.fffK";
        internal const string DateTimeOffsetFormat = "yyyy-MM-ddTHH:mm:ss.fffzzz";

        private readonly RelationalTypeMapper _typeMapper;        
        private DatabaseModel _database;
        private DatabaseModelModifier _databaseModelModifier;

        protected MigrationOperationSqlGenerator([NotNull] RelationalTypeMapper typeMapper)
        {
            Check.NotNull(typeMapper, "typeMapper");

            _typeMapper = typeMapper;

        }

        public virtual RelationalTypeMapper TypeMapper
        {
            get { return _typeMapper; }
        }

        public virtual DatabaseModel Database
        {
            get { return _database; }

            [param: NotNull]
            set
            {
                Check.NotNull(value, "value");

                _database = value.Clone();
            }
        }

        // TODO: Inject this via constructor?
        public virtual DatabaseModelModifier DatabaseModelModifier
        {
            get { return _databaseModelModifier; }

            [param: NotNull]
            set
            {
                Check.NotNull(value, "value");

                _databaseModelModifier = value;
            }
        }

        public virtual IEnumerable<SqlStatement> Generate([NotNull] IEnumerable<MigrationOperation> migrationOperations)
        {
            Check.NotNull(migrationOperations, "migrationOperations");

            foreach (var operation in migrationOperations)
            {
                var statement = Generate(operation);

                _databaseModelModifier.Modify(_database, operation);

                yield return statement;
            }
        }

        public virtual SqlStatement Generate([NotNull] MigrationOperation operation)
        {
            Check.NotNull(operation, "operation");

            var builder = new IndentedStringBuilder();

            operation.GenerateSql(this, builder);

            // TODO: Should we support implementations of Generate that output more than one SQL statement?
            return new SqlStatement(builder.ToString());
        }

        public virtual void Generate([NotNull] CreateDatabaseOperation createDatabaseOperation, [NotNull] IndentedStringBuilder stringBuilder)
        {
            Check.NotNull(createDatabaseOperation, "createDatabaseOperation");
            Check.NotNull(stringBuilder, "stringBuilder");

            stringBuilder
                .Append("CREATE DATABASE ")
                .Append(DelimitIdentifier(createDatabaseOperation.DatabaseName));
        }

        public virtual void Generate([NotNull] DropDatabaseOperation dropDatabaseOperation, [NotNull] IndentedStringBuilder stringBuilder)
        {
            Check.NotNull(dropDatabaseOperation, "dropDatabaseOperation");

            stringBuilder
                .Append("DROP DATABASE ")
                .Append(DelimitIdentifier(dropDatabaseOperation.DatabaseName));
        }

        public virtual void Generate([NotNull] CreateSequenceOperation createSequenceOperation, [NotNull] IndentedStringBuilder stringBuilder)
        {
            Check.NotNull(createSequenceOperation, "createSequenceOperation");

            var sequence = createSequenceOperation.Sequence;

            stringBuilder
                .Append("CREATE SEQUENCE ")
                .Append(DelimitIdentifier(sequence.Name))
                .Append(" AS ")
                .Append(sequence.DataType)
                .Append(" START WITH ")
                .Append(sequence.StartWith)
                .Append(" INCREMENT BY ")
                .Append(sequence.IncrementBy);
        }

        public virtual void Generate([NotNull] DropSequenceOperation dropSequenceOperation, [NotNull] IndentedStringBuilder stringBuilder)
        {
            Check.NotNull(dropSequenceOperation, "dropSequenceOperation");

            stringBuilder
                .Append("DROP SEQUENCE ")
                .Append(DelimitIdentifier(dropSequenceOperation.SequenceName));
        }

        public virtual void Generate([NotNull] CreateTableOperation createTableOperation, [NotNull] IndentedStringBuilder stringBuilder)
        {
            Check.NotNull(createTableOperation, "createTableOperation");

            var table = createTableOperation.Table;

            stringBuilder
                .Append("CREATE TABLE ")
                .Append(DelimitIdentifier(table.Name))
                .AppendLine(" (");

            using (stringBuilder.Indent())
            {
                GenerateColumns(table, table.Columns, stringBuilder);

                var primaryKey = table.PrimaryKey;

                if (primaryKey != null)
                {
                    stringBuilder.AppendLine(",");

                    GeneratePrimaryKey(
                        new AddPrimaryKeyOperation(
                            table.Name,
                            primaryKey.Name,
                            primaryKey.Columns.Select(c => c.Name).ToArray(),
                            primaryKey.IsClustered),
                        stringBuilder);
                }

                GenerateTableConstraints(createTableOperation, stringBuilder);
            }

            stringBuilder
                .AppendLine()
                .Append(")");
        }

        protected virtual void GenerateTableConstraints([NotNull] CreateTableOperation createTableOperation, [NotNull] IndentedStringBuilder stringBuilder)
        {
        }

        public virtual void Generate([NotNull] DropTableOperation dropTableOperation, [NotNull] IndentedStringBuilder stringBuilder)
        {
            Check.NotNull(dropTableOperation, "dropTableOperation");

            stringBuilder
                .Append("DROP TABLE ")
                .Append(DelimitIdentifier(dropTableOperation.TableName));
        }

        public virtual void Generate([NotNull] RenameTableOperation renameTableOperation, [NotNull] IndentedStringBuilder stringBuilder)
        {
            throw new NotImplementedException();
        }

        public virtual void Generate([NotNull] MoveTableOperation moveTableOperation, [NotNull] IndentedStringBuilder stringBuilder)
        {
            Check.NotNull(moveTableOperation, "moveTableOperation");

            stringBuilder
                .Append("ALTER SCHEMA ")
                .Append(DelimitIdentifier(moveTableOperation.NewSchema))
                .Append(" TRANSFER ")
                .Append(DelimitIdentifier(moveTableOperation.TableName));
        }

        public virtual void Generate([NotNull] AddColumnOperation addColumnOperation, [NotNull] IndentedStringBuilder stringBuilder)
        {
            Check.NotNull(addColumnOperation, "addColumnOperation");
            Check.NotNull(stringBuilder, "stringBuilder");

            stringBuilder
                .Append("ALTER TABLE ")
                .Append(DelimitIdentifier(addColumnOperation.TableName))
                .Append(" ADD ");

            var table = Database.GetTable(addColumnOperation.TableName);
            GenerateColumn(table, addColumnOperation.Column, stringBuilder);
        }

        public virtual void Generate([NotNull] DropColumnOperation dropColumnOperation, [NotNull] IndentedStringBuilder stringBuilder)
        {
            Check.NotNull(dropColumnOperation, "dropColumnOperation");

            stringBuilder
                .Append("ALTER TABLE ")
                .Append(DelimitIdentifier(dropColumnOperation.TableName))
                .Append(" DROP COLUMN ")
                .Append(DelimitIdentifier(dropColumnOperation.ColumnName));
        }

        public virtual void Generate([NotNull] AlterColumnOperation alterColumnOperation, [NotNull] IndentedStringBuilder stringBuilder)
        {
            Check.NotNull(alterColumnOperation, "alterColumnOperation");

            var table = Database.GetTable(alterColumnOperation.TableName);
            var newColumn = alterColumnOperation.NewColumn;
            
            stringBuilder
                .Append("ALTER TABLE ")
                .Append(DelimitIdentifier(alterColumnOperation.TableName))
                .Append(" ALTER COLUMN ")
                .Append(DelimitIdentifier(newColumn.Name))
                .Append(" ")
                .Append(GenerateDataType(table, newColumn))
                .Append(newColumn.IsNullable ? " NULL" : " NOT NULL");
        }

        public virtual void Generate([NotNull] AddDefaultConstraintOperation addDefaultConstraintOperation, [NotNull] IndentedStringBuilder stringBuilder)
        {
            Check.NotNull(addDefaultConstraintOperation, "addDefaultConstraintOperation");

            stringBuilder
                .Append("ALTER TABLE ")
                .Append(DelimitIdentifier(addDefaultConstraintOperation.TableName))
                .Append(" ALTER COLUMN ")
                .Append(DelimitIdentifier(addDefaultConstraintOperation.ColumnName))
                .Append(" SET DEFAULT ");

            stringBuilder.Append(addDefaultConstraintOperation.DefaultSql ?? GenerateLiteral((dynamic)addDefaultConstraintOperation.DefaultValue));
        }

        public virtual void Generate([NotNull] DropDefaultConstraintOperation dropDefaultConstraintOperation, [NotNull] IndentedStringBuilder stringBuilder)
        {
            Check.NotNull(dropDefaultConstraintOperation, "dropDefaultConstraintOperation");

            stringBuilder
                .Append("ALTER TABLE ")
                .Append(DelimitIdentifier(dropDefaultConstraintOperation.TableName))
                .Append(" ALTER COLUMN ")
                .Append(DelimitIdentifier(dropDefaultConstraintOperation.ColumnName))
                .Append(" DROP DEFAULT");
        }

        public virtual void Generate([NotNull] RenameColumnOperation renameColumnOperation, [NotNull] IndentedStringBuilder stringBuilder)
        {
            throw new NotImplementedException();
        }

        public virtual void Generate([NotNull] AddPrimaryKeyOperation addPrimaryKeyOperation, [NotNull] IndentedStringBuilder stringBuilder)
        {
            Check.NotNull(addPrimaryKeyOperation, "addPrimaryKeyOperation");

            stringBuilder
                .Append("ALTER TABLE ")
                .Append(DelimitIdentifier(addPrimaryKeyOperation.TableName))
                .Append(" ADD ");

            GeneratePrimaryKey(addPrimaryKeyOperation, stringBuilder);
        }

        public virtual void Generate([NotNull] DropPrimaryKeyOperation dropPrimaryKeyOperation, [NotNull] IndentedStringBuilder stringBuilder)
        {
            Check.NotNull(dropPrimaryKeyOperation, "dropPrimaryKeyOperation");

            stringBuilder
                .Append("ALTER TABLE ")
                .Append(DelimitIdentifier(dropPrimaryKeyOperation.TableName))
                .Append(" DROP CONSTRAINT ")
                .Append(DelimitIdentifier(dropPrimaryKeyOperation.PrimaryKeyName));
        }

        public virtual void Generate([NotNull] AddForeignKeyOperation addForeignKeyOperation, [NotNull] IndentedStringBuilder stringBuilder)
        {
            Check.NotNull(addForeignKeyOperation, "addForeignKeyOperation");

            stringBuilder
                .Append("ALTER TABLE ")
                .Append(DelimitIdentifier(addForeignKeyOperation.TableName))
                .Append(" ADD ");

            GenerateForeignKey(addForeignKeyOperation, stringBuilder);
        }

        public virtual void Generate([NotNull] DropForeignKeyOperation dropForeignKeyOperation, [NotNull] IndentedStringBuilder stringBuilder)
        {
            Check.NotNull(dropForeignKeyOperation, "dropForeignKeyOperation");

            stringBuilder
                .Append("ALTER TABLE ")
                .Append(DelimitIdentifier(dropForeignKeyOperation.TableName))
                .Append(" DROP CONSTRAINT ")
                .Append(DelimitIdentifier(dropForeignKeyOperation.ForeignKeyName));
        }

        public virtual void Generate([NotNull] CreateIndexOperation createIndexOperation, [NotNull] IndentedStringBuilder stringBuilder)
        {
            Check.NotNull(createIndexOperation, "createIndexOperation");

            stringBuilder.Append("CREATE");

            if (createIndexOperation.IsUnique)
            {
                stringBuilder.Append(" UNIQUE");
            }

            // TODO: Move to SqlServer
            if (createIndexOperation.IsClustered)
            {
                stringBuilder.Append(" CLUSTERED");
            }

            stringBuilder
                .Append(" INDEX ")
                .Append(DelimitIdentifier(createIndexOperation.IndexName))
                .Append(" ON ")
                .Append(DelimitIdentifier(createIndexOperation.TableName))
                .Append(" (")
                .Append(createIndexOperation.ColumnNames.Select(DelimitIdentifier).Join())
                .Append(")");
        }

        public virtual void Generate([NotNull] DropIndexOperation dropIndexOperation, [NotNull] IndentedStringBuilder stringBuilder)
        {
            Check.NotNull(dropIndexOperation, "dropIndexOperation");
            Check.NotNull(stringBuilder, "stringBuilder");

            stringBuilder
                .Append("DROP INDEX ")
                .Append(DelimitIdentifier(dropIndexOperation.IndexName));
        }

        public virtual void Generate([NotNull] RenameIndexOperation renameIndexOperation, [NotNull] IndentedStringBuilder stringBuilder)
        {
            throw new NotImplementedException();
        }

        public virtual void Generate([NotNull] CopyDataOperation copyDataOperation, [NotNull] IndentedStringBuilder stringBuilder)
        {
            Check.NotNull(copyDataOperation, "copyDataOperation");
            Check.NotNull(stringBuilder, "stringBuilder");

            stringBuilder
                .Append("INSERT INTO ")
                .Append(DelimitIdentifier(copyDataOperation.TargetTableName))
                .Append(" ( ")
                .Append(copyDataOperation.TargetColumnNames.Select(DelimitIdentifier).Join())
                .AppendLine(" )");

            using (stringBuilder.Indent())
            {
                stringBuilder
                    .Append("SELECT ")
                    .Append(copyDataOperation.SourceColumnNames.Select(DelimitIdentifier).Join())
                    .Append(" FROM ")
                    .Append(DelimitIdentifier(copyDataOperation.SourceTableName));
            }
        }

        public virtual void Generate([NotNull] SqlOperation sqlOperation, [NotNull] IndentedStringBuilder stringBuilder)
        {
            Check.NotNull(sqlOperation, "sqlOperation");
            Check.NotNull(stringBuilder, "stringBuilder");

            stringBuilder.Append(sqlOperation.Sql);
        }

        public virtual string GenerateDataType([NotNull] Table table, [NotNull] Column column)
        {
            Check.NotNull(table, "table");
            Check.NotNull(column, "column");

            if (!string.IsNullOrEmpty(column.DataType))
            {
                return column.DataType;
            }

            var isKey
                = table.PrimaryKey != null
                  && table.PrimaryKey.Columns.Contains(column)
                  || table.ForeignKeys.SelectMany(k => k.Columns).Contains(column);

            return _typeMapper.GetTypeMapping(column.DataType, column.Name, column.ClrType, isKey, column.IsTimestamp).StoreTypeName;
        }

        public virtual string GenerateLiteral([NotNull] object value)
        {
            return string.Format(CultureInfo.InvariantCulture, "{0}", value);
        }

        public virtual string GenerateLiteral(bool value)
        {
            return value ? "1" : "0";
        }

        public virtual string GenerateLiteral([NotNull] string value)
        {
            Check.NotNull(value, "value");

            return "'" + value + "'";
        }

        public virtual string GenerateLiteral(Guid value)
        {
            return "'" + value + "'";
        }

        public virtual string GenerateLiteral(DateTime value)
        {
            return "'" + value.ToString(DateTimeFormat, CultureInfo.InvariantCulture) + "'";
        }

        public virtual string GenerateLiteral(DateTimeOffset value)
        {
            return "'" + value.ToString(DateTimeOffsetFormat, CultureInfo.InvariantCulture) + "'";
        }

        public virtual string GenerateLiteral(TimeSpan value)
        {
            return "'" + value + "'";
        }

        public virtual string GenerateLiteral([NotNull] byte[] value)
        {
            Check.NotNull(value, "value");

            var stringBuilder = new StringBuilder("0x");

            foreach (var @byte in value)
            {
                stringBuilder.Append(@byte.ToString("X2", CultureInfo.InvariantCulture));
            }

            return stringBuilder.ToString();
        }

        public virtual string DelimitIdentifier(SchemaQualifiedName schemaQualifiedName)
        {
            return
                (schemaQualifiedName.IsSchemaQualified
                    ? DelimitIdentifier(schemaQualifiedName.Schema) + "."
                    : string.Empty)
                + DelimitIdentifier(schemaQualifiedName.Name);
        }

        public virtual string DelimitIdentifier([NotNull] string identifier)
        {
            Check.NotEmpty(identifier, "identifier");

            return "\"" + EscapeIdentifier(identifier) + "\"";
        }

        public virtual string EscapeIdentifier([NotNull] string identifier)
        {
            Check.NotEmpty(identifier, "identifier");

            return identifier.Replace("\"", "\"\"");
        }

        public virtual string DelimitLiteral([NotNull] string literal)
        {
            Check.NotNull(literal, "literal");

            return "'" + EscapeLiteral(literal) + "'";
        }

        public virtual string EscapeLiteral([NotNull] string literal)
        {
            Check.NotNull(literal, "literal");

            return literal.Replace("'", "''");
        }

        protected virtual void GenerateColumns(
            [NotNull] Table table, [NotNull] IReadOnlyList<Column> columns, [NotNull] IndentedStringBuilder stringBuilder)
        {
            Check.NotNull(table, "table");
            Check.NotNull(columns, "columns");
            Check.NotNull(stringBuilder, "stringBuilder");

            if (columns.Count == 0)
            {
                return;
            }

            GenerateColumn(table, columns[0], stringBuilder);

            for (var i = 1; i < columns.Count; i++)
            {
                stringBuilder.AppendLine(",");

                GenerateColumn(table, columns[i], stringBuilder);
            }
        }

        protected virtual void GenerateColumn(
            [NotNull] Table table, [NotNull] Column column, [NotNull] IndentedStringBuilder stringBuilder)
        {
            Check.NotNull(table, "table");
            Check.NotNull(column, "column");
            Check.NotNull(stringBuilder, "stringBuilder");

            stringBuilder
                .Append(DelimitIdentifier(column.Name))
                .Append(" ");

            stringBuilder.Append(column.DataType ?? GenerateDataType(table, column));

            if (!column.IsNullable)
            {
                stringBuilder.Append(" NOT NULL");
            }

            GenerateColumnTraits(column, stringBuilder);

            if (column.DefaultSql != null)
            {
                stringBuilder
                    .Append(" DEFAULT ")
                    .Append(column.DefaultSql);
            }
            else if (column.DefaultValue != null)
            {
                stringBuilder
                    .Append(" DEFAULT ")
                    .Append(GenerateLiteral(column.DefaultValue));
            }
        }

        protected virtual void GenerateColumnTraits([NotNull] Column column, [NotNull] IndentedStringBuilder stringBuilder)
        {
        }

        protected virtual void GeneratePrimaryKey(
            [NotNull] AddPrimaryKeyOperation primaryKeyOperation,
            [NotNull] IndentedStringBuilder stringBuilder)
        {
            Check.NotNull(primaryKeyOperation, "primaryKeyOperation");
            Check.NotNull(stringBuilder, "stringBuilder");

            stringBuilder
                .Append("CONSTRAINT ")
                .Append(DelimitIdentifier(primaryKeyOperation.PrimaryKeyName))
                .Append(" PRIMARY KEY");

            GeneratePrimaryKeyTraits(primaryKeyOperation, stringBuilder);

            stringBuilder
                .Append(" (")
                .Append(primaryKeyOperation.ColumnNames.Select(DelimitIdentifier).Join())
                .Append(")");
        }

        protected virtual void GeneratePrimaryKeyTraits(
            [NotNull] AddPrimaryKeyOperation primaryKeyOperation,
            [NotNull] IndentedStringBuilder stringBuilder)
        {
        }

        protected virtual void GenerateForeignKey(
            [NotNull] AddForeignKeyOperation foreignKeyOperation,
            [NotNull] IndentedStringBuilder stringBuilder)
        {
            Check.NotNull(foreignKeyOperation, "foreignKeyOperation");
            Check.NotNull(stringBuilder, "stringBuilder");

            stringBuilder
                .Append("CONSTRAINT ")
                .Append(DelimitIdentifier(foreignKeyOperation.ForeignKeyName))
                .Append(" FOREIGN KEY (")
                .Append(foreignKeyOperation.ColumnNames.Select(DelimitIdentifier).Join())
                .Append(") REFERENCES ")
                .Append(DelimitIdentifier(foreignKeyOperation.ReferencedTableName))
                .Append(" (")
                .Append(foreignKeyOperation.ReferencedColumnNames.Select(DelimitIdentifier).Join())
                .Append(")");

            if (foreignKeyOperation.CascadeDelete)
            {
                stringBuilder.Append(" ON DELETE CASCADE");
            }
        }
    }
}
