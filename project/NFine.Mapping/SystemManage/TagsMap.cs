using NFine.Domain.Entity.SystemManage;
using System.Data.Entity.ModelConfiguration;

namespace NFine.Mapping.SystemManage
{
    public class TagsMap : EntityTypeConfiguration<TagsEntity>
    {
        public TagsMap()
        {
            this.ToTable("Sys_Tags");
            this.HasKey(t => t.F_Id);
        }
    }
}
