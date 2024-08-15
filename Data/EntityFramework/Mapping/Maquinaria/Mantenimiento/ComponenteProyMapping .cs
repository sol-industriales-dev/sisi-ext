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
    public class ComponenteProyMapping  : EntityTypeConfiguration<tblM_ComponenteProy>
    {
        public ComponenteProyMapping()
        {
        HasKey(x => x.id);
        Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
        Property(x => x.aplicar).HasColumnName("aplicar");
        Property(x => x.estatus).HasColumnName("estatus");
        Property(x => x.idAct).HasColumnName("idAct");
        Property(x => x.tipoPm).HasColumnName("tipoPm");
        ToTable("tblM_ComponenteProy");
        }
    }
}
