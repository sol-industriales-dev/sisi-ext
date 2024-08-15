using Core.DTO.Maquinaria.Captura.conciliacion;
using Core.DTO.Principal.Generales;
using Core.Entity.Maquinaria.Captura;
using Core.Entity.Maquinaria.Catalogo.Cararatulas;
using Core.Entity.Principal.Multiempresa;
using Core.Entity.Principal.Usuarios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DAO.Maquinaria
{
    public interface IConciliacionDAO
    {
         tblP_Autoriza getAuth(int perfil, string ac);
        string GetNameObra(int id);
        void setCorreo(int conciliacionid);
        void setReenviarCorreo(int conciliacionid);
        List<tblM_CapConciliacionHorometros> getConciliaciones(int id);
        List<tblAutorizaConciliacionDTO> getAutorizaciones(int CCID, int fechaID, DateTime fechaInio, DateTime fechaFin, int estatus, bool esQuincena);
        bool GuardarCaratula(tblM_EncCaratula enc, List<tblM_CapCaratula> lst, List<tblM_EncCaratula_Concideracion> lstCon);
        int getConciliacionesExiste(int fechaID, int centroCostosID,DateTime fechaInicio,DateTime fechaFinal);
        bool saveOrUpdateConciliacion(List<tblM_CapConciliacionHorometros> obj, tblM_CapEncConciliacionHorometros objDet);
        tblM_EncCaratula getEncabezado(int ccID);
        tblM_EncCaratula getEncCartulaFromIdCaratula(int idEnc);
        List<tblM_CapCaratula> getLstPrecios(int idEnc);
        int getMonedaCaratula(int ccID);
        List<tblM_CapCaratula> getNewLstPrecios(string cc);
        List<tblM_CatConsideracionCostoHora> getLstFullConsiceracion();
        int getLengthConsideraciones();
        List<tblM_EncCaratula_Concideracion> getLstConsiceracionWhereEnc(int enc);
        List<ComboDTO> fillCboCentrosCosto(int usuarioID);
        dynamic getCboGrupo();
        dynamic getCboModelo(int idGrupo);
        dynamic getCboCC();
        List<ConciliacionHorometrosDTO> getTblConciliacion(tblM_EncCaratula enc, DateTime fechaInicio, DateTime fechaFinal);
        tblM_AutorizaConciliacionHorometros loadAutorizacion(int validaID);
        tblM_AutorizaConciliacionHorometros loadAutorizacionFromConciliacacionId(int conciliacionId);
        tblM_CapEncConciliacionHorometros getCapEncConciliacion(int id);
        bool sendValidacion(int conciliacionID, int respuesta, int idUsuario, string comentario);

        List<tblAutorizaConciliacionDTO> getAutorizacionesPendientes(int CCID);

        List<tblM_AutorizacionCaratulaPreciosU> loadTlbAutorizacionesCaratula(int cc, int estatus);

        tblAutorizaCaratulaDTO loadAutorizacionCaratula(int objID);

        List<caratulaPreciosDTO> getCaratulaByID(int idEnc);

        bool autorizacionUsuario(int obj, int Autoriza, int tipo, string comentario);

        bool getModeloExiste(int p, string CentroCostos);
        tblP_CC getCC(int ccID);

        #region Facturado
        List<ConciliacionFacturadoDTO> getConciliacionesAFacturar(bool estado, string folio, int cc, DateTime fechaInicio, DateTime fechaFin);
        bool indicarFacturacion(int conciliacionID, List<string> factura);
        List<string> getFacturasConciliacion(int conciliacionID);
        #endregion

    }
}
