using Core.Entity.ControlObra;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.ControlObra
{
    public class tblCO_ADP_EvalSubContratistaMapping : EntityTypeConfiguration<tblCO_ADP_EvalSubContratista>
    {
        public tblCO_ADP_EvalSubContratistaMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");

            Property(x => x.idAreaCuenta).HasColumnName("idAreaCuenta");
            Property(x => x.idSubContratista).HasColumnName("idSubContratista");
            Property(x => x.Calificacion).HasColumnName("Calificacion");
            Property(x => x.firmaAutorizacion).HasColumnName("firmaAutorizacion");
            Property(x => x.fechaAutorizacion).HasColumnName("fechaAutorizacion");
            Property(x => x.usuarioAutorizacion).HasColumnName("usuarioAutorizacion");
            Property(x => x.evaluacionPendiente).HasColumnName("evaluacionPendiente");
            Property(x => x.idSubConAsignacion).HasColumnName("idSubConAsignacion");
            HasRequired(x => x.evaluacionConAsignacion).WithMany().HasForeignKey(x => x.idSubConAsignacion);

            ToTable("tblCO_ADP_EvalSubContratista");
        }
    }
}
