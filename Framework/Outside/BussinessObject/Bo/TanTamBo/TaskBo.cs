using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BussinessObject.Enum;
using BussinessObject.Helper;
using DataAccess;
using EntitiesObject.Entities.TanTamEntities;
using Logger;
using MyUtility.Extensions;
using Newtonsoft.Json;

namespace BussinessObject.Bo.TanTamBo
{
    /// <summary>
    /// Business Object cho quản lý Task
    /// </summary>
    public class TaskBo : BaseBo<DBNull>
    {
        public TaskBo()
            : base(DaoFactory.Task) { }

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
        public Ins_Tasks_Create_Result CreateTask(
            string title,
            int createdUserObj,
            int companyId,
            string defaultView,
            string color
        )
        {
            return DaoFactory.Task.CreateTask(title, createdUserObj, companyId, defaultView, color);
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
            // Xóa task field subtask dựa trên task_id từ task_field_value
            // DeleteAllTaskFieldSubtask(taskId);
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
        public Ins_Task_Group_Create_Result CreateTaskGroup(
            int bundleId,
            string name,
            string color,
            string position
        )
        {
            return DaoFactory.Task.CreateTaskGroup(bundleId, name, color, position);
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

        /// <summary>
        /// Cập nhật tên và màu sắc của group cùng lúc
        /// </summary>
        /// <param name="groupId">ID của group cần cập nhật</param>
        /// <param name="name">Tên mới của group</param>
        /// <param name="color">Màu sắc mới của group</param>
        /// <returns>Thông tin group đã cập nhật</returns>
        public Ins_Task_Group_Update_Color_And_Name_Result UpdateTaskGroupColorAndName(
            int groupId,
            string name,
            string color
        )
        {
            return DaoFactory.Task.UpdateTaskGroupColorAndName(groupId, name, color);
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
        public Ins_Task_Sub_Create_Result CreateTaskSub(
            string title,
            string alias,
            int? bundleId,
            int? createdUserId,
            int? assignedId,
            string position,
            DateTime deadline
        )
        {
            return DaoFactory.Task.CreateTaskSub(
                title,
                alias,
                bundleId,
                createdUserId,
                assignedId,
                position,
                deadline
            );
        }

        /// <summary>
        /// Lấy danh sách sub-tasks theo bundle
        /// </summary>
        /// <param name="bundleId">ID của bundle</param>
        /// <returns>Danh sách sub-tasks</returns>
        public List<Ins_Task_Sub_ListByBundle_Result> GetTaskSubListByBundle(int bundleId)
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
        public Ins_Task_Sub_Update_Deadline_Result UpdateTaskSubDeadline(
            int id,
            DateTime? deadline,
            DateTime? startDate
        )
        {
            return DaoFactory.Task.UpdateTaskSubDeadline(id, deadline, startDate);
        }

        /// <summary>
        /// Thêm collaborators cho task (chuẩn BO gọi DAO)
        /// </summary>
        /// <param name="taskId">ID của task</param>
        /// <param name="userIds">Danh sách userIds (chuỗi, phân cách bằng dấu phẩy)</param>
        /// <returns>Số lượng collaborators đã thêm</returns>
        public int AddTaskCollaborators(int taskId, int userIds)
        {
            return DaoFactory.Task.Ins_Task_Add_Collaborator(taskId, userIds);
        }

        /// <summary>
        /// Xóa tất cả collaborators của task theo task_id
        /// </summary>
        /// <param name="taskId">ID của task</param>
        /// <returns>Số lượng collaborators đã xóa</returns>
        public int DeleteTaskCollaborators(int taskId)
        {
            return DaoFactory.Task.DeleteTaskCollaborators(taskId);
        }

        /// <summary>
        /// Lấy thông tin collaborators của task (sử dụng Ins_Task_Collaborator_Info)
        /// </summary>
        /// <param name="taskId">ID của task</param>
        /// <returns>Danh sách collaborators</returns>
        public List<Ins_Task_Collaborator_Info_Result> GetTaskCollaboratorsByTask(int taskId)
        {
            return DaoFactory.Task.GetTaskCollaboratorsByTask(taskId);
        }

        /// <summary>
        /// Cập nhật tiêu đề của sub-task
        /// </summary>
        /// <param name="id">ID của sub-task</param>
        /// <param name="title">Tiêu đề mới</param>
        /// <param name="titleNosign">Tiêu đề không dấu mới</param>
        /// <param name="alias">Alias của sub-task</param>
        /// <returns>Thông tin sub-task đã cập nhật</returns>
        public Ins_Task_Sub_Update_Title_Result UpdateSubTaskTitle(
            int id,
            string title,
            string titleNosign,
            string alias
        )
        {
            return DaoFactory.Task.UpdateSubTaskTitle(id, title, titleNosign, alias);
        }

        /// <summary>
        /// Cập nhật mô tả của sub-task
        /// </summary>
        /// <param name="id">ID của sub-task</param>
        /// <param name="description">Mô tả mới</param>
        /// <returns>Thông tin sub-task đã cập nhật</returns>
        public Ins_Task_Sub_Update_Description_Result UpdateSubTaskDescription(
            int id,
            string description
        )
        {
            return DaoFactory.Task.UpdateSubTaskDescription(id, description);
        }

        /// <summary>
        /// Cập nhật task với response đầy đủ thông tin
        /// </summary>
        public object UpdateTaskWithFullResponse(
            int id,
            string title,
            string description,
            int? bundleId,
            DateTime? deadline,
            DateTime? startDate,
            int? duration,
            bool? isCompleted,
            int? completionPercentage,
            int? assignedId,
            int? taskId,
            int updatedUserId
        )
        {
            try
            {
                // Validate id
                if (id <= 0)
                {
                    return new
                    {
                        error_code = ResponseResultEnum.InvalidInput.Value(),
                        message = new[] { ResponseResultEnum.InvalidInput.Text() },
                        from_cache = (object)null,
                        data = new { },
                    };
                }

                // Tạo alias từ title nếu có
                string alias = null;
                string titleNosign = null;
                if (!string.IsNullOrEmpty(title))
                {
                    alias = MyUtility
                        .Extensions.StringExtension.ConvertToUnSign(title)
                        .Replace("-", "_");
                    titleNosign = MyUtility.Extensions.StringExtension.ConvertToUnSign(title);
                }

                // Gọi method UpdateTaskSubById từ DAO
                var result = DaoFactory.Task.UpdateTaskSubById(
                    id,
                    null, // ordinalNumber - giữ nguyên
                    bundleId,
                    null, // sortIndex - giữ nguyên
                    null, // privateSortIndex - giữ nguyên
                    title,
                    titleNosign,
                    description,
                    alias,
                    null, // shopId - giữ nguyên
                    null, // createdUserId - giữ nguyên
                    deadline,
                    startDate,
                    duration,
                    DateTime.Now, // updatedAt - thời gian hiện tại
                    null, // completedAt - giữ nguyên
                    isCompleted,
                    completionPercentage,
                    assignedId,
                    taskId
                );

                if (result == null)
                {
                    return new
                    {
                        error_code = ResponseResultEnum.NotFound.Value(),
                        message = new[] { ResponseResultEnum.NotFound.Text() },
                        from_cache = (object)null,
                        data = new { },
                    };
                }

                // Lấy thông tin user tạo task
                var creatorInfo = GetTaskCreatorInfo(result.created_user_id ?? 0);

                // Tạo response với thông tin đầy đủ
                var responseData = new
                {
                    id = result.id,
                    title = result.title,
                    title_nosign = result.title_nosign,
                    description = result.description,
                    alias = result.alias,
                    bundle_id = result.bundle_id,
                    deadline = result.deadline,
                    start_date = result.start_date,
                    duration = result.duration,
                    is_completed = result.is_completed,
                    completion_percentage = result.completion_percentage ?? 0,
                    assigned_id = result.created_user_id,
                    created_at = result.created_at,
                    updated_at = result.updated_at,
                    completed_at = result.completed_at,
                    user = creatorInfo != null
                        ? new
                        {
                            id = creatorInfo.id,
                            username = creatorInfo.username,
                            name = creatorInfo.name,
                            identification = creatorInfo.identification,
                            branch_id = creatorInfo.branch_id,
                        }
                        : null,
                    status = result.is_completed.HasValue
                        ? new
                        {
                            is_completed = result.is_completed.Value,
                            completion_percentage = result.completion_percentage ?? 0,
                        }
                        : new { is_completed = false, completion_percentage = 0 },
                };

                return new
                {
                    error_code = ResponseResultEnum.Success.Value(),
                    message = new[] { ResponseResultEnum.Success.Text() },
                    from_cache = (object)null,
                    data = responseData,
                };
            }
            catch (Exception ex)
            {
                CommonLogger.DefaultLogger.Error("TaskBo.UpdateTaskWithFullResponse Error", ex);
                return new
                {
                    error_code = ResponseResultEnum.SystemError.Value(),
                    message = new[] { ResponseResultEnum.SystemError.Text() },
                    from_cache = (object)null,
                    data = new { },
                };
            }
        }

        #endregion

        #region Task Fields

        /// <summary>
        /// Tạo field cho task (dùng store Ins_Task_Field_Create và Ins_Task_Field_Option)
        /// </summary>
        public object CreateTaskField(
            int object_id,
            string title,
            int key,
            bool add_to_lib,
            bool notify_when_value_changed,
            int only_created_user_edit,
            int active,
            string source,
            bool is_default
        )
        {
            try
            {
                // Gọi store tạo field
                var fieldResult = DaoFactory.Task.Ins_Task_Field_Create(
                    object_id,
                    title,
                    key,
                    "",
                    add_to_lib,
                    notify_when_value_changed,
                    is_default
                );
                if (fieldResult == null || fieldResult.id <= 0)
                    return null;
                // Trả về object fieldResult (các property cần thiết)
                return fieldResult;
            }
            catch (Exception ex)
            {
                CommonLogger.DefaultLogger.Error("TaskBo.CreateTaskField Error", ex);
                return null;
            }
        }

        public object CreateTaskFieldOption(
            int fieldId,
            string title,
            string color,
            int sort_index,
            string alias
        )
        {
            try
            {
                var result = DaoFactory.Task.Ins_Task_Field_Option(
                    fieldId,
                    title,
                    color,
                    sort_index,
                    alias
                );
                return result;
            }
            catch (Exception ex)
            {
                CommonLogger.DefaultLogger.Error("TaskBo.CreateTaskFieldOption Error", ex);
                return null;
            }
        }

        #endregion

        public Ins_Task_Update_AssignedUser_Result UpdateTaskAssignedUser(
            int taskId,
            int? assignedUser
        )
        {

            return DaoFactory.Task.UpdateTaskAssignedUser(taskId, assignedUser);
        }

        // Lấy field value của subtask theo title (dùng store Ins_Task_Get_Sub_Field_Value_ByTitle)
        public int GetSubFieldValueByTitle(int subtaskId, string title = null)
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

        public List<Ins_Task_Sub_ListBySubTaskId_Result> GetTaskSubListBySubTaskId(int subTaskId)
        {
            return DaoFactory.Task.GetTaskSubListBySubTaskId(subTaskId);
        }

        public List<Ins_Task_Sub_Delete_BySubTaskId_Result> DeleteSubTaskBySubTaskId(int subTaskId)
        {
            return DaoFactory.Task.DeleteSubTaskBySubTaskId(subTaskId);
        }

        public object GetTaskBundleList(int companyId, int userId, string type, bool isAll)
        {
            try
            {
                var bundles = GetTaskList(null, companyId);
                var items =
                    bundles
                        ?.Select(b =>
                        {
                            var creator = BoFactory
                                .Task.GetTaskUsersByUser(b?.created_user_id ?? 0)
                                .FirstOrDefault();
                            return (object)
                                new
                                {
                                    id = b.id,
                                    name = b.name ?? "",
                                    title = b.name ?? "",
                                    icon = b.icon,
                                    is_favorite = b.favored,
                                    default_view = b.view_id,
                                    color = b.color,
                                    updated_at = b.updated_at?.ToString("yyyy-MM-dd HH:mm:ss"),
                                    managers = (
                                        BoFactory.Task.GetTaskManagersByTask(b.id)
                                        ?? new List<Ins_Task_ManagersByTask_Result>()
                                    )
                                        .Select(m => new
                                        {
                                            id = m.id ?? 0,
                                            name = m.name ?? "",
                                            username = m.phone_number ?? "",
                                            identification = m.department_id != null
                                                ? m.department_id.ToString()
                                                : "",
                                            branch_id = m.branch_id ?? 0,
                                        })
                                        .ToList(),
                                    is_archived = b.is_archived,
                                    task_done = DaoFactory.Task.Ins_TaskSubTasks_CountCompleted(
                                        b.id
                                    ),
                                    task_count = b.task_count,
                                    task_overdue = DaoFactory.Task.Ins_TaskSubTasks_CountOverdue(
                                        b.id
                                    ),
                                    users = (
                                        GetTaskUsersByTask(b.id)
                                        ?? new List<Ins_Task_UsersByTask_Result>()
                                    )
                                        .Select(u => new
                                        {
                                            id = u.id ?? 0,
                                            user_id = u.id ?? 0,
                                            name = u.name ?? "",
                                            username = u.username ?? "",
                                            department_id = u.department_id ?? 0,
                                            branch_id = u.branch_id ?? 0,
                                        })
                                        .ToList(),
                                    user_count = b.user_count,
                                    created_user_obj = new
                                    {
                                        id = creator.id,
                                        user_id = creator.id,
                                        name = creator.name,
                                        username = creator.username ?? "",
                                        department_id = creator.DepartmentID ?? 0,
                                        branch_id = creator.BranchId ?? 0,
                                    },
                                    default_deadline_time = b.default_deadline_time,
                                    default_start_time = b.default_start_time,
                                };
                        })
                        .ToList() ?? new List<object>();

                return new
                {
                    error_code = ResponseResultEnum.Success.Value(),
                    message = new[] { ResponseResultEnum.Success.Text() },
                    from_cache = (object)null,
                    data = new { items, meta = new object[] { } },
                };
            }
            catch (Exception ex)
            {
                return new
                {
                    error_code = ResponseResultEnum.SystemError.Value(),
                    message = new[] { ResponseResultEnum.SystemError.Text() },
                    from_cache = (object)null,
                    data = new { items = new object[] { }, meta = new object[] { } },
                };
            }
        }

        /// <summary>
        /// Lấy danh sách group task theo bundle_id và task_level
        /// </summary>
        /// <param name="bundleId">ID của task chính (bundle)</param>
        /// <param name="companyId">ID của công ty</param>
        /// <param name="taskLevel">Mảng các level task (optional)</param>
        /// <returns>Object response theo format JSON mẫu</returns>
        public object GetListGroupTask(int bundleId, int companyId, string[] taskLevel = null)
        {
            try
            {
                if (bundleId.Equals(null))
                {
                    return new
                    {
                        error_code = ResponseResultEnum.InvalidInput.Value(),
                        message = new[] { ResponseResultEnum.InvalidInput.Text() },
                        from_cache = (object)null,
                        data = new { items = new List<object>(), meta = new object[] { } },
                    };
                }

                // Lấy thông tin task (bundle) chính
                var taskList = GetTaskList(bundleId, companyId);
                var mainTask = taskList?.FirstOrDefault(t => t.id == bundleId);

                if (mainTask == null)
                {
                    return new
                    {
                        error_code = ResponseResultEnum.SystemError.Value(),
                        message = new[] { ResponseResultEnum.SystemError.Text() },
                        from_cache = (object)null,
                        data = new { items = new List<object>(), meta = new object[] { } },
                    };
                }

                // Lấy danh sách groups của task
                var taskGroups = GetTaskGroupsByTaskId(bundleId);

                if (taskGroups == null || !taskGroups.Any())
                {
                    // Nếu không có groups, trả về task chính với items rỗng
                    return new
                    {
                        error_code = ResponseResultEnum.Success.Value(),
                        message = new[] { ResponseResultEnum.Success.Text() },
                        from_cache = (object)null,
                        data = new { items = new List<object>(), meta = new object[] { } },
                    };
                }

                // Xây dựng response theo cấu trúc JSON mẫu
                var items = taskGroups
                    .Select(group =>
                    {
                        // Lấy danh sách sub-tasks trong group này
                        var subTasks = GetTaskSubListByBundle(group.id);
                        var groupItems = subTasks
                            .Select(subTask =>
                            {
                                // Lấy thông tin user tạo task
                                var createdUser = GetTaskUsersByUser(subTask.created_user_id ?? 0)
                                    .FirstOrDefault();
                                var assignedUser = subTask.assigned_id.HasValue
                                    ? GetTaskUsersByUser(subTask.assigned_id.Value).FirstOrDefault()
                                    : null;

                                // Lấy tất cả customized fields cho sub task (một lần gọi DB)
                                var allCustomizedFields = DaoFactory.Task.GetSubTaskFieldList(
                                    null,
                                    subTask.id
                                );

                                var collaboratorUser = GetTaskCollaboratorsByTask(subTask.id);
                                var collaboratorObjs =
                                    collaboratorUser
                                        ?.Select(c => new
                                        {
                                            id = c.id,
                                            name = c.name,
                                            username = c.username,
                                            identification = c.identification,
                                            branch_id = c.branch_id,
                                        })
                                        .Cast<object>()
                                        .ToList() ?? new List<object>();
                                var collaborators = collaboratorObjs
                                    .Select(c => ((dynamic)c).id)
                                    .ToArray();

                                // Tách ra priority fields và các fields khác
                                var customizedFieldsPriority = allCustomizedFields
                                    ?.Where(x => x.alias == (int)TaskFieldEnum.priority_id)
                                    .FirstOrDefault();

                                var customizedFieldsCustomized = new Dictionary<string, object>();

                                if (allCustomizedFields != null)
                                {
                                    foreach (
                                        var f in allCustomizedFields.Where(f => f?.alias == null)
                                    )
                                    {
                                        var fieldId = f?.id?.ToString() ?? "";
                                        if (!string.IsNullOrEmpty(fieldId))
                                        {
                                            customizedFieldsCustomized[fieldId] = new
                                            {
                                                key = System.Enum.GetName(
                                                    typeof(TaskFieldElementTypeEnum),
                                                    f?.type ?? 0
                                                ),
                                                value = f?.value_text ?? "",
                                                title = f?.title ?? "",
                                                title_nosign = f?.title ?? "",
                                                color = f?.color ?? "",
                                                sort_index = f?.index_num ?? 0,
                                            };
                                            var test = customizedFieldsCustomized[fieldId];
                                        }
                                    }
                                }
                                var labelList =
                                    DaoFactory.Task.Ins_Task_Label_Value_GetList_BybundleId(
                                        subTask.id
                                    );
                                var labels = labelList
                                    ?.Select(l => new
                                    {
                                        id = l?.id ?? 0,
                                        name = l?.name ?? "",
                                        color = l?.color ?? "",
                                        title = l?.title ?? "",
                                        sort_index = l?.sort_index ?? 0,
                                        bundle_id = l?.bundle_id ?? 0,
                                    })
                                    .ToList();
                                var label_id =
                                    labels?.Select(l => l.id).ToList() ?? new List<int>();
                                return new
                                {
                                    id = subTask.id,
                                    status_id = group.id,
                                    bundle_id = bundleId,
                                    ordinal_number = subTask.ordinal_number,
                                    collaborator_ids = collaborators,
                                    collaborator_objs = collaboratorObjs,
                                    sort_index = subTask.sort_index,
                                    private_sort_index = subTask.private_sort_index,
                                    title = subTask.title,
                                    title_nosign = subTask.title_nosign,
                                    description = subTask.description,
                                    alias = subTask.alias,
                                    shop_id = subTask.shop_id,
                                    created_user_id = subTask.created_user_id,
                                    deadline = subTask.deadline?.ToString("yyyy-MM-dd HH:mm:ss"),
                                    is_milestone = 0,
                                    assigned_id = subTask.assigned_id?.ToString(),
                                    old_status_id = (string)null,
                                    created_at = subTask.created_at?.ToString(
                                        "yyyy-MM-dd HH:mm:ss"
                                    ),
                                    old_bundle_id = bundleId,
                                    assigned_at = subTask.updated_at?.ToString(
                                        "yyyy-MM-dd HH:mm:ss"
                                    ),
                                    deadline_time = subTask.deadline?.ToString("HH:mm"),
                                    recurring_config = (object)null,
                                    recurring_type = (object)null,
                                    label_ids = label_id,
                                    contact_id = (object)null,
                                    done_note = (object)null,
                                    is_done = subTask.is_completed,
                                    location = (object)null,
                                    start_date = subTask.start_date?.ToString(
                                        "yyyy-MM-dd HH:mm:ss"
                                    ),
                                    onesignal_exprired_soon_ids = new object[] { },
                                    onesignal_exprired_ids = new object[] { },
                                    customized_fields = customizedFieldsCustomized,
                                    sub_task_count = 0,
                                    bundle_obj = new
                                    {
                                        id = bundleId,
                                        name = mainTask.name,
                                        title = mainTask.name,
                                        icon = mainTask.icon,
                                        is_favorite = mainTask.favored,
                                        default_view = mainTask.view_id,
                                        color = mainTask.color,
                                        updated_at = mainTask.updated_at?.ToString(
                                            "yyyy-MM-dd HH:mm:ss"
                                        ),
                                        managers = new object[] { },
                                        is_archived = mainTask.is_archived,
                                        default_deadline_time = mainTask.default_deadline_time,
                                        default_start_time = mainTask.default_start_time,
                                    },
                                    is_from_group = 1,
                                    unsign_title = subTask.title_nosign,
                                    status_obj = new
                                    {
                                        id = group.id.ToString(),
                                        name = group.name,
                                        color = group.color,
                                        index = group.index,
                                    },
                                    priority_obj = customizedFieldsPriority != null
                                        ? new
                                        {
                                            id = customizedFieldsPriority?.id ?? null,
                                            name = customizedFieldsPriority?.name ?? null,
                                            color = customizedFieldsPriority?.color ?? null,
                                            index_num = 0,
                                            is_default = 0,
                                            key = (object)null,
                                            value = customizedFieldsPriority?.value ?? null,
                                            type = "priority",
                                            api = (object)null,
                                            keyIndex = (object)null,
                                            select_type = (object)null,
                                        }
                                        : null,
                                    labels = labels,
                                    created_user_obj = createdUser != null
                                        ? new
                                        {
                                            id = createdUser?.id ?? null,
                                            name = createdUser?.name ?? null,
                                            username = createdUser?.username ?? null,
                                            identification = createdUser?.identification ?? null,
                                            branch_id = createdUser?.BranchId ?? null,
                                        }
                                        : null,
                                    duration = subTask.duration,
                                    sub_task_done = 0,
                                    assigned_user = assignedUser != null
                                        ? new
                                        {
                                            id = assignedUser?.id ?? null,
                                            user_id = assignedUser?.id ?? null,
                                            name = assignedUser?.name ?? null,
                                            username = assignedUser?.username ?? null,
                                            department_id = assignedUser?.DepartmentID ?? null,
                                            branch_id = assignedUser?.BranchId ?? null,
                                            is_avatar = 0,
                                        }
                                        : null,
                                    permission = new string[] { "assigner_permission" },
                                    deadline_date = subTask.deadline?.ToString("yyyy-MM-dd"),
                                };
                            })
                            .ToList();

                        return new
                        {
                            id = group.id.ToString(),
                            name = group.name,
                            color = group.color,
                            index = group.index,
                            bundle_id = bundleId,
                            items = groupItems,
                            meta = new
                            {
                                total = groupItems.Count,
                                per_page = 15,
                                current_page = 1,
                                total_pages = 0,
                            },
                        };
                    })
                    .ToList();

                return new
                {
                    error_code = ResponseResultEnum.Success.Value(),
                    message = new[] { ResponseResultEnum.Success.Text() },
                    from_cache = (object)null,
                    data = new { items = items, meta = new object[] { } },
                };
            }
            catch (Exception ex)
            {
                CommonLogger.DefaultLogger.Error("TaskBo.GetListGroupTask Error", ex);
                return new
                {
                    error_code = ResponseResultEnum.SystemError.Value(),
                    message = new[] { ResponseResultEnum.SystemError.Text() },
                    from_cache = (object)null,
                    data = new { items = new List<object>(), meta = new object[] { } },
                };
            }
        }

        /// <summary>
        /// Cập nhật thông tin status (group) trong task bundle
        /// </summary>
        /// <param name="statusId">ID của status (group) cần cập nhật</param>
        /// <param name="name">Tên mới của status</param>
        /// <param name="color">Màu sắc mới của status</param>
        /// <returns>Object response theo format JSON mẫu</returns>
        public object UpdateTaskBundleStatus(int statusId, string name, string color)
        {
            try
            {
                if (statusId.Equals(null))
                {
                    return new
                    {
                        error_code = ResponseResultEnum.InvalidInput.Value(),
                        message = new[] { ResponseResultEnum.InvalidInput.Text() },
                        from_cache = (object)null,
                        data = new { },
                    };
                }

                // Cập nhật tên và màu sắc cùng lúc
                var updateResult = DaoFactory.Task.UpdateTaskGroupColorAndName(
                    statusId,
                    name,
                    color
                );
                if (updateResult == null)
                {
                    return new
                    {
                        error_code = ResponseResultEnum.SystemError.Value(),
                        message = new[] { ResponseResultEnum.SystemError.Text() },
                        from_cache = (object)null,
                        data = new { },
                    };
                }

                // Lấy thông tin group sau khi cập nhật
                var taskGroups = DaoFactory.Task.GetTaskGroupsById(statusId);
                var updatedGroup = taskGroups?.FirstOrDefault(g => g.id == statusId);

                if (updatedGroup == null)
                {
                    return new
                    {
                        error_code = ResponseResultEnum.SystemError.Value(),
                        message = new[] { ResponseResultEnum.SystemError.Text() },
                        from_cache = (object)null,
                        data = new { },
                    };
                }

                // Đếm số lượng sub-tasks trong group này
                var subTasks = GetTaskSubListByBundle(statusId);
                int taskCount = subTasks?.Count ?? 0;

                return new
                {
                    error_code = ResponseResultEnum.Success.Value(),
                    message = new[] { ResponseResultEnum.Success.Text() },
                    from_cache = (object)null,
                    data = new
                    {
                        id = updatedGroup.id,
                        name = updatedGroup.name,
                        title = updatedGroup.name,
                        color = updatedGroup.color,
                        sort_index = updatedGroup.index,
                        task_count = taskCount,
                    },
                };
            }
            catch (Exception ex)
            {
                CommonLogger.DefaultLogger.Error("TaskBo.UpdateTaskBundleStatus Error", ex);
                return new
                {
                    error_code = ResponseResultEnum.SystemError.Value(),
                    message = new[] { ResponseResultEnum.SystemError.Text() },
                    from_cache = (object)null,
                    data = new { },
                };
            }
        }

        /// <summary>
        /// Lấy chi tiết task theo task_id
        /// </summary>
        /// <param name="taskId">ID của task</param>
        /// <param name="companyId">ID của công ty</param>
        /// <returns>Object response theo format JSON mẫu</returns>
        public object GetTaskDetailById(int taskId, int companyId)
        {
            try
            {
                // Lấy thông tin sub-task theo subtaskId (taskId truyền vào)
                var subTask = DaoFactory.Task.Ins_Task_Sub_GetById(taskId);
                if (subTask == null)
                {
                    return new
                    {
                        error_code = ResponseResultEnum.Success.Value(),
                        message = new[] { ResponseResultEnum.NotFound.Text() },
                        from_cache = (object)null,
                        data = new { },
                    };
                }

                var labelList = DaoFactory.Task.Ins_Task_Label_Value_GetList_BybundleId(subTask.id);
                var labels = labelList
                    ?.Select(l => new
                    {
                        id = l.id,
                        name = l.name,
                        color = l.color,
                        title = l.title,
                        sort_index = l.sort_index,
                        bundle_id = l.bundle_id,
                    })
                    .ToList();

                // Lấy thông tin user tạo sub-task
                var createdUser = GetTaskUsersByUser(subTask.created_user_id ?? 0).FirstOrDefault();
                var assignedUser = subTask.assigned_id.HasValue
                    ? GetTaskUsersByUser(subTask.assigned_id.Value).FirstOrDefault()
                    : null;

                var collaboratorUser = GetTaskCollaboratorsByTask(subTask.id);
                var collaboratorObjs =
                    collaboratorUser
                        ?.Select(c => new
                        {
                            id = c.id,
                            name = c.name,
                            username = c.username,
                            identification = c.identification,
                            branch_id = c.branch_id,
                        })
                        .Cast<object>()
                        .ToList() ?? new List<object>();
                var collaborators = collaboratorObjs.Select(c => ((dynamic)c).id).ToArray();
                // Lấy customized fields cho sub-task
                var customizedFieldRows = new List<dynamic>();
                var customizedFields = customizedFieldRows
                    .GroupBy(f => f.field_id)
                    .ToDictionary(
                        fieldGroup =>
                            fieldGroup.First().key?.ToString() ?? fieldGroup.Key.ToString(),
                        fieldGroup => new
                        {
                            key = fieldGroup.First().title,
                            value = fieldGroup.First().value_text,
                            title = fieldGroup.First().title,
                            title_nosign = fieldGroup.First().title?.ToLower().Replace(" ", ""),
                            color = fieldGroup.First().color,
                            sort_index = 0,
                        }
                    );
                // Lấy thông tin bundle (task chính)
                var mainTask = GetTaskList(subTask.task_id, companyId)
                    ?.FirstOrDefault(t => t.id == subTask.task_id);
                var bundleObj =
                    mainTask != null
                        ? new
                        {
                            id = mainTask.id,
                            name = mainTask.name,
                            title = mainTask.name,
                            icon = mainTask.icon,
                            is_favorite = mainTask.favored,
                            default_view = mainTask.view_id,
                            color = mainTask.color,
                            updated_at = mainTask.updated_at?.ToString("yyyy-MM-dd HH:mm:ss"),
                            managers = new object[] { },
                            is_archived = mainTask.is_archived,
                            default_deadline_time = mainTask.default_deadline_time,
                            default_start_time = mainTask.default_start_time,
                        }
                        : null;

                // Lấy thông tin status (group)
                var group = GetTaskGroupsByTaskId(mainTask?.id ?? 0)
                    ?.FirstOrDefault(g => g.id == subTask.task_id);
                var statusObj =
                    group != null
                        ? new
                        {
                            id = group.id.ToString(),
                            name = group.name,
                            title = group.name,
                            color = group.color,
                            sort_index = group.index,
                            task_count = GetTaskSubListByBundle(group.id)?.Count ?? 0,
                        }
                        : null;

                // Lấy priority (mặc định)
                var priorityList = DaoFactory.Task.GetSubTaskFieldList(
                    (int)TaskFieldEnum.priority_id,
                    subTask.id
                );
                var priorityResult = priorityList?.FirstOrDefault();
                object priorityObj;
                if (priorityResult == null)
                {
                    // Fallback to default priority
                    priorityObj = new
                    {
                        id = 0,
                        name = "",
                        key = (object)null,
                        value = "",
                        type = "",
                        api = (object)null,
                        index_num = 0,
                        color = "",
                        keyIndex = (object)null,
                        select_type = (object)null,
                        dropDownData = (object)null,
                        optionList = (object)null,
                        children = (object)null,
                        titleIndex = (object)null,
                        is_default = 1,
                        title = "",
                    };
                }
                else
                {
                    // Convert from result to priority object
                    priorityObj = new
                    {
                        id = priorityResult.id,
                        name = priorityResult.name,
                        key = (object)null,
                        value = priorityResult.value,
                        type = priorityResult.type,
                        api = (object)null,
                        index_num = priorityResult.index_num ?? 0,
                        color = priorityResult.color,
                        keyIndex = (object)null,
                        select_type = (object)null,
                        dropDownData = (object)null,
                        optionList = (object)null,
                        children = (object)null,
                        titleIndex = (object)null,
                        is_default = 1,
                    };
                }

                // Lấy section (mặc định)
                var sectionObj = new { id = 0, name = "" };

                return new
                {
                    error_code = ResponseResultEnum.Success.Value(),
                    message = new[] { ResponseResultEnum.Success.Text() },
                    from_cache = (object)null,
                    data = new
                    {
                        id = subTask.id.ToString(),
                        title = subTask.title,
                        is_form = (object)null,
                        is_milestone = 0,
                        forms = (object)null,
                        sort_index = subTask.sort_index ?? 0,
                        ordinal_number = subTask.ordinal_number,
                        done_note = (object)null,
                        is_done = subTask.is_completed,
                        unsign_title = subTask.title_nosign,
                        file_count = 0,
                        recurring_type = (object)null,
                        collaborators = collaborators ?? new dynamic[] { },
                        collaborator_objs = collaboratorObjs ?? new List<object>(),
                        location = (object)null,
                        status_id = subTask.task_id,
                        is_task_approval = 0,
                        is_looping = (object)null,
                        loop_type = (object)null,
                        loop_interval = (object)null,
                        percent = 0,
                        duration = subTask.duration,
                        form_info = new object[] { },
                        description = subTask.description,
                        created_at = subTask.created_at?.ToString("yyyy-MM-dd HH:mm"),
                        customized_fields = customizedFields,
                        deadline = subTask.deadline?.ToString("yyyy-MM-dd"),
                        deadline_date = subTask.deadline?.ToString("yyyy-MM-dd"),
                        deadline_time = subTask.deadline?.ToString("HH:mm"),
                        start_date = subTask.start_date?.ToString("yyyy-MM-dd"),
                        start_time = subTask.start_date?.ToString("HH:mm"),
                        sub_task_done = 0,
                        comment_count = 0,
                        permission = new string[] { "assigner_permission", "assigner_permission" },
                        section_id = 0,
                        section_obj = sectionObj,
                        bundle_id = subTask.bundle_id?.ToString(),
                        bundle_obj = bundleObj,
                        assigned_user = assignedUser != null
                            ? new
                            {
                                id = assignedUser.id,
                                name = assignedUser.name,
                                username = assignedUser.username,
                                identification = assignedUser.identification,
                                branch_id = assignedUser.BranchId?.ToString(),
                                is_avatar = 0,
                            }
                            : null,
                        priority_obj = priorityObj,
                        status_obj = statusObj,
                        created_user_obj = createdUser != null
                            ? new
                            {
                                id = createdUser.id,
                                name = createdUser.name,
                                username = createdUser.username,
                                identification = createdUser.identification,
                                branch_id = createdUser.BranchId?.ToString(),
                                is_avatar = 0,
                            }
                            : null,
                        recurring_config = (object)null,
                        cover_image_obj = (object)null,
                        labels = labels,
                    },
                };
            }
            catch (Exception ex)
            {
                CommonLogger.DefaultLogger.Error("TaskBo.GetTaskDetailById Error", ex);
                return new
                {
                    error_code = ResponseResultEnum.SystemError.Value(),
                    message = new[] { ResponseResultEnum.SystemError.Text() },
                    from_cache = (object)null,
                    data = new { },
                };
            }
        }

        /// <summary>
        /// Lấy danh sách user theo bundle_id (task_id) sử dụng Ins_Task_GetListUsers_ByTaskId
        /// </summary>
        /// <param name="bundleId">ID của bundle/task</param>
        /// <returns>Danh sách user</returns>
        public List<Ins_Task_GetListUsers_ByTaskId_Result> GetTaskUsersByBundleId(int bundleId)
        {
            return DaoFactory.Task.Ins_Task_GetListUsers_ByTaskId(bundleId);
        }

        /// <summary>
        /// Lấy danh sách user theo bundle_id (task_id) trả về object response đúng format mẫu
        /// </summary>
        /// <param name="bundleId">ID của bundle/task</param>
        /// <returns>Object response theo format mẫu</returns>
        public object GetTaskUserListResponse(int bundleId)
        {
            try
            {
                var users = GetTaskUsersByBundleId(bundleId);
                var items =
                    users
                        ?.Select(u =>
                            (object)
                                new
                                {
                                    id = u.id,
                                    user_id = u.id.ToString(),
                                    name = u.name,
                                    username = u.username,
                                    department_id = u.DepartmentID?.ToString() ?? "",
                                    branch_id = u.BranchId?.ToString() ?? "",
                                }
                        )
                        .ToList() ?? new List<object>();

                return new
                {
                    error_code = ResponseResultEnum.Success.Value(),
                    message = new[] { ResponseResultEnum.Success.Text() },
                    from_cache = (object)null,
                    data = new { items },
                };
            }
            catch
            {
                return new
                {
                    error_code = ResponseResultEnum.SystemError.Value(),
                    message = new[] { ResponseResultEnum.SystemError.Text() },
                    from_cache = (object)null,
                    data = new { items = new object[] { } },
                };
            }
        }

        public object SetTaskBundleFavorite(int bundleId, bool isFavorite)
        {
            try
            {
                var result = DaoFactory.Task.SetTaskBundleFavorite(bundleId, isFavorite);
                return new
                {
                    error_code = ResponseResultEnum.Success.Value(),
                    message = new[] { ResponseResultEnum.Success.Text() },
                    from_cache = (object)null,
                    data = result,
                };
            }
            catch (Exception ex)
            {
                return new
                {
                    error_code = ResponseResultEnum.SystemError.Value(),
                    message = new[] { ResponseResultEnum.SystemError.Text() },
                    from_cache = (object)null,
                    data = new { },
                };
            }
        }

        /// <summary>
        /// Tạo task bundle status (group) mới
        /// </summary>
        /// <param name="bundleId">ID của bundle (task)</param>
        /// <param name="name">Tên của status/group</param>
        /// <param name="color">Màu sắc của status</param>
        /// <param name="position">Vị trí của status</param>
        /// <returns>Response object với thông tin status đã tạo</returns>
        public object CreateTaskBundleStatus(
            int bundleId,
            string name,
            string color,
            string position
        )
        {
            try
            {
                // Set default values nếu không có
                if (string.IsNullOrEmpty(name))
                {
                    name = "Không tiêu đề";
                }

                if (string.IsNullOrEmpty(color))
                {
                    color = "#cccccc";
                }

                if (string.IsNullOrEmpty(position))
                {
                    position = "last";
                }

                // Gọi method CreateTaskGroup từ DAO
                var result = DaoFactory.Task.CreateTaskGroup(bundleId, name, color, position);

                if (result != null)
                {
                    return new
                    {
                        error_code = ResponseResultEnum.Success.Value(),
                        message = new[] { ResponseResultEnum.Success.Text() },
                        from_cache = (object)null,
                        data = new
                        {
                            id = result.id.ToString(),
                            name = result.name,
                            title = result.name,
                            color = result.color,
                            sort_index = result.index,
                            task_count = 0,
                        },
                    };
                }
                else
                {
                    return new
                    {
                        error_code = ResponseResultEnum.SystemError.Value(),
                        message = new[] { ResponseResultEnum.SystemError.Text() },
                        from_cache = (object)null,
                        data = new { },
                    };
                }
            }
            catch (Exception ex)
            {
                // Log error
                Logger.CommonLogger.DefaultLogger.ErrorFormat(
                    "CreateTaskBundleStatus Exception bundleId {0}, name {1}, color {2}, position {3}, EX: {4}",
                    bundleId,
                    name,
                    color,
                    position,
                    ex.ToString()
                );

                return new
                {
                    error_code = ResponseResultEnum.SystemError.Value(),
                    message = new[] { ResponseResultEnum.SystemError.Text() },
                    from_cache = (object)null,
                    data = new { },
                };
            }
        }

        /// <summary>
        /// Xóa task bundle status theo ID
        /// </summary>
        /// <param name="statusId">ID của status cần xóa</param>
        /// <returns>Response object với kết quả xóa</returns>
        public object DeleteTaskBundleStatus(int statusId)
        {
            try
            {
                if (statusId <= 0)
                {
                    return new
                    {
                        error_code = ResponseResultEnum.InvalidInput.Value(),
                        message = new[] { "Status ID không hợp lệ." },
                        from_cache = (object)null,
                        data = new { },
                    };
                }

                var result = DaoFactory.Task.DeleteTaskBundleStatus(statusId);

                return new
                {
                    error_code = ResponseResultEnum.Success.Value(),
                    message = new[] { "Successfully" },
                    from_cache = (object)null,
                    data = "Thành công.",
                };
            }
            catch (Exception ex)
            {
                CommonLogger.DefaultLogger.Error("TaskBo.DeleteTaskBundleStatus Error", ex);
                return new
                {
                    error_code = ResponseResultEnum.SystemError.Value(),
                    message = new[] { ResponseResultEnum.SystemError.Text() },
                    from_cache = (object)null,
                    data = new { },
                };
            }
        }

        /// <summary>
        /// Tạo task mới (sub-task) với response format đầy đủ
        /// </summary>
        /// <param name="id">ID của task (có thể là timestamp)</param>
        /// <param name="title">Tiêu đề task</param>
        /// <param name="bundleId">ID của bundle</param>
        /// <param name="statusId">ID của status</param>
        /// <param name="position">Vị trí của task</param>
        /// <param name="createdUserId">ID người tạo</param>
        /// <returns>Response object với thông tin task đã tạo</returns>
        public object CreateTaskWithFullResponse(
            string title,
            string deadlineDate,
            int bundleId,
            int statusId,
            string position,
            int createdUserId
        )
        {
            try
            {
                // Parse bundle_id từ string sang int
                if (bundleId <= 0)
                {
                    return new
                    {
                        error_code = ResponseResultEnum.InvalidInput.Value(),
                        message = new[] { ResponseResultEnum.InvalidInput.Text() },
                        from_cache = (object)null,
                        data = new { },
                    };
                }

                // Set default values
                if (string.IsNullOrEmpty(position))
                {
                    position = "last";
                }

                // Tạo alias từ title
                string alias = MyUtility
                    .Extensions.StringExtension.ConvertToUnSign(title)
                    .Replace("-", "_");

                DateTime? deadline = null;
                if (!string.IsNullOrEmpty(deadlineDate))
                {
                    if (DateTime.TryParse(deadlineDate, out var parsedDate))
                        deadline = parsedDate;
                }

                // Gọi method CreateTaskSub từ DAO
                var result = DaoFactory.Task.CreateTaskSub(
                    title,
                    alias,
                    statusId,
                    createdUserId,
                    null,
                    position,
                    deadline
                );

                if (result != null)
                {
                    // Lấy thông tin user tạo task
                    var createdUser = GetTaskUsersByUser(result.created_user_id ?? 0)
                        .FirstOrDefault();

                    // Lấy thông tin bundle
                    var bundleInfo = GetTaskList(result.bundle_id, 0)?.FirstOrDefault();

                    // Lấy thông tin status
                    var statusInfo = GetTaskGroupsByTaskId(result.bundle_id ?? 0)
                        ?.FirstOrDefault(s => s.id == statusId);

                    // add field vào task dùng Ins_Task_Field_Add
                    Ins_Task_Sub_Field_Add_ByType((int)TaskFieldEnum.priority_id, result.id);
                    Ins_Task_Sub_Field_Add_ByType((int)TaskFieldEnum.assigned_id, result.id);
                    Ins_Task_Sub_Field_Add_ByType((int)TaskFieldEnum.deadline, result.id);
                    Ins_Task_Sub_Field_Add_ByType((int)TaskFieldEnum.label_ids, result.id);
                    Ins_Task_Sub_Field_Add_ByType((int)TaskFieldEnum.created_user_id, result.id);
                    Ins_Task_Sub_Field_Add_ByType((int)TaskFieldEnum.collaborators, result.id);
                    Ins_Task_Sub_Field_Add_ByType((int)TaskFieldEnum.created_at, result.id);

                    //gọi danh sách tất cả các field của task dùng GetTaskFieldList
                    var fieldList = BoFactory.Task.GetTaskFieldList("", bundleId);
                    //loại trừ các field có type là mấy cái Priority, assigned_id, deadline, label_ids, created_user_id, collaborators, created_at
                    fieldList = fieldList
                        .Where(f =>
                            f.key != (int)TaskFieldEnum.priority_id
                            && f.key != (int)TaskFieldEnum.assigned_id
                            && f.key != (int)TaskFieldEnum.deadline
                            && f.key != (int)TaskFieldEnum.label_ids
                            && f.key != (int)TaskFieldEnum.created_user_id
                            && f.key != (int)TaskFieldEnum.collaborators
                            && f.key != (int)TaskFieldEnum.created_at
                        )
                        .ToList();
                    //thêm các field vào task dùng Ins_Task_Sub_Field_Add_ById
                    if (fieldList != null && fieldList.Count > 0)
                    {
                        foreach (var field in fieldList)
                        {
                            BoFactory.Task.Ins_Task_Sub_Field_Add_ById(result.id, field.field_id);
                        }
                    }
                    // thêm collaborators tương tự như ở kia
                    var collaboratorUser = GetTaskCollaboratorsByTask(result.id);
                    var collaboratorObjs =
                        collaboratorUser
                            ?.Select(c => new
                            {
                                id = c.id,
                                name = c.name,
                                username = c.username,
                                identification = c.identification,
                                branch_id = c.branch_id,
                            })
                            .Cast<object>()
                            .ToList() ?? new List<object>();
                    var collaborators = collaboratorObjs.Select(c => ((dynamic)c).id).ToArray();
                    // Tạo response object đầy đủ
                    var responseData = new
                    {
                        id = result.id,
                        title = result.title,
                        is_form = (object)null,
                        is_milestone = 0,
                        forms = (object)null,
                        sort_index = result.sort_index ?? -36,
                        ordinal_number = result.ordinal_number ?? 25,
                        deadline = result.deadline,
                        deadline_date = result.deadline?.ToString("yyyy-MM-dd"),
                        start_date = result.start_date,
                        done_note = (object)null,
                        is_done = (object)null,
                        unsign_title = result.title_nosign,
                        file_count = 0,
                        recurring_type = (object)null,
                        location = (object)null,
                        status_id = statusId,
                        is_task_approval = 0,
                        is_looping = (object)null,
                        loop_type = (object)null,
                        loop_interval = (object)null,
                        percent = 0,
                        duration = result.duration,
                        form_info = new object[] { },
                        description = result.description,
                        created_at = result.created_at?.ToString("yyyy-MM-dd HH:mm"),
                        sub_task_done = 0,
                        comment_count = 0,
                        permission = new string[] { "assigner_permission", "assigner_permission" },
                        section_id = 0,
                        section_obj = new { id = 0, name = "" },
                        bundle_id = bundleId,
                        bundle_obj = bundleInfo != null
                            ? new
                            {
                                id = bundleInfo.id.ToString(),
                                name = bundleInfo.name,
                                title = bundleInfo.name,
                                icon = bundleInfo.icon,
                                is_favorite = bundleInfo.favored,
                                default_view = bundleInfo.view_id,
                                color = bundleInfo.color,
                                updated_at = bundleInfo.updated_at?.ToString("yyyy-MM-dd HH:mm:ss"),
                                managers = new object[] { },
                                is_archived = bundleInfo.is_archived,
                                default_deadline_time = bundleInfo.default_deadline_time,
                                default_start_time = bundleInfo.default_start_time,
                            }
                            : null,
                        status_obj = statusInfo != null
                            ? new
                            {
                                id = statusInfo.id.ToString(),
                                name = statusInfo.name,
                                title = statusInfo.name,
                                color = statusInfo.color,
                                sort_index = statusInfo.index,
                                task_count = 2,
                            }
                            : null,
                        created_user_obj = createdUser != null
                            ? new
                            {
                                id = createdUser.id.ToString(),
                                name = createdUser.name,
                                username = createdUser.username,
                                identification = createdUser.identification,
                                branch_id = createdUser.BranchId?.ToString(),
                                is_avatar = 0,
                            }
                            : null,
                        recurring_config = (object)null,
                        collaborator_objs = collaboratorObjs ?? new List<object>(),
                        collaborators = collaborators ?? new dynamic[] { },
                        cover_image_obj = (object)null,
                    };

                    return new
                    {
                        error_code = ResponseResultEnum.Success.Value(),
                        message = new[] { ResponseResultEnum.Success.Text() },
                        from_cache = (object)null,
                        data = responseData,
                    };
                }
                else
                {
                    return new
                    {
                        error_code = ResponseResultEnum.SystemError.Value(),
                        message = new[] { ResponseResultEnum.SystemError.Text() },
                        from_cache = (object)null,
                        data = new { },
                    };
                }
            }
            catch (Exception ex)
            {
                // Log error
                Logger.CommonLogger.DefaultLogger.ErrorFormat(
                    "CreateTaskWithFullResponse Exception title {0}, bundleId {1}, statusId {2}, position {3}, createdUserId {4}, EX: {5}",
                    title,
                    bundleId,
                    statusId,
                    position,
                    createdUserId,
                    ex.ToString()
                );

                return new
                {
                    error_code = ResponseResultEnum.SystemError.Value(),
                    message = new[] { ResponseResultEnum.SystemError.Text() },
                    from_cache = (object)null,
                    data = new { },
                };
            }
        }

        /// <summary>
        /// Lấy danh sách elements theo type (priority, status, etc.)
        /// </summary>
        /// <param name="type">Loại element (priority, status, category, label)</param>
        /// <returns>Danh sách elements</returns>
        public object GetElementListByType(string type)
        {
            try
            {
                // Validate type
                if (string.IsNullOrEmpty(type))
                {
                    return new
                    {
                        error_code = ResponseResultEnum.InvalidInput.Value(),
                        message = new[] { ResponseResultEnum.InvalidInput.Text() },
                        from_cache = (object)null,
                        data = new { },
                    };
                }

                // Map type string sang field_id từ enum
                int fieldId = 0;

                if (type.ToLower() == "priority")
                {
                    fieldId = (int)TaskFieldEnum.priority_id;

                    // Gọi DAO để lấy danh sách elements
                    var elements = GetTaskFieldByType(fieldId);

                    if (elements == null || !elements.Any())
                    {
                        return new
                        {
                            error_code = ResponseResultEnum.NotFound.Value(),
                            message = new[] { ResponseResultEnum.NotFound.Text() },
                            from_cache = (object)null,
                            data = new List<object>(),
                        };
                    }

                    var responseData = elements
                        .Select(element => new
                        {
                            id = element.option_id,
                            name = element.option_title,
                            key = (string)null,
                            value = element.option_title?.ToLower().Replace(" ", "_"),
                            type = type.ToLower(),
                            api = (string)null,
                            index_num = element.option_sort_index ?? 0,
                            color = element.option_color,
                            keyIndex = (string)null,
                            select_type = (string)null,
                            dropDownData = (string)null,
                            optionList = (string)null,
                            children = (string)null,
                            titleIndex = (string)null,
                            is_default = 0,
                            title = element.option_title,
                        })
                        .ToList();
                    return new
                    {
                        error_code = ResponseResultEnum.Success.Value(),
                        message = new[] { ResponseResultEnum.Success.Text() },
                        from_cache = (object)null,
                        data = responseData,
                    };
                }
                else if (type.ToLower() == "dropdown")
                {
                    fieldId = (int)TaskFieldElementTypeEnum.dropdown;
                    var elements = GetTaskFieldList("", fieldId);
                    if (elements == null || !elements.Any())
                    {
                        return new
                        {
                            error_code = ResponseResultEnum.NotFound.Value(),
                            message = new[] { ResponseResultEnum.NotFound.Text() },
                            from_cache = (object)null,
                            data = new List<object>(),
                        };
                    }

                    // Map sang response format theo mẫu
                    var responseData = elements
                        .Select(element => new
                        {
                            id = element.field_id.ToString(),
                            title = element.field_title,
                            title_nosign = element.field_title?.ToLower().Replace(" ", ""),
                            add_to_lib = element.add_to_library,
                            notify_when_value_changed = element.notify_on_change,
                            key = "dropdown",
                            is_default = false,
                            used_objects = 1,
                            sort_index = 0,
                            objects = new Dictionary<string, object> { },
                            object_id = "",
                            active = 1,
                            options = new List<object>
                            {
                                new
                                {
                                    id = "option1",
                                    title = "Option 1",
                                    color = "#a4cf30",
                                    sort_index = 0,
                                    title_nosign = "option 1",
                                },
                                new
                                {
                                    id = "option2",
                                    title = "Option 2",
                                    color = "#ea4e9d",
                                    sort_index = 1,
                                    title_nosign = "option 2",
                                },
                            },
                            option_by_id = new Dictionary<string, object>
                            {
                                {
                                    "option1",
                                    new
                                    {
                                        id = "option1",
                                        title = "Option 1",
                                        color = "#a4cf30",
                                        sort_index = 0,
                                        title_nosign = "option 1",
                                    }
                                },
                                {
                                    "option2",
                                    new
                                    {
                                        id = "option2",
                                        title = "Option 2",
                                        color = "#ea4e9d",
                                        sort_index = 1,
                                        title_nosign = "option 2",
                                    }
                                },
                            },
                            option_by_title_nosign = new Dictionary<string, object>
                            {
                                {
                                    "option 1",
                                    new
                                    {
                                        id = "option1",
                                        title = "Option 1",
                                        color = "#a4cf30",
                                        sort_index = 0,
                                        title_nosign = "option 1",
                                    }
                                },
                                {
                                    "option 2",
                                    new
                                    {
                                        id = "option2",
                                        title = "Option 2",
                                        color = "#ea4e9d",
                                        sort_index = 1,
                                        title_nosign = "option 2",
                                    }
                                },
                            },
                        })
                        .ToList();

                    return new
                    {
                        error_code = ResponseResultEnum.Success.Value(),
                        message = new[] { ResponseResultEnum.Success.Text() },
                        from_cache = (object)null,
                        data = responseData,
                    };
                }
                else
                {
                    return new
                    {
                        error_code = ResponseResultEnum.InvalidInput.Value(),
                        message = new[] { ResponseResultEnum.InvalidInput.Text() },
                        from_cache = (object)null,
                        data = new { },
                    };
                }

                // Map sang response format theo mẫu

                return new
                {
                    error_code = ResponseResultEnum.Success.Value(),
                    message = new[] { ResponseResultEnum.Success.Text() },
                    from_cache = (object)null,
                    data = new { },
                };
            }
            catch (System.Data.Entity.Core.EntityCommandExecutionException entityEx)
            {
                // Handle Entity Framework exceptions
                if (
                    entityEx.InnerException != null
                    && entityEx.InnerException is System.Data.SqlClient.SqlException sqlEx
                )
                {
                    return new
                    {
                        error_code = ResponseResultEnum.SystemError.Value(),
                        message = new[] { ResponseResultEnum.SystemError.Text() },
                        from_cache = (object)null,
                        data = new { },
                    };
                }
                else
                {
                    CommonLogger.DefaultLogger.Error("TaskBo.GetElementListByType Error", entityEx);
                    return new
                    {
                        error_code = ResponseResultEnum.SystemError.Value(),
                        message = new[] { ResponseResultEnum.SystemError.Text() },
                        from_cache = (object)null,
                        data = new { },
                    };
                }
            }
            catch (Exception ex)
            {
                CommonLogger.DefaultLogger.Error("TaskBo.GetElementListByType Error", ex);
                return new
                {
                    error_code = ResponseResultEnum.SystemError.Value(),
                    message = new[] { ResponseResultEnum.SystemError.Text() },
                    from_cache = (object)null,
                    data = new { },
                };
            }
        }

        /// <summary>
        /// Cập nhật priority cho subtask và trả về detail subtask
        /// </summary>
        public object UpdateSubTaskPriority(int subTaskId, int? priorityId, int companyId, int userId)
        {
            try
            {
                if (subTaskId <= 0)
                {
                    return new
                    {
                        error_code = ResponseResultEnum.InvalidInput.Value(),
                        message = new[] { "Subtask ID hoặc Priority ID không hợp lệ." },
                        from_cache = (object)null,
                        data = new { },
                    };
                }
                // Gọi DAO để update
                DaoFactory.Task.UpdateSubTaskPriority(subTaskId, null, priorityId);

                var priorityData = GetElementListByType("priority") as dynamic;
                var priorityName = "";
                if (priorityData?.data != null && priorityId.HasValue)
                {
                    foreach (var item in priorityData.data)
                    {
                        if (item.id.ToString() == priorityId.Value.ToString())
                        {
                            priorityName = item.name;
                            break;
                        }
                    }
                }

                var taskCommentResult = DaoFactory.Comment.CreateTaskComment(
                    subTaskId,
                    userId,
                    "{0} " + "đã thay đổi độ ưu tiên sang" + " {1}",
                    "task",
                    false,
                    false,
                    "change_attribute",
                    "priority_id"
                );

                if (taskCommentResult != null)
                {
                    DaoFactory.Comment.AddTaskCommentMention(taskCommentResult.Id.Value, userId, null);
                    DaoFactory.Comment.AddTaskCommentMention(taskCommentResult.Id.Value, null, string.IsNullOrEmpty(priorityName) ? "—" : priorityName);
                }

                // Lấy lại detail subtask sau khi update
                var detail = GetTaskDetailById(subTaskId, companyId);
                return detail;
            }
            catch (System.Data.Entity.Core.EntityCommandExecutionException entityEx)
            {
                // Handle Entity Framework exceptions
                if (
                    entityEx.InnerException != null
                    && entityEx.InnerException is System.Data.SqlClient.SqlException sqlEx
                )
                {
                    return new
                    {
                        error_code = ResponseResultEnum.Failed.Value(),
                        message = new[] { sqlEx.Message },
                        from_cache = (object)null,
                        data = new { },
                    };
                }
                else
                {
                    CommonLogger.DefaultLogger.Error(
                        "TaskBo.UpdateSubTaskPriority Error",
                        entityEx
                    );
                    return new
                    {
                        error_code = ResponseResultEnum.SystemError.Value(),
                        message = new[] { ResponseResultEnum.SystemError.Text() },
                        from_cache = (object)null,
                        data = new { },
                    };
                }
            }
            catch (Exception ex)
            {
                CommonLogger.DefaultLogger.Error("TaskBo.UpdateSubTaskPriority Error", ex);
                return new
                {
                    error_code = ResponseResultEnum.SystemError.Value(),
                    message = new[] { ResponseResultEnum.SystemError.Text() },
                    from_cache = (object)null,
                    data = new { },
                };
            }
        }

        /// <summary>
        /// Cập nhật custom field cho subtask
        /// </summary>
        public object UpdateTaskCustomField(
            int subTaskId,
            int customizedFieldId,
            string value,
            int companyId
        )
        {
            try
            {
                // Gọi DAO để update
                var result = DaoFactory.Task.UpdateTaskCustomField(
                    customizedFieldId,
                    subTaskId,
                    value
                );

                // Lấy lại detail subtask sau khi update
                var detail = GetTaskDetailById(subTaskId, companyId);
                return detail;
            }
            catch (Exception ex)
            {
                CommonLogger.DefaultLogger.Error("TaskBo.UpdateTaskCustomField Error", ex);
                return new
                {
                    error_code = ResponseResultEnum.SystemError.Value(),
                    message = new[] { ResponseResultEnum.SystemError.Text() },
                    from_cache = (object)null,
                    data = new { },
                };
            }
        }

        /// <summary>
        /// Cập nhật thông tin bundle (task) và các quan hệ liên quan
        /// </summary>
        public object UpdateTaskBundle(
            int bundleId,
            string name,
            string defaultView,
            List<string> departmentIds,
            List<string> positionIds,
            List<string> branchIds,
            List<string> userIds,
            int companyId
        )
        {
            try
            {
                // Cập nhật bundle chính
                DaoFactory.Task.UpdateTaskBundleById(bundleId, name, defaultView, null);
                DaoFactory.Task.DeleteBasicRelationsByTaskId(bundleId);

                // Thêm lại các quan hệ mới
                if (branchIds != null)
                    foreach (var branchId in branchIds)
                        CreateTaskBranch(bundleId, int.TryParse(branchId, out var bid) ? bid : 0);
                if (departmentIds != null)
                    foreach (var depId in departmentIds)
                        CreateTaskDepartment(bundleId, int.TryParse(depId, out var did) ? did : 0);
                if (positionIds != null)
                    foreach (var posId in positionIds)
                        CreateTaskPosition(bundleId, int.TryParse(posId, out var pid) ? pid : 0);
                if (userIds != null)
                    foreach (var userId in userIds)
                        CreateTaskUser(bundleId, int.TryParse(userId, out var uid) ? uid : 0);
                // Lấy lại detail bundle mới nhất
                var bundle = GetTaskList(bundleId, companyId)?.FirstOrDefault();
                if (bundle == null)
                {
                    return new
                    {
                        error_code = ResponseResultEnum.NoData.Value(),
                        message = new[] { ResponseResultEnum.NoData.Text() },
                        from_cache = (object)null,
                        data = new { },
                    };
                }
                // Lấy managers
                var managers = GetTaskManagersByTask(bundle.id)
                    ?.Select(m => new
                    {
                        id = m.id?.ToString(),
                        name = m.name,
                        username = m.phone_number,
                        identification = m.department_id?.ToString() ?? "",
                        branch_id = m.branch_id?.ToString() ?? "",
                    })
                    .ToList();
                // Lấy users
                var users = GetTaskUsersByTask(bundle.id)
                    ?.Select(u => new
                    {
                        id = u.id?.ToString(),
                        user_id = u.id?.ToString(),
                        name = u.name,
                        username = u.username,
                        department_id = u.department_id?.ToString() ?? "",
                        branch_id = u.branch_id?.ToString() ?? "",
                    })
                    .ToList();
                // Lấy creator
                var creator = GetTaskCreatorInfo(bundle.id);
                // Lấy departments, positions, branches mới nhất
                var departments = DaoFactory
                    .Task.Ins_TaskDepartments_GetList_ByTaskId(bundle.id)
                    ?.Select(d => new
                    {
                        id = d.id,
                        name = d.name,
                        parent_id = d.parent_id,
                    })
                    .ToList();
                var positions = DaoFactory
                    .Task.Ins_TaskPositions_GetList_ByTaskId(bundle.id)
                    ?.Select(p => new { value = p.id, label = p.name })
                    .ToList();
                var branches = DaoFactory
                    .Task.Ins_TaskBranches_GetList_ByTaskId(bundle.id)
                    ?.Select(b => new { value = b.branch_id, label = b.name })
                    .ToList();
                // Lấy statuses (giả sử là group)
                var statuses = GetTaskGroupsByTaskId(bundle.id)
                    ?.Select(g => new
                    {
                        id = g.id,
                        name = g.name,
                        title = g.name,
                        color = g.color,
                        sort_index = g.index,
                        task_count = 0, // cần bổ sung nếu có
                    })
                    .ToList();
                return new
                {
                    error_code = ResponseResultEnum.Success.Value(),
                    message = new[] { ResponseResultEnum.Success.Text() },
                    from_cache = (object)null,
                    data = new
                    {
                        id = bundle.id,
                        name = bundle.name,
                        title = bundle.name,
                        icon = bundle.icon,
                        is_favorite = bundle.favored,
                        default_view = bundle.view_id,
                        color = bundle.color,
                        updated_at = bundle.updated_at?.ToString("yyyy-MM-dd HH:mm:ss"),
                        managers = managers,
                        is_archived = bundle.is_archived,
                        created_user_obj = creator != null
                            ? new
                            {
                                id = creator.id,
                                name = creator.name,
                                username = creator.username,
                                identification = creator.identification,
                                branch_id = creator.branch_id?.ToString() ?? "",
                            }
                            : null,
                        description = "",
                        task_done = bundle.task_done,
                        task_count = bundle.task_count,
                        departments = departments,
                        positions = positions,
                        branches = branches,
                        users = users,
                        user_count = users?.Count ?? 0,
                        statuses = statuses,
                        section_id = 0,
                        section_obj = new { id = 0, name = "" },
                        my_tasks = new
                        {
                            task_count = 0,
                            task_done = 0,
                            task_overdue = 0,
                        },
                        default_deadline_time = bundle.default_deadline_time,
                        default_start_time = bundle.default_start_time,
                    },
                };
            }
            catch (Exception ex)
            {
                CommonLogger.DefaultLogger.Error("TaskBo.UpdateTaskBundle Error", ex);
                return new
                {
                    error_code = ResponseResultEnum.SystemError.Value(),
                    message = new[] { ResponseResultEnum.SystemError.Text() },
                    from_cache = (object)null,
                    data = new { },
                };
            }
        }

        /// <summary>
        /// Lấy chi tiết bundle (task) theo mẫu detail
        /// </summary>
        public object GetTaskBundleDetail(int bundleId, int companyId)
        {
            var bundle = GetTaskList(bundleId, companyId)?.FirstOrDefault();
            if (bundle == null)
            {
                return new
                {
                    error_code = ResponseResultEnum.NotFound.Value(),
                    message = new[] { "Không tìm thấy bundle." },
                    from_cache = (object)null,
                    data = new { },
                };
            }
            // Lấy managers
            var managers = GetTaskManagersByTask(bundle.id)
                ?.Select(m => new
                {
                    id = m.id?.ToString(),
                    name = m.name,
                    username = m.phone_number,
                    identification = m.department_id?.ToString() ?? "",
                    branch_id = m.branch_id?.ToString() ?? "",
                })
                .ToList();
            // Lấy users
            var users = GetTaskUsersByTask(bundle.id)
                ?.Select(u => new
                {
                    id = u.id?.ToString(),
                    user_id = u.id?.ToString(),
                    name = u.name,
                    username = u.username,
                    department_id = u.department_id?.ToString() ?? "",
                    branch_id = u.branch_id?.ToString() ?? "",
                })
                .ToList();
            // Lấy creator
            var creator = GetTaskCreatorInfo(bundle.id);
            // Lấy departments, positions, branches (giả sử có các hàm lấy này, nếu không có thì để rỗng)
            var departments = DaoFactory
                .Task.Ins_TaskDepartments_GetList_ByTaskId(bundle.id)
                ?.Select(d => new
                {
                    id = d.id,
                    name = d.name,
                    parent_id = d.parent_id,
                })
                .ToList();
            var positions = DaoFactory
                .Task.Ins_TaskPositions_GetList_ByTaskId(bundle.id)
                ?.Select(p => new { value = p.id, label = p.name })
                .ToList();
            var branches = DaoFactory
                .Task.Ins_TaskBranches_GetList_ByTaskId(bundle.id)
                ?.Select(b => new { value = b.branch_id, label = b.name })
                .ToList();
            // Lấy statuses (giả sử là group)
            var statuses = GetTaskGroupsByTaskId(bundle.id)
                ?.Select(g => new
                {
                    id = g.id,
                    name = g.name,
                    title = g.name,
                    color = g.color,
                    sort_index = g.index,
                    task_count = 0, // cần bổ sung nếu có
                })
                .ToList();
            return new
            {
                error_code = ResponseResultEnum.Success.Value(),
                message = new[] { ResponseResultEnum.Success.Text() },
                from_cache = (object)null,
                data = new
                {
                    id = bundle.id,
                    name = bundle.name,
                    title = bundle.name,
                    icon = bundle.icon,
                    is_favorite = bundle.favored,
                    default_view = bundle.view_id,
                    color = bundle.color,
                    updated_at = bundle.updated_at?.ToString("yyyy-MM-dd HH:mm:ss"),
                    managers = managers,
                    is_archived = bundle.is_archived,
                    created_user_obj = creator != null
                        ? new
                        {
                            id = creator.id,
                            name = creator.name,
                            username = creator.username,
                            identification = creator.identification,
                            branch_id = creator.branch_id?.ToString() ?? "",
                        }
                        : null,
                    description = "",
                    task_done = bundle.task_done,
                    task_count = bundle.task_count,
                    departments = departments,
                    positions = positions,
                    branches = branches,
                    users = users,
                    user_count = users?.Count ?? 0,
                    statuses = statuses,
                    my_tasks = new
                    {
                        task_count = 0,
                        task_done = 0,
                        task_overdue = 0,
                    },
                    default_deadline_time = bundle.default_deadline_time,
                    default_start_time = bundle.default_start_time,
                },
            };
        }

        /// <summary>
        /// Cập nhật status cho subtask khi kéo thả sang First/Last (gọi Ins_TaskSubTasks_UpdateSortIndexFirstOrlast)
        /// </summary>
        public object UpdateTaskSubStatusFirstOrLast(
            int subTaskId,
            int statusId,
            int sortIndex,
            string position
        )
        {
            try
            {
                var result = DaoFactory.Task.Ins_TaskSubTasks_UpdateSortIndexFirstOrlast(
                    subTaskId,
                    statusId,
                    sortIndex,
                    position
                );

                return new
                {
                    error_code = ResponseResultEnum.Success.Value(),
                    message = new[] { "Cập nhật status subtask thành công" },
                    from_cache = (object)null,
                    data = new { },
                };
            }
            catch (Exception ex)
            {
                CommonLogger.DefaultLogger.Error("TaskBo.UpdateTaskSubStatusFirstOrLast Error", ex);
                return new
                {
                    error_code = ResponseResultEnum.SystemError.Value(),
                    message = new[] { ResponseResultEnum.SystemError.Text() },
                    from_cache = (object)null,
                    data = new { },
                };
            }
        }

        public object UpdateTaskSubStatusMid(
            int subTaskId,
            int sortTaskId,
            int statusId,
            int index,
            string position
        )
        {
            try
            {
                // Lấy danh sách subtask của cả 2 group
                var list = DaoFactory.Task.Ins_TaskSubTasks_GetListIndex_ById(
                    subTaskId,
                    sortTaskId
                );

                if (list == null || list.Count == 0)
                {
                    return new
                    {
                        error_code = ResponseResultEnum.NotFound.Value(),
                        message = new[] { "Không tìm thấy subtask hoặc status" },
                        from_cache = (object)null,
                        data = new { },
                    };
                }

                var movingItem = list.FirstOrDefault(x => x.id == subTaskId);
                if (movingItem == null)
                {
                    return new
                    {
                        error_code = ResponseResultEnum.NotFound.Value(),
                        message = new[] { "Không tìm thấy subtask cần di chuyển" },
                        from_cache = (object)null,
                        data = new { },
                    };
                }

                int? currentBundleId = movingItem.bundle_id;
                int? targetBundleId = list.FirstOrDefault(x => x.id == statusId)?.bundle_id;

                // Xử lý khi di chuyển giữa 2 bundle khác nhau
                if (currentBundleId != targetBundleId)
                {
                    // Tách list thành 2 list theo bundle_id
                    var listCurrentBundle = list.Where(x => x.bundle_id == currentBundleId)
                        .ToList();
                    var listTargetBundle = list.Where(x => x.bundle_id == targetBundleId).ToList();

                    // Loại bỏ movingItem khỏi list hiện tại
                    listCurrentBundle = listCurrentBundle.Where(x => x.id != subTaskId).ToList();

                    // Cập nhật lại sort_index cho list hiện tại
                    for (int i = 0; i < listCurrentBundle.Count; i++)
                    {
                        listCurrentBundle[i].sort_index = i;
                        DaoFactory.Task.Ins_TaskSubTasks_UpdateSortIndexMid(
                            listCurrentBundle[i].id,
                            statusId,
                            listCurrentBundle[i].sort_index ?? 0
                        );
                    }

                    // Thêm movingItem vào list đích tại vị trí index
                    if (index < 0)
                        index = 0;
                    if (index > listTargetBundle.Count)
                        index = listTargetBundle.Count;

                    listTargetBundle.Insert(index, movingItem);
                    movingItem.bundle_id = targetBundleId;

                    // Cập nhật lại sort_index cho list đích
                    for (int i = 0; i < listTargetBundle.Count; i++)
                    {
                        listTargetBundle[i].sort_index = i;
                        DaoFactory.Task.Ins_TaskSubTasks_UpdateSortIndexMid(
                            listTargetBundle[i].id,
                            statusId,
                            listCurrentBundle[i].sort_index ?? 0
                        );
                    }
                }
                else
                {
                    // Di chuyển trong cùng 1 bundle
                    var filteredList = list.Where(x => x.id != subTaskId).ToList();

                    // Chèn movingItem vào vị trí index
                    if (index < 0)
                        index = 0;
                    if (index > filteredList.Count)
                        index = filteredList.Count;

                    filteredList.Insert(index, movingItem);

                    // Cập nhật lại sort_index cho toàn bộ list
                    for (int i = 0; i < filteredList.Count; i++)
                    {
                        filteredList[i].sort_index = i;
                        DaoFactory.Task.Ins_TaskSubTasks_UpdateSortIndexMid(
                            filteredList[i].id,
                            statusId,
                            filteredList[i].sort_index ?? 0
                        );
                    }
                }

                return new
                {
                    error_code = ResponseResultEnum.Success.Value(),
                    message = new[] { "Cập nhật vị trí subtask thành công." },
                    from_cache = (object)null,
                    data = new { },
                };
            }
            catch (Exception ex)
            {
                CommonLogger.DefaultLogger.Error("TaskBo.UpdateTaskSubStatusMid Error", ex);
                return new
                {
                    error_code = ResponseResultEnum.SystemError.Value(),
                    message = new[] { ResponseResultEnum.SystemError.Text() },
                    from_cache = (object)null,
                    data = new { },
                };
            }
        }

        /// <summary>
        /// Cập nhật icon và màu sắc cho bundle (dùng store Ins_Task_Update_Color_And_Name) và trả về response đầy đủ
        /// </summary>
        public object UpdateTaskBundleIconAndColor(
            int bundleId,
            string icon,
            string color,
            int companyId
        )
        {
            try
            {
                var result = DaoFactory.Task.Ins_Task_Update_Color_And_Name(bundleId, icon, color);
                // Lấy lại thông tin bundle mới nhất
                var bundle = GetTaskList(bundleId, companyId)?.FirstOrDefault();
                if (bundle == null)
                {
                    return new
                    {
                        error_code = ResponseResultEnum.SystemError.Value(),
                        message = new[] { ResponseResultEnum.SystemError.Text() },
                        from_cache = (object)null,
                        data = new { },
                    };
                }
                // Lấy managers
                var managers = GetTaskManagersByTask(bundle.id)
                    ?.Select(m => new
                    {
                        id = m.id?.ToString(),
                        name = m.name,
                        username = m.phone_number,
                        identification = m.department_id?.ToString() ?? "",
                        branch_id = m.branch_id?.ToString() ?? "",
                    })
                    .ToList();
                // Lấy users
                var users = GetTaskUsersByTask(bundle.id)
                    ?.Select(u => new
                    {
                        id = u.id?.ToString(),
                        user_id = u.id?.ToString(),
                        name = u.name,
                        username = u.username,
                        department_id = u.department_id?.ToString() ?? "",
                        branch_id = u.branch_id?.ToString() ?? "",
                    })
                    .ToList();
                // Lấy creator
                var creator = GetTaskCreatorInfo(bundle.id);
                return new
                {
                    error_code = ResponseResultEnum.Success.Value(),
                    message = new[] { ResponseResultEnum.Success.Text() },
                    from_cache = (object)null,
                    data = new
                    {
                        id = bundle.id,
                        name = bundle.name,
                        title = bundle.name,
                        icon = bundle.icon,
                        is_favorite = bundle.favored,
                        default_view = bundle.view_id,
                        color = bundle.color,
                        updated_at = bundle.updated_at?.ToString("yyyy-MM-dd HH:mm:ss"),
                        managers = managers,
                        is_archived = bundle.is_archived,
                        task_done = bundle.task_done,
                        task_count = bundle.task_count,
                        task_overdue = bundle.task_overdue,
                        users = users,
                        user_count = users?.Count ?? 0,
                        created_user_obj = creator != null
                            ? new
                            {
                                id = creator.id,
                                name = creator.name,
                                username = creator.username,
                                identification = creator.identification,
                                branch_id = creator.branch_id?.ToString() ?? "",
                            }
                            : null,
                    },
                };
            }
            catch (Exception ex)
            {
                CommonLogger.DefaultLogger.Error("TaskBo.UpdateTaskBundleIconAndColor Error", ex);
                return new
                {
                    error_code = ResponseResultEnum.SystemError.Value(),
                    message = new[] { ResponseResultEnum.SystemError.Text() },
                    from_cache = (object)null,
                    data = new { },
                };
            }
        }

        /// <summary>
        /// Tạo label cho bundle (dùng store Ins_Task_Label_CreateByBundleId)
        /// </summary>
        public object CreateTaskLabel(
            int bundleId,
            string name,
            string color,
            int? userId = null,
            int? sortIndex = 0,
            string title = null
        )
        {
            try
            {
                // Nếu title null hoặc rỗng thì lấy theo name
                if (string.IsNullOrEmpty(title))
                    title = name;
                var result = DaoFactory.Task.Ins_Task_Label_CreateByBundleId(
                    bundleId,
                    name,
                    color,
                    userId,
                    sortIndex,
                    title
                );
                if (result != null)
                {
                    return new
                    {
                        error_code = ResponseResultEnum.Success.Value(),
                        message = new[] { ResponseResultEnum.Success.Text() },
                        from_cache = (object)null,
                        data = result,
                    };
                }
                else
                {
                    CommonLogger.DefaultLogger.Error("TaskBo.CreateTaskLabel Error");

                    return new
                    {
                        error_code = ResponseResultEnum.SystemError.Value(),
                        message = new[] { ResponseResultEnum.SystemError.Text() },
                        from_cache = (object)null,
                        data = new { },
                    };
                }
            }
            catch (Exception ex)
            {
                CommonLogger.DefaultLogger.Error("TaskBo.CreateTaskLabel Error", ex);
                return new
                {
                    error_code = ResponseResultEnum.SystemError.Value(),
                    message = new[] { ResponseResultEnum.SystemError.Text() },
                    from_cache = (object)null,
                    data = new { },
                };
            }
        }

        /// <summary>
        /// Lấy danh sách label theo bundle_id (dùng store Ins_Task_Label_GetListByBundleId)
        /// </summary>
        public object GetTaskLabelList(int bundleId)
        {
            try
            {
                var labels =
                    DaoFactory.Task.Ins_Task_Label_GetListByBundleId(bundleId)
                    ?? new List<Ins_Task_Label_GetListByBundleId_Result>();
                var items = labels
                    .Select(l => new
                    {
                        id = l.id,
                        name = l.name,
                        color = l.color,
                        user_id = l.user_id,
                        sort_index = l.sort_index,
                        title = l.title,
                        bundle_id = l.bundle_id,
                    })
                    .ToList();
                return new
                {
                    error_code = ResponseResultEnum.Success.Value(),
                    message = new[] { ResponseResultEnum.Success.Text() },
                    from_cache = (object)null,
                    data = new { items, meta = new object[] { } },
                };
            }
            catch (Exception ex)
            {
                CommonLogger.DefaultLogger.Error("TaskBo.GetTaskLabelList Error", ex);
                return new
                {
                    error_code = ResponseResultEnum.SystemError.Value(),
                    message = new[] { ResponseResultEnum.SystemError.Text() },
                    from_cache = (object)null,
                    data = new { items = new object[] { }, meta = new object[] { } },
                };
            }
        }

        /// <summary>
        /// Lấy thông tin field theo ID (dùng store Ins_Task_Field_GetById)
        /// </summary>
        public List<Ins_Task_Field_GetById_Result> GetTaskFieldById(int fieldId, int objectId)
        {
            return DaoFactory.Task.GetTaskFieldById(fieldId, objectId);
        }

        /// <summary>
        /// Lấy danh sách task field theo type (dùng store Ins_Task_Field_GetByType)
        /// </summary>
        /// <param name="fieldType">Loại field</param>
        /// <returns>Danh sách task field</returns>
        public List<Ins_Task_Field_GetByType_Result> GetTaskFieldByType(int fieldType)
        {
            return DaoFactory.Task.GetTaskFieldByType(fieldType);
        }

        /// <summary>
        /// Lấy danh sách task field custom theo type (dùng store Ins_Task_Field_Custom_GetByType)
        /// </summary>
        /// <param name="fieldType">Loại field</param>
        /// <returns>Danh sách task field custom</returns>
        public List<Ins_Task_Field_Custom_GetByType_Result> GetTaskFieldCustomByType(int fieldType)
        {
            return DaoFactory.Task.GetTaskFieldCustomByType(fieldType);
        }

        /// <summary>
        /// Xóa task label theo ID (dùng store Ins_Task_Label_Delete)
        /// </summary>
        /// <param name="labelId">ID của label cần xóa</param>
        /// <returns>Kết quả xóa</returns>
        public int DeleteTaskLabel(int labelId)
        {
            return DaoFactory.Task.DeleteTaskLabel(labelId);
        }

        /// <summary>
        /// Cập nhật task label theo ID (dùng store Ins_Task_Label_Update)
        /// </summary>
        /// <param name="labelId">ID của label cần cập nhật</param>
        /// <param name="name">Tên mới của label</param>
        /// <param name="color">Màu sắc mới</param>
        /// <param name="bundleId">Bundle ID</param>
        /// <returns>Kết quả cập nhật</returns>
        public Ins_Task_Label_Update_Result UpdateTaskLabel(
            int labelId,
            string name,
            string color,
            int bundleId
        )
        {
            return DaoFactory.Task.UpdateTaskLabel(labelId, name, color, bundleId);
        }

        /// <summary>
        /// Cập nhật task field theo ID (dùng store Ins_Task_Field_Update)
        /// </summary>
        /// <param name="fieldId">ID của field cần cập nhật</param>
        /// <param name="title">Tiêu đề mới</param>
        /// <param name="titleNosign">Tiêu đề không dấu</param>
        /// <param name="addToLib">Thêm vào thư viện</param>
        /// <param name="notifyWhenValueChanged">Thông báo khi giá trị thay đổi</param>
        /// <param name="key">Key của field</param>
        /// <param name="isDefault">Có phải mặc định</param>
        /// <param name="sortIndex">Thứ tự sắp xếp</param>
        /// <param name="objectId">Object ID</param>
        /// <param name="active">Trạng thái hoạt động</param>
        /// <param name="onlyCreatedUserEdit">Chỉ user tạo mới được sửa</param>
        /// <param name="source">Nguồn</param>
        /// <returns>Kết quả cập nhật</returns>
        public Ins_Task_Field_Update_Result UpdateTaskField(
            int fieldId,
            string title,
            string description,
            bool add_to_library,
            bool notify_on_change
        )
        {
            return DaoFactory.Task.UpdateTaskField(
                fieldId,
                title,
                description,
                add_to_library,
                notify_on_change
            );
        }

        /// <summary>
        /// Cập nhật task field option theo ID (dùng store Ins_Task_Field_Option_Update)
        /// </summary>
        /// <param name="optionId">ID của option cần cập nhật</param>
        /// <param name="title">Tiêu đề mới</param>
        /// <param name="color">Màu sắc mới</param>
        /// <param name="sortIndex">Thứ tự sắp xếp</param>
        /// <param name="titleNosign">Tiêu đề không dấu</param>
        /// <param name="actionOption">Hành động option</param>
        /// <returns>Kết quả cập nhật</returns>
        public int UpdateTaskFieldOption(
            int optionId,
            string title,
            string color,
            int sortIndex,
            string titleNosign,
            string actionOption
        )
        {
            return DaoFactory.Task.UpdateTaskFieldOption(
                optionId,
                title,
                color,
                sortIndex,
                titleNosign,
                actionOption
            );
        }

        /// <summary>
        /// Xóa task field theo ID (dùng store Ins_TaskField_Delete)
        /// </summary>
        /// <param name="fieldId">ID của field cần xóa</param>
        /// <param name="source">Nguồn</param>
        /// <param name="objectId">Object ID</param>
        /// <returns>Kết quả xóa</returns>
        public int DeleteTaskField(int field_value_Id, string source, int? objectId)
        {
            return DaoFactory.Task.DeleteTaskField(field_value_Id, source, objectId);
        }

        /// <summary>
        /// Lấy danh sách task fields theo source và object_id (dùng store Ins_Task_Field_GetAll)
        /// </summary>
        /// <param name="source">Nguồn</param>
        /// <param name="objectId">Object ID</param>
        /// <returns>Danh sách task fields</returns>
        public List<Ins_Task_Field_GetAll_Result> GetTaskFieldList(string source, int objectId)
        {
            return DaoFactory.Task.GetTaskFieldList(source, objectId);
        }

        /// <summary>
        /// Lấy danh sách task fields theo source và object_id (dùng store Ins_Task_Field_GetAll)
        /// </summary>
        /// <param name="source">Nguồn</param>
        /// <param name="objectId">Object ID</param>
        /// <returns>Danh sách task fields</returns>
        public List<Ins_Task_Field_GetType_Result> GetTaskFieldListByType(
            string source,
            int objectId,
            int type
        )
        {
            return DaoFactory.Task.GetTaskFieldListByType(source, objectId, type);
        }

        /// <summary>
        /// Thêm field vào subtask theo type
        /// </summary>
        /// <param name="fieldType">Loại field</param>
        /// <param name="subTaskId">ID của subtask</param>
        /// <returns>Số lượng record được thêm</returns>
        public int Ins_Task_Sub_Field_Add_ByType(int fieldType, int subTaskId)
        {
            return DaoFactory.Task.Ins_Task_Sub_Field_Add_ByType(fieldType, subTaskId);
        }

        /// <summary>
        /// Thêm field vào subtask theo ID
        /// </summary>
        /// <param name="taskId">ID của task</param>
        /// <param name="fieldId">ID của field</param>
        /// <returns>Số lượng record được thêm</returns>
        public int Ins_Task_Sub_Field_Add_ById(int taskId, int fieldId)
        {
            return DaoFactory.Task.Ins_Task_Sub_Field_Add_ById(taskId, fieldId);
        }

        /// <summary>
        /// Thêm field vào task theo type
        /// </summary>
        /// <param name="fieldType">Loại field</param>
        /// <param name="objectId">ID của object (task)</param>
        /// <returns>Số lượng record được thêm</returns>
        public int Ins_Task_Field_Add_ByType(int fieldType, int objectId)
        {
            return DaoFactory.Task.Ins_Task_Field_Add_ByType(fieldType, objectId);
        }

        /// <summary>
        /// Thêm field vào task theo ID
        /// </summary>
        /// <param name="fieldId">ID của field</param>
        /// <param name="objectId">ID của object (task)</param>
        /// <returns>Số lượng record được thêm</returns>
        public int Ins_Task_Field_Add_ById(int fieldId, int objectId, bool active)
        {
            return DaoFactory.Task.Ins_Task_Field_Add_ById(fieldId, objectId, active);
        }

        /// <summary>
        /// Bật/tắt field value
        /// </summary>
        /// <param name="id">ID của field value</param>
        /// <param name="active">Trạng thái active</param>
        /// <returns>Số lượng record được cập nhật</returns>
        public int TurnOnOffTaskFieldValue(int id, bool active)
        {
            return DaoFactory.Task.TurnOnOffTaskFieldValue(id, active);
        }

        /// <summary>
        /// Lấy danh sách task fields và trả về response format chuẩn
        /// </summary>
        /// <param name="source">Nguồn</param>
        /// <param name="objectId">Object ID</param>
        /// <returns>Response format chuẩn</returns>
        public object GetTaskFieldListResponse(string source, int objectId)
        {
            try
            {
                if (string.IsNullOrEmpty(source))
                {
                    return new
                    {
                        error_code = ResponseResultEnum.InvalidInput.Value(),
                        message = new[] { "Source không được để trống." },
                        from_cache = (object)null,
                        data = new { },
                    };
                }

                var fieldList = GetTaskFieldList(source, objectId);
                if (fieldList == null || !fieldList.Any())
                {
                    return new
                    {
                        error_code = ResponseResultEnum.Success.Value(),
                        message = new[] { "Không có task fields nào." },
                        from_cache = (object)null,
                        data = new List<object>(),
                    };
                }

                // Group fields by ID và build response
                var groupedFields = fieldList.GroupBy(f => f.field_id).ToList();
                var responseData = new List<object>();
                foreach (var group in groupedFields)
                {
                    var field = group.First();

                    // Build objects dictionary từ field data
                    var objects = new Dictionary<string, object>();
                    var options = fieldList
                        .Where(f => f.field_id == field.field_id)
                        .Select(f => new
                        {
                            id = f.option_id,
                            title = f.option_title,
                            title_nosign = f.option_title,
                            color = f.option_color,
                            sort_index = f.option_index,
                        })
                        .ToList();
                    var fieldData = new
                    {
                        id = field.value_field_id,
                        title = field.field_title,
                        title_nosign = field.field_title,
                        add_to_lib = field.add_to_library,
                        notify_when_value_changed = field.notify_on_change,
                        key = field.field_type != null
                            ? System
                                .Enum.GetName(typeof(TaskFieldElementTypeEnum), field.field_type)
                                .ToLower()
                            : System.Enum.GetName(typeof(TaskFieldEnum), field.key).ToLower(),
                        is_default = field.is_default,
                        options = options.Count > 0
                            ? options.Cast<object>().ToList()
                            : new List<object>(),
                        used_objects = 0,
                        sort_index = 0,
                        objects = objects,
                        object_id = 0,
                        active = field.active,
                    };

                    responseData.Add(fieldData);
                }

                return new
                {
                    error_code = ResponseResultEnum.Success.Value(),
                    message = new[] { "Lấy danh sách task fields thành công." },
                    from_cache = (object)null,
                    data = responseData,
                };
            }
            catch (Exception ex)
            {
                CommonLogger.DefaultLogger.Error("TaskBo.GetTaskFieldListResponse Error", ex);
                return new
                {
                    error_code = ResponseResultEnum.SystemError.Value(),
                    message = new[] { "Đã xảy ra lỗi hệ thống." },
                    from_cache = (object)null,
                    data = new { },
                };
            }
        }

        /// <summary>
        /// Lấy danh sách task fields và trả về response format chuẩn
        /// </summary>
        /// <param name="source">Nguồn</param>
        /// <param name="objectId">Object ID</param>
        /// <returns>Response format chuẩn</returns>
        public object GetTaskFieldCustomListResponse(string key, int objectId)
        {
            try
            {
                var fieldType = System.Enum.TryParse<TaskFieldElementTypeEnum>(
                    key,
                    out var parsedType
                )
                    ? parsedType
                    : TaskFieldElementTypeEnum.text;

                var fieldList = GetTaskFieldListByType("", objectId, (int)fieldType);
                if (fieldList == null || !fieldList.Any())
                {
                    return new
                    {
                        error_code = ResponseResultEnum.Success.Value(),
                        message = new[] { "Không có task fields nào." },
                        from_cache = (object)null,
                        data = new List<object>(),
                    };
                }

                // Group fields by ID và build response
                var groupedFields = fieldList.GroupBy(f => f.field_id).ToList();
                var responseData = new List<object>();
                foreach (var group in groupedFields)
                {
                    var field = group.First();

                    // Build objects dictionary từ field data
                    var objects = new Dictionary<string, object>();
                    var options = fieldList
                        .Where(f => f.field_id == field.field_id)
                        .Select(f => new
                        {
                            id = f.option_id,
                            title = f.option_title,
                            title_nosign = f.option_title,
                            color = f.option_color,
                            sort_index = f.option_index,
                        })
                        .ToList();
                    var fieldData = new
                    {
                        id = field.value_field_id,
                        title = field.field_title,
                        title_nosign = field.field_title,
                        add_to_lib = field.add_to_library,
                        notify_when_value_changed = field.notify_on_change,
                        key = field.field_type != null
                            ? System
                                .Enum.GetName(typeof(TaskFieldElementTypeEnum), field.field_type)
                                .ToLower()
                            : System.Enum.GetName(typeof(TaskFieldEnum), field.key).ToLower(),
                        is_default = field.is_default,
                        options = options.Count > 0
                            ? options.Cast<object>().ToList()
                            : new List<object>(),
                        used_objects = 0,
                        sort_index = 0,
                        objects = objects,
                        object_id = 0,
                        active = field.active,
                    };

                    responseData.Add(fieldData);
                }

                return new
                {
                    error_code = ResponseResultEnum.Success.Value(),
                    message = new[] { "Lấy danh sách task fields thành công." },
                    from_cache = (object)null,
                    data = responseData,
                };
            }
            catch (Exception ex)
            {
                CommonLogger.DefaultLogger.Error("TaskBo.GetTaskFieldListResponse Error", ex);
                return new
                {
                    error_code = ResponseResultEnum.SystemError.Value(),
                    message = new[] { "Đã xảy ra lỗi hệ thống." },
                    from_cache = (object)null,
                    data = new { },
                };
            }
        }

        /// <summary>
        /// Xóa task field và trả về response format chuẩn
        /// </summary>
        /// <param name="fieldId">ID của field cần xóa</param>
        /// <param name="source">Nguồn</param>
        /// <param name="objectId">Object ID</param>
        /// <returns>Response format chuẩn</returns>
        public object DeleteTaskFieldResponse(int field_value_Id, string source, int? objectId)
        {
            try
            {
                if (field_value_Id <= 0)
                {
                    return new
                    {
                        error_code = ResponseResultEnum.InvalidInput.Value(),
                        message = new[] { "Field ID không hợp lệ." },
                        from_cache = (object)null,
                        data = new { },
                    };
                }

                var result = DeleteTaskField(field_value_Id, source, objectId);

                return new
                {
                    error_code = ResponseResultEnum.Success.Value(),
                    message = new[] { "Xóa task field thành công." },
                    from_cache = (object)null,
                };
            }
            catch (System.Data.Entity.Core.EntityCommandExecutionException entityEx)
            {
                if (
                    entityEx.InnerException != null
                    && entityEx.InnerException is System.Data.SqlClient.SqlException sqlEx
                )
                {
                    return new
                    {
                        error_code = ResponseResultEnum.Failed.Value(),
                        message = new[] { sqlEx.Message },
                        from_cache = (object)null,
                        data = new { },
                    };
                }
                else
                {
                    CommonLogger.DefaultLogger.Error(
                        "TaskBo.DeleteTaskFieldResponse Error",
                        entityEx
                    );
                    return new
                    {
                        error_code = ResponseResultEnum.SystemError.Value(),
                        message = new[] { "Đã xảy ra lỗi hệ thống." },
                        from_cache = (object)null,
                        data = new { },
                    };
                }
            }
            catch (Exception ex)
            {
                CommonLogger.DefaultLogger.Error("TaskBo.DeleteTaskFieldResponse Error", ex);
                return new
                {
                    error_code = ResponseResultEnum.SystemError.Value(),
                    message = new[] { "Đã xảy ra lỗi hệ thống." },
                    from_cache = (object)null,
                    data = new { },
                };
            }
        }

        /// <summary>
        /// Cập nhật task field và trả về response format chuẩn
        /// </summary>
        /// <param name="fieldId">ID của field cần cập nhật</param>
        /// <param name="title">Tiêu đề mới</param>
        /// <param name="titleNosign">Tiêu đề không dấu</param>
        /// <param name="addToLib">Thêm vào thư viện</param>
        /// <param name="notifyWhenValueChanged">Thông báo khi giá trị thay đổi</param>
        /// <param name="key">Key của field</param>
        /// <param name="isDefault">Có phải mặc định</param>
        /// <param name="sortIndex">Thứ tự sắp xếp</param>
        /// <param name="objectId">Object ID</param>
        /// <param name="active">Trạng thái hoạt động</param>
        /// <param name="onlyCreatedUserEdit">Chỉ user tạo mới được sửa</param>
        /// <param name="source">Nguồn</param>
        /// <param name="updateOption">Có cập nhật options không</param>
        /// <param name="options">Danh sách options cần cập nhật</param>
        /// <param name="objects">Objects</param>
        /// <returns>Response format chuẩn</returns>
        public object UpdateTaskFieldResponse(
            int fieldId,
            string title,
            string titleNosign,
            bool addToLib,
            bool notifyWhenValueChanged,
            string key,
            bool? isDefault,
            int sortIndex,
            int objectId,
            bool active,
            bool onlyCreatedUserEdit,
            string source,
            bool updateOption,
            List<object> options = null,
            Dictionary<string, object> objects = null
        )
        {
            try
            {
                if (fieldId <= 0)
                {
                    return new
                    {
                        error_code = ResponseResultEnum.InvalidInput.Value(),
                        message = new[] { "Field ID không hợp lệ." },
                        from_cache = (object)null,
                        data = new { },
                    };
                }

                if (string.IsNullOrEmpty(title))
                {
                    return new
                    {
                        error_code = ResponseResultEnum.InvalidInput.Value(),
                        message = new[] { "Tiêu đề không được để trống." },
                        from_cache = (object)null,
                        data = new { },
                    };
                }

                if (string.IsNullOrEmpty(key))
                {
                    return new
                    {
                        error_code = ResponseResultEnum.InvalidInput.Value(),
                        message = new[] { "Key không được để trống." },
                        from_cache = (object)null,
                        data = new { },
                    };
                }
                string description = "";
                // Cập nhật field
                var fieldResult = UpdateTaskField(
                    fieldId,
                    title,
                    description,
                    addToLib,
                    notifyWhenValueChanged
                );

                if (fieldResult == null)
                {
                    return new
                    {
                        error_code = ResponseResultEnum.NotFound.Value(),
                        message = new[] { "Không tìm thấy field hoặc cập nhật thất bại." },
                        from_cache = (object)null,
                        data = new { },
                    };
                }
                var optionId = "";
                var optionTitle = "";
                var optionTitleNosign = "";
                // Cập nhật options nếu có
                var responseOptions = new List<object>();
                if (updateOption && options != null && options.Any())
                {
                    foreach (var option in options)
                    {
                        optionId = PropertyHelper.GetPropertyValue<string>(option, "id");
                        optionTitle = PropertyHelper.GetPropertyValue<string>(option, "title");
                        var optionColor = PropertyHelper.GetPropertyValue<string>(option, "color");
                        var optionSortIndex = PropertyHelper.GetPropertyValue<int>(
                            option,
                            "sort_index"
                        );
                        optionTitleNosign = PropertyHelper.GetPropertyValue<string>(
                            option,
                            "title_nosign"
                        );
                        var optionActionOption = PropertyHelper.GetPropertyValue<string>(
                            option,
                            "action_option"
                        );

                        if (
                            !string.IsNullOrEmpty(optionId)
                            && int.TryParse(optionId, out int optionIdInt)
                        )
                        {
                            var optionResult = UpdateTaskFieldOption(
                                optionIdInt,
                                optionTitle,
                                optionColor,
                                optionSortIndex,
                                optionTitleNosign ?? optionTitle,
                                optionActionOption ?? ""
                            );
                        }
                    }
                }

                var fieldInfoList = GetTaskFieldById(fieldResult.field_id, objectId);
                if (fieldInfoList == null || !fieldInfoList.Any())
                {
                    return new
                    {
                        error_code = ResponseResultEnum.NotFound.Value(),
                        message = new[] { "Không tìm thấy thông tin field" },
                        from_cache = (object)null,
                        data = new { },
                    };
                }

                // Xử lý options từ DB
                responseOptions = new List<object>();
                if (fieldInfoList != null && fieldInfoList.Any())
                {
                    foreach (var option1 in fieldInfoList)
                    {
                        if (
                            option1.option_id.HasValue
                            && !string.IsNullOrEmpty(option1.option_title)
                        )
                        {
                            responseOptions.Add(
                                new
                                {
                                    id = option1.option_id,
                                    title = option1.option_title,
                                    title_nosign = option1.option_title,
                                    color = option1.option_color ?? "",
                                    sort_index = option1.option_sort_index ?? 0,
                                }
                            );
                        }
                    }
                }
                var optionById = responseOptions
                    .Where(o => PropertyHelper.GetPropertyValue<string>(o, "id") != null)
                    .ToDictionary(o => PropertyHelper.GetPropertyValue<string>(o, "id"), o => o);
                var optionByTitleNosign = responseOptions
                    .Where(o => PropertyHelper.GetPropertyValue<string>(o, "title_nosign") != null)
                    .ToDictionary(
                        o => PropertyHelper.GetPropertyValue<string>(o, "title_nosign"),
                        o => o
                    );
                var fieldInfo = fieldInfoList.FirstOrDefault();
                // Build response data
                var responseData = new
                {
                    id = fieldResult.field_value_id,
                    title = fieldInfo?.field_title ?? title,
                    title_nosign = fieldInfo?.field_title ?? title,
                    add_to_lib = fieldInfo?.add_to_library ?? addToLib ? 1 : 0,
                    notify_when_value_changed = fieldInfo?.notify_on_change
                    ?? notifyWhenValueChanged
                        ? 1
                        : 0,
                    key = fieldInfo?.field_type != null
                        ? System
                            .Enum.GetName(typeof(TaskFieldElementTypeEnum), fieldInfo.field_type)
                            .ToLower()
                        : "",
                    is_default = fieldInfo.is_default,
                    used_objects = 1,
                    sort_index = sortIndex,
                    objects = objects ?? new Dictionary<string, object>(),
                    object_id = objectId,
                    active = fieldInfo.active,
                    options = responseOptions,
                    option_by_id = optionById,
                    option_by_title_nosign = optionByTitleNosign,
                    only_created_user_edit = onlyCreatedUserEdit ? 1 : 0,
                    source = source,
                    update_option = updateOption ? 1 : 0,
                };

                return new
                {
                    error_code = ResponseResultEnum.Success.Value(),
                    message = new[] { "Cập nhật field thành công." },
                    from_cache = (object)null,
                    data = responseData,
                };
            }
            catch (Exception ex)
            {
                CommonLogger.DefaultLogger.Error("UpdateTaskFieldResponse Error", ex);
                return new
                {
                    error_code = ResponseResultEnum.SystemError.Value(),
                    message = new[] { "Đã xảy ra lỗi hệ thống." },
                    from_cache = (object)null,
                    data = new { },
                };
            }
        }

        /// <summary>
        /// Cập nhật task label và trả về response format chuẩn
        /// </summary>
        /// <param name="labelId">ID của label cần cập nhật</param>
        /// <param name="name">Tên mới của label</param>
        /// <param name="color">Màu sắc mới</param>
        /// <param name="bundleId">Bundle ID</param>
        /// <returns>Response format chuẩn</returns>
        public object UpdateTaskLabelResponse(int labelId, string name, string color, int bundleId)
        {
            try
            {
                if (labelId <= 0)
                {
                    return new
                    {
                        error_code = ResponseResultEnum.InvalidInput.Value(),
                        message = new[] { "Label ID không hợp lệ." },
                        from_cache = (object)null,
                        data = new { },
                    };
                }

                if (string.IsNullOrEmpty(name))
                {
                    return new
                    {
                        error_code = ResponseResultEnum.InvalidInput.Value(),
                        message = new[] { "Tên label không được để trống." },
                        from_cache = (object)null,
                        data = new { },
                    };
                }

                if (string.IsNullOrEmpty(color))
                {
                    return new
                    {
                        error_code = ResponseResultEnum.InvalidInput.Value(),
                        message = new[] { "Màu sắc không được để trống." },
                        from_cache = (object)null,
                        data = new { },
                    };
                }

                if (bundleId <= 0)
                {
                    return new
                    {
                        error_code = ResponseResultEnum.InvalidInput.Value(),
                        message = new[] { "Bundle ID không hợp lệ." },
                        from_cache = (object)null,
                        data = new { },
                    };
                }

                var result = UpdateTaskLabel(labelId, name, color, bundleId);

                return new
                {
                    error_code = ResponseResultEnum.Success.Value(),
                    message = new[] { "Cập nhật label thành công." },
                    from_cache = (object)null
                };
            }
            catch (Exception ex)
            {
                CommonLogger.DefaultLogger.Error("UpdateTaskLabelResponse Error", ex);
                return new
                {
                    error_code = ResponseResultEnum.SystemError.Value(),
                    message = new[] { "Đã xảy ra lỗi hệ thống." },
                    from_cache = (object)null,
                    data = new { },
                };
            }
        }

        /// <summary>
        /// Tạo task field với options và trả về response đầy đủ
        /// </summary>
        public object CreateTaskFieldWithOptions(
            int objectId,
            string title,
            string key,
            bool addToLib,
            bool notifyWhenValueChanged,
            string source,
            List<object> options = null
        )
        {
            try
            {
                // Parse key thành enum type
                var fieldType = System.Enum.TryParse<TaskFieldElementTypeEnum>(
                    key,
                    out var parsedType
                )
                    ? parsedType
                    : TaskFieldElementTypeEnum.text;

                // Tạo field
                var fieldResult = CreateTaskField(
                    objectId,
                    title,
                    (int)fieldType,
                    addToLib,
                    notifyWhenValueChanged,
                    0, // only_created_user_edit
                    1, // active
                    source ?? "",
                    false
                );

                if (fieldResult == null)
                    return new
                    {
                        error_code = ResponseResultEnum.SystemError.Value(),
                        message = new[] { "Tạo thuộc tính thất bại" },
                        from_cache = (object)null,
                        data = new { },
                    };

                // Lấy field ID
                var fieldId = PropertyHelper.GetPropertyValue<int>(fieldResult, "id");
                if (fieldType == TaskFieldElementTypeEnum.dropdown)
                {
                    // Tạo options nếu có
                    if (options != null && options.Count > 0)
                    {
                        foreach (var opt in options)
                        {
                            var optTitle = PropertyHelper.GetPropertyValue<string>(opt, "title");
                            var optColor = PropertyHelper.GetPropertyValue<string>(opt, "color");
                            var optSortIndex = PropertyHelper.GetPropertyValue<int>(
                                opt,
                                "sort_index"
                            );

                            // Tự động sinh title_nosign và alias từ title
                            var titleNosign = MyUtility.Extensions.StringExtension.ConvertToUnSign(
                                optTitle
                            );
                            var alias = titleNosign.Replace("-", "_");

                            CreateTaskFieldOption(fieldId, optTitle, optColor, optSortIndex, alias);
                        }
                    }
                }
                // Field đã được tạo thành công
                if (fieldId > 0)
                {
                    // Field đã được liên kết với task
                    Ins_Task_Field_Add_ById(fieldId, objectId, false);
                }

                // Lấy thông tin field đã tạo
                var fieldInfoList = GetTaskFieldById(fieldId, objectId);
                if (fieldInfoList == null || !fieldInfoList.Any())
                {
                    return new
                    {
                        error_code = ResponseResultEnum.NotFound.Value(),
                        message = new[] { "Không tìm thấy thông tin field" },
                        from_cache = (object)null,
                        data = new { },
                    };
                }

                // Xử lý options từ DB
                var responseOptions = new List<object>();
                if (fieldInfoList != null && fieldInfoList.Any())
                {
                    foreach (var option in fieldInfoList)
                    {
                        if (option.option_id.HasValue && !string.IsNullOrEmpty(option.option_title))
                        {
                            responseOptions.Add(
                                new
                                {
                                    id = option.option_id.ToString(),
                                    title = option.option_title,
                                    title_nosign = option.option_title,
                                    color = option.option_color ?? "",
                                    sort_index = option.option_sort_index ?? 0,
                                }
                            );
                        }
                    }
                }

                // Nếu không có options từ DB, sử dụng options từ request
                if (!responseOptions.Any() && options != null && options.Count > 0)
                {
                    foreach (var opt in options)
                    {
                        responseOptions.Add(
                            new
                            {
                                id = PropertyHelper.GetPropertyValue<string>(opt, "id"),
                                title = PropertyHelper.GetPropertyValue<string>(opt, "title"),
                                title_nosign = PropertyHelper.GetPropertyValue<string>(
                                    opt,
                                    "title_nosign"
                                ),
                                color = PropertyHelper.GetPropertyValue<string>(opt, "color"),
                                sort_index = PropertyHelper.GetPropertyValue<int>(
                                    opt,
                                    "sort_index"
                                ),
                            }
                        );
                    }
                }

                var optionById = responseOptions.ToDictionary(
                    o => PropertyHelper.GetPropertyValue<string>(o, "id"),
                    o => o
                );
                var optionByTitleNosign = responseOptions.ToDictionary(
                    o => PropertyHelper.GetPropertyValue<string>(o, "title_nosign"),
                    o => o
                );

                // Build response
                var fieldInfo = fieldInfoList.FirstOrDefault();
                var responseData = new
                {
                    id = fieldId,
                    title = fieldInfo?.field_title ?? title,
                    title_nosign = fieldInfo?.field_title ?? title,
                    add_to_lib = fieldInfo?.add_to_library ?? addToLib,
                    notify_when_value_changed = fieldInfo?.notify_on_change
                        ?? notifyWhenValueChanged,
                    key = fieldInfo.field_type != null
                        ? System
                            .Enum.GetName(typeof(TaskFieldElementTypeEnum), fieldInfo.field_type)
                            .ToLower()
                        : "",
                    is_default = fieldInfo.is_default,
                    used_objects = 0,
                    sort_index = 0,
                    objects = "",
                    object_id = objectId,
                    active = fieldInfo.active,
                    options = responseOptions,
                    option_by_id = optionById,
                    option_by_title_nosign = optionByTitleNosign,
                    source = source,
                };

                return new
                {
                    error_code = ResponseResultEnum.Success.Value(),
                    message = new[] { ResponseResultEnum.Success.Text() },
                    from_cache = (object)null,
                    data = responseData,
                };
            }
            catch (Exception ex)
            {
                CommonLogger.DefaultLogger.Error("TaskBo.CreateTaskFieldWithOptions Error", ex);
                return new
                {
                    error_code = ResponseResultEnum.SystemError.Value(),
                    message = new[] { ResponseResultEnum.SystemError.Text() },
                    from_cache = (object)null,
                    data = new { },
                };
            }
        }

        /// <summary>
        /// Gán label cho task (dùng store Ins_Task_Label_Value_Add) và trả về detail task
        /// </summary>
        public object SetTaskLabel(int taskId, List<int> labelIds, int companyId, int userId)
        {
            try
            {
                if (taskId <= 0)
                {
                    return new
                    {
                        error_code = ResponseResultEnum.InvalidInput.Value(),
                        message = new[] { ResponseResultEnum.InvalidInput.Text() },
                        from_cache = (object)null,
                        data = new { },
                    };
                }
                // Xóa tất cả label cũ của task
                DaoFactory.Task.Ins_Task_Label_Value_DeleteByBundleId(taskId);
                // Thêm label mới
                if (labelIds != null && labelIds.Count > 0)
                {
                    string labelNames = "";

                    foreach (var labelId in labelIds)
                    {
                        DaoFactory.Task.Ins_Task_Label_Value_Add(labelId, taskId);

                        var taskLabelDetail = DaoFactory.Task.GetTaskLabelDetail(labelId);
                        labelNames += taskLabelDetail.name + ", ";
                    }

                    if (labelNames != "") {
                        labelNames = labelNames.Substring(0, labelNames.Length - 2);
                    }

                    var taskCommentResult = DaoFactory.Comment.CreateTaskComment(
                    taskId, 
                    userId, 
                    "{0} " + "đã thay đổi nhãn sang" + " {1}", 
                    "task",
                    false,
                    false,
                    "set_attribute",
                    "label_ids"
                    );

                    DaoFactory.Comment.AddTaskCommentMention(taskCommentResult.Id.Value, userId, null);
                    DaoFactory.Comment.AddTaskCommentMention(taskCommentResult.Id.Value, null, labelNames);
                }

                if (labelIds == null || labelIds.Count == 0) {
                    var taskCommentResult = DaoFactory.Comment.CreateTaskComment(
                    taskId, 
                    userId, 
                    "{0} " + "đã bỏ nhãn", 
                    "task",
                    false,
                    false,
                    "set_attribute",
                    "label_ids"
                    );

                    DaoFactory.Comment.AddTaskCommentMention(taskCommentResult.Id.Value, userId, null);
                }


                // Trả về detail task giống GetTaskDetailById
                return GetTaskDetailById(taskId, companyId);
            }
            catch (Exception ex)
            {
                CommonLogger.DefaultLogger.Error("TaskBo.SetTaskLabel Error", ex);
                return new
                {
                    error_code = ResponseResultEnum.SystemError.Value(),
                    message = new[] { ResponseResultEnum.SystemError.Text() },
                    from_cache = (object)null,
                    data = new { },
                };
            }
        }

        /// <summary>
        /// Xóa task label theo ID (dùng store Ins_Task_Label_Delete)
        /// </summary>
        /// <param name="labelId">ID của label cần xóa</param>
        /// <returns>Response object với kết quả xóa</returns>
        public object DeleteTaskLabelResponse(int labelId)
        {
            try
            {
                if (labelId <= 0)
                {
                    return new
                    {
                        error_code = ResponseResultEnum.InvalidInput.Value(),
                        message = new[] { "Label ID không hợp lệ." },
                        from_cache = (object)null,
                        data = new { },
                    };
                }

                var result = DeleteTaskLabel(labelId);

                if (result != null)
                {
                    return new
                    {
                        error_code = ResponseResultEnum.Success.Value(),
                        message = new[] { "Thành công." },
                        from_cache = (object)null,
                        data = "Thành công.",
                    };
                }
                else
                {
                    return new
                    {
                        error_code = ResponseResultEnum.NotFound.Value(),
                        message = new[] { "Không tìm thấy label hoặc xóa thất bại." },
                        from_cache = (object)null,
                        data = new { },
                    };
                }
            }
            catch (System.Data.Entity.Core.EntityCommandExecutionException entityEx)
            {
                // Handle Entity Framework exceptions
                if (
                    entityEx.InnerException != null
                    && entityEx.InnerException is System.Data.SqlClient.SqlException sqlEx
                )
                {
                    return new
                    {
                        error_code = ResponseResultEnum.Failed.Value(),
                        message = new[] { sqlEx.Message },
                        from_cache = (object)null,
                        data = new { },
                    };
                }
                else
                {
                    CommonLogger.DefaultLogger.Error(
                        "TaskBo.DeleteTaskLabelResponse Error",
                        entityEx
                    );
                    return new
                    {
                        error_code = ResponseResultEnum.SystemError.Value(),
                        message = new[] { ResponseResultEnum.SystemError.Text() },
                        from_cache = (object)null,
                        data = new { },
                    };
                }
            }
            catch (Exception ex)
            {
                CommonLogger.DefaultLogger.Error("TaskBo.DeleteTaskLabelResponse Error", ex);
                return new
                {
                    error_code = ResponseResultEnum.SystemError.Value(),
                    message = new[] { ResponseResultEnum.SystemError.Text() },
                    from_cache = (object)null,
                    data = new { },
                };
            }
        }
    }
}
