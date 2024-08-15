using Core.Entity.Maquinaria.Captura;
using Core.Service.Maquinaria.Capturas;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.Maquinaria.Captura
{
    public class CapturaOTMapping : EntityTypeConfiguration<tblM_CapOrdenTrabajo>
    {
        CapturaOTMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.CC).HasColumnName("CC");
            Property(x => x.DescripcionMotivo).HasColumnName("DescripcionMotivo");
            Property(x => x.DescripcionTiempoMuerto).HasColumnName("DescripcionTiempoMuerto");
            Property(x => x.EconomicoID).HasColumnName("EconomicoID");
            Property(x => x.Comentario).HasColumnName("Comentario");
            Property(x => x.FechaCreacion).HasColumnName("FechaCreacion");
            Property(x => x.FechaEntrada).HasColumnName("FechaEntrada");
            Property(x => x.FechaSalida).HasColumnName("FechaSalida").IsOptional();

            Property(x => x.horometro).HasColumnName("horometro");
            Property(x => x.MotivoParo).HasColumnName("MotivoParo");
            Property(x => x.TiempoMuerto).HasColumnName("TiempoMuerto");
            Property(x => x.TiempoReparacion).HasColumnName("TiempoReparacion");
            Property(x => x.TiempoTotalParo).HasColumnName("TiempoTotalParo");
            Property(x => x.TipoParo1).HasColumnName("TipoParo1");
            Property(x => x.TipoParo2).HasColumnName("TipoParo2");
            Property(x => x.TipoParo3).HasColumnName("TipoParo3");
            Property(x => x.Turno).HasColumnName("Turno");

            Property(x => x.TiempoHorasTotal).HasColumnName("TiempoHorasTotal");
            Property(x => x.TiempoHorasReparacion).HasColumnName("TiempoHorasReparacion");
            Property(x => x.TiempoHorasMuerto).HasColumnName("TiempoHorasMuerto");
            Property(x => x.TiempoMinutosTotal).HasColumnName("TiempoMinutosTotal");
            Property(x => x.TiempoMinutosReparacion).HasColumnName("TiempoMinutosReparacion");
            Property(x => x.TiempoMinutosMuerto).HasColumnName("TiempoMinutosMuerto");

            Property(x => x.usuarioCapturaID).HasColumnName("usuarioCapturaID");


            Property(x => x.TipoOT).HasColumnName("TipoOT");
            Property(x => x.EstatusOT).HasColumnName("EstatusOT");
            Property(x => x.folio).HasColumnName("folio");

            ToTable("tblM_CapOrdenTrabajo");
        }
    }
}
