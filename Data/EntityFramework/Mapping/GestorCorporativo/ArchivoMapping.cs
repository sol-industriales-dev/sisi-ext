using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using Core.Entity.GestorCorporativo;

namespace Data.EntityFramework.Mapping.GestorCorporativo
{
    public class ArchivoMapping : EntityTypeConfiguration<tblGC_Archivo>
    {
        public ArchivoMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.nombre).HasColumnName("nombre");
            Property(x => x.ruta).HasColumnName("ruta");
            Property(x => x.nivel).HasColumnName("nivel");
            Property(x => x.fechaCreacion).HasColumnName("fechaCreacion");
            Property(x => x.esCarpeta).HasColumnName("esCarpeta");
            Property(x => x.orden).HasColumnName("orden");
            Property(x => x.usuarioCreadorID).HasColumnName("usuarioCreadorID");
            HasRequired(x => x.usuario).WithMany().HasForeignKey(x => x.usuarioCreadorID);
            Property(x => x.activo).HasColumnName("activo");
            Property(x => x.grupoCarpeta).HasColumnName("grupoCarpeta");
            Property(x => x.subGrupoCarpeta).HasColumnName("subGrupoCarpeta");
            ToTable("tblGC_Archivo");
        }
    }
}
