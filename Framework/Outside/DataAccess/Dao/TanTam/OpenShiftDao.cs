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
            bool isDraft, int createdBy, string branchIds, string positionIds, out int openShiftId, out bool isReactivated);
        
        List<Ins_OpenShift_List_Result> GetList(int companyId, DateTime startDate, DateTime endDate);
        
        List<Ins_OpenShift_ShiftListByWorkingDay_Result> GetShiftListByWorkingDay(int companyId, int page, int status, DateTime workingDay, int isAll);
        
        bool DeleteOpenShift(int openShiftId, int companyId, int deletedBy);
        
        OpenShiftCompleteDetailResult GetCompleteDetail(int openShiftId, int companyId);

        int PublishOpenShift(string openShiftIds, int companyId, int publishedBy);
    }

    /// <summary>
    /// Implementation of OpenShift data access operations
    /// </summary>
    internal class OpenShiftDao : DaoFactories<TanTamEntities, DBNull>, IOpenShiftDao
    {
        public void CreateOpenShift(string shiftId, int companyId, int totalEmployees, DateTime workingDay, 
            bool isDraft, int createdBy, string branchIds, string positionIds, out int openShiftId, out bool isReactivated)
        {
            using (Uow)
            {
                openShiftId = 0;
                isReactivated = false;

                var out_openShiftId = new ObjectParameter("OpenShiftId", typeof(int));
                var out_isReactivated = new ObjectParameter("IsReactivated", typeof(bool));

                Uow.Context.Ins_OpenShift_Create(shiftId, companyId, totalEmployees, workingDay,
                    isDraft, createdBy, branchIds, positionIds, out_openShiftId, out_isReactivated);

                if (out_openShiftId != null && out_openShiftId.Value != null)
                    int.TryParse(out_openShiftId.Value.ToString(), out openShiftId);

                if (out_isReactivated != null && out_isReactivated.Value != null)
                    bool.TryParse(out_isReactivated.Value.ToString(), out isReactivated);
            }
        }

        public List<Ins_OpenShift_List_Result> GetList(int companyId, DateTime startDate, DateTime endDate)
        {
            using (Uow)
            {
                return Uow.Context.Ins_OpenShift_List(companyId, startDate, endDate).ToList();
            }
        }

        public List<Ins_OpenShift_ShiftListByWorkingDay_Result> GetShiftListByWorkingDay(int companyId, int page, int status, DateTime workingDay, int isAll)
        {
            using (Uow)
            {
                return Uow.Context.Ins_OpenShift_ShiftListByWorkingDay(companyId, page, status, workingDay, isAll).ToList();
            }
        }

        public bool DeleteOpenShift(int openShiftId, int companyId, int deletedBy)
        {
            using (Uow)
            {
                // ✅ CORRECT DAO PATTERN - Let exceptions bubble up for proper SQL error handling
                var result = Uow.Context.Ins_OpenShift_Delete(openShiftId, companyId, deletedBy);
                return result != null && result.Any();
            }
        }

        public OpenShiftCompleteDetailResult GetCompleteDetail(int openShiftId, int companyId)
        {
            using (Uow)
            {
                var result = new OpenShiftCompleteDetailResult();
                
                using (var command = Uow.Context.Database.Connection.CreateCommand())
                {
                    command.CommandText = "Ins_OpenShift_GetCompleteDetail";
                    command.CommandType = CommandType.StoredProcedure;
                    
                    var param1 = command.CreateParameter();
                    param1.ParameterName = "@OpenShiftId";
                    param1.Value = openShiftId;
                    command.Parameters.Add(param1);
                    
                    var param2 = command.CreateParameter();
                    param2.ParameterName = "@CompanyId";
                    param2.Value = companyId;
                    command.Parameters.Add(param2);
                    
                    if (command.Connection.State != ConnectionState.Open)
                        command.Connection.Open();
                    
                    using (var reader = command.ExecuteReader())
                    {
                        // ✅ READ RESULT SET 1: Main Detail - Map directly to result object
                        if (reader.HasRows && reader.Read())
                        {
                            result.id = Convert.ToInt32(reader["id"]).ToString();
                            result.shift_name = reader["shift_name"]?.ToString();
                            result.total_employees = Convert.ToInt32(reader["total_employees"]);
                            result.shift_id = reader["shift_id"]?.ToString();
                            result.start_time = reader["start_time"]?.ToString();
                            result.end_time = reader["end_time"]?.ToString();
                            result.working_day = reader["working_day"]?.ToString();
                            result.timezone = reader["timezone"]?.ToString();
                            result.is_draft = Convert.ToBoolean(reader["is_draft"]);
                            
                            // ✅ Map status object
                            result.status = new OpenShiftStatusModel
                            {
                                not_available = Convert.ToInt32(reader["not_available"]),
                                status_color = new List<string> { "#838BA3", "#EBEBEB" }
                            };
                        }
                        
                        // ✅ READ RESULT SET 2: Branches - Map to branches list
                        reader.NextResult();
                        while (reader.Read())
                        {
                            result.branches.Add(new OpenShiftBranchResult
                            {
                                id = Convert.ToInt32(reader["id"]).ToString(),
                                name = reader["name"]?.ToString()
                            });
                        }
                        
                        // ✅ READ RESULT SET 3: Positions - Map to positions list
                        reader.NextResult();
                        while (reader.Read())
                        {
                            result.positions.Add(new OpenShiftPositionResult
                            {
                                id = Convert.ToInt32(reader["id"]).ToString(),
                                name = reader["name"]?.ToString()
                            });
                        }
                        
                        // ✅ READ RESULT SET 4: Users - Map to users list
                        reader.NextResult();
                        while (reader.Read())
                        {
                            result.users.Add(new OpenShiftUserResult
                            {
                                id = reader["id"]?.ToString(),
                                name = reader["name"]?.ToString(),
                                employee_code = reader["employee_code"]?.ToString(),
                                position = reader["position"]?.ToString(),
                                avatar = reader["avatar"]?.ToString(),
                                status = Convert.ToInt32(reader["status"]),
                                registered_at = reader["registered_at"]?.ToString()
                            });
                        }
                    }
                }
                
                return result;
            }
        }

        public int PublishOpenShift(string openShiftIds, int companyId, int publishedBy)
        {
            using (Uow)
            {
                // ✅ Use Entity Framework context method - CORRECT PATTERN
                return Uow.Context.Ins_OpenShift_Publish(openShiftIds, companyId, publishedBy);
            }
        }
    }
} 