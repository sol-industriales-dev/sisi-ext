using Core.DAO.Administracion.Seguridad.AgrupacionCC;
using Core.DTO.Administracion.Seguridad.AgrupacionCC;
using Core.DTO.Utils.Data;
using Core.DTO;
using Core.Entity.Administrativo.Seguridad.Indicadores;
using Core.Enum.Multiempresa;
using Core.DTO.Principal.Generales;
using Data.EntityFramework.Context;
using Data.EntityFramework.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infrastructure.Utils;
using Core.Enum.Principal.Bitacoras;
using System.Data.Odbc;
using Core.Enum.Principal;

namespace Data.DAO.Administracion.Seguridad.AgrupacionCC
{
    public class AgrupacionCCDAO : GenericDAO<tblS_IncidentesAgrupacionCC>, IAgrupacionCCDAO
    {
        string Controller = "AgrupacionCCController";
        private Dictionary<string, object> resultado;


        public List<ComboDTO> getCC()
        {
            try
            {
                       
              
                int Construplan = Convert.ToInt32(EmpresaEnum.Construplan);
                int Arrendadora = Convert.ToInt32(EmpresaEnum.Arrendadora);
               

      
                var lstAgrupacionDetConstruplan = _context.tblS_IncidentesAgrupacionCCDet.Where(x => x.esActivo == true && x.idEmpresa == Construplan).Select(r => new AgrupacionCCDTO
                {
                    cc = r.cc
                }).ToList();
               
                List<AgrupacionCCDTO> lstAgrupacionDetArrendadora = _context.tblS_IncidentesAgrupacionCCDet.Where(x => x.esActivo == true && x.idEmpresa == Arrendadora).Select(r => new AgrupacionCCDTO
                {
                    cc = r.cc
                }).ToList();
                
                string parametroConstruplan = "";
                foreach (var item in lstAgrupacionDetConstruplan)
                {
                    parametroConstruplan += "'" + item.cc.Trim() + "'" + ",";
                }
                parametroConstruplan = parametroConstruplan.TrimEnd(',');
                string queryConstruplan = "";
                if (parametroConstruplan != "")
                {
                    queryConstruplan = @"SELECT cc as departamento , descripcion FROM DBA.cc WHERE cc NOT IN ( " + parametroConstruplan + " )";
                }
                else
                {
                    queryConstruplan = @"SELECT cc as departamento , descripcion FROM DBA.cc";
                }


                string parametroArrendadora = "";
                foreach (var item in lstAgrupacionDetArrendadora)
                {
                    parametroArrendadora += "'" + item.cc.Trim() + "'" + ",";
                }
                parametroArrendadora = parametroArrendadora.TrimEnd(',');
                string queryArrendadora = "";
                if (parametroArrendadora != "")
                {
                    queryArrendadora = @"SELECT cc as departamento , descripcion FROM DBA.cc WHERE cc NOT IN ( " + parametroArrendadora + " )";
                }
                else
                {
                    queryArrendadora = @"SELECT cc as departamento , descripcion FROM DBA.cc";
                }



                var odbcConstruplan = new OdbcConsultaDTO();
                //odbcConstruplan.consulta = String.Format(queryConstruplan);
                var odbcArrendadora = new OdbcConsultaDTO();
                odbcArrendadora.consulta = String.Format(queryArrendadora);

                List<ComboDTO> lstConstruplan = new List<ComboDTO>();
                //var lstConstr = _contextEnkontrol.Select<AgrupacionCCDTO>(EnkontrolEnum.CplanRh, odbcConstruplan).Select(x => new ComboDTO
                var arr = parametroConstruplan.Trim('"');
                //var lstConstr = _context.tblP_CC.Where(x => !arr.Contains(x.cc) && x.estatus).ToList().Select(x => new ComboDTO

                var lstConstr = _context.tblP_CC.Where(x => x.estatus).ToList().Select(x => new ComboDTO
                {
                    Value = Convert.ToString(1) + "-" + x.cc,
                    Text = x.cc + " - " + x.descripcion,
                    Prefijo = Convert.ToString(1)
                }).ToList();

                var lstArrend = _contextEnkontrol.Select<AgrupacionCCDTO>(EnkontrolEnum.ArrenProd, odbcArrendadora).Select(x => new ComboDTO
                {
                    Value = Convert.ToString(2) + "-" + x.departamento,
                    Text = x.departamento + " - " + x.descripcion,
                    Prefijo = Convert.ToString(2)
                }).ToList();
                List<ComboDTO> data = new List<ComboDTO>();

                foreach (var item in lstConstr)
                {
                    data.Add(item);
                }
                foreach (var item in lstArrend)
                {
                    data.Add(item);
                }

                if (vSesiones.sesionEmpresaActual == 3)
                {

                    var lstConstrCol = _context.tblP_CC.Where(x => x.estatus).ToList().Select(x => new ComboDTO
                    {
                        Value = Convert.ToString(1) + "-" + x.cc,
                        Text = x.cc + " - " + x.descripcion,
                        Prefijo = "COLOMBIA"
                    }).ToList();
                    data = lstConstrCol;

                }else if(vSesiones.sesionEmpresaActual == 6)
                {
                    var lstConstrPeru = _context.tblP_CC.Where(x => x.estatus).ToList().Select(x => new ComboDTO
                    {
                        Value = Convert.ToString(1) + "-" + x.cc,
                        Text = x.cc + " - " + x.descripcion,
                        Prefijo = "PERÚ"
                    }).ToList();
                    data = lstConstrPeru;
                }

                return data.OrderBy(x => x.Text).ToList();

            }
            catch (Exception e)
            {
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, "Ocurrió un error interno al intentar obtener el catálogo tipos de emleados");
                return null;
            }
        }

