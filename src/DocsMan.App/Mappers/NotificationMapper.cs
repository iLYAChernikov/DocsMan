using DocsMan.Blazor.Shared.DTOs;
using DocsMan.Domain.Entity;

namespace DocsMan.App.Mappers
{
	public static class NotificationMapper
	{
		public static NotificationDto? ToDto(this Notification ent) =>
			ent == null ? null : new()
			{
				Id = ent.Id,
				Title = ent.Title,
				Description = ent.Description,
				DateTime = ent.DateTime
			};

		public static Notification? ToEntity(this NotificationDto dto) =>
			dto == null ? null : new()
			{
				Id = dto.Id,
				Title = dto.Title,
				Description = dto.Description,
				DateTime = dto.DateTime
			};
	}
}