using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Maquinaria.Captura.OT
{
    public class OTDTO
    {
        public string FOLIO { get; set; }
        public string ECONOMICO { get; set; }
        public string NOMBRE_PERSONAL { get; set; }
        public string PUESTO { get; set; }
        public string HORA_INICIO { get; set; }
        public string HORA_FIN { get; set; }
        public string OBRA { get; set; }
        public string MODELO { get; set; }
        public string FECHA_HOROMETRO { get; set; }
        public string HOROMETRO { get; set; }
        public string TURNO { get; set; }
        public string MOTIVO_PARO { get; set; }
        public string COMENTARIO_PARO { get; set; }
        public string TIPO_PARO_1 { get; set; }
        public string TIPO_PARO_2 { get; set; }
        public string TIPO_PARO_3 { get; set; }
        public string HORA_ENTRADA { get; set; }
        public string HORA_SALIDA { get; set; }
        public string TIEMPO_PARO_TOTAL_HRS { get; set; }
        public string TIEMPO_PARO_TOTAL_MIN { get; set; }
        public string TIEMPO_REPARACION_HRS { get; set; }
        public string TIEMPO_REPARACION_MIN { get; set; }
        public string TIEMPO_MUERTO_HRS { get; set; }
        public string TIEMPO_MUERTO_MIN { get; set; }
        public string MOTIVO_TIEMPO_MUERTO { get; set; }
        public string METODO_DE_SOLUCION { get; set; }
    }
}
