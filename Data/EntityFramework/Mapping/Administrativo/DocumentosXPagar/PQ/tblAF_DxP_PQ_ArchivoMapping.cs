using Core.Entity.Administrativo.DocumentosXPagar.PQ;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.Administrativo.DocumentosXPagar.PQ
{
    public class tblAF_DxP_PQ_ArchivoMapping : EntityTypeConfiguration<tblAF_DxP_PQ_Archivo>
    {
        public tblAF_DxP_PQ_ArchivoMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.fechaCarga).HasColumnName("fechaCarga");
            Property(x => x.ubicacionArchivo).HasColumnName("ubicacionArchivo");
            Property(x => x.nombreArchivo).HasColumnName("nombreArchivo");
            Property(x => x.estatus).HasColumnName("estatus");
            Property(x => x.fechaCreacion).HasColumnName("fechaCreacion");
            Property(x => x.fechaModificacion).HasColumnName("fechaModificacion");
            Property(x => x.usuarioCreacionId).HasColumnName("usuarioCreacionId");
            Property(x => x.usuarioModificacionId).HasColumnName("usuarioModificacionId");
            HasRequired(x => x.usuarioCreacion).WithMany().HasForeignKey(y => y.usuarioCreacionId);
            HasRequired(x => x.usuarioModificacion).WithMany().HasForeignKey(y => y.usuarioModificacionId);
            ToTable("tblAF_DxP_PQ_Archivo");
        }
    }
}
