/**********************************************************************
 * Author: VAnnl
 * ####################################################################
 * Author:......................
 * DateModify: .................
 * Description: ................
 *
 *********************************************************************/

using System.ComponentModel;
using Newtonsoft.Json;

namespace DataAccessRedis.Model
{
    internal class EventMember
    {
    }

    public class EventRunningRedisModel
    {
        [Description("")] // map voi column cua DB
        [JsonProperty("eID")] // map voi key cua Redis
        public int EventId { get; set; }

        [Description("")]
        [JsonProperty("eNa")]
        public string EventName { get; set; }

        [Description("")]
        [JsonProperty("beg")]
        public string BeginDate { get; set; }

        [Description("")]
        [JsonProperty("end")]
        public string EndDate { get; set; }
    }
}