using Core.Entity.Maquinaria.Inventario;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Maquinaria.Inventario
{
    public class rptTiempoAsignacion
    {

        public tblM_AutorizacionSolicitudes tblM_AutorizacionSolicitudes { get; set; }
        public tblM_AsignacionEquipos tblM_AsignacionEquipos { get; set; }
    }
}
