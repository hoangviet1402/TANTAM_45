using System;
using System.Collections.Generic;

namespace Framework.Outside.DataAccess.Interface
{
    public interface ITaskDao
    {
        #region Task Groups
        Ins_Task_Group_Create_Result CreateTaskGroup(int bundleId, string name, string color, string position);
        List<Ins_Task_GetTaskGroupsByTaskId_Result> GetTaskGroupsByTaskId(int taskId);
        #endregion

        #region Sub Tasks
        Ins_Task_Sub_Create_Result CreateTaskSub(string title, string alias, int? bundleId, int? createdUserId, string position);
        List<Ins_Task_Sub_ListByBundle_Result> GetTaskSubListByBundle(string bundleId);
        #endregion

        #region Task Fields
        // ... existing code ...
        #endregion
    }
} 