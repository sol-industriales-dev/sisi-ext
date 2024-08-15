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
    public class CatConfiabilidadMapping : EntityTypeConfiguration<tblCom_CC_CatConfiabilidad>
    {
        public CatConfiabilidadMapping()
        {
            HasKey(x => x.Id);
            Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("Id");
            Property(x => x.TipoRequisicion).HasColumnName("TipoRequisicion");
            Property(x => x.Precio).HasColumnName("Precio");
            Property(x => x.TiempoEntrega).HasColumnName("TiempoEntrega");
            Property(x => x.CondicionPago).HasColumnName("CondicionPago");
            Property(x => x.LAB).HasColumnName("LAB");
            Property(x => x.ConfiabilidadProveedor).HasColumnName("ConfiabilidadProveedor");
            Property(x => x.Calidad).HasColumnName("Calidad");
            Property(x => x.ServicioPostVenta).HasColumnName("ServicioPostVenta");
            ToTable("tblCom_CC_CatConfiabilidad");
        }
    }
}
