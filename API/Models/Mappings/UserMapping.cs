using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MySql.EntityFrameworkCore.Extensions;
namespace API.Models.Mappings
{
    public class UserMapping : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("users");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnName("id").UseMySQLAutoIncrementColumn("id");
            builder.Property(x => x.Name).HasColumnName("name").HasMaxLength(100).IsRequired();
            builder.Property(x => x.Email).HasColumnName("email").HasMaxLength(100).IsRequired();
            builder.Property(x => x.PasswordHash).HasColumnName("password_hash").HasMaxLength(100).IsRequired();
            builder.Property(x => x.PasswordSalt).HasColumnName("password_salt").HasMaxLength(100).IsRequired();
            builder.Property(x => x.Active).HasColumnName("active").IsRequired();
        }
    }
}
