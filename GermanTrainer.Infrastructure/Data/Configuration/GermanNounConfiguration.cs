using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using GermanTrainer.Core.Models;

namespace GermanTrainer.Infrastructure.Data.Configuration;

partial class GermanNounConfiguration : IEntityTypeConfiguration<GermanNoun>
{
    public void Configure(EntityTypeBuilder<GermanNoun> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Word).IsRequired().HasMaxLength(50);
        builder.Property(x => x.Article).IsRequired().HasMaxLength(10);
        builder.Property(x => x.Translation).IsRequired().HasMaxLength(200);
        builder.Property(x => x.PluralForm).HasMaxLength(50);
        builder.Property(x => x.Level).IsRequired().HasMaxLength(3);

        builder.HasData(GetGermanNounA1Seeds());
        builder.HasData(GetGermanNounA2Seeds());
        builder.HasData(GetGermanNounB1Seeds());

    }
}
