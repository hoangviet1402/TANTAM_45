using DataAccess.Entities;
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
    /// Interface for Shift data access operations
    /// </summary>
    public interface IShiftDao : IBaseFactories<DBNull>
    {
        List<Ins_Time_GetList> GetTimes(string lang);
        int ShiftCreateInfo(Ins_Shift_Create_Parameter parameter);
        List<Ins_Shift_Branch_Create_Result> Shift_Branch_Create(Ins_Shift_Branch_Create_Parameter parameter);
        List<Ins_Shift_CreateTimeInOutConfig_Result> ShiftCreateTimeInOutConfig(Ins_Shift_Create_Parameter parameter, int shiftID);
        int ShiftAssignmentCreate(Ins_ShiftAssignment_Create_Parameter parameter);
        List<Ins_ShiftAssignment_Branch_Create_Result> ShiftAssignment_CreateBranch(Ins_ShiftAssignment_Branch_Create_Parameter parameter);
        List<Ins_ShiftAssignment_Position_Create_Result> ShiftAssignment_CreatePosition(Ins_ShiftAssignment_Position_Create_Parameter parameter);
        List<Ins_ShiftAssignment_Department_Create_Result> ShiftAssignment_CreateDepartment(Ins_ShiftAssignment_Department_Create_Parameter parameter);
        int ShiftAssignment_CreateAssignment(Ins_ShiftAssignment_CreateAssignment_Parameter parameter);
    }

    /// <summary>
    /// Implementation of Shift data access operations
    /// </summary>
    internal class ShiftDao : DaoFactories<TanTamEntities, DBNull>, IShiftDao
    {
        public List<Ins_Time_GetList> GetTimes(string lang)
        {
            using (Uow)
            {
                var result = Uow.Context.Ins_Time_GetList(lang);
                return result.Select(x => new Ins_Time_GetList
                {
                    ID = x.ID,
                    Name = x.Name,
                    Type = x.Type,
                    Value = x.Value,
                    IsHour = x.IsHour
                }).ToList();
            }
        }

        public int ShiftCreateInfo(Ins_Shift_Create_Parameter parameter)
        {
            using (Uow)
            {
                var out_shiftId = new ObjectParameter("ShiftId", typeof(int));
                
                var result = Uow.Context.Ins_Shift_Create(
                    parameter.CompanyID,
                    parameter.Name,
                    parameter.NameNosign,
                    parameter.ShiftKey,
                    parameter.Coefficient,
                    parameter.MinimumWorkingHour,
                    parameter.Note,
                    parameter.EarlyCheckOut,
                    parameter.LatelyCheckIn,
                    parameter.MaxLateCheckInOutMinute,
                    parameter.MinSoonCheckInOutMinute,
                    parameter.Status,
                    parameter.Type,
                    parameter.SortIndex,
                    parameter.IsOvertimeShift,
                    parameter.MealCoefficient,
                    parameter.Timezone,
                    out_shiftId
                );

                // The stored procedure returns the value directly
                return result;
            }
        }

        public List<Ins_Shift_CreateTimeInOutConfig_Result> ShiftCreateTimeInOutConfig(Ins_Shift_Create_Parameter parameter, int shiftID)
        {
            using (Uow)
            {
                var result = Uow.Context.Ins_Shift_CreateTimeInOutConfig(
                    shiftID,
                    parameter.StartHourId,
                    parameter.StartMinuteId,
                    parameter.EndHourId,
                    parameter.EndMinuteId,
                    parameter.StartCheckInMinuteId,
                    parameter.EndCheckInMinuteId,
                    parameter.StartCheckOutMinuteId,
                    parameter.EndCheckOutMinuteId,
                    parameter.StartCheckInHourId,
                    parameter.EndCheckInHourId,
                    parameter.StartCheckOutHourId,
                    parameter.EndCheckOutHourId,
                    parameter.MaxLateCheckInOutMinute,
                    parameter.MinSoonCheckInOutMinute,
                    "vi"
                );
                
                return result.ToList();
            }
        }

        public List<Ins_Shift_Branch_Create_Result> Shift_Branch_Create(Ins_Shift_Branch_Create_Parameter parameter)
        {
            using (Uow)
            {
                var result = Uow.Context.Ins_Shift_Branch_Create(
                    parameter.ShiftID,
                    parameter.CompanyID,
                    parameter.BranchID,
                    parameter.IsInsertOne
                );
                
                return result.ToList();
            }
        }

        public int ShiftAssignmentCreate(Ins_ShiftAssignment_Create_Parameter parameter)
        {
            using (Uow)
            {
                var out_shiftAssignmentId = new ObjectParameter("ShiftAssignmentId", typeof(int));
                
                var result = Uow.Context.Ins_ShiftAssignment_Create(
                    parameter.CompanyID,
                    parameter.ShiftID,
                    parameter.Title,
                    parameter.SortIndex,
                    parameter.AutoApprove,
                    parameter.PayrollConfigType,
                    parameter.AssignmentTypeObj,
                    parameter.GenerateTimekeepingTypeObj,
                    parameter.Type,
                    out_shiftAssignmentId
                );

                // The stored procedure returns the value directly
                return result;
            }
        }

        public List<Ins_ShiftAssignment_Branch_Create_Result> ShiftAssignment_CreateBranch(Ins_ShiftAssignment_Branch_Create_Parameter parameter)
        {
            using (Uow)
            {
                var result = Uow.Context.Ins_ShiftAssignment_Branch_Create(
                    parameter.ShiftAssignmentID,
                    parameter.CompanyID,
                    parameter.BranchID,
                    parameter.IsInsertOne
                );
                
                return result.ToList();
            }
        }

        public List<Ins_ShiftAssignment_Position_Create_Result> ShiftAssignment_CreatePosition(Ins_ShiftAssignment_Position_Create_Parameter parameter)
        {
            using (Uow)
            {
                var result = Uow.Context.Ins_ShiftAssignment_Position_Create(
                    parameter.ShiftAssignmentID,
                    parameter.CompanyID,
                    parameter.PositionID,
                    parameter.IsInsertOne
                );
                
                return result.ToList();
            }
        }

        public List<Ins_ShiftAssignment_Department_Create_Result> ShiftAssignment_CreateDepartment(Ins_ShiftAssignment_Department_Create_Parameter parameter)
        {
            using (Uow)
            {
                var result = Uow.Context.Ins_ShiftAssignment_Department_Create(
                    parameter.ShiftAssignmentID,
                    parameter.CompanyID,
                    parameter.DepartmentID,
                    parameter.IsInsertOne
                );
                
                return result.ToList();
            }
        }

        public int ShiftAssignment_CreateAssignment(Ins_ShiftAssignment_CreateAssignment_Parameter parameter)
        {
            using (Uow)
            {
                var out_assignmentId = new ObjectParameter("AssignmentId", typeof(int));
                
                var result = Uow.Context.Ins_ShiftAssignment_CreateAssignment(
                    parameter.ShiftAssignmentID,
                    parameter.ShiftID,
                    parameter.Label,
                    parameter.DateOfWeek,
                    out_assignmentId
                );

                // The stored procedure returns the value directly
                return result;
            }
        }
    }
} 