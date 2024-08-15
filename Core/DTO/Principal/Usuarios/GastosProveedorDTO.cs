using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Principal.Usuarios
{
    public class GastosProveedorDTO
    {
        public int numpro { get; set; }
        public string proveedorDesc { get; set; }
        public long cfd_folio { get; set; }
        public string referenciaoc { get; set; }
        public string cc { get; set; }
        public string ccDesc { get; set; }
        public string areaCuenta { get; set; }
        public string factura { get; set; }
        public decimal total { get; set; }
        public string estatus { get; set; }
        public DateTime fecha { get; set; }
        public string fechaString { get; set; }
        public int? nivel_aut { get; set; }
        public bool cerrado { get; set; }
    }
}
