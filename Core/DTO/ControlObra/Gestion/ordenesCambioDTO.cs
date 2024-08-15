using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.ControlObra.Gestion
{
    public class ordenesCambioDTO
    {
        public int id { get; set; }
        public DateTime fechaEfectiva { get; set; }
        public string Proyecto { get; set; }
        public string CLiente { get; set; }
        public string Contratista { get; set; }
        public string Direccion { get; set; }
        public string NoOrden { get; set; }
        public bool esCobrable { get; set; }
        public string cc { get; set; }
        public string Antecedentes { get; set; }
        public int idSubContratista { get; set; }
        public int status { get; set; }
        public string tabla { get; set; }
        public string numeroDeContrato { get; set; }
        public decimal totalDeMontos { get; set; }
        public string ubicacionProyecto { get; set; }
        public DateTime fechaSuscripcion { get; set; }
        public DateTime fechaExpiracion { get; set; }
        public int idContrato { get; set; }
        public string otrasCondicioes { get; set; }
        public int diasDeContrato { get; set; }
        public int sumaTotalDeDias { get; set; }
        public string representanteLegal { get; set; }
        public bool mostrarFirmas { get; set; }
    }
}
