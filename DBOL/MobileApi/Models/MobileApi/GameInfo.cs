namespace TanTamApi.Models.MobileApi
{
    public class FlashVarMobile
    {
        public string FlashSource_WithoutVar { get; set; }
        public string StrFlashVars { get; set; }
        public int HeightFlash { get; set; }

        public string SocketKey { get; set; }
        public string ServerIp { get; set; }
        public string ServerPort { get; set; }

        public string StrFlashVarRSPort { get; set; }
        public string RSPort { get; set; }

        public string StrFlashVarWSPort { get; set; }
        public string WSPort { get; set; }

        public string StrFlashVarWSSPort { get; set; }
        public string WSSPort { get; set; }

        public int RoomID { get; set; }
    }
}