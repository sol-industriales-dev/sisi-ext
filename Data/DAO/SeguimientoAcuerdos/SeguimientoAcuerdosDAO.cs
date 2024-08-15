using Core.DAO.SeguimientoAcuerdos;
using Core.DTO;
using Core.DTO.SeguimientoAcuerdos;
using Core.Entity.Principal.Alertas;
using Core.Entity.Principal.Usuarios;
using Core.Entity.SeguimientoAcuerdos;
using Core.Enum.Principal.Bitacoras;
using Core.Enum.SeguimientoAcuerdos;
using Data.EntityFramework.Generic;
using Data.Factory.Principal.Alertas;
using Infrastructure.Utils;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Core.Entity.FileManager;
using Core.Enum.Multiempresa;
using Data.EntityFramework.Context;
using Core.DTO.Utils.Data;
using Core.Enum.Principal;

namespace Data.DAO.SeguimientoAcuerdos
{
    public class SeguimientoAcuerdosDAO : GenericDAO<tblSA_Minuta>, ISeguimientoAcuerdosDAO
    {
        public int guardarMinuta(tblSA_Minuta obj, bool nuevaVersion)
        {
            IObjectSet<tblSA_Minuta> _objectSet = ((IObjectContextAdapter)_context).ObjectContext.CreateObjectSet<tblSA_Minuta>();
            if (!Exists(obj))
            {
                if (obj == null) { throw new ArgumentNullException("Entity"); }
                obj.fechaInicio = DateTime.Now;
                obj.fechaCompromiso = DateTime.Now;
                _objectSet.AddObject(obj);
                _context.SaveChanges();
                SaveBitacora((int)BitacoraEnum.MINUTA, (int)AccionEnum.AGREGAR, obj.id, JsonUtils.convertNetObjectToJson(obj));
                //SaveEntity(obj, (int)BitacoraEnum.MINUTA);
                //Guardar en Envio Gestor
                insertEnvioGestor(obj.id);
            }
            else
            {
                tblSA_Minuta existing = _context.tblSA_Minuta.Find(obj.id);
                if (existing != null)
                {
                    existing.proyecto = obj.proyecto;
                    existing.titulo = obj.titulo;
                    existing.lugar = obj.lugar;
                    existing.fecha = obj.fecha;
                    existing.horaInicio = obj.horaInicio;
                    existing.horaFin = obj.horaFin;
                    existing.descripcion = obj.descripcion;

                    _context.SaveChanges();
                    SaveBitacora((int)BitacoraEnum.MINUTA, (int)AccionEnum.ACTUALIZAR, obj.id, JsonUtils.convertNetObjectToJson(obj));
                    var acti = _context.tblSA_Actividades.Where(x => x.minutaID == obj.id && x.columna == 100).ToList();
                    acti.ForEach(x => x.enVersion = false);
                    _context.SaveChanges();
                }
                //SaveEntityWithDelet(obj, (int)BitacoraEnum.MINUTA);
            }
            return obj.id;
        }
        public void guardarDescripcion(tblSA_Minuta obj, bool nuevaVersion)
        {
            tblSA_Minuta existing = _context.Set<tblSA_Minuta>().Find(obj.id);
            if (existing != null)
            {
                _context.Entry(existing).CurrentValues.SetValues(obj);
                _context.SaveChanges();
                SaveBitacora((int)BitacoraEnum.MINUTA, (int)AccionEnum.ACTUALIZAR, obj.id, JsonUtils.convertNetObjectToJson(obj));
            }
        }
        public int guardarActividad(tblSA_Actividades obj)
        {
            IObjectSet<tblSA_Actividades> _objectSet = ((IObjectContextAdapter)_context).ObjectContext.CreateObjectSet<tblSA_Actividades>();
            if (!ExistsA(obj))
            {
                if (obj == null) { throw new ArgumentNullException("Entity"); }
                _objectSet.AddObject(obj);
                _context.SaveChanges();
                var actividades = _context.tblSA_Actividades.Where(x => x.minutaID == obj.minutaID);
                var menor = actividades.OrderBy(x => x.fechaInicio).FirstOrDefault();
                var mayor = actividades.OrderByDescending(x => x.fechaCompromiso).FirstOrDefault();
                var minuta = _context.tblSA_Minuta.FirstOrDefault(x => x.id == obj.minutaID);
                minuta.fechaInicio = menor.fechaInicio;
                minuta.fechaCompromiso = mayor.fechaCompromiso;
                _context.SaveChanges();
                //  SaveBitacora((int)BitacoraEnum.ACTIVIDAD, (int)AccionEnum.AGREGAR, obj.id, JsonUtils.convertNetObjectToJson(obj));
                //SaveEntity(obj, (int)BitacoraEnum.MINUTA);
            }
            else
            {
                tblSA_Actividades existing = _context.Set<tblSA_Actividades>().Find(obj.id);
                if (existing != null)
                {
                    _context.Entry(existing).CurrentValues.SetValues(obj);
                    _context.SaveChanges();
                    var actividades = _context.tblSA_Actividades.Where(x => x.minutaID == obj.minutaID);
                    var menor = actividades.OrderBy(x => x.fechaInicio).FirstOrDefault();
                    var mayor = actividades.OrderByDescending(x => x.fechaCompromiso).FirstOrDefault();
                    var minuta = _context.tblSA_Minuta.FirstOrDefault(x => x.id == obj.minutaID);
                    minuta.fechaInicio = menor.fechaInicio;
                    minuta.fechaCompromiso = mayor.fechaCompromiso;
                    _context.SaveChanges();
                    SaveBitacora((int)BitacoraEnum.ACTIVIDAD, (int)AccionEnum.ACTUALIZAR, obj.id, JsonUtils.convertNetObjectToJson(obj));
                }
                //SaveEntityWithDelet(obj, (int)BitacoraEnum.MINUTA);
            }
            return obj.id;
        }
        public tblSA_ComentariosDTO guardarComentario(tblSA_Comentarios obj, HttpPostedFileBase file)
        {
            IObjectSet<tblSA_Comentarios> _objectSet = ((IObjectContextAdapter)_context).ObjectContext.CreateObjectSet<tblSA_Comentarios>();
            if (obj == null) { throw new ArgumentNullException("Entity"); }

            _objectSet.AddObject(obj);
            _context.SaveChanges();
            if (file != null)
            {
                obj.adjuntoNombre = Path.GetFileName(file.FileName);
                obj.adjuntoExt = Path.GetExtension(file.FileName);
                obj.adjunto = GlobalUtils.ConvertFileToByte(file.InputStream);
                _context.SaveChanges();
            }

            var temp = _context.tblSA_Actividades.Find(obj.actividadID);
            temp.comentariosCount = temp.comentarios.Count;
            _context.SaveChanges();

            try
            {
                var comentarioDTO = new tblSA_ComentariosDTO
                {
                    id = obj.id,
                    actividadID = obj.actividadID,
                    comentario = obj.comentario,
                    usuarioNombre = obj.usuarioNombre,
                    usuarioID = obj.usuarioID,
                    fecha = obj.fecha.ToShortDateString(),
                    tipo = obj.tipo,
                    adjuntoNombre = obj.adjuntoNombre
                };

                SaveBitacora((int)BitacoraEnum.COMENTARIO, (int)AccionEnum.AGREGAR, obj.id, JsonUtils.convertNetObjectToJson(comentarioDTO));
            }
            catch (Exception) { };

            var result = new tblSA_ComentariosDTO();
            result.id = obj.id;
            result.actividadID = obj.actividadID;
            result.comentario = obj.comentario;
            result.usuarioNombre = obj.usuarioNombre;
            result.usuarioID = obj.usuarioID;
            result.fecha = obj.fecha.ToShortDateString();
            result.tipo = obj.tipo;
            result.adjuntoNombre = obj.adjuntoNombre;
            enviarCorreosComentario(result);
            return result;
        }
        public int guardarParticipante(tblSA_Participante obj)
        {
            IObjectSet<tblSA_Participante> _objectSet = ((IObjectContextAdapter)_context).ObjectContext.CreateObjectSet<tblSA_Participante>();
            if (obj == null) { throw new ArgumentNullException("Entity"); }
            _objectSet.AddObject(obj);
            _context.SaveChanges();
            SaveBitacora((int)BitacoraEnum.PARTICIPANTE, (int)AccionEnum.AGREGAR, obj.id, JsonUtils.convertNetObjectToJson(obj));
            return obj.id;
        }
        public void eliminarParticipante(tblSA_Participante obj)
        {
            IObjectSet<tblSA_Participante> _objectSet = ((IObjectContextAdapter)_context).ObjectContext.CreateObjectSet<tblSA_Participante>();
            var p = _context.tblSA_Participante.FirstOrDefault(x => x.minutaID == obj.minutaID && x.participanteID == obj.participanteID);
            _objectSet.DeleteObject(p);
            _context.SaveChanges();
            SaveBitacora((int)BitacoraEnum.PARTICIPANTE, (int)AccionEnum.ELIMINAR, obj.id, JsonUtils.convertNetObjectToJson(obj));
        }
        public tblSA_MinutaDTO getMinuta(int id, int empresa = 0)
        {
            if (empresa == 0) { empresa = vSesiones.sesionEmpresaActual; }
            var result = new tblSA_Minuta();
            var mObj = new tblSA_MinutaDTO();

            using (var ctx = new MainContext(empresa))
            {
                result = ctx.tblSA_Minuta.FirstOrDefault(x => x.id == id);
                var resps = ctx.tblSA_Responsables.Where(x => x.minutaID == id).ToList();

                mObj.id = result.id;
                mObj.proyecto = result.proyecto;
                mObj.titulo = result.titulo;
                mObj.lugar = result.lugar;
                mObj.fecha = result.fecha.ToShortDateString();
                mObj.horaInicio = result.horaInicio;
                mObj.horaFin = result.horaFin;
                mObj.creadorID = result.creadorID;
                mObj.descripcion = result.descripcion;
                mObj.participantes = new List<tblSA_ParticipanteDTO>();
                mObj.actividades = new List<tblSA_ActividadesDTO>();
                mObj.fechaInicio = result.fechaInicio.ToShortDateString();
                mObj.fechaCompromiso = result.fechaCompromiso.ToShortDateString();
                foreach (var i in result.participantes)
                {
                    var pObj = new tblSA_ParticipanteDTO();
                    pObj.id = i.id;
                    pObj.minutaID = i.minutaID;
                    pObj.participanteID = i.participanteID;
                    pObj.participante = i.participante;
                    mObj.participantes.Add(pObj);
                }
                foreach (var i in result.actividades.Where(x => x.enVersion == true))
                {
                    var pObj = new tblSA_ActividadesDTO();
                    pObj.id = i.id;
                    pObj.minuta = i.minuta.titulo;
                    pObj.minutaID = i.minutaID;
                    pObj.columna = i.columna;
                    pObj.orden = i.orden;
                    pObj.tipo = i.tipo;
                    pObj.actividad = i.actividad;
                    pObj.descripcion = i.descripcion;
                    pObj.responsableID = 0;
                    pObj.responsable = string.Join("/", resps.Where(x => x.actividadID == i.id).Select(x => x.usuarioText).ToList());
                    pObj.fechaInicio = i.fechaInicio.ToShortDateString();
                    pObj.fechaCompromiso = i.fechaCompromiso.ToShortDateString();
                    pObj.prioridad = i.prioridad;
                    pObj.comentariosCount = i.comentariosCount;
                    pObj.comentarios = new List<tblSA_ComentariosDTO>();
                    pObj.revisaID = i.revisaID;
                    pObj.revisa = i.revisa ?? null;
                    pObj.responsablesID = new List<int>();
                    pObj.responsablesID = ctx.tblSA_Responsables.Where(x => x.actividadID == i.id).Select(x => x.usuarioID).ToList();
                    mObj.actividades.Add(pObj);
                    foreach (var j in i.comentarios)
                    {
                        var cObj = new tblSA_ComentariosDTO();
                        cObj.id = j.id;
                        cObj.actividadID = j.actividadID;
                        cObj.comentario = j.comentario;
                        cObj.usuarioNombre = j.usuarioNombre;
                        cObj.usuarioID = j.usuarioID;
                        cObj.fecha = j.fecha.ToShortDateString();
                        cObj.tipo = j.tipo;
                        cObj.adjuntoNombre = j.adjuntoNombre;
                        var temp = mObj.actividades.FirstOrDefault(x => x.id == i.id);
                        temp.comentarios.Add(cObj);

                    }

                }
            }
            return mObj;
        }
        public tblSA_MinutaDTO getMinutaForVersion(int id)
        {
            var result = new tblSA_Minuta();
            result = _context.tblSA_Minuta.FirstOrDefault(x => x.id == id);

            var mObj = new tblSA_MinutaDTO();
            mObj.id = result.id;
            mObj.proyecto = result.proyecto;
            mObj.titulo = result.titulo;
            mObj.lugar = result.lugar;
            mObj.fecha = result.fecha.ToShortDateString();
            mObj.horaInicio = result.horaInicio;
            mObj.horaFin = result.horaFin;
            mObj.creadorID = result.creadorID;
            mObj.descripcion = result.descripcion;
            mObj.participantes = new List<tblSA_ParticipanteDTO>();
            mObj.actividades = new List<tblSA_ActividadesDTO>();
            mObj.fechaInicio = result.fechaInicio.ToShortDateString();
            mObj.fechaCompromiso = result.fechaCompromiso.ToShortDateString();
            foreach (var i in result.participantes)
            {
                var pObj = new tblSA_ParticipanteDTO();
                pObj.id = i.id;
                pObj.minutaID = i.minutaID;
                pObj.participanteID = i.participanteID;
                pObj.participante = i.participante;
                mObj.participantes.Add(pObj);
            }
            foreach (var i in result.actividades.Where(x => x.columna < 100))
            {
                var pObj = new tblSA_ActividadesDTO();
                pObj.id = i.id;
                pObj.minuta = i.minuta.titulo;
                pObj.minutaID = i.minutaID;
                pObj.columna = i.columna;
                pObj.orden = i.orden;
                pObj.tipo = i.tipo;
                pObj.actividad = i.actividad;
                pObj.descripcion = i.descripcion;
                pObj.responsableID = i.responsableID;
                pObj.responsable = i.responsable;
                pObj.fechaInicio = i.fechaInicio.ToShortDateString();
                pObj.fechaCompromiso = i.fechaCompromiso.ToShortDateString();
                pObj.prioridad = i.prioridad;
                pObj.comentariosCount = i.comentariosCount;
                pObj.comentarios = new List<tblSA_ComentariosDTO>();
                pObj.revisaID = i.revisaID;
                pObj.revisa = i.revisa;
                mObj.actividades.Add(pObj);
                foreach (var j in i.comentarios)
                {
                    var cObj = new tblSA_ComentariosDTO();
                    cObj.id = j.id;
                    cObj.actividadID = j.actividadID;
                    cObj.comentario = j.comentario;
                    cObj.usuarioNombre = j.usuarioNombre;
                    cObj.usuarioID = j.usuarioID;
                    cObj.fecha = j.fecha.ToShortDateString();
                    cObj.tipo = j.tipo;
                    cObj.adjuntoNombre = j.adjuntoNombre;
                    var temp = mObj.actividades.FirstOrDefault(x => x.id == i.id);
                    temp.comentarios.Add(cObj);

                }

            }
            return mObj;
        }
        public List<tblSA_MinutaDTO> getAllMinutas(int user)
        {
            var mr = _context.tblSA_Responsables.Where(x => x.usuarioID == user).Select(x => x.minutaID).Distinct().ToList();
            var mrf = _context.tblSA_Actividades.Where(x => mr.Contains(x.minutaID) && x.columna < 100).Select(x => x.minutaID).Distinct().ToList();

            var result = _context.tblSA_Minuta.Where(x =>
                mrf.Contains(x.id) ||
                (x.actividades.Any(y => y.columna < 100) && x.creadorID == user) ||
                (x.interesados.Any(y => y.interesadoID == user && y.actividad.columna < 100)) ||
                (x.participantes.Any(y => y.participanteID == user) && x.actividades.Any(y => y.columna < 100))
            ).OrderByDescending(x => x.fecha).Distinct().ToList();
            var dto = new List<tblSA_MinutaDTO>();
            foreach (var i in result)
            {
                var o = new tblSA_MinutaDTO();
                o.id = i.id;
                o.proyecto = i.proyecto;
                o.titulo = i.titulo;
                o.lugar = i.lugar;
                o.fecha = i.fecha.ToShortDateString();
                o.horaInicio = i.horaInicio;
                o.horaFin = i.horaFin;
                o.fechaInicio = i.fechaInicio.ToShortDateString();
                o.fechaCompromiso = i.fechaCompromiso.ToShortDateString();
                //var actividades = i.actividades.Where(x => x.responsableID == user || x.minuta.creadorID == user).Count();
                //o.ver = actividades > 0 ? true : false;
                o.ver = true;
                o.creadorID = i.creadorID;
                dto.Add(o);
            }
            return dto;
        }
        public List<tblSA_ActividadesDTO> getAllActividades(int user)
        {
            //nuevo
            var resultado = _context.Select<ActividadesDTO>(new DapperDTO
            {
                baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
                consulta = @"
                            SELECT
	                            act.id,
	                            minu.titulo AS 'minuta',
	                            act.minutaID,
	                            act.columna,
	                            act.orden,
	                            act.tipo,
	                            act.actividad,
	                            act.descripcion,
	                            act.responsableID,
	                            act.responsable,
	                            CONVERT(varchar, act.fechaInicio, 103) AS 'fechaInicio',
	                            CONVERT(varchar, act.fechaCompromiso, 103) AS 'fechaCompromiso',
	                            act.prioridad,
	                            act.comentariosCount,
	                            CASE
		                            WHEN inte.id IS NOT NULL THEN 1 ELSE 0
	                            END AS 'interesado',
	                            CASE
		                            WHEN res.id IS NOT NULL THEN 1 ELSE 0
	                            END AS 'responsablesbool',
	                            act.revisaID,
	                            act.revisa,
	                            com.id AS 'comID',
	                            com.actividadID AS 'comActividadID',
	                            com.comentario AS 'comComentario',
	                            com.usuarioNombre AS 'comUsuarioNombre',
	                            com.usuarioID AS 'comUsuarioID',
	                            CONVERT(varchar, com.fecha, 103) AS 'comFecha',
	                            com.tipo AS 'comTipo',
	                            com.adjuntoNombre AS 'comAdjuntoNombre',
	                            res2.usuarioID AS 'responsablesID'
                            FROM
	                            tblSA_Actividades AS act
                            INNER JOIN
	                            tblSA_Minuta AS minu
	                            ON
		                            minu.id = act.minutaID
                            LEFT JOIN
	                            tblSA_Interesados AS inte
	                            ON
		                            inte.actividadID = act.id AND
		                            inte.interesadoID = @paramID1
                            LEFT JOIN
	                            tblSA_Responsables AS res
	                            ON
		                            res.actividadID = act.id AND
		                            res.usuarioID = @paramID2
                            LEFT JOIN
	                            tblSA_Comentarios AS com
	                            ON
		                            com.actividadID = act.id
                            LEFT JOIN
	                            tblSA_Responsables as res2
	                            ON
		                            res2.actividadID = act.id
                            WHERE
	                            act.columna < 100 AND
	                            (
		                            (act.id IN (SELECT actividadID FROM tblSA_Responsables WHERE usuarioID = @paramID3)) OR
		                            (act.id IN (SELECT actividadID FROM tblSA_Interesados WHERE interesadoID = @paramID4))
	                            )",
                parametros = new { paramID1 = user, paramID2 = user, paramID3 = user, paramID4 = user }
            });

            var actividades = new List<tblSA_ActividadesDTO>();
            foreach (var item in resultado.GroupBy(x => x.id))
            {
                var pObj = new tblSA_ActividadesDTO();
                pObj.id = item.Key;
                pObj.minuta = item.First().minuta;
                pObj.minutaID = item.First().minutaID;
                pObj.columna = item.First().columna;
                pObj.orden = item.First().orden;
                pObj.tipo = item.First().tipo;
                pObj.actividad = item.First().actividad;
                pObj.descripcion = item.First().descripcion;
                pObj.responsableID = item.First().responsableID;
                pObj.responsable = item.First().responsable;
                pObj.fechaInicio = item.First().fechaInicio;
                pObj.fechaCompromiso = item.First().fechaCompromiso;
                pObj.prioridad = item.First().prioridad;
                pObj.comentariosCount = item.First().comentariosCount;
                
                pObj.comentarios = new List<tblSA_ComentariosDTO>();
                if (item.Any(x => x.comID != 0))
                {
                    foreach (var comentario in item.Where(x => x.comID != 0).GroupBy(x => x.comID))
                    {
                        var comen = new tblSA_ComentariosDTO();
                        comen.id = comentario.First().comID;
                        comen.actividadID = comentario.First().comActividadID;
                        comen.comentario = comentario.First().comComentario;
                        comen.usuarioNombre = comentario.First().comUsuarioNombre;
                        comen.usuarioID = comentario.First().comUsuarioID;
                        comen.fecha = comentario.First().comFecha;
                        comen.tipo = comentario.First().comTipo;
                        comen.adjuntoNombre = comentario.First().comAdjuntoNombre;
                        pObj.comentarios.Add(comen);
                    }
                }

                pObj.interesado = item.First().interesado;
                pObj.responsablesbool = item.First().responsablesbool;
                pObj.revisaID = item.First().revisaID;
                pObj.revisa = item.First().revisa;

                pObj.responsablesID = new List<int>();
                foreach (var responsables in item.GroupBy(x => x.responsablesID))
                {
                    pObj.responsablesID.Add(responsables.Key);
                }
                //pObj.responsablesID = _context.tblSA_Responsables.Where(x => x.actividadID == i.id).Select(x => x.usuarioID).ToList();
                actividades.Add(pObj);
            }
            //
            /*
            var mr = _context.tblSA_Responsables.Where(x => x.usuarioID == user).Select(x => x.actividadID).Distinct().ToList();
            var mrf = _context.tblSA_Actividades.Where(x => mr.Contains(x.id) && x.columna < 100).Select(x => x.id).Distinct().ToList();
            var mi = _context.tblSA_Interesados.Where(x => x.interesadoID == user).Select(x => x.actividadID).Distinct().ToList();
            var mif = _context.tblSA_Actividades.Where(x => mi.Contains(x.id) && x.columna < 100).Select(x => x.id).Distinct().ToList();
            var m = new List<int>();
            m.AddRange(mrf);
            m.AddRange(mif);
            var mf = new List<int>();
            mf.AddRange(m.Distinct());


            var result = _context.tblSA_Actividades.Where(x => mf.Contains(x.id)).Distinct().ToList();

            var actividades = new List<tblSA_ActividadesDTO>();
            foreach (var i in result)
            {
                var pObj = new tblSA_ActividadesDTO();
                pObj.id = i.id;
                pObj.minuta = i.minuta.titulo;
                pObj.minutaID = i.minutaID;
                pObj.columna = i.columna;
                pObj.orden = i.orden;
                pObj.tipo = i.tipo;
                pObj.actividad = i.actividad;
                pObj.descripcion = i.descripcion;
                pObj.responsableID = i.responsableID;
                pObj.responsable = i.responsable;
                pObj.fechaInicio = i.fechaInicio.ToShortDateString();
                pObj.fechaCompromiso = i.fechaCompromiso.ToShortDateString();
                pObj.prioridad = i.prioridad;
                pObj.comentariosCount = i.comentariosCount;
                pObj.comentarios = new List<tblSA_ComentariosDTO>();
                pObj.interesado = _context.tblSA_Interesados.FirstOrDefault(x => x.actividadID == pObj.id && x.interesadoID == user) != null ? true : false;
                pObj.responsablesbool = _context.tblSA_Responsables.FirstOrDefault(x => x.actividadID == pObj.id && x.usuarioID == user) != null ? true : false;
                pObj.revisaID = i.revisaID;
                pObj.revisa = i.revisa;
                pObj.responsablesID = new List<int>();
                pObj.responsablesID = _context.tblSA_Responsables.Where(x => x.actividadID == i.id).Select(x => x.usuarioID).ToList();
                actividades.Add(pObj);
                foreach (var j in i.comentarios)
                {
                    var cObj = new tblSA_ComentariosDTO();
                    cObj.id = j.id;
                    cObj.actividadID = j.actividadID;
                    cObj.comentario = j.comentario;
                    cObj.usuarioNombre = j.usuarioNombre;
                    cObj.usuarioID = j.usuarioID;
                    cObj.fecha = j.fecha.ToShortDateString();
                    cObj.tipo = j.tipo;
                    cObj.adjuntoNombre = j.adjuntoNombre;
                    var temp = actividades.FirstOrDefault(x => x.id == i.id);
                    temp.comentarios.Add(cObj);

                }

            }*/
            return actividades;
        }
        public void avanceActividad(int id, int columna)
        {
            tblSA_Actividades existing = _context.Set<tblSA_Actividades>().Find(id);
            if (existing != null)
            {
                existing.columna = columna;
                _context.SaveChanges();
                string minutaObject;
                try
                {
                    minutaObject = JsonUtils.convertNetObjectToJson(existing);
                }
                catch (Exception)
                {
                    minutaObject = "No se pudo serializar el objeto";
                }
                SaveBitacora((int)BitacoraEnum.ACTIVIDAD, (int)AccionEnum.ACTUALIZAR, id, minutaObject);
            }
        }
        public bool Exists(tblSA_Minuta obj)
        {
            return _context.tblSA_Minuta.Where(x => x.id == obj.id).ToList().Count > 0 ? true : false;
        }
        public bool ExistsA(tblSA_Actividades obj)
        {
            return _context.tblSA_Actividades.Where(x => x.id == obj.id).ToList().Count > 0 ? true : false;
        }
        public List<tblSA_ParticipanteDTO> getParticipantes(int minuta)
        {
            var result = new List<tblSA_ParticipanteDTO>();
            var temp = _context.tblSA_Participante.Where(x => x.minutaID == minuta);
            foreach (var i in temp)
            {
                var o = new tblSA_ParticipanteDTO();
                o.id = i.id;
                o.minutaID = i.minutaID;
                o.participante = i.participante;
                o.participanteID = i.participanteID;
                result.Add(o);
            }
            return result;
        }
        public bool validarPromoverAvanceActividad(int actividadID, int usuarioID, bool desdeMinuta = false)
        {

            var minuta = _context.tblSA_Minuta.FirstOrDefault(x => x.actividades.Any(y => y.id == actividadID));
            var actividad = _context.tblSA_Actividades.FirstOrDefault(x => x.id == actividadID);
            var responsables = _context.tblSA_Responsables.Where(x => x.actividadID == actividadID).Select(x => x.usuarioID).Distinct().ToList();
            var usuariosResponsables = _context.tblP_Usuario.FirstOrDefault(x => responsables.Contains(x.id));
            var autorizante = _context.tblP_AccionesVistatblP_Usuario.FirstOrDefault(x => x.tblP_AccionesVista_id == 6 && x.tblP_Usuario_id == usuarioID);
            // Si se intenta promover desde la Minuta verifica si es el Líder.
            if (desdeMinuta)
            {
                // Si el usuario es el líder de la minuta, también es válido a promover la actividad.
                if (minuta.creadorID == usuarioID)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else if ((autorizante != null && responsables.Contains(usuarioID)))
            {
                return true;
            }
            else if (actividad.revisaID == usuarioID)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool esResponsableACtividad(int id, int u)
        {
            var temp = _context.tblSA_Responsables.FirstOrDefault(x => x.actividadID == id && x.usuarioID == u);
            return temp != null ? true : false;
        }
        public void promoverAvanceActividad(tblSA_PromoverActividad obj)
        {
            AlertaFactoryServices alertaFactoryServices = new AlertaFactoryServices();

            IObjectSet<tblSA_PromoverActividad> _objectSet = ((IObjectContextAdapter)_context).ObjectContext.CreateObjectSet<tblSA_PromoverActividad>();
            if (obj == null) { throw new ArgumentNullException("Entity"); }
            var aut = _context.tblSA_Actividades.FirstOrDefault(x => x.id == obj.actividadID);
            var cre = _context.tblSA_Minuta.FirstOrDefault(x => x.id == aut.minutaID);
            obj.jefeID = aut.revisaID == 0 ? cre.creadorID : aut.revisaID;
            obj.estatus = true;
            obj.fechaRegistro = DateTime.Now;
            obj.fechaAccion = DateTime.Now;
            obj.accion = (int)AccionEnum.AGREGAR;
            obj.responsableID = vSesiones.sesionUsuarioDTO.id;
            _objectSet.AddObject(obj);
            _context.SaveChanges();
            tblSA_Comentarios nuevo = new tblSA_Comentarios();
            nuevo.actividadID = obj.actividadID;
            nuevo.comentario = obj.observacion;
            nuevo.usuarioNombre = vSesiones.sesionUsuarioDTO.nombre;
            nuevo.usuarioID = vSesiones.sesionUsuarioDTO.id;
            nuevo.fecha = DateTime.Now;
            nuevo.tipo = "new";
            nuevo.adjuntoNombre = "";
            guardarComentario(nuevo, null);
            SaveBitacora((int)BitacoraEnum.PARTICIPANTE, (int)AccionEnum.AGREGAR, obj.id, JsonUtils.convertNetObjectToJson(obj));
        }
        public void promocionAvanceActividad(int actividadID, int accion, string observacion)
        {
            tblSA_PromoverActividad existing = _context.tblSA_PromoverActividad.Find(actividadID);
            if (existing != null)
            {
                existing.accion = accion;
                existing.fechaAccion = DateTime.Now;
                existing.estatus = false;
                _context.SaveChanges();
                if (existing.accion == (int)AccionEnum.ACTUALIZAR)
                {
                    tblSA_Actividades actividad = _context.tblSA_Actividades.Find(existing.actividadID);
                    actividad.columna = existing.columna;
                    _context.SaveChanges();
                    SaveBitacora((int)BitacoraEnum.ACTIVIDAD, (int)AccionEnum.ACTUALIZAR, actividadID, JsonUtils.convertNetObjectToJson(actividad));
                }
                else if (accion == (int)AccionEnum.ELIMINAR)
                {
                    tblSA_Comentarios nuevo = new tblSA_Comentarios();
                    nuevo.actividadID = existing.actividadID;
                    nuevo.comentario = observacion;
                    nuevo.usuarioNombre = vSesiones.sesionUsuarioDTO.nombre;
                    nuevo.usuarioID = vSesiones.sesionUsuarioDTO.id;
                    nuevo.fecha = DateTime.Now;
                    nuevo.tipo = "new";
                    nuevo.adjuntoNombre = "";
                    guardarComentario(nuevo, null);
                }
            }
        }
        public List<PromoverDTO> getAllActividadesAPromover(int user)
        {
            var result = new List<PromoverDTO>();
            var temp = _context.tblSA_PromoverActividad.Where(x => x.jefeID == user && x.estatus == true).ToList();
            var us = temp.Select(x => x.responsableID).ToList();
            var ac = temp.Select(x => x.actividadID);
            var acts = _context.tblSA_Responsables.Where(x => ac.Contains(x.actividadID));
            var resp = _context.tblP_Usuario.Where(x => us.Contains(x.id));
            foreach (var i in temp)
            {
                var o = new PromoverDTO();
                o.id = i.id;
                o.minuta = _context.tblSA_Minuta.FirstOrDefault(x => x.actividades.Any(y => y.id == i.actividadID)).titulo;
                o.actividadID = i.actividadID;
                o.actividad = i.actividad.actividad;
                o.observacion = i.observacion;
                o.fechaRegistro = i.fechaRegistro.ToShortDateString();
                o.columna = i.columna;
                o.descripcion = i.actividad.descripcion;
                if (i.responsableID == 0)
                {
                    var u = acts.FirstOrDefault(x => x.actividadID == i.actividadID);
                    o.responsable = u.usuarioText;
                }
                else
                {
                    var u = resp.FirstOrDefault(x => x.id == i.responsableID);
                    o.responsable = u.nombre + " " + u.apellidoPaterno + " " + u.apellidoMaterno;
                }
                result.Add(o);
            }
            return result;
        }
        public int getJefeID(int actividadID)
        {
            var actividad = _context.tblSA_Actividades.FirstOrDefault(x => x.id == actividadID);
            var usuarioResponsable = _context.tblP_Usuario.FirstOrDefault(x => x.id == actividad.responsableID);
            var puestoPadre = usuarioResponsable.puesto.puestoPadreID;
            if (puestoPadre != null)
            {
                var jefePuestoID = usuarioResponsable.puesto.puestoPadreID;
                var usuarioJefe = _context.tblP_Usuario.FirstOrDefault(x => x.puesto.id == jefePuestoID);
                return usuarioJefe.id;
            }
            else
            {
                return 0;
            }

        }
        public List<minutaRptDTO> getMinutaPrint(int minuta, int empresa = 0)
        {
            if (empresa == 0) { empresa = vSesiones.sesionEmpresaActual; }
            var result = new List<minutaRptDTO>();
            using (var ctx = new MainContext(empresa))
            {
                var temp = ctx.tblSA_Actividades.Where(x => x.minutaID == minuta && x.enVersion == true).ToList();
                var unicos = temp.Select(x => x.id).Distinct().ToList();
                var resp = ctx.tblSA_Responsables.Where(x => unicos.Contains(x.actividadID)).ToList();
                var no = 1;
                foreach (var i in temp)
                {
                    var o = new minutaRptDTO();
                    o.no = no++;
                    o.planteamiento = i.actividad + ":\r\n" + i.descripcion + "\r\n";
                    o.seguimiento = "";
                    foreach (var j in i.comentarios.ToList())
                    {
                        o.seguimiento += "*" + j.usuarioNombre + " - " + j.fecha.ToShortDateString() + ":\r\n" + j.comentario + "\r\n";
                    }

                    o.fechaCompromiso = i.fechaCompromiso.ToString("dd/MM/yy");
                    var r = resp.Where(x => x.actividadID == i.id).Select(x => x.usuario).Distinct().ToList();
                    o.responsable = string.Join(" / ", r.Select(y => (y.nombre + " " + y.apellidoPaterno)));
                    result.Add(o);
                }
            }
            return result;
        }
        public List<listaAsistenciaMinutaRptDTO> getListaAsistenciaMinutaPrint(int minuta, int empresa = 0)
        {
            if (empresa == 0) { empresa = vSesiones.sesionEmpresaActual; }
            var result = new List<listaAsistenciaMinutaRptDTO>();

            using (var ctx = new MainContext(empresa))
            {
                var temp = ctx.tblSA_Participante.Where(x => x.minutaID == minuta).ToList();
                var no = 1;
                foreach (var i in temp)
                {
                    var o = new listaAsistenciaMinutaRptDTO();
                    o.no = no++;
                    o.nombre = i.participante;
                    var usuario = ctx.tblP_Usuario.FirstOrDefault(x => x.id == i.participanteID);
                    o.area = usuario.puesto.departamento.descripcion;
                    o.correo = usuario.correo;
                    result.Add(o);
                }
            }

            return result;
        }
        public int guardarResponsable(tblSA_Responsables obj)
        {
            if (obj.actividadID != 0)
            {
                IObjectSet<tblSA_Responsables> _objectSet = ((IObjectContextAdapter)_context).ObjectContext.CreateObjectSet<tblSA_Responsables>();
                if (obj == null) { throw new ArgumentNullException("Entity"); }
                _objectSet.AddObject(obj);
                _context.SaveChanges();
                SaveBitacora((int)BitacoraEnum.RESPONSABLESMINUTA, (int)AccionEnum.AGREGAR, obj.id, JsonUtils.convertNetObjectToJson(obj));
                return obj.id;
            }
            else
                return 0;

        }
        public void eliminarResponsable(tblSA_Responsables obj)
        {
            IObjectSet<tblSA_Responsables> _objectSet = ((IObjectContextAdapter)_context).ObjectContext.CreateObjectSet<tblSA_Responsables>();
            var p = _context.tblSA_Responsables.FirstOrDefault(x => x.actividadID == obj.actividadID && x.usuarioID == obj.usuarioID);
            _objectSet.DeleteObject(p);
            _context.SaveChanges();
            SaveBitacora((int)BitacoraEnum.RESPONSABLESMINUTA, (int)AccionEnum.ELIMINAR, obj.id, JsonUtils.convertNetObjectToJson(obj));
        }
        public List<tblSA_ResponsablesDTO> getResponsablesPorActividad(int actividad)
        {
            var result = new List<tblSA_ResponsablesDTO>();
            var temp = _context.tblSA_Responsables.Where(x => x.actividadID == actividad).ToList();
            foreach (var i in temp)
            {
                var o = new tblSA_ResponsablesDTO();
                o.id = i.id;
                o.minutaID = i.minutaID;
                o.usuario = i.usuarioText;
                o.usuarioID = i.usuarioID;
                result.Add(o);
            }
            return result;
        }
        public void guardarResponsables(List<tblSA_Responsables> obj)
        {
            IObjectSet<tblSA_Responsables> _objectSet = ((IObjectContextAdapter)_context).ObjectContext.CreateObjectSet<tblSA_Responsables>();
            if (obj == null) { throw new ArgumentNullException("Entity"); }
            foreach (var i in obj)
            {
                if (_context.tblSA_Responsables.FirstOrDefault(x => x.usuarioID == i.usuarioID && x.actividadID == i.actividadID) == null)
                {
                    _objectSet.AddObject(i);
                    _context.SaveChanges();
                }
            }

            foreach (var i in obj)
            {
                SaveBitacora((int)BitacoraEnum.RESPONSABLESMINUTA, (int)AccionEnum.AGREGAR, i.id, JsonUtils.convertNetObjectToJson(obj));
            }
        }
        public int guardarInteresado(tblSA_Interesados obj)
        {
            if (obj.actividadID != 0)
            {
                IObjectSet<tblSA_Interesados> _objectSet = ((IObjectContextAdapter)_context).ObjectContext.CreateObjectSet<tblSA_Interesados>();
                if (obj == null) { throw new ArgumentNullException("Entity"); }
                _objectSet.AddObject(obj);
                _context.SaveChanges();
                SaveBitacora((int)BitacoraEnum.INTERESADO, (int)AccionEnum.AGREGAR, obj.id, JsonUtils.convertNetObjectToJson(obj));
                return obj.id;
            }
            else
                return 0;

        }
        public void eliminarInteresado(tblSA_Interesados obj)
        {
            IObjectSet<tblSA_Interesados> _objectSet = ((IObjectContextAdapter)_context).ObjectContext.CreateObjectSet<tblSA_Interesados>();
            var p = _context.tblSA_Interesados.FirstOrDefault(x => x.actividadID == obj.actividadID && x.interesadoID == obj.interesadoID);
            _objectSet.DeleteObject(p);
            _context.SaveChanges();
            SaveBitacora((int)BitacoraEnum.INTERESADO, (int)AccionEnum.ELIMINAR, obj.id, JsonUtils.convertNetObjectToJson(obj));
        }
        public List<tblSA_InteresadosDTO> getInteresadosPorActividad(int actividad)
        {
            var result = new List<tblSA_InteresadosDTO>();
            var temp = _context.tblSA_Interesados.Where(x => x.actividadID == actividad).ToList();
            foreach (var i in temp)
            {
                var o = new tblSA_InteresadosDTO();
                o.id = i.id;
                o.minutaID = i.minutaID;
                o.interesado = i.interesado;
                o.interesadoID = i.interesadoID;
                result.Add(o);
            }
            return result;
        }
        public void guardarInteresados(List<tblSA_Interesados> obj)
        {
            IObjectSet<tblSA_Interesados> _objectSet = ((IObjectContextAdapter)_context).ObjectContext.CreateObjectSet<tblSA_Interesados>();
            if (obj == null) { throw new ArgumentNullException("Entity"); }
            foreach (var i in obj)
            {
                if (_context.tblSA_Interesados.FirstOrDefault(x => x.interesadoID == i.interesadoID && x.actividadID == i.actividadID) == null)
                {
                    _objectSet.AddObject(i);
                    _context.SaveChanges();
                }
            }

            foreach (var i in obj)
            {
                SaveBitacora((int)BitacoraEnum.INTERESADO, (int)AccionEnum.AGREGAR, i.id, JsonUtils.convertNetObjectToJson(obj));
            }
        }
        public tblSA_Comentarios getComentarioByID(int id)
        {
            return _context.tblSA_Comentarios.FirstOrDefault(x => x.id == id);
        }
        public List<string> enviarCorreos(int minutaID, List<int> usuarios, List<Byte[]> downloadPDF)
        {
            var result = new List<string>();
            var contactos = _context.tblP_Usuario.Where(x => usuarios.Contains(x.id)).ToList();


            var minuta = _context.tblSA_Minuta.FirstOrDefault(x => x.id == minutaID);
            var actividades = _context.tblSA_Actividades.Where(x => x.minutaID == minutaID).ToList();
            var participantes = _context.tblSA_Participante.Where(x => x.minutaID == minutaID).ToList();
            var interesados = _context.tblSA_Interesados.Where(x => x.minutaID == minutaID).ToList();
            var responsables = _context.tblSA_Responsables.Where(x => x.minutaID == minutaID).ToList();
            foreach (var c in contactos)
            {
                try
                {
                    List<int> ai = new List<int>();
                    var aiTemp = interesados.Where(x => x.interesadoID == c.id).Select(x => x.actividadID).ToList();
                    ai.AddRange(aiTemp);
                    List<string> correo = new List<string>();
                    correo.Add(c.correo);

                    var subject = "Minuta:   " + minuta.titulo + " Fecha: " + minuta.fecha.ToShortDateString();
                    var body = @"Proyecto:   " + minuta.proyecto + @"<br/>Evento:     " + minuta.titulo + @"<br/>Sala;       " + minuta.lugar + @"<br/>Fecha;      " + minuta.fecha.ToShortDateString() + @"<br/>Hora inicio:" + minuta.horaInicio + @"<br/>Hora fin   :" + minuta.horaFin + @"<br/><br/>Actividades que le fueron asignadas:" + @"<br/>";
                    foreach (var a in responsables.Where(x => x.usuarioID == c.id))
                    {
                        body += @"Actividad:       " + a.actividad.actividad + @"<br/>
                                  Fecha inicio:    " + a.actividad.fechaInicio.ToShortDateString() + @"
                                  Fecha compromiso:" + a.actividad.fechaCompromiso.ToShortDateString() + "<br/>-----------------<br/>";
                    }
                    body += @"<br/>Actividades en que se le marco como interesado:<br/>";
                    foreach (var a in actividades.Where(x => ai.Contains(x.id)))
                    {
                        body += @"Actividad:       " + a.actividad + @"<br/>
                                  Fecha inicio:    " + a.fechaInicio.ToShortDateString() + @"
                                  Fecha compromiso:" + a.fechaCompromiso.ToShortDateString() + "<br/>-----------------<br/>";
                    }

                    var r = GlobalUtils.sendEmailAdjuntoInMemory(string.Format("{0}: {1}", PersonalUtilities.GetNombreEmpresa(), subject), body, correo, downloadPDF);

                    if (!r)
                    {
                        result.Add((c.nombre + " " + c.apellidoPaterno + " " + c.apellidoMaterno));
                    }
                }
                catch (Exception ex)
                {
                    result.Add((c.nombre + " " + c.apellidoPaterno + " " + c.apellidoMaterno));
                    LogError(0, 0, "SeguimientoAcuerdosController", "enviarCorreos", ex, AccionEnum.CORREO, 0, null);
                }
            }

            return result;
        }
        public List<string> enviarCorreosComentario(tblSA_ComentariosDTO comentarioObj)
        {
            var result = new List<string>();
            var responsables = _context.tblSA_Responsables.Where(x => x.actividadID == comentarioObj.actividadID && x.usuarioID != comentarioObj.usuarioID).Select(x => x.usuarioID).Distinct().ToList();
            var comentarios = _context.tblSA_Comentarios.Where(x => x.actividadID == comentarioObj.actividadID && x.usuarioID != comentarioObj.usuarioID).Select(x => x.usuarioID).Distinct().ToList();
            var comprobacion = _context.tblSA_Responsables.FirstOrDefault(x => x.actividadID == comentarioObj.actividadID && x.usuarioID == comentarioObj.usuarioID);
            var isResponsable = comprobacion != null ? true : false;
            var contactos = _context.tblP_Usuario.Where(x => (isResponsable ? comentarios.Contains(x.id) : responsables.Contains(x.id))).ToList();
            var actividad = _context.tblSA_Actividades.FirstOrDefault(x => x.id == comentarioObj.actividadID);
            var usuarioComento = _context.tblP_Usuario.FirstOrDefault(x => x.id == comentarioObj.usuarioID);
            var minuta = _context.tblSA_Minuta.FirstOrDefault(x => x.id == actividad.minutaID);

            foreach (var c in contactos)
            {
                try
                {
                    List<string> correo = new List<string>();
                    correo.Add(c.correo);

                    var subject = "Numero comentario en Minuta:   " + minuta.titulo + " Fecha Comentario: " + comentarioObj.fecha;

                    var body = @"Actividad:        " + actividad.actividad + @"<br/>
                                 Fecha inicio:     " + actividad.fechaInicio.ToShortDateString() + @"
                                 Fecha compromiso: " + actividad.fechaCompromiso.ToShortDateString() + "<br/>-----------------<br/>";
                    body += @"Usuario comento:  " + (usuarioComento.nombre ?? "") + " " + (usuarioComento.apellidoPaterno ?? "") + " " + (usuarioComento.apellidoMaterno ?? "") + @"<br/>
                                 Fecha comentario: " + comentarioObj.fecha + @"<br/>
                                 Comentario:       " + comentarioObj.comentario + @"<br/>
                                 Archivo adjunto:  " + (string.IsNullOrEmpty(comentarioObj.adjuntoNombre) ? "---" : comentarioObj.adjuntoNombre) + "<br/>-----------------<br/>";


                    var r = GlobalUtils.sendEmail(string.Format("{0}: {1}", PersonalUtilities.GetNombreEmpresa(), subject), body, correo);
                }
                catch (Exception ex)
                {
                    //result.Add((c.nombre + " " + c.apellidoPaterno + " " + c.apellidoMaterno));
                }
            }

            return result;
        }
        public List<tblP_Usuario> FillComboParticipantes(int minutaID)
        {
            List<int> lista = new List<int>();
            var temp = _context.tblSA_Responsables.Where(x => x.minutaID == minutaID).Select(x => x.usuarioID).ToList();
            lista.AddRange(temp);
            int temp2 = _context.tblSA_Minuta.FirstOrDefault(x => x.id == minutaID).creadorID;
            lista.Add(temp2);
            List<tblP_Usuario> data = new List<tblP_Usuario>();
            data = _context.tblP_Usuario.Where(x => lista.Contains(x.id)).Distinct().ToList();
            return data;
        }
        public List<tblSA_Minuta> getMinutas(int user)
        {
            return _context.tblSA_Minuta.Where(x => x.creadorID == user).ToList();
        }
        public List<tblP_Usuario> FillComboUsuarios(int minutaID)
        {
            List<int> lista = new List<int>();
            var temp = _context.tblSA_Participante.Where(x => x.minutaID == minutaID).Select(x => x.participanteID).ToList();
            lista.AddRange(temp);
            int temp2 = _context.tblSA_Minuta.FirstOrDefault(x => x.id == minutaID).creadorID;
            lista.Add(temp2);
            var temp3 = _context.tblSA_Interesados.Where(x => x.minutaID == minutaID).Select(x => x.interesadoID).ToList();
            lista.AddRange(temp3);
            List<tblP_Usuario> data = new List<tblP_Usuario>();
            List<int> listaUnica = new List<int>();
            listaUnica = lista.Distinct().ToList();
            data = _context.tblP_Usuario.Where(x => listaUnica.Contains(x.id)).ToList();
            return data;
        }
        private string HoraToString(int hora)
        {
            string result = "";
            switch (hora)
            {
                case 0:
                    result = "00:00";
                    break;
                case 1:
                    result = "01:00";
                    break;
                case 2:
                    result = "02:00";
                    break;
                case 3:
                    result = "03:00";
                    break;
                case 4:
                    result = "04:00";
                    break;
                case 5:
                    result = "05:00";
                    break;
                case 6:
                    result = "06:00";
                    break;
                case 7:
                    result = "07:00";
                    break;
                case 8:
                    result = "08:00";
                    break;
                case 9:
                    result = "09:00";
                    break;
                case 10:
                    result = "10:00";
                    break;
                case 11:
                    result = "11:00";
                    break;
                case 12:
                    result = "12:00";
                    break;
                case 13:
                    result = "13:00";
                    break;
                case 14:
                    result = "14:00";
                    break;
                case 15:
                    result = "15:00";
                    break;
                case 16:
                    result = "16:00";
                    break;
                case 17:
                    result = "17:00";
                    break;
                case 18:
                    result = "18:00";
                    break;
                case 19:
                    result = "19:00";
                    break;
                case 20:
                    result = "20:00";
                    break;
                case 21:
                    result = "21:00";
                    break;
                case 22:
                    result = "22:00";
                    break;
                case 23:
                    result = "23:00";
                    break;
                default:
                    break;
            }
            return result;
        }
        public void setDataCustom()
        {
            var actividades = _context.tblSA_Actividades.ToList();
            IObjectSet<tblSA_Responsables> _objectSet = ((IObjectContextAdapter)_context).ObjectContext.CreateObjectSet<tblSA_Responsables>();
            foreach (var i in actividades)
            {
                var r = new tblSA_Responsables();
                r.minutaID = i.minutaID;
                r.usuarioID = i.responsableID;
                r.usuarioText = i.responsable;
                r.actividadID = i.id;
                _objectSet.AddObject(r);
                _context.SaveChanges();
            }

        }
        public int getActivitiesCount(int id)
        {
            var cantidad = 0;
            cantidad = _context.tblSA_Actividades.Where(x => x.minutaID == id).Count();
            return cantidad;
        }
        public List<ActividadesTipoDTO> getReporteActividades(List<int> departamentoID, DateTime fechaInicio, DateTime fechaFin, int estatus)
        {
            fechaFin = fechaFin.AddHours(23).AddMinutes(59).AddSeconds(59);
            var result = new List<ActividadesTipoDTO>();
            var responsables = _context.tblSA_Responsables.Where(x => departamentoID.Contains(x.usuario.puesto.departamentoID) && x.actividad.fechaCompromiso >= fechaInicio && x.actividad.fechaCompromiso <= fechaFin && (estatus == 1 ? x.actividad.columna < 100 : x.actividad.columna == 100)).ToList();
            var usuarios = _context.tblP_Usuario.ToList();
            foreach (var i in responsables)
            {
                var o = new ActividadesTipoDTO();
                o.creadorMinutaID = i.minuta.creadorID;
                o.creadorMinuta = getNombreUsuarioByID(o.creadorMinutaID, usuarios);
                o.minutaID = i.minutaID;
                o.minuta = i.minuta.titulo;
                o.departamentoID = i.usuario.puesto.departamentoID;
                o.departamento = i.usuario.puesto.departamento.descripcion;
                o.actividadID = i.actividadID;
                o.actividad = i.actividad.actividad;
                o.responsableID = i.usuarioID;
                o.responsable = getNombreUsuarioByID(i.usuarioID, usuarios);
                o.vFechaInicio = i.actividad.fechaInicio;
                o.fechaInicio = i.actividad.fechaInicio.ToShortDateString();
                o.vFechaFin = i.actividad.fechaCompromiso;
                o.fechaFin = i.actividad.fechaCompromiso.ToShortDateString();
                o.prioridad = EnumExtensions.GetDescription((PrioridadEnum)i.actividad.prioridad);
                result.Add(o);
            }
            return result;
        }
        public List<Minutas_ActividadesPendientesDTO> getReporteMinutasPendientes(List<int> departamentoID, DateTime fechaInicio, DateTime fechaFin)
        {
            fechaFin = fechaFin.AddHours(23).AddMinutes(59).AddSeconds(59);
            var result = new List<Minutas_ActividadesPendientesDTO>();
            var minutasID = _context.tblSA_Actividades.Where(x => departamentoID.Contains(x.minuta.creador.puesto.departamentoID) && x.fechaCompromiso >= fechaInicio && x.fechaCompromiso <= fechaFin && x.columna < 100).Select(x => x.minutaID).ToList();
            var minutas = _context.tblSA_Minuta.Where(x => minutasID.Contains(x.id)).ToList();
            var usuarios = _context.tblP_Usuario.ToList();
            foreach (var i in minutas)
            {
                var u = usuarios.FirstOrDefault(x => x.id == i.creadorID);
                var o = new Minutas_ActividadesPendientesDTO();
                o.creadorMinutaID = i.creadorID;
                o.creadorMinuta = getNombreUsuarioByID(o.creadorMinutaID, usuarios);
                o.minutaID = i.id;
                o.minuta = "<a target='_blank' href='/SeguimientoAcuerdos/Acuerdo?minuta=" + i.id + "'> " + i.titulo + "</a>";
                if (u.puesto != null)
                {
                    o.departamentoID = u.puesto.departamentoID;
                    o.departamento = u.puesto.departamento.descripcion;

                }

                var actividades = _context.tblSA_Actividades.Where(x => x.minutaID == o.minutaID).ToList();
                o.actividadesPendientes = actividades.Where(x => x.columna < 100).ToList().Count();
                o.actividadesFinalizadas = actividades.Where(x => x.columna == 100).ToList().Count();
                o.actividadesTotal = actividades.Count();
                o.btnListaAsistencia = "<button class=\"btn\" title=\"Seguimiento\" onclick=\"imprimirListaAsistencia(" + i.id + ")\"><i class=\"fa fa-print\"><!--<i--></i></button>";
                o.btnReporteMinuta = "<button class=\"btn\" title=\"Seguimiento\" onclick=\"imprimirMinuta(" + i.id + ")\"><i class=\"fa fa-print\"><!--<i--></i></button>";

                result.Add(o);
            }
            return result;
        }
        public List<EstadisticoMinutasDTO> getReporteEstadisticoMinutas(List<int> departamentoID, DateTime fechaInicio, DateTime fechaFin)
        {
            fechaFin = fechaFin.AddHours(23).AddMinutes(59).AddSeconds(59);
            var result = new List<EstadisticoMinutasDTO>();
            var departamentos = _context.tblP_Departamento.Where(x => departamentoID.Contains(x.id)).ToList();
            foreach (var i in departamentos)
            {
                var o = new EstadisticoMinutasDTO();
                o.departamentoID = i.id;
                o.departamento = i.descripcion;
                var minutas = _context.tblSA_Minuta.Where(x => x.creador.puesto.departamentoID == i.id && x.fecha >= fechaInicio && x.fecha <= fechaFin).ToList();
                o.minutasFinalizadas = minutas.Where(x => x.actividades.Any(y => y.columna < 100) == false).Count();
                o.minutasActividadPendiente = minutas.Where(x => x.actividades.Any(y => y.columna < 100) == true).Count();
                o.minutasTotal = minutas.Count();
                result.Add(o);
            }
            return result;
        }
        public List<BitacoraSeguimientoAcuerdosDTO> getBitacoraMinutas(List<int> departamentoID, DateTime fechaInicio, DateTime fechaFin)
        {
            fechaFin = fechaFin.AddHours(23).AddMinutes(59).AddSeconds(59);
            var result = new List<BitacoraSeguimientoAcuerdosDTO>();
            var minutas = _context.tblSA_Minuta.Where(x => departamentoID.Contains(x.creador.puesto.departamentoID) && x.fecha >= fechaInicio && x.fecha <= fechaFin).ToList();
            var usuarios = _context.tblP_Usuario.ToList();
            foreach (var i in minutas)
            {
                var o = new BitacoraSeguimientoAcuerdosDTO();
                o.Departamento = i.creador.puesto.departamento.descripcion;
                o.Usuario = getNombreUsuarioByID(i.creadorID, usuarios);
                o.Proyecto = i.proyecto;
                o.Minuta = "<a target='_blank' href='/SeguimientoAcuerdos/Acuerdo?minuta=" + i.id + "'> " + i.titulo + "</a>";
                o.vFecha = i.fecha;
                o.fecha = i.fecha.ToShortDateString();
                o.horaInicio = i.horaInicio;
                o.horaFin = i.horaFin;
                o.lugar = i.lugar;
                o.btnListaAsistencia = "<button class=\"btn\" title=\"Seguimiento\" onclick=\"imprimirListaAsistencia(" + i.id + ")\"><i class=\"fa fa-print\"><!--<i--></i></button>";
                o.btnReporteMinuta = "<button class=\"btn\" title=\"Seguimiento\" onclick=\"imprimirMinuta(" + i.id + ")\"><i class=\"fa fa-print\"><!--<i--></i></button>";
                result.Add(o);
            }
            return result;
        }
        public string getNombreUsuarioByID(int id, List<tblP_Usuario> lista)
        {
            var result = "";
            var usuario = lista.FirstOrDefault(x => x.id == id);
            result = (usuario.nombre ?? "") + " " + (usuario.apellidoPaterno ?? "") + " " + (usuario.apellidoMaterno ?? "");
            return result;
        }
        public tblP_Usuario getUsuario(int id, List<tblP_Usuario> lista)
        {
            return lista.FirstOrDefault(x => x.id == id);
        }
        public List<tblP_Departamento> getDepartamentos()
        {
            return _context.tblP_Departamento.ToList();
        }
        private bool insertEnvioGestor(int minutaID)
        {
            try
            {
                var empresa = vSesiones.sesionEmpresaActual;
                var minuta = _context.tblSA_Minuta.FirstOrDefault(x => x.id == minutaID);
                using (var ctx = new MainContext((int)EmpresaEnum.Construplan))
                {
                    tblFM_EnvioDocumento documento = new tblFM_EnvioDocumento();
                    documento.id = 0;
                    documento.documentoID = minutaID;
                    documento.descripcion = minuta.proyecto + "-" + minuta.titulo;
                    documento.tipoDocumento = 18;
                    documento.usuarioID = minuta.creadorID;
                    documento.estatus = 0;
                    documento.empresa = empresa;
                    documento.fecha = DateTime.Today;
                    ctx.tblFM_EnvioDocumento.Add(documento);
                    ctx.SaveChanges();
                    return true;
                }
            }
            catch (Exception e)
            {
                return false;
            }
        }
    }
}
