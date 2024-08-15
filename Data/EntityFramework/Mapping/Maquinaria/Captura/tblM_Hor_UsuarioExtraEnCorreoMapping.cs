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
    public class tblM_Hor_UsuarioExtraEnCorreoMapping : EntityTypeConfiguration<tblM_Hor_UsuarioExtraEnCorreo>
    {
        public tblM_Hor_UsuarioExtraEnCorreoMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            HasRequired(x => x.usuario).WithMany().HasForeignKey(x => x.usuarioId);
            ToTable("tblM_Hor_UsuarioExtraEnCorreo");
        }
    }
}
