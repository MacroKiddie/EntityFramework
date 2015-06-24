// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.Data.Entity.Metadata.ModelConventions;
using Xunit;

namespace Microsoft.Data.Entity.Relational.Metadata
{
    public class RelationalPropertyAttributeConventionTest
    {
        public class A
        {
            public int Id { get; set; }

            [Column("Post Name", Order = 1, TypeName = "DECIMAL")]
            public string Name { get; set; }
        }

        [Fact]
        public void ColumnAttribute_sets_column_name_order_and_type()
        {
            var modelBuilder = new ModelBuilder(new RelationalConventionSetBuilder().AddConventions(new CoreConventionSetBuilder().CreateConventionSet()));
            var entityType = modelBuilder.Entity<A>();

            Assert.Equal("Post Name", entityType.Property(e => e.Name).Metadata.Relational().Column);
            Assert.Equal(1, entityType.Property(e => e.Name).Metadata.Relational().ColumnOrder);
            Assert.Equal("DECIMAL", entityType.Property(e => e.Name).Metadata.Relational().ColumnType);
        }
    }
}
