using System;
using System.Collections.Generic;
using System.Linq;
using DataAccessRedis.Constant;
using DataAccessRedis.Model;
//using DataAccess;
//using MyConfig;

namespace DataAccessRedis.Dao.WebDao
{
    public interface IAssociationRedis
    {
        /// <summary>
        ///     <para>Author: DungDA</para>
        ///     <para>Date: 20/05/2016</para>
        ///     <para>Description: Redis: lấy data cho box cập nhật trang bang hội chi tiết</para>
        /// </summary>
        /// <returns></returns>
        List<AssMemberTopUpdateResponseModelRedis> GetBoxDataTopUpdate(int boxType, int assId, int top,
            DateTime dtmClearCacheRedis);

        /// <summary>
        /// <para>Author: DungDA</para>
        /// <para>Date: 20/05/2016</para>
        /// <para>Description: Redis: Set data cho box cập nhật trang bang hội chi tiết</para>
        /// </summary>
        /// <returns></returns>
        //RedisStatusCode SetBoxDataTopUpdate(int boxType, int assId, int top, DateTime dtmClearCacheRedis,
        //    List<Out_RankingInfoDetail_GetBoxData_Result> lstData);

        /// <summary>
        ///     <para>Author: DungDA</para>
        ///     <para>Date: 20/05/2016</para>
        ///     <para>Description: Redis: lấy data cho box đăng nhập sau cùng trang bang hội chi tiết</para>
        /// </summary>
        /// <returns></returns>
        List<AssMemberLastLoginResponseModelRedis> GetBoxDataLastLogin(int boxType, int assId, int top,
            DateTime dtmClearCacheRedis);

        /// <summary>
        ///     <para>Author: DungDA</para>
        ///     <para>Date: 23/05/2016</para>
        ///     <para>Description: Redis: lấy data cho box bài mới nhất trang bang hội chi tiết</para>
        /// </summary>
        /// <returns></returns>
        List<AssMemberNewestArticlesResponseModelRedis> GetBoxDataAssNewestArticles(int boxType, int assId, int top,
            DateTime dtmClearCacheRedis);

        /// <summary>
        ///     <para>Author: DungDA</para>
        ///     <para>Date: 23/05/2016</para>
        ///     <para>Description: Redis: lấy data cho box top đóng góp trang bang hội chi tiết</para>
        /// </summary>
        /// <returns></returns>
        List<AssMemberTopContributeResponseModelRedis> GetBoxDataTopContribute(int boxType, int assId, int top,
            DateTime dtmClearCacheRedis);

        /// <summary>
        ///     <para>Author: DungDA</para>
        ///     <para>Date: 23/05/2016</para>
        ///     <para>Description: Redis: lấy data cho box top kinh nghiệm tháng trang bang hội chi tiết</para>
        /// </summary>
        /// <returns></returns>
        List<AssMemberTopContributeExpResponseModelRedis> GetBoxDataTopContributeExpInMonth(int boxType, int assId,
            int top, DateTime dtmClearCacheRedis);

        /// <summary>
        /// <para>Author: DungDA</para>
        /// <para>Date: 23/05/2016</para>
        /// <para>Description: Redis: Set data cho box top kinh nghiệm tháng trang bang hội chi tiết</para>
        /// </summary>
        /// <returns></returns>
        //RedisStatusCode SetBoxDataTopContributeExpInMonth(int boxType, int assId, int top, DateTime dtmClearCacheRedis, List<Out_RankingInfoDetail_GetBoxData_Result> lstData);

        /// <summary>
        ///     <para>Author: DungDA</para>
        ///     <para>Date: 06/06/2016</para>
        ///     <para>Description: Redis: Lấy data cho box tin nhanh bang hội trang bang hội chung</para>
        /// </summary>
        /// <returns></returns>
        List<Association_GetArticles_AssNewRedis> GetAssociation_GetArticles_AssNew(int top, int rowStart, int rowEnd,
            DateTime dtmClearCacheRedis);

        /// <summary>
        ///     <para>Author: DungDA</para>
        ///     <para>Date: 07/06/2016</para>
        ///     <para>Description: Redis: Lấy data cho box bang hội mạnh trong tháng</para>
        /// </summary>
        /// <returns></returns>
        List<Association_GetTopMaxInMonth_Redis> GetAssociation_GetTopMaxInMonth(int top, int keyword,
            DateTime dtmClearCacheRedis);

        /// <summary>
        ///     <para>Author: DungDA</para>
        ///     <para>Date: 07/06/2016</para>
        ///     <para>Description: Redis: Set data cho box bang hội mạnh trong tháng</para>
        /// </summary>
        /// <returns></returns>
        //RedisStatusCode SetAssociation_GetTopMaxInMonth(int top, int keyword, DateTime dtmClearCacheRedis,
        //    List<Out_Association_GetTopMaxInMonth_Result> lstData);

        //List<Association_TopAssociationInMonth_Redis> GetTopAssociationUpLevelInMonth(int top, int keyword, DateTime dtmClearCacheRedis);

        //RedisStatusCode SetTopAssociationUpLevelInMonth(int top, int keyword, DateTime dtmClearCacheRedis);
        List<Association_TopAssociationTopCoin_Redis> GetTopAssociationTopCoin(int pageLength,
            DateTime dtmClearCacheRedis);

        //RedisStatusCode SetTopAssociationTopCoin(int pageLength, DateTime dtmClearCacheRedis, List<Out_Association_GetTopCoin_Result> lstData);

        List<Association_AssociationMember_Redis> GetAssociationMember(string firstLetter, int startIndex,
            int pageLength, DateTime dtmClearCacheRedis, out int totalRow);

        //RedisStatusCode SetAssociationMember(string firstLetter, int startIndex, DateTime dtmClearCacheRedis, List<Out_Association_Search_Result> lstData,int totalRow);

        /// <summary>
        ///     <para>Author: DungDA</para>
        ///     <para>Date: 09/06/2016</para>
        ///     <para>Description: Redis: Lấy data cho box Sự kiện bang hội</para>
        /// </summary>
        /// <returns></returns>
        List<Association_GetArticles_AssEvent_Redis> GetArticles_AssEvent(int categoryID, int rowStart, int rowEnd,
            out int totalRow, DateTime dtmClearCacheRedis);

