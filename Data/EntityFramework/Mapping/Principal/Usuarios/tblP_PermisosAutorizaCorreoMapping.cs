using Core.Entity.Principal.Usuarios;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.Principal.Usuarios
{
    public class tblP_PermisosAutorizaCorreoMapping : EntityTypeConfiguration<tblP_PermisosAutorizaCorreo>
    {
        public tblP_PermisosAutorizaCorreoMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");

            Property(x => x.usuarioID).HasColumnName("usuarioID");
            Property(x => x.permiso).HasColumnName("permiso");
            Property(x => x.estatus).HasColumnName("estatus");

            ToTable("tblP_PermisosAutorizaCorreo");
        }
    }
}
