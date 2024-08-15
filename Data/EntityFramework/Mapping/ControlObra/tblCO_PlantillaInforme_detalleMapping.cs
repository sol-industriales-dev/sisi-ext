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
    class tblCO_PlantillaInforme_detalleMapping : EntityTypeConfiguration<tblCO_PlantillaInforme_detalle>
    {
        public tblCO_PlantillaInforme_detalleMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.ordenDiapositiva).HasColumnName("ordenDiapositiva");
            Property(x => x.tituloDiapositiva).HasColumnName("tituloDiapositiva");
            Property(x => x.contenido).HasColumnName("contenido");

            Property(x => x.plantilla_id).HasColumnName("plantilla_id");
            HasRequired(x => x.plantilla).WithMany(x => x.plantillaDetalles).HasForeignKey(d => d.plantilla_id);


            ToTable("tblCO_PlantillaInforme_detalle");
        }
    }
}
