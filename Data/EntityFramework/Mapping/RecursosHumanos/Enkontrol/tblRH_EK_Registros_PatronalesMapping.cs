using Core.Entity.Administrativo.RecursosHumanos.Enkontrol;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.RecursosHumanos.Enkontrol
{
    public class tblRH_EK_Registros_PatronalesMapping : EntityTypeConfiguration<tblRH_EK_Registros_Patronales>
    {
        public tblRH_EK_Registros_PatronalesMapping()
        {
            HasKey(e => e.clave_reg_pat);
            Property(e => e.clave_reg_pat).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("clave_reg_pat");

            ToTable("tblRH_EK_Registros_Patronales");
        }
    }
}
