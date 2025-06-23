using DataAccess;
using EntitiesObject.Entities.TanTamEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussinessObject.Bo.TanTamBo
{
    /// <summary>
    /// Business Object cho quản lý Task
    /// </summary>
    public class TaskBo : BaseBo<DBNull>
    {
        public TaskBo() : base(DaoFactory.Task)
        {
        }

        #region Task Management

        /// <summary>
        /// Tạo task mới
        /// </summary>
        /// <param name="title">Tiêu đề task</param>
        /// <param name="createdUserObj">ID người tạo</param>
        /// <param name="defaultView">View mặc định</param>
        /// <param name="color">Màu sắc</param>
        /// <param name="departmentIds">Danh sách ID phòng ban (phân cách bằng dấu phẩy)</param>
        /// <param name="positionIds">Danh sách ID vị trí (phân cách bằng dấu phẩy)</param>
        /// <param name="branchIds">Danh sách ID chi nhánh (phân cách bằng dấu phẩy)</param>
        /// <param name="userIds">Danh sách ID người dùng (phân cách bằng dấu phẩy)</param>
        /// <returns>Thông tin task đã tạo</returns>
        public Ins_Tasks_Create_Result CreateTask(string title, int createdUserObj, string defaultView, string color, 
            string departmentIds, string positionIds, string branchIds, string userIds)
        {
            return DaoFactory.Task.CreateTask(title, createdUserObj, defaultView, color, 
                departmentIds, positionIds, branchIds, userIds);
        }

        /// <summary>
        /// Lấy chi tiết task theo ID
        /// </summary>
        /// <param name="taskId">ID của task</param>
        /// <returns>Thông tin chi tiết task</returns>
        public Ins_Task_List_Result GetTaskDetail(int taskId)
        {
            return DaoFactory.Task.GetTaskDetail(taskId);
        }

        /// <summary>
        /// Lấy danh sách tasks
        /// </summary>
        /// <param name="taskId">ID task cụ thể (null để lấy tất cả)</param>
        /// <returns>Danh sách tasks</returns>
        public List<Ins_Task_List_Result> GetTaskList(int? taskId = null)
        {
            return DaoFactory.Task.GetTaskList(taskId);
        }

        #endregion

        #region Task Groups

        /// <summary>
        /// Lấy danh sách nhóm task theo task ID
        /// </summary>
        /// <param name="taskId">ID của task</param>
        /// <returns>Danh sách nhóm task</returns>
        public List<Ins_Task_GetTaskGroupsByTaskId_Result> GetTaskGroupsByTaskId(int taskId)
        {
            return DaoFactory.Task.GetTaskGroupsByTaskId(taskId);
        }

        /// <summary>
        /// Tạo nhóm task mới
        /// </summary>
        /// <param name="bundleId">ID bundle</param>
        /// <param name="name">Tên nhóm</param>
        /// <param name="color">Màu sắc</param>
        /// <param name="position">Vị trí</param>
        /// <returns>Thông tin nhóm task đã tạo</returns>
        public Ins_Task_Group_Create_Result CreateTaskGroup(int bundleId, string name, string color, string position)
        {
            return DaoFactory.Task.CreateTaskGroup(bundleId, name, color, position);
        }

        #endregion

        #region Task Users & Managers

        /// <summary>
        /// Lấy danh sách quản lý của task
        /// </summary>
        /// <param name="taskId">ID của task</param>
        /// <returns>Danh sách quản lý</returns>
        public List<Ins_Task_ManagersByTask_Result> GetTaskManagersByTask(int taskId)
        {
            return DaoFactory.Task.GetTaskManagersByTask(taskId);
        }

        /// <summary>
        /// Lấy danh sách người dùng được gán cho task
        /// </summary>
        /// <param name="taskId">ID của task</param>
        /// <returns>Danh sách người dùng</returns>
        public List<Ins_Task_UsersByTask_Result> GetTaskUsersByTask(int taskId)
        {
            return DaoFactory.Task.GetTaskUsersByTask(taskId);
        }

        /// <summary>
        /// Lấy danh sách tasks được gán cho người dùng
        /// </summary>
        /// <param name="userId">ID của người dùng</param>
        /// <returns>Danh sách tasks</returns>
        public List<Ins_Task_UsersByUser_Result> GetTaskUsersByUser(int userId)
        {
            return DaoFactory.Task.GetTaskUsersByUser(userId);
        }

        /// <summary>
        /// Lấy thông tin người tạo task
        /// </summary>
        /// <param name="taskId">ID của task</param>
        /// <returns>Thông tin người tạo</returns>
        public Ins_Task_CreatorInfo_Result GetTaskCreatorInfo(int taskId)
        {
            return DaoFactory.Task.GetTaskCreatorInfo(taskId);
        }

        #endregion

        #region Sub Tasks

        /// <summary>
        /// Tạo sub-task mới
        /// </summary>
        public Ins_Task_Sub_Create_Result CreateTaskSub(string title, string alias, int? bundleId, int? createdUserId, string position)
        {
            return DaoFactory.Task.CreateTaskSub(title, alias, bundleId, createdUserId, position);
        }

        /// <summary>
        /// Lấy danh sách sub-tasks theo bundle
        /// </summary>
        /// <param name="bundleId">ID của bundle</param>
        /// <returns>Danh sách sub-tasks</returns>
        public List<Ins_Task_Sub_ListByBundle_Result> GetTaskSubListByBundle(string bundleId)
        {
            return DaoFactory.Task.GetTaskSubListByBundle(bundleId);
        }

        #endregion

        #region Task Fields

        /// <summary>
        /// Tạo trường task với các tùy chọn
        /// </summary>
        /// <param name="objectId">ID đối tượng</param>
        /// <param name="title">Tiêu đề trường</param>
        /// <param name="titleNosign">Tiêu đề không dấu</param>
        /// <param name="addToLib">Thêm vào thư viện</param>
        /// <param name="notifyWhenValueChanged">Thông báo khi giá trị thay đổi</param>
        /// <param name="fieldKey">Khóa trường</param>
        /// <param name="isDefault">Có phải mặc định</param>
        /// <param name="createdUserId">ID người tạo</param>
        /// <param name="sortIndex">Thứ tự sắp xếp</param>
        /// <param name="active">Trạng thái hoạt động</param>
        /// <param name="objectSortIndex">Thứ tự sắp xếp đối tượng</param>
        /// <param name="objectActive">Trạng thái hoạt động đối tượng</param>
        /// <returns>Kết quả tạo trường</returns>
        public int CreateTaskFieldWithOptions(int objectId, string title, string titleNosign, bool? addToLib, 
            bool? notifyWhenValueChanged, string fieldKey, bool? isDefault, int? createdUserId, 
            int? sortIndex, bool? active, int? objectSortIndex, bool? objectActive)
        {
            return DaoFactory.Task.CreateTaskFieldWithOptions(objectId, title, titleNosign, addToLib, 
                notifyWhenValueChanged, fieldKey, isDefault, createdUserId, sortIndex, active, 
                objectSortIndex, objectActive);
        }

        #endregion
    }
}
