using Core.Entity.RecursosHumanos.Bajas;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.RecursosHumanos.Bajas
{
    public class tblRH_Baja_RegistroMapping : EntityTypeConfiguration<tblRH_Baja_Registro>
    {
        public tblRH_Baja_RegistroMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");

            ToTable("tblRH_Baja_Registro");
        }
    }
}
