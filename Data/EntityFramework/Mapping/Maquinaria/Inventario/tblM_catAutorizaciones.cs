using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.DAO.Maquinaria.Inventario
{
    public class tblM_catAutorizaciones
    {
        public int id { get; set; }
        public int usuarioGerenteID { get; set; }
        public int usuarioGerenteAreaID { get; set; }
        public int usuarioDirectorDivisionID { get; set; }
        public int usuarioAltaDireccionID { get; set; }

        public bool autorizaGerente { get; set; }
        public bool autorizaGerenteArea { get; set; }
        public bool autorizaDirectorDivision { get; set; }
        public bool autorizaAltaDireccion { get; set; }
        public string areaCuenta { get; set; }
        public bool estatus { get; set; }
    }
}
