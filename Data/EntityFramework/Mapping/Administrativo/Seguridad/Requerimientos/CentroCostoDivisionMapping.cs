using Core.Entity.Administrativo.Seguridad.Requerimientos;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.Administrativo.Seguridad.Requerimientos
{
    public class CentroCostoDivisionMapping : EntityTypeConfiguration<tblS_Req_CentroCostoDivision>
    {
        public CentroCostoDivisionMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");

            Property(x => x.cc).HasColumnName("cc");
            Property(x => x.division).HasColumnName("division");
            Property(x => x.fechaCreacion).HasColumnName("fechaCreacion");
            Property(x => x.estatus).HasColumnName("estatus");
            Property(x => x.idEmpresa).HasColumnName("idEmpresa");
            Property(x => x.idAgrupacion).HasColumnName("idAgrupacion");

            ToTable("tblS_Req_CentroCostoDivision");
        }
    }
}
