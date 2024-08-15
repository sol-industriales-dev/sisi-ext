using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Maquinaria.Mantenimiento
{
    public class tblM_CatPM_CatActividadPM
    {
        /// <summary>
        /// Id de CAts
        /// </summary>
        public int id { get; set; }
        public int idAct { get; set; }//id de la actividad 
        public int idCatTipoActividad { get; set; }// si es esquema, extra, indicacion
        public int idPM { get; set; }//0 si no pertenece a un pm si no pm1 pm2 pm3 pm4
        public int orden { get; set; }//orden en que ira ordenada para visualizar mas adelante
        public int modeloEquipoID{ get; set; }//modelo sobre el cual actuara la configuracion
        public bool estado { get; set; }//modelo sobre el cual actuara la configuracion
        public bool  leyenda { get; set; }// tipo leyenda
        public int perioricidad { get; set; }//  raguilar 21/05/18 que cada tiempo se realiza
        public int idDN { get; set; }//  raguilar 16/06/18 agregado idDN
        public int UsuarioCap { get; set; }
        public DateTime fechaCaptura { get; set; }//raguilar 26/06/18 registro de fecha
    }
}
