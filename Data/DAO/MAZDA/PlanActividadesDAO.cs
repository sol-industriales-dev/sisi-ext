using Core.DAO.MAZDA;
using Core.DTO.MAZDA;
using Core.Entity.MAZDA;
using Core.Enum.MAZDA;
using Data.EntityFramework.Generic;
//using Infrastructure.DTO;
using Core.DTO.Principal.Generales;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infrastructure.Utils;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using OfficeOpenXml.Table;
using OfficeOpenXml.Drawing;

namespace Data.DAO.MAZDA
{
    class PlanActividadesDAO : GenericDAO<tblMAZ_Cuadrilla>, IPlanActividadesDAO
    {
        #region CUADRILLAS
        public List<CuadrillaDTO> getCuadrillas(int cuadrillaID, string personal)
        {
            var list = _context.tblMAZ_Cuadrilla.ToList().Where(x => x.estatus == true && (cuadrillaID != 0 ? x.id == cuadrillaID : true)).Select(y => new CuadrillaDTO
            {
                id = y.id,
                descripcion = y.descripcion,
                personal = _context.tblMAZ_Usuario_Cuadrilla.ToList().Where(w => w.cuadrillaID == y.id).ToList().Count > 0 ?
                _context.tblMAZ_Usuario_Cuadrilla.ToList().Where(w => w.cuadrillaID == y.id).Select(z => z.nombre + " " + z.apellidoPaterno + " " + z.apellidoMaterno).Aggregate((i, j) => i + ", " + j) : ""
            }).ToList();

            var listPersonalFiltrado = list.Where(x => x.personal.ToUpper().Replace(" ", "").Contains(personal.ToUpper().Replace(" ", ""))).Select(y => new CuadrillaDTO
            {
                id = y.id,
                descripcion = y.descripcion,
                personal = _context.tblMAZ_Usuario_Cuadrilla.ToList().Where(w => w.cuadrillaID == y.id).ToList().Count > 0 ?
                _context.tblMAZ_Usuario_Cuadrilla.ToList().Where(w => w.cuadrillaID == y.id).Select(z => z.nombre + " " + z.apellidoPaterno + " " + z.apellidoMaterno).Aggregate((i, j) => i + ", " + j) : ""
            }).ToList();

            return listPersonalFiltrado;
        }
        public void GuardarCuadrilla(string desc, List<UsuarioMAZDADTO> personal)
        {
            try
            {
                tblMAZ_Cuadrilla newCua = new tblMAZ_Cuadrilla();

                newCua.descripcion = desc;
                newCua.estatus = true;

                _context.tblMAZ_Cuadrilla.Add(newCua);
                _context.SaveChanges();

                if (personal != null)
                {
                    var ids = personal.Select(w => w.id).ToList();
                    var usuarios = _context.tblMAZ_Usuario_Cuadrilla.Where(x => x.estatus == true && ids.Contains(x.id)).ToList();

                    foreach (var usu in usuarios)
                    {
                        usu.cuadrillaID = newCua.id;
                        usu.nivel = 3;

                        var ordenUltimo = _context.tblMAZ_Usuario_Cuadrilla.Where(x => x.estatus == true && x.cuadrillaID == newCua.id).ToList();
                        usu.orden = ordenUltimo.Count > 0 ? ordenUltimo.Select(x => x.orden).Max() + 1 : 1;

                        _context.Entry(usu).State = System.Data.Entity.EntityState.Modified;
                        _context.SaveChanges();
                    }
                }
            }
            catch (Exception) { }
        }
        public void EditarCuadrilla(int id, string desc, List<UsuarioMAZDADTO> personal)
        {
            try
            {
                var cuadrilla = _context.tblMAZ_Cuadrilla.Where(x => x.estatus == true && x.id == id).FirstOrDefault();

                if (cuadrilla != null)
                {
                    cuadrilla.descripcion = desc;

                    _context.Entry(cuadrilla).State = System.Data.Entity.EntityState.Modified;
                    _context.SaveChanges();

                    if (personal != null)
                    {
                        var usuariosAnt = _context.tblMAZ_Usuario_Cuadrilla.Where(x => x.estatus == true && x.cuadrillaID == cuadrilla.id).ToList();

                        foreach (var usuAnt in usuariosAnt)
                        {
                            usuAnt.cuadrillaID = 0;

                            _context.Entry(usuAnt).State = System.Data.Entity.EntityState.Modified;
                            _context.SaveChanges();
                        }

                        var ids = personal.Select(w => w.id).ToList();
                        var usuarios = _context.tblMAZ_Usuario_Cuadrilla.Where(x => x.estatus == true && ids.Contains(x.id)).ToList();

                        foreach (var usu in usuarios)
                        {
                            usu.cuadrillaID = cuadrilla.id;

                            _context.Entry(usu).State = System.Data.Entity.EntityState.Modified;
                            _context.SaveChanges();
                        }
                    }
                    else
                    {
                        var usuarios = _context.tblMAZ_Usuario_Cuadrilla.Where(x => x.estatus == true && x.cuadrillaID == cuadrilla.id).ToList();

                        foreach (var usu in usuarios)
                        {
                            usu.cuadrillaID = 0;

                            _context.Entry(usu).State = System.Data.Entity.EntityState.Modified;
                            _context.SaveChanges();
                        }
                    }
                }
            }
            catch (Exception) { }
        }
        public CuadrillaDTO getCuadrilla(int id)
        {
            var personal = _context.tblMAZ_Usuario_Cuadrilla.Where(x => x.estatus == true && x.cuadrillaID == id).Select(y => new UsuarioMAZDADTO
            {
                id = y.id,
                nombre = y.nombre,
                apellidoPaterno = y.apellidoPaterno,
                apellidoMaterno = y.apellidoMaterno,
                nombreCompleto = y.nombre + " " + y.apellidoPaterno + " " + y.apellidoMaterno
            }).ToList();

            var cuadrilla = _context.tblMAZ_Cuadrilla.ToList().Where(x => x.estatus == true && x.id == id).Select(y => new CuadrillaDTO
            {
                id = y.id,
                descripcion = y.descripcion,
                personalLista = personal
            }).FirstOrDefault();

            return cuadrilla;
        }
        public void RemoveCuadrilla(int id)
        {
            try
            {
                var cuadrilla = _context.tblMAZ_Cuadrilla.Where(x => x.estatus == true && x.id == id).FirstOrDefault();

                if (cuadrilla != null)
                {
                    cuadrilla.estatus = false;

                    _context.Entry(cuadrilla).State = System.Data.Entity.EntityState.Modified;
                    _context.SaveChanges();

                    var usuariosCuadrilla = _context.tblMAZ_Usuario_Cuadrilla.Where(x => x.estatus == true && x.cuadrillaID == cuadrilla.id).ToList();

                    foreach (var usuCua in usuariosCuadrilla)
                    {
                        usuCua.cuadrillaID = 0;

                        _context.Entry(usuCua).State = System.Data.Entity.EntityState.Modified;
                        _context.SaveChanges();
                    }

                    var areasCuadrilla = _context.tblMAZ_Area.Where(x => x.estatus == true && x.cuadrillaID == cuadrilla.id).ToList();

                    foreach (var areaCua in areasCuadrilla)
                    {
                        areaCua.cuadrillaID = 0;

                        _context.Entry(areaCua).State = System.Data.Entity.EntityState.Modified;
                        _context.SaveChanges();
                    }
                }
            }
            catch (Exception) { }
        }
        #endregion

        #region AREAS
        public List<AreaDTO> getAreas(int cuadrillaID, string area)
        {
            var areaUpper = area.ToUpper();
            var list = _context.tblMAZ_Area.Where(x => x.estatus == true && (cuadrillaID != 0 ? x.cuadrillaID == cuadrillaID : true) && (x.descripcion.ToUpper().Contains(areaUpper))).Select(y => new AreaDTO
            {
                id = y.id,
                descripcion = y.descripcion,
                cuadrillaID = y.cuadrillaID,
                cuadrilla = _context.tblMAZ_Cuadrilla.Where(z => z.estatus == true && z.id == y.cuadrillaID).Select(w => w.descripcion).FirstOrDefault()
            }).ToList();

            return list;
        }
        public bool GuardarArea(tblMAZ_Area area, List<tblMAZ_Area_Referencia> referencias)
        {
            try
            {
                _context.tblMAZ_Area.Add(area);
                _context.SaveChanges();

                if (area.id > 0)
                {
                    referencias.ForEach(e =>
                    {
                        e.areaID = area.id;
                        e.estatus = true;
                        _context.tblMAZ_Area_Referencia.Add(e);
                        _context.SaveChanges();
                    });
                }
            }
            catch (Exception) { }

            return area.id > 0;
        }
        public void EditarArea(int id, string desc, int cuadrillaID)
        {
            try
            {
                var area = _context.tblMAZ_Area.Where(x => x.estatus == true && x.id == id).FirstOrDefault();

                if (area != null)
                {
                    area.descripcion = desc;
                    area.cuadrillaID = cuadrillaID;

                    _context.Entry(area).State = System.Data.Entity.EntityState.Modified;
                    _context.SaveChanges();
                }
            }
            catch (Exception) { }
        }
        public AreaDTO getArea(int id)
        {
            var area = _context.tblMAZ_Area.ToList().Where(x => x.estatus == true && x.id == id).Select(y => new AreaDTO
            {
                id = y.id,
                descripcion = y.descripcion,
                cuadrillaID = y.cuadrillaID
            }).FirstOrDefault();

            return area;
        }
        public void RemoveArea(int id)
        {
            try
            {
                var area = _context.tblMAZ_Area.Where(x => x.estatus == true && x.id == id).FirstOrDefault();

                if (area != null)
                {
                    area.estatus = false;

                    _context.Entry(area).State = System.Data.Entity.EntityState.Modified;
                    _context.SaveChanges();

                    var actividadesArea = _context.tblMAZ_Actividad.Where(x => x.estatus == true && x.areaID == area.id).ToList();

                    foreach (var actArea in actividadesArea)
                    {
                        actArea.areaID = 0;

                        _context.Entry(actArea).State = System.Data.Entity.EntityState.Modified;
                        _context.SaveChanges();
                    }
                }
            }
            catch (Exception) { }
        }
        public int GetUltimoArchivoArea()
        {
            return _context.tblMAZ_Area_Referencia.ToList().LastOrDefault() != null ? _context.tblMAZ_Area_Referencia.ToList().LastOrDefault().id : 0;
        }
        public void QuitarReferenciaArea(int areaID)
        {
            var referencias = _context.tblMAZ_Area_Referencia.Where(x => x.estatus == true && x.areaID == areaID).ToList();

            foreach (var refe in referencias)
            {
                refe.estatus = false;

                _context.Entry(refe).State = System.Data.Entity.EntityState.Modified;
                _context.SaveChanges();
            }
        }
        public bool GuardarReferenciaArea(int areaID, List<tblMAZ_Area_Referencia> referencias)
        {
            try
            {
                referencias.ForEach(e =>
                {
                    e.areaID = areaID;
                    e.estatus = true;
                    _context.tblMAZ_Area_Referencia.Add(e);
                    _context.SaveChanges();
                });
            }
            catch (Exception) { }

            return true;
        }
        #endregion

        #region ACTIVIDADES
        public List<ActividadDTO> getActividades(int cuadrillaID, int periodo, string area, string actividad)
        {
            var areaUpper = area.ToUpper();
            var areas = _context.tblMAZ_Area.Where(x => x.descripcion.ToUpper().Contains(areaUpper)).Select(y => y.id).ToList();

            var list = _context.tblMAZ_Actividad.Where(x =>
                x.estatus == true &&
                (cuadrillaID != 0 ? (_context.tblMAZ_Area.Where(z => z.id == x.areaID).Select(w => w.cuadrillaID).FirstOrDefault()) == cuadrillaID : true) &&
                (periodo != 0 ? x.periodo == periodo : true) &&
                //(_context.tblMAZ_Area.Where(z => z.estatus == true && z.id == x.areaID).Select(w => w.descripcion).FirstOrDefault()).Contains(area) &&
                areas.Contains(x.areaID) &&
                x.descripcion.Contains(actividad)).Select(y => new ActividadDTO
                {
                    id = y.id,
                    descripcion = y.descripcion,
                    areaID = y.areaID,
                    area = _context.tblMAZ_Area.Where(z => z.estatus == true && z.id == y.areaID).Select(w => w.descripcion).FirstOrDefault(),
                    periodo = y.periodo,
                    periodoDesc = ((PeriodoEnum)y.periodo).ToString(),
                    cuadrillaID = _context.tblMAZ_Area.Where(z => z.id == y.areaID).Select(w => w.cuadrillaID).FirstOrDefault(),
                    cuadrilla = _context.tblMAZ_Cuadrilla.Where(z => z.id == _context.tblMAZ_Area.Where(w => w.id == y.areaID).Select(r => r.cuadrillaID).FirstOrDefault()).Select(t => t.descripcion).FirstOrDefault()
                }).ToList();

            return list;
        }
        public void GuardarActividad(string desc, string descripcion, int cuadrillaID, string area, int periodo)
        {
            try
            {
                tblMAZ_Actividad newAct = new tblMAZ_Actividad();

                newAct.descripcion = desc;
                newAct.detalle = descripcion;

                var checkArea = _context.tblMAZ_Area.Where(x => x.estatus == true && x.descripcion == area && x.cuadrillaID == cuadrillaID).FirstOrDefault();

                if (checkArea != null)
                {
                    newAct.areaID = checkArea.id;
                }
                else
                {
                    tblMAZ_Area newArea = new tblMAZ_Area();

                    newArea.descripcion = area;
                    newArea.cuadrillaID = cuadrillaID;
                    newArea.estatus = true;

                    _context.tblMAZ_Area.Add(newArea);
                    _context.SaveChanges();

                    newAct.areaID = newArea.id;
                }

                newAct.periodo = periodo;
                newAct.estatus = true;

                _context.tblMAZ_Actividad.Add(newAct);
                _context.SaveChanges();
            }
            catch (Exception) { }
        }
        public void EditarActividad(int id, string desc, string descripcion, int cuadrillaID, string area, int periodo)
        {
            try
            {
                var actividad = _context.tblMAZ_Actividad.Where(x => x.estatus == true && x.id == id).FirstOrDefault();

                if (actividad != null)
                {
                    actividad.descripcion = desc;
                    actividad.detalle = descripcion;

                    var checkArea = _context.tblMAZ_Area.Where(x => x.estatus == true && x.descripcion == area && x.cuadrillaID == cuadrillaID).FirstOrDefault();

                    if (checkArea != null)
                    {
                        actividad.areaID = checkArea.id;
                    }
                    else
                    {
                        tblMAZ_Area newArea = new tblMAZ_Area();

                        newArea.descripcion = area;
                        newArea.cuadrillaID = cuadrillaID;
                        newArea.estatus = true;

                        _context.tblMAZ_Area.Add(newArea);
                        _context.SaveChanges();

                        actividad.areaID = newArea.id;
                    }

                    actividad.periodo = periodo;

                    _context.Entry(actividad).State = System.Data.Entity.EntityState.Modified;
                    _context.SaveChanges();
                }
            }
            catch (Exception) { }
        }
        public ActividadDTO getActividad(int id)
        {
            var actividad = _context.tblMAZ_Actividad.ToList().Where(x => x.estatus == true && x.id == id).Select(y => new ActividadDTO
            {
                id = y.id,
                descripcion = y.descripcion,
                detalle = y.detalle,
                areaID = y.areaID,
                area = _context.tblMAZ_Area.Where(z => z.estatus == true && z.id == y.areaID).Select(w => w.descripcion).FirstOrDefault(),
                periodo = y.periodo,
                cuadrillaID = _context.tblMAZ_Area.Where(z => z.id == y.areaID).Select(w => w.cuadrillaID).FirstOrDefault()
            }).FirstOrDefault();

            return actividad;
        }
        public void RemoveActividad(int id)
        {
            try
            {
                var actividad = _context.tblMAZ_Actividad.Where(x => x.estatus == true && x.id == id).FirstOrDefault();

                if (actividad != null)
                {
                    actividad.estatus = false;

                    _context.Entry(actividad).State = System.Data.Entity.EntityState.Modified;
                    _context.SaveChanges();
                }
            }
            catch (Exception) { }
        }
        public List<ActividadDTO> getActividadesAC()
        {
            var acID = 1;

            var checkAC = _context.tblMAZ_Cuadrilla.Where(x => x.descripcion == "AIRES ACONDICIONADOS").FirstOrDefault();

            if (checkAC != null)
            {
                acID = checkAC.id;
            }

            var areasID = _context.tblMAZ_Area.Where(x => x.estatus == true && x.cuadrillaID == acID).Select(y => y.id).ToList();

            var actividades = _context.tblMAZ_Actividad.Where(x => x.estatus == true && areasID.Contains(x.areaID)).Select(y => new ActividadDTO
            {
                id = y.id,
                descripcion = y.descripcion,
                areaID = y.areaID,
                area = _context.tblMAZ_Area.Where(z => z.estatus == true && z.id == y.areaID).Select(w => w.descripcion).FirstOrDefault(),
                periodo = y.periodo,
                cuadrillaID = _context.tblMAZ_Area.Where(z => z.id == y.areaID).Select(w => w.cuadrillaID).FirstOrDefault(),
                cuadrilla = _context.tblMAZ_Cuadrilla.Where(z => z.id == _context.tblMAZ_Area.Where(w => w.id == y.areaID).Select(r => r.cuadrillaID).FirstOrDefault()).Select(t => t.descripcion).FirstOrDefault()
            }).ToList();

            return actividades;
        }
        #endregion

