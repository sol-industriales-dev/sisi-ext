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
    public class tblRH_EK_Empl_Duracion_ContratoMapping : EntityTypeConfiguration<tblRH_EK_Empl_Duracion_Contrato>
    {
        public tblRH_EK_Empl_Duracion_ContratoMapping()
        {
            HasKey(x => x.clave_duracion);
            Property(x => x.clave_duracion).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("clave_duracion");

            ToTable("tblRH_EK_Empl_Duracion_Contrato");
        }
    }
}
