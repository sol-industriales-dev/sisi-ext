using Core.Entity.RecursosHumanos.Captura;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.RecursosHumanos.Captura
{
    public class tblRH_BN_IncidenciaMapping : EntityTypeConfiguration<tblRH_BN_Incidencia>
    {
        public tblRH_BN_IncidenciaMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.evaluacionID).HasColumnName("evaluacionID");
            Property(x => x.bonoUnicoID).HasColumnName("bonoUnicoID");
            Property(x => x.usuarioID).HasColumnName("usuarioID");
            Property(x => x.id_incidencia).HasColumnName("id_incidencia");
            Property(x => x.anio).HasColumnName("anio");
            Property(x => x.periodo).HasColumnName("periodo");
            Property(x => x.anio).HasColumnName("anio");
            Property(x => x.cc).HasColumnName("cc");
            Property(x => x.tipo_nomina).HasColumnName("tipo_nomina");
            Property(x => x.estatus).HasColumnName("estatus");
            Property(x => x.estatusDesc).HasColumnName("estatusDesc");
            Property(x => x.empleado_modifica).HasColumnName("empleado_modifica");
            Property(x => x.fecha_modifica).HasColumnName("fecha_modifica");
            Property(x => x.usuario_auto).HasColumnName("usuario_auto");
            Property(x => x.fecha_auto).HasColumnName("fecha_auto");
            Property(x => x.nombreEmpMod).HasColumnName("nombreEmpMod");
            Property(x => x.usuario_autoriza_sigoplan).HasColumnName("usuario_autoriza_sigoplan");
            Property(x => x.layoutEnviado).HasColumnName("layoutEnviado");

            HasRequired(x => x.usuario).WithMany().HasForeignKey(y => y.usuarioID);

            ToTable("tblRH_BN_Incidencia");
        }
    }
}
