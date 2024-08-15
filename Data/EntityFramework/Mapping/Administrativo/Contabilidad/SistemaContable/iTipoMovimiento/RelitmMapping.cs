using Core.Entity.Administrativo.Contabilidad.Sistema_Contable.iTiposMovimientos;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.Administrativo.Contabilidad.SistemaContable.iTipoMovimiento
{
    public class RelitmMapping : EntityTypeConfiguration<tblC_TM_Relitm>
    {
        public RelitmMapping()
        {
            HasKey(x => x.Id);
            Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("Id");
            Property(x => x.PalEmpresa).HasColumnName("PalEmpresa");
            Property(x => x.PalSistema).HasColumnName("PalSistema");
            Property(x => x.PaliTm).HasColumnName("PaliTm");
            Property(x => x.PaliTmPeru).HasColumnName("PaliTmPeru");
            Property(x => x.SecEmpresa).HasColumnName("SecEmpresa");
            Property(x => x.SecSistema).HasColumnName("SecSistema");
            Property(x => x.SeciTm).HasColumnName("SeciTm");
            Property(x => x.idUsuarioRegistro).HasColumnName("idUsuarioRegistro");
            Property(x => x.esActivo).HasColumnName("esActivo");
            Property(x => x.fechaRegistro).HasColumnName("fechaRegistro");
            ToTable("tblC_TM_Relitm");
        }
    }
}
