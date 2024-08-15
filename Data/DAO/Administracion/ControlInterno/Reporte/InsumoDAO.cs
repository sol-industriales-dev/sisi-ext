using Core.DAO.Administracion.ControlInterno.Almacen;
using Core.DTO;
using Core.DTO.Administracion;
using Core.DTO.Administracion.ControlInterno;
using Core.DTO.Principal.Generales;
using Core.DTO.Utils.Data;
using Core.Entity.Administrativo.ControlInterno.Almacen;
using Core.Entity.StarSoft.Almacen;
using Core.Entity.StarSoft.Requisiciones;
using Core.Enum.Multiempresa;
using Data.EntityFramework.Context;
using Data.EntityFramework.Generic;
using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.DAO.Administracion.ControlInterno.Reporte
{
    public class InsumoDAO : GenericDAO<tblAlm_MergeInsumos>, IinsumosDAO
    {
        public List<insumosDTO> getListaInsumo(int insumoID, int anio)
        {
            List<insumosDTO> objReturn = new List<insumosDTO>();


            string insumoConstruplan = "";
            string insumoArrendadora = "";
            int insumo = Convert.ToInt32(insumoID);

            var listaInsumosSigoplan = _context.tblAlm_MergeInsumos.Where(c => c.insumoC == insumo || c.insumoA == insumo);

            if (listaInsumosSigoplan.Count() > 0)
            {
                //var listaInsumosConstruplan = listaInsumosSigoplan.Where(x => x.insumoC == insumo).Select(x => x.insumoC).ToList();
                //    var listaInsumosArrendadora = listaInsumosSigoplan.Where(x => x.insumoA == insumo).Select(x => x.insumoA).ToList();

                insumoConstruplan = listaInsumosSigoplan.FirstOrDefault(x => x.insumoC == insumo).insumoC.ToString();
                insumoArrendadora = listaInsumosSigoplan.FirstOrDefault(x => x.insumoC == insumo).insumoA.ToString();//listaInsumosConstruplan.FirstOrDefault().ToString();
                // insumoArrendadora = listaInsumosArrendadora.FirstOrDefault().ToString();

            }
            else
            {
                insumoConstruplan = insumoID.ToString();
                insumoArrendadora = insumoID.ToString();
            }

            string ConsultaDataInsumo = "SELECT  insumo, descripcion FROM insumos WHERE insumo =";


            var dataInsumoConstruplan = (List<detInsumoDTO>)_contextEnkontrol.Where(ConsultaDataInsumo + insumoConstruplan).ToObject<List<detInsumoDTO>>();
            var dataInsumoArrendadora = (List<detInsumoDTO>)ContextArrendadora.Where(ConsultaDataInsumo + insumoArrendadora).ToObject<List<detInsumoDTO>>();


            string ConsultaInsumos = @"SELECT almacen,
                                                            cc,
                                                            ano,
                                                            COALESCE(existencia_ent_ini, 0) as 'existencia_ent_ini',
                                                            COALESCE(existencia_sal_ini, 0) as 'existencia_sal_ini',
                                                            existencia_ent_ene,
                                                            existencia_sal_ene,
                                                            existencia_ent_feb,
                                                            existencia_sal_feb,
                                                            existencia_ent_mar,
                                                            existencia_sal_mar,
                                                            existencia_ent_abr,
                                                            existencia_sal_abr,
                                                            existencia_ent_may,
                                                            existencia_sal_may,
                                                            existencia_ent_jun,
                                                            existencia_sal_jun,
                                                            existencia_ent_jul,
                                                            existencia_sal_jul,
                                                            existencia_ent_ago,
                                                            existencia_sal_ago,
                                                            existencia_ent_sep,
                                                            existencia_sal_sep,
                                                            existencia_ent_oct,
                                                            existencia_sal_oct,
                                                            existencia_ent_nov,
                                                            existencia_sal_nov,
                                                            existencia_ent_dic,
                                                            existencia_sal_dic, 
                                                            insumo
                                                            FROM si_acumula_almacen WHERE  ano ='" + DateTime.Now.Year + "' AND insumo=";

            var AcumuladoConstruplan = (List<existenciaInsumosDTO>)_contextEnkontrol.Where(ConsultaInsumos + insumoConstruplan).ToObject<List<existenciaInsumosDTO>>();
            var AcumuladoArrendadora = (List<existenciaInsumosDTO>)ContextArrendadora.Where(ConsultaInsumos + insumoArrendadora).ToObject<List<existenciaInsumosDTO>>();


            decimal totalArrendadora = 0;
            decimal totalConstruplan = 0;

            foreach (var a in AcumuladoConstruplan)
            {
                insumosDTO objData = new insumosDTO();
                decimal cantidadConstruplan = 0;

                decimal sumas = a.existencia_ent_abr + a.existencia_ent_ago + a.existencia_ent_dic +
                                      a.existencia_ent_ene + a.existencia_ent_feb + a.existencia_ent_ini +
                                      a.existencia_ent_jul + a.existencia_ent_jun + a.existencia_ent_mar +
                                      a.existencia_ent_may + a.existencia_ent_nov + a.existencia_ent_oct +
                                      a.existencia_ent_sep;

                decimal resultado = sumas - a.existencia_sal_abr - a.existencia_sal_ago - a.existencia_sal_dic - a.existencia_sal_ene - a.existencia_sal_feb - a.existencia_sal_jul - a.existencia_sal_jun - a.existencia_sal_mar - a.existencia_sal_may
                                 - a.existencia_sal_nov - a.existencia_sal_oct - a.existencia_sal_sep;
                cantidadConstruplan = resultado;
                objData.almacen = a.almacen;
                objData.numInsumoConstruplan = insumo;
                objData.desInsumoConstruplan = dataInsumoConstruplan.FirstOrDefault() != null ? dataInsumoConstruplan.FirstOrDefault().descripcion : "";
                objData.cantidadInsumos = cantidadConstruplan;
                objData.locacion = 1;
                objReturn.Add(objData);

            }

            foreach (var a in AcumuladoArrendadora)
            {
                insumosDTO objData = new insumosDTO();
                decimal cantidadArrendadora = 0;
                decimal sumas = a.existencia_ent_abr + a.existencia_ent_ago + a.existencia_ent_dic +
                                      a.existencia_ent_ene + a.existencia_ent_feb + a.existencia_ent_ini +
                                      a.existencia_ent_jul + a.existencia_ent_jun + a.existencia_ent_mar +
                                      a.existencia_ent_may + a.existencia_ent_nov + a.existencia_ent_oct +
                                      a.existencia_ent_sep;

                decimal resultado = sumas - a.existencia_sal_abr - a.existencia_sal_ago - a.existencia_sal_dic - a.existencia_sal_ene - a.existencia_sal_feb - a.existencia_sal_jul - a.existencia_sal_jun - a.existencia_sal_mar - a.existencia_sal_may
                                 - a.existencia_sal_nov - a.existencia_sal_oct - a.existencia_sal_sep;
                objData.almacen = a.almacen;
                cantidadArrendadora = resultado;
                totalArrendadora += resultado;
                objData.numInsumoArrendadora = dataInsumoArrendadora.FirstOrDefault() != null ? dataInsumoArrendadora.FirstOrDefault().insumo : 0;
                objData.desInsumoConstruplan = dataInsumoConstruplan.FirstOrDefault() != null ? dataInsumoConstruplan.FirstOrDefault().descripcion : "";
                objData.cantidadInsumos = cantidadArrendadora;
                objData.locacion = 2;
                objReturn.Add(objData);
            }

            return objReturn;

        }


        public List<insumosDTO> getInsumo(int insumo, int anio, int tipo)
        {
            List<insumosDTO> objReturn = new List<insumosDTO>();
            string insumoIDConstruplan = "";
            string insumoIDArrendadora = "";
            List<tblAlm_MergeInsumos> listaInsumosSigoplan = new List<tblAlm_MergeInsumos>();
            switch (tipo)
            {
                case 1:
                    {
                        listaInsumosSigoplan = _context.tblAlm_MergeInsumos.Where(c => c.insumoC == insumo).ToList();


                        if (listaInsumosSigoplan.Count() > 0)
                        {
                            insumoIDConstruplan = listaInsumosSigoplan.FirstOrDefault().insumoC.ToString();
                            insumoIDArrendadora = listaInsumosSigoplan.FirstOrDefault().insumoA.ToString();


                        }
                        else
                        {
                            listaInsumosSigoplan.Add(new tblAlm_MergeInsumos
                            {
                                id = 0,
                                estatus = true,
                                insumoC = insumo,
                                insumoA = 0,

                            });
                            insumoIDConstruplan = insumo.ToString();
                            insumoIDArrendadora = "0";
                        }
                        break;

                    }
                case 2:
                    {
                        listaInsumosSigoplan = _context.tblAlm_MergeInsumos.Where(c => c.insumoA == insumo).ToList();


                        if (listaInsumosSigoplan.Count() > 0)
                        {
                            insumoIDConstruplan = listaInsumosSigoplan.FirstOrDefault().insumoC.ToString();
                            insumoIDArrendadora = listaInsumosSigoplan.FirstOrDefault().insumoA.ToString();

                        }
                        else
                        {
                            listaInsumosSigoplan.Add(new tblAlm_MergeInsumos
                            {
                                id = 0,
                                estatus = true,
                                insumoC = 0,
                                insumoA = insumo,

                            });
                            insumoIDConstruplan = "0";
                            insumoIDArrendadora = insumo.ToString();

                        }
                        break;
                    }
                default:
                    return null;
            }

            if (listaInsumosSigoplan.Count() > 0)
            {

                string ConsultaDataInsumo = "SELECT  insumo, descripcion FROM insumos WHERE insumo =";

                var dataInsumoConstruplan = (List<detInsumoDTO>)_contextEnkontrol.Where(ConsultaDataInsumo + insumoIDConstruplan).ToObject<List<detInsumoDTO>>();
                var dataInsumoArrendadora = (List<detInsumoDTO>)ContextArrendadora.Where(ConsultaDataInsumo + insumoIDArrendadora).ToObject<List<detInsumoDTO>>();

                string ConsultaInsumos = @"SELECT almacen,
                                                            cc,
                                                            ano,
                                                            COALESCE(existencia_ent_ini, 0) as 'existencia_ent_ini',
                                                            COALESCE(existencia_sal_ini, 0) as 'existencia_sal_ini',
                                                            existencia_ent_ene,
                                                            existencia_sal_ene,
                                                            existencia_ent_feb,
                                                            existencia_sal_feb,
                                                            existencia_ent_mar,
                                                            existencia_sal_mar,
                                                            existencia_ent_abr,
                                                            existencia_sal_abr,
                                                            existencia_ent_may,
                                                            existencia_sal_may,
                                                            existencia_ent_jun,
                                                            existencia_sal_jun,
                                                            existencia_ent_jul,
                                                            existencia_sal_jul,
                                                            existencia_ent_ago,
                                                            existencia_sal_ago,
                                                            existencia_ent_sep,
                                                            existencia_sal_sep,
                                                            existencia_ent_oct,
                                                            existencia_sal_oct,
                                                            existencia_ent_nov,
                                                            existencia_sal_nov,
                                                            existencia_ent_dic,
                                                            existencia_sal_dic, 
                                                            insumo
                                                            FROM si_acumula_almacen WHERE ano = '" + DateTime.Now.Year + "'AND  insumo=";


                var AcumuladoConstruplan = (List<existenciaInsumosDTO>)_contextEnkontrol.Where(ConsultaInsumos + insumoIDConstruplan).ToObject<List<existenciaInsumosDTO>>();
                var AcumuladoArrendadora = (List<existenciaInsumosDTO>)ContextArrendadora.Where(ConsultaInsumos + insumoIDArrendadora).ToObject<List<existenciaInsumosDTO>>();

                decimal totalArrendadora = 0;
                decimal totalConstruplan = 0;

                foreach (var a in AcumuladoConstruplan)
                {
                    insumosDTO objData = new insumosDTO();
                    decimal cantidadConstruplan = 0;

                    decimal sumas = a.existencia_ent_ini +
                                    a.existencia_ent_ene +
                                    a.existencia_ent_feb +
                                    a.existencia_ent_mar +
                                    a.existencia_ent_abr +
                                    a.existencia_ent_may +
                                    a.existencia_ent_jun +
                                    a.existencia_ent_jul +
                                    a.existencia_ent_ago +
                                    a.existencia_ent_sep +
                                    a.existencia_ent_oct +
                                    a.existencia_ent_nov +
                                    a.existencia_ent_dic;


                    decimal resultado = sumas -
                                        a.existencia_sal_ini -
                                        a.existencia_sal_ene -
                                        a.existencia_sal_feb -
                                        a.existencia_sal_mar -
                                        a.existencia_sal_abr -
                                        a.existencia_sal_may -
                                        a.existencia_sal_jun -
                                        a.existencia_sal_jul -
                                        a.existencia_sal_ago -
                                        a.existencia_sal_sep -
                                        a.existencia_sal_oct -
                                        a.existencia_sal_nov -
                                        a.existencia_sal_dic;


                    // resultado = a.existencia_ent_ini - a.existencia_sal_ini;
                    cantidadConstruplan = resultado;
                    objData.almacen = a.almacen;
                    objData.numInsumoConstruplan = insumo;
                    objData.desInsumoConstruplan = dataInsumoConstruplan.FirstOrDefault() != null ? dataInsumoConstruplan.FirstOrDefault().descripcion : "";
                    objData.cantidadInsumos = cantidadConstruplan;
                    objData.locacion = 1;
                    objReturn.Add(objData);

                }


                foreach (var a in AcumuladoArrendadora)
                {
                    insumosDTO objData = new insumosDTO();
                    decimal cantidadArrendadora = 0;
                    decimal sumas = a.existencia_ent_ini +
                                    a.existencia_ent_ene +
                                    a.existencia_ent_feb +
                                    a.existencia_ent_mar +
                                    a.existencia_ent_abr +
                                    a.existencia_ent_may +
                                    a.existencia_ent_jun +
                                    a.existencia_ent_jul +
                                    a.existencia_ent_ago +
                                    a.existencia_ent_sep +
                                    a.existencia_ent_oct +
                                    a.existencia_ent_nov +
                                    a.existencia_ent_dic;


                    decimal resultado = sumas -
                                        a.existencia_sal_ini -
                                        a.existencia_sal_ene -
                                        a.existencia_sal_feb -
                                        a.existencia_sal_mar -
                                        a.existencia_sal_abr -
                                        a.existencia_sal_may -
                                        a.existencia_sal_jun -
                                        a.existencia_sal_jul -
                                        a.existencia_sal_ago -
                                        a.existencia_sal_sep -
                                        a.existencia_sal_oct -
                                        a.existencia_sal_nov -
                                        a.existencia_sal_dic;

                    //resultado = a.existencia_ent_ini - a.existencia_sal_ini;
                    objData.almacen = a.almacen;
                    cantidadArrendadora = resultado;
                    objData.numInsumoArrendadora = dataInsumoArrendadora.FirstOrDefault() != null ? dataInsumoArrendadora.FirstOrDefault().insumo : 0;
                    objData.desInsumoArrendadora = dataInsumoArrendadora.FirstOrDefault() != null ? dataInsumoArrendadora.FirstOrDefault().descripcion : "";
                    objData.locacion = 2;
                    objData.cantidadInsumos = cantidadArrendadora;
                    objReturn.Add(objData);
                }
            }
            return objReturn;
        }
        public List<insumosDTO> getInsumoMultiple(List<int> insumos, int anio, int tipo, string almacen)
        {
            List<insumosDTO> listaResultado = new List<insumosDTO>();

            #region Información Inicial Perú
            List<TABALM> listaAlmacenes = new List<TABALM>();
            List<MAEART> listaInsumos = new List<MAEART>();

            if ((EmpresaEnum)vSesiones.sesionEmpresaActual == EmpresaEnum.Peru)
            {
                using (var _starsoft = new MainContextPeruStarSoft003BDCOMUN())
                {
                    listaAlmacenes = _starsoft.TABALM.ToList();
                    listaInsumos = _starsoft.MAEART.ToList();
                }
            }
            #endregion

            foreach (var insumo in insumos)
            {
                switch ((EmpresaEnum)vSesiones.sesionEmpresaActual)
                {
                    case EmpresaEnum.Peru:
                        {
                            #region PERÚ
                            listaResultado.AddRange(_context.tblAlm_MovimientosDet.Where(x => x.estatusHabilitado && x.insumo == insumo).ToList().GroupBy(x => x.almacen).Select(x => new insumosDTO
                            {
                                almacen = x.Key.ToString(),
                                almacenNombre = listaAlmacenes.Where(y => Int32.Parse(y.TAALMA) == x.Key).Select(z => z.TADESCRI).FirstOrDefault(),
                                numInsumoConstruplan = insumo,
                                desInsumoConstruplan = listaInsumos.Where(y => Int32.Parse(y.ACODIGO) == insumo).Select(z => z.ADESCRI).FirstOrDefault(),
                                cantidadInsumos = x.Sum(y => y.tipo_mov < 50 ? y.cantidad : (y.cantidad * -1)),
                                locacion = 1
                            }).ToList());
                            break;
                            #endregion
                        }
                    default:
                        {
                            #region DEMÁS EMPRESAS
                            string insumoIDConstruplan = "";
                            string insumoIDArrendadora = "";
                            List<tblAlm_MergeInsumos> listaInsumosSigoplan = new List<tblAlm_MergeInsumos>();

                            switch ((EmpresaEnum)tipo)
                            {
                                case EmpresaEnum.Construplan:
                                    {
                                        listaInsumosSigoplan = _context.tblAlm_MergeInsumos.Where(c => c.insumoC == insumo).ToList();

                                        if (listaInsumosSigoplan.Count() > 0)
                                        {
                                            insumoIDConstruplan = listaInsumosSigoplan.FirstOrDefault().insumoC.ToString();
                                            insumoIDArrendadora = listaInsumosSigoplan.FirstOrDefault().insumoA.ToString();
                                        }
                                        else
                                        {
                                            listaInsumosSigoplan.Add(new tblAlm_MergeInsumos
                                            {
                                                id = 0,
                                                estatus = true,
                                                insumoC = insumo,
                                                insumoA = 0
                                            });
                                            insumoIDConstruplan = insumo.ToString();
                                            insumoIDArrendadora = "0";
                                        }
                                        break;
                                    }
                                case EmpresaEnum.Arrendadora:
                                    {
                                        listaInsumosSigoplan = _context.tblAlm_MergeInsumos.Where(c => c.insumoA == insumo).ToList();

                                        if (listaInsumosSigoplan.Count() > 0)
                                        {
                                            insumoIDConstruplan = listaInsumosSigoplan.FirstOrDefault().insumoC.ToString();
                                            insumoIDArrendadora = listaInsumosSigoplan.FirstOrDefault().insumoA.ToString();
                                        }
                                        else
                                        {
                                            listaInsumosSigoplan.Add(new tblAlm_MergeInsumos
                                            {
                                                id = 0,
                                                estatus = true,
                                                insumoC = 0,
                                                insumoA = insumo
                                            });
                                            insumoIDConstruplan = "0";
                                            insumoIDArrendadora = insumo.ToString();
                                        }
                                        break;
                                    }
                                default:
                                    {
                                        return null;
                                    }
                            }

                            if (listaInsumosSigoplan.Count() > 0)
                            {
                                #region Consulta Acumulados
                                string consultaAcumulados = @"
                                    SELECT
                                        a.almacen,
                                        a.descripcion as almacenNombre,
                                        ac.cc,
                                        ac.ano,
                                        CASE WHEN ac.existencia_ent_ini IS NULL THEN 0 ELSE ac.existencia_ent_ini END as existencia_ent_ini,
                                        CASE WHEN ac.existencia_sal_ini IS NULL THEN 0 ELSE ac.existencia_sal_ini END as existencia_sal_ini,
                                        ac.existencia_ent_ene,
                                        ac.existencia_sal_ene,
                                        ac.existencia_ent_feb,
                                        ac.existencia_sal_feb,
                                        ac.existencia_ent_mar,
                                        ac.existencia_sal_mar,
                                        ac.existencia_ent_abr,
                                        ac.existencia_sal_abr,
                                        ac.existencia_ent_may,
                                        ac.existencia_sal_may,
                                        ac.existencia_ent_jun,
                                        ac.existencia_sal_jun,
                                        ac.existencia_ent_jul,
                                        ac.existencia_sal_jul,
                                        ac.existencia_ent_ago,
                                        ac.existencia_sal_ago,
                                        ac.existencia_ent_sep,
                                        ac.existencia_sal_sep,
                                        ac.existencia_ent_oct,
                                        ac.existencia_sal_oct,
                                        ac.existencia_ent_nov,
                                        ac.existencia_sal_nov,
                                        ac.existencia_ent_dic,
                                        ac.existencia_sal_dic, 
                                        ac.insumo
                                    FROM si_acumula_almacen ac
                                        INNER JOIN si_almacen AS a ON ac.almacen = a.almacen
                                    WHERE ac.ano = '" + DateTime.Now.Year + "' AND ac.insumo = ";
                                #endregion

                                #region Construplan
                                var dataInsumoConstruplan = (List<detInsumoDTO>)_contextEnkontrol.Where("SELECT  insumo, descripcion FROM insumos WHERE insumo = " + insumoIDConstruplan).ToObject<List<detInsumoDTO>>();
                                var AcumuladoConstruplan = (List<existenciaInsumosDTO>)_contextEnkontrol.Where(consultaAcumulados + insumoIDConstruplan).ToObject<List<existenciaInsumosDTO>>();

                                foreach (var a in AcumuladoConstruplan)
                                {
                                    decimal entradas = a.existencia_ent_ini +
                                        a.existencia_ent_ene + a.existencia_ent_feb + a.existencia_ent_mar + a.existencia_ent_abr + a.existencia_ent_may + a.existencia_ent_jun +
                                        a.existencia_ent_jul + a.existencia_ent_ago + a.existencia_ent_sep + a.existencia_ent_oct + a.existencia_ent_nov + a.existencia_ent_dic;

                                    decimal resultado = entradas - a.existencia_sal_ini -
                                        a.existencia_sal_ene - a.existencia_sal_feb - a.existencia_sal_mar - a.existencia_sal_abr - a.existencia_sal_may - a.existencia_sal_jun -
                                        a.existencia_sal_jul - a.existencia_sal_ago - a.existencia_sal_sep - a.existencia_sal_oct - a.existencia_sal_nov - a.existencia_sal_dic;

                                    listaResultado.Add(new insumosDTO
                                    {
                                        almacen = a.almacen,
                                        almacenNombre = a.almacenNombre,
                                        numInsumoConstruplan = insumo,
                                        desInsumoConstruplan = dataInsumoConstruplan.FirstOrDefault() != null ? dataInsumoConstruplan.FirstOrDefault().descripcion : "",
                                        cantidadInsumos = resultado,
                                        locacion = 1
                                    });
                                }
                                #endregion

                                #region Arrendadora
                                var dataInsumoArrendadora = (List<detInsumoDTO>)ContextArrendadora.Where("SELECT  insumo, descripcion FROM insumos WHERE insumo = " + insumoIDArrendadora).ToObject<List<detInsumoDTO>>();
                                var AcumuladoArrendadora = (List<existenciaInsumosDTO>)ContextArrendadora.Where(consultaAcumulados + insumoIDArrendadora).ToObject<List<existenciaInsumosDTO>>();

                                foreach (var a in AcumuladoArrendadora)
                                {
                                    decimal entradas = a.existencia_ent_ini +
                                        a.existencia_ent_ene + a.existencia_ent_feb + a.existencia_ent_mar + a.existencia_ent_abr + a.existencia_ent_may + a.existencia_ent_jun +
                                        a.existencia_ent_jul + a.existencia_ent_ago + a.existencia_ent_sep + a.existencia_ent_oct + a.existencia_ent_nov + a.existencia_ent_dic;

                                    decimal resultado = entradas - a.existencia_sal_ini -
                                        a.existencia_sal_ene - a.existencia_sal_feb - a.existencia_sal_mar - a.existencia_sal_abr - a.existencia_sal_may - a.existencia_sal_jun -
                                        a.existencia_sal_jul - a.existencia_sal_ago - a.existencia_sal_sep - a.existencia_sal_oct - a.existencia_sal_nov - a.existencia_sal_dic;

                                    listaResultado.Add(new insumosDTO
                                    {
                                        almacen = a.almacen,
                                        almacenNombre = a.almacenNombre,
                                        numInsumoArrendadora = dataInsumoArrendadora.FirstOrDefault() != null ? dataInsumoArrendadora.FirstOrDefault().insumo : 0,
                                        desInsumoArrendadora = dataInsumoArrendadora.FirstOrDefault() != null ? dataInsumoArrendadora.FirstOrDefault().descripcion : "",
                                        locacion = 2,
                                        cantidadInsumos = resultado
                                    });
                                }
                                #endregion
                            }
                            break;
                            #endregion
                        }
                }
            }

            return listaResultado;
        }

        private string returnConcatInsumos(List<int> p)
        {
            string res = "";

            foreach (var item in p)
            {

                res += "'" + p + "',";
            }

            return res.Trim(',');
        }

        public List<detInsumoDTO> getListaInsumos(string term, int TipoInsumo, int GrupoInsumo, int sistema)
        {
            switch ((EmpresaEnum)vSesiones.sesionEmpresaActual)
            {
                case EmpresaEnum.Peru:
                    {
                        #region PERÚ
                        term = term.ToUpper().Trim();

                        using (var _starsoft = new MainContextPeruStarSoft003BDCOMUN())
                        {
                            var listaInsumos = _starsoft.MAEART.Where(x => x.ADESCRI != null).ToList().Where(x => x.ADESCRI.Contains(term)).Select(x => new detInsumoDTO
                            {
                                insumo = Int32.Parse(x.ACODIGO),
                                descripcion = x.ADESCRI
                            }).ToList();

                            if (listaInsumos.Count() > 500)
                            {
                                listaInsumos = listaInsumos.Take(500).ToList();
                            }

                            return listaInsumos;
                        }
                        #endregion
                    }
                default:
                    {
                        term = term.Replace("'", "''"); //Se reemplazan las comillas simples por simples duplicadas para que entre la consulta de SQL correctamente

                        string Anexo = "";

                        if (term.Equals("") && TipoInsumo == 0 && GrupoInsumo == 0)
                        {
                            return new List<detInsumoDTO>();
                        }
                        else
                        {
                            if (!string.IsNullOrEmpty(term))
                            {
                                Anexo = " WHERE descripcion like '%" + term + "%' ";

                                if (TipoInsumo != 0)
                                {
                                    Anexo = "AND tipo =" + TipoInsumo;
                                }

                                if (GrupoInsumo != 0)
                                {
                                    Anexo = (!string.IsNullOrEmpty(Anexo) ? Anexo : "") + " AND grupo=" + GrupoInsumo;
                                }
                            }
                            else
                            {
                                if (TipoInsumo != 0)
                                {
                                    Anexo = " WHERE tipo = " + TipoInsumo;

                                    if (GrupoInsumo != 0)
                                    {
                                        Anexo = (!string.IsNullOrEmpty(Anexo) ? Anexo : "") + " AND grupo = " + GrupoInsumo;
                                    }
                                }
                                else
                                {
                                    if (GrupoInsumo != 0)
                                    {
                                        Anexo = " WHERE " + (!string.IsNullOrEmpty(Anexo) ? Anexo : "") + " grupo = " + GrupoInsumo;
                                    }
                                }
                            }

                            switch (sistema)
                            {
                                case 1:
                                    {
                                        return ((List<detInsumoDTO>)_contextEnkontrol.Where("SELECT TOP 500 insumo, descripcion, tipo, grupo FROM insumos " + Anexo).ToObject<List<detInsumoDTO>>()).ToList();
                                    }
                                case 2:
                                    {
                                        return ((List<detInsumoDTO>)ContextArrendadora.Where("SELECT TOP 500 insumo, descripcion, tipo, grupo FROM insumos " + Anexo).ToObject<List<detInsumoDTO>>()).ToList();
                                    }
                                default:
                                    {
                                        return null;
                                    }
                            }
                        }
                    }
            }
        }

        public List<detInsumoDTO> getInsumo(string term, int TipoInsumo, int GrupoInsumo, int sistema)
        {
            switch ((EmpresaEnum)vSesiones.sesionEmpresaActual)
            {
                case EmpresaEnum.Peru:
                    {
                        #region PERÚ
                        term = term.ToUpper().Trim();

                        using (var _starsoft = new MainContextPeruStarSoft003BDCOMUN())
                        {
                            return _starsoft.MAEART.Where(x => x.ADESCRI != null).ToList().Where(x => x.ADESCRI.Contains(term)).Select(x => new detInsumoDTO
                            {
                                insumo = Int32.Parse(x.ACODIGO),
                                descripcion = x.ADESCRI
                            }).ToList();
                        }
                        #endregion
                    }
                default:
                    {
                        #region DEMÁS EMPRESAS
                        string Anexo = "";

                        if (TipoInsumo != 0)
                        {
                            Anexo = "AND tipo = " + TipoInsumo;
                        }

                        if (GrupoInsumo != 0)
                        {
                            Anexo = (!string.IsNullOrEmpty(Anexo) ? Anexo : "") + " AND grupo = " + GrupoInsumo;
                        }

                        switch (sistema)
                        {
                            case 1:
                                {
                                    return ((List<detInsumoDTO>)_contextEnkontrol.Where("SELECT TOP 10  insumo, descripcion FROM insumos WHERE descripcion like '%" + term + "%' " + Anexo).ToObject<List<detInsumoDTO>>()).Take(10).ToList();
                                }
                            case 2:
                                {
                                    return ((List<detInsumoDTO>)ContextArrendadora.Where("SELECT TOP 10  insumo, descripcion FROM insumos WHERE descripcion like '%" + term + "%' " + Anexo).ToObject<List<detInsumoDTO>>()).Take(10).ToList();
                                }
                            default:
                                {
                                    return null;
                                }
                        }
                        #endregion
                    }
            }
        }

        public List<ComboDTO> fillTipoInsumos(int sistema)
        {
            switch (sistema)
            {
                case 1:
                    {
                        return ((List<ComboDTO>)_contextEnkontrol.Where("SELECT tipo_insumo AS Value, descripcion AS Text FROM tipos_insumo").ToObject<List<ComboDTO>>()).Select(x => new ComboDTO { Value = x.Value, Text = x.Text }).ToList();
                    }
                case 2:
                    {
                        return ((List<ComboDTO>)ContextArrendadora.Where("SELECT tipo_insumo AS Value, descripcion AS Text FROM tipos_insumo").ToObject<List<ComboDTO>>()).Select(x => new ComboDTO { Value = x.Value, Text = x.Text }).ToList();
                    }
                default:
                    {
                        return null;
                    }
            }
        }

        public List<ComboDTO> fillGrupoInsumos(int tipo, int sistema)
        {
            string concat = "";
            if (tipo != 0)
            {
                concat = "WHERE tipo_insumo=" + tipo;
            }

            string ConsultaInsumo = "SELECT  grupo_insumo AS Value, descripcion AS Text FROM grupos_insumo " + concat;
            switch (sistema)
            {
                case 1:
                    {
                        var dataInsumoConstruplan = (List<ComboDTO>)_contextEnkontrol.Where(ConsultaInsumo).ToObject<List<ComboDTO>>();
                        return dataInsumoConstruplan.Select(x => new ComboDTO
                        {
                            Value = x.Value,
                            Text = x.Text

                        }).ToList();
                    }
                case 2:
                    {
                        var dataInsumoConstruplan = (List<ComboDTO>)ContextArrendadora.Where(ConsultaInsumo).ToObject<List<ComboDTO>>();
                        return dataInsumoConstruplan.Select(x => new ComboDTO
                        {
                            Value = x.Value,
                            Text = x.Text

                        }).ToList();
                    }
                default:
                    return null;
            }
        }

        public ComboDTO getInsumoTipoGrupoByID(int idInsumo)
        {
            //ComboDTO data = new ComboDTO();
            //string ConsultaInsumo = "SELECT  descripcion AS Value, tipo AS Text, grupo as Prefijo FROM insumos WHERE insumo = " + idInsumo;
            //var dataInsumoConstruplan = (List<ComboDTO>)_contextEnkontrol.Where(ConsultaInsumo).ToObject<List<ComboDTO>>();
            //data.Value = dataInsumoConstruplan.FirstOrDefault().Value;
            //data.Text = dataInsumoConstruplan.FirstOrDefault().Text;
            //data.Prefijo = dataInsumoConstruplan.FirstOrDefault().Prefijo;
            //return data;
            var odbc = new OdbcConsultaDTO()
            {
                consulta = "SELECT  descripcion AS Value, tipo AS Text, grupo as Prefijo FROM insumos WHERE insumo = ?"
            };
            odbc.parametros.Add(new OdbcParameterDTO() { nombre = "insumo", tipo = OdbcType.Numeric, valor = idInsumo });
            var data = _contextEnkontrol.Select<ComboDTO>(EnkontrolEnum.CplanProd, odbc).FirstOrDefault() ?? new ComboDTO();
            return data;
        }
    }
}
