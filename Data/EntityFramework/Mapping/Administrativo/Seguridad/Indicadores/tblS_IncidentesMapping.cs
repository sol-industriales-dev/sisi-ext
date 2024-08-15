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
    class tblS_IncidentesMapping : EntityTypeConfiguration<tblS_Incidentes>
    {
        public tblS_IncidentesMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.claveContratista).HasColumnName("claveContratista");
            Property(x => x.claveEmpleado).HasColumnName("claveEmpleado");
            Property(x => x.edadEmpleado).HasColumnName("edadEmpleado");
            Property(x => x.horasTrabajadasEmpleado).HasColumnName("horasTrabajadasEmpleado");
            Property(x => x.diasTrabajadosEmpleado).HasColumnName("diasTrabajadosEmpleado");
            Property(x => x.riesgo).HasColumnName("riesgo");
            Property(x => x.esExterno).HasColumnName("esExterno");
            Property(x => x.trabajoPlaneado).HasColumnName("trabajoPlaneado");
            Property(x => x.actividadRutinaria).HasColumnName("actividadRutinaria");
            Property(x => x.capacitadoEmpleado).HasColumnName("capacitadoEmpleado");
            Property(x => x.accidentesAnterioresEmpleado).HasColumnName("accidentesAnterioresEmpleado");
            Property(x => x.cc).HasColumnName("cc");
            Property(x => x.lugarAccidente).HasColumnName("lugarAccidente");
            Property(x => x.fechaAccidente).HasColumnName("fechaAccidente");
            Property(x => x.trabajoRealizaba).HasColumnName("trabajoRealizaba");
            Property(x => x.puestoEmpleado).HasColumnName("puestoEmpleado");
            Property(x => x.claveSupervisor).HasColumnName("claveSupervisor").IsOptional();
            Property(x => x.supervisorCargoEmpleado).HasColumnName("supervisorCargoEmpleado");
            Property(x => x.descripcionAccidente).HasColumnName("descripcionAccidente");
            Property(x => x.instruccionTrabajo).HasColumnName("instruccionTrabajo");
            Property(x => x.porqueSehizo).HasColumnName("porqueSehizo");
            Property(x => x.nombreEmpleadoExterno).HasColumnName("nombreEmpleadoExterno");
            Property(x => x.lugarJunta).HasColumnName("lugarJunta");
            Property(x => x.fechaJunta).HasColumnName("fechaJunta");
            Property(x => x.horaInicio).HasColumnName("horaInicio");
            Property(x => x.horaFin).HasColumnName("horaFin");

            Property(x => x.informe_id).HasColumnName("informe_id");
            HasRequired(x => x.Informe).WithMany(x => x.Incidentes).HasForeignKey(x => x.informe_id);

            Property(x => x.tipoAccidente_id).HasColumnName("tipoAccidente_id");
            Property(x => x.subclasificacionID).HasColumnName("subclasificacionID");
            HasRequired(x => x.TiposAccidente).WithMany(x => x.Incidentes).HasForeignKey(d => d.tipoAccidente_id);

            Property(x => x.departamento_id).HasColumnName("departamento_id");
            HasRequired(x => x.Departamentos).WithMany(x => x.Incidentes).HasForeignKey(d => d.departamento_id);

            Property(x => x.tipoLesion_id).HasColumnName("tipoLesion_id");
            HasRequired(x => x.TiposLesion).WithMany(x => x.Incidentes).HasForeignKey(d => d.tipoLesion_id);

            Property(x => x.parteCuerpo_id).HasColumnName("parteCuerpo_id");
            HasRequired(x => x.PartesCuerpo).WithMany(x => x.Incidentes).HasForeignKey(d => d.parteCuerpo_id);

            Property(x => x.agenteImplicado_id).HasColumnName("agenteImplicado_id");
            HasRequired(x => x.AgentesImplicados).WithMany(x => x.Incidentes).HasForeignKey(d => d.agenteImplicado_id);

            Property(x => x.experienciaEmpleado_id).HasColumnName("experienciaEmpleado_id");
            HasRequired(x => x.ExperienciaEmpleado).WithMany(x => x.Incidentes).HasForeignKey(d => d.experienciaEmpleado_id);

            Property(x => x.antiguedadEmpleado_id).HasColumnName("antiguedadEmpleado_id");
            HasRequired(x => x.AntiguedadEmpleado).WithMany(x => x.Incidentes).HasForeignKey(d => d.antiguedadEmpleado_id);

            Property(x => x.turnoEmpleado_id).HasColumnName("turnoEmpleado_id");
            HasRequired(x => x.TurnoEmpleado).WithMany(x => x.Incidentes).HasForeignKey(d => d.turnoEmpleado_id);

            Property(x => x.tipoContacto_id).HasColumnName("tipoContacto_id");
            HasRequired(x => x.TiposContacto).WithMany(x => x.Incidentes).HasForeignKey(d => d.tipoContacto_id);

            Property(x => x.protocoloTrabajo_id).HasColumnName("protocoloTrabajo_id");
            HasRequired(x => x.ProtocolosTrabajo).WithMany(x => x.Incidentes).HasForeignKey(d => d.protocoloTrabajo_id);

            Property(x => x.tecnicaInvestigacion_id).HasColumnName("tecnicaInvestigacion_id");
            HasRequired(x => x.TecnicasInvestigacion).WithMany(x => x.Incidentes).HasForeignKey(d => d.tecnicaInvestigacion_id);

            Property(x => x.idEmpresa).HasColumnName("idEmpresa");
            Property(x => x.idAgrupacion).HasColumnName("idAgrupacion");

            ToTable("tblS_Incidentes");
        }
    }
}
