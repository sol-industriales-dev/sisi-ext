using Core.Entity.Administrativo.Proyecciones;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.Proyecciones
{
    public class CapCifrasPrincipalesMapping : EntityTypeConfiguration<tblPro_CapCifrasPrincipales>
    {
        public CapCifrasPrincipalesMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.ejercicioAnio).HasColumnName("ejercicioAnio");
            Property(x => x.estatus).HasColumnName("estatus");
            Property(x => x.MesInicio).HasColumnName("MesInicio");


            Property(x => x.UtilidadPlaneadaAnioActual).HasColumnName("UtilidadPlaneadaAnioActual");
            Property(x => x.UtilidadPlaneadaAnioAnterior).HasColumnName("UtilidadPlaneadaAnioAnterior");
            Property(x => x.UtilidadPlaneadaMesActual).HasColumnName("UtilidadPlaneadaMesActual");

            Property(x => x.UtilidadRealAnioActual).HasColumnName("UtilidadRealAnioActual");
            Property(x => x.UtilidadRealAnioAnterior).HasColumnName("UtilidadRealAnioAnterior");
            Property(x => x.UtilidadRealMesActual).HasColumnName("UtilidadRealMesActual");

            Property(x => x.VentaProyectadaAlAnio).HasColumnName("VentaProyectadaAlAnio");
            Property(x => x.VentaProyectadaAnioAnterior).HasColumnName("VentaProyectadaAnioAnterior");
            Property(x => x.VentaProyectadaMesActual).HasColumnName("VentaProyectadaMesActual");

            Property(x => x.VentaRealAnioAnterior).HasColumnName("VentaRealAnioAnterior");
            Property(x => x.VentaRealMesActual).HasColumnName("VentaRealMesActual");
            Property(x => x.VentaRealProyectdaAlAnio).HasColumnName("VentaRealProyectdaAlAnio");

            ToTable("tblPro_CapCifrasPrincipales");
        }
    }
}
