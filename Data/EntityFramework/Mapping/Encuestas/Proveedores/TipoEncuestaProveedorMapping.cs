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
    public class TipoEncuestaProveedorMapping : EntityTypeConfiguration<tblEN_TipoEncuestaProveedor>
    {
        public TipoEncuestaProveedorMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.descripcion).HasColumnName("descripcion");
            Property(x => x.tipoDependencia).HasColumnName("tipoDependencia");
            Property(x => x.esActivo).HasColumnName("esActivo");
            Property(x => x.fechaRegistro).HasColumnName("fechaRegistro");
            ToTable("tblEN_TipoEncuestaProveedor");
        }
    }
}
