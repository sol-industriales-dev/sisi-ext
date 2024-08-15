using Core.Entity.Administrativo.RecursosHumanos.Mural;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.Administrativo.RecursosHumanos.Mural
{
    public class tblRH_Mural_PostItMapping : EntityTypeConfiguration<tblRH_Mural_PostIt>
    {
        public tblRH_Mural_PostItMapping()
        {
            HasKey(x => x.Id);
            Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.IdMural).HasColumnName("idMural");
            Property(x => x.Texto).HasColumnName("texto");
            Property(x => x.PosicionX).HasColumnName("posicionX");
            Property(x => x.PosicionY).HasColumnName("posicionY");
            Property(x => x.ColorFondo).HasColumnName("colorFondo");
            Property(x => x.Altura).HasColumnName("altura");
            Property(x => x.Ancho).HasColumnName("ancho");
            Property(x => x.IdSeccion).HasColumnName("idSeccion");
            Property(x => x.Estatus).HasColumnName("estatus");
            Property(x => x.FechaCreacion).HasColumnName("fechaCreacion");
            Property(x => x.FechaModificacion).HasColumnName("fechaModificacion");
            Property(x => x.IdUsuarioCreacion).HasColumnName("idUsuarioCreacion");
            HasRequired(x => x.Mural).WithMany().HasForeignKey(y => y.IdMural);
            HasRequired(x => x.UsuarioCreacion).WithMany().HasForeignKey(y => y.IdUsuarioCreacion);
            HasOptional(x => x.Seccion).WithMany().HasForeignKey(y => y.IdSeccion);
            ToTable("tblRH_Mural_PostIt");
        }
    }
}
