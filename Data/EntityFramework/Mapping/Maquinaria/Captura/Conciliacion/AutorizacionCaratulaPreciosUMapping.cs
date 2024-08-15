using Core.Entity.Maquinaria.Catalogo.Cararatulas;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.Maquinaria.Captura.Conciliacion
{
    public class AutorizacionCaratulaPreciosUMapping : EntityTypeConfiguration<tblM_AutorizacionCaratulaPreciosU>
    {
        public AutorizacionCaratulaPreciosUMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.cadenaElabora).HasColumnName("cadenaElabora");
            Property(x => x.cadenaVobo1).HasColumnName("cadenaVobo1");
            Property(x => x.cadenaVobo2).HasColumnName("cadenaVobo2");
            Property(x => x.caratulaID).HasColumnName("caratulaID");
            Property(x => x.estadoCaratula).HasColumnName("estadoCaratula");
            Property(x => x.fechaAutoriza).HasColumnName("fechaAutoriza");
            Property(x => x.fechaElaboracion).HasColumnName("fechaElaboracion");
            Property(x => x.fechaVobo1).HasColumnName("fechaVobo1");
            Property(x => x.fechaVobo2).HasColumnName("fechaVobo2");
            Property(x => x.firmaAutoriza).HasColumnName("firmaAutoriza");
            Property(x => x.firmaElabora).HasColumnName("firmaElabora");
            Property(x => x.firmaVobo1).HasColumnName("firmaVobo1");
            Property(x => x.firmaVobo2).HasColumnName("firmaVobo2");
            Property(x => x.obraID).HasColumnName("obraID");
            Property(x => x.usuarioAutoriza).HasColumnName("usuarioAutoriza");
            Property(x => x.usuarioElaboraID).HasColumnName("usuarioElaboraID");
            Property(x => x.usuarioFirma).HasColumnName("usuarioFirma");
            Property(x => x.usuarioVobo1).HasColumnName("usuarioVobo1");
            Property(x => x.usuarioVobo2).HasColumnName("usuarioVobo2");
            Property(x => x.comentario).HasColumnName("comentario");

            ToTable("tblM_AutorizacionCaratulaPreciosU");
        }
    }
}
