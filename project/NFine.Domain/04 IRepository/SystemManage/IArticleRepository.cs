using NFine.Data;
using NFine.Domain.Entity.SystemManage;
using System;
using System.Collections.Generic;

namespace NFine.Domain.IRepository.SystemManage
{
    public interface IArticleRepository : IRepositoryBase<ArticleEntity>
    {
        List<ArticleEntity> GetList(string navId, string keywords, string Where, string OrderBy = " F_SortCode ASC,F_CreatorTime DESC", int Top = 0);

        void DeleteFormForged(string keyValue);
    }
}
