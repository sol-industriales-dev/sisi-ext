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
    class tblMAZ_Usuario_CuadrillaMapping : EntityTypeConfiguration<tblMAZ_Usuario_Cuadrilla>
    {
        public tblMAZ_Usuario_CuadrillaMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.nombre).HasColumnName("nombre");
            Property(x => x.apellidoPaterno).HasColumnName("apellidoPaterno");
            Property(x => x.apellidoMaterno).HasColumnName("apellidoMaterno");
            Property(x => x.correo).HasColumnName("correo");
            Property(x => x.nombreUsuario).HasColumnName("nombreUsuario");
            Property(x => x.contrasena).HasColumnName("contrasena");
            Property(x => x.cuadrillaID).HasColumnName("cuadrillaID");
            //Property(x => x.actividadPeriodoID).HasColumnName("actividadPeriodoID");
            Property(x => x.nivel).HasColumnName("nivel");
            Property(x => x.orden).HasColumnName("orden");
            Property(x => x.estatus).HasColumnName("estatus");
            ToTable("tblMAZ_Usuario_Cuadrilla");
        }
    }
}
