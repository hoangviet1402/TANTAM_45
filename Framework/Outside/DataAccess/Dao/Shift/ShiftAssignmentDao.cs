using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Runtime.InteropServices;
using DataAccess.EF;
using DataAccess.Interface;
using DataAccess.Model.Shift;
using EntitiesObject.Entities.TanTamEntities;

namespace DataAccess.Dao.Shift
{
    public interface IShiftAssignmentDao : IBaseFactories<DBNull>
    {
        int ShiftAssignmentCreate(Ins_ShiftAssignment_Create_Parameter parameter);
        int ShiftAssignment_CreateAssignment(Ins_ShiftAssignment_CreateAssignment_Parameter parameter);
        List<Ins_ShiftAssignment_Branch_Create_Result> ShiftAssignment_CreateBranch(Ins_ShiftAssignment_Branch_Create_Parameter parameter, out int assignmentID);
        List<Ins_ShiftAssignment_Position_Create_Result> ShiftAssignment_CreatePosition(Ins_ShiftAssignment_Position_Create_Parameter parameter);
        List<Ins_ShiftAssignment_Department_Create_Result> ShiftAssignment_CreateDepartment(Ins_ShiftAssignment_Department_Create_Parameter parameter);
        int ShiftAssignment_User_Create(int shiftAssignmentID, int accountMapID, int type);
        List<Ins_ShiftAssignment_User_WorkingDay_Log_GetByShiftAssignmentUserWorkingDay_Result> GetShiftAssignmentUserWorkingDayLogsByShiftAssignmentUserWorkingDay(int shiftAssignmentUserWorkingDayId);
        decimal? CreateShiftAssignmentUserWorkingDayLog(int shiftAssignmentUserWorkingDayId, int actionType, int clockType, DateTime actionTime, string reason, int createdBy);
        bool ClearShiftAssignmentBranches(int shiftAssignmentId);
        bool ClearShiftAssignmentDepartments(int shiftAssignmentId);
        bool ClearShiftAssignmentPositions(int shiftAssignmentId);
        bool ClearAllAssignments(int shiftAssignmentId);
        Ins_ShiftAssignment_User_WorkingDay_Log_Trash_Result TrashShiftAssignmentUserWorkingDayLog(int logId, int trashedBy, string reason);
        List<Ins_ShiftAssignment_GetByBranchSimple_Result> ShiftAssignment_GetByBranchSimple(int branchId);
        List<Ins_ShiftAssignment_GetAllEmployerInShift_Result> ShiftAssignment_GetAllEmployerInShift(int shiftAssignmentId, int weekOfYear, DateTime dateFrom, DateTime dateTo, int companyId);
        List<Ins_EmployeesInfo_GetDetailForAddShift_Result> EmployeesInfo_GetDetailForAddShift(int companyId, int shiftID, int branchID, DateTime dateCheck);
        List<Ins_ShiftAssignment_User_WorkingDay_Log_FromDayToDay_Result> ShiftAssignment_User_WorkingDay_Log_FromDayToDay(int accountMapID, int companyId, DateTime dateFrom, DateTime dateTo);
        List<Ins_ShiftAssignment_User_GeSimpletByAccountId_Result> ShiftAssignment_User_GeSimpletByAccountId(int accountMapID, int type);

        List<Ins_ShiftAssignment_Department_GetByShiftID_Result>   ShiftAssignment_Department_GetByShiftID(int accountMapID, int shiftID);
        List<Ins_ShiftAssignment_Position_GetByShiftID_Result> ShiftAssignment_Position_GetByShiftID(int accountMapID, int type);
        List<Ins_ShiftAssignment_Branch_GetByShiftID_Result> ShiftAssignment_Branch_GetByShiftID(int accountMapID, int type);
        List<Ins_ShiftAssignment_User_WorkingDay_GetDateToDate_Result> LogChamCongHo_GetDateToDate(int accountMapID, DateTime dateFrom, DateTime dateTo);
    }

    internal class ShiftAssignmentDao : DaoFactories<TanTamEntities, DBNull>, IShiftAssignmentDao
    {
        public List<Ins_ShiftAssignment_User_WorkingDay_GetDateToDate_Result> LogChamCongHo_GetDateToDate(int accountMapID, DateTime dateFrom, DateTime dateTo)
        {
            using (Uow)
            {
                var data = Uow.Context.Ins_ShiftAssignment_User_WorkingDay_GetDateToDate(accountMapID, dateFrom, dateTo);
                return data.ToList();
            }
        }
        public List<Ins_ShiftAssignment_Department_GetByShiftID_Result> ShiftAssignment_Department_GetByShiftID(int accountMapID, int shiftID)
        {
            using (Uow)
            {
                var data = Uow.Context.Ins_ShiftAssignment_Department_GetByShiftID(accountMapID, shiftID);
                return data.ToList();
            }
        }

