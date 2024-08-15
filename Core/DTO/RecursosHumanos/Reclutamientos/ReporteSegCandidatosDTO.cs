using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.RecursosHumanos.Reclutamientos
{
    public class ReporteSegCandidatosDTO
    {
        public int id { get; set; }
        public int idPuesto { get; set; }
        public string puesto { get; set; }
        public string departamento { get; set; }
        public string nombre { get; set; }
        public int edad { get; set; }
        public string nss { get; set; }
        public string residencia { get; set; }
        public string telefono { get; set; }
        public DateTime fechaEntrevista { get; set; }
        public decimal porcProceso { get; set; }
        public DateTime fechaLiberacion { get; set; }
        public int tiempoTranscurrido { get; set; }
        public string comentarios { get; set; }
        public int claveDepto { get; set; }
        public DateTime fechaNacimiento { get; set; }
        public int idGestionSolicitud { get; set; }
        public int idCandidato { get; set; }
        public string estatusFase { get; set; }
        public int estatus { get; set; }
        public DateTime fechaCreacion { get; set; }
        public string cc { get; set; }
    }
}