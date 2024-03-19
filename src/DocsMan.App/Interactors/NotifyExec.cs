using DocsMan.App.Mappers;
using DocsMan.App.Storage.RepositoryPattern;
using DocsMan.App.Storage.Transaction;
using DocsMan.Blazor.Shared.DTOs;
using DocsMan.Blazor.Shared.OutputData;
using DocsMan.Domain.BinderEntity;
using DocsMan.Domain.Entity;

namespace DocsMan.App.Interactors
{
	public class NotifyExec
	{
		private IRepository<Notification> _notifyRepos;
		private IBindingRepository<Profile_Notify> _notifyBind;
		private IUnitWork _unitWork;

		public NotifyExec(
			IRepository<Notification> notifyRepos,
			IBindingRepository<Profile_Notify> notifyBind,
			IUnitWork unitWork)
		{
			_notifyRepos = notifyRepos;
			_notifyBind = notifyBind;
			_unitWork = unitWork;
		}

		public async Task<Response<int>> CreateNotify(NotificationDto dto)
		{
			try
			{
				var notify = dto?.ToEntity();
				await _notifyRepos.CreateAsync(notify);
				await _unitWork.Commit();

				return new(notify.Id);
			}
			catch (ArgumentNullException ex)
			{
				return new($"Пустые входные данные: {ex.ParamName}", "Internal error of entity null props");
			}
			catch (NullReferenceException ex)
			{
				return new("Запись не найдена", ex.Message);
			}
			catch (Exception ex)
			{
				return new("Ошибка создания", ex.Message);
			}
		}

		public async Task<Response> DeleteNotify(int id)
		{
			try
			{
				await _notifyRepos.DeleteAsync(id);
				await _unitWork.Commit();

				return new();
			}
			catch (ArgumentNullException ex)
			{
				return new("Пустые входные данные", ex.ParamName);
			}
			catch (NullReferenceException ex)
			{
				return new("Запись не найдена", ex.Message);
			}
			catch (Exception ex)
			{
				return new("Ошибка удаления", ex.Message);
			}
		}

		public async Task<Response> CreateBindNotify(int profileId, int notifyId)
		{
			try
			{
				await _notifyBind.CreateBindAsync(
					new()
					{
						ProfileId = profileId,
						NotificationId = notifyId,
						IsRead = false
					});
				await _unitWork.Commit();

				return new();
			}
			catch (ArgumentNullException ex)
			{
				return new($"Пустые входные данные: {ex.ParamName}", "Internal error of entity null props");
			}
			catch (NullReferenceException ex)
			{
				return new("Запись не найдена", ex.Message);
			}
			catch (Exception ex)
			{
				return new("Ошибка создания", ex.Message);
			}
		}

		public async Task<Response> DeleteBindNotify(int profileId, int notifyId)
		{
			try
			{
				await _notifyBind.DeleteBindAsync(
					new()
					{
						ProfileId = profileId,
						NotificationId = notifyId
					});
				await _unitWork.Commit();

				return new();
			}
			catch (ArgumentNullException ex)
			{
				return new("Пустые входные данные", ex.ParamName);
			}
			catch (NullReferenceException ex)
			{
				return new("Запись не найдена", ex.Message);
			}
			catch (Exception ex)
			{
				return new("Ошибка удаления", ex.Message);
			}
		}

		public async Task<Response> ReadNotify(int profileId, int notifyId)
		{
			try
			{
				var bind = (await _notifyBind.GetAllBinds())?
					.FirstOrDefault(x => x.ProfileId == profileId && x.NotificationId == notifyId);
				bind.IsRead = true;
				await _unitWork.Commit();

				return new();
			}
			catch (ArgumentNullException ex)
			{
				return new("Пустые входные данные", ex.ParamName);
			}
			catch (NullReferenceException ex)
			{
				return new("Запись не найдена", ex.Message);
			}
			catch (Exception ex)
			{
				return new("Ошибка изменения", ex.Message);
			}
		}

		public async Task<Response> ForgetNotify(int profileId, int notifyId)
		{
			try
			{
				var bind = (await _notifyBind.GetAllBinds())?
					.FirstOrDefault(x => x.ProfileId == profileId && x.NotificationId == notifyId);
				bind.IsRead = false;
				await _unitWork.Commit();

				return new();
			}
			catch (ArgumentNullException ex)
			{
				return new("Пустые входные данные", ex.ParamName);
			}
			catch (NullReferenceException ex)
			{
				return new("Запись не найдена", ex.Message);
			}
			catch (Exception ex)
			{
				return new("Ошибка изменения", ex.Message);
			}
		}

		public async Task<Response<IEnumerable<NotificationDto>?>> GetAll()
		{
			try
			{
				return new((await _notifyRepos.GetAllAsync())?.Select(x => x.ToDto()));
			}
			catch (ArgumentNullException ex)
			{
				return new("Пустые входные данные", ex.ParamName);
			}
			catch (NullReferenceException ex)
			{
				return new("Запись не найдена", ex.Message);
			}
			catch (Exception ex)
			{
				return new("Ошибка получения", ex.Message);
			}
		}

		public async Task<Response> ClearNotify(int profileId, int notifyId)
		{
			try
			{
				var countBind = (await _notifyBind.GetAllBindsNoTracking())?
					.Where(x => x.NotificationId == notifyId)?
					.Count();
				if (countBind == 1)
					return await DeleteNotify(notifyId);
				else
					return await DeleteBindNotify(profileId, notifyId);
			}
			catch (ArgumentNullException ex)
			{
				return new("Пустые входные данные", ex.ParamName);
			}
			catch (NullReferenceException ex)
			{
				return new("Запись не найдена", ex.Message);
			}
			catch (Exception ex)
			{
				return new("Ошибка удаления", ex.Message);
			}
		}
	}
}