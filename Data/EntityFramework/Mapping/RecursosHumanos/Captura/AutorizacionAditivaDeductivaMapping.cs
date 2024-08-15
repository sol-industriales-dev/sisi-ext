using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity.ModelConfiguration;//entitytype
using Core.Entity.RecursosHumanos.Captura;
using System.ComponentModel.DataAnnotations.Schema;//database regeneradeoption
namespace Data.EntityFramework.Mapping.RecursosHumanos.Captura
{
    public class AutorizacionAditivaDeductivaMapping : EntityTypeConfiguration<tblRH_AutorizacionAditivaDeductiva>
    {

        public AutorizacionAditivaDeductivaMapping()
            {
                HasKey(x => x.id);
                Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
                Property(x => x.id_AditivaDeductiva).HasColumnName("id_AditivaDeductiva");
                Property(x => x.clave_Aprobador).HasColumnName("clave_Aprobador");
                Property(x => x.nombre_Aprobador).HasColumnName("nombre_Aprobador");
                Property(x => x.puestoAprobador).HasColumnName("puestoAprobador");
                Property(x => x.responsable).HasColumnName("responsable");
                Property(x => x.estatus).HasColumnName("estatus");
                Property(x => x.firma).HasColumnName("firma");
                Property(x => x.autorizando).HasColumnName("autorizando");
                Property(x => x.rechazado).HasColumnName("rechazado");
                Property(x => x.orden).HasColumnName("orden");
                Property(x => x.tipoAutoriza).HasColumnName("tipoAutoriza");
                Property(x => x.fechafirma).HasColumnName("fechafirma");
                Property(x => x.comentario).HasColumnName("comentario");
                ToTable("tblRH_AutorizacionAditivaDeductiva");
            }
    }
}