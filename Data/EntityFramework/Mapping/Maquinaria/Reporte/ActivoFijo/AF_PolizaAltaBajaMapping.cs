using Core.Entity.Maquinaria.Reporte.ActivoFijo;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.Maquinaria.Reporte.ActivoFijo
{
    public class AF_PolizaAltaBajaMapping : EntityTypeConfiguration<tblC_AF_PolizaAltaBaja>
    {
        public AF_PolizaAltaBajaMapping()
        {
            HasKey(x => x.Id);
            Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("Id");
            Property(x => x.Año).HasColumnName("Año");
            Property(x => x.Mes).HasColumnName("Mes");
            Property(x => x.Poliza).HasColumnName("Poliza");
            Property(x => x.TP).HasColumnName("TP");
            Property(x => x.Linea).HasColumnName("Linea");
            Property(x => x.TM).HasColumnName("TM");
            Property(x => x.Cuenta).HasColumnName("Cuenta");
            Property(x => x.Subcuenta).HasColumnName("Subcuenta");
            Property(x => x.SubSubcuenta).HasColumnName("SubSubcuenta");
            Property(x => x.Concepto).HasColumnName("Concepto");
            Property(x => x.Monto).HasColumnName("Monto");
            Property(x => x.Factura).HasColumnName("Factura");
            Property(x => x.FechaMovimiento).HasColumnName("FechaMovimiento");
            Property(x => x.TipoActivo).HasColumnName("TipoActivo");
            Property(x => x.Estatus).HasColumnName("Estatus");
            Property(x => x.FechaCreacion).HasColumnName("FechaCreacion");
            Property(x => x.IdUsuarioCreacion).HasColumnName("IdUsuarioCreacion");
            Property(x => x.FechaModificacion).HasColumnName("FechaModificacion");
            Property(x => x.IdUsuarioModificacion).HasColumnName("IdUsuarioModificacion");
            Property(x => x.Excepcion).HasColumnName("Excepcion");
            HasRequired(x => x.Usuario).WithMany().HasForeignKey(d => d.IdUsuarioCreacion);
            HasRequired(x => x.DescripcionTipoActivo).WithMany().HasForeignKey(d => d.TipoActivo);
            HasRequired(x => x.UsuarioModificacion).WithMany().HasForeignKey(d => d.IdUsuarioModificacion);
            ToTable("tblC_AF_PolizaAltaBaja");
        }
    }
}