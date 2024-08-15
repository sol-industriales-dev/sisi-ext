using Core.Entity.Administrativo.Contabilidad.Sistema_Contable.CentroCostos;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.Administrativo.Contabilidad.SistemaContable.CentroCostos
{
    public class RelObrasMapping : EntityTypeConfiguration<tblC_CC_RelObras>
    {
        public RelObrasMapping()
        {
            HasKey(x => x.Id);
            Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("Id");
            Property(x => x.PalEmpresa).HasColumnName("PalEmpresa");
            Property(x => x.PalObra).HasColumnName("PalObra");
            Property(x => x.SecEmpresa).HasColumnName("SecEmpresa");
            Property(x => x.SecObra).HasColumnName("SecObra");
            Property(x => x.idUsuarioRegistro).HasColumnName("idUsuarioRegistro");
            Property(x => x.esActivo).HasColumnName("esActivo");
            Property(x => x.fechaRegistro).HasColumnName("fechaRegistro");
            ToTable("tblC_CC_RelObras");
        }

    }
}
