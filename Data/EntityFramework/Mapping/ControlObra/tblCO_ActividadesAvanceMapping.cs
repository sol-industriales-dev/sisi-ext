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
    class tblCO_ActividadesAvanceMapping : EntityTypeConfiguration<tblCO_Actividades_Avance>
    {
        public tblCO_ActividadesAvanceMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");          
            Property(x => x.fechaInicio).HasColumnName("fechaInicio");
            Property(x => x.fechaFin).HasColumnName("fechaFin");
            Property(x => x.periodoAvance).HasColumnName("periodoAvance");
            Property(x => x.autorizado).HasColumnName("autorizado");
            Property(x => x.estatus).HasColumnName("estatus");
            Property(x => x.capitulo_id).HasColumnName("capitulo_id");
            HasRequired(x => x.capitulo).WithMany(x => x.actividad_avance).HasForeignKey(d => d.capitulo_id);

            ToTable("tblCO_Actividades_Avance");
        }
    }
}
