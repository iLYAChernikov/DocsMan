using DocsMan.Domain.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DocsMan.Adapter.EntityConfigurations
{
	public class PersonalDocumentConfig : IEntityTypeConfiguration<PersonalDocument>
	{
		public void Configure(EntityTypeBuilder<PersonalDocument> builder)
		{
			builder.HasKey(x => new { x.ProfileId, x.TypeId });

			builder
				.HasOne(x => x.PersonalDocumentType)
				.WithMany()
				.HasForeignKey(x => x.TypeId);
		}
	}
}