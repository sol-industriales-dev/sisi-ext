using Core.Entity.Administrativo.Contabilidad;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.Administrativo.Contabilidad
{
    public class CadenaPrincipalMapping : EntityTypeConfiguration<tblC_CadenaPrincipal>
    {
        public CadenaPrincipalMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.centro_costos).HasColumnName("centro_costos");
            Property(x => x.nombCC).HasColumnName("nombCC");
            Property(x => x.numProveedor).HasColumnName("numProveedor");
            Property(x => x.proveedor).HasColumnName("proveedor");
            Property(x => x.total).HasColumnName("total");
            Property(x => x.estatus).HasColumnName("estatus");
            Property(x => x.numNafin).HasColumnName("numNafin");
            Property(x => x.factoraje).HasColumnName("factoraje");
            Property(x => x.banco).HasColumnName("banco");
            Property(x => x.fecha).HasColumnName("fecha");
            Property(x => x.fechaVencimiento).HasColumnName("fechaVencimiento");
            Property(x => x.pagado).HasColumnName("pagado");
            Property(x => x.estadoAutorizacion).HasColumnName("estadoAutorizacion");
            Property(x => x.comentarioRechazo).HasColumnName("comentarioRechazo");
            Property(x => x.firma).HasColumnName("firma");
            Property(x => x.firmaVoBo).HasColumnName("firmaVobo");
            Property(x => x.firmaValidado).HasColumnName("firmaValidado");
            ToTable("tblC_CadenaPrincipal");
        }
    }
}
