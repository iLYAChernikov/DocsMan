using DocsMan.Domain.BinderEntity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DocsMan.Adapter.EntityConfigurations
{
	public class Profile_Document_Config : IEntityTypeConfiguration<Profile_Document>
	{
		public void Configure(EntityTypeBuilder<Profile_Document> builder)
		{
			builder.HasKey(x => new { x.ProfileId, x.DocumentId });
		}
	}
}