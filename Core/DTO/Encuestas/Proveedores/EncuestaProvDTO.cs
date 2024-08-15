using Core.Entity.Encuestas;
using Core.Entity.Principal.Usuarios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Encuestas.Proveedores
{
    public class EncuestaProvDTO
    {
        public int id { get; set; }
        public int encuestaID { get; set; }
        public tblEN_EncuestaProveedores encuesta { get; set; }
        public int preguntaID { get; set; }
        public tblEN_PreguntasProveedores pregunta { get; set; }
        public int usuarioRespondioID { get; set; }
        public tblP_Usuario usuarioRespondio { get; set; }
        public int encuestaFolioID { get; set; }
        public decimal calificacion { get; set; }
        public DateTime fecha { get; set; }
        public int tipoEncuesta { get; set; }
        public string respuesta { get; set; }
        public decimal? porcentaje { get; set; }
        public EncuestaProvDetDTO detalle { get; set; }
        public string nombreEvaluador { get; set; }
    }

    public class EncuestaProvDetDTO{
        public int id { get; set; }
        public string centroCostos { get; set; }
        public string nombreProveedor { get; set; }
        public string comentarios { get; set; }
        public int evaluadorID { get; set; }
        public int numProveedor { get; set; }
        public decimal? calificacion { get; set; }
        public string tipoMoneda { get; set; }
    }

    public class EncuestaConDetalleDTO
    {
        public List<EncuestaProvDTO> EncuestaProvDTO { get; set; }
        public List<EncuestaProvDetDTO> EncuestaProvDetDTO { get; set; }

        public EncuestaConDetalleDTO()
        {
            EncuestaProvDTO = new List<EncuestaProvDTO>();
            EncuestaProvDetDTO = new List<EncuestaProvDetDTO>();
        }
    }
}