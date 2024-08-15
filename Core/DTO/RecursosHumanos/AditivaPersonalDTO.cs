using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.RecursosHumanos
{
    //reaguilar consulta 23/11/17
    //por centro de costos y puesto

    public class AditivaPersonal
    {
        public int altas { get; set; }//cantidad de personal actual en el puesto
        public int cantidad { get; set; }//cantidad de personal necesario para el puesto
        public string categoria { get; set; }// categoria
        public string puesto { get; set; }
        public string id_plantilla { get; set; }
        public string condicionInicial { get; set; }
        public string condicionActual { get; set; }
        public string soporte { get; set; }
        public string link { get; set; }

        #region MODULO CH/RECLUTAMIENTO/VACANTES/GESTION SOLICITUDES
        //public int idSolicitud { get; set; }
        #endregion
    }
}
