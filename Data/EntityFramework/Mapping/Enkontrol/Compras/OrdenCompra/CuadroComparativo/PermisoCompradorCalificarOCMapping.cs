using Core.Entity.Enkontrol.Compras.OrdenCompra.CuadroComparativo;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.Enkontrol.Compras.OrdenCompra.CuadroComparativo
{
    public class PermisoCompradorCalificarOCMapping : EntityTypeConfiguration<tblCom_CC_PermisoCompradorCalificarOC>
    {
        public PermisoCompradorCalificarOCMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            HasRequired(x => x.comprador).WithMany().HasForeignKey(x => x.usuarioId);
            ToTable("tblCom_CC_PermisoCompradorCalificarOC");
        }
    }
}
