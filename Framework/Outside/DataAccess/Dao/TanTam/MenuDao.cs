using System;
using System.Collections.Generic;
using System.Linq;
using DataAccess.EF;
using DataAccess.Interface;
using EntitiesObject.Entities.TanTamEntities;

namespace DataAccess.Dao.TanTam
{
    public interface IMenuDao : IBaseFactories<DBNull>
    {
        List<Ins_Menu_GetByRoleId_Result> GetMenuByRoleId(int roleId);
        void AddMenu(int? parentId, string key, string title, string url, string icon, string apiRouteName, int status, int order, int menuType);
        void UpdateMenu(int id, int? parentId, string key, string title, string url, string icon, string apiRouteName, int status, int order, int menuType);
        void DeleteMenu(int id);
        List<int> GetMenuIdsByRole(int roleId);
        void AddMenuRole(int menuId, int roleId);
        void DeleteMenuRole(int menuId, int roleId);
        List<Ins_Menu_GetAll_Result> GetAllMenu();
    }

    internal class MenuDao : DaoFactories<TanTamEntities, DBNull>, IMenuDao
    {
        public List<Ins_Menu_GetByRoleId_Result> GetMenuByRoleId(int roleId)
        {
            using (Uow)
            {
                return Uow.Context.Ins_Menu_GetByRoleId(roleId).ToList();
            }
        }
        public void AddMenu(int? parentId, string key, string title, string url, string icon, string apiRouteName, int status, int order, int menuType)
        {
            Uow.Context.Ins_Menu_Add(parentId, key, title, url, icon, apiRouteName, status, order, menuType);
        }
        public void UpdateMenu(int id, int? parentId, string key, string title, string url, string icon, string apiRouteName, int status, int order, int menuType)
        {
            Uow.Context.Ins_Menu_Update(id, parentId, key, title, url, icon, apiRouteName, status, order, menuType);
        }
        public void DeleteMenu(int id)
        {
            Uow.Context.Ins_Menu_Delete(id);
        }
        public List<int> GetMenuIdsByRole(int roleId)
        {
            return Uow.Context.Ins_MenuRole_GetMenuIdsByRole(roleId).Where(x => x.HasValue).Select(x => x.Value).ToList();
        }
        public void AddMenuRole(int menuId, int roleId)
        {
            Uow.Context.Ins_MenuRole_Add(menuId, roleId);
        }
        public void DeleteMenuRole(int menuId, int roleId)
        {
            Uow.Context.Ins_MenuRole_Delete(menuId, roleId);
        }
        public List<Ins_Menu_GetAll_Result> GetAllMenu()
        {
            using (Uow)
            {
                return Uow.Context.Ins_Menu_GetAll().ToList();
            }
        }
    }
} 