using DocsMan.Blazor.Shared.DTOs;
using DocsMan.Domain.Entity;

namespace DocsMan.App.Mappers
{
	public static class GroupMapper
	{
		public static GroupDto? ToDto(this Group ent) =>
			ent == null ? null : new()
			{
				Id = ent.Id,
				Title = ent.Title,
				Description = ent.Description
			};

		public static Group? ToEntity(this GroupDto dto) =>
			dto == null ? null : new()
			{
				Id = dto.Id,
				Title = dto.Title,
				Description = dto.Description
			};
	}
}