using Core.Entity.ControlObra;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Data.EntityFramework.Mapping.ControlObra
{
    class tblCO_DivisionesMapping : EntityTypeConfiguration<tblCO_Divisiones>
    {
        public tblCO_DivisionesMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.division).HasColumnName("division");
            Property(x => x.estatus).HasColumnName("estatus");

            ToTable("tblCO_Divisiones");
        }
    }
}
