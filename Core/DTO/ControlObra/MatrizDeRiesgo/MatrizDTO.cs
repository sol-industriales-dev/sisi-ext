using Core.Entity.ControlObra.MatrizDeRiesgo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.ControlObra.MatrizDeRiesgo
{
    public class MatrizDTO
    {

        public int id { get; set; }   
        public int idContrato { get; set; }
        public DateTime fechaElaboracion { get; set; }
        public string cc { get; set; }
        public string nombreDelProyecto { get; set; }
        public string personajeElaboro { get; set; }
        public string faseDelProyecto { get; set; }
        public int baja { get; set; }
        public int bajaFin { get; set; }
        public int media { get; set; }
        public int mediaFin { get; set; }
        public int alta { get; set; }
        public int altaFin { get; set; }
        public string tiempoBaja { get; set; }
        public string costoBaja { get; set; }
        public string calidadBaja { get; set; }
        public string tiempoMedia { get; set; }
        public string costoMedia { get; set; }
        public string calidadMedia { get; set; }
        public string tiempoAlta { get; set; }
        public string costoAlta { get; set; }
        public string calidadAlta { get; set; }
        public List<tblCO_MatrizDeRiesgoDetDTO> lstDetalleGuardado { get; set; }

    }
}
