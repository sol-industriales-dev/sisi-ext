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
    public class tblEN_GrupoInsumoMapping : EntityTypeConfiguration<tblEN_GrupoInsumo>
    {
        public tblEN_GrupoInsumoMapping()
        {
            HasKey(x => x.Id);
            Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.TipoEncuestaId).HasColumnName("tipoEncuestaId");
            Property(x => x.TipoInsumo).HasColumnName("tipoInsumo");
            Property(x => x.GrupoInsumo).HasColumnName("grupoInsumo");
            Property(x => x.Descripcion).HasColumnName("descripcion");
            Property(x => x.Familia).HasColumnName("familia");
            Property(x => x.Consecutivo).HasColumnName("consecutivo");
            Property(x => x.Estatus).HasColumnName("estatus");
            HasRequired(x => x.TipoEncuesta).WithMany().HasForeignKey(y => y.TipoEncuestaId);
            ToTable("tblEN_GrupoInsumo");
        }
    }
}