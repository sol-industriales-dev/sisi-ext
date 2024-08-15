using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Enkontrol.Requisicion
{
    public class ProveedorLinkDTO
    {
        #region SQL
        public int id { get; set; }
        public string cc { get; set; }
        public Int64 idProveedor { get; set; }
        public int numRequisicion { get; set; }
        public string hash { get; set; }
        public string link { get; set; }
        public int idEmpresa { get; set; }
        public int idProveedorCreacion { get; set; }
        public int idProveedorModificacion { get; set; }
        public DateTime fechaCreacionProveedor { get; set; }
        public DateTime fechaModificacionProveedor { get; set; }
        public int idEstatusRegistro { get; set; }
        public int idUsuarioCreacion { get; set; }
        public int idUsuarioModificacion { get; set; }
        public bool esEnvioCorreoExterno { get; set; }
        public bool esEnvioCorreoSIGOPLAN { get; set; }
        public DateTime fechaCreacion { get; set; }
        public DateTime fechaModificacion { get; set; }
        public bool registroActivo { get; set; }
        #endregion

        #region ADICIONAL
        public string proveedor { get; set; }
        public string estatusRegistro { get; set; }
        public string nombre { get; set; }
        public int numpro { get; set; }
        public List<string> lstCorreos { get; set; }
        public string envioCorreo { get; set; }
        public string descripcion { get; set; }
        #endregion
    }
}