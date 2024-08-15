using Core.DAO.Enkontrol.Compras;
using Core.Entity.Enkontrol.Compras;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Core.Service.Enkontrol.Compras
{
    public class GestionProveedorService : IGestionProveedorDAO
    {
                #region CONSTRUCTOR
        public IGestionProveedorDAO _iGestionProveedorDAO;
        public IGestionProveedorDAO iGestionProveedorDAO
        {
            get { return _iGestionProveedorDAO; }
            set { _iGestionProveedorDAO = value; }
        }
        public GestionProveedorService(IGestionProveedorDAO iGestionProveedorDAO)
        {
            this.iGestionProveedorDAO = iGestionProveedorDAO;
        }
        #endregion

        public Dictionary<string, object> getProveedores()
        {
            return _iGestionProveedorDAO.getProveedores();
        }

        public Dictionary<string, object> getDatosProveedores(int id)
        {
            return _iGestionProveedorDAO.getDatosProveedores(id);
        }
        public Dictionary<string, object> GuardarEditarProveedor(tblCom_MAEPROV proveedor, HttpPostedFileBase objFile)
        {
            return _iGestionProveedorDAO.GuardarEditarProveedor(proveedor, objFile);
        }
        public Dictionary<string, object> GuardadoCajaChica(tblCom_MAEPROV proveedor, HttpPostedFileBase objFile)
        {
            return _iGestionProveedorDAO.GuardadoCajaChica(proveedor, objFile);
        }        

        public Dictionary<string, object> AutorizarProveedor(int id)
        {
            return _iGestionProveedorDAO.AutorizarProveedor(id);
        }

        public Dictionary<string, object> eliminarProveedor(int id)
        {
            return _iGestionProveedorDAO.eliminarProveedor(id);
        }

        public Dictionary<string, object> getCuentasBancosProveedores(string anexo)
        {
            return _iGestionProveedorDAO.getCuentasBancosProveedores(anexo);
        }

        public Dictionary<string, object> getDatosCuentasBancoProveedor(int id)
        {
            return _iGestionProveedorDAO.getDatosCuentasBancoProveedor(id);
        }
        public Dictionary<string, object> guardarEditarCuentaBancoProveedor(tblCom_CuentasBancosProveedor CuentasBancosProveedor)
        {
            return _iGestionProveedorDAO.guardarEditarCuentaBancoProveedor(CuentasBancosProveedor);
        }
        public Dictionary<string, object> eliminarCuentaBancoProveedor(int id)
        {
            return _iGestionProveedorDAO.eliminarCuentaBancoProveedor(id);
        }
        public Dictionary<string, object> NotificarAltaProveedor(int id)
        {
            return _iGestionProveedorDAO.NotificarAltaProveedor(id);
        }

        #region ARCHIVOS
        public Dictionary<string, object> RemoveArchivos(int idArchivo)
        {
            return _iGestionProveedorDAO.RemoveArchivos(idArchivo);
        }

        public Tuple<Stream, string> descargarArchivo(int idArchivo)
        {
            return _iGestionProveedorDAO.descargarArchivo(idArchivo);
        }

        #endregion
    }
}
