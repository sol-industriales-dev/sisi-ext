using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Maquinaria.BackLogs
{
    public class RequisicionesDTO
    {
        public int id { get; set; }
        public string numRequisicion { get; set; }
        public int idBackLog { get; set; }
        public bool esActivo { get; set; }
        public DateTime fechaModificacionRequisicion { get; set; }
        public string numero { get; set; } //PROPIEDAD DE ENKONTROL
        public DateTime fecha { get; set; }
        public string comentarios { get; set; }
        public string partida { get; set; }
        public int insumo { get; set; }
        public DateTime fecha_requerido { get; set; }
        public int cantidad { get; set; }
        public DateTime fecha_ordenada { get; set; }
        public string descripcion { get; set; }
    }
}