// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Data.Common;
using System.Diagnostics;
using Microsoft.Data.Entity.Storage;

namespace Microsoft.Data.Entity.Relational
{
    public class UntypedValueBufferFactory : IRelationalValueBufferFactory
    {
        public virtual ValueBuffer CreateValueBuffer(DbDataReader dataReader)
        {
            Debug.Assert(dataReader != null); // hot path

            var fieldCount = dataReader.FieldCount;

            if (fieldCount == 0)
            {
                return ValueBuffer.Empty;
            }

            var values = new object[fieldCount];

            dataReader.GetValues(values);

            for (var i = 0; i < fieldCount; i++)
            {
                if (ReferenceEquals(values[i], DBNull.Value))
                {
                    values[i] = null;
                }
            }

            return new ValueBuffer(values);
        }
    }
}
