using Core.Entity.Maquinaria.Inventario;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.Maquinaria.Inventario
{
    class SolicitudReemplazoDetMapping : EntityTypeConfiguration<tblM_SolicitudReemplazoDet>
    {
        SolicitudReemplazoDetMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.AsignacionEquiposID).HasColumnName("AsignacionEquiposID");
            Property(x => x.SolicitudEquipoReemplazoID).HasColumnName("SolicitudEquipoReemplazoID");
            Property(x => x.estatus).HasColumnName("estatus");
            Property(x => x.nombreArchivo).HasColumnName("nombreArchivo");
            Property(x => x.ruta).HasColumnName("ruta");
            Property(x => x.Comentario).HasColumnName("Comentario");
            Property(x => x.fechaInicio).HasColumnName("fechaInicio");
            Property(x => x.fechaFin).HasColumnName("fechaFin");
            HasRequired(x => x.SolicitudEquipoReemplazo).WithMany().HasForeignKey(y => y.SolicitudEquipoReemplazoID);
            HasRequired(x => x.AsignacionEquipos).WithMany().HasForeignKey(y => y.AsignacionEquiposID);

            ToTable("tblM_SolicitudReemplazoDet");
        }
    }
}
