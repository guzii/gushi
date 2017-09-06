using NFine.Code;
using NFine.Domain.Entity.SystemManage;
using NFine.Domain.IRepository.SystemManage;
using NFine.Repository.SystemManage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace NFine.Application.SystemManage
{
    public class SpecialArticleApp
    {
        private ISpecialArticleRepository service = new SpecialArticleRepository();

        public List<SpecialArticleEntity> GetList()
        {
            return service.IQueryable(a => a.F_DeleteMark == false).ToList();
        }
        public List<SpecialArticleEntity> GetList(Expression<Func<SpecialArticleEntity, bool>> Func)
        {
            return service.IQueryable(Func).Where(a => a.F_DeleteMark == false).ToList();
        }
        public SpecialArticleEntity GetForm(string keyValue)
        {
            return service.FindEntity(keyValue);
        }
        public void DeleteForm(string keyValue)
        {
            service.Delete(t => t.F_Id == keyValue);
        }
        public void SubmitForm(SpecialArticleEntity specialArticleEntity, string keyValue)
        {
            if (!string.IsNullOrEmpty(keyValue))
            {
                specialArticleEntity.Modify(keyValue);
                service.Update(specialArticleEntity);
            }
            else
            {
                specialArticleEntity.Create();
                service.Insert(specialArticleEntity);
            }
        }


    }
}
