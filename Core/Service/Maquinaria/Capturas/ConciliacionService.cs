using Core.DAO.Maquinaria;
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

namespace Core.Service.Maquinaria.Capturas
{
    public class ConciliacionService : IConciliacionDAO
    {
        #region Atributos
        private IConciliacionDAO m_ConciliacionDAO;
        #endregion
        #region Propiedades
        public IConciliacionDAO ConciliacionDAO
        {
            get { return m_ConciliacionDAO; }
            set { m_ConciliacionDAO = value; }
        }
        #endregion
        #region Constructor

        public ConciliacionService(IConciliacionDAO conciliacionDAO)
        {
            this.ConciliacionDAO = conciliacionDAO;
        }
        #endregion
        public bool GuardarCaratula(tblM_EncCaratula enc, List<tblM_CapCaratula> lst, List<tblM_EncCaratula_Concideracion> lstCon)
        {
            return this.ConciliacionDAO.GuardarCaratula(enc, lst, lstCon);
        }
        public tblM_EncCaratula getEncabezado(int ccID)
        {
            return this.ConciliacionDAO.getEncabezado(ccID);
        }
        public tblM_EncCaratula getEncCartulaFromIdCaratula(int idEnc)
        {
            return ConciliacionDAO.getEncCartulaFromIdCaratula(idEnc);
        }
        public List<tblM_CapCaratula> getLstPrecios(int idEnc)
        {
            return this.ConciliacionDAO.getLstPrecios(idEnc);
        }
        public int getMonedaCaratula(int ccID)
        {
            return ConciliacionDAO.getMonedaCaratula(ccID);
        }
        public List<tblM_CapCaratula> getNewLstPrecios(string cc)
        {
            return this.ConciliacionDAO.getNewLstPrecios(cc);
        }
        public bool saveOrUpdateConciliacion(List<tblM_CapConciliacionHorometros> obj, tblM_CapEncConciliacionHorometros objDet)
        {
            return this.ConciliacionDAO.saveOrUpdateConciliacion(obj, objDet);
        }
        public List<ComboDTO> fillCboCentrosCosto(int usuarioID)
        {
            return this.ConciliacionDAO.fillCboCentrosCosto(usuarioID);
        }
        public List<tblM_CatConsideracionCostoHora> getLstFullConsiceracion()
        {
            return this.ConciliacionDAO.getLstFullConsiceracion();
        }
        public int getLengthConsideraciones()
        {
            return this.ConciliacionDAO.getLengthConsideraciones();
        }
        public List<tblM_EncCaratula_Concideracion> getLstConsiceracionWhereEnc(int enc)
        {
            return this.ConciliacionDAO.getLstConsiceracionWhereEnc(enc);
        }
        public dynamic getCboGrupo()
        {
            return this.ConciliacionDAO.getCboGrupo();
        }
        public dynamic getCboModelo(int idGrupo)
        {
            return this.ConciliacionDAO.getCboModelo(idGrupo);
        }
        public dynamic getCboCC()
        {
            return this.ConciliacionDAO.getCboCC();
        }
        public List<ConciliacionHorometrosDTO> getTblConciliacion(tblM_EncCaratula enc, DateTime fechaInicio, DateTime fechaFinal)
        {
            return this.ConciliacionDAO.getTblConciliacion(enc, fechaInicio, fechaFinal);
        }
        public tblM_AutorizaConciliacionHorometros loadAutorizacion(int validaID)
        {
            return ConciliacionDAO.loadAutorizacion(validaID);
        }
        public tblM_AutorizaConciliacionHorometros loadAutorizacionFromConciliacacionId(int conciliacionId)
        {
            return ConciliacionDAO.loadAutorizacionFromConciliacacionId(conciliacionId);
        }
        public int getConciliacionesExiste(int fechaID, int centroCostosID, DateTime fechaInicio, DateTime fechaFinal)
        {
            return this.ConciliacionDAO.getConciliacionesExiste(fechaID, centroCostosID, fechaInicio, fechaFinal);
        }
        public List<tblAutorizaConciliacionDTO> getAutorizaciones(int CCID, int fechaID, DateTime fechaInio, DateTime fechaFin, int estatus, bool esQuincena)
        {
            return this.ConciliacionDAO.getAutorizaciones(CCID, fechaID, fechaInio, fechaFin, estatus, esQuincena);
        }
        public List<tblM_CapConciliacionHorometros> getConciliaciones(int id)
        {
            return this.ConciliacionDAO.getConciliaciones(id);
        }
        public tblM_CapEncConciliacionHorometros getCapEncConciliacion(int id)
        {
            return this.ConciliacionDAO.getCapEncConciliacion(id);
        }
        public bool sendValidacion(int conciliacionID, int respuesta, int idUsuario, string comentario)
        {
            return ConciliacionDAO.sendValidacion(conciliacionID, respuesta, idUsuario, comentario);
        }
        public void setCorreo(int conciliacionID)
        {
            ConciliacionDAO.setCorreo(conciliacionID);
        }
        public void setReenviarCorreo(int conciliacionid)
        {
            ConciliacionDAO.setReenviarCorreo(conciliacionid);
        }
        public List<tblAutorizaConciliacionDTO> getAutorizacionesPendientes(int CCID)
        {
            return ConciliacionDAO.getAutorizacionesPendientes(CCID);
        }

        public List<tblM_AutorizacionCaratulaPreciosU> loadTlbAutorizacionesCaratula(int cc, int estatus)
        {
            return ConciliacionDAO.loadTlbAutorizacionesCaratula(cc, estatus);
        }

        public tblAutorizaCaratulaDTO loadAutorizacionCaratula(int objID)
        {
            return ConciliacionDAO.loadAutorizacionCaratula(objID);
        }

        public string GetNameObra(int id)
        {
            return ConciliacionDAO.GetNameObra(id);
        }

        public List<caratulaPreciosDTO> getCaratulaByID(int idEnc)
        {
            return ConciliacionDAO.getCaratulaByID(idEnc);
        }
        public bool autorizacionUsuario(int obj, int Autoriza, int tipo, string comentario)
        {
            return ConciliacionDAO.autorizacionUsuario(obj, Autoriza, tipo, comentario);
        }

        public bool getModeloExiste(int obj, string Autoriza)
        {
            return ConciliacionDAO.getModeloExiste(obj, Autoriza);
        }

        public tblP_Autoriza getAuth(int perfil, string ac)
        {
            return ConciliacionDAO.getAuth(perfil, ac);
        }

        public tblP_CC getCC(int ccID)
        {
            return ConciliacionDAO.getCC(ccID);
        }
        #region Facturado
        public List<ConciliacionFacturadoDTO> getConciliacionesAFacturar(bool estado, string folio, int cc, DateTime fechaInicio, DateTime fechaFin)
        {
            return ConciliacionDAO.getConciliacionesAFacturar(estado, folio, cc, fechaInicio, fechaFin);
        }
        public bool indicarFacturacion(int conciliacionID, List<string> factura)
        {
            return ConciliacionDAO.indicarFacturacion(conciliacionID, factura);
        }
        public List<string> getFacturasConciliacion(int conciliacionID)
        {
            return ConciliacionDAO.getFacturasConciliacion(conciliacionID);
        }
        #endregion
    }
}
