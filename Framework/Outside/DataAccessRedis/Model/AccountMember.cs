/**********************************************************************
 * Author: VAnnl
 * ####################################################################
 * Author:......................
 * DateModify: .................
 * Description: ................
 *
 *********************************************************************/

using System;
using System.ComponentModel;
using Newtonsoft.Json;

namespace DataAccessRedis.Model
{
    internal class AccountMember
    {
    }

    public class AssociationMember_ListUserNewOrOld_Redis
    {
        [Description("")]
        [JsonProperty("userid")]
        public int UserID { get; set; }

        [Description("")]
        [JsonProperty("nick")]
        public string NickName { get; set; }

        [Description("")]
        [JsonProperty("date")]
        public DateTime CreateDate { get; set; }

        [Description("")]
        [JsonProperty("pub")]
        public int PubUserID { get; set; }

        [Description("")]
        [JsonProperty("coin")]
        public decimal Coin { get; set; }

        [Description("")]
        [JsonProperty("vipp")]
        public int VIPPoint { get; set; }
    }

    public class Account_GetBirthdayUsers_Redis
    {
        [Description("")]
        [JsonProperty("bir")]
        public int BirthDayID { get; set; }

        [Description("")]
        [JsonProperty("user")]
        public int UserID { get; set; }

        [Description("")]
        [JsonProperty("nick")]
        public string NickName { get; set; }

        [Description("")]
        [JsonProperty("pub")]
        public int PubUserID { get; set; }

        [Description("")]
        [JsonProperty("vipp")]
        public int VipPoint { get; set; }
    }
}