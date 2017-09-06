using System;
using System.Collections.Generic;

namespace NFine.Domain.Entity.SystemManage
{
    public class SpecialEntity : IEntity<SpecialEntity>, ICreationAudited, IDeleteAudited, IModificationAudited
    {
        public string F_Id { get; set; }
        public string F_ParentId { get; set; }
        public string F_EnCode { get; set; }
        public string F_Name { get; set; }
        public string F_SubName { get; set; }
        public int? F_SortCode { get; set; }
        public bool F_IsHighlight { get; set; }
        public bool? F_EnabledMark { get; set; }
        public bool? F_DeleteMark { get; set; }
        public string F_Keywords { get; set; }
        public string F_Description { get; set; }
        public DateTime? F_CreatorTime { get; set; }
        public string F_CreatorUserId { get; set; }
        public DateTime? F_LastModifyTime { get; set; }
        public string F_LastModifyUserId { get; set; }
        public DateTime? F_DeleteTime { get; set; }
        public string F_DeleteUserId { get; set; }
        [System.NonSerialized]
        public List<SpecialEntity> subList = new List<SpecialEntity>();
}
}
