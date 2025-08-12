using System;
using System.Collections.Generic;
using System.Linq;
using DataAccess.Interface;
using DataAccess.EF;
using EntitiesObject.Entities.TanTamEntities;

namespace DataAccess.Dao.TanTam
{
    public interface IPermissionDao : IBaseFactories<DBNull>
    {
        List<Ins_PermissionGroup_GetAll_Result> GetAllPermissionGroups();
        List<Ins_Permission_GetAll_Result> GetAllPermissions();
        List<Ins_EmployeePermission_GetByEmployeeId_Result> GetEmployeePermissions(int employeeId);
        void InsertDefaultPermissionsForEmployee(int employeeId, int roleId);
        bool CheckEmployeePermission(int employeeId, string permissionKey);
        void DeleteEmployeePermissionsByType(int employeeId, int type);
        void InsertEmployeePermission(int employeeId, int permissionId);
        int GetEmployeeRole(int employeeId);
        int AddPermissionGroup(int? parentId, string title, string label, string url, string icon, string apiRouteName, bool isSystem, int sortIndex);
        int UpdatePermissionGroup(int id, int? parentId, string title, string label, string url, string icon, string apiRouteName, bool isSystem, int sortIndex);
        int DeletePermissionGroup(int id);
        int AddPermission(int groupId, string name, string key, string routeName, int sortIndex, int type);
        int UpdatePermission(int id, int groupId, string name, string key, string routeName, int sortIndex, int type);
        int DeletePermission(int id);
    }

    internal class PermissionDao : DaoFactories<TanTamEntities, DBNull>, IPermissionDao
    {
        public List<Ins_PermissionGroup_GetAll_Result> GetAllPermissionGroups()
        {
            using (Uow)
            {
                return Uow.Context.Ins_PermissionGroup_GetAll().ToList();
            }
        }

        public List<Ins_Permission_GetAll_Result> GetAllPermissions()
        {
            using (Uow)
            {
                return Uow.Context.Ins_Permission_GetAll().ToList();
            }
        }

        public List<Ins_EmployeePermission_GetByEmployeeId_Result> GetEmployeePermissions(int employeeId)
        {
            using (Uow)
            {
                return Uow.Context.Ins_EmployeePermission_GetByEmployeeId(employeeId).ToList();
            }
        }

        public void InsertDefaultPermissionsForEmployee(int employeeId, int roleId)
        {
            using (Uow)
            {
                Uow.Context.Ins_EmployeePermission_InsertDefault(employeeId, roleId);
            }
        }

        public bool CheckEmployeePermission(int employeeId, string permissionKey)
        {
            using (Uow)
            {
                var hasPermission = Uow.Context.Ins_EmployeePermission_Check(employeeId, permissionKey).FirstOrDefault();
                return hasPermission > 0;
            }
        }

        public void DeleteEmployeePermissionsByType(int employeeId, int type)
        {
            using (Uow)
            {
                Uow.Context.Ins_EmployeePermission_DeleteByType(employeeId, type);
            }
        }

        public void InsertEmployeePermission(int employeeId, int permissionId)
        {
            using (Uow)
            {
                Uow.Context.Ins_EmployeePermission_Insert(employeeId, permissionId);
            }
        }

        public int GetEmployeeRole(int employeeId)
        {
            using (Uow)
            {
                var result = Uow.Context.Ins_Employee_GetRole(employeeId);
                return result.FirstOrDefault() ?? 0;
            }
        }

        public int AddPermissionGroup(int? parentId, string title, string label, string url, string icon, string apiRouteName, bool isSystem, int sortIndex)
        {
            using (Uow)
            {
                var outId = new System.Data.Entity.Core.Objects.ObjectParameter("NewId", typeof(int));
                Uow.Context.Ins_PermissionGroup_Add(parentId, title, label, url, icon, apiRouteName, isSystem, sortIndex, outId);
                return (int)outId.Value;
            }
        }
        public int UpdatePermissionGroup(int id, int? parentId, string title, string label, string url, string icon, string apiRouteName, bool isSystem, int sortIndex)
        {
            using (Uow)
            {
                return Uow.Context.Ins_PermissionGroup_Update(id, parentId, title, label, url, icon, apiRouteName, isSystem, sortIndex);
            }
        }
        public int DeletePermissionGroup(int id)
        {
            using (Uow)
            {
                return Uow.Context.Ins_PermissionGroup_Delete(id);
            }
        }
        public int AddPermission(int groupId, string name, string key, string routeName, int sortIndex, int type)
        {
            using (Uow)
            {
                var outId = new System.Data.Entity.Core.Objects.ObjectParameter("NewId", typeof(int));
                Uow.Context.Ins_Permission_Add(groupId, name, key, routeName, sortIndex, type, outId);
                return (int)outId.Value;
            }
        }
        public int UpdatePermission(int id, int groupId, string name, string key, string routeName, int sortIndex, int type)
        {
            using (Uow)
            {
                return Uow.Context.Ins_Permission_Update(id, groupId, name, key, routeName, sortIndex, type);
            }
        }
        public int DeletePermission(int id)
        {
            using (Uow)
            {
                return Uow.Context.Ins_Permission_Delete(id);
            }
        }
    }
}