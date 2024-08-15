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
    public class tblCOES_PlantillatblCOES_ElementotblCOES_RequerimientoMapping : EntityTypeConfiguration<tblCOES_PlantillatblCOES_ElementotblCOES_Requerimiento>
    {
        public tblCOES_PlantillatblCOES_ElementotblCOES_RequerimientoMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");

            ToTable("tblCOES_PlantillatblCOES_ElementotblCOES_Requerimiento");
        }
    }
}
