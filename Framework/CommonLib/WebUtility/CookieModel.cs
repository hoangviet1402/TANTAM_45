using System;

namespace WebUtility
{
    /// <summary>
    ///     Author: PhatVT
    ///     <para>Date 26/12/2014</para>
    ///     <para>Cau hinh khoi tao Cookie</para>
    /// </summary>
    public class CookieModel
    {
        public CookieModel(string domain, string name, object value)
        {
            ExpireTime = null;
            Domain = domain;
            Name = name;
            Value = value.ToString();
        }

        public CookieModel(string domain, string name, object value, DateTime expireTime)
        {
            ExpireTime = expireTime;
            Domain = domain;
            Name = name;
            Value = value.ToString();
        }

        public CookieModel(string domain, string name, object value, int expireTime)
        {
            ExpireTime = DateTime.Now.AddMinutes(expireTime);
            Domain = domain;
            Name = name;
            Value = value.ToString();
        }

        public DateTime? ExpireTime { get; set; }

        public string Name { get; set; }

        public string Value { get; set; }

        public string Domain { get; set; }

        public string Path { get; set; }
    }
}