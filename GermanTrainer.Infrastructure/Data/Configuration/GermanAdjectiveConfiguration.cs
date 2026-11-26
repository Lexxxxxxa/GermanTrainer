using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using GermanTrainer.Core.Models;

namespace GermanTrainer.Infrastructure.Data.Configuration;

public partial class GermanAdjectiveConfiguration : IEntityTypeConfiguration<GermanAdjective>
{
    public void Configure(EntityTypeBuilder<GermanAdjective> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Word).IsRequired().HasMaxLength(50);
        builder.Property(x => x.Translation).IsRequired().HasMaxLength(200);
        builder.Property(x => x.Level).IsRequired().HasMaxLength(3);

        builder.HasData(GetGermanAdjectiveA1Seeds());
        builder.HasData(GetGermanAdjectiveA2Seeds());
        builder.HasData(GetGermanAdjectiveB1Seeds());

    }
}
