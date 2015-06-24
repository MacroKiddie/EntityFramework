// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.Data.Entity.Internal;
using Xunit;

namespace Microsoft.Data.Entity.Metadata.ModelConventions
{
    public class PropertyAttributeConventionTest
    {
        public class A
        {
            [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
            public int Id { get; set; }

            [ConcurrencyCheck]
            public Guid RowVersion { get; set; }

            [Required]
            public string Name { get; set; }

            [Key]
            public int MyPrimaryKey { get; set; }
        }

        public class B
        {
            [Key]
            public int Id { get; set; }

            [Key]
            public int MyPrimaryKey { get; set; }
        }

        [Fact]
        public void ConcurrencyCheckAttribute_sets_concurrency_token()
        {
            var modelBuilder = new ModelBuilder(new CoreConventionSetBuilder().CreateConventionSet());
            var entityTypeBuilder = modelBuilder.Entity<A>();

            Assert.True(entityTypeBuilder.Property(e => e.RowVersion).Metadata.IsConcurrencyToken);
        }

        [Fact]
        public void DatabaseGeneratedAttribute_sets_store_generated_pattern()
        {
            var modelBuilder = new ModelBuilder(new CoreConventionSetBuilder().CreateConventionSet());
            var entityTypeBuilder = modelBuilder.Entity<A>();

            Assert.Equal(StoreGeneratedPattern.Computed, entityTypeBuilder.Property(e => e.Id).Metadata.StoreGeneratedPattern);
        }

        [Fact]
        public void RequiredAttribute_sets_is_nullable()
        {
            var modelBuilder = new ModelBuilder(new CoreConventionSetBuilder().CreateConventionSet());
            var entityTypeBuilder = modelBuilder.Entity<A>();

            Assert.False(entityTypeBuilder.Property(e => e.Name).Metadata.IsNullable);
        }

        [Fact]
        public void KeyAttribute_sets_primary_key_for_single_property()
        {
            var modelBuilder = new ModelBuilder(new CoreConventionSetBuilder().CreateConventionSet());
            var entityTypeBuilder = modelBuilder.Entity<A>();

            Assert.Equal(1, entityTypeBuilder.Metadata.GetPrimaryKey().Properties.Count);
            Assert.Equal("MyPrimaryKey", entityTypeBuilder.Metadata.GetPrimaryKey().Properties[0].Name);
        }

        [Fact]
        public void KeyAttribute_throws_when_setting_composite_primary_key()
        {
            var modelBuilder = new ModelBuilder(new CoreConventionSetBuilder().CreateConventionSet());
            var entityTypeBuilder = modelBuilder.Entity<B>();

            Assert.Equal(2, entityTypeBuilder.Metadata.GetPrimaryKey().Properties.Count);
            Assert.Equal("Id", entityTypeBuilder.Metadata.GetPrimaryKey().Properties[0].Name);
            Assert.Equal("MyPrimaryKey", entityTypeBuilder.Metadata.GetPrimaryKey().Properties[1].Name);

            Assert.Equal(
                Strings.CompositePKWithDataAnnotation(entityTypeBuilder.Metadata.DisplayName()),
                Assert.Throws<InvalidOperationException>(() => modelBuilder.Validate()).Message);
        }

        [Fact]
        public void KeyAttribute_does_not_throw_when_setting_composite_primary_key_if_fluent_api_used()
        {
            var model = new MyContext().Model;

            Assert.Equal(2, model.GetEntityType(typeof(B)).GetPrimaryKey().Properties.Count);
            Assert.Equal("MyPrimaryKey", model.GetEntityType(typeof(B)).GetPrimaryKey().Properties[0].Name);
            Assert.Equal("Id", model.GetEntityType(typeof(B)).GetPrimaryKey().Properties[1].Name);
        }

        public class MyContext : DbContext
        {
            protected internal override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            {
                optionsBuilder.UseInMemoryStore();
            }

            protected internal override void OnModelCreating(ModelBuilder modelBuilder)
            {
                modelBuilder.Entity<B>().Key(e => new { e.MyPrimaryKey, e.Id});
            }
        }
    }
}
