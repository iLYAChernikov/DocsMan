using DocsMan.Blazor.Shared.DTOs;
using DocsMan.Domain.Entity;

namespace DocsMan.App.Mappers
{
	public static class FolderMapper
	{
		public static FolderDto? ToDto(this Folder ent) =>
			ent == null ? null : new()
			{
				Id = ent.Id,
				Name = ent.Name,
				Description = ent.Description
			};

		public static Folder? ToEntity(this FolderDto dto) =>
			dto == null ? null : new()
			{
				Id = dto.Id,
				Name = dto.Name,
				Description = dto.Description
			};
	}
}