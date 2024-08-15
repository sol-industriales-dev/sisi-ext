using Core.Entity.Maquinaria.Mantenimiento;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.DTO.Maquinaria.Mantenimiento;

namespace Data.EntityFramework.Mapping.Maquinaria.Mantenimiento
{
    public class FullCalendarRender : EntityTypeConfiguration<tblM_RenderFullCalendar>
    {
        public FullCalendarRender()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.personalRealizo).HasColumnName("personalRealizo");
            Property(x => x.fechaMantenimientoActual).HasColumnName("fechaMantenimientoActual");
            Property(x => x.tipoMantenimientoActual).HasColumnName("tipoMantenimientoActual");
            Property(x => x.observaciones).HasColumnName("observaciones");
            Property(x => x.economicoID).HasColumnName("economicoID");
            Property(x => x.title).HasColumnName("title");
            Property(x => x.start).HasColumnName("start");
            Property(x => x.description).HasColumnName("description");
            Property(x => x.color).HasColumnName("color");
            Property(x => x.UltimoHorometro).HasColumnName("UltimoHorometro");
            Property(x => x.horometroProyectado).HasColumnName("horometroProyectado");
            Property(x => x.fechaProyectada).HasColumnName("fechaProyectada");
            Property(x => x.idMaquina).HasColumnName("idMaquina");
            Property(x => x.idMantenimiento).HasColumnName("idMantenimiento");
            Property(x => x.HorometroPm).HasColumnName("HorometroPm");
            Property(x => x.borderColor).HasColumnName("borderColor");
            Property(x => x.estadoMantenimiento).HasColumnName("estadoMantenimiento");
            ToTable("tblM_RenderFullCalendar");
        }
    }
}
