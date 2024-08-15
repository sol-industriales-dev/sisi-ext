using Core.Enum.Administracion.Seguridad.ActoCondicion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Administracion.Seguridad.ActoCondicion
{
    public class InfoInfraccionDTO
    {
        public bool estatus { get; set; }
        public string mensaje { get; set; }

        public string descripcion { get; set; }
        public int nivelInfraccion { get; set; }
        public int nivelInfraccionAcumulado { get; set; }
        public int numeroFalta { get; set; }

    }
}
