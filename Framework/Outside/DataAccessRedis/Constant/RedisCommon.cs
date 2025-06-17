using Newtonsoft.Json;

namespace DataAccessRedis.Constant
{
    public class RedisCommon
    {
        /// <summary>
        ///     <para>Author: TrungTT</para>
        ///     <para>Date: 2016-05-20</para>
        ///     <para>Description: Parse data tu Redis sang class nhan ve</para>
        /// </summary>
        /// <returns></returns>
        public static T ConvertFromRedis<T>(string jsonData)
        {
            var objData = default(T);
            if (!string.IsNullOrEmpty(jsonData)) objData = JsonConvert.DeserializeObject<T>(jsonData);
            return objData;
        }

        /// <summary>
        ///     <para>Author: TrungTT</para>
        ///     <para>Date: 2016-05-20</para>
        ///     <para>Description: Parse data tu class sang Redis</para>
        /// </summary>
        /// <returns></returns>
        public static string ConvertToRedis<T>(T model)
        {
            var objData = JsonConvert.SerializeObject(model);
            return objData;
        }
    }
}