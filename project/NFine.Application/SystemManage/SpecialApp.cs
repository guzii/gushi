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
    public class SpecialApp
    {
        private ISpecialRepository service = new SpecialRepository();
        private ISpecialArticleRepository specialArticleservice = new SpecialArticleRepository();
        private ISpecialNavigationRepository specialNavigationservice = new SpecialNavigationRepository();
        public List<SpecialEntity> GetList()
        {
            return service.IQueryable(a => a.F_DeleteMark == false)
                .OrderBy(t => t.F_SortCode).ToList();
        }
        public SpecialEntity GetEntityByEnCode(string Code, System.Linq.Expressions.Expression<Func<SpecialEntity, bool>> Func)
        {
            return GetList(Func).Find(a => a.F_EnCode == Code);
        }
        public List<SpecialEntity> GetList(Expression<Func<SpecialEntity, bool>> Func)
        {
            return service.IQueryable(a => a.F_DeleteMark == false).Where(Func).OrderBy(t => t.F_SortCode).ToList();
        }
        public List<SpecialEntity> GetTreeList(Expression<Func<SpecialEntity, bool>> Func)
        {
            List<SpecialEntity> List = service.IQueryable(a => a.F_DeleteMark == false).Where(Func).OrderBy(t => t.F_SortCode).ToList();
            List<SpecialEntity> ListParent = List.FindAll(a => a.F_ParentId == "0");
            foreach (var item in ListParent)
            {
                ChildSubList(List, item);
            }
            return ListParent;
        }
        public void ChildSubList(List<SpecialEntity> List, SpecialEntity entity)
        {
            foreach (SpecialEntity item in List)
            {
                if ((item.F_ParentId + "").ToLower() == entity.F_Id.ToLower())
                {
                    entity.subList.Add(item);
                    ChildSubList(List, item);
                }
            }
        }


        public List<SpecialEntity> GetChildList(string keyValue, bool isContainCurrentNode = false)
        {
            if (string.IsNullOrEmpty(keyValue))
            {
                return GetList();
            }
            List<SpecialEntity> Child = new List<SpecialEntity>();
            ChildList(GetList(a => a.F_EnabledMark == true), keyValue, ref Child);
            if (isContainCurrentNode)
            {
                Child.Add(GetForm(keyValue));
            }
            return Child;
        }

        public void ChildList(List<SpecialEntity> List, string keyValue, ref List<SpecialEntity> Child)
        {
            foreach (SpecialEntity item in List)
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

        public SpecialEntity GetForm(string keyValue)
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
        public void SubmitForm(SpecialEntity specialEntity, string keyValue)
        {
            if (!string.IsNullOrEmpty(keyValue))
            {
                if (service.IQueryable().Count(t => t.F_EnCode.Equals(specialEntity.F_EnCode)
                && !t.F_Id.Equals(keyValue)) > 0)
                {
                    throw new Exception("修改失败！操作的对象编号已存在。");
                }
                specialEntity.Modify(keyValue);
                service.Update(specialEntity);
            }
            else
            {
                if (service.IQueryable().Count(t => t.F_EnCode.Equals(specialEntity.F_EnCode)) > 0)
                {
                    throw new Exception("添加失败！操作的对象编号已存在。");
                }
                if (specialEntity.F_DeleteMark == null)
                {
                    specialEntity.F_DeleteMark = false;
                }
                specialEntity.Create();
                service.Insert(specialEntity);
            }
        }

        public void SubmitArticleForm(string SpecialId, string ArticleIds)
        {
            string[] ArticleId = ArticleIds.Split(',');
            for (int i = 0; i < ArticleId.Length; i++)
            {
                string ArtId = ArticleId[i];
                if (!string.IsNullOrEmpty(ArtId))
                {
                    if (specialArticleservice.IQueryable().Count(t => t.F_SpecialId == SpecialId && t.F_ArticleId == ArtId) == 0)
                    {
                        SpecialArticleEntity specialArticleEntity = new SpecialArticleEntity();
                        specialArticleEntity.F_ArticleId = ArtId;
                        specialArticleEntity.F_SpecialId = SpecialId;
                        specialArticleEntity.F_DeleteMark = false;
                        specialArticleEntity.Create();
                        specialArticleservice.Insert(specialArticleEntity);
                    }
                }
            }
        }
        public void SubmitNavigationForm(string NavigationId, string SpecialIds)
        {
            specialNavigationservice.Delete(a => a.F_NavigationId == NavigationId);
            string[] SpecialIdArr = SpecialIds.Split(',');
            for (int i = 0; i < SpecialIdArr.Length; i++)
            {
                string SpecialId = SpecialIdArr[i];
                if (!string.IsNullOrEmpty(SpecialId))
                {
                    if (specialNavigationservice.IQueryable().Count(t => t.F_SpecialId == SpecialId && t.F_NavigationId == NavigationId) == 0)
                    {
                        SpecialNavigationEntity specialNavigationEntity = new SpecialNavigationEntity();
                        specialNavigationEntity.F_NavigationId = NavigationId;
                        specialNavigationEntity.F_SpecialId = SpecialId;
                        specialNavigationEntity.F_DeleteMark = false;
                        specialNavigationEntity.Create();
                        specialNavigationservice.Insert(specialNavigationEntity);
                    }
                }
            }
        }

        public List<SpecialEntity> GetSpecialList(string NavigationId, bool IsRecommand = false)
        {
            List<string> SpecialNavigationList = new List<string>();
            if (IsRecommand)
            {
                SpecialNavigationList = specialNavigationservice.IQueryable(a => a.F_NavigationId == NavigationId && a.F_IsRecommand == true).Select(a => a.F_SpecialId).ToList();
            }
            else
            {
                SpecialNavigationList = specialNavigationservice.IQueryable(a => a.F_NavigationId == NavigationId).Select(a => a.F_SpecialId).ToList();
            }
            string SpecialNavigationIds = SpecialNavigationList.ToJson(",");
            return service.IQueryable(a => SpecialNavigationIds.Contains(a.F_Id)).Where(a => a.F_ParentId != "0").ToList();
        }
        public List<SpecialEntity> GetReSpecialList(string NavigationId, bool IsRecommand = false)
        {
            List<string> SpecialNavigationList = new List<string>();
            if (IsRecommand)
            {
                SpecialNavigationList = specialNavigationservice.IQueryable(a => a.F_NavigationId == NavigationId && a.F_IsRecommand == true).Select(a => a.F_SpecialId).ToList();
            }
            else
            {
                SpecialNavigationList = specialNavigationservice.IQueryable(a => a.F_NavigationId == NavigationId).Select(a => a.F_SpecialId).ToList();
            }
            string SpecialNavigationIds = SpecialNavigationList.ToJson(",");
            return service.IQueryable(a => SpecialNavigationIds.Contains(a.F_Id)).ToList();
        }

        public void SubmitReNavigationForm(string NavigationId, string SpecialIds)
        {
            List<SpecialNavigationEntity> SpecialNavigationList = specialNavigationservice.IQueryable(a => a.F_NavigationId == NavigationId).ToList();
            foreach (var item in SpecialNavigationList)
            {
                if (SpecialIds.Contains(item.F_SpecialId))
                {
                    item.F_IsRecommand = true;
                }
                else
                {
                    item.F_IsRecommand = false;
                }
                specialNavigationservice.Update(item);
            }
        }

    }
}
