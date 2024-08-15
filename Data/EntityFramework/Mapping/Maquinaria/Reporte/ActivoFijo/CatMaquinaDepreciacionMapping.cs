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
    public class CatMaquinaDepreciacionMapping : EntityTypeConfiguration<tblM_CatMaquinaDepreciacion>
    {
        CatMaquinaDepreciacionMapping()
        {
            HasKey(x => x.Id);
            Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("Id");
            Property(x => x.IdCatMaquina).HasColumnName("IdCatMaquina");
            Property(x => x.IdPoliza).HasColumnName("IdPoliza");
            Property(x => x.FechaInicioDepreciacion).HasColumnName("FechaInicioDepreciacion");
            Property(x => x.PorcentajeDepreciacion).HasColumnName("PorcentajeDepreciacion");
            Property(x => x.MesesTotalesDepreciacion).HasColumnName("MesesTotalesDepreciacion");
            Property(x => x.TipoDelMovimiento).HasColumnName("TipoDelMovimiento");
            Property(x => x.IdPolizaReferenciaAlta).HasColumnName("IdPolizaReferenciaAlta");
            Property(x => x.CapturaAutomatica).HasColumnName("CapturaAutomatica");
            Property(x => x.IdBajaCosto).HasColumnName("IdBajaCosto");
            Property(x => x.Estatus).HasColumnName("Estatus");
            Property(x => x.FechaCreacion).HasColumnName("FechaCreacion");
            Property(x => x.IdUsuarioCreacion).HasColumnName("IdUsuarioCreacion");
            Property(x => x.FechaModificacion).HasColumnName("FechaModificacion");
            Property(x => x.IdUsuarioModificacion).HasColumnName("IdUsuarioModificacion");
            //HasRequired(x => x.Maquina).WithMany().HasForeignKey(d => d.IdCatMaquina);
            HasOptional(x => x.Costo).WithMany().HasForeignKey(d => d.IdBajaCosto);
            HasRequired(x => x.Poliza).WithMany().HasForeignKey(d => d.IdPoliza);
            HasRequired(x => x.PolizaAlta).WithMany().HasForeignKey(d => d.IdPolizaReferenciaAlta);
            HasRequired(x => x.Usuario).WithMany().HasForeignKey(d => d.IdUsuarioCreacion);
            HasRequired(x => x.Movimiento).WithMany().HasForeignKey(d => d.TipoDelMovimiento);
            HasRequired(x => x.UsuarioModificacion).WithMany().HasForeignKey(d => d.IdUsuarioModificacion);
            ToTable("tblM_CatMaquinaDepreciacion");
        }
    }
}
