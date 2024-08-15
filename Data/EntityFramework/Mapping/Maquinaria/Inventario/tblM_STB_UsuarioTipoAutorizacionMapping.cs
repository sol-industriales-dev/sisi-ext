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
    public class tblM_STB_UsuarioTipoAutorizacionMapping : EntityTypeConfiguration<tblM_STB_UsuarioTipoAutorizacion>
    {
        public tblM_STB_UsuarioTipoAutorizacionMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            ToTable("tblM_STB_UsuarioTipoAutorizacion");
        }

    }
}
