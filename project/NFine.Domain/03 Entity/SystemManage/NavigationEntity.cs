/*******************************************************************************
 * Copyright © 2016 NFine.Framework 版权所有
 * Author: NFine
 * Description: NFine快速开发平台
 * Website：http://www.nfine.cn
*********************************************************************************/
using System;
using System.Collections.Generic;

namespace NFine.Domain.Entity.SystemManage
{
    public class NavigationEntity : IEntity<NavigationEntity>, ICreationAudited, IModificationAudited, IDeleteAudited
    {
        public string F_Id { get; set; }
        public string F_ParentId { get; set; }
        public string F_EnCode { get; set; }
        public string F_Name { get; set; }
        public string F_NavName { get; set; }
        public bool F_IsNav { get; set; }
        public bool F_IsRecommend { get; set; }
        public bool F_IsHeader { get; set; }
        public string F_NavImg { get; set; }
        /// <summary>
        /// 0 文章列表 1 图片形式 2 短文展示 
        /// </summary>
        public int F_Style { get; set; }
        public int? F_SortCode { get; set; }
        public bool? F_EnabledMark { get; set; }
        public bool? F_DeleteMark { get; set; }
        public string F_Introduction { get; set; }
        public string F_Title { get; set; }
        public string F_Keywords { get; set; }
        public string F_Description { get; set; }
        public DateTime? F_CreatorTime { get; set; }
        public string F_CreatorUserId { get; set; }
        public DateTime? F_LastModifyTime { get; set; }
        public string F_LastModifyUserId { get; set; }
        public DateTime? F_DeleteTime { get; set; }
        public string F_DeleteUserId { get; set; }

        [System.NonSerialized]
        public string PatentEnCode = string.Empty;
        [System.NonSerialized]
        public List<NavigationEntity> ChildNavigationList = new List<NavigationEntity>();
        [System.NonSerialized]
        public List<ArticleEntity> ChildArticleList = new List<ArticleEntity>();
        [System.NonSerialized]
        public List<ArticleEntity> RecommendArticle = new List<ArticleEntity>();
        [System.NonSerialized]
        public List<SpecialEntity> RecommendSpecial = new List<SpecialEntity>();
    }
}
