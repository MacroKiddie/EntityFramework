// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.Data.Entity.Metadata.ModelConventions;
using Xunit;

namespace Microsoft.Data.Entity.Relational.Metadata
{
    public class RelationalEntityTypeAttributeConventionTest
    {
        [Table("MyTable", Schema = "MySchema")]
        public class A
        {
            public int Id { get; set; }

            public string Name { get; set; }
        }

        [Fact]
        public void TableAttribute_set_table_name_and_schema_for_entity_type()
        {
            var modelBuilder = new ModelBuilder(new RelationalConventionSetBuilder().AddConventions(new CoreConventionSetBuilder().CreateConventionSet()));
            var entityType = modelBuilder.Entity<A>();

            Assert.Equal("MyTable", entityType.Metadata.Relational().Table);
            Assert.Equal("MySchema", entityType.Metadata.Relational().Schema);
        }
    }
}
