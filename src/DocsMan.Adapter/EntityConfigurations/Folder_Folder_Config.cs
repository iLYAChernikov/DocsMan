using DocsMan.Domain.BinderEntity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DocsMan.Adapter.EntityConfigurations
{
	public class Folder_Folder_Config : IEntityTypeConfiguration<Folder_Folder>
	{
		public void Configure(EntityTypeBuilder<Folder_Folder> builder)
		{
			builder.HasKey(x => new { x.OwnerFolderId, x.ChildFolderId });
		}
	}
}