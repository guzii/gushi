using NFine.Data;
using NFine.Domain.Entity.SystemManage;
using System.Collections.Generic;

namespace NFine.Domain.IRepository.SystemManage
{
    public interface ITagsRepository : IRepositoryBase<TagsEntity>
    {
        bool ExistTitle(string Title);

        void DeleteForge(List<string> KeyValues);
    }
}
