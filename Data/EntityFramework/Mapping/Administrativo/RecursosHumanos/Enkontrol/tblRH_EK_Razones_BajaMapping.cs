using Core.Entity.Administrativo.RecursosHumanos.Enkontrol;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.Administrativo.RecursosHumanos.Enkontrol
{
    public class tblRH_EK_Razones_BajaMapping : EntityTypeConfiguration<tblRH_EK_Razones_Baja>
    {
       public tblRH_EK_Razones_BajaMapping()
       {
           HasKey(x => x.clave_razon_baja);
           Property(x => x.clave_razon_baja).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("clave_razon_baja");
           ToTable("tblRH_EK_Razones_Baja");
       }
    }
}
