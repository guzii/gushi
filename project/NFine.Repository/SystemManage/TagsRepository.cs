using NFine.Data;
using NFine.Domain.Entity.SystemManage;
using NFine.Domain.IRepository.SystemManage;
using NFine.Repository.SystemManage;
using System.Collections.Generic;
using System.Text;

namespace NFine.Repository.SystemManage
{
    public class TagsRepository : RepositoryBase<TagsEntity>, ITagsRepository
    {
        public bool ExistTitle(string Title)
        {
            TagsEntity tagsEntity = this.FindEntity(a => a.F_Title.ToLower() == Title.ToLower());
            if (tagsEntity != null && !string.IsNullOrEmpty(tagsEntity.F_Id))
                return true;
            else
                return false;
        }

        public void DeleteForge(List<string> KeyValues)
        {
            using (var db = new RepositoryBase().BeginTrans())
            {
                foreach (var item in KeyValues)
                {
                    db.Delete<TagsEntity>(a => a.F_Id == item);
                }
                db.Commit();
            }
        }
    }
}
