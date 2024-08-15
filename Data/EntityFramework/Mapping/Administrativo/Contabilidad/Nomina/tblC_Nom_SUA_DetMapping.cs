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
    public class tblC_Nom_SUA_DetMapping : EntityTypeConfiguration<tblC_Nom_SUA_Det>
    {
        public tblC_Nom_SUA_DetMapping() 
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.suaID).HasColumnName("suaID");
            Property(x => x.clave_empleado).HasColumnName("clave_empleado");
            Property(x => x.nss).HasColumnName("nss");
            Property(x => x.rfc).HasColumnName("rfc");
            Property(x => x.nombre).HasColumnName("nombre");
            Property(x => x.cc).HasColumnName("cc");
            Property(x => x.dias).HasColumnName("dias");
            Property(x => x.lic).HasColumnName("lic");
            Property(x => x.salarioDiario).HasColumnName("salarioDiario");
            Property(x => x.incapacidades).HasColumnName("incapacidades");
            Property(x => x.ausentismos).HasColumnName("ausentismos");
            Property(x => x.cuotaFija).HasColumnName("cuotaFija");
            Property(x => x.excedentePatronal).HasColumnName("excedentePatronal");
            Property(x => x.excedenteObrera).HasColumnName("excedenteObrera");
            Property(x => x.prestacionesPatronal).HasColumnName("prestacionesPatronal");
            Property(x => x.prestacionesObrera).HasColumnName("prestacionesObrera");
            Property(x => x.gastosMedicosPatronal).HasColumnName("gastosMedicosPatronal");
            Property(x => x.gastosMedicosObrera).HasColumnName("gastosMedicosObrera");
            Property(x => x.riesgosTrabajo).HasColumnName("riesgosTrabajo");
            Property(x => x.invalidezVidaPatronal).HasColumnName("invalidezVidaPatronal");
            Property(x => x.invalidezVidaObrera).HasColumnName("invalidezVidaObrera");
            Property(x => x.guarderiasPrestaciones).HasColumnName("guarderiasPrestaciones");
            Property(x => x.patronal).HasColumnName("patronal");
            Property(x => x.obrera).HasColumnName("obrera");
            Property(x => x.subtotal).HasColumnName("subtotal");
            HasRequired(x => x.sua).WithMany().HasForeignKey(x => x.suaID);

            ToTable("tblC_Nom_SUA_Det");
        }
    }
}
