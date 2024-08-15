using Core.Entity.Maquinaria.Inventario;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.Maquinaria.Inventario
{
    public class CatFinancieroMapping : EntityTypeConfiguration<tblM_Comp_CatFinanciero> 
    {
        public CatFinancieroMapping() 
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.descripcion).HasColumnName("descripcion");
            Property(x => x.fechaRegistro).HasColumnName("fechaRegistro");
            Property(x => x.usuarioRegistra).HasColumnName("usuarioRegistra");
            Property(x => x.estado).HasColumnName("estado");
            ToTable("tblM_Comp_CatFinanciero");
        }
    }
}
