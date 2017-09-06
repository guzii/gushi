using System;

namespace NFine.Domain.Entity.SystemManage
{
   public  class TagsEntity : IEntity<TagsEntity>, ICreationAudited, IDeleteAudited, IModificationAudited
    {
        public string F_Id { get; set; }
        public string F_Title { get; set; }
        public bool F_Recommend { get; set; }
        public int F_SortCode { get; set; }
        public bool? F_DeleteMark { get; set; }
        public bool F_EnabledMark { get; set; }
        public DateTime? F_CreatorTime { get; set; }
        public string F_CreatorUserId { get; set; }
        public DateTime? F_LastModifyTime { get; set; }
        public string F_LastModifyUserId { get; set; }
        public DateTime? F_DeleteTime { get; set; }
        public string F_DeleteUserId { get; set; }
    }
}
