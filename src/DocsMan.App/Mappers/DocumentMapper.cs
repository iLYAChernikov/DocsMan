using DocsMan.Blazor.Shared.DTOs;
using DocsMan.Domain.Entity;

namespace DocsMan.App.Mappers
{
	public static class DocumentMapper
	{
		public static DocumentDto? ToDto(this Document ent) =>
			ent == null ? null : new()
			{
				Id = ent.Id,
				Name = ent.Name,
				FileType = ent.FileType,
				Description = ent.Description,
				FileId = ent.FileId,
				IsDeleted = ent.IsDeleted
			};

		public static Document? ToEntity(this DocumentDto dto) =>
			dto == null ? null : new()
			{
				Id = dto.Id,
				Name = dto.Name,
				FileType = dto.FileType,
				Description = dto.Description,
				FileId = dto.FileId,
				IsDeleted = dto.IsDeleted
			};
	}
}