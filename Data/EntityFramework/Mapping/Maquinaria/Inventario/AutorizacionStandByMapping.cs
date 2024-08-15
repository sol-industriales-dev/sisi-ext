using Core.Entity.Maquinaria.Inventario;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.Maquinaria.Inventario
{
    public class AutorizacionStandByMapping : EntityTypeConfiguration<tblM_AutorizacionStandBy>
    {
        public AutorizacionStandByMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.usuarioAutoriza).HasColumnName("usuarioAutoriza");
            Property(x => x.usuarioSolicita).HasColumnName("usuarioSolicita");
            Property(x => x.estatus).HasColumnName("estatus");
            Property(x => x.fechaSolicitud).HasColumnName("fechaSolicitud");
            Property(x => x.comentario).HasColumnName("comentario");
            Property(x => x.autorizacion).HasColumnName("autorizacion");
            Property(x => x.tipoStandBy).HasColumnName("tipoStandBy");
            Property(x => x.idEconomico).HasColumnName("idEconomico");

            Property(x => x.comentarioSolicitud).HasColumnName("comentarioSolicitud");
            Property(x => x.idAsignacion).HasColumnName("idAsignacion");
            Property(x => x.horasParo).HasColumnName("horasParo");
            Property(x => x.fechaAutorizacion).HasColumnName("fechaAutorizacion");
            Property(x => x.CC).HasColumnName("CC");
            ToTable("tblM_AutorizacionStandBy");
        }
    }
}