        #region REVISION
        public bool GuardarRevision(tblMAZ_Revision_AC rev, List<tblMAZ_Revision_AC_Ayudantes> ayu, List<tblMAZ_Revision_AC_Detalle> det, List<tblMAZ_Revision_AC_Evidencia> evi)
        {
            rev.observaciones = rev.observaciones ?? string.Empty;
            rev.ayudantes = 0;
            rev.fechaCaptura = DateTime.Now;
            _context.tblMAZ_Revision_AC.Add(rev);
            _context.SaveChanges();
            if (rev.id > 0)
            {
                ayu.ForEach(a =>
                {
                    a.revisionID = rev.id;
                    _context.tblMAZ_Revision_AC_Ayudantes.Add(a);
                    _context.SaveChanges();
                });
                det.ForEach(d =>
                {
                    d.revisionID = rev.id;
                    d.observaciones = d.observaciones ?? string.Empty;

                    d.ultMant = "";
                    d.sigMant = "";
                    d.reprogramacion = "";
                    d.estatusInfo = "";

                    _context.tblMAZ_Revision_AC_Detalle.Add(d);
                    _context.SaveChanges();
                });
                evi.ForEach(e =>
                {
                    e.idRevision = rev.id;
                    _context.tblMAZ_Revision_AC_Evidencia.Add(e);
                    _context.SaveChanges();
                });
            }
            return rev.id > 0;
        }
        public bool GuardarRevisionCuadrilla(tblMAZ_Revision_Cuadrilla rev, List<tblMAZ_Revision_Cuadrilla_Ayudantes> ayu, List<tblMAZ_Revision_Cuadrilla_Detalle> det, List<tblMAZ_Revision_Cuadrilla_Evidencia> evi)
        {
            rev.observaciones = rev.observaciones ?? string.Empty;
            rev.fechaCaptura = DateTime.Now;
            _context.tblMAZ_Revision_Cuadrilla.Add(rev);
            _context.SaveChanges();
            if (rev.id > 0)
            {
                ayu.ForEach(a =>
                {
                    a.revisionID = rev.id;
                    _context.tblMAZ_Revision_Cuadrilla_Ayudantes.Add(a);
                    _context.SaveChanges();
                });
                det.ForEach(d =>
                {
                    d.revisionID = rev.id;
                    d.estadoString = d.estadoString ?? string.Empty;

                    d.ultMant = "";
                    d.sigMant = "";
                    d.reprogramacion = "";
                    d.estatusInfo = "";

                    _context.tblMAZ_Revision_Cuadrilla_Detalle.Add(d);
                    _context.SaveChanges();
                });
                evi.ForEach(e =>
                {
                    e.idRevision = rev.id;
                    _context.tblMAZ_Revision_Cuadrilla_Evidencia.Add(e);
                    _context.SaveChanges();
                });
            }
            return rev.id > 0;
        }
        public RevisionACDTO getRevisionAC(int revisionID)
        {
            var equipos = _context.tblMAZ_Equipo_AC.Where(x => x.estatus == true).ToList();
            var areas = _context.tblMAZ_Area.Where(x => x.estatus == true).ToList();
            var personal = _context.tblMAZ_Usuario_Cuadrilla.Where(x => x.estatus == true).ToList();
            var actividades = _context.tblMAZ_Actividad.Where(x => x.estatus == true).ToList();

            var detalle = _context.tblMAZ_Revision_AC_Detalle.ToList().Where(x => x.estatus == true && x.revisionID == revisionID).Select(w => new RevisionAC_DetalleDTO
            {
                id = w.id,
                tipo = w.tipo,
                actividadID = w.actividadID,
                actividad = actividades.Where(y => y.id == w.actividadID).FirstOrDefault().descripcion,
                realizo = w.realizo,
                observaciones = w.observaciones,
                revisionID = w.revisionID
            }).ToList();

            var ayudantes = _context.tblMAZ_Revision_AC_Ayudantes.Where(x => x.estatus == true && x.revisionID == revisionID).ToList();
            var ayudantesNombres = "";
            var contador = 1;

            foreach (var ayudante in ayudantes)
            {
                if (contador > 1)
                {
                    ayudantesNombres += ", " + personal.Where(x => x.id == ayudante.idPersonal).Select(y => string.Format("{0} {1} {2}", y.nombre, y.apellidoPaterno, y.apellidoMaterno)).FirstOrDefault();
                }
                else
                {
                    ayudantesNombres += personal.Where(x => x.id == ayudante.idPersonal).Select(y => string.Format("{0} {1} {2}", y.nombre, y.apellidoPaterno, y.apellidoMaterno)).FirstOrDefault();
                }
                contador++;
            }

            var revision = (from rev in _context.tblMAZ_Revision_AC.ToList()
                            where rev.estatus == true && rev.id == revisionID
                            select new RevisionACDTO
                            {
                                id = rev.id,
                                equipoID = rev.equipoID,
                                equipo = equipos.Where(y => y.id == rev.equipoID).FirstOrDefault().descripcion,
                                tonelaje = rev.tonelaje.ToString("0.00"),
                                areaID = rev.area,
                                area = areas.Where(y => y.id == rev.area).FirstOrDefault().descripcion,
                                periodo = ((PeriodoEnum)rev.periodo).ToString(),
                                tecnicoID = rev.tecnico,
                                tecnico = rev.tecnico != 0 ? personal.Where(y => y.id == rev.tecnico).Select(w => string.Format("{0} {1} {2}", w.nombre, w.apellidoPaterno, w.apellidoMaterno)).FirstOrDefault() : "",
                                ayudantes = ayudantesNombres,
                                observaciones = rev.observaciones,
                                fechaCaptura = rev.fechaCaptura,
                                detalle = detalle.Where(y => y.revisionID == rev.id).ToList()
                            }).FirstOrDefault();

            return revision;
        }
        public RevisionCuaDTO getRevisionCua(int cuadrillaID, int revisionID)
        {
            var actividades = _context.tblMAZ_Actividad.Where(x => x.estatus == true).ToList();
            var cuadrillas = _context.tblMAZ_Cuadrilla.Where(x => x.estatus == true).ToList();
            var personal = _context.tblMAZ_Usuario_Cuadrilla.Where(x => x.estatus == true).ToList();

            var detalle = _context.tblMAZ_Revision_Cuadrilla_Detalle.ToList().Where(x => x.estatus == true && x.revisionID == revisionID).Select(w => new RevisionCua_DetalleDTO
            {
                id = w.id,
                actividadID = w.actividadID,
                actividad = actividades.Where(y => y.id == w.actividadID).FirstOrDefault().descripcion,
                realizo = w.realizo,
                estadoString = w.estadoString,
                revisionID = w.revisionID
            }).ToList();

            var ayudantes = _context.tblMAZ_Revision_Cuadrilla_Ayudantes.Where(x => x.estatus == true && x.revisionID == revisionID).ToList();
            var ayudantesNombres = "";
            var contador = 1;

            foreach (var ayudante in ayudantes)
            {
                if (contador > 1)
                {
                    ayudantesNombres += ", " + personal.Where(x => x.id == ayudante.idPersonal).Select(y => string.Format("{0} {1} {2}", y.nombre, y.apellidoPaterno, y.apellidoMaterno)).FirstOrDefault();
                }
                else
                {
                    ayudantesNombres += personal.Where(x => x.id == ayudante.idPersonal).Select(y => string.Format("{0} {1} {2}", y.nombre, y.apellidoPaterno, y.apellidoMaterno)).FirstOrDefault();
                }
                contador++;
            }

            var revision = _context.tblMAZ_Revision_Cuadrilla.ToList().Where(x => x.estatus == true && x.id == revisionID).Select(w => new RevisionCuaDTO
            {
                id = w.id,
                cuadrillaID = w.cuadrillaID,
                cuadrilla = cuadrillas.Where(y => y.id == w.cuadrillaID).FirstOrDefault().descripcion,
                mes = w.mes,
                mesDesc = ((MesEnum)w.mes).ToString(),
                tecnicoID = w.tecnico,
                tecnico = w.tecnico != 0 ? personal.Where(y => y.id == w.tecnico).Select(q => string.Format("{0} {1} {2}", q.nombre, q.apellidoPaterno, q.apellidoMaterno)).FirstOrDefault() : "",
                ayudantes = ayudantesNombres,
                observaciones = w.observaciones,
                fechaCaptura = w.fechaCaptura,
                detalle = detalle.Where(y => y.revisionID == w.id).ToList()
            }).FirstOrDefault();

            return revision;
        }
        public int GetUltimoArchivo(int tipo)
        {
            switch (tipo)
            {
                case 1:
                    return _context.tblMAZ_Revision_AC_Evidencia.ToList().LastOrDefault() != null ? _context.tblMAZ_Revision_AC_Evidencia.ToList().LastOrDefault().id : 0;
                case 2:
                    return _context.tblMAZ_Revision_Cuadrilla_Evidencia.ToList().LastOrDefault() != null ? _context.tblMAZ_Revision_Cuadrilla_Evidencia.ToList().LastOrDefault().id : 0;
                default:
                    return _context.tblMAZ_Revision_Cuadrilla_Evidencia.ToList().LastOrDefault() != null ? _context.tblMAZ_Revision_Cuadrilla_Evidencia.ToList().LastOrDefault().id : 0;
            }
        }
        public int GetUltimaRevision(int tipo)
        {
            switch (tipo)
            {
                case 1:
                    return _context.tblMAZ_Revision_AC.ToList().LastOrDefault() != null ? _context.tblMAZ_Revision_AC.ToList().LastOrDefault().id : 0;
                case 2:
                    return _context.tblMAZ_Revision_Cuadrilla.ToList().LastOrDefault() != null ? _context.tblMAZ_Revision_Cuadrilla.ToList().LastOrDefault().id : 0;
                default:
                    return _context.tblMAZ_Revision_Cuadrilla.ToList().LastOrDefault() != null ? _context.tblMAZ_Revision_Cuadrilla.ToList().LastOrDefault().id : 0;
            }
        }
        public List<string> getEvidenciasAC(int revisionID)
        {
            var evidencias = _context.tblMAZ_Revision_AC_Evidencia.Where(x => x.idRevision == revisionID).ToList();

            List<string> arr64 = new List<string>();

            foreach (var evi in evidencias)
            {
                var extension = evi.ruta.Split('.')[1];

                if (File.Exists(evi.ruta))
                {
                    using (var fileStream = new FileStream(evi.ruta, FileMode.Open, FileAccess.Read))
                    {
                        byte[] arrBytes = ReadFully(fileStream);

                        var cadena64 = Convert.ToBase64String(arrBytes);

                        arr64.Add("data:image/" + extension + ";base64, " + cadena64);
                    }
                }
            }

            return arr64;
        }
        public List<string> getEvidenciasCua(int revisionID)
        {
            var evidencias = _context.tblMAZ_Revision_Cuadrilla_Evidencia.Where(x => x.idRevision == revisionID).ToList();

            List<string> arr64 = new List<string>();

            foreach (var evi in evidencias)
            {
                var extension = evi.ruta.Split('.')[1];

                if (File.Exists(evi.ruta))
                {
                    using (var fileStream = new FileStream(evi.ruta, FileMode.Open, FileAccess.Read))
                    {
                        byte[] arrBytes = ReadFully(fileStream);

                        var cadena64 = Convert.ToBase64String(arrBytes);

                        arr64.Add("data:image/" + extension + ";base64, " + cadena64);
                    }
                }
            }

            return arr64;
        }
        #endregion

