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
    public class tblM_PresupuestoHCMapping : EntityTypeConfiguration<tblM_PresupuestoHC>
    {
        tblM_PresupuestoHCMapping() 
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.obra).HasColumnName("obra");
            Property(x => x.pAutorizado).HasColumnName("pAutorizado");
            Property(x => x.pProgramado).HasColumnName("pProgramado");
            Property(x => x.eProgramado).HasColumnName("eProgramado");
            Property(x => x.eNoProgramado).HasColumnName("eNoProgramado");
            Property(x => x.pTotal).HasColumnName("pTotal");
            Property(x => x.eTotal).HasColumnName("eTotal");
            Property(x => x.bolsa).HasColumnName("bolsa");
            ToTable("tblM_PresupuestoHC");
        }
    }
}
