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

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<Role>().HasData(defaultRoles);
		}
	}
}