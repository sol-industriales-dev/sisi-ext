using Core.Entity.Administrativo.Contabilidad;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.Administrativo.Contabilidad
{
    public class ConceptoConsolidadosMapping : EntityTypeConfiguration<tblEF_ConceptoConsolidados>
    {
        public ConceptoConsolidadosMapping()
        {
            HasKey(x => x.id);
            HasRequired(x => x.grupo).WithMany().HasForeignKey(x => x.grupoId);
            ToTable("tblEF_ConceptoConsolidados");
        }
    }
}
