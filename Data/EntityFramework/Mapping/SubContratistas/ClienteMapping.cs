using Core.Entity.SubContratistas;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.SubContratistas
{
    public class ClienteMapping : EntityTypeConfiguration<tblX_Cliente>
    {
        public ClienteMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");

            Property(x => x.nombre).HasColumnName("nombre");
            Property(x => x.direccion).HasColumnName("direccion");
            Property(x => x.nombreCorto).HasColumnName("nombreCorto");
            Property(x => x.codigoPostal).HasColumnName("codigoPostal");
            Property(x => x.rfc).HasColumnName("rfc");
            Property(x => x.correo).HasColumnName("correo");
            Property(x => x.fisica).HasColumnName("fisica");
            Property(x => x.estatus).HasColumnName("estatus");

            ToTable("tblX_Cliente");
        }
    }
}
