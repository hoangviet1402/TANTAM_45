using System;

namespace DataAccessMongo.Model.RequestLogCollection
{
    public class RequestLogModel
    {
        public string Ip { get; set; }

        public string Domain { get; set; }

        public string Referer { get; set; }

        public dynamic Param { get; set; }

        public DateTime CreateDate { get; set; }
    }
}