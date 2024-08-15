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
    public class CatPMCatActividadPMMApping : EntityTypeConfiguration<tblM_CatPM_CatActividadPM>
    {
        public CatPMCatActividadPMMApping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.idAct).HasColumnName("idActPM");
            Property(x => x.idCatTipoActividad).HasColumnName("idCatTipoActividad");
            Property(x => x.idPM).HasColumnName("idPM");
            Property(x => x.orden).HasColumnName("orden");
            Property(x => x.modeloEquipoID).HasColumnName("modeloEquipoID");
            Property(x => x.estado).HasColumnName("estado");
            Property(x => x.leyenda).HasColumnName("leyenda");
            Property(x => x.perioricidad).HasColumnName("perioricidad");
            Property(x => x.idDN).HasColumnName("idDN");
            Property(x => x.UsuarioCap).HasColumnName("UsuarioCap");
            Property(x => x.fechaCaptura).HasColumnName("fechaCaptura");
            ToTable("tblM_CatPM_CatActividadPM");
        }
    }
}
