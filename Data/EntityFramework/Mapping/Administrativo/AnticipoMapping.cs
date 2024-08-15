using Core.Entity.Administrativo.Contabilidad;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.Administrativo
{
    public class AnticipoMapping : EntityTypeConfiguration<tblC_Anticipo>
    {
        public AnticipoMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.proveedor).HasColumnName("proveedor");
            Property(x => x.numProveedor).HasColumnName("numProveedor");
            Property(x => x.numNafin).HasColumnName("numNafin");
            Property(x => x.anticipo).HasColumnName("anticipo");
            Property(x => x.IVA).HasColumnName("IVA");
            Property(x => x.tipoCambio).HasColumnName("tipoCambio");
            Property(x => x.monto).HasColumnName("monto");
            Property(x => x.concepto).HasColumnName("concepto");
            Property(x => x.factoraje).HasColumnName("factoraje");
            Property(x => x.cif).HasColumnName("cif");
            Property(x => x.banco).HasColumnName("banco");
            Property(x => x.fecha).HasColumnName("fecha");
            Property(x => x.fechaVencimiento).HasColumnName("fechaVencimiento");
            Property(x => x.estatus).HasColumnName("estatus");
            Property(x => x.tipoMoneda).HasColumnName("tipoMoneda");
            Property(x => x.centro_costos).HasColumnName("centro_costos");
            Property(x => x.nombCC).HasColumnName("nombCC");
            ToTable("tblC_Anticipo");
        }
    }
}
