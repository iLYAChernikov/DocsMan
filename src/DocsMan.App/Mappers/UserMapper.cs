using DocsMan.Blazor.Shared.DTOs;
using DocsMan.Domain.Entity;

namespace DocsMan.App.Mappers
{
	public static class UserMapper
	{
		public static UserDto? ToDto(this User ent) =>
			ent == null ? null : new()
			{
				Id = ent.Id,
				Email = ent.Email,
				Password = ent.Password
			};

		public static User? ToEntity(this UserDto dto) =>
			dto == null ? null : new()
			{
				Id = dto.Id,
				Email = dto.Email,
				Password = dto.Password
			};
	}
}