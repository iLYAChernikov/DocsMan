using DocsMan.Adapter;
using DocsMan.Adapter.Repository;
using DocsMan.Adapter.Repository.Bindings;
using DocsMan.Adapter.Transaction;
using DocsMan.App.Interactors;
using DocsMan.App.Storage.RepositoryPattern;
using DocsMan.App.Storage.Transaction;
using DocsMan.Domain.BinderEntity;
using DocsMan.Domain.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

namespace DocsMan.Blazor
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);

			// Add services to the container.

			//	DB context
			builder.Services.AddDbContext<DocsMan_DBContext>(
				settings => settings.UseSqlite(builder.Configuration.GetConnectionString("SQLiteConnection")));

			//	transaction
			builder.Services.AddTransient<IUnitWork, UnitWork>();

			//	repositories
			builder.Services.AddScoped<IRepository<Role>, GenericRepository<Role>>();
			builder.Services.AddScoped<IRepository<User>, GenericRepository<User>>();
			builder.Services.AddScoped<IRepository<PersonalDocumentType>, GenericRepository<PersonalDocumentType>>();
			builder.Services.AddScoped<IRepository<UploadFile>, GenericRepository<UploadFile>>();
			builder.Services.AddScoped<IRepository<Document>, GenericRepository<Document>>();
			builder.Services.AddScoped<IRepository<DocumentHistory>, GenericRepository<DocumentHistory>>();

			builder.Services.AddScoped<IRepository<Profile>, ProfileRepository>();
			builder.Services.AddScoped<IRepository<PersonalDocument>, PersonalDocumentRepository>();

			//	binding repositories
			builder.Services.AddScoped<IBindingRepository<User_Role>, User_Role_BindRepository>();
			builder.Services.AddScoped<IBindingRepository<Profile_Document>, Profile_Document_BindRepository>();

			//	interactors
			builder.Services.AddScoped<RoleExec>();
			builder.Services.AddScoped<UserExec>();
			builder.Services.AddScoped<ProfileExec>();
			builder.Services.AddScoped<UploadFileExec>();
			builder.Services.AddScoped<PersonalDocumentTypeExec>();
			builder.Services.AddScoped<FileManagerExec>();
			builder.Services.AddScoped<DocumentHistoryExec>();

			builder.Services.AddControllers();
			builder.Services.AddControllersWithViews();
			builder.Services.AddRazorPages();

			//	swagger
			builder.Services.AddSwaggerGen(options =>
			{
				options.SwaggerDoc("v1", new OpenApiInfo { Title = "DocsMan_Service", Version = "v1" });
			});

			var app = builder.Build();

			// Configure the HTTP request pipeline.
			if ( app.Environment.IsDevelopment() )
			{
				app.UseWebAssemblyDebugging();
				app.UseSwagger();
				app.UseSwaggerUI(c =>
				{
					c.SwaggerEndpoint("/swagger/v1/swagger.json", "DocsMan Blazor API v1.0");
				});
			}
			else
			{
				app.UseExceptionHandler("/Error");
				// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
				app.UseHsts();
			}

			app.UseHttpsRedirection();

			app.UseBlazorFrameworkFiles();
			app.UseStaticFiles();

			app.UseRouting();

			app.MapRazorPages();
			app.MapControllers();
			app.MapFallbackToFile("index.html");

			app.Run();
		}
	}
}