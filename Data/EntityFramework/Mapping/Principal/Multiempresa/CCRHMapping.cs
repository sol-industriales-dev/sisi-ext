using Core.Entity.Principal.Multiempresa;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.Principal.Multiempresa
{
    public class CCRHMapping : EntityTypeConfiguration<tblP_CCRH>
    {
        public CCRHMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.cc).HasColumnName("cc");
            Property(x => x.area).HasColumnName("area");
            Property(x => x.cuenta).HasColumnName("cuenta");
            Property(x => x.areaCuenta).HasColumnName("areaCuenta");
            Property(x => x.descripcion).HasColumnName("descripcion");
            Property(x => x.estatus).HasColumnName("estatus");
            ToTable("tblP_CCRH");
        }

    }
}
