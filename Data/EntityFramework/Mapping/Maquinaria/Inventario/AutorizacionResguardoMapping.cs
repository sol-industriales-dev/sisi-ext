using Core.Entity.Maquinaria.Inventario;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.Maquinaria.Inventario
{
    public class AutorizacionResguardoMapping : EntityTypeConfiguration<tblM_AutorizacionResguardo>
    {
        public AutorizacionResguardoMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.ResguardoVehiculoID).HasColumnName("ResguardoVehiculoID");
            Property(x => x.fechaRegistro).HasColumnName("fechaRegistro");
            Property(x => x.estatus).HasColumnName("estatus");
            Property(x => x.usuarioElaboroFirma).HasColumnName("usuarioElaboroFirma");
            Property(x => x.usuarioElaboroID).HasColumnName("usuarioElaboroID");
            Property(x => x.usuarioReguardoIDEK).HasColumnName("usuarioReguardoIDEK");
            Property(x => x.usuarioSeguridadFirma).HasColumnName("usuarioSeguridadFirma");
            Property(x => x.usuarioSeguridadID).HasColumnName("usuarioSeguridadID");

            ToTable("tblM_AutorizacionResguardo");
        }
    }
}
