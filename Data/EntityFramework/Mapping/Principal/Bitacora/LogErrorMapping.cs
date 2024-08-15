using Core.Entity.Principal.Bitacoras;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.Principal.Bitacora
{
    public class LogErrorMapping : EntityTypeConfiguration<tblP_LogError>
    {
        public LogErrorMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.empresa).HasColumnName("empresa");
            Property(x => x.sistema).HasColumnName("sistema");
            Property(x => x.modulo).HasColumnName("modulo");
            Property(x => x.controlador).HasColumnName("controlador");
            Property(x => x.accion).HasColumnName("accion");
            Property(x => x.mensaje).HasColumnName("mensaje");
            Property(x => x.tipo).HasColumnName("tipo");
            Property(x => x.usuarioID).HasColumnName("usuarioID");
            Property(x => x.fecha).HasColumnName("fecha");
            Property(x => x.registroID).HasColumnName("registroID");
            Property(x => x.objeto).HasColumnName("objeto");
            Property(x => x.publicIP).HasColumnName("publicIP");
            Property(x => x.localIP).HasColumnName("localIP");
            ToTable("tblP_LogError");
        }
    }
}
