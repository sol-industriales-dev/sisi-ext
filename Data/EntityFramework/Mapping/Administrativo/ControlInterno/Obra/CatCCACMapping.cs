using Core.Entity.Administrativo.ControlInterno.Obra;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.Administrativo.ControlInterno.Obra
{
    public class CatCCACMapping : EntityTypeConfiguration<tblM_O_CatCCAC>
    {
        public CatCCACMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.tipo).HasColumnName("tipo");
            Property(x => x.clave).HasColumnName("clave");
            Property(x => x.descripcion).HasColumnName("descripcion");
            Property(x => x.fechaArranque).HasColumnName("fechaArranque");
            Property(x => x.idUsuarioRegistro).HasColumnName("idUsuarioRegistro");
            Property(x => x.authEstado).HasColumnName("authEstado");
            Property(x => x.exiteEnkontrol).HasColumnName("exiteEnkontrol");
            Property(x => x.esActivo).HasColumnName("esActivo");
            Property(x => x.fechaRegistro).HasColumnName("fechaRegistro");
            HasRequired(x => x.usuario).WithMany().HasForeignKey(y => y.idUsuarioRegistro);
            ToTable("tblM_O_CatCCAC");
        }
    }
}
