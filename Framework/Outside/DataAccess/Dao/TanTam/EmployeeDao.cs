using DataAccess.EF;
using DataAccess.Interface;
using EntitiesObject.Entities.TanTamEntities;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;

namespace DataAccess.Dao.TanTamDao
{
    /// <summary>
    /// Interface for Employee data access operations
    /// </summary>
    public interface IEmployeeDao : IBaseFactories<DBNull>
    {
        // Employee management
        void CreateEmployee(string fullName, string employeesCode, string phone, string phoneCode, 
            string email, string password, int companyId, int branchId, int role, string deviceId, 
            out int employeeAccountId, out int isNewUser, out int needSetPassword, out int needSetCompany);
        
        Ins_Employee_GetEmployeeID_Result GetEmployeeDetail(int employeeId);
        
        List<Ins_Employee_GetAll_Result> GetEmployeeList(int companyId, int page, int pageSize, 
            string fullName = null, bool? isActive = null);
        
        int DeleteEmployee(int employeeAccountId);
        
        int DeleteMultiEmployee(string employeeAccountIds);
        
        int ResetEmployeePassword(int employeeAccountMapId, string passwordHash);
        
        int UpdateEmployeeDetails(int employeeId, string fullName, DateTime? birthDate, string gender, 
            string employeeCode, int? displayOrder, string email, string phone, string phoneCode);
        
        List<Ins_Employee_GetList_Result> GetEmployeeFilterList(int companyId, int page, int pageSize, 
            DateTime startDate, DateTime endDate, bool isNoNeedTimekeeping, int totalRecords);
        
        string GetNextEmployeeCode(int companyId);
    }

    /// <summary>
    /// Implementation of Employee data access operations
    /// </summary>
    internal class EmployeeDao : DaoFactories<TanTamEntities, DBNull>, IEmployeeDao
    {
        public void CreateEmployee(string fullName, string employeesCode, string phone, string phoneCode, 
            string email, string password, int companyId, int branchId, int role, string deviceId, 
            out int employeeAccountId, out int isNewUser, out int needSetPassword, out int needSetCompany)
        {
            using (Uow)
            {
                employeeAccountId = 0;
                isNewUser = 0;
                needSetPassword = 0;
                needSetCompany = 0;
                
                var out_employeeAccountId = new ObjectParameter("EmployeeAccountId", typeof(int));
                var out_isNewUser = new ObjectParameter("IsNewUser", typeof(int));
                var out_needSetPassword = new ObjectParameter("NeedSetPassword", typeof(int));
                var out_needSetCompany = new ObjectParameter("NeedSetCompany", typeof(int));
                
                Uow.Context.Ins_Employee_Create(fullName, employeesCode, phone, phoneCode, email, 
                    password, companyId, branchId, role, deviceId, out_employeeAccountId, out_isNewUser, 
                    out_needSetPassword, out_needSetCompany);

                if (out_employeeAccountId != null && out_employeeAccountId.Value != null)
                    int.TryParse(out_employeeAccountId.Value.ToString(), out employeeAccountId);

                if (out_isNewUser != null && out_isNewUser.Value != null)
                    int.TryParse(out_isNewUser.Value.ToString(), out isNewUser);

                if (out_needSetPassword != null && out_needSetPassword.Value != null)
                    int.TryParse(out_needSetPassword.Value.ToString(), out needSetPassword);

                if (out_needSetCompany != null && out_needSetCompany.Value != null)
                    int.TryParse(out_needSetCompany.Value.ToString(), out needSetCompany);
            }
        }

        public Ins_Employee_GetEmployeeID_Result GetEmployeeDetail(int employeeId)
        {
            using (Uow)
            {
                var result = Uow.Context.Ins_Employee_GetEmployeeID(employeeId);
                return result.FirstOrDefault();
            }
        }

        public List<Ins_Employee_GetAll_Result> GetEmployeeList(int companyId, int page, int pageSize, 
            string fullName = null, bool? isActive = null)
        {
            using (Uow)
            {
                var result = Uow.Context.Ins_Employee_GetAll(companyId, page, pageSize, fullName, isActive);
                return result.ToList();
            }
        }

        public int DeleteEmployee(int employeeAccountId)
        {
            using (Uow)
            {
                return Uow.Context.Ins_Employee_Delete(employeeAccountId);
            }
        }

        public int DeleteMultiEmployee(string employeeAccountIds)
        {
            using (Uow)
            {
                return Uow.Context.Ins_Employee_MultiDelete(employeeAccountIds);
            }
        }

        public int ResetEmployeePassword(int employeeAccountMapId, string passwordHash)
        {
            using (Uow)
            {
                return Uow.Context.Ins_Employee_ResetPass(employeeAccountMapId, passwordHash);
            }
        }

        public int UpdateEmployeeDetails(int employeeId, string fullName, DateTime? birthDate, string gender, 
            string employeeCode, int? displayOrder, string email, string phone, string phoneCode)
        {
            using (Uow)
            {
                return Uow.Context.Ins_Employee_UpdateDetails(employeeId, fullName, birthDate, gender, 
                    employeeCode, displayOrder, email, phone, phoneCode);
            }
        }

        public List<Ins_Employee_GetList_Result> GetEmployeeFilterList(int companyId, int page, int pageSize, 
            DateTime startDate, DateTime endDate, bool isNoNeedTimekeeping, int totalRecords)
        {
            using (Uow)
            {
                var out_totalRecords = new ObjectParameter("TotalRecords", typeof(int));
                var result = Uow.Context.Ins_Employee_GetList(companyId, page, pageSize, startDate, endDate, 
                    isNoNeedTimekeeping, out_totalRecords);
                return result.ToList();
            }
        }

        public string GetNextEmployeeCode(int companyId)
        {
            using (Uow)
            {
                var result = Uow.Context.Ins_Employee_GetLastEmployeeCode(companyId);
                return result.FirstOrDefault();
            }
        }
    }
} 