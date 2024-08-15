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
    class CierreInventarioFisicoMapping : EntityTypeConfiguration<tblAlm_CierreInventarioFisico>
    {
        public CierreInventarioFisicoMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");

            Property(x => x.almacen).HasColumnName("almacen");
            Property(x => x.fecha).HasColumnName("fecha");
            Property(x => x.usuarioCaptura).HasColumnName("usuarioCaptura");
            Property(x => x.fechaCaptura).HasColumnName("fechaCaptura");
            Property(x => x.estatus).HasColumnName("estatus");

            ToTable("tblAlm_CierreInventarioFisico");
        }
    }
}
