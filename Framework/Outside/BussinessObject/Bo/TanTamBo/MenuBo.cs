using System.Collections.Generic;
using System.Linq;
using DataAccess;
using BussinessObject.Models.Menu;
using Logger;
using Newtonsoft.Json;

namespace BussinessObject.Bo.TanTamBo
{
    public class MenuBo
    {
        public List<MenuDto> GetMenuTreeByRole(int roleId)
        {
            var flatMenus = DaoFactory.Menu.GetMenuByRoleId(roleId);
            var lookup = flatMenus.ToLookup(x => x.ParentId);
            List<MenuDto> Build(int? parentId, int menuType)
            {
                return lookup[parentId]
                    .Where(x => x.MenuType == menuType && x.Status == 1)
                    .OrderBy(x => x.Order)
                    .Select(x => new MenuDto
                    {
                        Id = x.Id,
                        ParentId = x.ParentId,
                        Key = x.Key,
                        Title = x.Title,
                        Url = x.Url,
                        Icon = x.Icon,
                        ApiRouteName = x.ApiRouteName,
                        Status = x.Status,
                        Order = x.Order,
                        MenuType = x.MenuType,
                        SubMenu = Build(x.Id, 1),
                        Children = Build(x.Id, 2)
                    }).ToList();
            }
            return Build(null, 0);
        }
        public void AddMenu(MenuDto dto)
        {
            DaoFactory.Menu.AddMenu(dto.ParentId, dto.Key, dto.Title, dto.Url, dto.Icon, dto.ApiRouteName, dto.Status, dto.Order, dto.MenuType);
        }
        public void UpdateMenu(MenuDto dto)
        {
            DaoFactory.Menu.UpdateMenu(dto.Id, dto.ParentId, dto.Key, dto.Title, dto.Url, dto.Icon, dto.ApiRouteName, dto.Status, dto.Order, dto.MenuType);
        }
        public void DeleteMenu(int id)
        {
            DaoFactory.Menu.DeleteMenu(id);
        }
        public List<int> GetMenuIdsByRole(int roleId)
        {
            return DaoFactory.Menu.GetMenuIdsByRole(roleId);
        }
        public void AddMenuRole(int menuId, int roleId)
        {
            DaoFactory.Menu.AddMenuRole(menuId, roleId);
        }
        public void DeleteMenuRole(int menuId, int roleId)
        {
            DaoFactory.Menu.DeleteMenuRole(menuId, roleId);
        }
        public List<MenuDto> GetAllMenuTree()
        {
            var flatMenus = DaoFactory.Menu.GetAllMenu();
            var lookup = flatMenus.ToLookup(x => x.ParentId);
            List<MenuDto> Build(int? parentId, int menuType)
            {
                return lookup[parentId]
                    .Where(x => x.MenuType == menuType)
                    .OrderBy(x => x.Order)
                    .Select(x => new MenuDto
                    {
                        Id = x.Id,
                        ParentId = x.ParentId,
                        Key = x.Key,
                        Title = x.Title,
                        Url = x.Url,
                        Icon = x.Icon,
                        ApiRouteName = x.ApiRouteName,
                        Status = x.Status,
                        Order = x.Order,
                        MenuType = x.MenuType,
                        SubMenu = Build(x.Id, 1),
                        Children = Build(x.Id, 2)
                    }).ToList();
            }
            return Build(null, 0);
        }
    }
} 