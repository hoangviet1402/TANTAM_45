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
        int ShiftAssignment_User_Create(int shiftAssignmentID, int accountMapID);
        List<Ins_ShiftAssignment_User_WorkingDay_Log_GetByShiftAssignmentUserWorkingDay_Result> GetShiftAssignmentUserWorkingDayLogsByShiftAssignmentUserWorkingDay(int shiftAssignmentUserWorkingDayId);
        int GetCompanyIdByShiftAssignmentUserWorkingDayId(int shiftAssignmentUserWorkingDayId);
        decimal? CreateShiftAssignmentUserWorkingDayLog(
            int shiftAssignmentUserWorkingDayId,
            int actionType,
            int clockType,
            DateTime actionTime,
            string reason,
            int createdBy
        );
        Ins_ShiftAssignment_User_WorkingDay_Log_Trash_Result TrashShiftAssignmentUserWorkingDayLog(int logId, int trashedBy, string reason);
    }

    internal class ShiftAssignmentDao : DaoFactories<TanTamEntities, DBNull>, IShiftAssignmentDao
    {
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

        public int ShiftAssignment_User_Create(int shiftAssignmentID,int accountMapID)
        {
            using (Uow)
            {
                int shiftAssignment_UserId = 0;

                var out_shiftAssignment_UserId = new ObjectParameter("ShiftAssignment_UserId", typeof(int));

                var data = Uow.Context.Ins_ShiftAssignment_User_Create(
                    shiftAssignmentID,
                    accountMapID,
                    out_shiftAssignment_UserId);

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

        // Lấy CompanyId từ ShiftAssignmentUserWorkingDayId
        public int GetCompanyIdByShiftAssignmentUserWorkingDayId(int shiftAssignmentUserWorkingDayId)
        {
            using (Uow)
            {
                var sql = @"SELECT TOP 1 eam.CompanyID
                            FROM ShiftAssignment_User_WorkingDay suw
                            INNER JOIN ShiftAssignment_User sau ON suw.ShiftAssignmentUserId = sau.ID
                            INNER JOIN EmployeeAccountMap eam ON sau.AccountMapID = eam.Id
                            WHERE suw.Id = @p0";
                var result = Uow.Context.Database.SqlQuery<int?>(sql, shiftAssignmentUserWorkingDayId).FirstOrDefault();
                return result ?? 0;
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
    }
}
