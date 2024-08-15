using Core.Entity.Administrativo.Proyecciones;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.Proyecciones
{
    public class CatResponsableMapping : EntityTypeConfiguration<tblPro_CatResponsables>
    {
        public CatResponsableMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.responsableID).HasColumnName("responsableID");
            Property(x => x.Descripcion).HasColumnName("Descripcion");
            Property(x => x.Color).HasColumnName("Color");
            Property(x => x.Abreviatura).HasColumnName("Abreviatura");
            ToTable("tblPro_CatResponsables");
        }
    }
}