        /// <summary>
        ///     <para>Author: DungDA</para>
        ///     <para>Date: 22/06/2016</para>
        ///     <para>Description: Redis: lấy data cho box  top đóng góp tháng trang bang hội chi tiết</para>
        /// </summary>
        /// <returns></returns>
        List<TopMonthContributorRedis> GetBoxDataTopContributeInMonth(int boxType, int assId, int top
            , DateTime dtmClearCacheRedis);

        /// <summary>
        /// <para>Author: DungDA</para>
        /// <para>Date: 22/06/2016</para>
        /// <para>Description: Redis: Set data cho box top đóng góp tháng trang bang hội chi tiết</para>
        /// </summary>
        /// <returns></returns>
        //RedisStatusCode SetBoxDataTopContributeInMonth(int boxType, int assId, int top,DateTime dtmClearCacheRedis, List<Out_RankingInfoDetail_GetBoxData_Result> lstData);

        /// <summary>
        ///     <para>Author: DungDA</para>
        ///     <para>Date: 22/06/2016</para>
        ///     <para>Description: Redis: lấy data cho box top điểm tích lũy trang bang hội chi tiết</para>
        /// </summary>
        /// <returns></returns>
        List<AssMemberTopContributeExpResponseRedis> GetBoxDataTopContributeExp(int boxType, int assId, int top,
            DateTime dtmClearCacheRedis);

        /// <summary>
        /// <para>Author: DungDA</para>
        /// <para>Date: 22/06/2016</para>
        /// <para>Description: Redis: Set data cho box top điểm tích lũy trang bang hội chi tiết</para>
        /// </summary>
        /// <returns></returns>
        //RedisStatusCode SetBoxDataTopContributeExp(int boxType, int assId, int top, DateTime dtmClearCacheRedits,
        //    List<Out_RankingInfoDetail_GetBoxData_Result> lstData);

        /// <summary>
        ///     <para>Author: DungDA</para>
        ///     <para>Date: 23/06/2016</para>
        ///     <para>Description: Redis: lấy data cho box không đăng nhập trang bang hội chi tiết</para>
        /// </summary>
        /// <returns></returns>
        List<AssMemberNotLoginResponseRedis> GetBoxDataNotLogin(int boxType, int assId, int top,
            DateTime dtmClearCacheRedis);

        /// <summary>
        /// <para>Author: DungDA</para>
        /// <para>Date: 23/06/2016</para>
        /// <para>Description: Redis: Set data cho box không đăng nhập trang bang hội chi tiết</para>
        /// </summary>
        /// <returns></returns>
        //RedisStatusCode SetBoxDataNotLogin(int boxType, int assId, int top, DateTime dtmClearCacheRedis,
        //    List<Out_RankingInfoDetail_GetBoxData_Result> lstData);
        /// <summary>
        /// <para>Author: HUYHT</para>
        /// <para>Date: 24/06/2016</para>
        /// <para>Description: Redis: Set data cho box Giới thiệu bang hội chung</para>
        /// </summary>
        /// <returns></returns>
        //RedisStatusCode SetArticles_AssEvent(int categoryId, int rowStart, int rowEnd, out int totalRow,
        //    DateTime dtmClearCacheRedis);

        /// <summary>
        /// <para>Author: HUYHT</para>
        /// <para>Date: 24/06/2016</para>
        /// <para>Description: Redis: Set data cho box Giới thiệu bang hội chung</para>
        /// </summary>
        /// <returns></returns>
        //RedisStatusCode SetBoxDataLastLogin(int boxType, int assId, int top, DateTime dtmClearCacheRedis);

        //RedisStatusCode SetAssociation_GetArticles_AssNew(int top, int rowStart, int rowEnd, DateTime dtmClearCacheRedis);
        //RedisStatusCode SetBoxDataTopContribute(int boxType, int assId, int top, DateTime dtmClearCacheRedis, List<Out_RankingInfoDetail_GetBoxData_Result> lstData);
        //RedisStatusCode SetBoxDataAssNewestArticles(int boxType, int assId, int top, DateTime dtmClearCacheRedis);
    }

    internal class AssociationRedis : IAssociationRedis
    {
        #region BoxDataTopUpdate

        /// <summary>
        ///     <para>Author: DungDA</para>
        ///     <para>Date: 20/05/2016</para>
        ///     <para>Description: Redis: lấy data cho box cập nhật trang bang hội chi tiết</para>
        /// </summary>
        /// <returns></returns>
        public List<AssMemberTopUpdateResponseModelRedis> GetBoxDataTopUpdate(int boxType, int assId, int top,
            DateTime dtmClearCacheRedis)
        {
            var strKey = RedisConstants.GetBoxDataTopUpdate + ":" + assId + ":" + top;
            var lstData = new List<AssMemberTopUpdateResponseModelRedis>();
            var objRedisData = RedisFactory.MyRedis.Get(strKey);
            if (!string.IsNullOrEmpty(objRedisData))
                lstData = RedisCommon.ConvertFromRedis<List<AssMemberTopUpdateResponseModelRedis>>(objRedisData);

            //temp delete
            //if (lstData == null || !lstData.Any())
            //{
            //    SetBoxDataTopUpdate(boxType, assId, top, dtmClearCacheRedis);
            //    objRedisData = RedisFactory.MyRedis.Get(strKey);
            //    if (!string.IsNullOrEmpty(objRedisData))
            //        lstData = RedisCommon.ConvertFromRedis<List<AssMemberTopUpdateResponseModelRedis>>(objRedisData);
            //}

            return lstData;
        }

        /// <summary>
        /// <para>Author: DungDA</para>
        /// <para>Date: 20/05/2016</para>
        /// <para>Description: Redis: Set data cho box cập nhật trang bang hội chi tiết</para>
        /// </summary>
        /// <returns></returns>
        //public RedisStatusCode SetBoxDataTopUpdate(int boxType, int assId, int top, DateTime dtmClearCacheRedis, List<Out_RankingInfoDetail_GetBoxData_Result> lstData)
        //{
        //    var strKey = RedisConstants.GetBoxDataTopUpdate + ":" + assId + ":" + top;
        //    //var lstData = DaoFactory.RankingInfoDetail.GetBoxData(boxType, assId, top);

