using Core.DTO.ControlObra;
using Core.DAO.ControlObra;
using Core.Entity.ControlObra;
using Data.EntityFramework.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Globalization;
using System.Data;
using System.Data.OleDb;
using System.Web;
using System.IO;
using Data.Factory.Principal.Archivos;
using Data.DAO.Principal.Usuarios;
using Core.DTO;
using Core.Enum.ControlObra;
using Core.DTO.Principal.Multiempresas;
using Data.DAO.Principal.Usuarios;
using Data.Factory.Principal.Usuarios;
using Data.EntityFramework.Context;
using Newtonsoft.Json;
using Core.Entity.Principal.Multiempresa;
using Core.DTO.Principal.Generales;
using Infrastructure.Utils;
using Core.DTO.ControlObra.EvaluacionSubcontratista;
using Core.DTO.Utils.Data;
using Core.Enum.Principal;


namespace Data.DAO.ControlObra
{
    class ControlObraDAO : GenericDAO<tblCO_Capitulos>, IControlObraDAO
    {

        private Dictionary<string, object> resultado = new Dictionary<string, object>();
        private readonly string RutaBase = @"\\REPOSITORIO\Proyecto\SIGOPLAN\CONTROL_OBRA";
        private const string RutaLocal = @"C:\Proyecto\SIGOPLAN\CONTROL_OBRA";
        private readonly string RutaControlObra;
        public ControlObraDAO()
        {
            resultado.Clear();

#if DEBUG
            RutaControlObra = Path.Combine(RutaLocal, @"EVALUACION_DE_SUBCONTRATISTAS");
#endif

            //RutaControlObra = Path.Combine(RutaBase, @"EVALUACION_DE_SUBCONTRATISTAS");

        }

        #region variables
        // Variables a utilizar en los diccionarios de resultados.
        public readonly string SUCCESS = "success";
        public readonly string ERROR = "error";
        public readonly string estructura = "_Estructura";
        public readonly string precios = "_Precios";
        public static string _msgError = "";

        // Calculos de acumulados en base al avance Anterior
        public static decimal? AntacumAnterior = 0;
        public static decimal? AntacumActual = 0;
        public static decimal? AntavancePeriodoPorcentaje = 0;
        public static decimal? AntavanceAcumActualPorcentaje = 0;
        public static decimal? importeAvanceAnt = 0;
        public static decimal? importeAvancePeriodo = 0;
        public static decimal? importeAvanceAcumulado = 0;

        // ReCalculos de acumulados para el avance siguiente (si es que tiene)
        public static decimal? SigacumAnterior = 0;
        public static decimal? SigacumActual = 0;
        public static decimal? SigavanceAcumActualPorcentaje = 0;

        //private readonly string NOMBRE_CONTROLADOR = "ControlObraController";

        ArchivoFactoryServices archivofs = new ArchivoFactoryServices();
        UsuarioFactoryServices ufs = new UsuarioFactoryServices();
        #endregion

