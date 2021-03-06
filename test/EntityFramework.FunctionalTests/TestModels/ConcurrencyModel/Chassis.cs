// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace ConcurrencyModel
{
    public class Chassis
    {
        public byte[] Version { get; set; }

        public int TeamId { get; set; }

        public string Name { get; set; }

        public virtual Team Team { get; set; }
    }
}
