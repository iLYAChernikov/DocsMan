using DocsMan.Blazor.Shared.DTOs;
using DocsMan.Domain.Entity;

namespace DocsMan.App.Mappers
{
	public static class ProfileMapper
	{
		public static ProfileDto? ToDto(this Profile ent) =>
			ent == null ? null : new()
			{
				Id = ent.Id,
				UserId = ent.UserId,
				Name = ent.Name,
				SurName = ent.SurName,
				LastName = ent.LastName,
				Birthdate = ent.Birthdate,
				Email = ent.Email,
				PhoneNumber = ent.PhoneNumber,
				Gender = ent.Gender == Gender.Man ? GenderDto.Man : GenderDto.Woman
			};

		public static Profile? ToEntity(this ProfileDto dto) =>
			dto == null ? null : new()
			{
				Id = dto.Id,
				UserId = dto.UserId,
				Name = dto.Name,
				SurName = dto.SurName,
				LastName = dto.LastName,
				Birthdate = dto.Birthdate,
				PhoneNumber = dto.PhoneNumber,
				Gender = dto.Gender == GenderDto.Man ? Gender.Man : Gender.Woman
			};
	}
}