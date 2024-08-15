using Core.Entity.Administrativo.cotizaciones;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.Administrativo.Cotizaciones
{
    public class CotizacionMapping : EntityTypeConfiguration<tblAD_Cotizaciones>
    {
        public CotizacionMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.folio).HasColumnName("folio");
            Property(x => x.cc).HasColumnName("cc");
            Property(x => x.cliente).HasColumnName("cliente");
            Property(x => x.proyecto).HasColumnName("proyecto");
            Property(x => x.monto).HasColumnName("monto");
            Property(x => x.estatus).HasColumnName("estatus");
            Property(x => x.fechaCaptura).HasColumnName("fechaCaptura");
            Property(x => x.activo).HasColumnName("activo");
            Property(x => x.comentariosCount).HasColumnName("comentariosCount");
            Property(x => x.revision).HasColumnName("revision");
            Property(x => x.fechaStatus).HasColumnName("fechaStatus");
            Property(x => x.year).HasColumnName("year");
            Property(x => x.Margen).HasColumnName("Margen");
            Property(x => x.tipoMoneda).HasColumnName("tipoMoneda");
            Property(x => x.fechaProbableF).HasColumnName("fechaProbableF");
            Property(x => x.contacto).HasColumnName("contacto");

            ToTable("tblAD_Cotizaciones");
        }
    }
}
