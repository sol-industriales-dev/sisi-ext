using Core.DAO.RecursosHumanos.Mural;
using Core.DTO;
using Core.DTO.Principal.Generales;
using Core.DTO.RecursosHumanos.Mural;
using Core.Entity.Administrativo.RecursosHumanos.Mural;
using Data.EntityFramework.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.DAO.RecursosHumanos.Mural
{
    public class MuralDAO : GenericDAO<tblMural_Data>, IMuralDAO
    {
        #region Viejo
        public Respuesta EliminarSeccion(int idSeccion)
        {
            var r = new Respuesta();

            try
            {
                var seccion = _context.tblRH_Mural_Seccion.First(f => f.Id == idSeccion);

                seccion.Estatus = false;
                seccion.FechaModificacion = DateTime.Now;

                _context.SaveChanges();

                r.Success = true;
                r.Message = "Ok";
            }
            catch (Exception ex)
            {
                r.Message += ex.Message;
            }

            return r;
        }

        public Respuesta GuardarSeccion(tblRH_Mural_Seccion seccion)
        {
            var r = new Respuesta();

            try
            {
                if (seccion.Id != 0)
                {
                    var modificarSeccion = _context.tblRH_Mural_Seccion.FirstOrDefault(f => f.Id == seccion.Id && f.Estatus);

                    if (modificarSeccion != null)
                    {
                        modificarSeccion.Altura = seccion.Altura;
                        modificarSeccion.Ancho = seccion.Ancho;
                        modificarSeccion.ColorFondo = seccion.ColorFondo;
                        modificarSeccion.FechaModificacion = DateTime.Now;
                        modificarSeccion.PosicionX = seccion.PosicionX;
                        modificarSeccion.PosicionY = seccion.PosicionY;

                        _context.SaveChanges();

                        r.Success = true;
                        r.Message = "Ok";
                        r.Value = modificarSeccion.Id;
                    }
                    else
                    {
                        r.Message = "No se completó la modificación de la sección";
                    }
                }
                else
                {
                    seccion.Estatus = true;
                    seccion.FechaCreacion = DateTime.Now;
                    seccion.FechaModificacion = seccion.FechaCreacion;
                    seccion.IdUsuarioCreacion = vSesiones.sesionUsuarioDTO.id;

                    _context.tblRH_Mural_Seccion.Add(seccion);
                    _context.SaveChanges();

                    r.Success = true;
                    r.Message = "Ok";
                    r.Value = seccion.Id;
                }
            }
            catch (Exception ex)
            {
                r.Message += ex.Message;
            }

            return r;
        }
        public Respuesta EliminarPostIt(int idPostIt)
        {
            var r = new Respuesta();

            try
            {
                var postIt = _context.tblRH_Mural_PostIt.First(f => f.Id == idPostIt);
                
                postIt.Estatus = false;
                postIt.FechaModificacion = DateTime.Now;

                _context.SaveChanges();

                r.Success = true;
                r.Message = "Ok";
            }
            catch (Exception ex)
            {
                r.Message += ex.Message;
            }

            return r;
        }

        public Respuesta SavePostIt(tblRH_Mural_PostIt postIt)
        {
            var r = new Respuesta();

            try
            {
                if (postIt.Id != 0)
                {
                    var modificarPostIt = _context.tblRH_Mural_PostIt.FirstOrDefault(f => f.Id == postIt.Id && postIt.Estatus);

                    if (modificarPostIt != null)
                    {
                        modificarPostIt.Altura = postIt.Altura;
                        modificarPostIt.Ancho = postIt.Ancho;
                        modificarPostIt.ColorFondo = postIt.ColorFondo;
                        modificarPostIt.FechaModificacion = DateTime.Now;
                        modificarPostIt.IdSeccion = postIt.IdSeccion;
                        modificarPostIt.PosicionX = postIt.PosicionX;
                        modificarPostIt.PosicionY = postIt.PosicionY;
                        modificarPostIt.Texto = postIt.Texto;

                        _context.SaveChanges();

                        r.Success = true;
                        r.Message = "Ok";
                        r.Value = modificarPostIt.Id;
                    }
                    else
                    {
                        r.Message = "No se completó la modificación de la nota";
                    }
                }
                else
                {
                    postIt.Estatus = true;
                    postIt.FechaCreacion = DateTime.Now;
                    postIt.FechaModificacion = postIt.FechaCreacion;
                    postIt.IdUsuarioCreacion = vSesiones.sesionUsuarioDTO.id;

                    _context.tblRH_Mural_PostIt.Add(postIt);
                    _context.SaveChanges();

                    r.Success = true;
                    r.Message = "Ok";
                    r.Value = postIt.Id;
                }
            }
            catch (Exception ex)
            {
                r.Message += ex.Message;
            }

            return r;
        }

        public Respuesta CrearMural(tblRH_Mural mural)
        {
            var r = new Respuesta();

            try
            {
                var newMural = new tblRH_Mural();
                newMural.Color = mural.Color;
                newMural.Estatus = true;
                newMural.FechaCreacion = DateTime.Now;
                newMural.FechaModificacion = newMural.FechaCreacion;
                newMural.IdUsuarioCreacion = vSesiones.sesionUsuarioDTO.id;
                newMural.Titulo = mural.Titulo;

                _context.tblRH_Mural.Add(newMural);
                _context.SaveChanges();

                r.Success = true;
                r.Message = "Ok";
                r.Value = newMural.Id;
            }
            catch (Exception ex)
            {
                r.Message += ex.Message;
            }

            return r;
        }

        public Respuesta GetMural(int idMural)
        {
            var r = new Respuesta();

            try
            {
                var mural = _context.tblRH_Mural.FirstOrDefault(f => f.Id == idMural && f.Estatus);

                if (mural != null)
                {
                    var muralPostItDTO = new MuralPostItDTO();
                    var muralDTO = new MuralDTO();
                    var postItDTO = new List<PostItDTO>();
                    var seccionDTO = new List<SeccionDTO>();

                    muralDTO.Id = mural.Id;
                    muralDTO.Titulo = mural.Titulo;
                    muralDTO.Color = mural.Color;
                    muralDTO.FechaModificacion = mural.FechaModificacion;

                    foreach (var item in mural.PostItList.Where(x => x.Estatus))
                    {
                        var postIt = new PostItDTO();

                        postIt.Id = item.Id;
                        postIt.Texto = item.Texto;
                        postIt.PosicionX = item.PosicionX;
                        postIt.PosicionY = item.PosicionY;
                        postIt.Color = item.ColorFondo;
                        postIt.Altura = item.Altura;
                        postIt.Ancho = item.Ancho;
                        postIt.IdSeccion = item.IdSeccion;

                        postItDTO.Add(postIt);
                    }

                    foreach (var item in mural.SeccionList.Where(x => x.Estatus))
                    {
                        var seccion = new SeccionDTO();

                        seccion.Id = item.Id;
                        seccion.PosicionX = item.PosicionX;
                        seccion.PosicionY = item.PosicionY;
                        seccion.Color = item.ColorFondo;
                        seccion.Altura = item.Altura;
                        seccion.Ancho = item.Ancho;

                        seccionDTO.Add(seccion);
                    }

                    muralPostItDTO.Mural = muralDTO;
                    muralPostItDTO.PostIt = postItDTO;
                    muralPostItDTO.Seccion = seccionDTO;

                    r.Success = true;
                    r.Message = "Ok";
                    r.Value = muralPostItDTO;
                }
                else
                {
                    r.Message = "No se encontró el mural";
                }
            }
            catch (Exception ex)
            {
                r.Message += ex.Message;
            }

            return r;
        }

        public Respuesta SaveMural(tblRH_Mural mural, List<tblRH_Mural_PostIt> postIt)
        {
            var r = new Respuesta();

            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    mural.Estatus = true;
                    mural.FechaCreacion = DateTime.Now;
                    mural.FechaModificacion = DateTime.Now;
                    mural.IdUsuarioCreacion = vSesiones.sesionUsuarioDTO.id;

                    if (mural.Id != 0)
                    {
                        var muralGuardado = _context.tblRH_Mural.First(f => f.Id == mural.Id && f.Estatus);

                        muralGuardado.Color = mural.Color;
                        muralGuardado.FechaModificacion = mural.FechaModificacion;
                        muralGuardado.Titulo = mural.Titulo;

                        var nuevosPostIt = new List<tblRH_Mural_PostIt>();
                        var postItAEliminar = new List<tblRH_Mural_PostIt>();

                        foreach (var item in muralGuardado.PostItList.Where(w => w.Estatus))
                        {
                            var existePostIt = postIt.FirstOrDefault(f => f.Id == item.Id);

                            if (existePostIt != null)
                            {
                                item.Altura = existePostIt.Altura;
                                item.Ancho = existePostIt.Ancho;
                                item.ColorFondo = existePostIt.ColorFondo;
                                item.FechaModificacion = mural.FechaModificacion;
                                item.PosicionX = existePostIt.PosicionX;
                                item.PosicionY = existePostIt.PosicionY;
                                item.Texto = existePostIt.Texto;

                                postIt.Remove(existePostIt);
                            }
                            else
                            {
                                postItAEliminar.Add(item);
                            }
                        }

                        foreach (var item in postItAEliminar)
                        {
                            item.Estatus = false;
                            item.FechaModificacion = mural.FechaModificacion;
                        }

                        foreach (var item in postIt)
                        {
                            item.Estatus = true;
                            item.FechaCreacion = mural.FechaModificacion;
                            item.FechaModificacion = mural.FechaModificacion;
                            item.IdMural = muralGuardado.Id;

                            muralGuardado.PostItList.Add(item);
                        }

                        _context.SaveChanges();
                    }
                    else
                    {
                        _context.tblRH_Mural.Add(mural);
                        _context.SaveChanges();

                        foreach (var notas in postIt)
                        {
                            notas.IdMural = mural.Id;
                            notas.Estatus = true;
                            notas.FechaCreacion = mural.FechaCreacion;
                            notas.FechaModificacion = mural.FechaModificacion;
                            notas.IdUsuarioCreacion = mural.IdUsuarioCreacion;
                        }

                        _context.tblRH_Mural_PostIt.AddRange(postIt);
                        _context.SaveChanges();
                    }

                    transaction.Commit();

                    r.Success = true;
                    r.Message = "Ok";
                }
                catch (Exception ex)
                {
                    transaction.Rollback();

                    r.Message += ex.Message;
                }
            }

            return r;
        }

        public Dictionary<string, object> CboxMural()
        {
            var r = new Dictionary<string, object>();

            try
            {
                var murales = _context.tblRH_Mural.Where(x => x.Estatus);

                var cbox = murales.Select(m => new ComboDTO
                {
                    Value = m.Id.ToString(),
                    Text = m.Titulo
                });

                r.Add(SUCCESS, true);
                r.Add(ITEMS, cbox);
            }
            catch (Exception ex)
            {
                r.Add(SUCCESS, false);
                r.Add(MESSAGE, ex.Message);
            }

            return r;
        }
        #endregion

        #region Nuevo
        public void setMural(int id,string datos,string icono)
        {
            var d = _context.tblMural_Workspace.FirstOrDefault(x=>x.id == id);
            d.contenido = datos;
            d.fechaModificacion = DateTime.Now;
            if (!icono.Equals("nosave"))
            {
                d.icono = icono;
            }
            _context.SaveChanges();
        }

        public tblMural_Workspace getMural(int id)
        {
            var d = _context.tblMural_Workspace.FirstOrDefault(x => x.id == id);
            return d;
        }

        public List<tblMural_Workspace> getMuralList(bool propio)
        {
            var murales = new List<tblMural_Workspace>();

            if (propio)
            {
                murales = _context.tblMural_Workspace.Where(x=>x.usuarioID == vSesiones.sesionUsuarioDTO.id && x.estatus == true).ToList();
            }
            else {
                var muralestemp = _context.tblMural_Workspace_Members.Where(x=>x.usuarioID == vSesiones.sesionUsuarioDTO.id && x.estatus == true).Select(x=>x.workSpaceID).ToList();
                murales = _context.tblMural_Workspace.Where(x => muralestemp.Contains(x.id) && x.estatus == true).ToList();
            }
            return murales;
        }
        public void createNewMural(string nombre, string desc)
        {

            var mural = new tblMural_Workspace();
            mural.nombre = nombre;
            mural.descripcion = desc;
            mural.usuarioID = vSesiones.sesionUsuarioDTO.id;
            mural.fechaCreacion = DateTime.Now;
            mural.fechaModificacion = DateTime.Now;
            mural.contenido = "";
            mural.icono = "";
            mural.modificado = false;
            mural.estatus = true;
            _context.tblMural_Workspace.Add(mural);
            _context.SaveChanges();
        }
        public void renameMural(int id, string nombre)
        {
            var obj = _context.tblMural_Workspace.FirstOrDefault(x => x.id == id);
            obj.nombre = nombre;
            _context.SaveChanges();
        }
        public void duplicateNewMural(int id, string nombre, string desc)
        {
            var obj = _context.tblMural_Workspace.FirstOrDefault(x=>x.id == id);
            var mural = new tblMural_Workspace();
            mural.nombre = nombre;
            mural.descripcion = desc;
            mural.usuarioID = vSesiones.sesionUsuarioDTO.id;
            mural.fechaCreacion = DateTime.Now;
            mural.fechaModificacion = DateTime.Now;
            mural.contenido = obj.contenido;
            mural.icono = obj.icono;
            mural.modificado = true;
            mural.estatus = true;
            _context.tblMural_Workspace.Add(mural);
            _context.SaveChanges();
        }
        public void deleteMural(int id)
        {
            var obj = _context.tblMural_Workspace.FirstOrDefault(x => x.id == id);
            obj.estatus = false;
            _context.SaveChanges();
        }
        public void setUsuarioMural(int idUsuario, int idMural, int tipo)
        {
            var user = _context.tblP_Usuario.FirstOrDefault(x=>x.id == idUsuario);
            var userName = (string.IsNullOrEmpty(user.nombre) ? "": user.nombre) + " " + (string.IsNullOrEmpty(user.apellidoPaterno) ? "" : user.apellidoPaterno) + " " + (string.IsNullOrEmpty(user.apellidoMaterno) ? "" : user.apellidoMaterno);
            var member = new tblMural_Workspace_Members();
            member.usuarioID = idUsuario;
            member.workSpaceID = idMural;
            member.tipo = tipo;
            member.estatus = true;
            member.usuarioNombre = userName;
            _context.tblMural_Workspace_Members.Add(member);
            _context.SaveChanges();
        }
        public void updateUsuarioMural(int id, int tipo)
        {
            var member = _context.tblMural_Workspace_Members.FirstOrDefault(x=>x.id == id);
            member.tipo = tipo;
            _context.SaveChanges();
        }
        public void deleteUsuarioMural(int id)
        {
            var member = _context.tblMural_Workspace_Members.FirstOrDefault(x => x.id == id);
            _context.tblMural_Workspace_Members.Remove(member);
            _context.SaveChanges();
        }
        public List<tblMural_Workspace_Members> getUserMuralList(int id)
        {
            var members = _context.tblMural_Workspace_Members.Where(x => x.workSpaceID == id && x.estatus == true).ToList();
            return members;
        }

        public int getTipoPermiso(int muralID)
        {
            int tipo = 0;
            var c = _context.tblMural_Workspace.FirstOrDefault(x => x.id == muralID && x.usuarioID == vSesiones.sesionUsuarioDTO.id);
            var d = _context.tblMural_Workspace_Members.FirstOrDefault(x=>x.workSpaceID == muralID && x.usuarioID == vSesiones.sesionUsuarioDTO.id);
            if (c != null) {
                tipo = 0;
            }
            else if (d != null)
            {
                tipo = d.tipo;
            }
            else {
                tipo = -1;
            }
            return tipo;
        }
        #endregion
    }
}
