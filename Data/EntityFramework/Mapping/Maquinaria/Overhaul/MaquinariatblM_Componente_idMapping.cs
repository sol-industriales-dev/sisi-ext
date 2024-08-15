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
    public class MaquinariatblM_Componente_idMapping : EntityTypeConfiguration<tblM_MaquinariatblM_Componente>
    {

        public MaquinariatblM_Componente_idMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.tblM_CatComponente_id).HasColumnName("tblM_CatComponente_id");
            Property(x => x.tblM_CatMaquina_id).HasColumnName("tblM_CatMaquina_id");
            Property(x => x.estatus).HasColumnName("estatus");
            ToTable("tblM_ComentariosNotaCredito");
        }
    }
}
