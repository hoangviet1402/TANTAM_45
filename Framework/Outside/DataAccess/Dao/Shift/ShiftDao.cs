using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess.EF;
using DataAccess.Interface;
using DataAccess.Model.Shift;
using EntitiesObject.Entities.TanTamEntities;

namespace DataAccess.Dao.Shift
{
    public interface IShiftDao : IBaseFactories<DBNull>
    {
        List<Ins_Time_GetList_Result> GetTimes(string lang);
        int ShiftCreateInfo(Ins_Shift_Create_Parameter parameter);
        List<Ins_Shift_Branch_Create_Result> Shift_Branch_Create(Ins_Shift_Branch_Create_Parameter parameter);
        List<Ins_Shift_CreateTimeInOutConfig_Result> ShiftCreateTimeInOutConfig(Ins_Shift_Create_Parameter parameter);
    }

    internal class ShiftDao : DaoFactories<TanTamEntities, DBNull>, IShiftDao
    {
        public  List<Ins_Time_GetList_Result> GetTimes(string lang)
        {
            using (Uow)
            {
                var data = Uow.Context.Ins_Time_GetList(lang);
                return data.ToList();
            }
        }

        public  int ShiftCreateInfo(Ins_Shift_Create_Parameter parameter)
        {
            using (Uow)
            {
                var outResult = 0;

                var out_OutResult = new ObjectParameter("ShiftId", typeof(int));

                var data = Uow.Context.Ins_Shift_Create(
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
                    parameter.GenerateTimekeepingType,
                    out_OutResult);

                if (out_OutResult != null && out_OutResult.Value != null)
                    int.TryParse(out_OutResult.Value.ToString(), out outResult);
                return outResult;
            }
        }

        public  List<Ins_Shift_CreateTimeInOutConfig_Result> ShiftCreateTimeInOutConfig(Ins_Shift_Create_Parameter parameter)
        {
            using (Uow)
            {
                
                var data = Uow.Context.Ins_Shift_CreateTimeInOutConfig(
                    parameter.ShiftId,
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
                    "vi");
                return data.ToList();
            }           
        }

        public  List<Ins_Shift_Branch_Create_Result> Shift_Branch_Create(Ins_Shift_Branch_Create_Parameter parameter)
        {
            using (Uow)
            {
                var out_OutResult = new ObjectParameter("AssignmentID", typeof(int));
                var data = Uow.Context.Ins_Shift_Branch_Create(
                    parameter.ShiftID,
                    parameter.CompanyID,
                    parameter.BranchID,
                    parameter.IsInsertOne);

                return data.ToList();
            }          
        }
    }
}
