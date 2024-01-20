using DocsMan.Domain.BinderEntity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DocsMan.Adapter.EntityConfigurations
{
	public class Profile_Notify_Config : IEntityTypeConfiguration<Profile_Notify>
	{
		public void Configure(EntityTypeBuilder<Profile_Notify> builder)
		{
			builder.HasKey(x => new { x.ProfileId, x.NotificationId });
		}
	}
}