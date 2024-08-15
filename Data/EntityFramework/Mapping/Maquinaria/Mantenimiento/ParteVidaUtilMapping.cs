using Core.Entity.Maquinaria.Mantenimiento;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.Maquinaria.Mantenimiento
{
    public class ParteVidaUtilMapping : EntityTypeConfiguration<tblM_CatParteVidaUtil>
    {
        public ParteVidaUtilMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.descripcion).HasColumnName("descripcion");
            Property(x => x.vidaUtilMax).HasColumnName("vidaUtilMax");
            Property(x => x.vidaUtilMin).HasColumnName("vidaUtilMin");
            ToTable("tblM_CatParteVidaUtil");
        }
    }
}
