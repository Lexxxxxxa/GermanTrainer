using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using GermanTrainer.Models;

namespace GermanTrainer.Data.Configuration
{
    partial class GermanVerbConfiguration : IEntityTypeConfiguration<GermanVerb>
    {
        public void Configure(EntityTypeBuilder<GermanVerb> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Word).IsRequired().HasMaxLength(50);
            builder.Property(x => x.Translation).IsRequired().HasMaxLength(200);
            builder.Property(x => x.Level).IsRequired().HasMaxLength(3);

            builder.HasData(GetGermanVerbBatch1());
            builder.HasData(GetGermanVerbBatch2());
            builder.HasData(GetGermanVerbBatch3());
            builder.HasData(GetGermanVerbBatch4());
            builder.HasData(GetGermanVerbBatch5());
            builder.HasData(GetGermanVerbBatch6());
            builder.HasData(GetGermanVerbBatch7());
            builder.HasData(GetGermanVerbBatch8());
            builder.HasData(GetGermanVerbBatch9());
            builder.HasData(GetGermanVerbBatch10());
            builder.HasData(GetGermanVerbBatch11());
            builder.HasData(GetGermanVerbBatch12());
            builder.HasData(GetGermanVerbBatch13());
            builder.HasData(GetGermanVerbBatch14());
            builder.HasData(GetGermanVerbBatch15());
        }
    }
}