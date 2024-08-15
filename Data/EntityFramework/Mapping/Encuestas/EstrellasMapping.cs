using Core.Entity.Encuestas;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.Encuestas
{
    class EstrellasMapping : EntityTypeConfiguration<tblEN_Estrellas>
    {
        public EstrellasMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");

            Property(x => x.estrellas).HasColumnName("estrellas");
            Property(x => x.descripcion).HasColumnName("descripcion");
            Property(x => x.minimo).HasColumnName("minimo");
            Property(x => x.maximo).HasColumnName("maximo");
            Property(x => x.estatus).HasColumnName("estatus");

            ToTable("tblEN_Estrellas");
        }
    }
}
