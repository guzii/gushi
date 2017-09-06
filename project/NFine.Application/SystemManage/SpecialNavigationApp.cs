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
    public class SpecialNavigationApp
    {
        private ISpecialNavigationRepository service = new SpecialNavigationRepository();

        public List<SpecialNavigationEntity> GetList()
        {
            return service.IQueryable(a => a.F_DeleteMark == false).ToList();
        }
        public List<SpecialNavigationEntity> GetList(Expression<Func<SpecialNavigationEntity, bool>> Func)
        {
            return service.IQueryable(Func).Where(a => a.F_DeleteMark == false).ToList();
        }
        public SpecialNavigationEntity GetForm(string keyValue)
        {
            return service.FindEntity(keyValue);
        }
        public void DeleteForm(string keyValue)
        {
            service.Delete(t => t.F_Id == keyValue);
        }
        public void SubmitForm(SpecialNavigationEntity specialNavigationEntity, string keyValue)
        {
            if (!string.IsNullOrEmpty(keyValue))
            {
                specialNavigationEntity.Modify(keyValue);
                service.Update(specialNavigationEntity);
            }
            else
            {
                specialNavigationEntity.Create();
                service.Insert(specialNavigationEntity);
            }
        }


    }
}
