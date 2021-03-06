// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;

namespace Microsoft.Data.Entity.Metadata.Compiled
{
    public abstract class CompiledProperty<TProperty> : CompiledMetadataBase
    {
        private readonly IEntityType _entityType;

        protected CompiledProperty(IEntityType entityType)
        {
            _entityType = entityType;
        }

        public Type PropertyType
        {
            get { return typeof(TProperty); }
        }

        public Type UnderlyingType
        {
            get { return Nullable.GetUnderlyingType(typeof(TProperty)) ?? typeof(TProperty); }
        }

        public virtual ValueGenerationOnSave ValueGenerationOnSave
        {
            get { return ValueGenerationOnSave.None; }
        }

        public virtual ValueGenerationOnAdd ValueGenerationOnAdd
        {
            get { return ValueGenerationOnAdd.None; }
        }

        public bool IsNullable
        {
            get { return typeof(TProperty).IsNullableType(); }
        }

        public IEntityType EntityType
        {
            get { return _entityType; }
        }

        public bool IsShadowProperty
        {
            get { return false; }
        }

        public bool IsConcurrencyToken
        {
            // TODO:
            get { return true; }
        }

        public int OriginalValueIndex
        {
            // TODO:
            get { return -1; }
        }
    }
}
