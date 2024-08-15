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
    public class tblOS_SALAS_SalaJuntasMapping : EntityTypeConfiguration<tblOS_SALAS_SalaJuntas>
    {
        public tblOS_SALAS_SalaJuntasMapping()
        {
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            ToTable("tblOS_SALAS_SalaJuntas");
        }
    }
}
