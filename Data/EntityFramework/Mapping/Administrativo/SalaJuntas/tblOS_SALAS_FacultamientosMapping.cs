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
    public class tblOS_SALAS_FacultamientosMapping : EntityTypeConfiguration<tblOS_SALAS_Facultamientos>
    {
        public tblOS_SALAS_FacultamientosMapping()
        {
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            ToTable("tblOS_SALAS_Facultamientos");
        }
    }
}
