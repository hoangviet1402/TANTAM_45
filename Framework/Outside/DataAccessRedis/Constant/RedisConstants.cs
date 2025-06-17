/**********************************************************************
 * Author: HuyHT
 * DateCreate: 06-25-2014
 * Description: RedisContant
 * ####################################################################
 * Author:......................
 * DateModify: .................
 * Description: Dinh nghia redis key bang 3 chu cai dau tien
 *
 *********************************************************************/

namespace DataAccessRedis.Constant
{
    public class RedisConstants
    {
        public const string RankingDetailBox = "rdb";
        public const string GetBoxDataTopContributeInMonth = "gbm";
        public const string GetBoxDataTopUpdate = "gbu";
        public const string GetBoxDataLastLogin = "gbl";
        public const string GetBoxDataAssNewestArticles = "gba";
        public const string GetBoxDataTopContribute = "gbc";
        public const string GetBoxDataTopContributeExpInMonth = "gbe";

        public const string GetAssociationGetArticlesAssNew = "gga";

        //public const string GetAssociationGetTopMaxInMonth = "ggi";
        public const string GetTopAssociationUpLevelInMonth = "ggi";
        public const string GetTopAssociationTopCoin = "gttc";
        public const string GetArticlesAssEvent = "gaa";
        public const string GetListUserNewOrOld = "glu";
        public const string GetAssociationMember = "gam";
        public const string GetBirthdayUsers = "gbd";
        public const string GetBoxDataTopContributeExp = "gce";
        public const string GetBoxDataNotLogin = "gnl";
        public const string GetTopAssociationAwardInMonth = "gaam";
    }

    public class EventRedisConstant
    {
        public const string GetEventRunning = "ger";
    }

    public enum RedisStatusCode
    {
        Fail,
        Success,
        SystemError,
        TimeOut
    }

    public class RedisConstantsEvent
    {
        public const string EventNoHu = "enohu";
        public const string EventNoHuTotal = "enohu_total";

        public const string EventNoHuMini = "enohumini";
        public const string EventNoHuTotalMini = "enohumini_total";
    }
}