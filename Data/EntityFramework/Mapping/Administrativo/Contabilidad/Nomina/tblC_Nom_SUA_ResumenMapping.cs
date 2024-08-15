using Core.Entity.Administrativo.Contabilidad.Nomina;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Entity.Administrativo.Contabilidad.Nomina;

namespace Data.EntityFramework.Mapping.Administrativo.Contabilidad.Nomina
{
    public class tblC_Nom_SUA_ResumenMapping : EntityTypeConfiguration<tblC_Nom_SUA_Resumen>
    {
        public tblC_Nom_SUA_ResumenMapping() 
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.suaID).HasColumnName("suaID");
            Property(x => x.cc).HasColumnName("cc");
            Property(x => x.ac).HasColumnName("ac");
            Property(x => x.estado).HasColumnName("estado");
            Property(x => x.registroPatronal).HasColumnName("registroPatronal");
            Property(x => x.descripcionCC).HasColumnName("descripcionCC");
            Property(x => x.imssPatronal).HasColumnName("imssPatronal");
            Property(x => x.imssObrero).HasColumnName("imssObrero");
            Property(x => x.rcvPatronal).HasColumnName("rcvPatronal");
            Property(x => x.rcvObrero).HasColumnName("rcvObrero");
            Property(x => x.infonavit).HasColumnName("infonavit");
            Property(x => x.amortizacion).HasColumnName("amortizacion");
            Property(x => x.isn).HasColumnName("isn");
            Property(x => x.registroActivo).HasColumnName("registroActivo");
            HasRequired(x => x.sua).WithMany().HasForeignKey(x => x.suaID);

            ToTable("tblC_Nom_SUA_Resumen");
        }
    }
}