        //    if (lstData.Any())
        //    {
        //        var listData = lstData.Select(r => new AssMemberTopUpdateResponseModelRedis
        //        {
        //            CreateDate = r.Value2,
        //            UserId = r.ObjectId.GetValueOrDefault(0),
        //            PubUserId = r.Value1,
        //            Content = r.ObjectName,
        //            Nick = r.Value3,
        //        }).ToList();
        //        var lst = RedisCommon.ConvertToRedis(listData);
        //        RedisFactory.MyRedis.Set(strKey, lst, dtmClearCacheRedis);
        //    }

        //    return RedisStatusCode.Success;
        //}

        #endregion

        #region BoxDataLastLogin

        /// <summary>
        ///     <para>Author: DungDA</para>
        ///     <para>Date: 20/05/2016</para>
        ///     <para>Description: Redis: lấy data cho box đăng nhập sau cùng trang bang hội chi tiết</para>
        /// </summary>
        /// <returns></returns>
        public List<AssMemberLastLoginResponseModelRedis> GetBoxDataLastLogin(int boxType, int assId, int top,
            DateTime dtmClearCacheRedis)
        {
            var strKey = RedisConstants.GetBoxDataLastLogin + ":" + assId + ":" + top;
            var lstData = new List<AssMemberLastLoginResponseModelRedis>();
            var objRedisData = RedisFactory.MyRedis.Get(strKey);
            if (!string.IsNullOrEmpty(objRedisData))
                lstData = RedisCommon.ConvertFromRedis<List<AssMemberLastLoginResponseModelRedis>>(objRedisData);

            //temp delete
            //if (lstData == null || !lstData.Any())
            //{
            //    //SetBoxDataLastLogin(boxType, assId, top, dtmClearCacheRedis);
            //    objRedisData = RedisFactory.MyRedis.Get(strKey);
            //    if (!string.IsNullOrEmpty(objRedisData))
            //        lstData = RedisCommon.ConvertFromRedis<List<AssMemberLastLoginResponseModelRedis>>(objRedisData);
            //}

            return lstData;
        }

        /// <summary>
        /// <para>Author: DungDA</para>
        /// <para>Date: 20/05/2016</para>
        /// <para>Description: Redis: Set data cho box đăng nhập sau cùng trang bang hội chi tiết</para>
        /// </summary>
        /// <returns></returns>
        //public RedisStatusCode SetBoxDataLastLogin(int boxType, int assId, int top, DateTime dtmClearCacheRedis)
        //{
        //    var strKey = RedisConstants.GetBoxDataLastLogin + ":" + assId + ":" + top;
        //    var lstData = DaoFactory.RankingInfoDetail.GetBoxData(boxType, assId, top);

        //    if (lstData.Any())
        //    {
        //        var listData = lstData.Select(r => new AssMemberLastLoginResponseModelRedis
        //        {
        //            UserId = string.IsNullOrEmpty(r.Value1) ? 0 : int.Parse(r.Value1),
        //            LastLogin = r.Value2,
        //            PubUserId = r.Value3,
        //            IsOwner = r.IsOwner.GetValueOrDefault(false).ToString(),
        //            IsSubOwner = r.IsSubOwner.GetValueOrDefault(false).ToString(),
        //            NickName = r.ObjectName,

        //            Coin = r.Coin.GetValueOrDefault(0),
        //        }).ToList();
        //        var lst = RedisCommon.ConvertToRedis(listData);
        //        RedisFactory.MyRedis.Set(strKey, lst, dtmClearCacheRedis);
        //    }

        //    return RedisStatusCode.Success;
        //}

        #endregion

        #region BoxDataAssNewestArticles

        /// <summary>
        ///     <para>Author: DungDA</para>
        ///     <para>Date: 23/05/2016</para>
        ///     <para>Description: Redis: lấy data cho box bài mới nhất trang bang hội chi tiết</para>
        /// </summary>
        /// <returns></returns>
        public List<AssMemberNewestArticlesResponseModelRedis> GetBoxDataAssNewestArticles(int boxType, int assId,
            int top, DateTime dtmClearCacheRedis)
        {
            var strKey = RedisConstants.GetBoxDataAssNewestArticles + ":" + assId + ":" + top;
            var lstData = new List<AssMemberNewestArticlesResponseModelRedis>();
            var objRedisData = RedisFactory.MyRedis.Get(strKey);
            if (!string.IsNullOrEmpty(objRedisData))
                lstData = RedisCommon.ConvertFromRedis<List<AssMemberNewestArticlesResponseModelRedis>>(objRedisData);

            //temp delete
            //if (lstData == null || !lstData.Any())
            //{
            //    //SetBoxDataAssNewestArticles(boxType, assId, top, dtmClearCacheRedis);
            //    objRedisData = RedisFactory.MyRedis.Get(strKey);
            //    if (!string.IsNullOrEmpty(objRedisData))
            //        lstData = RedisCommon.ConvertFromRedis<List<AssMemberNewestArticlesResponseModelRedis>>(objRedisData);
            //}
            return lstData;
        }

        /// <summary>
        /// <para>Author: DungDA</para>
        /// <para>Date: 23/05/2016</para>
        /// <para>Description: Redis: Set data cho box bài mới nhất trang bang hội chi tiết</para>
        /// </summary>
        /// <returns></returns>
        //public RedisStatusCode SetBoxDataAssNewestArticles(int boxType, int assId, int top, DateTime dtmClearCacheRedis)
        //{
        //    var strKey = RedisConstants.GetBoxDataAssNewestArticles + ":" + assId + ":" + top;
        //    var lstData = DaoFactory.RankingInfoDetail.GetBoxData(boxType, assId, top);

        //    if (lstData.Any())
        //    {
        //        var listData = lstData.Select(x => new AssMemberNewestArticlesResponseModelRedis
        //        {
        //            ArticleId = string.IsNullOrEmpty(x.Value1) ? 0 : int.Parse(x.Value1),
        //            TextId = x.Value3,
        //            Title = x.ObjectName,

        //        }).ToList();

        //        var lst = RedisCommon.ConvertToRedis(listData);
        //        RedisFactory.MyRedis.Set(strKey, lst, dtmClearCacheRedis);
        //    }

        //    return RedisStatusCode.Success;
        //}

        #endregion

        #region BoxDataTopContribute

