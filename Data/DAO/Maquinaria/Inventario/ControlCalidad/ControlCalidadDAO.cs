using Core.DAO.Maquinaria.Inventario.ControlCalidad;
using Core.DTO;
using Core.Entity.Maquinaria.Inventario;
using Core.Entity.Maquinaria.Inventario.ControlCalidad;
using Core.Enum.Principal.Bitacoras;
using Data.EntityFramework.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.DAO.Maquinaria.Inventario.ControlCalidad
{
    public class ControlCalidadDAO : GenericDAO<tblM_CatControlCalidad>, IControlCalidadDAO
    {

        #region Referencias.
        private readonly Dictionary<string, object> resultado = new Dictionary<string, object>();
        private const string nombreControlador = "ConstrolCalidadController";
        #endregion
        public List<tblM_CatControlCalidad> getListControlCalidad()
        {
            return _context.tblM_CatControlCalidad.OrderBy(x => x.id).ToList();
        }
        public tblM_CatControlCalidad getControlCalidadById(int idAsignacion, int TipoControl, int TipoFiltro)
        {
            tblM_CatControlCalidad res = new tblM_CatControlCalidad();
            if (TipoFiltro != 1)
            {
                res = _context.tblM_CatControlCalidad.Where(x => x.IdAsignacion == idAsignacion && x.TipoControl == TipoControl).OrderByDescending(x => x.id).FirstOrDefault();//cambio para que tome el ultimo

            }
            if (res == null)
            {
                res = new tblM_CatControlCalidad();
            }
            return res;
        }
        public tblM_CatControlCalidad getControlCalidadById(int idAsignacion, int TipoControl)
        {
            //var res = _context.tblM_CatControlCalidad.FirstOrDefault(x => x.IdAsignacion == idAsignacion && x.TipoControl == TipoControl);
            var res = _context.tblM_CatControlCalidad.Where(x => x.IdAsignacion == idAsignacion && x.TipoControl == TipoControl).OrderByDescending(x => x.id).FirstOrDefault();//cambio para que tome el ultimo
            if (res == null)
            {
                res = new tblM_CatControlCalidad();
            }
            return res;
        }

        public tblM_CatControlCalidad saveControlCalidad(tblM_CatControlCalidad obj)
        {
            try
            {
                
                if (obj.id == 0)
                {
                    SaveEntity(obj, (int)BitacoraEnum.CONTROLCALIDAD);
                }
                else
                {
                    Update(obj, obj.id, (int)BitacoraEnum.CONTROLCALIDAD);
                }

                if (obj.TipoControl == 2 || obj.TipoControl == 4)
                {
                    var maq = _context.tblM_CatMaquina.FirstOrDefault(x => x.id == obj.IdEconomico);
                    maq.estatus = 1;
                    _context.SaveChanges();
                }
            }
            catch (Exception e)
            {
                return new tblM_CatControlCalidad();
            }

            return obj;
        }

        public tblM_CatControlCalidad getByIDAsignacion(int id)
        {

            return _context.tblM_CatControlCalidad.FirstOrDefault(x => x.IdAsignacion == id);

        }

        public tblM_CatControlCalidad getByIDAsignacionTipo(int id, int tipoControl)
        {
            var data = _context.tblM_CatControlCalidad.Where(x => x.IdAsignacion == id && x.TipoControl == tipoControl).OrderByDescending(x=>x.id);
            return data.FirstOrDefault();
        }


        public Dictionary<string, object> guardarControlMovimientoMaquinaria(tblM_CatControlCalidad objCalidad,
                                                  List<tblM_RelPreguntaControlCalidad> lstRespuestas,
                                                  tblM_ControlMovimientoMaquinaria objControl,
                                                  string areaCuentaDestino, string areaCuentaOrigen,
                                                  int tipoControl, int envioEspecial)
        {
            using (var dbTransaction = _context.Database.BeginTransaction())
            {
                try
                {
                    int usuarioCreadorID = vSesiones.sesionUsuarioDTO.id;

                    if (objCalidad == null)
                    {
                        resultado.Add(SUCCESS, false);
                        resultado.Add(MESSAGE, "El control de Calidad de encuentra vacío, favor de verificar la información para continuar.");
                        return resultado;
                    }
                    //Guardar el control de calidad.
                    _context.tblM_CatControlCalidad.Add(objCalidad);
                    _context.SaveChanges();
                    //Guarda las respuestas del control de calidad.
                    lstRespuestas.ForEach(x => x.IdControl = objCalidad.id);
                    _context.tblM_RelPreguntaControlCalidad.AddRange(lstRespuestas);
                    _context.SaveChanges();
                    //Guardar el control de movimiento.
                    _context.tblM_ControlMovimientoMaquinaria.Add(objControl);

                    var asignacionObj = _context.tblM_AsignacionEquipos.First(x => x.id == objCalidad.IdAsignacion);
                    var bandera = cambiarAsignacion(asignacionObj, objCalidad.TipoControl, areaCuentaDestino, areaCuentaOrigen, envioEspecial);

                    if (bandera)
                    {

                        dbTransaction.Commit();

                    }
                    else
                    {
                        resultado.Add(SUCCESS, false);
                        resultado.Add(MESSAGE, "hubo un erro al momento de guardar la información, Favor de consultar a sistemas.");
                        return resultado;
                    }
                    resultado.Add(SUCCESS, true);
                    resultado.Add("ControlID", objControl.id);

                }
                catch (Exception e)
                {
                    dbTransaction.Rollback();
                    LogError(0, 0, nombreControlador, "GuardarMovimientosMaquinaria", e, AccionEnum.ACTUALIZAR, 0, null);
                    resultado.Add(SUCCESS, false);
                    resultado.Add(MESSAGE, "Ocurrió un error al guardar el movimiento de maquinaria.");
                }
                return resultado;
            }

        }

        public bool cambiarAsignacion(tblM_AsignacionEquipos asignacion, int tipoControl, string acDestino, string acOrigen, int envioEspecial)
        {
            try
            {
                var maquinaria = _context.tblM_CatMaquina.First(m => m.id == asignacion.noEconomicoID);
                asignacion.estatus += 1;
                switch (tipoControl)
                {
                    case 2:
                        {
                            maquinaria.centro_costos = asignacion.cc;
                            var pendientesMovimiento = _context.tblM_AsignacionEquipos
                                             .Where(a => a.noEconomicoID == maquinaria.id && a.id != asignacion.id && a.estatus != 10);

                            foreach (tblM_AsignacionEquipos asignacionItem in pendientesMovimiento)
                            {
                                asignacionItem.estatus = 10;
                            }
                        }
                        break;
                    case 3:
                        {
                            if (envioEspecial == 3 || envioEspecial == 4)
                            {
                                maquinaria.centro_costos = "997";
                                maquinaria.TipoBajaID = 6;
                                maquinaria.estatus = 9;
                                asignacion.estatus = 10;
                            }

                        }
                        break;
                    case 4:
                        {
                            asignacion.estatus = 3;
                            if (envioEspecial == 1) //Envio Especial 1 = Talle Mecanico Central
                            {
                                maquinaria.centro_costos = "1010";

                            }
                            else if (envioEspecial == 2) //Envio Especial 2 = Taller PatioMAquinaria
                            {
                                maquinaria.centro_costos = "1015";

                            }
                            else if (envioEspecial == 3)
                            {
                                asignacion.estatus = 2;
                            }
                        }
                        break;
                    default:
                        break;
                }
                _context.SaveChanges();
                return true;
            }
            catch (Exception)
            {

                return false;
            }
        }
    }
}
