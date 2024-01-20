using DocsMan.Adapter.EntityConfigurations;
using DocsMan.Domain.BinderEntity;
using DocsMan.Domain.Entity;

using Microsoft.EntityFrameworkCore;

namespace DocsMan.Adapter
{
	public class DocsMan_DBContext : DbContext
	{
		public DocsMan_DBContext(DbContextOptions<DocsMan_DBContext> options) : base(options)
		{
			Database.EnsureCreatedAsync();
		}

		DbSet<Document> Documents { get; set; }
		DbSet<DocumentHistory> DocumentHistories { get; set; }
		DbSet<Folder> Folders { get; set; }
		DbSet<Group> Groups { get; set; }
		DbSet<Notification> Notifications { get; set; }
		DbSet<PersonalDocument> PersonalDocuments { get; set; }
		DbSet<PersonalDocumentType> PersonalDocumentTypes { get; set; }
		DbSet<Profile> Profiles { get; set; }
		DbSet<Role> Roles { get; set; }
		DbSet<UploadFile> Files { get; set; }
		DbSet<User> Users { get; set; }

		DbSet<Folder_Document> Folder_Documents { get; set; }
		DbSet<Folder_Folder> Folder_Folders { get; set; }
		DbSet<Profile_Document> Profile_Documents { get; set; }
		DbSet<Profile_Group> Profile_Groups { get; set; }
		DbSet<Profile_Notify> Profile_Notifications { get; set; }
		DbSet<User_Role> User_Roles { get; set; }


		private List<Role> defaultRoles = new()
		{
			new Role(){ Id = 1, Title = "superAdmin",   Description = "control system settings"     },
			new Role(){ Id = 2, Title = "admin",        Description = "control in system"           },
			new Role(){ Id = 3, Title = "user",         Description = "default system user"         }
		};

		private List<PersonalDocumentType> personalDocumentTypes = new()
		{
			new PersonalDocumentType(){ Id = 1, Title = "Фото", Description = "фото профиля, аватарка" }
		};

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder
				.ApplyConfiguration(new DocumentHistoryConfig());
			modelBuilder
				.ApplyConfiguration(new Folder_Document_Config());
			modelBuilder
				.ApplyConfiguration(new Folder_Folder_Config());
			modelBuilder
				.ApplyConfiguration(new PersonalDocumentConfig());
			modelBuilder
				.ApplyConfiguration(new Profile_Document_Config());
			modelBuilder
				.ApplyConfiguration(new Profile_Group_Config());
			modelBuilder
				.ApplyConfiguration(new Profile_Notify_Config());
			modelBuilder
				.ApplyConfiguration(new User_Role_Config());

			modelBuilder.Entity<Role>().HasData(defaultRoles);
			modelBuilder.Entity<PersonalDocumentType>().HasData(personalDocumentTypes);
		}
	}
}