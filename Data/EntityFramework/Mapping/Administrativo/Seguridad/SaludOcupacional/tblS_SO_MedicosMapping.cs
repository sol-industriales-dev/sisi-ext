using Core.Entity.Administrativo.Seguridad.SaludOcupacional;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.Administrativo.Seguridad.SaludOcupacional
{
    public class tblS_SO_MedicosMapping : EntityTypeConfiguration<tblS_SO_Medicos>
    {
        public tblS_SO_MedicosMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.nombre).HasColumnName("nombre");
            Property(x => x.puesto).HasColumnName("puesto");
            Property(x => x.cedulaProfesional).HasColumnName("cedulaProfesional");
            Property(x => x.empresa).HasColumnName("empresa");
            Property(x => x.idUsuarioCreacion).HasColumnName("idUsuarioCreacion");
            Property(x => x.idUsuarioModificacion).HasColumnName("idUsuarioModificacion");
            Property(x => x.fechaCreacion).HasColumnName("fechaCreacion");
            Property(x => x.fechaModificacion).HasColumnName("fechaModificacion");
            Property(x => x.registroActivo).HasColumnName("registroActivo");

            ToTable("tblS_SO_Medicos");
        }
    }
}
