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
    public class tblEN_UsuariosExcepcionTop20Mapping : EntityTypeConfiguration<tblEN_UsuariosExcepcionTop20>
    {
        public tblEN_UsuariosExcepcionTop20Mapping()
        {
            HasKey(x => x.Id);
            Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.IdUsuario).HasColumnName("idUsuario");
            Property(x => x.Estatus).HasColumnName("estatus");
            ToTable("tblEN_UsuariosExcepcionTop20");
        }
    }
}
