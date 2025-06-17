/**********************************************************************
 * Author: ThongNT
 * DateCreate: 19-12-2014 
 * Description: Ho tro truy suat CacheObject  
 * ####################################################################
 * Author:......................
 * DateModify: .................
 * Description: ................
 * 
 *********************************************************************/
using System;

namespace MyMemCache
{
    public class CacheFactory
    {
       /// <summary>
        /// Author: ThongNT
        /// <para>Date : 19-12-2014</para>
        /// <para>Khoi tao Cache</para>
        /// </summary>
        /// <param name="model">Cau hinh su dung Cache</param>
        public void InitCache(CacheConfigModel model)
        {
            if (model == null) { throw new Exception("MyMemCache.CacheFactory.InitCache.CacheModel is invalid"); }
            /* Check model.MemoryCache */
            switch (model.MemoryCache)
            {
                default:
                    MemoryCache = new MyMemoryCache(model);
                    break;
            }
        }

        /// <summary>
        /// Author: ThongNT
        /// <para>Date 19-12-2014</para>
        /// <para>Cache in-process</para>
        /// </summary>
        public static IMyCache MemoryCache
        {
            get; set;
        }
    }
}
