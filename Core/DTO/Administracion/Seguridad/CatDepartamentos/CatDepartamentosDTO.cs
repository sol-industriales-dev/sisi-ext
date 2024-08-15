using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Administracion.Seguridad.CatDepartamentos
{
    public class CatDepartamentosDTO
    {
        public int id { get; set; }
        public int clave_depto { get; set; }
        public string descripcion { get; set; }
        public string cc { get; set; }
        public int idAreaOperativa { get; set; }
        public string NombreAreaOperativa { get; set; }
        public int idEmpresa { get; set; }
        public bool esActivo { get; set; }

    }
}
