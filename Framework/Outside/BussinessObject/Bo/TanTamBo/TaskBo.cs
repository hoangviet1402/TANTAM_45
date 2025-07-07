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
        /// <param name="companyId">ID công ty</param>
        /// <param name="defaultView">View mặc định</param>
        /// <param name="color">Màu sắc</param>
        /// <param name="departmentIds">Danh sách ID phòng ban (phân cách bằng dấu phẩy)</param>
        /// <param name="positionIds">Danh sách ID vị trí (phân cách bằng dấu phẩy)</param>
        /// <param name="branchIds">Danh sách ID chi nhánh (phân cách bằng dấu phẩy)</param>
        /// <param name="userIds">Danh sách ID người dùng (phân cách bằng dấu phẩy)</param>
        /// <returns>Thông tin task đã tạo</returns>
        public Ins_Tasks_Create_Result CreateTask(string title, int createdUserObj, int companyId, string defaultView, string color, 
            string departmentIds, string positionIds, string branchIds, string userIds)
        {
            return DaoFactory.Task.CreateTask(title, createdUserObj, companyId, defaultView, color, 
                departmentIds, positionIds, branchIds, userIds);
        }

        /// <summary>
        /// Lấy chi tiết task theo ID
        /// </summary>
        /// <param name="taskId">ID của task</param>
        /// <param name="companyId">ID của công ty</param>
        /// <returns>Thông tin chi tiết task</returns>
        public Ins_Task_List_Result GetTaskDetail(int taskId, int companyId)
        {
            return DaoFactory.Task.GetTaskDetail(taskId, companyId);
        }

        /// <summary>
        /// Lấy danh sách tasks
        /// </summary>
        /// <param name="taskId">ID task cụ thể (null để lấy tất cả)</param>
        /// <param name="companyId">ID của công ty</param>
        /// <returns>Danh sách tasks</returns>
        public List<Ins_Task_List_Result> GetTaskList(int? taskId, int companyId)
        {
            return DaoFactory.Task.GetTaskList(taskId, companyId);
        }

        /// <summary>
        /// Xóa task và tất cả dữ liệu liên quan (gọi lần lượt các stored procedure nhỏ)
        /// </summary>
        /// <param name="taskId">ID của task cần xóa</param>
        /// <returns>Danh sách tasks còn lại</returns>
        public List<Ins_Tasks_Delete_Main_Result> DeleteTask(int taskId, int companyId)
        {
            // Xóa fields và options
            DeleteAllFields(taskId);
            // Xóa các quan hệ (users, managers, departments, ...)
            DeleteAllRelations(taskId);
            // Xóa tất cả subtasks và collaborators
            DeleteAllSubTasksByTaskId(taskId);
            // Xóa tất cả group
            DeleteAllGroupsByTaskId(taskId);
            // Xóa task chính
            return DeleteMainTask(taskId, companyId);
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

        /// <summary>
        /// Xóa group và tất cả subtasks trong group
        /// </summary>
        /// <param name="groupId">ID của group cần xóa</param>
        /// <returns>Danh sách group còn lại trong task</returns>
        public List<Ins_Task_Group_Delete_Result> DeleteTaskGroup(int groupId)
        {
            return DaoFactory.Task.DeleteTaskGroup(groupId);
        }

        /// <summary>
        /// Cập nhật tên của group
        /// </summary>
        /// <param name="groupId">ID của group cần cập nhật</param>
        /// <param name="name">Tên mới của group</param>
        /// <returns>Thông tin group đã cập nhật</returns>
        public Ins_Task_Group_Update_Name_Result UpdateTaskGroupName(int groupId, string name)
        {
            return DaoFactory.Task.UpdateTaskGroupName(groupId, name);
        }

        /// <summary>
        /// Cập nhật màu sắc của group
        /// </summary>
        /// <param name="groupId">ID của group cần cập nhật</param>
        /// <param name="color">Màu sắc mới của group</param>
        /// <returns>Thông tin group đã cập nhật</returns>
        public Ins_Task_Group_Update_Color_Result UpdateTaskGroupColor(int groupId, string color)
        {
            return DaoFactory.Task.UpdateTaskGroupColor(groupId, color);
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
        public Ins_Task_Sub_Create_Result CreateTaskSub(string title, string alias, int? bundleId, int? createdUserId, int? assignedId, string position)
        {
            return DaoFactory.Task.CreateTaskSub(title, alias, bundleId, createdUserId, assignedId, position);
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

        /// <summary>
        /// Cập nhật trạng thái hoàn thành của sub-task
        /// </summary>
        /// <param name="id">ID của sub-task</param>
        /// <param name="isCompleted">Trạng thái hoàn thành</param>
        /// <returns>Thông tin sub-task đã cập nhật</returns>
        public Ins_Task_Sub_Update_Completed_Result UpdateTaskSubCompleted(int id, bool isCompleted)
        {
            return DaoFactory.Task.UpdateTaskSubCompleted(id, isCompleted);
        }

        /// <summary>
        /// Cập nhật deadline của sub-task
        /// </summary>
        /// <param name="id">ID của sub-task</param>
        /// <param name="deadline">Deadline mới</param>
        /// <param name="startDate">Ngày bắt đầu</param>
        /// <returns>Thông tin sub-task đã cập nhật</returns>
        public Ins_Task_Sub_Update_Deadline_Result UpdateTaskSubDeadline(int id, DateTime? deadline, DateTime? startDate)
        {
            return DaoFactory.Task.UpdateTaskSubDeadline(id, deadline, startDate);
        }

        /// <summary>
        /// Thêm collaborators cho task
        /// </summary>
        /// <param name="taskId">ID của task</param>
        /// <param name="userIds">Danh sách ID người dùng (phân cách bằng dấu phẩy)</param>
        /// <returns>Số lượng collaborators đã thêm</returns>
        public int AddTaskCollaborators(string taskId, string userIds)
        {
            return DaoFactory.Task.AddTaskCollaborators(taskId, userIds);
        }

        /// <summary>
        /// Cập nhật tiêu đề của sub-task
        /// </summary>
        /// <param name="id">ID của sub-task</param>
        /// <param name="title">Tiêu đề mới</param>
        /// <param name="titleNosign">Tiêu đề không dấu mới</param>
        /// <param name="alias">Alias của sub-task</param>
        /// <returns>Thông tin sub-task đã cập nhật</returns>
        public Ins_Task_Sub_Update_Title_Result UpdateSubTaskTitle(int id, string title, string titleNosign, string alias)
        {
            return DaoFactory.Task.UpdateSubTaskTitle(id, title, titleNosign, alias);
        }

        #endregion

        #region Task Fields

        /// <summary>
        /// Tạo trường task với các tùy chọn
        /// </summary>
        /// <param name="taskId">ID của task</param>
        /// <param name="title">Tiêu đề trường</param>
        /// <param name="type">Loại trường</param>
        /// <param name="description">Mô tả trường</param>
        /// <param name="addToLibrary">Thêm vào thư viện</param>
        /// <param name="notifyOnChange">Thông báo khi giá trị thay đổi</param>
        /// <returns>Kết quả tạo trường</returns>
        public Ins_Task_Field_Create_Result CreateTaskField(int taskId, string title, string type, string description, bool? addToLibrary, bool? notifyOnChange)
        {
            return DaoFactory.Task.CreateTaskField(taskId, title, type, description, addToLibrary, notifyOnChange);
        }

        #endregion

        public List<Ins_Task_Update_AssignedUser_Result> UpdateTaskAssignedUser(int taskId, int? assignedUser)
        {
            return DaoFactory.Task.UpdateTaskAssignedUser(taskId, assignedUser);
        }

        // Lấy customized fields cho subtask/task (dùng store Ins_Task_Get_CustomizedFields_And_Values_ByTask)
        /// <summary>
        /// Lấy customized fields cho subtask/task
        /// </summary>
        /// <param name="taskId">ID của task</param>
        /// <returns>Số lượng customized fields đã xử lý</returns>
        public int GetCustomizedFieldsAndValuesByTask(int taskId)
        {
            return DaoFactory.Task.GetCustomizedFieldsAndValuesByTask(taskId);
        }

        // Lấy field value của subtask theo title (dùng store Ins_Task_Get_Sub_Field_Value_ByTitle)
        public List<Ins_Task_Get_Sub_Field_Value_ByTitle_Result> GetSubFieldValueByTitle(int subtaskId, string title = null)
        {
            return DaoFactory.Task.GetSubFieldValueByTitle(subtaskId, title);
        }

        public void InsertTaskFieldOptionsBulk(int fieldId, string options)
        {
            DaoFactory.Task.InsertTaskFieldOptionsBulk(fieldId, options);
        }

        /// <summary>
        /// Xóa tất cả quan hệ của task (managers, users, departments, v.v.)
        /// </summary>
        public int DeleteAllRelations(int taskId)
        {
            return DaoFactory.Task.DeleteAllRelations(taskId);
        }

        /// <summary>
        /// Xóa tất cả fields và options của task
        /// </summary>
        public int DeleteAllFields(int taskId)
        {
            return DaoFactory.Task.DeleteAllFields(taskId);
        }

        /// <summary>
        /// Xóa tất cả subtasks và collaborators theo taskId
        /// </summary>
        public int DeleteAllSubTasksByTaskId(int taskId)
        {
            return DaoFactory.Task.DeleteAllSubTasksByTaskId(taskId);
        }

        /// <summary>
        /// Xóa tất cả group theo taskId
        /// </summary>
        public int DeleteAllGroupsByTaskId(int taskId)
        {
            return DaoFactory.Task.DeleteAllGroupsByTaskId(taskId);
        }

        /// <summary>
        /// Xóa task chính (không xóa dữ liệu liên quan)
        /// </summary>
        public List<Ins_Tasks_Delete_Main_Result> DeleteMainTask(int taskId, int companyId)
        {
            return DaoFactory.Task.DeleteMainTask(taskId, companyId);
        }

        public Ins_TaskBranches_Create_Result CreateTaskBranch(int taskId, int branchId)
        {
            return DaoFactory.Task.CreateTaskBranch(taskId, branchId);
        }
        public Ins_TaskDepartments_Create_Result CreateTaskDepartment(int taskId, int departmentId)
        {
            return DaoFactory.Task.CreateTaskDepartment(taskId, departmentId);
        }
        public Ins_TaskPositions_Create_Result CreateTaskPosition(int taskId, int positionId)
        {
            return DaoFactory.Task.CreateTaskPosition(taskId, positionId);
        }
        public Ins_TaskTaskUsers_Create_Result CreateTaskUser(int taskId, int userId)
        {
            return DaoFactory.Task.CreateTaskUser(taskId, userId);
        }
    }
}
