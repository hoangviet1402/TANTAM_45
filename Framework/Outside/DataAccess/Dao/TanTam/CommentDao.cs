using DataAccess.EF;
using DataAccess.Interface;
using EntitiesObject.Entities.TanTamEntities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DataAccess.Dao.TanTamDao
{
    /// <summary>
    /// Interface for Comment data access operations
    /// </summary>
    public interface ICommentDao : IBaseFactories<DBNull>
    {
        Ins_TaskComment_Create_Result CreateTaskComment(int taskId, int userId, string content, string source = "task", bool isSystem = false, bool isComment = true, string type = "comment", string attribute = null);
        void AddTaskCommentMention(int taskCommentId, int? userId = null, string name = null);
        void AddTaskCommentAttachment(int taskCommentId, string name, string path, string pathDate, int userId, string extension, long size);
        List<Ins_TaskComment_GetByTaskId_Result> GetByTaskId(int taskId, string source = "task", int page = 1, int perPage = 5);
        List<Ins_TaskComment_GetMentionsById_Result> GetMentionsById(int taskCommentId);
        List<Ins_TaskComment_GetFileUploadsById_Result> GetFileUploadsById(int taskCommentId);
        Dictionary<int, List<Ins_TaskComment_GetMentionsById_Result>> GetMentionsByCommentIds(List<int> commentIds);
        Dictionary<int, List<Ins_TaskComment_GetFileUploadsById_Result>> GetFileUploadsByCommentIds(List<int> commentIds);
        Ins_TaskComment_GetFileUploadById_Result GetFileUploadById(int fileId);
        Ins_TaskComment_Delete_Result DeleteTaskComment(int commentId, int userId);
        Ins_TaskComment_Update_Result UpdateTaskComment(int commentId, int userId, string content);
    }

    /// <summary>
    /// Implementation of Comment data access operations
    /// </summary>
    public class CommentDao : DaoFactories<TanTamEntities, DBNull>, ICommentDao
    {
        public Ins_TaskComment_Create_Result CreateTaskComment(int taskId, int userId, string content, string source = "task", bool isSystem = false, bool isComment = true, string type = "comment", string attribute = null)
        {
            using (Uow)
            {
                return Uow.Context.Ins_TaskComment_Create(taskId, userId, content, source, isSystem, isComment, type, attribute).FirstOrDefault();
            }
        }

        public void AddTaskCommentMention(int taskCommentId, int? userId = null, string name = null)
        {
            using (Uow)
            {
                Uow.Context.Ins_TaskComment_AddMentions(taskCommentId, userId, name);
            }
        }

        public List<Ins_TaskComment_GetByTaskId_Result> GetByTaskId(int taskId, string source = "task", int page = 1, int perPage = 5)
        {
            using (Uow)
            {
                return Uow.Context.Ins_TaskComment_GetByTaskId(taskId, source, page, perPage).ToList();
            }
        }

        public void AddTaskCommentAttachment(int taskCommentId, string name, string path, string pathDate, int userId, string extension, long size)
        {
            using (Uow)
            {
                Uow.Context.Ins_TaskComment_AddAttachment(taskCommentId, name, path, pathDate, userId, extension, size);
            }
        }

        public List<Ins_TaskComment_GetMentionsById_Result> GetMentionsById(int taskCommentId)
        {
            using (Uow)
            {
                return Uow.Context.Ins_TaskComment_GetMentionsById(taskCommentId).ToList();
            }
        }

        public List<Ins_TaskComment_GetFileUploadsById_Result> GetFileUploadsById(int taskCommentId)
        {
            using (Uow)
            {
                return Uow.Context.Ins_TaskComment_GetFileUploadsById(taskCommentId).ToList();
            }
        }

        public Ins_TaskComment_GetFileUploadById_Result GetFileUploadById(int fileId)
        {
            using (Uow)
            {
                return Uow.Context.Ins_TaskComment_GetFileUploadById(fileId).FirstOrDefault();
            }
        }

        public Ins_TaskComment_Delete_Result DeleteTaskComment(int commentId, int userId)
        {
            using (Uow)
            {
                return Uow.Context.Ins_TaskComment_Delete(commentId, userId).FirstOrDefault();
            }
        }

        public Ins_TaskComment_Update_Result UpdateTaskComment(int commentId, int userId, string content)
        {
            using (Uow)
            {
                return Uow.Context.Ins_TaskComment_Update(commentId, userId, content).FirstOrDefault();
            }
        }

        /// <summary>
        /// Lấy mentions cho nhiều comments cùng lúc để tối ưu N+1 query
        /// </summary>
        /// <param name="commentIds">Danh sách comment IDs</param>
        /// <returns>Dictionary với key là commentId và value là list mentions</returns>
        public Dictionary<int, List<Ins_TaskComment_GetMentionsById_Result>> GetMentionsByCommentIds(List<int> commentIds)
        {
            var result = new Dictionary<int, List<Ins_TaskComment_GetMentionsById_Result>>();
            
            if (commentIds == null || !commentIds.Any())
                return result;

            // Initialize dictionary với empty lists
            foreach (var commentId in commentIds)
            {
                result[commentId] = new List<Ins_TaskComment_GetMentionsById_Result>();
            }

            using (Uow)
            {
                // Lấy tất cả mentions cho các comment IDs
                foreach (var commentId in commentIds)
                {
                    var mentions = Uow.Context.Ins_TaskComment_GetMentionsById(commentId).OrderBy(m => m.Id).ToList();
                    result[commentId] = mentions;
                }
            }

            return result;
        }

        /// <summary>
        /// Lấy file uploads cho nhiều comments cùng lúc để tối ưu N+1 query
        /// </summary>
        /// <param name="commentIds">Danh sách comment IDs</param>
        /// <returns>Dictionary với key là commentId và value là list file uploads</returns>
        public Dictionary<int, List<Ins_TaskComment_GetFileUploadsById_Result>> GetFileUploadsByCommentIds(List<int> commentIds)
        {
            var result = new Dictionary<int, List<Ins_TaskComment_GetFileUploadsById_Result>>();
            
            if (commentIds == null || !commentIds.Any())
                return result;

            // Initialize dictionary với empty lists
            foreach (var commentId in commentIds)
            {
                result[commentId] = new List<Ins_TaskComment_GetFileUploadsById_Result>();
            }

            using (Uow)
            {
                // Lấy tất cả file uploads cho các comment IDs
                foreach (var commentId in commentIds)
                {
                    var fileUploads = Uow.Context.Ins_TaskComment_GetFileUploadsById(commentId).OrderBy(f => f.Id).ToList();
                    result[commentId] = fileUploads;
                }
            }

            return result;
        }
    }
}