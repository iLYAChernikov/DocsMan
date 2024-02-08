using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using AltairCA.Blazor.WebAssembly.Cookie.Framework;

namespace DocsMan.Blazor.Client
{
	public class Program
	{
		public static async Task Main(string[] args)
		{
			var builder = WebAssemblyHostBuilder.CreateDefault(args);
			builder.RootComponents.Add<App>("#app");
			builder.RootComponents.Add<HeadOutlet>("head::after");

			builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

			builder.Services.AddAltairCACookieService(options =>
			{
				options.DefaultExpire = TimeSpan.FromHours(12);
			});

			await builder.Build().RunAsync();
		}
	}
}