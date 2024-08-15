using Core.DAO.Enkontrol.Compras;
using Core.DTO.Enkontrol.OrdenCompra;
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
    public class AltaProveedorService : IAltaProveedorDAO
    {
          #region Atributos
        private IAltaProveedorDAO m_iAltaProveedorDAO;
        #endregion Atributos
          #region Propiedades
        private IAltaProveedorDAO iAltaProveedorDAO
        {
            get { return m_iAltaProveedorDAO; }
            set { m_iAltaProveedorDAO = value; }
        }
        #endregion Propiedades
        #region Constructores
        public AltaProveedorService(IAltaProveedorDAO IAltaProveedorDAO)
        {
            this.iAltaProveedorDAO = IAltaProveedorDAO;
        }
        #endregion Constructores


        public Dictionary<string, object> getProveedores()
        {
            return m_iAltaProveedorDAO.getProveedores();
        }

        public Dictionary<string, object> obtenerDatosProveedores(int id, int numpro)
        {
            return m_iAltaProveedorDAO.obtenerDatosProveedores(id, numpro);
        }
        public Dictionary<string, object> GuardarProveedor(tblCom_sp_proveedoresDTO objProveedor, HttpPostedFileBase objFile)
        {
            return m_iAltaProveedorDAO.GuardarProveedor(objProveedor, objFile);
        }
        public Dictionary<string, object> GuardarEditarProveedorColombia(tblCom_sp_proveedoresColombiaDTO objProveedor, HttpPostedFileBase objFile)
        {
            return m_iAltaProveedorDAO.GuardarEditarProveedorColombia(objProveedor, objFile);
        }
        public Dictionary<string, object> AutorizarProveedor(int id)
        {
            return m_iAltaProveedorDAO.AutorizarProveedor(id);
        }
        public Dictionary<string, object> NotificarAltaProveedor(int id)
        {
            return m_iAltaProveedorDAO.NotificarAltaProveedor(id);
        }
        public Dictionary<string, object> eliminarProveedor(int id)
        {
            return m_iAltaProveedorDAO.eliminarProveedor(id);
        }
        public Dictionary<string, object> GetArchivosAdjuntos(int idArchivo)
        {
            return m_iAltaProveedorDAO.GetArchivosAdjuntos(idArchivo);
        }
        public Dictionary<string, object> VisualizarArchivoAdjunto(int idArchivo)
        {
            return m_iAltaProveedorDAO.VisualizarArchivoAdjunto(idArchivo);
        }
        public Dictionary<string, object> EliminarArchivoAdjunto(int idArchivo)
        {
            return m_iAltaProveedorDAO.EliminarArchivoAdjunto(idArchivo);
        }
        public Tuple<Stream, string> descargarArchivo(int idArchivo)
        {
            return m_iAltaProveedorDAO.descargarArchivo(idArchivo);
        }

        #region GENERALES
        public int GetLastProveedor(int tipoProveedor)
        {
            return m_iAltaProveedorDAO.GetLastProveedor(tipoProveedor);
        }
        #endregion

        #region FillCombos
        public List<Core.DTO.Principal.Generales.ComboDTO> FillComboCiudad()
        {
            return m_iAltaProveedorDAO.FillComboCiudad();
        }
        public List<Core.DTO.Principal.Generales.ComboDTO> FillComboTipoProveedor()
        {
            return m_iAltaProveedorDAO.FillComboTipoProveedor();
        }

        public List<Core.DTO.Principal.Generales.ComboDTO> FillComboTipoTercero()
        {
            return m_iAltaProveedorDAO.FillComboTipoTercero();
        }
        public List<Core.DTO.Principal.Generales.ComboDTO> FillComboTipoOperacion()
        {
            return m_iAltaProveedorDAO.FillComboTipoOperacion();
        }
        public List<Core.DTO.Principal.Generales.ComboDTO> FillComboTipoPagoTerceroTrans()
        {
            return m_iAltaProveedorDAO.FillComboTipoPagoTerceroTrans();
        }
        public List<Core.DTO.Principal.Generales.ComboDTO> FillComboTipoMovBase()
        {
            return m_iAltaProveedorDAO.FillComboTipoMovBase();
        }
        public List<Core.DTO.Principal.Generales.ComboDTO> FillComboTipoMoneda()
        {
            return m_iAltaProveedorDAO.FillComboTipoMoneda();
        }
        public List<Core.DTO.Principal.Generales.ComboDTO> FillComboBancos()
        {
            return m_iAltaProveedorDAO.FillComboBancos();
        }
        public List<Core.DTO.Principal.Generales.ComboDTO> FillComboTipoRegimen()
        {
            return m_iAltaProveedorDAO.FillComboTipoRegimen();
        }
        
        #endregion

    }
}
