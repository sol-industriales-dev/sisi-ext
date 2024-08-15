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

    public class tblRH_BN_EventoMapping : EntityTypeConfiguration<tblRH_BN_Evento>
    {
        public tblRH_BN_EventoMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.cc).HasColumnName("cc");
            Property(x => x.tipoNomina).HasColumnName("tipoNomina");
            Property(x => x.justificacion).HasColumnName("justificacion");
            Property(x => x.fechaRegistro).HasColumnName("fechaRegistro");
            Property(x => x.fechaAplicacion).HasColumnName("fechaAplicacion");
            Property(x => x.usuarioID).HasColumnName("usuarioID");
            Property(x => x.estatus).HasColumnName("estatus");
            Property(x => x.periodo).HasColumnName("periodo");
            Property(x => x.aplicado).HasColumnName("aplicado");
            Property(x => x.fechaInicio).HasColumnName("fechaInicio");
            Property(x => x.fechaFin).HasColumnName("estatus");
            
            HasRequired(x => x.usuario).WithMany().HasForeignKey(y => y.usuarioID);

            ToTable("tblRH_BN_Evento");
        }
    }
}
