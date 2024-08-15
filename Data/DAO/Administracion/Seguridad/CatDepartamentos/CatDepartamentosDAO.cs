using Core.DAO.Administracion.Seguridad.CatDepartamentos;
using Core.DTO;
using Core.DTO.Administracion.Seguridad.Capacitacion;
using Core.DTO.Administracion.Seguridad.CatDepartamentos;
using Core.DTO.Principal.Generales;
using Core.DTO.Utils.Data;
using Core.Entity.Administrativo.Seguridad.CatDepartamentos;
using Core.Entity.Administrativo.Seguridad.Indicadores;
using Core.Entity.Principal.Multiempresa;
using Core.Enum.Multiempresa;
using Core.Enum.Principal;
using Core.Enum.Principal.Bitacoras;
using Data.EntityFramework.Context;
using Data.EntityFramework.Generic;
using Infrastructure.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.DAO.Administracion.Seguridad.CatDepartamentos
{
    public class CatDepartamentosDAO : GenericDAO<tblS_CatDepartamentos>, ICatDepartamentosDAO
    {
        string Controller = "CatDepartamentosController";
        private Dictionary<string, object> resultado;

        public Dictionary<string, object> getClaveDepto()
        {
            try
            {
                resultado = new Dictionary<string, object>();
                var lstCC = new List<Core.DTO.Principal.Generales.ComboGroupDTO>();

                string queryBase = @"SELECT cc AS Value , clave_depto + ' - ' + desc_depto AS Text,clave_depto as Id FROM DBA.sn_departamentos";
                var odbc = new OdbcConsultaDTO() { consulta = queryBase };
if(vSesiones.sesionEmpresaActual==1 ||vSesiones.sesionEmpresaActual==2 )
{
    #region SE CREA LISTADO DE CC DE CONSTRUPLAN
    var lstCCConstruplan = _contextEnkontrol.Select<Core.DTO.Principal.Generales.ComboDTO>(EnkontrolEnum.CplanRh, odbc).Select(x => new ComboDTO
    {
        Value = x.Value,
        Text = x.Text,
        Prefijo = x.Id
    }).ToList();
    if (lstCCConstruplan.Count() > 0)
    {
        lstCC.Add(new Core.DTO.Principal.Generales.ComboGroupDTO { label = "CONSTRUPLAN", options = lstCCConstruplan });
    }
    #endregion

    #region SE CREA LISTADO DE CC DE ARRENDADORA
    var lstCCArrendadora = _contextEnkontrol.Select<Core.DTO.Principal.Generales.ComboDTO>(EnkontrolEnum.ArrenRh, odbc).Select(x => new ComboDTO
    {
        Value = x.Value,
        Text = x.Text,
        Prefijo = x.Id
    }).ToList();
    if (lstCCArrendadora.Count() > 0)
    {
        lstCC.Add(new Core.DTO.Principal.Generales.ComboGroupDTO { label = "ARRENDADORA", options = lstCCArrendadora });
    }
    #endregion
}else if(vSesiones.sesionEmpresaActual==3)
{

    #region SE CREA LISTADO DE CC DE COLOMBIA
    var lstCCColombia = _context.tblP_CC.Where(x => x.estatus).Select(x => new Core.DTO.Principal.Generales.ComboDTO
    {
        Value = x.cc,
        Text = x.cc + " - " + x.descripcion
    }).ToList();
    if (lstCCColombia.Count() > 0)
    {
        lstCC.Add(new Core.DTO.Principal.Generales.ComboGroupDTO { label = "COLOMBIA", options = lstCCColombia });
    }
    #endregion
}
else if (vSesiones.sesionEmpresaActual == 6)
{
    #region SE CREA LISTADO DE CC DE PERU
    var lstCCPeru = _context.tblC_Nom_CatalogoCC.Where(x => x.estatus).Select(x => new Core.DTO.Principal.Generales.ComboDTO
    {
        Value = x.cc,
        Text = x.cc + " - " + x.ccDescripcion
    }).ToList();
    if (lstCCPeru.Count() > 0)
    {
        lstCC.Add(new Core.DTO.Principal.Generales.ComboGroupDTO { label = "PERU", options = lstCCPeru });
    }
    #endregion
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
                resultado.Add(MESSAGE, "Ocurrió un error interno al intentar obtener el catálogo tipos de emleados");
                return null;
            }
            return resultado;
        }

        public List<ComboDTO> getAreaOperativa()
        {
            var data = _context.tblS_CatAreasOperativas.Where(x => x.esActivo).Select(x => new ComboDTO
            {
                Value = x.id.ToString(),
                Text = x.descripcion
            }).ToList();
            return data;
        }

        public bool CrearDepartamento(tblS_CatDepartamentos objDepartamento)
        {
            try
            {

                objDepartamento.esActivo = true;
                _context.tblS_CatDepartamentos.Add(objDepartamento);
                _context.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                LogError(2, 0, Controller, "CrearDepartamento", e, AccionEnum.AGREGAR, 0, JsonUtils.convertNetObjectToJson(objDepartamento));
                return false;
            }
        }

        public List<CatDepartamentosDTO> GetCatDepartamentos(CatDepartamentosDTO objDeptFiltro)
        {
            try
            {
                List<tblS_CatAreasOperativas> lstAreas = _context.tblS_CatAreasOperativas.ToList();

                var lstCatDepartamentos = new List<CatDepartamentosDTO>();
                  
                if ((objDeptFiltro.cc == string.Empty || objDeptFiltro.cc == null) && objDeptFiltro.clave_depto <= 0 && objDeptFiltro.idAreaOperativa <= 0)
                {
                   
                    lstCatDepartamentos = _context.tblS_CatDepartamentos.ToList().Select(x => new CatDepartamentosDTO
                    {
                        id = x.id,
                        clave_depto = x.clave_depto,
                        descripcion = x.descripcion,
                        cc = x.cc,
                        idAreaOperativa = x.idAreaOperativa,
                        NombreAreaOperativa = lstAreas.Where(y => y.id == x.idAreaOperativa).Select(c => c.descripcion).FirstOrDefault(),
                        idEmpresa = x.idEmpresa,                     
                        esActivo = x.esActivo
                    }).ToList();
                }
                else
                {
                    lstCatDepartamentos = _context.tblS_CatDepartamentos.Where(x =>
                        ((objDeptFiltro.cc == string.Empty || objDeptFiltro.cc == null) ? true : x.cc == objDeptFiltro.cc) &&
                        (objDeptFiltro.clave_depto > 0 ? x.clave_depto == objDeptFiltro.clave_depto : true) &&
                        (objDeptFiltro.idAreaOperativa > 0 ? x.idAreaOperativa == objDeptFiltro.idAreaOperativa : true)).ToList().Select(x => new CatDepartamentosDTO
                        {
                            id = x.id,
                            clave_depto = x.clave_depto,
                            descripcion = x.descripcion,
                            cc = x.cc,
                            idAreaOperativa = x.idAreaOperativa,
                            NombreAreaOperativa = lstAreas.Where(y => y.id == x.idAreaOperativa).Select(c => c.descripcion).FirstOrDefault(),
                            idEmpresa = x.idEmpresa,
                            esActivo = x.esActivo
                        }).ToList();
                }


                if (vSesiones.sesionEmpresaActual == 6)
                {
                    if ((objDeptFiltro.cc == string.Empty || objDeptFiltro.cc == null) && objDeptFiltro.clave_depto <= 0 && objDeptFiltro.idAreaOperativa <= 0)
                    {

                        lstCatDepartamentos = _context.tblS_CatDepartamentos.ToList().Select(x => new CatDepartamentosDTO
                        {
                            id = x.id,
                            clave_depto = x.clave_depto,
                            descripcion = x.descripcion,
                            cc = x.cc,
                            idAreaOperativa = x.idAreaOperativa,
                            NombreAreaOperativa = lstAreas.Where(y => y.id == x.idAreaOperativa).Select(c => c.descripcion).FirstOrDefault(),
                            idEmpresa = x.idEmpresa,
                            esActivo = x.esActivo
                        }).ToList().Where(x => x.esActivo).ToList();
                    }
                    else
                    {
                        lstCatDepartamentos = _context.tblS_CatDepartamentos.Where(x =>
                            ((objDeptFiltro.cc == string.Empty || objDeptFiltro.cc == null) ? true : x.cc == objDeptFiltro.cc) &&
                            (objDeptFiltro.clave_depto > 0 ? x.clave_depto == objDeptFiltro.clave_depto : true) &&
                            (objDeptFiltro.idAreaOperativa > 0 ? x.idAreaOperativa == objDeptFiltro.idAreaOperativa : true)).ToList().Select(x => new CatDepartamentosDTO
                            {
                                id = x.id,
                                clave_depto = x.clave_depto,
                                descripcion = x.descripcion,
                                cc = x.cc,
                                idAreaOperativa = x.idAreaOperativa,
                                NombreAreaOperativa = lstAreas.Where(y => y.id == x.idAreaOperativa).Select(c => c.descripcion).FirstOrDefault(),
                                idEmpresa = x.idEmpresa,
                                esActivo = x.esActivo
                            }).ToList().Where(x => x.esActivo).ToList();
                    }
                }
                return lstCatDepartamentos;
            }
            catch (Exception e)
            {
                LogError(2, 0, Controller, "GetCatDepartamentos", e, AccionEnum.CONSULTA, 0, 0);
                return null;
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

        public List<string> ObtenerAreaOperativa(int idAreaOperativa)
        {
            try
            {
                if (idAreaOperativa > 0)
                {
                    var areaOperativa = _context.tblS_CatAreasOperativas.Where(x => x.id == idAreaOperativa && x.esActivo).Select(x => x.descripcion).ToList();
                    if (areaOperativa.Count() > 0)
                        return areaOperativa;
                    else
                        return null;
                }
                return null;
            }
            catch (Exception e)
            {
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, "Ocurrió un error interno al intentar obtener la descripción del Área operativa");
                return null;
            }
        }

        public bool ActivarDesactivarDepartamento(int id)
        {
            try
            {
                var Eliminar = _context.tblS_CatDepartamentos.FirstOrDefault(x => x.id == id);
                if (Eliminar.esActivo)
                    Eliminar.esActivo = false;
                else
                    Eliminar.esActivo = true;

                _context.SaveChanges();

                return true;
            }
            catch (Exception e)
            {
                LogError(2, 0, Controller, "ActivarDesactivarDepartamento", e, AccionEnum.ELIMINAR, id, 0);
                return false;
            }
        }

        public bool ActualizarDepartamento(tblS_CatDepartamentos objDepartamento)
        {
            try
            {
                var Actualizar = _context.tblS_CatDepartamentos.FirstOrDefault(x => x.id == objDepartamento.id);
                Actualizar.idAreaOperativa = objDepartamento.idAreaOperativa;
                Actualizar.descripcion = objDepartamento.descripcion;
                _context.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                LogError(2, 0, Controller, "ActualizarDepartamento", e, AccionEnum.ACTUALIZAR, 0, JsonUtils.convertNetObjectToJson(objDepartamento));
                return false;
            }
        }

        public bool EsRegistroUnico(tblS_CatDepartamentos objDepartamento)
        {
            try
            {
                //var data = _context.tblS_CatDepartamentos.Where(x => x.cc == objDepartamento.cc && 
                //                                                     x.clave_depto == objDepartamento.clave_depto &&
                //                                                     x.idAreaOperativa == objDepartamento.idAreaOperativa).ToList();
                var data = new Object();
                if (objDepartamento.clave_depto > 0)
                    data = _context.tblS_CatDepartamentos.Where(x => x.clave_depto == objDepartamento.clave_depto && x.idAreaOperativa == objDepartamento.idAreaOperativa).Count();

                if (Convert.ToInt32(data) > 0)
                    return true;
                else
                    return false;
            }
            catch (Exception e)
            {
                LogError(2, 0, Controller, "EsRegistroUnico", e, AccionEnum.CONSULTA, 0, objDepartamento);
                return false;
            }
        }

        public List<ComboDTO> getCC()
        {
            try
            {
                var areasPorCC = new List<string>();

                string queryBase = @"SELECT cc as departamento , descripcion FROM DBA.cc";

                var odbc = new OdbcConsultaDTO();
                odbc.consulta = String.Format(queryBase);

                var data = _contextEnkontrol.Select<DepartamentoDTO>(EnkontrolEnum.CplanRh, odbc).Select(x => new ComboDTO
                {
                    Value = x.departamento,
                    Text = x.departamento + " - " + x.descripcion,
                    Prefijo = x.departamento
                }).ToList();
                return data;

            }
            catch (Exception e)
            {
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, "Ocurrió un error interno al intentar obtener el catálogo tipos de emleados");
                return null;
            }

            #region
            //var dataCC =  _context.tblP_CC.Where(r => r.cc == cc).Select(x => new ComboDTO
            // {
            //     Value = x.id.ToString(),
            //     Text = x.cc + " - " + x.descripcion,
            //     Prefijo = x.cc
            // }).ToList();



            //            using (var _db = new MainContext((int)EmpresaEnum.Arrendadora))
            //            {
            //                var lstCCArrendadora = _db.tblP_CC.Where(x => x.estatus == true && x.cc == cc).OrderBy(x => x.descripcion).ToList();
            //                var dataCCArrendadora = lstCCArrendadora.Select(x => new ComboDTO
            //                {
            //                    Value = x.id.ToString(),
            //                    Text = x.areaCuenta + " - " + x.descripcion,
            //                    Prefijo = x.areaCuenta
            //                }).ToList();
            //                dataCC.AddRange(dataCCArrendadora);
            //            }

            #endregion
        }


        public Dictionary<string, object> ObtenerComboCCAmbasEmpresas()
        {
            try
            {
                resultado = new Dictionary<string, object>();
                var lstCC = new List<Core.DTO.Principal.Generales.ComboGroupDTO>();

                string strQuery = @"SELECT cc as Value, (cc + ' - ' + descripcion) as Text FROM cc ORDER BY cc";
                var odbc = new OdbcConsultaDTO() { consulta = strQuery };

                if(vSesiones.sesionEmpresaActual==1|| vSesiones.sesionEmpresaActual==2)
                {
                    #region SE CREA LISTADO DE CC DE CONSTRUPLAN
                    //var lstCCConstruplan = _contextEnkontrol.Select<Core.DTO.Principal.Generales.ComboDTO>(EnkontrolEnum.CplanRh, odbc);
                    var lstCCConstruplan = _context.Select<ComboDTO>(new DapperDTO
                    {
                        baseDatos = MainContextEnum.Construplan,
                        consulta = "SELECT cc as Value, (cc + ' - ' + ccDescripcion) as Text FROM tblC_Nom_CatalogoCC ORDER BY cc",
                    });

                    if (lstCCConstruplan.Count() > 0)
                    {
                        lstCC.Add(new Core.DTO.Principal.Generales.ComboGroupDTO { label = "CONSTRUPLAN", options = lstCCConstruplan });
                    }
                    #endregion

                    #region SE CREA LISTADO DE CC DE ARRENDADORA
                    //var lstCCArrendadora = _contextEnkontrol.Select<Core.DTO.Principal.Generales.ComboDTO>(EnkontrolEnum.ArrenRh, odbc);}

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
                    #region SE CREA LISTADO DE CC COLOMBIA
                    //var lstCCConstruplan = _contextEnkontrol.Select<Core.DTO.Principal.Generales.ComboDTO>(EnkontrolEnum.CplanRh, odbc);
                    var lstCCEmpresas = _context.Select<ComboDTO>(new DapperDTO
                    {
                        baseDatos = MainContextEnum.Colombia,
                        consulta = "SELECT cc as Value, (cc + ' - ' + ccDescripcion) as Text FROM tblC_Nom_CatalogoCC ORDER BY cc",
                    });

                    if (lstCCEmpresas.Count() > 0)
                    {
                        lstCC.Add(new Core.DTO.Principal.Generales.ComboGroupDTO { label = "COLOMBIA", options = lstCCEmpresas });
                    }
                    #endregion
                }
                else if (vSesiones.sesionEmpresaActual == 6)
                {
                    #region SE CREA LISTADO DE CC PARA PERU
                    //var lstCCConstruplan = _contextEnkontrol.Select<Core.DTO.Principal.Generales.ComboDTO>(EnkontrolEnum.CplanRh, odbc);
                    var lstCCEmpresas = _context.Select<ComboDTO>(new DapperDTO
                    {
                        baseDatos = MainContextEnum.PERU,
                        consulta = "SELECT cc as Value, (cc + ' - ' + ccDescripcion) as Text FROM tblC_Nom_CatalogoCC ORDER BY cc",
                    });

                    if (lstCCEmpresas.Count() > 0)
                    {
                        lstCC.Add(new Core.DTO.Principal.Generales.ComboGroupDTO { label = "PERU", options = lstCCEmpresas });
                    }
                    #endregion
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
        public Dictionary<string, object> ObtenerCCporDepartamento(string cc, int idEmpresa)
        {
            try
            {
                var lstCC = new List<Core.DTO.Principal.Generales.ComboGroupDTO>();
                resultado = new Dictionary<string, object>();
                //string strQuery = @"SELECT clave_depto as Value, clave_depto+' - ' + desc_depto as Text FROM DBA.sn_departamentos WHERE cc = '" + cc + "'";
                //var odbc = new OdbcConsultaDTO() { consulta = strQuery };
               if (idEmpresa == (int)EmpresaEnum.Construplan)
                  {
                      var lstCCConstruplan = _context.Select<Core.DTO.Principal.Generales.ComboDTO>(new DapperDTO
                      {
                          baseDatos = MainContextEnum.Construplan,
                          consulta = @"SELECT clave_depto AS Value, (CAST(clave_depto as varchar) + ' - ' + desc_depto) AS Text FROM tblRH_EK_Departamentos WHERE cc = '" + cc + "'"
                      });

                      #region SE CREA LISTADO DE CC DE CONSTRUPLAN
                      //var lstCCConstruplan = _contextEnkontrol.Select<Core.DTO.Principal.Generales.ComboDTO>(EnkontrolEnum.CplanRh, odbc);
                      if (lstCCConstruplan.Count() > 0)
                      {
                          foreach (var itemCP in lstCCConstruplan)
                          {
                              itemCP.Prefijo = EnkontrolEnum.CplanRh.ToString();
                          }
                          lstCC.Add(new Core.DTO.Principal.Generales.ComboGroupDTO { label = "CONSTRUPLAN", options = lstCCConstruplan });
                      }
                      #endregion

                  }
                if (idEmpresa == (int)EmpresaEnum.Arrendadora)
                  {
                      var lstCCArrendadora = _context.Select<Core.DTO.Principal.Generales.ComboDTO>(new DapperDTO
                      {
                          baseDatos = MainContextEnum.Arrendadora,
                          consulta = @"SELECT clave_depto AS Value, (CAST(clave_depto as varchar) + ' - ' + desc_depto) AS Text FROM tblRH_EK_Departamentos WHERE cc = '" + cc + "'"
                      });

                      #region SE CREA LISTADO DE CC DE ARRENDADORA
                      //var lstCCArrendadora = _contextEnkontrol.Select<Core.DTO.Principal.Generales.ComboDTO>(EnkontrolEnum.ArrenRh, odbc);
                      if (lstCCArrendadora.Count() > 0)
                      {
                          foreach (var itemArr in lstCCArrendadora)
                          {
                              itemArr.Prefijo = EnkontrolEnum.ArrenRh.ToString();
                          }
                          lstCC.Add(new Core.DTO.Principal.Generales.ComboGroupDTO { label = "ARRENDADORA", options = lstCCArrendadora });
                      }
                      #endregion

                  }
                if (idEmpresa == (int)EmpresaEnum.Colombia)
                {
                    var lstCCColombia = _context.Select<Core.DTO.Principal.Generales.ComboDTO>(new DapperDTO
                    {
                        baseDatos = MainContextEnum.Colombia,
                        consulta = @"SELECT clave_depto AS Value, (CAST(cc as varchar) + ' - ' + desc_depto) AS Text FROM tblRH_EK_Departamentos WHERE cc = '" + cc + "'"
                    });

                    #region SE CREA LISTADO DE CC DE COLOMBIA
                    //var lstCCArrendadora = _contextEnkontrol.Select<Core.DTO.Principal.Generales.ComboDTO>(EnkontrolEnum.ArrenRh, odbc);
                    if (lstCCColombia.Count() > 0)
                    {
                        foreach (var itemCol in lstCCColombia)
                        {
                            itemCol.Prefijo = "COLOMBIA";
                        }
                        lstCC.Add(new Core.DTO.Principal.Generales.ComboGroupDTO { label = "COLOMBIA", options = lstCCColombia });
                    }
                }
                        #endregion


                if (idEmpresa == (int)EmpresaEnum.Peru)
                {
                    {
                        var lstCCPeru = _context.Select<Core.DTO.Principal.Generales.ComboDTO>(new DapperDTO
                            {
                                baseDatos = MainContextEnum.PERU,
                                consulta = @"SELECT clave_depto AS Value, (CAST(cc as varchar) + ' - ' + desc_depto) AS Text FROM tblRH_EK_Departamentos WHERE cc = '" + cc + "'"
                            });

                        #region SE CREA LISTADO DE CC DE PERU
                        //var lstCCArrendadora = _contextEnkontrol.Select<Core.DTO.Principal.Generales.ComboDTO>(EnkontrolEnum.ArrenRh, odbc);
                        if (lstCCPeru.Count() > 0)
                        {
                            foreach (var itemPeru in lstCCPeru)
                            {
                                itemPeru.Prefijo = "PERU";
                            }
                            lstCC.Add(new Core.DTO.Principal.Generales.ComboGroupDTO { label = "PERU", options = lstCCPeru });
                        }
                        #endregion
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
        public Dictionary<string, object> ObtenerCCporDepartamentoEditar(string claveDepto, int idEmpresa)
        {
            try
            {
                var lstCC = new List<Core.DTO.Principal.Generales.ComboGroupDTO>();
                resultado = new Dictionary<string, object>();

                if(idEmpresa==1 || idEmpresa==2)
                {
                    string strQuery = @"SELECT cc as Value, cc+' - ' + descripcion as Text FROM DBA.cc WHERE cc = '" + claveDepto + "'";
                    var odbc = new OdbcConsultaDTO() { consulta = strQuery };

                    if (idEmpresa == (int)EmpresaEnum.Construplan)
                    {
                        #region SE CREA LISTADO DE CC DE CONSTRUPLAN
                        var lstCCConstruplan = _contextEnkontrol.Select<Core.DTO.Principal.Generales.ComboDTO>(EnkontrolEnum.CplanRh, odbc);
                        if (lstCCConstruplan.Count() > 0)
                        {
                            foreach (var itemCP in lstCCConstruplan)
                            {
                                itemCP.Prefijo = EnkontrolEnum.CplanRh.ToString();
                            }
                            lstCC.Add(new Core.DTO.Principal.Generales.ComboGroupDTO { label = "CONSTRUPLAN", options = lstCCConstruplan });
                        }
                        #endregion

                    }
                    if (idEmpresa == (int)EmpresaEnum.Arrendadora)
                    {
                        #region SE CREA LISTADO DE CC DE ARRENDADORA
                        var lstCCArrendadora = _contextEnkontrol.Select<Core.DTO.Principal.Generales.ComboDTO>(EnkontrolEnum.ArrenRh, odbc);
                        if (lstCCArrendadora.Count() > 0)
                        {
                            foreach (var itemArr in lstCCArrendadora)
                            {
                                itemArr.Prefijo = EnkontrolEnum.ArrenRh.ToString();
                            }
                            lstCC.Add(new Core.DTO.Principal.Generales.ComboGroupDTO { label = "ARRENDADORA", options = lstCCArrendadora });
                        }
                        #endregion

                    }
                }
                else if (idEmpresa == (int)EmpresaEnum.Colombia)
                    {
                        var lstCCColombia = _context.Select<Core.DTO.Principal.Generales.ComboDTO>(new DapperDTO
                        {
                            baseDatos = MainContextEnum.Colombia,
                            consulta = @"SELECT clave_depto AS Value, (CAST(clave_depto as varchar) + ' - ' + desc_depto) AS Text FROM tblRH_EK_Departamentos WHERE cc = '" + claveDepto + "'"
                        });
                        if (lstCCColombia.Count() > 0)
                        {
                            foreach (var itemCol in lstCCColombia)
                            {
                                itemCol.Prefijo = "COLOMBIA";
                            }
                            lstCC.Add(new Core.DTO.Principal.Generales.ComboGroupDTO { label = "COLOMBIA", options = lstCCColombia });
                        }
                    }
                else if (idEmpresa == (int)EmpresaEnum.Peru)
                    {
                        var lstCCPeru = _context.Select<Core.DTO.Principal.Generales.ComboDTO>(new DapperDTO
                        {
                            baseDatos = MainContextEnum.PERU,
                            consulta = @"SELECT clave_depto AS Value, (CAST(clave_depto as varchar) + ' - ' + desc_depto) AS Text FROM tblRH_EK_Departamentos WHERE cc = '" + claveDepto + "'"
                        });
                        if (lstCCPeru.Count() > 0)
                        {
                            foreach (var itemPer in lstCCPeru)
                            {
                                itemPer.Prefijo = "PERU";
                            }
                            lstCC.Add(new Core.DTO.Principal.Generales.ComboGroupDTO { label = "PERU", options = lstCCPeru });
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




    }
}
