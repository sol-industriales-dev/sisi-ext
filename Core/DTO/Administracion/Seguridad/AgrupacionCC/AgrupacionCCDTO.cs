using Core.Entity.Administrativo.Seguridad.Indicadores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Administracion.Seguridad.AgrupacionCC
{
    public class AgrupacionCCDTO
    {
        public string departamento { get; set; }
        public string descripcion { get; set; }
        public int idDet { get; set; }
        public int id { get; set; }
        public string nomAgrupacion { get; set; }
        public string cc { get; set; }
        public bool esActivo { get; set; }

        public List<AgrupacionCCDet> lstDatos { get; set; }

        public string Mensaje { get; set; }
        public bool Exitoso { get; set; }

    }
}
