using Core.Entity.RecursosHumanos.Captura;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.RecursosHumanos.Captura
{
    public class tblRH_BN_Evento_AutMapping : EntityTypeConfiguration<tblRH_BN_Evento_Aut>
    {
        public tblRH_BN_Evento_AutMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.eventoID).HasColumnName("eventoID");
            Property(x => x.aprobadorClave).HasColumnName("aprobadorClave");
            Property(x => x.aprobadorNombre).HasColumnName("aprobadorNombre");
            Property(x => x.aprobadorPuesto).HasColumnName("aprobadorPuesto");
            Property(x => x.tipo).HasColumnName("tipo");
            Property(x => x.estatus).HasColumnName("estatus");
            Property(x => x.firma).HasColumnName("firma");
            Property(x => x.autorizando).HasColumnName("autorizando");
            Property(x => x.orden).HasColumnName("orden");
            Property(x => x.comentario).HasColumnName("comentario");
            Property(x => x.fecha).HasColumnName("fecha");
            HasRequired(x => x.aprobador).WithMany().HasForeignKey(y => y.aprobadorClave);
            HasRequired(x => x.evento).WithMany().HasForeignKey(y => y.eventoID);

            ToTable("tblRH_BN_Evento_Aut");
        }
    }
}
