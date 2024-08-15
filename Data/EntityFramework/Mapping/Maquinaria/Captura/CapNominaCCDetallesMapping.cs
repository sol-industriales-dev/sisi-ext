using Core.Entity.Maquinaria.Captura;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.Maquinaria.Captura
{
    public class CapNominaCCDetallesMapping : EntityTypeConfiguration<tblM_CapNominaCC_Detalles>
    {
        public CapNominaCCDetallesMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.idNomina).HasColumnName("idNomina");
            Property(x => x.economico).HasColumnName("economico");
            Property(x => x.descripcion).HasColumnName("descripcion");
            Property(x => x.cc).HasColumnName("cc");
            Property(x => x.hh).HasColumnName("hh");
            Property(x => x.cargoP).HasColumnName("cargoP");
            Property(x => x.cargoD).HasColumnName("cargoD");
            ToTable("tblM_CapNominaCC_Detalles");
        }
    }
}
