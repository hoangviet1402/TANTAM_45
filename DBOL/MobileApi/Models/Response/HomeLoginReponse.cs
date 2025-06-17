using System;
using System.Collections.Generic;

namespace TanTamApi.Models.Response
{
    public class HomeLoginMobileReponse
    {
        public HomeLoginMobileReponse()
        {
            IsCaptcha = 0;
        }

        public int UserId { get; set; }
        public string DisplayName { get; set; }
        public string Token { get; set; }
        public string Pass { get; set; }
        public DateTime CreateDate { get; set; }
        public bool IsSuccess { get; set; }
        public List<string> ErrorList { get; set; }        
        public string ApiUrl { get; set; }
        public int IsCaptcha { get; set; }
    }
}