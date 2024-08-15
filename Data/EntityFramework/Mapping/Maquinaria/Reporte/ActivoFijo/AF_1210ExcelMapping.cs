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
    public class AF_1210ExcelMapping : EntityTypeConfiguration<tblC_AF_1210Excel>
    {
        public AF_1210ExcelMapping()
        {
            HasKey(x => x.Id);
            Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("Id");
            Property(x => x.Cuenta).HasColumnName("Cuenta");
            Property(x => x.Subcuenta).HasColumnName("Subcuenta");
            Property(x => x.SubSubcuenta).HasColumnName("SubSubcuenta");
            Property(x => x.FechaAlta).HasColumnName("FechaAlta");
            Property(x => x.FechaInicioDep).HasColumnName("FechaInicioDep");
            Property(x => x.Factura).HasColumnName("Factura");
            Property(x => x.CC).HasColumnName("CC");
            Property(x => x.Clave).HasColumnName("Clave");
            Property(x => x.Descripcion).HasColumnName("Descripcion");
            Property(x => x.TipoActivo).HasColumnName("TipoActivo");
            Property(x => x.Tipo).HasColumnName("Tipo");
            Property(x => x.MOI).HasColumnName("MOI");
            Property(x => x.AltasEquipo).HasColumnName("AltasEquipo");
            Property(x => x.Componentes).HasColumnName("Componentes");
            Property(x => x.FechaBaja).HasColumnName("FechaBaja");
            Property(x => x.PolizaBaja).HasColumnName("PolizaBaja");
            Property(x => x.MontoBaja).HasColumnName("MontoBaja");
            Property(x => x.PorcentajeDep).HasColumnName("PorcentajeDep");
            Property(x => x.MesesTotalesDep).HasColumnName("MesesTotalesDep");
            ToTable("tblC_AF_1210Excel");
        }
    }

    public class AF_1210ExcelFaltantesMapping : EntityTypeConfiguration<tblC_AF_1210ExcelFaltantes>
    {
        public AF_1210ExcelFaltantesMapping()
        {
            HasKey(x => x.Id);
            Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("Id");
            Property(x => x.Cuenta).HasColumnName("Cuenta");
            Property(x => x.NumeroRenglonExcel).HasColumnName("NumeroRenglonExcel");
            Property(x => x.NumeroEconomico).HasColumnName("NumeroEconomico");
            Property(x => x.Motivo).HasColumnName("Motivo");
            ToTable("tblC_AF_1210ExcelFaltantes");
        }
    }
}