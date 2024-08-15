using Core.DAO.Contabilidad.Propuesta;
using Core.DTO.Contabilidad.Propuesta.Nomina;
using Core.DTO.Principal.Generales;
using Core.DTO.Utils.Data;
using Core.Entity.Administrativo.Contabilidad.Poliza;
using Core.Entity.Administrativo.Contabilidad.Propuesta.Nomina;
using Core.Enum.Multiempresa;
using Data.EntityFramework.Context;
using Data.EntityFramework.Generic;
using System.Data.Entity.Migrations;
using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infrastructure.Utils;
using Core.Enum.Principal.Bitacoras;
using Data.DAO.Contabilidad.Reportes;
using Core.Enum.Administracion.Propuesta.Nomina;
using Data.Factory.Maquinaria.Catalogos;
using Data.DAO.Maquinaria.Catalogos;
using Core.DTO;

namespace Data.DAO.Contabilidad.Propuesta
{
    public class NominaResumenDAO : GenericDAO<tblC_NominaResumen>, INominaResumenDAO
    {
        string nombreControlador = "Nominas";
        #region guardar
        public bool guardarNominaPoliza(List<tblC_NominaPoliza> lst)
        {
            var esGuardado = false;
            using(var dbTransaction = _context.Database.BeginTransaction())
            try
            {
                var ahora = DateTime.Now;
                var fecha_inicio = lst.Min(pol => pol.fecha);
                var fecha_fin = lst.Max(pol => pol.fecha);
                var lstBD = getLstPolizaNominaActiva(fecha_inicio, fecha_fin);
                lst.ForEach(pol =>
                {
                    var bd = lstBD.FirstOrDefault(polbd => polbd.poliza == pol.poliza && polbd.cc == pol.cc && polbd.tipoCuenta == pol.tipoCuenta && polbd.cargo - polbd.abono + polbd.iva - polbd.retencion == pol.cargo - pol.abono + pol.iva - pol.retencion);
                    if(bd == null)
                    {

                    }
                    else
                    {
                        pol.id = bd.id;
                        pol.esActivo = bd.esActivo;
                    }
                    pol.esActivo = true;
                    pol.fechaCaptura = ahora;
                    _context.tblC_NominaPoliza.AddOrUpdate(pol);
                    SaveChanges();
                });
                esGuardado = lst.Any();
                if(esGuardado)
                {
                    var entity = lst.FirstOrDefault();
                    SaveBitacora((int)BitacoraEnum.NominaPoliza, (int)AccionEnum.AGREGAR, entity.id, JsonUtils.convertNetObjectToJson(entity));
                }
                dbTransaction.Commit();
            }
            catch(Exception o_O)
            {
                var entity = lst.FirstOrDefault();
                esGuardado = false;
                dbTransaction.Rollback();
                LogError(vSesiones.sesionSistemaActual, vSesiones.sesionCurrentView, nombreControlador, "guardarNominaPoliza", o_O, AccionEnum.ACTUALIZAR, 0, entity);
            }
            return esGuardado;
        }
        public bool guardarNominaResumen(List<tblC_NominaResumen> lst)
        {
            var esGuardado = false;
            using (var dbTransaction = _context.Database.BeginTransaction())
            try
            {
                var ahora = DateTime.Now;
                var fecha_inicio = lst.Min(nom => nom.fecha_inicial);
                var fecha_fin = lst.Max(nom => nom.fecha_final);
                var lstbd = getLstResumenNominaActiva(fecha_inicio, fecha_fin);
                lst.ForEach(nom =>
                {
                    var bd = lstbd.FirstOrDefault(nombd => nombd.cc.Equals(nom.cc) && nombd.tipoCuenta.Equals(nom.tipoCuenta) && nombd.tipoNomina.Equals(nom.tipoNomina));
                    if (bd == null)
                    {
                        nom.esActivo = true;
                    }
                    else
                    {
                        nom.id = bd.id;
                        nom.esActivo = bd.esActivo;
                    }
                    nom.fechaCaptura = ahora;
                        _context.tblC_NominaResumen.AddOrUpdate(nom);
                        SaveChanges();
                });
                esGuardado = lst.Any();
                if (esGuardado)
                {
                    var entity = lst.FirstOrDefault();
                    SaveBitacora((int)BitacoraEnum.NominaResumen, (int)AccionEnum.AGREGAR, entity.id, JsonUtils.convertNetObjectToJson(entity));
                }
                SaveChanges();
                dbTransaction.Commit();
            }
            catch (Exception o_O)
            {
                var entity = lst.FirstOrDefault();
                esGuardado = false;
                dbTransaction.Rollback();
                LogError(vSesiones.sesionSistemaActual, vSesiones.sesionCurrentView, nombreControlador, "guardarNominaResumen", o_O, AccionEnum.ACTUALIZAR, 0, entity);
            }
            return esGuardado;
        }
        #endregion
        #region Consulta Sigoplan
        public List<tblC_NominaPoliza> getLstPolizaNominaActiva(DateTime fecha_inicio, DateTime fecha_fin)
        {
            return _context.tblC_NominaPoliza.ToList()
                .Where(pol => pol.esActivo)
                .Where(pol => pol.fecha >= fecha_inicio && pol.fecha <= fecha_fin)
                .ToList();
        }
        public List<tblC_NominaPoliza> getLstPolizaNominaActiva(int mes)
        {
            return _context.tblC_NominaPoliza.ToList()
                .Where(pol => pol.esActivo)
                .Where(pol => pol.mes == mes)
                .ToList();
        }
        public List<tblC_NominaResumen> getLstResumenNominaActiva(DateTime fecha_inicio)
        {
            return _context.tblC_NominaResumen.ToList()
                .Where(nom => nom.esActivo)
                .Where(nom => nom.fecha_inicial >= fecha_inicio)
                .ToList();
        }
        public List<tblC_NominaResumen> getLstResumenNominaActiva(DateTime fecha_inicio, DateTime fecha_fin)
        {
            return _context.tblC_NominaResumen.ToList()
                .Where(nom => nom.esActivo)
                .Where(nom => nom.fecha_inicial >= fecha_inicio && nom.fecha_final <= fecha_fin)
                .ToList();
        }
        public List<tblC_NominaResumen> getLstResumenNominaActiva(DateTime fecha_inicio, DateTime fecha_fin, int tipoNomina)
        {
            return _context.tblC_NominaResumen.ToList()
                .Where(nom => nom.esActivo)
                .Where(nom => nom.tipoNomina == tipoNomina)
                .Where(nom => nom.fecha_inicial >= fecha_inicio && nom.fecha_final <= fecha_fin)
                .ToList();
        }

