using Core.Entity.Maquinaria._Caratulas;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.Maquinaria._Caratulas
{
    public class tblM_CaratulaConceptosMapping : EntityTypeConfiguration<tblM_CaratulaConceptos>
    {
        public tblM_CaratulaConceptosMapping() 
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.conceptos).HasColumnName("conceptos");
            Property(x => x.tipoMoneda).HasColumnName("tipoMoneda");

            ToTable("tblM_CaratulaConceptos");
        }
    }
}