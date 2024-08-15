using Core.DTO.Maquinaria.Captura;
using Core.Entity.Maquinaria.Catalogo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DAO.Maquinaria.Catalogos
{
    public interface IdocumentosMaquinariaDAO
    {
        void Guardar(tblM_DocumentosMaquinaria obj);

        tblM_DocumentosMaquinaria getDocumentosByID(int documentosID);

        List<tblM_DocumentosMaquinaria> listaDocumentos(int economicoID);

        DocumentosAnexosExistenDTO getByEconomico(int economico);

        void actualizarArchivoEconomico(tblM_DocumentosMaquinaria obj, int idDocumento);

        Dictionary<string, object> eliminarArchivoEconomico(int idDocumento);
    }
}
