using Core.Entity.Administrativo.SalaJuntas;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.Administrativo.SalaJuntas
{
    public class tbIP_SalasMapping : EntityTypeConfiguration<tblP_Salas>
    {
        public tbIP_SalasMapping()
        {

            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");

            ToTable("tblP_Salas");
        }

    }
}
