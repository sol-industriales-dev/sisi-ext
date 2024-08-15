using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity.SqlServer;
using Data.EntityFramework.Generic;
using Core.Entity.Maquinaria.BackLogs;
using Core.DTO.Principal.Generales;
using Core.Enum.Principal.Bitacoras;
using Core.Entity.Maquinaria.Catalogo;
using Core.Entity.Maquinaria.Caratulas;
using Core.DTO.Maquinaria.Caratulas;
using Core.DAO.Maquinaria.Caratulas;
using System.Web;
using OfficeOpenXml;
using Core.DTO.Maquinaria._Caratulas;
using Core.Entity.Maquinaria._Caratulas;
using Core.Entity.Principal.Usuarios;
using Core.Entity.Principal.Multiempresa;
using Core.DTO;
using Core.Enum.Principal;
using Infrastructure.Utils;
using Core.DTO.Utils.Auth;
using Core.Entity.Principal.Alertas;
using System.IO;


namespace Data.DAO.Maquinaria.Caratulas
{
    public class CaratulasDAO : GenericDAO<tblM_Caratulas>, ICaratulasDAO
    {
        private Dictionary<string, object> result = new Dictionary<string, object>();

        #region FILL COMBOS
        public List<ComboDTO> FillAreasCuentas()
        {
            var ids = _context.tblM_IndicadoresCaratula.Select(x => x.idCC).ToList();
            var data = _context.tblP_CC.Where(x => x.estatus && !ids.Contains(x.id)).Select(x => new ComboDTO
            {
                Value = x.id.ToString(),
                Text = x.areaCuenta + " - " + x.descripcion
            }).ToList();

            return data.ToList();
        }

        public List<ComboDTO> FillCboModelo()
        {
            var idModelos = _context.tblM_CaratulaDet.Select(y => y.idModelo).ToList();
            var dataModelo = _context.tblM_CatModeloEquipo.Where(r=> !idModelos.Contains(r.id)).Select(x => new ComboDTO
            {
                Value = x.id.ToString(),
                Text = x.descripcion
            }).ToList();

            return dataModelo;
        }

        public List<ComboDTO> FillCboGrupo()
        {
            var dataGrupo = _context.tblM_CatGrupoMaquinaria.Select(x => new ComboDTO
            {
                Value = x.id.ToString(),
                Text = x.descripcion
            }).ToList();

            return dataGrupo;
        }



        public List<ComboDTO> FillCaratulas()
        {
            List<tblM_Caratula> ultimoRegistro = _context.tblM_Caratula.OrderByDescending(x => x.id).ToList();
            int idRegistro = ultimoRegistro.Count() > 0 ? idRegistro = ultimoRegistro.FirstOrDefault().idCaratula : 0;
            if (idRegistro == 0)
                throw new Exception("Ocurrió un error.");
            var data = _context.tblM_Caratula.Where(x => x.idCaratula == idRegistro).Select(x => new ComboDTO
            {
                Value = x.idCaratula.ToString(),
                Text = "Caratula " + x.idCaratula
            }).ToList();
            return data.ToList();
        }
        #endregion

        #region MOSTRAR EXCEL SELECCIONADO EN DATATABLE
        public List<CaratulaGuardadoDTO> MostrarArchivo(HttpPostedFileBase archivo, decimal tipoCambio)
        {
            CaratulaGuardadoDTO objRetornar = new CaratulaGuardadoDTO();
            List<CaratulaGuardadoDTO> lstReturn = new List<CaratulaGuardadoDTO>();
            int error = 0;
            int errorhorizontal = 0;
            int ejem = 0;
            try
            {
                ExcelPackage package = new ExcelPackage(archivo.InputStream);
                ExcelWorksheet worksheet = package.Workbook.Worksheets.FirstOrDefault();
                List<tblM_CatGrupoMaquinaria> lstGrupo = _context.tblM_CatGrupoMaquinaria.Where(x => x.estatus).ToList();
                List<tblM_CatModeloEquipo> lstModelo = _context.tblM_CatModeloEquipo.Where(x => x.estatus).ToList();
                // get number of rows and columns in the sheet
                int rows = worksheet.Dimension.Rows; // 20
                int columns = worksheet.Dimension.Columns; // 7   
                tblM_CatGrupoMaquinaria objGrupo = new tblM_CatGrupoMaquinaria();
                tblM_CatModeloEquipo objModelo = new tblM_CatModeloEquipo();
                objGrupo.descripcion = "-";
                objModelo.descripcion = "-";
                // loop through the worksheet rows and columns
                for (int i = 2; i <= rows; i++)
                {
                    error++;
                    if (error == 16)
                    {
                        error = error;
                    }
                    errorhorizontal = 0;
                    objRetornar = new CaratulaGuardadoDTO();
                    for (int j = 1; j <= columns; j++)
                    {
                        errorhorizontal++;
                        string content = "";
                        if (worksheet.Cells[i, j].Value != null)
                        {
                            content = worksheet.Cells[i, j].Value.ToString();
                        }
                        switch (j)
                        {
                            case 1:
                                if (content == "")
                                {
                                    content = "0";
                                }
                                objRetornar.idGrupo = Convert.ToInt32(content);
                                objRetornar.lstCatGrupo= lstGrupo.Where(y => y.id == objRetornar.idGrupo).FirstOrDefault() == null ? objGrupo : lstGrupo.Where(y => y.id == objRetornar.idGrupo).FirstOrDefault();

                                break;
                            case 2:
                                if (content == "")
                                {
                                    content = "0";
                                }
                                objRetornar.idMod = Convert.ToInt32(content);
                                objRetornar.idModelo = convertir(objRetornar.idMod);
                                objRetornar.lstCatModelo = lstModelo.Where(y => y.id == objRetornar.idMod).FirstOrDefault() == null ? objModelo : lstModelo.Where(y => y.id == objRetornar.idMod).FirstOrDefault();
                                break;
                            case 3: if (content == "")
                                {
                                    content = "0";
                                }
                                objRetornar.depreciacionDLLS = Convert.ToDecimal(content);
                                objRetornar.depreciacionMXN = Convert.ToDecimal(content) * tipoCambio ;
                                break;
                            case 4: if (content == "")
                                {
                                    content = "0";
                                }
                                objRetornar.inversionDLLS = Convert.ToDecimal(content);
                                objRetornar.inversionMXN = Convert.ToDecimal(content) * tipoCambio ;
                                break;
                            case 5: if (content == "")
                                {
                                    content = "0";
                                }
                                objRetornar.seguroDLLS = Convert.ToDecimal(content);
                                objRetornar.seguroMXN = Convert.ToDecimal(content) * tipoCambio ;
                                break;
                            case 6: if (content == "")
                                {
                                    content = "0";
                                }
                                objRetornar.filtroDLLS = Convert.ToDecimal(content);
                                objRetornar.filtroMXN = Convert.ToDecimal(content) * tipoCambio ;
                                break;
                            case 7: if (content == "")
                                {
                                    content = "0";
                                }
                                objRetornar.mantenimientoDLLS = Convert.ToDecimal(content);
                                objRetornar.mantenimientoMXN = Convert.ToDecimal(content) * tipoCambio ;
                                break;
                            case 8: if (content == "")
                                {
                                    content = "0";
                                }
                                objRetornar.manoObraDLLS = Convert.ToDecimal(content) / tipoCambio ;
                                objRetornar.manoObraMXN = Convert.ToDecimal(content) ;
                                break;
                            case 9: if (content == "")
                                {
                                    content = "0";
                                }
                                objRetornar.auxiliarDLLS = Convert.ToDecimal(content);
                                objRetornar.auxiliarMXN = Convert.ToDecimal(content) * tipoCambio ;
                                break;
                            case 10: if (content == "")
                                {
                                    content = "0";
                                }
                                objRetornar.indirectosDLLS = Convert.ToDecimal(content);
                                objRetornar.indirectosMXN = Convert.ToDecimal(content) * tipoCambio ;
                                break;
                            case 11: if (content == "")
                                {
                                    content = "0";
                                }
                                objRetornar.depreciacionOHDLLS = Convert.ToDecimal(content);
                                objRetornar.depreciacionOHMXN = Convert.ToDecimal(content) * tipoCambio ;
                                break;
                            case 12: if (content == "")
                                {
                                    content = "0";
                                }
                                objRetornar.aceiteDLLS = Convert.ToDecimal(content);
                                objRetornar.aceiteMXN = Convert.ToDecimal(content) * tipoCambio ;
                                break;
                            case 13: if (content == "")
                                {
                                    content = "0";
                                }
                                objRetornar.carilleriaDLLS = Convert.ToDecimal(content);
                                objRetornar.carilleriaMXN = Convert.ToDecimal(content) * tipoCambio ;
                                break;
                            case 14: if (content == "")
                                {
                                    content = "0";
                                }
                                objRetornar.ansulDLLS = Convert.ToDecimal(content);
                                objRetornar.ansulMXN = Convert.ToDecimal(content) * tipoCambio ;
                                break;
                            case 15: if (content == "")
                                {
                                    content = "0";
                                }
                                objRetornar.utilidadDLLS = Convert.ToDecimal(content);
                                objRetornar.utilidadMXN = Convert.ToDecimal(content) * tipoCambio ;
                                break;
                            case 16: if (content == "")
                                {
                                    content = "0";
                                }
                                objRetornar.costoDLLS = Convert.ToDecimal(content);
                                objRetornar.Agrupacion = "-";
                                objRetornar.costoMXN = Convert.ToDecimal(content) * tipoCambio ;
                                objRetornar.tipoHoraDia = 1;
                                break;
                            case 17: if (content == "")
                                {
                                    content = "0";
                                }
                                objRetornar.tipoHoraDia = Convert.ToInt32(content);
                                break;
                        }
                    }
                    lstReturn.Add(objRetornar);
                }
                var lstretorn = lstReturn.Where(r => r.lstCatGrupo.id != 0).ToList();
                return lstretorn;
            }
            catch (Exception e)
            {
                error = error;
                errorhorizontal = errorhorizontal;
                return null;
            }

        }

        public List<tblM_CaratulaConceptos> conceptosMoneda()
        {
            List<tblM_CaratulaConceptos> lst = _context.tblM_CaratulaConceptos.ToList();
            return lst;
        }

        #endregion

