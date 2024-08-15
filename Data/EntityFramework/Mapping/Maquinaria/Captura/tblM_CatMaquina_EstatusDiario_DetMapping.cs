using Core.Entity.Maquinaria.Captura;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.Maquinaria.Captura
{
    public class tblM_CatMaquina_EstatusDiario_DetMapping : EntityTypeConfiguration<tblM_CatMaquina_EstatusDiario_Det>
    {
        public tblM_CatMaquina_EstatusDiario_DetMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.estatusDiarioID).HasColumnName("estatusDiarioID");
            Property(x => x.noEconomicoID).HasColumnName("noEconomicoID");
            Property(x => x.noEconomico).HasColumnName("noEconomico");
            Property(x => x.activo).HasColumnName("activo");
            Property(x => x.causa).HasColumnName("causa");
            Property(x => x.fecha_inicial).HasColumnName("fecha_inicial");
            Property(x => x.fecha_proyectada).HasColumnName("fecha_proyectada");
            Property(x => x.fecha_real).HasColumnName("fecha_real");
            Property(x => x.tiempo_respuesta_str).HasColumnName("tiempo_respuesta_str");
            Property(x => x.tiempo_respuesta).HasColumnName("tiempo_respuesta");
            Property(x => x.acciones).HasColumnName("acciones");

            ToTable("tblM_CatMaquina_EstatusDiario_Det");
        }
    }
}
