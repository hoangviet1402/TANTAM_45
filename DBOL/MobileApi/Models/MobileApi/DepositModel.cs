using System.Collections.Generic;
using Newtonsoft.Json;

namespace TanTamApi.Models.MobileApi
{
    public class WalletModel
    {
        [JsonProperty("wn", NullValueHandling = NullValueHandling.Ignore)]
        public string Name { get; set; }

        [JsonProperty("icu", NullValueHandling = NullValueHandling.Ignore)]
        public string IconUrl { get; set; }

        [JsonProperty("rid", NullValueHandling = NullValueHandling.Ignore)]
        public int ReasonID { get; set; }

        [JsonProperty("st", NullValueHandling = NullValueHandling.Ignore)]
        public int SubType { get; set; }

        [JsonProperty("gu", NullValueHandling = NullValueHandling.Ignore)]
        public string GuideUrl { get; set; }

        [JsonProperty("rp", NullValueHandling = NullValueHandling.Ignore)]
        public float RateKm { get; set; }

        [JsonProperty("wp", NullValueHandling = NullValueHandling.Ignore)]
        public List<WalletProductModel> Products { get; set; }
    }

    public class WalletProductModel
    {
        [JsonProperty("pid", NullValueHandling = NullValueHandling.Ignore)]
        public int ProductID { get; set; }

        [JsonProperty("am", NullValueHandling = NullValueHandling.Ignore)]
        public decimal Amount { get; set; }

        [JsonProperty("oi", NullValueHandling = NullValueHandling.Ignore)]
        public decimal Coin { get; set; }

        [JsonProperty("vp", NullValueHandling = NullValueHandling.Ignore)]
        public int VipPoint { get; set; }

        [JsonProperty("cc", NullValueHandling = NullValueHandling.Ignore)]
        public string Currency { get; set; }
    }

    #region card

    public class PaymentCardConfig
    {
        //public int Id { get; set; }
        [JsonProperty("cdt", NullValueHandling = NullValueHandling.Ignore)]
        public int CardType { get; set; }

        [JsonProperty("img", NullValueHandling = NullValueHandling.Ignore)]
        public string Img { get; set; }

        [JsonProperty("lam", NullValueHandling = NullValueHandling.Ignore)]
        public List<PaymentCardDetailConfig> ListAmounts { get; set; }

        [JsonProperty("cs", NullValueHandling = NullValueHandling.Ignore)]
        public string CheckSum { get; set; }
    }

    public class PaymentCardDetailConfig
    {
        public PaymentCardDetailConfig(int amount, int coin, int spins, int vipPoint)
        {
            Amount = amount;
            Coin = coin;
            Spins = spins;
            VipPoint = vipPoint;
        }

        [JsonProperty("am", NullValueHandling = NullValueHandling.Ignore)]
        public int Amount { get; set; }

        [JsonProperty("oi", NullValueHandling = NullValueHandling.Ignore)]
        public int Coin { get; set; }

        [JsonProperty("spi", NullValueHandling = NullValueHandling.Ignore)]
        public int Spins { get; set; }

        [JsonProperty("vp", NullValueHandling = NullValueHandling.Ignore)]
        public int VipPoint { get; set; }
    }

    #endregion

    #region SMS

    public class PaymentSmsConfig
    {
        [JsonProperty("cdt", NullValueHandling = NullValueHandling.Ignore)]
        public int CardType { get; set; }

        [JsonProperty("img", NullValueHandling = NullValueHandling.Ignore)]
        public string Img { get; set; }

        [JsonProperty("phr", NullValueHandling = NullValueHandling.Ignore)]
        public string PhoneReceiver { get; set; }

        [JsonProperty("lam", NullValueHandling = NullValueHandling.Ignore)]
        public List<PaymentSmsDetailConfig> ListAmounts { get; set; }

        [JsonProperty("cs", NullValueHandling = NullValueHandling.Ignore)]
        public string CheckSum { get; set; }
    }

    public class PaymentSmsDetailConfig
    {
        [JsonProperty("am", NullValueHandling = NullValueHandling.Ignore)]
        public int Amount { get; set; }

        [JsonProperty("oi", NullValueHandling = NullValueHandling.Ignore)]
        public int Coin { get; set; }

        [JsonProperty("syn", NullValueHandling = NullValueHandling.Ignore)]
        public string Syntax { get; set; }

        [JsonProperty("spi", NullValueHandling = NullValueHandling.Ignore)]
        public int Spins { get; set; }

        [JsonProperty("vp", NullValueHandling = NullValueHandling.Ignore)]
        public int VipPoint { get; set; }
    }

    #endregion

    #region MOMO

    public class PaymentMomoConfig
    {
        [JsonProperty("img", NullValueHandling = NullValueHandling.Ignore)]
        public string Img { get; set; }

        [JsonProperty("lam", NullValueHandling = NullValueHandling.Ignore)]
        public List<PaymentMomoDetailConfig> ListAmounts { get; set; }
    }

    public class PaymentMomoDetailConfig
    {
        public PaymentMomoDetailConfig(int amount, int coin, int spins, int vipPoint)
        {
            Amount = amount;
            Coin = coin;
            Spins = spins;
            VipPoint = vipPoint;
        }

        [JsonProperty("am", NullValueHandling = NullValueHandling.Ignore)]
        public int Amount { get; set; }

        [JsonProperty("oi", NullValueHandling = NullValueHandling.Ignore)]
        public int Coin { get; set; }

        [JsonProperty("spi", NullValueHandling = NullValueHandling.Ignore)]
        public int Spins { get; set; }

        [JsonProperty("vp", NullValueHandling = NullValueHandling.Ignore)]
        public int VipPoint { get; set; }
    }

    #endregion
}