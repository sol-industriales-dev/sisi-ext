using Core.Entity.Enkontrol.Compras;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Core.DAO.Enkontrol.Compras
{
    public interface IGestionProveedorDAO
    {
        Dictionary<string, object> getProveedores();
        Dictionary<string, object> getDatosProveedores(int id);
        Dictionary<string, object> GuardarEditarProveedor(tblCom_MAEPROV proveedor, HttpPostedFileBase objFile);
        Dictionary<string, object> GuardadoCajaChica(tblCom_MAEPROV proveedor, HttpPostedFileBase objFile);
        
        Dictionary<string, object> AutorizarProveedor(int id);
        Dictionary<string, object> eliminarProveedor(int id);

        Dictionary<string, object> getCuentasBancosProveedores(string anexo);
        Dictionary<string, object> getDatosCuentasBancoProveedor(int id);
        Dictionary<string, object> guardarEditarCuentaBancoProveedor(tblCom_CuentasBancosProveedor CuentasBancosProveedor);
        Dictionary<string, object> eliminarCuentaBancoProveedor(int id);
        Dictionary<string, object> NotificarAltaProveedor(int id);

        #region ARCHIVOS
        Dictionary<string, object> RemoveArchivos(int idArchivo);
        Tuple<Stream, string> descargarArchivo(int idArchivo);
        
        #endregion
    }
}
