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
    class tblCO_UnidadesMapping : EntityTypeConfiguration<tblCO_Unidades>
    {
        public tblCO_UnidadesMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.unidad).HasColumnName("unidad");
            Property(x => x.estatus).HasColumnName("estatus");       
            ToTable("tblCO_Unidades");
        }
    }
}
