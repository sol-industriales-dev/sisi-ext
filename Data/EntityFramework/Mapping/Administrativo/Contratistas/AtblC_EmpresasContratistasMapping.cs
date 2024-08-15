using Core.Entity.Administrativo.Contratistas;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.Administrativo.Contratistas
{
    class AtblC_EmpresasContratistasMapping : EntityTypeConfiguration<tblS_IncidentesEmpresasContratistas> 
    {
        public AtblC_EmpresasContratistasMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.nombreEmpresa).HasColumnName("nombreEmpresa");
            Property(x => x.esActivo).HasColumnName("esActivo");

            ToTable("tblS_IncidentesEmpresasContratistas");
        }
    }
}
