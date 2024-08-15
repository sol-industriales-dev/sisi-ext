using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.ControlObra.EvaluacionSubcontratista
{
    public class ContratoSubContratistaDTO
    {
        public int id { get; set; }
        public int idPlantilla { get; set; }
        public int idContrato { get; set; }
        public int idPadre { get; set; }
        public int numeroProveedor { get; set; }
        public string nombre { get; set; }
        public string direccion { get; set; }
        public string nombreCorto { get; set; }
        public string codigoPostal { get; set; }
        public int estatusAutorizacion { get; set; }
        public string cc { get; set; }
        public string numeroContrato { get; set; }
        public bool existeEvaluacionAnterior { get; set; }
        public bool existeEvaluacionPendiente { get; set; }
        public int status { get; set; }
        public int idSubContratista { get; set; }
        public int idAsignacion { get; set; }
        public string descripcion { get; set; }
        public string descripcioncc { get; set; }
        public string mensaje { get; set; }
        public int evaluacionAnteriorid { get; set; }
        public int evaluacionActual { get; set; }
        public string fechaPeriodo { get; set; }
        public DateTime fechaCreacion { get; set; }
        public int tipoUsuario { get; set; }
        public bool statusVobo { get; set; }
        public double promedio { get; set; }
        public List<EvaluacionesDTO> evaluaciones { get; set; }
        public string nombreEvaluacion { get; set; }
        public string emails { get; set; }
        public string perdiodoFechas { get; set; }
        public int statusAutorizacion { get; set; }
        public DateTime fechaInicial { get; set; }
        public DateTime fechaFinal { get; set; }
        public bool evaluacionActiva { get; set; }
        public int tipoUsuarioID { get; set; }
    }
}
