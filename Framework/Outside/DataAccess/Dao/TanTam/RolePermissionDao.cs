using System;
using System.Collections.Generic;
using System.Linq;
using DataAccess.Interface;
using DataAccess.EF;
using EntitiesObject.Entities.TanTamEntities;

namespace DataAccess.Dao.TanTam
{
    public interface IRolePermissionDao : IBaseFactories<DBNull>
    {
        List<Ins_RolePermission_GetList_Result> GetRolePermissions(int roleId);
        List<Ins_RolePermission_GetList_Result> GetRolePermissionsByType(int roleId, int type);
        void AddRolePermission(int roleId, int permissionId);
        void DeleteRolePermission(int roleId, int permissionId);
        void DeleteAllRolePermissions(int roleId);
    }

    internal class RolePermissionDao : DaoFactories<TanTamEntities, DBNull>, IRolePermissionDao
    {
        public List<Ins_RolePermission_GetList_Result> GetRolePermissions(int roleId)
        {
            using (Uow)
            {
                return Uow.Context.Ins_RolePermission_GetList(roleId).ToList();
            }
        }

        public List<Ins_RolePermission_GetList_Result> GetRolePermissionsByType(int roleId, int type)
        {
            using (Uow)
            {
                // Lấy tất cả quyền của role
                var allRolePermissions = Uow.Context.Ins_RolePermission_GetList(roleId).ToList();
                
                // Lấy tất cả permissions để filter theo type
                var allPermissions = Uow.Context.Ins_Permission_GetAll().ToList();
                
                // Filter theo type và trả về kết quả
                var permissionsByType = allPermissions.Where(p => p.Type == type).ToList();
                
                return allRolePermissions
                    .Where(rp => permissionsByType.Any(p => p.Id == rp.PermissionId))
                    .ToList();
            }
        }

        public void AddRolePermission(int roleId, int permissionId)
        {
            using (Uow)
            {
                Uow.Context.Ins_RolePermission_Add(roleId, permissionId);
            }
        }

        public void DeleteRolePermission(int roleId, int permissionId)
        {
            using (Uow)
            {
                Uow.Context.Ins_RolePermission_Delete(roleId, permissionId);
            }
        }

        public void DeleteAllRolePermissions(int roleId)
        {
            using (Uow)
            {
                Uow.Context.Ins_RolePermission_DeleteAll(roleId);
            }
        }
    }
} 