        #region EQUIPOS
        public bool GuardarEquipo(tblMAZ_Equipo_AC equi, List<tblMAZ_Equipo_Referencia> referencias)
        {
            try
            {
                _context.tblMAZ_Equipo_AC.Add(equi);
                _context.SaveChanges();

                if (equi.id > 0)
                {
                    referencias.ForEach(e =>
                    {
                        e.equipoID = equi.id;
                        e.estatus = true;
                        _context.tblMAZ_Equipo_Referencia.Add(e);
                        _context.SaveChanges();
                    });
                }
            }
            catch (Exception) { }

            return equi.id > 0;
        }
        public void EditarEquipo(int id, string descripcion, string caracteristicas, string modelo, string tonelaje, int subAreaID, string subArea, int cantidad, bool estatus)
        {
            try
            {
                var equipo = _context.tblMAZ_Equipo_AC.Where(x => x.estatus == true && x.id == id).FirstOrDefault();

                if (equipo != null)
                {
                    equipo.descripcion = descripcion;
                    equipo.caracteristicas = caracteristicas;
                    equipo.modelo = modelo;
                    equipo.tonelaje = tonelaje;

                    if (subAreaID != 0)
                    {
                        equipo.subAreaID = subAreaID;
                    }
                    else
                    {
                        equipo.subAreaID = 0;
                    }
                    equipo.subArea = subArea;
                    equipo.cantidad = cantidad;
                    equipo.estatus = estatus;

                    _context.Entry(equipo).State = System.Data.Entity.EntityState.Modified;
                    _context.SaveChanges();
                }
            }
            catch (Exception) { }
        }
        public void RemoveEquipo(int id)
        {
            try
            {
                var equipo = _context.tblMAZ_Equipo_AC.Where(x => x.estatus == true && x.id == id).FirstOrDefault();

                if (equipo != null)
                {
                    equipo.estatus = false;

                    _context.Entry(equipo).State = System.Data.Entity.EntityState.Modified;
                    _context.SaveChanges();
                }
            }
            catch (Exception) { }
        }
        public List<EquipoDTO> getEquipos(string descripcion, string subArea, int periodo)
        {
            var subAreas = _context.tblMAZ_SubArea.Where(x => x.estatus == true).ToList();

            var subAreaUpper = subArea.ToUpper();
            var subAreasID = _context.tblMAZ_SubArea.Where(x => x.descripcion.ToUpper().Contains(subAreaUpper)).Select(y => y.id).ToList();

            var list = _context.tblMAZ_Equipo_AC.ToList().Where(x =>
                x.estatus == true &&
                (descripcion != "" ? x.descripcion.Contains(descripcion) : true) &&
                (subArea != "" ? subAreasID.Contains(x.subAreaID) : true)).Select(y => new EquipoDTO
                {
                    id = y.id,
                    descripcion = y.descripcion,
                    caracteristicas = y.caracteristicas,
                    modelo = y.modelo,
                    tonelaje = y.tonelaje,
                    subAreaID = y.subAreaID,
                    subArea = subAreas.Where(z => z.id == y.subAreaID).Select(w => w.descripcion).FirstOrDefault(),
                    cantidad = y.cantidad,
                    estatus = y.estatus
                }).ToList();

            return list;
        }
        public List<EquipoDTO> getEquiposCatalogo(List<int> arrCuadrillas, List<int> arrAreas, List<int> arrSubAreas)
        {
            var cuadrillas = _context.tblMAZ_Cuadrilla.Where(x => x.estatus == true).ToList();
            var areas = _context.tblMAZ_Area.Where(x => x.estatus == true).ToList();
            var subAreas = _context.tblMAZ_SubArea.Where(x => x.estatus == true).ToList();

            var areasID = arrCuadrillas != null ? areas.Where(x => arrCuadrillas.Contains(x.cuadrillaID)).Select(y => y.id).ToList() : null;
            var subAreasIDFromAreas = arrAreas != null ? subAreas.Where(x => arrAreas.Contains(x.areaID)).Select(y => y.id).ToList() :
                areasID != null ? subAreas.Where(x => areasID.Contains(x.areaID)).Select(y => y.id).ToList() : arrSubAreas != null ? subAreas.Where(x => arrSubAreas.Contains(x.id)).Select(y => y.id).ToList() : null;

            var list = _context.tblMAZ_Equipo_AC.ToList().Where(x =>
                (x.estatus == true) &&
                (subAreasIDFromAreas != null ? subAreasIDFromAreas.Contains(x.subAreaID) : true)).Select(y => new EquipoDTO
                {
                    id = y.id,
                    descripcion = y.descripcion,
                    caracteristicas = y.caracteristicas,
                    modelo = y.modelo,
                    tonelaje = y.tonelaje,
                    subAreaID = y.subAreaID,
                    subArea = subAreas.Where(z => z.id == y.subAreaID).Select(w => w.descripcion).FirstOrDefault(),
                    areaID = subAreas.Where(z => z.id == y.subAreaID).Select(z => z.areaID).FirstOrDefault(),
                    area = areas.Where(z => z.id == subAreas.Where(w => w.id == y.subAreaID).Select(t => t.areaID).FirstOrDefault()).Select(v => v.descripcion).FirstOrDefault(),
                    cuadrillaID = areas.Where(z => z.id == subAreas.Where(w => w.id == y.subAreaID).Select(t => t.areaID).FirstOrDefault()).Select(v => v.cuadrillaID).FirstOrDefault(),
                    cantidad = y.cantidad,
                    estatus = y.estatus
                }).ToList();

            return list;
        }
        public EquipoDTO getEquipo(int id)
        {
            var areas = _context.tblMAZ_Area.ToList();
            var subAreas = _context.tblMAZ_SubArea.ToList();

            var equipo = _context.tblMAZ_Equipo_AC.ToList().Where(x => x.estatus == true && x.id == id).Select(y => new EquipoDTO
            {
                id = y.id,
                descripcion = y.descripcion,
                caracteristicas = y.caracteristicas,
                modelo = y.modelo,
                tonelaje = y.tonelaje,
                cantidad = y.cantidad,
                subAreaID = y.subAreaID,
                subArea = y.subArea,
                areaID = subAreas.Where(z => z.id == y.subAreaID).Select(z => z.areaID).FirstOrDefault(),
                cuadrillaID = areas.Where(z => z.id == subAreas.Where(w => w.id == y.subAreaID).Select(t => t.areaID).FirstOrDefault()).Select(v => v.cuadrillaID).FirstOrDefault()
            }).FirstOrDefault();

            return equipo;
        }
        public EquipoDTO getEquipoAC(int equipoID)
        {
            var subAreas = _context.tblMAZ_SubArea.Where(X => X.estatus == true).ToList();

            var equipo = _context.tblMAZ_Equipo_AC.ToList().Where(x => x.estatus == true && x.id == equipoID).Select(y => new EquipoDTO
            {
                id = y.id,
                descripcion = y.descripcion,
                tonelaje = y.tonelaje,
                subAreaID = y.subAreaID,
                subArea = subAreas.Where(z => z.id == y.subAreaID).Select(w => w.descripcion).FirstOrDefault(),
            }).FirstOrDefault();

            return equipo;
        }
        public List<EquipoAreaDTO> getEquiposAreas(int cuadrillaID)
        {
            var areas = _context.tblMAZ_Area.ToList().Where(x => x.estatus == true && x.cuadrillaID == cuadrillaID).Select(x => new EquipoAreaDTO
            {
                id = x.id,
                descripcion = x.descripcion,
                tipo = 1
            }).ToList();

            var equipos = _context.tblMAZ_Equipo_AC.ToList().Where(x => x.estatus == true && areas.Select(y => y.id).Contains(x.subAreaID)).Select(x => new EquipoAreaDTO
            {
                id = x.id,
                descripcion = x.descripcion,
                tipo = 2
            }).ToList();

            List<EquipoAreaDTO> list = new List<EquipoAreaDTO>();
            list.AddRange(areas);
            list.AddRange(equipos);

            return list.OrderBy(x => x.tipo).ToList();
        }
        public int GetUltimoArchivoEquipo()
        {
            return _context.tblMAZ_Equipo_Referencia.ToList().LastOrDefault() != null ? _context.tblMAZ_Equipo_Referencia.ToList().LastOrDefault().id : 0;
        }
        public void QuitarReferenciaEquipo(int equipoID)
        {
            var referencias = _context.tblMAZ_Equipo_Referencia.Where(x => x.estatus == true && x.equipoID == equipoID).ToList();

            foreach (var refe in referencias)
            {
                refe.estatus = false;

                _context.Entry(refe).State = System.Data.Entity.EntityState.Modified;
                _context.SaveChanges();
            }
        }
        public bool GuardarReferenciaEquipo(int equipoID, List<tblMAZ_Equipo_Referencia> referencias)
        {
            try
            {
                referencias.ForEach(e =>
                {
                    e.equipoID = equipoID;
                    e.estatus = true;
                    _context.tblMAZ_Equipo_Referencia.Add(e);
                    _context.SaveChanges();
                });
            }
            catch (Exception) { }

            return true;
        }
        public List<string> getReferencias(List<int> equiposID)
        {
            var referencias = _context.tblMAZ_Equipo_Referencia.ToList().Where(x => x.estatus == true && equiposID.Contains(x.equipoID)).ToList();

            List<string> arr64 = new List<string>();

            foreach (var refe in referencias)
            {
                var extension = refe.ruta.Split('.')[1];

                if (File.Exists(refe.ruta))
                {
                    using (var fileStream = new FileStream(refe.ruta, FileMode.Open, FileAccess.Read))
                    {
                        byte[] arrBytes = ReadFully(fileStream);

                        var cadena64 = Convert.ToBase64String(arrBytes);

                        arr64.Add("data:image/" + extension + ";base64, " + cadena64);
                    }
                }
            }

            var equipos = _context.tblMAZ_Equipo_AC.ToList().Where(x => x.estatus == true && equiposID.Contains(x.id)).ToList();
            var areas = _context.tblMAZ_Area.ToList().Where(x => x.estatus == true && equipos.Select(y => y.subAreaID).Contains(x.id)).ToList();

            var referenciasAreas = _context.tblMAZ_Area_Referencia.ToList().Where(x => x.estatus == true && areas.Select(y => y.id).Contains(x.areaID)).ToList();

            foreach (var refeArea in referenciasAreas)
            {
                var extension = refeArea.ruta.Split('.')[1];

                if (File.Exists(refeArea.ruta))
                {
                    using (var fileStream = new FileStream(refeArea.ruta, FileMode.Open, FileAccess.Read))
                    {
                        byte[] arrBytes = ReadFully(fileStream);

                        var cadena64 = Convert.ToBase64String(arrBytes);

                        arr64.Add("data:image/" + extension + ";base64, " + cadena64);
                    }
                }
            }

            return arr64;
        }
        #endregion

        #region USUARIOS   
        public void GuardarUsuario(string nombre, string apellidoPaterno, string apellidoMaterno, string correo, string usuario, int cuadrillaID)
        {
            try
            {
                tblMAZ_Usuario_Cuadrilla newUsu = new tblMAZ_Usuario_Cuadrilla();

                newUsu.nombre = nombre;
                newUsu.apellidoPaterno = apellidoPaterno;
                newUsu.apellidoMaterno = apellidoMaterno;
                newUsu.correo = correo;
                newUsu.nombreUsuario = usuario;

                newUsu.contrasena = "OgbHgloV%2bXwb%2fQFYQhyppg%3d%3d";

                newUsu.cuadrillaID = cuadrillaID;
                newUsu.nivel = cuadrillaID != 0 ? 3 : 2;

                var ordenUltimo = _context.tblMAZ_Usuario_Cuadrilla.Where(x => x.estatus == true && x.cuadrillaID == cuadrillaID).ToList();

                newUsu.orden = ordenUltimo.Count > 0 ? ordenUltimo.Select(x => x.orden).Max() + 1 : 1;
                newUsu.estatus = true;

                _context.tblMAZ_Usuario_Cuadrilla.Add(newUsu);
                _context.SaveChanges();
            }
            catch (Exception) { }
        }
        public void EditarUsuario(int id, string nombre, string apellidoPaterno, string apellidoMaterno, string correo, string usuario, int cuadrillaID)
        {
            try
            {
                var usu = _context.tblMAZ_Usuario_Cuadrilla.Where(x => x.estatus == true && x.id == id).FirstOrDefault();

                if (usu != null)
                {
                    usu.nombre = nombre;
                    usu.apellidoPaterno = apellidoPaterno;
                    usu.apellidoMaterno = apellidoMaterno;
                    usu.correo = correo;
                    usu.nombreUsuario = usuario;
                    usu.cuadrillaID = cuadrillaID;

                    _context.Entry(usu).State = System.Data.Entity.EntityState.Modified;
                    _context.SaveChanges();
                }
            }
            catch (Exception) { }
        }
        public List<UsuarioMAZDADTO> getUsuarios(string usuario, int cuadrillaID)
        {
            var nombreFiltrado = usuario.Replace(" ", "").ToUpper();

            var list = _context.tblMAZ_Usuario_Cuadrilla.Where(x => x.estatus == true && (x.nombre + x.apellidoPaterno + x.apellidoMaterno).ToUpper().Contains(nombreFiltrado) && (cuadrillaID != 0 ? x.cuadrillaID == cuadrillaID : true)).Select(y => new UsuarioMAZDADTO
            {
                id = y.id,
                nombre = y.nombre,
                apellidoPaterno = y.apellidoPaterno,
                apellidoMaterno = y.apellidoMaterno,
                nombreCompleto = y.nombre + " " + y.apellidoPaterno + " " + y.apellidoMaterno,
                correo = y.correo,
                nombreUsuario = y.nombreUsuario,
                cuadrillaID = y.cuadrillaID,
                cuadrilla = _context.tblMAZ_Cuadrilla.Where(w => w.id == y.cuadrillaID).Select(z => z.descripcion).FirstOrDefault(),
                nivel = y.nivel,
                orden = y.orden
            }).ToList();

            return list;
        }
        public UsuarioMAZDADTO getUsuario(int id)
        {
            var usuario = _context.tblMAZ_Usuario_Cuadrilla.Where(x => x.estatus == true && x.id == id).Select(y => new UsuarioMAZDADTO
            {
                id = y.id,
                nombre = y.nombre,
                apellidoPaterno = y.apellidoPaterno,
                apellidoMaterno = y.apellidoMaterno,
                nombreCompleto = y.nombre + " " + y.apellidoPaterno + " " + y.apellidoMaterno,
                correo = y.correo,
                nombreUsuario = y.nombreUsuario,
                cuadrillaID = y.cuadrillaID,
                cuadrilla = _context.tblMAZ_Cuadrilla.Where(w => w.id == y.cuadrillaID).Select(z => z.descripcion).FirstOrDefault()
            }).FirstOrDefault();

            return usuario;
        }
        public void RemoveUsuario(int id)
        {
            try
            {
                var usuario = _context.tblMAZ_Usuario_Cuadrilla.Where(x => x.estatus == true && x.id == id).FirstOrDefault();

                if (usuario != null)
                {
                    usuario.estatus = false;

                    _context.Entry(usuario).State = System.Data.Entity.EntityState.Modified;
                    _context.SaveChanges();
                }
            }
            catch (Exception) { }
        }
        public List<tblMAZ_Usuario_Cuadrilla> GetEmpleadoList()
        {
            var personal = _context.tblMAZ_Usuario_Cuadrilla.Where(x => x.estatus == true && x.cuadrillaID != 0).ToList();

            return personal;
        }
        #endregion

        #region PLAN MAESTRO
        public List<CuadrillaDTO> getPlanMaestro()
        {
            var planMaestro = _context.tblMAZ_Cuadrilla.Where(x => x.estatus == true).Select(x => new CuadrillaDTO
            {
                id = x.id,
                descripcion = x.descripcion
            }).ToList();

            foreach (var cua in planMaestro)
            {
                cua.areas = _context.tblMAZ_Area.Where(x => x.estatus == true && x.cuadrillaID == cua.id).Select(x => new AreaDTO
                {
                    id = x.id,
                    descripcion = x.descripcion,
                    cuadrillaID = cua.id,
                    cuadrilla = cua.descripcion
                }).OrderBy(y => y.id).ToList();

                foreach (var ar in cua.areas)
                {
                    ar.actividades = _context.tblMAZ_Actividad.Where(x => x.estatus == true && x.areaID == ar.id).Select(x => new ActividadDTO
                    {
                        id = x.id,
                        descripcion = x.descripcion,
                        periodo = x.periodo,
                        periodoDesc = ((PeriodoEnum)x.periodo).ToString(),
                        areaID = ar.id,
                        area = ar.descripcion,
                        cuadrillaID = cua.id,
                        cuadrilla = cua.descripcion
                    }).OrderBy(y => y.periodo).ToList();
                }
            }

            return planMaestro;
        }
        public List<PlanMaestroDTO> getPlanMaestroOrdenado(List<int> arrCuadrillas, List<int> arrPeriodos, List<string> arrAreas, List<string> arrActividades, List<int> arrMeses)
        {
            var cuadrillas = _context.tblMAZ_Cuadrilla.Where(x => x.estatus == true).ToList();
            var areas = _context.tblMAZ_Area.Where(x => x.estatus == true).ToList();
            var actividades = _context.tblMAZ_Actividad.Where(x => x.estatus == true).ToList();

            var areasIDCuadrillas = arrCuadrillas != null ? areas.Where(x => arrCuadrillas.Contains(x.cuadrillaID)).Select(y => y.id).ToList() : null;
            var areasCuadrillas = arrAreas != null ? areas.Where(x => arrAreas.Contains(x.descripcion)).Select(y => y.id).ToList() : null;

            var planesMes = _context.tblMAZ_PlanMes.Where(x => x.estatus == true).ToList();
            var planesMesCuadrillas = _context.tblMAZ_Cuadrilla.ToList().Where(x => x.estatus == true && planesMes.Select(y => y.cuadrillaID).Contains(x.id)).ToList();
            var planesMesAreas = planesMesCuadrillas != null ? _context.tblMAZ_Area.ToList().Where(x => x.estatus == true && planesMesCuadrillas.Select(y => y.id).Contains(x.cuadrillaID)).ToList() : null;
            var planesMesActividades = planesMesAreas != null ? _context.tblMAZ_Actividad.ToList().Where(x => x.estatus == true && planesMesAreas.Select(y => y.id).Contains(x.areaID)).ToList() : null;

            Dictionary<int, List<int>> dict = new Dictionary<int, List<int>>();

            dict.Add(1, new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 });
            dict.Add(2, new List<int> { 1, 3, 5, 7, 9, 11 });
            dict.Add(3, new List<int> { 3, 6, 9, 12 });
            dict.Add(4, new List<int> { 4, 10 });
            dict.Add(5, new List<int> { 9 });

