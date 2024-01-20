using DocsMan.Blazor.Shared.DTOs;
using DocsMan.Domain.Entity;

namespace DocsMan.App.Mappers
{
	public static class PersonalDocumentTypeMapper
	{
		public static PersonalDocumentTypeDto? ToDto(this PersonalDocumentType ent) =>
			ent == null ? null : new()
			{
				Id = ent.Id,
				Title = ent.Title,
				Description = ent.Description
			};

		public static PersonalDocumentType? ToEntity(this PersonalDocumentTypeDto dto) =>
			dto == null ? null : new()
			{
				Id = dto.Id,
				Title = dto.Title,
				Description = dto.Description
			};
	}
}