using NFine.Domain.Entity.SystemManage;
using System.Data.Entity.ModelConfiguration;
namespace NFine.Mapping.SystemManage
{
    public class SpecialMap : EntityTypeConfiguration<SpecialEntity>
    {
        public SpecialMap()
        {
            this.ToTable("Sys_Special");
            this.HasKey(t => t.F_Id);
        }
    }
}