        /// <summary>
        ///     <para>Author: DungDA</para>
        ///     <para>Date: 23/05/2016</para>
        ///     <para>Description: Redis: lấy data cho box top đóng góp trang bang hội chi tiết</para>
        /// </summary>
        /// <returns></returns>
        public List<AssMemberTopContributeResponseModelRedis> GetBoxDataTopContribute(int boxType, int assId, int top,
            DateTime dtmClearCacheRedis)
        {
            var strKey = RedisConstants.GetBoxDataTopContribute + ":" + boxType + ":" + assId + ":" + top;
            var lstData = new List<AssMemberTopContributeResponseModelRedis>();
            var objRedisData = RedisFactory.MyRedis.Get(strKey);
            if (!string.IsNullOrEmpty(objRedisData))
                lstData = RedisCommon.ConvertFromRedis<List<AssMemberTopContributeResponseModelRedis>>(objRedisData);

            //temp delete
            //if (lstData == null || !lstData.Any())
            //{
            //    SetBoxDataTopContribute(boxType, assId, top, dtmClearCacheRedis);
            //    objRedisData = RedisFactory.MyRedis.Get(strKey);
            //    if (!string.IsNullOrEmpty(objRedisData))
            //        lstData = RedisCommon.ConvertFromRedis<List<AssMemberTopContributeResponseModelRedis>>(objRedisData);
            //}

            return lstData;
        }

        /// <summary>
        /// <para>Author: DungDA</para>
        /// <para>Date: 23/05/2016</para>
        /// <para>Description: Redis: Set data cho box top đóng góp trang bang hội chi tiết</para>
        /// </summary>
        /// <returns></returns>
        //public RedisStatusCode SetBoxDataTopContribute(int boxType, int assId, int top, DateTime dtmClearCacheRedis, List<Out_RankingInfoDetail_GetBoxData_Result> lstData)
        //{
        //    var strKey = RedisConstants.GetBoxDataTopContribute + ":" + boxType + ":" + assId + ":" + top;
        //    //var lstData = DaoFactory.RankingInfoDetail.GetBoxData(boxType, assId, top);

        //    if (lstData.Any())
        //    {
        //        var listData = lstData.Select(r => new AssMemberTopContributeResponseModelRedis
        //        {
        //            UserId = string.IsNullOrEmpty(r.Value1) ? 0 : int.Parse(r.Value1),
        //            NickName = r.ObjectName,
        //            PubUserId = r.Value3,
        //            Coin = string.IsNullOrEmpty(r.Value2) ? 0 : decimal.Parse(r.Value2),
        //        }).ToList();
        //        var lst = RedisCommon.ConvertToRedis(listData);
        //        RedisFactory.MyRedis.Set(strKey, lst, dtmClearCacheRedis);

        //    }

        //    return RedisStatusCode.Success;
        //}

        #endregion

        #region BoxDataTopContributeExpInMonth

        /// <summary>
        ///     <para>Author: DungDA</para>
        ///     <para>Date: 23/05/2016</para>
        ///     <para>Description: Redis: lấy data cho box top kinh nghiệm tháng trang bang hội chi tiết</para>
        /// </summary>
        /// <returns></returns>
        public List<AssMemberTopContributeExpResponseModelRedis> GetBoxDataTopContributeExpInMonth(int boxType,
            int assId, int top, DateTime dtmClearCacheRedis)
        {
            var strKey = RedisConstants.GetBoxDataTopContributeExpInMonth + ":" + assId + ":" + top;
            var lstData = new List<AssMemberTopContributeExpResponseModelRedis>();
            var objRedisData = RedisFactory.MyRedis.Get(strKey);
            if (!string.IsNullOrEmpty(objRedisData))
                lstData = RedisCommon.ConvertFromRedis<List<AssMemberTopContributeExpResponseModelRedis>>(objRedisData);

            ////temp delete
            //if (lstData == null || !lstData.Any())
            //{
            //    SetBoxDataTopContributeExpInMonth(boxType, assId, top, dtmClearCacheRedis);
            //    objRedisData = RedisFactory.MyRedis.Get(strKey);
            //    if (!string.IsNullOrEmpty(objRedisData))
            //        lstData = RedisCommon.ConvertFromRedis<List<AssMemberTopContributeExpResponseModelRedis>>(objRedisData);
            //}

            return lstData;
        }

        /// <summary>
        /// <para>Author: DungDA</para>
        /// <para>Date: 23/05/2016</para>
        /// <para>Description: Redis: Set data cho box top kinh nghiệm tháng trang bang hội chi tiết</para>
        /// </summary>
        /// <returns></returns>
        //public RedisStatusCode SetBoxDataTopContributeExpInMonth(int boxType, int assId, int top, DateTime dtmClearCacheRedis, List<Out_RankingInfoDetail_GetBoxData_Result> lstData)
        //{
        //    var strKey = RedisConstants.GetBoxDataTopContributeExpInMonth + ":" + assId + ":" + top;
        //    //var lstData = DaoFactory.RankingInfoDetail.GetBoxData(boxType, assId, top);

        //    if (lstData.Any())
        //    {
        //        var listData = lstData.Select(r => new AssMemberTopContributeExpResponseModelRedis
        //        {
        //            UserId = string.IsNullOrEmpty(r.Value1) ? 0 : int.Parse(r.Value1),
        //            NickName = r.ObjectName,
        //            PubUserId = r.Value3,
        //            Exp = string.IsNullOrEmpty(r.Value2) ? 0 : decimal.Parse(r.Value2),
        //            AssociationId = r.ObjectId.GetValueOrDefault(0),
        //            Total = (int)(r.Total.HasValue ? r.Total.Value : 0),
        //        }).ToList();
        //        var lst = RedisCommon.ConvertToRedis(listData);
        //        RedisFactory.MyRedis.Set(strKey, lst, dtmClearCacheRedis);
        //    }

        //    return RedisStatusCode.Success;
        //}

        #endregion

        #region Box tin nhanh bang hội

