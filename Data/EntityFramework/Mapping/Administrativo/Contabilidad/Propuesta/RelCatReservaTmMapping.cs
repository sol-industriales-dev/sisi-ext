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
    public class RelCatReservaTmMapping : EntityTypeConfiguration<tblC_RelCatReservaTm>
    {
        public RelCatReservaTmMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.idCatReserva).HasColumnName("idCatReserva");
            Property(x => x.idTipoProrrateo).HasColumnName("idTipoProrrateo");
            Property(x => x.generado).HasColumnName("generado");
            Property(x => x.tm).HasColumnName("tm");
            Property(x => x.esActivo).HasColumnName("esActivo");
            Property(x => x.fechaRegistro).HasColumnName("fechaRegistro");
            ToTable("tblC_RelCatReservaTm");
        }
    }
}
