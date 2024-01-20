using DocsMan.Blazor.Shared.DTOs;
using DocsMan.Domain.Entity;

namespace DocsMan.App.Mappers
{
	public static class PersonalDocumentMapper
	{
		public static PersonalDocumentDto? ToDto(this PersonalDocument ent) =>
			ent == null ? null : new()
			{
				ProfileId = ent.ProfileId,
				TypeId = ent.TypeId,
				PersonalDocumentType = ent.PersonalDocumentType.ToDto(),
				FileId = ent.FileId,
				Text = ent.Text
			};

		public static PersonalDocument? ToEntity(this PersonalDocumentDto dto) =>
			dto == null ? null : new()
			{
				ProfileId = dto.ProfileId,
				TypeId = dto.TypeId,
				FileId = dto.FileId,
				Text = dto.Text
			};
	}
}