        #region GUARDAR MODELO
        public bool GuardarModelo(tblM_Caratulas parametros)
        {
            try
            {
                tblM_Caratulas obj = new tblM_Caratulas();
                if (parametros.idGrupo != 0)
                {
                    obj.idGrupo = parametros.idGrupo;
                    obj.idModelo = parametros.idModelo;
                    obj.depreciacion = parametros.depreciacion;
                    obj.inversion = parametros.inversion;
                    obj.seguro = parametros.seguro;
                    obj.filtros = parametros.filtros;
                    obj.mantenimientoCo = parametros.mantenimientoCo;
                    obj.manoObra = 0;
                    obj.auxiliar = parametros.auxiliar;
                    obj.indirectosMatriz = parametros.indirectosMatriz;
                    obj.depreciacionOH = parametros.depreciacionOH;
                    obj.aceite = parametros.aceite;
                    obj.carilleria = parametros.carilleria;
                    obj.ansul = parametros.ansul;
                    obj.utilidad = parametros.utilidad;
                    obj.costoTotal = parametros.costoTotal;
                    obj.esActivo = true;
                    obj.tipoHoraDia = parametros.tipoHoraDia;
                    _context.tblM_Caratulas.Add(obj);
                    _context.SaveChanges();
                }
                return true;
            }
            catch (Exception e)
            {
                return false;
                throw;
            }
        }
        #endregion

        #region MOSTRAR EN CARATULA NUEVO MODELO REGISTRADO
        public List<CaratulaGuardadoDTO> GetCaratula()
        {
            try
            {
                #region SE OBTIENE EL ID DEL ULTIMO MODELO REGISTRADO
                List<tblM_Caratulas> ultimoRegistro = _context.tblM_Caratulas.Where(x => x.esActivo).OrderByDescending(x => x.id).ToList();
                int idRegistro = ultimoRegistro.Count() > 0 ? idRegistro = ultimoRegistro.FirstOrDefault().id : 0;
                if (idRegistro == 0)
                    throw new Exception("Ocurrió un error.");
                #endregion
                List<tblM_Caratulas> lst = _context.tblM_Caratulas.Where(z => z.esActivo && z.id == idRegistro).ToList();


                List<CaratulaGuardadoDTO> lstCaratula = new List<CaratulaGuardadoDTO>();
                if (lst.Count() > 0)
                {
                    lstCaratula = lst.Where(x => x.id == x.id).ToList().Select(x => new CaratulaGuardadoDTO
                    {
                        id = x.id,
                        idModelo = convertir(x.idModelo),
                        Agrupacion = "-",
                        lstCatGrupo = _context.tblM_CatGrupoMaquinaria.Where(y => y.id == x.idGrupo).FirstOrDefault(),
                        lstCatModelo = _context.tblM_CatModeloEquipo.Where(y => y.id == x.idModelo).FirstOrDefault(),
                        depreciacionDLLS = x.depreciacion,
                        inversionDLLS = x.inversion,
                        seguroDLLS = x.seguro,
                        filtroDLLS = x.filtros,
                        mantenimientoDLLS = x.mantenimientoCo,
                        manoObraDLLS = x.manoObra,
                        auxiliarDLLS = x.auxiliar,
                        indirectosDLLS = x.indirectosMatriz,
                        depreciacionOHDLLS = x.depreciacionOH,
                        aceiteDLLS = x.aceite,
                        carilleriaDLLS = x.carilleria,
                        ansulDLLS = x.ansul,
                        utilidadDLLS = x.utilidad,
                        costoDLLS = x.costoTotal,
                        tipoHoraDia = x.tipoHoraDia
                    }).ToList();
                }

                return lstCaratula;
            }
            catch (Exception e)
            {
                LogError(2, 0, "CapturaController", "GetCaratula", e, AccionEnum.CONSULTA, 0, 0);
                return null;
            }
        }
        public List<int> convertir(int i)
        {
            List<int> d = new List<int>();
            d.Add(i);
            return d;
        }
        #endregion

        #region GUARDAR CARATULA
        public List<GuardarCaratulaDTO> retornarLista(List<CaratulaGuardadoDTO> listaCaratula)
        {
            List<GuardarCaratulaDTO> lst = new List<GuardarCaratulaDTO>();
            GuardarCaratulaDTO obj = new GuardarCaratulaDTO();

            foreach (var item in listaCaratula)
            {
                foreach (var item2 in item.idModelo)
                {
                    obj = new GuardarCaratulaDTO();
                    obj.idGrupo = item.lstCatGrupo.id;
                    obj.caratula = 0;
                    obj.idModelo = item2;
                    obj.depreciacionDLLS = item.depreciacionDLLS;
                    obj.depreciacionMXN = item.depreciacionMXN;
                    obj.inversionDLLS = item.inversionDLLS;
                    obj.inversionMXN = item.inversionMXN;
                    obj.seguroDLLS = item.seguroDLLS;
                    obj.seguroMXN = item.seguroMXN;
                    obj.filtrosDLLS = item.filtroDLLS;
                    obj.filtrosMXN = item.filtroMXN;
                    obj.mantenimientoDLLS = item.mantenimientoDLLS;
                    obj.mantenimientoMXN = item.mantenimientoMXN;
                    obj.manoObraDLLS = item.manoObraDLLS;
                    obj.manoObraMXN = item.manoObraMXN;
                    obj.auxiliarDLLS = item.auxiliarDLLS;
                    obj.auxiliarMXN = item.auxiliarMXN;
                    obj.indirectosDLLS = item.indirectosDLLS;
                    obj.indirectosMXN = item.indirectosMXN;
                    obj.depreciacionOHDLLS = item.depreciacionOHDLLS;
                    obj.depreciacionOHMXN = item.depreciacionOHMXN;
                    obj.aceiteDLLS = item.aceiteDLLS;
                    obj.aceiteMXN = item.aceiteMXN;
                    obj.carilleriaDLLS = item.carilleriaDLLS;
                    obj.carilleriaMXN = item.carilleriaMXN;
                    obj.ansulDLLS = item.ansulDLLS;
                    obj.ansulMXN = item.ansulMXN;
                    obj.utilidadDLLS = item.utilidadDLLS;
                    obj.utilidadMXN = item.utilidadMXN;
                    obj.costoDLLS = item.costoDLLS;
                    obj.costoMXN = item.costoMXN;
                    obj.tipoCambio = item.tipoCambio;
                    obj.tipoHoraDia = item.tipoHoraDia;
                    lst.Add(obj);
                }
            }
            return lst;
        }
        public bool GuardarCaratula(List<CaratulaGuardadoDTO> listaCaratula, decimal tipoCambio, int idTecnico, int idSubdireccionMaquinaria)
        {
            try
            {
                List<GuardarCaratulaDTO> listaCaratula2 = new List<GuardarCaratulaDTO>();
                listaCaratula2 = retornarLista(listaCaratula);
                #region CODIGO OLI

                tblM_CaratulaDet parametros = new tblM_CaratulaDet();
                tblM_CaratulaAut autorizaT = new tblM_CaratulaAut();
                tblM_CaratulaAut autorizaS = new tblM_CaratulaAut();
                tblM_Caratula padre = new tblM_Caratula();
                List<int> lstUsuariosAutorizantes = new List<int>();
                lstUsuariosAutorizantes.Add(idTecnico);
                lstUsuariosAutorizantes.Add(idSubdireccionMaquinaria);

                var lstUsuario = _context.tblP_Usuario.Where(x => x.estatus && lstUsuariosAutorizantes.Contains(x.id)).ToList();
                var o = _context.tblM_CaratulaDet.OrderByDescending(x => x.caratula).FirstOrDefault();
                var idUsuario = vSesiones.sesionUsuarioDTO.id;
                int id = 0;
                if (o == null)
                {
                    id = 1;
                }
                else
                {
                    id = o.caratula + 1;
                }
                foreach (var item in listaCaratula2)
                {
                    parametros.idGrupo = item.idGrupo;
                    parametros.caratula = id;
                    parametros.idModelo = item.idModelo;
                    parametros.depreciacionDLLS = item.depreciacionDLLS;
                    parametros.depreciacionMXN = item.depreciacionMXN;
                    parametros.inversionDLLS = item.inversionDLLS;
                    parametros.inversionMXN = item.inversionMXN;
                    parametros.seguroDLLS = item.seguroDLLS;
                    parametros.seguroMXN = item.seguroMXN;
                    parametros.filtroDLLS = item.filtrosDLLS;
                    parametros.filtroMXN = item.filtrosMXN;
                    parametros.mantenimientoDLLS = item.mantenimientoDLLS;
                    parametros.mantenimientoMXN = item.mantenimientoMXN;
                    parametros.manoObraDLLS = item.manoObraDLLS;
                    parametros.manoObraMXN = item.manoObraMXN;
                    parametros.auxiliarDLLS = item.auxiliarDLLS;
                    parametros.auxiliarMXN = item.auxiliarMXN;
                    parametros.indirectosDLLS = item.indirectosDLLS;
                    parametros.indirectosMXN = item.indirectosMXN;
                    parametros.depreciacionOHDLLS = item.depreciacionOHDLLS;
                    parametros.depreciacionOHMXN = item.depreciacionOHMXN;
                    parametros.aceiteDLLS = item.aceiteDLLS;
                    parametros.aceiteMXN = item.aceiteMXN;
                    parametros.carilleriaDLLS = item.carilleriaDLLS;
                    parametros.carilleriaMXN = item.carilleriaMXN;
                    parametros.ansulDLLS = item.ansulDLLS;
                    parametros.ansulMXN = item.ansulMXN;
                    parametros.utilidadDLLS = item.utilidadDLLS;
                    parametros.utilidadMXN = item.utilidadMXN;
                    parametros.costoDLLS = item.costoDLLS;
                    parametros.costoMXN = item.costoMXN;
                    parametros.idUsuarioTecnico = lstUsuario.Where(x => x.id == item.idusuarioTecnico).Select(x => Convert.ToInt32(x.cveEmpleado)).FirstOrDefault();
                    parametros.idUsuarioServicio = lstUsuario.Where(x => x.id == item.idusuarioServicio).Select(x => Convert.ToInt32(x.cveEmpleado)).FirstOrDefault();
                    parametros.idUsuarioConstruccion = lstUsuario.Where(x => x.id == item.idusuarioConstruccion).Select(x => Convert.ToInt32(x.cveEmpleado)).FirstOrDefault();
                    parametros.tipoCambio = item.tipoCambio;
                    parametros.estatus = 0;
                    parametros.fechaAutorizacion = DateTime.Now;
                    parametros.tipoHoraDia = item.tipoHoraDia;
                    _context.tblM_CaratulaDet.Add(parametros);
                    _context.SaveChanges();
                }
                #endregion

                tblP_Alerta alerta = new tblP_Alerta();
                alerta.moduloID = (int)BitacoraEnum.CARATULAS;
                alerta.userEnviaID = idUsuario;
                alerta.userRecibeID = idTecnico;
                alerta.tipoAlerta = 2;
                alerta.sistemaID = 1;
                alerta.objID = 0;
                alerta.visto = false;
                alerta.url = "/Caratulas/Autorizantes";
                alerta.msj = "Tienes una autorizacion pendiente de caratula";
                _context.tblP_Alerta.Add(alerta);
                _context.SaveChanges();

                var alertare = _context.tblP_Alerta.OrderByDescending(r => r.id).FirstOrDefault();
                autorizaT = new tblM_CaratulaAut();
                autorizaT.idCaratula = id;
                autorizaT.claveAutorizante = idUsuario;
                autorizaT.esAutorizado = true;
                autorizaT.estatus = 0;
                autorizaT.idAlerta = alertare.id;
                autorizaT.nombreAutorizante = lstUsuario.Where(x => x.id == idTecnico).Select(s => s.nombre + " " + s.apellidoPaterno + " " + s.apellidoMaterno).FirstOrDefault();
                autorizaT.idUsuarioTecnico = idTecnico;
                autorizaT.orden = 0;
                //if (autorizaS.idUsuarioTecnico == autorizaT.idUsuarioTecnico)
                //{
                //    throw new Exception("No se puede agregar el mismo autorizante");
                //}
                //if (autorizaC.idUsuarioTecnico == autorizaT.idUsuarioTecnico)
                //{
                //    throw new Exception("No se puede agregar el mismo autorizante");
                //}
                _context.tblM_CaratulaAut.Add(autorizaT);
                _context.SaveChanges();

                autorizaS = new tblM_CaratulaAut();
                autorizaS.idCaratula = id;
                autorizaS.claveAutorizante = idUsuario;
                autorizaS.estatus = 0;
                autorizaS.idAlerta = 0;
                autorizaS.esAutorizado = false;
                autorizaS.nombreAutorizante = lstUsuario.Where(x => x.id == idSubdireccionMaquinaria).Select(s => s.nombre + " " + s.apellidoPaterno + " " + s.apellidoMaterno).FirstOrDefault();
                autorizaS.idUsuarioTecnico = idSubdireccionMaquinaria;
                autorizaS.orden = 0;
                //if (autorizaT.idUsuarioTecnico == autorizaS.idUsuarioTecnico)
                //{
                //    throw new Exception("No se puede agregar el mismo autorizante");
                //}
                //if (autorizaC.idUsuarioTecnico == autorizaS.idUsuarioTecnico)
                //{
                //    throw new Exception("No se puede agregar el mismo autorizante");
                //}
                _context.tblM_CaratulaAut.Add(autorizaS);
                _context.SaveChanges();

                //autorizaC = new tblM_CaratulaAut();
                //autorizaC.idCaratula = id;
                //autorizaC.claveAutorizante = idUsuario;
                //autorizaC.estatus = 0;
                //autorizaC.idAlerta = 0;
                //autorizaC.esAutorizado = false;
                //autorizaC.nombreAutorizante = lstUsuario.Where(x => x.id == idConstruccion).Select(s => s.nombre + " " + s.apellidoPaterno + " " + s.apellidoMaterno).FirstOrDefault();
                //autorizaC.idUsuarioTecnico = idConstruccion;
                //autorizaC.orden = 0;
                ////if (autorizaT.idUsuarioTecnico == autorizaC.idUsuarioTecnico)
                ////{
                ////    throw new Exception("No se puede agregar el mismo autorizante");
                ////}
                ////if (autorizaS.idUsuarioTecnico == autorizaC.idUsuarioTecnico)
                ////{
                ////    throw new Exception("No se puede agregar el mismo autorizante");                    
                ////}
                //_context.tblM_CaratulaAut.Add(autorizaC);
                //_context.SaveChanges();

                padre.idCaratula = id;
                padre.autorizada = 0;
                padre.usuario = idUsuario;
                padre.tipoCambio = tipoCambio;
                _context.tblM_Caratula.Add(padre);
                _context.SaveChanges();

                return true;
            }
            catch (Exception o_O)
            {
                return false;
                throw;
            }
        }
        #endregion

