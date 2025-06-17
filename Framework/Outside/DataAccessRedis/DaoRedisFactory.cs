/**********************************************************************
 * Author: HuyHT
 * DateCreate: 06-25-2014
 * Description: DaoFactory
 * ####################################################################
 * Author:......................
 * DateModify: .................
 * Description: ................
 *
 *********************************************************************/

using DataAccessRedis.Dao.Achievement;
using DataAccessRedis.Dao.EventDao;
using DataAccessRedis.Dao.Minigame;
using DataAccessRedis.Dao.ServerGame;
using DataAccessRedis.Dao.SystemManagementDao;
using DataAccessRedis.Dao.WebDao;
using IAssociationMemberDao = DataAccessRedis.Dao.WebDao.IAssociationMemberDao;

namespace DataAccessRedis
{
    public class DaoRedisFactory
    {
        public static IAssociationMemberDao AssociationMember => new AssociationMemberDao();

        public static IAssociationRedis AssociationRedis => new AssociationRedis();

        public static IAccountRedis AccountRedis => new AccountRedis();

        public static IEventInfoDao EventInfoRedis => new EventInfoDao();

        public static IBanCaRedis ServerBanCaRedis => new BanCaRedis();

        public static IJackPot JackPotRedis => new JackPot();

        public static IAchievement AchievementRedis => new Achievement();

        public static IE1810_FreeSpin E1810FreeSpinRedis => new E1810_FreeSpin();

        public static IE1806_CardSpin E1806CardSpinRedis => new E1806_CardSpin();

        public static IServerGame ServerGame => new ServerGame();

        public static IFriendRedisDao FriendRedis => new FriendRedisDao();
    }
}