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
    public class tblF_CapPrefacturaMapping : EntityTypeConfiguration<tblF_CapPrefactura>
    {
        public tblF_CapPrefacturaMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.idRepPrefactura).HasColumnName("idRepPrefactura");
            Property(x => x.Renglon).HasColumnName("Renglon");
            Property(x => x.Tipo).HasColumnName("Tipo");
            Property(x => x.Cantidad).HasColumnName("Cantidad").HasPrecision(20, 4); ;
            Property(x => x.Unidad).HasColumnName("Unidad");
            Property(x => x.Precio).HasColumnName("Precio").HasPrecision(20, 4); ;
            Property(x => x.Importe).HasColumnName("Importe").HasPrecision(20, 4); ;
            Property(x => x.Concepto).HasColumnName("Concepto");
            
            ToTable("tblF_CapPrefactura");
        }
    }
}
