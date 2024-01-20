using DocsMan.Blazor.Shared.DTOs;
using DocsMan.Domain.Entity;

namespace DocsMan.App.Mappers
{
	public static class RoleMapper
	{
		public static RoleDto? ToDto(this Role ent) =>
			ent == null ? null : new()
			{
				Id = ent.Id,
				Title = ent.Title,
				Description = ent.Description
			};

		public static Role? ToEntity(this RoleDto dto) =>
			dto == null ? null : new()
			{
				Id = dto.Id,
				Title = dto.Title,
				Description = dto.Description
			};
	}
}