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
    class FiniquitoDetalleMapping : EntityTypeConfiguration<tblRH_FiniquitoDetalle>
    {
        public FiniquitoDetalleMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.conceptoID).HasColumnName("conceptoID");
            Property(x => x.conceptoInfo).HasColumnName("conceptoInfo");
            Property(x => x.operacion1).HasColumnName("operacion1");
            Property(x => x.operacion2).HasColumnName("operacion2");
            Property(x => x.operacion3).HasColumnName("operacion3");
            Property(x => x.operacion4).HasColumnName("operacion4");
            Property(x => x.conceptoDetalle).HasColumnName("conceptoDetalle");
            Property(x => x.resultado).HasColumnName("resultado");
            Property(x => x.estatus).HasColumnName("estatus");
            Property(x => x.finiquitoID).HasColumnName("finiquitoID");
            ToTable("tblRH_FiniquitoDetalle");
        }
    }
}
