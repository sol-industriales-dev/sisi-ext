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
    public class FileVehiculoMapping : EntityTypeConfiguration<tblS_FileVehiculo>
    {
        public FileVehiculoMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.idVehiculo).HasColumnName("idVehiculo");
            Property(x => x.rutaArchivo).HasColumnName("rutaArchivo");
            Property(x => x.nombreArchivo).HasColumnName("nombreArchivo");
            Property(x => x.subida).HasColumnName("subida");
            Property(x => x.usuario).HasColumnName("usuario");
            ToTable("tblS_FileVehiculo");
        }
    }
}
