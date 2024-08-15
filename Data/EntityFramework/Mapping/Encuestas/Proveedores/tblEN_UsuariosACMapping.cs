using Core.Entity.Encuestas.Proveedores;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.Encuestas.Proveedores
{
    public class tblEN_UsuariosACMapping : EntityTypeConfiguration<tblEN_UsuariosAC>
    {
        public tblEN_UsuariosACMapping()
        {
            HasKey(x => x.Id);
            Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.UsuarioInsumoId).HasColumnName("usuarioInsumoId");
            Property(x => x.AreaCuenta).HasColumnName("areaCuenta");
            Property(x => x.Estatus).HasColumnName("estatus");
            HasRequired(x => x.UsuarioInsumo).WithMany().HasForeignKey(y => y.UsuarioInsumoId);
            ToTable("tblEN_UsuariosAC");
        }
    }
}
