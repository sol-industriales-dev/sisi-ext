using Core.Entity.Administrativo.Seguridad.Indicadores;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.Administrativo.Seguridad.Indicadores
{
    public class tblS_IncidentesTipoProcedimientosVioladosMapping : EntityTypeConfiguration<tblS_IncidentesTipoProcedimientosViolados>
    {
        public tblS_IncidentesTipoProcedimientosVioladosMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.Procedimineto).HasColumnName("Procedimineto");

            HasMany(x => x.lstInformes)
                .WithMany(x => x.procedimientosViolados)
                .Map(ip =>
                {
                    ip.MapRightKey("idInformePreliminar");
                    ip.MapLeftKey("idProcedimientoViolado");
                    ip.ToTable("tblS_IncidentesInformePreliminarProcedimientoViolado");
                });

            ToTable("tblS_IncidentesTipoProcedimientosViolados");
        }
    }
}
