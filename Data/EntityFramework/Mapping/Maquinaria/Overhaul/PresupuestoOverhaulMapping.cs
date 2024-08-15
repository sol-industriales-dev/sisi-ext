using Core.Entity.Maquinaria.Overhaul;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.Maquinaria.Overhaul
{
    public class PresupuestoOverhaulMapping : EntityTypeConfiguration<tblM_PresupuestoOverhaul>
    {
        PresupuestoOverhaulMapping() 
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.modelo).HasColumnName("modelo");
            Property(x => x.anio).HasColumnName("anio");
            Property(x => x.calendarioID).HasColumnName("calendarioID");
            Property(x => x.estado).HasColumnName("estado");
            Property(x => x.JsonObras).HasColumnName("JsonObras");
            Property(x => x.cerrado).HasColumnName("cerrado");
            ToTable("tblM_PresupuestoOverhaul");
        }
    }
}

