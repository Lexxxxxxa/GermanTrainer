using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using GermanTrainer.Models;
using System;

namespace GermanTrainer.Data.Configuration
{
    partial class GermanAdjectiveConfiguration : IEntityTypeConfiguration<GermanAdjective>
    {
        public void Configure(EntityTypeBuilder<GermanAdjective> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Word).IsRequired().HasMaxLength(50);
            builder.Property(x => x.Translation).IsRequired().HasMaxLength(200);

            builder.HasData(GetGermanAdjectiveBatch1());
            builder.HasData(GetGermanAdjectiveBatch2());
            builder.HasData(GetGermanAdjectiveBatch3());
            builder.HasData(GetGermanAdjectiveBatch4());
            builder.HasData(GetGermanAdjectiveBatch5());
            builder.HasData(GetGermanAdjectiveBatch8());
        }
    }
}