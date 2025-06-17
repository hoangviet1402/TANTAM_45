using System.Collections.Generic;
using DataAccessMongo.Model.Defender;

namespace DataAccessMongo.Module.Defender
{
    public interface IDefender
    {
        List<SM_UserInfoModel> GetUserGun(int userID);
        long ActiveGetUserGun(int userId, List<UserGunInfo> listGun);
        long ActiveUserSKill(int userId, List<UserSkillInfo> listSkill);
        long UpgradeUserGun(int userId, int gunKind, int level, int exp);

        long UpgradeUserMineral(int userId, List<UserMineralInfo> listMineral);
        long CreateUserMineral(int userId, List<UserMineralInfo> listMineral);

        long UpgradeUserMaterial(int userId, List<UserMaterialInfo> listMaterial);
        long InsertUserMaterial(int userId, List<UserMaterialInfo> listMaterial);

        void InsertUserGun(SM_UserInfoModel user);
        void InsertUserDefaultGun(int userId, List<UserGunInfo> listGun);
        void InsertUserDefaultSkill(int userId, List<UserSkillInfo> listSkill);
        void InsertUserDefaultTower(int userId, List<UserTowerInfo> listTower);
        long UpgradeUserTower(int userId, int towerKind, int level, int exp);
    }
}