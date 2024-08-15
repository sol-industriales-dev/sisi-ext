using Core.Entity.Maquinaria.Inventario.ControlCalidad;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.Maquinaria.Inventario.ControlCalidad
{
    public class ControlMovimientoMaquinariaMapping : EntityTypeConfiguration<tblM_ControlMovimientoMaquinaria>
    {
       public ControlMovimientoMaquinariaMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.bitacora).HasColumnName("bitacora");
            Property(x => x.companiaTransporte).HasColumnName("companiaTransporte");
            Property(x => x.compañiaResponsable).HasColumnName("compañiaResponsable");
            Property(x => x.controlCalidad).HasColumnName("controlCalidad");
            Property(x => x.diasTranslado).HasColumnName("diasTranslado");
            Property(x => x.fechaElaboracion).HasColumnName("fechaElaboracion");
            Property(x => x.fechaRecepcionEmbarque).HasColumnName("fechaRecepcionEmbarque");
            Property(x => x.firma).HasColumnName("firma");
            Property(x => x.horometros).HasColumnName("horometros");
            Property(x => x.kilometraje).HasColumnName("kilometraje");
            Property(x => x.lugar).HasColumnName("lugar");
            Property(x => x.noEconomico).HasColumnName("noEconomico");
            Property(x => x.nombreResponsable).HasColumnName("nombreResponsable");
            Property(x => x.nota).HasColumnName("nota");
            Property(x => x.pedAduana).HasColumnName("pedAduana");
            Property(x => x.placas).HasColumnName("placas");
            Property(x => x.responsableTrasnporte).HasColumnName("responsableTrasnporte");
            Property(x => x.tanque1).HasColumnName("tanque1");
            Property(x => x.tanque2).HasColumnName("tanque2");
            Property(x => x.lugarRecepcion).HasColumnName("lugarRecepcion");

            Property(x => x.ReporteFalla).HasColumnName("ReporteFalla");
            Property(x => x.copiaFactura).HasColumnName("copiaFactura");
            Property(x => x.manualMant).HasColumnName("manualMant");
            Property(x => x.manualOperacion).HasColumnName("manualOperacion");


            Property(x => x.tipoControl).HasColumnName("tipoControl");
            Property(x => x.Transporte).HasColumnName("Transporte");
            Property(x => x.solicitudEquipoID).HasColumnName("solicitudEquipoID");
            Property(x => x.asignacionEquipoId).HasColumnName("asignacionEquipoId");


            Property(x => x.nombreResponsableEnvio).HasColumnName("nombreResponsableEnvio");
            Property(x => x.compañiaResponsableEnvio).HasColumnName("compañiaResponsableEnvio");
            Property(x => x.nombreResponsableRecepcion).HasColumnName("nombreResponsableRecepcion");
            Property(x => x.compañiaResponsableRecepcion).HasColumnName("compañiaResponsableRecepcion");

            Property(x => x.Nombre).HasColumnName("Nombre");
            Property(x => x.RutaArchivo).HasColumnName("RutaArchivo");

            HasRequired(x => x.SolicitudEquipo).WithMany().HasForeignKey(y => y.solicitudEquipoID);

            ToTable("tblM_ControlMovimientoMaquinaria");
        }
    }
}
