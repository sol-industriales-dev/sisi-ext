using Core.Entity.RecursosHumanos.Reclutamientos;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.RecursosHumanos.Reclutamientos
{
    public class tblRH_REC_UniformesMapping : EntityTypeConfiguration<tblRH_REC_Uniformes>
    {
        public tblRH_REC_UniformesMapping()
        {
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.clave_empleado).HasColumnName("clave_empleado");
            Property(x => x.fechaEntrega).HasColumnName("fechaEntrega");
            Property(x => x.calzado).HasColumnName("calzado");
            Property(x => x.camisa).HasColumnName("camisa");
            Property(x => x.pantalon).HasColumnName("pantalon");
            Property(x => x.overol).HasColumnName("overol");
            Property(x => x.uniforme_dama).HasColumnName("uniforme_dama");
            Property(x => x.otros).HasColumnName("otros");
            Property(x => x.comentarios).HasColumnName("comentarios");
            Property(x => x.entrego_calzado).HasColumnName("entrego_calzado");
            Property(x => x.entrego_camisa).HasColumnName("entrego_camisa");
            Property(x => x.entrego_pantalon).HasColumnName("entrego_pantalon");
            Property(x => x.entrego_overol).HasColumnName("entrego_overol");
            Property(x => x.entrego_uniforme_dama).HasColumnName("entrego_uniforme_dama");
            Property(x => x.esActivo).HasColumnName("esActivo");
            Property(x => x.fechaCreacion).HasColumnName("fechaCreacion");
            Property(x => x.fechaModificacion).HasColumnName("fechaModificacion");
            Property(x => x.idUsuarioCreacion).HasColumnName("idUsuarioCreacion");
            Property(x => x.idUsuarioModificacion).HasColumnName("idUsuarioModificacion");

            ToTable("tblRH_REC_Uniformes");
        }
    }
}