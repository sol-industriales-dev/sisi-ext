using Core.Entity.RecursosHumanos.Desempeno;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.RecursosHumanos.Desempeno
{
    public class DetObservacionMapping : EntityTypeConfiguration<tblRH_ED_DetObservacion>
    {
        public DetObservacionMapping()
        {
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.idMeta).HasColumnName("idMeta");
            Property(x => x.idEvaluacion).HasColumnName("idEvaluacion");
            Property(x => x.idUsuario).HasColumnName("idUsuario");
            Property(x => x.idJefe).HasColumnName("idJefe");
            Property(x => x.autoEvaluacion).HasColumnName("autoEvaluacion").HasPrecision(20, 4);
            Property(x => x.jefeEvaluacion).HasColumnName("jefeEvaluacion").HasPrecision(20, 4);
            Property(x => x.autoObservacion).HasColumnName("autoObservacion");
            Property(x => x.jefeObservacion).HasColumnName("jefeObservacion");
            Property(x => x.esAutoEvaluado).HasColumnName("esAutoEvaluado");
            Property(x => x.esJefeEvaluado).HasColumnName("esJefeEvaluado");
            Property(x => x.notificado).HasColumnName("notificado");
            Property(x => x.notificadoJefeAUsuario).HasColumnName("notificadoJefeAUsuario");
            Property(x => x.esActivo).HasColumnName("esActivo");
            Property(x => x.fechaRegistro).HasColumnName("fechaRegistro");
            HasRequired(x => x.meta).WithMany().HasForeignKey(y => y.idMeta);
            HasRequired(x => x.evaluacion).WithMany().HasForeignKey(y => y.idEvaluacion);
            HasRequired(x => x.usuario).WithMany().HasForeignKey(y => y.idUsuario);
            HasRequired(x => x.jefe).WithMany().HasForeignKey(y => y.idJefe);
            HasRequired(x => x.lstEvidencia).WithMany().HasForeignKey(y => y.id);
            ToTable("tblRH_ED_DetObservacion");
        }
    }
}
