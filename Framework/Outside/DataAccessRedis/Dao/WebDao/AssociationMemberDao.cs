using System.Collections.Generic;
using DataAccessRedis.Constant;
using DataAccessRedis.Infrastructure;
using DataAccessRedis.Model;
using Newtonsoft.Json;
//using DataAccess;
//using MyConfig;

namespace DataAccessRedis.Dao.WebDao
{
    public interface IAssociationMemberDao
    {
        /// <summary>
        ///     Lấy top thành viên có nhiều kinh nghiệm
        ///     Author: HuyHt
        /// </summary>
        /// <param name="associationId"></param>
        /// <param name="pageLength"></param>
        /// <returns></returns>
        List<AssociationMemberGetTopExpUsers> GetTopExpUsers(int associationId, int pageLength);
        //List<TopMonthContributor> GetTopMonth(int associationId, int pageLength,out decimal total);
        //void SetValuToRankingDetail(int boxType, DateTime dtmClearCacheRedis);
    }

    internal class AssociationMemberDao : IAssociationMemberDao
    {
        public IRedisRepository _redisRepository { get; set; }

        public List<AssociationMemberGetTopExpUsers> GetTopExpUsers(int associationId, int pageLength)
        {
            var lstData = new List<AssociationMemberGetTopExpUsers>();
            var strKey = RedisConstants.RankingDetailBox + ":" + 4;
            var objRedisData = _redisRepository.Get(strKey);
            if (!string.IsNullOrEmpty(objRedisData))
                lstData = JsonConvert.DeserializeObject<List<AssociationMemberGetTopExpUsers>>(objRedisData);
            return lstData;
        }
        //public void SetValuToRankingDetail(int boxType, DateTime dtmClearCacheRedis)
        //{
        //    var strKey = RedisConstants.RankingDetailBox + ":" + boxType;

        //    var lstData = DaoFactory.RankingInfoDetail.GetBoxDataRedis(boxType).ToList();
        //    if (lstData.Any())
        //        RedisFactory.MyRedis.Set(strKey, JsonConvert.SerializeObject(lstData), dtmClearCacheRedis);
        //}
        //public List<TopMonthContributor> GetTopMonth(int associationId, int pageLength, out decimal total)
        //{
        //    var lstData = new List<TopMonthContributor>();
        //    var strKey = RedisConstants.GetTopDongGopMonth + ":" + associationId;
        //    var objRedisData = _redisRepository.Get(strKey);
        //    if (!string.IsNullOrEmpty(objRedisData))
        //        lstData = JsonConvert.DeserializeObject<List<TopMonthContributor>>(objRedisData);
        //    total = lstData.Count();
        //    return lstData;
        //}
    }
}