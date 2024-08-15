using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Maquinaria._Caratulas
{
    public class GuardarCaratulaDTO
    {
        public int id { get; set; }
        public string grupo { get; set; }
        public string modelo { get; set; }
        public decimal depreciacionDLLS { get; set; }
        public decimal depreciacionMXN { get; set; }
        public decimal inversionDLLS { get; set; }
        public decimal inversionMXN { get; set; }
        public decimal seguroDLLS { get; set; }
        public decimal seguroMXN { get; set; }
        public decimal filtrosDLLS { get; set; }
        public decimal filtrosMXN { get; set; }
        public decimal mantenimientoDLLS { get; set; }
        public decimal mantenimientoMXN { get; set; }
        public decimal manoObraDLLS { get; set; }
        public decimal manoObraMXN { get; set; }
        public decimal auxiliarDLLS { get; set; }
        public decimal auxiliarMXN { get; set; }
        public decimal indirectosDLLS { get; set; }
        public decimal indirectosMXN { get; set; }
        public decimal depreciacionOHDLLS { get; set; }
        public decimal depreciacionOHMXN { get; set; }
        public decimal aceiteDLLS { get; set; }
        public decimal aceiteMXN { get; set; }
        public decimal carilleriaDLLS { get; set; }
        public decimal carilleriaMXN { get; set; }
        public decimal ansulDLLS { get; set; }
        public decimal ansulMXN { get; set; }
        public decimal utilidadDLLS { get; set; }
        public decimal utilidadMXN { get; set; }
        public decimal costoDLLS { get; set; }
        public decimal costoMXN { get; set; }
        public decimal tipoCambio { get; set; }
        public int idusuarioTecnico { get; set; }        
        public int idusuarioServicio { get; set; }        
        public int idusuarioConstruccion { get; set; }       
        public decimal porcentajeAuxiliar { get; set; }
        public decimal porcentajeIndirectos { get; set; }
        public int caratula { get; set; }
        public int tipoHoraDia { get; set; }
        public string usuario { get; set; }
        public string usuarioServicio { get; set; }
        public string usuarioConstruccion { get; set; }


        public int idGrupo { get; set; }
        public int idModelo { get; set; }
    }
}
