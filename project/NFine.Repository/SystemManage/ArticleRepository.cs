using NFine.Code;
using NFine.Data;
using NFine.Domain.Entity.SystemManage;
using NFine.Domain.IRepository.SystemManage;
using NFine.Repository.SystemManage;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq.Expressions;
using System.Text;

namespace NFine.Repository.SystemManage
{
    public class ArticleRepository : RepositoryBase<ArticleEntity>, IArticleRepository
    {
        public List<ArticleEntity> GetList(string navId, string keywords, string Where, string OrderBy = " F_SortCode ASC,F_CreatorTime DESC", int Top = 0)
        {
            using (var db = new RepositoryBase().BeginTrans())
            {
                List<DbParameter> param = new List<DbParameter>();
                StringBuilder strSql = new StringBuilder();
                strSql.Append("SELECT");
                if (Top != 0 && Top > 0)
                {
                    strSql.Append(" TOP " + Top);
                }
                strSql.Append(@"   F_ID
                                  ,F_NavID
                                  ,F_EnCode
                                  ,F_Title
                                  ,F_Zhaiyao
                                  ,F_Link
                                  ,F_ImgUrl
                                  ,F_Content
                                  ,F_SEOTitle
                                  ,F_SEOKeywords
                                  ,F_SEOdescription
                                  ,F_Tags
                                  ,F_SortCode
                                  ,F_EnabledMark
                                  ,F_DeleteMark
                                  ,F_Ismsg
                                  ,F_Ishot
                                  ,F_Isrecommend
                                  ,F_IsTop
                                  ,F_LikeCount
                                  ,F_HateCount
                                  ,F_ReadCount
                                  ,F_Aothor
                                  ,F_CreatorUserId
                                  ,F_CreatorTime
                                  ,F_LastModifyTime
                                  ,F_LastModifyUserId
                                  ,F_DeleteTime
                                  ,F_DeleteUserId
                                  ,F_SaveStyle
                                  ,F_ContentLink
                                  FROM Sys_Article where F_DeleteMark=0 ");

                if (!string.IsNullOrEmpty(keywords))
                {
                    strSql.Append(" AND (F_Title LIKE @keywords OR F_Tags LIKE @keywords OR F_Aothor LIKE @keywords OR F_EnCode LIKE @keywords) ");
                    param.Add(new SqlParameter("@keywords", "'%" + keywords + "'%"));
                }
                strSql.Append(" AND F_NavID IN (" + navId + ") ");
                if (!string.IsNullOrEmpty(Where))
                {
                    strSql.Append(" AND " + Where);
                }
                strSql.Append(" Order By " + OrderBy);

                return db.FindList<ArticleEntity>(strSql.ToString(), param.ToArray());

            }
        }
        /// <summary>
        /// 伪删除
        /// </summary>
        /// <param name="keyValue"></param>
        public void DeleteFormForged(string keyValue)
        {
            using (var db = new RepositoryBase().BeginTrans())
            {
                ArticleEntity articleEntity = db.FindEntity<ArticleEntity>(keyValue);
                articleEntity.F_DeleteMark = true;
                var LoginInfo = OperatorProvider.Provider.GetCurrent();
                if (LoginInfo != null)
                {
                    articleEntity.F_DeleteUserId = LoginInfo.UserId;
                }
                articleEntity.F_DeleteTime = DateTime.Now;

                int i = db.Update<ArticleEntity>(articleEntity);
                db.Commit();
            }
        }
    }
}
