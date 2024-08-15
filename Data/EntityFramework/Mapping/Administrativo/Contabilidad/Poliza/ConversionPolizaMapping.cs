using Core.Entity.Administrativo.Contabilidad.Poliza;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.Administrativo.Contabilidad.Poliza
{
    public class ConversionPolizaMapping : EntityTypeConfiguration<tblC_SC_ConversionPoliza>
    {
        public ConversionPolizaMapping()
        {
            HasKey(x => x.Id);
            Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("Id");
            Property(x => x.PalEmpresa).HasColumnName("PalEmpresa");
            Property(x => x.PalYear).HasColumnName("PalYear");
            Property(x => x.PalMes).HasColumnName("PalMes");
            Property(x => x.PalTP).HasColumnName("PalTP");
            Property(x => x.PalPoliza).HasColumnName("PalPoliza");
            Property(x => x.PalCargo).HasColumnName("PalCargo").HasPrecision(22,4);
            Property(x => x.PalAbono).HasColumnName("PalAbono").HasPrecision(22, 4);
            Property(x => x.SecEmpresa).HasColumnName("SecEmpresa");
            Property(x => x.SecYear).HasColumnName("SecYear");
            Property(x => x.SecMes).HasColumnName("SecMes");
            Property(x => x.SecTP).HasColumnName("SecTP");
            Property(x => x.SecPoliza).HasColumnName("SecPoliza");
            Property(x => x.SecCargo).HasColumnName("SecCargo").HasPrecision(22, 4);
            Property(x => x.SecAbono).HasColumnName("SecAbono").HasPrecision(22, 4);
            Property(x => x.idUsuarioRegistro).HasColumnName("idUsuarioRegistro");
            Property(x => x.esActivo).HasColumnName("esActivo");
            Property(x => x.fechaRegistro).HasColumnName("fechaRegistro");
            ToTable("tblC_SC_ConversionPoliza");
        }
    }
}
