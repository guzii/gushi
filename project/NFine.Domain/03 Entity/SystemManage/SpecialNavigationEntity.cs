using System;

namespace NFine.Domain.Entity.SystemManage
{
    public class SpecialNavigationEntity : IEntity<SpecialNavigationEntity>, ICreationAudited, IDeleteAudited, IModificationAudited
    {
        public string F_Id { get; set; }
        public string F_SpecialId { get; set; }
        public string F_NavigationId { get; set; }
        public bool F_IsRecommand { get; set; }
        public DateTime? F_CreatorTime { get; set; }
        public string F_CreatorUserId { get; set; }
        public DateTime? F_LastModifyTime { get; set; }
        public string F_LastModifyUserId { get; set; }
        public DateTime? F_DeleteTime { get; set; }
        public string F_DeleteUserId { get; set; }

        public bool? F_DeleteMark { get; set; }
    }
}
