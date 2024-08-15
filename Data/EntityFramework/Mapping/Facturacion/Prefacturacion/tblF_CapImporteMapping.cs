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
    public class tblF_CapImporteMapping : EntityTypeConfiguration<tblF_CapImporte>
    {
        public tblF_CapImporteMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.idReporte).HasColumnName("idReporte");
            Property(x => x.Label).HasColumnName("Label");
            Property(x => x.Valor).HasColumnName("Valor");
            Property(x => x.Renglon).HasColumnName("Renglon");
            ToTable("tblF_CapImporte");
        }
    }
}
