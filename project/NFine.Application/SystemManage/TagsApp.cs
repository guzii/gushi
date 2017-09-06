using NFine.Domain.Entity.SystemManage;
using NFine.Domain.IRepository.SystemManage;
using NFine.Repository.SystemManage;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NFine.Application.SystemManage
{
    public class TagsApp
    {
        private ITagsRepository service = new TagsRepository();
        public bool ExistTitle(string Title)
        {
            return service.ExistTitle(Title);
        }
        public void DeleteForge(List<string> KeyValues)
        {
            service.DeleteForge(KeyValues);
        }
        public List<TagsEntity> GetList()
        {
            return service.IQueryable(a => a.F_DeleteMark == false).ToList();
        }
        public TagsEntity GetForm(string keyValue)
        {
            return service.FindEntity(keyValue);
        }
        public void DeleteForm(string keyValue)
        {
            service.Delete(t => t.F_Id == keyValue);
        }
        public void SubmitForm(TagsEntity tagsEntity, string keyValue)
        {
            if (!string.IsNullOrEmpty(keyValue))
            {
                tagsEntity.Modify(keyValue);
                service.Update(tagsEntity);
            }
            else
            {
                tagsEntity.Create();
                service.Insert(tagsEntity);
            }
        }
    }
}
