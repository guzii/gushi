using NFine.Domain.Entity.SystemManage;
using System.Data.Entity.ModelConfiguration;
namespace NFine.Mapping.SystemManage
{
    public class SpecialNavigationMap : EntityTypeConfiguration<SpecialNavigationEntity>
    {
        public SpecialNavigationMap()
        {
            this.ToTable("Sys_SpecialNavigation");
            this.HasKey(t => t.F_Id);
        }
    }
}
