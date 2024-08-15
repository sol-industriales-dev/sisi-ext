using Core.Entity.Administrativo.Contabilidad;
using Core.Enum.Administracion.CadenaProductiva;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DAO.Contabilidad.Reportes
{
    public interface ICadenaPrincipalDAO
    {
        tblC_CadenaPrincipal Guardar(tblC_CadenaPrincipal obj);
        List<tblC_CadenaPrincipal> GetDocumentosGuardados();
        List<tblC_CadenaPrincipal> GetDocumentosAplicados();
        List<tblC_CadenaPrincipal> GetAllDocumentos();
        tblC_CadenaPrincipal GetDocumento(int id);
        bool Eliminar(tblC_CadenaPrincipal obj);

        /// <summary>
        /// Verifica si el usuario actual tiene permiso de vista para dar VoBo a cadenas principales.
        /// </summary>
        /// <returns></returns>
        bool TienePermisoVoBo();

        /// <summary>
        /// Actualiza estadoAutorizacion de una cadena principal al asignar el VoBo (estadoAutorizacion = 1).
        /// </summary>
        /// <param name="cadenaID"></param>
        /// <returns></returns>
        Dictionary<string, object> AsignarVoBoCadena(int cadenaID);

        /// <summary>
        /// Obtiene el estado de autorización de una cadena principal.
        /// </summary>
        /// <param name="cadenaID"></param>
        /// <returns></returns>
        EstadoAutorizacionCadenaEnum ObtenerEstadoAutorizacionCadena(int cadenaID);

        /// <summary>
        /// Obtiene las cadenas principales que ya tienen el vobo aplicado y están pendientes por autorizar.
        /// </summary>
        /// <returns></returns>
        Dictionary<string, object> GetDocumentosPorAutorizar();

        /// <summary>
        /// Autoriza una lista de documentos (estadoAutorizacion = 2)
        /// </summary>
        /// <param name="idsDocumentosPorAutorizar"></param>
        /// <returns></returns>
        Dictionary<string, object> AutorizarDocumentos(List<int> idsDocumentosPorAutorizar);

        /// <summary>
        /// Rechaza una cadena principal e indica un comentario de rechazo.
        /// </summary>
        /// <param name="cadenaID"></param>
        /// <param name="comentarioRechazo"></param>
        /// <returns></returns>
        Dictionary<string, object> RechazarDocumento(int cadenaID, string comentarioRechazo);

        /// <summary>
        /// Obtiene el listado de facturas (desde SIGOPLAN) de una cadena principal.
        /// </summary>
        /// <param name="cadenaID"></param>
        /// <returns></returns>
        Dictionary<string, object> ObtenerFacturasCadena(int cadenaID);

        /// <summary>
        /// Obtiene la ruta de descarga de un archivo PDF de una factura específica.
        /// </summary>
        /// <param name="numProveedor"></param>
        /// <param name="factura"></param>
        /// <param name="cc"></param>
        /// <returns></returns>
        Dictionary<string, object> ObtenerRutaPDFFactura(string numProveedor, string factura, string cc);

        /// <summary>
        /// Obtiene la ruta de descarga de un archivo XML de una factura específica.
        /// </summary>
        /// <param name="numProveedor"></param>
        /// <param name="factura"></param>
        /// <param name="cc"></param>
        /// <returns></returns>
        Dictionary<string, object> ObtenerRutaXMLFactura(string numProveedor, string factura, string cc);

        /// <summary>
        /// Obtiene la firma de autorización de una cadena en caso que esté autorizada.
        /// </summary>
        /// <param name="cadenaID"></param>
        /// <returns></returns>
        string ObtenerFirmaAutorizacionCadena(int cadenaID, int tipo);
    }
}
