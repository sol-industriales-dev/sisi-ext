using Core.Entity.Administrativo.Seguridad.Capacitacion.CincoS;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.Administrativo.Seguridad.Capacitacion.CincoS
{
    public class tbl5s_UsuarioMapping : EntityTypeConfiguration<tbl5s_Usuario>
    {
        public tbl5s_UsuarioMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            HasRequired(x => x.usuario).WithMany().HasForeignKey(x => x.usuarioId);
            HasOptional(x => x.privilegio).WithMany().HasForeignKey(x => x.privilegioId);
            HasOptional(x => x.areaOperativa).WithMany().HasForeignKey(x => x.areaOperativaId);
            ToTable("tbl5s_Usuario");
        }
    }
}
