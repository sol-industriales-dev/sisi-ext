using Core.Entity.Administrativo.Seguridad.Requerimientos;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.Administrativo.Seguridad.Requerimientos
{
    public class EvidenciaAuditoriaMapping : EntityTypeConfiguration<tblS_Req_Evidencia_Auditoria>
    {
        public EvidenciaAuditoriaMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");

            Property(x => x.cc).HasColumnName("cc");
            Property(x => x.requerimientoID).HasColumnName("requerimientoID");
            Property(x => x.puntoID).HasColumnName("puntoID");
            Property(x => x.fechaPunto).HasColumnName("fechaPunto");
            Property(x => x.rutaEvidencia).HasColumnName("rutaEvidencia");
            Property(x => x.comentariosCaptura).HasColumnName("comentariosCaptura");
            Property(x => x.fechaCaptura).HasColumnName("fechaCaptura");
            Property(x => x.usuarioCapturaID).HasColumnName("usuarioCapturaID");
            Property(x => x.usuarioEvaluadorID).HasColumnName("usuarioEvaluadorID");
            Property(x => x.comentariosEvaluador).HasColumnName("comentariosEvaluador");
            Property(x => x.aprobado).HasColumnName("aprobado");
            Property(x => x.calificacion).HasColumnName("calificacion");
            Property(x => x.fechaEvaluacion).HasColumnName("fechaEvaluacion");
            Property(x => x.division).HasColumnName("division");
            Property(x => x.estatus).HasColumnName("estatus");
            Property(x => x.idEmpresa).HasColumnName("idEmpresa");
            Property(x => x.idAgrupacion).HasColumnName("idAgrupacion");

            ToTable("tblS_Req_Evidencia_Auditoria");
        }
    }
}
