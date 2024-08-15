using Core.Entity.Administrativo.Contabilidad.Nomina;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.Administrativo.Contabilidad.Nomina
{
    public class tblC_Nom_CatalogoCCMapping : EntityTypeConfiguration<tblC_Nom_CatalogoCC>
    {
        public tblC_Nom_CatalogoCCMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.cc).HasColumnName("cc");
            Property(x => x.ccDescripcion).HasColumnName("ccDescripcion");
            Property(x => x.clasificacionCcId).HasColumnName("clasificacionCcId");
            Property(x => x.estatus).HasColumnName("estatus");
            Property(x => x.fechaCreacion).HasColumnName("fechaCreacion");
            Property(x => x.fechaModificacion).HasColumnName("fechaModificacion");
            Property(x => x.usuarioCreacionId).HasColumnName("usuarioCreacionId");
            Property(x => x.area).HasColumnName("area");
            Property(x => x.cuenta).HasColumnName("cuenta");
            Property(x => x.usuarioModificacionId).HasColumnName("usuarioModificacionId");
            Property(x => x.semanal).HasColumnName("semanal");
            Property(x => x.quincenal).HasColumnName("quincenal");
            Property(x => x.depositoSindicato).HasColumnName("depositoSindicato");
            Property(x => x.porcentajeSindicato).HasColumnName("porcentajeSindicato");
            HasRequired(x => x.clasificacion).WithMany().HasForeignKey(y => y.clasificacionCcId);
            HasRequired(x => x.usuarioCreacion).WithMany().HasForeignKey(y => y.usuarioCreacionId);
            HasRequired(x => x.usuarioModificacion).WithMany().HasForeignKey(y => y.usuarioModificacionId);
            ToTable("tblC_Nom_CatalogoCC");
        }
    }
}
