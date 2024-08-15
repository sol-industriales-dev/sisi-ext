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
    public class ParosMapping:EntityTypeConfiguration<tblM_Paros>
    {

        public ParosMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.id_maquina).HasColumnName("id_maquina");
            Property(x => x.fecha_paro).HasColumnName("fecha_paro");
            Property(x => x.descripcion).HasColumnName("descripcion");
            ToTable("tblM_Paros");
        }

    }
}
