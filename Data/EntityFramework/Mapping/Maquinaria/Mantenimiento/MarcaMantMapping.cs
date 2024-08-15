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
    public class MarcaMantMapping : EntityTypeConfiguration<tblM_CatMarcaMant>
    {
        public MarcaMantMapping() 
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.marca).HasColumnName("marca");
            ToTable("tblM_ActvContPM");
        }
    }
}
