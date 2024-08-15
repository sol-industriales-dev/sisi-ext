using Core.Entity.Maquinaria.Rentabilidad;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.Maquinaria.Rentabilidad
{
    public class KBDivisionDetalleMapping : EntityTypeConfiguration<tblM_KBDivisionDetalle>
    {
        public KBDivisionDetalleMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.divisionID).HasColumnName("divisionID");
            Property(x => x.acID).HasColumnName("acID");
            Property(x => x.estatus).HasColumnName("estatus");

            HasRequired(x => x.division).WithMany(x => x.divisionDetalle).HasForeignKey(d => d.divisionID);
            HasRequired(x => x.ac).WithMany().HasForeignKey(d => d.acID);
            ToTable("tblM_KBDivisionDetalle");
        }
    }
}
