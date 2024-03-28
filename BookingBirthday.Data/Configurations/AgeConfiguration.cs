using BookingBirthday.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingBirthday.Data.Configurations
{
    public class AgeConfiguration : IEntityTypeConfiguration<Age>
    {
        public void Configure(EntityTypeBuilder<Age> builder)
        {
            builder.ToTable("Age");

            // Primary Key
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).UseIdentityColumn();

            // Other properties
            
            builder.Property(x => x.Name).IsUnicode().IsRequired();



        }
    }
}
