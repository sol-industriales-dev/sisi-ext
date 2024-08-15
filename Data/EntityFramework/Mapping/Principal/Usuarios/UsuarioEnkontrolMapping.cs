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
    public class UsuarioEnkontrolMapping : EntityTypeConfiguration<tblP_Usuario_Enkontrol>
    {
        public UsuarioEnkontrolMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.idUsuario).HasColumnName("idUsuario");
            Property(x => x.empleado).HasColumnName("empleado");
            Property(x => x.sn_empleado).HasColumnName("sn_empleado");
            Property(x => x.password).HasColumnName("password");
            ToTable("tblP_Usuario_Enkontrol");
        }
    }
}
