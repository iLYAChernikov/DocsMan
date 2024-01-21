using DocsMan.Domain.BinderEntity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DocsMan.Adapter.EntityConfigurations
{
	public class Profile_Folder_Config : IEntityTypeConfiguration<Profile_Folder>
	{
		public void Configure(EntityTypeBuilder<Profile_Folder> builder)
		{
			builder.HasKey(x => new { x.ProfileId, x.FolderId });
		}
	}
}