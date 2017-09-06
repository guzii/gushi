using NFine.Code;
using NFine.Domain.Entity.SystemManage;
using NFine.Domain.IRepository.SystemManage;
using NFine.Repository.SystemManage;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace NFine.Application.SystemManage
{
    public class ArticleApp
    {
        private IArticleRepository service = new ArticleRepository();

        public List<ArticleEntity> GetList()
        {
            return service.IQueryable(a => a.F_DeleteMark == false).ToList();
        }
        public List<ArticleEntity> GetList(Expression<Func<ArticleEntity, bool>> FuncWhere, int Top = 78)
        {
            return service.IQueryable(a => a.F_DeleteMark == false).Where(FuncWhere)
                .OrderByDescending(a => a.F_CreatorTime).Take(Top).ToList();
        }
        public List<ArticleEntity> GetHotList()
        {
            return service.IQueryable(a => a.F_DeleteMark == false && a.F_EnabledMark == true)
                .OrderByDescending(a => a.F_ReadCount).OrderByDescending(a => a.F_LikeCount).Take(15).ToList();
        }
        public List<ArticleEntity> GetList(Pagination pagination, string navId, string keywords, Expression<Func<ArticleEntity, bool>> FuncWhere)
        {
            var expression = ExtLinq.True<ArticleEntity>();
            if (!string.IsNullOrEmpty(keywords))
            {
                expression = expression.And(t => t.F_Title.Contains(keywords) || t.F_Tags.Contains(keywords) || t.F_Aothor.Contains(keywords));
            }
            expression = expression.And(t => navId.Contains(t.F_NavID));
            expression = expression.And(FuncWhere);
            return service.FindList(expression, pagination).OrderBy(a => a.F_SortCode).ThenByDescending(a => a.F_CreatorTime).ToList();
        }
        public List<ArticleEntity> GetListSpecial(Pagination pagination, string specialArticleIds, string keywords, Expression<Func<ArticleEntity, bool>> FuncWhere)
        {
            var expression = ExtLinq.True<ArticleEntity>();
            if (!string.IsNullOrEmpty(keywords))
            {
                expression = expression.And(t => t.F_Title.Contains(keywords) || t.F_Tags.Contains(keywords) || t.F_Aothor.Contains(keywords));
            }
            expression = expression.And(t => specialArticleIds.Contains(t.F_Id));
            expression = expression.And(FuncWhere);
            var data = service.FindList(expression, pagination);
            return service.FindList(expression, pagination).OrderBy(a => a.F_SortCode).ThenByDescending(a => a.F_CreatorTime).ToList();
        }
        public List<ArticleEntity> GetList(Pagination pagination, string navId, string specialArticleIds, string keywords, Expression<Func<ArticleEntity, bool>> FuncWhere)
        {
            var expression = ExtLinq.True<ArticleEntity>();
            if (!string.IsNullOrEmpty(keywords))
            {
                expression = expression.And(t => t.F_Title.Contains(keywords) || t.F_Tags.Contains(keywords) || t.F_Aothor.Contains(keywords));
            }
            expression = expression.And(t => navId.Contains(t.F_NavID));
            expression = expression.And(t => !specialArticleIds.Contains(t.F_Id));
            expression = expression.And(FuncWhere);
            return service.FindList(expression, pagination).OrderBy(a => a.F_SortCode).ThenByDescending(a => a.F_CreatorTime).ToList();

        }
        public List<ArticleEntity> GetList(string navId, string keywords, string Where = "", string OrderBy = " F_SortCode ASC,F_CreatorTime DESC", int Top = 0)
        {
            return service.GetList(navId, keywords, Where, OrderBy, Top);
        }
        public ArticleEntity GetForm(string keyValue)
        {
            return service.FindEntity(keyValue);
        }
        public ArticleEntity GetFormByEnCode(string EnCode)
        {
            return service.IQueryable(a => a.F_EnCode == EnCode && a.F_EnabledMark == true && a.F_DeleteMark == false).FirstOrDefault();
        }
        public void DeleteForm(string keyValue)
        {
            service.Delete(t => t.F_Id == keyValue);
        }
        public void DeleteFormForged(string keyValue)
        {
            service.DeleteFormForged(keyValue);
        }
        public void SubmitForm(ArticleEntity articleEntity, string keyValue)
        {
            if (!string.IsNullOrEmpty(keyValue))
            {
                articleEntity.Modify(keyValue);
                service.Update(articleEntity);
            }
            else
            {
                if (articleEntity.F_DeleteMark == null)
                    articleEntity.F_DeleteMark = false;
                articleEntity.Create();
                service.Insert(articleEntity);
            }
        }
        /// <summary>
        /// Mvc 分页
        /// </summary>
        /// <param name="order"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public IPagedList<ArticleEntity> getMvcPageDataList(Func<ArticleEntity, bool> where, Func<ArticleEntity, object> order, int pageIndex, int pageSize)
        {
            //这里为了方便直接用的EF测试，其实这里可以直接用一个获得的list比如：userInfoList.ToPagedList(page, pageSize));
            return GetList().Where<ArticleEntity>(where).OrderByDescending(order).ToPagedList(pageIndex, pageSize);
        }
    }
}
