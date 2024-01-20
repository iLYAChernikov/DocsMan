using DocsMan.Domain.BinderEntity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DocsMan.Adapter.EntityConfigurations
{
	public class User_Role_Config : IEntityTypeConfiguration<User_Role>
	{
		public void Configure(EntityTypeBuilder<User_Role> builder)
		{
			builder.HasKey(x => new { x.UserId, x.RoleId });
		}
	}
}