        public Dictionary<string, object> GetNominasQuincenalesSemanales(DateTime fechaInicio, DateTime fechaFinal, int tipoNomina)
        {
            var resultado = new Dictionary<string, object>();
            try
            {
                var nominasActivas = getLstPolizaNominaActiva(fechaInicio, fechaFinal).Where(nomina => nomina.tipoNomina.Equals(tipoNomina)).ToList();
                var listaObras = new CadenaProductivaDAO().lstObra();
                var listaNominas = new List<NominaResumenDTO>();
                var lstResumen = getLstResumenNominaActiva(fechaInicio, fechaFinal, tipoNomina);
                #region Ravelio
                var obrasAdministrativasRavelio = listaObras.Where(x => x.bit_area == "3").OrderBy(x => x.cc).ToList();
                var nominasActivasRavelio = nominasActivas.Where(x => x.tipoCuenta == (int)tipoCuentaNominaEnum.ConstructoraRavelio).ToList();
                var lstResumenRavelio = lstResumen.Where(r => r.tipoCuenta == (int)tipoCuentaNominaEnum.ConstructoraRavelio).ToList();

                var listaNominasAdministrativasRavelio = new List<NominaResumenDTO>();
                listaNominasAdministrativasRavelio.AddRange(
                    nominasActivasRavelio
                    .GroupBy(g => g.cc)
                    .Where(w => obrasAdministrativasRavelio.Any(c => c.cc == w.Key))
                    .Select(nomina => new NominaResumenDTO()
                    {
                        cc = nomina.Key,
                        descripcion = listaObras.FirstOrDefault(c => c.cc == nomina.Key).descripcion,
                        nomina = nomina.Sum(s => s.cargo - s.abono),
                        iva = nomina.Sum(s => s.iva),
                        total = nomina.Sum(s => s.cargo - s.abono + s.iva),
                        noEmpleado = lstResumenRavelio.Where(w => w.cc == nomina.Key).Sum(s => s.noEmpleado),
                        noPracticante = lstResumenRavelio.Where(w => w.cc == nomina.Key).Sum(s => s.noPracticante),
                        clase = "normal",
                        tipoCuenta = tipoCuentaNominaEnum.ConstructoraRavelio,
                        fecha_inicial = fechaInicio,
                        fecha_final = fechaFinal,
                        tipoNomina = (tipoNominaPropuestaEnum)tipoNomina,
                        division = "Administrativo"
                    }).ToList());
                var obrasAdministrativasCerradas = listaObras.Where(x => x.bit_area == "-1").OrderBy(x => x.cc).ToList();
                var nominasObrasAdministrativasCerradas = nominasActivasRavelio.Where(x => obrasAdministrativasCerradas.Any(y => y.cc == x.cc)).ToList();
                var empleadosObrasAdministrativasCerradas = lstResumenRavelio.Where(x => nominasObrasAdministrativasCerradas.Any(y => y.cc == x.cc)).ToList();

                var obraAdministrativaCerrada = new NominaResumenDTO
                {
                    cc = "0-1",
                    descripcion = "OBRA CERRADA",
                    nomina = nominasObrasAdministrativasCerradas.Sum(x => x.cargo - x.abono),
                    iva = nominasObrasAdministrativasCerradas.Sum(x => x.iva),
                    total = nominasObrasAdministrativasCerradas.Sum(x => (x.cargo - x.abono + x.iva)),
                    noEmpleado = (empleadosObrasAdministrativasCerradas != null && empleadosObrasAdministrativasCerradas.Count > 0) ? empleadosObrasAdministrativasCerradas.Sum(y => y.noEmpleado) : 0,
                    noPracticante = (empleadosObrasAdministrativasCerradas != null && empleadosObrasAdministrativasCerradas.Count > 0) ? empleadosObrasAdministrativasCerradas.Sum(y => y.noPracticante) : 0,
                    clase = "normal",
                    tipoCuenta = tipoCuentaNominaEnum.ConstructoraRavelio,
                    fecha_inicial = fechaInicio,
                    fecha_final = fechaFinal,
                    tipoNomina = (tipoNominaPropuestaEnum)tipoNomina,
                    division = "Administrativo"
                };

                if(nominasObrasAdministrativasCerradas.Sum(x => (x.cargo - x.abono + x.iva)) > 0)
                {
                    listaNominasAdministrativasRavelio.Add(obraAdministrativaCerrada);
                }

                decimal totalNominaAdministrativoRavelio = listaNominasAdministrativasRavelio.Sum(x => x.nomina) + obraAdministrativaCerrada.nomina;
                decimal totalIvaAdministrativoRavelio = listaNominasAdministrativasRavelio.Sum(x => x.iva) + obraAdministrativaCerrada.iva;
                decimal totalAdministrativoRavelio = (listaNominasAdministrativasRavelio.Sum(x => x.total) + obraAdministrativaCerrada.total);
                decimal totalEmpleadosAdministrativoRavelio = listaNominasAdministrativasRavelio.Sum(x => x.noEmpleado) + obraAdministrativaCerrada.noEmpleado;
                int totalPracticantesAdministrativoRavelio = listaNominasAdministrativasRavelio.Sum(x => x.noPracticante) + obraAdministrativaCerrada.noPracticante;

                if(totalAdministrativoRavelio > 0)
                {
                    // SE AGREGAN OBRAS ADMINISTRATIVAS RAVELIO
                    listaNominasAdministrativasRavelio.Add(new NominaResumenDTO
                    {
                        cc = "ZZZ",
                        descripcion = "Total",
                        nomina = totalNominaAdministrativoRavelio,
                        iva = totalIvaAdministrativoRavelio,
                        total = totalAdministrativoRavelio,
                        noEmpleado = totalEmpleadosAdministrativoRavelio,
                        noPracticante = totalPracticantesAdministrativoRavelio,
                        clase = "totalCuadro",
                        tipoCuenta = tipoCuentaNominaEnum.ConstructoraRavelio,
                        division = "Administrativo"
                    });

                    listaNominas.AddRange(listaNominasAdministrativasRavelio);
                }

                listaNominas.Add(new NominaResumenDTO
                {
                    cc = "ZZZ",
                    descripcion = "",
                    clase = "vacio"
                });

                // Industrial 
                var obrasIndustriales = listaObras.Where(x => x.bit_area == "2" || x.bit_area == "22").OrderBy(x => x.cc).ToList();
                var listaNominasIndustrialesRavelio = new List<NominaResumenDTO>();
                listaNominasIndustrialesRavelio.AddRange(
                    nominasActivasRavelio
                    .GroupBy(g => g.cc)
                    .Where(w => obrasIndustriales.Any(c => c.cc == w.Key))
                    .Select(nomina => new NominaResumenDTO()
                {
                    cc = nomina.Key,
                    descripcion = obrasIndustriales.FirstOrDefault(c => c.cc == nomina.Key).descripcion,
                    nomina = nomina.Sum(s => s.cargo - s.abono),
                    iva = nomina.Sum(s => s.iva),
                    total = nomina.Sum(s => s.cargo - s.abono + s.iva),
                    noEmpleado = lstResumenRavelio.Where(w => w.cc == nomina.Key).Sum(s => s.noEmpleado),
                    noPracticante = lstResumenRavelio.Where(w => w.cc == nomina.Key).Sum(s => s.noPracticante),
                    clase = "normal",
                    tipoCuenta = tipoCuentaNominaEnum.ConstructoraRavelio,
                    fecha_inicial = fechaInicio,
                    fecha_final = fechaFinal,
                    tipoNomina = (tipoNominaPropuestaEnum)tipoNomina,
                    division = "Industrial"
                }).ToList());
                var obrasIndustrialesCerradas = listaObras.Where(x => x.bit_area == "-2" || x.bit_area == "-22").OrderBy(x => x.cc).ToList();
                var nominasObrasIndustrialesCerradas = nominasActivasRavelio.Where(x => obrasIndustrialesCerradas.Any(y => y.cc == x.cc)).ToList();
                var empleadosObrasIndustrialesCerradas = lstResumenRavelio.Where(x => nominasObrasIndustrialesCerradas.Any(y => y.cc == x.cc)).ToList();

                var obraIndustrialCerrada = new NominaResumenDTO
                {
                    cc = "0-2",
                    descripcion = "OBRA CERRADA",
                    nomina = nominasObrasIndustrialesCerradas.Sum(x => x.cargo - x.abono),
                    iva = nominasObrasIndustrialesCerradas.Sum(x => x.iva),
                    total = nominasObrasIndustrialesCerradas.Sum(x => (x.cargo - x.abono + x.iva)),
                    noEmpleado = (empleadosObrasIndustrialesCerradas != null && empleadosObrasIndustrialesCerradas.Count > 0) ? empleadosObrasIndustrialesCerradas.Sum(x => x.noEmpleado) : 0,
                    noPracticante = (empleadosObrasIndustrialesCerradas != null && empleadosObrasIndustrialesCerradas.Count > 0) ? empleadosObrasIndustrialesCerradas.Sum(x => x.noPracticante) : 0,
                    clase = "normal",
                    tipoCuenta = tipoCuentaNominaEnum.ConstructoraRavelio,
                    fecha_inicial = fechaInicio,
                    fecha_final = fechaFinal,
                    tipoNomina = (tipoNominaPropuestaEnum)tipoNomina,
                    division = "Industrial"
                };

                if(obraIndustrialCerrada.total > 0)
                {
                    listaNominasIndustrialesRavelio.Add(obraIndustrialCerrada);
                }

                decimal totalNominaIndustrialRavelio = listaNominasIndustrialesRavelio.Sum(x => x.nomina) + obraIndustrialCerrada.nomina;
                decimal totalIvaIndustrialRavelio = listaNominasIndustrialesRavelio.Sum(x => x.iva) + obraIndustrialCerrada.iva;
                decimal totalIndustrialRavelio = (listaNominasIndustrialesRavelio.Sum(x => x.total) + obraIndustrialCerrada.total);
                decimal totalEmpleadosIndustrialRavelio = listaNominasIndustrialesRavelio.Sum(x => x.noEmpleado) + obraIndustrialCerrada.noEmpleado;
                int totalPracticantesIndustrialRavelio = listaNominasIndustrialesRavelio.Sum(x => x.noPracticante) + obraIndustrialCerrada.noPracticante;

                if(totalIndustrialRavelio > 0)
                {
                    listaNominas.Add(new NominaResumenDTO
                    {
                        cc = "C.C.",
                        descripcion = "AREA INDUSTRIAL",
                        nomina = 0,
                        iva = 0,
                        total = 0,
                        noEmpleado = 0,
                        clase = "encabezadoTabla",
                        division = "Industrial"
                    });

                    listaNominasIndustrialesRavelio.Add(new NominaResumenDTO
                    {
                        cc = "ZZZ",
                        descripcion = "Total",
                        nomina = totalNominaIndustrialRavelio,
                        iva = totalIvaIndustrialRavelio,
                        total = totalIndustrialRavelio,
                        noEmpleado = totalEmpleadosIndustrialRavelio,
                        noPracticante = totalPracticantesIndustrialRavelio,
                        tipoCuenta = tipoCuentaNominaEnum.ConstructoraRavelio,
                        clase = "totalCuadro",
                        division = "Industrial"
                    });

                    listaNominasIndustrialesRavelio.Add(new NominaResumenDTO
                    {
                        cc = "ZZZ",
                        descripcion = "",
                        nomina = 0,
                        iva = 0,
                        total = 0,
                        noEmpleado = 0,
                        clase = "vacio"
                    });

                    // SE AGREGAN OBRAS INDUSTRIALES RAVELIO
                    listaNominas.AddRange(listaNominasIndustrialesRavelio);
                }

                ///////////////////////////////////// Obras individuales 
                var obrasIndividualesRavelio = listaObras.Where(x =>
                    x.bit_area != "3" &&
                    x.bit_area != "-1" &&
                    x.bit_area != "2" &&
                    x.bit_area != "22" &&
                    x.bit_area != "-2" &&
                    x.bit_area != "-22")
                    .OrderBy(x => x.cc)
                    .ToList();
                var listaNominasIndividualesRavelio = new List<NominaResumenDTO>();
                listaNominasIndividualesRavelio.AddRange(
                    nominasActivasRavelio
                    .GroupBy(g => g.cc)
                    .Where(w => obrasIndividualesRavelio.Any(c => c.cc == w.Key))
                    .Select(nomina => new NominaResumenDTO()
                {
                    cc = nomina.Key,
                    descripcion = obrasIndividualesRavelio.FirstOrDefault(c => c.cc == nomina.Key).descripcion,
                    nomina = nomina.Sum(s => s.cargo - s.abono),
                    iva = nomina.Sum(s => s.iva),
                    total = nomina.Sum(s => s.cargo - s.abono + s.iva),
                    noEmpleado = lstResumenRavelio.Where(w => w.cc == nomina.Key).Sum(s => s.noEmpleado),
                    noPracticante = lstResumenRavelio.Where(w => w.cc == nomina.Key).Sum(s => s.noPracticante),
                    clase = "obraIndividual",
                    tipoCuenta = tipoCuentaNominaEnum.ConstructoraRavelio,
                    fecha_inicial = fechaInicio,
                    fecha_final = fechaFinal,
                    tipoNomina = (tipoNominaPropuestaEnum)tipoNomina,
                    division = "Individual"
                }).ToList());

                if(listaNominasIndividualesRavelio.Count > 0)
                {
                    listaNominas.AddRange(listaNominasIndividualesRavelio);
                }

                decimal totalNominaRavelio = totalNominaAdministrativoRavelio + totalNominaIndustrialRavelio + listaNominasIndividualesRavelio.Sum(x => x.nomina);
                decimal totalIvaRavelio = totalIvaAdministrativoRavelio + totalIvaIndustrialRavelio + listaNominasIndividualesRavelio.Sum(x => x.iva);
                decimal totalRavelio = totalAdministrativoRavelio + totalIndustrialRavelio + listaNominasIndividualesRavelio.Sum(x => x.total);
                decimal totalEmpleadosRavelio = totalEmpleadosAdministrativoRavelio + totalEmpleadosIndustrialRavelio + listaNominasIndividualesRavelio.Sum(x => x.noEmpleado);
                int totalPracticantesRavelio = totalPracticantesAdministrativoRavelio + totalPracticantesIndustrialRavelio + listaNominasIndividualesRavelio.Sum(x => x.noPracticante);


                listaNominas.Add(new NominaResumenDTO
                {
                    cc = "ZZZ",
                    descripcion = "TOTAL TRANSFERENCIA A RAVELIO, SA DE CV",
                    nomina = totalNominaRavelio,
                    iva = totalIvaRavelio,
                    total = totalRavelio,
                    noEmpleado = totalEmpleadosRavelio,
                    noPracticante = totalPracticantesRavelio,
                    clase = "totalCuenta " + tipoCuentaNominaEnum.ConstructoraRavelio.GetDescription(),
                    tipoCuenta = tipoCuentaNominaEnum.ConstructoraRavelio,
                    division = "Ravelio"
                });

                listaNominas.Add(new NominaResumenDTO
                {
                    cc = "ZZZ",
                    descripcion = "",
                    nomina = 0,
                    iva = 0,
                    total = 0,
                    noEmpleado = 0,
                    clase = "vacio"
                });
                #endregion

                #region Construplan
                var lstTipoCuentaCplanNomina = new List<int>() 
                {
                    (int)tipoCuentaNominaEnum.ProvisionServicioAdministrativos,
                    (int)tipoCuentaNominaEnum.CONSTRUPLAN
                };
                var nominasActivasConstruplan = nominasActivas.Where(x => lstTipoCuentaCplanNomina.Contains(x.tipoCuenta));
                // Administrativo (Obra)
                var obrasAdministrativasConstruplan = listaObras.Where(x => x.bit_area == "3").OrderBy(x => x.cc).ToList();
                var listaNominasAdministrativasConstruplan = new List<NominaResumenDTO>();
                var lstResumenCplan = lstResumen.Where(x => lstTipoCuentaCplanNomina.Contains(x.tipoCuenta)).ToList();
                foreach(var obraAdministrativa in obrasAdministrativasConstruplan)
                {
                    var nomina = nominasActivasConstruplan.FirstOrDefault(x => x.cc == obraAdministrativa.cc);
                    if(nomina != null)
                    {
                        lstResumenCplan
                           .Where(x => x.cc == nomina.cc)
                           .GroupBy(x => x.cc).ToList()
                           .ForEach(x =>
                           {
                               listaNominasAdministrativasConstruplan.Add(new NominaResumenDTO
                               {
                                   cc = nomina.cc,
                                   descripcion = obraAdministrativa.descripcion,
                                   nomina = nomina.cargo - nomina.abono,
                                   iva = nomina.iva,
                                   total = nomina.cargo - nomina.abono + nomina.iva,
                                   noEmpleado = x != null ? x.Sum(y => y.noEmpleado) : 0,
                                   noPracticante = x.Sum(y => y.noPracticante),
                                   clase = "normal",
                                   tipoCuenta = (tipoCuentaNominaEnum)x.FirstOrDefault().tipoCuenta,
                                   fecha_inicial = fechaInicio,
                                   fecha_final = fechaFinal,
                                   tipoNomina = (tipoNominaPropuestaEnum)tipoNomina,
                                   division = "Cplan"
                               });
                           });

                    }
                }
                var obrasAdministrativasCerradasConstruplan = listaObras.Where(x => x.bit_area == "-1").OrderBy(x => x.cc).ToList();
                var nominasObrasAdministrativasCerradasConstruplan = nominasActivasConstruplan.Where(x => obrasAdministrativasCerradasConstruplan.Any(y => y.cc == x.cc)).ToList();
                var empleadosObrasAdministrativasCerradasConstruplan = lstResumenCplan.Where(x => nominasObrasAdministrativasCerradasConstruplan.Any(y => y.cc == x.cc)).ToList();

                var obraAdministrativaCerradaConstruplan = new NominaResumenDTO
                {
                    cc = "0-1",
                    descripcion = "OBRA CERRADA",
                    nomina = nominasObrasAdministrativasCerradasConstruplan.Sum(x => x.cargo - x.abono),
                    iva = nominasObrasAdministrativasCerradasConstruplan.Sum(x => x.iva),
                    total = nominasObrasAdministrativasCerradasConstruplan.Sum(x => (x.cargo - x.abono + x.iva)),
                    noEmpleado = (empleadosObrasAdministrativasCerradasConstruplan != null && empleadosObrasAdministrativasCerradasConstruplan.Count > 0) ? empleadosObrasAdministrativasCerradasConstruplan.Sum(x => x.noEmpleado) : 0,
                    noPracticante = (empleadosObrasAdministrativasCerradasConstruplan != null && empleadosObrasAdministrativasCerradasConstruplan.Count > 0) ? empleadosObrasAdministrativasCerradasConstruplan.Sum(x => x.noPracticante) : 0,
                    clase = "normal",
                    tipoCuenta = tipoCuentaNominaEnum.CONSTRUPLAN,
                    fecha_inicial = fechaInicio,
                    fecha_final = fechaFinal,
                    tipoNomina = (tipoNominaPropuestaEnum)tipoNomina,
                    division = "Cplan"
                };

                if(obraAdministrativaCerradaConstruplan.total > 0)
                {
                    listaNominasAdministrativasConstruplan.Add(obraAdministrativaCerradaConstruplan);
                }

                decimal totalNominaAdministrativoConstruplan = listaNominasAdministrativasConstruplan.Sum(x => x.nomina) + obraAdministrativaCerradaConstruplan.nomina;
                decimal totalIvaAdministrativoConstruplan = listaNominasAdministrativasConstruplan.Sum(x => x.iva) + obraAdministrativaCerradaConstruplan.iva;
                decimal totalAdministrativoConstruplan = (listaNominasAdministrativasConstruplan.Sum(x => x.total) + obraAdministrativaCerradaConstruplan.total);
                decimal totalEmpleadosAdministrativoConstruplan = listaNominasAdministrativasConstruplan.Sum(x => x.noEmpleado) + obraAdministrativaCerradaConstruplan.noEmpleado;
                int totalPracticantesAdministrativoConstruplan = listaNominasAdministrativasConstruplan.Sum(x => x.noPracticante) + obraAdministrativaCerradaConstruplan.noPracticante;


                if(totalAdministrativoConstruplan > 0)
                {
                    listaNominas.Add(new NominaResumenDTO
                    {
                        cc = "C.C.",
                        descripcion = "OBRAS CPLAN",
                        nomina = 0,
                        iva = 0,
                        total = 0,
                        noEmpleado = 0,
                        clase = "encabezadoTabla"
                    });

                    listaNominas.AddRange(listaNominasAdministrativasConstruplan);

                    listaNominas.Add(new NominaResumenDTO
                    {
                        cc = "ZZZ",
                        descripcion = "Total",
                        nomina = totalNominaAdministrativoConstruplan,
                        iva = totalIvaAdministrativoConstruplan,
                        total = totalAdministrativoConstruplan,
                        noEmpleado = totalEmpleadosAdministrativoConstruplan,
                        noPracticante = totalPracticantesAdministrativoConstruplan,
                        clase = "totalCuadro",
                        tipoCuenta = tipoCuentaNominaEnum.CONSTRUPLAN,
                        division = "Cplan"
                    });

                    listaNominas.Add(new NominaResumenDTO
                    {
                        cc = "ZZZ",
                        descripcion = "",
                        nomina = 0,
                        iva = 0,
                        total = 0,
                        noEmpleado = 0,
                        clase = "vacio"
                    });
                }


                // Industrial 
                var obrasIndustrialesConstruplan = listaObras.Where(x => x.bit_area == "2" || x.bit_area == "22").OrderBy(x => x.cc).ToList();
                var listaNominasIndustrialesConstruplan = new List<NominaResumenDTO>();
                foreach(var obraIndustrial in obrasIndustrialesConstruplan)
                {
                    tblC_NominaPoliza nomina = nominasActivasConstruplan.FirstOrDefault(x => x.cc == obraIndustrial.cc);
                    if(nomina != null)
                    {
                        var resumen = lstResumenCplan.FirstOrDefault(x => x.cc == nomina.cc);
                        listaNominasIndustrialesConstruplan.Add(new NominaResumenDTO
                        {
                            cc = nomina.cc,
                            descripcion = obraIndustrial.descripcion,
                            nomina = nomina.cargo - nomina.abono,
                            iva = nomina.iva,
                            total = nomina.cargo - nomina.abono + nomina.iva,
                            noEmpleado = resumen != null ? resumen.noEmpleado : 0,
                            noPracticante = resumen != null ? resumen.noPracticante : 0,
                            clase = "normal",
                            tipoCuenta = tipoCuentaNominaEnum.ProvisionServicioAdministrativos,
                            fecha_inicial = fechaInicio,
                            fecha_final = fechaFinal,
                            tipoNomina = (tipoNominaPropuestaEnum)tipoNomina,
                            division = "Cplan"
                        });
                    }
                }
                var obrasIndustrialesCerradasConstruplan = listaObras.Where(x => x.bit_area == "-2" || x.bit_area == "-22").OrderBy(x => x.cc).ToList();
                var nominasObrasIndustrialesCerradasConstruplan = nominasActivasConstruplan.Where(x => obrasIndustrialesCerradasConstruplan.Any(y => y.cc == x.cc)).ToList();
                var empleadosObrasIndustrialesCerradasConstruplan = lstResumenCplan.Where(x => nominasObrasIndustrialesCerradasConstruplan.Any(y => y.cc == x.cc)).ToList();

                var obraIndustrialCerradaConstruplan = new NominaResumenDTO
                {
                    cc = "0-2",
                    descripcion = "OBRA CERRADA",
                    nomina = nominasObrasIndustrialesCerradasConstruplan.Sum(x => x.cargo - x.abono),
                    iva = nominasObrasIndustrialesCerradasConstruplan.Sum(x => x.iva),
                    total = nominasObrasIndustrialesCerradasConstruplan.Sum(x => (x.cargo - x.abono + x.iva)),
                    noEmpleado = (empleadosObrasIndustrialesCerradasConstruplan != null && empleadosObrasIndustrialesCerradasConstruplan.Count > 0) ? empleadosObrasIndustrialesCerradasConstruplan.Sum(y => y.noEmpleado) : 0,
                    noPracticante = (empleadosObrasIndustrialesCerradasConstruplan != null && empleadosObrasIndustrialesCerradasConstruplan.Count > 0) ? empleadosObrasIndustrialesCerradasConstruplan.Sum(y => y.noPracticante) : 0,
                    clase = "normal",
                    tipoCuenta = tipoCuentaNominaEnum.CONSTRUPLAN,
                    fecha_inicial = fechaInicio,
                    fecha_final = fechaFinal,
                    tipoNomina = (tipoNominaPropuestaEnum)tipoNomina,
                    division = "Cplan"
                };

                if(obraIndustrialCerradaConstruplan.total > 0)
                {
                    listaNominasIndustrialesConstruplan.Add(obraIndustrialCerradaConstruplan);
                }

                decimal totalNominaIndustrialConstruplan = listaNominasIndustrialesConstruplan.Sum(x => x.nomina) + obraIndustrialCerradaConstruplan.nomina;
                decimal totalIvaIndustrialConstruplan = listaNominasIndustrialesConstruplan.Sum(x => x.iva) + obraIndustrialCerradaConstruplan.iva;
                decimal totalIndustrialConstruplan = (listaNominasIndustrialesConstruplan.Sum(x => x.total) + obraIndustrialCerradaConstruplan.total);
                decimal totalEmpleadosIndustrialConstruplan = listaNominasIndustrialesConstruplan.Sum(x => x.noEmpleado) + obraIndustrialCerradaConstruplan.noEmpleado;
                int totalPracticantesIndustrialConstruplan = listaNominasIndustrialesConstruplan.Sum(x => x.noPracticante) + obraIndustrialCerradaConstruplan.noPracticante;


                if(totalIndustrialConstruplan > 0)
                {
                    listaNominas.Add(new NominaResumenDTO
                    {
                        cc = "C.C.",
                        descripcion = "AREA INDUSTRIAL CPLAN",
                        nomina = 0,
                        iva = 0,
                        total = 0,
                        noEmpleado = 0,
                        clase = "encabezadoTabla"
                    });

                    listaNominas.AddRange(listaNominasIndustrialesConstruplan);

                    listaNominas.Add(new NominaResumenDTO
                    {
                        cc = "ZZZ",
                        descripcion = "Total",
                        nomina = totalNominaIndustrialConstruplan,
                        iva = totalIvaIndustrialConstruplan,
                        total = totalIndustrialConstruplan,
                        noEmpleado = totalEmpleadosIndustrialConstruplan,
                        noPracticante = totalPracticantesIndustrialConstruplan,
                        clase = "totalCuadro",
                        tipoCuenta = tipoCuentaNominaEnum.CONSTRUPLAN,
                        division = "Cplan"
                    });

                    listaNominas.Add(new NominaResumenDTO
                    {
                        cc = "ZZZ",
                        descripcion = "",
                        nomina = 0,
                        iva = 0,
                        total = 0,
                        noEmpleado = 0,
                        clase = "vacio"
                    });
                }

                // Obras individuales 
                var obrasIndividualesConstruplan = listaObras.Where(x =>
                    x.bit_area != "3" &&
                    x.bit_area != "-1" &&
                    x.bit_area != "2" &&
                    x.bit_area != "22" &&
                    x.bit_area != "-2" &&
                    x.bit_area != "-22")
                    .OrderBy(x => x.cc)
                    .ToList();

                var listaNominasIndividualesConstruplan = new List<NominaResumenDTO>();
                foreach(var obraIndividual in obrasIndividualesConstruplan)
                {
                    tblC_NominaPoliza nomina = nominasActivasConstruplan.FirstOrDefault(x => x.cc == obraIndividual.cc);
                    if(nomina != null)
                    {
                        var resumen = lstResumenCplan.FirstOrDefault(x => x.cc == nomina.cc);
                        listaNominasIndividualesConstruplan.Add(new NominaResumenDTO
                        {
                            cc = nomina.cc,
                            descripcion = obraIndividual.descripcion,
                            nomina = nomina.cargo - nomina.abono,
                            iva = nomina.iva,
                            total = nomina.cargo - nomina.abono + nomina.iva,
                            noEmpleado = resumen != null ? resumen.noEmpleado : 0,
                            noPracticante = resumen != null ? resumen.noPracticante : 0,
                            clase = "obraIndividual",
                            tipoCuenta = tipoCuentaNominaEnum.CONSTRUPLAN,
                            fecha_inicial = fechaInicio,
                            fecha_final = fechaFinal,
                            tipoNomina = (tipoNominaPropuestaEnum)tipoNomina,
                            division = "Cplan"
                        });
                    }
                }

                if(listaNominasIndividualesConstruplan.Count > 0)
                {
                    listaNominas.AddRange(listaNominasIndividualesConstruplan);
                }
                decimal totalNominaConstruplan = totalNominaAdministrativoConstruplan + totalNominaIndustrialConstruplan + listaNominasIndividualesConstruplan.Sum(x => x.nomina);
                decimal totalIvaConstruplan = totalIvaAdministrativoConstruplan + totalIvaIndustrialConstruplan + listaNominasIndividualesConstruplan.Sum(x => x.iva);
                decimal totalConstruplan = totalAdministrativoConstruplan + totalIndustrialConstruplan + listaNominasIndividualesConstruplan.Sum(x => x.total);
                decimal totalEmpleadosConstruplan = totalEmpleadosAdministrativoConstruplan + totalEmpleadosIndustrialConstruplan + listaNominasIndividualesConstruplan.Sum(x => x.noEmpleado);
                int totalPracticantesConstruplan = totalPracticantesAdministrativoConstruplan + totalPracticantesIndustrialConstruplan + listaNominasIndividualesConstruplan.Sum(x => x.noPracticante);

                listaNominas.Add(new NominaResumenDTO
                {
                    cc = "ZZZ",
                    descripcion = "TOTAL TRANSFERENCIA A CONSTRUPLAN",
                    nomina = totalNominaConstruplan,
                    iva = totalIvaConstruplan,
                    total = totalConstruplan,
                    noEmpleado = totalEmpleadosConstruplan,
                    noPracticante = totalPracticantesConstruplan,
                    clase = "totalCuenta CONSTRUPLAN",
                    tipoCuenta = tipoCuentaNominaEnum.CONSTRUPLAN,
                    division = "Cplan"
                });

                listaNominas.Add(new NominaResumenDTO
                {
                    cc = "ZZZ",
                    descripcion = "",
                    nomina = 0,
                    iva = 0,
                    total = 0,
                    noEmpleado = 0,
                    clase = "vacio"
                });
                #endregion

                #region Arrendadora
                var nominasActivasArrendadora = nominasActivas.Where(x => x.tipoCuenta == (int)tipoCuentaNominaEnum.Arrendadora).ToList();
                var obrasArrendadora = new CentroCostosDAO().getLstCcArrendadoraProd().OrderBy(o => o.Value);
                var listaNominasArrendadora = new List<NominaResumenDTO>();
                var lstResumenArrendadora = lstResumen.Where(x => x.tipoCuenta == (int)tipoCuentaNominaEnum.Arrendadora).ToList();
                listaNominasArrendadora.AddRange(nominasActivasArrendadora.GroupBy(g => g.cc).Select(nomina => new NominaResumenDTO()
                {
                    cc = nomina.Key,
                    descripcion = obrasArrendadora.FirstOrDefault(c => c.Value == nomina.Key).Text.Split('-')[1],
                    nomina = nomina.Sum(s => s.cargo - s.abono),
                    iva = nomina.Sum(s => s.iva),
                    total = nomina.Sum(s => s.cargo - s.abono + s.iva),
                    noEmpleado = lstResumenArrendadora.Where(w => w.cc == nomina.Key).Sum(s => s.noEmpleado),
                    noPracticante = lstResumenArrendadora.Where(w => w.cc == nomina.Key).Sum(s => s.noPracticante),
                    clase = "normal",
                    tipoCuenta = tipoCuentaNominaEnum.Arrendadora,
                    fecha_inicial = fechaInicio,
                    fecha_final = fechaFinal,
                    tipoNomina = (tipoNominaPropuestaEnum)tipoNomina,
                    division = "Arrendadora"
                }).ToList());

                decimal totalNominaArrendadora = listaNominasArrendadora.Sum(x => x.nomina);
                decimal totalIvaArrendadora = listaNominasArrendadora.Sum(x => x.iva);
                decimal totalArrendadora = listaNominasArrendadora.Sum(x => x.total);
                decimal totalEmpleadosArrendadora = listaNominasArrendadora.Sum(x => x.noEmpleado);
                int totalPracticantesArrendadora = listaNominasArrendadora.Sum(x => x.noPracticante);

                if(totalArrendadora > 0)
                {
                    listaNominas.Add(new NominaResumenDTO
                    {
                        cc = "C.C.",
                        descripcion = "ARRENDADORA CPLAN",
                        nomina = 0,
                        iva = 0,
                        total = 0,
                        noEmpleado = 0,
                        clase = "encabezadoTabla"
                    });

                    listaNominas.AddRange(listaNominasArrendadora);
                }

                listaNominas.Add(new NominaResumenDTO
                {
                    cc = "ZZZ",
                    descripcion = "TOTAL TRANSFERENCIA A ARRENDADORA CONSTRUPLAN",
                    nomina = totalNominaArrendadora,
                    iva = totalIvaArrendadora,
                    total = totalArrendadora,
                    noEmpleado = totalEmpleadosArrendadora,
                    noPracticante = totalPracticantesArrendadora,
                    clase = "totalCuenta " + tipoCuentaNominaEnum.Arrendadora.GetDescription(),
                    tipoCuenta = tipoCuentaNominaEnum.Arrendadora,
                    division = "Arrendadora"
                });

                listaNominas.Add(new NominaResumenDTO
                {
                    cc = "ZZZ",
                    descripcion = "",
                    nomina = 0,
                    iva = 0,
                    total = 0,
                    noEmpleado = 0,
                    clase = "vacio"
                });

                listaNominas.Add(new NominaResumenDTO
                {
                    cc = "ZZZ",
                    descripcion = "",
                    nomina = 0,
                    iva = 0,
                    total = 0,
                    noEmpleado = 0,
                    clase = "vacio"
                });

                listaNominas.Add(new NominaResumenDTO
                {
                    cc = "ZZZ",
                    descripcion = String.Format("TOTAL GENERAL DEL {0:00}-{1:00} DE {2} {3}.", fechaInicio.Day, fechaFinal.Day, fechaFinal.ToString("MMMMM").ToUpper(), fechaFinal.Year),
                    nomina = totalNominaRavelio + totalNominaConstruplan + totalNominaArrendadora,
                    iva = totalIvaRavelio + totalIvaConstruplan + totalIvaArrendadora,
                    total = totalRavelio + totalConstruplan + totalArrendadora,
                    noEmpleado = totalEmpleadosRavelio + totalEmpleadosConstruplan + totalEmpleadosArrendadora,
                    noPracticante = totalPracticantesRavelio + totalPracticantesConstruplan + totalPracticantesArrendadora,
                    clase = "totalGeneral",
                    division = "total"
                });

                resultado.Add("listaNominas", listaNominas);
                #endregion

                resultado.Add(SUCCESS, true);
            }
            catch(Exception)
            {
                resultado.Add(SUCCESS, false);
            }
            return resultado;
        }