        public List<Ins_ShiftAssignment_Position_GetByShiftID_Result> ShiftAssignment_Position_GetByShiftID(int accountMapID, int shiftID)
        {
            using (Uow)
            {
                var data = Uow.Context.Ins_ShiftAssignment_Position_GetByShiftID(accountMapID, shiftID);
                return data.ToList();
            }
        }

        public List<Ins_ShiftAssignment_Branch_GetByShiftID_Result> ShiftAssignment_Branch_GetByShiftID(int accountMapID, int shiftID)
        {
            using (Uow)
            {
                var data = Uow.Context.Ins_ShiftAssignment_Branch_GetByShiftID(accountMapID, shiftID);
                return data.ToList();
            }
        }
        public List<Ins_ShiftAssignment_User_GeSimpletByAccountId_Result> ShiftAssignment_User_GeSimpletByAccountId(int accountMapID, int type)
        {
            using (Uow)
            {
                var data = Uow.Context.Ins_ShiftAssignment_User_GeSimpletByAccountId(accountMapID, type);
                return data.ToList();
            }
        }
        public List<Ins_ShiftAssignment_GetAllEmployerInShift_Result> ShiftAssignment_GetAllEmployerInShift(int shiftAssignmentId, int weekOfYear, DateTime dateFrom, DateTime dateTo, int companyId)
        {
            using (Uow)
            {
                var data = Uow.Context.Ins_ShiftAssignment_GetAllEmployerInShift(shiftAssignmentId, weekOfYear, dateFrom, dateTo, companyId);
                return data.ToList();
            }
        }
        public List<Ins_ShiftAssignment_GetByBranchSimple_Result> ShiftAssignment_GetByBranchSimple(int branchId)
        {
            using (Uow)
            {
                var data = Uow.Context.Ins_ShiftAssignment_GetByBranchSimple(branchId);
                return data.ToList();
            }
        }
        public int ShiftAssignmentCreate(Ins_ShiftAssignment_Create_Parameter parameter)
        {
            using (Uow)
            {
                var outResult = 0;

                var out_OutResult = new ObjectParameter("ShiftAssignmentId", typeof(int));

                var data = Uow.Context.Ins_ShiftAssignment_Create(
                    parameter.CompanyID,
                    parameter.ShiftID,
                    parameter.Title,
                    parameter.SortIndex,
                    parameter.AutoApprove,
                    parameter.PayrollConfigType,
                    parameter.AssignmentType,
                    parameter.Type,
                    parameter.GenerateTimekeepingType,
                    out_OutResult);

                if (out_OutResult != null && out_OutResult.Value != null)
                    int.TryParse(out_OutResult.Value.ToString(), out outResult);
                return outResult;
            }
        }

        public int ShiftAssignment_CreateAssignment(Ins_ShiftAssignment_CreateAssignment_Parameter parameter)
        {
            using (Uow)
            {
                var outResult = 0;

                var out_OutResult = new ObjectParameter("AssignmentID", typeof(int));

                var data = Uow.Context.Ins_ShiftAssignment_CreateAssignment(
                    parameter.ShiftAssignmentID,
                    parameter.ShiftID,
                    parameter.Label,
                    parameter.DateOfWeek,
                    out_OutResult);

                if (out_OutResult != null && out_OutResult.Value != null)
                    int.TryParse(out_OutResult.Value.ToString(), out outResult);
                return outResult;
            }
        }

        public List<Ins_ShiftAssignment_Branch_Create_Result> ShiftAssignment_CreateBranch(Ins_ShiftAssignment_Branch_Create_Parameter parameter, out int assignmentID)
        {
            using (Uow)
            {
                assignmentID = 0;
                var out_OutResult = new ObjectParameter("AssignmentID", typeof(int));
                var data = Uow.Context.Ins_ShiftAssignment_Branch_Create(
                    parameter.ShiftAssignmentID,
                    parameter.CompanyID,
                    parameter.BranchID,
                    parameter.IsInsertOne);
                return data.ToList();
            }
        }

        public List<Ins_ShiftAssignment_Position_Create_Result> ShiftAssignment_CreatePosition(Ins_ShiftAssignment_Position_Create_Parameter parameter)
        {
            using (Uow)
            {
                var data = Uow.Context.Ins_ShiftAssignment_Position_Create(
                    parameter.ShiftAssignmentID,
                    parameter.CompanyID,
                    parameter.PositionID,
                    parameter.IsInsertOne);

                return data.ToList();
            }
        }

