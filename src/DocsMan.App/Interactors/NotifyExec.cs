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
		private IRepository<Profile> _profileRepos;
		private IUnitWork _unitWork;

		public NotifyExec(
			IRepository<Notification> notifyRepos,
			IBindingRepository<Profile_Notify> notifyBind,
			IRepository<Profile> profileRepos,
			IUnitWork unitWork)
		{
			_notifyRepos = notifyRepos;
			_notifyBind = notifyBind;
			_profileRepos = profileRepos;
			_unitWork = unitWork;
		}

		public async Task<Response> CreateNotify(NotificationDto dto)
		{
			try
			{
				await _notifyRepos.CreateAsync(dto?.ToEntity());
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
	}
}