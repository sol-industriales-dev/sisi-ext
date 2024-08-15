using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Administracion.DocumentosXPagar
{
    public class CedulaMensualDetalleDTO
    {
        public int tipoMoneda { get; set; }
        public string descripcionMoneda { get; set; }
        public List<CedulaMensualDetalleInstitucionesDTO> detalles { get; set; }

        public CedulaMensualDetalleDTO()
        {
            detalles = new List<CedulaMensualDetalleInstitucionesDTO>();
        }
    }
}
