using Core.Entity.Administrativo.Contratistas;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.Administrativo.Contratistas
{
    public class tblP_EstadoMapping : EntityTypeConfiguration<tblP_Estado>
    {
        public tblP_EstadoMapping()
        {
            HasKey(x => x.idEstado);
            Property(x => x.idEstado).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("idEstado");
            Property(x => x.idPais).HasColumnName("idPais");
            Property(x => x.Estado).HasColumnName("Estado");

            ToTable("tblP_Estado");
        }
    }
}
