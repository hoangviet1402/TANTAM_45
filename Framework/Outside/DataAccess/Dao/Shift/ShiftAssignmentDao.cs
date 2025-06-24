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
    }
}
