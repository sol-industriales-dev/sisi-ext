using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Entity.Maquinaria.Mantenimiento;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Data.EntityFramework.Mapping.Maquinaria.Mantenimiento
{
    public class tblPmMapping : EntityTypeConfiguration<tblM_CatPM1>
    {
        public tblPmMapping ()
        {
        HasKey(x => x.id);
        Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
        //Property(x => x.factor).HasColumnName("factor");
        Property(x => x.nombre).HasColumnName("nombre");
        ToTable("tblM_CatPM1");
        }
    }
}
