using Core.Entity.Enkontrol.Compras.OrdenCompra;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.Enkontrol.Compras.OrdenCompra
{
    class Retenciones_InternaMapping : EntityTypeConfiguration<tblCom_Retenciones_Interna>
    {
        public Retenciones_InternaMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.retencion).HasColumnName("retencion");
            ToTable("tblCom_Retenciones_Interna");
        }
    }
}
