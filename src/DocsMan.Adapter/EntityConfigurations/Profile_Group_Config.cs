using DocsMan.Domain.BinderEntity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DocsMan.Adapter.EntityConfigurations
{
	public class Profile_Group_Config : IEntityTypeConfiguration<Profile_Group>
	{
		public void Configure(EntityTypeBuilder<Profile_Group> builder)
		{
			builder.HasKey(x => new { x.ProfileId, x.GroupId });
		}
	}
}