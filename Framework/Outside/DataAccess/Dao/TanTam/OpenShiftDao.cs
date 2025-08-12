using DataAccess.EF;
using DataAccess.Interface;
using DataAccess.Model.OpenShift;
using EntitiesObject.Entities.TanTamEntities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Core.Objects;
using System.Linq;

namespace DataAccess.Dao.TanTamDao
{
    /// <summary>
    /// Interface for OpenShift data access operations
    /// Uses optimized single stored procedure call for detail operations
    /// </summary>
    public interface IOpenShiftDao : IBaseFactories<DBNull>
    {
        void CreateOpenShift(string shiftId, int companyId, int totalEmployees, DateTime workingDay, 
            bool isDraft, int createdBy, out int openShiftId, out bool isReactivated);
        void AddBranchToOpenShift(int openShiftId, int branchId, int companyId);
        List<Ins_OpenShift_List_Result> GetList(int companyId, DateTime startDate, DateTime endDate);
        bool DeleteOpenShift(int openShiftId, int companyId, int deletedBy);
        int PublishOpenShiftSingle(int openShiftId, int companyId, int publishedBy);
        Ins_OpenShift_GetDetail_Result GetDetail(int openShiftId, int companyId);
        List<Ins_OpenShift_GetBranches_Result> GetBranches(int openShiftId, int companyId);
        List<Ins_OpenShift_GetPositions_Result> GetPositions(int openShiftId, int companyId);
        List<Ins_OpenShift_GetUsers_Result> GetUsers(int openShiftId, int companyId);
    }

    /// <summary>
    /// Implementation of OpenShift data access operations
    /// </summary>
    internal class OpenShiftDao : DaoFactories<TanTamEntities, DBNull>, IOpenShiftDao
    {
        public void CreateOpenShift(string shiftId, int companyId, int totalEmployees, DateTime workingDay, 
            bool isDraft, int createdBy, out int openShiftId, out bool isReactivated)
        {
            using (Uow)
            {
                openShiftId = 0;
                isReactivated = false;

                var out_openShiftId = new ObjectParameter("OpenShiftId", typeof(int));
                var out_isReactivated = new ObjectParameter("IsReactivated", typeof(bool));

                Uow.Context.Ins_OpenShift_Create_V2(shiftId, companyId, totalEmployees, workingDay,
                    isDraft, createdBy, out_openShiftId, out_isReactivated);

                if (out_openShiftId != null && out_openShiftId.Value != null)
                    int.TryParse(out_openShiftId.Value.ToString(), out openShiftId);

                if (out_isReactivated != null && out_isReactivated.Value != null)
                    bool.TryParse(out_isReactivated.Value.ToString(), out isReactivated);
            }
        }

        public void AddBranchToOpenShift(int openShiftId, int branchId, int companyId)
        {
            using (Uow)
            {
                Uow.Context.Ins_OpenShift_AddBranch(openShiftId, branchId, companyId);
            }
        }

        public List<Ins_OpenShift_List_Result> GetList(int companyId, DateTime startDate, DateTime endDate)
        {
            using (Uow)
            {
                return Uow.Context.Ins_OpenShift_List(companyId, startDate, endDate).ToList();
            }
        }

        public bool DeleteOpenShift(int openShiftId, int companyId, int deletedBy)
        {
            using (Uow)
            {
                var result = Uow.Context.Ins_OpenShift_Delete(openShiftId, companyId, deletedBy);
                return result != null && result.Any();
            }
        }

        public Ins_OpenShift_GetDetail_Result GetDetail(int openShiftId, int companyId)
        {
            using (Uow)
            {
                return Uow.Context.Ins_OpenShift_GetDetail(openShiftId, companyId).FirstOrDefault();
            }
        }

        public List<Ins_OpenShift_GetBranches_Result> GetBranches(int openShiftId, int companyId)
        {
            using (Uow)
            {
                return Uow.Context.Ins_OpenShift_GetBranches(openShiftId, companyId).ToList();
            }
        }

        public List<Ins_OpenShift_GetPositions_Result> GetPositions(int openShiftId, int companyId)
        {
            using (Uow)
            {
                return Uow.Context.Ins_OpenShift_GetPositions(openShiftId, companyId).ToList();
            }
        }

        public List<Ins_OpenShift_GetUsers_Result> GetUsers(int openShiftId, int companyId)
        {
            using (Uow)
            {
                return Uow.Context.Ins_OpenShift_GetUsers(openShiftId, companyId).ToList();
            }
        }

        public int PublishOpenShiftSingle(int openShiftId, int companyId, int publishedBy)
        {
            using (Uow)
            {
                return Uow.Context.Ins_OpenShift_PublishSingle(openShiftId, companyId, publishedBy);
            }
        }
    }
} 