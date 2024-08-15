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
    public class tblC_AF_PolizasExluidasParaCapturaAutomaticaMapping : EntityTypeConfiguration<tblC_AF_PolizasExcluidasParaCapturaAutomatica>
    {
        public tblC_AF_PolizasExluidasParaCapturaAutomaticaMapping()
        {
            HasKey(x => x.Id);
            Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("Id");
            Property(x => x.Año).HasColumnName("Año");
            Property(x => x.Mes).HasColumnName("Mes");
            Property(x => x.Poliza).HasColumnName("Poliza");
            Property(x => x.TipoPoliza).HasColumnName("TipoPoliza");
            Property(x => x.Linea).HasColumnName("Linea");
            Property(x => x.Estatus).HasColumnName("Estatus");
            ToTable("tblC_AF_PolizasExcluidasParaCapturaAutomatica");
        }
    }
}