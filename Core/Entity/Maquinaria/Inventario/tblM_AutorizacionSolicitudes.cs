using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Maquinaria.Inventario
{
    public class tblM_AutorizacionSolicitudes
    {
        public int id { get; set; }
        public int solicitudEquipoID { get; set; }
        public string folio { get; set; }
        public int usuarioElaboro { get; set; }
        public bool firmaElaboro { get; set; }
        public int gerenteObra { get; set; }
        public bool firmaGerenteObra { get; set; }
        public int directorDivision { get; set; }
        public bool firmaDirectorDivision { get; set; }

        public int GerenteDirector { get; set; }
        public bool firmaGerenteDirector { get; set; }

        public int altaDireccion { get; set; }
        public bool firmaAltaDireccion { get; set; }
        public string observaciones { get; set; }
        public string cadenaFirmaElabora { get; set; }
        public string cadenaFirmaGerenteObra { get; set; }
        public string cadenaFirmaDirector { get; set; }
        public string cadenaFirmaGerenteDirector { get; set; }
        public string cadenaFirmaDireccion { get; set; }
        public virtual tblM_SolicitudEquipo SolicitudEquipo { get; set; }

        public DateTime? FechaDireccion { get; set; }
        public DateTime? FechaGerenteObra { get; set; }
        public DateTime? FechaDirectorDivision { get; set; }
        public DateTime? FechaGerenteDirector { get; set; }
        public DateTime? FechaElabora { get; set; }
        public int directorServicios { get; set; }
        public bool firmaServicios { get; set; }
        public string cadenaFirmaServicios { get; set; }
        public DateTime? FechaServicios { get; set; }
    }
}
