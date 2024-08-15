using Core.Entity.Maquinaria.Captura;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.Maquinaria.Captura
{
    public class MotivosParoMapping : EntityTypeConfiguration<tblM_CatCriteriosCausaParo>
    {
        MotivosParoMapping()
        {
            HasKey(x => x);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.CausaParo).HasColumnName("CausaParo");
            Property(x => x.DescripcionParo).HasColumnName("DescripcionParo");
            Property(x => x.TiempoMantenimiento).HasColumnName("TiempoMantenimiento");
            Property(x => x.TipoParo).HasColumnName("TipoParo");

            ToTable("tblM_CatCriteriosCausaParo");
        }
    }
}
