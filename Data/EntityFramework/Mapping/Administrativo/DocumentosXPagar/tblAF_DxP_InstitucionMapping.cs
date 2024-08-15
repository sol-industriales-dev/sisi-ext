using Core.Entity.Administrativo.DocumentosXPagar;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.Administrativo.DocumentosXPagar
{
    public class tblAF_DxP_InstitucionMapping : EntityTypeConfiguration<tblAF_DxP_Institucion>
    {
        public tblAF_DxP_InstitucionMapping()
        {
            HasKey(x => x.Id);
            Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.Nombre).HasColumnName("nombre");
            Property(x => x.esPQ).HasColumnName("esPQ");
            Property(x => x.Estatus).HasColumnName("estatus");
            Property(x => x.FechaCreacion).HasColumnName("fechaCreacion");
            Property(x => x.UsuarioCreacionId).HasColumnName("usuarioCreacionId");
            Property(x => x.FechaModificacion).HasColumnName("fechaModificacion");
            Property(x => x.UsuarioModificacionId).HasColumnName("usuarioModificacionId");
            HasRequired(x => x.UsuarioCreacion).WithMany().HasForeignKey(y => y.UsuarioCreacionId);
            HasRequired(x => x.UsuarioModificacion).WithMany().HasForeignKey(y => y.UsuarioModificacionId);
            ToTable("tblAF_DxP_Institucion");
        }
    }
}