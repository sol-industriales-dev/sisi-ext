using Core.Entity.Administrativo.Seguridad;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.Administrativo.Seguridad
{
    public class VehiculoMapping : EntityTypeConfiguration<tblS_Vehiculo>
    {
        public VehiculoMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.cc).HasColumnName("cc");
            Property(x => x.fecha).HasColumnName("fecha");
            Property(x => x.economico).HasColumnName("economico");
            Property(x => x.decripcion).HasColumnName("decripcion");
            Property(x => x.marcaModelo).HasColumnName("marcaModelo");
            Property(x => x.placas).HasColumnName("placas");
            Property(x => x.tipoEncierro).HasColumnName("tipoEncierro");
            Property(x => x.responsable).HasColumnName("responsable");
            Property(x => x.puesto).HasColumnName("puesto");
            Property(x => x.tipoLicencia).HasColumnName("tipoLicencia");
            Property(x => x.numLicencia).HasColumnName("numLicencia");
            Property(x => x.vigenciaLicencia).HasColumnName("vigenciaLicencia");
            Property(x => x.kilometraje).HasColumnName("kilometraje");
            Property(x => x.preventivo).HasColumnName("preventivo");
            Property(x => x.requerimientos).HasColumnName("requerimientos");
            Property(x => x.notas).HasColumnName("notas");
            ToTable("tblS_Vehiculo");
        }
    }
}
