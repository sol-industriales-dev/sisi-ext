using Core.Entity.Maquinaria.Captura;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.Maquinaria.Captura.Conciliacion
{
    public class ConsideracionCostoHoraMapping : EntityTypeConfiguration<tblM_CatConsideracionCostoHora>
    {
        public ConsideracionCostoHoraMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.descripcion).HasColumnName("descripcion");
            Property(x => x.tipo).HasColumnName("tipo");
            ToTable("tblM_CatConsideracionCostoHora");
        }
    }
}
