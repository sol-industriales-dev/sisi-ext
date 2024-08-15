using Core.Entity.RecursosHumanos.Desempeno;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.RecursosHumanos.Desempeno
{
    public class DetMetasMapping : EntityTypeConfiguration<tblRH_ED_DetMetas>
    {
        public DetMetasMapping()
        {
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.idProceso).HasColumnName("idProceso");
            Property(x => x.idUsuario).HasColumnName("idUsuario");
            Property(x => x.idJefe).HasColumnName("idJefe");
            Property(x => x.nombre).HasColumnName("nombre");
            Property(x => x.tipo).HasColumnName("tipo");
            Property(x => x.descripcion).HasColumnName("descripcion");
            Property(x => x.peso).HasColumnName("peso");
            Property(x => x.esVobo).HasColumnName("esVobo");
            Property(x => x.notificado).HasColumnName("notificado");
            Property(x => x.esActivo).HasColumnName("esActivo");
            Property(x => x.fechaRegistro).HasColumnName("fechaRegistro");
            HasRequired(x => x.proceso).WithMany().HasForeignKey(y => y.idProceso);
            HasRequired(x => x.estrategia).WithMany().HasForeignKey(y => y.tipo);
            HasRequired(x => x.usuario).WithMany().HasForeignKey(y => y.idUsuario);
            HasRequired(x => x.jefe).WithMany().HasForeignKey(y => y.idJefe);
            ToTable("tblRH_ED_DetMetas");
        }
    }
}
