using Core.Entity.Administrativo.Seguridad.Evaluacion;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.Administrativo.Seguridad.Evaluacion
{
    public class EvaluacionMapping : EntityTypeConfiguration<tblSED_Evaluacion>
    {
        public EvaluacionMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");

            Property(x => x.empleadoID).HasColumnName("empleadoID");
            Property(x => x.actividadID).HasColumnName("actividadID");
            Property(x => x.rutaEvidencia).HasColumnName("rutaEvidencia");
            Property(x => x.comentariosEmpleado).HasColumnName("comentariosEmpleado");
            Property(x => x.fechaActividad).HasColumnName("fechaActividad");
            Property(x => x.fechaCaptura).HasColumnName("fechaCaptura");
            Property(x => x.ponderacionActual).HasColumnName("ponderacionActual");
            Property(x => x.periodicidadActual).HasColumnName("periodicidadActual");
            Property(x => x.aplica).HasColumnName("aplica");
            Property(x => x.evaluadorID).HasColumnName("evaluadorID");
            Property(x => x.comentariosEvaluador).HasColumnName("comentariosEvaluador");
            Property(x => x.aprobado).HasColumnName("aprobado");
            Property(x => x.fechaEvaluacion).HasColumnName("fechaEvaluacion");
            Property(x => x.division).HasColumnName("division");
            Property(x => x.estatus).HasColumnName("estatus");

            ToTable("tblSED_Evaluacion");
        }
    }
}