            var planMaestroOrdenado = actividades.Where(x =>
                (areasIDCuadrillas != null ? areasIDCuadrillas.Contains(x.areaID) : true) &&
                (arrPeriodos != null ? arrPeriodos.Contains(x.periodo) : true) &&
                (areasCuadrillas != null ? areasCuadrillas.Contains(x.areaID) : true) &&
                (arrActividades != null ? arrActividades.Contains(x.descripcion.Trim()) : true)
                ).Select(y => new PlanMaestroDTO
                {
                    id = y.id,
                    descripcion = y.descripcion,
                    periodo = y.periodo,
                    periodoDesc = ((PeriodoEnum)y.periodo).ToString(),
                    areaID = y.areaID,
                    area = areas.Where(z => z.id == y.areaID).Select(w => w.descripcion).FirstOrDefault(),
                    cuadrillaID = areas.Where(z => z.id == y.areaID).Select(w => w.cuadrillaID).FirstOrDefault(),
                    cuadrilla = cuadrillas.Where(z => z.id == (areas.Where(w => w.id == y.areaID).Select(q => q.cuadrillaID).FirstOrDefault())).Select(r => r.descripcion).FirstOrDefault(),
                    mes1 = planesMes.Count > 0 ? planesMes.Where(z => z.estatus == true && z.cuadrillaID == (areas.Where(q => q.id == y.areaID).Select(w => w.cuadrillaID).FirstOrDefault()) && z.periodo == y.periodo && z.mes == 1).FirstOrDefault() != null ? true : false : false,
                    mes2 = planesMes.Count > 0 ? planesMes.Where(z => z.estatus == true && z.cuadrillaID == (areas.Where(q => q.id == y.areaID).Select(w => w.cuadrillaID).FirstOrDefault()) && z.periodo == y.periodo && z.mes == 2).FirstOrDefault() != null ? true : false : false,
                    mes3 = planesMes.Count > 0 ? planesMes.Where(z => z.estatus == true && z.cuadrillaID == (areas.Where(q => q.id == y.areaID).Select(w => w.cuadrillaID).FirstOrDefault()) && z.periodo == y.periodo && z.mes == 3).FirstOrDefault() != null ? true : false : false,
                    mes4 = planesMes.Count > 0 ? planesMes.Where(z => z.estatus == true && z.cuadrillaID == (areas.Where(q => q.id == y.areaID).Select(w => w.cuadrillaID).FirstOrDefault()) && z.periodo == y.periodo && z.mes == 4).FirstOrDefault() != null ? true : false : false,
                    mes5 = planesMes.Count > 0 ? planesMes.Where(z => z.estatus == true && z.cuadrillaID == (areas.Where(q => q.id == y.areaID).Select(w => w.cuadrillaID).FirstOrDefault()) && z.periodo == y.periodo && z.mes == 5).FirstOrDefault() != null ? true : false : false,
                    mes6 = planesMes.Count > 0 ? planesMes.Where(z => z.estatus == true && z.cuadrillaID == (areas.Where(q => q.id == y.areaID).Select(w => w.cuadrillaID).FirstOrDefault()) && z.periodo == y.periodo && z.mes == 6).FirstOrDefault() != null ? true : false : false,
                    mes7 = planesMes.Count > 0 ? planesMes.Where(z => z.estatus == true && z.cuadrillaID == (areas.Where(q => q.id == y.areaID).Select(w => w.cuadrillaID).FirstOrDefault()) && z.periodo == y.periodo && z.mes == 7).FirstOrDefault() != null ? true : false : false,
                    mes8 = planesMes.Count > 0 ? planesMes.Where(z => z.estatus == true && z.cuadrillaID == (areas.Where(q => q.id == y.areaID).Select(w => w.cuadrillaID).FirstOrDefault()) && z.periodo == y.periodo && z.mes == 8).FirstOrDefault() != null ? true : false : false,
                    mes9 = planesMes.Count > 0 ? planesMes.Where(z => z.estatus == true && z.cuadrillaID == (areas.Where(q => q.id == y.areaID).Select(w => w.cuadrillaID).FirstOrDefault()) && z.periodo == y.periodo && z.mes == 9).FirstOrDefault() != null ? true : false : false,
                    mes10 = planesMes.Count > 0 ? planesMes.Where(z => z.estatus == true && z.cuadrillaID == (areas.Where(q => q.id == y.areaID).Select(w => w.cuadrillaID).FirstOrDefault()) && z.periodo == y.periodo && z.mes == 10).FirstOrDefault() != null ? true : false : false,
                    mes11 = planesMes.Count > 0 ? planesMes.Where(z => z.estatus == true && z.cuadrillaID == (areas.Where(q => q.id == y.areaID).Select(w => w.cuadrillaID).FirstOrDefault()) && z.periodo == y.periodo && z.mes == 11).FirstOrDefault() != null ? true : false : false,
                    mes12 = planesMes.Count > 0 ? planesMes.Where(z => z.estatus == true && z.cuadrillaID == (areas.Where(q => q.id == y.areaID).Select(w => w.cuadrillaID).FirstOrDefault()) && z.periodo == y.periodo && z.mes == 12).FirstOrDefault() != null ? true : false : false,
                    mesesFiltro = arrMeses,
                    periodoFiltro = dict.Where(w => w.Key == y.periodo).Select(q => q.Value).FirstOrDefault()
                }).ToList();

