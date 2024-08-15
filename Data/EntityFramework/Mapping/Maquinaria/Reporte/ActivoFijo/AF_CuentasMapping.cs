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
    public class AF_CuentasMapping : EntityTypeConfiguration<tblC_AF_Cuentas>
    {
        public AF_CuentasMapping()
        {
            HasKey(x => x.Id);
            Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("Id");
            Property(x => x.Cuenta).HasColumnName("Cuenta");
            Property(x => x.Descripcion).HasColumnName("Descripcion");
            Property(x => x.FechaCreacion).HasColumnName("FechaCreacion");
            Property(x => x.MesesDeDepreciacion).HasColumnName("MesesDeDepreciacion");
            Property(x => x.PorcentajeDepreciacion).HasColumnName("PorcentajeDepreciacion");
            Property(x => x.IdUsuarioCreacion).HasColumnName("IdUsuarioCreacion");
            Property(x => x.Estatus).HasColumnName("Estatus");
            Property(x => x.EsMaquinaria).HasColumnName("EsMaquinaria");
            HasRequired(x => x.Usuario).WithMany().HasForeignKey(d => d.IdUsuarioCreacion);
            ToTable("tblC_AF_Cuentas");
        }
    }
}