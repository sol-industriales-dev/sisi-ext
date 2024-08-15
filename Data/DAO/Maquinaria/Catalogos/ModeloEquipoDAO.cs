using Core.DAO.Maquinaria.Catalogos;
using Core.DTO.Maquinaria.Catalogos;
using Core.Entity.Maquinaria.Catalogo;
using Core.Entity.Maquinaria.Overhaul;
using Core.Enum.Principal.Bitacoras;
using Data.EntityFramework.Generic;
using Data.Factory.Principal.Archivos;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Data.DAO.Maquinaria.Catalogos
{
    public class ModeloEquipoDAO : GenericDAO<tblM_CatModeloEquipo>, IModeloEquipoDAO
    {
        private ArchivoFactoryServices archivofs = new ArchivoFactoryServices();

        public tblM_CatModeloEquipo getModeloByID(int id)
        {
            return _context.tblM_CatModeloEquipo.FirstOrDefault(x => x.id == id);
        }
        public void Guardar(tblM_CatModeloEquipo obj)
        {
            if (!Exists(obj))
            {
                if (obj.id == 0)
                {
                    SaveEntity(obj, (int)BitacoraEnum.MODELO);
                    var rel = new tblM_CatMarcaEquipotblM_CatGrupoMaquinaria();
                    rel.tblM_CatGrupoMaquinaria_id = (int)obj.idGrupo;
                    rel.tblM_CatMarcaEquipo_id = obj.marcaEquipoID;
                    rel.isActivo = true;
                    _context.tblM_CatMarcaEquipotblM_CatGrupoMaquinaria.Add(rel);
                    _context.SaveChanges();
                }
                else
                {
                    Update(obj, obj.id, (int)BitacoraEnum.MODELO);
                    var temp = _context.tblM_CatMarcaEquipotblM_CatGrupoMaquinaria.FirstOrDefault(x => x.isActivo.HasValue && x.isActivo.Value && x.tblM_CatGrupoMaquinaria_id == obj.idGrupo && x.tblM_CatMarcaEquipo_id == obj.marcaEquipoID);
                    if (temp == null)
                    {
                        var rel = new tblM_CatMarcaEquipotblM_CatGrupoMaquinaria();
                        rel.tblM_CatGrupoMaquinaria_id = (int)obj.idGrupo;
                        rel.tblM_CatMarcaEquipo_id = obj.marcaEquipoID;
                        rel.isActivo = true;
                        _context.tblM_CatMarcaEquipotblM_CatGrupoMaquinaria.Add(rel);
                        _context.SaveChanges();
                    }
                }
            }
            else
            {
                throw new Exception("Ya existe una marca con esa descripción");
            }
        }
        public bool Exists(tblM_CatModeloEquipo obj)
        {
            return _context.tblM_CatTipoMaquinaria.Where(x => x.descripcion == obj.descripcion &&
                                        x.id != obj.id).ToList().Count > 0 ? true : false;
        }

        public List<tblM_CatMarcaEquipo> FillCboMarcaEquipo(bool estatus)
        {
            return _context.tblM_CatMarcaEquipo.Where(x => x.estatus == estatus).ToList();
        }
        public List<tblM_CatGrupoMaquinaria> fillGrupoMaquinaria(bool estatus)
        {
            return _context.tblM_CatGrupoMaquinaria.Where(x => x.estatus == estatus).OrderBy(X => X.descripcion).ToList();
        }
        public List<ModeloEquipoDTO> FillGridModeloEquipo(tblM_CatModeloEquipo obj, string grupoDesc)
        {
            var listaModelos = (
                from modelo in _context.tblM_CatModeloEquipo
                join marca in _context.tblM_CatMarcaEquipo on modelo.marcaEquipoID equals marca.id
                join grupo in _context.tblM_CatGrupoMaquinaria on modelo.idGrupo equals grupo.id
                where
                    (string.IsNullOrEmpty(obj.descripcion) == true ? modelo.descripcion == modelo.descripcion : modelo.descripcion.Contains(obj.descripcion)) &&
                    (obj.marcaEquipoID == 0 ? modelo.marcaEquipoID == modelo.marcaEquipoID : obj.marcaEquipoID == modelo.marcaEquipoID) &&
                    modelo.estatus == obj.estatus &&
                    modelo.marcaEquipo.estatus &&
                    ((grupoDesc != null && grupoDesc != "") ? grupo.descripcion.Contains(grupoDesc) : true)
                select new ModeloEquipoDTO
                {
                    id = modelo.id,
                    descripcion = modelo.descripcion,
                    estatus = modelo.estatus,
                    marcaEquipoID = modelo.marcaEquipoID,
                    nomCorto = modelo.nomCorto,
                    noComponente = modelo.noComponente,
                    idGrupo = modelo.idGrupo,
                    Ruta = modelo.Ruta,
                    overhaul = modelo.overhaul,
                    marcaDesc = marca.descripcion,
                    grupoDesc = grupo.descripcion
                }
            ).ToList();

            return listaModelos;
        }

        public List<tblM_CatModeloEquipo> FillCboModelo(int idGrupo)
        {
            return _context.tblM_CatModeloEquipo.Where(x => x.estatus == true && idGrupo == 0 ? true : x.idGrupo == idGrupo).ToList();
        }
        public List<tblM_CatModeloEquipo> GetLstModeloActivos()
        {
            return (from modelo in _context.tblM_CatModeloEquipo
                    where modelo.estatus && modelo.marcaEquipo.estatus
                    select modelo).ToList();
        }
        public void SubirArchivos(tblM_CatModeloEquipo obj, HttpPostedFileBase file)
        {
            if (true)
            {
                if (obj.id == 0)
                {

                    if (file != null)
                    {



                        string extension = Path.GetExtension(file.FileName);
                        SaveEntity(obj, (int)BitacoraEnum.CONTROLENVIO);

                        string FileName = obj.descripcion + extension;
                        string Ruta = archivofs.getArchivo().getUrlDelServidor(4) + FileName;
                        SaveArchivo(file, Ruta);


                        obj.Ruta = Ruta;
                        //  obj.Nombre = FileName;
                        //  obj.RutaArchivo = Ruta;


                        Update(obj, obj.id, (int)BitacoraEnum.CONTROLENVIO);
                    }
                    else
                    {
                        //  obj.Nombre = "";
                        //  obj.RutaArchivo = "";
                        SaveEntity(obj, (int)BitacoraEnum.CONTROLENVIO);
                    }
                }

                else
                    Update(obj, obj.id, (int)BitacoraEnum.CONTROLENVIO);
            }
            else
            {
                if (obj.id == 0)
                    throw new Exception("Ya se capturo el registro.");
                else
                    Update(obj, obj.id, (int)BitacoraEnum.CONTROLENVIO);
            }
        }

        public void SaveArchivo(HttpPostedFileBase archivo, string ruta)
        {

            byte[] data;
            using (Stream inputStream = archivo.InputStream)
            {
                MemoryStream memoryStream = inputStream as MemoryStream;
                if (memoryStream == null)
                {
                    memoryStream = new MemoryStream();
                    inputStream.CopyTo(memoryStream);
                }
                data = memoryStream.ToArray();
            }
            ruta = ruta.Replace("C:\\", "\\REPOSITORIO\\");
            File.WriteAllBytes(ruta, data);
        }

        public tblM_CatModeloEquipo LoadArchivos(int id)
        {
            return _context.tblM_CatModeloEquipo.FirstOrDefault(x => x.id.Equals(id));
        }

        public void GuardarSubconjuntos(List<int> listaSubConjuntos, List<string> listaNumParte, int idModelo) 
        {
            try
            {
                var subconjuntosBorrar = _context.tblM_CatModeloEquipotblM_CatSubConjunto.Where(x => x.modeloID == idModelo);
                _context.tblM_CatModeloEquipotblM_CatSubConjunto.RemoveRange(subconjuntosBorrar);
                for (int i = 0; i < listaSubConjuntos.Count; i++)
                {
                    tblM_CatModeloEquipotblM_CatSubConjunto aux = new tblM_CatModeloEquipotblM_CatSubConjunto();
                    aux.id = 0;
                    aux.modeloID = idModelo;
                    aux.subconjuntoID = listaSubConjuntos[i];
                    aux.numParte = listaNumParte[i];
                    _context.tblM_CatModeloEquipotblM_CatSubConjunto.Add(aux);

                }
                _context.SaveChanges();
            }
            catch (Exception e) { }
        }

    }
}