            return planMaestroOrdenado;
        }
        public PlanMesDTO getPlanMes(int cuadrillaID, int periodo, int mes)
        {
            var plan = _context.tblMAZ_PlanMes.Where(x => x.estatus == true && x.cuadrillaID == cuadrillaID && x.periodo == periodo && x.mes == mes).FirstOrDefault();

            if (plan != null)
            {
                var dias = _context.tblMAZ_PlanMes_Detalle_Dia.Where(x => x.estatus == true).ToList();
                var revAC = _context.tblMAZ_Revision_AC.Where(x => x.estatus == true).ToList();
                var revCua = _context.tblMAZ_Revision_Cuadrilla.Where(x => x.estatus == true).ToList();

                var detalles = _context.tblMAZ_PlanMes_Detalle.ToList().Where(x => x.estatus == true && x.planMesID == plan.id).Select(y => new PlanMes_DetalleDTO
                {
                    id = y.id,
                    planMesID = y.planMesID,
                    tipo = y.tipo,
                    equipoID = y.equipoID,
                    //equipo = y.equipoAreaDesc,
                    dias = dias.Where(z => z.planMesDetalleID == y.id).Select(w => w.dia).ToList(),
                    checkRev = (revAC.Where(q => q.planMesDetalleID == y.id).FirstOrDefault() != null || revCua.Where(w => w.planMesDetalleID == y.id).FirstOrDefault() != null) ? true : false,
                    revisionID = revAC.Where(q => q.planMesDetalleID == y.id).FirstOrDefault() != null ? revAC.Where(q => q.planMesDetalleID == y.id).FirstOrDefault().id :
                                 revCua.Where(w => w.planMesDetalleID == y.id).FirstOrDefault() != null ? revCua.Where(w => w.planMesDetalleID == y.id).FirstOrDefault().id : 0
                }).ToList();

                var obj = new PlanMesDTO
                {
                    id = plan.id,
                    cuadrillaID = plan.cuadrillaID,
                    periodo = plan.periodo,
                    mes = plan.mes,
                    anio = plan.anio,
                    detalle = detalles
                };

                return obj;
            }
            else
            {
                return null;
            }
        }
        public List<PlanMesDTO> getPlanMesGeneral(int mes)
        {
            var planes = _context.tblMAZ_PlanMes.Where(x => x.estatus == true && x.mes == mes).ToList();

            List<PlanMesDTO> listObj = new List<PlanMesDTO>();

            if (planes.Count > 0)
            {
                var dias = _context.tblMAZ_PlanMes_Detalle_Dia.Where(x => x.estatus == true).ToList();
                var revAC = _context.tblMAZ_Revision_AC.Where(x => x.estatus == true).ToList();
                var revCua = _context.tblMAZ_Revision_Cuadrilla.Where(x => x.estatus == true).ToList();

                var cuadrillas = _context.tblMAZ_Cuadrilla.Where(x => x.estatus == true).ToList();

                foreach (var plan in planes)
                {
                    var detalles = _context.tblMAZ_PlanMes_Detalle.ToList().Where(x => x.estatus == true && x.planMesID == plan.id).Select(y => new PlanMes_DetalleDTO
                    {
                        id = y.id,
                        planMesID = y.planMesID,
                        tipo = y.tipo,
                        cuadrilla = cuadrillas.Where(z => z.id == plan.cuadrillaID).Select(t => t.descripcion).FirstOrDefault(),
                        periodo = ((PeriodoEnum)plan.periodo).ToString(),
                        equipoID = y.equipoID,
                        dias = dias.Where(z => z.planMesDetalleID == y.id).Select(w => w.dia).ToList(),
                        checkRev = (revAC.Where(q => q.planMesDetalleID == y.id).FirstOrDefault() != null || revCua.Where(w => w.planMesDetalleID == y.id).FirstOrDefault() != null) ? true : false,
                        revisionID = revAC.Where(q => q.planMesDetalleID == y.id).FirstOrDefault() != null ? revAC.Where(q => q.planMesDetalleID == y.id).FirstOrDefault().id :
                                     revCua.Where(w => w.planMesDetalleID == y.id).FirstOrDefault() != null ? revCua.Where(w => w.planMesDetalleID == y.id).FirstOrDefault().id : 0
                    }).ToList();

                    listObj.Add(new PlanMesDTO
                    {
                        id = plan.id,
                        cuadrillaID = plan.cuadrillaID,
                        cuadrilla = cuadrillas.Where(z => z.id == plan.cuadrillaID).Select(t => t.descripcion).FirstOrDefault(),
                        periodo = plan.periodo,
                        periodoDesc = ((PeriodoEnum)plan.periodo).ToString(),
                        mes = plan.mes,
                        anio = plan.anio,
                        detalle = detalles
                    });
                }

                return listObj;
            }
            else
            {
                return null;
            }
        }
        public List<PlanMesDTO> getPlanMesEquipo(int cuadrillaID, int periodo, List<int> equipoID)
        {
            var plan = _context.tblMAZ_PlanMes.Where(x => x.estatus == true && x.cuadrillaID == cuadrillaID && x.periodo == periodo && x.anio == DateTime.Now.Year).ToList();
            var planID = plan.Select(x => x.id).ToList();

            if (plan != null && plan.Count > 0)
            {
                List<PlanMesDTO> listObj = new List<PlanMesDTO>();

                var dias = _context.tblMAZ_PlanMes_Detalle_Dia.Where(x => x.estatus == true).ToList();
                var revAC = _context.tblMAZ_Revision_AC.Where(x => x.estatus == true).ToList();
                var revCua = _context.tblMAZ_Revision_Cuadrilla.Where(x => x.estatus == true).ToList();

                var detalles = _context.tblMAZ_PlanMes_Detalle.ToList().Where(x => x.estatus == true && planID.Contains(x.planMesID) && equipoID.Contains(x.equipoID)).Select(y => new PlanMes_DetalleDTO
                {
                    id = y.id,
                    planMesID = y.planMesID,
                    tipo = y.tipo,
                    equipoID = y.equipoID,
                    dias = dias.Where(z => z.planMesDetalleID == y.id).Select(w => w.dia).ToList(),
                    checkRev = (revAC.Where(q => q.planMesDetalleID == y.id).FirstOrDefault() != null || revCua.Where(w => w.planMesDetalleID == y.id).FirstOrDefault() != null) ? true : false,
                    revisionID = revAC.Where(q => q.planMesDetalleID == y.id).FirstOrDefault() != null ? revAC.Where(q => q.planMesDetalleID == y.id).FirstOrDefault().id :
                                 revCua.Where(w => w.planMesDetalleID == y.id).FirstOrDefault() != null ? revCua.Where(w => w.planMesDetalleID == y.id).FirstOrDefault().id : 0
                }).ToList();

                var listP = _context.tblMAZ_PlanMes.ToList().Where(x =>
                        (x.estatus == true) &&
                        (planID.Contains(x.id))

                        ).Select(y => new PlanMesDTO
                        {
                            id = y.id,
                            cuadrillaID = y.cuadrillaID,
                            periodo = y.periodo,
                            mes = y.mes,
                            anio = y.anio,
                            detalle = detalles.Where(z => z.planMesID == y.id).ToList()
                        }).OrderBy(y => y.mes).ToList();
                return listP;
            }
            else
            {
                return null;
            }
        }
        public void GuardarPlanMes(PlanMesDTO plan)
        {
            var checkPlan = _context.tblMAZ_PlanMes.Where(x => x.estatus == true && x.cuadrillaID == plan.cuadrillaID && x.periodo == plan.periodo && x.mes == plan.mes && x.anio == plan.anio).FirstOrDefault();

            if (checkPlan == null)
            {
                if (plan.detalle.Count == 1 && plan.detalle.Select(x => x.equipoID).FirstOrDefault() == 0)
                {

                }
                else
                {
                    saveTablasPlanMes(plan);
                }
            }
            else
            {
                if (plan.detalle.Count == 1 && plan.detalle.Select(x => x.equipoID).FirstOrDefault() == 0)
                {
                    checkPlan.estatus = false;

                    _context.Entry(checkPlan).State = System.Data.Entity.EntityState.Modified;
                    _context.SaveChanges();

                    var detalle = _context.tblMAZ_PlanMes_Detalle.Where(x => x.estatus == true && x.planMesID == checkPlan.id).ToList();

                    foreach (var det in detalle)
                    {
                        det.estatus = false;

                        _context.Entry(det).State = System.Data.Entity.EntityState.Modified;
                        _context.SaveChanges();

                        var dias = _context.tblMAZ_PlanMes_Detalle_Dia.Where(x => x.estatus == true && x.planMesDetalleID == det.id).ToList();

                        foreach (var dia in dias)
                        {
                            dia.estatus = false;

                            _context.Entry(dia).State = System.Data.Entity.EntityState.Modified;
                            _context.SaveChanges();
                        }
                    }
                }
                else
                {
                    var detalle = _context.tblMAZ_PlanMes_Detalle.Where(x => x.estatus == true && x.planMesID == checkPlan.id).ToList();

                    foreach (var det in detalle)
                    {
                        det.estatus = false;

                        _context.Entry(det).State = System.Data.Entity.EntityState.Modified;
                        _context.SaveChanges();

                        var dias = _context.tblMAZ_PlanMes_Detalle_Dia.Where(x => x.estatus == true && x.planMesDetalleID == det.id).ToList();

                        foreach (var dia in dias)
                        {
                            dia.estatus = false;

                            _context.Entry(dia).State = System.Data.Entity.EntityState.Modified;
                            _context.SaveChanges();
                        }
                    }

                    updateTablasPlanMes(plan, checkPlan.id);
                }
            }
        }
        void saveTablasPlanMes(PlanMesDTO plan)
        {
            tblMAZ_PlanMes planMes = new tblMAZ_PlanMes();

            planMes.cuadrillaID = plan.cuadrillaID;
            planMes.periodo = plan.periodo;
            planMes.mes = plan.mes;
            planMes.anio = plan.anio;
            planMes.estatus = true;

            _context.tblMAZ_PlanMes.Add(planMes);
            _context.SaveChanges();

            foreach (var det in plan.detalle)
            {
                tblMAZ_PlanMes_Detalle planMesDet = new tblMAZ_PlanMes_Detalle();

                planMesDet.planMesID = planMes.id;
                planMesDet.tipo = det.tipo;
                planMesDet.equipoID = det.equipoID;
                planMesDet.estatus = true;

                _context.tblMAZ_PlanMes_Detalle.Add(planMesDet);
                _context.SaveChanges();

                foreach (var dia in det.dias)
                {
                    tblMAZ_PlanMes_Detalle_Dia planMesDetDia = new tblMAZ_PlanMes_Detalle_Dia();

                    planMesDetDia.planMesDetalleID = planMesDet.id;
                    planMesDetDia.dia = dia;
                    planMesDetDia.estatus = true;

                    _context.tblMAZ_PlanMes_Detalle_Dia.Add(planMesDetDia);
                    _context.SaveChanges();
                }
            }
        }
        void updateTablasPlanMes(PlanMesDTO plan, int planMesID)
        {
            foreach (var det in plan.detalle)
            {
                tblMAZ_PlanMes_Detalle planMesDet = new tblMAZ_PlanMes_Detalle();

                planMesDet.planMesID = planMesID;
                planMesDet.tipo = det.tipo;
                planMesDet.equipoID = det.equipoID;
                planMesDet.estatus = true;

                _context.tblMAZ_PlanMes_Detalle.Add(planMesDet);
                _context.SaveChanges();

                foreach (var dia in det.dias)
                {
                    tblMAZ_PlanMes_Detalle_Dia planMesDetDia = new tblMAZ_PlanMes_Detalle_Dia();

                    planMesDetDia.planMesDetalleID = planMesDet.id;
                    planMesDetDia.dia = dia;
                    planMesDetDia.estatus = true;

                    _context.tblMAZ_PlanMes_Detalle_Dia.Add(planMesDetDia);
                    _context.SaveChanges();
                }
            }
        }
        public List<ComboDTO> getAllDays(int year)
        {
            try
            {
                var DiasM = new List<KeyValuePair<int, string[]>>();
                var start = new DateTime(year, 1, 1);

                while (start.Year == year)
                {
                    DiasM.Add(new KeyValuePair<int, string[]>(start.Day,
                        new string[] { DateTimeFormatInfo.CurrentInfo.GetDayName(start.DayOfWeek).ToString().ToUpper().Substring(0, 1), start.Month.ToString() }));
                    start = start.AddDays(1);
                }

                var daysM = DiasM.Select(x => new ComboDTO
                {
                    Value = x.Key.ToString(),
                    Text = x.Value[0].ToString(),
                    Prefijo = x.Value[1].ToString()
                }).ToList();

                return daysM;

            }
            catch (Exception e)
            {
                return null;
            }
        }
        public MemoryStream GenerarPlanExcel(List<EquipoDTO> equipos, List<ComboDTO> dias, List<PlanMesDTO> planMesDetalle, int periodoID, List<ReporteDiarioDTO> revision)
        {
            try
            {
                using (ExcelPackage package = new ExcelPackage())
                {
                    string[] months = new string[] { "Enero", "Febrero", "Marzo", "Abril", "Mayo", "Junio", "Julio", "Agosto", "Septiembre", "Octubre", "Noviembre", "Diciembre" };
                    var mMayor = package.Workbook.Worksheets.Add("Desarrollo Mantenimiento");

                    //ENCABEZADO LOGO
                    var byteImg = Convert.FromBase64String("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");
                    Image img;
                    using (var stream = new MemoryStream(byteImg))
                    {
                        img = Image.FromStream(stream);
                    }
                                                    
                    ExcelPicture pic = mMayor.Drawings.AddPicture("logo", img);
                    pic.SetPosition(2, 0, 0, 0);
                    pic.SetSize(60);

                    mMayor.Cells[1, 1, 9, 5].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    mMayor.Cells[6, 6, 9, 7].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    mMayor.Cells["F1:G1"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    mMayor.Cells[1, 1, 9, 5].Style.Fill.BackgroundColor.SetColor(0, 255, 255, 255);
                    mMayor.Cells[6, 6, 9, 7].Style.Fill.BackgroundColor.SetColor(0, 255, 255, 255);
                    mMayor.Cells["F1:G1"].Style.Fill.BackgroundColor.SetColor(0, 255, 255, 255);

                    mMayor.Cells["F2"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    mMayor.Cells["F3"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    mMayor.Cells["F4"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    mMayor.Cells["F5"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    mMayor.Cells["F6"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    mMayor.Cells["F7"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    mMayor.Cells["F8"].Style.Fill.PatternType = ExcelFillStyle.Solid;

                    mMayor.Cells["F2"].Style.Fill.BackgroundColor.SetColor(0, 0, 176, 240);
                    mMayor.Cells["F2"].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    mMayor.Cells["G2"].Value = "Mensual";
                    mMayor.Cells["G2"].Style.Font.Size = 7;
                    mMayor.Cells["G2"].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    mMayor.Cells["G2"].Style.Border.Bottom.Style = ExcelBorderStyle.Dotted;

                    mMayor.Cells["F3"].Style.Fill.BackgroundColor.SetColor(0, 112, 48, 160);
                    mMayor.Cells["F3"].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    mMayor.Cells["G3"].Value = "Bimestral";
                    mMayor.Cells["G3"].Style.Font.Size = 7;
                    mMayor.Cells["G3"].Style.Border.Bottom.Style = ExcelBorderStyle.Dotted;

                    mMayor.Cells["F4"].Style.Fill.BackgroundColor.SetColor(0, 128, 128, 128);
                    mMayor.Cells["F4"].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    mMayor.Cells["F4"].AutoFitColumns();
                    mMayor.Cells["G4"].Value = "Trimestral";
                    mMayor.Cells["G4"].Style.Font.Size = 7;
                    mMayor.Cells["G4"].Style.Border.Bottom.Style = ExcelBorderStyle.Dotted;

                    mMayor.Cells["F5"].Style.Fill.BackgroundColor.SetColor(0, 237, 125, 49);
                    mMayor.Cells["F5"].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    mMayor.Cells["F5"].AutoFitColumns();
                    mMayor.Cells["G5"].Value = "Semestral";
                    mMayor.Cells["G5"].Style.Font.Size = 7;
                    mMayor.Cells["G5"].Style.Border.Bottom.Style = ExcelBorderStyle.Dotted;

                    mMayor.Cells["F6"].Style.Fill.BackgroundColor.SetColor(0, 0, 0, 0);
                    mMayor.Cells["F6"].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    mMayor.Cells["F6"].AutoFitColumns();
                    mMayor.Cells["G6"].Value = "Anual";
                    mMayor.Cells["G6"].Style.Font.Size = 7;
                    mMayor.Cells["G6"].Style.Border.Bottom.Style = ExcelBorderStyle.Dotted;

                    mMayor.Cells["F7"].Style.Fill.BackgroundColor.SetColor(0, 11, 102, 35);
                    mMayor.Cells["F7"].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    mMayor.Cells["F7"].AutoFitColumns();
                    mMayor.Cells["G7"].Value = "Se realizó";
                    mMayor.Cells["G7"].Style.Font.Size = 7;
                    mMayor.Cells["G7"].Style.Border.Bottom.Style = ExcelBorderStyle.Dotted;

                    mMayor.Cells["F8"].Style.Fill.BackgroundColor.SetColor(0, 255, 255, 0);
                    mMayor.Cells["F8"].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    mMayor.Cells["F8"].AutoFitColumns();
                    mMayor.Cells["G8"].Value = "Reprogramación";
                    mMayor.Cells["G8"].Style.Font.Size = 7;
                    mMayor.Cells["G8"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;


                    mMayor.Column(6).Width = 3;
                    mMayor.Row(2).Height = 10;
                    mMayor.Row(3).Height = 10;
                    mMayor.Row(4).Height = 10;
                    mMayor.Row(5).Height = 10;
                    mMayor.Row(6).Height = 10;
                    mMayor.Row(7).Height = 10;
                    mMayor.Row(8).Height = 10;

                    //HEADER TABLA EQUIPOS
                    mMayor.Cells["A10"].Value = "Equipo";

                    mMayor.Cells["B10"].Value = "Cantidad";
                    mMayor.Cells["B10"].Style.Border.BorderAround(ExcelBorderStyle.Thin);

                    mMayor.Cells[10, 3, 11, 4].Merge = true;
                    mMayor.Cells[10, 3, 11, 4].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    mMayor.Cells["C10"].Value = "Tipo/Caracteristicas";

                    mMayor.Cells[10, 5, 11, 7].Merge = true;
                    mMayor.Cells[10, 5, 11, 7].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    mMayor.Cells["E10"].Value = "Subárea";

                    mMayor.Cells["A10:A11"].Merge = true;
                    mMayor.Cells["A10:A11"].Style.Border.BorderAround(ExcelBorderStyle.Thin);

                    mMayor.Cells["B10:B11"].Merge = true;
                    mMayor.Cells["B10:B11"].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    mMayor.View.FreezePanes(12, 8);

                    using (var rng = mMayor.Cells["A10:G10"])
                    {
                        rng.Style.Font.Bold = true;
                        rng.Style.Font.Color.SetColor(Color.Black);
                        rng.Style.WrapText = true;
                        rng.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        rng.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        rng.Style.Fill.PatternType = ExcelFillStyle.Solid;
                        rng.Style.Fill.BackgroundColor.SetColor(0, 255, 192, 0);
                    }

                    // CABECERA MESES
                    int lastColumn = 7;
                    mMayor.Cells[1, 8, 5, lastColumn + dias.Count].Merge = true;
                    mMayor.Cells[1, 8, 5, lastColumn + dias.Count].Value = "PLAN MAESTRO";
                    mMayor.Cells[1, 8, 5, lastColumn + dias.Count].Style.Font.Size = 38;
                    mMayor.Cells[1, 8, 5, lastColumn + dias.Count].Style.Font.Bold = true;
                    mMayor.Cells[1, 8, 5, lastColumn + dias.Count].Style.Font.Color.SetColor(Color.Black);
                    mMayor.Cells[1, 8, 5, lastColumn + dias.Count].Style.WrapText = true;
                    mMayor.Cells[1, 8, 5, lastColumn + dias.Count].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    mMayor.Cells[1, 8, 5, lastColumn + dias.Count].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    mMayor.Cells[1, 8, 5, lastColumn + dias.Count].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    mMayor.Cells[1, 8, 5, lastColumn + dias.Count].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    mMayor.Cells[1, 8, 5, lastColumn + dias.Count].Style.Fill.BackgroundColor.SetColor(0, 237, 125, 49);

                    mMayor.Cells[6, 8, 7, lastColumn + dias.Count].Merge = true;
                    mMayor.Cells[6, 8, 7, lastColumn + dias.Count].Value = "FY" + DateTime.Now.Year;
                    mMayor.Cells[6, 8, 7, lastColumn + dias.Count].Style.Font.Size = 25;
                    mMayor.Cells[6, 8, 7, lastColumn + dias.Count].Style.Font.Bold = true;
                    mMayor.Cells[6, 8, 7, lastColumn + dias.Count].Style.Font.Color.SetColor(Color.Black);
                    mMayor.Cells[6, 8, 7, lastColumn + dias.Count].Style.WrapText = true;
                    mMayor.Cells[6, 8, 7, lastColumn + dias.Count].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    mMayor.Cells[6, 8, 7, lastColumn + dias.Count].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    mMayor.Cells[6, 8, 7, lastColumn + dias.Count].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    mMayor.Cells[6, 8, 7, lastColumn + dias.Count].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    mMayor.Cells[6, 8, 7, lastColumn + dias.Count].Style.Fill.BackgroundColor.SetColor(0, 255, 192, 0);

                    //CABECERA MESES
                    int starCellMes = 8;
                    int DaysInMonth = 0;
                    for (int i = 0; i < months.Length; i++)
                    {
                        DaysInMonth = DateTime.DaysInMonth(DateTime.Now.Year, i + 1);

                        mMayor.Cells[8, starCellMes].Value = months[i].ToUpper();
                        mMayor.Cells[8, starCellMes, 9, starCellMes + DaysInMonth - 1].Merge = true;
                        mMayor.Cells[8, starCellMes, 8, starCellMes + DaysInMonth - 1].Style.Font.Bold = true;
                        mMayor.Cells[8, starCellMes, 8, starCellMes + DaysInMonth - 1].Style.Font.Size = 18;
                        mMayor.Cells[8, starCellMes, 8, starCellMes + DaysInMonth - 1].Style.Font.Color.SetColor(Color.Black);
                        mMayor.Cells[8, starCellMes, 8, starCellMes + DaysInMonth - 1].Style.WrapText = true;
                        mMayor.Cells[8, starCellMes, 8, starCellMes + DaysInMonth - 1].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        mMayor.Cells[8, starCellMes, 8, starCellMes + DaysInMonth - 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        mMayor.Cells[8, starCellMes, 8, starCellMes + DaysInMonth - 1].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        mMayor.Cells[8, starCellMes, 9, starCellMes + DaysInMonth - 1].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                        mMayor.Cells[8, starCellMes, 8, starCellMes + DaysInMonth - 1].Style.Fill.BackgroundColor.SetColor(0, 255, 217, 102);

                        starCellMes += DateTime.DaysInMonth(DateTime.Now.Year, i + 1);
                    }

                    int starCellDias = 8;
                    for (int i = 0; i < dias.Count; i++)
                    {
                        //LETRA
                        mMayor.Cells[10, starCellDias].Value = dias[i].Text.ToUpper();
                        mMayor.Cells[10, starCellDias].Style.Font.Bold = true;
                        mMayor.Cells[10, starCellDias].Style.Font.Color.SetColor(Color.Black);
                        mMayor.Cells[10, starCellDias].Style.WrapText = true;
                        mMayor.Cells[10, starCellDias].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        mMayor.Cells[10, starCellDias].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        mMayor.Cells[10, starCellDias].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        mMayor.Cells[10, starCellDias].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                        mMayor.Cells[10, starCellDias].Style.Fill.BackgroundColor.SetColor(0, 255, 217, 102);


                        //NUMERO
                        mMayor.Cells[11, starCellDias].Value = dias[i].Value;
                        mMayor.Cells[11, starCellDias].Style.Font.Bold = true;
                        mMayor.Cells[11, starCellDias].Style.Font.Color.SetColor(Color.Black);
                        mMayor.Cells[11, starCellDias].Style.WrapText = true;
                        mMayor.Cells[11, starCellDias].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        mMayor.Cells[11, starCellDias].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        mMayor.Cells[11, starCellDias].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        mMayor.Cells[11, starCellDias].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                        mMayor.Cells[11, starCellDias].Style.Fill.BackgroundColor.SetColor(0, 255, 255, 255);

                        mMayor.Column(starCellDias).Width = 3;
                        starCellDias++;
                    }

                    //EQUIPOS DETALLE
                    int startRowDetalle = 12;
                    int rowMerge = 0;
                    string area = "";
                    foreach (var equipo in equipos)
                    {
                        rowMerge = startRowDetalle + 2;
                        mMayor.Cells[startRowDetalle, 1, rowMerge, 1].Merge = true;
                        mMayor.Cells[startRowDetalle, 1, rowMerge, 1].AutoFitColumns();
                        mMayor.Cells[startRowDetalle, 1, rowMerge, 1].Value = equipo.descripcion;
                        mMayor.Cells[startRowDetalle, 1, rowMerge, 1].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                        mMayor.Cells[startRowDetalle, 1, rowMerge, 1].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                        mMayor.Cells[startRowDetalle, 2, rowMerge, 2].Merge = true;
                        mMayor.Cells[startRowDetalle, 2, rowMerge, 2].Value = equipo.cantidad;
                        mMayor.Cells[startRowDetalle, 2, rowMerge, 2].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        mMayor.Cells[startRowDetalle, 2, rowMerge, 2].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        mMayor.Cells[startRowDetalle, 2, rowMerge, 2].Style.Border.Right.Style = ExcelBorderStyle.Thin;

                        mMayor.Cells[startRowDetalle, 3, rowMerge, 4].Merge = true;
                        mMayor.Cells[startRowDetalle, 3, rowMerge, 4].AutoFitColumns();
                        mMayor.Cells[startRowDetalle, 3, rowMerge, 4].Value = equipo.caracteristicas;
                        mMayor.Cells[startRowDetalle, 3, rowMerge, 4].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                        mMayor.Cells[startRowDetalle, 3, rowMerge, 4].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                        mMayor.Cells[startRowDetalle, 5, rowMerge, 7].Merge = true;
                        mMayor.Cells[startRowDetalle, 5, rowMerge, 7].Value = equipo.subArea;
                        mMayor.Cells[startRowDetalle, 5, rowMerge, 7].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                        mMayor.Cells[startRowDetalle, 5, rowMerge, 7].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                        mMayor.Cells[startRowDetalle, 1, rowMerge, 7].Style.Border.BorderAround(ExcelBorderStyle.Thin);

                        startRowDetalle = rowMerge + 1;
                        area = equipo.area;
                    }

                    // DIAS DETALLE
                    int startRow = 12; //row donde inicia la informacion
                    int startColumn = 8;  //columna donde inician los dias
                    int startRowMerge = 0;  //row donde inicia cada equipo
                    foreach (var equipo in equipos)
                    {
                        startRowMerge = startRow + 2; // se le suma dos al inicio de cada equipo porque cada equipo toma 3 rows
                        mMayor.Cells[startRowMerge, startColumn, startRowMerge, 364 + startColumn].Style.Border.Bottom.Style = ExcelBorderStyle.Thin; //le agrega un borda al finalizar los rows de cada equipo

                        for (int k = 0; k < dias.Count; k++)
                        {

                            if (dias[k].Text == "D")
                            {
                                mMayor.Cells[startRow, startColumn, startRowMerge, startColumn].Style.Border.Top.Style = ExcelBorderStyle.Dotted;
                                mMayor.Cells[startRow, startColumn, startRowMerge, startColumn].Style.Border.Right.Style = ExcelBorderStyle.Medium;
                            }
                            else
                            {
                                mMayor.Cells[startRow, startColumn, startRowMerge, startColumn].Style.Border.Top.Style = ExcelBorderStyle.Dotted;
                                mMayor.Cells[startRow, startColumn, startRowMerge, startColumn].Style.Border.Right.Style = ExcelBorderStyle.Dotted;
                            }

                            //COMPARA LOS DIAS DEL PLAN MAESTRO DETALLE
                            if (planMesDetalle != null)
                            {
                                for (int j = 0; j < planMesDetalle.Count; j++)
                                {
                                    if (planMesDetalle[j].mes.ToString() == dias[k].Prefijo)
                                    {
                                        for (int i = 0; i < planMesDetalle[j].detalle.Count; i++)
                                        {
                                            if (planMesDetalle[j].detalle[i].equipoID == equipo.id)
                                            {
                                                if (planMesDetalle[j].detalle[i].dias.Contains(Int32.Parse(dias[k].Value)))
                                                {
                                                    switch (periodoID)
                                                    {
                                                        case 1:
                                                            mMayor.Cells[startRow, startColumn].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                                            mMayor.Cells[startRow, startColumn].Style.Fill.BackgroundColor.SetColor(0, 0, 176, 240);
                                                            break;
                                                        case 2:
                                                            mMayor.Cells[startRow, startColumn].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                                            mMayor.Cells[startRow, startColumn].Style.Fill.BackgroundColor.SetColor(0, 112, 48, 160);
                                                            break;
                                                        case 3:
                                                            mMayor.Cells[startRow, startColumn].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                                            mMayor.Cells[startRow, startColumn].Style.Fill.BackgroundColor.SetColor(0, 128, 128, 128);
                                                            break;
                                                        case 4:
                                                            mMayor.Cells[startRow, startColumn].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                                            mMayor.Cells[startRow, startColumn].Style.Fill.BackgroundColor.SetColor(0, 237, 125, 49);
                                                            break;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }

                            //COMPARA LOS DIAS DE REVISION
                            if (revision != null)
                            {
                                for (int j = 0; j < revision.Count; j++)
                                {
                                    if (revision[j].fechaCaptura.Month.ToString() == dias[k].Prefijo)
                                    {
                                        if (revision[j].fechaCaptura.Day.ToString() == dias[k].Value)
                                        {
                                            for (int i = 0; i < revision[j].equiposID.Count; i++)
                                            {
                                                if (revision[j].equiposID[i] == equipo.id)
                                                {
                                                    mMayor.Cells[startRow + 1, startColumn].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                                    mMayor.Cells[startRow + 1, startColumn].Style.Fill.BackgroundColor.SetColor(0, 11, 102, 35);
                                                }
                                            }
                                        }
                                    }

                                    if (revision[j].fechaReprogramacion != null && revision[j].fechaReprogramacion != "")
                                    {
                                        if (Convert.ToDateTime(revision[j].fechaReprogramacion).Month.ToString() == dias[k].Prefijo)
                                        {
                                            if (Convert.ToDateTime(revision[j].fechaReprogramacion).Day.ToString() == dias[k].Value)
                                            {
                                                for (int i = 0; i < revision[j].equiposID.Count; i++)
                                                {
                                                    if (revision[j].equiposID[i] == equipo.id)
                                                    {
                                                        mMayor.Cells[startRow + 2, startColumn].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                                        mMayor.Cells[startRow + 2, startColumn].Style.Fill.BackgroundColor.SetColor(0, 255, 255, 0);
                                                    }
                                                }
                                            }
                                        }

                                    }
                                }
                            }

                            startColumn++;
                        }

                        startRow = startRowMerge + 1; //se le asiga el ultimo row mas uno para que empiece otro equipo
                        startColumn = 8; // se reiniciar la columna al inicio de los dias
                    }

                    package.Compression = CompressionLevel.BestSpeed;
                    List<byte[]> lista = new List<byte[]>();
                    var bytes = new MemoryStream();
                    using (var exportData = new MemoryStream())
                    {
                        package.SaveAs(exportData);
                        bytes = exportData;
                    }
                    return bytes;
                }

            }
            catch (Exception e)
            {
                return null;
            }
        }
        public List<ReporteDiarioDTO> getRevisionActividadEquipo(List<EquipoDTO> equipos, int cuadrillaID, int areaID)
        {
            List<ReporteDiarioDTO> reporte = new List<ReporteDiarioDTO>();
            var revisionesAC = _context.tblMAZ_Revision_AC.ToList().Where(x => x.area == areaID).ToList();
            var revisionesCua = _context.tblMAZ_Revision_Cuadrilla.ToList().Where(x => x.cuadrillaID == cuadrillaID).ToList();
            //var planMesDetalles = _context.tblMAZ_PlanMes_Detalle.ToList();

            //REVISIONES AC
            foreach (var revAC in revisionesAC)
            {
                var revAcDet = _context.tblMAZ_Revision_AC_Detalle.Where(x => x.estatus == true && x.revisionID == revAC.id && x.realizo == true).ToList();
                foreach (var det in revAcDet)
                {
                    var rep = _context.tblMAZ_Reporte_Actividades.Where(x => x.estatus == true && x.revisionTipo == 1 && x.revisionDetalleID == det.id).FirstOrDefault();

                    if (rep != null)
                    {
                        var repEquipos = _context.tblMAZ_Reporte_Actividades_Equipo.Where(x => x.estatus == true && x.reporteActividadesID == rep.id).ToList();

                        reporte.Add(new ReporteDiarioDTO
                        {
                            equiposID = repEquipos.Count > 0 ? repEquipos.Select(x => x.equipoID).ToList() : null,
                            semaforo = rep.semaforo,
                            reprogramacion = rep.reprogramacion,
                            estatus = rep.estatusInfo,
                            fechaCaptura = revAC.fechaCaptura,
                            monthFechaCaptura = revAC.fechaCaptura.Month,
                            dayFechaCaptura = revAC.fechaCaptura.Day,
                            fechaReprogramacion = rep.fechaReprogramacion.ToString(),
                            monthFechaReprogramacion = rep.fechaReprogramacion != null ? rep.fechaReprogramacion.Value.Month : 0,
                            dayFechaReprogramacion = rep.fechaReprogramacion != null ? rep.fechaReprogramacion.Value.Day : 0
                        });
                    }
                    else
                    {
                        reporte.Add(new ReporteDiarioDTO
                        {
                            equiposID = equipos.Where(x => x.id == revAC.equipoID).Select(z => z.id).ToList(),
                            reprogramacion = det.reprogramacion,
                            estatus = det.estatusInfo,
                            fechaCaptura = revAC.fechaCaptura
                        });
                    }
                }
            }

            //REVISIONES CUADRILLAS
            foreach (var revCua in revisionesCua)
            {
                var revCuaDet = _context.tblMAZ_Revision_Cuadrilla_Detalle.Where(x => x.estatus == true && x.revisionID == revCua.id && x.realizo == true).ToList();

                foreach (var det in revCuaDet)
                {
                    var rep = _context.tblMAZ_Reporte_Actividades.Where(x => x.estatus == true && x.revisionTipo == 2 && x.revisionDetalleID == det.id).FirstOrDefault();
                    if (rep != null)
                    {
                        var repEquipos = _context.tblMAZ_Reporte_Actividades_Equipo.Where(x => x.estatus == true && x.reporteActividadesID == rep.id).ToList();

                        reporte.Add(new ReporteDiarioDTO
                        {
                            equiposID = repEquipos.Count > 0 ? repEquipos.Select(x => x.equipoID).ToList() : null,
                            semaforo = rep.semaforo,
                            reprogramacion = rep.reprogramacion,
                            estatus = rep.estatusInfo,
                            fechaCaptura = revCua.fechaCaptura,
                            monthFechaCaptura = revCua.fechaCaptura.Month,
                            dayFechaCaptura = revCua.fechaCaptura.Day,
                            fechaReprogramacion = rep.fechaReprogramacion.ToString(),
                            monthFechaReprogramacion = rep.fechaReprogramacion != null ? rep.fechaReprogramacion.Value.Month : 0,
                            dayFechaReprogramacion = rep.fechaReprogramacion != null ? rep.fechaReprogramacion.Value.Day : 0
                        });
                    }
                    else
                    {
                        reporte.Add(new ReporteDiarioDTO
                        {
                            equiposID = new List<int>(),
                            reprogramacion = det.reprogramacion,
                            estatus = det.estatusInfo,
                            fechaCaptura = revCua.fechaCaptura
                        });
                    }
                }
            }

            return reporte;
        }

        #endregion

        #region SUBAREAS   
        public List<subAreaDTO> getSubAreas(int areaID, string subArea)
        {
            var subAreaUpper = subArea.ToUpper();
            var list = _context.tblMAZ_SubArea.Where(x => x.estatus == true && (areaID != 0 ? x.areaID == areaID : true) && (x.descripcion.ToUpper().Contains(subAreaUpper))).Select(y => new subAreaDTO
            {
                id = y.id,
                descripcion = y.descripcion,
            }).ToList();

            return list;
        }
        public List<subAreaDTO> getSubAreasCatalogo(List<int> arrCuadrillas, List<int> arrAreas)
        {
            var cuadrillas = _context.tblMAZ_Cuadrilla.Where(x => x.estatus == true).ToList();
            var areas = _context.tblMAZ_Area.Where(x => x.estatus == true).ToList();
            var subAreas = _context.tblMAZ_SubArea.Where(x => x.estatus == true).ToList();

            var areasID = arrCuadrillas != null ? areas.Where(x => arrCuadrillas.Contains(x.cuadrillaID)).Select(y => y.id).ToList() : null;
            var subAreasIDFromAreas = arrAreas != null ? subAreas.Where(x => arrAreas.Contains(x.areaID)).Select(y => y.id).ToList() :
                areasID != null ? subAreas.Where(x => areasID.Contains(x.areaID)).Select(y => y.id).ToList() : null;

            var list = _context.tblMAZ_SubArea.ToList().Where(x =>
                   (x.estatus == true) &&
                   (subAreasIDFromAreas != null ? subAreasIDFromAreas.Contains(x.id) : true)).Select(y => new subAreaDTO
                   {
                       id = y.id,
                       descripcion = y.descripcion,
                       areaID = y.areaID,
                       cuadrillaID = areas.Where(w => w.estatus == true && w.id == y.areaID).Select(z => z.cuadrillaID).FirstOrDefault(),
                       area = areas.Where(w => w.estatus == true && w.id == y.areaID).Select(z => z.descripcion).FirstOrDefault(),
                       cuadrilla = cuadrillas.Where(w => w.estatus == true && w.id == areas.Where(z => z.id == y.areaID).Select(v => v.cuadrillaID).FirstOrDefault()).Select(m => m.descripcion).FirstOrDefault()
                   }).ToList();
            return list;
        }
        public subAreaDTO getSubArea(int id)
        {
            var areas = _context.tblMAZ_Area.Where(x => x.estatus == true).ToList();
            var subArea = _context.tblMAZ_SubArea.ToList().Where(x => x.estatus == true && x.id == id).Select(y => new subAreaDTO
            {
                id = y.id,
                descripcion = y.descripcion,
                areaID = y.areaID,
                cuadrillaID = areas.Where(w => w.estatus == true && w.id == y.areaID).Select(z => z.cuadrillaID).FirstOrDefault(),
            }).FirstOrDefault();

            return subArea;
        }
        public bool GuardarSubArea(tblMAZ_SubArea subArea, List<tblMAZ_Subarea_Referencia> referencias)
        {
            try
            {
                _context.tblMAZ_SubArea.Add(subArea);
                _context.SaveChanges();

                if (subArea.id > 0)
                {
                    referencias.ForEach(e =>
                    {
                        e.subAreaID = subArea.id;
                        e.estatus = true;
                        _context.tblMAZ_Subarea_Referencia.Add(e);
                        _context.SaveChanges();
                    });
                }
            }
            catch (Exception) { }

            return subArea.id > 0;
        }
        public void EditarSubArea(int id, string descripcion, int areaID, bool estatus)
        {
            try
            {
                var subarea = _context.tblMAZ_SubArea.Where(x => x.estatus == true && x.id == id).FirstOrDefault();

                if (subarea != null)
                {
                    subarea.descripcion = descripcion;

                    if (areaID != 0)
                    {
                        subarea.areaID = areaID;
                    }
                    else
                    {
                        subarea.areaID = 0;
                    }
                    subarea.estatus = estatus;

                    _context.Entry(subarea).State = System.Data.Entity.EntityState.Modified;
                    _context.SaveChanges();
                }
            }
            catch (Exception) { }
        }
        public void RemoveSubArea(int id)
        {
            try
            {
                var subarea = _context.tblMAZ_SubArea.Where(x => x.estatus == true && x.id == id).FirstOrDefault();

                if (subarea != null)
                {
                    subarea.estatus = false;

                    _context.Entry(subarea).State = System.Data.Entity.EntityState.Modified;
                    _context.SaveChanges();
                }
            }
            catch (Exception) { }
        }
        public int GetUltimoArchivoSubArea()
        {
            return _context.tblMAZ_Subarea_Referencia.ToList().LastOrDefault() != null ? _context.tblMAZ_Subarea_Referencia.ToList().LastOrDefault().id : 0;
        }
        public void QuitarReferenciaSubarea(int subareaID)
        {
            var referencias = _context.tblMAZ_Subarea_Referencia.Where(x => x.estatus == true && x.subAreaID == subareaID).ToList();

            foreach (var refe in referencias)
            {
                refe.estatus = false;
                _context.Entry(refe).State = System.Data.Entity.EntityState.Modified;
                _context.SaveChanges();
            }
        }
        public bool GuardarReferenciaSubArea(int subareaID, List<tblMAZ_Subarea_Referencia> referencias)
        {
            try
            {
                referencias.ForEach(e =>
                {
                    e.subAreaID = subareaID;
                    e.estatus = true;
                    _context.tblMAZ_Subarea_Referencia.Add(e);
                    _context.SaveChanges();
                });
            }
            catch (Exception) { }

            return true;
        }
        #endregion

        #region REPORTE DIARIO
        public List<ReporteDiarioDTO> getReporteDiario(string fecha)
        {
            List<ReporteDiarioDTO> reporte = new List<ReporteDiarioDTO>();

            DateTime fechaDate = new DateTime();
            bool flagFecha = false;

            if (fecha != "")
            {
                DateTime dateTemp;

                if (DateTime.TryParse(fecha, out dateTemp))
                {
                    fechaDate = DateTime.Parse(fecha);
                    flagFecha = true;
                }
            }

            var revisionesAC = _context.tblMAZ_Revision_AC.ToList().Where(x => (flagFecha == true ? x.fechaCaptura.Date == fechaDate.Date : true)).ToList();
            var revisionesCua = _context.tblMAZ_Revision_Cuadrilla.ToList().Where(x => (flagFecha == true ? x.fechaCaptura.Date == fechaDate.Date : true)).ToList();

            var cuadrillas = _context.tblMAZ_Cuadrilla.Where(x => x.estatus == true).ToList();
            var equipos = _context.tblMAZ_Equipo_AC.ToList(); //Sin la validación true para poder consultar equipos pasados por si se eliminan
            var areas = _context.tblMAZ_Area.Where(x => x.estatus == true).ToList();

            var evidenciasAC = _context.tblMAZ_Revision_AC_Evidencia.ToList();
            var detalleAC = _context.tblMAZ_Revision_AC_Detalle.Where(x => x.estatus == true).ToList();

            var actividades = _context.tblMAZ_Actividad.Where(x => x.estatus == true).ToList();

            var referenciasAC = _context.tblMAZ_Equipo_Referencia.Where(x => x.estatus == true).ToList();

            //var reportesAC = _context.tblMAZ_Reporte_Actividades.Where(x => x.estatus == true && x.revisionTipo == 1).ToList();
            //var reportesCua = _context.tblMAZ_Reporte_Actividades.Where(x => x.estatus == true && x.revisionTipo == 2).ToList();
            //var reportesEquipos = _context.tblMAZ_Reporte_Actividades_Equipo.Where(x => x.estatus == true).ToList();

            foreach (var revAC in revisionesAC)
            {
                //var revAcDet = _context.tblMAZ_Revision_AC_Detalle.Where(x => x.estatus == true && x.revisionID == revAC.id).ToList();
                var revAcDet = _context.tblMAZ_Revision_AC_Detalle.Where(x => x.estatus == true && x.revisionID == revAC.id && x.realizo == true).ToList();

                foreach (var det in revAcDet)
                {
                    var act = actividades.Where(x => x.id == det.actividadID).FirstOrDefault();

                    var rep = _context.tblMAZ_Reporte_Actividades.Where(x => x.estatus == true && x.revisionTipo == 1 && x.revisionDetalleID == det.id).FirstOrDefault();

                    if (rep != null)
                    {
                        var repEquipos = _context.tblMAZ_Reporte_Actividades_Equipo.Where(x => x.estatus == true && x.reporteActividadesID == rep.id).ToList();
                        var repEvidencias = _context.tblMAZ_Reporte_Actividades_Evidencia.Where(x => x.estatus == true && x.reporteActividadesID == rep.id).Select(y => y.id).ToList();

                        reporte.Add(new ReporteDiarioDTO
                        {
                            cuadrillaID = 1,
                            cuadrilla = "AIRES ACONDICIONADOS",
                            actividad = act.descripcion,
                            equipoID = repEquipos.Count > 0 ? repEquipos.Select(x => x.equipoID).FirstOrDefault() : 0,
                            equiposID = repEquipos.Count > 0 ? repEquipos.Select(x => x.equipoID).ToList() : null,
                            //equipo = equipos.Where(x => x.id == revAC.equipoID).FirstOrDefault().descripcion,
                            ultMantenimiento = rep.ultMant,
                            sigMantenimiento = rep.sigMant,
                            areaID = repEquipos.Count > 0 ? equipos.Where(x => x.id == repEquipos.FirstOrDefault().equipoID).FirstOrDefault().subAreaID : equipos.Where(x => x.id == revAC.equipoID).FirstOrDefault().subAreaID,
                            areaEjecucion = repEquipos.Count > 0 ? areas.Where(x => x.id == equipos.Where(w => w.id == repEquipos.FirstOrDefault().equipoID).FirstOrDefault().subAreaID).FirstOrDefault().descripcion :
                            areas.Where(x => x.id == equipos.Where(w => w.id == revAC.equipoID).FirstOrDefault().subAreaID).FirstOrDefault().descripcion,
                            descripcionActividadID = 0,
                            descripcionActividad = rep.descripcionActividad,
                            //semaforo = (detalleAC.Where(x => x.revisionID == revAC.id && x.realizo == true).ToList().Count > 0 && detalleAC.Where(x => x.revisionID == revAC.id && x.realizo == false).ToList().Count == 0) ? 1 :
                            //           (detalleAC.Where(x => x.revisionID == revAC.id && x.realizo == true).ToList().Count > 0 && detalleAC.Where(x => x.revisionID == revAC.id && x.realizo == false).ToList().Count > 0) ? 2 : 0,
                            semaforo = rep.semaforo,
                            reprogramacion = rep.reprogramacion,
                            estatus = rep.estatusInfo,
                            evidenciasID = evidenciasAC.Where(x => x.idRevision == revAC.id).Select(y => y.id).ToList(),
                            evidencias = "",
                            referenciasID = referenciasAC.Where(x => x.equipoID == revAC.equipoID).ToList().Count > 0 ? referenciasAC.Where(x => x.equipoID == revAC.equipoID).Select(y => y.id).ToList() : null,
                            referencias = "",
                            fechaCaptura = revAC.fechaCaptura,
                            fechaReprogramacion = rep.fechaReprogramacion != null ? rep.fechaReprogramacion.Value.ToString("MM.dd.yyyy") : "",
                            revisionTipo = 1,
                            revisionID = revAC.id,
                            revisionDetID = det.id
                        });
                    }
                    else
                    {
                        reporte.Add(new ReporteDiarioDTO
                        {
                            cuadrillaID = 1,
                            cuadrilla = "AIRES ACONDICIONADOS",
                            actividad = act.descripcion,
                            equipoID = revAC.equipoID,
                            equiposID = null,
                            equipo = equipos.Where(x => x.id == revAC.equipoID).FirstOrDefault().descripcion,
                            ultMantenimiento = det.ultMant,
                            sigMantenimiento = det.sigMant,
                            areaID = equipos.Where(x => x.id == revAC.equipoID).FirstOrDefault().subAreaID,
                            areaEjecucion = areas.Where(x => x.id == equipos.Where(w => w.id == revAC.equipoID).FirstOrDefault().subAreaID).FirstOrDefault().descripcion,
                            descripcionActividadID = 0,
                            descripcionActividad = act.detalle,
                            //semaforo = (detalleAC.Where(x => x.revisionID == revAC.id && x.realizo == true).ToList().Count > 0 && detalleAC.Where(x => x.revisionID == revAC.id && x.realizo == false).ToList().Count == 0) ? 1 :
                            //           (detalleAC.Where(x => x.revisionID == revAC.id && x.realizo == true).ToList().Count > 0 && detalleAC.Where(x => x.revisionID == revAC.id && x.realizo == false).ToList().Count > 0) ? 2 : 0,
                            semaforo = 1,
                            reprogramacion = det.reprogramacion,
                            estatus = det.estatusInfo,
                            evidenciasID = evidenciasAC.Where(x => x.idRevision == revAC.id).Select(y => y.id).ToList(),
                            evidencias = "",
                            referenciasID = referenciasAC.Where(x => x.equipoID == revAC.equipoID).ToList().Count > 0 ? referenciasAC.Where(x => x.equipoID == revAC.equipoID).Select(y => y.id).ToList() : null,
                            referencias = "",

                            fechaCaptura = revAC.fechaCaptura,
                            revisionTipo = 1,
                            revisionID = revAC.id,
                            revisionDetID = det.id
                        });
                    }
                }
            }

            var evidenciasCua = _context.tblMAZ_Revision_Cuadrilla_Evidencia.ToList();
            var detalleCua = _context.tblMAZ_Revision_Cuadrilla_Detalle.Where(x => x.estatus == true).ToList();

            //.Where(x => x.estatus == true)
            var planMesDetalles = _context.tblMAZ_PlanMes_Detalle.ToList();

            var referenciasCua = _context.tblMAZ_Equipo_Referencia.Where(x => x.estatus == true).ToList();

            foreach (var revCua in revisionesCua)
            {
                var revCuaDet = _context.tblMAZ_Revision_Cuadrilla_Detalle.Where(x => x.estatus == true && x.revisionID == revCua.id && x.realizo == true).ToList();

                foreach (var det in revCuaDet)
                {
                    var act = actividades.Where(x => x.id == det.actividadID).FirstOrDefault();

                    var rep = _context.tblMAZ_Reporte_Actividades.Where(x => x.estatus == true && x.revisionTipo == 2 && x.revisionDetalleID == det.id).FirstOrDefault();

                    if (rep != null)
                    {
                        var repEquipos = _context.tblMAZ_Reporte_Actividades_Equipo.Where(x => x.estatus == true && x.reporteActividadesID == rep.id).ToList();
                        var repEvidencias = _context.tblMAZ_Reporte_Actividades_Evidencia.Where(x => x.estatus == true && x.reporteActividadesID == rep.id).Select(y => y.id).ToList();

                        reporte.Add(new ReporteDiarioDTO
                        {
                            cuadrillaID = revCua.cuadrillaID,
                            cuadrilla = cuadrillas.Where(x => x.id == revCua.cuadrillaID).FirstOrDefault().descripcion,
                            actividad = act.descripcion,
                            equipoID = repEquipos.Count > 0 ? repEquipos.Select(x => x.equipoID).FirstOrDefault() : 0,
                            equiposID = repEquipos.Count > 0 ? repEquipos.Select(x => x.equipoID).ToList() : null,
                            //equipo = revCua.planMesDetalleID != 0 ? planMesDetalles.Where(x => x.id == revCua.planMesDetalleID).FirstOrDefault().tipo == 2 ? planMesDetalles.Where(x => x.id == revCua.planMesDetalleID).FirstOrDefault().equipoAreaDesc : "" : "",
                            ultMantenimiento = rep.ultMant,
                            sigMantenimiento = rep.sigMant,
                            areaID = areas.Where(r => r.id == actividades.Where(t => t.id == det.actividadID).FirstOrDefault().areaID).FirstOrDefault().id,
                            areaEjecucion = areas.Where(r => r.id == actividades.Where(t => t.id == det.actividadID).FirstOrDefault().areaID).FirstOrDefault().descripcion,
                            // areaEjecucion = revCua.planMesDetalleID != 0 ?
                            //                     planMesDetalles.Where(x => x.id == revCua.planMesDetalleID).FirstOrDefault().tipo == 1 ?
                            //                         "cambiar esto bien" : //planMesDetalles.Where(x => x.id == revCua.planMesDetalleID).FirstOrDefault() :
                            //                 equipos.Where(w => w.id == planMesDetalles.Where(x => x.id == revCua.planMesDetalleID).FirstOrDefault().equipoID).FirstOrDefault().subArea :
                            //                 areas.Where(r => r.id == actividades.Where(t => t.id == det.actividadID).FirstOrDefault().areaID).FirstOrDefault().descripcion,
                            descripcionActividadID = 0,
                            descripcionActividad = rep.descripcionActividad,
                            //semaforo = (detalleCua.Where(x => x.revisionID == revCua.id && x.realizo == true).ToList().Count > 0 && detalleCua.Where(x => x.revisionID == revCua.id && x.realizo == false).ToList().Count == 0) ? 1 :
                            //           (detalleCua.Where(x => x.revisionID == revCua.id && x.realizo == true).ToList().Count > 0 && detalleCua.Where(x => x.revisionID == revCua.id && x.realizo == false).ToList().Count > 0) ? 2 : 0,
                            semaforo = rep.semaforo,
                            reprogramacion = rep.reprogramacion,
                            estatus = rep.estatusInfo,
                            evidenciasID = evidenciasCua.Where(x => x.idRevision == revCua.id).Select(y => y.id).ToList(),
                            evidencias = "",
                            referenciasID = revCua.planMesDetalleID != 0 ?
                            planMesDetalles.Where(x => x.id == revCua.planMesDetalleID).FirstOrDefault().tipo == 2 ?
                            referenciasCua.Where(w => w.equipoID == planMesDetalles.Where(x => x.id == revCua.planMesDetalleID).FirstOrDefault().equipoID).Select(q => q.id).ToList() :
                            null :
                            null,
                            referencias = "",
                            fechaCaptura = revCua.fechaCaptura,
                            fechaReprogramacion = rep.fechaReprogramacion != null ? rep.fechaReprogramacion.Value.ToString("MM.dd.yyyy") : "",
                            revisionTipo = 2,
                            revisionID = revCua.id,
                            revisionDetID = det.id
                        });
                    }
                    else
                    {
                        reporte.Add(new ReporteDiarioDTO
                        {
                            cuadrillaID = revCua.cuadrillaID,
                            cuadrilla = cuadrillas.Where(x => x.id == revCua.cuadrillaID).FirstOrDefault().descripcion,
                            actividad = act.descripcion,
                            equipoID = revCua.planMesDetalleID != 0 ?
                            planMesDetalles.Where(x => x.id == revCua.planMesDetalleID).FirstOrDefault().tipo == 2 ?
                            planMesDetalles.Where(x => x.id == revCua.planMesDetalleID).FirstOrDefault().equipoID :
                            0 :
                            0,
                            equiposID = null,
                            equipo = revCua.planMesDetalleID != 0 ? equipos.Where(w => w.id == planMesDetalles.Where(x => x.id == revCua.planMesDetalleID).FirstOrDefault().equipoID).FirstOrDefault().descripcion : "",
                            //equipo = revCua.planMesDetalleID != 0 ? planMesDetalles.Where(x => x.id == revCua.planMesDetalleID).FirstOrDefault().tipo == 2 ? "cambiar esto" : "" : "", //planMesDetalles.Where(x => x.id == revCua.planMesDetalleID).FirstOrDefault().equipoAreaDesc : "" : "",
                            ultMantenimiento = det.ultMant,
                            sigMantenimiento = det.sigMant,
                            areaID = areas.Where(r => r.id == actividades.Where(t => t.id == det.actividadID).FirstOrDefault().areaID).FirstOrDefault().id,
                            areaEjecucion = areas.Where(r => r.id == actividades.Where(t => t.id == det.actividadID).FirstOrDefault().areaID).FirstOrDefault().descripcion,
                            //areaEjecucion = revCua.planMesDetalleID != 0 ?
                            //                planMesDetalles.Where(x => x.id == revCua.planMesDetalleID).FirstOrDefault().tipo == 1 ?
                            //                "cambiar esto" : // planMesDetalles.Where(x => x.id == revCua.planMesDetalleID).FirstOrDefault().equipoAreaDesc :
                            //                equipos.Where(w => w.id == planMesDetalles.Where(x => x.id == revCua.planMesDetalleID).FirstOrDefault().equipoID).FirstOrDefault().subArea :
                            //                areas.Where(r => r.id == actividades.Where(t => t.id == det.actividadID).FirstOrDefault().areaID).FirstOrDefault().descripcion,
                            descripcionActividadID = 0,
                            descripcionActividad = act.detalle,
                            //semaforo = (detalleCua.Where(x => x.revisionID == revCua.id && x.realizo == true).ToList().Count > 0 && detalleCua.Where(x => x.revisionID == revCua.id && x.realizo == false).ToList().Count == 0) ? 1 :
                            //           (detalleCua.Where(x => x.revisionID == revCua.id && x.realizo == true).ToList().Count > 0 && detalleCua.Where(x => x.revisionID == revCua.id && x.realizo == false).ToList().Count > 0) ? 2 : 0,
                            semaforo = 1,
                            reprogramacion = det.reprogramacion,
                            estatus = det.estatusInfo,
                            evidenciasID = evidenciasCua.Where(x => x.idRevision == revCua.id).Select(y => y.id).ToList(),
                            evidencias = "",
                            referenciasID = revCua.planMesDetalleID != 0 ?
                            planMesDetalles.Where(x => x.id == revCua.planMesDetalleID).FirstOrDefault().tipo == 2 ?
                            referenciasCua.Where(w => w.equipoID == planMesDetalles.Where(x => x.id == revCua.planMesDetalleID).FirstOrDefault().equipoID).Select(q => q.id).ToList() :
                            null :
                            null,
                            referencias = "",

                            fechaCaptura = revCua.fechaCaptura,
                            revisionTipo = 2,
                            revisionID = revCua.id,
                            revisionDetID = det.id
                        });
                    }
                }
            }

            return reporte.OrderByDescending(x => x.fechaCaptura).ToList();
        }
        public static byte[] ReadFully(Stream input)
        {
            byte[] buffer = new byte[16 * 1024];
            using (MemoryStream ms = new MemoryStream())
            {
                int read;
                while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
                {
                    ms.Write(buffer, 0, read);
                }
                return ms.ToArray();
            }
        }
        public bool GuardarInfoRevDet(tblMAZ_Reporte_Actividades info, List<tblMAZ_Reporte_Actividades_Equipo> eq, List<tblMAZ_Reporte_Actividades_Evidencia> evi, bool flagPasarEvidencias)
        {
            var checkAnterior = 0;

            if (info.revisionTipo != 1)
            {
                var repAct = _context.tblMAZ_Reporte_Actividades.Where(x => x.estatus == true && x.revisionTipo == 2 && x.revisionDetalleID == info.revisionDetalleID).ToList();
                checkAnterior = repAct.Select(x => x.id).LastOrDefault();

                foreach (var ra in repAct)
                {
                    ra.estatus = false;

                    _context.Entry(ra).State = System.Data.Entity.EntityState.Modified;
                    _context.SaveChanges();
                }
            }
            else
            {
                var repAct = _context.tblMAZ_Reporte_Actividades.Where(x => x.estatus == true && x.revisionTipo == 1 && x.revisionDetalleID == info.revisionDetalleID).ToList();
                checkAnterior = repAct.Select(x => x.id).LastOrDefault();

                foreach (var ra in repAct)
                {
                    ra.estatus = false;

                    _context.Entry(ra).State = System.Data.Entity.EntityState.Modified;
                    _context.SaveChanges();
                }
            }

            _context.tblMAZ_Reporte_Actividades.Add(info);
            _context.SaveChanges();

            if (info.id > 0)
            {
                eq.ForEach(a =>
                {
                    a.reporteActividadesID = info.id;
                    _context.tblMAZ_Reporte_Actividades_Equipo.Add(a);
                    _context.SaveChanges();
                });

                evi.ForEach(e =>
                {
                    e.reporteActividadesID = info.id;
                    _context.tblMAZ_Reporte_Actividades_Evidencia.Add(e);
                    _context.SaveChanges();
                });

                if (flagPasarEvidencias)
                {
                    if (checkAnterior != 0)
                    {
                        var evidenciasPasadas = _context.tblMAZ_Reporte_Actividades_Evidencia.Where(x => x.reporteActividadesID == checkAnterior).ToList();

                        foreach (var ep in evidenciasPasadas)
                        {
                            ep.reporteActividadesID = info.id;

                            _context.tblMAZ_Reporte_Actividades_Evidencia.Add(ep);
                            _context.SaveChanges();
                        }
                    }
                }
            }

            var flagCorreo = false;
            var hoy = DateTime.Now.Date;
            var hoyMes = hoy.Month;

            if (info.revisionTipo == 1)
            {
                var revisionDetalle = _context.tblMAZ_Revision_AC_Detalle.Where(x => x.estatus == true && x.id == info.revisionDetalleID).FirstOrDefault();

                if (revisionDetalle != null)
                {
                    var revision = _context.tblMAZ_Revision_AC.Where(x => x.id == revisionDetalle.revisionID).FirstOrDefault();
                    var planMesDetalle = _context.tblMAZ_PlanMes_Detalle.Where(x => x.estatus == true && x.id == revision.planMesDetalleID).FirstOrDefault();

                    if (planMesDetalle != null)
                    {
                        var planMes = _context.tblMAZ_PlanMes.Where(x => x.estatus == true && x.id == planMesDetalle.planMesID).FirstOrDefault();
                        var ultimoDiaPlanMesDetalle = _context.tblMAZ_PlanMes_Detalle_Dia.ToList().Where(x => x.estatus == true && x.planMesDetalleID == planMesDetalle.id).LastOrDefault();

                        if (ultimoDiaPlanMesDetalle != null)
                        {
                            var ultimaFecha = new DateTime(planMes.anio, planMes.mes, ultimoDiaPlanMesDetalle.dia);

                            if (hoy > ultimaFecha.Date)
                            {
                                flagCorreo = true;
                            }
                        }
                    }
                }
            }
            else
            {
                var revisionDetalle = _context.tblMAZ_Revision_Cuadrilla_Detalle.Where(x => x.estatus == true && x.id == info.revisionDetalleID).FirstOrDefault();

                if (revisionDetalle != null)
                {
                    var revision = _context.tblMAZ_Revision_Cuadrilla.Where(x => x.id == revisionDetalle.revisionID).FirstOrDefault();
                    var planMesDetalle = _context.tblMAZ_PlanMes_Detalle.Where(x => x.estatus == true && x.id == revision.planMesDetalleID).FirstOrDefault();

                    if (planMesDetalle != null)
                    {
                        var planMes = _context.tblMAZ_PlanMes.Where(x => x.estatus == true && x.id == planMesDetalle.planMesID).FirstOrDefault();
                        var ultimoDiaPlanMesDetalle = _context.tblMAZ_PlanMes_Detalle_Dia.ToList().Where(x => x.estatus == true && x.planMesDetalleID == planMesDetalle.id).LastOrDefault();

                        if (ultimoDiaPlanMesDetalle != null)
                        {
                            var ultimaFecha = new DateTime(planMes.anio, planMes.mes, ultimoDiaPlanMesDetalle.dia);

                            if (hoy > ultimaFecha.Date)
                            {
                                flagCorreo = true;
                            }
                        }
                    }
                }
            }

            if ((info.semaforo == 2 || info.semaforo == 3) && flagCorreo == true)
            {
                var usuariosMAZDA = _context.tblMAZ_Usuario_Cuadrilla.Where(x => x.estatus == true && x.cuadrillaID == 0).ToList();

                if (info.revisionTipo == 1)
                {
                    var revisionDetalle = _context.tblMAZ_Revision_AC_Detalle.Where(x => x.estatus == true && x.id == info.revisionDetalleID).FirstOrDefault();

                    if (revisionDetalle != null)
                    {
                        var revision = _context.tblMAZ_Revision_AC.Where(x => x.id == revisionDetalle.revisionID).FirstOrDefault();
                        var actividad = _context.tblMAZ_Actividad.Where(x => x.estatus == true && x.id == revisionDetalle.actividadID).FirstOrDefault();
                        var area = _context.tblMAZ_Area.Where(x => x.id == actividad.areaID).FirstOrDefault();
                        var cuadrilla = _context.tblMAZ_Cuadrilla.Where(x => x.id == area.cuadrillaID).FirstOrDefault();

                        GlobalUtils.sendEmail(string.Format("{0} {1}", PersonalUtilities.GetNombreEmpresa(), ("Actividad " + (info.semaforo == 2 ? "pendiente" : "no realizada"))), "La actividad '" + actividad.descripcion +
                        "' se ha marcado como " + (info.semaforo == 2 ? "'Pendiente'" : "'No Realizada'") + " en el Reporte Diario.<br/>" +
                        "Cuadrilla: " + cuadrilla.descripcion + "<br/>" +
                        "Periodo: " + (PeriodoEnum)revision.periodo, usuariosMAZDA.Select(x => x.correo).ToList());
                    }
                }
                else
                {
                    var revisionDetalle = _context.tblMAZ_Revision_Cuadrilla_Detalle.Where(x => x.estatus == true && x.id == info.revisionDetalleID).FirstOrDefault();

                    if (revisionDetalle != null)
                    {
                        var revision = _context.tblMAZ_Revision_Cuadrilla.Where(x => x.id == revisionDetalle.revisionID).FirstOrDefault();
                        var actividad = _context.tblMAZ_Actividad.Where(x => x.estatus == true && x.id == revisionDetalle.actividadID).FirstOrDefault();
                        var area = _context.tblMAZ_Area.Where(x => x.id == actividad.areaID).FirstOrDefault();
                        var cuadrilla = _context.tblMAZ_Cuadrilla.Where(x => x.id == area.cuadrillaID).FirstOrDefault();

                        GlobalUtils.sendEmail(string.Format("{0} {1}", PersonalUtilities.GetNombreEmpresa(), ("Actividad " + (info.semaforo == 2 ? "pendiente" : "no realizada"))), "La actividad '" + actividad.descripcion +
                        "' se ha marcado como " + (info.semaforo == 2 ? "'Pendiente'" : "'No Realizada'") + " en el Reporte Diario.<br/>" +
                        "Cuadrilla: " + cuadrilla.descripcion + "<br/>" +
                        "Mes: " + (MesEnum)revision.mes, usuariosMAZDA.Select(x => x.correo).ToList());
                    }
                }
            }

            return info.id > 0;
        }
        public int GetUltimoArchivoReporteAct()
        {
            return _context.tblMAZ_Reporte_Actividades_Evidencia.ToList().LastOrDefault() != null ? _context.tblMAZ_Reporte_Actividades_Evidencia.ToList().LastOrDefault().id : 0;
        }
        public int GetUltimoReporteAct()
        {
            return _context.tblMAZ_Reporte_Actividades.ToList().LastOrDefault() != null ? _context.tblMAZ_Reporte_Actividades.ToList().LastOrDefault().id : 0;
        }
        public List<string> getEvidenciasReporte(int revDetID)
        {
            List<string> arr64 = new List<string>();

            var reporte = _context.tblMAZ_Reporte_Actividades.Where(x => x.estatus == true && x.revisionDetalleID == revDetID).FirstOrDefault();

            if (reporte != null)
            {
                var evidencias = _context.tblMAZ_Reporte_Actividades_Evidencia.Where(x => x.estatus == true && x.reporteActividadesID == reporte.id).ToList();

                foreach (var evi in evidencias)
                {
                    var extension = evi.ruta.Split('.')[1];

                    if (File.Exists(evi.ruta))
                    {
                        using (var fileStream = new FileStream(evi.ruta, FileMode.Open, FileAccess.Read))
                        {
                            byte[] arrBytes = ReadFully(fileStream);

                            var cadena64 = Convert.ToBase64String(arrBytes);

                            arr64.Add("data:image/" + extension + ";base64, " + cadena64);
                        }
                    }
                }
            }

            return arr64;
        }
        public byte[] ResizeImageToByteArray(byte[] image, int width, int height)
        {
            using (var ms = new MemoryStream(image))
            {
                System.Drawing.Image img = System.Drawing.Image.FromStream(ms);

                var destRect = new Rectangle(0, 0, width, height);
                var destImage = new Bitmap(width, height);

                destImage.SetResolution(img.HorizontalResolution, img.VerticalResolution);

                using (var graphics = Graphics.FromImage(destImage))
                {
                    graphics.CompositingMode = CompositingMode.SourceCopy;
                    graphics.CompositingQuality = CompositingQuality.HighQuality;
                    graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                    graphics.SmoothingMode = SmoothingMode.HighQuality;
                    graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                    using (var wrapMode = new ImageAttributes())
                    {
                        wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                        graphics.DrawImage(img, destRect, 0, 0, img.Width, img.Height, GraphicsUnit.Pixel, wrapMode);
                    }
                }

                return imageToByteArray((Image)destImage);
            }
        }
        public byte[] imageToByteArray(System.Drawing.Image imageIn)
        {
            MemoryStream ms = new MemoryStream();
            imageIn.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
            return ms.ToArray();
        }
        #endregion
    }
}