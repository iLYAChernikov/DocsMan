using DocsMan.Adapter;
using DocsMan.Adapter.Repository;
using DocsMan.Adapter.Repository.Bindings;
using DocsMan.Adapter.Transaction;
using DocsMan.App.Interactors;
using DocsMan.App.Storage.RepositoryPattern;
using DocsMan.App.Storage.Transaction;
using DocsMan.Blazor.Server.DataStorage;
using DocsMan.Domain.BinderEntity;
using DocsMan.Domain.Entity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
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

			//  Start Authentication Block

			//	for getting settings in appsettings.json
			DocsMan_AuthenticationOptions GetAuthenticationOptions(IConfiguration configuration) =>
				configuration.GetSection("AuthOptions").Get<DocsMan_AuthenticationOptions>();

			//	bind method
			builder.Services.AddSingleton<DocsMan_AuthenticationOptions>
				(provider => GetAuthenticationOptions(builder.Configuration));

			//	get auth settings
			var authOpts = GetAuthenticationOptions(builder.Configuration);

			builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
				.AddJwtBearer(opts =>
				{
					opts.TokenValidationParameters = new TokenValidationParameters()
					{
						ValidateIssuer = true,
						ValidIssuer = authOpts.Issuer,

						ValidateAudience = true,
						ValidAudience = authOpts.Audience,

						ValidateLifetime = true,

						ValidateIssuerSigningKey = true,

						IssuerSigningKey = authOpts.GetSymmetricSecurityKey()
					};
				});

			// End Authentication Block

			//	||	Other Services settings
			//	\/

			//	Repositories services

			//	Transaction
			builder.Services.AddTransient<IUnitWork, UnitWork>();

			//	Repositories
			builder.Services.AddScoped<IRepository<Role>, GenericRepository<Role>>();
			builder.Services.AddScoped<IRepository<User>, GenericRepository<User>>();
			builder.Services.AddScoped<IRepository<PersonalDocumentType>, GenericRepository<PersonalDocumentType>>();
			builder.Services.AddScoped<IRepository<UploadFile>, GenericRepository<UploadFile>>();
			builder.Services.AddScoped<IRepository<Document>, GenericRepository<Document>>();
			builder.Services.AddScoped<IRepository<DocumentHistory>, GenericRepository<DocumentHistory>>();

			builder.Services.AddScoped<IRepository<Profile>, ProfileRepository>();
			builder.Services.AddScoped<IRepository<PersonalDocument>, PersonalDocumentRepository>();

			//	Binding Repositories
			builder.Services.AddScoped<IBindingRepository<User_Role>, User_Role_BindRepository>();
			builder.Services.AddScoped<IBindingRepository<Profile_Document>, Profile_Document_BindRepository>();

			//	Interactors
			builder.Services.AddScoped<RoleExec>();
			builder.Services.AddScoped<UserExec>();
			builder.Services.AddScoped<ProfileExec>();
			builder.Services.AddScoped<UploadFileExec>();
			builder.Services.AddScoped<PersonalDocumentTypeExec>();
			builder.Services.AddScoped<FileManagerExec>();
			builder.Services.AddScoped<DocumentHistoryExec>();
			builder.Services.AddScoped<AuthExec>();

			builder.Services.AddControllers();
			builder.Services.AddControllersWithViews();
			builder.Services.AddRazorPages();

			//	Swagger
			builder.Services.AddSwaggerGen(options =>
			{
				options.SwaggerDoc("v2", new OpenApiInfo { Title = "DocsMan_Service", Version = "v2" });

				//	Authentication for Swagger
				options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
				{
					Name = "Authorization",
					Type = SecuritySchemeType.ApiKey,
					Scheme = JwtBearerDefaults.AuthenticationScheme,
					BearerFormat = "JWT",
					In = ParameterLocation.Header,
					Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 1safsfsdfdfd\"",
				});

				options.AddSecurityRequirement(new OpenApiSecurityRequirement
			{
				{
					new OpenApiSecurityScheme
					{
						Reference = new OpenApiReference
						{
							Type = ReferenceType.SecurityScheme,
							Id = "Bearer"
						}
					},
					new string[] { }
				}
			});
			});

			//	cors
			builder.Services.AddCors(options => options.AddPolicy("CorsPolicy",
			policyBuilder =>
			{
				policyBuilder
				.WithOrigins("https://localhost:7075")
				.AllowAnyHeader()
				.AllowAnyMethod();
			}
			));

			var app = builder.Build();

			// Configure the HTTP request pipeline.
			if (app.Environment.IsDevelopment())
			{
				app.UseWebAssemblyDebugging();
				app.UseSwagger();
				app.UseSwaggerUI(c =>
				{
					c.SwaggerEndpoint("/swagger/v2/swagger.json", "DocsMan Blazor API v2.0");
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

			app.UseCors("CorsPolicy");

			app.UseAuthentication();
			app.UseAuthorization();

			app.MapRazorPages();
			app.MapControllers();
			app.MapFallbackToFile("index.html");

			app.Run();
		}
	}
}