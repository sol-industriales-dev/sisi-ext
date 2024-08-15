using Core.Entity.Maquinaria.Reporte;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.Maquinaria.Reporte
{
    public class RptIndicadorMapping : EntityTypeConfiguration<tblM_RptIndicador>
    {
        RptIndicadorMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.Tipo).HasColumnName("Tipo");
            Property(x => x.FechaInicio).HasColumnName("FechaInicio");
            Property(x => x.FechaFin).HasColumnName("FechaFin");
            Property(x => x.datosJson).HasColumnName("datosJson");
            Property(x => x.Conclusion).HasColumnName("Conclusion");
            Property(x => x.Tc).HasColumnName("Tc");
            Property(x => x.CC).HasColumnName("CC");
            ToTable("tblM_RptIndicador");
        }
    }
}
