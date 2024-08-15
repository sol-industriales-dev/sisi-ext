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

    public class tblRH_BN_Evento_DetMapping : EntityTypeConfiguration<tblRH_BN_Evento_Det>
    {
        public tblRH_BN_Evento_DetMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.eventoID).HasColumnName("eventoID");
            Property(x => x.cveEmp).HasColumnName("cveEmp");
            Property(x => x.nombreEmp).HasColumnName("nombreEmp");
            Property(x => x.puestoEmp).HasColumnName("puestoEmp");
            Property(x => x.tipoNomina).HasColumnName("tipoNomina");
            Property(x => x.monto).HasColumnName("monto");
            Property(x => x.estatus).HasColumnName("estatus");
            Property(x => x.fechaAplicacion).HasColumnName("fechaAplicacion");
            Property(x => x.periodoNomina).HasColumnName("periodoNomina");

            HasRequired(x => x.evento).WithMany().HasForeignKey(y => y.eventoID);

            ToTable("tblRH_BN_Evento_Det");
        }
    }
}
