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
    public class CatCCAC_AuthMapping : EntityTypeConfiguration<tblM_O_CatCCAC_Auth>
    {
        public CatCCAC_AuthMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.idCatalogo).HasColumnName("idCatalogo");
            Property(x => x.idUsuario).HasColumnName("idUsuario");
            Property(x => x.orden).HasColumnName("orden");
            Property(x => x.fechaFirma).HasColumnName("fechaFirma");
            Property(x => x.firma).HasColumnName("firma");
            Property(x => x.authEstado).HasColumnName("authEstado");
            Property(x => x.motivoRechazo).HasColumnName("motivoRechazo");
            Property(x => x.esActivo).HasColumnName("esActivo");
            Property(x => x.fechaRegistro).HasColumnName("fechaRegistro");
            HasRequired(x => x.usuario).WithMany().HasForeignKey(y => y.idUsuario);
            ToTable("tblM_O_CatCCAC_Auth");
        }
    }
}
