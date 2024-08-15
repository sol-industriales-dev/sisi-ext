using Core.DAO.Cursos;
using Core.DTO;
using Core.Entity.Cursos;
using Core.Entity.Principal.Alertas;
using Core.Enum.Principal.Alertas;
using Data.DAO.Principal.Usuarios;
using Data.EntityFramework.Generic;
using Data.Factory.Principal.Alertas;
using Infrastructure.Utils;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.DAO.Principal.Usuarios;
using Core.Enum.Principal.Bitacoras;
using Core.DTO.Principal.Generales;
using Core.Entity.Principal.Usuarios;



namespace Data.DAO.Cursos
{
    public class CursosDAO: GenericDAO<tblCU_Curso>, ICursoDAO
    {
        public tblCU_Curso GuardarCurso(tblCU_Curso objCurso)
        {
            try
            {
                if (objCurso.id == 0)
                {
                    objCurso.estado = true;
                    SaveEntity(objCurso, (int)BitacoraEnum.Curso);
                }
                else
                {
                    var temp = _context.tblCu_Curso.FirstOrDefault(x => x.id == objCurso.id&& x.estado== true);
                    //temp.estado = objCurso.estado;
                    temp.id = objCurso.id;
                    temp.descripcion = objCurso.descripcion;
                    temp.fecha = objCurso.fecha;
                    temp.fechaCaptura = objCurso.fechaCaptura;
                    temp.folio = objCurso.folio;
                    temp.nombreCurso = objCurso.nombreCurso;
                    temp.usuarioCap = objCurso.usuarioCap;
                    temp.nomUsuarioCap = objCurso.nomUsuarioCap;
                    Update(temp, temp.id, (int)BitacoraEnum.Curso);
                }

            }
            catch (Exception e)
            {
                return new tblCU_Curso();
            }
            return objCurso;
        }
        public tblCU_Modulo GuardarModulo(tblCU_Modulo objModulo)
        {
            IObjectSet<tblCU_Modulo> _objectSet = ((IObjectContextAdapter)_context).ObjectContext.CreateObjectSet<tblCU_Modulo>();
            try
            {
                if (objModulo.id == 0)
                {
                    objModulo.estado = true;
                    _objectSet.AddObject(objModulo);
                    _context.SaveChanges();
                }
                else
                {
                    var temp = _context.tblCu_Modulo.FirstOrDefault(x => x.id == objModulo.id);
                    temp.descripcion = objModulo.descripcion;
                    temp.nombreModulo = objModulo.nombreModulo;
                    //temp.estado = false;
                    _context.SaveChanges();
                }

            }
            catch (Exception e)
            {
                return new tblCU_Modulo();
            }
            return objModulo;
        }
        public tblCU_ModuloDet GuardarModuloDet(tblCU_ModuloDet objModuloDet)
        {
            IObjectSet<tblCU_ModuloDet> _objectSet = ((IObjectContextAdapter)_context).ObjectContext.CreateObjectSet<tblCU_ModuloDet>();
            try
            {
                if (objModuloDet.id == 0)
                {
                    objModuloDet.estado = true;
                    _objectSet.AddObject(objModuloDet);
                    _context.SaveChanges();
                }
                else if (objModuloDet.idModulo > 0 && objModuloDet.estado == false)//guardar pagina nueva en modulo ya existente
                {
                    objModuloDet.estado = true;
                    _objectSet.AddObject(objModuloDet);
                    _context.SaveChanges();
                }
                else if (objModuloDet.idModulo > 0 && objModuloDet.estado == true)//editar pagina de modulo existente
                {
                    var temp = _context.tblCu_ModuloDet.FirstOrDefault(x => x.id == objModuloDet.id);
                    temp.estado = true;
                    temp.descripcion = objModuloDet.descripcion;
                    temp.contenido = objModuloDet.contenido;
                    temp.estado = objModuloDet.estado;
                    temp.pagina = objModuloDet.pagina;
                    temp.idModulo = objModuloDet.idModulo;
                    _context.SaveChanges();

                }
            }
            catch (Exception e)
            {
                return new tblCU_ModuloDet();
            }
            return objModuloDet;
        }
        public List<tblCU_Curso> GetListCursos(int id, string folio, string nombre, int combo)
        {
            validaCompletoCurso();    
            var result = new List<tblCU_Curso>();
            try
            {
                result = _context.tblCu_Curso.Where(x => (x.estado) &&
                ((combo == 2 ? x.completo == false : (combo == 3) ? x.completo == true : true))
                && (id == 0 ? true : x.id == id) && (folio == "" ? true : x.folio == folio)).ToList();
            }
            catch (Exception)
            {
                throw;
            }
            return result;
        }
        //7/2/18 11:52
        private void validaCompletoCurso()
        {
            var cursoslst = _context.tblCu_Curso.Where(x => (x.estado)).ToList();
            foreach (var curso in cursoslst)
            {
                bool cursoCompleto = true;//curso
                bool moduloCompleto = false;//curso
                var resultModulo = new List<tblCU_Modulo>();
                var resultModuloDet = new List<tblCU_ModuloDet>();
                int longresultModulo;
                int longresultModuloDet;
                int detallesCcontenido = 0;
                resultModulo = _context.tblCu_Modulo.Where(x => x.idCurso == curso.id&& x.estado== true).ToList();
                longresultModulo = resultModulo.Count();//numero de modulos del curso
                foreach (var modulo in resultModulo)
                {
                    resultModuloDet = _context.tblCu_ModuloDet.Where(x => x.idModulo == modulo.id&& x.estado== true).ToList();
                    longresultModuloDet = resultModuloDet.Count();//numero de detalles del modulo
                    if (resultModuloDet.Count > 0)//si no cuenta con detalle los modulos
                    {
                        foreach (var moduloDet in resultModuloDet)
                        {
                            if (moduloDet.idModulo == modulo.id)
                            {
                                if (moduloDet.contenido != null && moduloDet.contenido!="")//cuenta los detalles con contenido
                                {
                                    detallesCcontenido +=1;
                                }
                            }
                        }
                        if (longresultModuloDet == detallesCcontenido)//si toda las paginas tienen contenido es emodulo esta completo
                        {
                            //cursoCompleto = true;
                            moduloCompleto = true;
                        }
                        else
                        {
                            cursoCompleto = false;
                            moduloCompleto = false;
                        }
                    }
                    else
                    {
                        moduloCompleto = false;
                        cursoCompleto = false;
                    }
                    var tempmod = _context.tblCu_Modulo.FirstOrDefault(x => x.id == modulo.id);
                    tempmod.completo = moduloCompleto;
                    Update(tempmod, tempmod.id, (int)BitacoraEnum.Modulo);
                    detallesCcontenido = 0;
                }
                //cursoCompleto = moduloCompleto;
                var temp = _context.tblCu_Curso.FirstOrDefault(x => x.id == curso.id);
                temp.completo =cursoCompleto;
                Update(temp, temp.id, (int)BitacoraEnum.Curso);
              
            }
        }
        public List<tblCU_Modulo> getModuloid(int id)
        {
            var result = new List<tblCU_Modulo>();
            if (id > 0)
            {
                result = _context.tblCu_Modulo.Where(x => x.idCurso == id && x.estado!=false).ToList();
            }
            return result;
        }
        public List<tblCU_ModuloDet> getModuloDetid(int id)
        {
            var result = new List<tblCU_ModuloDet>();
            if (id > 0)
            {
                result = _context.tblCu_ModuloDet.Where(x => x.idModulo == id && x.estado != false).ToList();
            }
            return result;
        }
        public List<ComboDTO> FillComboCurso()
        {
            var result = new List<tblCU_Curso>();
            result = _context.tblCu_Curso.Where(x => x.id != 0).ToList();
            List<ComboDTO> lstobjComboDTO = new List<ComboDTO>();
            foreach (var item in result)
            {
                ComboDTO objComboDTO = new ComboDTO();
                objComboDTO.Text = item.nombreCurso;
                objComboDTO.Value = item.id.ToString();
                lstobjComboDTO.Add(objComboDTO);
            }
            return lstobjComboDTO;
        }
        public tblCU_Examen GuardarExamen(tblCU_Examen objExamen)
        {
            IObjectSet<tblCU_Examen> _objectSet = ((IObjectContextAdapter)_context).ObjectContext.CreateObjectSet<tblCU_Examen>();
            try
            {
                
                if (objExamen.id == 0)
                {
                    _objectSet.AddObject(objExamen);
                    _context.SaveChanges();
                   
                }
                else
                {
                    var temp = _context.tblCU_Examen.FirstOrDefault(x => x.id == objExamen.id);
                    temp.id = objExamen.id;
                    temp.idCurso = objExamen.idCurso;
                    temp.nombreExamen = objExamen.nombreExamen;
                    temp.fecha = objExamen.fecha;
                    temp.fechaCaptura = objExamen.fechaCaptura;
                    temp.usuarioCap = objExamen.usuarioCap;
                    temp.nomUsuarioCap = objExamen.nomUsuarioCap;
                    temp.descripcion = objExamen.descripcion;
                    temp.folio = objExamen.folio;
                    temp.editable = objExamen.editable;
                    SaveBitacora((int)BitacoraEnum.Examen, (int)AccionEnum.ACTUALIZAR, objExamen.id, JsonUtils.convertNetObjectToJson(temp));
                }
            }
            catch (Exception e)
            {
                return new tblCU_Examen();
            }
            return objExamen;
        }
        //GuardarPregunta
        //    GuardarRespuesta
        public tblCU_ExamenPregunta GuardarPregunta(tblCU_ExamenPregunta objPregunta)
        {
            IObjectSet<tblCU_ExamenPregunta> _objectSet = ((IObjectContextAdapter)_context).ObjectContext.CreateObjectSet<tblCU_ExamenPregunta>();
            try
            {
                if (objPregunta.id == 0)
                {
                    _objectSet.AddObject(objPregunta);
                    _context.SaveChanges();
                    //SaveEntity(objModulo, (int)BitacoraEnum.Modulo); forma normal en la misma interfaz
                    //SaveBitacora((int)BitacoraEnum.Modulo, (int)AccionEnum.AGREGAR, objModulo.id, JsonUtils.convertNetObjectToJson(objModulo));
                }
                else
                {
                    //var temp = _context.tblRH_AditivaDeductiva.FirstOrDefault(x => x.id == objAditivaDet.id);
                    //temp.id = objAditivaDet.id;
                    //temp.cCid = objAditivaDet.cCid;
                    //temp.cC = objAditivaDet.cC;
                    //temp.aprobado = objAditivaDet.aprobado;
                    //temp.fechaCaptura = objAditivaDet.fechaCaptura;
                    //temp.fecha_Alta = objAditivaDet.fecha_Alta;
                    //temp.folio = objAditivaDet.folio;
                    //temp.nomUsuarioCap = objAditivaDet.nomUsuarioCap;
                    //temp.rechazado = objAditivaDet.rechazado;
                    //temp.usuarioCap = objAditivaDet.usuarioCap;
                    //Update(temp, temp.id, (int)BitacoraEnum.AditivaPersonal);
                }

            }
            catch (Exception e)
            {
                return new tblCU_ExamenPregunta();
            }
            return objPregunta;
        }
        public tblCU_ExamenRespuesta GuardarRespuesta(tblCU_ExamenRespuesta objRespuesta)
        {
            IObjectSet<tblCU_ExamenRespuesta> _objectSet = ((IObjectContextAdapter)_context).ObjectContext.CreateObjectSet<tblCU_ExamenRespuesta>();
            try
            {
                if (objRespuesta.id == 0)
                {
                    _objectSet.AddObject(objRespuesta);
                    _context.SaveChanges();
                    //SaveEntity(objModulo, (int)BitacoraEnum.Modulo); forma normal en la misma interfaz
                    //SaveBitacora((int)BitacoraEnum.Modulo, (int)AccionEnum.AGREGAR, objModulo.id, JsonUtils.convertNetObjectToJson(objModulo));
                }
                else
                {
                    //var temp = _context.tblRH_AditivaDeductiva.FirstOrDefault(x => x.id == objAditivaDet.id);
                    //temp.id = objAditivaDet.id;
                    //temp.cCid = objAditivaDet.cCid;
                    //temp.cC = objAditivaDet.cC;
                    //temp.aprobado = objAditivaDet.aprobado;
                    //temp.fechaCaptura = objAditivaDet.fechaCaptura;
                    //temp.fecha_Alta = objAditivaDet.fecha_Alta;
                    //temp.folio = objAditivaDet.folio;
                    //temp.nomUsuarioCap = objAditivaDet.nomUsuarioCap;
                    //temp.rechazado = objAditivaDet.rechazado;
                    //temp.usuarioCap = objAditivaDet.usuarioCap;
                    //Update(temp, temp.id, (int)BitacoraEnum.AditivaPersonal);
                }

            }
            catch (Exception e)
            {
                return new tblCU_ExamenRespuesta();
            }
            return objRespuesta;
        }
        //eliminar modulo
        public void EliminaModulo(int id)
        {
            try
            {
                var temp = _context.tblCu_Modulo.FirstOrDefault(x => x.id == id);
                temp.estado = false;
                _context.SaveChanges();

            }
            catch (Exception e)
            {
            
            }
            
        }
        //eliminar modulodetalle por modulo 26/1/18 15:32raguilar 
        public void EliminaModuloDet(int id)
        {
            try
            {
                //var temp = _context.tblCu_ModuloDet.AddRange(x => x.idModulo == id);
                //var temp = _context.tblCu_Modulo.ToList().AddRange(x=> x)
                var temp = _context.tblCu_ModuloDet.ToList().Where(x => x.idModulo == id);

                foreach (var item in temp)
                {
                    item.estado = false;
                    _context.SaveChanges();
                }
            }
            catch (Exception e)
            {
            
            }
            
        }
        //eliminar modulodetalle por idDetalle 6/2/18 12:33raguilar 
        public void EliminaModuloDetbyId(int id)
        {
            try
            {
                //var temp = _context.tblCu_ModuloDet.AddRange(x => x.idModulo == id);
                //var temp = _context.tblCu_Modulo.ToList().AddRange(x=> x)
                var temp = _context.tblCu_ModuloDet.ToList().Where(x => x.id == id);

                foreach (var item in temp)
                {
                    item.estado = false;
                    _context.SaveChanges();
                }
            }
            catch (Exception e)
            {

            }

        }
        //validaacion nueva 1/2/18
        public bool ComparaPagina(int pagina, int modulo) 
        {
            bool  FlagExiste = false;
            try
            {
                var temp = _context.tblCu_ModuloDet.Where(x => x.pagina == pagina && x.idModulo == modulo&& x.estado== true);
                if (temp.Count()>0)
                {
                    FlagExiste = true;
                }
            }
            catch (Exception)
            {
                
                throw;
            }
            return FlagExiste;
        }
        //Elimina Curso completo
        public int  EliminaCurso(int IdCurso) {
            var tempModulos = _context.tblCu_Modulo.Where(x => x.idCurso == IdCurso && x.estado == true).ToList();
            foreach (var modulos in tempModulos)
            {
                modulos.estado = false;
                var tempModulosDet = _context.tblCu_ModuloDet.Where(x => x.idModulo == modulos.id && x.estado == true).ToList();
                foreach (var modulosDet in tempModulosDet)
                {
                    if (modulos.id== modulosDet.idModulo)
                    {
                        modulosDet.estado = false;
                    }
                }
            }
            var tempCurso = _context.tblCu_Curso.Where(x => x.id == IdCurso && x.estado == true).First();
            tempCurso.estado = false;
            _context.SaveChanges();
            return tempCurso.id;      
        }


        public List<object> GetListDeptos()
        {
            var deptos = new List<tblP_Departamento>();
            var result = new List<object>();
            try
            {
                deptos = _context.tblP_Departamento.ToList();
                result = deptos.Select(x => new { x.id, x.abreviacion, x.descripcion }).Cast<object>().ToList();
            }
            catch (Exception)
            {
                throw;
            }
            return result;
        }

        public void AsignarCurso(List<tblCU_Asignacion> lstAsignacion)
        {
            IObjectSet<tblCU_Asignacion> _objectSet = ((IObjectContextAdapter)_context).ObjectContext.CreateObjectSet<tblCU_Asignacion>();
            try
            {
                foreach (var objAsignacion in lstAsignacion)
                {
                    if (objAsignacion.id == 0)
                    {
                        _objectSet.AddObject(objAsignacion);
                        _context.SaveChanges();
                    }
                    else
                    {

                    }
                }
            }
            catch (Exception e)
            {

            }

        }
    }
}
