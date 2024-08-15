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
    public class tblRH_EK_Geo_DepartamentosMapping : EntityTypeConfiguration<tblRH_EK_Geo_Departamentos>
    {
        public tblRH_EK_Geo_DepartamentosMapping()
        {
            HasKey(e => e.id);
            Property(e => e.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("clave_reg_pat");

            ToTable("tblRH_EK_Geo_Departamentos");
        }
    }
}
