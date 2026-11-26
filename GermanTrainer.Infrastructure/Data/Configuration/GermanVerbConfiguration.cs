using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using GermanTrainer.Core.Models;

namespace GermanTrainer.Infrastructure.Data.Configuration;

partial class GermanVerbConfiguration : IEntityTypeConfiguration<GermanVerb>
{
    public void Configure(EntityTypeBuilder<GermanVerb> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Word).IsRequired().HasMaxLength(50);
        builder.Property(x => x.Translation).IsRequired().HasMaxLength(200);
        builder.Property(x => x.Level).IsRequired().HasMaxLength(3);

        builder.HasData(GetGermanVerbA1Seeds());
        builder.HasData(GetGermanVerbA2Seeds());
        builder.HasData(GetGermanVerbB1Seeds());
    }
}
