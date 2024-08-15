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
    public class tblM_STB_EconomicoBloqueadoMapping : EntityTypeConfiguration<tblM_STB_EconomicoBloqueado>
    {
        public tblM_STB_EconomicoBloqueadoMapping() 
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.noEconomico).HasColumnName("noEconomico");
            Property(x => x.ccEconomico).HasColumnName("ccEconomico");
            Property(x => x.registroActivo).HasColumnName("registroActivo");
            Property(x => x.fechaRegistro).HasColumnName("fechaRegistro");
            Property(x => x.fechaModificacion).HasColumnName("fechaModificacion");
            Property(x => x.usuarioRegistro).HasColumnName("usuarioRegistro");
            Property(x => x.usuarioModifico).HasColumnName("usuarioModifico");

            ToTable("tblM_STB_EconomicoBloqueado");
        }
    }
}