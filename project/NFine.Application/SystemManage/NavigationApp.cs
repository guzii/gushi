using NFine.Code;
using NFine.Domain.Entity.SystemManage;
using NFine.Domain.IRepository.SystemManage;
using NFine.Repository.SystemManage;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NFine.Application.SystemManage
{
    public class NavigationApp
    {
        private INavigationRepository service = new NavigationRepository();
        public List<NavigationEntity> GetList()
        {
            return service.IQueryable(a => a.F_DeleteMark == false).OrderBy(t => t.F_SortCode).ToList();
        }
        public NavigationEntity GetEntityByEnCode(string Code, System.Linq.Expressions.Expression<Func<NavigationEntity, bool>> Func)
        {
            return GetList(Func).Find(a => a.F_EnCode == Code);
        }
        public List<NavigationEntity> GetList(System.Linq.Expressions.Expression<Func<NavigationEntity, bool>> Func)
        {
            return service.IQueryable(a => a.F_DeleteMark == false).Where(Func).OrderBy(t => t.F_SortCode).ToList();
        }
        public bool ExistChild(string keyValue)
        {
            return GetList().Where(a => a.F_ParentId == keyValue).Count() > 0 ? true : false;
        }

        public List<NavigationEntity> GetChildList(string keyValue, bool isContainCurrentNode = false)
        {
            if (string.IsNullOrEmpty(keyValue))
            {
                return GetList();
            }
            List<NavigationEntity> Child = new List<NavigationEntity>();
            ChildList(GetList(), keyValue, ref Child);
            if (isContainCurrentNode)
            {
                Child.Add(GetForm(keyValue));
            }
            return Child;
        }

        private void ChildList(List<NavigationEntity> List, string keyValue, ref List<NavigationEntity> Child)
        {
            foreach (NavigationEntity item in List)
            {
                if ((item.F_ParentId + "").ToLower() == keyValue.ToLower())
                {
                    if (!Child.Contains(item))
                    {
                        Child.Add(item);
                        ChildList(List, item.F_Id, ref Child);
                    }
                }
            }
        }

        public NavigationEntity GetForm(System.Linq.Expressions.Expression<Func<NavigationEntity, bool>> Func)
        {
            return service.FindEntity(Func);
        }
        public NavigationEntity GetForm(string keyValue)
        {
            return service.FindEntity(keyValue);
        }
        public void DeleteForm(string keyValue)
        {
            if (service.IQueryable().Count(t => t.F_ParentId.Equals(keyValue)) > 0)
            {
                throw new Exception("删除失败！操作的对象包含了下级数据。");
            }
            else
            {
                service.Delete(t => t.F_Id == keyValue);
            }
        }
        public void SubmitForm(NavigationEntity navigationEntity, string keyValue)
        {
            if (!string.IsNullOrEmpty(keyValue))
            {
                if (service.IQueryable().Count(t => t.F_EnCode.Equals(navigationEntity.F_EnCode)
                && !t.F_Id.Equals(keyValue)) > 0)
                {
                    throw new Exception("修改失败！操作的对象编号已存在。");
                }
                navigationEntity.Modify(keyValue);
                service.Update(navigationEntity);
            }
            else
            {
                if (service.IQueryable().Count(t => t.F_EnCode.Equals(navigationEntity.F_EnCode)) > 0)
                {
                    throw new Exception("添加失败！操作的对象编号已存在。");
                }
                if (navigationEntity.F_DeleteMark == null)
                {
                    navigationEntity.F_DeleteMark = false;
                }
                navigationEntity.Create();
                service.Insert(navigationEntity);
            }
        }

    }
}
