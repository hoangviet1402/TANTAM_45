using DataAccess.EF;
using DataAccess.Interface;
using EntitiesObject.Entities.TanTamEntities;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Dao.TanTamDao
{
    /// <summary>
    /// Interface for Task data access operations
    /// </summary>
    public interface ITaskDao : IBaseFactories<DBNull>
    {
        // Task management
        
        Ins_Task_List_Result GetTaskDetail(int taskId);
        
        List<Ins_Task_List_Result> GetTaskList(int? taskId = null);
        
        Ins_Tasks_Create_Result CreateTask(string title, int createdUserObj, string defaultView, string color, 
            string departmentIds, string positionIds, string branchIds, string userIds);
        
        List<Ins_Task_GetTaskGroupsByTaskId_Result> GetTaskGroupsByTaskId(int taskId);
        
        List<Ins_Task_ManagersByTask_Result> GetTaskManagersByTask(int taskId);
        
        List<Ins_Task_UsersByTask_Result> GetTaskUsersByTask(int taskId);
        
        List<Ins_Task_UsersByUser_Result> GetTaskUsersByUser(int userId);
        
        Ins_Task_CreatorInfo_Result GetTaskCreatorInfo(int taskId);
        
        Ins_Task_Group_Create_Result CreateTaskGroup(int bundleId, string name, string color, string position);
        
        Ins_Task_Sub_Create_Result CreateTaskSub(string title, string alias, int? bundleId, int? createdUserId, string position);
        
        List<Ins_Task_Sub_ListByBundle_Result> GetTaskSubListByBundle(string bundleId);
        
        int CreateTaskFieldWithOptions(int objectId, string title, string titleNosign, bool? addToLib, 
            bool? notifyWhenValueChanged, string fieldKey, bool? isDefault, int? createdUserId, 
            int? sortIndex, bool? active, int? objectSortIndex, bool? objectActive);
    }

    /// <summary>
    /// Implementation of Task data access operations
    /// </summary>
    internal class TaskDao : DaoFactories<TanTamEntities, DBNull>, ITaskDao
    {


        public Ins_Task_List_Result GetTaskDetail(int taskId)
        {
            using (Uow)
            {
                var result = Uow.Context.Ins_Task_List(taskId);
                return result.FirstOrDefault();
            }
        }

        public List<Ins_Task_List_Result> GetTaskList(int? taskId = null)
        {
            using (Uow)
            {
                var result = Uow.Context.Ins_Task_List(taskId);
                return result.ToList();
            }
        }

        public Ins_Tasks_Create_Result CreateTask(string title, int createdUserObj, string defaultView, string color, 
            string departmentIds, string positionIds, string branchIds, string userIds)
        {
            using (Uow)
            {
                var result = Uow.Context.Ins_Tasks_Create(title, createdUserObj, defaultView, color, 
                    departmentIds, positionIds, branchIds, userIds);
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

        public Ins_Task_Sub_Create_Result CreateTaskSub(string title, string alias, int? bundleId, int? createdUserId, string position)
        {
            using (Uow)
            {
                var result = Uow.Context.Ins_Task_Sub_Create(title, alias, bundleId, createdUserId, position);
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

        public int CreateTaskFieldWithOptions(int objectId, string title, string titleNosign, bool? addToLib, 
            bool? notifyWhenValueChanged, string fieldKey, bool? isDefault, int? createdUserId, 
            int? sortIndex, bool? active, int? objectSortIndex, bool? objectActive)
        {
            using (Uow)
            {
                return Uow.Context.Ins_Task_CreateTaskFieldWithOptions(objectId, title, titleNosign, addToLib, 
                    notifyWhenValueChanged, fieldKey, isDefault, createdUserId, sortIndex, active, 
                    objectSortIndex, objectActive);
            }
        }
    }
}