        #region GUARDAR INDICADORES
        public List<IndicadoresCaratulaDTO> GetIndicadores()
        {
            try
            {
                List<tblM_IndicadoresCaratula> lst = _context.tblM_IndicadoresCaratula.ToList();
                List<tblP_CC> lstCC = _context.tblP_CC.ToList();

                List<IndicadoresCaratulaDTO> lstIndicadores = new List<IndicadoresCaratulaDTO>();
                if (lst.Count() > 0)
                {
                    lstIndicadores = lst.Where(x => x.id == x.id).ToList().Select(x => new IndicadoresCaratulaDTO
                    {
                        id = x.id,
                        idCC = x.idCC,
                        descripcion = "",
                        areaCuenta = "",
                        moneda = x.moneda,
                        Moneda = x.moneda ? "DLLS" : "MXN",
                        manoObra = x.manoObra,
                        manoobra = x.manoObra ? "Con mano de obra" : "Sin mano de obra",
                        auxiliar = x.auxiliar,
                        indirectos = x.indirectos
                    }).ToList();

                    foreach (var item in lstIndicadores)
                    {
                        var cc = lstCC.FirstOrDefault(x => x.id == item.idCC);
                        item.descripcion = cc.descripcion;
                        item.areaCuenta = cc.areaCuenta;
                        item.area = cc.area;
                        item.cuenta = cc.cuenta;
                    }
                }

                return lstIndicadores.OrderBy(x=>x.area).ThenBy(x=>x.cuenta).ToList();
            }
            catch (Exception e)
            {
                LogError(2, 0, "CapturaController", "GetCaratula", e, AccionEnum.CONSULTA, 0, 0);
                return null;
            }
        }

        public bool GuardarIndicadores(tblM_IndicadoresCaratula parametros)
        {
            try
            {
                tblM_IndicadoresCaratula obj = new tblM_IndicadoresCaratula();
                if (parametros.idCC != 0)
                {
                    obj.idCC = parametros.idCC;
                    obj.moneda = parametros.moneda;
                    obj.manoObra = parametros.manoObra;
                    obj.auxiliar = parametros.auxiliar;
                    obj.indirectos = parametros.indirectos;
                    _context.tblM_IndicadoresCaratula.Add(obj);
                    _context.SaveChanges();
                }
                return true;
            }
            catch (Exception)
            {
                return false;
                throw;
            }
        }


        public bool ActualizarIndicadoresNuevos(List<tblM_IndicadoresCaratula> lstNuevoIndicadores)
        {
            try
            {

                foreach (var item in lstNuevoIndicadores)
                {
                    var obj = _context.tblM_IndicadoresCaratula.Where(r => r.id == item.id).FirstOrDefault();

                    if (item != null)
                    {
                        obj.manoObra = item.manoObra;
                        obj.moneda = item.moneda;
                        obj.auxiliar = item.auxiliar;
                        obj.indirectos = item.indirectos;
                        _context.SaveChanges();

                    }
                }


                return true;
            }
            catch (Exception)
            {
                return false;
                throw;
            }
        }
        #endregion

        #region REPORTE CARATULA
        public List<ReporteCaratulaDTO> GetReporte(int idCaratula)
        {
            var registro = _context.tblM_Caratula.Where(r => r.idCaratula == idCaratula).FirstOrDefault();

            List<ReporteCaratulaDTO> data = new List<ReporteCaratulaDTO>();
            var tipoCambio = registro.tipoCambio;
            data = _context.tblM_CaratulaDet.Where(x => x.caratula == idCaratula).ToList().Select(x => new ReporteCaratulaDTO
                {
                    id = x.id,
                    equipo = _context.tblM_CatGrupoMaquinaria.Where(w => w.id == x.idGrupo).FirstOrDefault() == null ? "" : _context.tblM_CatGrupoMaquinaria.Where(w => w.id == x.idGrupo).FirstOrDefault().descripcion,
                    modelo = _context.tblM_CatModeloEquipo.Where(w => w.id == x.idModelo).FirstOrDefault() == null ? "" : _context.tblM_CatModeloEquipo.Where(w => w.id == x.idModelo).FirstOrDefault().descripcion,
                    depreciacionDLLS = x.depreciacionDLLS,
                    depreciacionMXN = x.depreciacionMXN,
                    inversionDLLS = x.inversionDLLS,
                    inversionMXN = x.inversionMXN,
                    seguroDLLS = x.seguroDLLS,
                    seguroMXN = x.seguroMXN,
                    filtroDLLS = x.filtroDLLS,
                    filtroMXN = x.filtroMXN,
                    mantenimientoDLLS = x.mantenimientoDLLS,
                    mantenimientoMXN = x.mantenimientoMXN,
                    manoObraDLLS = x.manoObraDLLS,
                    manoObraMXN = x.manoObraMXN,
                    auxiliarDLLS = x.auxiliarDLLS,
                    auxiliarMXN = x.auxiliarMXN,
                    indirectosDLLS = x.indirectosDLLS,
                    indirectosMXN = x.indirectosMXN,
                    depreciacionOHDLLS = x.depreciacionOHDLLS,
                    depreciacionOHMXN = x.depreciacionOHMXN,
                    aceiteDLLS = x.aceiteDLLS,
                    aceiteMXN = x.aceiteMXN,
                    carilleriaDLLS = x.carilleriaDLLS,
                    carilleriaMXN = x.carilleriaMXN,
                    ansulDLLS = x.ansulDLLS,
                    ansulMXN = x.ansulMXN,
                    utilidadDLLS = x.utilidadDLLS,
                    utilidadMXN = x.utilidadMXN,
                    costoDLLS = x.costoDLLS,
                    costoMXN = x.costoMXN,
                    tipoCambio = tipoCambio,
                    tipoHoraDia = x.tipoHoraDia == 1 ? "COSTO/ HORA":"COSTO/ DIA",
                }).ToList();
            return data;
        }
        #endregion

