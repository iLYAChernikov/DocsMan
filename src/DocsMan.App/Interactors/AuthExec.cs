using DocsMan.App.Storage.RepositoryPattern;
using DocsMan.Blazor.Shared.OutputData;
using DocsMan.Domain.Entity;

namespace DocsMan.App.Interactors
{
	public class AuthExec
	{
		private IRepository<Profile> _profileRepos;

		public AuthExec(IRepository<Profile> profileRepos)
		{
			_profileRepos = profileRepos;
		}

		public async Task<Response<int>> GetProfileId(string? userTextData)
		{
			int userId;
			if (string.IsNullOrWhiteSpace(userTextData) || !int.TryParse(userTextData, out userId) || userId <= 0)
				return new("Ошибка авторизации", "Can't get user id");
			else
			{
				var profile = (await _profileRepos.GetAllAsync())?
					.FirstOrDefault(x => x.UserId == userId);

				if (profile == null)
					return new("Ошибка авторизации", "Can't get profile id");
				else
					return new(profile.Id);
			}
		}
	}
}