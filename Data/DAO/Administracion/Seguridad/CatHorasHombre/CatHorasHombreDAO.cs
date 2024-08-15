using Core.DAO.Administracion.Seguridad;
using Core.Entity.Administrativo.Seguridad.CatHorasHombre;
using Data.EntityFramework.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.DTO.Principal.Generales;
using Core.Enum.Administracion.Seguridad.Evaluacion;
using Core.Enum.Principal.Bitacoras;
using Infrastructure.Utils;
using Core.DTO.Utils.Data;
using Data.EntityFramework.Context;
using Core.DTO.Administracion.Seguridad.Capacitacion;
using Core.Enum.Multiempresa;
using System.Data.Odbc;
using Core.Entity.Principal.Multiempresa;
using Core.Enum.Principal;
using Core.DTO;



namespace Data.DAO.Administracion.Seguridad.CatHorasHombre
{
    public class CatHorasHombreDAO : GenericDAO<tblS_CatHorasHombre>, ICatHorasHombreDAO
    {
        string Controller = "CatHorasHombreController";
        private Dictionary<string, object> resultado = new Dictionary<string, object>();

        public List<ComboDTO> getCC()
        {
            var dataCC = _context.tblP_CC.Select(x => new ComboDTO
            {
                Value = x.id.ToString(),
                Text = x.cc + " - " + x.descripcion,
                Prefijo = x.cc
            }).ToList();

       
            using (var _db = new MainContext((int)EmpresaEnum.Arrendadora))
            {
                var lstCCArrendadora = _db.tblP_CC.Where(x => x.estatus == true).OrderBy(x => x.descripcion).ToList();
                var dataCCArrendadora = lstCCArrendadora.Select(x => new ComboDTO
                {
                    Value = x.id.ToString(),
                    Text = x.areaCuenta + " - " + x.descripcion,
                    Prefijo = x.areaCuenta
                }).ToList();
                dataCC.AddRange(dataCCArrendadora);
            }

            return dataCC;
        }

        public List<ComboDTO> getRoles()
        {
            var dataRoles = _context.tblP_RolesGrupoTrabajo.Select(x => new ComboDTO
            {
                Value = x.id.ToString(),
                Text = x.descripcion,
                Prefijo = x.cantDiasLaborales.ToString()
            }).ToList();
            return dataRoles;
        }

        public bool ActualizarHorasHombre(tblS_CatHorasHombre objHorasHombre)
        {
            try
            {
                var Actualizar = _context.tblS_CatHorasHombre.FirstOrDefault(x => x.id == objHorasHombre.id);
                if (vSesiones.sesionEmpresaActual == 3 || vSesiones.sesionEmpresaActual == 6)
                {
                    Actualizar.idGrupo = objHorasHombre.idGrupo;
                    Actualizar.fechaInicio = objHorasHombre.fechaInicio;
                    Actualizar.horas = objHorasHombre.horas;
                    Actualizar.fechaModificacion = DateTime.Now;   
                    Actualizar.idEmpresa = (int)vSesiones.sesionEmpresaActual;
                    _context.SaveChanges();
                }
                else
                {
                    Actualizar.idGrupo = objHorasHombre.idGrupo;
                    Actualizar.fechaInicio = objHorasHombre.fechaInicio;
                    Actualizar.horas = objHorasHombre.horas;
                    Actualizar.fechaModificacion = DateTime.Now;
                    //Actualizar.esActivo = true;
                    //Actualizar.catCC = null;
                    _context.SaveChanges();
                    //SaveBitacora(0, (int)AccionEnum.ACTUALIZAR, (int)Actualizar.id, JsonUtils.convertNetObjectToJson(Actualizar));
                   
                }
             
                

                return true;
            }
            catch (Exception e)
            {
                LogError(2, 0, Controller, "ActualizarHorasHombre", e, AccionEnum.ACTUALIZAR, 0, JsonUtils.convertNetObjectToJson(objHorasHombre));
                return false;
            }
        }

