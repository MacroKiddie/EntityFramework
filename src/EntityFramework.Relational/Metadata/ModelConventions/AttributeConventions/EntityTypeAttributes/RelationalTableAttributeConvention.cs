// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.Data.Entity.Metadata.Internal;
using Microsoft.Data.Entity.Metadata.ModelConventions.AttributeConventions.EntityTypeAttributes;
using Microsoft.Data.Entity.Utilities;

namespace Microsoft.Data.Entity.Relational.Metadata.ModelConventions.AttributeConventions.EntityTypeAttributes
{
    public class RelationalTableAttributeConvention : EntityTypeAttributeConvention<TableAttribute>
    {
        public override void Apply(InternalEntityTypeBuilder entityTypeBuilder, TableAttribute attribute)
        {
            Check.NotNull(entityTypeBuilder, nameof(entityTypeBuilder));
            Check.NotNull(attribute, nameof(attribute));

            if (!string.IsNullOrWhiteSpace(attribute.Schema))
            {
                entityTypeBuilder.Metadata.Relational().Schema = attribute.Schema;
            }

            if (!string.IsNullOrWhiteSpace(attribute.Name))
            {
                entityTypeBuilder.Metadata.Relational().Table = attribute.Name;
            }
        }
    }
}
