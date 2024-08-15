using Core.DAO.Maquinaria.Catalogos;
using Core.DTO.Maquinaria.Captura;
using Core.Entity.Maquinaria.Catalogo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Service.Maquinaria.Catalogos
{
    public class DocumentosMaquinariaService : IdocumentosMaquinariaDAO
    {
        #region Atributos
        private IdocumentosMaquinariaDAO m_interfazDAO;
        #endregion Atributos

        #region Propiedades
        private IdocumentosMaquinariaDAO interfazDAO
        {
            get { return m_interfazDAO; }
            set { m_interfazDAO = value; }
        }
        #endregion Propiedades

        #region Constructores
        public DocumentosMaquinariaService(IdocumentosMaquinariaDAO InterfazDAO)
        {
            this.interfazDAO = InterfazDAO;
        }
        #endregion Constructores
        public void Guardar(tblM_DocumentosMaquinaria obj)
        {
            interfazDAO.Guardar(obj);
        }

        public tblM_DocumentosMaquinaria getDocumentosByID(int documentosID)
        {
            return interfazDAO.getDocumentosByID(documentosID);
        }

        public List<tblM_DocumentosMaquinaria> listaDocumentos(int economicoID)
        {
            return interfazDAO.listaDocumentos(economicoID);

        }
        public DocumentosAnexosExistenDTO getByEconomico(int economico)
        {
            return interfazDAO.getByEconomico(economico);
        }

        public void actualizarArchivoEconomico(tblM_DocumentosMaquinaria obj, int idDocumento)
        {
            interfazDAO.actualizarArchivoEconomico(obj, idDocumento);
        }

        public Dictionary<string, object> eliminarArchivoEconomico(int idDocumento)
        {
            return interfazDAO.eliminarArchivoEconomico(idDocumento);
        }
    }
}