        public bool CrearHorasHombre(tblS_CatHorasHombre objHorasHombre)
        {
            try
            {
                if (vSesiones.sesionEmpresaActual == 3 || vSesiones.sesionEmpresaActual == 6)
                {
                    objHorasHombre.fechaCreacion = DateTime.Now;
                    objHorasHombre.fechaModificacion = DateTime.Now;
                    objHorasHombre.esActivo = true;
                    objHorasHombre.idEmpresa=(int)vSesiones.sesionEmpresaActual;
                    _context.tblS_CatHorasHombre.Add(objHorasHombre);
                    _context.SaveChanges();
                    //SaveBitacora(0, (int)AccionEnum.AGREGAR, 0, JsonUtils.convertNetObjectToJson(objHorasHombre));
                }else{
                    objHorasHombre.fechaCreacion = DateTime.Now;
                    objHorasHombre.fechaModificacion = DateTime.Now;
                    objHorasHombre.esActivo = true;
                  
                    _context.tblS_CatHorasHombre.Add(objHorasHombre);
                    _context.SaveChanges();
                }
                return true;
            }
            catch (Exception e)
            {
                LogError(2, 0, Controller, "CrearHorasHombre", e, AccionEnum.AGREGAR, 0, JsonUtils.convertNetObjectToJson(objHorasHombre));
                return false;
            }
        }

        public bool ValidarRegistroUnico(tblS_CatHorasHombre objHorasHombre)
        {
            try
            {
                var HorasHombre = _context.tblS_CatHorasHombre.Where(x => x.cc == objHorasHombre.cc && x.clave_depto == objHorasHombre.clave_depto).ToList();
                if (HorasHombre.Count() > 0)
                    return true;
                else
                    return false;
            }
            catch (Exception e)
            {
                LogError(2, 0, Controller, "ValidarRegistroUnico", e, AccionEnum.CONSULTA, 0, objHorasHombre);
                return false;
            }
        }

        public List<tblS_CatHorasHombre> GetHorasHombre(tblS_CatHorasHombre objHorasHombre)
        {
            try
            {
                var lstHorasHombre = new List<tblS_CatHorasHombre>();
                if (objHorasHombre.cc != "" && objHorasHombre.clave_depto <= 0 && objHorasHombre.idGrupo <= 0)
                {
                    lstHorasHombre = _context.tblS_CatHorasHombre.ToList();
                }
                else
                {
                    lstHorasHombre = _context.tblS_CatHorasHombre.Where(x => (objHorasHombre.cc != "" ? x.cc == objHorasHombre.cc : true) &&
                                                               (objHorasHombre.clave_depto > 0 ? x.clave_depto == objHorasHombre.clave_depto : true) &&
                                                               (objHorasHombre.idGrupo > 0 ? x.idGrupo == objHorasHombre.idGrupo : true)).ToList();
                }

                //for (int i = 0; i < lstHorasHombre.Count(); i++)
                //{
                //    int idCC = lstHorasHombre[i].idCC;
                //    var CC = GetCCHorasHombre(idCC);
                //    lstHorasHombre[i]. += " - " + CC[0].descripcion;
                //}

                return lstHorasHombre;
            }
            catch (Exception e)
            {
                LogError(2, 0, Controller, "GetHorasHombre", e, AccionEnum.CONSULTA, 0, 0);
                return null;
            }
        }

        //public List<tblP_CC> GetCCHorasHombre(int idCC)
        //{
        //    try
        //    {
        //        return _context.tblP_CC.Where(x => x.id == idCC).ToList();
        //    }
        //    catch (Exception e)
        //    {
        //        LogError(2, 0, Controller, "GetCCHorasHombre", e, AccionEnum.CONSULTA, 0, 0);
        //        return null;
        //    }
        //}


        public bool EliminarHorasHombre(int id)
        {
            try
            {
                var Eliminar = _context.tblS_CatHorasHombre.FirstOrDefault(x => x.id == id);
                if (Eliminar.esActivo)
                    Eliminar.esActivo = false;
                else
                    Eliminar.esActivo = true;

                _context.SaveChanges();

                //SaveBitacora(0, (int)AccionEnum.ELIMINAR, id, JsonUtils.convertNetObjectToJson(Eliminar));
                return true;
            }
            catch (Exception e)
            {
                LogError(2, 0, Controller, "EliminarHorasHombre", e, AccionEnum.ELIMINAR, id, 0);
                return false;
            }
        }

