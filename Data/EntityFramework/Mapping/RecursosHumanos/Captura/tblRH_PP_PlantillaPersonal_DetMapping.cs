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
    public class tblRH_PP_PlantillaPersonal_DetMapping : EntityTypeConfiguration<tblRH_PP_PlantillaPersonal_Det>
    {
        public tblRH_PP_PlantillaPersonal_DetMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.plantillaID).HasColumnName("plantillaID");
            Property(x => x.puestoNumero).HasColumnName("puestoNumero");
            Property(x => x.puesto).HasColumnName("puesto");
            Property(x => x.departamentoNumero).HasColumnName("departamentoNumero");
            Property(x => x.departamento).HasColumnName("departamento");
            Property(x => x.tipoNomina).HasColumnName("tipoNomina");
            Property(x => x.personalNecesario).HasColumnName("personalNecesario");
            Property(x => x.sueldoBase).HasColumnName("sueldoBase");
            Property(x => x.sueldoComplemento).HasColumnName("sueldoComplemento");
            Property(x => x.sueldoTotal).HasColumnName("sueldoTotal");
            HasRequired(x => x.plantilla).WithMany().HasForeignKey(y => y.plantillaID);
            Property(x => x.plantilla_Puesto_EKID).HasColumnName("plantilla_Puesto_EKID");
            Property(x => x.tabulador_Puesto_EKID).HasColumnName("tabulador_Puesto_EKID");
            ToTable("tblRH_PP_PlantillaPersonal_Det");
        }
    }
}
