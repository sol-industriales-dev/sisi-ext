using Core.Entity.Administrativo.Seguridad.Indicadores;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.Administrativo.Seguridad.Indicadores
{
    class tblS_IncidentesInformePreliminarMapping : EntityTypeConfiguration<tblS_IncidentesInformePreliminar>
    {
        public tblS_IncidentesInformePreliminarMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.folio).HasColumnName("folio");
            Property(x => x.claveEmpleado).HasColumnName("claveEmpleado");
            Property(x => x.personaInformo).HasColumnName("personaInformo");
            Property(x => x.cc).HasColumnName("cc");
            Property(x => x.fechaInforme).HasColumnName("fechaInforme");
            Property(x => x.fechaIncidente).HasColumnName("fechaIncidente");
            Property(x => x.fechaIngresoEmpleado).HasColumnName("fechaIngresoEmpleado");
            Property(x => x.departamentoEmpleado).HasColumnName("departamentoEmpleado");
            Property(x => x.claveSupervisor).HasColumnName("claveSupervisor").IsOptional();
            Property(x => x.supervisorEmpleado).HasColumnName("supervisorEmpleado");
            Property(x => x.tipoLesion).HasColumnName("tipoLesion");
            Property(x => x.descripcionIncidente).HasColumnName("descripcionIncidente");
            Property(x => x.accionInmediata).HasColumnName("accionInmediata");
            Property(x => x.terminado).HasColumnName("terminado");
            Property(x => x.aplicaRIA).HasColumnName("aplicaRIA");
            Property(x => x.riesgo).HasColumnName("riesgo");
            Property(x => x.esExterno).HasColumnName("esExterno");
            Property(x => x.nombreExterno).HasColumnName("nombreExterno");
            Property(x => x.claveContratista).HasColumnName("claveContratista");
            Property(x => x.rutaPreliminar).HasColumnName("rutaPreliminar");
            Property(x => x.rutaRIA).HasColumnName("rutaRIA");
            Property(x => x.estatusAvance).HasColumnName("estatusAvance");

            Property(x => x.tipoAccidente_id).HasColumnName("tipoAccidente_id");
            HasRequired(x => x.TiposAccidente).WithMany(x => x.InformesPreliminares).HasForeignKey(d => d.tipoAccidente_id);
            Property(x => x.subclasificacionID).HasColumnName("subclasificacionID");

            Property(x => x.tipoContacto_id).HasColumnName("tipoContacto_id");
            HasRequired(x => x.TipoContacto).WithMany().HasForeignKey(d => d.tipoContacto_id);

            Property(x => x.parteCuerpo_id).HasColumnName("parteCuerpo_id");
            HasRequired(x => x.ParteCuerpo).WithMany().HasForeignKey(d => d.parteCuerpo_id);

            Property(x => x.agenteImplicado_id).HasColumnName("agenteImplicado_id");
            HasRequired(x => x.AgenteImplicado).WithMany().HasForeignKey(d => d.agenteImplicado_id);

            Property(x => x.departamento_id).HasColumnName("departamento_id");
            HasRequired(x => x.Departamentos).WithMany(x => x.InformesPreliminares).HasForeignKey(d => d.departamento_id);

            HasMany(x => x.procedimientosViolados)
                .WithMany(x => x.lstInformes)
                .Map(ip =>
                {
                    ip.MapLeftKey("idInformePreliminar");
                    ip.MapRightKey("idProcedimientoViolado");
                    ip.ToTable("tblS_IncidentesInformePreliminarProcedimientoViolado");
                });
            
            #region Campos nuevos del RIA.
            Property(x => x.lugarAccidente).HasColumnName("lugarAccidente");
            Property(x => x.tipoLesion_id).HasColumnName("tipoLesion_id");
            HasRequired(x => x.TiposLesion).WithMany(x => x.Informes).HasForeignKey(d => d.tipoLesion_id);
            Property(x => x.actividadRutinaria).HasColumnName("actividadRutinaria");
            Property(x => x.trabajoPlaneado).HasColumnName("trabajoPlaneado");
            Property(x => x.trabajoRealizaba).HasColumnName("trabajoRealizaba");
            Property(x => x.protocoloTrabajo_id).HasColumnName("protocoloTrabajo_id");
            HasRequired(x => x.ProtocolosTrabajo).WithMany(x => x.Informes).HasForeignKey(d => d.protocoloTrabajo_id);
            Property(x => x.experienciaEmpleado_id).HasColumnName("experienciaEmpleado_id");
            HasRequired(x => x.ExperienciaEmpleado).WithMany(x => x.Informes).HasForeignKey(d => d.experienciaEmpleado_id);
            Property(x => x.antiguedadEmpleado_id).HasColumnName("antiguedadEmpleado_id");
            HasRequired(x => x.AntiguedadEmpleado).WithMany(x => x.Informes).HasForeignKey(d => d.antiguedadEmpleado_id);
            Property(x => x.turnoEmpleado_id).HasColumnName("turnoEmpleado_id");
            HasRequired(x => x.TurnoEmpleado).WithMany(x => x.Informes).HasForeignKey(d => d.turnoEmpleado_id);
            Property(x => x.horasTrabajadasEmpleado).HasColumnName("horasTrabajadasEmpleado");
            Property(x => x.diasTrabajadosEmpleado).HasColumnName("diasTrabajadosEmpleado");
            Property(x => x.capacitadoEmpleado).HasColumnName("capacitadoEmpleado");
            Property(x => x.accidentesAnterioresEmpleado).HasColumnName("accidentesAnterioresEmpleado");
            Property(x => x.descripcionAccidente).HasColumnName("descripcionAccidente");
            #endregion
            Property(x => x.idEmpresa).HasColumnName("idEmpresa");
            Property(x => x.idAgrupacion).HasColumnName("idAgrupacion");

            ToTable("tblS_IncidentesInformePreliminar");
        }
    }
}
