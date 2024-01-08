using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Sepid.Robot.Persistence.Configurations
{
    public class DeviceConfiguration : IEntityTypeConfiguration<Domain.Entities.Device>
    {
        public void Configure(EntityTypeBuilder<Domain.Entities.Device> builder)
        {
            builder.Property(e => e.GuidRow).IsRequired();
            builder.Property(e => e.Id).IsRequired();
            builder.HasKey(e => e.Id);

            builder.Property(e => e.Name).HasMaxLength(500).IsRequired();
            builder.Property(e => e.DeviceId).HasMaxLength(500).IsRequired();
            builder.Property(e => e.DeviceSecret).HasMaxLength(500).IsRequired(false);
            builder.Property(e => e.Region).HasMaxLength(500).IsRequired(false);
            builder.Property(e => e.CreateDate).IsRequired();
            builder.Property(e => e.DeleteDate).IsRequired(false);
            builder.Property(e => e.Description).IsRequired(false);
        }
    }
}
