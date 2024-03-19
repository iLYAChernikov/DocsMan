using DocsMan.Domain.BinderEntity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DocsMan.Adapter.EntityConfigurations
{
	public class Folder_Document_Config : IEntityTypeConfiguration<Folder_Document>
	{
		public void Configure(EntityTypeBuilder<Folder_Document> builder)
		{
			builder.HasKey(x => new { x.FolderId, x.DocumentId });
		}
	}
}