        #region AUTORIZAR Y RECHAZAR CARATULA
        public Dictionary<string, object> ListaAutorizantes(int idCaratula)
        {
            var result = new Dictionary<string, object>();
            var registro = _context.tblM_Caratula.Where(r => r.idCaratula == idCaratula).FirstOrDefault();

            if (registro == null)
            {
                List<authDTO> lst = new List<authDTO>();
                result.Add("autorizantes", lst);
                result.Add(SUCCESS, true);
            }
            else
            {
                try
                {
                    var caratula = _context.tblM_Caratula.FirstOrDefault(w => w.idCaratula == idCaratula);
                    var caratulaAut = _context.tblM_CaratulaAut.ToList().Where(w => w.idCaratula == idCaratula).ToList();
                    var esSucces = caratulaAut.Count > 0;
                    if (esSucces)
                    {
                        var clase = string.Empty;
                        var idUsuario = vSesiones.sesionUsuarioDTO.id;
                        var auxUsuario = _context.tblP_Usuario.FirstOrDefault(x => x.id == caratula.usuario);
                        var lst = caratulaAut.Select(a => new authDTO()
                        {
                            idRegistro = a.id,
                            idAuth = a.claveAutorizante,
                            idPadre = a.idCaratula,
                            orden = a.orden,
                            comentario = a.comentario ?? string.Empty,
                            descripcion = "",
                            firma = a.firma ?? string.Empty,
                            nombre = a.nombreAutorizante,
                            authEstado = a.esAutorizado && a.idUsuarioTecnico == idUsuario ? authEstadoEnum.EnTurno : (authEstadoEnum)a.estatus,
                            clase = a.esAutorizado && a.idUsuarioTecnico == idUsuario ? authEstadoEnum.EnTurno.GetDescription() : ((authEstadoEnum)a.estatus).GetDescription(),
                            siguiente = a.idAlerta,
                            estatus = a.estatus
                        }).OrderBy(x => x.orden).ToList();
                        result.Add("autorizantes", lst);
                        result.Add(MESSAGE, auxUsuario == null ? "" : string.Format("Capturó: {0} {1} {2}", auxUsuario.nombre, auxUsuario.apellidoPaterno, auxUsuario.apellidoMaterno));

                        //var resultadoComparacion = bonofs.ObtenerComparativaVersionesPlantilla(plan);
                        //result.Add("comparacion", resultadoComparacion);
                    }
                    result.Add(SUCCESS, esSucces);
                }
                catch (Exception e)
                {
                    result.Add(MESSAGE, e.Message);
                    result.Add(SUCCESS, false);
                }
            }
            return result;
        }



        public Dictionary<string, object> Autorizar(authDTO caratulas, int id)
        {
            var result = new Dictionary<string, object>();
            List<tblM_Caratula> ultimoRegistro = _context.tblM_Caratula.OrderByDescending(x => x.id).ToList();
            int idRegistro = ultimoRegistro.Count() > 0 ? idRegistro = ultimoRegistro.FirstOrDefault().idCaratula : 0;
            if (idRegistro == 0)
                throw new Exception("Ocurrió un error.");
            using (var dbTransaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var stAuth = (int)authEstadoEnum.Autorizado;
                    var tablaPadre = _context.tblM_Caratula.FirstOrDefault(x => x.idCaratula == idRegistro);
                    var detalles = _context.tblM_CaratulaAut.Where(x => x.idCaratula == idRegistro).ToList();
                    var det = detalles.FirstOrDefault(x => x.id == caratulas.idRegistro);
                    det.esAutorizado = false;
                    det.estatus = stAuth;
                    det.firma = GlobalUtils.CrearFirmaDigital(idRegistro, DocumentosEnum.Caratula, vSesiones.sesionUsuarioDTO.id);
                    det.fechaAutorizacion = DateTime.Now;
                    if (detalles.All(a => a.estatus == stAuth))
                    {
                        tablaPadre.autorizada = stAuth;
                        tablaPadre.fechaAutorizacion = det.fechaAutorizacion;
                    }
                    else
                    {
                        var sigAuth = detalles.FirstOrDefault(w => w.estatus == (int)authEstadoEnum.EnEspera);
                        sigAuth.esAutorizado = true;
                    }

                    var objAlerta = _context.tblP_Alerta.Where(r => r.id == det.idAlerta).FirstOrDefault();
                    if (objAlerta != null)
                    {
                        objAlerta.visto = true;
                        _context.SaveChanges();
                    }
                    List<tblM_CaratulaAut> lstAutorizantesulti = _context.tblM_CaratulaAut.Where(r => r.idCaratula == idRegistro && r.estatus == (int)authEstadoEnum.EnEspera).ToList();
                    var siguiente = lstAutorizantesulti.FirstOrDefault();

                    if (siguiente != null)
                    {
                        tblP_Alerta alerta = new tblP_Alerta();
                        alerta.moduloID = (int)BitacoraEnum.CARATULAS;
                        alerta.userEnviaID = id;
                        alerta.userRecibeID = siguiente.idUsuarioTecnico;
                        alerta.tipoAlerta = 2;
                        alerta.sistemaID = 1;
                        alerta.objID = 0;
                        alerta.visto = false;
                        alerta.url = "/Caratulas/Autorizantes";
                        alerta.msj = "Tienes una autorizacion pendiente de caratula";
                        _context.tblP_Alerta.Add(alerta);
                        _context.SaveChanges();
                    }
                    _context.SaveChanges();
                    if (siguiente != null)
                    {

                        #region SE OBTIENE ID LA ULTIMA ALERTA DE CARATULAS
                        int idUltimaAlerta = _context.tblP_Alerta.Where(w => w.tipoAlerta == 2 && w.sistemaID == 1 &&
                                                                             w.url == "/Caratulas/Autorizantes" && w.msj == "Tienes una autorizacion pendiente de caratula").OrderByDescending(o => o.id)
                                                                             .Select(s => s.id).FirstOrDefault();
                        #endregion

                        #region
                        tblM_CaratulaAut objAsignarAlertaID = _context.tblM_CaratulaAut.Where(w => w.idCaratula == idRegistro && w.idUsuarioTecnico == siguiente.idUsuarioTecnico && w.esAutorizado).FirstOrDefault();
                        objAsignarAlertaID.idAlerta = idUltimaAlerta;
                        _context.SaveChanges();
                        #endregion
                    }

                    dbTransaction.Commit();
                    result.Add(SUCCESS, true);
                }
                catch (Exception e)
                {
                    dbTransaction.Rollback();
                    var nombreFuncion = System.Reflection.MethodBase.GetCurrentMethod().Name;
                    LogError(0, 0, "", nombreFuncion, e, AccionEnum.ACTUALIZAR, 0, caratulas);
                    result.Add(MESSAGE, e.Message);
                    result.Add(SUCCESS, false);
                }
            }

            return result;
        }


        public Dictionary<string, object> Rechazar(authDTO caratulas)
        {
            var result = new Dictionary<string, object>();
            List<tblM_Caratula> ultimoRegistro = _context.tblM_Caratula.OrderByDescending(x => x.id).ToList();
            int idRegistro = ultimoRegistro.Count() > 0 ? idRegistro = ultimoRegistro.FirstOrDefault().idCaratula : 0;
            if (idRegistro == 0)
                throw new Exception("Ocurrió un error.");
            using (var dbTransaction = _context.Database.BeginTransaction())
            {
                try
                {
                    if (caratulas.comentario == null || caratulas.comentario.Trim().Length < 10)
                    {
                        result.Add(MESSAGE, "No se rechazó la solicitud. El comentario viene vacío.");
                        result.Add(SUCCESS, false);
                        return result;
                    }
                    var caratula = _context.tblM_Caratula.FirstOrDefault(x => x.idCaratula == idRegistro);
                    var detalles = _context.tblM_CaratulaAut.Where(x => x.idCaratula == idRegistro).ToList();
                    var det = detalles.FirstOrDefault(x => x.id == caratulas.idRegistro);


                    det.comentario = caratulas.comentario;
                    det.fechaAutorizacion = DateTime.Now;
                    det.estatus = caratula.autorizada = (int)authEstadoEnum.Rechazado;
                    det.esAutorizado = false;

                    _context.SaveChanges();
                    dbTransaction.Commit();
                    result.Add(SUCCESS, true);
                }
                catch (Exception e)
                {
                    dbTransaction.Rollback();
                    var nombreFuncion = System.Reflection.MethodBase.GetCurrentMethod().Name;
                    LogError(0, 0, "", nombreFuncion, e, AccionEnum.ACTUALIZAR, 0, caratulas);
                    result.Add(MESSAGE, e.Message);
                    result.Add(SUCCESS, false);
                }
            }

            return result;
        }
        #endregion

