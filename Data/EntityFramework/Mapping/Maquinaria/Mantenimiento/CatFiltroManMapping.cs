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
    public class CatFiltroManMapping : EntityTypeConfiguration<tblM_CatFiltroMant>
    {
        public CatFiltroManMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.descripcion).HasColumnName("Descripcion");
            Property(x => x.estado).HasColumnName("estado");
            Property(x => x.marca).HasColumnName("marca");
            Property(x => x.modelo).HasColumnName("modelo");
            Property(x => x.sintetico).HasColumnName("sintetico");
            ToTable("tblM_CatFiltroMant");
        }

    }
}


