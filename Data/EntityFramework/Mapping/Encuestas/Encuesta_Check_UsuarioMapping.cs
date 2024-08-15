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
    class Encuesta_Check_UsuarioMapping : EntityTypeConfiguration<tblEN_Encuesta_Check_Usuario>
    {
        public Encuesta_Check_UsuarioMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.encuestaID).HasColumnName("encuestaID");
            Property(x => x.usuarioID).HasColumnName("usuarioID");
            Property(x => x.crear).HasColumnName("crear");
            Property(x => x.ver).HasColumnName("ver");
            Property(x => x.editar).HasColumnName("editar");
            Property(x => x.enviar).HasColumnName("enviar");
            Property(x => x.contestaTelefonica).HasColumnName("contestaTelefonica");
            Property(x => x.recibeNotificacion).HasColumnName("recibeNotificacion");
            Property(x => x.contestaPapel).HasColumnName("contestaPapel");
            Property(x => x.estatus).HasColumnName("estatus");
            HasRequired(x => x.Encuesta).WithMany().HasForeignKey(y => y.encuestaID);
            ToTable("tblEN_Encuesta_Check_Usuario");
        }
    }
}
