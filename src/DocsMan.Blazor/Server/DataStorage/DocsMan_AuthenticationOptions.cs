using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace DocsMan.Blazor.Server.DataStorage
{
	public class DocsMan_AuthenticationOptions
	{
		// Издатель токена
		public string Issuer { get; set; } = null!;

		// Потребитель токена
		public string Audience { get; set; } = null!;

		// Время жизни токена
		public TimeSpan LifeTime { get; set; }

		// Ключ для симметричного шифрования
		public string SecretKey { get; set; } = null!;

		public SymmetricSecurityKey GetSymmetricSecurityKey()
		{
			return new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SecretKey));
		}
	}
}