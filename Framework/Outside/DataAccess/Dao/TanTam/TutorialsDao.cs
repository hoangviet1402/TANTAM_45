using DataAccess.EF;
using DataAccess.Interface;
using EntitiesObject.Entities.TanTamEntities;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;

namespace DataAccess.Dao.TanTamDao
{
    public interface ITutorialsDao : IBaseFactories<DBNull>
    {
        List<Ins_UserTutorials_Initialize_Result> UserTutorials_Initialize(int accountMapID);
        List<Ins_UserTutorials_GetPendingTutorials_Result> UserTutorials_GetPendingTutorials(int accountMapID);
        List<Ins_UserTutorials_AllTutorials_Result> UserTutorials_GetAll(int accountMapID);
        int UserTutorials_Complete(int accountMapID, int tutorialId);
    }

    internal class TutorialsDao : DaoFactories<TanTamEntities, DBNull>, ITutorialsDao
    {
        public List<Ins_UserTutorials_Initialize_Result> UserTutorials_Initialize(int accountMapID)
        {
            using (Uow)
            {
                var result = Uow.Context.Ins_UserTutorials_Initialize(accountMapID);
                return result.ToList();
            }
        }

        public List<Ins_UserTutorials_AllTutorials_Result> UserTutorials_GetAll(int accountMapID)
        {
            using (Uow)
            {
                var result = Uow.Context.Ins_UserTutorials_AllTutorials(accountMapID);
                return result.ToList();
            }
        }

        public List<Ins_UserTutorials_GetPendingTutorials_Result> UserTutorials_GetPendingTutorials(int accountMapID)
        {
            using (Uow)
            {
                var result = Uow.Context.Ins_UserTutorials_GetPendingTutorials(accountMapID);
                return result.ToList();
            }
        }

        public int UserTutorials_Complete(int accountMapID, int tutorialId)
        {
            using (Uow)
            {
                var outResult = 0;
                var out_OutResult = new ObjectParameter("OutResult", typeof(int));
                var data = Uow.Context.Ins_UserTutorials_Complete(accountMapID, tutorialId, out_OutResult);

                if (out_OutResult != null && out_OutResult.Value != null)
                    int.TryParse(out_OutResult.Value.ToString(), out outResult);
                return outResult;
            }
        }
    }
} 