        #region ENVIAR CORREO A AUTORIZANTES
        public Dictionary<string, object> EnviarCorreo(List<Byte[]> downloadPDF)
        {

            var result = new Dictionary<string, object>();
            var usuarioEnvia = vSesiones.sesionUsuarioDTO;

            List<tblM_Caratula> ultimoRegistro = _context.tblM_Caratula.OrderByDescending(x => x.id).ToList();
            int idRegistro = ultimoRegistro.Count() > 0 ? idRegistro = ultimoRegistro.FirstOrDefault().idCaratula : 0;
            if (idRegistro == 0)
                throw new Exception("Ocurrió un error.");
            try
            {


                var usuarios = _context.tblP_Usuario.Where(x => x.estatus).ToList();
                var autorizantes = _context.tblM_CaratulaAut.Where(x => x.idCaratula == idRegistro).ToList();
                var autoriza = _context.tblM_CaratulaAut.Where(x => x.idCaratula == idRegistro).FirstOrDefault();


                List<string> correo = new List<string>();

#if DEBUG
                foreach (var item in autorizantes)
                {
                    correo.Add(usuarios.Where(x => x.id == item.idUsuarioTecnico).Select(x => x.correo).FirstOrDefault());
                }
                //correo.Add("luis.olivarria@construplan.com.mx");
#else
                foreach (var item in autorizantes)
                {
                    correo.Add(usuarios.Where(x => x.id == item.idUsuarioTecnico).Select(x => x.correo).FirstOrDefault());
                }
#endif

                var asunto = "Caratula Arrendadora";
                string cuerpoCorreo = "";

                cuerpoCorreo += "<html><head><style>table {font-family: arial, sans-serif;border-collapse: collapse;width: 100%;}td, th {border: 1px solid #dddddd;text-align: left;padding: 8px;}";
                cuerpoCorreo += " tr:nth-child(even) {background-color: #dddddd;} .autorizado{background-color: #008f39 } .rechazado{background-color: #FF0000 } .enEspera{background-color: #FFFFFF }</style>";
                cuerpoCorreo += " </head>";
                cuerpoCorreo += " <body lang=ES-MX link='#0563C1' vlink='#954F72'>";
                cuerpoCorreo += "<div class=WordSection1>";
                if (autoriza.estatus == (int)authEstadoEnum.EnEspera)
                {
                    cuerpoCorreo += "<p class=MsoNormal>Lista de los autorizantes para la caratula<o:p></o:p></p>";
                }
                else
                {
                    if (autoriza.estatus == (int)authEstadoEnum.Autorizado)
                    {
                        cuerpoCorreo += "<p class=MsoNormal>Se registró una autorización de la caratula de: " + usuarioEnvia.nombre + " " + usuarioEnvia.apellidoPaterno + " " + usuarioEnvia.apellidoMaterno + ".<o:p></o:p></p>";
                    }
                }
                cuerpoCorreo += "<table id='tblM_AutorizanteAdquisicion' class='table-bordered hover stripe order-column dataTable no-footer' role='grid' aria-describedby='tblM_AutorizanteAdquisicion_info' style='width: 0px;'>";
                cuerpoCorreo += " <thead>";
                cuerpoCorreo += "<tr role='row'>";
                cuerpoCorreo += "<th class='sorting_disabled' rowspan='1' colspan='1' style='width: 0px;'>Nombre</th>";
                cuerpoCorreo += "<th class='sorting_disabled' rowspan='1' colspan='1' style='width: 0px;'>Estado</th>";
                cuerpoCorreo += "<th class='sorting_disabled' rowspan='1' colspan='1' style='width: 0px;'>Fecha Autorizacion</th>";
                cuerpoCorreo += "<th class='sorting_disabled' rowspan='1' colspan='1' style='width: 0px;'>Comentario de rechazo</th>";
                cuerpoCorreo += "</tr>";
                cuerpoCorreo += "</thead>";
                foreach (var item in autorizantes)
                {
                    string statussss = item.estatus == 1 ? "Autorizado" : item.estatus == 2 ? "Rechazado" : "En espera";
                    cuerpoCorreo += "<tr>";
                    cuerpoCorreo += "<td>" + item.nombreAutorizante + "</td>";
                    if (item.estatus == (int)authEstadoEnum.Autorizado)
                    {
                        cuerpoCorreo += "<td class='autorizado'>" + statussss + "</td>";
                    }
                    else
                    {
                        if (item.estatus == (int)authEstadoEnum.Rechazado)
                        {
                            cuerpoCorreo += "<td class='rechazado'>" + statussss + "</td>";
                        }
                        else
                        {
                            if (item.estatus == (int)authEstadoEnum.EnEspera)
                            {
                                cuerpoCorreo += "<td class='enEspera'>" + statussss + "</td>";
                            }
                        }
                    }
                    cuerpoCorreo += "<td>" + item.fechaAutorizacion + "</td>";
                    cuerpoCorreo += "<td>" + item.comentario + "</td>";
                    cuerpoCorreo += "</tr>";
                }
                cuerpoCorreo += "</table>";
                cuerpoCorreo += " <p class=MsoNormal>";
                cuerpoCorreo += "Favor de ingresar al sistema <a href='http://sigoplan.construplan.com.mx/'>SIGOPLAN</a>,";
                cuerpoCorreo += " en el apartado de Maquinaria, menú captura, submenú por evento, en el apartado de caratulas, en autorizar.<o:p></o:p>";
                cuerpoCorreo += "</p>";
                cuerpoCorreo += "<p class=MsoNormal>";
                cuerpoCorreo += "También puede acceder ingresando normalmente al sistema y dando clic en la notificación correspondiente.<o:p></o:p>";
                cuerpoCorreo += "</p>";
                cuerpoCorreo += "<p class=MsoNormal>";
                cuerpoCorreo += "PD. Se informa que esta notificación es autogenerada por el sistema SIGOPLAN y no es necesario dar una respuesta.<o:p></o:p>";
                cuerpoCorreo += "</p>";
                cuerpoCorreo += "<p class=MsoNormal>";
                cuerpoCorreo += "Gracias.<o:p></o:p>";
                cuerpoCorreo += "</p>";
                cuerpoCorreo += "</div>";
                cuerpoCorreo += "</body>";
                cuerpoCorreo += "</html>";

                GlobalUtils.sendEmailAdjuntoInMemorySend(string.Format("{0}: {1}", PersonalUtilities.GetNombreEmpresa(), asunto), cuerpoCorreo, correo, downloadPDF, "Reporte Caratula");


                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return result;
        }
        #endregion

        #region ENVIAR CORREO AL GUARDAR LA CARATULA
        public Dictionary<string, object> EnviarCorreoGuardarCaratula(List<Byte[]> downloadPDF)
        {

            var result = new Dictionary<string, object>();
            var usuarioEnvia = vSesiones.sesionUsuarioDTO;

            List<tblM_Caratula> ultimoRegistro = _context.tblM_Caratula.OrderByDescending(x => x.id).ToList();
            int idRegistro = ultimoRegistro.Count() > 0 ? idRegistro = ultimoRegistro.FirstOrDefault().idCaratula : 0;
            if (idRegistro == 0)
                throw new Exception("Ocurrió un error.");
            try
            {
                var usuarios = _context.tblP_Usuario.Where(x => x.estatus).ToList();
                var autorizantes = _context.tblM_CaratulaAut.Where(x => x.idCaratula == idRegistro).ToList();
                List<string> correo = new List<string>();

#if DEBUG
                foreach (var item in autorizantes)
                {
                    correo.Add(usuarios.Where(x => x.id == item.idUsuarioTecnico).Select(x => x.correo).FirstOrDefault());
                }
                //correo.Add("adan.gonzalez@construplan.com.mx");
#else
                foreach (var item in autorizantes)
                {
                    correo.Add(usuarios.Where(x => x.id == item.idUsuarioTecnico).Select(x => x.correo).FirstOrDefault());
                }
#endif

                var asunto = "Caratula Arrendadora";
                string cuerpoCorreo = "";
                cuerpoCorreo += "<html><head><style>table {font-family: arial, sans-serif;border-collapse: collapse;width: 100%;}td, th {border: 1px solid #dddddd;text-align: left;padding: 8px;}";
                cuerpoCorreo += " tr:nth-child(even) {background-color: #dddddd;} .autorizado{background-color: #008f39 } .rechazado{background-color: #FF0000 } .enEspera{background-color: #FFFFFF }</style>";
                cuerpoCorreo += " </head>";
                cuerpoCorreo += " <body lang=ES-MX link='#0563C1' vlink='#954F72'>";
                cuerpoCorreo += "<div class=WordSection1>";
                cuerpoCorreo += "<p class=MsoNormal>Lista de los autorizantes para la caratula<o:p></o:p></p>";
                cuerpoCorreo += "<table id='tblM_AutorizanteAdquisicion' class='table-bordered hover stripe order-column dataTable no-footer' role='grid' aria-describedby='tblM_AutorizanteAdquisicion_info' style='width: 0px;'>";
                cuerpoCorreo += " <thead>";
                cuerpoCorreo += "<tr role='row'>";
                cuerpoCorreo += "<th class='sorting_disabled' rowspan='1' colspan='1' style='width: 0px;'>Nombre</th>";
                cuerpoCorreo += "<th class='sorting_disabled' rowspan='1' colspan='1' style='width: 0px;'>Estado</th>";
                cuerpoCorreo += "<th class='sorting_disabled' rowspan='1' colspan='1' style='width: 0px;'>Fecha Autorizacion</th>";
                cuerpoCorreo += "<th class='sorting_disabled' rowspan='1' colspan='1' style='width: 0px;'>Comentario de rechazo</th>";
                cuerpoCorreo += "</tr>";
                cuerpoCorreo += "</thead>";
                foreach (var item in autorizantes)
                {
                    string statussss = item.estatus == 1 ? "Autorizado" : item.estatus == 2 ? "Rechazado" : "En espera";
                    cuerpoCorreo += "<tr>";
                    cuerpoCorreo += "<td>" + item.nombreAutorizante + "</td>";
                    if (item.estatus == (int)authEstadoEnum.Autorizado)
                    {
                        cuerpoCorreo += "<td class='autorizado'>" + statussss + "</td>";
                    }
                    else
                    {
                        if (item.estatus == (int)authEstadoEnum.Rechazado)
                        {
                            cuerpoCorreo += "<td class='rechazado'>" + statussss + "</td>";
                        }
                        else
                        {
                            if (item.estatus == (int)authEstadoEnum.EnEspera)
                            {
                                cuerpoCorreo += "<td class='enEspera'>" + statussss + "</td>";
                            }
                        }
                    }
                    cuerpoCorreo += "<td>" + item.fechaAutorizacion + "</td>";
                    cuerpoCorreo += "<td>" + item.comentario + "</td>";
                    cuerpoCorreo += "</tr>";
                }
                cuerpoCorreo += "</table>";
                cuerpoCorreo += " <p class=MsoNormal>";
                cuerpoCorreo += "Favor de ingresar al sistema <a href='http://sigoplan.construplan.com.mx/'>SIGOPLAN</a>,";
                cuerpoCorreo += " en el apartado de Maquinaria, menú captura, submenú por evento, en el apartado de caratulas, en autorizar.<o:p></o:p>";
                cuerpoCorreo += "</p>";
                cuerpoCorreo += "<p class=MsoNormal>";
                cuerpoCorreo += "También puede acceder ingresando normalmente al sistema y dando clic en la notificación correspondiente.<o:p></o:p>";
                cuerpoCorreo += "</p>";
                cuerpoCorreo += "<p class=MsoNormal>";
                cuerpoCorreo += "PD. Se informa que esta notificación es autogenerada por el sistema SIGOPLAN y no es necesario dar una respuesta.<o:p></o:p>";
                cuerpoCorreo += "</p>";
                cuerpoCorreo += "<p class=MsoNormal>";
                cuerpoCorreo += "Gracias.<o:p></o:p>";
                cuerpoCorreo += "</p>";
                cuerpoCorreo += "</div>";
                cuerpoCorreo += "</body>";
                cuerpoCorreo += "</html>";

                GlobalUtils.sendEmailAdjuntoInMemorySend(string.Format("{0}: {1}", PersonalUtilities.GetNombreEmpresa(), asunto), cuerpoCorreo, correo, downloadPDF, "Reporte Caratula");
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return result;
        }
        #endregion

        #region AUTOCARGADO DE LA CARATULA AUTORIZADA
        public Dictionary<string, object> CargarCaratulaActiva(List<int> lstTipoHoraDia)
        {

            var result = new Dictionary<string, object>();
            try
            {
                decimal tipoCambio = 1;
                var conceptos = _context.tblM_CaratulaConceptos.ToList();

                var caratulasActivas = _context.tblM_Caratula.Where(x => x.autorizada == 1).OrderByDescending(x => x.fechaAutorizacion).ToList();
                if (caratulasActivas.Count() > 0)
                {
                    var caratula = caratulasActivas.FirstOrDefault();
                    tipoCambio = caratula.tipoCambio;

                    List<tblM_CaratulaDet> detalles = new List<tblM_CaratulaDet>();
                    if (lstTipoHoraDia == null)
                        detalles = _context.tblM_CaratulaDet.Where(x => x.caratula == caratula.idCaratula).ToList();
                    else
                        detalles = _context.tblM_CaratulaDet.Where(x => x.caratula == caratula.idCaratula && lstTipoHoraDia.Contains(x.tipoHoraDia)).ToList();

                    var gruposIDs = detalles.Select(x => x.idGrupo).Distinct().ToList();
                    var modelosIDs = detalles.Select(x => x.idModelo).Distinct().ToList();
                    var grupos = _context.tblM_CatGrupoMaquinaria.Where(y => gruposIDs.Contains(y.id)).ToList();
                    var modelos = _context.tblM_CatModeloEquipo.Where(y => modelosIDs.Contains(y.id)).ToList();
                    var lstAgrupaciones = _context.tblM_CaratulaAgrupacionEnc.Where(r => r.esActivo).ToList();
                    List<tblM_CaratulaAgrupacionDet> lstCaratulaAgrupacionDet = _context.tblM_CaratulaAgrupacionDet.ToList();
                    List<tblM_CatModeloEquipo> lstCatModeloEquipo = _context.tblM_CatModeloEquipo.ToList();

                    var data = detalles.Select(x => new
                        {
                            id = x.id,
                            idGrupo = x.idGrupo,
                            idModelo = x.idModelo,
                            lstCatModelo = x.lstCatModelo,
                            lstCatGrupo = x.lstCatGrupo,
                            Agrupacion = lstAgrupaciones.Where(r => r.id == regresarid(x.idGrupo, x.idModelo, lstCaratulaAgrupacionDet)).FirstOrDefault() == null ? x.lstCatGrupo.descripcion+ " - " +x.lstCatModelo.descripcion : lstAgrupaciones.Where(r => r.id == regresarid(x.idGrupo, x.idModelo, lstCaratulaAgrupacionDet)).FirstOrDefault().AgrupacionCaratula,
                            idAgrupacion = lstAgrupaciones.Where(r => r.id == regresarid(x.idGrupo, x.idModelo, lstCaratulaAgrupacionDet)).FirstOrDefault() == null ? 0 : lstAgrupaciones.Where(r => r.id == regresarid(x.idGrupo, x.idModelo, lstCaratulaAgrupacionDet)).FirstOrDefault().id,
                            depreciacionDLLS = x.depreciacionDLLS,
                            inversionDLLS = x.inversionDLLS,
                            seguroDLLS = x.seguroDLLS,
                            filtroDLLS = x.filtroDLLS,
                            mantenimientoDLLS = x.mantenimientoDLLS,
                            manoObraDLLS = x.manoObraDLLS,
                            auxiliarDLLS = x.auxiliarDLLS,
                            indirectosDLLS = x.indirectosDLLS,
                            depreciacionOHDLLS = x.depreciacionOHDLLS,
                            aceiteDLLS = x.aceiteDLLS,
                            carilleriaDLLS = x.carilleriaDLLS,
                            ansulDLLS = x.ansulDLLS,
                            utilidadDLLS = x.utilidadDLLS,
                            costoDLLS = x.depreciacionDLLS + x.inversionDLLS + x.seguroDLLS + x.filtroDLLS + x.mantenimientoDLLS + x.manoObraDLLS + x.auxiliarDLLS + x.indirectosDLLS + x.depreciacionOHDLLS + x.aceiteDLLS + x.carilleriaDLLS + x.ansulDLLS + x.utilidadDLLS,
                            depreciacionMXN = x.depreciacionMXN,
                            inversionMXN = x.inversionMXN,
                            seguroMXN = x.seguroMXN,
                            filtroMXN = x.filtroMXN,
                            mantenimientoMXN = x.mantenimientoMXN,
                            manoObraMXN = x.manoObraMXN,
                            auxiliarMXN = x.auxiliarMXN,
                            indirectosMXN = x.indirectosMXN,
                            depreciacionOHMXN = x.depreciacionOHMXN,
                            aceiteMXN = x.aceiteMXN,
                            carilleriaMXN = x.carilleriaMXN,
                            ansulMXN = x.ansulMXN,
                            utilidadMXN = x.utilidadMXN,
                            costoMXN = x.depreciacionMXN + x.inversionMXN + x.seguroMXN + x.filtroMXN + x.mantenimientoMXN + x.manoObraMXN + x.auxiliarMXN + x.indirectosMXN + x.depreciacionOHMXN + x.aceiteMXN + x.carilleriaMXN + x.ansulMXN + x.utilidadMXN,
                            estatus = 1,
                            tipoHoraDia = x.tipoHoraDia
                        }).ToList();
                    var lst = data.GroupBy(y => y.Agrupacion).ToList();

                    List<CaratulaGuardadoDTO> lstCaratulasGuardado = new List<CaratulaGuardadoDTO>();
                    CaratulaGuardadoDTO objCaratulasGuardado = new CaratulaGuardadoDTO();

                    foreach (var item in lst)
                    {
                        objCaratulasGuardado = new CaratulaGuardadoDTO();
                        objCaratulasGuardado.id = item.Select(y => y.id).FirstOrDefault();
                        objCaratulasGuardado.lstCatGrupo = item.Select(y => y.lstCatGrupo).FirstOrDefault();
                        objCaratulasGuardado.lstCatModelo = item.Select(y => y.lstCatModelo).FirstOrDefault();
                        objCaratulasGuardado.idModelo = item.Select(y => y.idModelo).ToList();
                        objCaratulasGuardado.stringModelo = retornarmodelosstring(item.Select(y => y.idModelo).ToList(), lstCatModeloEquipo);
                        objCaratulasGuardado.Agrupacion = item.Select(y => y.idAgrupacion).FirstOrDefault() == 0 ? "-" : item.Select(y => y.Agrupacion).FirstOrDefault();
                        objCaratulasGuardado.idAgrupacion = item.Select(y => y.idAgrupacion).FirstOrDefault();
                        objCaratulasGuardado.depreciacionDLLS = item.Select(y => y.depreciacionDLLS).FirstOrDefault();
                        objCaratulasGuardado.depreciacionMXN = item.Select(y => y.depreciacionMXN).FirstOrDefault();
                        objCaratulasGuardado.inversionDLLS = item.Select(y => y.inversionDLLS).FirstOrDefault();
                        objCaratulasGuardado.inversionMXN = item.Select(y => y.inversionMXN).FirstOrDefault();
                        objCaratulasGuardado.seguroDLLS = item.Select(y => y.seguroDLLS).FirstOrDefault();
                        objCaratulasGuardado.seguroMXN = item.Select(y => y.seguroMXN).FirstOrDefault();
                        objCaratulasGuardado.filtroDLLS = item.Select(y => y.filtroDLLS).FirstOrDefault();
                        objCaratulasGuardado.filtroMXN = item.Select(y => y.filtroMXN).FirstOrDefault();
                        objCaratulasGuardado.mantenimientoDLLS = item.Select(y => y.mantenimientoDLLS).FirstOrDefault();
                        objCaratulasGuardado.mantenimientoMXN = item.Select(y => y.mantenimientoMXN).FirstOrDefault();
                        objCaratulasGuardado.manoObraDLLS = item.Select(y => y.manoObraDLLS).FirstOrDefault();
                        objCaratulasGuardado.manoObraMXN = item.Select(y => y.manoObraMXN).FirstOrDefault();
                        objCaratulasGuardado.auxiliarDLLS = item.Select(y => y.auxiliarDLLS).FirstOrDefault();
                        objCaratulasGuardado.auxiliarMXN = item.Select(y => y.auxiliarMXN).FirstOrDefault();
                        objCaratulasGuardado.indirectosDLLS = item.Select(y => y.indirectosDLLS).FirstOrDefault();
                        objCaratulasGuardado.indirectosMXN = item.Select(y => y.indirectosMXN).FirstOrDefault();
                        objCaratulasGuardado.depreciacionOHDLLS = item.Select(y => y.depreciacionOHDLLS).FirstOrDefault();
                        objCaratulasGuardado.depreciacionOHMXN = item.Select(y => y.depreciacionOHMXN).FirstOrDefault();
                        objCaratulasGuardado.aceiteDLLS = item.Select(y => y.aceiteDLLS).FirstOrDefault();
                        objCaratulasGuardado.aceiteMXN = item.Select(y => y.aceiteMXN).FirstOrDefault();
                        objCaratulasGuardado.carilleriaDLLS = item.Select(y => y.carilleriaDLLS).FirstOrDefault();
                        objCaratulasGuardado.carilleriaMXN = item.Select(y => y.carilleriaMXN).FirstOrDefault();
                        objCaratulasGuardado.ansulDLLS = item.Select(y => y.ansulDLLS).FirstOrDefault();
                        objCaratulasGuardado.ansulMXN = item.Select(y => y.ansulMXN).FirstOrDefault();
                        objCaratulasGuardado.utilidadDLLS = item.Select(y => y.utilidadDLLS).FirstOrDefault();
                        objCaratulasGuardado.utilidadMXN = item.Select(y => y.utilidadMXN).FirstOrDefault();
                        objCaratulasGuardado.costoDLLS = item.Select(y => y.costoDLLS).FirstOrDefault();
                        objCaratulasGuardado.costoMXN = item.Select(y => y.costoMXN).FirstOrDefault();
                        objCaratulasGuardado.estatus = item.Select(y => y.estatus).FirstOrDefault();
                        objCaratulasGuardado.tipoHoraDia = item.Select(y => y.tipoHoraDia).FirstOrDefault();
                        lstCaratulasGuardado.Add(objCaratulasGuardado);
                    }






                    result.Add("data", lstCaratulasGuardado);
                    result.Add("tipoCambio", tipoCambio);
                    result.Add("conceptosMoneda", conceptos);
                    result.Add(SUCCESS, true);
                }




            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return result;
        }
        public List<string> retornarmodelosstring(List<int> modelos, List<tblM_CatModeloEquipo> lstCatModeloEquipo = null)
        {
            List<string> retor = new List<string>();
            List<tblM_CatModeloEquipo> lst = new List<tblM_CatModeloEquipo>();
            try
            {
                if (lstCatModeloEquipo != null)
                {
                    lst = lstCatModeloEquipo.Where(r => modelos.Contains(r.id)).ToList();
                    retor = lstCatModeloEquipo.Where(r => modelos.Contains(r.id)).Select(n => n.descripcion).ToList();
                }
                else
                {
                    lst = _context.tblM_CatModeloEquipo.Where(r => modelos.Contains(r.id)).ToList();
                    retor = _context.tblM_CatModeloEquipo.Where(r => modelos.Contains(r.id)).Select(n => n.descripcion).ToList();
                }

                if (lst != null)
                {
                    if (lstCatModeloEquipo != null)
                        retor = lstCatModeloEquipo.Where(r => modelos.Contains(r.id)).Select(n => n.descripcion).ToList();
                    else
                        retor = _context.tblM_CatModeloEquipo.Where(r => modelos.Contains(r.id)).Select(n => n.descripcion).ToList();
                }
            }
            catch (Exception e)
            {
                LogError(0, 0, "CaratulasController", "retornarmodelosstring", e, AccionEnum.CONSULTA, 0, 0);
            }

            return retor;
        }
        public int regresarid(int idGrupo, int idModelo, List<tblM_CaratulaAgrupacionDet> lstCaratulaAgrupacionDet = null)
        {
            int a = 0;
            try
            {
                tblM_CaratulaAgrupacionDet obj = new tblM_CaratulaAgrupacionDet();
                if (lstCaratulaAgrupacionDet != null)
                    obj = lstCaratulaAgrupacionDet.Where(f => f.idModelo == idModelo && f.idGrupo == idGrupo && f.esActivo).FirstOrDefault();
                else
                    obj = _context.tblM_CaratulaAgrupacionDet.Where(f => f.idModelo == idModelo && f.idGrupo == idGrupo && f.esActivo).FirstOrDefault();

                if (obj != null)
                {
                    if (lstCaratulaAgrupacionDet != null) 
                        a = lstCaratulaAgrupacionDet.Where(f => f.idModelo == idModelo && f.idGrupo == idGrupo && f.esActivo).FirstOrDefault().idAgrupacion;
                    else
                        a = _context.tblM_CaratulaAgrupacionDet.Where(f => f.idModelo == idModelo && f.idGrupo == idGrupo && f.esActivo).FirstOrDefault().idAgrupacion;
                }
            }
            catch (Exception e)
            {
                LogError(0, 0, "CaratulasController", "regresarid", e, AccionEnum.CONSULTA, 0, 0);
            }

            return a;
        }

        #endregion
        public bool ObtenerAutorizante(int id)
        {
            bool Autorizante = false;

            tblM_CaratulaAut autoriza = new tblM_CaratulaAut();
            try
            {

                autoriza = _context.tblM_CaratulaAut.Where(r => r.idUsuarioTecnico == id && r.esAutorizado).FirstOrDefault();
                if (autoriza == null)
                {
                    return Autorizante = false;
                }
                else
                {
                    return Autorizante = true;
                }
            }
            catch (Exception)
            {
                return Autorizante;
            }
        }


        public List<ComboDTO> obtenerComboCaratulras()
        {
            List<ComboDTO> lstCaratulas = new List<ComboDTO>();
            try
            {
                var obj = _context.tblM_Caratula.OrderByDescending(r => r.id).FirstOrDefault();
                lstCaratulas = _context.tblM_Caratula.Where(r => r.id == obj.id).ToList().Select(y => new ComboDTO
                {
                    Value = y.id.ToString(),
                    Text = "Version Caratula " + y.idCaratula.ToString()
                }).ToList();
            }
            catch (Exception)
            {
                throw;
            }
            return lstCaratulas;
        }
        public List<ComboDTO> obtenerCC()
        {
            List<ComboDTO> lstcc = new List<ComboDTO>();
            try
            {
                var indicadores = _context.tblM_IndicadoresCaratula.Select(y => y.idCC).ToList();
                lstcc = _context.tblP_CC.Where(r => indicadores.Contains(r.id)).ToList().Select(y => new ComboDTO
                {
                    Value = y.id.ToString(),
                    Text = y.areaCuenta + " " + y.descripcion
                }).ToList();
            }
            catch (Exception)
            {
                throw;
            }
            return lstcc;
        }


        public Dictionary<string, object> obtenerCaratula(int idCaratula, int idCC, int status, int esHoraDia)
        {
            var result = new Dictionary<string, object>();
            try
            {

                List<CaratulasDTO> data = new List<CaratulasDTO>();
                decimal tipoCambio = 1;
                var conceptos = _context.tblM_CaratulaConceptos.ToList();


                var objIndicador = _context.tblM_IndicadoresCaratula.Where(r => r.idCC == idCC).FirstOrDefault();
                var cc = _context.tblP_CC.FirstOrDefault(x=>x.id == idCC);
                if (objIndicador != null)
                {
                    var caratulasActivas = _context.tblM_Caratula.Where(x => x.id == idCaratula).ToList();
                    if (caratulasActivas.Count() > 0)
                    {
                        var caratula = caratulasActivas.FirstOrDefault();
                        tipoCambio = caratula.tipoCambio;
                        var detalles = _context.tblM_CaratulaDet.Where(x => x.caratula == caratula.idCaratula).ToList();
                        var gruposIDs = detalles.Select(x => x.idGrupo).Distinct().ToList();
                        var modelosIDs = detalles.Select(x => x.idModelo).Distinct().ToList();
                        var grupos = _context.tblM_CatGrupoMaquinaria.Where(y => gruposIDs.Contains(y.id)).ToList();
                        var modelos = _context.tblM_CatModeloEquipo.Where(y => modelosIDs.Contains(y.id)).ToList();
                        var maquinas = _context.tblM_CatMaquina.Where(x => x.centro_costos.Equals(cc.areaCuenta)).Select(x => new { grupoMaquinariaID = x.grupoMaquinariaID, modeloEquipoID = x.modeloEquipoID }).Distinct().ToList();
                        var moneda = objIndicador != null ? objIndicador.moneda? 2 : 1 : 0;
                        foreach (var i in maquinas)
                        {
                            var x = detalles.FirstOrDefault(y=>y.idGrupo == i.grupoMaquinariaID && y.idModelo == i.modeloEquipoID);
                            var grupo = grupos.FirstOrDefault(y => y.id == i.grupoMaquinariaID);
                            var modelo = modelos.FirstOrDefault(y => y.id == i.modeloEquipoID);
                            if (x != null)
                            {

                                var o = new CaratulasDTO();
                                o.idGrupo = x.idGrupo;
                                o.idModelo = x.idModelo;
                                o.grupo = grupo == null ? "" : grupo.descripcion;
                                o.modelo = modelo == null ? "" : modelo.descripcion;
                                o.depreciacion = moneda == 2 ? x.depreciacionDLLS : x.depreciacionMXN;
                                o.inversion = moneda == 2 ? x.inversionDLLS : x.inversionMXN;
                                o.seguro = moneda == 2 ? x.seguroDLLS : x.seguroMXN;
                                o.filtros = moneda == 2 ? x.filtroDLLS : x.filtroMXN;
                                o.mantenimientoCo = moneda == 2 ? x.mantenimientoDLLS : x.mantenimientoMXN;
                                o.manoObra = moneda == 2 ? x.manoObraDLLS : x.manoObraMXN;
                                o.auxiliar = moneda == 2 ? x.auxiliarDLLS : x.auxiliarMXN;
                                o.indirectosMatriz = moneda == 2 ? x.indirectosDLLS : x.indirectosMXN;
                                o.depreciacionOH = moneda == 2 ? x.depreciacionDLLS : x.depreciacionMXN;
                                o.aceite = moneda == 2 ? x.aceiteDLLS : x.aceiteMXN;
                                o.carilleria = moneda == 2 ? x.carilleriaDLLS : x.carilleriaMXN;
                                o.ansul = moneda == 2 ? x.ansulDLLS : x.ansulMXN;
                                o.utilidad = moneda == 2 ? x.utilidadDLLS : x.utilidadMXN;
                                o.costoTotal = moneda == 2 ? x.costoDLLS : x.costoMXN;
                                o.tipoHoraDia = x.tipoHoraDia == 1 ? "HORA" : "DIA";
                                o.esActivo = true;
                                o.IndicadorManoObra = objIndicador == null ? true : objIndicador.manoObra;
                                o.IndicadorTipoMoneda = objIndicador == null ? true : objIndicador.moneda;
                                o.IndicadorAxuliar = objIndicador == null ? 1 : objIndicador.auxiliar;
                                o.IndicadorIndirectos = objIndicador == null ? 1 : objIndicador.indirectos;
                                o.esHoraDia = (int)x.tipoHoraDia;
                                data.Add(o);
                            }
                            else {
                                var o = new CaratulasDTO();
                                o.idGrupo = i.grupoMaquinariaID;
                                o.idModelo = i.modeloEquipoID;
                                o.grupo = grupo == null ? "" : grupo.descripcion;
                                o.modelo = modelo == null ? "" : modelo.descripcion;
                                o.depreciacion = 0;
                                o.inversion = 0;
                                o.seguro = 0;
                                o.filtros = 0;
                                o.mantenimientoCo = 0;
                                o.manoObra = 0;
                                o.auxiliar = 0;
                                o.indirectosMatriz = 0;
                                o.depreciacionOH = 0;
                                o.aceite = 0;
                                o.carilleria = 0;
                                o.ansul = 0;
                                o.utilidad = 0;
                                o.costoTotal = 0;
                                o.tipoHoraDia = "---";
                                o.esActivo = true;
                                o.IndicadorManoObra = objIndicador == null ? true : objIndicador.manoObra;
                                o.IndicadorTipoMoneda = objIndicador == null ? true : objIndicador.moneda;
                                o.IndicadorAxuliar = objIndicador == null ? 1 : objIndicador.auxiliar;
                                o.IndicadorIndirectos = objIndicador == null ? 1 : objIndicador.indirectos;
                                o.esHoraDia = 0;
                                data.Add(o);
                            }
                        }
                    }
                    var dataHoraDia = data.Where(w => w.esHoraDia == esHoraDia).Distinct().ToList();
                    result.Add("data", dataHoraDia);
                    result.Add("tipoCambio", tipoCambio);
                    result.Add("conceptosMoneda", conceptos);
                    result.Add("IndicadorManoObra", objIndicador == null ? true : objIndicador.manoObra);
                    result.Add("IndicadorTipoMoneda", objIndicador == null ? true : objIndicador.moneda);
                    result.Add("IndicadorAxuliar", objIndicador == null ? 1 : objIndicador.auxiliar);
                    result.Add("IndicadorIndirectos", objIndicador == null ? 1 : objIndicador.indirectos);
                    result.Add(SUCCESS, true);
                }
                else
                {
                    result.Add(ITEMS, "NO HAY ASIGNADO CONCEPTO PARA ESE CC");
                    result.Add(SUCCESS, false);
                }

            }
            catch (Exception e)
            {
                LogError(2, 0, "CapturaController", "GetCaratula", e, AccionEnum.CONSULTA, 0, 0);
                return null;
            }
            return result;
        }


        public Dictionary<string, object> obtenerHistorialCaratulas(int estatus)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var resultado = _context.tblM_Caratula.Where(r => r.autorizada == estatus).ToList().Select(y => new
                    {
                        id = y.idCaratula,
                        NombreCaratula = "Version Caratula " + y.idCaratula,
                        estatus = RetornarEstatus(y.autorizada),
                        tipodeCambio = y.tipoCambio
                    }).ToList();
                result.Add(ITEMS, resultado);
                result.Add(SUCCESS, true);

            }
            catch (Exception)
            {
                result.Add(ITEMS, null);
                result.Add(SUCCESS, false);
            }
            return result;
        }
        public string RetornarEstatus(int autorizada)
        {
            string nombreEstatus = "";
            switch (autorizada)
            {
                case 0:
                    nombreEstatus = "En espera";
                    break;
                case 1:
                    nombreEstatus = "Autorizada";
                    break;
                case 2:
                    nombreEstatus = "Rechazada";
                    break;
            }
            return nombreEstatus;
        }





        public Dictionary<string, object> obtenerAgrupacionCaratulas()
        {
            var result = new Dictionary<string, object>();
            try
            {

                var lstRetornar = _context.tblM_CaratulaAgrupacionEnc.Where(r => r.esActivo).ToList().Select(y => new
                {
                    id = y.id,
                    AgrupacionCaratula = y.AgrupacionCaratula,
                    idGrupo = _context.tblM_CaratulaAgrupacionDet.Where(r => r.idAgrupacion == y.id && r.esActivo).FirstOrDefault() == null ? 0 : _context.tblM_CaratulaAgrupacionDet.Where(r => r.idAgrupacion == y.id && r.esActivo).FirstOrDefault().idGrupo,
                    modelosid = _context.tblM_CaratulaAgrupacionDet.Where(r => r.idAgrupacion == y.id).Select(r => r.idModelo).ToList(),
                    lstDetalle = _context.tblM_CaratulaAgrupacionDet.Where(r => r.idAgrupacion == y.id && r.esActivo).ToList().Select(n => new
                    {
                        id = n.id,
                        idGrupo = n.idGrupo,
                        grupoDescripcion = _context.tblM_CatGrupoMaquinaria.Where(r => r.id == n.idGrupo && r.estatus).FirstOrDefault() == null ? "" : _context.tblM_CatGrupoMaquinaria.Where(r => r.id == n.idGrupo && r.estatus).FirstOrDefault().descripcion,
                        ModeloDescripcion = _context.tblM_CatModeloEquipo.Where(r => r.id == n.idModelo && r.estatus).FirstOrDefault() == null ? "" : _context.tblM_CatModeloEquipo.Where(r => r.id == n.idModelo && r.estatus).FirstOrDefault().descripcion
                    }).ToList(),
                }).ToList();
                if (lstRetornar.Count() != 0)
                {
                    result.Add(ITEMS, lstRetornar);
                    result.Add(SUCCESS, true);
                }
                else
                {
                    result.Add(ITEMS, "No se encontraron datos");
                    result.Add(SUCCESS, false);
                }

            }
            catch (Exception)
            {
                result.Add(ITEMS, null);
                result.Add(SUCCESS, false);
            }
            return result;
        }
        public List<ComboDTO> obtenerGrupos()
        {
            var result = new List<ComboDTO>();
            result = _context.tblM_CatGrupoMaquinaria.ToList().Select(y => new ComboDTO
            {
                Value = y.id.ToString(),
                Text = y.descripcion
            }).ToList();
            return result;
        }
        public List<ComboDTO> obtenerModelos(int idGrupo, int Editar, int Agrupacion)
        {
            var result = new List<ComboDTO>();
            if (Editar == 1)
            {
                var yaAgrupados = _context.tblM_CaratulaAgrupacionDet.Where(r => r.esActivo).Select(y => y.idModelo).ToList();
                result = _context.tblM_CatModeloEquipo.Where(r => r.idGrupo == idGrupo && !yaAgrupados.Contains(r.id)).ToList().Select(y => new ComboDTO
                {
                    Value = y.id.ToString(),
                    Text = y.descripcion
                }).ToList();
            }
            else
            {
                var yaAgrupados = _context.tblM_CaratulaAgrupacionDet.Where(r => r.idGrupo == idGrupo && r.esActivo && r.idAgrupacion != Agrupacion).Select(y => y.idModelo).ToList();
                result = _context.tblM_CatModeloEquipo.Where(r => r.idGrupo == idGrupo && !yaAgrupados.Contains(r.id)).ToList().Select(y => new ComboDTO
                {
                    Value = y.id.ToString(),
                    Text = y.descripcion
                }).ToList();

            }
            return result;
        }
        public bool EliminarAgrupacion(int id)
        {
            bool eliminado = false;
            try
            {
                var objAgrupacion = _context.tblM_CaratulaAgrupacionEnc.Where(r => r.id == id).FirstOrDefault();
                if (objAgrupacion != null)
                {
                    objAgrupacion.esActivo = false;
                    _context.SaveChanges();

                    var lstDetalle = _context.tblM_CaratulaAgrupacionDet.Where(r => r.idAgrupacion == id).ToList();
                    if (lstDetalle.Count() != 0)
                    {
                        foreach (var item in lstDetalle)
                        {
                            item.esActivo = false;
                            _context.SaveChanges();
                            eliminado = true;
                        }
                    }
                }
            }
            catch (Exception)
            {
                eliminado = false;
            }
            return eliminado;
        }
        public bool EliminarModeloAgrupacion(int id)
        {
            bool eliminado = false;
            try
            {
                var lstDetalle = _context.tblM_CaratulaAgrupacionDet.Where(r => r.id == id).FirstOrDefault();
                if (lstDetalle != null)
                {
                    lstDetalle.esActivo = false;
                    _context.SaveChanges();
                    eliminado = true;
                }
            }
            catch (Exception)
            {
                eliminado = false;
            }
            return eliminado;
        }

        public Dictionary<string, object> GuardarEditar(CaratulaEncDTO parametros)
        {
            var result = new Dictionary<string, object>();

            try
            {

                var obj = _context.tblM_CaratulaAgrupacionEnc.Where(r => r.id == parametros.id).FirstOrDefault();
                if (obj == null)
                {
                    var obj1 = _context.tblM_CaratulaAgrupacionEnc.Where(r => r.AgrupacionCaratula == parametros.AgrupacionCaratula && r.esActivo).FirstOrDefault();
                    if (obj1 == null)
                    {
                        obj = new tblM_CaratulaAgrupacionEnc();
                        obj.AgrupacionCaratula = parametros.AgrupacionCaratula;
                        obj.esActivo = true;
                        _context.tblM_CaratulaAgrupacionEnc.Add(obj);
                        _context.SaveChanges();
                        int ultimo = _context.tblM_CaratulaAgrupacionEnc.OrderByDescending(r => r.id).FirstOrDefault().id;
                        foreach (var item in parametros.lstDetalle)
                        {
                            var detalle = _context.tblM_CaratulaAgrupacionDet.Where(r => r.id == item.id).FirstOrDefault();
                            if (detalle == null)
                            {
                                detalle = new tblM_CaratulaAgrupacionDet();
                                detalle.idAgrupacion = ultimo;
                                detalle.idGrupo = item.idGrupo;
                                detalle.idModelo = item.idModelo;
                                detalle.esActivo = true;
                                _context.tblM_CaratulaAgrupacionDet.Add(detalle);
                                _context.SaveChanges();
                            }
                        }
                    }
                    else
                    {
                        result.Add(ITEMS, "Ya existe una agrupacion con ese nombre");
                        result.Add(SUCCESS, false);
                    }
                }
                else
                {
                    obj.AgrupacionCaratula = parametros.AgrupacionCaratula;
                    _context.SaveChanges();
                    var detalle = _context.tblM_CaratulaAgrupacionDet.Where(r => r.idAgrupacion == obj.id).ToList();
                    foreach (var item in detalle)
                    {
                        item.esActivo = false;
                        _context.SaveChanges();
                    }
                    foreach (var item in parametros.lstDetalle)
                    {
                        var detalle2 = _context.tblM_CaratulaAgrupacionDet.Where(r => r.idAgrupacion == item.idAgrupacion && r.idModelo == item.idModelo).FirstOrDefault();
                        if (detalle2 != null)
                        {
                            detalle2.idModelo = item.idModelo;
                            detalle2.esActivo = true;
                            _context.SaveChanges();
                        }
                        else
                        {
                            detalle2 = new tblM_CaratulaAgrupacionDet();
                            detalle2.idAgrupacion = obj.id;
                            detalle2.idGrupo = item.idGrupo;
                            detalle2.idModelo = item.idModelo;
                            detalle2.esActivo = true;
                            _context.tblM_CaratulaAgrupacionDet.Add(detalle2);
                            _context.SaveChanges();
                        }
                    }
                }
                result.Add(ITEMS, "Guardado con exito");
                result.Add(SUCCESS, true);


            }
            catch (Exception)
            {

                result.Add(ITEMS, null);
                result.Add(SUCCESS, false);
            }
            return result;
        }

        public List<ComboDTO> ObtenerAgrupaciones()
        {
            var lst = _context.tblM_CaratulaAgrupacionEnc.Where(d => d.esActivo).Select(r => new ComboDTO
            {
                Value = _context.tblM_CaratulaAgrupacionDet.Where(d => d.idAgrupacion == r.id).FirstOrDefault() == null ? "0" : _context.tblM_CaratulaAgrupacionDet.Where(d => d.idAgrupacion == r.id).FirstOrDefault().idGrupo.ToString(),
                Text = r.AgrupacionCaratula,
            }).ToList();
            return lst;
        }


    }
}
