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
    public class FacturaParcialMapping : EntityTypeConfiguration<tblC_FacturaParcial>
    {
        public FacturaParcialMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.idCadena).HasColumnName("idCadena");
            Property(x => x.numProv).HasColumnName("numProv");
            Property(x => x.factura).HasColumnName("factura");
            Property(x => x.total).HasColumnName("total");
            Property(x => x.abonado).HasColumnName("abonado");
            Property(x => x.ultimoAbono).HasColumnName("ultimoAbono");
            ToTable("tblC_FacturaParcial");
        }
    }
}
