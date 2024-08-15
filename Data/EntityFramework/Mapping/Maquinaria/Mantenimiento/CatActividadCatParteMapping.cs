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
    public class CatActividadCatParteMapping : EntityTypeConfiguration<tblM_CatActividadPM_tblM_CatParte>
    {
        public CatActividadCatParteMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.idActividadPM).HasColumnName("idActividadPM");
            Property(x => x.idParte).HasColumnName("idParte");
            Property(x => x.modeloEquipoID).HasColumnName("modeloEquipoID");
            ToTable("tblM_CatActividadPM_tblM_CatParte");
        }
    }
}
