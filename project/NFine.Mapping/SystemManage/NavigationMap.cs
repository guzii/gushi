using NFine.Domain.Entity.SystemManage;
using System.Data.Entity.ModelConfiguration;
namespace NFine.Mapping.SystemManage
{
    public class NavigationMap : EntityTypeConfiguration<NavigationEntity>
    {
        public NavigationMap()
        {
            this.ToTable("Sys_Navigation");
            this.HasKey(t => t.F_Id);
        }
    }
}
