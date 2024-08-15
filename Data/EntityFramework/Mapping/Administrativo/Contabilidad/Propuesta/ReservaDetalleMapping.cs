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
    public class ReservaDetalleMapping : EntityTypeConfiguration<tblC_ReservaDetalle>
    {
        public ReservaDetalleMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.idReserva).HasColumnName("idReserva");
            Property(x => x.tipo).HasColumnName("tipo");
            Property(x => x.fecha).HasColumnName("fecha");
            Property(x => x.cc).HasColumnName("cc");
            Property(x => x.numero).HasColumnName("numero");
            Property(x => x.cargo).HasColumnName("cargo");
            Property(x => x.abono).HasColumnName("abono");
            Property(x => x.poliza).HasColumnName("poliza");
            Property(x => x.tp).HasColumnName("tp");
            Property(x => x.esActivo).HasColumnName("esActivo");
            Property(x => x.ultimoCambio).HasColumnName("ultimoCambio");
            ToTable("tblC_ReservaDetalle");
        }
    }
}
