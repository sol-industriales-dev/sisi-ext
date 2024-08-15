using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.SeguimientoAcuerdos
{
    public class ActividadesDTO
    {
        public int id { get; set; }
        public string minuta { get; set; }
        public int minutaID { get; set; }
        public int columna { get; set; }
        public int orden { get; set; }
        public string tipo { get; set; }
        public string actividad { get; set; }
        public string descripcion { get; set; }
        public int responsableID { get; set; }
        public string responsable { get; set; }
        public string fechaInicio { get; set; }
        public string fechaCompromiso { get; set; }
        public int prioridad { get; set; }
        public int comentariosCount { get; set; }
        public bool interesado { get; set; }
        public bool responsablesbool { get; set; }
        public int revisaID { get; set; }
        public string revisa { get; set; }
        public int comID { get; set; }
        public int comActividadID { get; set; }
        public string comComentario { get; set; }
        public string comUsuarioNombre { get; set; }
        public int comUsuarioID { get; set; }
        public string comFecha { get; set; }
        public string comTipo { get; set; }
        public string comAdjuntoNombre { get; set; }
        public int responsablesID { get; set; }
    }
}