        /// <summary>
        ///     <para>Author: DungDA</para>
        ///     <para>Date: 06/06/2016</para>
        ///     <para>Description: Redis: Lấy data cho box tin nhanh bang hội trang bang hội chung</para>
        /// </summary>
        /// <returns></returns>
        public List<Association_GetArticles_AssNewRedis> GetAssociation_GetArticles_AssNew(int top, int rowStart,
            int rowEnd, DateTime dtmClearCacheRedis)
        {
            var strKey = RedisConstants.GetAssociationGetArticlesAssNew + ":" + top + ":" + rowStart + ":" + rowEnd;
            var lstData = new List<Association_GetArticles_AssNewRedis>();
            var objRedisData = RedisFactory.MyRedis.Get(strKey);
            if (!string.IsNullOrEmpty(objRedisData))
                lstData = RedisCommon.ConvertFromRedis<List<Association_GetArticles_AssNewRedis>>(objRedisData);

            //temp delete
            //if (lstData == null || !lstData.Any())
            //{
            //    SetAssociation_GetArticles_AssNew(top, rowStart, rowEnd, dtmClearCacheRedis);
            //    objRedisData = RedisFactory.MyRedis.Get(strKey);
            //    if (!string.IsNullOrEmpty(objRedisData))
            //        lstData = RedisCommon.ConvertFromRedis<List<Association_GetArticles_AssNewRedis>>(objRedisData);
            //}

            return lstData;
        }

        /// <summary>
        /// <para>Author: DungDA</para>
        /// <para>Date: 06/06/2016</para>
        /// <para>Description: Redis: Set data cho box tin nhanh bang hội trang bang hội chung</para>
        /// </summary>
        /// <returns></returns>
        //public RedisStatusCode SetAssociation_GetArticles_AssNew(int top, int rowStart, int rowEnd, DateTime dtmClearCacheRedis)
        //{
        //    var strKey = RedisConstants.GetAssociationGetArticlesAssNew + ":" + top + ":" + rowStart + ":" + rowEnd;
        //    var lstData = DaoFactory.Association.GetArticles_AssNew(top, rowStart, rowEnd);

        //    if (lstData.Any())
        //    {
        //        var listData = lstData.Select(r => new Association_GetArticles_AssNewRedis
        //        {
        //            AssNameID = r.AssNameID,
        //            PubUserID = r.PubUserID.GetValueOrDefault(0),
        //            UserID = r.UserID.GetValueOrDefault(0),
        //            AssociationID = r.AssociationID.GetValueOrDefault(0),
        //            CreateDate = r.CreateDate.GetValueOrDefault().ToString("yyyy-MM-dd HH:mm:ss.fff"),
        //            Name = r.Name,
        //            NickName = r.NickName,
        //            STT = r.STT.GetValueOrDefault(),
        //            Comment = r.Comment,
        //            ContCoin = r.ContCoin.GetValueOrDefault(0),
        //            ContExp = r.ContCoin.GetValueOrDefault(),
        //            Emotion = r.Emotion,
        //            IsOwner = r.IsOwner.GetValueOrDefault(),
        //            IsSubOwner = r.IsSubOwner.GetValueOrDefault()
        //        }).ToList();
        //        var lst = RedisCommon.ConvertToRedis(listData);
        //        RedisFactory.MyRedis.Set(strKey, lst, dtmClearCacheRedis);
        //    }

        //    return RedisStatusCode.Success;
        //}

        #endregion

        #region danh sach bang hoi - VanNL

        public List<Association_AssociationMember_Redis> GetAssociationMember(string firstLetter, int startIndex,
            int pageLength, DateTime dtmClearCacheRedis, out int totalRow)
        {
            string strKey;
            if (firstLetter == string.Empty || firstLetter == "Tất cả")
                strKey = RedisConstants.GetAssociationMember + ":" + "All" + ":" + startIndex;
            else
                strKey = RedisConstants.GetAssociationMember + ":" + firstLetter + ":" + startIndex;
            var lstData = new List<Association_AssociationMember_Redis>();

            var objRedisData = RedisFactory.MyRedis.Get(strKey);
            if (!string.IsNullOrEmpty(objRedisData))
                lstData = RedisCommon.ConvertFromRedis<List<Association_AssociationMember_Redis>>(objRedisData);

            totalRow = lstData.Select(x => x.TotalRow).FirstOrDefault();
            //foreach (var key in objRedisData)
            //{
            //    var strLst = RedisFactory.MyRedis.Get(key);
            //    if (!string.IsNullOrEmpty(strLst))
            //    {
            //        var lst = RedisCommon.ConvertFromRedis<List<Association_AssociationMember_Redis>>(strLst);
            //        if (lst.Any())
            //        {
            //            lstData.AddRange(lst);
            //        }
            //    }
            //}

            //if (!string.IsNullOrEmpty(objRedisData))
            //    lstData = RedisCommon.ConvertFromRedis<List<Association_AssociationMember_Redis>>(objRedisData);

            //temp delete
            if (lstData.Any())
            {
                //SetAssociationMember(firstLetter, startIndex, dtmClearCacheRedis);
                var intTotalRow = lstData.Count;
                //lstData = lstData.Take(startIndex).Take()
            }
            else
            {
                var firstRecord = lstData.FirstOrDefault();
                //totalRow = firstRecord != null ? firstRecord.TotalRow : 0;
            }

            //totalRow = 0;
            return lstData;
        }

        //public RedisStatusCode SetAssociationMember(string firstLetter, int startIndex, DateTime dtmClearCacheRedis, List<Out_Association_Search_Result> lstData,int totalRow)
        //{
        //    var strKey = RedisConstants.GetAssociationMember + ":" + firstLetter + ":" + startIndex;
        //    //var lstData = DaoFactory.Association.Search(firstLetter, keyword, isActive, startIndex, pageLength, out totalRow);

        //    if (lstData.Any())
        //    {
        //        var listData = lstData.Select(r => new Association_AssociationMember_Redis
        //        {
        //            AssociationID = r.AssociationID.HasValue ? r.AssociationID.Value : 0,
        //            AssNameID = r.AssNameID,
        //            Name = r.Name,
        //            ImagePath = r.ImagePath,
        //            LevelNo = r.LevelNo.HasValue ? r.LevelNo.Value : 0,
        //            AssCoin = r.AssCoin.HasValue ? r.AssCoin.Value : 0,
        //            OwnerNickName = r.OwnerNickName,
        //            MemberCount = r.MemberCount.HasValue ? r.MemberCount.Value : 0,
        //            PubUserID = r.PubUserID.HasValue ? r.PubUserID.Value : 0,
        //            RowNumber = r.RowNumber.HasValue ? r.RowNumber.Value : 0,
        //            TotalRow = totalRow,
        //        }).ToList();
        //        string lst = RedisCommon.ConvertToRedis(listData);
        //        RedisFactory.MyRedis.Set(strKey, lst, dtmClearCacheRedis);
        //    }
        //    return RedisStatusCode.Success;
        //}

