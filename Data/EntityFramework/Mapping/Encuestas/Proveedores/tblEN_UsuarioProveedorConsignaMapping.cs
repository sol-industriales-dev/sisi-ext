using Core.Entity.Encuestas.Proveedores;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.Encuestas.Proveedores
{
    public class tblEN_UsuarioProveedorConsignaMapping : EntityTypeConfiguration<tblEN_UsuarioProveedorConsigna>
    {
        public tblEN_UsuarioProveedorConsignaMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            ToTable("tblEN_UsuarioProveedorConsigna");
        }
    }
}
