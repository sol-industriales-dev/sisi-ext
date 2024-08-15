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
    public class RelacionCCMantMapping : EntityTypeConfiguration<tblM_RelacionCCMant>
    {
        public RelacionCCMantMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.cc).HasColumnName("cc");
            Property(x => x.estado).HasColumnName("estado");
            Property(x => x.usuarioID).HasColumnName("usuarioID");
            ToTable("tblM_RelacionCCMant");
        }
    }
}
