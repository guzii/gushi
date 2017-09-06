using NFine.Domain.Entity.SystemManage;
using System.Data.Entity.ModelConfiguration;

namespace NFine.Mapping.SystemManage
{
    public class ArticleMap : EntityTypeConfiguration<ArticleEntity>
    {
        public ArticleMap()
        {
            this.ToTable("Sys_Article");
            this.HasKey(t => t.F_Id);
        }
    }
}
