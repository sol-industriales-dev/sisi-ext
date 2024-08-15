using Core.Entity.Maquinaria.Mantenimiento;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.Maquinaria.Mantenimiento
{
   public class ComponenteVincMapping:EntityTypeConfiguration<tblM_ComponenteMantenimiento>
    {
       public ComponenteVincMapping()
       {
           HasKey(x => x.id);
           Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
           Property(x => x.modeloEquipoID).HasColumnName("modeloEquipoID");
           Property(x => x.estado).HasColumnName("estado");
           Property(x => x.idAct).HasColumnName("idAct");
           Property(x => x.idCompVis).HasColumnName("idCompVis");
           Property(x => x.idCatTipoActividad).HasColumnName("idCatTipoActividad");
           Property(x => x.idPM).HasColumnName("idPM");
           Property(x => x.UsuarioCap).HasColumnName("UsuarioCap");
           Property(x => x.fechaCaptura).HasColumnName("fechaCaptura");
           ToTable("tblM_CatActividadPM");
       }
    }
}
