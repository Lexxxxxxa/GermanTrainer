using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using GermanTrainer.Models;

namespace GermanTrainer.Data.Configuration
{
    public partial class GermanAdjectiveConfiguration : IEntityTypeConfiguration<GermanAdjective>
    {
        public void Configure(EntityTypeBuilder<GermanAdjective> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Word).IsRequired().HasMaxLength(50);
            builder.Property(x => x.Translation).IsRequired().HasMaxLength(200);
            builder.Property(x => x.Level).IsRequired().HasMaxLength(3);

            builder.HasData(GetGermanAdjectiveBatch1());
            builder.HasData(GetGermanAdjectiveBatch2());
            builder.HasData(GetGermanAdjectiveBatch3());
            builder.HasData(GetGermanAdjectiveBatch4());
            builder.HasData(GetGermanAdjectiveBatch5());
            builder.HasData(GetGermanAdjectiveBatch6());
            builder.HasData(GetGermanAdjectiveBatch7());
            builder.HasData(GetGermanAdjectiveBatch8());
            builder.HasData(GetGermanAdjectiveBatch9());
            builder.HasData(GetGermanAdjectiveBatch10());
            builder.HasData(GetGermanAdjectiveBatch11());
            builder.HasData(GetGermanAdjectiveBatch12());
            builder.HasData(GetGermanAdjectiveBatch13());
            builder.HasData(GetGermanAdjectiveBatch14());
            builder.HasData(GetGermanAdjectiveBatch15());

        }
    }
}