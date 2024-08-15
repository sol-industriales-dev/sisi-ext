using Core.DAO.Maquinaria.KPI;
using Core.DTO;
using Core.DTO.Maquinaria.Captura.KPI;
using Core.DTO.Maquinaria.KPI.Captura;
using Core.DTO.Principal.Generales;
using Core.DTO.Utils.Auth;
using Core.DTO.Utils.Data;
using Core.Entity.Maquinaria.Catalogo;
using Core.Entity.Maquinaria.KPI;
using Core.Enum.Maquinaria.KPI.CatalogoCodigo;
using Core.Entity.Principal.Usuarios;
using Core.Enum.Multiempresa;
using Core.Enum.Principal;
using Core.Enum.Principal.Bitacoras;
using Data.EntityFramework.Context;
using Data.EntityFramework.Generic;
using Data.Factory.Maquinaria.KPI;
using Infrastructure.Utils;
using Infrastructure.Utils;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.DTO.Maquinaria.KPI.Dashboard;
using Core.DTO.Utils;
using OfficeOpenXml;
using Core.Enum.Maquinaria.KPI;
using System.IO;
using System.Drawing;
using System.Web;
using Core.Entity.Maquinaria;
using Core.DTO.Maquinaria.Captura;
using System.Data;
using Core.DTO.Maquinaria.KPI;
using Core.DTO.Maquinaria.KPI.Autorizaciones;

namespace Data.DAO.Maquinaria.KPI
{
    public class KPIDAO : GenericDAO<tblM_KPI_CodigosParo>, IKPIDAO
    {

        #region variables y constructor
        private readonly Dictionary<string, object> resultado = new Dictionary<string, object>();
        KPIFactoryServices kpiFactoryServices = new KPIFactoryServices();
        private const string nombreControlador = "KPIController";

        /// <summary>
        /// Constructor
        /// </summary>
        public KPIDAO()
        {
            resultado.Clear();
        }
        #endregion
        #region CodigosParos
        public Dictionary<string, object> GuardarCodigoParo(tblM_KPI_CodigosParo codigoParo)
        {
            using (var dbTransaction = _context.Database.BeginTransaction())
            {
                try
                {
                    int usuarioCreadorID = vSesiones.sesionUsuarioDTO.id;

                    if (codigoParo.id == 0)
                    {
                        if (_context.tblM_KPI_CodigosParo.Any(x => x.codigo == codigoParo.codigo))
                        {
                            resultado.Add(SUCCESS, false);
                            resultado.Add(MESSAGE, "Ya existe un codigo con esa númeracion..");
                            return resultado;
                        }
                        codigoParo.usuarioIDCrea = usuarioCreadorID;
                        codigoParo.usuarioIDModifica = usuarioCreadorID;
                        codigoParo.fechaCreacion = DateTime.Now;
                        codigoParo.fechaModificacion = DateTime.Now;
                        _context.tblM_KPI_CodigosParo.Add(codigoParo);
                        _context.SaveChanges();
                    }
                    else
                    {
                        var actualizacionCodigo = _context.tblM_KPI_CodigosParo.FirstOrDefault(x => x.id == codigoParo.id);

                        actualizacionCodigo.descripcion = codigoParo.descripcion;
                        actualizacionCodigo.codigo = codigoParo.codigo;
                        actualizacionCodigo.activo = codigoParo.activo;
                        actualizacionCodigo.fechaModificacion = DateTime.Now;
                        actualizacionCodigo.usuarioIDModifica = usuarioCreadorID;
                        _context.SaveChanges();
                    }
                    _context.SaveChanges();
                    resultado.Add(SUCCESS, true);
                    dbTransaction.Commit();
                }
                catch (Exception e)
                {
                    dbTransaction.Rollback();
                    LogError(0, 0, nombreControlador, "GuardarCodigoParo", e, AccionEnum.ACTUALIZAR, 0, codigoParo);
                    resultado.Add(SUCCESS, false);
                    resultado.Add(MESSAGE, "Ocurrió un error al guardar los codigos .");
                }
                return resultado;
            }
        }
        public Dictionary<string, object> CargarCodigosParo(string codigoParo)
        {
            try
            {
                _context.Configuration.AutoDetectChangesEnabled = false;

                var dataCodigosParo = _context.tblM_KPI_CodigosParo
                    .Where(x => x.activo && string.IsNullOrEmpty(codigoParo) ? true : x.codigo.Contains(codigoParo))
                    .ToList();

                resultado.Add("dataCodigosParo", dataCodigosParo);
                resultado.Add(SUCCESS, true);

                return resultado;
            }
            catch (Exception e)
            {
                LogError(0, 0, nombreControlador, "CargarCodigosParo", e, AccionEnum.CONSULTA, 0, null);
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, "Ocurrió un error al buscar los codigos de paros.");
                return resultado;
            }
        }
        #endregion
        #region Captura diaria
        public List<tblM_KPI_Homologado> CargarCapturaDiaria(BusqKpiDiariaDTO busq)
        {
            _context.Configuration.AutoDetectChangesEnabled = false;
            var kpiBuscados = ConsultaHomologado(busq);
            var equipos = ConsultaEquipos(busq).ToList();
            var equiposFaltantes = from eq in equipos
                                   where kpiBuscados.Any(kpi => kpi.idEconomico != eq.id)
                                   select eq;

            if (!kpiBuscados.Any() || equiposFaltantes.Any())
            {
                var codigosParo = ConsultaCodigoParo(busq.ac);
                foreach (var codigo in codigosParo)
                {
                    kpiBuscados.Concat(from equipo in equiposFaltantes
                                       select new tblM_KPI_Homologado
                                       {
                                           economico = equipo.noEconomico,
                                           idEconomico = equipo.id,
                                           idGrupo = equipo.grupoMaquinariaID,
                                           idModelo = equipo.modeloEquipoID,
                                           ac = equipo.centro_costos,
                                           fecha = busq.fecha,
                                           turno = busq.turno,
                                           codigoParo = codigo.codigo,
                                           idParo = codigo.id,
                                           activo = true,
                                       });
                }
            }
            return kpiBuscados.ToList();
        }

        public Dictionary<string, object> GetCapturaDiaria(BusqKpiDiariaDTO busq)
        {
            DateTime fechaAnterior = busq.fecha.AddDays(-1);
            var codigosParo = ConsultaCodigoParo(busq.ac);
            var capturaDiaria = _context.tblM_KPI_Homologado.Where(r => r.fecha == busq.fecha && r.ac == busq.ac && busq.idGrupo == r.idGrupo && (busq.idModelo != 0 ? busq.idModelo == r.idModelo : true)).ToList();
            var capturaAnterior = _context.tblM_KPI_Homologado.Where(r => r.valor > 0).Where(r => r.fecha == fechaAnterior && r.ac == busq.ac && busq.idGrupo == r.idGrupo && (busq.idModelo != 0 ? busq.idModelo == r.idModelo : true)).ToList();


            var existeKPI = capturaDiaria.Where(r => authEstadoEnum.Autorizado == r.authEstado).Any();

            var equipos = _context.tblM_CatMaquina.Where(r => r.centro_costos == busq.ac && r.grupoMaquinariaID == busq.idGrupo && (busq.idModelo != 0 ? r.modeloEquipoID == busq.idModelo : true)).Select(r => new KPIHomoEquiposDTO
              {
                grupo = r.grupoMaquinaria.descripcion,
                modelo = r.modeloEquipo.descripcion,
                economico = r.noEconomico,
                economicoID = r.id,
                id = 0,
                grupoID = r.grupoMaquinariaID,
                modeloID = r.modeloEquipoID,
                horometro = _context.tblM_CapHorometro.Where(x => x.CC == busq.ac && (x.Fecha == busq.fecha ) && x.Economico.Equals(r.noEconomico) && x.turno == busq.turno ).Select(y=>y.HorasTrabajo).FirstOrDefault(),
                tieneRegistros = _context.tblM_CapHorometro.Any(x => x.CC == busq.ac && x.Economico.Equals(r.noEconomico) && ((x.Fecha == busq.fecha && x.turno >= busq.turno) || (x.Fecha > busq.fecha)))
            }).OrderBy(x => x.economico).ToList();


            //var lstEconomicos = equipos.Select(r => r.economico).OrderBy(x => x).ToList();

            //var horometrosDiarios = _context.tblM_CapHorometro.Where(r => r.CC == busq.ac && (r.Fecha >= busq.fecha && r.Fecha <= busq.fecha) && lstEconomicos.Contains(r.Economico) && r.turno == busq.turno).ToList();

            //var horometrosDiarios = (from a in _context.tblM_CapHorometro
            //                         join b in equipos on a.Economico equals b.economico
            //                         where a.CC == busq.ac && (a.Fecha >= busq.fecha && a.Fecha <= DateTime.Now)
            //                         select a).ToList();


            List<tblCategorias> tablaCategorias = new List<tblCategorias>();
            List<tblCategorias> tablaEconomicos = new List<tblCategorias>();

            tablaCategorias.Add(new tblCategorias
            {
                cod = "",
                descripcion = "Equipos",
                descripcionGrupo = "",
                tipoGrupo = 0
            });

            tablaCategorias.Add(new tblCategorias
            {
                cod = "",
                descripcion = "Modelo",
                descripcionGrupo = "",
                tipoGrupo = 0
            });
            tablaCategorias.Add(new tblCategorias
            {
                coidID = 0,
                cod = "",
                descripcion = "Economicos",
                descripcionGrupo = "",
                tipoGrupo = 0
            });
            tablaCategorias.Add(new tblCategorias
            {
                coidID = 0,
                cod = "",
                descripcion = "Horometros",
                descripcionGrupo = "",
                tipoGrupo = 0
            });

            int tipoParoTemp = 0;
            foreach (var codigo in codigosParo)
            {
                if (tipoParoTemp != 0 && tipoParoTemp != codigo.tipoParo)
                {
                    tablaCategorias.Add(new tblCategorias
                    {
                        coidID = 0,
                        cod = "",
                        descripcion = "TOTAL " + EnumExtensions.GetDescription((Tipo_ParoEnum)tipoParoTemp),
                        descripcionGrupo = "",
                        tipoGrupo = tipoParoTemp
                    });
                }

                tablaCategorias.Add(new tblCategorias
                {
                    coidID = codigo.id,
                    cod = codigo.codigo,
                    descripcion = codigo.descripcion,
                    descripcionGrupo = EnumExtensions.GetDescription((Tipo_ParoEnum)codigo.tipoParo),
                    tipoGrupo = codigo.tipoParo
                });

                tipoParoTemp = codigo.tipoParo;
            }

            tablaCategorias.Add(new tblCategorias
            {
                cod = "",
                descripcion = "TOTAL " + EnumExtensions.GetDescription((Tipo_ParoEnum)tipoParoTemp),
                descripcionGrupo = "",
                tipoGrupo = tipoParoTemp
            });
            var horas = getHorasDiaDec(busq.ac);
            //resultado.Add("capturaHorometros", horometrosDiarios);
            resultado.Add("capturaDiaria", capturaDiaria);
            resultado.Add("tblDescripciones", tablaCategorias);
            resultado.Add("listaEconomicos", equipos);
            resultado.Add("existeKPI", existeKPI);
            resultado.Add("capturaAnterior", capturaAnterior);
            resultado.Add("horasObra", horas);
            resultado.Add(SUCCESS, true);
            return resultado;
        }

        //  private IQueryable<tblM_CatMaquina> ConsultaEquipos(BusqKpiDiariaDTO busq)
        #endregion
        #region Autorizantes
        public bool GuardarAutorizacion(tblM_KPI_AuthHomologado auth)
        {
            var esGuardado = false;
            using (var _tran = _context.Database.BeginTransaction())
                try
                {
                    _context.tblM_KPI_AuthHomologado.AddOrUpdate(auth);
                    _context.SaveChanges();
                    _tran.Commit();

                    var consulta = new StoreProcedureDTO { nombre = "spM_KPI_ValidateRegs" };
                    consulta.parametros.Add(new OdbcParameterDTO { nombre = "ac", tipoSql = SqlDbType.VarChar, valor = auth.AC });
                    consulta.parametros.Add(new OdbcParameterDTO { nombre = "anio", tipoSql = SqlDbType.Int, valor = auth.Año });
                    consulta.parametros.Add(new OdbcParameterDTO { nombre = "semana", tipoSql = SqlDbType.Int, valor = auth.Semana });
                    var lst = _context.sp_Select<dynamic>(consulta);
                    esGuardado = true;
                }
                catch (Exception o_O)
                {
                    _tran.Rollback();
                    LogError(0, 0, nombreControlador, "GuardarAutorizacion", o_O, AccionEnum.ACTUALIZAR, auth.Id, auth);
                }
            return esGuardado;
        }
        public List<tblM_KPI_AuthHomologado> CargarAutorizantes(BusqKpiAuthDTO busq)
        {
            _context.Configuration.AutoDetectChangesEnabled = false;
            var Autorizantes = (
                from auth in _context.tblM_KPI_AuthHomologado.AsQueryable()
                where
                    (busq.estatus == authEstadoEnum.EnEspera ? auth.Activo : true) &&
                    (busq.año != 0 ? (auth.Año == busq.año && auth.Semana == busq.semana) : true) &&
                    //auth.AuthEstado == busq.estatus &&
                    (busq.ac == "TODOS" ? true : auth.AC == busq.ac)
                select auth
            ).ToList();

            switch (busq.estatus)
            {
                case authEstadoEnum.EnEspera:
                case authEstadoEnum.Autorizado:
                case authEstadoEnum.Rechazado:
                    Autorizantes = Autorizantes.Where(x => x.AuthEstado == busq.estatus).ToList();
                    break;
                case authEstadoEnum.SoloPendienteVoBo:
                    Autorizantes = Autorizantes.Where(x => x.FirmaVobo1 == 0).ToList();
                    break;
                case authEstadoEnum.SoloPendienteAutorizacion:
                    Autorizantes = Autorizantes.Where(x => x.FirmaVobo1 == 1 && x.FirmaAutoriza == 0).ToList();
                    break;
            }

            return Autorizantes;
        }
        public List<tblM_KPI_AuthHomologado> CargarPendientes(BusqKpiAuthDTO busq)
        {
            _context.Configuration.AutoDetectChangesEnabled = false;
            var Autorizantes = from auth in _context.tblM_KPI_AuthHomologado.AsQueryable()
                               where auth.AuthEstado == authEstadoEnum.EnEspera
                               && (busq.ac == "TODOS" ? true : auth.AC == busq.ac)
                               select auth;
            return Autorizantes.ToList();
        }
        public List<tblM_KPI_Homologado> CargarCapturaBit(int id) //Carga segun el id del autorriza
        {
            _context.Configuration.AutoDetectChangesEnabled = false;
            var encabezado = (from bit in _context.tblM_KPI_KPICapturaBit.AsQueryable()
                              where bit.idAutoriza == id
                              select bit).FirstOrDefault();
            var capturas = _context.tblM_KPI_Homologado.Where(x=>x.ac == encabezado.ac && x.fecha == encabezado.fechaInicio).ToList();
            return capturas;
        }

        public List<tblM_KPI_Homologado> CargarCapturaBitFechas(DateTime fechaInicio, DateTime fechaFin, string ac) //Carga segun el id del autorriza
        {
            List<tblM_KPI_Homologado> kpi = new List<tblM_KPI_Homologado>();
            _context.Configuration.AutoDetectChangesEnabled = false;
            var encabezado = _context.tblM_KPI_Homologado.Where(r => r.fecha == fechaInicio && r.ac == ac);
            return encabezado.ToList();
        }

        public List<tblM_CatMaquina> CargarMaquinas(List<int> ids)
        {
            _context.Configuration.AutoDetectChangesEnabled = false;
            var maquinas = from maq in _context.tblM_CatMaquina.AsQueryable()
                           where ids.Contains(maq.id)
                           select maq;
            return maquinas.ToList();
        }
        public List<authDTO> AuthCargar(int id)
        {
            var auth = ConsultaAutorizante(id);
            var usuarios = ConsultaTodosUsuariosActivos();
            var usuarioElabora = usuarios.FirstOrDefault(us => us.id == auth.UsuarioElaboraID);
            var usuarioVobo1 = usuarios.FirstOrDefault(us => us.id == auth.UsuarioVobo1);
            var usuarioVobo2 = usuarios.FirstOrDefault(us => us.id == auth.UsuarioVobo2);
            var usuarioAuth = usuarios.FirstOrDefault(us => us.id == auth.UsuarioAutoriza);
            var autorizantes = new List<authDTO>
            {
                //new authDTO {
                //    idRegistro = auth.Id,
                //    idPadre = auth.Id,
                //    idAuth = auth.UsuarioElaboraID,
                //    orden = 1,
                //    nombre = string.Format("{0} {1} {2}", usuarioElabora.nombre, usuarioElabora.apellidoPaterno, usuarioElabora.apellidoMaterno).ToUpper(),
                //    //descripcion = usuarioElabora.puesto.descripcion.ToUpper(),
                //    descripcion = "Elabora",
                //    firma = auth.CadenaElabora ?? string.Empty,
                //    clase = string.Empty,
                //},
                new authDTO {
                    idRegistro = auth.Id,
                    idPadre = auth.Id,
                    idAuth = auth.UsuarioVobo1,
                    orden = 2,
                    nombre = string.Format("{0} {1} {2}", usuarioVobo1.nombre, usuarioVobo1.apellidoPaterno, usuarioVobo1.apellidoMaterno).ToUpper(),
                    //descripcion = usuarioVobo1.puesto.descripcion.ToUpper(),
                    descripcion = "Vobo",
                    firma = auth.CadenaVobo1 ?? string.Empty,
                    clase = string.Empty,
                },
                // new authDTO {
                //    idRegistro = auth.Id,
                //    idPadre = auth.Id,
                //    idAuth = auth.UsuarioVobo2,
                //    orden = 3,
                //    nombre = string.Format("{0} {1} {2}", usuarioVobo2.nombre, usuarioVobo2.apellidoPaterno, usuarioVobo2.apellidoMaterno).ToUpper(),
                //    //descripcion = usuarioVobo2.puesto.descripcion.ToUpper(),
                //    descripcion = "Vobo 2",
                //    firma = auth.CadenaVobo2 ?? string.Empty,
                //    clase = string.Empty,
                //},
                 new authDTO {
                    idRegistro = auth.Id,
                    idPadre = auth.Id,
                    idAuth = auth.UsuarioAutoriza,
                    orden = 4,
                    nombre = string.Format("{0} {1} {2}", usuarioAuth.nombre, usuarioAuth.apellidoPaterno, usuarioAuth.apellidoMaterno).ToUpper(),
                    //descripcion = usuarioAuth.puesto.descripcion.ToUpper(),
                    descripcion = "Autorizante",
                    firma = auth.CadenaAutoriza ?? string.Empty,
                    clase = string.Empty,
                }
            };
            var ordenEnTurno = autorizantes.Any(w => string.IsNullOrEmpty(w.firma)) ? autorizantes.Where(w => string.IsNullOrEmpty(w.firma)).Min(m => m.orden) : 0;
            autorizantes.ForEach(autorizante =>
            {
                autorizante.comentario = string.Empty;
                if (autorizante.firma.Contains("A"))
                {
                    autorizante.authEstado = authEstadoEnum.Autorizado;
                }
                else if (autorizante.firma.Contains("R"))
                {
                    autorizante.authEstado = authEstadoEnum.Rechazado;
                    autorizante.comentario = auth.Comentario;
                }
                else if (string.IsNullOrEmpty(autorizante.firma))
                {
                    if (ordenEnTurno == autorizante.orden)
                    {
                        autorizante.authEstado = authEstadoEnum.EnTurno;
                    }
                    else
                    {
                        autorizante.authEstado = authEstadoEnum.EnEspera;
                    }
                }
            });
            return autorizantes;
        }
        public List<tblM_KPI_CodigosParo> CodigosParo(string ac)
        {
            return ConsultaCodigoParo(ac).ToList();
        }
        #endregion
        #region Combobox
        public Dictionary<string, object> getCboACKPI()
        {
            return resultado;
        }
        public List<ComboDTO> ComboPeriodo()
        {
            var items = new List<ComboDTO>();
            var capturas = from kpi in _context.tblM_KPI_AuthHomologado.AsQueryable()
                           where kpi.Activo
                           select kpi;
            var años = (from kpi in capturas
                        group kpi by kpi.Año into gpo
                        orderby gpo.Key descending
                        select gpo.Key).ToList();
            foreach (var año in años)
            {
                var semanas = (from kpi in capturas
                               where kpi.Año == año
                               group kpi by kpi.Semana into gpo
                               orderby gpo.Key descending
                               select gpo.Key).ToList();
                foreach (var semana in semanas)
                {
                    var fecha = DatetimeUtils.primerDiaSemana(año, semana);
                    var min = fecha.AddDays(1);
                    var max = fecha.AddDays(6);
                    items.Add(new ComboDTO
                    {
                        Text = string.Format("{0:dd/MM/yyyy} - {1:dd/MM/yyyy}", min, max),
                        Value = "",
                        Prefijo = JsonConvert.SerializeObject(new
                        {
                            año,
                            semana,
                            min = min.ToShortDateString(),
                            max = max.ToShortDateString()
                        })
                    });
                }
            }
            return items;
        }
        public List<ComboDTO> ComboAreaCuenta()
        {
            try
            {
                var odbcCplan = new OdbcConsultaDTO()
                {
                    consulta = @"SELECT DISTINCT CAST(area AS Varchar)+'-'+CAST(cuenta AS Varchar) AS Value, CAST(area AS Varchar)+'-'+CAST(cuenta AS Varchar)+' '+descripcion AS Text, area, cuenta
                                            FROM si_area_cuenta 
                                            ORDER BY area, cuenta"
                };
                var lst = _contextEnkontrol.Select<Core.DTO.Principal.Generales.ComboDTO>(EnkontrolAmbienteEnum.Prod, odbcCplan);
                return lst;
            }
            catch (Exception o_O) { return new List<Core.DTO.Principal.Generales.ComboDTO>(); }
        }
        #endregion
        #region Auxiliares
        private IQueryable<tblM_CatMaquina> ConsultaEquipos(BusqKpiDiariaDTO busq)
        {
            var buscarModelo = busq.idModelo == 0;
            return from eq in _context.tblM_CatMaquina.AsQueryable()
                   where eq.estatus > 0 && eq.centro_costos == busq.ac && eq.grupoMaquinariaID == busq.idGrupo && (buscarModelo ? true : eq.modeloEquipoID == busq.idModelo)
                   select eq;
        }
        private IQueryable<tblM_KPI_Homologado> ConsultaHomologado(BusqKpiDiariaDTO busq)
        {
            var buscarModelo = busq.idModelo == 0;
            return from kpi in _context.tblM_KPI_Homologado.AsQueryable()
                   where kpi.activo && kpi.fecha == busq.fecha && kpi.turno == busq.turno && kpi.ac == busq.ac && kpi.idGrupo == busq.idGrupo && (buscarModelo ? true : kpi.idModelo == busq.idModelo)
                   select kpi;
        }
        private IQueryable<tblM_KPI_CodigosParo> ConsultaCodigoParo(string ac)
        {
            return (from codigo in _context.tblM_KPI_CodigosParo.AsQueryable()
                    where codigo.activo && (string.IsNullOrEmpty(ac) ? true : codigo.areaCuenta == ac)
                    select codigo).OrderBy(r => r.tipoParo);
        }
        public tblM_KPI_AuthHomologado ConsultaAutorizante(int id)
        {
            return (from auth in _context.tblM_KPI_AuthHomologado
                    where auth.Activo && auth.Id == id
                    select auth).FirstOrDefault();
        }