        public List<ComboDTO> getCCTodos(int idAgrupacionCC)
        {
            try
            {
                int Construplan = Convert.ToInt32(EmpresaEnum.Construplan);
                int Arrendadora = Convert.ToInt32(EmpresaEnum.Arrendadora);
                List<AgrupacionCCDTO> lstAgrupacionDetConstruplan = _context.tblS_IncidentesAgrupacionCCDet.Where(x => x.esActivo == true && x.idEmpresa == Construplan && x.idAgrupacionCC != idAgrupacionCC).Select(r => new AgrupacionCCDTO
                {
                    cc = r.cc
                }).ToList();
                List<AgrupacionCCDTO> lstAgrupacionDetArrendadora = _context.tblS_IncidentesAgrupacionCCDet.Where(x => x.esActivo == true && x.idEmpresa == Arrendadora && x.idAgrupacionCC != idAgrupacionCC).Select(r => new AgrupacionCCDTO
                {
                    cc = r.cc
                }).ToList();
                string parametroConstruplan = "";
                foreach (var item in lstAgrupacionDetConstruplan)
                {
                    parametroConstruplan += "'" + item.cc.Trim() + "'" + ",";
                }
                parametroConstruplan = parametroConstruplan.TrimEnd(',');
                string queryConstruplan = "";
                if (parametroConstruplan != "")
                {
                    queryConstruplan = @"SELECT cc as departamento , descripcion FROM DBA.cc WHERE cc NOT IN ( " + parametroConstruplan + " )";
                }
                else
                {
                    queryConstruplan = @"SELECT cc as departamento , descripcion FROM DBA.cc";
                }


                string parametroArrendadora = "";
                foreach (var item in lstAgrupacionDetArrendadora)
                {
                    parametroArrendadora += "'" + item.cc.Trim() + "'" + ",";
                }
                parametroArrendadora = parametroArrendadora.TrimEnd(',');
                string queryArrendadora = "";
                if (parametroArrendadora != "")
                {
                    queryArrendadora = @"SELECT cc as departamento , descripcion FROM DBA.cc WHERE cc NOT IN ( " + parametroArrendadora + " )";
                }
                else
                {
                    queryArrendadora = @"SELECT cc as departamento , descripcion FROM DBA.cc";
                }

                var odbcConstruplan = new OdbcConsultaDTO();
                odbcConstruplan.consulta = String.Format(queryConstruplan);
                var odbcArrendadora = new OdbcConsultaDTO();
                odbcArrendadora.consulta = String.Format(queryArrendadora);


                //var lstConstr = _contextEnkontrol.Select<AgrupacionCCDTO>(EnkontrolEnum.CplanRh, odbcConstruplan).Select(x => new ComboDTO
                var arr = parametroConstruplan.Trim('"');
                var lstConstr = _context.tblP_CC.Where(x => !arr.Contains(x.cc) && x.estatus).ToList().Select(x => new ComboDTO
                //var lstConstr = _context.tblP_CC.ToList().Select(x => new ComboDTO
                {
                    Value = Convert.ToString(1) + "-" + x.cc,
                    Text = x.cc + " - " + x.descripcion,
                    Prefijo = Convert.ToString(1)
                }).ToList();
                var lstArrend = _contextEnkontrol.Select<AgrupacionCCDTO>(EnkontrolEnum.ArrenRh, odbcArrendadora).Select(x => new ComboDTO
                {
                    Value = Convert.ToString(2) + "-" + x.departamento,
                    Text = x.departamento + " - " + x.descripcion,
                    Prefijo = Convert.ToString(2),

                }).ToList();
                List<ComboDTO> data = new List<ComboDTO>();

                foreach (var item in lstConstr)
                {
                    data.Add(item);
                }
                foreach (var item in lstArrend)
                {
                    data.Add(item);
                }

                return data;

            }
            catch (Exception e)
            {
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, "Ocurrió un error interno al intentar obtener el catálogo tipos de emleados");
                return null;
            }
        }

        public List<ComboDTO> ObtnerAgrupacion()
        {
            List<ComboDTO> data = _context.tblS_IncidentesAgrupacionCC.Where(x => x.esActivo).Select(x => new ComboDTO
            {
                Value = x.id.ToString(),
                Text = x.nomAgrupacion

            }).ToList();
            return data;
        }

        public List<AgrupacionCCDTO> GetDetalleAgrupacion(int idAgrupacionCC)
        {
            try
            {
                List<AgrupacionCCDTO> lstAgrupacionCC;
                List<AgrupacionCCDTO> lst = new List<AgrupacionCCDTO>();
                if (idAgrupacionCC == 0)
                {
                    lstAgrupacionCC = _context.tblS_IncidentesAgrupacionCC.Where(x => x.esActivo == true).Select(x => new AgrupacionCCDTO
                    {
                        id = x.id,
                        nomAgrupacion = x.nomAgrupacion
                    }).ToList();
                }
                else
                {
                    lstAgrupacionCC = _context.tblS_IncidentesAgrupacionCC.Where(x => x.id == idAgrupacionCC && x.esActivo == true).Select(x => new AgrupacionCCDTO
                    {
                        id = x.id,
                        nomAgrupacion = x.nomAgrupacion
                    }).ToList();
                }


                foreach (AgrupacionCCDTO item in lstAgrupacionCC)
                {
                    item.lstDatos = _context.tblS_IncidentesAgrupacionCCDet.Where(x => x.idAgrupacionCC == item.id && x.esActivo == true).ToList().Select(xd => new AgrupacionCCDet
                    {
                        id = xd.id,
                        idDet = xd.idAgrupacionCC,
                        cc = xd.cc.Trim() + " - " + Nombre(xd.idEmpresa, xd.cc.Trim()),
                        value = xd.idEmpresa + "-" + xd.cc

                    }).ToList();
                }

                return lstAgrupacionCC;
            }
            catch (Exception e)
            {
                LogError(2, 0, Controller, "tblS_IncidentesAgrupacionesCCDet", e, AccionEnum.CONSULTA, 0, 0);
                return null;
            }
        }

        public string Nombre(int idEmpresa, string cc)
        {
            string nombre = "";
            var odbc = new OdbcConsultaDTO();
            string queryConstruplan = @"SELECT cc as departamento , descripcion FROM DBA.cc";
            string queryArrendadora = @"SELECT cc as departamento , descripcion FROM DBA.cc";

            if (idEmpresa == 1)
            {
                odbc.consulta = String.Format(queryConstruplan);
                //List<AgrupacionCCDTO> dataConstruplan = _contextEnkontrol.Select<AgrupacionCCDTO>(EnkontrolEnum.CplanRh, odbc).Select(x => new AgrupacionCCDTO
                List<AgrupacionCCDTO> dataConstruplan = _context.tblP_CC.Where(x => x.cc == cc).ToList().Select(x => new AgrupacionCCDTO
                {
                    //cc = x.departamento,
                    cc = x.cc,
                    descripcion = x.descripcion
                }).ToList();
                nombre = dataConstruplan.Where(y => y.cc == cc).Select(yx => yx.descripcion).FirstOrDefault();
            }
            else
            {

                odbc.consulta = String.Format(queryArrendadora);
                List<AgrupacionCCDTO> dataArrendadora = _contextEnkontrol.Select<AgrupacionCCDTO>(EnkontrolEnum.ArrenRh, odbc).Select(x => new AgrupacionCCDTO
                {
                    cc = x.departamento,
                    descripcion = x.descripcion
                }).ToList();
                nombre = dataArrendadora.Where(y => y.cc == cc).Select(yx => yx.descripcion).FirstOrDefault();
            }


            return nombre;
        }

        public AgrupacionCCDTO CrearAgrupacion(AgrupacionCCDTO _objAgrupaciones, List<tblS_IncidentesAgrupacionCCDet> lstAgrupaciones)
        {
            AgrupacionCCDTO objDTO = new AgrupacionCCDTO();

            using (var dbContextTransaction = _context.Database.BeginTransaction())
            {
                try
                {
                    tblS_IncidentesAgrupacionCC objBusqueda = _context.tblS_IncidentesAgrupacionCC.Where(x => x.nomAgrupacion == _objAgrupaciones.nomAgrupacion && x.esActivo).FirstOrDefault();
                    if (objBusqueda == null)
                    {
                        tblS_IncidentesAgrupacionCC objAdd = new tblS_IncidentesAgrupacionCC();

                        objAdd.fechaCreacion = DateTime.Now;
                        objAdd.fechaModificacion = null;

                        objAdd.esActivo = true;
                        objAdd.nomAgrupacion = _objAgrupaciones.nomAgrupacion;
                        //_context.tblS_IncidentesAgrupacionCC.Attach(objAdd);
                        _context.tblS_IncidentesAgrupacionCC.Add(objAdd);

                        _context.SaveChanges();

                        int a = (from i in _context.tblS_IncidentesAgrupacionCC orderby i.id descending select i.id).FirstOrDefault();
                        tblS_IncidentesAgrupacionCCDet objAgrupacionDet;
                        if(vSesiones.sesionEmpresaActual==3 || vSesiones.sesionEmpresaActual==6)
                        {
                            foreach (var item in lstAgrupaciones)
                            {
                                objAgrupacionDet = new tblS_IncidentesAgrupacionCCDet();
                                objAgrupacionDet.idAgrupacionCC = a;
                                objAgrupacionDet.cc = item.cc;
                                objAgrupacionDet.idEmpresa =(int)vSesiones.sesionEmpresaActual;
                                objAgrupacionDet.esActivo = true;
                                _context.tblS_IncidentesAgrupacionCCDet.Add(objAgrupacionDet);
                                _context.SaveChanges();
                            }
                        }
                        else
                        {
                            foreach (var item in lstAgrupaciones)
                            {
                                objAgrupacionDet = new tblS_IncidentesAgrupacionCCDet();
                                objAgrupacionDet.idAgrupacionCC = a;
                                objAgrupacionDet.cc = item.cc;
                                objAgrupacionDet.idEmpresa = item.idEmpresa;
                                objAgrupacionDet.esActivo = true;
                                _context.tblS_IncidentesAgrupacionCCDet.Add(objAgrupacionDet);
                                _context.SaveChanges();
                            }
                        }
                      
                        dbContextTransaction.Commit();
                        objDTO.Exitoso = true;
                        objDTO.Mensaje = "Guardado Con Exito!";
                        return objDTO;
                    }
                    else
                    {
                        objDTO.Exitoso = false;
                        objDTO.Mensaje = "Ya existe una agrupación con este nombre.";
                        return objDTO;
                    }

                }
                catch (Exception e)
                {
                    dbContextTransaction.Rollback();
                    LogError(2, 0, "BackLogsController", "CrearBackLog", e, AccionEnum.AGREGAR, 0, JsonUtils.convertNetObjectToJson(_objAgrupaciones));
                    return objDTO;
                }
            }
        }

        public bool EditarAgrupacion(int id, string NuevoNombre, string[] lstAgrupacion)
        {
            using (var dbContextTransaction = _context.Database.BeginTransaction())
            {
                try
                {
                    #region SE VERIFICA QUE EL NOMBRE DE LA AGRUPACIÓN ESTE DISPONIBLE
                    int disponible = _context.tblS_IncidentesAgrupacionCC.Where(x => x.id != id && x.nomAgrupacion == NuevoNombre && x.esActivo).Count();
                    if (disponible > 0)
                        throw new Exception("Ya existe una agrupación con este nombre.");
                    #endregion

                    tblS_IncidentesAgrupacionCC AgrupacionCC = _context.tblS_IncidentesAgrupacionCC.FirstOrDefault(x => x.id == id);
                    AgrupacionCC.nomAgrupacion = NuevoNombre;
                    _context.SaveChanges();

                    List<tblS_IncidentesAgrupacionCCDet> lstAgrupacionDet = _context.tblS_IncidentesAgrupacionCCDet.Where(x => x.idAgrupacionCC == id).ToList();
                    foreach (var item in lstAgrupacionDet)
                    {
                        tblS_IncidentesAgrupacionCCDet objAgrupacion = _context.tblS_IncidentesAgrupacionCCDet.FirstOrDefault(x => x.id == item.id);
                        _context.tblS_IncidentesAgrupacionCCDet.Remove(objAgrupacion);
                        _context.SaveChanges();
                    }
                    foreach (var item in lstAgrupacion)
                    {

                        tblS_IncidentesAgrupacionCCDet objAdd = new tblS_IncidentesAgrupacionCCDet();
                        objAdd.esActivo = true;
                        objAdd.cc = item.Split('-')[1];
                        objAdd.idEmpresa = Convert.ToInt32(item.Split('-')[0]);
                        objAdd.idAgrupacionCC = id;
                        _context.tblS_IncidentesAgrupacionCCDet.Add(objAdd);
                        _context.SaveChanges();

                    }

                    dbContextTransaction.Commit();
                    return true;
                }
                catch (Exception e)
                {
                    dbContextTransaction.Rollback();
                    LogError(2, 0, Controller, "ActivarDesactivarDepartamento", e, AccionEnum.ACTUALIZAR, id, 0);
                    return false;
                }
            }
        }

        public bool EliminarAgrupacion(int id, int esActivo)
        {
            using (var dbContextTransaction = _context.Database.BeginTransaction())
            {
                try
                {
                    tblS_IncidentesAgrupacionCC objActualizar = _context.tblS_IncidentesAgrupacionCC.FirstOrDefault(x => x.id == id);
                    List<tblS_IncidentesAgrupacionCCDet> lst = _context.tblS_IncidentesAgrupacionCCDet.Where(x => x.idAgrupacionCC == id).ToList();
                    if (esActivo == 1)
                    {
                        objActualizar.esActivo = true;
                        _context.SaveChanges();
                        foreach (var item in lst)
                        {
                            tblS_IncidentesAgrupacionCCDet obj = _context.tblS_IncidentesAgrupacionCCDet.FirstOrDefault(x => x.idAgrupacionCC == item.idAgrupacionCC);
                            obj.esActivo = true;
                            _context.SaveChanges();
                        }
                    }
                    else
                    {
                        objActualizar.esActivo = false;
                        _context.SaveChanges();
                        foreach (var item in lst)
                        {
                            tblS_IncidentesAgrupacionCCDet obj = _context.tblS_IncidentesAgrupacionCCDet.FirstOrDefault(x => x.id == item.id);
                            obj.esActivo = false;
                            _context.SaveChanges();
                        }
                    }

                    dbContextTransaction.Commit();
                    return true;
                }
                catch (Exception e)
                {
                    dbContextTransaction.Rollback();
                    LogError(2, 0, Controller, "ActivarDesactivarDepartamento", e, AccionEnum.ACTUALIZAR, id, 0);
                    return false;

                }

            }

        }

        public List<ComboDTO> obtenerAgrupacionCombo()
        {
            try
            {
                var lstAgrupacion = _context.tblS_IncidentesAgrupacionCC.Where(x => x.esActivo == true).Select(x => new ComboDTO
                {
                    Value = x.id.ToString(),
                    Text = x.nomAgrupacion,
                    Prefijo = x.id.ToString()
                }).ToList();

                return lstAgrupacion;

            }
            catch (Exception e)
            {
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, "Ocurrió un error interno al intentar obtener el catálogo tipos de emleados");
                return null;
            }
        }
    }
}