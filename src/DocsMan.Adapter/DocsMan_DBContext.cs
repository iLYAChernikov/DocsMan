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

		public DbSet<Document> Documents { get; set; }
		public DbSet<DocumentHistory> DocumentHistories { get; set; }
		public DbSet<Folder> Folders { get; set; }
		public DbSet<Group> Groups { get; set; }
		public DbSet<Notification> Notifications { get; set; }
		public DbSet<PersonalDocument> PersonalDocuments { get; set; }
		public DbSet<PersonalDocumentType> PersonalDocumentTypes { get; set; }
		public DbSet<Profile> Profiles { get; set; }
		public DbSet<Role> Roles { get; set; }
		public DbSet<UploadFile> Files { get; set; }
		public DbSet<User> Users { get; set; }

		public DbSet<Folder_Document> Folder_Documents { get; set; }
		public DbSet<Folder_Folder> Folder_Folders { get; set; }
		public DbSet<Profile_Document> Profile_Documents { get; set; }
		public DbSet<Profile_Folder> Profile_Folders { get; set; }
		public DbSet<Profile_Group> Profile_Groups { get; set; }
		public DbSet<Profile_Notify> Profile_Notifications { get; set; }
		public DbSet<User_Role> User_Roles { get; set; }


		private List<Role> defaultRoles = new()
		{
			new (){ Id = 1, Title = "user",         Description = "default user"             },
			new (){ Id = 2, Title = "superAdmin",   Description = "control system user"      }
		};

		private List<PersonalDocumentType> personalDocumentTypes = new()
		{
			new (){ Id = 1, Title = "Фото", Description = "фото профиля, аватарка" }
		};

		private User superUser = new()
		{
			Id = 1,
			Email = "admin",
			Password = "123"
		};

		private List<User_Role> superUserRoles = new()
		{
			new (){ UserId = 1, RoleId = 1 },
			new (){ UserId = 1, RoleId = 2 }
		};

		private Profile superUserProfile = new()
		{
			Id = 1,
			UserId = 1,
			Gender = Gender.Man,
			Birthdate = new DateTime(2002, 11, 20),
			SurName = "SuperAdmin",
			Name = "Chief",
			LastName = "Qwerty"
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
			modelBuilder
				.ApplyConfiguration(new Profile_Folder_Config());

			modelBuilder.Entity<Role>().HasData(defaultRoles);
			modelBuilder.Entity<PersonalDocumentType>().HasData(personalDocumentTypes);
			modelBuilder.Entity<User>().HasData(superUser);
			modelBuilder.Entity<User_Role>().HasData(superUserRoles);
			modelBuilder.Entity<Profile>().HasData(superUserProfile);
		}
	}
}