        public Dictionary<string, object> GetNominasOtros(DateTime fechaInicio, DateTime fechaFinal)
        {
            var resultado = new Dictionary<string, object>();
            try
            {
                List<tblC_NominaPoliza> nominasActivas = getLstPolizaNominaActiva(fechaInicio, fechaFinal)
                    .Where(x =>
                        x.tipoNomina != (int)tipoNominaPropuestaEnum.NA &&
                        x.tipoNomina != (int)tipoNominaPropuestaEnum.Semanal &&
                        x.tipoNomina != (int)tipoNominaPropuestaEnum.Quincenal)
                        .ToList();

                var listaObras = new CadenaProductivaDAO().lstObra();

                var listaNominas = new List<NominaResumenDTO>();

                // Administrativo
                var obrasAdministrativas = listaObras.Where(x => x.bit_area == "3").OrderBy(x => x.cc).ToList();

                // Cerradas Administrativas
                var obrasAdministrativasCerradas = listaObras.Where(x => x.bit_area == "-1").OrderBy(x => x.cc).ToList();

                // Industrial
                var obrasIndustriales = listaObras.Where(x => x.bit_area == "2" || x.bit_area == "22").OrderBy(x => x.cc).ToList();

                // Cerradas Industriales
                var obrasIndustrialesCerradas = listaObras.Where(x => x.bit_area == "-2" || x.bit_area == "-22").OrderBy(x => x.cc).ToList();

                var obrasIndividuales = listaObras
                    .Where(x =>
                        x.bit_area != "3" && x.bit_area != "-1" &&
                        x.bit_area != "2" && x.bit_area != "22" &&
                        x.bit_area != "-2" && x.bit_area != "-22")
                        .OrderBy(x => x.cc)
                        .ToList();

                // Arrendadora


                var listaCuentasCplan = new List<int> { 
                    (int)tipoCuentaNominaEnum.ConstructoraRavelio, 
                    (int)tipoCuentaNominaEnum.ProvisionServicioAdministrativos, 
                    (int)tipoCuentaNominaEnum.ServiciosAdministrativosComplementaria,
                    (int)tipoCuentaNominaEnum.SONMONT,
                    (int)tipoCuentaNominaEnum.REGFORTE,
                    (int)tipoCuentaNominaEnum.CONSTRUPLAN
                };


                ///////////////////////////////////////////////////////////// GROUP BY
                nominasActivas.GroupBy(otros => new { otros.tipoCuenta, otros.tipoNomina }).OrderByDescending(x => x.Key.tipoCuenta).ThenBy(x => x.Key.tipoNomina).ToList().ForEach(otros =>
                {
                    var listaNominasAdministrativas = new List<NominaResumenDTO>();
                    var listaNominasIndustriales = new List<NominaResumenDTO>();
                    var listaNominasIndividuales = new List<NominaResumenDTO>();
                    var listaNominasArrendadora = new List<NominaResumenDTO>();

                    // Administrativo
                    otros.Where(nomina =>
                            obrasAdministrativas.Any(obra => obra.cc == nomina.cc) &&
                            listaCuentasCplan.Contains(nomina.tipoCuenta))
                        .GroupBy(x => x.cc).ToList().ForEach(nomina =>
                        {
                            var obra = obrasAdministrativas.First(x => x.cc == nomina.Key);
                            if(nomina.Sum(x => x.cargo - x.abono) > 0)
                            {
                                listaNominasAdministrativas.Add(new NominaResumenDTO
                                {
                                    cc = nomina.Key,
                                    descripcion = obra.descripcion,
                                    nomina = nomina.Sum(x => x.cargo - x.abono),
                                    iva = nomina.Sum(x => x.iva),
                                    retencion = nomina.Sum(x => x.retencion),
                                    total = nomina.Sum(x => x.cargo - x.abono + x.iva - x.retencion),
                                    clase = "normal",
                                    tipoCuenta = (tipoCuentaNominaEnum)otros.Key.tipoCuenta,
                                    fecha_inicial = fechaInicio,
                                    fecha_final = fechaFinal,
                                    tipoNomina = (tipoNominaPropuestaEnum)otros.Key.tipoNomina,
                                    descripcionCuenta = ((tipoCuentaNominaEnum)otros.Key.tipoCuenta).GetDescription(),
                                    descripcionNomina = ((tipoNominaPropuestaEnum)otros.Key.tipoNomina).GetDescription()
                                });
                            }
                        });

                    // Cerradas Administrativas
                    otros.Where(nomina =>
                                    obrasAdministrativasCerradas.Any(obra => obra.cc == nomina.cc) &&
                                    listaCuentasCplan.Contains(nomina.tipoCuenta))
                        .GroupBy(x => x.cc).ToList().ForEach(nomina =>
                        {
                            if(nomina.Sum(x => x.cargo - x.abono) > 0)
                            {
                                listaNominasAdministrativas.Add(new NominaResumenDTO
                                 {
                                     cc = "0-1",
                                     descripcion = "OBRA CERRADA ",
                                     nomina = nomina.Sum(x => x.cargo - x.abono),
                                     iva = nomina.Sum(x => x.iva),
                                     retencion = nomina.Sum(x => x.retencion),
                                     total = nomina.Sum(x => x.cargo - x.abono + x.iva - x.retencion),
                                     clase = "normal",
                                     tipoCuenta = (tipoCuentaNominaEnum)otros.Key.tipoCuenta,
                                     fecha_inicial = fechaInicio,
                                     fecha_final = fechaFinal,
                                     tipoNomina = (tipoNominaPropuestaEnum)otros.Key.tipoNomina,
                                     descripcionCuenta = ((tipoCuentaNominaEnum)otros.Key.tipoCuenta).GetDescription(),
                                     descripcionNomina = ((tipoNominaPropuestaEnum)otros.Key.tipoNomina).GetDescription()
                                 });
                            }
                        });

                    if(listaNominasAdministrativas.Count > 0)
                    {
                        listaNominasAdministrativas.Add(new NominaResumenDTO
                        {
                            cc = "ZZZ",
                            descripcion = "TOTAL " + ((tipoNominaPropuestaEnum)otros.Key.tipoNomina).GetDescription(),
                            nomina = listaNominasAdministrativas.Sum(x => x.nomina),
                            iva = listaNominasAdministrativas.Sum(x => x.iva),
                            retencion = listaNominasAdministrativas.Sum(x => x.retencion),
                            total = listaNominasAdministrativas.Sum(x => x.total),
                            clase = "totalCuadro",
                            descripcionCuenta = ((tipoCuentaNominaEnum)otros.Key.tipoCuenta).GetDescription(),
                            descripcionNomina = ((tipoNominaPropuestaEnum)otros.Key.tipoNomina).GetDescription()
                        });

                        listaNominasAdministrativas.Add(new NominaResumenDTO
                        {
                            cc = "ZZZ",
                            descripcion = "",
                            clase = "vacio",
                            descripcionCuenta = ((tipoCuentaNominaEnum)otros.Key.tipoCuenta).GetDescription(),
                            descripcionNomina = ((tipoNominaPropuestaEnum)otros.Key.tipoNomina).GetDescription()
                        });

                        listaNominas.AddRange(listaNominasAdministrativas);
                    }

                    // Industrial
                    otros.Where(nomina =>
                            obrasIndustriales.Any(obra => obra.cc == nomina.cc) &&
                            listaCuentasCplan.Any(x => nomina.tipoCuenta == x))
                        .GroupBy(x => x.cc).ToList().ForEach(nomina =>
                        {
                            var obra = obrasIndustriales.First(x => x.cc == nomina.Key);
                            if(nomina.Sum(x => x.cargo - x.abono) > 0)
                            {
                                listaNominasIndustriales.Add(new NominaResumenDTO
                                {
                                    cc = nomina.Key,
                                    descripcion = obra.descripcion,
                                    nomina = nomina.Sum(x => x.cargo - x.abono),
                                    iva = nomina.Sum(x => x.iva),
                                    retencion = nomina.Sum(x => x.retencion),
                                    total = nomina.Sum(x => x.cargo - x.abono + x.iva - x.retencion),
                                    clase = "normal",
                                    tipoCuenta = (tipoCuentaNominaEnum)otros.Key.tipoCuenta,
                                    fecha_inicial = fechaInicio,
                                    fecha_final = fechaFinal,
                                    tipoNomina = (tipoNominaPropuestaEnum)otros.Key.tipoNomina,
                                    descripcionCuenta = ((tipoCuentaNominaEnum)otros.Key.tipoCuenta).GetDescription(),
                                    descripcionNomina = ((tipoNominaPropuestaEnum)otros.Key.tipoNomina).GetDescription()
                                });
                            }
                        });

                    // Cerradas Industriales
                    otros.Where(nomina =>
                                    obrasAdministrativasCerradas.Any(obra => obra.cc == nomina.cc) &&
                                    listaCuentasCplan.Any(obra => nomina.tipoCuenta == obra))
                        .GroupBy(x => x.cc).ToList().ForEach(nomina =>
                        {
                            if(nomina.Sum(x => x.cargo - x.abono) > 0)
                            {
                                var obra = obrasAdministrativasCerradas.First(x => x.cc == nomina.Key);
                                listaNominasIndustriales.Add(new NominaResumenDTO
                                {
                                    cc = "0-2",
                                    descripcion = "OBRA CERRADA ",
                                    nomina = nomina.Sum(x => x.cargo - x.abono),
                                    iva = nomina.Sum(x => x.iva),
                                    retencion = nomina.Sum(x => x.retencion),
                                    total = nomina.Sum(x => x.cargo - x.abono + x.iva - x.retencion),
                                    clase = "normal",
                                    tipoCuenta = (tipoCuentaNominaEnum)otros.Key.tipoCuenta,
                                    fecha_inicial = fechaInicio,
                                    fecha_final = fechaFinal,
                                    tipoNomina = (tipoNominaPropuestaEnum)otros.Key.tipoNomina,
                                    descripcionCuenta = ((tipoCuentaNominaEnum)otros.Key.tipoCuenta).GetDescription(),
                                    descripcionNomina = ((tipoNominaPropuestaEnum)otros.Key.tipoNomina).GetDescription()
                                });
                            }
                        });

                    if(listaNominasIndustriales.Count > 0)
                    {
                        listaNominas.Add(new NominaResumenDTO
                        {
                            cc = "C.C.",
                            descripcion = "AREA INDUSTRIAL",
                            nomina = 0,
                            iva = 0,
                            total = 0,
                            noEmpleado = 0,
                            clase = "encabezadoTabla",
                            descripcionCuenta = ((tipoCuentaNominaEnum)otros.Key.tipoCuenta).GetDescription(),
                            descripcionNomina = ((tipoNominaPropuestaEnum)otros.Key.tipoNomina).GetDescription()
                        });

                        listaNominasIndustriales.Add(new NominaResumenDTO
                        {
                            cc = "ZZZ",
                            descripcion = "TOTALES " + ((tipoNominaPropuestaEnum)otros.Key.tipoNomina).GetDescription(),
                            nomina = listaNominasIndustriales.Sum(x => x.nomina),
                            iva = listaNominasIndustriales.Sum(x => x.iva),
                            retencion = listaNominasIndustriales.Sum(x => x.retencion),
                            total = listaNominasIndustriales.Sum(x => x.total),
                            clase = "totalCuadro",
                            descripcionCuenta = ((tipoCuentaNominaEnum)otros.Key.tipoCuenta).GetDescription(),
                            descripcionNomina = ((tipoNominaPropuestaEnum)otros.Key.tipoNomina).GetDescription()
                        });

                        listaNominasIndustriales.Add(new NominaResumenDTO
                        {
                            cc = "ZZZ",
                            descripcion = "",
                            clase = "vacio",
                            descripcionCuenta = ((tipoCuentaNominaEnum)otros.Key.tipoCuenta).GetDescription(),
                            descripcionNomina = ((tipoNominaPropuestaEnum)otros.Key.tipoNomina).GetDescription()
                        });

                        listaNominas.AddRange(listaNominasIndustriales);
                    }

                    // Obras Individuales
                    otros.Where(nomina =>
                            obrasIndividuales.Any(obra => obra.cc == nomina.cc) &&
                            listaCuentasCplan.Any(x => nomina.tipoCuenta == x))
                        .GroupBy(x => x.cc).ToList().ForEach(nomina =>
                        {
                            var obra = obrasIndividuales.First(x => x.cc == nomina.Key);
                            if(nomina.Sum(x => x.cargo - x.abono) > 0)
                            {
                                listaNominasIndividuales.Add(new NominaResumenDTO
                                {
                                    cc = nomina.Key,
                                    descripcion = obra.descripcion,
                                    nomina = nomina.Sum(x => x.cargo - x.abono),
                                    iva = nomina.Sum(x => x.iva),
                                    retencion = nomina.Sum(x => x.retencion),
                                    total = nomina.Sum(x => x.cargo - x.abono + x.iva - x.retencion),
                                    clase = "obraIndividual",
                                    tipoCuenta = (tipoCuentaNominaEnum)otros.Key.tipoCuenta,
                                    fecha_inicial = fechaInicio,
                                    fecha_final = fechaFinal,
                                    tipoNomina = (tipoNominaPropuestaEnum)otros.Key.tipoNomina,
                                    descripcionCuenta = ((tipoCuentaNominaEnum)otros.Key.tipoCuenta).GetDescription(),
                                    descripcionNomina = ((tipoNominaPropuestaEnum)otros.Key.tipoNomina).GetDescription()
                                });
                            }
                        });

                    if(listaNominasIndividuales.Count > 0)
                    {
                        listaNominas.AddRange(listaNominasIndividuales);
                    }

                    // total cuenta
                    if(listaNominasAdministrativas.Count > 0 || listaNominasIndustriales.Count > 0 || listaNominasIndividuales.Count > 0)
                    {
                        listaNominas.Add(new NominaResumenDTO
                        {
                            cc = "zzz",
                            descripcion = "TOTAL TRANSFERENCIA A " + ((tipoCuentaNominaEnum)otros.Key.tipoCuenta).GetDescription() + " POR " + ((tipoNominaPropuestaEnum)otros.Key.tipoNomina).GetDescription(),
                            nomina = listaNominasAdministrativas.Sum(x => x.nomina) + listaNominasIndustriales.Sum(x => x.nomina) + listaNominasIndividuales.Sum(x => x.nomina),
                            iva = listaNominasAdministrativas.Sum(x => x.iva) + listaNominasIndustriales.Sum(x => x.iva) + listaNominasIndividuales.Sum(x => x.iva),
                            retencion = listaNominasAdministrativas.Sum(x => x.retencion) + listaNominasIndustriales.Sum(x => x.retencion) + listaNominasIndividuales.Sum(x => x.retencion),
                            total = listaNominasAdministrativas.Sum(x => x.total) + listaNominasIndustriales.Sum(x => x.total) + listaNominasIndividuales.Sum(x => x.total),
                            clase = String.Format("totalCuenta {0}", ((tipoCuentaNominaEnum)otros.Key.tipoCuenta == tipoCuentaNominaEnum.ConstructoraRavelio) ? "RAVELIO" : "CONSTRUPLAN"),
                            descripcionCuenta = ((tipoCuentaNominaEnum)otros.Key.tipoCuenta).GetDescription(),
                            descripcionNomina = ((tipoNominaPropuestaEnum)otros.Key.tipoNomina).GetDescription()
                        });

                        listaNominas.Add(new NominaResumenDTO
                        {
                            cc = "zzz",
                            descripcion = "",
                            clase = "vacio",
                            descripcionCuenta = ((tipoCuentaNominaEnum)otros.Key.tipoCuenta).GetDescription(),
                            descripcionNomina = ((tipoNominaPropuestaEnum)otros.Key.tipoNomina).GetDescription()
                        });
                    }

                    // Si es arrendadora
                    if(otros.Key.tipoCuenta == (int)tipoCuentaNominaEnum.Arrendadora)
                    {
                        otros.Where(nomina =>
                                obrasAdministrativas.Any(obra => obra.cc == nomina.cc) &&
                                nomina.tipoCuenta == otros.Key.tipoCuenta)
                            .GroupBy(x => x.cc).ToList().ForEach(nomina =>
                            {
                                var obra = obrasAdministrativas.First(x => x.cc == nomina.Key);
                                if(nomina.Sum(x => x.cargo - x.abono) > 0)
                                {
                                    listaNominasArrendadora.Add(new NominaResumenDTO
                                    {
                                        cc = nomina.Key,
                                        descripcion = obra.descripcion,
                                        nomina = nomina.Sum(x => x.cargo - x.abono),
                                        iva = nomina.Sum(x => x.iva),
                                        retencion = nomina.Sum(x => x.retencion),
                                        total = nomina.Sum(x => x.cargo - x.abono + x.iva - x.retencion),
                                        clase = "normal",
                                        tipoCuenta = (tipoCuentaNominaEnum)otros.Key.tipoCuenta,
                                        fecha_inicial = fechaInicio,
                                        fecha_final = fechaFinal,
                                        tipoNomina = (tipoNominaPropuestaEnum)otros.Key.tipoNomina,
                                        descripcionCuenta = ((tipoCuentaNominaEnum)otros.Key.tipoCuenta).GetDescription(),
                                        descripcionNomina = ((tipoNominaPropuestaEnum)otros.Key.tipoNomina).GetDescription()
                                    });
                                }
                            });

                        if(listaNominasArrendadora.Count > 0)
                        {
                            listaNominas.Add(new NominaResumenDTO
                            {
                                cc = "C.C.",
                                descripcion = "ARRENDADORA CPLAN",
                                nomina = 0,
                                iva = 0,
                                total = 0,
                                noEmpleado = 0,
                                clase = "encabezadoTabla",
                                descripcionCuenta = ((tipoCuentaNominaEnum)otros.Key.tipoCuenta).GetDescription(),
                                descripcionNomina = ((tipoNominaPropuestaEnum)otros.Key.tipoNomina).GetDescription()
                            });

                            listaNominasArrendadora.Add(new NominaResumenDTO
                            {
                                cc = "ZZZ",
                                descripcion = "TOTALES " + ((tipoNominaPropuestaEnum)otros.Key.tipoNomina).GetDescription(),
                                nomina = listaNominasArrendadora.Sum(x => x.nomina),
                                iva = listaNominasArrendadora.Sum(x => x.iva),
                                retencion = listaNominasArrendadora.Sum(x => x.retencion),
                                total = listaNominasArrendadora.Sum(x => x.total),
                                clase = "totalCuadro",
                                descripcionCuenta = ((tipoCuentaNominaEnum)otros.Key.tipoCuenta).GetDescription(),
                                descripcionNomina = ((tipoNominaPropuestaEnum)otros.Key.tipoNomina).GetDescription()
                            });

                            listaNominasArrendadora.Add(new NominaResumenDTO
                            {
                                cc = "ZZZ",
                                descripcion = "",
                                clase = "vacio",
                                descripcionCuenta = ((tipoCuentaNominaEnum)otros.Key.tipoCuenta).GetDescription(),
                                descripcionNomina = ((tipoNominaPropuestaEnum)otros.Key.tipoNomina).GetDescription()
                            });

                            listaNominas.AddRange(listaNominasArrendadora);

                            listaNominas.Add(new NominaResumenDTO
                            {
                                cc = "zzz",
                                descripcion = "TOTAL TRANSFERENCIA A " + ((tipoCuentaNominaEnum)otros.Key.tipoCuenta).GetDescription() + " POR " + ((tipoNominaPropuestaEnum)otros.Key.tipoNomina).GetDescription(),
                                nomina = listaNominasArrendadora.Sum(x => x.nomina),
                                iva = listaNominasArrendadora.Sum(x => x.iva),
                                retencion = listaNominasArrendadora.Sum(x => x.retencion),
                                total = listaNominasArrendadora.Sum(x => x.total),
                                clase = "totalCuenta ARRENDADORA",
                                descripcionCuenta = ((tipoCuentaNominaEnum)otros.Key.tipoCuenta).GetDescription(),
                                descripcionNomina = ((tipoNominaPropuestaEnum)otros.Key.tipoNomina).GetDescription()
                            });

                            listaNominas.Add(new NominaResumenDTO
                            {
                                cc = "zzz",
                                descripcion = "",
                                clase = "vacio",
                                descripcionCuenta = ((tipoCuentaNominaEnum)otros.Key.tipoCuenta).GetDescription(),
                                descripcionNomina = ((tipoNominaPropuestaEnum)otros.Key.tipoNomina).GetDescription()
                            });
                        }
                    }
                });
                resultado.Add("listaNominas", listaNominas);
                resultado.Add(SUCCESS, true);
            }
            catch(Exception)
            {
                resultado.Add(SUCCESS, false);
            }


            return resultado;
        }
        #endregion
        #region Consulta Enkontrol
        public List<NominaPolizaDTO> getLstPolizaNomina(DateTime fecha_inicio, DateTime fecha_fin)
        {
            try
            {
                var empresa = vSesiones.sesionEmpresaActual;
                var parametros = parametrosLstPolizaNomina(fecha_inicio, fecha_fin);
                List<NominaPolizaDTO> lst = new List<NominaPolizaDTO>();
                switch (empresa) 
                {
                    case 1:
                        var cplan = new OdbcConsultaDTO()
                        {
                            consulta = queryLstPolizaNominaCplan(),
                            parametros = parametros
                        };
                        lst = _contextEnkontrol.Select<NominaPolizaDTO>(EnkontrolEnum.CplanProd, cplan);
                        break;
                    case 2:
                        var arrend = new OdbcConsultaDTO()
                        {
                            consulta = queryLstPolizaNominaArrend(),
                            parametros = parametros
                        };
                        lst = _contextEnkontrol.Select<NominaPolizaDTO>(EnkontrolEnum.ArrenProd, arrend);
                        break;
                    case 3:
                        break;
                    case 4:
                        var eici = new OdbcConsultaDTO()
                        {
                            consulta = queryLstPolizaNominaEICI(),
                            parametros = parametros
                        };
                        lst = _contextEnkontrol.Select<NominaPolizaDTO>(EnkontrolEnum.CplanEici, eici);
                        break;
                    case 5:
                        var integradora = new OdbcConsultaDTO()
                        {
                            consulta = queryLstPolizaNominaIntegradora(),
                            parametros = parametros
                        };
                        lst = _contextEnkontrol.Select<NominaPolizaDTO>(EnkontrolEnum.CplanIntegradora, integradora);
                        break;
                    default:
                        break;
                }
                return lst;
            }
            catch(Exception) { return new List<NominaPolizaDTO>(); }
        }
        string queryLstPolizaNominaCplan()
        {
            return string.Format(@"
                SELECT 
                    mov.year, mov.mes, mov.tp, mov.poliza, mov.linea, mov.sscta, mov.tm, mov.cc, mov.concepto, mov.monto, mov.referencia, pol.fechapol, 0 AS retencion 
                FROM 
                    (select year, mes, tp, poliza, linea, sscta, tm, cc, concepto, monto, referencia from sc_movpol where cta = 2110 and scta = 1 and tm = 2 and tp = '03' AND sscta in (2721,2720,2762,2821,2820,2710) ) mov 
                LEFT JOIN 
                    sc_polizas pol 
                ON 
                    pol.year = mov.year 
                    AND pol.mes = mov.mes 
                    AND pol.tp = mov.tp 
                    AND pol.poliza = mov.poliza
                    AND pol.fechapol BETWEEN ? AND ?
                    AND pol.status = 'A' 
                    AND pol.tp = '03' 
                WHERE pol.year IS NOT NULL");
        }
        string queryLstPolizaNominaArrend()
        {
            return string.Format(@"
                SELECT 
                    mov.year, mov.mes, mov.tp, mov.poliza, mov.linea, mov.sscta, mov.tm, mov.cc, mov.concepto, mov.monto, mov.referencia, pol.fechapol, 0 AS retencion 
                FROM 
                    (select year, mes, tp, poliza, linea, sscta, tm, cc, concepto, monto, referencia from sc_movpol where cta = 2110 and scta = 1 and tm = 2 and tp = '03' AND sscta in (2710, 2793)) mov 
                LEFT JOIN 
                    sc_polizas pol 
                ON 
                    pol.year = mov.year 
                    AND pol.mes = mov.mes 
                    AND pol.tp = mov.tp 
                    AND pol.poliza = mov.poliza
                    AND pol.fechapol BETWEEN ? AND ? 
                    AND pol.status = 'A' 
                    AND pol.tp = '03' 
                WHERE pol.year IS NOT NULL");
        }
        //TRONSET ANTES sscta 2780
        string queryLstPolizaNominaEICI()
        {
            return string.Format(@"                
                SELECT 
                    mov.year, mov.mes, mov.tp, mov.poliza, mov.linea, mov.sscta, mov.tm, mov.cc, mov.concepto, mov.monto, mov.referencia, pol.fechapol, 0 AS retencion 
                FROM 
                    (select year, mes, tp, poliza, linea, sscta, tm, cc, concepto, monto, referencia from sc_movpol where cta = 2110 and scta = 1 and tm = 2 and tp = '03' AND sscta in (2710, 2720)) mov 
                LEFT JOIN 
                    sc_polizas pol 
                ON 
                    pol.year = mov.year 
                    AND pol.mes = mov.mes 
                    AND pol.tp = mov.tp 
                    AND pol.poliza = mov.poliza
                    AND pol.fechapol BETWEEN ? AND ? 
                    AND pol.status = 'A' 
                    AND pol.tp = '03' 
                WHERE pol.year IS NOT NULL");
        }
        string queryLstPolizaNominaIntegradora()
        {
            return string.Format(@"
                SELECT 
                    mov.year, mov.mes, mov.tp, mov.poliza, mov.linea, mov.sscta, mov.tm, mov.cc, mov.concepto, mov.monto, mov.referencia, pol.fechapol, 0 AS retencion 
                FROM 
                    (select year, mes, tp, poliza, linea, sscta, tm, cc, concepto, monto, referencia from sc_movpol where cta = 2110 and scta = 1 and tm = 2 and tp = '03' AND sscta in (2)) mov 
                LEFT JOIN 
                    sc_polizas pol 
                ON 
                    pol.year = mov.year 
                    AND pol.mes = mov.mes 
                    AND pol.tp = mov.tp 
                    AND pol.poliza = mov.poliza
                    AND pol.fechapol BETWEEN ? AND ? 
                    AND pol.status = 'A' 
                    AND pol.tp = '03' 
                WHERE pol.year IS NOT NULL");
        }
        List<OdbcParameterDTO> parametrosLstPolizaNomina(DateTime fecha_inicio, DateTime fecha_fin)
        {
            var parameters = new List<OdbcParameterDTO>();
            parameters.Add(new OdbcParameterDTO() { nombre = "fechapol", tipo = OdbcType.Date, valor = fecha_inicio });
            parameters.Add(new OdbcParameterDTO() { nombre = "fechapol", tipo = OdbcType.Date, valor = fecha_fin });
            return parameters;
        }
        public List<PeriodosNominaDTO> getLstPeriodoNomina()
        {
            try
            {
                //const int meses = 6;
                var ahora = DateTime.Now;
                //var consultas = new List<OdbcConsultaDTO>();
                //for(int i = 0; i < meses; i++)
                //{
                //    consultas.Add(new OdbcConsultaDTO()
                //   {
                //        consulta = queryPeriodoNomina(),
                //        parametros = parametrosPeriodoNomina(ahora)
                //    });
                //    ahora = ahora.AddMonths(-1);
                //}
                //var lst = _contextEnkontrol.Select<PeriodosNominaDTO>(EnkontrolEnum.CplanRh, consultas);
                List<PeriodosNominaDTO> lst = _context.tblRH_EK_Periodos.Where(x => x.year == ahora.Year).Select(x => new PeriodosNominaDTO { 
                    tipo_nomina = x.tipo_nomina,
                    periodo = x.periodo,
                    tipo_periodo = x.tipo_periodo,
                    fecha_inicial = x.fecha_inicial,
                    fecha_final = x.fecha_final,
                    fecha_pago = x.fecha_pago,
                    mes_cc = x.mes_cc,
                    year = x.year,
                }).ToList();
                return lst;
            }
            catch(Exception) { return new List<PeriodosNominaDTO>(); }
        }
        public List<PeriodosNominaDTO> getLstPeriodoNominaAnt()
        {
            try
            {
                var odbc = new OdbcConsultaDTO()
                {
                    consulta = queryPeriodoNomina(),
                    parametros = parametrosPeriodoNominaAnt()
                };
                var lst = _contextEnkontrol.Select<PeriodosNominaDTO>(EnkontrolEnum.CplanRh, odbc);
                return lst;
            }
            catch(Exception) { return new List<PeriodosNominaDTO>(); }
        }
        string queryPeriodoNomina()
        {
            return string.Format(@"SELECT tipo_nomina, periodo, tipo_periodo, fecha_inicial, fecha_final, mes_cc, fecha_pago, year
                FROM sn_periodos 
                WHERE year = ? AND mes_cc = ?");
        }
        List<OdbcParameterDTO> parametrosPeriodoNomina(DateTime ahora)
        {
            var parameters = new List<OdbcParameterDTO>();
            parameters.Add(new OdbcParameterDTO() { nombre = "year", tipo = OdbcType.Numeric, valor = ahora.Year });
            parameters.Add(new OdbcParameterDTO() { nombre = "mes_cc", tipo = OdbcType.Numeric, valor = ahora.Month });
            return parameters;
        }
        List<OdbcParameterDTO> parametrosPeriodoNominaAnt()
        {
            var parameters = new List<OdbcParameterDTO>();
            var ahora = DateTime.Now;
            ahora.AddMonths(-1);
            parameters.Add(new OdbcParameterDTO() { nombre = "year", tipo = OdbcType.Numeric, valor = ahora.Year });
            parameters.Add(new OdbcParameterDTO() { nombre = "mes_cc", tipo = OdbcType.Numeric, valor = ahora.Month });
            return parameters;
        }
        public List<PeriodosNominaDTO> getLstSig4Semanas(DateTime fecha)
        {
            try
            {
                var odbc = new OdbcConsultaDTO()
                {
                    consulta = queryLstSig4Semanas(),
                    parametros = new List<OdbcParameterDTO>() { new OdbcParameterDTO() { nombre = "fecha_inicial", tipo = OdbcType.Date, valor = fecha } }
                };
                var lst = _contextEnkontrol.Select<PeriodosNominaDTO>(EnkontrolEnum.CplanRh, odbc).Take(4).ToList();
                return lst;
            }
            catch(Exception) { return new List<PeriodosNominaDTO>(); }
        }
        string queryLstSig4Semanas()
        {
            return @"SELECT tipo_nomina, periodo, tipo_periodo, fecha_inicial, fecha_final, mes_cc, fecha_pago
                        FROM sn_periodos 
                        WHERE tipo_periodo = 1 AND fecha_inicial >= ?";
        }
        #endregion
    }
}