        #endregion

        #region Box Sự kiện bang hội

        /// <summary>
        ///     <para>Author: DungDA</para>
        ///     <para>Date: 09/06/2016</para>
        ///     <para>Description: Redis: Lấy data cho box Sự kiện bang hội</para>
        /// </summary>
        /// <returns></returns>
        public List<Association_GetArticles_AssEvent_Redis> GetArticles_AssEvent(int categoryID, int rowStart,
            int rowEnd, out int totalRow, DateTime dtmClearCacheRedis)
        {
            var strKey = RedisConstants.GetArticlesAssEvent + ":" + categoryID + ":" + rowStart + ":" + rowEnd;
            var lstData = new List<Association_GetArticles_AssEvent_Redis>();
            var objRedisData = RedisFactory.MyRedis.Get(strKey);
            if (!string.IsNullOrEmpty(objRedisData))
                lstData = RedisCommon.ConvertFromRedis<List<Association_GetArticles_AssEvent_Redis>>(objRedisData);

            //temp delete
            if (lstData == null || !lstData.Any())
            {
                //SetArticles_AssEvent(categoryID, rowStart, rowEnd, out totalRow, dtmClearCacheRedis);
                objRedisData = RedisFactory.MyRedis.Get(strKey);
                if (!string.IsNullOrEmpty(objRedisData))
                    lstData = RedisCommon.ConvertFromRedis<List<Association_GetArticles_AssEvent_Redis>>(objRedisData);
            }

            totalRow = 0;
            if (lstData != null && lstData.Any())
            {
                var firstRecord = lstData.FirstOrDefault();
                totalRow = firstRecord != null ? firstRecord.TotalRow : 0;
            }

            return lstData;
        }

        /// <summary>
        /// <para>Author: DungDA</para>
        /// <para>Date: 09/06/2016</para>
        /// <para>Description: Redis: Set data cho box Sự kiện bang hội</para>
        /// </summary>
        /// <returns></returns>
        //public RedisStatusCode SetArticles_AssEvent(int categoryId, int rowStart, int rowEnd, out int totalRow, DateTime dtmClearCacheRedis)
        //{
        //    var strKey = RedisConstants.GetArticlesAssEvent + ":" + categoryId + ":" + rowStart + ":" + rowEnd ;

        //    var lstData = DaoFactory.Association.GetArticles_AssEvent(categoryId, rowStart, rowEnd, out totalRow);

        //    if (lstData.Any())
        //    {
        //        var listData = lstData.Select(d => new Association_GetArticles_AssEvent_Redis
        //        {
        //            ArticleID = d.ArticleID.GetValueOrDefault(0),
        //            AssImagePath = d.AssImagePath,
        //            AssNameID = d.AssNameID,
        //            AssociationID = d.AssociationID.GetValueOrDefault(0),
        //            Body = d.Body,
        //            CategoryID = d.CategoryID.GetValueOrDefault(0),
        //            CreateDate = d.CreateDate.GetValueOrDefault().ToString("yyyy-MM-dd HH:mm:ss.fff"),
        //            EmotionPath = d.EmotionPath,
        //            ImageLink = d.ImageLink,
        //            Name = d.Name,
        //            NickName = d.NickName,
        //            PubUserID = d.PubUserID.GetValueOrDefault(0),
        //            STT = d.STT.GetValueOrDefault(0),
        //            ShortDescription = d.ShortDescription,
        //            Status = d.Status.GetValueOrDefault(0),
        //            TextID = d.TextID,
        //            Title = d.Title,
        //            TotalRow = d.TotalRow.GetValueOrDefault(0),
        //            UpdateDate = d.UpdateDate.GetValueOrDefault().ToString("yyyy-MM-dd HH:mm:ss.fff"),
        //            UserID = d.UserID.GetValueOrDefault(0),
        //            ViewCount = d.ViewCount.GetValueOrDefault(0),
        //        }).ToList();
        //        var lst = RedisCommon.ConvertToRedis(listData);
        //        RedisFactory.MyRedis.Set(strKey, lst, dtmClearCacheRedis);
        //    }

        //    return RedisStatusCode.Success;
        //}

        #endregion

        #region BoxDataTopContributeExpInMonth

        /// <summary>
        ///     <para>Author: DungDA</para>
        ///     <para>Date: 22/06/2016</para>
        ///     <para>Description: Redis: lấy data cho box top đóng góp tháng trang bang hội chi tiết</para>
        /// </summary>
        /// <returns></returns>
        public List<TopMonthContributorRedis> GetBoxDataTopContributeInMonth(int boxType, int assId, int top,
            DateTime dtmClearCacheRedis)
        {
            var strKey = RedisConstants.GetBoxDataTopContributeInMonth + ":" + assId + ":" + top;
            var lstData = new List<TopMonthContributorRedis>();
            var objRedisData = RedisFactory.MyRedis.Get(strKey);
            if (!string.IsNullOrEmpty(objRedisData))
                lstData = RedisCommon.ConvertFromRedis<List<TopMonthContributorRedis>>(objRedisData);

            return lstData;
        }


        /// <summary>
        /// <para>Author: DungDA</para>
        /// <para>Date: 22/06/2016</para>
        /// <para>Description: Redis: Set data cho box top đóng góp tháng trang bang hội chi tiết</para>
        /// </summary>
        /// <returns></returns>
        //public RedisStatusCode SetBoxDataTopContributeInMonth(int boxType, int assId, int top, DateTime dtmClearCacheRedis, List<Out_RankingInfoDetail_GetBoxData_Result> lstData)
        //{
        //    var strKey = RedisConstants.GetBoxDataTopContributeInMonth + ":" + assId + ":" + top;
        //    //var lstData = DaoFactory.RankingInfoDetail.GetBoxData(boxType, assId, top);

        //    if (lstData.Any())
        //    {
        //        var listData = lstData.Select(r => new TopMonthContributorRedis
        //        {
        //            UserId = string.IsNullOrEmpty(r.Value1) ? 0 : int.Parse(r.Value1),
        //            NickName = r.ObjectName,
        //            PubUserId = string.IsNullOrEmpty(r.Value3) ? 0 : int.Parse(r.Value3),
        //            Coin = string.IsNullOrEmpty(r.Value2) ? 0 : decimal.Parse(r.Value2),
        //            Total = r.Total.GetValueOrDefault()
        //        }).ToList();

