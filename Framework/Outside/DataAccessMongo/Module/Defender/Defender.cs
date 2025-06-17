using System;
using System.Collections.Generic;
using DataAccessMongo.Base;
using DataAccessMongo.Enum;
using DataAccessMongo.Model.Defender;
using MongoDB.Driver;

namespace DataAccessMongo.Module.Defender
{
    public class Defender : IDefender
    {
        public List<SM_UserInfoModel> GetUserGun(int userID)
        {
            if (!MongoFactory.MongoEnable)
                return new List<SM_UserInfoModel>();

            if (userID <= 0)
                return null;

            var collection = GetCollection();
            var builder = Builders<SM_UserInfoModel>.Filter;

            var filter = builder.And(
                builder.Eq("UserID", userID)
            );
            return collection.Find(filter).ToList();
        }

        public long ActiveGetUserGun(int userId, List<UserGunInfo> listGun)
        {
            if (!MongoFactory.MongoEnable)
                return 0;

            var collection = GetCollection();
            var builder = Builders<SM_UserInfoModel>.Filter;


            var filter =
                Builders<SM_UserInfoModel>.Filter.And(Builders<SM_UserInfoModel>.Filter.Eq(d => d.UserID, userId));

            var update = Builders<SM_UserInfoModel>.Update.Set(c => c.ListGun, listGun);

            try
            {
                var result = collection.UpdateOne(
                    filter,
                    update,
                    new UpdateOptions { IsUpsert = false }
                );
                return result.IsAcknowledged ? result.ModifiedCount : 0;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public long ActiveUserSKill(int userId, List<UserSkillInfo> listSkill)
        {
            if (!MongoFactory.MongoEnable)
                return 0;

            var collection = GetCollection();
            var builder = Builders<SM_UserInfoModel>.Filter;


            var filter =
                Builders<SM_UserInfoModel>.Filter.And(Builders<SM_UserInfoModel>.Filter.Eq(d => d.UserID, userId));

            var update = Builders<SM_UserInfoModel>.Update.Set(c => c.ListSkill, listSkill);

            try
            {
                var result = collection.UpdateOne(
                    filter,
                    update,
                    new UpdateOptions { IsUpsert = false }
                );
                return result.IsAcknowledged ? result.ModifiedCount : 0;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public long UpgradeUserMineral(int userId, List<UserMineralInfo> listMineral)
        {
            if (!MongoFactory.MongoEnable)
                return 0;

            var collection = GetCollection();
            var builder = Builders<SM_UserInfoModel>.Filter;


            var filter =
                Builders<SM_UserInfoModel>.Filter.And(Builders<SM_UserInfoModel>.Filter.Eq(d => d.UserID, userId));
            var update = Builders<SM_UserInfoModel>.Update.Set(c => c.ListMineral, listMineral);
            try
            {
                var result = collection.UpdateOne(
                    filter,
                    update,
                    new UpdateOptions { IsUpsert = false }
                );
                return result.IsAcknowledged ? result.ModifiedCount : 0;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public long CreateUserMineral(int userId, List<UserMineralInfo> listMineral)
        {
            if (!MongoFactory.MongoEnable)
                return 0;

            var collection = GetCollection();
            var builder = Builders<SM_UserInfoModel>.Filter;


            var filter =
                Builders<SM_UserInfoModel>.Filter.And(Builders<SM_UserInfoModel>.Filter.Eq(d => d.UserID, userId));
            var update = Builders<SM_UserInfoModel>.Update.Set(c => c.ListMineral, listMineral);
            try
            {
                var result = collection.UpdateOne(
                    filter,
                    update,
                    new UpdateOptions { IsUpsert = false }
                );
                return result.IsAcknowledged ? result.ModifiedCount : 0;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public long InsertUserMaterial(int userId, List<UserMaterialInfo> listMaterial)
        {
            if (!MongoFactory.MongoEnable)
                return 0;

            var collection = GetCollection();
            var builder = Builders<SM_UserInfoModel>.Filter;


            var filter =
                Builders<SM_UserInfoModel>.Filter.And(Builders<SM_UserInfoModel>.Filter.Eq(d => d.UserID, userId));
            var update = Builders<SM_UserInfoModel>.Update.Set(c => c.ListMaterial, listMaterial);
            try
            {
                var result = collection.UpdateOne(
                    filter,
                    update,
                    new UpdateOptions { IsUpsert = false }
                );
                return result.IsAcknowledged ? result.ModifiedCount : 0;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public long UpgradeUserMaterial(int userId, List<UserMaterialInfo> listMaterial)
        {
            if (!MongoFactory.MongoEnable)
                return 0;

            var collection = GetCollection();
            var builder = Builders<SM_UserInfoModel>.Filter;


            var filter =
                Builders<SM_UserInfoModel>.Filter.And(Builders<SM_UserInfoModel>.Filter.Eq(d => d.UserID, userId));
            var update = Builders<SM_UserInfoModel>.Update.Set(c => c.ListMaterial, listMaterial);
            try
            {
                var result = collection.UpdateOne(
                    filter,
                    update,
                    new UpdateOptions { IsUpsert = false }
                );
                return result.IsAcknowledged ? result.ModifiedCount : 0;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public long UpgradeUserGun(int userId, int gunKind, int level, int exp)
        {
            if (!MongoFactory.MongoEnable)
                return 0;

            var collection = GetCollection();
            var builder = Builders<SM_UserInfoModel>.Filter;


            var filter = Builders<SM_UserInfoModel>.Filter
                .And(
                    Builders<SM_UserInfoModel>.Filter.Eq(d => d.UserID, userId),
                    Builders<SM_UserInfoModel>.Filter.ElemMatch(x => x.ListGun, p => p.GunKind == gunKind));


            var update = Builders<SM_UserInfoModel>.Update.Set(c => c.ListGun[-1].GunLevel, level)
                .Set(c => c.ListGun[-1].Exp, exp);
            try
            {
                var result = collection.UpdateOne(
                    filter,
                    update,
                    new UpdateOptions { IsUpsert = false }
                );
                return result.IsAcknowledged ? result.ModifiedCount : 0;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public void InsertUserDefaultSkill(int userId, List<UserSkillInfo> listSkill)
        {
            if (!MongoFactory.MongoEnable)
                return;

            var collection = GetCollection();
            var builder = Builders<SM_UserInfoModel>.Filter;


            var filter = Builders<SM_UserInfoModel>.Filter
                .And(Builders<SM_UserInfoModel>.Filter.Eq(d => d.UserID, userId));


            var update = Builders<SM_UserInfoModel>.Update.Set(c => c.ListSkill, listSkill);
            try
            {
                var result = collection.UpdateOne(
                    filter,
                    update,
                    new UpdateOptions { IsUpsert = false }
                );
            }
            catch (Exception ex)
            {
            }
        }

        public void InsertUserDefaultTower(int userId, List<UserTowerInfo> listTower)
        {
            if (!MongoFactory.MongoEnable)
                return;

            var collection = GetCollection();
            var builder = Builders<SM_UserInfoModel>.Filter;


            var filter = Builders<SM_UserInfoModel>.Filter
                .And(Builders<SM_UserInfoModel>.Filter.Eq(d => d.UserID, userId));


            var update = Builders<SM_UserInfoModel>.Update.Set(c => c.ListTower, listTower);
            try
            {
                var result = collection.UpdateOne(
                    filter,
                    update,
                    new UpdateOptions { IsUpsert = false }
                );
            }
            catch (Exception ex)
            {
            }
        }

        public void InsertUserDefaultGun(int userId, List<UserGunInfo> listGun)
        {
            if (!MongoFactory.MongoEnable)
                return;

            var collection = GetCollection();
            var builder = Builders<SM_UserInfoModel>.Filter;
            var filter = Builders<SM_UserInfoModel>.Filter
                .And(Builders<SM_UserInfoModel>.Filter.Eq(d => d.UserID, userId));


            var update = Builders<SM_UserInfoModel>.Update.Set(c => c.ListGun, listGun);
            try
            {
                var result = collection.UpdateOne(
                    filter,
                    update,
                    new UpdateOptions { IsUpsert = false }
                );
            }
            catch (Exception ex)
            {
            }
        }

        public void InsertUserGun(SM_UserInfoModel user)
        {
            if (!MongoFactory.MongoEnable)
                return;

            var collection = GetCollection();

            collection.InsertOne(user);
        }

        public long UpgradeUserTower(int userId, int towerKind, int level, int exp)
        {
            if (!MongoFactory.MongoEnable)
                return 0;

            var collection = GetCollection();
            var builder = Builders<SM_UserInfoModel>.Filter;

            var filter = Builders<SM_UserInfoModel>.Filter
                .And(
                    Builders<SM_UserInfoModel>.Filter.Eq(d => d.UserID, userId),
                    Builders<SM_UserInfoModel>.Filter.ElemMatch(x => x.ListTower, p => p.TowerKind == towerKind));


            var update = Builders<SM_UserInfoModel>.Update.Set(c => c.ListGun[-1].GunLevel, level)
                .Set(c => c.ListGun[-1].Exp, exp);
            try
            {
                var result = collection.UpdateOne(
                    filter,
                    update,
                    new UpdateOptions { IsUpsert = false }
                );
                return result.IsAcknowledged ? result.ModifiedCount : 0;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        private IMongoCollection<SM_UserInfoModel> GetCollection()
        {
            if (!MongoFactory.MongoEnable)
                return null;

            return MongoFactory.MongoBb.GetCollection<SM_UserInfoModel>(CollectionName.SM_UserInfo);
        }
    }
}