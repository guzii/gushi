using NFine.Domain.Entity.SystemManage;
using System.Data.Entity.ModelConfiguration;
namespace NFine.Mapping.SystemManage
{
    public class SpecialArticleMap : EntityTypeConfiguration<SpecialArticleEntity>
    {
        public SpecialArticleMap()
        {
            this.ToTable("Sys_SpecialArticle");
            this.HasKey(t => t.F_Id);
        }
    }
}
