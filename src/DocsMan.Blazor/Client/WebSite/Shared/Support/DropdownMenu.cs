namespace DocsMan.Blazor.Client.WebSite.Shared.Support;

public class DropdownMenu
{
	public string Title { get; set; } = string.Empty;
	public string Icon { get; set; } = string.Empty;
	public bool IsOpen { get; set; } = false;
	public List<SubMenu> Submenus { get; set; } = new();
}

public class SubMenu
{
	public string Title { get; set; } = string.Empty;
	public string Icon { get; set; } = string.Empty;
	public string HrefUrl { get; set; } = string.Empty;
}

public class ToggleStyleCSS
{
	private string _defaultStyle;
	private string _openStyle;
	private string _disabledStyle;
	public ToggleStyleCSS(string defaultStyle, string openStyle, string disabledStyle)
	{
		_defaultStyle = defaultStyle;
		_openStyle = openStyle;
		_disabledStyle = disabledStyle;
	}
	public string BlockStyle => IsDisabled ? _disabledStyle : IsOpen ? _openStyle : _defaultStyle;
	public bool IsOpen { get; set; } = false;
	public bool IsDisabled { get; set; } = true;
}

public static class CSS_Styles
{
	public static List<string> OpenCloseStyles => new() { "", "" };

	public static string GetImage(byte[]? imageData) =>
		imageData == null ?
		"/img/none-photo.png" :
		string.Format("data:image/png;base64," + Convert.ToBase64String(imageData));
}

public static class HeaderNavMenuList
{
	public static List<DropdownMenu> headerMenu = new()
	{
		new ()
		{
			Submenus = new()
			{
				new()
				{
					Title = "Профиль",
					Icon = "fas fa-user-tie",
					HrefUrl = "User/Profile"
				},
				new()
				{
					Title = "Документы",
					Icon = "fas fa-id-card",
					HrefUrl = "User/Docs"
				},
				new()
				{
					Title = "Группы",
					Icon = "fas fa-users",
					HrefUrl = "User/Groups"
				},
				new ()
				{
					Title = "Настройки",
					HrefUrl = "User/Profile/Settings",
					Icon = "fas fa-user-cog"
				}
			}
		}
	};
}

public static class SideBarNavMenuList
{
	public static List<SubMenu> userMenu = new()
	{
		new ()
		{
			Title = "Менеджер",
			Icon = "fas fa-folder-open",
			HrefUrl = "User/Files"
		},
		new ()
		{
			Title = "Корзина",
			Icon = "fa-solid fa-trash-arrow-up",
			HrefUrl = "User/Trash"
		},
	};

	public static List<SubMenu> adminMenu = new()
	{
		new ()
		{
			Title = "Admin Panel",
			Icon = "far fa-user-secret",
			HrefUrl = "Admin/Panel"
		}
	};

	public static List<SubMenu> adminPanelMenu = new()
	{
		new ()
		{
			Title = "Home",
			Icon = "far fa-home",
			HrefUrl = "/"
		},
		new ()
		{
			Title = "Roles",
			Icon = "fas fa-user-tag",
			HrefUrl = "Admin/Role"
		},
		new ()
		{
			Title = "DocsTypes",
			Icon = "fas fa-id-card",
			HrefUrl = "Admin/DocsType"
		},
		new ()
		{
			Title = "Groups",
			Icon = "fas fa-users",
			HrefUrl = "Admin/Group"
		},
		new ()
		{
			Title = "Notifying",
			Icon = "fas fa-envelope",
			HrefUrl = "Admin/Notify"
		},
	};
}