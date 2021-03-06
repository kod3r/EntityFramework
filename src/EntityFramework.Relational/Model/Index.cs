// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Microsoft.Data.Entity.Relational.Utilities;

namespace Microsoft.Data.Entity.Relational.Model
{
    // TODO: Consider adding more validation.
    public class Index
    {
        private string _name;
        private readonly IReadOnlyList<Column> _columns;
        private readonly bool _isUnique;
        private readonly bool _isClustered;

        public Index(
            [NotNull] string name,
            [NotNull] IReadOnlyList<Column> columns,
            bool isUnique,
            bool isClustered)
        {
            Check.NotEmpty(name, "name");
            Check.NotNull(columns, "columns");

            _name = name;
            _columns = columns;
            _isUnique = isUnique;
            _isClustered = isClustered;
        }

        public Index(
            [NotNull] string name,
            [NotNull] IReadOnlyList<Column> columns)
            : this(name, columns, isUnique: false, isClustered: false)
        {
        }

        public virtual Table Table
        {
            get { return _columns[0].Table; }
        }

        public virtual string Name
        {
            get { return _name; }

            [param: NotNull] set { _name = value; }
        }

        public virtual IReadOnlyList<Column> Columns
        {
            get { return _columns; }
        }

        public virtual bool IsUnique
        {
            get { return _isUnique; }
        }

        public virtual bool IsClustered
        {
            get { return _isClustered; }
        }

        public virtual Index Clone([NotNull] CloneContext cloneContext)
        {
            Check.NotNull(cloneContext, "cloneContext");

            return
                new Index(
                    Name,
                    Columns.Select(column => column.Clone(cloneContext)).ToArray(),
                    IsUnique,
                    IsClustered);
        }
    }
}
