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
    public class tblEN_UsuarioInsumoMapping : EntityTypeConfiguration<tblEN_UsuarioInsumo>
    {
        public tblEN_UsuarioInsumoMapping()
        {
            HasKey(x => x.Id);
            Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.UsuarioId).HasColumnName("usuarioId");
            Property(x => x.EmpleadoId).HasColumnName("empleadoId");
            Property(x => x.TipoEncuestaId).HasColumnName("tipoEncuestaId");
            Property(x => x.Estatus).HasColumnName("estatus");
            HasRequired(x => x.Usuario).WithMany().HasForeignKey(y => y.UsuarioId);
            HasRequired(x => x.TipoEncuesta).WithMany().HasForeignKey(y => y.TipoEncuestaId);
            ToTable("tblEN_UsuarioInsumo");
        }
    }
}