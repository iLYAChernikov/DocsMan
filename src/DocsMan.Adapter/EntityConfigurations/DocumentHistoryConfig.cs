using DocsMan.Domain.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DocsMan.Adapter.EntityConfigurations
{
	public class DocumentHistoryConfig : IEntityTypeConfiguration<DocumentHistory>
	{
		public void Configure(EntityTypeBuilder<DocumentHistory> builder)
		{
			builder.HasKey(x => new { x.DocumentId, x.DateTimeOfChanges });
		}
	}
}