using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess.EF;
using DataAccess.Interface;
using EntitiesObject.Entities.TanTamEntities;

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

        Ins_Tasks_Create_Result CreateTask(
            string title,
            int createdUserObj,
            int companyId,
            string defaultView,
            string color
        );

        List<Ins_Task_GetTaskGroupsByTaskId_Result> GetTaskGroupsByTaskId(int taskId);

        List<Ins_Task_ManagersByTask_Result> GetTaskManagersByTask(int taskId);

        List<Ins_Task_UsersByTask_Result> GetTaskUsersByTask(int taskId);

        List<Ins_Task_UsersByUser_Result> GetTaskUsersByUser(int userId);

        Ins_Task_CreatorInfo_Result GetTaskCreatorInfo(int taskId);

        Ins_Task_Group_Create_Result CreateTaskGroup(
            int bundleId,
            string name,
            string color,
            string position
        );

        Ins_Task_Group_Update_Name_Result UpdateTaskGroupName(int groupId, string name);

        Ins_Task_Group_Update_Color_Result UpdateTaskGroupColor(int groupId, string color);

        Ins_Task_Group_Update_Color_And_Name_Result UpdateTaskGroupColorAndName(
            int groupId,
            string name,
            string color
        );

        Ins_Task_Sub_Create_Result CreateTaskSub(
            string title,
            string alias,
            int? bundleId,
            int? createdUserId,
            int? assignedId,
            string position,
            DateTime? deadline
        );

        List<Ins_Task_Sub_ListByBundle_Result> GetTaskSubListByBundle(int bundleId);

        Ins_Task_Sub_Update_Completed_Result UpdateTaskSubCompleted(int id, bool isCompleted);

        Ins_Task_Sub_Update_Deadline_Result UpdateTaskSubDeadline(
            int id,
            DateTime? deadline,
            DateTime? startDate
        );

        Ins_Task_Update_AssignedUser_Result UpdateTaskAssignedUser(
            int taskId,
            int? assignedUser
        );

        int GetSubFieldValueByTitle(int subtaskId, string title = null);

        void InsertTaskFieldOptionsBulk(int fieldId, string options);

        Ins_Task_Sub_Update_Title_Result UpdateSubTaskTitle(
            int id,
            string title,
            string titleNosign,
            string alias
        );

        int DeleteAllRelations(int taskId);

        int DeleteAllFields(int taskId);

        int DeleteAllSubTasksByTaskId(int taskId);

        int DeleteAllGroupsByTaskId(int taskId);

        List<Ins_Tasks_Delete_Main_Result> DeleteMainTask(int taskId, int companyId);

        Ins_TaskBranches_Create_Result CreateTaskBranch(int taskId, int branchId);
        Ins_TaskDepartments_Create_Result CreateTaskDepartment(int taskId, int departmentId);
        Ins_TaskPositions_Create_Result CreateTaskPosition(int taskId, int positionId);
        Ins_TaskTaskUsers_Create_Result CreateTaskUser(int taskId, int userId);

        List<Ins_Task_Sub_ListBySubTaskId_Result> GetTaskSubListBySubTaskId(int subTaskId);

        List<Ins_Task_Sub_Delete_BySubTaskId_Result> DeleteSubTaskBySubTaskId(int subTaskId);

        Ins_Task_Sub_Update_Description_Result UpdateSubTaskDescription(int id, string description);

        Ins_Task_Sub_Update_ById_Result UpdateTaskSubById(
            int id,
            int? ordinalNumber,
            int? bundleId,
            int? sortIndex,
            int? privateSortIndex,
            string title,
            string titleNosign,
            string description,
            string alias,
            int? shopId,
            int? createdUserId,
            DateTime? deadline,
            DateTime? startDate,
            int? duration,
            DateTime? updatedAt,
            DateTime? completedAt,
            bool? isCompleted,
            int? completionPercentage,
            int? assignedId,
            int? taskId
        );

        int Ins_Task_Add_Collaborator(int taskId, int userId);

        /// <summary>
        /// Xóa tất cả collaborators của task theo task_id
        /// </summary>
        /// <param name="taskId">ID của task</param>
        /// <returns>Số lượng collaborators đã xóa</returns>
        int DeleteTaskCollaborators(int taskId);

        List<Ins_Task_GetListUsers_ByTaskId_Result> Ins_Task_GetListUsers_ByTaskId(int bundleId);

        int SetTaskBundleFavorite(int bundleId, bool isFavorite);

        /// <summary>
        /// Lấy danh sách task field theo ID
        /// </summary>
        /// <param name="fieldId">ID của field</param>
        /// <returns>Danh sách task field</returns>
        List<Ins_Task_Field_GetById_Result> GetTaskFieldById(int fieldId, int objectId);

        /// <summary>
        /// Cập nhật priority cho subtask (field_id, subtask_id, value_text, value_option_id)
        /// </summary>
        int UpdateSubTaskPriority(int subTaskId, string valueText, int? valueOptionId);

        /// <summary>
        /// Lấy danh sách customized field (ví dụ: priority) cho subtask
        /// </summary>
        List<Ins_Task_Field_Subtask_GetList_Result> GetSubTaskFieldList(int? alias, int subTaskId);

        /// <summary>
        /// Cập nhật custom field cho subtask
        /// </summary>
        int UpdateTaskCustomField(int valueFieldId, int subTaskId, string valueText);

        /// <summary>
        /// Cập nhật thông tin bundle (task) chính
        /// </summary>
        Ins_Task_Update_ById_Result UpdateTaskBundleById(
            int bundleId,
            string name,
            string defaultView,
            int? assigned_user
        );

        /// <summary>
        /// Cập nhật departments cho bundle
        /// </summary>
        int UpdateTaskDepartmentsByTaskId(int bundleId, int departmentId);

        /// <summary>
        /// Cập nhật positions cho bundle
        /// </summary>
        int UpdateTaskPositionsByTaskId(int bundleId, int positionId);

        /// <summary>
        /// Cập nhật branches cho bundle
        /// </summary>
        int UpdateTaskBranchesByTaskId(int bundleId, int branchId);

        /// <summary>
        /// Xóa toàn bộ quan hệ cơ bản của bundle (departments, positions, branches, users)
        /// </summary>
        int DeleteBasicRelationsByTaskId(int bundleId);

        /// <summary>
        /// Lấy danh sách branch theo taskId
        /// </summary>
        List<Ins_TaskBranches_GetList_ByTaskId_Result> Ins_TaskBranches_GetList_ByTaskId(
            int taskId
        );

        /// <summary>
        /// Lấy danh sách position theo taskId
        /// </summary>
        List<Ins_TaskPositions_GetList_ByTaskId_Result> Ins_TaskPositions_GetList_ByTaskId(
            int taskId
        );

        /// <summary>
        /// Lấy danh sách department theo taskId
        /// </summary>
        List<Ins_TaskDepartments_GetList_ByTaskId_Result> Ins_TaskDepartments_GetList_ByTaskId(
            int taskId
        );

        /// <summary>
        /// Cập nhật status cho subtask khi kéo thả sang First/Last (Ins_TaskSubTasks_UpdateSortIndexFirstOrlast)
        /// </summary>
        int Ins_TaskSubTasks_UpdateSortIndexFirstOrlast(
            int subTaskId,
            int statusId,
            int sortIndex,
            string position
        );

        /// <summary>
        /// Lấy danh sách subtask theo bundle_id và subtask_id (Ins_TaskSubTasks_GetListIndex_ById)
        /// </summary>
        List<Ins_TaskSubTasks_GetListIndex_ById_Result> Ins_TaskSubTasks_GetListIndex_ById(
            int subTaskId,
            int subTaskIdStatus
        );

        /// <summary>
        /// Cập nhật sort_index cho subtask khi kéo thả vào giữa (Ins_TaskSubTasks_UpdateSortIndexMid)
        /// </summary>
        int Ins_TaskSubTasks_UpdateSortIndexMid(int subTaskId, int statusId, int sortIndex);

        /// <summary>
        /// Lấy danh sách group theo statusId (gọi store Ins_Task_GetTaskGroupsById)
        /// </summary>
        List<Ins_Task_GetTaskGroupsById_Result> GetTaskGroupsById(int statusId);

        /// <summary>
        /// Lấy sub-task theo id (gọi store Ins_Task_Sub_GetById)
        /// </summary>
        Ins_Task_Sub_GetById_Result Ins_Task_Sub_GetById(int subTaskId);

        /// <summary>
        /// Cập nhật icon và màu sắc cho bundle (gọi store Ins_Task_Update_Color_And_Name)
        /// </summary>
        Ins_Task_Update_Color_And_Name_Result Ins_Task_Update_Color_And_Name(
            int bundleId,
            string icon,
            string color
        );

        /// <summary>
        /// Tạo label cho bundle (gọi store Ins_Task_Label_CreateByBundleId)
        /// </summary>
        List<Ins_Task_Label_CreateByBundleId_Result> Ins_Task_Label_CreateByBundleId(
            int bundleId,
            string name,
            string color,
            int? userId,
            int? sortIndex,
            string title = ""
        );

        /// <summary>
        /// Lấy danh sách label theo bundle_id (gọi store Ins_Task_Label_GetListByBundleId)
        /// </summary>
        List<Ins_Task_Label_GetListByBundleId_Result> Ins_Task_Label_GetListByBundleId(
            int bundleId
        );

        /// <summary>
        /// Gán label cho task (gọi store Ins_Task_Label_Value_Add)
        /// </summary>
        int Ins_Task_Label_Value_Add(int taskId, int labelId);

        /// <summary>
        /// Lấy danh sách label value theo bundleId (gọi store Ins_Task_Label_Value_GetList_BybundleId)
        /// </summary>
        List<Ins_Task_Label_Value_GetList_BybundleId_Result> Ins_Task_Label_Value_GetList_BybundleId(
            int bundleId
        );

        /// <summary>
        /// Xóa tất cả label value theo bundleId (gọi store Ins_Task_Label_Value_DeleteByBundleId)
        /// </summary>
        int Ins_Task_Label_Value_DeleteByBundleId(int bundleId);

        /// <summary>
        /// Đếm số lượng subtask hoàn thành theo bundleId (gọi store Ins_TaskSubTasks_CountCompleted)
        /// </summary>
        int Ins_TaskSubTasks_CountCompleted(int bundleId);

        /// <summary>
        /// Đếm số lượng subtask quá hạn theo bundleId (gọi store Ins_TaskSubTasks_CountOverdue)
        /// </summary>
        int Ins_TaskSubTasks_CountOverdue(int bundleId);

        /// <summary>
        /// Tạo field cho task (gọi store Ins_Task_Field_Create)
        /// </summary>
        Ins_Task_Field_Create_Result Ins_Task_Field_Create(
            int task_id,
            string title,
            int type,
            string description,
            bool add_to_library,
            bool notify_on_change,
            bool is_default
        );

        /// <summary>
        /// Tạo option cho field (gọi store Ins_Task_Field_Option)
        /// </summary>
        Ins_Task_Field_Option_Result Ins_Task_Field_Option(
            int field_id,
            string title,
            string color,
            int sort_index,
            string alias
        );

        /// <summary>
        /// Lấy danh sách task field theo type (gọi store Ins_Task_Field_GetByType)
        /// </summary>
        /// <param name="fieldType">Loại field</param>
        /// <returns>Danh sách task field</returns>
        List<Ins_Task_Field_GetByType_Result> GetTaskFieldByType(int fieldType);

        /// <summary>
        /// Lấy danh sách task field custom theo type (gọi store Ins_Task_Field_Custom_GetByType)
        /// </summary>
        /// <param name="fieldType">Loại field</param>
        /// <returns>Danh sách task field</returns>
        List<Ins_Task_Field_Custom_GetByType_Result> GetTaskFieldCustomByType(int fieldType);

        /// <summary>
        /// Xóa task label theo ID (gọi store Ins_Task_Label_Delete)
        /// </summary>
        /// <param name="labelId">ID của label cần xóa</param>
        /// <returns>Kết quả xóa</returns>
        int DeleteTaskLabel(int labelId);

        /// <summary>
        /// Cập nhật task label theo ID (gọi store Ins_Task_Label_Update)
        /// </summary>
        /// <param name="labelId">ID của label cần cập nhật</param>
        /// <param name="name">Tên mới của label</param>
        /// <param name="color">Màu sắc mới</param>
        /// <param name="bundleId">Bundle ID</param>
        /// <returns>Kết quả cập nhật</returns>
        Ins_Task_Label_Update_Result UpdateTaskLabel(
            int labelId,
            string name,
            string color,
            int bundleId
        );

        /// <summary>
        /// Cập nhật task field theo ID (gọi store Ins_Task_Field_Update)
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
        Ins_Task_Field_Update_Result UpdateTaskField(
            int fieldId,
            string title,
            string description,
            bool addToLibrary,
            bool notifyOnChange
        );

        /// <summary>
        /// Cập nhật task field option theo ID (gọi store Ins_Task_Field_Option_Update)
        /// </summary>
        /// <param name="optionId">ID của option cần cập nhật</param>
        /// <param name="title">Tiêu đề mới</param>
        /// <param name="color">Màu sắc mới</param>
        /// <param name="sortIndex">Thứ tự sắp xếp</param>
        /// <param name="titleNosign">Tiêu đề không dấu</param>
        /// <param name="actionOption">Hành động option</param>
        /// <returns>Kết quả cập nhật</returns>
        int UpdateTaskFieldOption(
            int optionId,
            string title,
            string color,
            int sortIndex,
            string titleNosign,
            string actionOption
        );

        /// <summary>
        /// Xóa task field theo ID (gọi store Ins_TaskField_Delete)
        /// </summary>
        /// <param name="fieldId">ID của field cần xóa</param>
        /// <param name="source">Nguồn</param>
        /// <param name="objectId">Object ID</param>
        /// <returns>Kết quả xóa</returns>
        int DeleteTaskField(int field_value_Id, string source, int? objectId);

        /// <summary>
        /// Lấy danh sách task fields theo source và object_id (gọi store Ins_Task_Field_GetAll)
        /// </summary>
        /// <param name="source">Nguồn</param>
        /// <param name="objectId">Object ID</param>
        /// <returns>Danh sách task fields</returns>
        List<Ins_Task_Field_GetAll_Result> GetTaskFieldList(string source, int objectId);

        /// <summary>
        /// Lấy danh sách task fields theo source và object_id (gọi store Ins_Task_Field_GetAll)
        /// </summary>
        /// <param name="source">Nguồn</param>
        /// <param name="objectId">Object ID</param>
        /// <returns>Danh sách task fields</returns>
        List<Ins_Task_Field_GetType_Result> GetTaskFieldListByType(string source, int objectId, int type);


        /// <summary>
        /// Thêm field vào subtask theo type (gọi store Ins_Task_Sub_Field_Add_ByType)
        /// </summary>
        /// <param name="fieldType">Loại field</param>
        /// <param name="subTaskId">ID của subtask</param>
        /// <returns>Số lượng record được thêm</returns>
        int Ins_Task_Sub_Field_Add_ByType(int fieldType, int subTaskId);

        /// <summary>
        /// Thêm field vào subtask theo ID (gọi store Ins_Task_Sub_Field_Add_ById)
        /// </summary>
        /// <param name="taskId">ID của task</param>
        /// <param name="fieldId">ID của field</param>
        /// <returns>Số lượng record được thêm</returns>
        int Ins_Task_Sub_Field_Add_ById(int taskId, int fieldId);

        /// <summary>
        /// Thêm field vào task theo type (gọi store Ins_Task_Field_Add_ByType)
        /// </summary>
        /// <param name="fieldType">Loại field</param>
        /// <param name="objectId">ID của object (task)</param>
        /// <returns>Số lượng record được thêm</returns>
        int Ins_Task_Field_Add_ByType(int fieldType, int objectId);

        /// <summary>
        /// Thêm field vào task theo ID (gọi store Ins_Task_Field_Add_ById)
        /// </summary>
        /// <param name="fieldId">ID của field</param>
        /// <param name="objectId">ID của object (task)</param>
        /// <returns>Số lượng record được thêm</returns>
        int Ins_Task_Field_Add_ById(int fieldId, int objectId, bool active);

        /// <summary>
        /// Bật/tắt field value (gọi store Ins_Task_Field_Value_Update)
        /// </summary>
        /// <param name="id">ID của field value</param>
        /// <param name="active">Trạng thái active</param>
        /// <returns>Số lượng record được cập nhật</returns>
        int TurnOnOffTaskFieldValue(int id, bool active);

        /// <summary>
        /// Lấy thông tin collaborators của task (gọi store Ins_Task_Collaborator_Info)
        /// </summary>
        /// <param name="taskId">ID của task</param>
        /// <returns>Danh sách collaborators</returns>
        List<Ins_Task_Collaborator_Info_Result> GetTaskCollaboratorsByTask(int taskId);

        /// <summary>
        /// Xóa task bundle status theo ID (gọi store Ins_Task_Group_Delete_ByGroupId)
        /// </summary>
        /// <param name="statusId">ID của status cần xóa</param>
        /// <returns>Kết quả xóa</returns>
        int DeleteTaskBundleStatus(int statusId);
        Ins_Task_Label_GetById_Result GetTaskLabelDetail(int labelId);
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

        public Ins_Tasks_Create_Result CreateTask(
            string title,
            int createdUserObj,
            int companyId,
            string defaultView,
            string color
        )
        {
            using (Uow)
            {
                var result = Uow.Context.Ins_Tasks_Create(
                    title,
                    createdUserObj,
                    companyId,
                    defaultView,
                    color
                );
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

        public Ins_Task_Group_Create_Result CreateTaskGroup(
            int bundleId,
            string name,
            string color,
            string position
        )
        {
            using (Uow)
            {
                var result = Uow.Context.Ins_Task_Group_Create(bundleId, name, color, position);
                return result.FirstOrDefault();
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

        public Ins_Task_Group_Update_Color_And_Name_Result UpdateTaskGroupColorAndName(
            int groupId,
            string name,
            string color
        )
        {
            using (Uow)
            {
                var result = Uow.Context.Ins_Task_Group_Update_Color_And_Name(groupId, name, color);
                return result.FirstOrDefault();
            }
        }

        public Ins_Task_Sub_Create_Result CreateTaskSub(
            string title,
            string alias,
            int? bundleId,
            int? createdUserId,
            int? assignedId,
            string position,
            DateTime? deadline
        )
        {
            using (Uow)
            {
                var result = Uow.Context.Ins_Task_Sub_Create(
                    title,
                    alias,
                    bundleId,
                    createdUserId,
                    assignedId,
                    position,
                    deadline
                );
                return result.FirstOrDefault();
            }
        }

        public List<Ins_Task_Sub_ListByBundle_Result> GetTaskSubListByBundle(int bundleId)
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

        public Ins_Task_Sub_Update_Deadline_Result UpdateTaskSubDeadline(
            int id,
            DateTime? deadline,
            DateTime? startDate
        )
        {
            using (Uow)
            {
                var result = Uow.Context.Ins_Task_Sub_Update_Deadline(id, deadline, startDate);
                return result.FirstOrDefault();
            }
        }

        public Ins_Task_Update_AssignedUser_Result UpdateTaskAssignedUser(
            int taskId,
            int? assignedUser
        )
        {
            using (Uow)
            {
                var result = Uow.Context.Ins_Task_Update_AssignedUser(taskId, assignedUser);
                return result.FirstOrDefault();
            }
        }

        public int GetSubFieldValueByTitle(int subtaskId, string title = null)
        {
            using (Uow)
            {
                var result = Uow.Context.Ins_Task_Get_Sub_Field_Value_ByTitle(subtaskId, title);
                return result;
            }
        }

        public void InsertTaskFieldOptionsBulk(int fieldId, string options)
        {
            using (Uow)
            {
                Uow.Context.Database.ExecuteSqlCommand(
                    "EXEC Ins_Task_Field_Options_Bulk @field_id, @options",
                    new System.Data.SqlClient.SqlParameter("@field_id", fieldId),
                    new System.Data.SqlClient.SqlParameter(
                        "@options",
                        options ?? (object)DBNull.Value
                    )
                );
            }
        }

        public Ins_Task_Sub_Update_Title_Result UpdateSubTaskTitle(
            int id,
            string title,
            string titleNosign,
            string alias
        )
        {
            using (Uow)
            {
                var result = Uow.Context.Ins_Task_Sub_Update_Title(id, title, titleNosign, alias);
                return result.FirstOrDefault();
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

        public List<Ins_Task_Sub_ListBySubTaskId_Result> GetTaskSubListBySubTaskId(int subTaskId)
        {
            using (Uow)
            {
                var result = Uow.Context.Ins_Task_Sub_ListBySubTaskId(subTaskId);
                return result.ToList();
            }
        }

        public List<Ins_Task_Sub_Delete_BySubTaskId_Result> DeleteSubTaskBySubTaskId(int subTaskId)
        {
            using (Uow)
            {
                var result = Uow.Context.Ins_Task_Sub_Delete_BySubTaskId(subTaskId);
                return result.ToList();
            }
        }

        public Ins_Task_Sub_Update_Description_Result UpdateSubTaskDescription(
            int id,
            string description
        )
        {
            using (Uow)
            {
                var result = Uow.Context.Ins_Task_Sub_Update_Description(id, description);
                return result.FirstOrDefault();
            }
        }

        public Ins_Task_Sub_Update_ById_Result UpdateTaskSubById(
            int id,
            int? ordinalNumber,
            int? bundleId,
            int? sortIndex,
            int? privateSortIndex,
            string title,
            string titleNosign,
            string description,
            string alias,
            int? shopId,
            int? createdUserId,
            DateTime? deadline,
            DateTime? startDate,
            int? duration,
            DateTime? updatedAt,
            DateTime? completedAt,
            bool? isCompleted,
            int? completionPercentage,
            int? assignedId,
            int? taskId
        )
        {
            using (Uow)
            {
                var result = Uow.Context.Ins_Task_Sub_Update_ById(
                    id,
                    ordinalNumber,
                    bundleId,
                    sortIndex,
                    privateSortIndex,
                    title,
                    titleNosign,
                    description,
                    alias,
                    shopId,
                    createdUserId,
                    deadline,
                    startDate,
                    duration,
                    updatedAt,
                    completedAt,
                    isCompleted,
                    completionPercentage,
                    assignedId,
                    taskId
                );
                return result.FirstOrDefault();
            }
        }

        public int Ins_Task_Add_Collaborator(int taskId, int userId)
        {
            using (Uow)
            {
                var result = Uow.Context.Ins_Task_Add_Collaborator(taskId, userId);
                return result.FirstOrDefault() ?? 0;
            }
        }

        public int DeleteTaskCollaborators(int taskId)
        {
            using (Uow)
            {
                return Uow.Context.Ins_Task_Sub_Collaborators_DeleteAll_ByTask_id(taskId);
            }
        }

        public List<Ins_Task_GetListUsers_ByTaskId_Result> Ins_Task_GetListUsers_ByTaskId(
            int bundleId
        )
        {
            using (Uow)
            {
                var result = Uow.Context.Ins_Task_GetListUsers_ByTaskId(bundleId);
                return result.ToList();
            }
        }

        public int SetTaskBundleFavorite(int bundleId, bool isFavorite)
        {
            using (Uow)
            {
                var result = Uow.Context.Ins_Task_Update_Completed(bundleId, isFavorite);
                // Nếu store trả về list, chỉ cần kiểm tra có kết quả là thành công
                return result != null && result.FirstOrDefault() != null ? 1 : 0;
            }
        }

        /// <summary>
        /// Lấy danh sách task field theo ID
        /// </summary>
        /// <param name="fieldId">ID của field</param>
        /// <returns>Danh sách task field</returns>
        public List<Ins_Task_Field_GetById_Result> GetTaskFieldById(int fieldId, int objectId)
        {
            using (Uow)
            {
                var result = Uow.Context.Ins_Task_Field_GetById(fieldId, objectId);
                return result.ToList();
            }
        }

        /// <summary>
        /// Cập nhật priority cho subtask (field_id, subtask_id, value_text, value_option_id)
        /// </summary>
        public int UpdateSubTaskPriority(int subTaskId, string valueText, int? valueOptionId)
        {
            using (Uow)
            {
                var result = Uow.Context.Ins_Task_field_subtask_Update(
                    subTaskId,
                    valueText,
                    valueOptionId
                );
                return result?.Count() ?? 0;
            }
        }

        /// <summary>
        /// Lấy danh sách customized field (ví dụ: priority) cho subtask
        /// </summary>
        public List<Ins_Task_Field_Subtask_GetList_Result> GetSubTaskFieldList(
            int? alias,
            int subTaskId
        )
        {
            using (Uow)
            {
                var result = Uow.Context.Ins_Task_Field_Subtask_GetList(subTaskId, alias);
                return result.ToList();
            }
        }

        /// <summary>
        /// Cập nhật custom field cho subtask
        /// </summary>
        public int UpdateTaskCustomField(int valueFieldId, int subTaskId, string valueText)
        {
            using (Uow)
            {
                var result = Uow.Context.Ins_Task_field_custom_subtask_Update(valueFieldId, subTaskId, valueText);
                return result;
            }
        }

        /// <summary>
        /// Cập nhật thông tin bundle (task) chính
        /// </summary>
        public Ins_Task_Update_ById_Result UpdateTaskBundleById(
            int bundleId,
            string name,
            string defaultView,
            int? assigned_user
        )
        {
            using (Uow)
            {
                var result = Uow.Context.Ins_Task_Update_ById(
                    bundleId,
                    name,
                    defaultView,
                    assigned_user
                );
                return result.FirstOrDefault();
            }
        }

        /// <summary>
        /// Cập nhật departments cho bundle
        /// </summary>
        public int UpdateTaskDepartmentsByTaskId(int bundleId, int departmentId)
        {
            using (Uow)
            {
                var result = Uow.Context.Ins_TaskDepartments_Update_ByTaskId(
                    bundleId,
                    departmentId
                );
                return result != null && result.FirstOrDefault() != null ? 1 : 0;
            }
        }

        /// <summary>
        /// Cập nhật positions cho bundle
        /// </summary>
        public int UpdateTaskPositionsByTaskId(int bundleId, int positionId)
        {
            using (Uow)
            {
                var result = Uow.Context.Ins_TaskPositions_Update_ByTaskId(bundleId, positionId);
                return result != null && result.FirstOrDefault() != null ? 1 : 0;
            }
        }

        /// <summary>
        /// Cập nhật branches cho bundle
        /// </summary>
        public int UpdateTaskBranchesByTaskId(int bundleId, int branchId)
        {
            using (Uow)
            {
                var result = Uow.Context.Ins_TaskBranches_Update_ByTaskId(bundleId, branchId);
                return result != null && result.FirstOrDefault() != null ? 1 : 0;
            }
        }

        /// <summary>
        /// Xóa toàn bộ quan hệ cơ bản của bundle (departments, positions, branches, users)
        /// </summary>
        public int DeleteBasicRelationsByTaskId(int bundleId)
        {
            using (Uow)
            {
                return Uow.Context.Ins_Tasks_Delete_BasicRelations_ByTaskId(bundleId);
            }
        }

        /// <summary>
        /// Lấy danh sách branch theo taskId
        /// </summary>
        public List<Ins_TaskBranches_GetList_ByTaskId_Result> Ins_TaskBranches_GetList_ByTaskId(
            int taskId
        )
        {
            using (Uow)
            {
                var result = Uow.Context.Ins_TaskBranches_GetList_ByTaskId(taskId);
                return result.ToList();
            }
        }

        /// <summary>
        /// Lấy danh sách position theo taskId
        /// </summary>
        public List<Ins_TaskPositions_GetList_ByTaskId_Result> Ins_TaskPositions_GetList_ByTaskId(
            int taskId
        )
        {
            using (Uow)
            {
                var result = Uow.Context.Ins_TaskPositions_GetList_ByTaskId(taskId);
                return result.ToList();
            }
        }

        /// <summary>
        /// Lấy danh sách department theo taskId
        /// </summary>
        public List<Ins_TaskDepartments_GetList_ByTaskId_Result> Ins_TaskDepartments_GetList_ByTaskId(
            int taskId
        )
        {
            using (Uow)
            {
                var result = Uow.Context.Ins_TaskDepartments_GetList_ByTaskId(taskId);
                return result.ToList();
            }
        }

        /// <summary>
        /// Cập nhật status cho subtask khi kéo thả sang First/Last (Ins_TaskSubTasks_UpdateSortIndexFirstOrlast)
        /// </summary>
        public int Ins_TaskSubTasks_UpdateSortIndexFirstOrlast(
            int subTaskId,
            int statusId,
            int sortIndex,
            string position
        )
        {
            using (Uow)
            {
                var result = Uow.Context.Ins_TaskSubTasks_UpdateSortIndexFirstOrLast(
                    subTaskId,
                    statusId,
                    sortIndex,
                    position
                );
                return result;
            }
        }

        /// <summary>
        /// Lấy danh sách subtask theo bundle_id và subtask_id (Ins_TaskSubTasks_GetListIndex_ById)
        /// </summary>
        public List<Ins_TaskSubTasks_GetListIndex_ById_Result> Ins_TaskSubTasks_GetListIndex_ById(
            int subTaskId,
            int subTaskIdStatus
        )
        {
            using (Uow)
            {
                var result = Uow.Context.Ins_TaskSubTasks_GetListIndex_ById(
                    subTaskId,
                    subTaskIdStatus
                );
                return result.ToList();
            }
        }

        /// <summary>
        /// Cập nhật sort_index cho subtask khi kéo thả vào giữa (Ins_TaskSubTasks_UpdateSortIndexMid)
        /// </summary>
        public int Ins_TaskSubTasks_UpdateSortIndexMid(int subTaskId, int statusId, int sortIndex)
        {
            using (Uow)
            {
                // Gọi stored procedure Ins_TaskSubTasks_UpdateSortIndexMid
                return Uow.Context.Ins_TaskSubTasks_UpdateSortIndexMid(
                    subTaskId,
                    statusId,
                    sortIndex
                );
            }
        }

        public List<Ins_Task_GetTaskGroupsById_Result> GetTaskGroupsById(int statusId)
        {
            using (Uow)
            {
                var result = Uow.Context.Ins_Task_GetTaskGroupsById(statusId);
                return result.ToList();
            }
        }

        /// <summary>
        /// Lấy sub-task theo id (gọi store Ins_Task_Sub_GetById)
        /// </summary>
        public Ins_Task_Sub_GetById_Result Ins_Task_Sub_GetById(int subTaskId)
        {
            using (Uow)
            {
                var result = Uow.Context.Ins_Task_Sub_GetById(subTaskId);
                return result.FirstOrDefault();
            }
        }

        public Ins_Task_Update_Color_And_Name_Result Ins_Task_Update_Color_And_Name(
            int bundleId,
            string icon,
            string color
        )
        {
            using (Uow)
            {
                var result = Uow.Context.Ins_Task_Update_Color_And_Name(bundleId, color, icon);
                return result.FirstOrDefault();
            }
        }

        public List<Ins_Task_Label_CreateByBundleId_Result> Ins_Task_Label_CreateByBundleId(
            int bundleId,
            string name,
            string color,
            int? userId,
            int? sortIndex,
            string title = ""
        )
        {
            using (Uow)
            {
                var result = Uow.Context.Ins_Task_Label_CreateByBundleId(
                    name,
                    color,
                    userId,
                    sortIndex,
                    title,
                    bundleId
                );
                return result.ToList();
            }
        }

        public List<Ins_Task_Label_GetListByBundleId_Result> Ins_Task_Label_GetListByBundleId(
            int bundleId
        )
        {
            using (Uow)
            {
                var result = Uow.Context.Ins_Task_Label_GetListByBundleId(bundleId);
                return result.ToList();
            }
        }

        public int Ins_Task_Label_Value_Add(int taskId, int labelId)
        {
            using (Uow)
            {
                var result = Uow.Context.Ins_Task_Label_Value_Add(taskId, labelId);

                return result;
            }
        }

        public List<Ins_Task_Label_Value_GetList_BybundleId_Result> Ins_Task_Label_Value_GetList_BybundleId(
            int bundleId
        )
        {
            using (Uow)
            {
                var result = Uow.Context.Ins_Task_Label_Value_GetList_BybundleId(bundleId);
                return result.ToList();
            }
        }

        public int Ins_Task_Label_Value_DeleteByBundleId(int bundleId)
        {
            using (Uow)
            {
                return Uow.Context.Ins_Task_Label_Value_DeleteByBundleId(bundleId);
            }
        }

        public int Ins_TaskSubTasks_CountCompleted(int bundleId)
        {
            using (Uow)
            {
                var result = Uow.Context.Ins_TaskSubTasks_CountCompleted(bundleId);
                var first = result.FirstOrDefault();
                if (first != null && first.completed_count.HasValue)
                    return first.completed_count.Value;
                return 0;
            }
        }

        public int Ins_TaskSubTasks_CountOverdue(int bundleId)
        {
            using (Uow)
            {
                var result = Uow.Context.Ins_TaskSubTasks_CountOverdue(bundleId);
                var value = result.FirstOrDefault();
                return value ?? 0;
            }
        }

        public Ins_Task_Field_Create_Result Ins_Task_Field_Create(
            int task_id,
            string title,
            int type,
            string description,
            bool add_to_library,
            bool notify_on_change,
            bool is_default
        )
        {
            using (Uow)
            {
                var result = Uow.Context.Ins_Task_Field_Create(
                    task_id,
                    title,
                    type,
                    description,
                    add_to_library,
                    notify_on_change,
                    is_default
                );
                return result.FirstOrDefault();
            }
        }

        public Ins_Task_Field_Option_Result Ins_Task_Field_Option(
            int field_id,
            string title,
            string color,
            int sort_index,
            string alias
        )
        {
            using (Uow)
            {
                var result = Uow.Context.Ins_Task_Field_Option(
                    field_id,
                    title,
                    color,
                    sort_index,
                    alias
                );
                return result.FirstOrDefault();
            }
        }

        /// <summary>
        /// Lấy danh sách task field theo type (gọi store Ins_Task_Field_GetByType)
        /// </summary>
        /// <param name="fieldType">Loại field</param>
        /// <returns>Danh sách task field</returns>
        public List<Ins_Task_Field_GetByType_Result> GetTaskFieldByType(int fieldType)
        {
            using (Uow)
            {
                var result = Uow.Context.Ins_Task_Field_GetByType(fieldType);
                return result.ToList();
            }
        }

        /// <summary>
        /// Lấy danh sách task field theo type (gọi store Ins_Task_Field_GetByType)
        /// </summary>
        /// <param name="fieldType">Loại field</param>
        /// <returns>Danh sách task field</returns>
        public List<Ins_Task_Field_Custom_GetByType_Result> GetTaskFieldCustomByType(int fieldType)
        {
            using (Uow)
            {
                var result = Uow.Context.Ins_Task_Field_Custom_GetByType(fieldType);
                return result.ToList();
            }
        }

        /// <summary>
        /// Xóa task label theo ID (gọi store Ins_Task_Label_Delete)
        /// </summary>
        /// <param name="labelId">ID của label cần xóa</param>
        /// <returns>Kết quả xóa</returns>
        public int DeleteTaskLabel(int labelId)
        {
            using (Uow)
            {
                var result = Uow.Context.Ins_Task_Label_Delete(labelId);
                return result;
            }
        }

        public Ins_Task_Label_Update_Result UpdateTaskLabel(
            int labelId,
            string name,
            string color,
            int bundleId
        )
        {
            using (Uow)
            {
                var result = Uow.Context.Ins_Task_Label_Update(
                    labelId,
                    name,
                    color,
                    name,
                    bundleId
                );
                return result.FirstOrDefault();
            }
        }

        public Ins_Task_Field_Update_Result UpdateTaskField(
            int fieldId,
            string title,
            string description,
            bool addToLibrary,
            bool notifyOnChange
        )
        {
            using (Uow)
            {
                var result = Uow.Context.Ins_Task_Field_Update(
                    fieldId,
                    title,
                    description,
                    addToLibrary,
                    notifyOnChange
                );
                return result.FirstOrDefault();
            }
        }

        public int UpdateTaskFieldOption(
            int optionId,
            string title,
            string color,
            int sortIndex,
            string titleNosign,
            string actionOption
        )
        {
            using (Uow)
            {
                var result = Uow.Context.Ins_Task_Field_Option_Update(
                    optionId,
                    title,
                    color,
                    sortIndex
                );
                return result;
            }
        }

        public int DeleteTaskField(int field_value_Id, string source, int? objectId)
        {
            using (Uow)
            {
                return Uow.Context.Ins_Task_Field_Delete(field_value_Id, objectId);
            }
        }

        public List<Ins_Task_Field_GetAll_Result> GetTaskFieldList(string source, int objectId)
        {
            using (Uow)
            {
                var result = Uow.Context.Ins_Task_Field_GetAll(objectId);
                return result.ToList();
            }
        }

        public List<Ins_Task_Field_GetType_Result> GetTaskFieldListByType(string source, int objectId, int type)
        {
            using (Uow)
            {
                var result = Uow.Context.Ins_Task_Field_GetType(objectId, type);
                return result.ToList();
            }
        }


        public int Ins_Task_Sub_Field_Add_ByType(int fieldType, int subTaskId)
        {
            using (Uow)
            {
                // Tạm thời sử dụng Ins_Task_Field_Add với fieldType làm fieldId
                // TODO: Tạo stored procedure Ins_Task_Sub_Field_Add_ByType nếu cần
                return Uow.Context.Ins_Task_Sub_Field_Add_ByType(fieldType, subTaskId);
            }
        }

        public int Ins_Task_Sub_Field_Add_ById(int taskId, int fieldId)
        {
            using (Uow)
            {
                return Uow.Context.Ins_Task_Sub_Field_Add_ById(taskId, fieldId);
            }
        }

        public int Ins_Task_Field_Add_ByType(int fieldType, int objectId)
        {
            using (Uow)
            {
                return Uow.Context.Ins_Task_Field_Add_ByType(fieldType, objectId);
            }
        }

        public int Ins_Task_Field_Add_ById(int fieldId, int objectId, bool active)
        {
            using (Uow)
            {
                return Uow.Context.Ins_Task_Field_Add_ById(fieldId, objectId, active);
            }
        }

        public int TurnOnOffTaskFieldValue(int id, bool active)
        {
            using (Uow)
            {
                return Uow.Context.Ins_Task_Field_Value_Update(id, active);
            }
        }

        public List<Ins_Task_Collaborator_Info_Result> GetTaskCollaboratorsByTask(int taskId)
        {
            using (Uow)
            {
                var result = Uow.Context.Ins_Task_Collaborator_Info(taskId);
                return result.ToList();
            }
        }

        public int DeleteTaskBundleStatus(int statusId)
        {
            using (Uow)
            {
                Uow.Context.Ins_Task_Group_Delete_ByGroupId(statusId);
                return 0;
            }
        }

        public Ins_Task_Label_GetById_Result GetTaskLabelDetail(int labelId)
        {
            using (Uow)
            {
                var result = Uow.Context.Ins_Task_Label_GetById(labelId);
                return result.FirstOrDefault();
            }
        }
    }
}
