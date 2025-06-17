/**********************************************************************
 * Author: HuyHT
 * DateCreate: 06-25-2014
 * Description: DaoFactory
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
    internal class AssociationMember
    {
    }

    public class AssociationMemberGetTopExpUsers
    {
        public int RowIndex { get; set; }
        public int BoxType { get; set; }
        public string BoxKey { get; set; }
        public int ObjectId { get; set; }
        public string ObjectName { get; set; }
        public string Value1 { get; set; }
        public string Value2 { get; set; }
        public string Value3 { get; set; }
        public bool IsOwner { get; set; }
        public bool IsSubOwner { get; set; }
        public decimal Coin { get; set; }
        public decimal Total { get; set; }
    }

    public class TopMonthContributorRedis
    {
        [JsonProperty("uid")] public int UserId { get; set; }

        [JsonProperty("nick")] public string NickName { get; set; }

        [JsonProperty("puid")] public int PubUserId { get; set; }

        [JsonProperty("avt")] public string EmotionPath { get; set; }

        [JsonProperty("coin")] public decimal Coin { get; set; }

        [JsonProperty("total")] public decimal Total { get; set; }
    }

    public class AssMemberNotLoginResponseModel
    {
        [Description("")] // map voi column cua DB
        [JsonProperty("nick")] // map voi key cua Redis
        public string NickName { get; set; }

        [Description("")] [JsonProperty("")] public int PubUserId { get; set; }

        [Description("")] [JsonProperty("")] public DateTime LastLoginUser { private get; set; }

        [Description("")] [JsonProperty("")] public int LastLogin => (int)(DateTime.Now - LastLoginUser).TotalDays;

        [Description("")] [JsonProperty("")] public int UserId { get; set; }

        [Description("")] [JsonProperty("")] public bool IsOwner { get; set; }

        [Description("")] [JsonProperty("")] public bool IsSubOwner { get; set; }
    }

    public class AssMemberTopUpdateResponseModelRedis
    {
        [Description("")]
        [JsonProperty("cdate")]
        public string CreateDate { get; set; }


        [Description("")]
        [JsonProperty("content")]
        public string Content { get; set; }


        [Description("")]
        [JsonProperty("nick")]
        public string Nick { get; set; }


        [Description("")]
        [JsonProperty("uid")]
        public int UserId { get; set; }


        [Description("")]
        [JsonProperty("puid")]
        public string PubUserId { get; set; }
    }

    public class AssMemberLastLoginResponseModelRedis
    {
        [Description("")]
        [JsonProperty("uid")]
        public int UserId { get; set; }

        [Description("")]
        [JsonProperty("puid")]
        public string PubUserId { get; set; }

        [Description("")]
        [JsonProperty("nickname")]
        public string NickName { get; set; }

        [Description("")]
        [JsonProperty("coin")]
        public decimal Coin { get; set; }

        [Description("")]
        [JsonProperty("llogin")]
        public string LastLogin { get; set; }

        [Description("")]
        [JsonProperty("iso")]
        public string IsOwner { get; set; }

        [Description("")]
        [JsonProperty("isso")]
        public string IsSubOwner { get; set; }
    }

    public class AssMemberNewestArticlesResponseModelRedis
    {
        [Description("")]
        [JsonProperty("assid")]
        public int ArticleId { get; set; }

        [Description("")]
        [JsonProperty("title")]
        public string Title { get; set; }

        [Description("")]
        [JsonProperty("tid")]
        public string TextId { get; set; }
    }

    public class AssMemberTopContributeResponseModelRedis
    {
        [Description("")]
        [JsonProperty("uid")]
        public int UserId { get; set; }

        [Description("")]
        [JsonProperty("nick")]
        public string NickName { get; set; }

        [Description("")]
        [JsonProperty("puid")]
        public string PubUserId { get; set; }

        [Description("")]
        [JsonProperty("coin")]
        public decimal Coin { get; set; }
    }

    public class AssMemberTopContributeExpResponseModelRedis
    {
        [Description("")]
        [JsonProperty("uid")]
        public int UserId { get; set; }

        [Description("")]
        [JsonProperty("assid")]
        public int AssociationId { get; set; }

        [Description("")]
        [JsonProperty("nick")]
        public string NickName { get; set; }

        [Description("")]
        [JsonProperty("puid")]
        public string PubUserId { get; set; }

        [Description("")]
        [JsonProperty("exp")]
        public decimal Exp { get; set; }

        [Description("")]
        [JsonProperty("total")]
        public int Total { get; set; }
    }

    public class Association_GetArticles_AssNewRedis
    {
        [Description("")]
        [JsonProperty("stt")]
        public int STT { get; set; }

        [Description("")]
        [JsonProperty("assid")]
        public int AssociationID { get; set; }

        [Description("")]
        [JsonProperty("uid")]
        public int UserID { get; set; }

        [Description("")]
        [JsonProperty("puid")]
        public int PubUserID { get; set; }

        [Description("")]
        [JsonProperty("iso")]
        public bool IsOwner { get; set; }

        [Description("")]
        [JsonProperty("nick")]
        public string NickName { get; set; }

        [Description("")]
        [JsonProperty("avt")]
        public string Emotion { get; set; }

        [Description("")]
        [JsonProperty("ccoint")]
        public decimal ContCoin { get; set; }

        [Description("")]
        [JsonProperty("isso")]
        public bool IsSubOwner { get; set; }

        [Description("")]
        [JsonProperty("cexp")]
        public decimal ContExp { get; set; }

        [Description("")] [JsonProperty("cm")] public string Comment { get; set; }

        [Description("")]
        [JsonProperty("cdate")]
        public string CreateDate { get; set; }

        [Description("")]
        [JsonProperty("assnameid")]
        public string AssNameID { get; set; }

        [Description("")]
        [JsonProperty("name")]
        public string Name { get; set; }
    }

    public class Association_GetTopMaxInMonth_Redis
    {
        [Description("")] [JsonProperty("id")] public int ID { get; set; }

        [Description("")]
        [JsonProperty("btype")]
        public string BoxType { get; set; }

        [Description("")]
        [JsonProperty("key")]
        public string Key { get; set; }

        [Description("")]
        [JsonProperty("oid")]
        public int ObjectId { get; set; }

        [Description("")]
        [JsonProperty("oname")]
        public string ObjectName { get; set; }

        [Description("")] [JsonProperty("v1")] public decimal Value1 { get; set; }

        [Description("")] [JsonProperty("v2")] public string Value2 { get; set; }

        [Description("")]
        [JsonProperty("cdate")]
        public string CreateDate { get; set; }

        [Description("")]
        [JsonProperty("ipatch")]
        public string ImagePath { get; set; }

        [Description("")]
        [JsonProperty("assnameid")]
        public string AssNameID { get; set; }

        [Description("")]
        [JsonProperty("puid")]
        public int PubUserID { get; set; }

        [Description("")]
        [JsonProperty("uid")]
        public int UserID { get; set; }

        [Description("")]
        [JsonProperty("nickname")]
        public string NickName { get; set; }

        [Description("")]
        [JsonProperty("expno")]
        public int ExpNo { get; set; }

        [Description("")]
        [JsonProperty("lvno")]
        public int LevelNo { get; set; }
    }

    //public class Association_TopAssociationInMonth_Redis
    //{
    //    [Description("")]
    //    [JsonProperty("")]
    //    public int ID { get; set; }

    //    [Description("")]
    //    [JsonProperty("")]
    //    public string BoxType { get; set; }

    //    [Description("")]
    //    [JsonProperty("")]
    //    public string Key { get; set; }

    //    [Description("")]
    //    [JsonProperty("")]
    //    public int ObjectId { get; set; }

    //    [Description("")]
    //    [JsonProperty("")]
    //    public string ObjectName { get; set; }

    //    [Description("")]
    //    [JsonProperty("")]
    //    public decimal Value1 { get; set; }

    //    [Description("")]
    //    [JsonProperty("")]
    //    public string Value2 { get; set; }

    //    [Description("")]
    //    [JsonProperty("")]
    //    public DateTime CreateDate { get; set; }

    //    [Description("")]
    //    [JsonProperty("")]
    //    public string ImagePath { get; set; }

    //    [Description("")]
    //    [JsonProperty("")]
    //    public string AssNameID { get; set; }

    //    [Description("")]
    //    [JsonProperty("")]
    //    public int PubUserID { get; set; }

    //    [Description("")]
    //    [JsonProperty("")]
    //    public int UserID { get; set; }

    //    [Description("")]
    //    [JsonProperty("")]
    //    public string NickName { get; set; }

    //    [Description("")]
    //    [JsonProperty("")]
    //    public int ExpNo { get; set; }

    //    [Description("")]
    //    [JsonProperty("")]
    //    public int LevelNo { get; set; }

    //}

    public class Association_TopAssociationTopCoin_Redis
    {
        [Description("")]
        [JsonProperty("assId")]
        public int AssociationID { get; set; }

        [Description("")]
        [JsonProperty("nameId")]
        public string AssNameID { get; set; }

        [Description("")]
        [JsonProperty("name")]
        public string Name { get; set; }

        [Description("")]
        [JsonProperty("imag")]
        public string ImagePath { get; set; }

        [Description("")]
        [JsonProperty("lev")]
        public int LevelNo { get; set; }

        [Description("")]
        [JsonProperty("mem")]
        public int MemberCount { get; set; }

        [Description("")]
        [JsonProperty("pubId")]
        public int PubUserID { get; set; }

        [Description("")]
        [JsonProperty("own")]
        public string OwnerNickName { get; set; }

        [Description("")]
        [JsonProperty("ass")]
        public decimal AssCoin { get; set; }

        [Description("")]
        [JsonProperty("row")]
        public int RowNumber { get; set; }
    }

    public class Association_GetArticles_AssEvent_Redis
    {
        [Description("")]
        [JsonProperty("stt")]
        public int STT { get; set; }

        [Description("")]
        [JsonProperty("uid")]
        public int UserID { get; set; }

        [Description("")]
        [JsonProperty("puid")]
        public int PubUserID { get; set; }

        [Description("")]
        [JsonProperty("nick")]
        public string NickName { get; set; }

        [Description("")]
        [JsonProperty("avatar")]
        public string EmotionPath { get; set; }

        [Description("")]
        [JsonProperty("apath")]
        public string AssImagePath { get; set; }

        [Description("")]
        [JsonProperty("arid")]
        public int ArticleID { get; set; }

        [Description("")]
        [JsonProperty("assid")]
        public int AssociationID { get; set; }

        [Description("")]
        [JsonProperty("tid")]
        public string TextID { get; set; }

        [Description("")]
        [JsonProperty("title")]
        public string Title { get; set; }

        [Description("")]
        [JsonProperty("body")]
        public string Body { get; set; }

        [Description("")]
        [JsonProperty("cdate")]
        public string CreateDate { get; set; }

        [Description("")]
        [JsonProperty("udate")]
        public string UpdateDate { get; set; }

        [Description("")]
        [JsonProperty("vcount")]
        public int ViewCount { get; set; }

        [Description("")]
        [JsonProperty("catid")]
        public int CategoryID { get; set; }

        [Description("")]
        [JsonProperty("status")]
        public int Status { get; set; }

        [Description("")]
        [JsonProperty("ilink")]
        public string ImageLink { get; set; }

        [Description("")]
        [JsonProperty("sdesc")]
        public string ShortDescription { get; set; }

        [Description("")]
        [JsonProperty("name")]
        public string Name { get; set; }

        [Description("")]
        [JsonProperty("assnameid")]
        public string AssNameID { get; set; }

        [Description("")]
        [JsonProperty("trow")]
        public int TotalRow { get; set; }
    }

    public class Association_AssociationMember_Redis
    {
        [Description("")]
        [JsonProperty("assId")]
        public int AssociationID { get; set; }

        [Description("")]
        [JsonProperty("nameId")]
        public string AssNameID { get; set; }

        [Description("")]
        [JsonProperty("name")]
        public string Name { get; set; }

        [Description("")]
        [JsonProperty("imag")]
        public string ImagePath { get; set; }

        [Description("")]
        [JsonProperty("lev")]
        public int LevelNo { get; set; }

        [Description("")]
        [JsonProperty("mem")]
        public int MemberCount { get; set; }

        [Description("")]
        [JsonProperty("pubId")]
        public int PubUserID { get; set; }

        [Description("")]
        [JsonProperty("own")]
        public string OwnerNickName { get; set; }

        [Description("")] [JsonProperty("")] public decimal AssCoin { get; set; }

        [Description("")]
        [JsonProperty("row")]
        public int RowNumber { get; set; }

        [JsonProperty("total")] public int TotalRow { get; set; }
    }

    public class AssMemberTopContributeExpResponseRedis
    {
        [Description("")]
        [JsonProperty("uid")]
        public int UserId { get; set; }

        [Description("")]
        [JsonProperty("assid")]
        public int AssociationId { get; set; }

        [Description("")]
        [JsonProperty("nick")]
        public string NickName { get; set; }

        [Description("")]
        [JsonProperty("puid")]
        public int PubUserId { get; set; }

        [Description("")]
        [JsonProperty("exp")]
        public decimal Exp { get; set; }
    }

    public class AssMemberNotLoginResponseRedis
    {
        [Description("")]
        [JsonProperty("nick")]
        public string NickName { get; set; }

        [Description("")]
        [JsonProperty("puid")]
        public int PubUserId { get; set; }

        [Description("")]
        [JsonProperty("lloginu")]
        public DateTime LastLoginUser { get; set; }

        [Description("")]
        [JsonProperty("llogin")]
        public int LastLogin => (int)(DateTime.Now - LastLoginUser).TotalDays;

        [Description("")]
        [JsonProperty("uid")]
        public int UserId { get; set; }

        [Description("")]
        [JsonProperty("iso")]
        public bool IsOwner { get; set; }

        [Description("")]
        [JsonProperty("isu")]
        public bool IsSubOwner { get; set; }
    }
}