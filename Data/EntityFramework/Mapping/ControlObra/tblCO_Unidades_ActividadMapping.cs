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
    class tblCO_Unidades_ActividadMapping : EntityTypeConfiguration<tblCO_Unidades_Actividad>
    {
        public tblCO_Unidades_ActividadMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");          
            Property(x => x.actividad_id).HasColumnName("actividad_id");
            HasRequired(x => x.Actividad).WithMany(x => x.unidadesCostos).HasForeignKey(d => d.actividad_id);
            Property(x => x.unidad_id).HasColumnName("unidad_id");
            HasRequired(x => x.unidad).WithMany(x => x.unidadesCostos).HasForeignKey(d => d.unidad_id);
            ToTable("tblCO_Unidades_Actividad");
        }
    }
}