        public List<string> ObtenerDepartamento(int clave_depto)
        {
            try
            {
                var areasPorCC = new List<string>();

                string queryBase = @"
                    SELECT clave_depto as id, desc_depto as departamento, cc
                    FROM DBA.sn_departamentos 
                    WHERE id = {0}";

                if (clave_depto > 0)
                {
                    var odbc = new OdbcConsultaDTO();
                    odbc.consulta = String.Format(queryBase, Convert.ToInt32(clave_depto));
                    var departamentosCplan = _contextEnkontrol.Select<DepartamentoDTO>(EnkontrolEnum.CplanRh, odbc);

                    resultado.Add(SUCCESS, true);
                    return departamentosCplan.Select(x => x.departamento).ToList();
                }
                else
                {
                    return null;
                    throw new Exception();
                }
            }
            catch (Exception e)
            {
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, "Ocurrió un error interno al intentar obtener el catálogo tipos de emleados");
                return null;
            }
        }

        public Dictionary<string, object> ObtenerComboCCAmbasEmpresas()
        {
            try
            {
                var lstCC = new List<Core.DTO.Principal.Generales.ComboGroupDTO>();

                if (vSesiones.sesionEmpresaActual == 1 || vSesiones.sesionEmpresaActual == 2)
                {
                    string strQuery = @"SELECT cc as Value, (cc + ' - ' + descripcion) as Text FROM cc ORDER BY cc";
                    var odbc = new OdbcConsultaDTO() { consulta = strQuery };

                    #region SE CREA LISTADO DE CC DE CONSTRUPLAN
                    //var lstCCConstruplan = _contextEnkontrol.Select<Core.DTO.Principal.Generales.ComboDTO>(EnkontrolEnum.CplanRh, odbc);

                    var lstCCConstruplan = _context.tblP_CC.Where(x => x.estatus).Select(x => new Core.DTO.Principal.Generales.ComboDTO
                    {
                        Value = x.cc,
                        Text = x.cc + " - " + x.descripcion
                    }).ToList();


                    if (lstCCConstruplan.Count() > 0)
                    {
                        foreach (var itemCP in lstCCConstruplan)
                        {
                            itemCP.Prefijo = Convert.ToString(1);
                        }
                        lstCC.Add(new Core.DTO.Principal.Generales.ComboGroupDTO { label = "CONSTRUPLAN", options = lstCCConstruplan });
                    }
                    #endregion

                    #region SE CREA LISTADO DE CC DE ARRENDADORA
                    //var lstCCArrendadora = _contextEnkontrol.Select<Core.DTO.Principal.Generales.ComboDTO>(EnkontrolEnum.ArrenRh, odbc);
                    //if (lstCCArrendadora.Count() > 0)
                    //{
                    //    foreach (var itemArr in lstCCArrendadora)
                    //    {
                    //        itemArr.Prefijo = Convert.ToString(2);
                    //    }
                    //    lstCC.Add(new Core.DTO.Principal.Generales.ComboGroupDTO { label = "ARRENDADORA", options = lstCCArrendadora });
                    //}

                    var lstCCArrendadora = _context.Select<ComboDTO>(new DapperDTO
                    {
                        baseDatos = MainContextEnum.Arrendadora,
                        consulta = "SELECT cc as Value, (cc + ' - ' + ccDescripcion) as Text FROM tblC_Nom_CatalogoCC ORDER BY cc",
                    });
                    if (lstCCArrendadora.Count() > 0)
                    {
                        lstCC.Add(new Core.DTO.Principal.Generales.ComboGroupDTO { label = "ARRENDADORA", options = lstCCArrendadora });
                    }
                    #endregion
                }
                else if (vSesiones.sesionEmpresaActual == 3)
                {
                    var lstCCColombia = _context.tblP_CC.Where(x => x.estatus).Select(x => new Core.DTO.Principal.Generales.ComboDTO
                    {
                        Value = x.cc,
                        Text = x.cc + " - " + x.descripcion
                    }).ToList();


                    if (lstCCColombia.Count() > 0)
                    {
                        foreach (var itemCOL in lstCCColombia)
                        {
                            itemCOL.Prefijo = Convert.ToString(1);
                        }
                        lstCC.Add(new Core.DTO.Principal.Generales.ComboGroupDTO { label = "COLOMBIA", options = lstCCColombia });
                    }
                }
                else if (vSesiones.sesionEmpresaActual == 6)
                {
                      var lstCCPeru = _context.tblP_CC.Where(x => x.estatus).Select(x => new Core.DTO.Principal.Generales.ComboDTO
                    {
                        Value = x.cc,
                        Text = x.cc + " - " + x.descripcion
                    }).ToList();


                      if (lstCCPeru.Count() > 0)
                    {
                        foreach (var itemPer in lstCCPeru)
                        {
                            itemPer.Prefijo = Convert.ToString(1);
                        }
                        lstCC.Add(new Core.DTO.Principal.Generales.ComboGroupDTO { label = "PERÚ", options = lstCCPeru });
                    }
                }
                
                if (lstCC.Count > 0)
                {
                    resultado.Add(ITEMS, lstCC);
                    resultado.Add(SUCCESS, true);
                }
                else
                {
                    resultado.Add(SUCCESS, false);
                    resultado.Add("EMPTY", true);
                }
            }
            catch (Exception e)
            {
                resultado.Add(SUCCESS, false);
                LogError(0, 0, "IndicadoresSeguridadController", "ObtenerComboCCAmbasEmpresas", e, AccionEnum.CONSULTA, 0, null);
            }
            return resultado;
        }


