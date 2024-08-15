using Core.DAO.Maquinaria.Catalogos;
using Core.DTO.Maquinaria.Captura;
using Core.Entity.Maquinaria.Catalogo;
using Core.Enum.Maquinaria;
using Core.Enum.Maquinaria.Reportes;
using Core.Enum.Principal.Bitacoras;
using Data.EntityFramework.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.DAO.Maquinaria.Catalogos
{
    public class DocumentosMaquinariaDAO : GenericDAO<tblM_DocumentosMaquinaria>, IdocumentosMaquinariaDAO
    {

        public void Guardar(tblM_DocumentosMaquinaria obj)
        {

            if (obj.id == 0)
                SaveEntity(obj, (int)BitacoraEnum.DOCUMENTOSMAQUINARIA);
            else
                Update(obj, obj.id, (int)BitacoraEnum.DOCUMENTOSMAQUINARIA);
        }

        public void actualizarArchivoEconomico(tblM_DocumentosMaquinaria obj, int idDocumento)
        {
            var documentoAnterior = _context.tblM_DocumentosMaquinaria.FirstOrDefault(x => x.id == idDocumento);

            if (documentoAnterior != null)
            {
                Delete(documentoAnterior, (int)BitacoraEnum.DOCUMENTOSMAQUINARIA);
                SaveEntity(obj, (int)BitacoraEnum.DOCUMENTOSMAQUINARIA);
            }
            else
            {
                throw new Exception("No se encuentra la información del documento anterior.");
            }
        }

        public tblM_DocumentosMaquinaria getDocumentosByID(int documentosID)
        {
            return _context.tblM_DocumentosMaquinaria.FirstOrDefault(x => x.id == documentosID);
        }

        public List<tblM_DocumentosMaquinaria> listaDocumentos(int economicoID)
        {
            return _context.tblM_DocumentosMaquinaria.Where(x => x.economicoID == economicoID).ToList(); ;
        }


        public DocumentosAnexosExistenDTO getByEconomico(int economico)
        {
            var rawData = _context.tblM_DocumentosMaquinaria.Where(x => x.economicoID == economico);

            DocumentosAnexosExistenDTO DocumentosAnexosExistenDTO = new DocumentosAnexosExistenDTO();


            DocumentosAnexosExistenDTO.certificacion = false;
            DocumentosAnexosExistenDTO.factura = false;
            DocumentosAnexosExistenDTO.pedimento = false;
            DocumentosAnexosExistenDTO.permisoCarga = false;
            DocumentosAnexosExistenDTO.poliza = false;
            DocumentosAnexosExistenDTO.tarjetaCirculacion = false;

            foreach (var item in rawData)
            {

                switch ((AnexosArchivosMaquinariaEnum)item.tipoArchivo)
                {
                    case AnexosArchivosMaquinariaEnum.FACTURA:
                        {
                            DocumentosAnexosExistenDTO.factura = true;
                            DocumentosAnexosExistenDTO.facturaID = item.id;
                        }
                        break;
                    case AnexosArchivosMaquinariaEnum.PEDIMENTO:
                        {
                            DocumentosAnexosExistenDTO.pedimento = true;
                            DocumentosAnexosExistenDTO.pedimentoID = item.id;
                        }
                        break;
                    case AnexosArchivosMaquinariaEnum.CERTIFICACION:
                        {
                            DocumentosAnexosExistenDTO.certificacion = true;
                            DocumentosAnexosExistenDTO.certificacionID = item.id;
                        }
                        break;
                    case AnexosArchivosMaquinariaEnum.PERMISO_ESPECIAL_DE_CARGA:
                        {
                            DocumentosAnexosExistenDTO.permisoCarga = true;
                            DocumentosAnexosExistenDTO.permisoCargaID = item.id;
                        }
                        break;
                    case AnexosArchivosMaquinariaEnum.POILIZA_DE_SEGUROS:
                        {
                            DocumentosAnexosExistenDTO.poliza = true;
                            DocumentosAnexosExistenDTO.polizaID = item.id;
                        }
                        break;

                    case AnexosArchivosMaquinariaEnum.TARJETA_DE_CIRCULACION:
                        {
                            DocumentosAnexosExistenDTO.tarjetaCirculacion = true;
                            DocumentosAnexosExistenDTO.tarjetaCirculacionID = item.id;
                        }
                        break;

                    default:
                        break;
                }


            }

            return DocumentosAnexosExistenDTO;
        }

        public Dictionary<string, object> eliminarArchivoEconomico(int idDocumento)
        {
            var resultado = new Dictionary<string, object>();

            using (var dbContextTransaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var registroDocumento = _context.tblM_DocumentosMaquinaria.FirstOrDefault(x => x.id == idDocumento);

                    if (registroDocumento != null)
                    {
                        Delete(registroDocumento, (int)BitacoraEnum.DOCUMENTOSMAQUINARIA);
                    }
                    else
                    {
                        throw new Exception("No se encuentra la información del documento.");
                    }

                    dbContextTransaction.Commit();
                    resultado.Add(SUCCESS, true);
                }
                catch (Exception e)
                {
                    dbContextTransaction.Rollback();
                    resultado.Add(SUCCESS, false);
                    resultado.Add(MESSAGE, e.Message);
                    LogError(0, 0, "CatInventarioController", "eliminarArchivoEconomico", e, AccionEnum.CONSULTA, 0, null);
                }
            }

            return resultado;
        }
    }
}
