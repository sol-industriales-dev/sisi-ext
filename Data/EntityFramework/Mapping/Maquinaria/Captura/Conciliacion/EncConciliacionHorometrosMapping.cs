using Core.Entity.Maquinaria.Captura;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.Maquinaria.Captura.Conciliacion
{
    public class EncConciliacionHorometrosMapping : EntityTypeConfiguration<tblM_CapEncConciliacionHorometros>
    {
        public EncConciliacionHorometrosMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.centroCostosID).HasColumnName("centroCostosID");
            Property(x => x.estatus).HasColumnName("estatus");
            Property(x => x.FechaCaptura).HasColumnName("FechaCaptura");
            Property(x => x.fechaID).HasColumnName("fechaID");
            Property(x => x.usuarioCaptura).HasColumnName("usuarioCaptura");
            Property(x => x.anio).HasColumnName("anio");
            Property(x => x.fechaInicio).HasColumnName("fechaInicio");
            Property(x => x.fechaFin).HasColumnName("fechaFin");
            Property(x => x.facturado).HasColumnName("facturado");
            Property(x => x.factura).HasColumnName("factura");

            ToTable("tblM_CapEncConciliacionHorometros");
        }
    }

}
