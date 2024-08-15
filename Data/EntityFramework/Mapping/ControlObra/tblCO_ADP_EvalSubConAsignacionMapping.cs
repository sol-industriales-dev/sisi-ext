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
    public class tblCO_ADP_EvalSubConAsignacionMapping : EntityTypeConfiguration<tblCO_ADP_EvalSubConAsignacion>
    {
        public tblCO_ADP_EvalSubConAsignacionMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            HasRequired(x => x.subcontratista).WithMany().HasForeignKey(x => x.idSubContratista);
            HasRequired(x => x.contrato).WithMany().HasForeignKey(x => x.idContrato);

            HasRequired(x => x.subcontratista).WithMany().HasForeignKey(x => x.idSubContratista);
            HasRequired(x => x.contrato).WithMany().HasForeignKey(x => x.idContrato);
            
            ToTable("tblCO_ADP_EvalSubConAsignacion");
        }
    }
}
