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
    public class tblEN_Top20ProveedoresMapping : EntityTypeConfiguration<tblEN_Top20Proveedores>
    {
        public tblEN_Top20ProveedoresMapping()
        {
            HasKey(x => x.Id);
            Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.Numero).HasColumnName("numero");
            Property(x => x.NombreCorto).HasColumnName("nombreCorto");
            Property(x => x.TipoTop20Id).HasColumnName("tipoTop20Id");
            Property(x => x.TipoEncuestaId).HasColumnName("tipoEncuestaId");
            Property(x => x.CantidadEvaluaciones).HasColumnName("cantidadEvaluaciones");
            Property(x => x.UsuarioId).HasColumnName("usuarioId");
            Property(x => x.Estatus).HasColumnName("estatus");
            HasRequired(x => x.TipoTop20).WithMany().HasForeignKey(y => y.TipoTop20Id);
            HasRequired(x => x.TipoEncuesta).WithMany().HasForeignKey(y => y.TipoEncuestaId);
            HasRequired(x => x.Usuario).WithMany().HasForeignKey(y => y.UsuarioId);
            ToTable("tblEN_Top20Proveedores");
        }
    }
}