        #region CAPITULOS
        public Dictionary<string, object> getCapitulosList(int capituloID)
        {
            var resultado = new Dictionary<string, object>();

            try
            {

                var proyectos = _context.tblCO_Capitulos.ToList().Where(y => y.estatus == true && (capituloID != 0 ? y.id == capituloID : true)).Select(x => new
                {
                    Value = x.id,
                    Text = x.capitulo
                }).OrderBy(y => y.Text).ToList();

                if (proyectos.Count > 0)
                {
                    resultado.Add(SUCCESS, true);
                    resultado.Add("items", proyectos);
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
                resultado.Add(ERROR, "Ocurrió un error interno al intentar obtener el catálogo de capitulos");
            }

            return resultado;
        }
        public Dictionary<string, object> getCapitulosCatalogo()
        {
            var resultado = new Dictionary<string, object>();

            try
            {
                var capitulos = _context.tblCO_Capitulos.ToList().Where(y => y.estatus == true).Select(x => new CapituloDTO
                {
                    id = x.id,
                    capitulo = x.capitulo,
                    fechaInicio = x.fechaInicio.ToShortDateString(),
                    fechaFin = x.fechaFin.ToShortDateString()
                }).ToList();

                if (capitulos.Count > 0)
                {
                    resultado.Add(SUCCESS, true);
                    resultado.Add("items", capitulos);
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
                resultado.Add(ERROR, "Ocurrió un error interno al intentar obtener el catálogo de capitulos");
            }

            return resultado;
        }
        public Dictionary<string, object> getCapitulo(int capituloID)
        {
            var resultado = new Dictionary<string, object>();

            try
            {
                var capitulo = _context.tblCO_Capitulos.ToList().Where(x => x.id == capituloID).Select(y => new CapituloDTO
                {
                    id = y.id,
                    capitulo = y.capitulo,
                    fechaInicio = y.fechaInicio.ToShortDateString(),
                    fechaFin = y.fechaFin.ToShortDateString(),
                    cc_id = y.cc_id,
                    cc = y.cc == null ? "" : y.cc.cc,
                    autorizante_id = y.autorizante_id,
                    periodoFacturacion = y.periodoFacturacion
                }).FirstOrDefault();

                resultado.Add(SUCCESS, true);
                resultado.Add("capitulo", capitulo);
            }
            catch (Exception e)
            {
                resultado.Add(SUCCESS, false);
                resultado.Add("error", "error");
            }

            return resultado;
        }
        public Dictionary<string, object> guardarCapitulo(tblCO_Capitulos capitulo)
        {
            var resultado = new Dictionary<string, object>();

            try
            {
                _context.tblCO_Capitulos.Add(capitulo);
                _context.SaveChanges();

                if (capitulo.id > 0)
                {
                    resultado.Add(SUCCESS, true);
                }
            }
            catch (Exception)
            {
                resultado.Add(SUCCESS, false);
            }

            return resultado;
        }
        public Dictionary<string, object> updateCapitulo(int capituloID, string capitulo, DateTime fechaInicio, DateTime fechaFin, int? cc_id, int? autorizante_id, int? periodoFacturacion)
        {
            var resultado = new Dictionary<string, object>();

            try
            {
                var tblCapitulo = _context.tblCO_Capitulos.Where(x => x.estatus == true && x.id == capituloID).FirstOrDefault();

                if (tblCapitulo != null)
                {
                    tblCapitulo.capitulo = capitulo;
                    tblCapitulo.fechaInicio = fechaInicio;
                    tblCapitulo.fechaFin = fechaFin;
                    tblCapitulo.cc_id = cc_id;
                    tblCapitulo.autorizante_id = autorizante_id;
                    tblCapitulo.periodoFacturacion = periodoFacturacion;

                    _context.Entry(tblCapitulo).State = System.Data.Entity.EntityState.Modified;
                    _context.SaveChanges();

                    resultado.Add(SUCCESS, true);
                }
            }
            catch (Exception)
            {
                resultado.Add(SUCCESS, false);
            }
            return resultado;
        }
        public Dictionary<string, object> removeCapitulo(int capituloID)
        {
            var resultado = new Dictionary<string, object>();

            try
            {
                var capitulo = _context.tblCO_Capitulos.Where(x => x.estatus == true && x.id == capituloID).FirstOrDefault();

                if (capitulo != null)
                {
                    capitulo.estatus = false;
                    _context.Entry(capitulo).State = System.Data.Entity.EntityState.Modified;
                    _context.SaveChanges();

                    resultado.Add(SUCCESS, true);
                }
            }
            catch (Exception)
            {
                resultado.Add(SUCCESS, false);
            }

            return resultado;
        }
        public Dictionary<string, object> obtenerCentrosCostos()
        {
            var resultado = new Dictionary<string, object>();

            try
            {
                var listaCC = _context.tblP_CC.Where(x => x.estatus == true).Select(x => new
                    {
                        Value = x.id,
                        Text = x.cc + " - " + x.descripcion.Trim(),
                        Prefijo = x.cc
                    }).OrderBy(x => x.Text).ToList();

                if (listaCC.Count > 0)
                {
                    resultado.Add(SUCCESS, true);
                    resultado.Add("items", listaCC);
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
            }

            return resultado;
        }
        public Dictionary<string, object> getPeriodoFacturacion()
        {
            var list = new Dictionary<int, string>();

            foreach (var name in Enum.GetNames(typeof(PeriodoFacturacionEnum)))
            {
                list.Add((int)Enum.Parse(typeof(PeriodoFacturacionEnum), name), Infrastructure.Utils.EnumExtensions.GetDescription((Enum)Enum.Parse(typeof(PeriodoFacturacionEnum), name)));
            }

            var result = new Dictionary<string, object>();
            result.Add("items", list.Select(x => new { Value = x.Key, Text = x.Value }).OrderBy(x => x.Value));
            result.Add(SUCCESS, true);

            return result;
        }
        public Dictionary<int, string> getStringPeriodoFacturacion()
        {
            var list = new Dictionary<int, string>();

            foreach (var name in Enum.GetNames(typeof(PeriodoFacturacionEnum)))
            {
                list.Add((int)Enum.Parse(typeof(PeriodoFacturacionEnum), name), Infrastructure.Utils.EnumExtensions.GetDescription((Enum)Enum.Parse(typeof(PeriodoFacturacionEnum), name)));
            }

            return list;
        }
        public int guardarCapituloID(tblCO_Capitulos capitulo)
        {
            int id = 0;
            try
            {
                _context.tblCO_Capitulos.Add(capitulo);
                _context.SaveChanges();

                id = capitulo.id > 0 ? capitulo.id : 0;
            }
            catch (Exception)
            {
                id = 0;
            }

            return id;
        }

        #endregion

        #region SUBCAPITULOS NIVEL I
        public Dictionary<string, object> getSubcapitulosN1List(int capituloID)
        {
            var resultado = new Dictionary<string, object>();

            try
            {

                var capitulos = _context.tblCO_Subcapitulos_Nivel1.ToList().Where(y => y.estatus == true && (y.capitulo_id == capituloID)).Select(x => new
                {
                    Value = x.id,
                    Text = x.subcapitulo
                }).OrderBy(y => y.Value).ToList();

                if (capitulos.Count > 0)
                {
                    resultado.Add(SUCCESS, true);
                    resultado.Add("items", capitulos);
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
                resultado.Add(ERROR, "Ocurrió un error interno al intentar obtener el catálogo de subcapitulos nivel I");
            }

            return resultado;
        }
        public Dictionary<string, object> getSubcapitulosN1Catalogo(List<int> listCapitulosID)
        {
            var resultado = new Dictionary<string, object>();

            try
            {
                var list = _context.tblCO_Subcapitulos_Nivel1.ToList().Where(x =>
                    (x.estatus == true) &&
                    (listCapitulosID != null ? listCapitulosID.Contains(x.capitulo_id) : true)).Select(y => new SubcapitulosNivel1DTO
                    {
                        id = y.id,
                        subcapitulo = y.subcapitulo,
                        fechaInicio = y.fechaInicio.ToShortDateString(),
                        fechaFin = y.fechaFin.ToShortDateString(),
                        capitulo = y.capitulo.capitulo
                    }).ToList();

                if (list.Count > 0)
                {
                    resultado.Add(SUCCESS, true);
                    resultado.Add("items", list);
                }
                else
                {
                    resultado.Add(SUCCESS, false);
                    resultado.Add("EMPTY", true);
                }
            }
            catch (Exception)
            {
                resultado.Add(SUCCESS, false);
                resultado.Add(ERROR, "Ocurrió un error interno al intentar obtener el catálogo de subcapitulos nivel I");
            }

            return resultado;
        }
        public Dictionary<string, object> getSubcapituloN1(int subcapituloID)
        {
            var resultado = new Dictionary<string, object>();

            try
            {
                var subcapitulo = _context.tblCO_Subcapitulos_Nivel1.ToList().Where(x => x.id == subcapituloID).Select(y => new SubcapitulosNivel1DTO
                {
                    id = y.id,
                    subcapitulo = y.subcapitulo,
                    fechaInicio = y.fechaInicio.ToShortDateString(),
                    fechaFin = y.fechaFin.ToShortDateString(),
                    capituloID = y.capitulo_id
                }).FirstOrDefault();

                resultado.Add(SUCCESS, true);
                resultado.Add("subcapitulo", subcapitulo);
            }
            catch (Exception e)
            {
                resultado.Add(SUCCESS, false);
                resultado.Add("error", "error");
            }

            return resultado;
        }
        public Dictionary<string, object> guardarSubcapituloN1(tblCO_Subcapitulos_Nivel1 subcapituloN1)
        {
            var resultado = new Dictionary<string, object>();

            try
            {
                _context.tblCO_Subcapitulos_Nivel1.Add(subcapituloN1);
                _context.SaveChanges();

                if (subcapituloN1.id > 0)
                {
                    resultado.Add(SUCCESS, true);
                }
            }
            catch (Exception)
            {
                resultado.Add(SUCCESS, false);
            }

            return resultado;
        }
        public Dictionary<string, object> updateSubcapituloN1(int subcapituloID, string subcapitulo, DateTime fechaInicio, DateTime fechaFin, int capituloID)
        {
            var resultado = new Dictionary<string, object>();

            try
            {
                var subcapitulos = _context.tblCO_Subcapitulos_Nivel1.Where(x => x.estatus == true && x.id == subcapituloID).FirstOrDefault();

                if (subcapitulos != null)
                {
                    subcapitulos.subcapitulo = subcapitulo;
                    subcapitulos.fechaInicio = fechaInicio;
                    subcapitulos.fechaFin = fechaFin;
                    subcapitulos.capitulo_id = capituloID;

                    _context.Entry(subcapitulos).State = System.Data.Entity.EntityState.Modified;
                    _context.SaveChanges();

                    resultado.Add(SUCCESS, true);
                }
            }
            catch (Exception)
            {
                resultado.Add(SUCCESS, false);
            }
            return resultado;
        }
        public Dictionary<string, object> removeSubcapituloN1(int subcapituloID)
        {
            var resultado = new Dictionary<string, object>();

            try
            {
                var subcapitulo = _context.tblCO_Subcapitulos_Nivel1.Where(x => x.estatus == true && x.id == subcapituloID).FirstOrDefault();

                if (subcapitulo != null)
                {
                    subcapitulo.estatus = false;
                    _context.Entry(subcapitulo).State = System.Data.Entity.EntityState.Modified;
                    _context.SaveChanges();

                    resultado.Add(SUCCESS, true);
                }
            }
            catch (Exception)
            {
                resultado.Add(SUCCESS, false);
            }

            return resultado;
        }
        public int guardarSubcapituloN1_ID(tblCO_Subcapitulos_Nivel1 subcapituloN1)
        {
            int id = 0;
            try
            {
                _context.tblCO_Subcapitulos_Nivel1.Add(subcapituloN1);
                _context.SaveChanges();

                id = subcapituloN1.id > 0 ? subcapituloN1.id : 0;
            }
            catch (Exception)
            {
                id = 0;
            }

            return id;
        }
        #endregion

        #region SUBCAPITULOS NIVEL II
        public Dictionary<string, object> getSubcapitulosN2List(int subcapituloN1_id)
        {
            var resultado = new Dictionary<string, object>();

            try
            {

                var subcapitulos = _context.tblCO_Subcapitulos_Nivel2.ToList().Where(y => y.estatus == true && (y.subcapituloN1_id == subcapituloN1_id)).Select(x => new
                {
                    Value = x.id,
                    Text = x.subcapitulo
                }).OrderBy(y => y.Value).ToList();

                if (subcapitulos.Count > 0)
                {
                    resultado.Add(SUCCESS, true);
                    resultado.Add("items", subcapitulos);
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
                resultado.Add(ERROR, "Ocurrió un error interno al intentar obtener el catálogo de subcapitulos nivel II");
            }

            return resultado;
        }
        public Dictionary<string, object> getSubcapitulosN2Catalogo(int subcapituloN1_id)
        {
            var resultado = new Dictionary<string, object>();

            try
            {

                var list = _context.tblCO_Subcapitulos_Nivel2.ToList().Where(x =>
                    (x.estatus == true) &&
                    (x.subcapituloN1_id == subcapituloN1_id)).Select(y => new SubcapitulosNivel2DTO
                    {
                        id = y.id,
                        subcapitulo = y.subcapitulo,
                        fechaInicio = y.fechaInicio.ToShortDateString(),
                        fechaFin = y.fechaFin.ToShortDateString(),
                        subcapituloN1 = y.subcapitulos_N1.subcapitulo,
                        capitulo = y.subcapitulos_N1.capitulo.capitulo
                    }).ToList();

                if (list.Count > 0)
                {
                    resultado.Add(SUCCESS, true);
                    resultado.Add("items", list);
                }
                else
                {
                    resultado.Add(SUCCESS, false);
                    resultado.Add("EMPTY", true);
                }
            }
            catch (Exception)
            {
                resultado.Add(SUCCESS, false);
                resultado.Add(ERROR, "Ocurrió un error interno al intentar obtener el catálogo de subcapitulos nivel II");
            }

            return resultado;
        }
        public Dictionary<string, object> getSubcapituloN2(int subcapituloID)
        {
            var resultado = new Dictionary<string, object>();

            try
            {


                var subcapitulo = _context.tblCO_Subcapitulos_Nivel2.ToList().Where(x => x.id == subcapituloID).Select(y => new SubcapitulosNivel2DTO
                {
                    id = y.id,
                    subcapitulo = y.subcapitulo,
                    fechaInicio = y.fechaInicio.ToShortDateString(),
                    fechaFin = y.fechaFin.ToShortDateString(),
                    subcapituloN1_id = y.subcapituloN1_id,
                    capitulo_id = y.subcapitulos_N1.capitulo.id
                }).FirstOrDefault();

                resultado.Add(SUCCESS, true);
                resultado.Add("subcapitulo", subcapitulo);
            }
            catch (Exception e)
            {
                resultado.Add(SUCCESS, false);
                resultado.Add("error", "error");
            }

            return resultado;
        }
        public Dictionary<string, object> guardarSubcapituloN2(tblCO_Subcapitulos_Nivel2 subcapituloN2)
        {
            var resultado = new Dictionary<string, object>();

            try
            {
                _context.tblCO_Subcapitulos_Nivel2.Add(subcapituloN2);
                _context.SaveChanges();

                if (subcapituloN2.id > 0)
                {
                    resultado.Add(SUCCESS, true);
                }
            }
            catch (Exception)
            {
                resultado.Add(SUCCESS, false);
            }

            return resultado;
        }
        public Dictionary<string, object> updateSubcapituloN2(int subcapituloID, string subcapitulo, DateTime fechaInicio, DateTime fechaFin, int subcapituloN1_id)
        {
            var resultado = new Dictionary<string, object>();

            try
            {
                var subcapitulos = _context.tblCO_Subcapitulos_Nivel2.Where(x => x.estatus == true && x.id == subcapituloID).FirstOrDefault();

                if (subcapitulos != null)
                {
                    subcapitulos.subcapitulo = subcapitulo;
                    subcapitulos.fechaInicio = fechaInicio;
                    subcapitulos.fechaFin = fechaFin;
                    subcapitulos.subcapituloN1_id = subcapituloN1_id;

                    _context.Entry(subcapitulos).State = System.Data.Entity.EntityState.Modified;
                    _context.SaveChanges();

                    resultado.Add(SUCCESS, true);
                }
            }
            catch (Exception)
            {
                resultado.Add(SUCCESS, false);
            }
            return resultado;
        }
        public Dictionary<string, object> removeSubcapituloN2(int subcapituloID)
        {
            var resultado = new Dictionary<string, object>();

            try
            {
                var subcapitulo = _context.tblCO_Subcapitulos_Nivel2.Where(x => x.estatus == true && x.id == subcapituloID).FirstOrDefault();

                if (subcapitulo != null)
                {
                    subcapitulo.estatus = false;
                    _context.Entry(subcapitulo).State = System.Data.Entity.EntityState.Modified;
                    _context.SaveChanges();

                    resultado.Add(SUCCESS, true);
                }
            }
            catch (Exception)
            {
                resultado.Add(SUCCESS, false);
            }

            return resultado;
        }
        public int guardarSubcapituloN2_ID(tblCO_Subcapitulos_Nivel2 subcapituloN2)
        {
            int id = 0;
            try
            {
                _context.tblCO_Subcapitulos_Nivel2.Add(subcapituloN2);
                _context.SaveChanges();

                id = subcapituloN2.id > 0 ? subcapituloN2.id : 0;
            }
            catch (Exception)
            {
                id = 0;
            }

            return id;
        }
        #endregion

        #region SUBCAPITULOS NIVEL III
        public Dictionary<string, object> getSubcapitulosN3List(int subcapituloN2_id)
        {
            var resultado = new Dictionary<string, object>();

            try
            {

                var subcapitulos = _context.tblCO_Subcapitulos_Nivel3.ToList().Where(y => y.estatus == true && (y.subcapituloN2_id == subcapituloN2_id)).Select(x => new
                {
                    Value = x.id,
                    Text = x.subcapitulo
                }).OrderBy(y => y.Value).ToList();

                if (subcapitulos.Count > 0)
                {
                    resultado.Add(SUCCESS, true);
                    resultado.Add("items", subcapitulos);
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
                resultado.Add(ERROR, "Ocurrió un error interno al intentar obtener el catálogo de subcapitulos nivel III");
            }

            return resultado;
        }
        public Dictionary<string, object> getSubcapitulosN3Catalogo(int subcapituloN2_id)
        {
            var resultado = new Dictionary<string, object>();

            try
            {

                var list = _context.tblCO_Subcapitulos_Nivel3.ToList().Where(x =>
                    (x.estatus == true) &&
                    (x.subcapituloN2_id == subcapituloN2_id)).Select(y => new SubcapitulosNivel3DTO
                    {
                        id = y.id,
                        subcapitulo = y.subcapitulo,
                        fechaInicio = y.fechaInicio.ToShortDateString(),
                        fechaFin = y.fechaFin.ToShortDateString(),
                        subcapituloN2_id = y.subcapituloN2_id
                    }).ToList();

                if (list.Count > 0)
                {
                    resultado.Add(SUCCESS, true);
                    resultado.Add("items", list);
                }
                else
                {
                    resultado.Add(SUCCESS, false);
                    resultado.Add("EMPTY", true);
                }
            }
            catch (Exception)
            {
                resultado.Add(SUCCESS, false);
                resultado.Add(ERROR, "Ocurrió un error interno al intentar obtener el catálogo de subcapitulos nivel III");
            }

            return resultado;
        }
        public Dictionary<string, object> getSubcapituloN3(int subcapituloID)
        {
            var resultado = new Dictionary<string, object>();

            try
            {


                var subcapitulo = _context.tblCO_Subcapitulos_Nivel3.ToList().Where(x => x.id == subcapituloID).Select(y => new SubcapitulosNivel3DTO
                {
                    id = y.id,
                    subcapitulo = y.subcapitulo,
                    fechaInicio = y.fechaInicio.ToShortDateString(),
                    fechaFin = y.fechaFin.ToShortDateString(),
                    subcapituloN2_id = y.subcapituloN2_id,
                    subcapituloN1_id = y.subcapitulo_N2.subcapituloN1_id,
                    capitulo_id = y.subcapitulo_N2.subcapitulos_N1.capitulo.id
                }).FirstOrDefault();

                resultado.Add(SUCCESS, true);
                resultado.Add("subcapitulo", subcapitulo);
            }
            catch (Exception e)
            {
                resultado.Add(SUCCESS, false);
                resultado.Add("error", "error");
            }

            return resultado;
        }
        public Dictionary<string, object> guardarSubcapituloN3(tblCO_Subcapitulos_Nivel3 subcapituloN3)
        {
            var resultado = new Dictionary<string, object>();

            try
            {
                _context.tblCO_Subcapitulos_Nivel3.Add(subcapituloN3);
                _context.SaveChanges();

                if (subcapituloN3.id > 0)
                {
                    resultado.Add(SUCCESS, true);
                }
            }
            catch (Exception)
            {
                resultado.Add(SUCCESS, false);
            }

            return resultado;
        }
        public Dictionary<string, object> updateSubcapituloN3(int subcapituloID, string subcapitulo, DateTime fechaInicio, DateTime fechaFin, int subcapituloN2_id)
        {
            var resultado = new Dictionary<string, object>();

            try
            {
                var subcapitulos = _context.tblCO_Subcapitulos_Nivel3.Where(x => x.estatus == true && x.id == subcapituloID).FirstOrDefault();

                if (subcapitulos != null)
                {
                    subcapitulos.subcapitulo = subcapitulo;
                    subcapitulos.fechaInicio = fechaInicio;
                    subcapitulos.fechaFin = fechaFin;
                    subcapitulos.subcapituloN2_id = subcapituloN2_id;

                    _context.Entry(subcapitulos).State = System.Data.Entity.EntityState.Modified;
                    _context.SaveChanges();

                    resultado.Add(SUCCESS, true);
                }
            }
            catch (Exception)
            {
                resultado.Add(SUCCESS, false);
            }
            return resultado;
        }
        public Dictionary<string, object> removeSubcapituloN3(int subcapituloID)
        {
            var resultado = new Dictionary<string, object>();

            try
            {
                var subcapitulo = _context.tblCO_Subcapitulos_Nivel3.Where(x => x.estatus == true && x.id == subcapituloID).FirstOrDefault();

                if (subcapitulo != null)
                {
                    subcapitulo.estatus = false;
                    _context.Entry(subcapitulo).State = System.Data.Entity.EntityState.Modified;
                    _context.SaveChanges();

                    resultado.Add(SUCCESS, true);
                }
            }
            catch (Exception)
            {
                resultado.Add(SUCCESS, false);
            }

            return resultado;
        }
        public int guardarSubcapituloN3_ID(tblCO_Subcapitulos_Nivel3 subcapituloN3)
        {
            int id = 0;
            try
            {
                _context.tblCO_Subcapitulos_Nivel3.Add(subcapituloN3);
                _context.SaveChanges();

                id = subcapituloN3.id > 0 ? subcapituloN3.id : 0;
            }
            catch (Exception)
            {
                id = 0;
            }

            return id;
        }
        #endregion

        #region ACTIVIDADES
        public Dictionary<string, object> getActividadesList(int subcapitulosN1_id, int subcapitulosN2_id, int subcapitulosN3_id)
        {
            var resultado = new Dictionary<string, object>();

            try
            {

                var actividades = _context.tblCO_Actividades.ToList().Where(y => y.estatus == true &&
                    (subcapitulosN1_id == -1 ? subcapitulosN2_id == -1 ? subcapitulosN3_id == -1 ? false : y.subcapituloN3_id == subcapitulosN3_id : y.subcapituloN2_id == subcapitulosN2_id : y.subcapituloN1_id == subcapitulosN1_id)
                    ).Select(x => new
                {
                    Value = x.id,
                    Text = x.actividad
                }).OrderBy(y => y.Value).ToList();

                if (actividades.Count > 0)
                {
                    resultado.Add(SUCCESS, true);
                    resultado.Add("items", actividades);
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
                resultado.Add(ERROR, "Ocurrió un error interno al intentar obtener el catálogo de actividades");
            }

            return resultado;
        }
        public Dictionary<string, object> getActividadLigadaSiguiente(int subcapitulosN1_id, int subcapitulosN2_id, int subcapitulosN3_id)
        {
            var resultado = new Dictionary<string, object>();

            try
            {

                //var actividades = _context.tblCO_Actividades.Where(x => x.estatus == true && x.estatusAvance == 0 && x.subFaseID == subFaseID).OrderBy(y => y.actividadPadreID).ToList().Take(1);

                //var actividad = actividades.Select(x => new ComboDTO
                //{
                //    Value = x.id,
                //    Text = x.actividad
                //}).ToList();

                //if (actividad.Count > 0)
                //{
                //    resultado.Add(SUCCESS, true);
                //    resultado.Add("items", actividad);
                //}
                //else
                //{
                //    resultado.Add(SUCCESS, false);
                //    resultado.Add("EMPTY", true);
                //}
            }
            catch (Exception e)
            {
                resultado.Add(SUCCESS, false);
                resultado.Add(ERROR, "Ocurrió un error interno al intentar obtener el catálogo de actividades");
            }

            return resultado;
        }
        public Dictionary<string, object> getActividadesCatalogo(int subcapitulosN1_id, int subcapitulosN2_id, int subcapitulosN3_id)
        {
            var resultado = new Dictionary<string, object>();

            try
            {
                var list = _context.tblCO_Actividades.ToList().Where(x =>
                    (x.estatus == true) &&
                     (subcapitulosN1_id == -1 ? subcapitulosN2_id == -1 ? subcapitulosN3_id == -1 ? false : x.subcapituloN3_id == subcapitulosN3_id : x.subcapituloN2_id == subcapitulosN2_id : x.subcapituloN1_id == subcapitulosN1_id)
                     ).Select(y => new ActividadesDTO
                    {
                        id = y.id,
                        actividad = y.actividad,
                        cantidad = y.cantidad,
                        fechaInicio = y.fechaInicio.ToShortDateString(),
                        fechaFin = y.fechaFin.ToShortDateString(),
                        estatus = y.estatus,
                        actividadPadreRequerida = y.actividadPadreRequerida,
                        subcapituloN3_id = y.subcapituloN3_id,
                        subcapituloN2_id = y.subcapituloN2_id,
                        subcapituloN1_id = y.subcapituloN1_id,
                        unidad_id = y.unidadesCostos.Where(x => x.actividad_id == y.id).Select(x => x.unidad_id).FirstOrDefault(),
                        unidad = y.unidadesCostos.Where(x => x.actividad_id == y.id).Select(x => x.unidad.unidad).FirstOrDefault(),
                        actividadPadre_id = y.actividadPadre_id,
                        precioUnitario = y.precioUnitario,
                        importeContratado = y.importeContratado,
                        tipoPeriodoAvance = y.tipoPeriodoAvance
                    }).OrderBy(x => x.actividadPadre_id).ToList();

                if (list.Count > 0)
                {
                    resultado.Add(SUCCESS, true);
                    resultado.Add("items", list);
                }
                else
                {
                    resultado.Add(SUCCESS, false);
                    resultado.Add("EMPTY", true);
                }
            }
            catch (Exception)
            {
                resultado.Add(SUCCESS, false);
                resultado.Add(ERROR, "Ocurrió un error interno al intentar obtener el catálogo de Actividades");
            }

            return resultado;
        }
        public Dictionary<string, object> getActividad(int actividadID)
        {
            var resultado = new Dictionary<string, object>();

            try
            {
                var actividad = _context.tblCO_Actividades.ToList().Where(x => x.id == actividadID).Select(y => new ActividadesDTO
                {
                    id = y.id,
                    actividad = y.actividad,
                    cantidad = y.cantidad,
                    precioUnitario = y.precioUnitario,
                    fechaInicio = y.fechaInicio.ToShortDateString(),
                    fechaFin = y.fechaFin.ToShortDateString(),
                    estatus = y.estatus,
                    actividadPadreRequerida = y.actividadPadreRequerida,
                    subcapituloN3_id = y.subcapituloN3_id,
                    subcapituloN2_id = y.subcapituloN2_id == null ? y.subcapituloN3_id == null ? -1 : y.subcapitulos_N3.subcapituloN2_id : y.subcapituloN2_id,
                    subcapituloN1_id = y.subcapituloN1_id == null ? y.subcapituloN2_id == null ? y.subcapituloN3_id == null ? -1 : y.subcapitulos_N3.subcapitulo_N2.subcapituloN1_id : y.subcapitulos_N2.subcapituloN1_id : y.subcapituloN1_id,
                    unidad_id = y.unidadesCostos.Where(x => x.actividad_id == y.id).Select(x => x.unidad_id).FirstOrDefault(),
                    unidad = y.unidadesCostos.Where(x => x.actividad_id == y.id).Select(x => x.unidad.unidad).FirstOrDefault(),
                    actividadPadre_id = y.actividadPadre_id,
                    capitulo_id = y.subcapituloN1_id == null ? y.subcapituloN2_id == null ? y.subcapituloN3_id == null ? -1 : y.subcapitulos_N3.subcapitulo_N2.subcapitulos_N1.capitulo_id : y.subcapitulos_N2.subcapitulos_N1.capitulo_id : y.subcapitulos_N1.capitulo_id
                }).FirstOrDefault();

                resultado.Add(SUCCESS, true);
                resultado.Add("actividad", actividad);
            }
            catch (Exception e)
            {
                resultado.Add(SUCCESS, false);
                resultado.Add("error", "error");
            }

            return resultado;
        }
        public Dictionary<string, object> guardarActividad(string actividad, decimal cantidad, int unidad_id, DateTime fechaInicio, DateTime fechaFin, int? subcapitulosN1_id, int? subcapitulosN2_id, int? subcapitulosN3_id, bool estatus, int? actividadPadre_id, bool actividadPadreRequerida, bool actividadTerminada)
        {
            var resultado = new Dictionary<string, object>();

            try
            {
                var actividades = new tblCO_Actividades()
                {
                    actividad = actividad,
                    cantidad = cantidad,
                    fechaInicio = fechaInicio,
                    fechaFin = fechaFin,
                    subcapituloN1_id = subcapitulosN1_id,
                    subcapituloN2_id = subcapitulosN2_id,
                    subcapituloN3_id = subcapitulosN3_id,
                    estatus = estatus,
                    actividadPadre_id = actividadPadre_id,
                    actividadPadreRequerida = actividadPadreRequerida,
                    actividadTerminada = actividadTerminada

                };
                _context.tblCO_Actividades.Add(actividades);
                _context.SaveChanges();


                if (actividades.id > 0)
                {
                    var unidad_actividad = new tblCO_Unidades_Actividad()
                    {
                        unidad_id = unidad_id,
                        actividad_id = actividades.id
                    };
                    _context.tblCO_Unidades_Actividad.Add(unidad_actividad);
                    _context.SaveChanges();

                    resultado.Add(SUCCESS, unidad_actividad.id > 0);
                }
            }
            catch (Exception)
            {
                resultado.Add(SUCCESS, false);
            }

            return resultado;
        }
        public Dictionary<string, object> updateActividad(int actividadID, string actividad, decimal cantidad, int unidad_id, DateTime fechaInicio, DateTime fechaFin, int? subcapitulosN1_id, int? subcapitulosN2_id, int? subcapitulosN3_id, int? actividadPadre_id, bool actividadPadreRequerida)
        {
            var resultado = new Dictionary<string, object>();

            try
            {
                var actividades = _context.tblCO_Actividades.Where(x => x.estatus == true && x.id == actividadID).FirstOrDefault();
                var acitivdad_unidad = _context.tblCO_Unidades_Actividad.Where(x => x.actividad_id == actividadID).FirstOrDefault();

                if (actividades != null)
                {
                    actividades.actividad = actividad;
                    actividades.cantidad = cantidad;
                    actividades.fechaInicio = fechaInicio;
                    actividades.fechaFin = fechaFin;
                    actividades.subcapituloN1_id = subcapitulosN3_id == null ? subcapitulosN2_id == null ? subcapitulosN1_id : null : null;
                    actividades.subcapituloN2_id = subcapitulosN3_id == null ? subcapitulosN2_id == null ? null : subcapitulosN2_id : null;
                    actividades.subcapituloN3_id = subcapitulosN3_id == null ? null : subcapitulosN3_id;
                    actividades.actividadPadre_id = actividadPadre_id;
                    actividades.actividadPadreRequerida = actividadPadreRequerida;

                    _context.Entry(actividades).State = System.Data.Entity.EntityState.Modified;
                    _context.SaveChanges();
                }

                if (acitivdad_unidad != null)
                {
                    acitivdad_unidad.unidad_id = unidad_id;
                    _context.Entry(acitivdad_unidad).State = System.Data.Entity.EntityState.Modified;
                    _context.SaveChanges();
                }

                resultado.Add(SUCCESS, true);
            }
            catch (Exception)
            {
                resultado.Add(SUCCESS, false);
            }
            return resultado;
        }
        public Dictionary<string, object> removeActividad(int actividadID)
        {
            var resultado = new Dictionary<string, object>();

            try
            {
                var actividad = _context.tblCO_Actividades.Where(x => x.estatus == true && x.id == actividadID).FirstOrDefault();

                if (actividad != null)
                {
                    actividad.estatus = false;
                    _context.Entry(actividad).State = System.Data.Entity.EntityState.Modified;
                    _context.SaveChanges();

                    resultado.Add(SUCCESS, true);
                }
            }
            catch (Exception)
            {
                resultado.Add(SUCCESS, false);
            }

            return resultado;
        }
        public Dictionary<string, object> updateActividadPeriodoValor(List<ActividadPeriodoAvanceDTO> archivos)
        {
            var resultado = new Dictionary<string, object>();
            bool seGuardo = false;

            using (DbContextTransaction dbContextTransaction = _context.Database.BeginTransaction())
            {
                try
                {
                    foreach (ActividadPeriodoAvanceDTO archivo in archivos)
                    {
                        seGuardo = updateActividadPeriodo(archivo.actividad_id, archivo.tipoPeriodo);

                        if (!seGuardo) break;
                    }
                }
                catch (Exception e)
                {
                    seGuardo = false;
                    resultado.Add(ERROR, "Ocurrió un error al guardar.");
                }

                if (seGuardo)
                {
                    resultado.Add(SUCCESS, true);
                    dbContextTransaction.Commit();
                }
                else
                {
                    resultado.Add(SUCCESS, false);
                    resultado.Add(ERROR, "Ocurrió un error al guardar.");
                    dbContextTransaction.Rollback();
                }
                dbContextTransaction.Dispose();
            }

            return resultado;
        }
        public int guardarActividad_ID(tblCO_Actividades actividad)
        {
            int id = 0;

            try
            {
                _context.tblCO_Actividades.Add(actividad);
                _context.SaveChanges();

                id = actividad.id > 0 ? actividad.id : 0;
            }
            catch (Exception)
            {
                id = 0;
            }

            return id;
        }
        public bool updateActividadPeriodo(int actividad_id, int tipoPeriodo)
        {
            bool valido = false;

            try
            {
                var actividades = _context.tblCO_Actividades.Where(x => x.id == actividad_id && x.estatus == true).FirstOrDefault();

                if (actividades != null)
                {
                    actividades.tipoPeriodoAvance = tipoPeriodo;
                    _context.Entry(actividades).State = System.Data.Entity.EntityState.Modified;
                    _context.SaveChanges();

                    valido = true;
                }
                else
                {
                    valido = false;
                }

            }
            catch (Exception)
            {
                valido = false;
            }

            return valido;
        }
        #endregion

        #region UNIDADES
        public Dictionary<string, object> getUnidadesList()
        {
            var resultado = new Dictionary<string, object>();

            try
            {

                var unidades = _context.tblCO_Unidades.ToList().Where(y => y.estatus == true).Select(x => new
                {
                    Value = x.id,
                    Text = x.unidad
                }).OrderBy(y => y.Text).ToList();

                if (unidades.Count > 0)
                {
                    resultado.Add(SUCCESS, true);
                    resultado.Add("items", unidades);
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
                resultado.Add(ERROR, "Ocurrió un error interno al intentar obtener el catálogo de unidades");
            }

            return resultado;
        }
        public Dictionary<string, object> getUnidadesCatalogo()
        {
            var resultado = new Dictionary<string, object>();

            try
            {

                var list = _context.tblCO_Unidades.ToList().Where(x =>
                    (x.estatus == true)).Select(y => new UnidadDTO
                    {
                        id = y.id,
                        unidad = y.unidad,
                        estatus = y.estatus

                    }).OrderBy(x => x.unidad).ToList();

                if (list.Count > 0)
                {
                    resultado.Add(SUCCESS, true);
                    resultado.Add("items", list);
                }
                else
                {
                    resultado.Add(SUCCESS, false);
                    resultado.Add("EMPTY", true);
                }
            }
            catch (Exception)
            {
                resultado.Add(SUCCESS, false);
                resultado.Add(ERROR, "Ocurrió un error interno al intentar obtener el catálogo de Unidades");
            }

            return resultado;
        }
        public Dictionary<string, object> getUnidad(int unidadID)
        {
            var resultado = new Dictionary<string, object>();

            try
            {

                var unidad = _context.tblCO_Unidades.ToList().Where(x => x.id == unidadID).Select(y => new UnidadDTO
                {
                    id = y.id,
                    unidad = y.unidad,
                    estatus = y.estatus
                }).FirstOrDefault();

                resultado.Add(SUCCESS, true);
                resultado.Add("unidad", unidad);
            }
            catch (Exception e)
            {
                resultado.Add(SUCCESS, false);
                resultado.Add("error", "error");
            }

            return resultado;
        }
        public Dictionary<string, object> guardarUnidad(string unidad)
        {
            var resultado = new Dictionary<string, object>();

            try
            {
                var unidades = new tblCO_Unidades();
                unidades.unidad = unidad;
                unidades.estatus = true;
                _context.tblCO_Unidades.Add(unidades);
                _context.SaveChanges();

                if (unidades.id > 0)
                {
                    resultado.Add(SUCCESS, true);
                }

            }
            catch (Exception e)
            {
                resultado.Add(SUCCESS, false);
            }

            return resultado;
        }
        public Dictionary<string, object> editarUnidad(int unidadID, string unidad)
        {
            var resultado = new Dictionary<string, object>();

            try
            {
                var unidades = _context.tblCO_Unidades.Where(x => x.estatus == true && x.id == unidadID).FirstOrDefault();

                if (unidades != null)
                {
                    unidades.unidad = unidad;
                    _context.Entry(unidades).State = System.Data.Entity.EntityState.Modified;
                    _context.SaveChanges();

                    resultado.Add(SUCCESS, true);
                }
            }
            catch (Exception)
            {
                resultado.Add(SUCCESS, false);
            }
            return resultado;
        }
        public Dictionary<string, object> removeUnidad(int unidadID)
        {
            var resultado = new Dictionary<string, object>();

            try
            {
                var unidades = _context.tblCO_Unidades.Where(x => x.estatus == true && x.id == unidadID).FirstOrDefault();

                if (unidades != null)
                {
                    unidades.estatus = false;
                    _context.Entry(unidades).State = System.Data.Entity.EntityState.Modified;
                    _context.SaveChanges();

                    resultado.Add(SUCCESS, true);
                }
            }
            catch (Exception)
            {
                resultado.Add(SUCCESS, false);
            }
            return resultado;
        }
        public int guardarUnidad_ID(tblCO_Unidades unidad)
        {
            int id = 0;
            try
            {
                _context.tblCO_Unidades.Add(unidad);
                _context.SaveChanges();

                id = unidad.id > 0 ? unidad.id : 0;
            }
            catch (Exception)
            {
                id = 0;
            }

            return id;
        }
        public int guardarUnidadActividadCosto(tblCO_Unidades_Actividad unidad_actividad)
        {
            int id = 0;
            try
            {
                _context.tblCO_Unidades_Actividad.Add(unidad_actividad);
                _context.SaveChanges();

                id = unidad_actividad.id > 0 ? unidad_actividad.id : 0;
            }
            catch (Exception)
            {
                id = 0;
            }

            return id;
        }
        public bool existeUnidad(string unidad)
        {
            bool existe = false;

            try
            {
                existe = _context.tblCO_Unidades.Where(x => x.unidad == unidad && x.estatus == true).Count() > 0 ? true : false;
            }
            catch (Exception e)
            {
                existe = false;
            }

            return existe;
        }
        public int getIDUnidad(string unidad)
        {
            int id = 0;
            try
            {
                id = _context.tblCO_Unidades.Where(x => x.unidad == unidad && x.estatus == true).Select(y => y.id).FirstOrDefault();
            }
            catch (Exception)
            {
                id = 0;
            }

            return id;
        }
        #endregion

        #region ACTIVIDADES AVANCE
        public Dictionary<string, object> guardarAvance(ActividadAvanceDTO actividadAvance, List<ActividadAvanceDetalleDTO> actividadAvance_detalle)
        {
            var resultado = new Dictionary<string, object>();
            bool seGuardo = false;

            using (DbContextTransaction dbContextTransaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var avancesAnteriores = _context.tblCO_Actividades_Avance.Where(x => x.capitulo_id == actividadAvance.capitulo_id && x.periodoAvance == 2 && (x.fechaInicio >= actividadAvance.fechaI && x.fechaFin <= actividadAvance.fechaF) && x.estatus == true && x.autorizado == false).ToList();

                    if (avancesAnteriores.Count > 0)
                    {
                        foreach (tblCO_Actividades_Avance avance in avancesAnteriores)
                        {
                            seGuardo = eliminarAvance(avance.id);
                            if (!seGuardo) break;
                        }
                    }
                    else seGuardo = true;

                    if (seGuardo)
                    {
                        int avance_id = guardarActividadAvance(actividadAvance);
                        if (avance_id > 0)
                        {
                            foreach (ActividadAvanceDetalleDTO avanceDetalle in actividadAvance_detalle)
                            {
                                seGuardo = guardarAvanceDetalle(avanceDetalle.actividad_id, avanceDetalle.cantidadAvance, avanceDetalle.fechaI, avanceDetalle.fechaF, avance_id);

                                if (!seGuardo) break;
                            }

                        }
                        else
                        {
                            seGuardo = false;
                            resultado.Add(ERROR, "Ocurrió un error al guardar el avance.");
                        }
                    }
                    else
                    {
                        resultado.Add(ERROR, "Ocurrió un error al guardar el avance.");
                    }

                }
                catch (Exception)
                {
                    seGuardo = false;
                }

                if (seGuardo)
                {
                    resultado.Add(SUCCESS, true);
                    dbContextTransaction.Commit();
                }
                else
                {
                    resultado.Add(SUCCESS, false);
                    dbContextTransaction.Rollback();
                }
                dbContextTransaction.Dispose();
            }

            return resultado;
        }
        public bool guardarAvanceDetalle(int actividadID, decimal cantidad, DateTime fechaInicio, DateTime fechaFin, int avanceID)
        {
            bool seGuardo = false;

            try
            {
                var actividades = _context.tblCO_Actividades.Where(x => x.estatus == true && x.id == actividadID).FirstOrDefault();
                bool esValidoAvanceActual = validarAvancePeriodo(cantidad, actividades.cantidad);
                bool validaGuardarAvance = false;
                bool validaActualizarEstatusActividad = false;

                if (esValidoAvanceActual)
                {
                    var actividadAvanceAnterior = _context.tblCO_Actividades_Avance_Detalle.ToList().Where(y => y.actividad_id == actividadID && y.estatus == true && (y.fechaInicio < fechaInicio) && (y.actividadAvance.periodoAvance == 2 ? y.actividadAvance.autorizado == true : y.actividadAvance.autorizado == false)).OrderBy(x => x.fechaInicio).LastOrDefault();
                    //var actividadAvanceSiguiente = _context.tblCO_Actividades_Avance_Detalle.ToList().Where(y => y.actividad_id == actividadID && y.estatus == true && (y.fechaInicio > fechaInicio)).OrderBy(x => x.fechaInicio).FirstOrDefault();

                    calcularAcumuladosAvance(actividadAvanceAnterior, cantidad, actividades.cantidad, actividades.precioUnitario);

                    //SI LO TIENE EL % ACUMULADO DEL NUEVO NO PUEDE SER MAYOR A 100
                    if (validadAcumActual(AntavanceAcumActualPorcentaje))
                    {
                        validaGuardarAvance = guardarActividadAvanceDetalle(actividadID, cantidad, fechaInicio, fechaFin, AntacumAnterior, AntacumActual, AntavancePeriodoPorcentaje, AntavanceAcumActualPorcentaje, avanceID, importeAvanceAnt, importeAvancePeriodo, importeAvanceAcumulado);

                        if (validaGuardarAvance)
                        {
                            //SI EL ACUMULADO DEL AVANCE ES 100% LA ACTIVIDA ESTA TERMINADA
                            if (AntavanceAcumActualPorcentaje == 100)
                            {
                                validaActualizarEstatusActividad = actualizarEstatusActividad(actividades, true);

                                if (validaActualizarEstatusActividad)
                                {
                                    seGuardo = true;
                                }
                                else
                                {
                                    seGuardo = false;
                                    //resultado.Add(ERROR, "Error al cambiar el estatus de la actividad a terminada.");
                                }
                            }
                            else
                            {
                                seGuardo = true;
                            }
                        }
                        else
                        {
                            seGuardo = false;
                            //resultado.Add(ERROR, "Los calculos del avance actual presentan errores.");
                        }

                    }
                    else
                    {
                        seGuardo = false;
                        //resultado.Add(ERROR, "Los calculos del avance actual presentan errores.");
                    }
                }
                else
                {
                    seGuardo = false;
                    // resultado.Add(ERROR, "Los calculos del avance actual presentan errores");
                }
            }
            catch (Exception)
            {
                seGuardo = false;
            }

            return seGuardo;
        }
        public bool validarAvancePeriodo(decimal avancePeriodo, decimal cantidadActividad)
        {
            bool isValid = false;

            if (avancePeriodo > cantidadActividad)
            {
                isValid = false;
            }
            else
            {
                isValid = true;
            }


            return isValid;
        }
        public static void calcularAcumuladosAvance(tblCO_Actividades_Avance_Detalle actividadAvanceAnterior, decimal cantidadAvance, decimal cantidadActividad, decimal? actividadPrecio)
        {
            AntacumAnterior = actividadAvanceAnterior == null ? 0 : actividadAvanceAnterior.acumuladoActual;
            AntacumActual = actividadAvanceAnterior == null ? cantidadAvance : actividadAvanceAnterior.acumuladoActual + cantidadAvance;
            AntavancePeriodoPorcentaje = cantidadActividad > 0 ? (cantidadAvance / cantidadActividad) * 100 : 0;
            AntavanceAcumActualPorcentaje = cantidadActividad > 0 ? actividadAvanceAnterior == null ? (cantidadAvance / cantidadActividad) * 100 : ((actividadAvanceAnterior.acumuladoActual + cantidadAvance) / cantidadActividad) * 100 : 0;

            importeAvanceAnt = actividadAvanceAnterior == null ? 0 : actividadAvanceAnterior.importeAvanceAcumulado;
            importeAvancePeriodo = actividadPrecio * cantidadAvance;
            importeAvanceAcumulado = importeAvanceAnt + importeAvancePeriodo;
        }
        public bool validadAcumActual(decimal? acumActual)
        {
            bool isValid;

            if (acumActual > 100)
            {
                isValid = false;
            }
            else
            {
                isValid = true;
            }

            return isValid;
        }
        public int guardarActividadAvance(ActividadAvanceDTO actividadAvance)
        {
            int id = 0;
            try
            {
                var avance = new tblCO_Actividades_Avance();
                avance.capitulo_id = actividadAvance.capitulo_id;
                avance.fechaInicio = actividadAvance.fechaI;
                avance.fechaFin = actividadAvance.fechaF;
                avance.periodoAvance = actividadAvance.periodoAvance;
                avance.autorizado = actividadAvance.autorizado;
                avance.estatus = actividadAvance.estatus;

                _context.tblCO_Actividades_Avance.Add(avance);
                _context.SaveChanges();

                id = avance.id;
            }
            catch (Exception e)
            {
                id = 0;
            }

            return id;
        }
        public bool guardarActividadAvanceDetalle(int actividadID, decimal cantidad, DateTime fechaInicio, DateTime fechaFin, decimal? AntacumAnterior, decimal? AntacumActual, decimal? AntavancePeriodoPorcentaje, decimal? AntavanceAcumActualPorcentaje, int avanceID, decimal? importeAvanceAnt, decimal? importeAvancePeriodo, decimal? importeAvanceAcumulado)
        {
            bool validaGuardar = false;
            try
            {
                var actividadAvance = new tblCO_Actividades_Avance_Detalle();
                actividadAvance.actividad_id = actividadID;
                actividadAvance.cantidadAvance = cantidad;
                actividadAvance.fechaInicio = fechaInicio;
                actividadAvance.fechaFin = fechaFin;
                actividadAvance.acumuladoAnterior = AntacumAnterior;
                actividadAvance.acumuladoActual = AntacumActual;
                actividadAvance.avancePorcentaje = AntavancePeriodoPorcentaje;
                actividadAvance.avanceAcumuladoPorcentaje = AntavanceAcumActualPorcentaje;
                actividadAvance.actividadAvance_id = avanceID;
                actividadAvance.estatus = true;
                actividadAvance.importeAvanceAnt = importeAvanceAnt;
                actividadAvance.importeAvancePeriodo = importeAvancePeriodo;
                actividadAvance.importeAvanceAcumulado = importeAvanceAcumulado;
                _context.tblCO_Actividades_Avance_Detalle.Add(actividadAvance);
                _context.SaveChanges();

                if (actividadAvance.id > 0) validaGuardar = true;
                else validaGuardar = false;

            }
            catch (Exception e)
            {
                validaGuardar = false;
            }

            return validaGuardar;
        }
        public bool actualizarEstatusActividad(tblCO_Actividades actividad, bool estatus)
        {
            bool validaGuardar = false;

            try
            {
                actividad.actividadTerminada = estatus;
                _context.Entry(actividad).State = System.Data.Entity.EntityState.Modified;
                _context.SaveChanges();

                validaGuardar = true;
            }
            catch (Exception e)
            {
                validaGuardar = false;
            }

            return validaGuardar;
        }
        public Dictionary<string, object> getFechasUltimoAvance(int capituloID)
        {
            var resultado = new Dictionary<string, object>();

            try
            {
                var avanceDiario = _context.tblCO_Actividades_Avance.Where(x => x.capitulo_id == capituloID && x.estatus == true && x.periodoAvance == 1).OrderBy(x => x.fechaInicio).ToList().Select(y => new ActividadAvanceDTO
                {
                    id = y.id,
                    capitulo_id = y.capitulo_id,
                    fechaInicio = y.fechaInicio.ToShortDateString(),
                    fechaFin = y.fechaFin.ToShortDateString()
                }).LastOrDefault();

                var avanceSemanal = _context.tblCO_Actividades_Avance.Where(x => x.capitulo_id == capituloID && x.estatus == true && x.periodoAvance == 2 && x.autorizado == true).OrderBy(z => z.fechaInicio).ToList().Select(y => new ActividadAvanceDTO
                {
                    id = y.id,
                    capitulo_id = y.capitulo_id,
                    fechaInicio = y.fechaInicio.ToShortDateString(),
                    fechaFin = y.fechaFin.ToShortDateString()
                }).LastOrDefault();

                var capitulo = _context.tblCO_Capitulos.ToList().Where(x => x.id == capituloID).Select(y => new CapituloDTO
                {
                    id = y.id,
                    fechaInicio = y.fechaInicio.ToShortDateString(),
                    fechaFin = y.fechaFin.ToShortDateString(),
                    periodoFact = getStringPeriodoFacturacion().Where(x => x.Key == y.periodoFacturacion).FirstOrDefault().Value
                }).FirstOrDefault();


                resultado.Add(SUCCESS, true);
                resultado.Add("avanceDiario", avanceDiario);
                resultado.Add("avanceSemanal", avanceSemanal);
                resultado.Add("capitulo", capitulo);
            }
            catch (Exception e)
            {
                resultado.Add(SUCCESS, false);
                resultado.Add("error", "Ocurrió un error el consultar las fechas");
            }

            return resultado;
        }
        public Dictionary<string, object> guardarAutorizacion(bool autorizacion, int avance_id)
        {
            var resultado = new Dictionary<string, object>();

            try
            {
                var avance = _context.tblCO_Actividades_Avance.Where(x => x.id == avance_id).FirstOrDefault();

                avance.autorizado = autorizacion;
                avance.estatus = autorizacion;

                _context.Entry(avance).State = System.Data.Entity.EntityState.Modified;
                _context.SaveChanges();

                resultado.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                resultado.Add(SUCCESS, false);
                resultado.Add(ERROR, "Ocurrió un error al guardar");
            }
            return resultado;
        }
        public bool eliminarAvance(int avanceID)
        {
            bool validaEliminar = false;

            try
            {
                var avance = _context.tblCO_Actividades_Avance.Where(x => x.id == avanceID && x.estatus == true && x.autorizado == false).FirstOrDefault();
                avance.estatus = false;
                avance.autorizado = false;

                _context.Entry(avance).State = System.Data.Entity.EntityState.Modified;
                _context.SaveChanges();

                validaEliminar = true;
            }
            catch (Exception e)
            {
                validaEliminar = false;
            }

            return validaEliminar;
        }
        public Dictionary<string, object> guardarAvanceFacturado(FacturadoDTO facturado, List<FacturadoDetalleDTO> facturadoDetalle)
        {
            var resultado = new Dictionary<string, object>();
            bool seGuardo = false;

            using (DbContextTransaction dbContextTransaction = _context.Database.BeginTransaction())
            {
                try
                {
                    int facturado_id = guardarFacturadoAvance(facturado);
                    if (facturado_id > 0)
                    {
                        foreach (FacturadoDetalleDTO detalle in facturadoDetalle)
                        {
                            seGuardo = guardarFacturadoDetalle(detalle, facturado_id);
                            if (!seGuardo) break;
                        }
                    }
                    else
                    {
                        seGuardo = false;
                        resultado.Add(ERROR, "Ocurrió un error al guardar el avance.");
                    }
                }
                catch (Exception e)
                {
                    seGuardo = false;
                }


                if (seGuardo)
                {
                    resultado.Add(SUCCESS, true);
                    dbContextTransaction.Commit();
                }
                else
                {
                    resultado.Add(SUCCESS, false);
                    dbContextTransaction.Rollback();
                }
                dbContextTransaction.Dispose();
            }

            return resultado;
        }
        public int guardarFacturadoAvance(FacturadoDTO facturado)
        {
            int id = 0;
            try
            {
                var facturadoAvance = new tblCO_Actividades_Facturado();
                facturadoAvance.capitulo_id = facturado.capitulo_id;
                facturadoAvance.fecha = facturado.fecha;
                facturadoAvance.autorizado = facturado.autorizado;
                facturadoAvance.estatus = facturado.estatus;

                _context.tblCO_Actividades_Facturado.Add(facturadoAvance);
                _context.SaveChanges();

                id = facturadoAvance.id;
            }
            catch (Exception e)
            {
                id = 0;
            }

            return id;
        }
        public bool guardarFacturadoDetalle(FacturadoDetalleDTO facturadoDetalle, int facturado_id)
        {
            bool validaGuardar = false;
            try
            {
                var detalle = new tblCO_Actividades_Facturado_Detalle();
                detalle.actividad_id = facturadoDetalle.actividad_id;
                detalle.volumen = facturadoDetalle.volumen;
                detalle.importe = facturadoDetalle.importe;
                detalle.actividadFacturado_id = facturado_id;
                detalle.estatus = true;
                _context.tblCO_Actividades_Facturado_Detalle.Add(detalle);
                _context.SaveChanges();

                if (detalle.id > 0) validaGuardar = true;
                else validaGuardar = false;
            }
            catch (Exception e)
            {
                validaGuardar = false;
            }

            return validaGuardar;
        }
        public Dictionary<string, object> getPeriodoAvance()
        {
            var list = new Dictionary<int, string>();

            foreach (var name in Enum.GetNames(typeof(periodoAvanceEnum)))
            {
                list.Add((int)Enum.Parse(typeof(periodoAvanceEnum), name), Infrastructure.Utils.EnumExtensions.GetDescription((Enum)Enum.Parse(typeof(periodoAvanceEnum), name)));
            }

            var result = new Dictionary<string, object>();
            result.Add("items", list.Select(x => new { Value = x.Key, Text = x.Value }).OrderBy(x => x.Value));
            result.Add(SUCCESS, true);

            return result;
        }
        #endregion

        #region REPORTE AVANCES
        public Dictionary<string, object> getActividadAvanceReporte(int subcapitulosN1_id, int subcapitulosN2_id, int subcapitulosN3_id, DateTime fechaInicio, DateTime fechaFin)
        {
            var resultado = new Dictionary<string, object>();

            try
            {
                var actividad = _context.tblCO_Actividades.ToList().Where(x => (x.estatus == true) &&
                    (subcapitulosN1_id == -1 ? subcapitulosN2_id == -1 ? subcapitulosN3_id == -1 ? false : x.subcapituloN3_id == subcapitulosN3_id : x.subcapituloN2_id == subcapitulosN2_id : x.subcapituloN1_id == subcapitulosN1_id)).Select(y => y.id).ToList();

                var actividades = _context.tblCO_Actividades.ToList().Where(x => (x.estatus == true) &&
                    (subcapitulosN1_id == -1 ? subcapitulosN2_id == -1 ? subcapitulosN3_id == -1 ? false : x.subcapituloN3_id == subcapitulosN3_id : x.subcapituloN2_id == subcapitulosN2_id : x.subcapituloN1_id == subcapitulosN1_id)).ToList();

                var avances = _context.tblCO_Actividades_Avance_Detalle.ToList().Where(x => actividad.Contains(x.actividad_id) && (x.fechaFin <= fechaFin) && (x.actividadAvance.periodoAvance == 2 ? x.actividadAvance.autorizado == true : x.actividadAvance.autorizado == false)).OrderByDescending(w => w.id).ToList();
                var facturado = _context.tblCO_Actividades_Facturado_Detalle.ToList().Where(x => actividad.Contains(x.actividad_id) && (x.facturado.fecha <= fechaFin)).ToList();

                var reporte = actividades.Select(x => new ReporteAvancesDTO
                {
                    actividad_id = x.id,
                    actividad = x.actividad,
                    unidad = x.unidadesCostos.Where(y => y.actividad_id == x.id).Select(y => y.unidad.unidad).FirstOrDefault(),
                    cantidad = x.cantidad,
                    fechaInicio = x.fechaInicio.ToShortDateString(),
                    fechaFin = x.fechaFin.ToShortDateString(),
                    acumAnterior = avances.ToList().Where(y => y.actividad_id == x.id).Take(1).Select(w => w.acumuladoAnterior).Sum(),
                    acumActual = avances.ToList().Where(y => y.actividad_id == x.id).Take(1).Select(w => w.acumuladoActual).Sum(),
                    avancePorcentaje = avances.ToList().Where(y => y.actividad_id == x.id).Take(1).Select(w => w.avancePorcentaje).Sum(),
                    avanceAcumuladoPorcentaje = avances.ToList().Where(y => y.actividad_id == x.id).Take(1).Select(w => w.avanceAcumuladoPorcentaje).Sum(),
                    subcapituloN3_id = x.subcapituloN3_id,
                    subcapituloN2_id = x.subcapituloN2_id,
                    subcapituloN1_id = x.subcapituloN1_id,
                    actividadPU = x.precioUnitario,
                    actividadCosto = x.costo,
                    importeContratado = x.importeContratado,
                    importeAvanceAnt = avances.ToList().Where(y => y.actividad_id == x.id).Take(1).Select(w => w.importeAvanceAnt).Sum(),
                    importeAvanceAcumulado = avances.ToList().Where(y => y.actividad_id == x.id).Take(1).Select(w => w.importeAvanceAcumulado).Sum(),
                    importeAvancePeriodo = avances.Where(y => (y.actividad_id == x.id) && (y.fechaInicio >= fechaInicio && y.fechaFin <= fechaFin)).Take(1).Select(y => y.importeAvancePeriodo).Sum(),
                    avancePeriodo = avances.Where(y => (y.actividad_id == x.id) && (y.fechaInicio >= fechaInicio && y.fechaFin <= fechaFin)).Select(y => y.cantidadAvance).Sum(),
                    volumenFacturado = facturado.Where(y => y.actividad_id == x.id).Select(w => w.volumen).Sum(),
                    volumenxFacturar = x.cantidad - facturado.Where(y => y.actividad_id == x.id).Select(w => w.volumen).Sum(),
                    importeFacturado = facturado.Where(y => y.actividad_id == x.id).Select(w => w.importe).Sum(),
                    importexFacturar = x.importeContratado - facturado.Where(y => y.actividad_id == x.id).Select(w => w.volumen).Sum(),
                    tipoPeriodoAvance = x.tipoPeriodoAvance
                }).ToList();


                if (reporte.Count > 0)
                {
                    resultado.Add(SUCCESS, true);
                    resultado.Add("items", reporte);
                }
                else
                {
                    resultado.Add(SUCCESS, false);
                    resultado.Add("EMPTY", true);
                }
            }
            catch (Exception)
            {
                resultado.Add(SUCCESS, false);
                resultado.Add(ERROR, "Ocurrió un error interno al intentar obtener el catálogo de avances");
            }

            return resultado;
        }
        public Dictionary<string, object> getConcentradoReporte(int capitulo_id, DateTime fechaInicio, DateTime fechaFin)
        {
            var resultado = new Dictionary<string, object>();

            try
            {
                List<SubcapitulosDTO> elementos = new List<SubcapitulosDTO>();
                List<ReporteConcentradoDTO> concentrado = new List<ReporteConcentradoDTO>();
                var subCapitulosN1 = _context.tblCO_Subcapitulos_Nivel1.Where(x => x.capitulo_id == capitulo_id && x.estatus == true).ToList();
                var subCapitulosN2 = _context.tblCO_Subcapitulos_Nivel2.Where(x => x.subcapitulos_N1.capitulo_id == capitulo_id && x.estatus == true).ToList();
                var subCapitulosN3 = _context.tblCO_Subcapitulos_Nivel3.Where(x => x.subcapitulo_N2.subcapitulos_N1.capitulo_id == capitulo_id && x.estatus == true).ToList();

                List<int?> subCapitulosN1_id = _context.tblCO_Subcapitulos_Nivel1.ToList().Where(x => x.capitulo_id == capitulo_id && x.estatus == true).Select(y => (int?)y.id).ToList();
                List<int?> subCapitulosN2_id = _context.tblCO_Subcapitulos_Nivel2.Where(x => x.subcapitulos_N1.capitulo_id == capitulo_id && x.estatus == true).Select(y => (int?)y.id).ToList();
                List<int?> subCapitulosN3_id = _context.tblCO_Subcapitulos_Nivel3.Where(x => x.subcapitulo_N2.subcapitulos_N1.capitulo_id == capitulo_id && x.estatus == true).Select(y => (int?)y.id).ToList();

                var actividades = _context.tblCO_Actividades.ToList().Where(x => (x.estatus == true) && (subCapitulosN1_id.Contains(x.subcapituloN1_id)) || (subCapitulosN2_id.Contains(x.subcapituloN2_id)) || subCapitulosN3_id.Contains(x.subcapituloN3_id)).ToList();
                var actividades_id = actividades.Select(x => x.id).ToList();

                var avances = _context.tblCO_Actividades_Avance_Detalle.ToList().Where(x => actividades_id.Contains(x.actividad_id) && (x.fechaFin <= fechaFin) && (x.actividadAvance.periodoAvance == 2 ? x.actividadAvance.autorizado == true : x.actividadAvance.autorizado == false)).OrderByDescending(w => w.id).ToList();
                var facturado = _context.tblCO_Actividades_Facturado_Detalle.ToList().Where(x => actividades_id.Contains(x.actividad_id) && (x.facturado.fecha <= fechaFin)).ToList();

                //AGREGAR CAPITULO PRIMERO
                elementos.Add(new SubcapitulosDTO
                {
                    capitulo_id = subCapitulosN1.First().capitulo_id,
                    subcapituloN1 = subCapitulosN1.First().capitulo.capitulo
                });

                foreach (var subcapitulo1 in subCapitulosN1)
                {
                    elementos.Add(new SubcapitulosDTO
                    {
                        subcapituloN1_id = subcapitulo1.id,
                        subcapituloN1 = subcapitulo1.subcapitulo
                    });

                    foreach (var subcapitulo2 in subCapitulosN2.Where(x => x.subcapituloN1_id == subcapitulo1.id).ToList())
                    {
                        elementos.Add(new SubcapitulosDTO
                        {
                            subcapituloN2_id = subcapitulo2.id,
                            subcapituloN2 = subcapitulo2.subcapitulo
                        });

                        foreach (var subcapitulo3 in subCapitulosN3.Where(x => x.subcapituloN2_id == subcapitulo2.id).ToList())
                        {
                            elementos.Add(new SubcapitulosDTO
                            {
                                subcapituloN3_id = subcapitulo3.id,
                                subcapituloN3 = subcapitulo3.subcapitulo
                            });
                        }
                    }
                }

                foreach (var elemento in elementos)
                {
                    decimal? presupuestoCapitulo = actividades.Select(y => y.importeContratado).Sum();
                    decimal? presupuesto = actividades.Where(y => elemento.subcapituloN1_id == null ? elemento.subcapituloN2_id == null ? y.subcapituloN3_id == elemento.subcapituloN3_id : y.subcapituloN2_id == elemento.subcapituloN2_id : y.subcapituloN1_id == elemento.subcapituloN1_id).Select(y => y.importeContratado).Sum();
                    decimal? ejecutadoCapitulo = avances.Select(y => y.importeAvancePeriodo).Sum();
                    decimal? ejecutado = avances.Where(y => elemento.subcapituloN1_id == null ? elemento.subcapituloN2_id == null ? y.actividad.subcapituloN3_id == elemento.subcapituloN3_id : y.actividad.subcapituloN2_id == elemento.subcapituloN2_id : y.actividad.subcapituloN1_id == elemento.subcapituloN1_id).Select(y => y.importeAvancePeriodo).Sum();
                    decimal? xEjecutarCapitulo = presupuestoCapitulo - ejecutadoCapitulo;
                    decimal? xEjecutar = presupuesto - ejecutado;
                    decimal? porcentajeAvanceCapitulo = presupuestoCapitulo > 0 ? ejecutadoCapitulo > 0 ? ejecutadoCapitulo / presupuestoCapitulo : 0 : 0;
                    decimal? porcentajeAvance = presupuesto > 0 ? ejecutado > 0 ? ejecutado / presupuesto : 0 : 0;
                    decimal? facturadoCapitulo = facturado.Select(y => y.importe).Sum();
                    decimal? facturadoC = facturado.Where(y => elemento.subcapituloN1_id == null ? elemento.subcapituloN2_id == null ? y.actividad.subcapituloN3_id == elemento.subcapituloN3_id : y.actividad.subcapituloN2_id == elemento.subcapituloN2_id : y.actividad.subcapituloN1_id == elemento.subcapituloN1_id).Select(y => y.importe).Sum();
                    decimal? xFacturarCapitulo = ejecutadoCapitulo - facturadoCapitulo;
                    decimal? xFacturarC = ejecutado - facturadoC;
                    decimal? semanaCapitulo = avances.Where(x => (x.fechaInicio >= fechaInicio && x.fechaFin <= fechaFin)).Select(y => y.importeAvancePeriodo).Sum();
                    decimal? semanaC = avances.Where(y => (y.fechaInicio >= fechaInicio && y.fechaFin <= fechaFin) && (elemento.subcapituloN1_id == null ? elemento.subcapituloN2_id == null ? y.actividad.subcapituloN3_id == elemento.subcapituloN3_id : y.actividad.subcapituloN2_id == elemento.subcapituloN2_id : y.actividad.subcapituloN1_id == elemento.subcapituloN1_id)).Select(y => y.importeAvancePeriodo).Sum();

                    decimal? presupuestoN1 = 0;
                    decimal? ejecutadoN1 = 0;
                    decimal? xEjecutarN1 = 0;
                    decimal? porcentajeAvanceN1 = 0;
                    decimal? facturadoN1 = 0;
                    decimal? xFacturarN1 = 0;
                    decimal? semanaN1 = 0;

                    decimal? presupuestoN2 = 0;
                    decimal? ejecutadoN2 = 0;
                    decimal? xEjecutarN2 = 0;
                    decimal? porcentajeAvanceN2 = 0;
                    decimal? facturadoN2 = 0;
                    decimal? xFacturarN2 = 0;
                    decimal? semanaN2 = 0;

                    if (elemento.subcapituloN1_id > 0)
                    {
                        List<int?> subcapitulos2 = subCapitulosN2.Where(x => x.subcapituloN1_id == elemento.subcapituloN1_id).Select(y => (int?)y.id).ToList();

                        if (subcapitulos2.Count > 0)
                        {
                            presupuestoN1 = actividades.Where(y => subcapitulos2.Contains(y.subcapituloN2_id)).Select(y => y.importeContratado).ToList().Sum();
                            ejecutadoN1 += avances.Where(y => subcapitulos2.Contains(y.actividad.subcapituloN2_id)).Select(y => y.importeAvancePeriodo).Sum();
                            facturadoN1 += facturado.Where(y => subcapitulos2.Contains(y.actividad.subcapituloN2_id)).Select(y => y.importe).Sum();
                            semanaN1 += avances.Where(y => subcapitulos2.Contains(y.actividad.subcapituloN2_id) && (y.fechaInicio >= fechaInicio && y.fechaFin <= fechaFin)).Select(y => y.importeAvancePeriodo).Sum();

                            List<int?> subcapitulos3 = subCapitulosN3.Where(x => subcapitulos2.Contains(x.subcapituloN2_id)).Select(y => (int?)y.id).ToList();

                            if (subcapitulos3.Count > 0)
                            {
                                presupuestoN1 += actividades.Where(y => subcapitulos3.Contains(y.subcapituloN3_id)).Select(y => y.importeContratado).Sum();
                                ejecutadoN1 += avances.Where(y => subcapitulos3.Contains(y.actividad.subcapituloN3_id)).Select(y => y.importeAvanceAcumulado).Sum();
                                facturadoN1 += facturado.Where(y => subcapitulos3.Contains(y.actividad.subcapituloN3_id)).Select(y => y.importe).Sum();
                                semanaN1 += avances.Where(y => subcapitulos3.Contains(y.actividad.subcapituloN2_id) && (y.fechaInicio >= fechaInicio && y.fechaFin <= fechaFin)).Select(y => y.importeAvancePeriodo).Sum();
                            }
                        }
                        else
                        {
                            presupuestoN1 = presupuesto;
                            ejecutadoN1 = ejecutado;
                            facturadoN1 = facturadoC;
                            semanaN1 = semanaC;
                        }
                    }

                    if (elemento.subcapituloN2_id > 0)
                    {

                        List<int?> subcapitulos3 = subCapitulosN3.Where(x => x.subcapituloN2_id == elemento.subcapituloN2_id).Select(y => (int?)y.id).ToList();

                        if (subcapitulos3.Count > 0)
                        {
                            presupuestoN2 = actividades.Where(y => subcapitulos3.Contains(y.subcapituloN3_id)).Select(y => y.importeContratado).Sum();
                            ejecutadoN2 = avances.Where(y => subcapitulos3.Contains(y.actividad.subcapituloN3_id)).Select(y => y.importeAvanceAcumulado).Sum();
                            facturadoN2 = facturado.Where(y => subcapitulos3.Contains(y.actividad.subcapituloN3_id)).Select(y => y.importe).Sum();
                            semanaN2 = avances.Where(y => subcapitulos3.Contains(y.actividad.subcapituloN3_id) && (y.fechaInicio >= fechaInicio && y.fechaFin <= fechaFin)).Select(y => y.importeAvancePeriodo).Sum();
                        }
                        else
                        {
                            presupuestoN2 = presupuesto;
                            ejecutadoN2 = ejecutado;
                            facturadoN2 = facturadoC;
                            semanaN2 = semanaC;
                        }
                    }

                    xEjecutarN1 = presupuestoN1 - ejecutadoN1;
                    xEjecutarN2 = presupuestoN2 - ejecutadoN2;
                    xFacturarN1 = ejecutadoN1 - facturadoN1;
                    xFacturarN2 = ejecutadoN2 - facturadoN2;
                    porcentajeAvanceN1 = presupuestoN1 > 0 ? ejecutadoN1 > 0 ? ejecutadoN1 / presupuestoN1 : 0 : 0;
                    porcentajeAvanceN2 = presupuestoN2 > 0 ? ejecutadoN2 > 0 ? ejecutadoN2 / presupuestoN2 : 0 : 0;

                    concentrado.Add(new ReporteConcentradoDTO
                    {
                        presupuesto = elemento.capitulo_id > 0 ? presupuestoCapitulo : elemento.subcapituloN1_id > 0 ? presupuestoN1 : elemento.subcapituloN2_id > 0 ? presupuestoN2 : presupuesto,
                        ejecutado = elemento.capitulo_id > 0 ? ejecutadoCapitulo : elemento.subcapituloN1_id > 0 ? ejecutadoN1 : elemento.subcapituloN2_id > 0 ? ejecutadoN2 : ejecutado,
                        xEjecutar = elemento.capitulo_id > 0 ? xEjecutarCapitulo : elemento.subcapituloN1_id > 0 ? xEjecutarN1 : elemento.subcapituloN2_id > 0 ? xEjecutarN2 : xEjecutar,
                        porcentajeAvance = elemento.capitulo_id > 0 ? porcentajeAvanceCapitulo : elemento.subcapituloN1_id > 0 ? porcentajeAvanceN1 : elemento.subcapituloN2_id > 0 ? porcentajeAvanceN2 : porcentajeAvance,
                        importeSemana = elemento.capitulo_id > 0 ? semanaCapitulo : elemento.subcapituloN1_id > 0 ? semanaN1 : elemento.subcapituloN2_id > 0 ? semanaN2 : semanaC,
                        facturado = elemento.capitulo_id > 0 ? facturadoCapitulo : elemento.subcapituloN1_id > 0 ? facturadoN1 : elemento.subcapituloN2_id > 0 ? facturadoN2 : facturadoC,
                        xFacturar = elemento.capitulo_id > 0 ? xFacturarCapitulo : elemento.subcapituloN1_id > 0 ? xFacturarN1 : elemento.subcapituloN2_id > 0 ? xFacturarN2 : xFacturarC,
                        subcapituloN3_id = elemento.subcapituloN3_id,
                        subcapituloN3 = elemento.subcapituloN3,
                        subcapituloN2_id = elemento.subcapituloN2_id,
                        subcapituloN2 = elemento.subcapituloN2,
                        subcapituloN1_id = elemento.subcapituloN1_id,
                        subcapituloN1 = elemento.subcapituloN1,
                        capitulo_id = elemento.capitulo_id,
                        capitulo = elemento.capitulo
                    });
                }


                if (concentrado != null)
                {
                    resultado.Add(SUCCESS, true);
                    resultado.Add("items", concentrado);
                }
                else
                {
                    resultado.Add(SUCCESS, false);
                    resultado.Add("EMPTY", true);
                }
            }
            catch (Exception)
            {
                resultado.Add(SUCCESS, false);
                resultado.Add(ERROR, "Ocurrió un error interno al intentar obtener el concentrado de avances");
            }

            return resultado;
        }
        public Dictionary<string, object> getAvancesAutorizar(int capituloID)
        {
            var resultado = new Dictionary<string, object>();

            try
            {
                int usuarioId = vSesiones.sesionUsuarioDTO.id;
                var avances = _context.tblCO_Actividades_Avance.ToList().Where(x => x.capitulo_id == capituloID && x.estatus == true && x.periodoAvance == 2 && x.autorizado == false && x.capitulo.autorizante_id == usuarioId).Select(y => new AvancesAutorizarDTO
                {
                    avance_id = y.id,
                    capitulo_id = y.capitulo_id,
                    capitulo = y.capitulo.capitulo,
                    cc_id = y.capitulo.cc_id,
                    cc = y.capitulo.cc == null ? "" : y.capitulo.cc.cc,
                    fechaInicio = y.fechaInicio.ToShortDateString(),
                    fechaFin = y.fechaFin.ToShortDateString(),
                    nombreAutorizante = y.capitulo.usuario == null ? "" : y.capitulo.usuario.nombre + ' ' + y.capitulo.usuario.apellidoPaterno + ' ' + y.capitulo.usuario.apellidoMaterno
                }).ToList();

                if (avances.Count > 0)
                {
                    resultado.Add(SUCCESS, true);
                    resultado.Add("items", avances);
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
                resultado.Add(ERROR, "Ocurrió un error interno al intentar obtener el catálogo de avances a autorizar");
            }

            return resultado;
        }
        public Dictionary<string, object> getActividadAvanceDetalleAutorizar(int subcapitulosN1_id, int subcapitulosN2_id, int subcapitulosN3_id, int actividadAvance_id)
        {
            var resultado = new Dictionary<string, object>();

            try
            {
                var actividad = _context.tblCO_Actividades.ToList().Where(x => (x.estatus == true) &&
                    (subcapitulosN1_id == -1 ? subcapitulosN2_id == -1 ? subcapitulosN3_id == -1 ? false : x.subcapituloN3_id == subcapitulosN3_id : x.subcapituloN2_id == subcapitulosN2_id : x.subcapituloN1_id == subcapitulosN1_id)).Select(y => y.id).ToList();

                var actividades = _context.tblCO_Actividades.ToList().Where(x => (x.estatus == true) &&
                    (subcapitulosN1_id == -1 ? subcapitulosN2_id == -1 ? subcapitulosN3_id == -1 ? false : x.subcapituloN3_id == subcapitulosN3_id : x.subcapituloN2_id == subcapitulosN2_id : x.subcapituloN1_id == subcapitulosN1_id)).ToList();

                var avances = _context.tblCO_Actividades_Avance_Detalle.ToList().Where(x => actividad.Contains(x.actividad_id) && x.actividadAvance_id == actividadAvance_id).OrderByDescending(w => w.id).ToList();
                var facturado = _context.tblCO_Actividades_Facturado_Detalle.ToList().Where(x => actividad.Contains(x.actividad_id)).ToList();

                var reporte = actividades.Select(x => new ReporteAvancesDTO
                {
                    actividad_id = x.id,
                    actividad = x.actividad,
                    actividadPU = x.precioUnitario,
                    actividadCosto = x.costo,
                    importeContratado = x.importeContratado,
                    unidad = x.unidadesCostos.Where(y => y.actividad_id == x.id).Select(y => y.unidad.unidad).FirstOrDefault(),
                    cantidad = x.cantidad,
                    fechaInicio = x.fechaInicio.ToShortDateString(),
                    fechaFin = x.fechaFin.ToShortDateString(),
                    acumAnterior = avances.Where(y => y.actividad_id == x.id).Take(1).Select(w => w.acumuladoAnterior).Sum(),
                    acumActual = avances.ToList().Where(y => y.actividad_id == x.id).Take(1).Select(w => w.acumuladoActual).Sum(),
                    avancePeriodo = avances.ToList().Where(y => y.actividad_id == x.id).Take(1).Select(w => w.cantidadAvance).Sum(),
                    avancePorcentaje = avances.ToList().Where(y => y.actividad_id == x.id).Take(1).Select(w => w.avancePorcentaje).Sum(),
                    avanceAcumuladoPorcentaje = avances.ToList().Where(y => y.actividad_id == x.id).Take(1).Select(w => w.avanceAcumuladoPorcentaje).Sum(),
                    subcapituloN3_id = x.subcapituloN3_id,
                    subcapituloN2_id = x.subcapituloN2_id,
                    subcapituloN1_id = x.subcapituloN1_id,
                    importeAvanceAnt = avances.ToList().Where(y => y.actividad_id == x.id).Take(1).Select(w => w.importeAvanceAnt).Sum(),
                    importeAvancePeriodo = avances.ToList().Where(y => y.actividad_id == x.id).Take(1).Select(w => w.importeAvancePeriodo).Sum(),
                    importeAvanceAcumulado = avances.ToList().Where(y => y.actividad_id == x.id).Take(1).Select(w => w.importeAvanceAcumulado).Sum(),
                    volumenFacturado = facturado.Where(y => y.actividad_id == x.id).Select(w => w.volumen).Sum(),
                    volumenxFacturar = x.cantidad - facturado.Where(y => y.actividad_id == x.id).Select(w => w.volumen).Sum(),
                    importeFacturado = facturado.Where(y => y.actividad_id == x.id).Select(w => w.importe).Sum(),
                    importexFacturar = x.importeContratado - facturado.Where(y => y.actividad_id == x.id).Select(w => w.volumen).Sum()
                }).ToList();


                if (reporte.Count > 0)
                {
                    resultado.Add(SUCCESS, true);
                    resultado.Add("items", reporte);
                }
                else
                {
                    resultado.Add(SUCCESS, false);
                    resultado.Add("EMPTY", true);
                }
            }
            catch (Exception)
            {
                resultado.Add(SUCCESS, false);
                resultado.Add(ERROR, "Ocurrió un error interno al intentar obtener el catálogo de avances");
            }

            return resultado;
        }
        #endregion

        #region IMPORTAR ARCHIVO OPUS
        public Dictionary<string, object> guardarInfoOpus(string nombreObra, string nombreSinEspacios, List<HttpPostedFileBase> archivos, int? periodoFacturacion, int? cc_id, int? autorizande_id)
        {
            var resultado = new Dictionary<string, object>();
            bool seGuardoArchivo = false;
            bool seGuardoObra = false;
            bool actualizarArchivos = false;
            string Ruta = "";

            try
            {
                //SI YA EXISTE CARPETA DE PROYECTO SE ELIMINAN LOS ARCHIVOS Y SE REEMPLAZAN CON LOS NUEVOS, SI NO SE CREA LA CARPETA.
                //Directory.CreateDirectory(archivofs.getArchivo().getUrlDelServidor(1016).Replace("\\\\REPOSITORIO\\", "C:\\") + nombreObra); // LOCAL
                //if (Directory.Exists(archivofs.getArchivo().getUrlDelServidor(1016) + nombreObra)){

                //    System.IO.DirectoryInfo di = new DirectoryInfo(archivofs.getArchivo().getUrlDelServidor(1016) + nombreObra);

                //    foreach (FileInfo file in di.GetFiles())
                //    {
                //        file.Delete();
                //    }
                //    //foreach (DirectoryInfo dir in di.GetDirectories())
                //    //{
                //    //    dir.Delete(true);
                //    //}

                //    actualizarArchivos = true;
                //}
                //else
                //{
                //    Directory.CreateDirectory(archivofs.getArchivo().getUrlDelServidor(1016) + nombreObra);
                //    actualizarArchivos = false;
                //}

                Directory.CreateDirectory(archivofs.getArchivo().getUrlDelServidor(1016) + nombreObra);

                foreach (HttpPostedFileBase file in archivos)
                {
                    string extension = Path.GetExtension(file.FileName);
                    string FileName = "";

                    if (file.FileName.Split('.')[0].EndsWith("3"))
                    {
                        //ESTRUCTURA
                        FileName = nombreSinEspacios + estructura + extension;
                    }
                    else if (file.FileName.Split('.')[0].EndsWith("P"))
                    {
                        //PRECIOS
                        FileName = nombreSinEspacios + precios + extension;
                    }
                    else
                    {
                        FileName = file.FileName;
                    }

                    //Ruta = archivofs.getArchivo().getUrlDelServidor(1016) + nombreObra + @"\" + FileName; // LOCAL
                    Ruta = archivofs.getArchivo().getUrlDelServidor(1016) + nombreObra + @"\" + FileName;

                    if (SaveArchivo(file, Ruta))
                        seGuardoArchivo = true;
                    else
                    {
                        seGuardoArchivo = false;
                        break;
                    }
                }

                if (seGuardoArchivo)
                {
                    //Ruta = Ruta.Replace("\\\\REPOSITORIO\\", "C:\\"); // LOCAL
                    seGuardoObra = guardarObra(nombreSinEspacios, Ruta, nombreObra, periodoFacturacion, cc_id, autorizande_id);

                    resultado.Add(SUCCESS, seGuardoObra);
                    if (!seGuardoObra) resultado.Add(ERROR, _msgError);
                }
                else
                {
                    resultado.Add(SUCCESS, false);
                    resultado.Add(ERROR, _msgError);
                }

            }
            catch (Exception e)
            {
                resultado.Add(SUCCESS, false);
                resultado.Add(ERROR, "Ocurrio un error al leer los archivos");
            }

            return resultado;
        }
        public bool guardarObra(string fileName, string Ruta, string nombreObra, int? periodoFacturacion, int? cc_id, int? autorizande_id)
        {
            bool seGuardo = false;

            using (DbContextTransaction dbContextTransaction = _context.Database.BeginTransaction())
            {
                try
                {
                    List<OpusDTO> dtOpusEstructura = getInfoOpus(fileName + estructura, Ruta) ?? new List<OpusDTO>();
                    List<OpusDTO> dtOpusPrecio = getInfoOpus(fileName + precios, Ruta) ?? new List<OpusDTO>();
                    tblCO_Capitulos capitulo;
                    tblCO_Subcapitulos_Nivel1 subcapituloN1;
                    tblCO_Subcapitulos_Nivel2 subcapituloN2;
                    tblCO_Subcapitulos_Nivel3 subcapituloN3;
                    tblCO_Unidades unidad;
                    tblCO_Actividades actividad;
                    tblCO_Unidades_Actividad unidad_actividad;

                    int capitulo_id = 0;
                    int subcapituloN1_id = 0;
                    int subcapituloN2_id = 0;
                    int subcapituloN3_id = 0;
                    int unidad_id = 0;
                    int actividad_id = 0;

                    foreach (OpusDTO Opus in dtOpusEstructura)
                    {
                        if (Opus.nivel == 0 && Opus.signo == "+")
                        {
                            capitulo = new tblCO_Capitulos
                            {
                                //capitulo = Opus.descripcion,
                                capitulo = nombreObra,
                                fechaInicio = Opus.fechaInicio,
                                fechaFin = Opus.fechaTermino,
                                periodoFacturacion = periodoFacturacion,
                                cc_id = cc_id,
                                autorizante_id = autorizande_id,
                                estatus = true
                            };

                            capitulo_id = guardarCapituloID(capitulo);
                            _msgError = validaGuardarConceptos(capitulo_id, 0);
                            if (_msgError != "") break;
                        }
                        else if (Opus.nivel == 1 && (Opus.signo == "*" || Opus.signo == "+"))
                        {
                            subcapituloN1 = new tblCO_Subcapitulos_Nivel1
                            {
                                subcapitulo = Opus.descripcion,
                                fechaInicio = Opus.fechaInicio,
                                fechaFin = Opus.fechaTermino,
                                capitulo_id = capitulo_id,
                                estatus = true
                            };

                            subcapituloN1_id = guardarSubcapituloN1_ID(subcapituloN1);
                            _msgError = validaGuardarConceptos(subcapituloN1_id, 1);
                            if (_msgError != "") break;
                        }
                        else if (Opus.nivel == 2 && (Opus.signo == "*" || Opus.signo == "+"))
                        {
                            subcapituloN2 = new tblCO_Subcapitulos_Nivel2
                            {
                                subcapitulo = Opus.descripcion,
                                fechaInicio = Opus.fechaInicio,
                                fechaFin = Opus.fechaTermino,
                                subcapituloN1_id = subcapituloN1_id,
                                estatus = true
                            };

                            subcapituloN2_id = guardarSubcapituloN2_ID(subcapituloN2);
                            _msgError = validaGuardarConceptos(subcapituloN2_id, 2);
                            if (_msgError != "") break;
                        }
                        else if (Opus.nivel == 2 && Opus.signo == "-")
                        {
                            actividad = new tblCO_Actividades()
                            {
                                actividad = Opus.descripcion,
                                cantidad = Opus.cantidad,
                                fechaInicio = Opus.fechaInicio,
                                fechaFin = Opus.fechaTermino,
                                subcapituloN1_id = subcapituloN1_id,
                                costo = Opus.costo,
                                precioUnitario = dtOpusPrecio.Where(x => x.claveOPUS == Opus.claveOPUS).Select(x => x.precioUnitario).Sum(),
                                importeContratado = Opus.cantidad > 0 ? (Opus.cantidad * dtOpusPrecio.Where(x => x.claveOPUS == Opus.claveOPUS).Select(x => x.precioUnitario).Sum()) : 0m,
                                tipoPeriodoAvance = 2,
                                estatus = true
                            };
                            actividad_id = guardarActividad_ID(actividad);
                            _msgError = validaGuardarConceptos(actividad_id, 4);
                            if (_msgError != "") break;

                            if (existeUnidad(Opus.unidad))
                            {
                                unidad_actividad = new tblCO_Unidades_Actividad()
                                {
                                    actividad_id = actividad_id,
                                    unidad_id = getIDUnidad(Opus.unidad)
                                };

                                _msgError = validaGuardarConceptos(guardarUnidadActividadCosto(unidad_actividad), 6);
                                if (_msgError != "") break;
                            }
                            else
                            {
                                unidad = new tblCO_Unidades()
                                {
                                    unidad = Opus.unidad,
                                    estatus = true
                                };
                                unidad_id = guardarUnidad_ID(unidad);
                                _msgError = validaGuardarConceptos(unidad_id, 5);
                                if (_msgError != "") break;

                                unidad_actividad = new tblCO_Unidades_Actividad()
                                {
                                    actividad_id = actividad_id,
                                    unidad_id = unidad_id
                                };
                                _msgError = validaGuardarConceptos(guardarUnidadActividadCosto(unidad_actividad), 6);
                                if (_msgError != "") break;
                            }

                        }
                        else if (Opus.nivel == 3 && Opus.signo == "*")
                        {
                            subcapituloN3 = new tblCO_Subcapitulos_Nivel3
                            {
                                subcapitulo = Opus.descripcion,
                                fechaInicio = Opus.fechaInicio,
                                fechaFin = Opus.fechaTermino,
                                subcapituloN2_id = subcapituloN2_id,
                                estatus = true
                            };

                            subcapituloN3_id = guardarSubcapituloN3_ID(subcapituloN3);
                            _msgError = validaGuardarConceptos(subcapituloN3_id, 3);
                            if (_msgError != "") break;
                        }
                        else if (Opus.nivel == 3 && Opus.signo == "-")
                        {
                            actividad = new tblCO_Actividades()
                            {
                                actividad = Opus.descripcion,
                                cantidad = Opus.cantidad,
                                fechaInicio = Opus.fechaInicio,
                                fechaFin = Opus.fechaTermino,
                                subcapituloN2_id = subcapituloN2_id,
                                costo = Opus.costo,
                                precioUnitario = dtOpusPrecio.Where(x => x.claveOPUS == Opus.claveOPUS).Select(x => x.precioUnitario).Sum(),
                                importeContratado = Opus.cantidad > 0 ? (Opus.cantidad * dtOpusPrecio.Where(x => x.claveOPUS == Opus.claveOPUS).Select(x => x.precioUnitario).Sum()) : 0m,
                                tipoPeriodoAvance = 2,
                                estatus = true
                            };
                            actividad_id = guardarActividad_ID(actividad);
                            _msgError = validaGuardarConceptos(actividad_id, 4);
                            if (_msgError != "") break;

                            if (existeUnidad(Opus.unidad))
                            {
                                unidad_actividad = new tblCO_Unidades_Actividad()
                                {
                                    actividad_id = actividad_id,
                                    unidad_id = getIDUnidad(Opus.unidad)
                                };
                                _msgError = validaGuardarConceptos(guardarUnidadActividadCosto(unidad_actividad), 6);
                                if (_msgError != "") break;
                            }
                            else
                            {
                                unidad = new tblCO_Unidades()
                                {
                                    unidad = Opus.unidad,
                                    estatus = true
                                };
                                unidad_id = guardarUnidad_ID(unidad);
                                _msgError = validaGuardarConceptos(unidad_id, 5);
                                if (_msgError != "") break;

                                unidad_actividad = new tblCO_Unidades_Actividad()
                                {
                                    actividad_id = actividad_id,
                                    unidad_id = unidad_id
                                };
                                _msgError = validaGuardarConceptos(guardarUnidadActividadCosto(unidad_actividad), 6);
                                if (_msgError != "") break;
                            }

                        }
                        else if (Opus.nivel == 4 && Opus.signo == "-")
                        {
                            actividad = new tblCO_Actividades()
                            {
                                actividad = Opus.descripcion,
                                cantidad = Opus.cantidad,
                                fechaInicio = Opus.fechaInicio,
                                fechaFin = Opus.fechaTermino,
                                subcapituloN3_id = subcapituloN3_id,
                                costo = Opus.costo,
                                precioUnitario = dtOpusPrecio.Where(x => x.claveOPUS == Opus.claveOPUS).Select(x => x.precioUnitario).Sum(),
                                importeContratado = Opus.cantidad > 0 ? (Opus.cantidad * dtOpusPrecio.Where(x => x.claveOPUS == Opus.claveOPUS).Select(x => x.precioUnitario).Sum()) : 0m,
                                tipoPeriodoAvance = 2,
                                estatus = true
                            };
                            actividad_id = guardarActividad_ID(actividad);
                            _msgError = validaGuardarConceptos(actividad_id, 4);
                            if (_msgError != "") break;

                            if (existeUnidad(Opus.unidad))
                            {
                                unidad_actividad = new tblCO_Unidades_Actividad()
                                {
                                    actividad_id = actividad_id,
                                    unidad_id = getIDUnidad(Opus.unidad)
                                };
                                _msgError = validaGuardarConceptos(guardarUnidadActividadCosto(unidad_actividad), 6);
                                if (_msgError != "") break;
                            }
                            else
                            {
                                unidad = new tblCO_Unidades()
                                {
                                    unidad = Opus.unidad,
                                    estatus = true
                                };
                                unidad_id = guardarUnidad_ID(unidad);
                                _msgError = validaGuardarConceptos(unidad_id, 5);
                                if (_msgError != "") break;

                                unidad_actividad = new tblCO_Unidades_Actividad()
                                {
                                    actividad_id = actividad_id,
                                    unidad_id = unidad_id
                                };
                                _msgError = validaGuardarConceptos(guardarUnidadActividadCosto(unidad_actividad), 6);
                                if (_msgError != "") break;
                            }
                        }
                    }

                    if (_msgError == "") seGuardo = true;
                    else seGuardo = false;
                }
                catch (Exception e)
                {
                    seGuardo = false;
                    _msgError = "Ocurrio un error al guardar la información en base de datos.";
                }

                if (seGuardo)
                {
                    dbContextTransaction.Commit();
                }
                else
                {
                    dbContextTransaction.Rollback();
                }
                dbContextTransaction.Dispose();
            }
            return seGuardo;
        }
        public string validaGuardarConceptos(int concepto, int nivel)
        {
            string valido = "";

            switch (nivel)
            {
                case 0:
                    valido = concepto > 0 ? "" : "Ocurrio un error al agregar capitulo";
                    break;
                case 1:
                    valido = concepto > 0 ? "" : "Ocurrio un error al agregar subcapitulo N1";
                    break;
                case 2:
                    valido = concepto > 0 ? "" : "Ocurrio un error al agregar subcapitulo N2";
                    break;
                case 3:
                    valido = concepto > 0 ? "" : "Ocurrio un error al agregar subcapitulo N3";
                    break;
                case 4:
                    valido = concepto > 0 ? "" : "Ocurrio un error al agregar actividades";
                    break;
                case 5:
                    valido = concepto > 0 ? "" : "Ocurrio un error al agregar unidad";
                    break;
                case 6:
                    valido = concepto > 0 ? "" : "Ocurrio un error al ligar la unidad";
                    break;
                default:
                    break;
            }

            return valido;
        }
        public List<OpusDTO> getInfoOpus(string fileName, string Ruta)
        {
            var dtResultados = LeerArchivoOpus(fileName, Ruta) ?? new DataTable();
            var resultado = new List<OpusDTO>();

            try
            {
                if (dtResultados.Rows.Count > 0)
                {
                    if (fileName.Contains(estructura))
                    {
                        resultado = dtResultados.AsEnumerable().Select(s => new OpusDTO
                        {
                            descripcion = s.Field<string>("DESCRIPCIO"),
                            nivel = Int16.Parse(s.Field<string>("NIVEL")),
                            signo = s.Field<string>("SIGNO"),
                            cantidad = s.Field<decimal>("CANTIDAD"),
                            fechaInicio = s.Field<DateTime>("FINI"),
                            fechaTermino = s.Field<DateTime>("FTER"),
                            unidad = s.Field<string>("UNIDAD"),
                            costo = s.Field<decimal>("COSTO"),
                            tipoRenglon = s.Field<string>("TIPOREN"),
                            claveOPUS = s.Field<string>("NOMBRE")
                        }).ToList();
                    }
                    else
                    {
                        resultado = dtResultados.AsEnumerable().Select(s => new OpusDTO
                        {
                            claveOPUS = s.Field<string>("NOMBRE"),
                            precioUnitario = s.Field<decimal>("PUNIT")
                        }).ToList();
                    }
                }
            }
            catch (Exception e)
            {
                _msgError = "Ocurrió un error al consultar la informacion de archivos.";
            }

            return resultado;
        }
        public DataTable LeerArchivoOpus(string fileName, string Ruta)
        {
            DataTable dt = new DataTable();
            try
            {
                using (OleDbConnection con = new OleDbConnection("Provider=vfpoledb;Data Source=" + Ruta + ";Collating Sequence=machine;"))
                {
                    var sql = "select * from " + fileName + ".DBF";
                    OleDbCommand cmd = new OleDbCommand(sql, con);
                    con.Open();
                    OleDbDataAdapter da = new OleDbDataAdapter(cmd);
                    da.Fill(dt);
                }
            }
            catch (Exception e)
            {
                _msgError = "Ocurrio un error al leer los archivos guardados.";
            }

            return dt;
        }
        public bool SaveArchivo(HttpPostedFileBase archivo, string ruta)
        {
            try
            {
                byte[] data;
                using (Stream inputStream = archivo.InputStream)
                {
                    MemoryStream memoryStream = inputStream as MemoryStream;
                    if (memoryStream == null)
                    {
                        memoryStream = new MemoryStream();
                        inputStream.CopyTo(memoryStream);
                    }
                    data = memoryStream.ToArray();
                }
                //ruta = ruta.Replace("\\\\REPOSITORIO\\", "C:\\"); //LOCAL     

                File.WriteAllBytes(ruta, data);
            }
            catch (Exception e)
            {
                _msgError = "Ocurrio un error al guardar los archivos del proyecto";
            }

            return File.Exists(ruta);
        }
        #endregion

        #region INFORME SEMANAL
        public Dictionary<string, object> ObtenerDivisiones()
        {
            var resultado = new Dictionary<string, object>();

            try
            {
                int usuario_id = vSesiones.sesionUsuarioDTO.id;
                string usuario_cc = vSesiones.sesionUsuarioDTO.cc;

                var divisiones = _context.tblCO_Divisiones.Select(x => new Core.DTO.ControlObra.DivisionesDTO
                {
                    id = x.id,
                    division = x.division,
                    estatus = x.estatus,
                    ccValido = x.centrosCostos.Where(y => y.cc == usuario_cc).ToList().Count > 0 ? true : false,
                    usuario_cc = usuario_cc
                }).ToList();

                if (divisiones.Count > 0)
                {
                    resultado.Add(SUCCESS, true);
                    resultado.Add("items", divisiones);
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
                resultado.Add(ERROR, "Ocurrió un error interno al intentar obtener el catálogo de divisiones");
            }
            return resultado;
        }
        public Dictionary<string, object> obtenerDivisionCC()
        {
            var resultado = new Dictionary<string, object>();

            try
            {
                int usuario_id = vSesiones.sesionUsuarioDTO.id;
                string usuario_cc = vSesiones.sesionUsuarioDTO.cc;

                var divisionCC = _context.tblCO_Divisiones.Where(x => x.estatus == true && x.id == x.centrosCostos.Where(y => y.cc == usuario_cc).FirstOrDefault().division_id).Select(x => new Core.DTO.ControlObra.DivisionesDTO
                {
                    id = x.id,
                    division = x.division,
                    usuario_cc = usuario_cc,
                    descripcion_cc = _context.tblP_CC.Where(y => y.cc == usuario_cc).Select(y => y.descripcion).FirstOrDefault(),
                    estatus = x.estatus
                }).FirstOrDefault();

                if (divisionCC != null)
                {
                    resultado.Add(SUCCESS, true);
                    resultado.Add("divisionCC", divisionCC);
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
                resultado.Add(ERROR, "Ocurrió un error interno al intentar obtener el centro de costo del usuario");
            }
            return resultado;
        }
        public Dictionary<string, object> getInformesSemanal(int division_id)
        {
            var resultado = new Dictionary<string, object>();

            try
            {
                var plantillas = _context.tblCO_informeSemanal.Where(x => x.estatus == true && x.plantilla.division_id == division_id).Select(x => new informeSemanalDTO
                {
                    id = x.id,
                    cc = x.cc,
                    cc_desc = _context.tblP_CC.Where(y => y.cc == x.cc).Select(y => y.descripcion).FirstOrDefault(),
                    fecha_st = x.fecha.ToString()
                }).OrderBy(x => x.cc).ToList();

                if (plantillas.Count > 0)
                {
                    resultado.Add(SUCCESS, true);
                    resultado.Add("items", plantillas);
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
                resultado.Add(ERROR, "Ocurrió un error interno al intentar obtener el catálogo de plantillas");
            }

            return resultado;
        }
        public Dictionary<string, object> getInformeSemanal(int informe_id)
        {
            var resultado = new Dictionary<string, object>();

            try
            {


                var informe = _context.tblCO_informeSemanal_detalle.Where(x => x.informe.estatus == true && x.informe.id == informe_id)
                   .OrderBy(x => x.ordenDiapositiva).Select(x => new informeSemanal_detalleDTO
                   {
                       id = x.id,
                       informe_id = x.informe_id,
                       plantilla_id = x.informe.plantilla_id,
                       ordenDiapositiva = x.ordenDiapositiva,
                       tituloDiapositiva = x.tituloDiapositiva,
                       contenido = x.contenido,
                       pdf = x.pdf,
                   }).ToList();

                string periodo = _context.tblCO_informeSemanal_detalle.Where(x => x.informe.estatus == true && x.informe.id == informe_id).FirstOrDefault().informe.periodo;
                string cc = _context.tblCO_informeSemanal.Where(y => y.id == informe_id).Select(y => y.cc).First();
                string cc_desc = _context.tblP_CC.Where(x => x.cc == _context.tblCO_informeSemanal.Where(y => y.id == informe_id).Select(y => y.cc).FirstOrDefault()).Select(x => x.descripcion).FirstOrDefault().Trim();

                if (informe != null && informe.Count > 0)
                {
                    resultado.Add(SUCCESS, true);
                    resultado.Add("informe", informe);
                    resultado.Add("cc", cc);
                    resultado.Add("periodo", periodo);
                    resultado.Add("cc_desc", cc_desc);
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
                resultado.Add(ERROR, "Ocurrió un error interno al intentar obtener el informe");
            }

            return resultado;
        }
        public Dictionary<string, object> getInformeSemanalContenido(int informe_id)
        {
            var resultado = new Dictionary<string, object>();

            try
            {
                string cc_desc = _context.tblP_CC.Where(x => x.cc == _context.tblCO_informeSemanal.Where(y => y.id == informe_id).Select(y => y.cc).FirstOrDefault()).Select(x => x.descripcion).FirstOrDefault();
                string periodo = _context.tblCO_informeSemanal.Where(x => x.id == informe_id).Select(x => x.periodo).FirstOrDefault();

                var informe = _context.tblCO_informeSemanal_detalle.Where(x => x.informe_id == informe_id).OrderBy(x => x.ordenDiapositiva).Select(x => new informeSemanal_detalleDTO
                {
                    id = x.id,
                    ordenDiapositiva = x.ordenDiapositiva,
                    tituloDiapositiva = x.tituloDiapositiva,
                    contenido = x.contenido,
                    pdf = x.pdf
                }).ToList();

                if (informe != null)
                {
                    resultado.Add(SUCCESS, true);
                    resultado.Add("informe", informe);
                    resultado.Add("cc_desc", cc_desc);
                    resultado.Add("periodo", periodo);
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
                resultado.Add(ERROR, "Ocurrió un error interno al intentar obtener la plantilla");
            }

            return resultado;
        }
        public Dictionary<string, object> getPlantillaInformeDetalle(int plantilla_id)
        {
            var resultado = new Dictionary<string, object>();

            try
            {
                var plantilla = _context.tblCO_PlantillaInforme_detalle.Where(x => x.plantilla.estatus == true && x.plantilla_id == plantilla_id).OrderBy(x => x.ordenDiapositiva).Select(x => new PlantillaInforme_detalleDTO
                {
                    id = x.id,
                    plantilla_id = x.plantilla_id,
                    ordenDiapositiva = x.ordenDiapositiva,
                    tituloDiapositiva = x.tituloDiapositiva,
                    contenido = x.contenido
                }).ToList();

                if (plantilla != null)
                {
                    resultado.Add(SUCCESS, true);
                    resultado.Add("plantilla", plantilla);
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
                resultado.Add(ERROR, "Ocurrió un error interno al intentar obtener la plantilla");
            }

            return resultado;
        }
        public Dictionary<string, object> getUltimoInforme()
        {
            var resultado = new Dictionary<string, object>();

            try
            {
                var cveEmpleado = vSesiones.sesionUsuarioDTO.cveEmpleado;
                decimal emp = decimal.Parse(cveEmpleado);

                //var objEmpleado = string.Format("SELECT a.CC_Contable FROM DBA.sn_empleados a INNER JOIN si_puestos b ON a.puesto = b.puesto AND a.tipo_nomina = b.tipo_nomina WHERE a.clave_empleado = {0} AND a.estatus_empleado = 'A'", emp);

                var usuarioJson = _context.Select<dynamic>(new DapperDTO
                {
                    baseDatos = MainContextEnum.Construplan,
                    consulta = @"SELECT a.CC_Contable 
                                    FROM tblRH_EK_Empleados a 
                                    INNER JOIN tblRH_EK_Puestos b ON a.puesto = b.puesto AND a.tipo_nomina = b.FK_TipoNomina
                                    WHERE a.clave_empleado = @emp AND a.estatus_empleado = 'A'",
                    parametros = new { emp }
                });

                //var usuarioJson = ContextEnKontrolNomina.Where(objEmpleado);
                string usuario_cc = (string)usuarioJson[0].CC_Contable;//"155";PRUEBA LOCAL
                string cc_desc = _context.tblP_CC.Where(x => x.cc == usuario_cc).Select(x => x.descripcion).FirstOrDefault().Trim();

                var informe = _context.tblCO_informeSemanal_detalle.Where(x => x.informe.estatus == true && x.informe.cc == usuario_cc)
                   .OrderBy(x => x.ordenDiapositiva).Select(x => new informeSemanal_detalleDTO
                   {
                       id = x.id,
                       informe_id = x.informe_id,
                       plantilla_id = x.informe.plantilla_id,
                       ordenDiapositiva = x.ordenDiapositiva,
                       tituloDiapositiva = x.tituloDiapositiva,
                       contenido = x.contenido,
                       pdf = x.pdf,
                   }).ToList();

                string periodo = _context.tblCO_informeSemanal_detalle.Where(x => x.informe.estatus == true && x.informe.cc == usuario_cc).FirstOrDefault().informe.periodo;

                if (informe != null && informe.Count > 0)
                {
                    resultado.Add(SUCCESS, true);
                    resultado.Add("informe", informe);
                    resultado.Add("cc", usuario_cc);
                    resultado.Add("periodo", periodo);
                    resultado.Add("cc_desc", cc_desc);
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
                resultado.Add(ERROR, "Ocurrió un error interno al intentar obtener el informe");
            }

            return resultado;
        }
        public Dictionary<string, object> getPlantillaInformeDetalleCC()
        {
            var resultado = new Dictionary<string, object>();

            try
            {
                var cveEmpleado = vSesiones.sesionUsuarioDTO.cveEmpleado;
                decimal emp = decimal.Parse(cveEmpleado);

                //var objEmpleado = string.Format("SELECT a.CC_Contable FROM DBA.sn_empleados a INNER JOIN si_puestos b ON a.puesto = b.puesto AND a.tipo_nomina = b.tipo_nomina WHERE a.clave_empleado = {0} AND a.estatus_empleado = 'A'", emp);

                var usuarioJson = _context.Select<dynamic>(new DapperDTO
                {
                    baseDatos = MainContextEnum.Construplan,
                    consulta = @"SELECT a.CC_Contable 
                                FROM tblRH_EK_Empleados a 
                                INNER JOIN tblRH_EK_Puestos b ON a.puesto = b.puesto AND a.tipo_nomina = b.FK_TipoNomina
                                WHERE a.clave_empleado = @emp AND a.estatus_empleado = 'A'",
                    parametros = new { emp }
                });

                //var usuarioJson = ContextEnKontrolNomina.Where(objEmpleado);
                string usuario_cc = (string)usuarioJson[0].CC_Contable;//"155";PRUEBA LOCAL
                string cc_desc = _context.tblP_CC.Where(x => x.cc == usuario_cc).Select(x => x.descripcion).FirstOrDefault().Trim();

                DateTime today = DateTime.Today;
                DateTime startDate = DateTime.Today.AddDays(-(int)DateTime.Today.DayOfWeek + (int)DayOfWeek.Monday);

                // The (... + 7) % 7 ensures we end up with a value in the range [0, 6]
                int daysUntilTuesday = ((int)DayOfWeek.Sunday - (int)today.DayOfWeek + 7) % 7;
                DateTime endDate = today.AddDays(daysUntilTuesday);

                string periodo = startDate.ToShortDateString() + " - " + endDate.ToShortDateString();

                var plantilla = _context.tblCO_PlantillaInforme_detalle.Where(x => x.plantilla.estatus == true && x.plantilla.division_id == x.plantilla.division.centrosCostos.Where(y => y.cc == usuario_cc).Select(y => y.division_id).FirstOrDefault())
                    .OrderBy(x => x.ordenDiapositiva).Select(x => new PlantillaInforme_detalleDTO
                {
                    id = x.id,
                    plantilla_id = x.plantilla_id,
                    ordenDiapositiva = x.ordenDiapositiva,
                    tituloDiapositiva = x.tituloDiapositiva,
                    contenido = x.contenido
                }).ToList();

                if (plantilla != null && plantilla.Count > 0)
                {
                    resultado.Add(SUCCESS, true);
                    resultado.Add("plantilla", plantilla);
                    resultado.Add("cc", usuario_cc);
                    resultado.Add("cc_desc", cc_desc);
                    resultado.Add("periodo", periodo);
                }
                else
                {
                    resultado.Add(SUCCESS, false);
                    resultado.Add("EMPTY", true);
                    resultado.Add("cc", usuario_cc);
                    resultado.Add("cc_desc", cc_desc);
                }

            }
            catch (Exception e)
            {
                resultado.Add(SUCCESS, false);
                resultado.Add(ERROR, "Ocurrió un error interno al intentar obtener la plantilla");
            }

            return resultado;
        }
        public Dictionary<string, object> getPlantillaDivision(int divisionID)
        {
            var resultado = new Dictionary<string, object>();

            try
            {
                var plantilla = _context.tblCO_PlantillaInforme.Where(x => x.division_id == divisionID && x.estatus == true).Select(x => new PlantillaInformeDTO
                {
                    id = x.id,
                    division_id = x.division_id,
                    division_desc = x.division.division
                }).FirstOrDefault();

                var division = _context.tblCO_Divisiones.Where(x => x.id == divisionID).Select(x => new Core.DTO.ControlObra.DivisionesDTO
                {
                    id = x.id,
                    division = x.division
                }).FirstOrDefault();

                if (plantilla != null)
                {
                    resultado.Add(SUCCESS, true);
                    resultado.Add("plantilla", plantilla);
                    resultado.Add("division", division);
                }
                else
                {
                    resultado.Add(SUCCESS, false);
                    resultado.Add("division", division);
                }
            }
            catch (Exception e)
            {
                resultado.Add(SUCCESS, false);
                resultado.Add(ERROR, "Ocurrió un error al obtener la plantilla");
            }


            return resultado;
        }
        public Dictionary<string, object> guardarPlantilla(PlantillaInformeDTO plantilla, List<PlantillaInforme_detalleDTO> plantilla_detalle)
        {
            var resultado = new Dictionary<string, object>();
            bool seGuardo = false;
            string msg_error = "";
            int id = 0;

            using (DbContextTransaction dbContextTransaction = _context.Database.BeginTransaction())
            {
                try
                {
                    //Desactivar Plantilla Anterior
                    var plantillasAnteriores = _context.tblCO_PlantillaInforme.Where(x => x.division_id == plantilla.division_id && x.estatus == true).FirstOrDefault();
                    if (plantillasAnteriores != null)
                    {
                        plantillasAnteriores.estatus = false;
                        _context.Entry(plantillasAnteriores).State = System.Data.Entity.EntityState.Modified;
                        _context.SaveChanges();
                    }


                    int plantilla_id = 0;
                    var plantillaInforme = new tblCO_PlantillaInforme()
                    {
                        division_id = plantilla.division_id,
                        cantidadDiapositivas = plantilla.cantidadDiapositivas,
                        estatus = plantilla.estatus
                    };
                    _context.tblCO_PlantillaInforme.Add(plantillaInforme);
                    _context.SaveChanges();

                    plantilla_id = plantillaInforme.id;

                    if (plantilla_id > 0)
                    {
                        foreach (var detalle in plantilla_detalle)
                        {
                            var diapositivas = new tblCO_PlantillaInforme_detalle()
                            {
                                plantilla_id = plantilla_id,
                                ordenDiapositiva = detalle.ordenDiapositiva,
                                tituloDiapositiva = detalle.tituloDiapositiva
                            };

                            _context.tblCO_PlantillaInforme_detalle.Add(diapositivas);
                            _context.SaveChanges();

                            if (diapositivas.id > 0)
                            {
                                id = plantilla_id;
                                seGuardo = true;
                                continue;
                            }
                            else
                            {
                                seGuardo = false;
                                msg_error = "Ocurrió un error al guardar las diapositivas de la plantilla";
                                break;
                            }
                        }
                    }
                    else
                    {
                        seGuardo = false;
                        msg_error = "Ocurrió un error al guardar la plantilla";
                    }

                }
                catch (Exception e)
                {
                    seGuardo = false;
                    msg_error = "Ocurrió una error el intentar guardar";
                }

                if (seGuardo)
                {
                    dbContextTransaction.Commit();
                    resultado.Add(SUCCESS, true);
                    resultado.Add("plantilla", id);
                }
                else
                {
                    dbContextTransaction.Rollback();
                    resultado.Add(SUCCESS, false);
                    resultado.Add(ERROR, msg_error);
                }
            }
            return resultado;
        }
        public Dictionary<string, object> guardarPlantillaContenido(int plantilla_id, List<tblCO_PlantillaInforme_detalle> plantilla_contenido)
        {
            var resultado = new Dictionary<string, object>();
            bool seGuardo = false;
            string msg_error = "";

            using (DbContextTransaction dbContextTransaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var detalles = _context.tblCO_PlantillaInforme_detalle.Where(x => x.plantilla_id == plantilla_id).ToList();

                    _context.tblCO_PlantillaInforme_detalle.RemoveRange(detalles);
                    _context.SaveChanges();

                    _context.tblCO_PlantillaInforme_detalle.AddRange(plantilla_contenido);
                    _context.SaveChanges();

                    var plantilla = _context.tblCO_PlantillaInforme.Where(x => x.id == plantilla_id).FirstOrDefault();
                    plantilla.cantidadDiapositivas = plantilla_contenido.Count();

                    _context.Entry(plantilla).State = System.Data.Entity.EntityState.Modified;
                    _context.SaveChanges();


                    seGuardo = true;
                }
                catch (Exception e)
                {
                    seGuardo = false;
                    msg_error = "Ocurrió una error el intentar guardar el contenido de la plantilla";
                }

                if (seGuardo)
                {
                    dbContextTransaction.Commit();
                    resultado.Add(SUCCESS, true);
                }
                else
                {
                    dbContextTransaction.Rollback();
                    resultado.Add(SUCCESS, false);
                    resultado.Add(ERROR, msg_error);
                }
            }

            return resultado;
        }
        public Dictionary<string, object> guardarInforme(informeSemanalDTO informe, List<informeSemanal_detalleDTO> informe_detalle)
        {
            var resultado = new Dictionary<string, object>();
            bool seGuardo = false;
            string msg_error = "";
            int informe_id = 0;

            using (DbContextTransaction dbContextTransaction = _context.Database.BeginTransaction())
            {
                try
                {
                    //Desactivar Informe Anterior
                    var informeAnterior = _context.tblCO_informeSemanal.Where(x => x.plantilla_id == informe.plantilla_id && x.cc == informe.cc && x.estatus == true).FirstOrDefault();
                    if (informeAnterior != null)
                    {
                        informeAnterior.estatus = false;
                        _context.Entry(informeAnterior).State = System.Data.Entity.EntityState.Modified;
                        _context.SaveChanges();
                    }

                    var informeSemanal = new tblCO_informeSemanal()
                    {
                        plantilla_id = informe.plantilla_id,
                        cc = informe.cc,
                        fecha = DateTime.Now,
                        periodo = informe.periodo,
                        estatus = informe.estatus
                    };
                    _context.tblCO_informeSemanal.Add(informeSemanal);
                    _context.SaveChanges();

                    informe_id = informeSemanal.id;

                    if (informe_id > 0)
                    {
                        foreach (var detalle in informe_detalle)
                        {
                            var diapositivas = new tblCO_informeSemanal_detalle()
                            {
                                informe_id = informe_id,
                                ordenDiapositiva = detalle.ordenDiapositiva,
                                tituloDiapositiva = detalle.tituloDiapositiva,
                                contenido = detalle.contenido,
                                pdf = detalle.pdf
                            };

                            _context.tblCO_informeSemanal_detalle.Add(diapositivas);
                            _context.SaveChanges();

                            if (diapositivas.id > 0)
                            {
                                seGuardo = true;
                                continue;
                            }
                            else
                            {
                                seGuardo = false;
                                msg_error = "Ocurrió un error al guardar el contenido del informe";
                                break;
                            }
                        }
                    }
                    else
                    {
                        seGuardo = false;
                        msg_error = "Ocurrió un error al guardar el informe";
                    }

                }
                catch (Exception e)
                {
                    seGuardo = false;
                    msg_error = "Ocurrió una error el intentar guardar";
                }

                if (seGuardo)
                {
                    dbContextTransaction.Commit();
                    resultado.Add(SUCCESS, true);
                    resultado.Add("informe_id", informe_id);
                }
                else
                {
                    dbContextTransaction.Rollback();
                    resultado.Add(SUCCESS, false);
                    resultado.Add(ERROR, msg_error);
                }
            }
            return resultado;
        }
        #endregion





        #region SUB CONTRATISTA

        public List<ComboDTO> getProyecto()
        {
            var lstProyecto = new List<ComboDTO>();
            try
            {
                lstProyecto = _context.tblP_CC.Where(r => r.estatus).ToList().Select(y => new ComboDTO
                {
                    Value = y.cc.ToString(),
                    Text = y.cc.ToString() +" - "+ y.descripcion
                }).ToList();
            }
            catch (Exception ex)
            {
                throw;
            }
            return lstProyecto;
        }
        public List<ComboDTO> getSubContratistas()
        {
            var lstProyecto = new List<ComboDTO>();
            try
            {
                lstProyecto = _context.tblS_IncidentesEmpleadoContratistas.Where(r => r.esActivo).ToList().Select(y => new ComboDTO
                {
                    Value = y.id.ToString(),
                    Text = y.nombre + " " + y.apePaterno + " " + y.apeMaterno
                }).ToList();
            }
            catch (Exception ex)
            {
                throw;
            }
            return lstProyecto;
        }
        public SubContratistasDTO getTblSubContratista(SubContratistasDTO parametros)
        {
            return null;
        }
        public SubContratistasDTO addEditSubContratista(List<HttpPostedFileBase> Archivo, List<SubContratistasDTO> parametros)
        {
            string urlArchivo = RutaControlObra;
            string FechaActual = Convert.ToString(parametros.Select(y => y.idSubContratista).FirstOrDefault().ToString() + "CO" + DateTime.Now.Year + DateTime.Now.Month + DateTime.Now.Day + DateTime.Now.Hour + DateTime.Now.Minute);
            urlArchivo = Path.Combine(urlArchivo, FechaActual);
            if (Archivo != null)
            {
                foreach (var Arch in Archivo)
                {
                    foreach (var param in parametros)
                    {
                        if (Arch.FileName == param.Archivo)
                        {
                            param.File = Arch;
                        }
                    }
                }

            }

            var listaRutaArchivos = new List<Tuple<HttpPostedFileBase, string>>();
            tblCO_ADP_EvalSubContratista objAdd = new tblCO_ADP_EvalSubContratista();

            int id = parametros.Select(y => y.idEvaluacion).FirstOrDefault();
            objAdd = _context.tblCO_ADP_EvalSubContratista.Where(r => r.id == id).FirstOrDefault();
            if (objAdd == null)
            {
                objAdd = new tblCO_ADP_EvalSubContratista();
                objAdd.idAreaCuenta = parametros.Select(y => y.AreaCuenta).FirstOrDefault();
                objAdd.idSubContratista = parametros.Select(y => y.idSubContratista).FirstOrDefault();
                objAdd.Calificacion = parametros.Select(y => y.Calificacion).FirstOrDefault();

                _context.tblCO_ADP_EvalSubContratista.Add(objAdd);
                _context.SaveChanges();


                foreach (var item in parametros)
                {
                    tblCO_ADP_EvalSubContratistaDet objAddDet = new tblCO_ADP_EvalSubContratistaDet();
                    objAddDet = _context.tblCO_ADP_EvalSubContratistaDet.Where(r => r.id == item.id).FirstOrDefault();
                    if (objAddDet == null)
                    {
                        objAddDet = new tblCO_ADP_EvalSubContratistaDet();
                        objAddDet.idEvaluacion = objAdd.id;
                        objAddDet.fechaDocumento = DateTime.Now;
                        objAddDet.idRow = item.idRow;
                        objAddDet.tipoEvaluacion = item.tipoEvaluacion;
                        if (item.Archivo != null)
                        {
                            objAddDet.rutaArchivo = Path.Combine(urlArchivo, item.Archivo);
                        }
                        _context.tblCO_ADP_EvalSubContratistaDet.Add(objAddDet);
                        _context.SaveChanges();

                        if (item.File != null)
                        {
                            listaRutaArchivos.Add(Tuple.Create(item.File, Path.Combine(urlArchivo, item.Archivo)));

                        }
                    }
                }

            }
            else
            {
                objAdd.Calificacion = parametros.Select(y => y.Calificacion).FirstOrDefault();
                _context.SaveChanges();
                foreach (var item in parametros)
                {
                    tblCO_ADP_EvalSubContratistaDet objAddDet = new tblCO_ADP_EvalSubContratistaDet();
                    objAddDet = _context.tblCO_ADP_EvalSubContratistaDet.Where(r => r.idRow == item.idRow && r.idEvaluacion == objAdd.id).FirstOrDefault();
                    if (objAddDet != null)
                    {
                        objAddDet.fechaDocumento = DateTime.Now;
                        objAddDet.tipoEvaluacion = item.tipoEvaluacion;
                        if (item.Archivo != null)
                        {
                            objAddDet.rutaArchivo = Path.Combine(urlArchivo, item.Archivo);
                        }
                        _context.SaveChanges();

                        if (item.File != null)
                        {
                            listaRutaArchivos.Add(Tuple.Create(item.File, Path.Combine(urlArchivo, item.Archivo)));

                        }
                    }
                }

            }

            foreach (var arch in listaRutaArchivos)
            {
                if (GlobalUtils.SaveHTTPPostedFile(arch.Item1, arch.Item2) == false)
                {
                    resultado.Clear();
                    resultado.Add(SUCCESS, false);
                    resultado.Add(MESSAGE, "Ocurrió un error al guardar los archivos en el servidor.");
                }
            }

            return null;
        }
        public List<SubContratistasDTO> CargarArchivosXSubcontratista(SubContratistasDTO parametros)
        {
            List<SubContratistasDTO> lstDetalle = new List<SubContratistasDTO>();
            try
            {
                var objSubContratista = _context.tblCO_ADP_EvalSubContratista.Where(r => r.idSubContratista == parametros.idSubContratista).FirstOrDefault();
                if (objSubContratista != null)
                {
                    var lstDatos = _context.tblCO_ADP_EvalSubContratistaDet.Where(r => r.tipoEvaluacion == parametros.tipoEvaluacion).ToList();
                    if (lstDatos.Count() != 0)
                    {
                        lstDetalle = lstDatos.Select(y => new SubContratistasDTO
                        {
                            id = y.id,
                            idRow = y.idRow,
                            tipoEvaluacion = y.tipoEvaluacion,
                            idEvaluacion = y.idEvaluacion,
                            rutaArchivo = y.rutaArchivo == null ? "Ningún archivo seleccionado" : y.rutaArchivo.Split('\\')[5],
                            Calificacion = _context.tblCO_ADP_EvalSubContratista.Where(n => n.id == y.idEvaluacion).Select(P => P.Calificacion).FirstOrDefault(),
                            idSubContratista = _context.tblCO_ADP_EvalSubContratista.Where(n => n.id == y.idEvaluacion).Select(P => P.idSubContratista).FirstOrDefault(),
                        }).ToList();
                    }
                }

            }
            catch (Exception ex)
            {
                throw;
            }
            return lstDetalle;
        }
        public List<SubContratistasDTO> ObtenerTblAutorizacion(SubContratistasDTO parametros)
        {
            List<SubContratistasDTO> lstDatos = new List<SubContratistasDTO>();
            try
            {
                lstDatos = _context.tblCO_ADP_EvalSubContratista.Where(r => r.idSubContratista == parametros.idSubContratista).ToList().Select(y => new SubContratistasDTO
                {
                    id = y.id,
                    Calificacion = y.Calificacion,
                    fechaAutorizacion = y.fechaAutorizacion,
                    tipoEvaluacionDesc = obtenerTipoEvaluacion(_context.tblCO_ADP_EvalSubContratistaDet.Where(r => r.idEvaluacion == y.id).Select(n => n.tipoEvaluacion).FirstOrDefault())
                }).ToList();
            }
            catch (Exception)
            {
                throw;
            }
            return lstDatos;
        }
        public string obtenerTipoEvaluacion(int tipoEvaluacion)
        {
            string Desc = "";
            switch (tipoEvaluacion)
            {
                case 1:
                    Desc = "Calidad ";
                    break;
                case 2:
                    Desc = "Planeacion/Programa";
                    break;
                case 3:
                    Desc = "Facturacion";
                    break;
                case 4:
                    Desc = "Seguridad";
                    break;
                case 5:
                    Desc = "Ambiental";
                    break;
                case 6:
                    Desc = "Efectivo";
                    break;
                case 7:
                    Desc = "Fuerza de trabajo y Atencion al cliente";
                    break;
            }
            return Desc;
        }


        public List<DivicionesMenuDTO> obtenerDiviciones()
        {
            List<DivicionesMenuDTO> lstDatos = new List<DivicionesMenuDTO>();
            List<DivicionesMenuDTO> lstDatos2 = new List<DivicionesMenuDTO>();
            try
            {
                lstDatos = _context.tblCO_ADP_EvaluacionDiv.Where(r => r.esActivo && r.SubContratista == false).ToList().Select(y => new DivicionesMenuDTO
                {
                    id = y.id,
                    idbutton = y.idbutton,
                    idsection = y.idsection,
                    toltips = y.toltips,
                    descripcion = y.descripcion,
                    esActivo = y.esActivo,
                    orden = y.orden
                }).ToList();
                lstDatos2 = lstDatos.OrderBy(n => n.orden).ToList();
            }
            catch (Exception ex)
            {
                throw;
            }
            return lstDatos2;
        }
        public DivicionesMenuDTO addEditDiviciones(DivicionesMenuDTO parametros)
        {
            DivicionesMenuDTO objDatos = new DivicionesMenuDTO();
            try
            {
                var obj = _context.tblCO_ADP_EvaluacionDiv.Where(y => y.id == parametros.id).FirstOrDefault();
                if (obj == null)
                {
                    int id = 0;
                    int orden = 0;
                    var objid = (from i in _context.tblCO_ADP_EvaluacionDiv orderby i.id descending select i).FirstOrDefault();
                    if (objid == null)
                    {
                        id = 1;
                        orden = 1;
                    }
                    else
                    {
                        id = 1 + _context.tblCO_ADP_EvaluacionDiv.OrderByDescending(n => n.id).FirstOrDefault().id;
                        orden = 1 + _context.tblCO_ADP_EvaluacionDiv.OrderByDescending(n => n.orden).FirstOrDefault().id;
                    }
                    obj = new tblCO_ADP_EvaluacionDiv();
                    obj.esActivo = true;
                    obj.descripcion = parametros.descripcion;
                    obj.toltips = parametros.toltips;
                    obj.idbutton = "btnEvaluacion" + id;
                    obj.idsection = "sectionDiv" + id;
                    obj.orden = orden;
                    _context.tblCO_ADP_EvaluacionDiv.Add(obj);
                    _context.SaveChanges();
                    var objUltimoid = (from i in _context.tblCO_ADP_EvaluacionDiv orderby i.id descending select i).FirstOrDefault();

                    if (parametros.lstRequerimientos != null)
                    {
                        foreach (var item in parametros.lstRequerimientos)
                        {
                            tblCO_ADP_EvaluacionReq objReq = new tblCO_ADP_EvaluacionReq();
                            int id2 = 0;
                            var objid2 = (from i in _context.tblCO_ADP_EvaluacionReq orderby i.id descending select i).FirstOrDefault();
                            if (objid2 == null)
                            {
                                id2 = 1;
                            }
                            else
                            {
                                id2 = 1 + _context.tblCO_ADP_EvaluacionReq.OrderByDescending(n => n.id).FirstOrDefault().id;
                            }
                            objReq.idDiv = objUltimoid.id;
                            objReq.texto = item.texto;
                            objReq.inputFile = "inputFile" + id2;
                            objReq.lblInput = "lblInput" + id2;
                            objReq.txtAComentario = "txtArea" + id2;
                            objReq.txtPlaneacion = "txtPlaneacion" + id2;
                            objReq.txtResponsable = "txtResponsable" + id2;
                            objReq.txtFechaCompromiso = "txtFechaCompromiso" + id2;
                            _context.tblCO_ADP_EvaluacionReq.Add(objReq);
                            _context.SaveChanges();
                        }
                    }

                }
                else
                {
                    obj.descripcion = parametros.descripcion;
                    obj.toltips = parametros.toltips;
                    _context.SaveChanges();
                    var objid = (from i in _context.tblCO_ADP_EvaluacionDiv orderby i.id descending select i).FirstOrDefault();
                    if (objid != null)
                    {
                        var lstReq = _context.tblCO_ADP_EvaluacionReq.Where(r => r.idDiv == objid.id).ToList();
                        if (lstReq.Count() != 0)
                        {
                            _context.tblCO_ADP_EvaluacionReq.RemoveRange(lstReq);
                            _context.SaveChanges();
                        }
                    }
                    if (parametros.lstRequerimientos != null)
                    {
                        foreach (var item in parametros.lstRequerimientos)
                        {
                            tblCO_ADP_EvaluacionReq objReq = new tblCO_ADP_EvaluacionReq();
                            int id2 = 0;
                            var objid2 = (from i in _context.tblCO_ADP_EvaluacionReq orderby i.id descending select i).FirstOrDefault();
                            if (objid2 == null)
                            {
                                id2 = 1;
                            }
                            else
                            {
                                id2 = 1 + _context.tblCO_ADP_EvaluacionReq.OrderByDescending(n => n.id).FirstOrDefault().id;
                            }
                            objReq.idDiv = parametros.id;
                            objReq.texto = item.texto;
                            objReq.inputFile = "inputFile" + id2;
                            objReq.lblInput = "lblInput" + id2;
                            objReq.txtAComentario = "txtArea" + id2;
                            objReq.txtPlaneacion = "txtPlaneacion" + id2;
                            objReq.txtResponsable = "txtResponsable" + id2;
                            objReq.txtFechaCompromiso = "txtFechaCompromiso" + id2;
                            _context.tblCO_ADP_EvaluacionReq.Add(objReq);
                            _context.SaveChanges();
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return objDatos;
        }
        public DivicionesMenuDTO eliminarDiviciones(int id)
        {
            DivicionesMenuDTO objDatos = new DivicionesMenuDTO();
            try
            {
                var obj = _context.tblCO_ADP_EvaluacionDiv.Where(y => y.id == id).FirstOrDefault();
                if (obj != null)
                {
                    _context.tblCO_ADP_EvaluacionDiv.Remove(obj);
                    _context.SaveChanges();
                    var lstDatos = _context.tblCO_ADP_EvaluacionReq.Where(r => r.idDiv == id).ToList();
                    if (lstDatos.Count() != 0)
                    {
                        _context.tblCO_ADP_EvaluacionReq.RemoveRange(lstDatos);
                        _context.SaveChanges();
                    }

                    objDatos.estatus = 1;
                    objDatos.mensaje = "Se ha eliminado con exito!";
                }
                else
                {
                    objDatos.estatus = 2;
                    objDatos.mensaje = "No se ha podido eliminar!";
                }

            }
            catch (Exception ex)
            {
                throw;
            }
            return objDatos;
        }
        public List<RequerimientosDTO> obtenerRequerimientos(int idDiv)
        {
            List<RequerimientosDTO> objDatos = new List<RequerimientosDTO>();
            try
            {
                objDatos = _context.tblCO_ADP_EvaluacionReq.Where(r => r.idDiv == idDiv).ToList().Select(y => new RequerimientosDTO
                {
                    id = y.id,
                    idDiv = y.idDiv,
                    texto = y.texto,
                    inputFile = y.inputFile,
                    lblInput = y.lblInput,
                    tipoFile = y.tipoFile,
                    txtAComentario = y.txtAComentario,
                    txtPlaneacion = y.txtPlaneacion,
                    txtResponsable = y.txtResponsable,
                    txtFechaCompromiso = y.txtFechaCompromiso,
                }).ToList();
            }
            catch (Exception ex)
            {
                throw;
            }
            return objDatos;
        }
        public List<DivicionesMenuDTO> obtenerDivicionesEvaluador()
        {
            List<DivicionesMenuDTO> lstDatos = new List<DivicionesMenuDTO>();
            List<DivicionesMenuDTO> lstDatos2 = new List<DivicionesMenuDTO>();
            try
            {
                lstDatos = _context.tblCO_ADP_EvaluacionDiv.Where(r => r.esActivo).ToList().Select(y => new DivicionesMenuDTO
                {
                    id = y.id,
                    idbutton = y.idbutton,
                    idsection = y.idsection,
                    toltips = y.toltips,
                    descripcion = y.descripcion,
                    esActivo = y.esActivo,
                    orden = y.orden
                }).ToList();
                lstDatos2 = lstDatos.OrderBy(n => n.orden).ToList();
            }
            catch (Exception ex)
            {
                throw;
            }
            return lstDatos2;
        }
        public  byte[] DescargarArchivos(long idDet)
        {
            var resultado = new Dictionary<string, object>();
            Stream fileStream;
            try
            {
                string pathExamen = _context.tblCO_ADP_EvalSubContratistaDet.Where(x => x.id == idDet).FirstOrDefault().rutaArchivo;
                fileStream = GlobalUtils.GetFileAsStream(pathExamen);

            }
            catch (Exception e)
            {
                fileStream = null;
            }

            //resultado.Add("nombreDescarga", version.nombre);
            //resultado.Add(SUCCESS, true);
            return ReadFully(fileStream);
        }
        public string getFileName(long idDet)
        {
            string fileName = "";
            try
            {
                string pathExamen = _context.tblCO_ADP_EvalSubContratistaDet.Where(x => x.id == idDet).FirstOrDefault().rutaArchivo;
                fileName = pathExamen.Split('\\')[5];
            }
            catch (Exception e)
            {
                fileName = "";
            }

            return fileName;
        }
        public static byte[] ReadFully(Stream input)
        {
            MemoryStream ms = new MemoryStream();
            if (input != null)
            {
                byte[] buffer = new byte[16 * 1024];
                int read;
                while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
                {
                    ms.Write(buffer, 0, read);
                }
            }
            return ms.ToArray();
        }

        #endregion

    }
}

