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
    public class CalificacionMapping : EntityTypeConfiguration<tblCom_CC_Calificacion>
    {
        public CalificacionMapping()
        {
            HasKey(x => x.Id);
            Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("Id");
            Property(x => x.Numero).HasColumnName("Numero");
            Property(x => x.CC).HasColumnName("CC");
            Property(x => x.Folio).HasColumnName("Folio");
            Property(x => x.TipoRequisicion).HasColumnName("TipoRequisicion");
            Property(x => x.Proveedor).HasColumnName("Proveedor");
            Property(x => x.Calificacion).HasColumnName("Calificacion");
            Property(x => x.Precio).HasColumnName("Precio");
            Property(x => x.TiempoEntrega).HasColumnName("TiempoEntrega");
            Property(x => x.CondicionPago).HasColumnName("CondicionPago");
            Property(x => x.LAB).HasColumnName("LAB");
            Property(x => x.ConfiabilidadProveedor).HasColumnName("ConfiabilidadProveedor");
            Property(x => x.Calidad).HasColumnName("Calidad");
            Property(x => x.ServicioPostVenta).HasColumnName("ServicioPostVenta");
            Property(x => x.idUsuarioRegistro).HasColumnName("idUsuarioRegistro");
            Property(x => x.esActivo).HasColumnName("esActivo");
            Property(x => x.fechaRegistro).HasColumnName("fechaRegistro");
            Property(x => x.PonderacionPrecio).HasColumnName("PonderacionPrecio");
            Property(x => x.PonderacionTiempoEntrega).HasColumnName("PonderacionTiempoEntrega");
            Property(x => x.PonderacionCondicionPago).HasColumnName("PonderacionCondicionPago");
            Property(x => x.PonderacionLAB).HasColumnName("PonderacionLAB");
            Property(x => x.PonderacionConfiabilidadProveedor).HasColumnName("PonderacionConfiabilidadProveedor");
            Property(x => x.PonderacionCalidad).HasColumnName("PonderacionCalidad");
            Property(x => x.PonderacionServicioPostVenta).HasColumnName("PonderacionServicioPostVenta");
            ToTable("tblCom_CC_Calificacion");
        }
    }
}
