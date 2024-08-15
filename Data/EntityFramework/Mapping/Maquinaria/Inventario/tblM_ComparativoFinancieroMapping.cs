using Core.Entity.Maquinaria.Inventario;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.Maquinaria.Inventario
{
    class tblM_ComparativoFinancieroMapping : EntityTypeConfiguration<tblM_ComparativoFinanciero>
    {
        public tblM_ComparativoFinancieroMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.idAsignacion).HasColumnName("idAsignacion");
            Property(x => x.financiera).HasColumnName("financiera");
            Property(x => x.esActivo).HasColumnName("esActivo");


            ToTable("tblM_ComparativoFinanciero");
        }
    }
}
