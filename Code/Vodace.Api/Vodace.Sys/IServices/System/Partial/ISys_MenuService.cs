using System.Collections.Generic;
using System.Threading.Tasks;
using Vodace.Core.Utilities;
using Vodace.Entity;
using Vodace.Entity.DomainModels;

namespace Vodace.Sys.IServices
{
    public partial interface ISys_MenuService
    {
        Task<object> GetMenu();
        List<Sys_Menu> GetCurrentMenuList();

        List<Sys_Menu> GetUserMenuList(int roleId);

        object GetCurrentMenuActionListNew();
        object GetCurrentMenuActionList();
        object GetCurrentMenuActionLisAll();

        object GetMenuActionList(int roleId);
        Task<WebResponseContent> Save(Sys_Menu menu);

        Task<WebResponseContent> DelMenu(int menuId);


        Task<object> GetTreeItem(int menuId);
        Task<WebResponseContent> GetListByPage(PageInput<MenuQuery> query);
        Task<WebResponseContent> GetParentMenu();
        Task<WebResponseContent> GetMenuById(int id);
        WebResponseContent AddData(MenuAddDto menu);
        WebResponseContent EditData(MenuDto menu);
        WebResponseContent DelData(int menuId);
        WebResponseContent SetMenuHidden(MenuHiddenDto menu);
        Task<WebResponseContent> GetMenuTree();
    }
}

