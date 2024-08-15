using Core.Entity.Facturacion.Estimacion;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.Facturacion.Estimacion
{
    public class EstimacionResumenMapping : EntityTypeConfiguration<tblF_EstimacionResumen>
    {
        public EstimacionResumenMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.numcte).HasColumnName("numcte");
            Property(x => x.cc).HasColumnName("cc");
            Property(x => x.factura).HasColumnName("factura");
            Property(x => x.fecha).HasColumnName("fecha");
            Property(x => x.fechavenc).HasColumnName("fechavenc");
            Property(x => x.linea).HasColumnName("linea");
            Property(x => x.estimacion).HasColumnName("estimacion").HasPrecision(22,4);
            Property(x => x.anticipo).HasColumnName("anticipo").HasPrecision(22, 4);
            Property(x => x.vencido).HasColumnName("vencido").HasPrecision(22, 4);
            Property(x => x.pronostico).HasColumnName("pronostico").HasPrecision(22, 4);
            Property(x => x.cobrado).HasColumnName("cobrado").HasPrecision(22, 4);
            Property(x => x.fechaResumen).HasColumnName("fechaResumen");
            Property(x => x.esActivo).HasColumnName("esActivo");
            Property(x => x.fechaCaptura).HasColumnName("fechaCaptura");
            Property(x => x.Enkontrol).HasColumnName("Enkontrol");
            ToTable("tblF_EstimacionResumen");
        }
    }
}
