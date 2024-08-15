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
    class tblCO_ADP_FacultamientosMapping : EntityTypeConfiguration<tblCO_ADP_Facultamientos>
    {
        public tblCO_ADP_FacultamientosMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");          

            ToTable("tblCO_ADP_Facultamientos");
        }
    }
}
