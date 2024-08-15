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
    class tblCO_PlantillaInformeMapping : EntityTypeConfiguration<tblCO_PlantillaInforme>
    {
        public tblCO_PlantillaInformeMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.cantidadDiapositivas).HasColumnName("cantidadDiapositivas");
            Property(x => x.estatus).HasColumnName("estatus");

            Property(x => x.division_id).HasColumnName("division_id");
            HasRequired(x => x.division).WithMany(x => x.plantillasInformes).HasForeignKey(d => d.division_id);


            ToTable("tblCO_PlantillaInforme");
        }
    }
}
