using Core.DTO.Enkontrol.OrdenCompra;
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
    public interface IAltaProveedorDAO
    {
        Dictionary<string, object> getProveedores();
        Dictionary<string, object> obtenerDatosProveedores(int id, int numpro);
        Dictionary<string, object> GuardarProveedor(tblCom_sp_proveedoresDTO objProveedor, HttpPostedFileBase objFile);//guardar editar bd construplan
        Dictionary<string, object> GuardarEditarProveedorColombia(tblCom_sp_proveedoresColombiaDTO objProveedor, HttpPostedFileBase objFile); //guardar editar bd colombia / kontrol 
        Dictionary<string, object> eliminarProveedor(int id);
        Dictionary<string, object> AutorizarProveedor(int id);
        Dictionary<string, object> GetArchivosAdjuntos(int idArchivo);
        Dictionary<string, object> VisualizarArchivoAdjunto(int idArchivo);
        Dictionary<string, object> NotificarAltaProveedor(int id);
        Dictionary<string, object> EliminarArchivoAdjunto(int idArchivo);
        Tuple<Stream, string> descargarArchivo(int idArchivo);

        #region GENERALES
        int GetLastProveedor(int tipoProveedor);
        #endregion

        #region Catalogos
        List<Core.DTO.Principal.Generales.ComboDTO> FillComboCiudad();
        List<Core.DTO.Principal.Generales.ComboDTO> FillComboTipoProveedor();
        List<Core.DTO.Principal.Generales.ComboDTO> FillComboTipoTercero();
        List<Core.DTO.Principal.Generales.ComboDTO> FillComboTipoOperacion();
        List<Core.DTO.Principal.Generales.ComboDTO> FillComboTipoPagoTerceroTrans();
        List<Core.DTO.Principal.Generales.ComboDTO> FillComboTipoMovBase();
        List<Core.DTO.Principal.Generales.ComboDTO> FillComboTipoMoneda();
        List<Core.DTO.Principal.Generales.ComboDTO> FillComboBancos();
        List<Core.DTO.Principal.Generales.ComboDTO> FillComboTipoRegimen();

        

        #endregion
    }
}