        public List<tblM_CatModeloEquipo> CboModeloEquipos(int grupoID)
        {
            return _context.tblM_CatModeloEquipo.Where(r => r.idGrupo == grupoID).ToList();
        }

        public List<tblM_CatGrupoMaquinaria> CboGrupoEquipos(string areaCuenta)
        {
            return _context.tblM_CatMaquina.Where(r => r.centro_costos == areaCuenta).Select(r => r.grupoMaquinaria).Distinct().ToList();
        }

        public Dictionary<string, object> saveOrUpdateCapturaDiaria(List<tblM_KPI_Homologado> capturaDiaria, List<tblM_CapHorometro> horometros)
        {
            using (var dbTransaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var primero = capturaDiaria.FirstOrDefault();
                    var ecos = capturaDiaria.Select(x=>x.economico).ToList();
                    var capturas = _context.tblM_KPI_Homologado.Where(r => r.ac == primero.ac && r.fecha == primero.fecha && r.turno == primero.turno && ecos.Contains(r.economico));
                    _context.tblM_KPI_Homologado.RemoveRange(capturas);
                    _context.tblM_KPI_Homologado.AddRange(capturaDiaria);
                    _context.SaveChanges();
                    //foreach (var item in capturaDiaria)
                    //{
                    //    var captura = _context.tblM_KPI_Homologado.FirstOrDefault(r => r.id == item.id);
                    //    if (captura != null)
                    //    {
                    //        captura.valor = item.valor;
                    //        _context.SaveChanges();
                    //    }
                    //    else
                    //    {
                    //        _context.tblM_KPI_Homologado.Add(item);
                    //        _context.SaveChanges();
                    //    }
                    //}
                    foreach (var item in horometros.Where(x=>x.HorasTrabajo>0 && !x.Ritmo))
                    {
                        var caphorometro = _context.tblM_CapHorometro.Where(r => r.Fecha <= item.Fecha && r.Economico == item.Economico).OrderByDescending(f => f.Fecha).ThenByDescending(f => f.turno).FirstOrDefault();
                        if (caphorometro.Fecha == item.Fecha && item.turno == caphorometro.turno)
                        {

                        }
                        else
                        {
                            var validHorometro = _context.tblM_CapHorometro.Where(r => r.Fecha >= item.Fecha && r.Economico == item.Economico && item.turno == r.turno).FirstOrDefault();
                            if (validHorometro == null)
                            {
                                item.Desfase = caphorometro.Desfase;
                                item.Ritmo = caphorometro.Ritmo;
                                item.HorometroAcumulado = caphorometro.HorometroAcumulado + item.HorasTrabajo;
                                item.Horometro = caphorometro.Horometro + item.HorasTrabajo;
                                item.FechaCaptura = DateTime.Now;
                                item.folio = "KPI";
                                _context.tblM_CapHorometro.Add(item);
                                _context.SaveChanges();
                            }
                        }
                    }

                    resultado.Add(SUCCESS, true);
                    dbTransaction.Commit();
                }
                catch (Exception e)
                {
                    dbTransaction.Rollback();
                    LogError(0, 0, nombreControlador, "AgregarCapturaDiaria", e, AccionEnum.AGREGAR, 0, capturaDiaria);
                    resultado.Add(MESSAGE, "Ocurrió un error en la captura diaria.");
                    resultado.Add(SUCCESS, false);
                }
            }

