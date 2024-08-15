using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Administracion.Seguridad.Capacitacion.CincoS
{
    public class InspeccionesDTO
    {
        #region SQL
        public int id { get; set; }
        public string inspeccion { get; set; }
        public int area { get; set; }
        public string areaDesc { get; set; }
        public int subAreaId { get; set; }
        public string subAreaDesc { get; set; }
        public string subAreaDescripcion { get; set; }
        public List<int> cincoS { get; set; }
        #endregion

        #region ADICIONAL
        public int cantPuntos { get; set; }
        public string descripcion { get; set; }
        public string accion { get; set; }
        #endregion
    }
}
