using Core.Entity.Maquinaria.Reporte.ActivoFijo.PolizaDepreciacion;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.Maquinaria.Reporte.ActivoFijo.PolizaDepreciacion
{
    public class tblC_AF_PolizaMapping : EntityTypeConfiguration<tblC_AF_Poliza>
    {
        public tblC_AF_PolizaMapping()
        {
            HasKey(x => x.Id);
            Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.Año).HasColumnName("año");
            Property(x => x.Mes).HasColumnName("mes");
            Property(x => x.Semana).HasColumnName("semana");
            Property(x => x.Poliza).HasColumnName("poliza");
            Property(x => x.TipoPoliza).HasColumnName("tipoPoliza");
            Property(x => x.FechaPoliza).HasColumnName("fechaPoliza");
            Property(x => x.Cargos).HasPrecision(24, 6).HasColumnName("cargos");
            Property(x => x.Abonos).HasPrecision(24, 6).HasColumnName("abonos");
            Property(x => x.ModuloEnkontrolId).HasColumnName("moduloEnkontrolId");
            Property(x => x.EstatusPolizaId).HasColumnName("estatusPolizaId");
            Property(x => x.Estatus).HasColumnName("estatus");
            Property(x => x.UsuarioCreacionId).HasColumnName("usuarioCreacionId");
            Property(x => x.FechaCreacion).HasColumnName("fechaCreacion");
            Property(x => x.UsuarioModificacionId).HasColumnName("usuarioModificacionId");
            Property(x => x.FechaModificacion).HasColumnName("fechaModificacion");
            HasRequired(x => x.Modulo).WithMany().HasForeignKey(y => y.ModuloEnkontrolId);
            HasRequired(x => x.EstatusPoliza).WithMany().HasForeignKey(y => y.EstatusPolizaId);
            HasRequired(x => x.UsuarioCreacion).WithMany().HasForeignKey(y => y.UsuarioCreacionId);
            HasRequired(x => x.UsuarioModificacion).WithMany().HasForeignKey(y => y.UsuarioModificacionId);
            ToTable("tblC_AF_Poliza");
        }
    }
}