using System;

namespace NFine.Domain.Entity.SystemManage
{
    public class ArticleEntity : IEntity<ArticleEntity>, ICreationAudited, IDeleteAudited, IModificationAudited
    {
        public string F_Id { get; set; }
        public string F_NavID { get; set; }
        public string F_EnCode { get; set; }
        public string F_Title { get; set; }
        public string F_Zhaiyao { get; set; }
        public string F_Link { get; set; }
        public string F_ImgUrl { get; set; }
        public string F_Content { get; set; }
        public bool F_SaveStyle { get; set; }
        public string F_ContentLink { get; set; }
        public string F_SEOTitle { get; set; }
        public string F_SEOKeywords { get; set; }
        public string F_SEOdescription { get; set; }
        public string F_Tags { get; set; }
        public int? F_SortCode { get; set; }
        public bool? F_Ishot { get; set; }
        public bool? F_Ismsg { get; set; }
        public bool? F_Isrecommend { get; set; }
        public bool? F_IsTop { get; set; }
        public int F_LikeCount { get; set; }
        public int F_HateCount { get; set; }
        public int F_ReadCount { get; set; }
        public string F_Aothor { get; set; }
        public bool? F_EnabledMark { get; set; }
        public bool? F_DeleteMark { get; set; }
        public DateTime? F_CreatorTime { get; set; }
        public string F_CreatorUserId { get; set; }
        public DateTime? F_LastModifyTime { get; set; }
        public string F_LastModifyUserId { get; set; }
        public DateTime? F_DeleteTime { get; set; }
        public string F_DeleteUserId { get; set; }

        [System.NonSerialized]
        public NavigationEntity NavEntity = new NavigationEntity();
    }
}
