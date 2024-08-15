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
    public  class CatSuministrosMapping:EntityTypeConfiguration<tblM_CatSuministros>
    {
        public CatSuministrosMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.descripcion).HasColumnName("descripcion");
            Property(x => x.estado).HasColumnName("estado");
            Property(x => x.nomeclatura).HasColumnName("nomeclatura");
            Property(x => x.sistema).HasColumnName("sistema");
            Property(x => x.tipo).HasColumnName("idParteVidaUtil");
            ToTable("tblM_CatSuministros");
        }
    }
}
