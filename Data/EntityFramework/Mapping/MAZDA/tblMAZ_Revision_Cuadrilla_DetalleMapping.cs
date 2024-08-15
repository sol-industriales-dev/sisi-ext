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
    class tblMAZ_Revision_Cuadrilla_DetalleMapping : EntityTypeConfiguration<tblMAZ_Revision_Cuadrilla_Detalle>
    {
        public tblMAZ_Revision_Cuadrilla_DetalleMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.actividadID).HasColumnName("actividadID");
            Property(x => x.realizo).HasColumnName("realizo");
            Property(x => x.estadoString).HasColumnName("estadoString");
            Property(x => x.revisionID).HasColumnName("revisionID");
            Property(x => x.estatus).HasColumnName("estatus");

            Property(x => x.ultMant).HasColumnName("ultMant");
            Property(x => x.sigMant).HasColumnName("sigMant");
            Property(x => x.reprogramacion).HasColumnName("reprogramacion");
            Property(x => x.estatusInfo).HasColumnName("estatusInfo");

            ToTable("tblMAZ_Revision_Cuadrilla_Detalle");
        }
    }
}
