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
    public class PolizaMapping : EntityTypeConfiguration<tblPo_Poliza>
    {
        public PolizaMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.poliza).HasColumnName("poliza");
            Property(x => x.tipoPoliza).HasColumnName("tipoPoliza");
            Property(x => x.estatus).HasColumnName("estatus");
            Property(x => x.generada).HasColumnName("generada");
            Property(x => x.fecha).HasColumnName("fecha");
            Property(x => x.estatus).HasColumnName("estatus");
            Property(x => x.concepto).HasColumnName("concepto");
            Property(x => x.cargo).HasColumnName("cargo");
            Property(x => x.abono).HasColumnName("abono");
            Property(x => x.diferencia).HasColumnName("diferencia");
            ToTable("tblPo_Poliza");
        }
    }
}
