using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.RecursosHumanos
{
    public class FiniquitoDTO
    {
        public int id { get; set; }
        public int claveEmpleado { get; set; }
        public string nombre { get; set; }
        public string ape_paterno { get; set; }
        public string ape_materno { get; set; }
        public string nombreCompleto { get; set; }
        public DateTime fechaAlta { get; set; }
        public DateTime? fechaBaja { get; set; }
        public int puestoID { get; set; }
        public string puesto { get; set; }
        public int tipoNominaID { get; set; }
        public string tipoNomina { get; set; }
        public string ccID { get; set; }
        public string cc { get; set; }
        public decimal salarioBase { get; set; }
        public decimal complemento { get; set; }
        public decimal bono { get; set; }
        public int formuloID { get; set; }
        public string formuloNombre { get; set; }
        public int voboID { get; set; }
        public string voboNombre { get; set; }
        public int autorizoID { get; set; }
        public string autorizoNombre { get; set; }
        public decimal total { get; set; }
        public int autorizado { get; set; }
        public List<FiniquitoDetalleDTO> detalle { get; set; }
    }
}
