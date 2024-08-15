using Core.Entity.Administrativo.Contabilidad;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.Administrativo.Contabilidad
{
    public class GrupoConsolidadoMapping : EntityTypeConfiguration<tblEF_GrupoConsolidado>
    {
        public GrupoConsolidadoMapping()
        {
            HasKey(x => x.id);
            ToTable("tblEF_GrupoConsolidado");
        }
    }
}
