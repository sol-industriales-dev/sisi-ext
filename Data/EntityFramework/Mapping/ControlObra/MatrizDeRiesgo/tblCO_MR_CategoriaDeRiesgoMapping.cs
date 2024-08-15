using Core.Entity.ControlObra.MatrizDeRiesgo;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.ControlObra.MatrizDeRiesgo
{
    public class tblCO_MR_CategoriaDeRiesgoMapping : EntityTypeConfiguration<tblCO_MR_CategoriaDeRiesgo>
    {
        public tblCO_MR_CategoriaDeRiesgoMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.descripcion).HasColumnName("descripcion");

            ToTable("tblCO_MR_CategoriaDeRiesgo");
        }

    }
}