        //        var lst = RedisCommon.ConvertToRedis(listData);
        //        RedisFactory.MyRedis.Set(strKey, lst, dtmClearCacheRedis);
        //    }

        //    return RedisStatusCode.Success;
        //}

        #endregion

        #region BoxDataTopContributeExp

        /// <summary>
        ///     <para>Author: DungDA</para>
        ///     <para>Date: 22/06/2016</para>
        ///     <para>Description: Redis: lấy data cho box top điểm tích lũy trang bang hội chi tiết</para>
        /// </summary>
        /// <returns></returns>
        public List<AssMemberTopContributeExpResponseRedis> GetBoxDataTopContributeExp(int boxType, int assId, int top,
            DateTime dtmClearCacheRedis)
        {
            var strKey = RedisConstants.GetBoxDataTopContributeExp + ":" + assId + ":" + top;
            var lstData = new List<AssMemberTopContributeExpResponseRedis>();
            var objRedisData = RedisFactory.MyRedis.Get(strKey);
            if (!string.IsNullOrEmpty(objRedisData))
                lstData = RedisCommon.ConvertFromRedis<List<AssMemberTopContributeExpResponseRedis>>(objRedisData);

            return lstData;
        }


        /// <summary>
        /// <para>Author: DungDA</para>
        /// <para>Date: 22/06/2016</para>
        /// <para>Description: Redis: Set data cho box top điểm tích lũy trang bang hội chi tiết</para>
        /// </summary>
        /// <returns></returns>
        //public RedisStatusCode SetBoxDataTopContributeExp(int boxType, int assId, int top, DateTime dtmClearCacheRedis, List<Out_RankingInfoDetail_GetBoxData_Result> lstData)
        //{
        //    var strKey = RedisConstants.GetBoxDataTopContributeExp + ":" + assId + ":" + top;
        //    //var lstData = DaoFactory.RankingInfoDetail.GetBoxData(boxType, assId, top);

        //    if (lstData.Any())
        //    {
        //        var listData = lstData.Select(r => new AssMemberTopContributeExpResponseRedis
        //        {
        //            UserId = string.IsNullOrEmpty(r.Value1) ? 0 : int.Parse(r.Value1),
        //            NickName = r.ObjectName,
        //            PubUserId = string.IsNullOrEmpty(r.Value3) ? 0 : int.Parse(r.Value3),
        //            Exp = string.IsNullOrEmpty(r.Value2) ? 0 : decimal.Parse(r.Value2),
        //            AssociationId = r.ObjectId.GetValueOrDefault(0),
        //        }).ToList();

        //        var lst = RedisCommon.ConvertToRedis(listData);
        //        RedisFactory.MyRedis.Set(strKey, lst, dtmClearCacheRedis);
        //    }

        //    return RedisStatusCode.Success;
        //}

        #endregion

        #region BoxDataNotLogin

        /// <summary>
        ///     <para>Author: DungDA</para>
        ///     <para>Date: 23/06/2016</para>
        ///     <para>Description: Redis: lấy data cho box không đăng nhập trang bang hội chi tiết</para>
        /// </summary>
        /// <returns></returns>
        public List<AssMemberNotLoginResponseRedis> GetBoxDataNotLogin(int boxType, int assId, int top,
            DateTime dtmClearCacheRedis)
        {
            var strKey = RedisConstants.GetBoxDataNotLogin + ":" + assId + ":" + top;
            var lstData = new List<AssMemberNotLoginResponseRedis>();
            var objRedisData = RedisFactory.MyRedis.Get(strKey);
            if (!string.IsNullOrEmpty(objRedisData))
                lstData = RedisCommon.ConvertFromRedis<List<AssMemberNotLoginResponseRedis>>(objRedisData);

            return lstData;
        }


        /// <summary>
        /// <para>Author: DungDA</para>
        /// <para>Date: 23/06/2016</para>
        /// <para>Description: Redis: Set data cho box không đăng nhập trang bang hội chi tiết</para>
        /// </summary>
        /// <returns></returns>
        //public RedisStatusCode SetBoxDataNotLogin(int boxType, int assId, int top, DateTime dtmClearCacheRedis, List<Out_RankingInfoDetail_GetBoxData_Result> lstData)
        //{
        //    var strKey = RedisConstants.GetBoxDataNotLogin + ":" + assId + ":" + top;
        //    //var lstData = DaoFactory.RankingInfoDetail.GetBoxData(boxType, assId, top);

        //    if (lstData.Any())
        //    {
        //        var listData = lstData.Select(r => new AssMemberNotLoginResponseRedis
        //        {
        //            UserId = string.IsNullOrEmpty(r.Value1) ? 0 : int.Parse(r.Value1),
        //            IsOwner = r.IsOwner.GetValueOrDefault(false),
        //            IsSubOwner = r.IsSubOwner.GetValueOrDefault(false),
        //            NickName = r.ObjectName,
        //            LastLoginUser = string.IsNullOrEmpty(r.Value2) ? DateTime.Parse("1900-01-01 00:00:00") : DateTime.Parse(r.Value2),
        //            PubUserId = string.IsNullOrEmpty(r.Value3) ? 0 : int.Parse(r.Value3),
        //        }).ToList();

        //        var lst = RedisCommon.ConvertToRedis(listData);
        //        RedisFactory.MyRedis.Set(strKey, lst, dtmClearCacheRedis);
        //    }

        //    return RedisStatusCode.Success;
        //}

        #endregion

        #region Box bang hội mạnh trong tháng

