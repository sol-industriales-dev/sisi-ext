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
    public class AF_RelSubCuentasMapping : EntityTypeConfiguration<tblC_AF_RelSubCuentas>
    {
        AF_RelSubCuentasMapping()
        {
            HasKey(x => x.Id);
            Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("Id");
            Property(x => x.IdCuenta).HasColumnName("IdCuenta");
            Property(x => x.Año).HasColumnName("Año");
            Property(x => x.Subcuenta).HasColumnName("Subcuenta");
            Property(x => x.SubSubcuenta).HasColumnName("SubSubcuenta");
            Property(x => x.EsOverhaul).HasColumnName("EsOverhaul");
            Property(x => x.PorcentajeDepreciacion).HasPrecision(10, 5).HasColumnName("PorcentajeDepreciacion");
            Property(x => x.MesesMaximoDepreciacion).HasColumnName("MesesMaximoDepreciacion");
            Property(x => x.Excluir).HasColumnName("Excluir");
            Property(x => x.Estatus).HasColumnName("Estatus");
            Property(x => x.FechaCreacion).HasColumnName("FechaCreacion");
            Property(x => x.IdUsuarioCreacion).HasColumnName("IdUsuarioCreacion");
            HasRequired(x => x.Cuenta).WithMany().HasForeignKey(d => d.IdCuenta);
            HasRequired(x => x.Usuario).WithMany().HasForeignKey(d => d.IdUsuarioCreacion);
            ToTable("tblC_AF_RelSubCuentas");
        }
    }
}