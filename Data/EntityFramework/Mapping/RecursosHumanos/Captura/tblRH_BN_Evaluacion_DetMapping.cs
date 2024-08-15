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
    public class tblRH_BN_Evaluacion_DetMapping : EntityTypeConfiguration<tblRH_BN_Evaluacion_Det>
    {
        public tblRH_BN_Evaluacion_DetMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.plantillaID).HasColumnName("plantillaID");
            Property(x => x.plantillaDetID).HasColumnName("plantillaDetID");
            Property(x => x.evaluacionID).HasColumnName("evaluacionID");
            Property(x => x.cve_Emp).HasColumnName("cve_Emp");
            Property(x => x.nombre_Emp).HasColumnName("nombre_Emp");
            Property(x => x.puestoCve_Emp).HasColumnName("puestoCve_Emp");
            Property(x => x.puesto_Emp).HasColumnName("puesto_Emp");
            Property(x => x.base_Emp).HasColumnName("base_Emp");
            Property(x => x.complemento_Emp).HasColumnName("complemento_Emp");
            Property(x => x.bono_FC).HasColumnName("bono_FC");
            Property(x => x.bono_Emp).HasColumnName("bono_Emp");
            Property(x => x.porcentaje_Asig).HasColumnName("porcentaje_Asig");
            Property(x => x.monto_Asig).HasColumnName("monto_Asig");
            Property(x => x.total_Nom).HasColumnName("total_Nom");
            Property(x => x.tipo_Nom).HasColumnName("tipo_Nom");
            Property(x => x.tipoCve_Nom).HasColumnName("tipoCve_Nom");
            Property(x => x.total_Mensual).HasColumnName("total_Mensual");
            Property(x => x.con_Bono).HasColumnName("con_Bono");
            Property(x => x.periodicidadCve).HasColumnName("periodicidadCve");
            Property(x => x.fechaAplicacion).HasColumnName("fechaAplicacion");
            Property(x => x.periodoNomina).HasColumnName("periodoNomina");

            
            HasRequired(x => x.plantilla).WithMany().HasForeignKey(y => y.plantillaID);
            HasRequired(x => x.plantillaDet).WithMany().HasForeignKey(y => y.plantillaDetID);
            HasRequired(x => x.evaluacion).WithMany().HasForeignKey(y => y.evaluacionID);


            ToTable("tblRH_BN_Evaluacion");
        }
    }
}
