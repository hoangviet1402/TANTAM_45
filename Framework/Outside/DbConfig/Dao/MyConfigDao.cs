using System;
using System.Data.Entity;
using System.Linq;
using System.Transactions;
using DbConfig.EF;

namespace DbConfig.Dao
{
    /// <summary>
    /// Author: PhatVT
    /// <para></para>
    /// IMyConfigDao is public contract
    /// </summary>
    internal interface IMyConfigDao
    {
        /// <summary>
        /// <Author>PhatVT</Author>
        /// <DateCreated>20/01/2015</DateCreated>
        /// <Description>Lấy thông tin cấu hình từ database</Description>
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        MyConfig Get(string key);

        /// <summary>
        /// <Author>PhatVT</Author>
        /// <DateCreated>20/01/2015</DateCreated>
        /// <Description>Lấy giá trị cấu hình từ database</Description>
        /// </summary>
        /// <param name="key"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        T GetValue<T>(string key, object defaultValue = null);

        /// <summary>
        /// <Author>PhatVT</Author>
        /// <DateCreated>20/01/2015</DateCreated>
        /// <Description>Lưu cấu hình</Description>
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        void Save(string key, object value);
    }

    internal class MyConfigDao : IMyConfigDao
    {
        /// <summary>
        /// <Author>PhatVT</Author>
        /// <DateCreated>20/01/2015</DateCreated>
        /// <Description>Lấy thông tin cấu hình từ database</Description>
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public MyConfig Get(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new NullReferenceException("key cannot be null");
            }
            if (key.StartsWith("get_") || key.StartsWith("set_"))
            {
                key = key.Substring(4);
            }

            using (var entity = new DbConfigEntities())
            {
                return (from myConfig in entity.MyConfigs
                        where myConfig.Key.Equals(key)
                        select myConfig).FirstOrDefault();
            }
        }

        /// <summary>
        /// <Author>PhatVT</Author>
        /// <DateCreated>20/01/2015</DateCreated>
        /// <Description>Lấy giá trị cấu hình từ database</Description>
        /// </summary>
        /// <param name="key"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public T GetValue<T>(string key, object defaultValue = null)
        {
            var config = Get(key);
            if (config == null)
            {
                Save(key, defaultValue);
                return (T)defaultValue;
            }
            else
            {
                return (T)Convert.ChangeType(config.Value, typeof(T));
            }
        }

        /// <summary>
        /// <Author>PhatVT</Author>
        /// <DateCreated>20/01/2015</DateCreated>
        /// <Description>Lưu cấu hình</Description>
        /// </summary>
        /// <para>Edit: TrungTT</para>
        /// <para>Date: 2015-04-16</para>
        /// <para>Description: Chinh phan sau config key vao DB neu khong ton tai config Key</para>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public void Save(string key, object value)
        {
            //if (value == null)
            //{
            //    throw new NullReferenceException("value cannot be null");
            //}

            var valueToSaved = "";

            try { valueToSaved = value != null ? value.ToString() : ""; }
            catch { valueToSaved = ""; }

            var config = Get(key);
            using (var entity = new DbConfigEntities())
            {
                if (config == null)
                {
                    config = new MyConfig
                    {
                        Key = key,
                        Value = valueToSaved, //value.ToString(),
                    };
                    entity.MyConfigs.Add(config);
                }
                else
                {
                    config.Value = valueToSaved; // value.ToString();
                    var dbEntry = entity.Entry(config);
                    if (dbEntry != null && dbEntry.State != EntityState.Modified)
                    {
                        dbEntry.State = EntityState.Modified;
                    }
                }
                entity.SaveChanges();
            }
        }
    }
}
