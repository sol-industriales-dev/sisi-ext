using Core.Entity.Maquinaria.Catalogo;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.Maquinaria.Catalogo
{
    class CcExclusionMapping : EntityTypeConfiguration<tblM_CC_Exclusion>
    {
        public CcExclusionMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.cc).HasColumnName("cc");
            Property(x => x.baseDatos).HasColumnName("baseDatos");
            Property(x => x.estatus).HasColumnName("estatus");
            ToTable("tblM_CC_Exclusion");
        }
    }
}
