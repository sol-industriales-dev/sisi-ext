using Core.Entity.Administrativo.Seguridad.Indicadores;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.Administrativo.Seguridad.AgrupacionCC
{
    public class tblS_IncidentesAgrupacionCCMapping : EntityTypeConfiguration<tblS_IncidentesAgrupacionCC>
    {
        public tblS_IncidentesAgrupacionCCMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.nomAgrupacion).HasColumnName("nomAgrupacion");
            Property(x => x.fechaCreacion).HasColumnName("fechaCreacion");
            Property(x => x.fechaModificacion).HasColumnName("fechaModificacion");
            Property(x => x.codigo).HasColumnName("codigo");
            Property(x => x.esActivo).HasColumnName("esActivo");

            ToTable("tblS_IncidentesAgrupacionCC");
        }
    }
}
