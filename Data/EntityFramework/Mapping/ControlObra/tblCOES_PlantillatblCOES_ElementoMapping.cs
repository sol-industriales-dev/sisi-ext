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
    public class tblCOES_PlantillatblCOES_ElementoMapping : EntityTypeConfiguration<tblCOES_PlantillatblCOES_Elemento>
    {
        public tblCOES_PlantillatblCOES_ElementoMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");

            ToTable("tblCOES_PlantillatblCOES_Elemento");
        }
    }
}
