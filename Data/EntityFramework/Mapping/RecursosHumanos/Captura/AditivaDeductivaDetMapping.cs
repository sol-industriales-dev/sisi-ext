using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity.ModelConfiguration;//entitytpype configuration
using Core.Entity.RecursosHumanos.Captura;
using System.ComponentModel.DataAnnotations.Schema;//databasegeneratedoption
namespace Data.EntityFramework.Mapping.RecursosHumanos.Captura
{
   public class AditivaDeductivaDetMapping:EntityTypeConfiguration<tblRH_AditivaDeductivaDet>
    {
       public AditivaDeductivaDetMapping()
       {
           HasKey(x => x.id);
           Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
           Property(x => x.id_AditivaDeductiva).HasColumnName("id_AditivaDeductiva");//llave foranea
           Property(x => x.categoria).HasColumnName("categoria");
           Property(x => x.personalNecesario).HasColumnName("personalNecesario");
           Property(x => x.personalExistente).HasColumnName("personalExistente");
           Property(x => x.personalFaltante).HasColumnName("personalFaltante");
           Property(x => x.lugaresPlantilla).HasColumnName("lugaresPlantilla");
           Property(x => x.numPersTotal).HasColumnName("numPersTotal");
           Property(x => x.aditiva).HasColumnName("aditiva");
           Property(x => x.deductiva).HasColumnName("deductiva");
           Property(x => x.justificacion).HasColumnName("justificacion");
           Property(x => x.puesto).HasColumnName("puesto");
           Property(x => x.estado).HasColumnName("estado");
           Property(x => x.nuevo).HasColumnName("nuevo");
           ToTable("tblRH_AditivaDeductivaDet");
       }
    }
}

