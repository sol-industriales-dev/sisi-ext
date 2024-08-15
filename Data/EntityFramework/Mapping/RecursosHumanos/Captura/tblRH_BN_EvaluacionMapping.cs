using Core.Entity.RecursosHumanos.Captura;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.RecursosHumanos.Captura
{
    public class tblRH_BN_EvaluacionMapping : EntityTypeConfiguration<tblRH_BN_Evaluacion>
    {
        public tblRH_BN_EvaluacionMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.plantillaID).HasColumnName("plantillaID");
            Property(x => x.fecha).HasColumnName("fecha");
            Property(x => x.estatus).HasColumnName("estatus");
            Property(x => x.usuarioEvaluoID).HasColumnName("usuarioEvaluoID");
            Property(x => x.cc).HasColumnName("cc");
            Property(x => x.tipoNomina).HasColumnName("tipoNomina");
            Property(x => x.anio).HasColumnName("anio");
            Property(x => x.periodo).HasColumnName("periodo");
            Property(x => x.fechaInicio).HasColumnName("fechaInicio");
            Property(x => x.fechaFin).HasColumnName("fechaFin");
            Property(x => x.fechaAplicacion).HasColumnName("fechaAplicacion");
            Property(x => x.aplicado).HasColumnName("aplicado");

            HasRequired(x => x.usuarioEvaluo).WithMany().HasForeignKey(y => y.usuarioEvaluoID);
            HasRequired(x => x.plantilla).WithMany().HasForeignKey(y => y.plantillaID);
            HasMany(x => x.listDetalle).WithRequired(x => x.evaluacion).HasForeignKey(x => x.evaluacionID);
            HasMany(x => x.listAutorizadores).WithRequired(x => x.evaluacion).HasForeignKey(x => x.evaluacionID);

            ToTable("tblRH_BN_Evaluacion");
        }
    }
}