        public Dictionary<string, object> ObtenerAreasPorCC(List<string> ccsCplan, int idEmpresa)
        {
            try
            {
                var areasPorCC = new List<ComboGroupDTO>();
                

                if ((ccsCplan == null || ccsCplan.Count == 0))
                {
                    resultado.Add(SUCCESS, false);
                    resultado.Add(MESSAGE, "La lista de centro de costos está vacía.");
                    return resultado;
                }
                if(vSesiones.sesionEmpresaActual==3 || vSesiones.sesionEmpresaActual==6)
                {
                    idEmpresa = 0;
                }


                if (idEmpresa == 1 || idEmpresa == 2)
                {
                    string queryBase = @"
                    SELECT clave_depto as id, desc_depto as departamento, cc
                    FROM DBA.sn_departamentos 
                    WHERE cc IN {0}";

                    var odbc = new OdbcConsultaDTO();
                    #region CONSTRUPLAN
                    if (ccsCplan != null && ccsCplan.Count > 0)
                    {
                        //odbc.consulta = String.Format(queryBase, ccsCplan.ToParamInValue());

                        //odbc.parametros.AddRange(
                        //    ccsCplan.Select(x => new OdbcParameterDTO() { nombre = "ccs", tipo = OdbcType.VarChar, valor = x })
                        //);

                        //var departamentosCplan = _contextEnkontrol.Select<DepartamentoDTO>(EnkontrolEnum.CplanRh, odbc);

                        var departamentosCplan = _context.Select<DepartamentoDTO>(new DapperDTO
                        {
                            baseDatos = MainContextEnum.Construplan,
                            consulta = @"SELECT clave_depto AS id, desc_depto AS departamento, cc FROM tblRH_EK_Departamentos WHERE cc IN @ccs",
                            parametros = new { ccs = ccsCplan }
                        });

                        if (departamentosCplan.Count > 0)
                        {
                            var areasCplan = departamentosCplan
                                .GroupBy(x => x.cc)
                                .OrderBy(x => x.Key)
                                .Select(x => new ComboGroupDTO
                                {
                                    label = "CONSTRUPLAN " + x.Key,
                                    options = x.Select(y => new ComboDTO { Value = y.id, Text = y.departamento, Prefijo = "1", Id = x.Key }).ToList()
                                }).ToList();

                            areasPorCC.AddRange(areasCplan);
                        }
                    }
                    #endregion
                
                //if (idEmpresa == 2)
                //{
                    #region ARRENDADORA
                    if (ccsCplan != null && ccsCplan.Count > 0)
                    {
                        //odbc.consulta = String.Format(queryBase, ccsCplan.ToParamInValue());

                        //odbc.parametros.Clear();

                        //odbc.parametros.AddRange(
                        //    ccsCplan.Select(x => new OdbcParameterDTO() { nombre = "ccs", tipo = OdbcType.VarChar, valor = x })
                        //);

                        //var departamentosArr = _contextEnkontrol.Select<DepartamentoDTO>(EnkontrolEnum.ArrenRh, odbc);

                        var departamentosArr = _context.Select<DepartamentoDTO>(new DapperDTO
                        {
                            baseDatos = MainContextEnum.Arrendadora,
                            consulta = @"SELECT clave_depto AS id, desc_depto AS departamento, cc FROM tblRH_EK_Departamentos WHERE cc IN @ccs",
                            parametros = new { ccs = ccsCplan }
                        });

                        if (departamentosArr.Count > 0)
                        {
                            var areasArr = departamentosArr
                                .GroupBy(x => x.cc)
                                .OrderBy(x => x.Key)
                                .Select(x => new ComboGroupDTO
                                {
                                    label = "ARRENDADORA " + x.Key,
                                    options = x.Select(y => new ComboDTO { Value = y.id, Text = y.departamento, Prefijo = "2", Id = x.Key }).ToList()
                                }).ToList();

                            areasPorCC.AddRange(areasArr);
                        }
                    }
                    #endregion
                }
                else if (vSesiones.sesionEmpresaActual==3)
                {
                    #region COLOMBIA
                  
                        var departamentosCol = _context.Select<DepartamentoDTO>(new DapperDTO
                        {
                            baseDatos = MainContextEnum.Colombia,
                            consulta = @"SELECT clave_depto AS id, desc_depto AS departamento, cc FROM tblRH_EK_Departamentos WHERE cc IN @ccs",
                            parametros = new { ccs = ccsCplan }
                        });

                        if (departamentosCol.Count > 0)
                        {
                            var areasCol = departamentosCol
                                .GroupBy(x => x.cc)
                                .OrderBy(x => x.Key)
                                .Select(x => new ComboGroupDTO
                                {
                                    label = "COLOMBIA " + x.Key,
                                    options = x.Select(y => new ComboDTO { Value = y.id, Text = y.departamento, Prefijo = "1", Id = x.Key }).ToList()
                                }).ToList();

                            areasPorCC.AddRange(areasCol);
                        }
                            #endregion
                }
                    else if (vSesiones.sesionEmpresaActual == 6)
                {
                    #region PERU
                  

                        var departamentosPer = _context.Select<DepartamentoDTO>(new DapperDTO
                        {
                            baseDatos = MainContextEnum.PERU,
                            consulta = @"SELECT clave_depto AS id, desc_depto AS departamento, cc FROM tblRH_EK_Departamentos WHERE cc IN @ccs",
                            parametros = new { ccs = ccsCplan }
                        });

                        if (departamentosPer.Count > 0)
                        {
                            var areasPer = departamentosPer
                                .GroupBy(x => x.cc)
                                .OrderBy(x => x.Key)
                                .Select(x => new ComboGroupDTO
                                {
                                    label = "PERU " + x.Key,
                                    options = x.Select(y => new ComboDTO { Value = y.id, Text = y.departamento, Prefijo = "1", Id = x.Key }).ToList()
                                }).ToList();

                            areasPorCC.AddRange(areasPer);
                        }
                    
                    #endregion
                }

                resultado.Add(SUCCESS, true);
                resultado.Add(ITEMS, areasPorCC);
            }
            catch (Exception e)
            {
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, "Ocurrió un error interno al intentar obtener el catálogo tipos de emleados");
            }

            return resultado;
        }

    }
}
