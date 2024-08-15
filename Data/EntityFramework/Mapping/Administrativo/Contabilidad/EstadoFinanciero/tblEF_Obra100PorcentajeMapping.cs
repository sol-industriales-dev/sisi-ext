using Core.Entity.Administrativo.Contabilidad.EstadoFinanciero;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.Administrativo.Contabilidad.EstadoFinanciero
{
    public class tblEF_Obra100PorcentajeMapping : EntityTypeConfiguration<tblEF_Obra100Porcentaje>
    {
        public tblEF_Obra100PorcentajeMapping()
        {
            HasKey(x => x.id);
            ToTable("tblEF_Obra100Porcentaje");
        }
    }
}
