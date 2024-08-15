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
    public class tblM_CatMaquina_EstatusDiario_Usuario_CCMapping : EntityTypeConfiguration<tblM_CatMaquina_EstatusDiario_Usuario_CC>
    {
        public tblM_CatMaquina_EstatusDiario_Usuario_CCMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.usuarioID).HasColumnName("usuarioID");
            Property(x => x.cc).HasColumnName("cc");
            
            ToTable("tblM_CatMaquina_EstatusDiario_Usuario_CC");
        }
    }
}
