using Core.DTO.ControlObra.EvaluacionSubcontratista;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Core.DTO.ControlObra.EvaluacionSubcontratista
{
    public class SubContratistasDTO
    {
        public string centroCostos { get; set; }
        public string AreaCuenta { get; set; }
        public int id { get; set; }
        public int idPlantilla { get; set; }
        public int idContrato { get; set; }
        public string nombreEvaluacion { get; set; }
        public string nombre { get; set; }
        public int idSubContratista { get; set; }
        public string RFC { get; set; }
        public int tipoEvaluacion { get; set; }
        public int tipoEvaluacionDet { get; set; }
        public int Calificacion { get; set; }
        public string Archivo { get; set; }
        public string Comentario { get; set; }
        public int idRow { get; set; }
        public HttpPostedFileBase File { get; set; }
        public int idEvaluacion { get; set; }
        public string rutaArchivo { get; set; }
        public DateTime? fechaAutorizacion { get; set; }
        public string tipoEvaluacionDesc { get; set; }

        public string planesDeAccion { get; set; }
        public string responsable { get; set; }
        public string fechaCompromiso { get; set; }
        public string mensaje { get; set; }
        public int status { get; set; }
        public double promedio { get; set; }
        public double promedioGlobal { get; set; }
        public int idAreaCuenta { get; set; }
        public int Indicador { get; set; }
        public string cc { get; set; }
        public bool evaluacionPendiente { get; set; }
        public int idAsignacion { get; set; }
        public List<ElementosDTO> lstRelacion { get; set; }
        public bool opcional { get; set; }
        public DateTime? fechaInicial { get; set; }
        public DateTime? fechaFinal { get; set; }
        public int? freqEval { get; set; }

        public bool tieneCargaDeArchivo { get; set; }
        public bool estaEvaluado { get; set; }
        public string btnElemento { get; set; }
        public string nombre_completo { get; set; }
        public string inpServicioContratado { get; set; }
        public string correo { get; set; }

        public int idEstado { get; set; }
        public int idMunicipio { get; set; }
        public int cantEvaluaciones { get; set; }
        public bool elementoVerde { get; set; }
        public int idElemento { get; set; }
        public string nombreSubcontratista { get; set; }
        public string strFechaInicial { get; set; }
        public string strFechaFinal { get; set; }
    }
}
