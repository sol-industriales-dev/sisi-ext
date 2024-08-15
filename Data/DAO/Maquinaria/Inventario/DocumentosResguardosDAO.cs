using Core.DAO.Maquinaria.Inventario;
using Core.Entity.Maquinaria.Inventario;
using Core.Enum.Principal.Bitacoras;
using Data.EntityFramework.Generic;
using Infrastructure.Utils;
using System;
using System.IO;
using System.Linq;
using System.Web;

namespace Data.DAO.Maquinaria.Inventario
{
    public class DocumentosResguardosDAO : GenericDAO<tblM_DocumentosResguardos>, IDocumentosResguardosDAO
    {
        public void Guardar(tblM_DocumentosResguardos obj)
        {

            if (obj.id == 0)
                SaveEntity(obj, (int)BitacoraEnum.DocumentosResguardo);
            else
                Update(obj, obj.id, (int)BitacoraEnum.DocumentosResguardo);
        }

        //raguilar retorna ruta documento 
        public tblM_DocumentosResguardos GetObjRutaDocumentobyID(int objIdResguardo)
        {
            return _context.tblM_DocumentosResguardos.FirstOrDefault(x => x.ResguardoID.Equals(objIdResguardo) && x.tipoArchivo == 9);
        }

        public tblM_DocumentosResguardos GetObjRutaDocumentoByIDGeneral(int objIdResguardo)
        {
            return _context.tblM_DocumentosResguardos.FirstOrDefault(x => x.id == objIdResguardo);
        }

        public void ActualizarArchivosResguardo(int resguardoID, HttpPostedFileBase archivoLicencia, HttpPostedFileBase archivoPoliza, HttpPostedFileBase archivoCurso, string rutaBase)
        {
            if (archivoLicencia != null && archivoLicencia.ContentLength > 0)
            {
                var documentoLicencia = _context.tblM_DocumentosResguardos.FirstOrDefault(x => x.ResguardoID == resguardoID && x.tipoArchivo == 1);

                if (documentoLicencia != null)
                {
                    string rutaViejaLicencia = documentoLicencia.nombreRuta.Trim();

                    documentoLicencia.nombreRuta = rutaBase + archivoLicencia.FileName.Trim();
                    documentoLicencia.nombreArchivo = archivoLicencia.FileName.Trim();
                    documentoLicencia.fechaSubido = DateTime.Now;
                    GlobalUtils.SaveArchivo(archivoLicencia, documentoLicencia.nombreRuta);

                    // Se elimina el archivo anterior.
                    if (File.Exists(rutaViejaLicencia))
                    {
                        File.Delete(rutaViejaLicencia);
                    }
                }
                else
                {
                    var fileLicencia = new tblM_DocumentosResguardos();
                    fileLicencia.ResguardoID = resguardoID;
                    fileLicencia.nombreRuta = rutaBase + archivoLicencia.FileName.Trim();
                    fileLicencia.nombreArchivo = archivoLicencia.FileName.Trim();
                    fileLicencia.tipoArchivo = 1;
                    fileLicencia.fechaSubido = DateTime.Now;
                    fileLicencia.tipoResguardo = 1;
                    GlobalUtils.SaveArchivo(archivoLicencia, fileLicencia.nombreRuta);

                    _context.tblM_DocumentosResguardos.Add(fileLicencia);
                }
            }

            if (archivoPoliza != null && archivoPoliza.ContentLength > 0)
            {
                var documentoPoliza = _context.tblM_DocumentosResguardos.FirstOrDefault(x => x.ResguardoID == resguardoID && x.tipoArchivo == 3);

                if (documentoPoliza != null)
                {
                    string rutaViejaPoliza = documentoPoliza.nombreRuta.Trim();

                    documentoPoliza.nombreRuta = rutaBase + archivoPoliza.FileName.Trim();
                    documentoPoliza.nombreArchivo = archivoPoliza.FileName.Trim();
                    documentoPoliza.fechaSubido = DateTime.Now;
                    GlobalUtils.SaveArchivo(archivoPoliza, documentoPoliza.nombreRuta);

                    // Se elimina el archivo anterior.
                    if (File.Exists(rutaViejaPoliza))
                    {
                        File.Delete(rutaViejaPoliza);
                    }
                }
                else
                {
                    var filePoliza = new tblM_DocumentosResguardos();
                    filePoliza.ResguardoID = resguardoID;
                    filePoliza.nombreRuta = rutaBase + archivoPoliza.FileName.Trim();
                    filePoliza.nombreArchivo = archivoPoliza.FileName.Trim();
                    filePoliza.tipoArchivo = 3;
                    filePoliza.fechaSubido = DateTime.Now;
                    filePoliza.tipoResguardo = 1;
                    GlobalUtils.SaveArchivo(archivoPoliza, filePoliza.nombreRuta);

                    _context.tblM_DocumentosResguardos.Add(filePoliza);
                }
            }

            if (archivoCurso != null && archivoCurso.ContentLength > 0)
            {
                var documentoCurso = _context.tblM_DocumentosResguardos.FirstOrDefault(x => x.ResguardoID == resguardoID && x.tipoArchivo == 9);

                if (documentoCurso != null)
                {
                    string rutaViejaCurso = documentoCurso.nombreRuta.Trim();

                    documentoCurso.nombreRuta = rutaBase + archivoCurso.FileName.Trim();
                    documentoCurso.nombreArchivo = archivoCurso.FileName.Trim();
                    documentoCurso.fechaSubido = DateTime.Now;
                    GlobalUtils.SaveArchivo(archivoCurso, documentoCurso.nombreRuta);

                    // Se elimina el archivo anterior.
                    if (File.Exists(rutaViejaCurso))
                    {
                        File.Delete(rutaViejaCurso);
                    }
                }
                else
                {
                    var fileCurso = new tblM_DocumentosResguardos();
                    fileCurso.ResguardoID = resguardoID;
                    fileCurso.nombreRuta = rutaBase + archivoCurso.FileName.Trim();
                    fileCurso.nombreArchivo = archivoCurso.FileName.Trim();
                    fileCurso.tipoArchivo = 9;
                    fileCurso.fechaSubido = DateTime.Now;
                    fileCurso.tipoResguardo = 1;
                    GlobalUtils.SaveArchivo(archivoCurso, fileCurso.nombreRuta);

                    _context.tblM_DocumentosResguardos.Add(fileCurso);
                }
            }

            _context.SaveChanges();
        }

    }
}
