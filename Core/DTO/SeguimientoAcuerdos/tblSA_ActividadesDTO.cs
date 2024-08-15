using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.SeguimientoAcuerdos
{
    public class tblSA_ActividadesDTO
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
        public List<int> responsablesID { get; set; }
        public List<string> responsables { get; set; }
        public string fechaInicio { get; set; }
        public string fechaCompromiso { get; set; }
        public int prioridad { get; set; }
        public int comentariosCount { get; set; }
        public virtual List<tblSA_ComentariosDTO> comentarios { get; set; }
        public bool interesado { get; set; }
        public bool responsablesbool { get; set; }
        public int revisaID { get; set; }
        public string revisa { get; set; }
    }
}
