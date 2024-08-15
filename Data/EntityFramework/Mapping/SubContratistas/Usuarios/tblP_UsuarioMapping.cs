using Core.Entity.SubContratistas.Usuarios;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.SubContratistas.Usuarios
{
    public class tblP_UsuarioMapping : EntityTypeConfiguration<tblP_Usuarios>
    {
        public tblP_UsuarioMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x._user).HasColumnName("_user");
            Property(x => x._pass).HasColumnName("_pass");
            Property(x => x.nombre_completo).HasColumnName("nombre_completo");
            Property(x => x.tipo).HasColumnName("tipo");
            Property(x => x.estatus).HasColumnName("estatus");
            Property(x => x.correo).HasColumnName("correo");

            ToTable("tblP_Usuario");
        }
    }
}
