using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MySql.EntityFrameworkCore.Extensions;
namespace API.Models.Mappings
{
    public class ConfigurationMapping : IEntityTypeConfiguration<Configuration>
    {
        public void Configure(EntityTypeBuilder<Configuration> builder)
        {
            builder.ToTable("configurations");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnName("id").UseMySQLAutoIncrementColumn("id");
            builder.Property(x => x.Name).HasColumnName("name").HasMaxLength(100).IsRequired();
            builder.Property(x => x.Url).HasColumnName("url").HasMaxLength(250).IsRequired();
            builder.Property(x => x.Key).HasColumnName("key").HasMaxLength(50).IsRequired();
            builder.Property(x => x.Active).HasColumnName("active").IsRequired();
        }
    }
}