            return resultado;
        }
        private IQueryable<tblP_Usuario> ConsultaTodosUsuariosActivos()
        {
            return from user in _context.tblP_Usuario.AsQueryable()
                   where user.estatus
                   select user;
        }

        public Dictionary<string, object> GuardarSemana(string ac, DateTime fechaInicio, DateTime fechaFinal, int semana)
        {
            using (var dbTransaction = _context.Database.BeginTransaction())
            {
                //try
                //{

                    var pAño = fechaFinal.Year;
                    var existeAutorizacion = _context.tblM_KPI_AuthHomologado.FirstOrDefault(r => r.AC == ac && r.fechaInicio == fechaInicio);
                    var autorizadores = GetAutorizadores(ac);
                    if (existeAutorizacion == null)
                    {

                        tblM_KPI_AuthHomologado autoObj = new tblM_KPI_AuthHomologado();
                        autoObj.AC = ac;
                        autoObj.Activo = true;
                        autoObj.Año = DateTime.Now.Year;
                        autoObj.AuthEstado = 0;
                        autoObj.Id = 0;
                        autoObj.Semana = 0;
                        autoObj.UsuarioAutoriza = autorizadores.FirstOrDefault(x => x.perfilAutorizaID.Equals(1)).usuarioID;
                        autoObj.UsuarioVobo1 = autorizadores.FirstOrDefault(x => x.perfilAutorizaID.Equals(5)).usuarioID;
                        autoObj.UsuarioVobo2 = 0;
                        autoObj.UsuarioElaboraID = vSesiones.sesionUsuarioDTO.id;
                        autoObj.FirmaAutoriza = 0;
                        autoObj.FirmaElabora = 0;
                        autoObj.FirmaVobo1 = 0;
                        autoObj.FirmaVobo2 = 0;

                        autoObj.FechaAutoriza = DateTime.Now;
                        autoObj.FechaElaboracion = DateTime.Now;
                        autoObj.FechaVobo1 = DateTime.Now;
                        autoObj.FechaVobo2 = DateTime.Now;
                        autoObj.fechaInicio = fechaInicio;
                        _context.tblM_KPI_AuthHomologado.Add(autoObj);
                        _context.SaveChanges();
                        var consulta = new StoreProcedureDTO { nombre = "spM_KPI_SaveConcentrado" };
                        consulta.parametros.Add(new OdbcParameterDTO { nombre = "ac", tipoSql = SqlDbType.VarChar, valor = ac });
                        consulta.parametros.Add(new OdbcParameterDTO { nombre = "fechaInicio", tipoSql = SqlDbType.DateTime, valor = fechaInicio });
                        consulta.parametros.Add(new OdbcParameterDTO { nombre = "fechaFin", tipoSql = SqlDbType.DateTime, valor = fechaInicio });
                        consulta.parametros.Add(new OdbcParameterDTO { nombre = "anio", tipoSql = SqlDbType.Int, valor = fechaFinal.Year });
                        consulta.parametros.Add(new OdbcParameterDTO { nombre = "semana", tipoSql = SqlDbType.Int, valor = 0 });
                        consulta.parametros.Add(new OdbcParameterDTO { nombre = "estatus", tipoSql = SqlDbType.Int, valor = (int)authEstadoEnum.EnTurno });
                        var lst = _context.sp_Select<dynamic>(consulta);


                        tblM_KPI_KPICapturaBit objBit = new tblM_KPI_KPICapturaBit();
                        objBit.año = fechaFinal.Year;
                        objBit.authEstado = authEstadoEnum.EnTurno;
                        objBit.fechaCaptura = DateTime.Now;
                        objBit.fechaFin = fechaInicio;
                        objBit.fechaInicio = fechaInicio;
                        objBit.id = 0;
 
                        objBit.semana = 0;
                        objBit.idAutoriza = autoObj.Id;
                        objBit.ac = ac;
                        _context.tblM_KPI_KPICapturaBit.Add(objBit);
                        _context.SaveChanges();

                        resultado.Add(SUCCESS, true);
                        dbTransaction.Commit();
                    }
                    else
                    {
                        int idExist = existeAutorizacion.Id;
                        _context.tblM_KPI_AuthHomologado.Remove(existeAutorizacion);
                        _context.SaveChanges();

                        var consulta = new StoreProcedureDTO { nombre = "spM_KPI_SaveConcentrado" };
                        consulta.parametros.Add(new OdbcParameterDTO { nombre = "ac", tipoSql = SqlDbType.VarChar, valor = ac });
                        consulta.parametros.Add(new OdbcParameterDTO { nombre = "fechaInicio", tipoSql = SqlDbType.DateTime, valor = fechaInicio });
                        consulta.parametros.Add(new OdbcParameterDTO { nombre = "fechaFin", tipoSql = SqlDbType.DateTime, valor = fechaInicio });
                        consulta.parametros.Add(new OdbcParameterDTO { nombre = "anio", tipoSql = SqlDbType.Int, valor = fechaFinal.Year });
                        consulta.parametros.Add(new OdbcParameterDTO { nombre = "semana", tipoSql = SqlDbType.Int, valor = 0 });
                        consulta.parametros.Add(new OdbcParameterDTO { nombre = "estatus", tipoSql = SqlDbType.Int, valor = (int)authEstadoEnum.EnTurno });
                        var lst = _context.sp_Select<dynamic>(consulta);


                        tblM_KPI_AuthHomologado autoObj = new tblM_KPI_AuthHomologado();
                        autoObj.AC = ac;
                        autoObj.Activo = true;
                        autoObj.Año = DateTime.Now.Year;
                        autoObj.AuthEstado = 0;
                        autoObj.Id = 0;
                        autoObj.Semana = semana;
                        autoObj.UsuarioAutoriza = autorizadores.FirstOrDefault(x => x.perfilAutorizaID.Equals(1)).usuarioID;
                        autoObj.UsuarioVobo1 = autorizadores.FirstOrDefault(x => x.perfilAutorizaID.Equals(5)).usuarioID;
                        autoObj.UsuarioVobo2 = autorizadores.FirstOrDefault(x => x.perfilAutorizaID.Equals(1)).usuarioID;
                        autoObj.UsuarioElaboraID = vSesiones.sesionUsuarioDTO.id;
                        autoObj.FirmaAutoriza = 0;
                        autoObj.FirmaElabora = 0;
                        autoObj.FirmaVobo1 = 0;
                        autoObj.FirmaVobo2 = 0;

                        autoObj.FechaAutoriza = DateTime.Now;
                        autoObj.FechaElaboracion = DateTime.Now;
                        autoObj.FechaVobo1 = DateTime.Now;
                        autoObj.FechaVobo2 = DateTime.Now;
                        autoObj.fechaInicio = fechaInicio;
                        _context.tblM_KPI_AuthHomologado.Add(autoObj);
                        _context.SaveChanges();

                        var objBit = _context.tblM_KPI_KPICapturaBit.FirstOrDefault(r => r.idAutoriza == idExist);
                        objBit.idAutoriza = autoObj.Id;
                        _context.SaveChanges();

                        resultado.Add(SUCCESS, true);
                        dbTransaction.Commit();
                    }
                //}
                //catch (Exception e)
                //{
                //    dbTransaction.Rollback();
                //    LogError(0, 0, nombreControlador, "AgregarAutorizacion", e, AccionEnum.AGREGAR, 0, null);
                //    resultado.Add(MESSAGE, "Ocurrió un error al generar la conciliacion de kpi semanal..");
                //    resultado.Add(SUCCESS, false);
                //}
            }

            return resultado;
        }
        public Dictionary<string, object> ValidarConcentrado(string ac, DateTime fechaInicio, DateTime fechaFinal, int semana)
        {
            using (var dbTransaction = _context.Database.BeginTransaction())
            {
                try
                {

                    var pAño = fechaFinal.Year;
                    var obj = _context.tblM_KPI_KPICapturaBit.FirstOrDefault(r => r.ac == ac && r.fechaInicio == fechaInicio);
                    obj.validado = true;
                    _context.SaveChanges();
                    dbTransaction.Commit();
                    resultado.Add(SUCCESS, true);
                }
                catch (Exception e)
                {
                    dbTransaction.Rollback();
                    resultado.Add(MESSAGE, "Ocurrió un error al validar la conciliacion de kpi semanal..");
                    resultado.Add(SUCCESS, false);
                }
            }

            return resultado;
        }
        private void genSendMail()
        {

        }

        public List<tblP_Autoriza> GetAutorizadores(string cc)
        {
            var resultado = from a in _context.tblP_Autoriza
                            join pa in _context.tblP_PerfilAutoriza on a.perfilAutorizaID equals pa.id
                            join ccU in _context.tblP_CC_Usuario on a.cc_usuario_ID equals ccU.id
                            where (!string.IsNullOrEmpty(cc) ? ccU.cc == cc : a.cc_usuario_ID == ccU.id)
                            select a;

            return resultado.ToList();
        }
        #endregion
        #region Dashboard

        public Dictionary<string, object> GetInfoFiltro(FiltroDTO filtro)
        {
            var r = new Dictionary<string, object>();
            var turnos = _context.tblM_KPI_Homologado_Turnos.FirstOrDefault(x => x.ac == filtro.areaCuenta);
            var horasDia = turnos == null ? 24 : turnos.horas_dia;
            try
            {
                filtro.fechaFin = filtro.fechaFin.AddHours(23).AddMinutes(59).AddSeconds(59);

                var homologados = _context.tblM_KPI_Homologado.Where
                    (w =>
                        w.fecha >= filtro.fechaInicio &&
                        w.fecha <= filtro.fechaFin &&
                        w.ac == filtro.areaCuenta &&
                        filtro.idEconomicos.Contains(w.idEconomico) &&
                        (filtro.turno == 0 ? true : w.turno == filtro.turno) &&
                            //w.activo && w.valor > 0
                        w.valor > 0 && w.validado
                    ).Join
                    (
                        _context.tblM_KPI_CodigosParo,
                        homolo => homolo.idParo,
                        codigo => codigo.id,
                        (homolo, codigo) => new { homolo, codigo }
                    ).Join
                    (
                        _context.tblM_CatMaquina,
                        homolo => homolo.homolo.idEconomico,
                        maquina => maquina.id,
                        (homolo, maquina) => new { homolo.homolo, homolo.codigo, maquina }
                    ).Join
                    (
                        _context.tblM_CatModeloEquipo,
                        homolo => homolo.homolo.idModelo,
                        modelo => modelo.id,
                        (homolo, modelo) => new { homolo.homolo, homolo.codigo, homolo.maquina, modelo }
                    ).Join
                    (
                        _context.tblM_CatGrupoMaquinaria,
                        homolo => homolo.homolo.idGrupo,
                        grupo => grupo.id,
                        (homolo, grupo) => new { homolo.homolo, homolo.codigo, homolo.maquina, homolo.modelo, grupo }
                    ).Select(m => new QueryHomologadosDTO()
                    {
                        homolo = m.homolo,
                        codigo = m.codigo,
                        maquina = m.maquina,
                        modelo = m.modelo,
                        grupo = m.grupo
                    });

                var __equipos = _context.tblM_CatMaquina.Where(w => filtro.idEconomicos.Contains(w.id)).ToList();

                var hrsProgramado = DatetimeUtils.DiasDiferencia(filtro.fechaInicio, filtro.fechaFin) * horasDia;
                var semanas = DatetimeUtils.SemanasEntreFechas(filtro.fechaInicio, filtro.fechaFin);
                var meses = DatetimeUtils.MesesEntreFechas(filtro.fechaInicio, filtro.fechaFin);

                var resultados = Calculos(filtro,homologados, hrsProgramado, semanas, meses, __equipos);

                try
                {
                    var excel = GetExcel(filtro, hrsProgramado);
                    r.Add("excel", excel);
                    r.Add("noExcel", false);
                }
                catch (Exception ex)
                {
                    r.Add("excel", null);
                    r.Add("noExcel", true);
                }

                var resultadosGraficaID = GetDatosGraficaParosReservaSinUso(homologados, filtro.areaCuenta);
                var resultadosGraficaDL = GetDatosGraficaParosDemora(homologados, filtro.areaCuenta);
                var resultadosGraficaMT = GetDatosGraficaParosMantenimientos(homologados, filtro.areaCuenta);

                var resultadoAnual = InfoAnual(filtro);

                r.Add(SUCCESS, true);

                r.Add("tiempos", resultados.tiempos);

                r.Add("disVsUti_economico", resultados.disVsUti_economico);
                r.Add("disVsUti_modelo", resultados.disVsUti_modelo);
                r.Add("disVsUti_grupo", resultados.disVsUti_grupo);

                r.Add("gpx_disVsUti_economico", resultados.gpx_disVsUti_economico);
                r.Add("gpx_disVsUti_modelo", resultados.gpx_disVsUti_modelo);
                r.Add("gpx_disVsUti_grupo", resultados.gpx_disVsUti_grupo);
                r.Add("gpx_disVsUti_semanal", resultados.gpx_disVsUti_semanal);
                r.Add("gpx_disVsUti_mensual", resultados.gpx_disVsUti_mensual);

                r.Add("gpx_opeVsTra_economico", resultados.gpx_opeVsTra_economico);
                r.Add("gpx_opeVsTra_modelo", resultados.gpx_opeVsTra_modelo);
                r.Add("gpx_opeVsTra_grupo", resultados.gpx_opeVsTra_grupo);
                r.Add("gpx_opeVsTra_semanal", resultados.gpx_opeVsTra_semanal);
                r.Add("gpx_opeVsTra_mensual", resultados.gpx_opeVsTra_mensual);

                r.Add("gpx_UT_economico", resultados.gpx_UT_economico);
                r.Add("gpx_UT_modelo", resultados.gpx_UT_modelo);
                r.Add("gpx_UT_grupo", resultados.gpx_UT_grupo);
                r.Add("gpx_UT_semanal", resultados.gpx_UT_semanal);
                r.Add("gpx_UT_mensual", resultados.gpx_UT_mensual);

                r.Add("anual", resultadoAnual);

                r.Add("resultadosGraficaID", resultadosGraficaID);
                r.Add("resultadosGraficaDL", resultadosGraficaDL);
                r.Add("resultadosGraficaMT", resultadosGraficaMT);
            }
            catch (Exception ex)
            {
                r.Add(SUCCESS, false);
                r.Add(MESSAGE, ex.Message);
            }

            return r;
        }

        public Dictionary<string, object> GetInfoFiltroCodigo(FiltroDTO filtro, string codigo, int tipo)
        {
            var r = new Dictionary<string, object>();

            try
            {
                codigo = codigo.Split('/')[0].Trim();
                filtro.fechaFin = filtro.fechaFin.AddHours(23).AddMinutes(59).AddSeconds(59);

                List<int> tiposParo = new List<int>();
                switch (tipo)
                {
                    case 1:
                        tiposParo.Add((int)Tipo_ParoEnum.mantenimiento_programado);
                        tiposParo.Add((int)Tipo_ParoEnum.mantenimiento_no_programado);
                        break;
                    case 2:
                        tiposParo.Add((int)Tipo_ParoEnum.sin_utilizar);
                        break;
                    case 3:
                        tiposParo.Add((int)Tipo_ParoEnum.demoras);
                        break;
                    default:
                        tiposParo.Add((int)Tipo_ParoEnum.demoras);
                        tiposParo.Add((int)Tipo_ParoEnum.sin_utilizar);
                        tiposParo.Add((int)Tipo_ParoEnum.mantenimiento_programado);
                        tiposParo.Add((int)Tipo_ParoEnum.mantenimiento_no_programado);
                        break;
                }

                var homologados = _context.tblM_KPI_Homologado.Where
                    (w =>
                        w.fecha >= filtro.fechaInicio &&
                        w.fecha <= filtro.fechaFin &&
                        w.ac == filtro.areaCuenta &&
                        filtro.idEconomicos.Contains(w.idEconomico) &&
                        (filtro.turno == 0 ? true : w.turno == filtro.turno) &&
                            //w.activo && w.valor > 0 &&
                        w.valor > 0 &&
                        w.codigoParo == codigo &&
                        tiposParo.Contains(w.idTipoParo)
                    ).Join
                    (
                        _context.tblM_KPI_CodigosParo,
                        homolo => homolo.idParo,
                        cod => cod.id,
                        (homolo, cod) => new { homolo, cod }
                    ).Join
                    (
                        _context.tblP_CC,
                        homolo => homolo.homolo.ac,
                        ac => ac.areaCuenta,
                        (homolo, ac) => new { homolo.homolo, homolo.cod, ac }
                    ).ToList().Select(m => new KPIHomologadoDetDTO()
                    {
                        codigo = m.homolo.codigoParo + " " + m.cod.descripcion,
                        economico = m.homolo.economico,
                        ac = m.ac.areaCuenta + " - " + m.ac.descripcion,
                        valor = m.homolo.valor,
                        fecha = m.homolo.fecha.ToString("dd/MM/yyyy"),
                        turno = m.homolo.turno == 1 ? "MATUTINO" : (m.homolo.turno == 2 ? "VESPERTINO" : (m.homolo.turno == 3 ? "NOCTURNO" : "INDEFINIDO")),

                    }).ToList(); ;

                r.Add(SUCCESS, true);

                r.Add("lst", homologados);
            }
            catch (Exception ex)
            {
                r.Add(SUCCESS, false);
                r.Add(MESSAGE, ex.Message);
            }

            return r;
        }

        private ResultadosDTO Calculos(FiltroDTO filtro,IQueryable<QueryHomologadosDTO> datosHomologados, int hrsProgramado, List<InfoPeriodoDTO> semanas, List<InfoPeriodoDTO> meses, List<tblM_CatMaquina> __equipos)
        {
            var resultados = new ResultadosDTO();
            var turnos = _context.tblM_KPI_Homologado_Turnos.FirstOrDefault(x => x.ac == filtro.areaCuenta);
            var horasDia = turnos == null ? 24 : turnos.horas_dia;
            var disponibilidadVsUtilizacionEconomico = new List<CalculosDTO>();
            var disponibilidadVsUtilizacionModelo = new List<CalculosDTO>();
            var disponibilidadVsUtilizacionGrupo = new List<CalculosDTO>();

            var gpx_dVSu_economico = new InfoGraficasDTO();
            var gpx_dVSu_modelo = new InfoGraficasDTO();
            var gpx_dVSu_grupo = new InfoGraficasDTO();
            var gpx_dVSu_semanal = new InfoGraficasDTO();
            var gpx_dVSu_mensual = new InfoGraficasDTO();

            var gpx_oVSt_economico = new InfoGraficasDTO();
            var gpx_oVSt_modelo = new InfoGraficasDTO();
            var gpx_oVSt_grupo = new InfoGraficasDTO();
            var gpx_oVSt_semanal = new InfoGraficasDTO();
            var gpx_oVSt_mensual = new InfoGraficasDTO();

            var gpx_UT_economico = new InfoGraficasDTO();
            var gpx_UT_modelo = new InfoGraficasDTO();
            var gpx_UT_grupo = new InfoGraficasDTO();
            var gpx_UT_semanal = new InfoGraficasDTO();
            var gpx_UT_mensual = new InfoGraficasDTO();

            foreach (var gbGrupo in datosHomologados.GroupBy(g => g.homolo.idGrupo))
            {
                foreach (var gbModelo in gbGrupo.GroupBy(g => g.homolo.idModelo))
                {
                    foreach (var gbEconomico in gbModelo.GroupBy(g => g.homolo.idEconomico))
                    {
                        var calculosEconomico = new CalculosDTO();

                        calculosEconomico.id = gbEconomico.Key;
                        calculosEconomico.descripcion = gbEconomico.First().homolo.economico;
                        calculosEconomico.tiempos = CalculosTiempos(gbEconomico, hrsProgramado);

                        disponibilidadVsUtilizacionEconomico.Add(calculosEconomico);

                        #region infoGrafic_gpx_economico
                        gpx_dVSu_economico.categorias.Add(calculosEconomico.descripcion);
                        gpx_dVSu_economico.serie1Descripcion = "DISPONIBILIDAD";
                        gpx_dVSu_economico.serie1.Add(Math.Round(((calculosEconomico.tiempos.hrsDisponible / (calculosEconomico.tiempos.hrsProgramado > 0 ? calculosEconomico.tiempos.hrsProgramado : 1)) * 100), 2));
                        gpx_dVSu_economico.serie2Descripcion = "UTILIZACIÓN";
                        #region V1
                        //gpx_dVSu_economico.serie2.Add(Math.Round(((calculosEconomico.tiempos.hrsTrabajo / calculosEconomico.tiempos.hrsProgramado) * 100), 2));
                        #endregion
                        gpx_dVSu_economico.serie2.Add(Math.Round(((calculosEconomico.tiempos.hrsOperacion / (calculosEconomico.tiempos.hrsDisponible > 0 ? calculosEconomico.tiempos.hrsDisponible : 1)) * 100), 2));

                        #region V1
                        //gpx_oVSt_economico.categorias.Add(calculosEconomico.descripcion);
                        //gpx_oVSt_economico.serie1Descripcion = "OPERACIÓN (OPT)";
                        //gpx_oVSt_economico.serie1.Add(calculosEconomico.tiempos.porOperacion);
                        //gpx_oVSt_economico.serie2Descripcion = "TRABAJO (WK)";
                        //gpx_oVSt_economico.serie2.Add(calculosEconomico.tiempos.porTrabajo);
                        #endregion
                        #region V2
                        gpx_oVSt_economico.categorias.Add(calculosEconomico.descripcion);
                        gpx_oVSt_economico.serie1Descripcion = "UTILIZACIÓN"; //UA
                        gpx_oVSt_economico.serie1.Add(Math.Round(((calculosEconomico.tiempos.hrsOperacion / (calculosEconomico.tiempos.hrsDisponible > 0 ? calculosEconomico.tiempos.hrsDisponible : 1)) * 100), 2));
                        gpx_oVSt_economico.serie2Descripcion = "EFICIENCIA DE LA UTILIZACIÓN"; //UE
                        gpx_oVSt_economico.serie2.Add(Math.Round(((calculosEconomico.tiempos.hrsTrabajo / (calculosEconomico.tiempos.hrsOperacion > 0 ? calculosEconomico.tiempos.hrsOperacion : 1)) * 100), 2));
                        #endregion

                        gpx_UT_economico.categorias.Add(calculosEconomico.descripcion);
                        gpx_UT_economico.serie1Descripcion = "UTILIZACIÓN";
                        gpx_UT_economico.serie1.Add(Math.Round(((calculosEconomico.tiempos.hrsTrabajo / (calculosEconomico.tiempos.hrsProgramado > 0 ? calculosEconomico.tiempos.hrsProgramado : 1)) * 100), 2));
                        #endregion
                    }

                    var calculosModelo = new CalculosDTO();

                    calculosModelo.id = gbModelo.Key;
                    calculosModelo.descripcion = gbModelo.First().maquina.modeloEquipo.descripcion;
                    calculosModelo.tiempos = CalculosTiempos(gbModelo, hrsProgramado * __equipos.Where(w => w.modeloEquipoID == gbModelo.Key).Count());

                    disponibilidadVsUtilizacionModelo.Add(calculosModelo);

                    #region infoGrafic_gpx_modelo
                    gpx_dVSu_modelo.categorias.Add(calculosModelo.descripcion);
                    gpx_dVSu_modelo.serie1Descripcion = "DISPONIBILIDAD";
                    gpx_dVSu_modelo.serie1.Add(Math.Round(((calculosModelo.tiempos.hrsDisponible / calculosModelo.tiempos.hrsProgramado) * 100), 2));
                    gpx_dVSu_modelo.serie2Descripcion = "UTILIZACIÓN";
                    gpx_dVSu_modelo.serie2.Add(Math.Round(((calculosModelo.tiempos.hrsOperacion / calculosModelo.tiempos.hrsDisponible) * 100), 2));

                    gpx_oVSt_modelo.categorias.Add(calculosModelo.descripcion);
                    gpx_oVSt_modelo.serie1Descripcion = "UTILIZACIÓN";
                    gpx_oVSt_modelo.serie1.Add(Math.Round(((calculosModelo.tiempos.hrsOperacion / calculosModelo.tiempos.hrsDisponible) * 100), 2));
                    gpx_oVSt_modelo.serie2Descripcion = "EFICIENCIA DE LA UTILIZACIÓN";
                    gpx_oVSt_modelo.serie2.Add(Math.Round(((calculosModelo.tiempos.hrsTrabajo / calculosModelo.tiempos.hrsOperacion) * 100), 2));

                    gpx_UT_modelo.categorias.Add(calculosModelo.descripcion);
                    gpx_UT_modelo.serie1Descripcion = "UTILIZACIÓN";
                    gpx_UT_modelo.serie1.Add(Math.Round(((calculosModelo.tiempos.hrsTrabajo / calculosModelo.tiempos.hrsProgramado) * 100), 2));
                    #endregion
                }

                var calculosGrupo = new CalculosDTO();

                calculosGrupo.id = gbGrupo.Key;
                calculosGrupo.descripcion = gbGrupo.First().maquina.grupoMaquinaria.descripcion;
                calculosGrupo.tiempos = CalculosTiempos(gbGrupo, hrsProgramado * __equipos.Where(w => w.grupoMaquinariaID == gbGrupo.Key).Count());

                disponibilidadVsUtilizacionGrupo.Add(calculosGrupo);

                #region infoGrafic_gpx_grupo
                gpx_dVSu_grupo.categorias.Add(calculosGrupo.descripcion);
                gpx_dVSu_grupo.serie1Descripcion = "DISPONIBILIDAD";
                gpx_dVSu_grupo.serie1.Add(Math.Round(((calculosGrupo.tiempos.hrsDisponible / calculosGrupo.tiempos.hrsProgramado) * 100), 2));
                gpx_dVSu_grupo.serie2Descripcion = "UTILIZACIÓN";
                gpx_dVSu_grupo.serie2.Add(Math.Round(((calculosGrupo.tiempos.hrsOperacion / calculosGrupo.tiempos.hrsDisponible) * 100), 2));

                gpx_oVSt_grupo.categorias.Add(calculosGrupo.descripcion);
                gpx_oVSt_grupo.serie1Descripcion = "UTILIZACIÓN";
                gpx_oVSt_grupo.serie1.Add(Math.Round(((calculosGrupo.tiempos.hrsOperacion / calculosGrupo.tiempos.hrsDisponible) * 100), 2));
                gpx_oVSt_grupo.serie2Descripcion = "EFICIENCIA DE LA UTILIZACIÓN";
                gpx_oVSt_grupo.serie2.Add(Math.Round(((calculosGrupo.tiempos.hrsTrabajo / calculosGrupo.tiempos.hrsOperacion) * 100), 2));

                gpx_UT_grupo.categorias.Add(calculosGrupo.descripcion);
                gpx_UT_grupo.serie1Descripcion = "UTILIZACIÓN";
                gpx_UT_grupo.serie1.Add(Math.Round(((calculosGrupo.tiempos.hrsTrabajo / calculosGrupo.tiempos.hrsProgramado) * 100), 2));
                #endregion
            }

            #region infoGrafic_gpx_mensual_semanal
            gpx_dVSu_mensual.serie1Descripcion = "DISPONIBILIDAD";
            gpx_dVSu_mensual.serie2Descripcion = "UTILIZACIÓN";

            gpx_dVSu_semanal.serie1Descripcion = "DISPONIBILIDAD";
            gpx_dVSu_semanal.serie2Descripcion = "UTILIZACIÓN";

            gpx_oVSt_semanal.serie1Descripcion = "DISPONIBILIDAD";
            gpx_oVSt_semanal.serie2Descripcion = "EFICIENCIA DE LA UTILIZACIÓN";

            gpx_oVSt_mensual.serie1Descripcion = "DISPONIBILIDAD";
            gpx_oVSt_mensual.serie2Descripcion = "EFICIENCIA DE LA UTILIZACIÓN";

            gpx_UT_semanal.serie1Descripcion = "UTILIZACIÓN";
            gpx_UT_mensual.serie1Descripcion = "UTILIZACIÓN";

            foreach (var mes in meses)
            {
                var _consulta = datosHomologados.Where
                    (w =>
                        w.homolo.fecha >= mes.fechaInicio &&
                        w.homolo.fecha <= mes.fechaFin
                    );

                if (_consulta.Count() == 0)
                {
                    continue;
                }
                var equipos = datosHomologados.Select(x => x.maquina.noEconomico).Distinct().Count();
                var __hrsProgramadoAlMes = (DatetimeUtils.DiasDiferencia(mes.fechaInicio, mes.fechaFin) * horasDia) * equipos;

                var calculos = CalculosTiempos(_consulta.GroupBy(g => g.homolo.idGrupo).First(), __hrsProgramadoAlMes);


                gpx_dVSu_mensual.categorias.Add(mes.fechaInicio.ToShortDateString() + " al " + mes.fechaFin.ToShortDateString());
                gpx_dVSu_mensual.serie1.Add(Math.Round(((calculos.hrsDisponible / calculos.hrsProgramado) * 100), 2));
                gpx_dVSu_mensual.serie2.Add(Math.Round(((calculos.hrsOperacion / calculos.hrsDisponible) * 100), 2));

                gpx_oVSt_mensual.categorias.Add(mes.fechaInicio.ToShortDateString() + " al " + mes.fechaFin.ToShortDateString());
                gpx_oVSt_mensual.serie1.Add(Math.Round(((calculos.hrsOperacion / calculos.hrsDisponible) * 100), 2));
                gpx_oVSt_mensual.serie2.Add(Math.Round(((calculos.hrsTrabajo / calculos.hrsOperacion) * 100), 2));

                gpx_UT_mensual.categorias.Add(mes.fechaInicio.ToShortDateString() + " al " + mes.fechaFin.ToShortDateString());
                gpx_UT_mensual.serie1.Add(Math.Round(((calculos.hrsTrabajo / calculos.hrsProgramado) * 100), 2));
            }

            foreach (var semana in semanas)
            {
                var _consulta = datosHomologados.Where
                    (w =>
                        w.homolo.fecha >= semana.fechaInicio &&
                        w.homolo.fecha <= semana.fechaFin
                    );

                if (_consulta.Count() == 0)
                {
                    continue;
                }
                var equipos = datosHomologados.Select(x => x.maquina.noEconomico).Distinct().Count();
                var __hrsProgramadoAlaSemana = (DatetimeUtils.DiasDiferencia(semana.fechaInicio, semana.fechaFin) * horasDia) * equipos;

                var calculos = CalculosTiempos(_consulta.GroupBy(g => g.homolo.idGrupo).First(), __hrsProgramadoAlaSemana);

                gpx_dVSu_semanal.categorias.Add(semana.fechaInicio.ToShortDateString() + " al " + semana.fechaFin.ToShortDateString());
                gpx_dVSu_semanal.serie1.Add(Math.Round(((calculos.hrsDisponible / calculos.hrsProgramado) * 100), 2));
                gpx_dVSu_semanal.serie2.Add(Math.Round(((calculos.hrsOperacion / calculos.hrsDisponible) * 100), 2));

                gpx_oVSt_semanal.categorias.Add(semana.fechaInicio.ToShortDateString() + " al " + semana.fechaFin.ToShortDateString());
                gpx_oVSt_semanal.serie1.Add(Math.Round(((calculos.hrsOperacion / calculos.hrsDisponible) * 100), 2));
                gpx_oVSt_semanal.serie2.Add(Math.Round(((calculos.hrsTrabajo / (calculos.hrsOperacion == 0 ? 1 : calculos.hrsOperacion)) * 100), 2));

                gpx_UT_semanal.categorias.Add(semana.fechaInicio.ToShortDateString() + " al " + semana.fechaFin.ToShortDateString());
                gpx_UT_semanal.serie1.Add(Math.Round(((calculos.hrsTrabajo / calculos.hrsProgramado) * 100), 2));
            }
            #endregion

            #region Tabla tiempos
            var tiempos = new TablaTiemposDTO();
            var numEconomicos = disponibilidadVsUtilizacionEconomico.Count > 0 ? disponibilidadVsUtilizacionEconomico.Count : 1;

            tiempos.hrsProgramado = hrsProgramado * __equipos.Count();
            tiempos.hrsProgramadoSM = Math.Round(disponibilidadVsUtilizacionEconomico.Sum(s => s.tiempos.hrsProgramadoSM), 2);
            tiempos.hrsNoProgramadoUM = Math.Round(disponibilidadVsUtilizacionEconomico.Sum(s => s.tiempos.hrsNoProgramadoUM), 2);
            tiempos.hrsMantenimiento = Math.Round(disponibilidadVsUtilizacionEconomico.Sum(s => s.tiempos.hrsMantenimiento), 2);
            tiempos.hrsDisponible = Math.Round(tiempos.hrsProgramado - tiempos.hrsMantenimiento, 2);
            tiempos.hrsParado = Math.Round(disponibilidadVsUtilizacionEconomico.Sum(s => s.tiempos.hrsParado), 2);
            tiempos.hrsOperacion = Math.Round(tiempos.hrsDisponible - tiempos.hrsParado, 2);
            tiempos.hrsDemora = Math.Round(disponibilidadVsUtilizacionEconomico.Sum(s => s.tiempos.hrsDemora), 2);
            tiempos.hrsTrabajo = Math.Round(tiempos.hrsOperacion - tiempos.hrsDemora, 2);

            tiempos.porNoProgramadoUM = Math.Round((tiempos.hrsNoProgramadoUM * 100) / (tiempos.hrsMantenimiento > 0 ? tiempos.hrsMantenimiento : 1), 2);
            tiempos.porProgramadoSM = Math.Round((tiempos.hrsProgramadoSM * 100) / (tiempos.hrsMantenimiento > 0 ? tiempos.hrsMantenimiento : 1), 2);
            tiempos.porDisponible = Math.Round((tiempos.hrsDisponible * 100) / (tiempos.hrsProgramado), 2);

            tiempos.porMantenimiento = Math.Round(100 - tiempos.porDisponible, 2);
            tiempos.porOperacion = Math.Round((tiempos.hrsOperacion * 100) / (tiempos.hrsDisponible > 0 ? tiempos.hrsDisponible : 1), 2);
            tiempos.porParado = Math.Round(100 - tiempos.porOperacion, 2);
            tiempos.porTrabajo = Math.Round((tiempos.hrsTrabajo * 100) / (tiempos.hrsOperacion > 0 ? tiempos.hrsOperacion : 1), 2);
            tiempos.porDemora = Math.Round(100 - tiempos.porTrabajo, 2);
            #endregion

            resultados.tiempos = tiempos;

            resultados.disVsUti_economico = disponibilidadVsUtilizacionEconomico;
            resultados.disVsUti_modelo = disponibilidadVsUtilizacionModelo;
            resultados.disVsUti_grupo = disponibilidadVsUtilizacionGrupo;

            resultados.gpx_disVsUti_economico = gpx_dVSu_economico;
            resultados.gpx_disVsUti_modelo = gpx_dVSu_modelo;
            resultados.gpx_disVsUti_grupo = gpx_dVSu_grupo;
            resultados.gpx_disVsUti_semanal = gpx_dVSu_semanal;
            resultados.gpx_disVsUti_mensual = gpx_dVSu_mensual;

            resultados.gpx_opeVsTra_economico = gpx_oVSt_economico;
            resultados.gpx_opeVsTra_modelo = gpx_oVSt_modelo;
            resultados.gpx_opeVsTra_grupo = gpx_oVSt_grupo;
            resultados.gpx_opeVsTra_semanal = gpx_oVSt_semanal;
            resultados.gpx_opeVsTra_mensual = gpx_oVSt_mensual;

            resultados.gpx_UT_economico = gpx_UT_economico;
            resultados.gpx_UT_modelo = gpx_UT_modelo;
            resultados.gpx_UT_grupo = gpx_UT_grupo;
            resultados.gpx_UT_semanal = gpx_UT_semanal;
            resultados.gpx_UT_mensual = gpx_UT_mensual;

            return resultados;
        }

        private TablaTiemposDTO CalculosTiempos<T>(IGrouping<T, QueryHomologadosDTO> datosHomologados, int hrsProgramado)
        {
            var mantenimientoProgramadoSM = datosHomologados.Where
                (w =>
                    w.codigo.tipoParo == (int)Tipo_ParoEnum.mantenimiento_programado
                );
            var hrsMantenimientoProgramadoSM = mantenimientoProgramadoSM.Select(m => m.homolo.valor).DefaultIfEmpty(0).Sum();

            var mantenimientoNoProgramadoUM = datosHomologados.Where
                (w =>
                    w.codigo.tipoParo == (int)Tipo_ParoEnum.mantenimiento_no_programado
                );
            var hrsMantenimientoNoProgramadoUM = mantenimientoNoProgramadoUM.Select(m => m.homolo.valor).DefaultIfEmpty(0).Sum();

            var hrsMantenimientoMT = hrsMantenimientoProgramadoSM + hrsMantenimientoNoProgramadoUM;
            var porMantenimientoProgramadoSM = (hrsMantenimientoProgramadoSM * 100) / (hrsMantenimientoMT > 0 ? hrsMantenimientoMT : 1);
            var porMantenimientoNoProgramadoUM = 100 - porMantenimientoProgramadoSM;

            var hrsDisponible = hrsProgramado - hrsMantenimientoMT; //-
            var porDisponible = (hrsDisponible * 100) / (hrsProgramado > 0 ? hrsProgramado : 1); //-
            var porMantenimientoMT = 100 - porDisponible; //-

            var parado = datosHomologados.Where
                (w =>
                    w.codigo.tipoParo == (int)Tipo_ParoEnum.sin_utilizar
                );
            var hrsParado = parado.Select(m => m.homolo.valor).DefaultIfEmpty(0).Sum();

            var hrsOperacion = hrsDisponible - hrsParado; //-
            var porOperacion = (hrsOperacion * 100) / (hrsDisponible > 0 ? hrsDisponible : 1); //-
            var porParado = 100 - porOperacion; //-

            var demora = datosHomologados.Where
                (w =>
                    w.codigo.tipoParo == (int)Tipo_ParoEnum.demoras
                );
            var hrsDemora = demora.Select(m => m.homolo.valor).DefaultIfEmpty(0).Sum();

            var hrsTrabajo = hrsOperacion - hrsDemora;
            var porTrabajo = (hrsTrabajo * 100) / (hrsOperacion > 0 ? hrsOperacion : 1);
            var porDemora = 100 - porTrabajo;

            var tiempos = new TablaTiemposDTO();

            tiempos.hrsProgramado = hrsProgramado;
            tiempos.hrsDisponible = hrsDisponible;
            tiempos.porDisponible = porDisponible;
            tiempos.hrsMantenimiento = hrsMantenimientoMT;
            tiempos.porMantenimiento = porMantenimientoMT;
            tiempos.hrsOperacion = hrsOperacion;
            tiempos.porOperacion = porOperacion;
            tiempos.hrsTrabajo = hrsTrabajo;
            tiempos.porTrabajo = porTrabajo;
            tiempos.hrsDemora = hrsDemora;
            tiempos.porDemora = porDemora;
            tiempos.hrsParado = hrsParado;
            tiempos.porParado = porParado;
            tiempos.hrsProgramadoSM = hrsMantenimientoProgramadoSM;
            tiempos.porProgramadoSM = porMantenimientoProgramadoSM;
            tiempos.hrsNoProgramadoUM = hrsMantenimientoNoProgramadoUM;
            tiempos.porNoProgramadoUM = porMantenimientoNoProgramadoUM;

            return tiempos;
        }

        private ResultadoAnualDTO InfoAnual(FiltroDTO filtro)
        {
            DateTime today = DateTime.Now;
            var turnos = _context.tblM_KPI_Homologado_Turnos.FirstOrDefault(x => x.ac == filtro.areaCuenta);
            var horasDia = turnos == null ? 24 : turnos.horas_dia;
            if (today.Year == filtro.fechaInicio.Year)
            {
                filtro.fechaInicio = new DateTime(filtro.fechaInicio.Year, 1, 1);
                filtro.fechaFin = today;
            }
            else
            {
                filtro.fechaInicio = new DateTime(filtro.fechaInicio.Year, 1, 1);
                filtro.fechaFin = new DateTime(filtro.fechaInicio.Year, 12, 31, 23, 59, 59);
            }

            var homologados = _context.tblM_KPI_Homologado.Where
                    (w =>
                        w.fecha >= filtro.fechaInicio &&
                        w.fecha <= filtro.fechaFin &&
                        w.ac == filtro.areaCuenta &&
                        filtro.idEconomicos.Contains(w.idEconomico) &&
                        (filtro.turno == 0 ? true : w.turno == filtro.turno) &&
                            //w.activo && w.valor > 0
                        w.valor > 0
                    ).Join
                    (
                        _context.tblM_KPI_CodigosParo,
                        homolo => homolo.idParo,
                        codigo => codigo.id,
                        (homolo, codigo) => new { homolo, codigo }
                    ).Join
                    (
                        _context.tblM_CatMaquina,
                        homolo => homolo.homolo.idEconomico,
                        maquina => maquina.id,
                        (homolo, maquina) => new { homolo.homolo, homolo.codigo, maquina }
                    ).Join
                    (
                        _context.tblM_CatModeloEquipo,
                        homolo => homolo.homolo.idModelo,
                        modelo => modelo.id,
                        (homolo, modelo) => new { homolo.homolo, homolo.codigo, homolo.maquina, modelo }
                    ).Join
                    (
                        _context.tblM_CatGrupoMaquinaria,
                        homolo => homolo.homolo.idGrupo,
                        grupo => grupo.id,
                        (homolo, grupo) => new { homolo.homolo, homolo.codigo, homolo.maquina, homolo.modelo, grupo }
                    ).Select(m => new QueryHomologadosDTO()
                    {
                        homolo = m.homolo,
                        codigo = m.codigo,
                        maquina = m.maquina,
                        modelo = m.modelo,
                        grupo = m.grupo
                    });

            var hrsProgramadas = DatetimeUtils.DiasDiferencia(filtro.fechaInicio, filtro.fechaFin) * horasDia * filtro.idEconomicos.Count;

            var resultados = new List<CalculosDTO>();

            foreach (var gbGrupo in homologados.GroupBy(g => g.homolo.idGrupo))
            {
                var calculos = new CalculosDTO();

                calculos.tiempos = CalculosTiempos(gbGrupo, hrsProgramadas);

                resultados.Add(calculos);
            }

            var tiempos = new TablaTiemposDTO();
            var grupos = resultados.Count;

            tiempos.hrsProgramado = hrsProgramadas;
            tiempos.hrsProgramadoSM = resultados.Sum(s => s.tiempos.hrsProgramadoSM);
            tiempos.hrsNoProgramadoUM = resultados.Sum(s => s.tiempos.hrsNoProgramadoUM);
            tiempos.hrsMantenimiento = resultados.Sum(s => s.tiempos.hrsMantenimiento);
            tiempos.hrsDisponible = hrsProgramadas - tiempos.hrsMantenimiento;
            tiempos.hrsParado = resultados.Sum(s => s.tiempos.hrsParado);
            tiempos.hrsOperacion = tiempos.hrsDisponible - tiempos.hrsParado;
            tiempos.hrsDemora = resultados.Sum(s => s.tiempos.hrsDemora);
            tiempos.hrsTrabajo = tiempos.hrsOperacion - tiempos.hrsDemora;

            tiempos.porNoProgramadoUM = (tiempos.hrsNoProgramadoUM * 100) / (tiempos.hrsMantenimiento > 0 ? tiempos.hrsMantenimiento : 1);
            tiempos.porProgramadoSM = (tiempos.hrsProgramadoSM * 100) / (tiempos.hrsMantenimiento > 0 ? tiempos.hrsMantenimiento : 1);
            tiempos.porDisponible = (tiempos.hrsDisponible * 100) / (tiempos.hrsProgramado);
            tiempos.porMantenimiento = 100 - tiempos.porDisponible;
            tiempos.porOperacion = (tiempos.hrsOperacion * 100) / (tiempos.hrsDisponible > 0 ? tiempos.hrsDisponible : 1);
            tiempos.porParado = 100 - tiempos.porOperacion;
            tiempos.porTrabajo = (tiempos.hrsTrabajo * 100) / (tiempos.hrsOperacion > 0 ? tiempos.hrsOperacion : 1);
            tiempos.porDemora = 100 - tiempos.porTrabajo;

            var r = new ResultadoAnualDTO();

            r.disponible = Math.Round(tiempos.porDisponible, 2);
            r.operacion = Math.Round(tiempos.porOperacion, 2);
            r.trabajo = Math.Round(tiempos.porTrabajo, 2);
            r.horas = hrsProgramadas;

            return r;
        }

        private MemoryStream GetExcel(FiltroDTO filtro, int hrsProgramado)
        {
            var homologados = _context.tblM_KPI_Homologado.Where
                    (w =>
                        w.fecha >= filtro.fechaInicio &&
                        w.fecha <= filtro.fechaFin &&
                        w.ac == filtro.areaCuenta &&
                        filtro.idEconomicos.Contains(w.idEconomico) &&
                            //w.turno == filtro.turno &&
                            //w.activo && w.valor > 0
                        w.valor > 0
                    ).Join
                    (
                        _context.tblM_KPI_CodigosParo,
                        homolo => homolo.idParo,
                        codigo => codigo.id,
                        (homolo, codigo) => new { homolo, codigo }
                    ).Join
                    (
                        _context.tblM_CatMaquina,
                        homolo => homolo.homolo.idEconomico,
                        maquina => maquina.id,
                        (homolo, maquina) => new { homolo.homolo, homolo.codigo, maquina }
                    ).Join
                    (
                        _context.tblM_CatModeloEquipo,
                        homolo => homolo.homolo.idModelo,
                        modelo => modelo.id,
                        (homolo, modelo) => new { homolo.homolo, homolo.codigo, homolo.maquina, modelo }
                    ).Join
                    (
                        _context.tblM_CatGrupoMaquinaria,
                        homolo => homolo.homolo.idGrupo,
                        grupo => grupo.id,
                        (homolo, grupo) => new { homolo.homolo, homolo.codigo, homolo.maquina, homolo.modelo, grupo }
                    ).Select(m => new QueryHomologadosDTO()
                    {
                        homolo = m.homolo,
                        codigo = m.codigo,
                        maquina = m.maquina,
                        modelo = m.modelo,
                        grupo = m.grupo
                    });

            var detallesExcel = new List<InfoExcelDTO>();

            foreach (var infoHomolo in homologados)
            {
                var dato = new InfoExcelDTO();

                dato.idTurno = infoHomolo.homolo.turno;
                dato.turnoDescripcion = Enum.GetName(typeof(Turnos_Enum), infoHomolo.homolo.turno);
                dato.tipoParo = (Tipo_ParoEnum)infoHomolo.codigo.tipoParo;
                dato.codigo = infoHomolo.codigo.codigo;
                dato.codigoDescripcion = infoHomolo.codigo.descripcion;
                dato.valor = infoHomolo.homolo.valor;
                dato.idEconomico = infoHomolo.homolo.idEconomico;
                dato.noEconomico = infoHomolo.maquina.noEconomico;
                dato.idGrupo = infoHomolo.homolo.idGrupo;
                dato.grupoDescripcion = infoHomolo.grupo.descripcion;
                dato.idModelo = infoHomolo.homolo.idModelo;
                dato.modeloDescripcion = infoHomolo.modelo.descripcion;

                detallesExcel.Add(dato);
            }

            using (ExcelPackage excel = new ExcelPackage())
            {
                var gbTurnos = detallesExcel.GroupBy(g => g.idTurno).OrderBy(o => o.Key);

                foreach (var gbTurno in gbTurnos)
                {
                    var todosLosTurnos = false;

                    do
                    {
                        var hojaExcel = excel.Workbook.Worksheets.Add(filtro.fechaInicio.ToString("dd-MM-yy") + " al " + filtro.fechaFin.ToString("dd-MM-yy") + " " + (todosLosTurnos ? "Total" : gbTurno.First().turnoDescripcion));

                        var headerTitulosEquipos = new List<string> { "Flotilla", "Equipo", "Modelo", "Referencia", "No. Económico" };

                        hojaExcel.Cells.Style.Font.Name = "Arial";
                        hojaExcel.Cells.Style.Font.Size = 12;

                        hojaExcel.View.ShowGridLines = false;

                        for (int i = 0; i < headerTitulosEquipos.Count; i++)
                        {
                            //renglon, columna
                            hojaExcel.Cells[2 + i, 2].Value = headerTitulosEquipos[i];

                            hojaExcel.Cells[2 + i, 2].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                            hojaExcel.Cells[2 + i, 2].Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                            hojaExcel.Cells[2 + i, 2].Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                            hojaExcel.Cells[2 + i, 2].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;

                            hojaExcel.Cells[2 + i, 2].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        }

                        var columnaSeccion = 4;
                        foreach (var gbGrupo in todosLosTurnos ? detallesExcel.GroupBy(g => g.idGrupo).OrderBy(o => o.Key) : gbTurno.GroupBy(g => g.idGrupo).OrderBy(o => o.Key))
                        {
                            var inicioSeccion = columnaSeccion;

                            foreach (var gbModelo in gbGrupo.GroupBy(g => g.idModelo).OrderBy(o => o.Key))
                            {
                                foreach (var gbEconomico in gbModelo.GroupBy(g => g.idEconomico).OrderBy(o => o.Key))
                                {
                                    hojaExcel.Cells[5, columnaSeccion].Value = gbModelo.First().modeloDescripcion;
                                    hojaExcel.Cells[6, columnaSeccion].Value = gbEconomico.First().noEconomico;

                                    hojaExcel.Cells[5, columnaSeccion].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                                    hojaExcel.Cells[5, columnaSeccion].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                                    hojaExcel.Cells[5, columnaSeccion].Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#fce4d6"));
                                    hojaExcel.Cells[5, columnaSeccion].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin, Color.Black);
                                    hojaExcel.Cells[6, columnaSeccion].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                                    hojaExcel.Cells[6, columnaSeccion].Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#c6e0b4"));
                                    hojaExcel.Cells[6, columnaSeccion].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                                    hojaExcel.Cells[6, columnaSeccion].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin, Color.Black);
                                    columnaSeccion++;
                                }
                            }

                            hojaExcel.Cells[6, columnaSeccion].Value = "PROMEDIO";

                            hojaExcel.Cells[6, columnaSeccion].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                            hojaExcel.Cells[6, columnaSeccion].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin, Color.Black);
                            hojaExcel.Cells[6, columnaSeccion].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                            hojaExcel.Cells[6, columnaSeccion].Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#d9d9d9"));

                            hojaExcel.Cells[3, inicioSeccion, 4, columnaSeccion - 1].Merge = true;
                            var cellMerged = hojaExcel.MergedCells[3, inicioSeccion];
                            hojaExcel.Cells[cellMerged].Value = gbGrupo.First().grupoDescripcion;

                            hojaExcel.Cells[cellMerged].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                            hojaExcel.Cells[cellMerged].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                            hojaExcel.Cells[cellMerged].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin, Color.Black);
                            hojaExcel.Cells[cellMerged].Style.Font.Size = 16;
                            hojaExcel.Cells[cellMerged].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                            hojaExcel.Cells[cellMerged].Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#fce4d6"));

                            columnaSeccion++;
                            columnaSeccion++;
                        }

                        hojaExcel.Cells[8, 1].Value = "Código";
                        hojaExcel.Cells[8, 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        hojaExcel.Cells[8, 1].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                        hojaExcel.Cells[8, 1].Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#ffff00"));
                        hojaExcel.Cells[8, 1].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin, Color.Black);

                        hojaExcel.Cells[8, 2].Value = "Categoría / Descripción";
                        hojaExcel.Cells[8, 2].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        hojaExcel.Cells[8, 2].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                        hojaExcel.Cells[8, 2].Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#c6e0b4"));
                        hojaExcel.Cells[8, 2].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin, Color.Black);

                        var rowTotalTrabajo = 0;
                        var rowTotalDemora = 0;
                        var rowTotalParado = 0;
                        var rowTotalMantenimientoProgramado = 0;
                        var rowTotalMantenimientoNoProgramado = 0;
                        var rowTotalMantenimiento = 0;

                        string[] formulas = 
                    {
                        "Tiempo Programado (S)",
                        "Tiempo Disponible (A)",
                        "Tiempo de Operación (OPT)",
                        "Tiempo de Trabajo (WK)",
                        "Disponibilidad Física (PA%)",
                        "Uso de la Disponibilidad (UA%)",
                        "Utilización (UT%)",
                        "Eficiencia de Trabajo (UE%)"
                    };

                        var renglonSeccion = 10;
                        foreach (var gbParo in todosLosTurnos ? detallesExcel.GroupBy(g => g.tipoParo).OrderBy(o => o.Key) : gbTurno.GroupBy(g => g.tipoParo).OrderBy(o => o.Key))
                        {
                            hojaExcel.Cells[renglonSeccion, 1, renglonSeccion, 2].Merge = true;
                            var cellMerged = hojaExcel.MergedCells[renglonSeccion, 1];
                            hojaExcel.Cells[cellMerged].Value = EnumExtensions.GetDescription(gbParo.First().tipoParo);

                            hojaExcel.Cells[cellMerged].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                            hojaExcel.Cells[cellMerged].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                            hojaExcel.Cells[cellMerged].Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#00b0f0"));
                            hojaExcel.Cells[cellMerged].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin, Color.Black);

                            renglonSeccion++;

                            var renglonSubHeaderCategoria = renglonSeccion;

                            hojaExcel.Cells[renglonSubHeaderCategoria, 1, renglonSubHeaderCategoria, 2].Merge = true;
                            hojaExcel.Cells[renglonSubHeaderCategoria, 1].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                            hojaExcel.Cells[renglonSubHeaderCategoria, 1].Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#d9d9d9"));
                            hojaExcel.Cells[renglonSubHeaderCategoria, 1].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin, Color.Black);

                            renglonSeccion++;

                            foreach (var gbCodigo in gbParo.GroupBy(g => g.codigo).OrderBy(o => o.Key))
                            {
                                hojaExcel.Cells[renglonSeccion, 1].Value = gbCodigo.First().codigo;
                                hojaExcel.Cells[renglonSeccion, 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                                hojaExcel.Cells[renglonSeccion, 1].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                                hojaExcel.Cells[renglonSeccion, 1].Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#acb9ca"));
                                hojaExcel.Cells[renglonSeccion, 1].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin, Color.Black);

                                hojaExcel.Cells[renglonSeccion, 2].Value = gbCodigo.First().codigoDescripcion;
                                hojaExcel.Cells[renglonSeccion, 2].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                                hojaExcel.Cells[renglonSeccion, 2].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                                hojaExcel.Cells[renglonSeccion, 2].Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#ededed"));
                                hojaExcel.Cells[renglonSeccion, 2].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin, Color.Black);

                                var columnaSeccionValores = 4;

                                foreach (var gbGrupo in todosLosTurnos ? detallesExcel.GroupBy(g => g.idGrupo).OrderBy(o => o.Key) : gbTurno.GroupBy(g => g.idGrupo).OrderBy(o => o.Key))
                                {
                                    var inicioSeccionValores = columnaSeccionValores;

                                    foreach (var gbModelo in gbGrupo.GroupBy(g => g.idModelo).OrderBy(o => o.Key))
                                    {
                                        foreach (var gbEconomico in gbModelo.GroupBy(g => g.idEconomico).OrderBy(o => o.Key))
                                        {
                                            hojaExcel.Cells[renglonSeccion, columnaSeccionValores].Value = gbEconomico.Where(w => w.tipoParo == gbParo.Key && w.codigo == gbCodigo.Key).Sum(s => s.valor);
                                            hojaExcel.Cells[renglonSeccion, columnaSeccionValores].Style.Numberformat.Format = "#,##0.00";
                                            hojaExcel.Cells[renglonSeccion, columnaSeccionValores].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;
                                            hojaExcel.Cells[renglonSeccion, columnaSeccionValores].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                                            hojaExcel.Cells[renglonSeccion, columnaSeccionValores].Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#ddebf7"));
                                            hojaExcel.Cells[renglonSeccion, columnaSeccionValores].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin, Color.Black);

                                            columnaSeccionValores++;
                                        }
                                    }

                                    hojaExcel.Cells[renglonSubHeaderCategoria, inicioSeccionValores, renglonSubHeaderCategoria, columnaSeccionValores].Merge = true;
                                    hojaExcel.Cells[renglonSubHeaderCategoria, inicioSeccionValores].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                                    hojaExcel.Cells[renglonSubHeaderCategoria, inicioSeccionValores].Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#d9d9d9"));

                                    var letraColumnaInicioRango = OfficeOpenXml.ExcelCellAddress.GetColumnLetter(columnaSeccionValores - gbGrupo.GroupBy(g => g.idEconomico).Count());
                                    var letraColumnaFinal = OfficeOpenXml.ExcelCellAddress.GetColumnLetter(columnaSeccionValores - 1);
                                    //hojaExcel.Cells[renglonSeccion, columnaSeccionValores].Formula = "SUM(" + letraColumnaInicioRango + renglonSeccion + ":" + letraColumnaFinal + renglonSeccion + ") / COLUMNS(" + letraColumnaInicioRango + renglonSeccion + ":" + letraColumnaFinal + renglonSeccion + ")";
                                    hojaExcel.Cells[renglonSeccion, columnaSeccionValores].Value = gbGrupo.Where(w => w.tipoParo == gbParo.Key && w.codigo == gbCodigo.Key).Sum(s => s.valor) / gbGrupo.GroupBy(g => g.idEconomico).Count();
                                    hojaExcel.Cells[renglonSeccion, columnaSeccionValores].Style.Numberformat.Format = "#,##0.00";
                                    hojaExcel.Cells[renglonSeccion, columnaSeccionValores].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;
                                    hojaExcel.Cells[renglonSeccion, columnaSeccionValores].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                                    hojaExcel.Cells[renglonSeccion, columnaSeccionValores].Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#ddebf7"));
                                    hojaExcel.Cells[renglonSeccion, columnaSeccionValores].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin, Color.Black);


                                    columnaSeccionValores++;
                                    columnaSeccionValores++;
                                }

                                renglonSeccion++;
                            }

                            hojaExcel.Cells[renglonSeccion, 1, renglonSeccion, 2].Merge = true;
                            hojaExcel.Cells[renglonSeccion, 1].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                            hojaExcel.Cells[renglonSeccion, 1].Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#d9d9d9"));

                            renglonSeccion++;

                            hojaExcel.Cells[renglonSeccion, 1, renglonSeccion, 2].Merge = true;
                            var cellMergedTotalSeccion = hojaExcel.MergedCells[renglonSeccion, 1];
                            hojaExcel.Cells[cellMergedTotalSeccion].Value = "Total " + EnumExtensions.GetDescription(gbParo.First().tipoParo);
                            hojaExcel.Cells[cellMergedTotalSeccion].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                            hojaExcel.Cells[cellMergedTotalSeccion].Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#acb9ca"));
                            hojaExcel.Cells[cellMergedTotalSeccion].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin, Color.Black);
                            hojaExcel.Cells[cellMergedTotalSeccion].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;

                            switch (gbParo.First().tipoParo)
                            {
                                case Tipo_ParoEnum.trabajo:
                                    rowTotalTrabajo = renglonSeccion;
                                    break;
                                case Tipo_ParoEnum.demoras:
                                    rowTotalDemora = renglonSeccion;
                                    break;
                                case Tipo_ParoEnum.sin_utilizar:
                                    rowTotalParado = renglonSeccion;
                                    break;

                                case Tipo_ParoEnum.mantenimiento_programado:
                                    rowTotalMantenimientoProgramado = renglonSeccion;
                                    break;
                                case Tipo_ParoEnum.mantenimiento_no_programado:
                                    rowTotalMantenimientoNoProgramado = renglonSeccion;
                                    break;
                            }

                            var columnaSeccionValoresTotales = 4;

                            foreach (var gbGrupo in todosLosTurnos ? detallesExcel.GroupBy(g => g.idGrupo).OrderBy(o => o.Key) : gbTurno.GroupBy(g => g.idGrupo).OrderBy(o => o.Key))
                            {
                                var inicioSeccionGris = columnaSeccionValoresTotales;
                                foreach (var gbModelo in gbGrupo.GroupBy(g => g.idModelo).OrderBy(o => o.Key))
                                {
                                    foreach (var gbEconomico in gbModelo.GroupBy(g => g.idEconomico).OrderBy(o => o.Key))
                                    {
                                        var letraColumna = OfficeOpenXml.ExcelCellAddress.GetColumnLetter(columnaSeccionValoresTotales);
                                        var renglonInicioRango = renglonSeccion - (gbParo.GroupBy(g => g.codigo).Count() + 1);
                                        //hojaExcel.Cells[renglonSeccion, columnaSeccionValoresTotales].Formula = "SUM(" + letraColumna + renglonInicioRango + ":" + letraColumna + (renglonSeccion - 2) + ")";
                                        hojaExcel.Cells[renglonSeccion, columnaSeccionValoresTotales].Value = gbParo.Where(w => w.idEconomico == gbEconomico.Key).Sum(s => s.valor);
                                        hojaExcel.Cells[renglonSeccion, columnaSeccionValoresTotales].Style.Numberformat.Format = "#,##0.00";
                                        hojaExcel.Cells[renglonSeccion, columnaSeccionValoresTotales].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                                        hojaExcel.Cells[renglonSeccion, columnaSeccionValoresTotales].Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#acb9ca"));
                                        hojaExcel.Cells[renglonSeccion, columnaSeccionValoresTotales].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin, Color.Black);

                                        columnaSeccionValoresTotales++;
                                    }
                                }

                                {
                                    var letraColumna = OfficeOpenXml.ExcelCellAddress.GetColumnLetter(columnaSeccionValoresTotales);
                                    var renglonInicioRango = renglonSeccion - (gbParo.GroupBy(g => g.codigo).Count() + 1);
                                    //hojaExcel.Cells[renglonSeccion, columnaSeccionValoresTotales].Formula = "SUM(" + letraColumna + renglonInicioRango + ":" + letraColumna + (renglonSeccion - 2) + ")";
                                    hojaExcel.Cells[renglonSeccion, columnaSeccionValoresTotales].Value = gbGrupo.Where(w => w.tipoParo == gbParo.Key).Sum(s => s.valor) / gbGrupo.GroupBy(g => g.idEconomico).Count();
                                    hojaExcel.Cells[renglonSeccion, columnaSeccionValoresTotales].Style.Numberformat.Format = "#,##0.00";
                                    hojaExcel.Cells[renglonSeccion, columnaSeccionValoresTotales].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                                    hojaExcel.Cells[renglonSeccion, columnaSeccionValoresTotales].Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#acb9ca"));
                                    hojaExcel.Cells[renglonSeccion, columnaSeccionValoresTotales].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin, Color.Black);

                                    hojaExcel.Cells[renglonSeccion - 1, inicioSeccionGris, renglonSeccion - 1, columnaSeccionValoresTotales].Merge = true;
                                    hojaExcel.Cells[renglonSeccion - 1, inicioSeccionGris].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                                    hojaExcel.Cells[renglonSeccion - 1, inicioSeccionGris].Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#d9d9d9"));

                                    columnaSeccionValoresTotales++;
                                    columnaSeccionValoresTotales++;
                                }
                            }

                            renglonSeccion++;
                            renglonSeccion++;
                        }

                        hojaExcel.Cells[renglonSeccion, 1, renglonSeccion, 2].Merge = true;
                        var mergeCell = hojaExcel.MergedCells[renglonSeccion, 1];
                        hojaExcel.Cells[mergeCell].Value = "Total Mantenimiento (MT)";
                        hojaExcel.Cells[mergeCell].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin, Color.Black);
                        hojaExcel.Cells[mergeCell].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;

                        rowTotalMantenimiento = renglonSeccion;

                        {
                            var columnaInicioCalculos = 4;
                            foreach (var gbGrupo in todosLosTurnos ? detallesExcel.GroupBy(g => g.idGrupo).OrderBy(o => o.Key) : gbTurno.GroupBy(g => g.idGrupo).OrderBy(o => o.Key))
                            {
                                foreach (var gbModelo in gbGrupo.GroupBy(g => g.idModelo).OrderBy(o => o.Key))
                                {
                                    foreach (var gbEconomico in gbModelo.GroupBy(g => g.idEconomico).OrderBy(o => o.Key))
                                    {
                                        hojaExcel.Cells[renglonSeccion, columnaInicioCalculos].Value = gbEconomico.Where
                                            (w =>
                                                w.tipoParo == Tipo_ParoEnum.mantenimiento_programado ||
                                                w.tipoParo == Tipo_ParoEnum.mantenimiento_no_programado
                                            ).Sum(s => s.valor);
                                        hojaExcel.Cells[renglonSeccion, columnaInicioCalculos].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin, Color.Black);
                                        hojaExcel.Cells[renglonSeccion, columnaInicioCalculos].Style.Numberformat.Format = "#,##0.00";

                                        columnaInicioCalculos++;
                                    }
                                }

                                {
                                    hojaExcel.Cells[renglonSeccion, columnaInicioCalculos].Value = gbGrupo.Where
                                        (w =>
                                            w.tipoParo == Tipo_ParoEnum.mantenimiento_programado ||
                                            w.tipoParo == Tipo_ParoEnum.mantenimiento_no_programado
                                        ).Sum(s => s.valor) / gbGrupo.GroupBy(g => g.idEconomico).Count();
                                    hojaExcel.Cells[renglonSeccion, columnaInicioCalculos].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin, Color.Black);
                                    hojaExcel.Cells[renglonSeccion, columnaInicioCalculos].Style.Numberformat.Format = "#,##0.00";
                                }

                                columnaInicioCalculos++;
                                columnaInicioCalculos++;
                            }
                        }

                        renglonSeccion++;
                        renglonSeccion++;

                        for (int i = 0; i < formulas.Length; i++)
                        {
                            hojaExcel.Cells[renglonSeccion, 1, renglonSeccion, 2].Merge = true;
                            var cellMerged = hojaExcel.MergedCells[renglonSeccion, 1];
                            hojaExcel.Cells[cellMerged].Value = formulas[i];
                            hojaExcel.Cells[cellMerged].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin, Color.Black);
                            hojaExcel.Cells[cellMerged].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                            if (i % 2 == 0)
                            {
                                hojaExcel.Cells[cellMerged].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                                hojaExcel.Cells[cellMerged].Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#d9d9d9"));
                            }

                            var columnaInicioCalculos = 4;
                            foreach (var gbGrupo in todosLosTurnos ? detallesExcel.GroupBy(g => g.idGrupo).OrderBy(o => o.Key) : gbTurno.GroupBy(g => g.idGrupo).OrderBy(o => o.Key))
                            {
                                foreach (var gbModelo in gbGrupo.GroupBy(g => g.idModelo).OrderBy(o => o.Key))
                                {
                                    foreach (var gbEconomico in gbModelo.GroupBy(g => g.idEconomico).OrderBy(o => o.Key))
                                    {
                                        var letraColumna = OfficeOpenXml.ExcelCellAddress.GetColumnLetter(columnaInicioCalculos);

                                        var __mantenimiento = gbEconomico.Where
                                                    (w =>
                                                        w.tipoParo == Tipo_ParoEnum.mantenimiento_programado ||
                                                        w.tipoParo == Tipo_ParoEnum.mantenimiento_no_programado
                                                    ).Sum(s => s.valor);
                                        var __disponible = hrsProgramado - __mantenimiento;
                                        var __parado = gbEconomico.Where(w => w.tipoParo == Tipo_ParoEnum.sin_utilizar).Sum(s => s.valor);
                                        var __operacion = __disponible - __parado;
                                        var __demoras = gbEconomico.Where(w => w.tipoParo == Tipo_ParoEnum.demoras).Sum(s => s.valor);
                                        var __trabajo = __operacion - __demoras;

                                        switch (i)
                                        {
                                            case 0:
                                                hojaExcel.Cells[renglonSeccion, columnaInicioCalculos].Value = hrsProgramado;
                                                break;
                                            case 1:
                                                //hojaExcel.Cells[renglonSeccion, columnaInicioCalculos].Formula = "SUM(" + hrsProgramado + "-" + letraColumna + rowTotalMantenimiento + ")";
                                                hojaExcel.Cells[renglonSeccion, columnaInicioCalculos].Value = __disponible;
                                                break;
                                            case 2:
                                                //hojaExcel.Cells[renglonSeccion, columnaInicioCalculos].Formula = "SUM(" + letraColumna + (renglonSeccion - 1) + "-" + letraColumna + rowTotalParado + ")";
                                                hojaExcel.Cells[renglonSeccion, columnaInicioCalculos].Value = __operacion;
                                                break;
                                            case 3:
                                                //hojaExcel.Cells[renglonSeccion, columnaInicioCalculos].Formula = "SUM(" + letraColumna + (renglonSeccion - 1) + "-" + letraColumna + rowTotalDemora + ")";
                                                hojaExcel.Cells[renglonSeccion, columnaInicioCalculos].Value = __trabajo;
                                                break;
                                            case 4:
                                                //hojaExcel.Cells[renglonSeccion, columnaInicioCalculos].Formula = "SUM(" + letraColumna + (renglonSeccion - 3) + "/" + letraColumna + (renglonSeccion - 4) + ")";
                                                hojaExcel.Cells[renglonSeccion, columnaInicioCalculos].Value = __disponible / hrsProgramado;
                                                hojaExcel.Cells[renglonSeccion, columnaInicioCalculos].Style.Numberformat.Format = "#0.00%";
                                                break;
                                            case 5:
                                                //hojaExcel.Cells[renglonSeccion, columnaInicioCalculos].Formula = "SUM(" + letraColumna + (renglonSeccion - 3) + "/" + letraColumna + (renglonSeccion - 4) + ")";
                                                hojaExcel.Cells[renglonSeccion, columnaInicioCalculos].Value = __operacion / __disponible;
                                                hojaExcel.Cells[renglonSeccion, columnaInicioCalculos].Style.Numberformat.Format = "#0.00%";
                                                break;
                                            case 6:
                                                //hojaExcel.Cells[renglonSeccion, columnaInicioCalculos].Formula = "SUM(" + letraColumna + (renglonSeccion - 3) + "/" + letraColumna + (renglonSeccion - 6) + ")";
                                                hojaExcel.Cells[renglonSeccion, columnaInicioCalculos].Value = __trabajo / hrsProgramado;
                                                hojaExcel.Cells[renglonSeccion, columnaInicioCalculos].Style.Numberformat.Format = "#0.00%";
                                                break;
                                            case 7:
                                                //hojaExcel.Cells[renglonSeccion, columnaInicioCalculos].Formula = "SUM(" + letraColumna + (renglonSeccion - 4) + "/" + letraColumna + (renglonSeccion - 5) + ")";
                                                hojaExcel.Cells[renglonSeccion, columnaInicioCalculos].Value = __trabajo / (__operacion == 0 ? 1 : __operacion);
                                                hojaExcel.Cells[renglonSeccion, columnaInicioCalculos].Style.Numberformat.Format = "#0.00%";
                                                break;
                                        }

                                        if (i % 2 == 0)
                                        {
                                            hojaExcel.Cells[renglonSeccion, columnaInicioCalculos].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                                            hojaExcel.Cells[renglonSeccion, columnaInicioCalculos].Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#d9d9d9"));
                                        }
                                        hojaExcel.Cells[renglonSeccion, columnaInicioCalculos].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin, Color.Black);

                                        columnaInicioCalculos++;
                                    }
                                }

                                {
                                    var letraColumna = OfficeOpenXml.ExcelCellAddress.GetColumnLetter(columnaInicioCalculos);

                                    var __mantenimiento = gbGrupo.Where
                                                    (w =>
                                                        w.tipoParo == Tipo_ParoEnum.mantenimiento_programado ||
                                                        w.tipoParo == Tipo_ParoEnum.mantenimiento_no_programado
                                                    ).Sum(s => s.valor) / gbGrupo.GroupBy(g => g.idEconomico).Count();
                                    var __disponible = hrsProgramado - __mantenimiento;
                                    var __parado = gbGrupo.Where(w => w.tipoParo == Tipo_ParoEnum.sin_utilizar).Sum(s => s.valor) / gbGrupo.GroupBy(g => g.idEconomico).Count();
                                    var __operacion = __disponible - __parado;
                                    var __demoras = gbGrupo.Where(w => w.tipoParo == Tipo_ParoEnum.demoras).Sum(s => s.valor) / gbGrupo.GroupBy(g => g.idEconomico).Count();
                                    var __trabajo = __operacion - __demoras;

                                    switch (i)
                                    {
                                        case 0:
                                            hojaExcel.Cells[renglonSeccion, columnaInicioCalculos].Value = hrsProgramado;
                                            break;
                                        case 1:
                                            //hojaExcel.Cells[renglonSeccion, columnaInicioCalculos].Formula = "SUM(" + hrsProgramado + "-" + letraColumna + rowTotalMantenimiento + ")";
                                            hojaExcel.Cells[renglonSeccion, columnaInicioCalculos].Value = __disponible;
                                            break;
                                        case 2:
                                            //hojaExcel.Cells[renglonSeccion, columnaInicioCalculos].Formula = "SUM(" + letraColumna + (renglonSeccion - 1) + "-" + letraColumna + rowTotalParado + ")";
                                            hojaExcel.Cells[renglonSeccion, columnaInicioCalculos].Value = __operacion;
                                            break;
                                        case 3:
                                            //hojaExcel.Cells[renglonSeccion, columnaInicioCalculos].Formula = "SUM(" + letraColumna + (renglonSeccion - 1) + "-" + letraColumna + rowTotalDemora + ")";
                                            hojaExcel.Cells[renglonSeccion, columnaInicioCalculos].Value = __trabajo;
                                            break;
                                        case 4:
                                            //hojaExcel.Cells[renglonSeccion, columnaInicioCalculos].Formula = "SUM(" + letraColumna + (renglonSeccion - 3) + "/" + letraColumna + (renglonSeccion - 4) + ")";
                                            hojaExcel.Cells[renglonSeccion, columnaInicioCalculos].Value = __disponible / hrsProgramado;
                                            hojaExcel.Cells[renglonSeccion, columnaInicioCalculos].Style.Numberformat.Format = "#0.00%";
                                            break;
                                        case 5:
                                            //hojaExcel.Cells[renglonSeccion, columnaInicioCalculos].Formula = "SUM(" + letraColumna + (renglonSeccion - 3) + "/" + letraColumna + (renglonSeccion - 4) + ")";
                                            hojaExcel.Cells[renglonSeccion, columnaInicioCalculos].Value = __operacion / __disponible;
                                            hojaExcel.Cells[renglonSeccion, columnaInicioCalculos].Style.Numberformat.Format = "#0.00%";
                                            break;
                                        case 6:
                                            //hojaExcel.Cells[renglonSeccion, columnaInicioCalculos].Formula = "SUM(" + letraColumna + (renglonSeccion - 3) + "/" + letraColumna + (renglonSeccion - 6) + ")";
                                            hojaExcel.Cells[renglonSeccion, columnaInicioCalculos].Value = __trabajo / hrsProgramado;
                                            hojaExcel.Cells[renglonSeccion, columnaInicioCalculos].Style.Numberformat.Format = "#0.00%";
                                            break;
                                        case 7:
                                            //hojaExcel.Cells[renglonSeccion, columnaInicioCalculos].Formula = "SUM(" + letraColumna + (renglonSeccion - 4) + "/" + letraColumna + (renglonSeccion - 5) + ")";
                                            hojaExcel.Cells[renglonSeccion, columnaInicioCalculos].Value = __trabajo / __operacion;
                                            hojaExcel.Cells[renglonSeccion, columnaInicioCalculos].Style.Numberformat.Format = "#0.00%";
                                            break;
                                    }

                                    if (i % 2 == 0)
                                    {
                                        hojaExcel.Cells[renglonSeccion, columnaInicioCalculos].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                                        hojaExcel.Cells[renglonSeccion, columnaInicioCalculos].Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#d9d9d9"));
                                    }
                                    hojaExcel.Cells[renglonSeccion, columnaInicioCalculos].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin, Color.Black);
                                }

                                columnaInicioCalculos++;
                                columnaInicioCalculos++;
                            }

                            renglonSeccion++;
                        }

                        ExcelRange rangoCompleto = hojaExcel.Cells[1, 1, hojaExcel.Dimension.End.Row, hojaExcel.Dimension.End.Column];

                        hojaExcel.Cells[1, 1, hojaExcel.Dimension.End.Row, hojaExcel.Dimension.End.Column].Style.Font.Bold = true;

                        rangoCompleto.AutoFitColumns();

                        if (gbTurno.Key == gbTurnos.Last().Key && !todosLosTurnos)
                        {
                            todosLosTurnos = true;
                        }
                        else
                        {
                            todosLosTurnos = false;
                        }

                    } while (todosLosTurnos);
                }

                var bytes = new MemoryStream();

                using (var stream = new MemoryStream())
                {
                    excel.SaveAs(stream);
                    bytes = stream;
                }

                return bytes;
            }
        }

        public List<ComboDTO> FillCboCC()
        {
            
                var lstCCHomologados = _context.tblM_KPI_Homologado.Select(x => x.ac).Distinct().ToList();
                if (lstCCHomologados.Count() > 0)
                {
                    List<string> lstAC = new List<string>();

                    var dataCC = _context.tblP_CC.Where(x => x.estatus && lstCCHomologados.Contains(x.areaCuenta)).ToList();
                    var returnCC = dataCC.Select(x => new ComboDTO
                    {
                        Value = x.areaCuenta.ToString(),
                        Text = x.areaCuenta + " " + x.descripcion
                    }).ToList();
                    return returnCC;
                }
            
           if(vSesiones.sesionEmpresaActual==3 || vSesiones.sesionEmpresaActual==6)
            {

                var ccEmpresas = _context.tblP_CC.Where(x => x.estatus && lstCCHomologados.Contains(x.areaCuenta)).ToList();
                var returnCC = ccEmpresas.Select(x => new ComboDTO
                {
                    Value = x.cc.ToString(),
                    Text = x.cc + " " + x.descripcion
                }).ToList();

                return returnCC;
            }

           
        
            return null;
        }

        public List<ComboDTO> FillCboGrupos()
        {
            var dataGrupos = _context.tblM_CatGrupoMaquinaria.Where(x => x.estatus && x.tipoEquipoID == 1).Select(x => new ComboDTO
            {
                Value = x.id.ToString(),
                Text = x.descripcion
            }).ToList();
            return dataGrupos;
        }

        public List<ComboDTO> FillCboModelos(List<int> lstGrupoID)
        {
            var dataModelos = _context.tblM_CatModeloEquipo.Where(x => lstGrupoID.Count > 0 ? lstGrupoID.Contains(x.idGrupo ?? 0) : x.id == x.id && x.estatus).Select(x => new ComboDTO
            {
                Value = x.id.ToString(),
                Text = x.descripcion
            }).ToList();
            return dataModelos;
        }
        public List<ComboDTO> FillCboGruposEnCaptura(List<string> lstCC)
        {
            var ecos = _context.tblM_KPI_Homologado.Where(x=>lstCC.Contains(x.ac)).Select(x=>x.idGrupo).Distinct().ToList();
            var dataGrupos = _context.tblM_CatGrupoMaquinaria.Where(x => x.estatus && ecos.Contains(x.id)).Select(x => new ComboDTO
            {
                Value = x.id.ToString(),
                Text = x.descripcion
            }).ToList();
            return dataGrupos;
        }

        public List<ComboDTO> FillCboModelosEnCaptura(List<string> lstCC,List<int> lstGrupoID)
        {
            var ecos = _context.tblM_KPI_Homologado.Where(x => lstCC.Contains(x.ac) && lstGrupoID.Contains(x.idGrupo)).Select(x => x.idModelo).Distinct().ToList();
            var dataModelos = _context.tblM_CatModeloEquipo.Where(x => x.estatus && ecos.Contains(x.id)).Select(x => new ComboDTO
            {
                Value = x.id.ToString(),
                Text = x.descripcion
            }).ToList();
            return dataModelos;
        }
        public List<ComboDTO> FillCboEconomico(List<string> lstCC, List<int> lstGrupoID, List<int> lstModeloID)
        {
            var dataModelos = _context.tblM_CatMaquina.Where(x => (lstCC.Count > 0 ? lstCC.Contains(x.centro_costos) : false) &&
                                                                  (lstGrupoID.Count > 0 ? lstGrupoID.Contains(x.grupoMaquinariaID) : false) &&
                                                                  (lstModeloID.Count > 0 ? lstModeloID.Contains(x.modeloEquipoID) : false) && x.estatus == 1).ToList();
            var data = dataModelos.Select(x => new ComboDTO
            {
                Value = x.id.ToString(),
                Text = x.noEconomico
            }).ToList();

            return data;
        }

        public Dictionary<string, object> GetDatosGraficaParosReservaSinUso(IQueryable<QueryHomologadosDTO> datosHomologados, string areaCuenta)
        {
            var resultados = new Dictionary<string, object>();
            try
            {
                var dataCodigosParo = datosHomologados.Where(x => x.codigo.tipoParo == (int)Tipo_ParoEnum.sin_utilizar).ToList();
                //var lstCodigoStr = dataCodigosParo.Select(x => x.codigo.codigo + " / " + x.codigo.descripcion).ToList();
                var lstCodigoStr = dataCodigosParo.Select(x => x.codigo.descripcion).ToList();
                var lstCodigos = lstCodigoStr.GroupBy(x => x).ToList();
                var lstCodigosParoID = dataCodigosParo.GroupBy(x => x.codigo.descripcion).ToList();
                List<string> lstCodigosJS = new List<string>();
                List<int> lstFrecuenciaCodigosParo = new List<int>();
                List<GraficaKPIHomDTO> datos = new List<GraficaKPIHomDTO>();

                if (lstCodigosParoID.Count > 0)
                {
                    List<string> lstValorCodigos = new List<string>();
                    foreach (var i in lstCodigosParoID)
                    {
                        GraficaKPIHomDTO dato = new GraficaKPIHomDTO();
                        string codigoFormato = i.Key;
                        string codigo = i.First().codigo.codigo;
                        if (!string.IsNullOrEmpty(codigo))
                        {
                            lstCodigosJS.Add(codigoFormato);
                            dato.codigosJS = codigoFormato;
                            var ValorCodigo = dataCodigosParo.Where(x => x.homolo.codigoParo == codigo).Select(x => x.homolo.valor).ToList();
                            decimal sumaValorCodigo = 0;
                            for (int y = 0; y < ValorCodigo.Count(); y++)
                            {
                                sumaValorCodigo += Convert.ToDecimal(ValorCodigo[y]);
                            }
                            lstValorCodigos.Add(sumaValorCodigo.ToString());
                            dato.valorCodigo = sumaValorCodigo;
                        }
                        var frecuenciaCodigosParo = _context.tblM_KPI_Homologado.Where(x => /*x.activo &&*/ x.valor > 0 && x.codigoParo == codigo && x.ac == areaCuenta &&
                                                                                             x.idTipoParo == (int)Tipo_ParoEnum.sin_utilizar && x.validado).Count();
                        dato.frecuenciaCodigoParo = frecuenciaCodigosParo;
                        lstFrecuenciaCodigosParo.Add(frecuenciaCodigosParo);
                        datos.Add(dato);
                    }
                    datos = datos.OrderByDescending(x => x.valorCodigo).ToList();
                    resultados.Add("lstValorCodigos", datos.OrderByDescending(x => x.valorCodigo).Select(x => x.valorCodigo.ToString()).ToList());
                    resultados.Add("lstCodigosJS", datos.OrderByDescending(x => x.valorCodigo).Select(x => x.codigosJS).ToList());
                    resultados.Add("lstFrecuenciaCodigosParo", datos.OrderByDescending(x => x.frecuenciaCodigoParo).Select(x => x.frecuenciaCodigoParo).ToList());
                }
            }
            catch (Exception ex)
            {
                resultados.Add(SUCCESS, false);
                resultados.Add(MESSAGE, ex.Message);
            }
            return resultados;
        }

        public Dictionary<string, object> GetDatosGraficaParosDemora(IQueryable<QueryHomologadosDTO> datosHomologados, string areaCuenta)
        {
            var resultados = new Dictionary<string, object>();
            try
            {
                var dataCodigosParo = datosHomologados.Where(x => x.codigo.tipoParo == (int)Tipo_ParoEnum.demoras).ToList();
                //var lstCodigoStr = dataCodigosParo.Select(x => x.codigo.codigo + " / " + x.codigo.descripcion).ToList();
                var lstCodigoStr = dataCodigosParo.Select(x => x.codigo.descripcion).ToList();
                var lstCodigos = lstCodigoStr.GroupBy(x => x).ToList();
                var lstCodigosParoID = dataCodigosParo.GroupBy(x => x.codigo.descripcion).ToList();
                List<string> lstCodigosJS = new List<string>();
                List<int> lstFrecuenciaCodigosParo = new List<int>();
                List<GraficaKPIHomDTO> datos = new List<GraficaKPIHomDTO>();

                if (lstCodigosParoID.Count > 0)
                {
                    List<string> lstValorCodigos = new List<string>();

                    foreach (var i in lstCodigosParoID)
                    {
                        GraficaKPIHomDTO dato = new GraficaKPIHomDTO();
                        string codigoFormato = i.Key;
                        string codigo = i.First().codigo.codigo;
                        if (!string.IsNullOrEmpty(codigo))
                        {
                            lstCodigosJS.Add(codigoFormato);
                            dato.codigosJS = codigoFormato;
                            var ValorCodigo = dataCodigosParo.Where(x => x.homolo.codigoParo == codigo).Select(x => x.homolo.valor).ToList();
                            decimal sumaValorCodigo = 0;
                            for (int y = 0; y < ValorCodigo.Count(); y++)
                            {
                                sumaValorCodigo += Convert.ToDecimal(ValorCodigo[y]);
                            }
                            lstValorCodigos.Add(sumaValorCodigo.ToString());
                            dato.valorCodigo = sumaValorCodigo;
                        }
                        var frecuenciaCodigosParo = _context.tblM_KPI_Homologado.Where(x => /*x.activo &&*/ x.valor > 0 && x.codigoParo == codigo && x.ac == areaCuenta &&
                                                                                             x.idTipoParo == (int)Tipo_ParoEnum.demoras && x.validado).Count();
                        dato.frecuenciaCodigoParo = frecuenciaCodigosParo;
                        lstFrecuenciaCodigosParo.Add(frecuenciaCodigosParo);
                        datos.Add(dato);
                    }
                    datos = datos.OrderByDescending(x => x.valorCodigo).ToList();
                    resultados.Add("lstValorCodigos", datos.OrderByDescending(x => x.valorCodigo).Select(x => x.valorCodigo.ToString()).ToList());
                    resultados.Add("lstCodigosJS", datos.OrderByDescending(x => x.valorCodigo).Select(x => x.codigosJS).ToList());
                    resultados.Add("lstFrecuenciaCodigosParo", datos.OrderByDescending(x => x.frecuenciaCodigoParo).Select(x => x.frecuenciaCodigoParo).ToList());
                }
            }
            catch (Exception ex)
            {
                resultados.Add(SUCCESS, false);
                resultados.Add(MESSAGE, ex.Message);
            }
            return resultados;
        }

        public Dictionary<string, object> GetDatosGraficaParosMantenimientos(IQueryable<QueryHomologadosDTO> datosHomologados, string areaCuenta)
        {
            var resultados = new Dictionary<string, object>();
            try
            {
                var dataCodigosParo = datosHomologados.Where(x => x.codigo.tipoParo == (int)Tipo_ParoEnum.mantenimiento_programado || x.codigo.tipoParo == (int)Tipo_ParoEnum.mantenimiento_no_programado).ToList();
                //var lstCodigoStr = dataCodigosParo.Select(x => x.codigo.codigo + " / " + x.codigo.descripcion).ToList();
                var lstCodigoStr = dataCodigosParo.Select(x => x.codigo.descripcion).ToList();
                var lstCodigos = lstCodigoStr.GroupBy(x => x).ToList();
                var lstCodigosParoID = dataCodigosParo.GroupBy(x => x.codigo.descripcion).ToList();
                List<string> lstCodigosJS = new List<string>();
                List<int> lstFrecuenciaCodigosParo = new List<int>();
                List<GraficaKPIHomDTO> datos = new List<GraficaKPIHomDTO>();

                if (lstCodigosParoID.Count > 0)
                {
                    List<string> lstValorCodigos = new List<string>();
                    foreach (var i in lstCodigosParoID)
                    {
                        GraficaKPIHomDTO dato = new GraficaKPIHomDTO();
                        string codigoFormato = i.Key;
                        string codigo = i.First().codigo.codigo;
                        if (!string.IsNullOrEmpty(codigo))
                        {
                            lstCodigosJS.Add(codigoFormato);
                            dato.codigosJS = codigoFormato;
                            var ValorCodigo = dataCodigosParo.Where(x => x.homolo.codigoParo == codigo).Select(x => x.homolo.valor).ToList();
                            decimal sumaValorCodigo = 0;
                            for (int y = 0; y < ValorCodigo.Count(); y++)
                            {
                                sumaValorCodigo += Convert.ToDecimal(ValorCodigo[y]);
                            }
                            lstValorCodigos.Add(sumaValorCodigo.ToString());
                            dato.valorCodigo = sumaValorCodigo;
                        }
                        var frecuenciaCodigosParo = _context.tblM_KPI_Homologado.Where(x => /*x.activo &&*/ x.valor > 0 && x.codigoParo == codigo && x.ac == areaCuenta &&
                                                                                             (x.idTipoParo == (int)Tipo_ParoEnum.mantenimiento_programado || x.idTipoParo == (int)Tipo_ParoEnum.mantenimiento_no_programado) && x.validado).Count();
                        dato.frecuenciaCodigoParo = frecuenciaCodigosParo;
                        lstFrecuenciaCodigosParo.Add(frecuenciaCodigosParo);
                        datos.Add(dato);
                    }
                    datos = datos.OrderByDescending(x => x.valorCodigo).ToList();
                    resultados.Add("lstValorCodigos", datos.OrderByDescending(x => x.valorCodigo).Select(x => x.valorCodigo.ToString()).ToList());
                    resultados.Add("lstCodigosJS", datos.OrderByDescending(x => x.valorCodigo).Select(x => x.codigosJS).ToList());
                    resultados.Add("lstFrecuenciaCodigosParo", datos.OrderByDescending(x => x.frecuenciaCodigoParo).Select(x => x.frecuenciaCodigoParo).ToList());
                }
            }
            catch (Exception ex)
            {
                resultados.Add(SUCCESS, false);
                resultados.Add(MESSAGE, ex.Message);
            }
            return resultados;
        }

        /*
         * 4: MANTENIMIENTO PROGRAMADO / NO PROGRAMADO. 
         * 3: PAROS DE RESERVA / SIN USO. 
         * 2: PAROS DE DEMORA.
        */
        #endregion
        #region ConcentradoRptKPI

        public Dictionary<string, object> GetConcentradoKPI(string ac, int grupoID, int modeloID, DateTime fechaInicio, DateTime fechaFin)
        {
            //try
            //{
                var turnos = _context.tblM_KPI_Homologado_Turnos.FirstOrDefault(x => x.ac == ac);
   
                decimal tiempo = turnos == null ? 24 : turnos.horas_dia ;
                var obj = _context.tblM_KPI_KPICapturaBit.FirstOrDefault(x => x.ac == ac && x.fechaInicio == fechaInicio);
                var dataM = _context.tblM_KPI_Homologado.Where(r => r.ac == ac && (grupoID != 0 ? r.idGrupo == grupoID : true) && (modeloID != 0 ? r.idModelo == modeloID : true) && (r.fecha >= fechaInicio && r.fecha <= fechaFin)).ToList();
                var datosAgrupados = dataM.GroupBy(r => new { r.economico });
                var infoGrupos = new List<kpiReporteConcentraadoDTO>();
                foreach (var f in datosAgrupados)
                {
                    var horas = getHorasDiaDec(ac);
                    var o = new kpiReporteConcentraadoDTO();
                    o.economico = f.Key.economico;
                    o.horasTrabajadas = Math.Round(dataM.Where(r => r.economico == f.Key.economico && (int)Tipo_ParoEnum.trabajo == r.idTipoParo).Sum(s => s.valor), 2);
                    o.horasMMTO = Math.Round(dataM.Where(r => r.economico == f.Key.economico && (r.idTipoParo == (int)Tipo_ParoEnum.mantenimiento_programado || r.idTipoParo == (int)Tipo_ParoEnum.mantenimiento_no_programado)).Sum(s => s.valor), 2);
                    o.horasReserva = Math.Round(dataM.Where(r => r.economico == f.Key.economico && r.idTipoParo == (int)Tipo_ParoEnum.sin_utilizar).Sum(s => s.valor), 2);
                    o.horasDemora = Math.Round(dataM.Where(r => r.economico == f.Key.economico && r.idTipoParo == (int)Tipo_ParoEnum.demoras).Sum(s => s.valor), 2);
                    o.disponibilidad = Math.Round((horas - dataM.Where(r => r.economico == f.Key.economico && (r.idTipoParo == (int)Tipo_ParoEnum.mantenimiento_programado || r.idTipoParo == (int)Tipo_ParoEnum.mantenimiento_no_programado)).Sum(s => s.valor)) / horas, 2) * 100;
                    o.utilizacion = (horas - dataM.Where(r => r.economico == f.Key.economico && (r.idTipoParo == (int)Tipo_ParoEnum.mantenimiento_programado || r.idTipoParo == (int)Tipo_ParoEnum.mantenimiento_no_programado)).Sum(s => s.valor)) == 0 ? 0 : Math.Round((dataM.Where(r => r.economico == f.Key.economico && (int)Tipo_ParoEnum.trabajo == r.idTipoParo).Sum(s => s.valor) / (horas - dataM.Where(r => r.economico == f.Key.economico && (r.idTipoParo == (int)Tipo_ParoEnum.mantenimiento_programado || r.idTipoParo == (int)Tipo_ParoEnum.mantenimiento_no_programado)).Sum(s => s.valor))) * 100, 2);
                    o.horasTotales = horas;
                    infoGrupos.Add(o);
                }

                var acNAme = _context.tblP_CC.FirstOrDefault(x => x.areaCuenta.Equals(ac));
                HttpContext.Current.Session["rptListaCapturaDiaria"] = infoGrupos;
                HttpContext.Current.Session["fechaTemp"] = fechaInicio.ToShortDateString();
                HttpContext.Current.Session["acTemp"] = ac + ' ' + acNAme.descripcion;
                resultado.Add("infoGrupos", infoGrupos);
                resultado.Add("generada", obj!=null);
                resultado.Add("validada", obj!=null ? obj.validado : false);
                resultado.Add(SUCCESS, true);
                return resultado;

            //}
            //catch (Exception e)
            //{

            //    return null;
            //}
        }
        public Dictionary<string, object> getHorasDia(string ac)
        {
            var turnos = _context.tblM_KPI_Homologado_Turnos.FirstOrDefault(x => x.ac == ac);
            var horasDia = turnos == null ? 24 : turnos.horas_dia;
            resultado.Add("horasDia", horasDia);
            return resultado;
        }
        public decimal getHorasDiaDec(string ac)
        {
            var turnos = _context.tblM_KPI_Homologado_Turnos.FirstOrDefault(x => x.ac == ac);
            var horasDia = turnos == null ? 24 : turnos.horas_dia;
            return horasDia;
        }
        #endregion
        #region REPORTE AUTORIZACIONES
        public Dictionary<string, object> GetInfoGraficasPDF(FiltroDTO filtro)
        {
            var r = new Dictionary<string, object>();
            var turnos = _context.tblM_KPI_Homologado_Turnos.FirstOrDefault(x => x.ac == filtro.areaCuenta);
            var horasDia = turnos == null ? 24 : turnos.horas_dia;
            try
            {
                filtro.fechaFin = filtro.fechaInicio;
                filtro.fechaFin = filtro.fechaFin.AddHours(23).AddMinutes(59).AddSeconds(59);

                #region PERIODO MENSUAL
                DateTime diaUno = new DateTime(filtro.fechaInicio.Year, filtro.fechaInicio.Month, 1);
                diaUno = diaUno.Date;
                #endregion

                #region PERIODOS SEMANALES
                var Autorizantes = (
                    from auth in _context.tblM_KPI_AuthHomologado.AsQueryable()
                    where
                        auth.AC == filtro.areaCuenta && auth.AuthEstado == authEstadoEnum.Autorizado
                    select auth
                ).OrderByDescending(e => e.fechaInicio).ToList();

                var primerMartes = Autorizantes.FirstOrDefault(e => e.fechaInicio.DayOfWeek == DayOfWeek.Tuesday);
                var fechaInicioSemana = primerMartes.fechaInicio.AddDays(-6);
                fechaInicioSemana = fechaInicioSemana.Date;
                var fechaFinMartes = primerMartes.fechaInicio.Date;
                #endregion

                var lstKPI = new List<tblM_KPI_Homologado>();

                //TOMAR TODOS LOS KPI DEL AC EN BASE A LA FECHA MAS VIEJA
                if (diaUno < fechaInicioSemana)
                {
                    lstKPI = _context.tblM_KPI_Homologado.Where(e => e.ac == filtro.areaCuenta && diaUno <= e.fecha && filtro.fechaInicio >= e.fecha).ToList();
                }
                else
                {
                    lstKPI = _context.tblM_KPI_Homologado.Where(e => e.ac == filtro.areaCuenta && fechaInicioSemana <= e.fecha && filtro.fechaInicio >= e.fecha).ToList();

                }

                var lstCC = new List<string>() { filtro.areaCuenta };
                var lstGruposHomolo = lstKPI.Select(x => x.idGrupo).Distinct().ToList();
                var lstGrupoID = _context.tblM_CatGrupoMaquinaria.Where(x => x.estatus && lstGruposHomolo.Contains(x.id)).Select(e => e.id).ToList();
                var lstModeloHomolo = lstKPI.Where(e => lstGrupoID.Contains(e.idGrupo)).Select(x => x.idModelo).Distinct().ToList();
                var lstModeloID = _context.tblM_CatModeloEquipo.Where(x => x.estatus && lstModeloHomolo.Contains(x.id)).Select(e => e.id).ToList();
                var __equipos = _context.tblM_CatMaquina.Where(x => (lstCC.Count > 0 ? lstCC.Contains(x.centro_costos) : false) &&
                                                      (lstGrupoID.Count > 0 ? lstGrupoID.Contains(x.grupoMaquinariaID) : false) &&
                                                      (lstModeloID.Count > 0 ? lstModeloID.Contains(x.modeloEquipoID) : false) && x.estatus == 1).ToList();

 

                #region KPI DIARIO

                var homologadosDiario = lstKPI.Where
                    (w =>
                        w.fecha >= filtro.fechaInicio &&
                        w.fecha <= filtro.fechaFin &&
                            //w.activo && w.valor > 0
                        w.valor > 0 && w.validado
                    ).Join
                    (
                        _context.tblM_KPI_CodigosParo,
                        homolo => homolo.idParo,
                        codigo => codigo.id,
                        (homolo, codigo) => new { homolo, codigo }
                    ).Join
                    (
                        _context.tblM_CatMaquina,
                        homolo => homolo.homolo.idEconomico,
                        maquina => maquina.id,
                        (homolo, maquina) => new { homolo.homolo, homolo.codigo, maquina }
                    ).Join
                    (
                        _context.tblM_CatModeloEquipo,
                        homolo => homolo.homolo.idModelo,
                        modelo => modelo.id,
                        (homolo, modelo) => new { homolo.homolo, homolo.codigo, homolo.maquina, modelo }
                    ).Join
                    (
                        _context.tblM_CatGrupoMaquinaria,
                        homolo => homolo.homolo.idGrupo,
                        grupo => grupo.id,
                        (homolo, grupo) => new { homolo.homolo, homolo.codigo, homolo.maquina, homolo.modelo, grupo }
                    ).Select(m => new QueryHomologadosDTO()
                    {
                        homolo = m.homolo,
                        codigo = m.codigo,
                        maquina = m.maquina,
                        modelo = m.modelo,
                        grupo = m.grupo
                    }).ToList();
                #endregion

                #region KPI SEMANAL

                

                var homologadosSemanal = lstKPI.Where
                    (w =>
                        w.fecha >= fechaInicioSemana &&
                        w.fecha <= fechaFinMartes &&
                            //w.activo && w.valor > 0
                        w.valor > 0 && w.validado
                    ).Join
                    (
                        _context.tblM_KPI_CodigosParo,
                        homolo => homolo.idParo,
                        codigo => codigo.id,
                        (homolo, codigo) => new { homolo, codigo }
                    ).Join
                    (
                        _context.tblM_CatMaquina,
                        homolo => homolo.homolo.idEconomico,
                        maquina => maquina.id,
                        (homolo, maquina) => new { homolo.homolo, homolo.codigo, maquina }
                    ).Join
                    (
                        _context.tblM_CatModeloEquipo,
                        homolo => homolo.homolo.idModelo,
                        modelo => modelo.id,
                        (homolo, modelo) => new { homolo.homolo, homolo.codigo, homolo.maquina, modelo }
                    ).Join
                    (
                        _context.tblM_CatGrupoMaquinaria,
                        homolo => homolo.homolo.idGrupo,
                        grupo => grupo.id,
                        (homolo, grupo) => new { homolo.homolo, homolo.codigo, homolo.maquina, homolo.modelo, grupo }
                    ).Select(m => new QueryHomologadosDTO()
                    {
                        homolo = m.homolo,
                        codigo = m.codigo,
                        maquina = m.maquina,
                        modelo = m.modelo,
                        grupo = m.grupo
                    }).ToList();
                #endregion

                #region KPI MENSUAL

                var homologadosMensual = lstKPI.Where
                    (w =>
                        w.fecha >= diaUno &&
                        w.fecha <= filtro.fechaInicio &&
                            //w.activo && w.valor > 0
                        w.valor > 0 && w.validado
                    ).Join
                    (
                        _context.tblM_KPI_CodigosParo,
                        homolo => homolo.idParo,
                        codigo => codigo.id,
                        (homolo, codigo) => new { homolo, codigo }
                    ).Join
                    (
                        _context.tblM_CatMaquina,
                        homolo => homolo.homolo.idEconomico,
                        maquina => maquina.id,
                        (homolo, maquina) => new { homolo.homolo, homolo.codigo, maquina }
                    ).Join
                    (
                        _context.tblM_CatModeloEquipo,
                        homolo => homolo.homolo.idModelo,
                        modelo => modelo.id,
                        (homolo, modelo) => new { homolo.homolo, homolo.codigo, homolo.maquina, modelo }
                    ).Join
                    (
                        _context.tblM_CatGrupoMaquinaria,
                        homolo => homolo.homolo.idGrupo,
                        grupo => grupo.id,
                        (homolo, grupo) => new { homolo.homolo, homolo.codigo, homolo.maquina, homolo.modelo, grupo }
                    ).Select(m => new QueryHomologadosDTO()
                    {
                        homolo = m.homolo,
                        codigo = m.codigo,
                        maquina = m.maquina,
                        modelo = m.modelo,
                        grupo = m.grupo
                    }).ToList();
                #endregion

                //var data = dataModelos.Select(x => new ComboDTO
                //{
                //    Value = x.id.ToString(),
                //    Text = x.noEconomico
                //}).ToList();
                //var __equipos = _context.tblM_CatMaquina.Where(w => lstModelos.Contains(w.modeloEquipoID)).ToList();

                var hrsProgramadoDiario = DatetimeUtils.DiasDiferencia(filtro.fechaInicio, filtro.fechaFin) * horasDia;
                var hrsProgramadoSemanal = DatetimeUtils.DiasDiferencia(fechaInicioSemana, fechaFinMartes) * horasDia;
                var hrsProgramadoMensual = DatetimeUtils.DiasDiferencia(diaUno, filtro.fechaInicio) * horasDia;

                var resultados = CalculosReporte(filtro, homologadosDiario, homologadosSemanal, homologadosMensual, hrsProgramadoDiario, hrsProgramadoSemanal, hrsProgramadoMensual, __equipos);

                //try
                //{
                //    var excel = GetExcel(filtro, hrsProgramado);
                //    r.Add("excel", excel);
                //    r.Add("noExcel", false);
                //}
                //catch (Exception ex)
                //{
                //    r.Add("excel", null);
                //    r.Add("noExcel", true);
                //}

                //var resultadosGraficaID = GetDatosGraficaParosReservaSinUso(homologados, filtro.areaCuenta);
                //var resultadosGraficaDL = GetDatosGraficaParosDemora(homologados, filtro.areaCuenta);
                //var resultadosGraficaMT = GetDatosGraficaParosMantenimientos(homologados, filtro.areaCuenta);

                //var resultadoAnual = InfoAnual(filtro);

                r.Add(SUCCESS, true);
                r.Add("descPeriodoDia", (" dia: " + filtro.fechaInicio.ToString("dd/MM/yyyy")));
                r.Add("descPeriodoSemana", (" del: " + fechaInicioSemana.ToString("dd/MM/yyyy") + " al " + fechaFinMartes.ToString("dd/MM/yyyy")));
                r.Add("descPeriodoMes", (" del : " + diaUno.ToString("dd/MM/yyyy") + " al " + filtro.fechaInicio.ToString("dd/MM/yyyy")));

                //r.Add("tiempos", resultados.tiempos);

                //r.Add("disVsUti_economico", resultados.disVsUti_economico);
                r.Add("disVsUti_modeloDiario", resultados.disVsUti_modeloDiario);
                r.Add("disVsUti_modeloSemanal", resultados.disVsUti_modeloSemanal);
                r.Add("disVsUti_modeloMensual", resultados.disVsUti_modeloMensual);
                //r.Add("disVsUti_grupo", resultados.disVsUti_grupo);

                //r.Add("gpx_disVsUti_economico", resultados.gpx_disVsUti_economico);
                r.Add("gpx_disVsUti_modeloDiario", resultados.gpx_disVsUti_modeloDiario);
                r.Add("gpx_disVsUti_modeloSemanal", resultados.gpx_disVsUti_modeloSemanal);
                r.Add("gpx_disVsUti_modeloMensual", resultados.gpx_disVsUti_modeloMensual);
                //r.Add("gpx_disVsUti_grupo", resultados.gpx_disVsUti_grupo);
                //r.Add("gpx_disVsUti_semanal", resultados.gpx_disVsUti_semanal);
                //r.Add("gpx_disVsUti_mensual", resultados.gpx_disVsUti_mensual);

                //r.Add("gpx_opeVsTra_economico", resultados.gpx_opeVsTra_economico);
                //r.Add("gpx_opeVsTra_modelo", resultados.gpx_opeVsTra_modelo);
                //r.Add("gpx_opeVsTra_grupo", resultados.gpx_opeVsTra_grupo);
                //r.Add("gpx_opeVsTra_semanal", resultados.gpx_opeVsTra_semanal);
                //r.Add("gpx_opeVsTra_mensual", resultados.gpx_opeVsTra_mensual);

                //r.Add("gpx_UT_economico", resultados.gpx_UT_economico);
                //r.Add("gpx_UT_modelo", resultados.gpx_UT_modelo);
                //r.Add("gpx_UT_grupo", resultados.gpx_UT_grupo);
                //r.Add("gpx_UT_semanal", resultados.gpx_UT_semanal);
                //r.Add("gpx_UT_mensual", resultados.gpx_UT_mensual);

                //r.Add("anual", resultadoAnual);

                //r.Add("resultadosGraficaID", resultadosGraficaID);
                //r.Add("resultadosGraficaDL", resultadosGraficaDL);
                //r.Add("resultadosGraficaMT", resultadosGraficaMT);
            }
            catch (Exception ex)
            {
                r.Add(SUCCESS, false);
                r.Add(MESSAGE, ex.Message);
            }

            return r;
        }

        private ResultadosReporteDTO CalculosReporte(FiltroDTO filtro,
            List<QueryHomologadosDTO> datosHomologadosDiario, List<QueryHomologadosDTO> datosHomologadosSemanal, List<QueryHomologadosDTO> datosHomologadosMensual,
            int hrsProgramadoDiario, int hrsProgramadoSemanal, int hrsProgramadoMensual, List<tblM_CatMaquina> __equipos)
        {
            var resultados = new ResultadosReporteDTO();
            var turnos = _context.tblM_KPI_Homologado_Turnos.FirstOrDefault(x => x.ac == filtro.areaCuenta);
            var horasDia = turnos == null ? 24 : turnos.horas_dia;
            //var disponibilidadVsUtilizacionEconomico = new List<CalculosDTO>();
            var disponibilidadVsUtilizacionModeloDiario = new List<CalculosDTO>();
            var disponibilidadVsUtilizacionModeloSemanal = new List<CalculosDTO>();
            var disponibilidadVsUtilizacionModeloMensual = new List<CalculosDTO>();
            //var disponibilidadVsUtilizacionGrupo = new List<CalculosDTO>();

            //var gpx_dVSu_economico = new InfoGraficasDTO();
            var gpx_dVSu_modeloDiario = new List<DatosGraficasDTO>();
            var gpx_dVSu_modeloSemanal = new List<DatosGraficasDTO>();
            var gpx_dVSu_modeloMensual = new List<DatosGraficasDTO>();

            //var gpx_dVSu_grupo = new InfoGraficasDTO();
            //var gpx_dVSu_semanal = new InfoGraficasDTO();
            //var gpx_dVSu_mensual = new InfoGraficasDTO();

            //var gpx_oVSt_economico = new InfoGraficasDTO();
            //var gpx_oVSt_modelo = new InfoGraficasDTO();
            //var gpx_oVSt_grupo = new InfoGraficasDTO();
            //var gpx_oVSt_semanal = new InfoGraficasDTO();
            //var gpx_oVSt_mensual = new InfoGraficasDTO();

            //var gpx_UT_economico = new InfoGraficasDTO();
            //var gpx_UT_modelo = new InfoGraficasDTO();
            //var gpx_UT_grupo = new InfoGraficasDTO();
            //var gpx_UT_semanal = new InfoGraficasDTO();
            //var gpx_UT_mensual = new InfoGraficasDTO();

            #region DIARIO
            foreach (var gbGrupo in datosHomologadosDiario.GroupBy(g => g.homolo.idGrupo))
            {
                foreach (var gbModelo in gbGrupo.GroupBy(g => g.homolo.idModelo))
                {
                    #region RESTO GRAFICAS

                    //foreach (var gbEconomico in gbModelo.GroupBy(g => g.homolo.idEconomico))
                    //{
                    //    var calculosEconomico = new CalculosDTO();

                    //    calculosEconomico.id = gbEconomico.Key;
                    //    calculosEconomico.descripcion = gbEconomico.First().homolo.economico;
                    //    calculosEconomico.tiempos = CalculosTiempos(gbEconomico, hrsProgramado);

                    //    disponibilidadVsUtilizacionEconomico.Add(calculosEconomico);

                    //    #region infoGrafic_gpx_economico
                    //    gpx_dVSu_economico.categorias.Add(calculosEconomico.descripcion);
                    //    gpx_dVSu_economico.serie1Descripcion = "DISPONIBILIDAD";
                    //    gpx_dVSu_economico.serie1.Add(Math.Round(((calculosEconomico.tiempos.hrsDisponible / (calculosEconomico.tiempos.hrsProgramado > 0 ? calculosEconomico.tiempos.hrsProgramado : 1)) * 100), 2));
                    //    gpx_dVSu_economico.serie2Descripcion = "UTILIZACIÓN";
                    //    #region V1
                    //    //gpx_dVSu_economico.serie2.Add(Math.Round(((calculosEconomico.tiempos.hrsTrabajo / calculosEconomico.tiempos.hrsProgramado) * 100), 2));
                    //    #endregion
                    //    gpx_dVSu_economico.serie2.Add(Math.Round(((calculosEconomico.tiempos.hrsOperacion / (calculosEconomico.tiempos.hrsDisponible > 0 ? calculosEconomico.tiempos.hrsDisponible : 1)) * 100), 2));

                    //    #region V1
                    //    //gpx_oVSt_economico.categorias.Add(calculosEconomico.descripcion);
                    //    //gpx_oVSt_economico.serie1Descripcion = "OPERACIÓN (OPT)";
                    //    //gpx_oVSt_economico.serie1.Add(calculosEconomico.tiempos.porOperacion);
                    //    //gpx_oVSt_economico.serie2Descripcion = "TRABAJO (WK)";
                    //    //gpx_oVSt_economico.serie2.Add(calculosEconomico.tiempos.porTrabajo);
                    //    #endregion
                    //    #region V2
                    //    gpx_oVSt_economico.categorias.Add(calculosEconomico.descripcion);
                    //    gpx_oVSt_economico.serie1Descripcion = "UTILIZACIÓN"; //UA
                    //    gpx_oVSt_economico.serie1.Add(Math.Round(((calculosEconomico.tiempos.hrsOperacion / (calculosEconomico.tiempos.hrsDisponible > 0 ? calculosEconomico.tiempos.hrsDisponible : 1)) * 100), 2));
                    //    gpx_oVSt_economico.serie2Descripcion = "EFICIENCIA DE LA UTILIZACIÓN"; //UE
                    //    gpx_oVSt_economico.serie2.Add(Math.Round(((calculosEconomico.tiempos.hrsTrabajo / (calculosEconomico.tiempos.hrsOperacion > 0 ? calculosEconomico.tiempos.hrsOperacion : 1)) * 100), 2));
                    //    #endregion

                    //    gpx_UT_economico.categorias.Add(calculosEconomico.descripcion);
                    //    gpx_UT_economico.serie1Descripcion = "UTILIZACIÓN";
                    //    gpx_UT_economico.serie1.Add(Math.Round(((calculosEconomico.tiempos.hrsTrabajo / (calculosEconomico.tiempos.hrsProgramado > 0 ? calculosEconomico.tiempos.hrsProgramado : 1)) * 100), 2));
                    //    #endregion
                    //}
                    #endregion

                    var calculosModeloDiario = new CalculosDTO();

                    calculosModeloDiario.id = gbModelo.Key;
                    calculosModeloDiario.descripcion = gbModelo.First().maquina.modeloEquipo.descripcion;
                    calculosModeloDiario.tiempos = CalculosTiempos(gbModelo, hrsProgramadoDiario * __equipos.Where(w => w.modeloEquipoID == gbModelo.Key).Count());

                    if (calculosModeloDiario.tiempos.hrsProgramado > 0 && calculosModeloDiario.tiempos.hrsDisponible > 0)
                    {
                        disponibilidadVsUtilizacionModeloDiario.Add(calculosModeloDiario);

                        #region infoGrafic_gpx_modelo

                        gpx_dVSu_modeloDiario.Add(new DatosGraficasDTO()
                        {
                            economico = calculosModeloDiario.descripcion,
                            disponibilidad = (calculosModeloDiario.tiempos.hrsDisponible / calculosModeloDiario.tiempos.hrsProgramado),
                            utilizacion = (calculosModeloDiario.tiempos.hrsOperacion / calculosModeloDiario.tiempos.hrsDisponible),
                        });

                        //gpx_dVSu_modeloDiario.categorias.Add(calculosModeloDiario.descripcion);
                        //gpx_dVSu_modeloDiario.serie1Descripcion = "DISPONIBILIDAD";
                        //gpx_dVSu_modeloDiario.serie1.Add(Math.Round(((calculosModeloDiario.tiempos.hrsDisponible / calculosModeloDiario.tiempos.hrsProgramado) * 100), 2));
                        //gpx_dVSu_modeloDiario.serie2Descripcion = "UTILIZACIÓN";
                        //gpx_dVSu_modeloDiario.serie2.Add(Math.Round(((calculosModeloDiario.tiempos.hrsOperacion / calculosModeloDiario.tiempos.hrsDisponible) * 100), 2));

                        //gpx_oVSt_modelo.categorias.Add(calculosModelo.descripcion);
                        //gpx_oVSt_modelo.serie1Descripcion = "UTILIZACIÓN";
                        //gpx_oVSt_modelo.serie1.Add(Math.Round(((calculosModelo.tiempos.hrsOperacion / calculosModelo.tiempos.hrsDisponible) * 100), 2));
                        //gpx_oVSt_modelo.serie2Descripcion = "EFICIENCIA DE LA UTILIZACIÓN";
                        //gpx_oVSt_modelo.serie2.Add(Math.Round(((calculosModelo.tiempos.hrsTrabajo / calculosModelo.tiempos.hrsOperacion) * 100), 2));

                        //gpx_UT_modelo.categorias.Add(calculosModelo.descripcion);
                        //gpx_UT_modelo.serie1Descripcion = "UTILIZACIÓN";
                        //gpx_UT_modelo.serie1.Add(Math.Round(((calculosModelo.tiempos.hrsTrabajo / calculosModelo.tiempos.hrsProgramado) * 100), 2));
                        #endregion
                    }
                }

                //var calculosGrupo = new CalculosDTO();

                //calculosGrupo.id = gbGrupo.Key;
                //calculosGrupo.descripcion = gbGrupo.First().maquina.grupoMaquinaria.descripcion;
                //calculosGrupo.tiempos = CalculosTiempos(gbGrupo, hrsProgramado * __equipos.Where(w => w.grupoMaquinariaID == gbGrupo.Key).Count());

                //disponibilidadVsUtilizacionGrupo.Add(calculosGrupo);

                #region infoGrafic_gpx_grupo
                //gpx_dVSu_grupo.categorias.Add(calculosGrupo.descripcion);
                //gpx_dVSu_grupo.serie1Descripcion = "DISPONIBILIDAD";
                //gpx_dVSu_grupo.serie1.Add(Math.Round(((calculosGrupo.tiempos.hrsDisponible / calculosGrupo.tiempos.hrsProgramado) * 100), 2));
                //gpx_dVSu_grupo.serie2Descripcion = "UTILIZACIÓN";
                //gpx_dVSu_grupo.serie2.Add(Math.Round(((calculosGrupo.tiempos.hrsOperacion / calculosGrupo.tiempos.hrsDisponible) * 100), 2));

                //gpx_oVSt_grupo.categorias.Add(calculosGrupo.descripcion);
                //gpx_oVSt_grupo.serie1Descripcion = "UTILIZACIÓN";
                //gpx_oVSt_grupo.serie1.Add(Math.Round(((calculosGrupo.tiempos.hrsOperacion / calculosGrupo.tiempos.hrsDisponible) * 100), 2));
                //gpx_oVSt_grupo.serie2Descripcion = "EFICIENCIA DE LA UTILIZACIÓN";
                //gpx_oVSt_grupo.serie2.Add(Math.Round(((calculosGrupo.tiempos.hrsTrabajo / calculosGrupo.tiempos.hrsOperacion) * 100), 2));

                //gpx_UT_grupo.categorias.Add(calculosGrupo.descripcion);
                //gpx_UT_grupo.serie1Descripcion = "UTILIZACIÓN";
                //gpx_UT_grupo.serie1.Add(Math.Round(((calculosGrupo.tiempos.hrsTrabajo / calculosGrupo.tiempos.hrsProgramado) * 100), 2));
                #endregion
            }
            #endregion

            #region SEMANAL
            foreach (var gbGrupo in datosHomologadosSemanal.GroupBy(g => g.homolo.idGrupo))
            {
                foreach (var gbModelo in gbGrupo.GroupBy(g => g.homolo.idModelo))
                {
                    #region RESTO GRAFICAS

                    //foreach (var gbEconomico in gbModelo.GroupBy(g => g.homolo.idEconomico))
                    //{
                    //    var calculosEconomico = new CalculosDTO();

                    //    calculosEconomico.id = gbEconomico.Key;
                    //    calculosEconomico.descripcion = gbEconomico.First().homolo.economico;
                    //    calculosEconomico.tiempos = CalculosTiempos(gbEconomico, hrsProgramado);

                    //    disponibilidadVsUtilizacionEconomico.Add(calculosEconomico);

                    //    #region infoGrafic_gpx_economico
                    //    gpx_dVSu_economico.categorias.Add(calculosEconomico.descripcion);
                    //    gpx_dVSu_economico.serie1Descripcion = "DISPONIBILIDAD";
                    //    gpx_dVSu_economico.serie1.Add(Math.Round(((calculosEconomico.tiempos.hrsDisponible / (calculosEconomico.tiempos.hrsProgramado > 0 ? calculosEconomico.tiempos.hrsProgramado : 1)) * 100), 2));
                    //    gpx_dVSu_economico.serie2Descripcion = "UTILIZACIÓN";
                    //    #region V1
                    //    //gpx_dVSu_economico.serie2.Add(Math.Round(((calculosEconomico.tiempos.hrsTrabajo / calculosEconomico.tiempos.hrsProgramado) * 100), 2));
                    //    #endregion
                    //    gpx_dVSu_economico.serie2.Add(Math.Round(((calculosEconomico.tiempos.hrsOperacion / (calculosEconomico.tiempos.hrsDisponible > 0 ? calculosEconomico.tiempos.hrsDisponible : 1)) * 100), 2));

                    //    #region V1
                    //    //gpx_oVSt_economico.categorias.Add(calculosEconomico.descripcion);
                    //    //gpx_oVSt_economico.serie1Descripcion = "OPERACIÓN (OPT)";
                    //    //gpx_oVSt_economico.serie1.Add(calculosEconomico.tiempos.porOperacion);
                    //    //gpx_oVSt_economico.serie2Descripcion = "TRABAJO (WK)";
                    //    //gpx_oVSt_economico.serie2.Add(calculosEconomico.tiempos.porTrabajo);
                    //    #endregion
                    //    #region V2
                    //    gpx_oVSt_economico.categorias.Add(calculosEconomico.descripcion);
                    //    gpx_oVSt_economico.serie1Descripcion = "UTILIZACIÓN"; //UA
                    //    gpx_oVSt_economico.serie1.Add(Math.Round(((calculosEconomico.tiempos.hrsOperacion / (calculosEconomico.tiempos.hrsDisponible > 0 ? calculosEconomico.tiempos.hrsDisponible : 1)) * 100), 2));
                    //    gpx_oVSt_economico.serie2Descripcion = "EFICIENCIA DE LA UTILIZACIÓN"; //UE
                    //    gpx_oVSt_economico.serie2.Add(Math.Round(((calculosEconomico.tiempos.hrsTrabajo / (calculosEconomico.tiempos.hrsOperacion > 0 ? calculosEconomico.tiempos.hrsOperacion : 1)) * 100), 2));
                    //    #endregion

                    //    gpx_UT_economico.categorias.Add(calculosEconomico.descripcion);
                    //    gpx_UT_economico.serie1Descripcion = "UTILIZACIÓN";
                    //    gpx_UT_economico.serie1.Add(Math.Round(((calculosEconomico.tiempos.hrsTrabajo / (calculosEconomico.tiempos.hrsProgramado > 0 ? calculosEconomico.tiempos.hrsProgramado : 1)) * 100), 2));
                    //    #endregion
                    //}
                    #endregion

                    var calculosModeloSemanal = new CalculosDTO();

                    calculosModeloSemanal.id = gbModelo.Key;
                    calculosModeloSemanal.descripcion = gbModelo.First().maquina.modeloEquipo.descripcion;
                    calculosModeloSemanal.tiempos = CalculosTiempos(gbModelo, hrsProgramadoSemanal * __equipos.Where(w => w.modeloEquipoID == gbModelo.Key).Count());

                    if (calculosModeloSemanal.tiempos.hrsProgramado > 0 && calculosModeloSemanal.tiempos.hrsDisponible > 0)
                    {
                        disponibilidadVsUtilizacionModeloSemanal.Add(calculosModeloSemanal);

                        #region infoGrafic_gpx_modelo

                        gpx_dVSu_modeloSemanal.Add(new DatosGraficasDTO()
                        {
                            economico = calculosModeloSemanal.descripcion,
                            disponibilidad = (calculosModeloSemanal.tiempos.hrsDisponible / calculosModeloSemanal.tiempos.hrsProgramado),
                            utilizacion = (calculosModeloSemanal.tiempos.hrsOperacion / calculosModeloSemanal.tiempos.hrsDisponible),
                        });

                        //gpx_dVSu_modeloSemanal.categorias.Add(calculosModeloSemanal.descripcion);
                        //gpx_dVSu_modeloSemanal.serie1Descripcion = "DISPONIBILIDAD";
                        //gpx_dVSu_modeloSemanal.serie1.Add(Math.Round(((calculosModeloSemanal.tiempos.hrsDisponible / calculosModeloSemanal.tiempos.hrsProgramado) * 100), 2));
                        //gpx_dVSu_modeloSemanal.serie2Descripcion = "UTILIZACIÓN";
                        //gpx_dVSu_modeloSemanal.serie2.Add(Math.Round(((calculosModeloSemanal.tiempos.hrsOperacion / calculosModeloSemanal.tiempos.hrsDisponible) * 100), 2));

                        //gpx_oVSt_modelo.categorias.Add(calculosModelo.descripcion);
                        //gpx_oVSt_modelo.serie1Descripcion = "UTILIZACIÓN";
                        //gpx_oVSt_modelo.serie1.Add(Math.Round(((calculosModelo.tiempos.hrsOperacion / calculosModelo.tiempos.hrsDisponible) * 100), 2));
                        //gpx_oVSt_modelo.serie2Descripcion = "EFICIENCIA DE LA UTILIZACIÓN";
                        //gpx_oVSt_modelo.serie2.Add(Math.Round(((calculosModelo.tiempos.hrsTrabajo / calculosModelo.tiempos.hrsOperacion) * 100), 2));

                        //gpx_UT_modelo.categorias.Add(calculosModelo.descripcion);
                        //gpx_UT_modelo.serie1Descripcion = "UTILIZACIÓN";
                        //gpx_UT_modelo.serie1.Add(Math.Round(((calculosModelo.tiempos.hrsTrabajo / calculosModelo.tiempos.hrsProgramado) * 100), 2));
                        #endregion
                    }
                    
                }

                //var calculosGrupo = new CalculosDTO();

                //calculosGrupo.id = gbGrupo.Key;
                //calculosGrupo.descripcion = gbGrupo.First().maquina.grupoMaquinaria.descripcion;
                //calculosGrupo.tiempos = CalculosTiempos(gbGrupo, hrsProgramado * __equipos.Where(w => w.grupoMaquinariaID == gbGrupo.Key).Count());

                //disponibilidadVsUtilizacionGrupo.Add(calculosGrupo);

                #region infoGrafic_gpx_grupo
                //gpx_dVSu_grupo.categorias.Add(calculosGrupo.descripcion);
                //gpx_dVSu_grupo.serie1Descripcion = "DISPONIBILIDAD";
                //gpx_dVSu_grupo.serie1.Add(Math.Round(((calculosGrupo.tiempos.hrsDisponible / calculosGrupo.tiempos.hrsProgramado) * 100), 2));
                //gpx_dVSu_grupo.serie2Descripcion = "UTILIZACIÓN";
                //gpx_dVSu_grupo.serie2.Add(Math.Round(((calculosGrupo.tiempos.hrsOperacion / calculosGrupo.tiempos.hrsDisponible) * 100), 2));

                //gpx_oVSt_grupo.categorias.Add(calculosGrupo.descripcion);
                //gpx_oVSt_grupo.serie1Descripcion = "UTILIZACIÓN";
                //gpx_oVSt_grupo.serie1.Add(Math.Round(((calculosGrupo.tiempos.hrsOperacion / calculosGrupo.tiempos.hrsDisponible) * 100), 2));
                //gpx_oVSt_grupo.serie2Descripcion = "EFICIENCIA DE LA UTILIZACIÓN";
                //gpx_oVSt_grupo.serie2.Add(Math.Round(((calculosGrupo.tiempos.hrsTrabajo / calculosGrupo.tiempos.hrsOperacion) * 100), 2));

                //gpx_UT_grupo.categorias.Add(calculosGrupo.descripcion);
                //gpx_UT_grupo.serie1Descripcion = "UTILIZACIÓN";
                //gpx_UT_grupo.serie1.Add(Math.Round(((calculosGrupo.tiempos.hrsTrabajo / calculosGrupo.tiempos.hrsProgramado) * 100), 2));
                #endregion
            }
            #endregion

            #region MENSUAL
            foreach (var gbGrupo in datosHomologadosMensual.GroupBy(g => g.homolo.idGrupo))
            {
                foreach (var gbModelo in gbGrupo.GroupBy(g => g.homolo.idModelo))
                {
                    #region RESTO GRAFICAS

                    //foreach (var gbEconomico in gbModelo.GroupBy(g => g.homolo.idEconomico))
                    //{
                    //    var calculosEconomico = new CalculosDTO();

                    //    calculosEconomico.id = gbEconomico.Key;
                    //    calculosEconomico.descripcion = gbEconomico.First().homolo.economico;
                    //    calculosEconomico.tiempos = CalculosTiempos(gbEconomico, hrsProgramado);

                    //    disponibilidadVsUtilizacionEconomico.Add(calculosEconomico);

                    //    #region infoGrafic_gpx_economico
                    //    gpx_dVSu_economico.categorias.Add(calculosEconomico.descripcion);
                    //    gpx_dVSu_economico.serie1Descripcion = "DISPONIBILIDAD";
                    //    gpx_dVSu_economico.serie1.Add(Math.Round(((calculosEconomico.tiempos.hrsDisponible / (calculosEconomico.tiempos.hrsProgramado > 0 ? calculosEconomico.tiempos.hrsProgramado : 1)) * 100), 2));
                    //    gpx_dVSu_economico.serie2Descripcion = "UTILIZACIÓN";
                    //    #region V1
                    //    //gpx_dVSu_economico.serie2.Add(Math.Round(((calculosEconomico.tiempos.hrsTrabajo / calculosEconomico.tiempos.hrsProgramado) * 100), 2));
                    //    #endregion
                    //    gpx_dVSu_economico.serie2.Add(Math.Round(((calculosEconomico.tiempos.hrsOperacion / (calculosEconomico.tiempos.hrsDisponible > 0 ? calculosEconomico.tiempos.hrsDisponible : 1)) * 100), 2));

                    //    #region V1
                    //    //gpx_oVSt_economico.categorias.Add(calculosEconomico.descripcion);
                    //    //gpx_oVSt_economico.serie1Descripcion = "OPERACIÓN (OPT)";
                    //    //gpx_oVSt_economico.serie1.Add(calculosEconomico.tiempos.porOperacion);
                    //    //gpx_oVSt_economico.serie2Descripcion = "TRABAJO (WK)";
                    //    //gpx_oVSt_economico.serie2.Add(calculosEconomico.tiempos.porTrabajo);
                    //    #endregion
                    //    #region V2
                    //    gpx_oVSt_economico.categorias.Add(calculosEconomico.descripcion);
                    //    gpx_oVSt_economico.serie1Descripcion = "UTILIZACIÓN"; //UA
                    //    gpx_oVSt_economico.serie1.Add(Math.Round(((calculosEconomico.tiempos.hrsOperacion / (calculosEconomico.tiempos.hrsDisponible > 0 ? calculosEconomico.tiempos.hrsDisponible : 1)) * 100), 2));
                    //    gpx_oVSt_economico.serie2Descripcion = "EFICIENCIA DE LA UTILIZACIÓN"; //UE
                    //    gpx_oVSt_economico.serie2.Add(Math.Round(((calculosEconomico.tiempos.hrsTrabajo / (calculosEconomico.tiempos.hrsOperacion > 0 ? calculosEconomico.tiempos.hrsOperacion : 1)) * 100), 2));
                    //    #endregion

                    //    gpx_UT_economico.categorias.Add(calculosEconomico.descripcion);
                    //    gpx_UT_economico.serie1Descripcion = "UTILIZACIÓN";
                    //    gpx_UT_economico.serie1.Add(Math.Round(((calculosEconomico.tiempos.hrsTrabajo / (calculosEconomico.tiempos.hrsProgramado > 0 ? calculosEconomico.tiempos.hrsProgramado : 1)) * 100), 2));
                    //    #endregion
                    //}
                    #endregion

                    var calculosModeloMensual = new CalculosDTO();

                    calculosModeloMensual.id = gbModelo.Key;
                    calculosModeloMensual.descripcion = gbModelo.First().maquina.modeloEquipo.descripcion;
                    calculosModeloMensual.tiempos = CalculosTiempos(gbModelo, hrsProgramadoMensual * __equipos.Where(w => w.modeloEquipoID == gbModelo.Key).Count());

                    if (calculosModeloMensual.tiempos.hrsProgramado > 0 && calculosModeloMensual.tiempos.hrsDisponible > 0)
                    {
                        disponibilidadVsUtilizacionModeloMensual.Add(calculosModeloMensual);

                        #region infoGrafic_gpx_modelo

                        gpx_dVSu_modeloMensual.Add(new DatosGraficasDTO()
                        {
                            economico = calculosModeloMensual.descripcion,
                            disponibilidad = (calculosModeloMensual.tiempos.hrsDisponible / calculosModeloMensual.tiempos.hrsProgramado),
                            utilizacion = (calculosModeloMensual.tiempos.hrsOperacion / calculosModeloMensual.tiempos.hrsDisponible),
                        });

                        //gpx_dVSu_modeloMensual.categorias.Add(calculosModeloMensual.descripcion);
                        //gpx_dVSu_modeloMensual.serie1Descripcion = "DISPONIBILIDAD";
                        //gpx_dVSu_modeloMensual.serie1.Add(Math.Round(((calculosModeloMensual.tiempos.hrsDisponible / calculosModeloMensual.tiempos.hrsProgramado) * 100), 2));
                        //gpx_dVSu_modeloMensual.serie2Descripcion = "UTILIZACIÓN";
                        //gpx_dVSu_modeloMensual.serie2.Add(Math.Round(((calculosModeloMensual.tiempos.hrsOperacion / calculosModeloMensual.tiempos.hrsDisponible) * 100), 2));

                        //gpx_oVSt_modelo.categorias.Add(calculosModelo.descripcion);
                        //gpx_oVSt_modelo.serie1Descripcion = "UTILIZACIÓN";
                        //gpx_oVSt_modelo.serie1.Add(Math.Round(((calculosModelo.tiempos.hrsOperacion / calculosModelo.tiempos.hrsDisponible) * 100), 2));
                        //gpx_oVSt_modelo.serie2Descripcion = "EFICIENCIA DE LA UTILIZACIÓN";
                        //gpx_oVSt_modelo.serie2.Add(Math.Round(((calculosModelo.tiempos.hrsTrabajo / calculosModelo.tiempos.hrsOperacion) * 100), 2));

                        //gpx_UT_modelo.categorias.Add(calculosModelo.descripcion);
                        //gpx_UT_modelo.serie1Descripcion = "UTILIZACIÓN";
                        //gpx_UT_modelo.serie1.Add(Math.Round(((calculosModelo.tiempos.hrsTrabajo / calculosModelo.tiempos.hrsProgramado) * 100), 2));
                        #endregion
                    }
                }

                //var calculosGrupo = new CalculosDTO();

                //calculosGrupo.id = gbGrupo.Key;
                //calculosGrupo.descripcion = gbGrupo.First().maquina.grupoMaquinaria.descripcion;
                //calculosGrupo.tiempos = CalculosTiempos(gbGrupo, hrsProgramado * __equipos.Where(w => w.grupoMaquinariaID == gbGrupo.Key).Count());

                //disponibilidadVsUtilizacionGrupo.Add(calculosGrupo);

                #region infoGrafic_gpx_grupo
                //gpx_dVSu_grupo.categorias.Add(calculosGrupo.descripcion);
                //gpx_dVSu_grupo.serie1Descripcion = "DISPONIBILIDAD";
                //gpx_dVSu_grupo.serie1.Add(Math.Round(((calculosGrupo.tiempos.hrsDisponible / calculosGrupo.tiempos.hrsProgramado) * 100), 2));
                //gpx_dVSu_grupo.serie2Descripcion = "UTILIZACIÓN";
                //gpx_dVSu_grupo.serie2.Add(Math.Round(((calculosGrupo.tiempos.hrsOperacion / calculosGrupo.tiempos.hrsDisponible) * 100), 2));

                //gpx_oVSt_grupo.categorias.Add(calculosGrupo.descripcion);
                //gpx_oVSt_grupo.serie1Descripcion = "UTILIZACIÓN";
                //gpx_oVSt_grupo.serie1.Add(Math.Round(((calculosGrupo.tiempos.hrsOperacion / calculosGrupo.tiempos.hrsDisponible) * 100), 2));
                //gpx_oVSt_grupo.serie2Descripcion = "EFICIENCIA DE LA UTILIZACIÓN";
                //gpx_oVSt_grupo.serie2.Add(Math.Round(((calculosGrupo.tiempos.hrsTrabajo / calculosGrupo.tiempos.hrsOperacion) * 100), 2));

                //gpx_UT_grupo.categorias.Add(calculosGrupo.descripcion);
                //gpx_UT_grupo.serie1Descripcion = "UTILIZACIÓN";
                //gpx_UT_grupo.serie1.Add(Math.Round(((calculosGrupo.tiempos.hrsTrabajo / calculosGrupo.tiempos.hrsProgramado) * 100), 2));
                #endregion
            }
            #endregion

            #region infoGrafic_gpx_mensual_semanal
            //gpx_dVSu_mensual.serie1Descripcion = "DISPONIBILIDAD";
            //gpx_dVSu_mensual.serie2Descripcion = "UTILIZACIÓN";

            //gpx_dVSu_semanal.serie1Descripcion = "DISPONIBILIDAD";
            //gpx_dVSu_semanal.serie2Descripcion = "UTILIZACIÓN";

            //gpx_oVSt_semanal.serie1Descripcion = "DISPONIBILIDAD";
            //gpx_oVSt_semanal.serie2Descripcion = "EFICIENCIA DE LA UTILIZACIÓN";

            //gpx_oVSt_mensual.serie1Descripcion = "DISPONIBILIDAD";
            //gpx_oVSt_mensual.serie2Descripcion = "EFICIENCIA DE LA UTILIZACIÓN";

            //gpx_UT_semanal.serie1Descripcion = "UTILIZACIÓN";
            //gpx_UT_mensual.serie1Descripcion = "UTILIZACIÓN";

            //foreach (var mes in meses)
            //{
            //    var _consulta = datosHomologados.Where
            //        (w =>
            //            w.homolo.fecha >= mes.fechaInicio &&
            //            w.homolo.fecha <= mes.fechaFin
            //        );

            //    if (_consulta.Count() == 0)
            //    {
            //        continue;
            //    }
            //    var equipos = datosHomologados.Select(x => x.maquina.noEconomico).Distinct().Count();
            //    var __hrsProgramadoAlMes = (DatetimeUtils.DiasDiferencia(mes.fechaInicio, mes.fechaFin) * horasDia) * equipos;

            //    var calculos = CalculosTiempos(_consulta.GroupBy(g => g.homolo.idGrupo).First(), __hrsProgramadoAlMes);


            //    gpx_dVSu_mensual.categorias.Add(mes.fechaInicio.ToShortDateString() + " al " + mes.fechaFin.ToShortDateString());
            //    gpx_dVSu_mensual.serie1.Add(Math.Round(((calculos.hrsDisponible / calculos.hrsProgramado) * 100), 2));
            //    gpx_dVSu_mensual.serie2.Add(Math.Round(((calculos.hrsOperacion / calculos.hrsDisponible) * 100), 2));

            //    gpx_oVSt_mensual.categorias.Add(mes.fechaInicio.ToShortDateString() + " al " + mes.fechaFin.ToShortDateString());
            //    gpx_oVSt_mensual.serie1.Add(Math.Round(((calculos.hrsOperacion / calculos.hrsDisponible) * 100), 2));
            //    gpx_oVSt_mensual.serie2.Add(Math.Round(((calculos.hrsTrabajo / calculos.hrsOperacion) * 100), 2));

            //    gpx_UT_mensual.categorias.Add(mes.fechaInicio.ToShortDateString() + " al " + mes.fechaFin.ToShortDateString());
            //    gpx_UT_mensual.serie1.Add(Math.Round(((calculos.hrsTrabajo / calculos.hrsProgramado) * 100), 2));
            //}

            //foreach (var semana in semanas)
            //{
            //    var _consulta = datosHomologados.Where
            //        (w =>
            //            w.homolo.fecha >= semana.fechaInicio &&
            //            w.homolo.fecha <= semana.fechaFin
            //        );

            //    if (_consulta.Count() == 0)
            //    {
            //        continue;
            //    }
            //    var equipos = datosHomologados.Select(x => x.maquina.noEconomico).Distinct().Count();
            //    var __hrsProgramadoAlaSemana = (DatetimeUtils.DiasDiferencia(semana.fechaInicio, semana.fechaFin) * horasDia) * equipos;

            //    var calculos = CalculosTiempos(_consulta.GroupBy(g => g.homolo.idGrupo).First(), __hrsProgramadoAlaSemana);

            //    gpx_dVSu_semanal.categorias.Add(semana.fechaInicio.ToShortDateString() + " al " + semana.fechaFin.ToShortDateString());
            //    gpx_dVSu_semanal.serie1.Add(Math.Round(((calculos.hrsDisponible / calculos.hrsProgramado) * 100), 2));
            //    gpx_dVSu_semanal.serie2.Add(Math.Round(((calculos.hrsOperacion / calculos.hrsDisponible) * 100), 2));

            //    gpx_oVSt_semanal.categorias.Add(semana.fechaInicio.ToShortDateString() + " al " + semana.fechaFin.ToShortDateString());
            //    gpx_oVSt_semanal.serie1.Add(Math.Round(((calculos.hrsOperacion / calculos.hrsDisponible) * 100), 2));
            //    gpx_oVSt_semanal.serie2.Add(Math.Round(((calculos.hrsTrabajo / calculos.hrsOperacion) * 100), 2));

            //    gpx_UT_semanal.categorias.Add(semana.fechaInicio.ToShortDateString() + " al " + semana.fechaFin.ToShortDateString());
            //    gpx_UT_semanal.serie1.Add(Math.Round(((calculos.hrsTrabajo / calculos.hrsProgramado) * 100), 2));
            //}
            #endregion

            #region Tabla tiempos
            //var tiempos = new TablaTiemposDTO();
            //var numEconomicos = disponibilidadVsUtilizacionEconomico.Count > 0 ? disponibilidadVsUtilizacionEconomico.Count : 1;

            //tiempos.hrsProgramado = hrsProgramado * __equipos.Count();
            //tiempos.hrsProgramadoSM = Math.Round(disponibilidadVsUtilizacionEconomico.Sum(s => s.tiempos.hrsProgramadoSM), 2);
            //tiempos.hrsNoProgramadoUM = Math.Round(disponibilidadVsUtilizacionEconomico.Sum(s => s.tiempos.hrsNoProgramadoUM), 2);
            //tiempos.hrsMantenimiento = Math.Round(disponibilidadVsUtilizacionEconomico.Sum(s => s.tiempos.hrsMantenimiento), 2);
            //tiempos.hrsDisponible = Math.Round(tiempos.hrsProgramado - tiempos.hrsMantenimiento, 2);
            //tiempos.hrsParado = Math.Round(disponibilidadVsUtilizacionEconomico.Sum(s => s.tiempos.hrsParado), 2);
            //tiempos.hrsOperacion = Math.Round(tiempos.hrsDisponible - tiempos.hrsParado, 2);
            //tiempos.hrsDemora = Math.Round(disponibilidadVsUtilizacionEconomico.Sum(s => s.tiempos.hrsDemora), 2);
            //tiempos.hrsTrabajo = Math.Round(tiempos.hrsOperacion - tiempos.hrsDemora, 2);

            //tiempos.porNoProgramadoUM = Math.Round((tiempos.hrsNoProgramadoUM * 100) / (tiempos.hrsMantenimiento > 0 ? tiempos.hrsMantenimiento : 1), 2);
            //tiempos.porProgramadoSM = Math.Round((tiempos.hrsProgramadoSM * 100) / (tiempos.hrsMantenimiento > 0 ? tiempos.hrsMantenimiento : 1), 2);
            //tiempos.porDisponible = Math.Round((tiempos.hrsDisponible * 100) / (tiempos.hrsProgramado), 2);

            //tiempos.porMantenimiento = Math.Round(100 - tiempos.porDisponible, 2);
            //tiempos.porOperacion = Math.Round((tiempos.hrsOperacion * 100) / (tiempos.hrsDisponible > 0 ? tiempos.hrsDisponible : 1), 2);
            //tiempos.porParado = Math.Round(100 - tiempos.porOperacion, 2);
            //tiempos.porTrabajo = Math.Round((tiempos.hrsTrabajo * 100) / (tiempos.hrsOperacion > 0 ? tiempos.hrsOperacion : 1), 2);
            //tiempos.porDemora = Math.Round(100 - tiempos.porTrabajo, 2);
            #endregion

            //resultados.tiempos = tiempos;

            //resultados.disVsUti_economico = disponibilidadVsUtilizacionEconomico;
            resultados.disVsUti_modeloDiario = disponibilidadVsUtilizacionModeloDiario;
            resultados.disVsUti_modeloSemanal = disponibilidadVsUtilizacionModeloSemanal;
            resultados.disVsUti_modeloMensual = disponibilidadVsUtilizacionModeloMensual;
            //resultados.disVsUti_grupo = disponibilidadVsUtilizacionGrupo;

            //resultados.gpx_disVsUti_economico = gpx_dVSu_economico;
            resultados.gpx_disVsUti_modeloDiario = gpx_dVSu_modeloDiario;
            resultados.gpx_disVsUti_modeloSemanal = gpx_dVSu_modeloSemanal;
            resultados.gpx_disVsUti_modeloMensual = gpx_dVSu_modeloMensual;
            //resultados.gpx_disVsUti_grupo = gpx_dVSu_grupo;
            //resultados.gpx_disVsUti_semanal = gpx_dVSu_semanal;
            //resultados.gpx_disVsUti_mensual = gpx_dVSu_mensual;

            //resultados.gpx_opeVsTra_economico = gpx_oVSt_economico;
            //resultados.gpx_opeVsTra_modelo = gpx_oVSt_modelo;
            //resultados.gpx_opeVsTra_grupo = gpx_oVSt_grupo;
            //resultados.gpx_opeVsTra_semanal = gpx_oVSt_semanal;
            //resultados.gpx_opeVsTra_mensual = gpx_oVSt_mensual;

            //resultados.gpx_UT_economico = gpx_UT_economico;
            //resultados.gpx_UT_modelo = gpx_UT_modelo;
            //resultados.gpx_UT_grupo = gpx_UT_grupo;
            //resultados.gpx_UT_semanal = gpx_UT_semanal;
            //resultados.gpx_UT_mensual = gpx_UT_mensual;

            return resultados;
        }

        public List<string> GetLstCorreosFacultamientos(string ac)
        {
            try
            {
                var lstAutorizadores = _context.tblP_CC_Usuario.ToList();
                var lstUsuarios = _context.tblP_Usuario.ToList();
                var lstCorreos = new List<string>();

                var usuariosAC = lstAutorizadores.Where(x => x.cc.Equals(ac)).Select(x => x.id).ToList();
                var autorizadores = _context.tblP_Autoriza.Where(x => usuariosAC.Contains(x.cc_usuario_ID)).ToList();

                var adminMaq = autorizadores.FirstOrDefault(x => x.perfilAutorizaID == 5);
                var gerenteObra = autorizadores.FirstOrDefault(x => x.perfilAutorizaID == 1);
                var directorArea = autorizadores.FirstOrDefault(x => x.perfilAutorizaID == 4);
                var directoDivision = autorizadores.FirstOrDefault(x => x.perfilAutorizaID == 2);
                //var directorServicios = autorizadores.FirstOrDefault(x => x.perfilAutorizaID == 11);
                //var altaDireccion = autorizadores.FirstOrDefault(x => x.perfilAutorizaID == 3);

                if (adminMaq != null)
                {
                    var objUsuarioAdmin = lstUsuarios.FirstOrDefault(e => e.id == adminMaq.usuarioID);

                    if (objUsuarioAdmin != null)
                    {
                        lstCorreos.Add(objUsuarioAdmin.correo);
                    }
                }

                if (gerenteObra != null)
                {
                    var objUsuarioGerenteObra = lstUsuarios.FirstOrDefault(e => e.id == gerenteObra.usuarioID);

                    if (objUsuarioGerenteObra != null)
                    {
                        lstCorreos.Add(objUsuarioGerenteObra.correo);
                    }
                }

                if (directorArea != null)
                {
                    var objUsuarioDirectorArea = lstUsuarios.FirstOrDefault(e => e.id == directorArea.usuarioID);

                    if (objUsuarioDirectorArea != null)
                    {
                        lstCorreos.Add(objUsuarioDirectorArea.correo);
                    }
                }

                if (directoDivision != null)
                {
                    var objUsuarioDirectorDivision = lstUsuarios.FirstOrDefault(e => e.id == directoDivision.usuarioID);

                    if (objUsuarioDirectorDivision != null)
                    {
                        lstCorreos.Add(objUsuarioDirectorDivision.correo);
                    }
                }

                lstCorreos = lstCorreos.Distinct().ToList();

                return lstCorreos;
            }
            catch (Exception e)
            {
                return new List<string>();
            }
        }
        #endregion

        public tblM_KPI_KPICapturaBit getHomologadobit(int id)
        {
            var kpiCapturaBit = _context.tblM_KPI_KPICapturaBit.FirstOrDefault(r => r.idAutoriza == id);
            return kpiCapturaBit;
        }
    }
}
