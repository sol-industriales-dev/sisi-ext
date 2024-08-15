using Core.Entity.Administrativo.Seguridad.Capacitacion;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.Administrativo.Seguridad.Capacitacion
{
    public class CapacitacionListaAutorizacionMapping : EntityTypeConfiguration<tblS_CapacitacionListaAutorizacion>
    {
        public CapacitacionListaAutorizacionMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");

            Property(x => x.claveLista).HasColumnName("claveLista");
            Property(x => x.cursoID).HasColumnName("cursoID");
            Property(x => x.revision).HasColumnName("revision");
            Property(x => x.jefeDepartamento).HasColumnName("jefeDepartamento");
            Property(x => x.gerenteProyecto).HasColumnName("gerenteProyecto");
            Property(x => x.coordinadorCSH).HasColumnName("coordinadorCSH");
            Property(x => x.secretarioCSH).HasColumnName("secretarioCSH");
            Property(x => x.seguridad).HasColumnName("seguridad");
            Property(x => x.fechaCreacion).HasColumnName("fechaCreacion");
            Property(x => x.division).HasColumnName("division");
            Property(x => x.estatus).HasColumnName("estatus");

            ToTable("tblS_CapacitacionListaAutorizacion");
        }
    }
}