        /// <summary>
        ///     <para>Author: DungDA</para>
        ///     <para>Date: 07/06/2016</para>
        ///     <para>Description: Redis: Lấy data cho box bang hội mạnh trong tháng</para>
        /// </summary>
        /// <returns></returns>
        public List<Association_GetTopMaxInMonth_Redis> GetAssociation_GetTopMaxInMonth(int top, int keyword,
            DateTime dtmClearCacheRedis)
        {
            var strKey = RedisConstants.GetTopAssociationUpLevelInMonth + ":" + top + ":" + keyword;
            var lstData = new List<Association_GetTopMaxInMonth_Redis>();
            var objRedisData = RedisFactory.MyRedis.Get(strKey);
            if (!string.IsNullOrEmpty(objRedisData))
                lstData = RedisCommon.ConvertFromRedis<List<Association_GetTopMaxInMonth_Redis>>(objRedisData);

            //temp delete
            //if (lstData == null || !lstData.Any())
            //{
            //    SetAssociation_GetTopMaxInMonth(top, keyword, dtmClearCacheRedis);
            //    objRedisData = RedisFactory.MyRedis.Get(strKey);
            //    if (!string.IsNullOrEmpty(objRedisData))
            //        lstData = RedisCommon.ConvertFromRedis<List<Association_GetTopMaxInMonth_Redis>>(objRedisData);
            //}

            return lstData;
        }

        /// <summary>
        ///     <para>Author: DungDA</para>
        ///     <para>Date: 07/06/2016</para>
        ///     <para>Description: Redis: Set data cho box bang hội mạnh trong tháng</para>
        /// </summary>
        /// <returns></returns>
        //public RedisStatusCode SetAssociation_GetTopMaxInMonth(int top, int keyword, DateTime dtmClearCacheRedis, List<Out_Association_GetTopMaxInMonth_Result> lstData )
        //{
        //    var strKey = RedisConstants.GetTopAssociationUpLevelInMonth + ":" + top + ":" + keyword;
        //    //var lstData = DaoFactory.Association.TopAssociationInMonth(top, keyword);

        //    if (lstData.Any())
        //    {
        //        var listData = lstData.Select(r => new Association_GetTopMaxInMonth_Redis
        //        {
        //            ID = r.ID.GetValueOrDefault(),
        //            BoxType = r.BoxType,
        //            Key = r.Key,
        //            ObjectId = r.ObjectId.GetValueOrDefault(),
        //            ObjectName = r.ObjectName,
        //            Value1 = r.Value1.GetValueOrDefault(),
        //            Value2 = r.Value2,
        //            CreateDate = r.CreateDate.GetValueOrDefault().ToString("yyyy-MM-dd HH:mm:ss.fff"),
        //            ImagePath = r.ImagePath,
        //            AssNameID = r.AssNameID,
        //            PubUserID = r.PubUserID.GetValueOrDefault(0),
        //            UserID = r.UserID,
        //            NickName = r.NickName,
        //            ExpNo = r.ExpNo.GetValueOrDefault(),
        //            LevelNo = r.LevelNo.GetValueOrDefault(),

        //        }).ToList();
        //        var lst = RedisCommon.ConvertToRedis(listData);
        //        RedisFactory.MyRedis.Set(strKey, lst, dtmClearCacheRedis);
        //    }

        //    return RedisStatusCode.Success;
        //}

        #endregion

        //#region
        //public List<Association_TopAssociationInMonth_Redis> GetTopAssociationUpLevelInMonth(int top, int keyword, DateTime dtmClearCacheRedis)
        //{
        //    var strKey = RedisConstants.GetTopAssociationUpLevelInMonth + ":" + top + ":" + keyword;
        //    var lstData = new List<Association_TopAssociationInMonth_Redis>();
        //    var objRedisData = RedisFactory.MyRedis.Get(strKey);
        //    if (!string.IsNullOrEmpty(objRedisData))
        //        lstData = RedisCommon.ConvertFromRedis<List<Association_TopAssociationInMonth_Redis>>(objRedisData);

        //    //temp delete
        //    //if (lstData == null || !lstData.Any())
        //    //    SetAssociation_GetTopMaxInMonth(top, keyword, dtmClearCacheRedis);

        //    return lstData;

        //}

        //public RedisStatusCode SetTopAssociationUpLevelInMonth(int top, int keyword, DateTime dtmClearCacheRedis)
        //{
        //    var strKey = RedisConstants.GetTopAssociationUpLevelInMonth + ":" + top + ":" + keyword;
        //    var lstData = DaoFactory.Association.TopAssociationInMonth(top, keyword);

        //    if (lstData.Any())
        //    {
        //        string lst = RedisCommon.ConvertToRedis(lstData);
        //        RedisFactory.MyRedis.Set(strKey, lst, dtmClearCacheRedis);
        //    }

        //    return RedisStatusCode.Success;
        //}

        //#endregion

        #region top phu bang - VanNL

        public List<Association_TopAssociationTopCoin_Redis> GetTopAssociationTopCoin(int pageLength,
            DateTime dtmClearCacheRedis)
        {
            var strKey = RedisConstants.GetTopAssociationTopCoin + ":" + pageLength;
            var lstData = new List<Association_TopAssociationTopCoin_Redis>();
            var objRedisData = RedisFactory.MyRedis.Get(strKey);
            if (!string.IsNullOrEmpty(objRedisData))
                lstData = RedisCommon.ConvertFromRedis<List<Association_TopAssociationTopCoin_Redis>>(objRedisData);

            //temp delete
            //if (lstData == null || !lstData.Any())
            //    SetTopAssociationTopCoin(pageLength, dtmClearCacheRedis);

            return lstData;
        }

        //public RedisStatusCode SetTopAssociationTopCoin(int pageLength, DateTime dtmClearCacheRedis, List<Out_Association_GetTopCoin_Result> lstData )
        //{
        //    var strKey = RedisConstants.GetTopAssociationTopCoin + ":" + pageLength;
        //    //var lstData = DaoFactory.Association.GetTopCoin(pageLength);

        //    if (lstData.Any())
        //    {
        //        var listData = lstData.Select(r => new Association_TopAssociationTopCoin_Redis
        //        {
        //            AssociationID = r.AssociationID,
        //            AssNameID = r.AssNameID,
        //            Name = r.Name,
        //            ImagePath = r.ImagePath,
        //            LevelNo = r.LevelNo.HasValue ? r.LevelNo.Value : 0,
        //            MemberCount = r.MemberBonus,
        //            OwnerNickName = r.OwnerNickName,
        //            AssCoin = r.AssCoin

        //        }).ToList();
        //        string lst = RedisCommon.ConvertToRedis(listData);
        //        RedisFactory.MyRedis.Set(strKey, lst, dtmClearCacheRedis);
        //    }

        //    return RedisStatusCode.Success;
        //}

        #endregion
    }
}