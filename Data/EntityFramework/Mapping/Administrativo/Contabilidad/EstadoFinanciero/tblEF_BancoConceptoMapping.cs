using Core.Entity.Administrativo.Contabilidad.EstadoFinanciero;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.Administrativo.Contabilidad.EstadoFinanciero
{
    public class tblEF_BancoConceptoMapping : EntityTypeConfiguration<tblEF_BancoConcepto>
    {
        public tblEF_BancoConceptoMapping()
        {
            HasKey(x => x.id);
            ToTable("tblEF_BancoConcepto");
        }
    }
}
