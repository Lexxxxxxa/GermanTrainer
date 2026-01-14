using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using GermanTrainer.Models;
using System;

namespace GermanTrainer.Data.Configuration
{
    partial class GermanNounConfiguration : IEntityTypeConfiguration<GermanNoun>
    {
        public void Configure(EntityTypeBuilder<GermanNoun> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Word).IsRequired().HasMaxLength(50);
            builder.Property(x => x.Article).IsRequired().HasMaxLength(10);
            builder.Property(x => x.Translation).IsRequired().HasMaxLength(200);
            builder.Property(x => x.PluralForm).HasMaxLength(50);

            builder.HasIndex(x => x.Word).IsUnique();

            builder.HasData(GetGermanNounBatch1());
            builder.HasData(GetGermanNounBatch2());
            builder.HasData(GetGermanNounBatch3());
            builder.HasData(GetGermanNounBatch4());
            builder.HasData(GetGermanNounBatch5());
            builder.HasData(GetGermanNounBatch8());
        }
    }
}