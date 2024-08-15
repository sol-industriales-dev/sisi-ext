using Core.Entity.Administrativo.Seguridad.Indicadores;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.Administrativo.Seguridad.Indicadores
{
    public class tblS_IncidentesAgrupacionContratistasDetMapping : EntityTypeConfiguration<tblS_IncidentesAgrupacionContratistasDet>
    {
        public tblS_IncidentesAgrupacionContratistasDetMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.idAgruContratista).HasColumnName("idAgruContratista");
            Property(x => x.idContratista).HasColumnName("idContratista");
            Property(x => x.esActivo).HasColumnName("esActivo");
            Property(x => x.fechaCreacion).HasColumnName("fechaCreacion");
            Property(x => x.fechaModificacion).HasColumnName("fechaModificacion");

            HasRequired(x => x.agrupacionContratistas).WithMany().HasForeignKey(x => x.idAgruContratista);
            HasRequired(x => x.contratistas).WithMany().HasForeignKey(x => x.idContratista);

            ToTable("tblS_IncidentesAgrupacionContratistasDet");
        }
    }
}