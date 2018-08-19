// Copyright (c) Philipp Wagner. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ModularEfCore.Example.Database.Model;
using ModularEfCore.Map;

namespace ModularEfCore.Example.Database.Map
{
    public class CustomerMap : BaseEntityMap<Customer>
    {
        protected override void InternalMap(EntityTypeBuilder<Customer> builder)
        {
            builder
                .ToTable("Sample", "Customer");

            builder
                .HasKey(x => x.Id)
                .HasName("PK_Customer");

            builder.Property(x => x.Id)
                .HasColumnName("Id")
                .ValueGeneratedOnAdd();

            builder
                .Property(x => x.FirstName)
                .HasColumnName("FirstName");

            builder
                .Property(x => x.LastName)
                .HasColumnName("LastName");
        }
    }
}
