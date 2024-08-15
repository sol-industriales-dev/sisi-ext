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
    public class AutorizacionSolicitudesMapping : EntityTypeConfiguration<tblM_AutorizacionSolicitudes>
    {
        AutorizacionSolicitudesMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.folio).HasColumnName("folio");
            Property(x => x.usuarioElaboro).HasColumnName("usuarioElaboro");
            Property(x => x.firmaElaboro).HasColumnName("firmaElaboro");
            Property(x => x.directorDivision).HasColumnName("directorDivision");
            Property(x => x.firmaDirectorDivision).HasColumnName("firmaDirectorDivision");
            Property(x => x.firmaGerenteObra).HasColumnName("firmaGerenteObra");
            Property(x => x.firmaGerenteDirector).HasColumnName("firmaGerenteDirector");
            Property(x => x.gerenteObra).HasColumnName("gerenteObra");
            Property(x => x.altaDireccion).HasColumnName("altaDireccion");
            Property(x => x.firmaAltaDireccion).HasColumnName("firmaAltaDireccion");
            Property(x => x.observaciones).HasColumnName("observaciones");
            Property(x => x.solicitudEquipoID).HasColumnName("solicitudEquipoID");
            Property(x => x.GerenteDirector).HasColumnName("GerenteDirector");

            Property(x => x.cadenaFirmaElabora).HasColumnName("cadenaFirmaElabora");
            Property(x => x.cadenaFirmaGerenteObra).HasColumnName("cadenaFirmaGerenteObra");
            Property(x => x.cadenaFirmaDirector).HasColumnName("cadenaFirmaDirector");
            Property(x => x.cadenaFirmaDireccion).HasColumnName("cadenaFirmaDireccion");
            Property(x => x.cadenaFirmaGerenteDirector).HasColumnName("cadenaFirmaGerenteDirector");

            Property(x => x.directorServicios).HasColumnName("directorServicios");
            Property(x => x.firmaServicios).HasColumnName("firmaServicios");
            Property(x => x.cadenaFirmaServicios).HasColumnName("cadenaFirmaServicios");
            Property(x => x.FechaServicios).HasColumnName("FechaServicios");
            HasRequired(x => x.SolicitudEquipo).WithMany().HasForeignKey(y => y.solicitudEquipoID);

            ToTable("tblM_AutorizacionSolicitudes");
        }
    }
}
