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
    public class PuntoMapping : EntityTypeConfiguration<tblS_Req_Punto>
    {
        public PuntoMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");

            Property(x => x.descripcion).HasColumnName("descripcion");
            Property(x => x.verificacion).HasColumnName("verificacion");
            Property(x => x.porcentaje).HasColumnName("porcentaje");
            Property(x => x.fechaCreacion).HasColumnName("fechaCreacion");
            Property(x => x.indice).HasColumnName("indice");
            Property(x => x.periodicidad).HasColumnName("periodicidad");
            Property(x => x.actividad).HasColumnName("actividad");
            Property(x => x.condicionante).HasColumnName("condicionante");
            Property(x => x.seccion).HasColumnName("seccion");
            Property(x => x.codigo).HasColumnName("codigo");
            Property(x => x.area).HasColumnName("area");
            Property(x => x.requerimientoID).HasColumnName("requerimientoID");
            Property(x => x.division).HasColumnName("division");
            Property(x => x.estatus).HasColumnName("estatus");

            ToTable("tblS_Req_Punto");
        }
    }
}