        public List<Ins_ShiftAssignment_Department_Create_Result> ShiftAssignment_CreateDepartment(Ins_ShiftAssignment_Department_Create_Parameter parameter)
        {
            using (Uow)
            {
                var data = Uow.Context.Ins_ShiftAssignment_Department_Create(
                    parameter.ShiftAssignmentID,
                    parameter.CompanyID,
                    parameter.DepartmentID,
                    parameter.IsInsertOne);

                return data.ToList();
            }
        }

        public int ShiftAssignment_User_Create(int shiftAssignmentID, int accountMapID, int type)
        {
            using (Uow)
            {
                int shiftAssignment_UserId = 0;

                var out_shiftAssignment_UserId = new ObjectParameter("ShiftAssignment_UserId", typeof(int));

                var data = Uow.Context.Ins_ShiftAssignment_User_Create(
                    shiftAssignmentID,
                    accountMapID,
                    out_shiftAssignment_UserId,
                    type);

                if (out_shiftAssignment_UserId != null && out_shiftAssignment_UserId.Value != null)
                    int.TryParse(out_shiftAssignment_UserId.Value.ToString(), out shiftAssignment_UserId);
                return shiftAssignment_UserId;
            }
        }

        public List<Ins_ShiftAssignment_User_WorkingDay_Log_GetByShiftAssignmentUserWorkingDay_Result> GetShiftAssignmentUserWorkingDayLogsByShiftAssignmentUserWorkingDay(int shiftAssignmentUserWorkingDayId)
        {
            using (Uow)
            {
                return Uow.Context.Ins_ShiftAssignment_User_WorkingDay_Log_GetByShiftAssignmentUserWorkingDay(shiftAssignmentUserWorkingDayId).ToList();
            }
        }

        public decimal? CreateShiftAssignmentUserWorkingDayLog(
            int shiftAssignmentUserWorkingDayId,
            int actionType,
            int clockType,
            DateTime actionTime,
            string reason,
            int createdBy
        )
        {
            using (Uow)
            {
                var result = Uow.Context.Ins_ShiftAssignment_User_WorkingDay_Log_Create(
                    shiftAssignmentUserWorkingDayId,
                    actionType,
                    clockType,
                    actionTime,
                    reason,
                    createdBy
                );
                return result?.FirstOrDefault();
            }
        }

        public Ins_ShiftAssignment_User_WorkingDay_Log_Trash_Result TrashShiftAssignmentUserWorkingDayLog(int logId, int trashedBy, string reason)
        {
            using (Uow)
            {
                var result = Uow.Context.Ins_ShiftAssignment_User_WorkingDay_Log_Trash(logId, trashedBy, reason);
                return result?.FirstOrDefault();
            }
        }

        public List<Ins_EmployeesInfo_GetDetailForAddShift_Result> EmployeesInfo_GetDetailForAddShift(int companyId, int shiftID, int branchID, DateTime dateCheck)
        {
            using (Uow)
            {
                var result = Uow.Context.Ins_EmployeesInfo_GetDetailForAddShift(companyId, shiftID, branchID, dateCheck);
                return result.ToList();
            }
        }

        public bool ClearShiftAssignmentBranches(int shiftAssignmentId)
        {
            using (Uow)
            {
                var result = Uow.Context.Ins_ShiftAssignment_ClearBranches(shiftAssignmentId);
                var resultValue = result.FirstOrDefault();
                return resultValue.HasValue && resultValue.Value > 0;
            }
        }

        public bool ClearShiftAssignmentDepartments(int shiftAssignmentId)
        {
            using (Uow)
            {
                var result = Uow.Context.Ins_ShiftAssignment_ClearDepartments(shiftAssignmentId);
                var resultValue = result.FirstOrDefault();
                return resultValue.HasValue && resultValue.Value > 0;
            }
        }

        public bool ClearShiftAssignmentPositions(int shiftAssignmentId)
        {
            using (Uow)
            {
                var result = Uow.Context.Ins_ShiftAssignment_ClearPositions(shiftAssignmentId);
                var resultValue = result.FirstOrDefault();
                return resultValue.HasValue && resultValue.Value > 0;
            }
        }

        public bool ClearAllAssignments(int shiftAssignmentId)
        {
            using (Uow)
            {
                var result = Uow.Context.Ins_Assignment_ClearAll(shiftAssignmentId);
                var resultValue = result.FirstOrDefault();
                return resultValue.HasValue && resultValue.Value > 0;
            }
        }
        public List<Ins_ShiftAssignment_User_WorkingDay_Log_FromDayToDay_Result> ShiftAssignment_User_WorkingDay_Log_FromDayToDay(int accountMapID, int companyId, DateTime dateFrom, DateTime dateTo)
        {
            using (Uow)
            {
                var result = Uow.Context.Ins_ShiftAssignment_User_WorkingDay_Log_FromDayToDay(accountMapID, companyId, dateFrom, dateTo);
                return result.ToList();
            }
        }
    }
}
