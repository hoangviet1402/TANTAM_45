using DataAccess.EF;
using DataAccess.Interface;
using EntitiesObject.Entities.TanTamEntities;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace DataAccess.Dao.TanTamDao
{
    /// <summary>
    /// Interface for Task data access operations
    /// </summary>
    public interface ITaskDao : IBaseFactories<DBNull>
    {
        // Task management
        
        Ins_Task_List_Result GetTaskDetail(int taskId, int companyId);
        
        List<Ins_Task_List_Result> GetTaskList(int? taskId, int companyId);
        
        Ins_Tasks_Create_Result CreateTask(string title, int createdUserObj, int companyId, string defaultView, string color, 
            string departmentIds, string positionIds, string branchIds, string userIds);
        
        List<Ins_Task_GetTaskGroupsByTaskId_Result> GetTaskGroupsByTaskId(int taskId);
        
        List<Ins_Task_ManagersByTask_Result> GetTaskManagersByTask(int taskId);
        
        List<Ins_Task_UsersByTask_Result> GetTaskUsersByTask(int taskId);
        
        List<Ins_Task_UsersByUser_Result> GetTaskUsersByUser(int userId);
        
        Ins_Task_CreatorInfo_Result GetTaskCreatorInfo(int taskId);
        
        Ins_Task_Group_Create_Result CreateTaskGroup(int bundleId, string name, string color, string position);
        
        List<Ins_Task_Group_Delete_Result> DeleteTaskGroup(int groupId);
        
        Ins_Task_Group_Update_Name_Result UpdateTaskGroupName(int groupId, string name);
        
        Ins_Task_Group_Update_Color_Result UpdateTaskGroupColor(int groupId, string color);
        
        Ins_Task_Sub_Create_Result CreateTaskSub(string title, string alias, int? bundleId, int? createdUserId, int? assignedId, string position);
        
        List<Ins_Task_Sub_ListByBundle_Result> GetTaskSubListByBundle(string bundleId);
        
        Ins_Task_Sub_Update_Completed_Result UpdateTaskSubCompleted(int id, bool isCompleted);
        
        Ins_Task_Sub_Update_Deadline_Result UpdateTaskSubDeadline(int id, DateTime? deadline, DateTime? startDate);
                
        int AddTaskCollaborators(string taskId, string userIds);
       
        List<Ins_Task_Update_AssignedUser_Result> UpdateTaskAssignedUser(int taskId, int? assignedUser);
        
        int GetCustomizedFieldsAndValuesByTask(int taskId);
        
        List<Ins_Task_Get_Sub_Field_Value_ByTitle_Result> GetSubFieldValueByTitle(int subtaskId, string title = null);
        
        Ins_Task_Field_Create_Result CreateTaskField(int taskId, string title, string type, string description, bool? addToLibrary, bool? notifyOnChange);
        
        void InsertTaskFieldOptionsBulk(int fieldId, string options);
        
        Ins_Task_Sub_Update_Title_Result UpdateSubTaskTitle(int id, string title, string titleNosign, string alias);
        
        List<Ins_Tasks_Delete_Result> DeleteTask(int taskId);

        int DeleteAllRelations(int taskId);

        int DeleteAllFields(int taskId);

        int DeleteAllSubTasksByTaskId(int taskId);

        int DeleteAllGroupsByTaskId(int taskId);

        List<Ins_Tasks_Delete_Main_Result> DeleteMainTask(int taskId, int companyId);

        Ins_TaskBranches_Create_Result CreateTaskBranch(int taskId, int branchId);
        Ins_TaskDepartments_Create_Result CreateTaskDepartment(int taskId, int departmentId);
        Ins_TaskPositions_Create_Result CreateTaskPosition(int taskId, int positionId);
        Ins_TaskTaskUsers_Create_Result CreateTaskUser(int taskId, int userId);
    }

    /// <summary>
    /// Implementation of Task data access operations
    /// </summary>
    internal class TaskDao : DaoFactories<TanTamEntities, DBNull>, ITaskDao
    {


        public Ins_Task_List_Result GetTaskDetail(int taskId, int companyId)
        {
            using (Uow)
            {
                var result = Uow.Context.Ins_Task_List(taskId, companyId);
                return result.FirstOrDefault();
            }
        }

        public List<Ins_Task_List_Result> GetTaskList(int? taskId, int companyId)
        {
            using (Uow)
            {
                var result = Uow.Context.Ins_Task_List(taskId, companyId);
                return result.ToList();
            }
        }

        public Ins_Tasks_Create_Result CreateTask(string title, int createdUserObj, int companyId, string defaultView, string color, 
            string departmentIds, string positionIds, string branchIds, string userIds)
        {
            using (Uow)
            {
                var result = Uow.Context.Ins_Tasks_Create(title, createdUserObj, companyId, defaultView, color);
                return result.FirstOrDefault();
            }
        }

        public List<Ins_Task_GetTaskGroupsByTaskId_Result> GetTaskGroupsByTaskId(int taskId)
        {
            using (Uow)
            {
                var result = Uow.Context.Ins_Task_GetTaskGroupsByTaskId(taskId);
                return result.ToList();
            }
        }

        public List<Ins_Task_ManagersByTask_Result> GetTaskManagersByTask(int taskId)
        {
            using (Uow)
            {
                var result = Uow.Context.Ins_Task_ManagersByTask(taskId);
                return result.ToList();
            }
        }

        public List<Ins_Task_UsersByTask_Result> GetTaskUsersByTask(int taskId)
        {
            using (Uow)
            {
                var result = Uow.Context.Ins_Task_UsersByTask(taskId);
                return result.ToList();
            }
        }

        public List<Ins_Task_UsersByUser_Result> GetTaskUsersByUser(int userId)
        {
            using (Uow)
            {
                var result = Uow.Context.Ins_Task_UsersByUser(userId);
                return result.ToList();
            }
        }

        public Ins_Task_CreatorInfo_Result GetTaskCreatorInfo(int taskId)
        {
            using (Uow)
            {
                var result = Uow.Context.Ins_Task_CreatorInfo(taskId);
                return result.FirstOrDefault();
            }
        }

        public Ins_Task_Group_Create_Result CreateTaskGroup(int bundleId, string name, string color, string position)
        {
            using (Uow)
            {
                var result = Uow.Context.Ins_Task_Group_Create(bundleId, name, color, position);
                return result.FirstOrDefault();
            }
        }

        public List<Ins_Task_Group_Delete_Result> DeleteTaskGroup(int groupId)
        {
            using (Uow)
            {
                var result = Uow.Context.Ins_Task_Group_Delete(groupId);
                return result.ToList();
            }
        }

        public Ins_Task_Group_Update_Name_Result UpdateTaskGroupName(int groupId, string name)
        {
            using (Uow)
            {
                var result = Uow.Context.Ins_Task_Group_Update_Name(groupId, name);
                return result.FirstOrDefault();
            }
        }

        public Ins_Task_Group_Update_Color_Result UpdateTaskGroupColor(int groupId, string color)
        {
            using (Uow)
            {
                var result = Uow.Context.Ins_Task_Group_Update_Color(groupId, color);
                return result.FirstOrDefault();
            }
        }

        public Ins_Task_Sub_Create_Result CreateTaskSub(string title, string alias, int? bundleId, int? createdUserId, int? assignedId, string position)
        {
            using (Uow)
            {
                var result = Uow.Context.Ins_Task_Sub_Create(title, alias, bundleId, createdUserId, assignedId, position);
                return result.FirstOrDefault();
            }
        }

        public List<Ins_Task_Sub_ListByBundle_Result> GetTaskSubListByBundle(string bundleId)
        {
            using (Uow)
            {
                var result = Uow.Context.Ins_Task_Sub_ListByBundle(bundleId);
                return result.ToList();
            }
        }

        public Ins_Task_Sub_Update_Completed_Result UpdateTaskSubCompleted(int id, bool isCompleted)
        {
            using (Uow)
            {
                var result = Uow.Context.Ins_Task_Sub_Update_Completed(id, isCompleted);
                return result.FirstOrDefault();
            }
        }

        public Ins_Task_Sub_Update_Deadline_Result UpdateTaskSubDeadline(int id, DateTime? deadline, DateTime? startDate)
        {
            using (Uow)
            {
                var result = Uow.Context.Ins_Task_Sub_Update_Deadline(id, deadline, startDate);
                return result.FirstOrDefault();
            }
        }

        public int AddTaskCollaborators(string taskId, string userIds)
        {
            using (Uow)
            {
                return Uow.Context.Ins_Task_Add_Collaborators(taskId, userIds);
            }
        }

        public List<Ins_Task_Update_AssignedUser_Result> UpdateTaskAssignedUser(int taskId, int? assignedUser)
        {
            using (Uow)
            {
                var result = Uow.Context.Ins_Task_Update_AssignedUser(taskId, assignedUser);
                return result.ToList();
            }
        }
        
        public int GetCustomizedFieldsAndValuesByTask(int taskId)
        {
            using (Uow)
            {
                return Uow.Context.Ins_Task_Get_CustomizedFields_And_Values_ByTask(taskId);
            }
        }

        public List<Ins_Task_Get_Sub_Field_Value_ByTitle_Result> GetSubFieldValueByTitle(int subtaskId, string title = null)
        {
            using (Uow)
            {
                var result = Uow.Context.Ins_Task_Get_Sub_Field_Value_ByTitle(subtaskId, title);
                return result.ToList();
            }
        }

        public Ins_Task_Field_Create_Result CreateTaskField(int taskId, string title, string type, string description, bool? addToLibrary, bool? notifyOnChange)
        {
            using (Uow)
            {
                var result = Uow.Context.Ins_Task_Field_Create(taskId, title, type, description, addToLibrary, notifyOnChange);
                return result.FirstOrDefault();
            }
        }

        public void InsertTaskFieldOptionsBulk(int fieldId, string options)
        {
            using (Uow)
            {
                Uow.Context.Database.ExecuteSqlCommand(
                    "EXEC Ins_Task_Field_Options_Bulk @field_id, @options",
                    new System.Data.SqlClient.SqlParameter("@field_id", fieldId),
                    new System.Data.SqlClient.SqlParameter("@options", options ?? (object)DBNull.Value)
                );
            }
        }

        public Ins_Task_Sub_Update_Title_Result UpdateSubTaskTitle(int id, string title, string titleNosign, string alias)
        {
            using (Uow)
            {
                var result = Uow.Context.Ins_Task_Sub_Update_Title(id, title, titleNosign, alias);
                return result.FirstOrDefault();
            }
        }

        public List<Ins_Tasks_Delete_Result> DeleteTask(int taskId)
        {
            using (Uow)
            {
                var result = Uow.Context.Ins_Tasks_Delete(taskId);
                return result.ToList();
            }
        }

        public int DeleteAllRelations(int taskId)
        {
            using (Uow)
            {
                return Uow.Context.Ins_Tasks_Delete_All_Relations(taskId);
            }
        }

        public int DeleteAllFields(int taskId)
        {
            using (Uow)
            {
                return Uow.Context.Ins_Tasks_Delete_all_Fields(taskId);
            }
        }

        public int DeleteAllSubTasksByTaskId(int taskId)
        {
            using (Uow)
            {
                return Uow.Context.Ins_Tasks_Delete_All_SubTasks_ByTaskId(taskId);
            }
        }

        public int DeleteAllGroupsByTaskId(int taskId)
        {
            using (Uow)
            {
                return Uow.Context.Ins_Tasks_Delete_All_Groups_ByTaskId(taskId);
            }
        }

        public List<Ins_Tasks_Delete_Main_Result> DeleteMainTask(int taskId, int companyId)
        {
            using (Uow)
            {
                var result = Uow.Context.Ins_Tasks_Delete_Main(taskId, companyId);
                return result.ToList();
            }
        }

        public Ins_TaskBranches_Create_Result CreateTaskBranch(int taskId, int branchId)
        {
            using (Uow)
            {
                var result = Uow.Context.Ins_TaskBranches_Create(taskId, branchId);
                return result.FirstOrDefault();
            }
        }

        public Ins_TaskDepartments_Create_Result CreateTaskDepartment(int taskId, int departmentId)
        {
            using (Uow)
            {
                var result = Uow.Context.Ins_TaskDepartments_Create(taskId, departmentId);
                return result.FirstOrDefault();
            }
        }

        public Ins_TaskPositions_Create_Result CreateTaskPosition(int taskId, int positionId)
        {
            using (Uow)
            {
                var result = Uow.Context.Ins_TaskPositions_Create(taskId, positionId);
                return result.FirstOrDefault();
            }
        }

        public Ins_TaskTaskUsers_Create_Result CreateTaskUser(int taskId, int userId)
        {
            using (Uow)
            {
                var result = Uow.Context.Ins_TaskTaskUsers_Create(taskId, userId);
                return result.FirstOrDefault();
            }
        }
    }
}
