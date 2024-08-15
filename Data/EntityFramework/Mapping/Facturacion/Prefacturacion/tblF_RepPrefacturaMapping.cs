using Core.Entity.Facturacion.Prefacturacion;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.Facturacion.Prefacturacion
{
    public class tblF_RepPrefacturaMapping : EntityTypeConfiguration<tblF_RepPrefactura>
    {
        public tblF_RepPrefacturaMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.Folio).HasColumnName("Folio");
            Property(x => x.Estado).HasColumnName("Estado");
            Property(x => x.CC).HasColumnName("CC");
            Property(x => x.Fecha).HasColumnName("Fecha");
            Property(x => x.MetodoPago).HasColumnName("MetodoPago");
            Property(x => x.Usocfdi).HasColumnName("Usocfdi");
            Property(x => x.Nombre).HasColumnName("Nombre");
            Property(x => x.Direccion).HasColumnName("Direccion");
            Property(x => x.Ciudad).HasColumnName("Ciudad");
            Property(x => x.CP).HasColumnName("CP");
            Property(x => x.RFC).HasColumnName("RFC");
            Property(x => x.PosicionImporte).HasColumnName("PosicionImporte");
            Property(x => x.VerCC).HasColumnName("VerCC");
            Property(x => x.VerMetodoPago).HasColumnName("VerMetodoPago");
            Property(x => x.VerTipoMoneda).HasColumnName("VerTipoMoneda");
            Property(x => x.CalAuto).HasColumnName("CalAuto");
            ToTable("tblF_RepPrefactura");
        }
    }
}
