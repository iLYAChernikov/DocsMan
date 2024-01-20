using DocsMan.Blazor.Shared.DTOs;
using DocsMan.Domain.Entity;

namespace DocsMan.App.Mappers
{
	public static class DocumentHistoryMapper
	{
		public static DocumentHistoryDto? ToDto(this DocumentHistory ent) =>
			ent == null ? null : new()
			{
				DocumentId = ent.DocumentId,
				DateTimeOfChanges = ent.DateTimeOfChanges,
				Description = ent.Description,
				FileId = ent.FileId
			};

		public static DocumentHistory? ToEntity(this DocumentHistoryDto dto) =>
			dto == null ? null : new()
			{
				DocumentId = dto.DocumentId,
				DateTimeOfChanges = dto.DateTimeOfChanges,
				Description = dto.Description,
				FileId = dto.FileId
			};
	}
}