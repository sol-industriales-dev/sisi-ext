using Core.Entity.MAZDA;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.MAZDA
{
    class tblMAZ_Revision_Cuadrilla_AyudantesMapping : EntityTypeConfiguration<tblMAZ_Revision_Cuadrilla_Ayudantes>
    {
        public tblMAZ_Revision_Cuadrilla_AyudantesMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.idPersonal).HasColumnName("idPersonal");
            Property(x => x.revisionID).HasColumnName("revisionID");
            Property(x => x.estatus).HasColumnName("estatus");

            ToTable("tblMAZ_Revision_Cuadrilla_Ayudantes");
        }
    }
}
