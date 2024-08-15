using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.RecursosHumanos.ReportesRH
{
    public class ExpedicionesDTO
    {
        public int id { get; set; }
        public int cveEmpleado { get; set; }
        public string cc { get; set; }
        public int idReporte { get; set; }
        public int idArchivo { get; set; }
        public int idUsuario { get; set; }
        public string firmaElect { get; set; }
        public DateTime fechaCreacion { get; set; }
        public DateTime fechaModificacion { get; set; }
        public int idUsuarioCreacion { get; set; }
        public int idUsuarioModificacion { get; set; }
        public bool esActivo { get; set; }
        //JOIN
        public string nombreEmpleado { get; set; }
        public string nombreExpidio { get; set; }
        public string ccDesc { get; set; }
        public int idArchivoExp { get; set; }
    }
}
