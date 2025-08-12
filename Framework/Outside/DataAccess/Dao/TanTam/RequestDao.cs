using DataAccess.EF;
using DataAccess.Interface;
using EntitiesObject.Entities.TanTamEntities;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;

namespace DataAccess.Dao.TanTamDao
{
    public interface IRequestForDao : IBaseFactories<DBNull>
    {
        List<Ins_RequestTypes_GetAll_Result> RequestTypes_GetAll(int companyID);
        int Request_CreateRequestWithShift(int requestTypesID, int status, int accountMapID, string reason, DateTime fromDate, DateTime toDate, DateTime startDateTime, DateTime endDateTime, decimal totalDay, int approverBy, int branchId, bool isApprovable, bool isCancelable, int shiftAssignmentUserID, string listPayRollID);
    }

    internal class RequestForDao : DaoFactories<TanTamEntities, DBNull>, IRequestForDao
    {
        public List<Ins_RequestTypes_GetAll_Result> RequestTypes_GetAll(int companyID)
        {
            using (Uow)
            {
                var result = Uow.Context.Ins_RequestTypes_GetAll(companyID);
                return result.ToList();
            }
        }

        public int Request_CreateRequestWithShift(int requestTypesID, int status, int accountMapID, string reason, DateTime fromDate, DateTime toDate, DateTime startDateTime, DateTime endDateTime, decimal totalDay, int approverBy, int branchId, bool isApprovable, bool isCancelable, int shiftAssignmentUserID, string listPayRollID)
        {
            using (Uow)
            {
                var outResult = 0;
                var out_OutResult = new ObjectParameter("OutResult", typeof(int));
                var data = Uow.Context.Ins_Request_CreateRequestWithShift(requestTypesID, status, accountMapID,reason,fromDate, 
                    toDate, startDateTime, endDateTime, totalDay, approverBy, branchId, isApprovable,isCancelable, shiftAssignmentUserID,  listPayRollID, out_OutResult);

                if (out_OutResult != null && out_OutResult.Value != null)
                    int.TryParse(out_OutResult.Value.ToString(), out outResult);
                return outResult;
            }
        }

       
    }
} 