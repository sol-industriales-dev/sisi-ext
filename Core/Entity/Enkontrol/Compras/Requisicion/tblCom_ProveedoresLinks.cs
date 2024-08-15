using Core.Enum.Enkontrol.Requisicion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Enkontrol.Compras.Requisicion
{
    public class tblCom_ProveedoresLinks
    {
        public int id { get; set; }
        public string cc { get; set; }
        public Int64 idProveedor { get; set; }
        public int numRequisicion { get; set; }
        public string hash { get; set; }
        public string link { get; set; }
        public int idEmpresa { get; set; }
        public int idProveedorCreacion { get; set; }
        public int idProveedorModificacion { get; set; }
        public DateTime? fechaCreacionProveedor { get; set; }
        public DateTime? fechaModificacionProveedor { get; set; }
        public EstatusRegistroProveedorLinkEnum idEstatusRegistro { get; set; }
        public bool esEnvioCorreoExterno { get; set; }
        public bool esEnvioCorreoSIGOPLAN { get; set; }
        public DateTime? fechaEnvioCorreo { get; set; }
        public int idUsuarioCreacion { get; set; }
        public int idUsuarioModificacion { get; set; }
        public DateTime fechaCreacion { get; set; }
        public DateTime? fechaModificacion { get; set; }
        public bool registroActivo { get; set; }
    }
}
