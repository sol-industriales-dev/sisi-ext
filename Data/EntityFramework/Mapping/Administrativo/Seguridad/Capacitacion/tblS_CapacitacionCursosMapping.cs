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
    class tblS_CapacitacionCursosMapping: EntityTypeConfiguration<tblS_CapacitacionCursos>
    {
        public tblS_CapacitacionCursosMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.nombre).HasColumnName("nombre");
            Property(x => x.duracion).HasColumnName("duracion");
            Property(x => x.objetivo).HasColumnName("objetivo");
            Property(x => x.temasPrincipales).HasColumnName("temasPrincipales");
            Property(x => x.claveCurso).HasColumnName("claveCurso");
            Property(x => x.referenciasNormativas).HasColumnName("referenciasNormativas");
            Property(x => x.nota).HasColumnName("nota");
            Property(x => x.estatus).HasColumnName("estatus");
            Property(x => x.clasificacion).HasColumnName("clasificacion");
            Property(x => x.tematica).HasColumnName("tematica");
            Property(x => x.esGeneral).HasColumnName("esGeneral");
            Property(x => x.aplicaTodosPuestos).HasColumnName("aplicaTodosPuestos");
            Property(x => x.capacitacionUnica).HasColumnName("capacitacionUnica");
            Property(x => x.isActivo).HasColumnName("isActivo");
            Property(x => x.fechaCreacion).HasColumnName("fechaCreacion");
            Property(x => x.usuarioCreadorID).HasColumnName("usuarioCreadorID");
            Property(x => x.fechaEdicion).HasColumnName("fechaEdicion");
            Property(x => x.usuarioEdicion).HasColumnName("usuarioEdicion");
            Property(x => x.division).HasColumnName("division");
            Property(x => x.reglaPersonalAutorizado).HasColumnName("reglaPersonalAutorizado");
            ToTable("tblS_CapacitacionCursos");
        }
    }
}
