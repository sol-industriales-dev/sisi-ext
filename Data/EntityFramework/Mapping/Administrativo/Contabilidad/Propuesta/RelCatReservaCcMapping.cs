using Core.Entity.Administrativo.Contabilidad.Propuesta.Acomulado;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.Administrativo.Contabilidad.Propuesta
{
    public class RelCatReservaCcMapping : EntityTypeConfiguration<tblC_RelCatReservaCc>
    {
        public RelCatReservaCcMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.idCatReserva).HasColumnName("idCatReserva");
            Property(x => x.idTipoProrrateo).HasColumnName("idTipoProrrateo");
            Property(x => x.esActivo).HasColumnName("esActivo");
            Property(x => x.fechaRegistro).HasColumnName("fechaRegistro");
            Property(x => x.cc).HasColumnName("cc");
            ToTable("tblC_RelCatReservaCc");
        }
    }
}
