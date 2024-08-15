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
    public class AsignacionEquiposMapping : EntityTypeConfiguration<tblM_AsignacionEquipos>
    {
        public AsignacionEquiposMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.noEconomicoID).HasColumnName("noEconomicoID");
            Property(x => x.cc).HasColumnName("cc");
            Property(x => x.estatus).HasColumnName("estatus");
            Property(x => x.fechaAsignacion).HasColumnName("fechaAsignacion");
            Property(x => x.fechaFin).HasColumnName("fechaFin");
            Property(x => x.fechaInicio).HasColumnName("fechaInicio");
            Property(x => x.folio).HasColumnName("folio");
            Property(x => x.Horas).HasColumnName("Horas");
            Property(x => x.solicitudEquipoID).HasColumnName("solicitudEquipoID");
            Property(x => x.SolicitudDetalleId).HasColumnName("SolicitudDetalleId");
            Property(x => x.CCOrigen).HasColumnName("CCOrigen");
            Property(x => x.FechaPromesa).HasColumnName("FechaPromesa");
            Property(x => x.Economico).HasColumnName("Economico");
            //prueba ragilar
            Property(x => x.StepPen).HasColumnName("StepPen");
            HasRequired(x => x.SolicitudEquipo).WithMany().HasForeignKey(y => y.solicitudEquipoID);
            HasRequired(x => x.SolicitudDetalle).WithMany().HasForeignKey(y => y.SolicitudDetalleId);
            ToTable("tblM_AsignacionEquipos");
        }
    }
}
