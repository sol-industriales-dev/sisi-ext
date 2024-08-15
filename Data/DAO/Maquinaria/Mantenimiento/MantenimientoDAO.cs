using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.DAO.Maquinaria.Mantenimiento;
using Core.Entity.Maquinaria.Mantenimiento;
using Data.EntityFramework.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Core.Objects;
using Core.Entity.Maquinaria.Catalogo;
using Core.Entity.Maquinaria;
using Core.Entity.Maquinaria.Captura;
using Core.Entity.RecursosHumanos.Catalogo;
using Data.EntityFramework.Context;
using Core.Enum.Principal.Bitacoras;
using Core.Enum.Maquinaria.Mantenimiento;
using Core.DTO.Maquinaria.Mantenimiento;
using Core.Entity.Principal.Usuarios;
using Core.DTO.RecursosHumanos;
using Core.DTO;
using Core.DTO.Maquinaria.Mantenimiento.DTO2._0;
using Infrastructure.Utils;
using Core.Enum.Multiempresa;
using Core.DTO.Utils.Data;
using System.Data.Odbc;
using Core.DTO.Principal.Generales;
using Core.Enum.Principal;
using System.IO;
using System.Web;
using OfficeOpenXml;
using System.Data.Entity;
using OfficeOpenXml.Style;
using System.Drawing;
using OfficeOpenXml.ConditionalFormatting;
using System.Drawing.Imaging;
namespace Data.DAO.Maquinaria.Mantenimiento
{
    public class MantenimientoDAO : GenericDAO<tblM_CatPM>, IMantenimientoDAO
    {
        private readonly string RutaBase = @"\\REPOSITORIO\Proyecto\SIGOPLAN\MAQUINARIA\MANTENIMIENTOPM";
        private const string RutaLocal = @"C:\Proyecto\SIGOPLAN\MAQUINARIA\MANTENIMIENTOPM";
        private const string _NOMBRE_CONTROLADOR = "MantenimientoController";
        private const int _SISTEMA = (int)SistemasEnum.MAQUINARIA;

        public tblM_BitacoraActividadesMantProy getActividadesProByID(int id)
        {
            return _context.tblM_BitacoraActividadesMantProy.FirstOrDefault(x => x.id == id);
        }
        public bool ActualizarActividadProgramada(tblM_BitacoraActividadesMantProy actividad)
        {
            try
            {
                var actividadActualizar = _context.tblM_BitacoraActividadesMantProy.FirstOrDefault(x => x.id == actividad.id);
                actividadActualizar.estatus = actividad.estatus;
                actividadActualizar.aplicar = actividad.aplicar;
                _context.SaveChanges();
                return true;
            }
            catch (Exception w)
            {
                return false;
            }
        }
        public tblM_BitacoraControlAceiteMant getBitHisLubByid(int id)
        {

            return _context.tblM_BitacoraControlAceiteMant.FirstOrDefault(x => x.id == id);
        }

        public List<tblM_CatFiltroMant> FillCboCatFiltros()
        {
            return _context.tblM_CatFiltroMant.ToList();
        }

        public List<ComboDTO> GetInsumoEnkontrol(List<string> codigos)
        {
            try
            {
                var obj = new OdbcConsultaDTO()
                {
                    consulta = string.Format(@"SELECT insumo as Value, SUBSTRING(descripcion, 0, CHARINDEX(' ', descripcion) - 1) as Text  FROM insumos where SUBSTRING(descripcion, 0, CHARINDEX(' ', descripcion) - 1) in {0}", codigos.ToParamInValue()),
                    parametros = parametrosCodigos(codigos)
                };
                var lstEkP = _contextEnkontrol.Select<ComboDTO>(EnkontrolEnum.CplanProd, obj);
                var lst = lstEkP.ToList();
                return lst;
            }
            catch (Exception) { return new List<ComboDTO>(); }
        }

        List<OdbcParameterDTO> parametrosCodigos(List<string> codigos)
        {
            var lst = new List<OdbcParameterDTO>();
            if (codigos.Count() > 0) { lst.AddRange(codigos.Select(s => new OdbcParameterDTO() { nombre = "descripcion", tipo = OdbcType.VarChar, valor = s })); }
            return lst;
        }

        public void saveInfoResetLubricantes(List<objRestLubricantesDTO> obj)
        {
            foreach (var item in obj)
            {
                var dataTempHis = _context.tblM_BitacoraControlAceiteMant.FirstOrDefault(x => x.id == item.id);
                var dataTempPro = _context.tblM_BitacoraControlAceiteMantProy.FirstOrDefault(x => x.idComp == item.componenteID && x.idMisc == item.lubricanteID && x.idMant == item.idMant && x.estatus);

                tblM_BitacoraControlAceiteMant objBitacoraControlAceiteMant = new tblM_BitacoraControlAceiteMant();
                tblM_BitacoraControlAceiteMantProy objBitacoraControlAceiteMantPro = new tblM_BitacoraControlAceiteMantProy();

                if (item.reset)
                {
                    objBitacoraControlAceiteMant.alta = true;
                    objBitacoraControlAceiteMant.Aplicado = true;
                    objBitacoraControlAceiteMant.fechaCaptura = DateTime.Now;
                    objBitacoraControlAceiteMant.Hrsaplico = item.hrsAplico;
                    objBitacoraControlAceiteMant.id = 0;
                    objBitacoraControlAceiteMant.idAct = 0;
                    objBitacoraControlAceiteMant.idComp = item.componenteID;
                    objBitacoraControlAceiteMant.idMant = item.idMant;
                    objBitacoraControlAceiteMant.idMisc = item.lubricanteID;
                    objBitacoraControlAceiteMant.prueba = item.prueba;
                    objBitacoraControlAceiteMant.UsuarioCap = vSesiones.sesionUsuarioDTO.id;
                    objBitacoraControlAceiteMant.vidaActual = item.vidaActual;
                    objBitacoraControlAceiteMant.VidaRestante = item.VidaRestante;
                    objBitacoraControlAceiteMant.Vigencia = item.VidaUtil;
                    objBitacoraControlAceiteMant.estatus = true;
                    _context.tblM_BitacoraControlAceiteMant.Add(objBitacoraControlAceiteMant);

                    objBitacoraControlAceiteMantPro.aplicado = false;
                    objBitacoraControlAceiteMantPro.estatus = true;
                    objBitacoraControlAceiteMantPro.fechaCaptura = DateTime.Now;
                    objBitacoraControlAceiteMantPro.FechaServicio = DateTime.Now;
                    objBitacoraControlAceiteMantPro.Hrsaplico = item.hrsAplico;
                    objBitacoraControlAceiteMantPro.id = 0;
                    objBitacoraControlAceiteMantPro.idAct = 0;
                    objBitacoraControlAceiteMantPro.idComp = item.componenteID;
                    objBitacoraControlAceiteMantPro.idMant = item.idMant;
                    objBitacoraControlAceiteMantPro.idMisc = item.lubricanteID;
                    objBitacoraControlAceiteMantPro.Observaciones = "";
                    objBitacoraControlAceiteMantPro.programado = false;
                    objBitacoraControlAceiteMantPro.prueba = item.prueba;
                    objBitacoraControlAceiteMantPro.UsuarioCap = vSesiones.sesionUsuarioDTO.id;
                    _context.tblM_BitacoraControlAceiteMantProy.Add(objBitacoraControlAceiteMantPro);

                    _context.SaveChanges();
                }
                else
                {

                    objBitacoraControlAceiteMant.alta = dataTempHis.alta;
                    objBitacoraControlAceiteMant.estatus = true;
                    objBitacoraControlAceiteMant.Aplicado = dataTempHis.Aplicado;
                    objBitacoraControlAceiteMant.fechaCaptura = DateTime.Now;
                    objBitacoraControlAceiteMant.Hrsaplico = dataTempHis.Hrsaplico;
                    objBitacoraControlAceiteMant.id = 0;
                    objBitacoraControlAceiteMant.idAct = dataTempHis.idAct;
                    objBitacoraControlAceiteMant.idComp = dataTempHis.idComp;
                    objBitacoraControlAceiteMant.idMant = item.idMant;
                    objBitacoraControlAceiteMant.idMisc = dataTempHis.idMisc;
                    objBitacoraControlAceiteMant.prueba = dataTempHis.prueba;
                    objBitacoraControlAceiteMant.UsuarioCap = vSesiones.sesionUsuarioDTO.id;
                    objBitacoraControlAceiteMant.vidaActual = dataTempHis.vidaActual;
                    objBitacoraControlAceiteMant.VidaRestante = dataTempHis.VidaRestante;
                    objBitacoraControlAceiteMant.Vigencia = dataTempHis.Vigencia;
                    _context.tblM_BitacoraControlAceiteMant.Add(objBitacoraControlAceiteMant);

                    if (dataTempPro != null)
                    {
                        objBitacoraControlAceiteMantPro.aplicado = dataTempPro.aplicado;
                        objBitacoraControlAceiteMantPro.estatus = true;
                        objBitacoraControlAceiteMantPro.fechaCaptura = DateTime.Now;
                        objBitacoraControlAceiteMantPro.FechaServicio = DateTime.Now;
                        objBitacoraControlAceiteMantPro.Hrsaplico = item.hrsAplico;
                        objBitacoraControlAceiteMantPro.id = 0;
                        objBitacoraControlAceiteMantPro.idAct = dataTempPro.idAct;
                        objBitacoraControlAceiteMantPro.idComp = dataTempPro.idComp;
                        objBitacoraControlAceiteMantPro.idMant = item.idMant;
                        objBitacoraControlAceiteMantPro.idMisc = dataTempPro.idMisc;
                        objBitacoraControlAceiteMantPro.Observaciones = dataTempPro.Observaciones;
                        objBitacoraControlAceiteMantPro.programado = dataTempPro.programado;
                        objBitacoraControlAceiteMantPro.prueba = dataTempPro.prueba;
                        objBitacoraControlAceiteMantPro.UsuarioCap = vSesiones.sesionUsuarioDTO.id;
                        objBitacoraControlAceiteMantPro.Vigencia = dataTempPro.Vigencia;
                        _context.tblM_BitacoraControlAceiteMantProy.Add(objBitacoraControlAceiteMantPro);
                    }

                    _context.SaveChanges();
                }

                if (dataTempHis != null)
                {
                    dataTempHis.estatus = false;
                    GuardarBitJG(dataTempHis);
                }

                if (dataTempPro != null)
                {
                    dataTempPro.estatus = false;
                    GuardarBitProyLub(dataTempPro);
                }

            }
            //     return false;
        }


        public List<objRestLubricantesDTO> getBitacoraLubricantesByMant(int idMant)
        {
            int modeloID = (from a in _context.tblM_MatenimientoPm
                            join b in _context.tblM_CatMaquina
                            on a.idMaquina equals b.id
                            where a.id == idMant
                            select b.modeloEquipoID).FirstOrDefault();

            var listaLubricantes = (from a in _context.tblM_PMComponenteLubricante
                                    join b in _context.tblM_CatSuministros
                                    on a.lubricanteID equals b.id
                                    join c in _context.tblM_CatComponentesViscosidades
                                    on a.componenteID equals c.id
                                    where a.modeloID == modeloID
                                    select new cboLubricantesDTO
                                    {
                                        cantidadLitros = a.cantidadLitros,
                                        componenteID = a.componenteID,
                                        edadSuministro = a.vidaLubricante,
                                        lubricanteID = b.id,
                                        nomeclatura = b.nomeclatura,
                                        descripcion = c.Descripcion

                                    }).ToList();


            var result = _context.tblM_BitacoraControlAceiteMant.Where(x => x.idMant == idMant && x.estatus).ToList();

            return result.Select(x => new objRestLubricantesDTO
            {
                componente = listaLubricantes.FirstOrDefault(r => r.componenteID == x.idComp).descripcion,
                id = x.id,
                VidaUtil = x.Vigencia,
                componenteID = x.idComp,
                comentario = _context.tblM_BitacoraControlAceiteMantProy.FirstOrDefault(p => p.idMant == idMant && p.idComp == x.idComp) != null ? _context.tblM_BitacoraControlAceiteMantProy.FirstOrDefault(p => p.idMant == idMant && p.idComp == x.idComp).Observaciones : "",
                hrsAplico = x.Hrsaplico,
                VidaRestante = x.VidaRestante,
                reset = false,
                Suministros = listaLubricantes.Where(r => r.componenteID == x.idComp).ToList(),
                idMant = idMant,
                lubricanteID = x.idMisc

            }).ToList();

        }


        public List<gridDetActividadesDTO> getDetActividadesMantProy(int modeloEquipoID)
        {
            var dataSetInfo = (from a in _context.tblM_PMComponenteModelo
                               join b in _context.tblM_PMComponenteFiltro
                               on a.componenteID equals b.componenteID
                               join c in _context.tblM_CatFiltroMant
                               on b.filtroID equals c.id
                               where a.modeloID == modeloEquipoID && b.modeloID == modeloEquipoID
                               select new gridDetActividadesDTO
                              {
                                  modeloEquipoID = a.modeloID,
                                  idAct = 0,
                                  idCompVis = a.componenteID,
                                  cantidad = b.cantidad,
                                  modelo = c.modelo,
                                  idFiltro = b.id,
                                  aplicar = false,
                                  programado = false,
                                  idMant = 0,
                                  tipoPMid = 0,
                                  componente = a.Componente.Descripcion
                              }).ToList();

            return dataSetInfo;
        }

        public List<tblM_MatenimientoPm> getListaEquiposAC(string areaCuenta)
        {
            var economicos = _context.tblM_CatMaquina.Where(x => x.centro_costos == areaCuenta && x.estatus != 0).Select(x => x.id).ToList();
            return _context.tblM_MatenimientoPm.Where(x => economicos.Contains(x.idMaquina) && x.estatus).ToList();
        }

        public List<tblM_CatPM_CatActividadPM> getActividadesByPM(int modeloEquipoID, int tipoPM)
        {

            var tipoPMID = _context.tblM_catPM.FirstOrDefault(x => x.id == tipoPM).PM;

            return _context.tblM_CatPM_CatActividadPM.Where(x => x.modeloEquipoID == modeloEquipoID && (x.idPM == tipoPMID || x.idPM == 0) && x.estado == true).ToList();
        }

        public tblM_BitacoraControlAEMantProy GetBitProyID(int uid)
        {
            return _context.tblM_BitacoraControlAEMantProy.FirstOrDefault(x => x.id == uid);
        }

        public tblM_BitacoraControlAceiteMantProy getBitProyLubByid(int id)
        {
            return _context.tblM_BitacoraControlAceiteMantProy.FirstOrDefault(x => id == x.id);
        }
        public List<dtPlaneacionSemanalPM> getPlaneacionSemanal(string areaCuenta, DateTime fechaInicio, DateTime FechaFin, string economico, bool semanal)
        {

            var equiposCentroCostos = _context.tblM_CatMaquina.Where(x => x.centro_costos.Equals(areaCuenta) && x.estatus != 0).Select(m => m.noEconomico).ToList();
            var catalogoAceites = _context.tblM_CatAceitesLubricantes.ToList();

            var objReturn = _context.tblM_MatenimientoPm
                    .Join(_context.tblM_catPM, p => p.tipoMantenimientoProy, pc => pc.id, (p, pc) => new { p, pc })
                    .Join(_context.tblM_CatMaquina, ppc => ppc.p.idMaquina, c => c.id, (ppc, c) => new { ppc, c })
                    .Where(x => x.c.centro_costos == areaCuenta && x.c.estatus != 0 && (economico == "" ? true : x.ppc.p.economicoID == economico)).ToList()
                    .Select(x =>
                    {
                        var auxComponentes = _context.tblM_BitacoraControlAceiteMantProy.Where(y => y.idMant == x.ppc.p.id && y.estatus).ToList();
                        var auxComponentesID = auxComponentes.Select(y => y.idComp).ToList();
                        var catComponentes = _context.tblM_CatComponentesViscosidades.Where(y => auxComponentesID.Contains(y.id)).ToList();


                        var componentesCombo = auxComponentes.Select(y => new ComboDTO
                        {
                            Value = catComponentes.FirstOrDefault(w => w.id == y.idComp).Descripcion + " " + catalogoAceites.FirstOrDefault(w => w.id == y.idMisc).Descripcion,
                            Text = catComponentes.FirstOrDefault(w => w.id == y.idComp).Descripcion,
                            Prefijo = catalogoAceites.FirstOrDefault(w => w.id == y.idMisc).Descripcion
                        }).ToList();
                        return new dtPlaneacionSemanalPM
                        {
                            economico = x.c.noEconomico,
                            tipoServicio = x.ppc.pc.tipoMantenimiento,
                            fechaProgramado = x.ppc.p.fechaProy,
                            fechaEjecutado = x.ppc.p.fechaPM,
                            horometroProgramado = x.ppc.p.horometroProy,
                            horometroEjecutado = x.ppc.p.horometroPMEjecutado,
                            observacion = x.ppc.p.observaciones != null ? x.ppc.p.observaciones : "",
                            estatusPM = x.ppc.p.estadoMantenimiento,
                            idMant = x.ppc.p.id,
                            idModelo = x.c.modeloEquipoID,
                            idPlaneador = x.ppc.p.UsuarioCap,
                            componentes = componentesCombo

                        };
                    }).ToList().Where(f => semanal ? (f.fechaProgramado.Date >= fechaInicio.Date && f.fechaProgramado.Date <= FechaFin.Date) : (f.fechaEjecutado.Date >= fechaInicio.Date && f.fechaEjecutado.Date <= FechaFin.Date));

            //var objReturn = (from pm in _context.tblM_MatenimientoPm
            //                 join tp in _context.tblM_catPM
            //                 on pm.tipoMantenimientoProy equals tp.id
            //                 join m in _context.tblM_CatMaquina
            //                 on pm.idMaquina equals m.id
            //                 where m.centro_costos == areaCuenta && m.estatus!=0
            //                 select new dtPlaneacionSemanalPM
            //                 {
            //                     economico = m.noEconomico,
            //                     tipoServicio = tp.tipoMantenimiento,
            //                     fechaProgramado = pm.fechaProy,
            //                     fechaEjecutado = pm.fechaPM,
            //                     horometroProgramado = pm.horometroProy,
            //                     horometroEjecutado = pm.horometroPMEjecutado,
            //                     observacion = pm.observaciones != null ? pm.observaciones : "",
            //                     estatusPM = pm.estadoMantenimiento,
            //                     idMant = pm.id,
            //                     idModelo = m.modeloEquipoID,
            //                     idPlaneador = pm.UsuarioCap,
            //                 }).ToList().Where(f => semanal ? (f.fechaProgramado.Date >= fechaInicio.Date && f.fechaProgramado.Date <= FechaFin.Date) : (f.fechaEjecutado.Date >= fechaInicio.Date && f.fechaEjecutado.Date <= FechaFin.Date));

            var idsMantenimiento = objReturn.Select(x => x.idMant).ToList();
            //var idsModelos = objReturn.Select(x => x.idModelo).ToList();
            var lubProg = _context.tblM_BitacoraControlAceiteMantProy.Where(x => idsMantenimiento.Contains(x.idMant) && x.estatus).ToList();
            var auxComponente = _context.tblM_PMComponenteModelo.ToList();

            var detallePres = objReturn.Select(x =>
            {
                var auxlubProg = lubProg.Where(y => y.idMant == x.idMant && y.programado).ToList();
                var stringLubProg = "";
                foreach (var item in auxlubProg)
                {
                    var objTempComponente = auxComponente.FirstOrDefault(c => c.componenteID == item.idComp && c.modeloID == x.idModelo);
                    if (objTempComponente != null) { stringLubProg += "Cambiar Fluido de " + objTempComponente.Componente.Descripcion + "\n\r"; }
                }
                return new dtPlaneacionSemanalPM
                {
                    economico = x.economico,
                    tipoServicio = x.tipoServicio,
                    fechaProgramado = x.fechaProgramado,
                    fechaEjecutado = x.fechaEjecutado,
                    horometroProgramado = x.horometroProgramado,
                    horometroEjecutado = x.horometroEjecutado,
                    observacion = x.observacion + "\n\n\r" + stringLubProg,
                    estatusPM = x.estatusPM,
                    idPlaneador = x.idPlaneador,
                    componentes = x.componentes,
                    idMant = x.idMant
                };
            }).ToList();

            return detallePres.ToList();
        }

        public List<tblM_CatComponentesViscosidades> getCatComponentesViscosidadesByModelo(int modeloID)
        {

            var result = from a in _context.tblM_PMComponenteModelo
                         join b in _context.tblM_CatComponentesViscosidades
                         on a.componenteID equals b.id
                         where a.modeloID == modeloID
                         select b;

            return result.ToList();
        }

        public List<tblM_CatComponentesViscosidades> getCatComponentesViscosidades()
        {
            return _context.tblM_CatComponentesViscosidades.ToList();
        }

        public List<tblM_CatPM> FillCombotablaPM(int Id, int Factor, string TipoMantenimiento)
        {
            var result = new List<tblM_CatPM>();
            try
            {
                result = _context.tblM_catPM.Where(x => (Id == 0 ? true : x.id == Id) && (TipoMantenimiento == "" ? true : x.tipoMantenimiento == TipoMantenimiento)).ToList();
            }
            catch (Exception e)
            {

            }
            return result;
        }
        private string SetNombreArchivo(string nombreBase, string fileName)
        {
            return String.Format("{0}{1}{2}", nombreBase, fileName.Split('.')[0], Path.GetExtension(fileName));
        }
        public tblM_MatenimientoPm GuardarPM(tblM_MatenimientoPm objMantenimiento)
        {
            Dictionary<string, object> resultado = new Dictionary<string, object>();
            if (objMantenimiento.id == 0)
            {
                IObjectSet<tblM_MatenimientoPm> _objectSet = ((IObjectContextAdapter)_context).ObjectContext.CreateObjectSet<tblM_MatenimientoPm>();
                try
                {
                    objMantenimiento.actual = true;
                    objMantenimiento.estatus = true;

                    if (objMantenimiento.tipoPM == 8)
                    {
                        objMantenimiento.tipoMantenimientoProy = 1;//vuelve a comenzar con un pm1
                    }
                    else
                    {
                        objMantenimiento.tipoMantenimientoProy = objMantenimiento.tipoMantenimientoProy + 1;
                    }
                    _objectSet.AddObject(objMantenimiento);

                    _context.SaveChanges();

                    GuardarRenderizado(objMantenimiento.id);
                }
                catch (Exception e)
                {
                }
                return objMantenimiento;
            }
            else
            {
                tblM_MatenimientoPm objSave = _context.tblM_MatenimientoPm.FirstOrDefault(x => x.id == objMantenimiento.id);
                objSave.estadoMantenimiento = objMantenimiento.estadoMantenimiento;
                objSave.planeador = objMantenimiento.planeador;
                objSave.personalRealizo = objMantenimiento.personalRealizo;
                _context.SaveChanges();
            
                return objSave;
            }
        }
        public Dictionary<string, object> GuardarDocumentoPM(tblM_DocumentoMantenimientoPM objDocumentoPM, HttpPostedFileBase objFile)
        {
            Dictionary<string, object> resultado = new Dictionary<string, object>();
            using (var dbContextTransaction = _context.Database.BeginTransaction())
            {
                try
                {

                    if (objFile != null)
                    {
                        #region SE REGISTRA EL ARCHIVO ADJUNTO
                        var archivoEdicion = _context.tblM_DocumentoMantenimientoPM.Where(x => x.idMantenimiento == objDocumentoPM.idMantenimiento).FirstOrDefault();
                        var listaRutaArchivos = new List<Tuple<HttpPostedFileBase, string>>();
#if DEBUG
                        var CarpetaNueva = Path.Combine(RutaLocal, objDocumentoPM.idMantenimiento.ToString());
#else
                        var CarpetaNueva = Path.Combine(RutaBase, objDocumentoPM.idMantenimiento.ToString());

#endif

                        // Verifica si existe la carpeta y si no, la crea.
                        if (GlobalUtils.VerificarExisteCarpeta(CarpetaNueva, true) == false)
                        {
                            dbContextTransaction.Rollback();
                            resultado.Add(SUCCESS, false);
                            resultado.Add(MESSAGE, "No se pudo crear la carpeta en el servidor.");
                            return resultado;
                        }
                        string nombreArchivo = SetNombreArchivo("EvidenciaMantenimientoPM", objFile.FileName);
                        string rutaArchivo = Path.Combine(CarpetaNueva, nombreArchivo);
                        listaRutaArchivos.Add(Tuple.Create(objFile, rutaArchivo));

                        // GUARDAR TABLA ARCHIVOS
                        tblM_DocumentoMantenimientoPM objEvidenciaPM = new tblM_DocumentoMantenimientoPM()
                        {
                            idMantenimiento = objDocumentoPM.idMantenimiento,
                            nombreArchivo = nombreArchivo,
                            rutaArchivo = rutaArchivo,
                            FK_UsuarioCreacion = (int)vSesiones.sesionUsuarioDTO.id,
                            fechaCreacion = DateTime.Now,
                            registroActivo = true
                        };
                        if (GlobalUtils.SaveHTTPPostedFile(objFile, rutaArchivo) == false)
                        {
                            dbContextTransaction.Rollback();
                            resultado.Clear();
                            resultado.Add(SUCCESS, false);
                            resultado.Add(MESSAGE, "Ocurrió un error al guardar los archivos en el servidor.");
                            return resultado;
                        }
                        _context.tblM_DocumentoMantenimientoPM.Add(objEvidenciaPM);
                        _context.SaveChanges();
                        dbContextTransaction.Commit();


                        //resultado.Add(MESSAGE, "Se han cargado las evidencias con éxito, Favor de consultar en en apartado de Consultas.");
                        //resultado.Add(SUCCESS, true);
                        #endregion
                    }



                }
                catch (Exception e)
                {
                    dbContextTransaction.Rollback();
                    resultado.Add(SUCCESS, false);
                    resultado.Add(MESSAGE, e.Message);
                }
            }
            return resultado;
        }

        public Tuple<Stream, string> descargarArchivo(int idArchivo)
        {
            try
            {
                var objArchivoAdjunto = _context.tblM_DocumentoMantenimientoPM.ToList().Where(w => w.idMantenimiento == idArchivo && w.registroActivo).OrderByDescending(c => c.id).First();

                var fileStream = GlobalUtils.GetFileAsStream(objArchivoAdjunto.rutaArchivo);
                string name = Path.GetFileName(objArchivoAdjunto.rutaArchivo);

                return Tuple.Create(fileStream, name);
            }
            catch (Exception e)
            {
                throw new Exception("No se encuentran archivos cargados.");
            }
        }

        public Dictionary<string, object> GetArchivosAdjuntos(int idArchivo)
        {
            Dictionary<string, object> resultado = new Dictionary<string, object>();
            try
            {
                var Archivo = _context.tblM_DocumentoMantenimientoPM.Where(x => x.registroActivo && x.idMantenimiento == idArchivo).ToList();
                if (Archivo.Count > 0)
                {
                    resultado.Add(ITEMS, Archivo);
                    resultado.Add(SUCCESS, true);
                }
                else { throw new Exception("No se encuentran archivos cargados."); }


            }
            catch (Exception e)
            {
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, e.Message);
            }

            return resultado;
        }
        public string getEmpleadosID(string id)
        {
            string nombreCompleto = "";
            try
            {
                //var resultado = (IList<tblRH_CatEmpleados>)ContextEnKontrolNomina.Where("SELECT(LTRIM(RTRIM(nombre))+' '+replace(ape_paterno, ' ', '')+' '+replace(ape_materno, ' ', '')) AS Nombre  FROM  sn_empleados where clave_empleado = (" + id + ")").ToObject<IList<tblRH_CatEmpleados>>();
                if (vSesiones.sesionEmpresaActual == 1 || vSesiones.sesionEmpresaActual == 2)
                {
                    var resultado = _context.Select<tblRH_CatEmpleados>(new DapperDTO
                    {
                        baseDatos = (int)vSesiones.sesionEmpresaActual == (int)EmpresaEnum.Construplan ? MainContextEnum.Construplan : MainContextEnum.Arrendadora,
                        consulta = @"SELECT(LTRIM(RTRIM(nombre))+' '+replace(ape_paterno, ' ', '')+' '+replace(ape_materno, ' ', '')) AS Nombre  
                                FROM  tblRH_EK_Empleados 
                                where clave_empleado = (" + id + ")",
                    });

                    nombreCompleto = resultado[0].Nombre;
                }
                else
                {
                    var resultado = _context.Select<tblRH_CatEmpleados>(new DapperDTO
                    {
                        baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
                        consulta = @"SELECT(LTRIM(RTRIM(nombre))+' '+replace(ape_paterno, ' ', '')+' '+replace(ape_materno, ' ', '')) AS Nombre  
                                FROM  tblRH_EK_Empleados 
                                where clave_empleado = (" + id + ")",
                    });

                    nombreCompleto = resultado[0].Nombre;
                }

            }
            catch (Exception e)
            {
            }
            return nombreCompleto;
        }
        private void GuardarRenderizado(int idMant)
        {
            IObjectSet<tblM_RenderFullCalendar> _objectSet = ((IObjectContextAdapter)_context).ObjectContext.CreateObjectSet<tblM_RenderFullCalendar>();
            tblM_RenderFullCalendar objTblRender = new tblM_RenderFullCalendar();
            tblM_MatenimientoPm objMantenimiento = new tblM_MatenimientoPm();
            objMantenimiento = _context.tblM_MatenimientoPm.Where(x => x.id == idMant && x.estatus == true).FirstOrDefault();

            //objMantenimiento.

            try
            {
                objTblRender.idMantenimiento = idMant;
                objTblRender.HorometroPm = objMantenimiento.horometroPM;
                objTblRender.personalRealizo = objMantenimiento.personalRealizo;
                objTblRender.fechaMantenimientoActual = Convert.ToString(objMantenimiento.fechaPM.ToString("yyyy-MM-dd hh:mm:ss"));
                objTblRender.tipoMantenimientoActual = objMantenimiento.tipoPM;
                objTblRender.observaciones = objMantenimiento.observaciones;
                objTblRender.economicoID = objMantenimiento.economicoID;
                objTblRender.title = objMantenimiento.economicoID;
                objTblRender.fechaProyectada = Convert.ToString(objMantenimiento.fechaProy.ToString("yyyy-MM-dd hh:mm:ss"));
                objTblRender.start = Convert.ToString(objMantenimiento.fechaProy.ToString("yyyy-MM-dd hh:mm:ss"));
                objTblRender.id = objMantenimiento.id;
                objTblRender.description = _context.tblM_catPM.Where(x => x.id == objMantenimiento.tipoMantenimientoProy).Select(z => z.tipoMantenimiento).FirstOrDefault() + " " + objMantenimiento.horometroProy;
                objTblRender.color = CalculoEstatus(objMantenimiento.fechaPM, objMantenimiento.fechaProy, objMantenimiento.economicoID);
                objTblRender.UltimoHorometro = getUltimoHorometro(objMantenimiento.economicoID).Horometro;
                objTblRender.horometroProyectado = (objMantenimiento.horometroProy);
                objTblRender.idMaquina = objMantenimiento.idMaquina;
                objTblRender.tipoMantenimientoActual = 0;
                _objectSet.AddObject(objTblRender);
                _context.SaveChanges();
            }
            catch (Exception e)
            {

                throw;
            }
        }
        public tblM_BitacoraControlAceiteMant GuardarBitJG(tblM_BitacoraControlAceiteMant objBitJG)
        {

            if (objBitJG.id == 0)
            {
                IObjectSet<tblM_BitacoraControlAceiteMant> _objectSet = ((IObjectContextAdapter)_context).ObjectContext.CreateObjectSet<tblM_BitacoraControlAceiteMant>();
                try
                {
                    _objectSet.AddObject(objBitJG);
                    _context.SaveChanges();
                }
                catch (Exception e)
                {
                }
            }
            else
            {

                _context.Entry(objBitJG).State = System.Data.Entity.EntityState.Modified;
                _context.SaveChanges();

            }

            return objBitJG;


        }
        public tblM_BitacoraControlAceiteMantProy GuardarBitProyLub(tblM_BitacoraControlAceiteMantProy objLubProy)
        {
            IObjectSet<tblM_BitacoraControlAceiteMantProy> _objLubProy = ((IObjectContextAdapter)_context).ObjectContext.CreateObjectSet<tblM_BitacoraControlAceiteMantProy>();
            try
            {
                if (objLubProy.id != 0)
                {


                    tblM_BitacoraControlAceiteMantProy objAceite = new tblM_BitacoraControlAceiteMantProy();
                    objAceite = _context.tblM_BitacoraControlAceiteMantProy.Where(x => x.id == objLubProy.id).FirstOrDefault();
                    objAceite.estatus = objLubProy.estatus;
                    objAceite.fechaCaptura = DateTime.Now; //objLubProy.fechaCaptura;
                    objAceite.FechaServicio = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day).AddHours(5);// DateTime.Now;  //objLubProy.FechaServicio;
                    objAceite.Hrsaplico = objLubProy.Hrsaplico;
                    objAceite.idComp = objLubProy.idComp;
                    objAceite.idMant = objLubProy.idMant;
                    objAceite.idMisc = objLubProy.idMisc;
                    objAceite.programado = objLubProy.programado;
                    objAceite.prueba = objLubProy.prueba;
                    objAceite.Vigencia = objLubProy.Vigencia;
                    objAceite.Observaciones = objLubProy.Observaciones;
                    objAceite.UsuarioCap = objLubProy.UsuarioCap;
                }
                else
                {
                    _objLubProy.AddObject(objLubProy);

                }
                _context.SaveChanges();

            }
            catch (Exception e)
            {
            }
            return objLubProy;
        }
        public tblM_BitacoraControlActExt GuardarBitAE(tblM_BitacoraControlActExt objAE)
        {
            IObjectSet<tblM_BitacoraControlActExt> _objectSet = ((IObjectContextAdapter)_context).ObjectContext.CreateObjectSet<tblM_BitacoraControlActExt>();
            try
            {
                _objectSet.AddObject(objAE);
                _context.SaveChanges();
            }
            catch (Exception e)
            {
            }
            return objAE;
        }
        public tblM_BitacoraControlDN GuardarBitDN(tblM_BitacoraControlDN objDN)
        {
            IObjectSet<tblM_BitacoraControlDN> _objectSet = ((IObjectContextAdapter)_context).ObjectContext.CreateObjectSet<tblM_BitacoraControlDN>();
            try
            {
                _objectSet.AddObject(objDN);
                _context.SaveChanges();
            }
            catch (Exception e)
            {
            }
            return objDN;
        }
        public tblM_ActvContPM GuardarContadores(tblM_ActvContPM objActContPM)
        {
            IObjectSet<tblM_ActvContPM> _objectSet = ((IObjectContextAdapter)_context).ObjectContext.CreateObjectSet<tblM_ActvContPM>();
            try
            {
                objActContPM.Actual = true;
                _objectSet.AddObject(objActContPM);
                _context.SaveChanges();
            }
            catch (Exception e)
            {
                //result = e.ToString();
            }
            return objActContPM;
        }
        public object ConsultarPMActivo()
        {
            var lstResultado = new List<object>();
            try
            {
                var lstPmActivo = _context.tblM_MatenimientoPm.Where(x => x.estatus).ToList();
                var lstCalActivo = _context.tblM_RenderFullCalendar.ToList();
                lstPmActivo.ForEach(x =>
                {
                    lstResultado.Add(lstCalActivo.FirstOrDefault(y => y.idMantenimiento.Equals(x.id)));
                });
            }
            catch (Exception e) { }
            return lstResultado;
        }

        public tblM_MatenimientoPm ConsultarPMbyID(int idobjMatenimientoPm)// consulta preventivo menor
        {
            var result = new tblM_MatenimientoPm();
            try
            {
                if (idobjMatenimientoPm != 0)
                {
                    //  result = _context.tblM_MatenimientoPm.Where(x => x.id == idobjMatenimientoPm && x.estatus == true).FirstOrDefault();

                    var obj = _context.tblM_MatenimientoPm.Where(x => x.id == idobjMatenimientoPm).FirstOrDefault();
                    var horometro = _context.tblM_CapHorometro.Where(x => x.Economico.Equals(obj.economicoID)).OrderByDescending(x => x.id).FirstOrDefault().Horometro;
                    obj.horometroUltCapturado = horometro;

                    result = obj;

                }
            }
            catch (Exception e)
            {

            }
            return result;
        }
        public tblM_CapHorometro getUltimoHorometro(string Econ)
        {
            var res = (from h in _context.tblM_CapHorometro
                       where (h.Economico.Equals(Econ))
                       select h).ToList();

            if (res.Count > 0)
            {

                return res.OrderByDescending(x => x.Fecha).First();
            }
            else
            {
                return null;
            }

        }
        public tblRH_CatEmpleados getCatEmpleados(int idEmpleado)
        {
            var getCatEmpleado = "SELECT emp.nombre+' '+emp.ape_paterno +' '+emp.ape_materno AS Nombre ,pu.descripcion AS puesto FROM DBA.sn_empleados as emp"
            + " inner join si_puestos as pu on emp.puesto = pu.puesto " +
            "WHERE clave_empleado=" + idEmpleado + "";
            try
            {
                if (vSesiones.sesionEmpresaActual == 1 || vSesiones.sesionEmpresaActual == 2)
                {
                    var resultado = _context.Select<tblRH_CatEmpleados>(new DapperDTO
                    {
                        baseDatos = (int)vSesiones.sesionEmpresaActual == (int)EmpresaEnum.Construplan ? MainContextEnum.Construplan : MainContextEnum.Arrendadora,
                        consulta = @"SELECT emp.nombre+' '+emp.ape_paterno +' '+emp.ape_materno AS Nombre ,pu.descripcion AS puesto 
			                    FROM tblRH_EK_Empleados as emp
                                inner join tblRH_EK_Puestos as pu on emp.puesto = pu.puesto 
                                WHERE clave_empleado=" + idEmpleado,
                    });
                    return resultado.FirstOrDefault();
                }
                else
                {
                    var resultado = _context.Select<tblRH_CatEmpleados>(new DapperDTO
                    {
                        baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
                        consulta = @"SELECT emp.nombre+' '+emp.ape_paterno +' '+emp.ape_materno AS Nombre ,pu.descripcion AS puesto 
			                    FROM tblRH_EK_Empleados as emp
                                inner join tblRH_EK_Puestos as pu on emp.puesto = pu.puesto 
                                WHERE clave_empleado=" + idEmpleado,
                    });
                    return resultado.FirstOrDefault();
                }
                //var resultado = (IList<tblRH_CatEmpleados>)ContextEnKontrolNomina.Where(getCatEmpleado).ToObject<IList<tblRH_CatEmpleados>>();



                //return resultado.FirstOrDefault();
            }
            catch
            {
                return null;
            }
        }

        //fecha poryectada.l
        private string CalculoEstatus(DateTime objfechaMantenimientoActual, DateTime objfechaProyectadaProximo, string objeconomicoID)
        {
            string color = "";

            TimeSpan ts = DateTime.Now - objfechaMantenimientoActual;
            // Difference in days.
            int differenceInDays = ts.Days;
            var fechaMantenimientoActual = Convert.ToString(objfechaMantenimientoActual.ToString("yyyy-MM-dd hh:mm:ss"));
            var FechaHoy = Convert.ToString(DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"));
            TimeSpan t1 = objfechaProyectadaProximo - objfechaMantenimientoActual;
            int diasFechaProyectada = t1.Days;
            int hrsPromDiaria = Convert.ToInt16(CalculoHrsPromDiario(objeconomicoID));
            var FactorFechaProyectada = diasFechaProyectada * hrsPromDiaria;
            double FactorHoy = (Convert.ToDouble(differenceInDays) * Convert.ToDouble((hrsPromDiaria)));//horas trabajadas al dia de hoy        

            if (FactorHoy > 275)
            {
                color = "#d43f3a";//ROJO
            }
            else if (FactorHoy <= 225)
            {
                color = "#4cae4c";//verde
            }
            else if (FactorHoy > 225 && FactorHoy < 275)
            {
                color = "#ec971f";//verde
            }
            return color;
        }
        private decimal CalculoHrsPromDiario(string economicoID)
        {
            IObjectSet<tblM_CapHorometro> _objectSetCapHorometro = ((IObjectContextAdapter)_context).ObjectContext.CreateObjectSet<tblM_CapHorometro>();
            IObjectSet<tblM_CapRitmoHorometro> _objsetCapRitmo = ((IObjectContextAdapter)_context).ObjectContext.CreateObjectSet<tblM_CapRitmoHorometro>();
            decimal result = 1;
            try
            {
                result = _context.tblM_CapRitmoHorometro.Where(y => y.economico == economicoID).Select(X => X.horasDiarias).FirstOrDefault();

                if (result == 0)
                {
                    var horometros = _objectSetCapHorometro.Where(y => y.Economico == economicoID).OrderByDescending(r => r.id).Take(40).ToList();
                    if (horometros != null) result = horometros.GroupBy(x => x.Fecha).Select(x => x.Sum(y => y.HorasTrabajo)).Average();
                }
            }
            catch (Exception)
            {

                var horometros = _objectSetCapHorometro.Where(y => y.Economico == economicoID).OrderByDescending(r => r.id).Take(40).ToList();
                if (horometros != null) result = horometros.GroupBy(x => x.Fecha).Select(x => x.Sum(y => y.HorasTrabajo)).Average();
            }
            return result;
        }
        private List<tblM_CapRitmoHorometro> CalculoHrsPromDiarioMaquinas(List<string> economicosID)
        {
            List<tblM_CapRitmoHorometro> result = new List<tblM_CapRitmoHorometro>();
            try
            {
                result = _context.tblM_CapRitmoHorometro.Where(y => economicosID.Contains(y.economico)).ToList();
            }
            catch (Exception) { }
            return result;
        }
        public decimal ConsultarRitmoAutomatico(string EconomicoID)// consulta preventivo menor
        {
            decimal promedioHoras = 0;
            IObjectSet<tblM_CapHorometro> _objectSetCapHorometro = ((IObjectContextAdapter)_context).ObjectContext.CreateObjectSet<tblM_CapHorometro>();
            try
            {
                var valoresRitmo = _objectSetCapHorometro.Where(x => x.Economico == EconomicoID).OrderByDescending(r => r.id).Take(20);
                promedioHoras = valoresRitmo.Sum(x => x.HorasTrabajo) / 20;

            }
            catch (Exception e)
            {

            }
            return promedioHoras;
        }
        public object ActividadadesbyID(int IDmaquinaria)
        {
            int modeloEquipoID = 0;
            //seleccionar maquina para obtener el modelo 1
            IObjectSet<tblM_CatMaquina> _objectSetCATModelo = ((IObjectContextAdapter)_context).ObjectContext.CreateObjectSet<tblM_CatMaquina>();
            tblM_CatMaquina objCatMAquina = new tblM_CatMaquina();//seleccionar el modelo
            objCatMAquina = _objectSetCATModelo.Where(x => x.id == IDmaquinaria).FirstOrDefault();
            modeloEquipoID = objCatMAquina.modeloEquipoID;
            //seleccionar las actividades relacionales 2
            List<tblM_CatPM_CatActividadPM> lstActividadesRel = new List<tblM_CatPM_CatActividadPM>();
            IObjectSet<tblM_CatPM_CatActividadPM> _objectSetActividadesRel = ((IObjectContextAdapter)_context).ObjectContext.CreateObjectSet<tblM_CatPM_CatActividadPM>();
            lstActividadesRel = _objectSetActividadesRel.Where(x => x.modeloEquipoID == modeloEquipoID).ToList();
            //seleccionar todas las actividades que le corresponde al modelo 3
            List<tblM_CatActividadPM> lstCatActividades = new List<tblM_CatActividadPM>();
            IObjectSet<tblM_CatActividadPM> _objectSetActividades = ((IObjectContextAdapter)_context).ObjectContext.CreateObjectSet<tblM_CatActividadPM>();
            lstCatActividades = _objectSetActividades.ToList();
            //seleccionar todo el catalogo PM
            List<tblM_CatPM> lstCatPM = new List<tblM_CatPM>();
            IObjectSet<tblM_CatPM> _objectSetCatPM = ((IObjectContextAdapter)_context).ObjectContext.CreateObjectSet<tblM_CatPM>();
            lstCatPM = _objectSetCatPM.ToList();
            //seleccionar todo el catalogo Partes Vida Util
            List<tblM_CatParteVidaUtil> lstObjCatPArteVida = new List<tblM_CatParteVidaUtil>();
            IObjectSet<tblM_CatParteVidaUtil> _objectSetCatParteVida = ((IObjectContextAdapter)_context).ObjectContext.CreateObjectSet<tblM_CatParteVidaUtil>();
            lstObjCatPArteVida = _objectSetCatParteVida.ToList();

            ////seleccionar la vida util de las partes relaconadas a la actividad
            List<tblM_CatActividadPM_tblM_CatParte> listobjActvParte = new List<tblM_CatActividadPM_tblM_CatParte>();
            IObjectSet<tblM_CatActividadPM_tblM_CatParte> _objectSetActvParte = ((IObjectContextAdapter)_context).ObjectContext.CreateObjectSet<tblM_CatActividadPM_tblM_CatParte>();
            listobjActvParte = _objectSetActvParte.ToList();

            //////relacion Actividades Vida Util
            List<tblM_ActvContPM> lstObjContAct = new List<tblM_ActvContPM>();
            IObjectSet<tblM_ActvContPM> _objectContAct = ((IObjectContextAdapter)_context).ObjectContext.CreateObjectSet<tblM_ActvContPM>();
            lstObjContAct = _objectContAct.ToList();

            var lstResultado = new object();
            try
            {
                lstResultado = lstActividadesRel.Select(x => new
               {
                   id = x.id,//Id d' la tabla relacionadora
                   orden = x.orden,//Orden en que Se mostrra Cada Actividad
                   descripcion = (lstCatActividades.Where(act => act.id == x.idAct && act.estado == true).Select(act => act.descripcionActividad)),//descripcion actividad
                   tipoMantenimiento = (lstCatPM.Where(pm => pm.id == x.idPM).Select(pm => pm.tipoMantenimiento)),//tipo de mantenimiento PM'
                   factorExtra = lstObjCatPArteVida.Join(listobjActvParte.Where(pm => pm.idActividadPM == x.idAct && pm.modeloEquipoID == modeloEquipoID), r => r.id, p => p.idParte, (r, p) => new { r.descripcion, r.id, r.vidaUtilMax, r.vidaUtilMin }),
                   Contador = lstObjContAct.Where(pm => pm.Actual == true && pm.idActividad == x.idAct && pm.idMaquina == IDmaquinaria).Select(pm => new { pm.Contador, pm.idParteVidaUtil }).Distinct().OrderBy(pm => pm.idParteVidaUtil).ToArray()

               });
            }
            catch (Exception e)
            {

            }
            return lstResultado;
        }
        public tblM_CapHorometro ConsultarUltimoHorometro(string fechaIniMante, string EconomicoID)
        {
            tblM_CapHorometro result = new tblM_CapHorometro();
            DateTime fechaMantenimiento = Convert.ToDateTime(fechaIniMante);
            //IObjectSet<tblM_CapHorometro> _objectSetCapHorometro = ((IObjectContextAdapter)_context).ObjectContext.CreateObjectSet<tblM_CapHorometro>();
            try
            {
                var maquina = _context.tblM_CatMaquina.FirstOrDefault(x => x.noEconomico == EconomicoID);
                var economicoHistorial = "";
                if (maquina != null) economicoHistorial = maquina.EconomicoCC;
                //result = _objectSetCapHorometro.Where(x => x.Economico == EconomicoID).OrderByDescending(r => r.id).Take(1);
                result = _context.tblM_CapHorometro.Where(x => x.Economico.Equals(EconomicoID) || (economicoHistorial == "" ? false : x.Economico.Equals(maquina.EconomicoCC))).OrderByDescending(x => new { x.Fecha, x.turno }).FirstOrDefault();

            }
            catch (Exception e)
            {
            }
            return result;
        }
        public object ConsultarIDmaquinaria(string EconomicoID)
        {
            var result = new object();
            try
            {
                result = _context.tblM_CatMaquina.Where(x => x.noEconomico == EconomicoID).Select(x =>
                    new
                    {
                        grupoMaquinariaID = x.grupoMaquinariaID,
                        modeloID = x.modeloEquipoID
                    }).FirstOrDefault();
            }
            catch (Exception e)
            {
            }
            return result;
        }
        public object ModificacionFecha(int id, DateTime FechaUpdate)
        {
            var result = new object();
            IObjectSet<tblM_MatenimientoPm> _objectSetMantenimientoPM = ((IObjectContextAdapter)_context).ObjectContext.CreateObjectSet<tblM_MatenimientoPm>();
            tblM_MatenimientoPm objMantenimientoPM = new tblM_MatenimientoPm();
            try
            {
                var objData = _context.tblM_MatenimientoPm.FirstOrDefault(c => c.id == id);

                if (objData != null)
                {
                    if (objData.estatus)
                    {
                        objMantenimientoPM = _objectSetMantenimientoPM.Where(x => x.id == id && x.actual == true).First();
                        objMantenimientoPM.fechaProy = FechaUpdate;
                        _context.SaveChanges();
                    }
                }
            }
            catch (Exception e)
            {

            }
            return result;
        }
        public object ConsultarFechaUltimoHorometro(decimal Horometro, string EconomicoID)
        {
            var result = new object();
            IObjectSet<tblM_CapHorometro> _objectSetCapHorometro = ((IObjectContextAdapter)_context).ObjectContext.CreateObjectSet<tblM_CapHorometro>();
            try
            {
                result = _objectSetCapHorometro.Where(x => x.Horometro == Horometro && x.Economico == EconomicoID).OrderByDescending(x => new { x.Fecha, x.turno }).FirstOrDefault();
                _context.SaveChanges();
            }
            catch (Exception e)
            {

            }
            return result;
        }
        public Dictionary<string, object> ConsultarIntervaloFecha(DateTime Fecha, string EconomicoID)
        {
            var result = new Dictionary<string, object>();
            var FechaLimiteInferior = new object();
            var FechaLimiteSuperior = new object();
            var FechaSeleccionada = new object();
            IObjectSet<tblM_CapHorometro> _objectSetCapHorometro = ((IObjectContextAdapter)_context).ObjectContext.CreateObjectSet<tblM_CapHorometro>();
            try
            {
                //.Select(pm=>  new  {  pm.Contador, pm.idParteVidaUtil} )
                FechaLimiteInferior = _objectSetCapHorometro.Where(x => x.Fecha < Fecha && x.Economico == EconomicoID).OrderByDescending(x => x.Fecha).Take(1).Select(x => new { x.Fecha, x.Horometro }).First();
            }
            catch (Exception)
            {
                FechaLimiteInferior = 0;
            }
            result.Add("FechaLimiteInferior", FechaLimiteInferior);
            try
            {
                FechaLimiteSuperior = _objectSetCapHorometro.Where(x => x.Fecha > Fecha && x.Economico == EconomicoID).OrderBy(x => x.Fecha).Take(1).Select(x => new { x.Fecha, x.Horometro }).First();
            }
            catch (Exception)
            {
                FechaLimiteSuperior = 0; ;
            }

            result.Add("FechaLimiteSuperior", FechaLimiteSuperior);
            try
            {
                FechaSeleccionada = _objectSetCapHorometro.Where(x => x.Fecha == Fecha && x.Economico == EconomicoID).OrderBy(x => x.Fecha).Take(1).Select(x => new { x.Fecha, x.Horometro }).First();
            }
            catch (Exception)
            {
                FechaLimiteSuperior = 0; ;
            }
            result.Add("FechaSeleccionada", FechaSeleccionada);
            return result;
        }
        public object ModificacionHorarioServicio(int idMaquina, DateTime inicio, DateTime fin)
        {
            var result = new object();
            IObjectSet<tblM_MatenimientoPm> _objectSetMantenimientoPM = ((IObjectContextAdapter)_context).ObjectContext.CreateObjectSet<tblM_MatenimientoPm>();
            tblM_MatenimientoPm objMantenimientoPM = new tblM_MatenimientoPm();

            try
            {
                objMantenimientoPM = _objectSetMantenimientoPM.Where(x => x.id == idMaquina && x.actual == true).First();
                objMantenimientoPM.fechaProy = inicio;
                objMantenimientoPM.fechaProyFin = fin;
                _context.SaveChanges();
            }
            catch (Exception e)
            {

            }
            return result;
        }
        public object FillGridActividad(tblM_CatActividadPM obj)
        {
            var result = new object();
            IObjectSet<tblM_CatActividadPM> _objectSetActividadPM = ((IObjectContextAdapter)_context).ObjectContext.CreateObjectSet<tblM_CatActividadPM>();
            IObjectSet<tblM_CatTipoActividad> _objectSetTipo = ((IObjectContextAdapter)_context).ObjectContext.CreateObjectSet<tblM_CatTipoActividad>();
            List<tblM_CatActividadPM> objActividadPM = new List<tblM_CatActividadPM>();
            try
            {
                objActividadPM = _objectSetActividadPM.Where(x => (obj.id == 0 ? true : x.id == obj.id)
                                && (obj.descripcionActividad == null ? true : x.descripcionActividad == obj.descripcionActividad)
                                && (obj.idCatTipoActividad == 0 ? true : x.idCatTipoActividad == obj.idCatTipoActividad)
                                && x.estado == true).ToList();
                result = objActividadPM.Select(x => new
                {
                    id = x.id,
                    descripcionActividad = x.descripcionActividad,
                    TipoActividad = (_objectSetTipo.Where(y => y.id == x.idCatTipoActividad).Select(act => act.descripcion)).FirstOrDefault(),//descripcion actividad
                    idTipoActividad = x.idCatTipoActividad
                });
            }
            catch (Exception e)
            {
            }
            return result;
        }
        public List<object> FillGridMiselaneo(int modeloEquipoID, int idAct, int idCompVis, int idTipo)
        {
            var lstResultado = new List<object>();
            try
            {
                if (1 == idTipo)
                {
                    List<tblM_CatFiltroMant> objFiltro = new List<tblM_CatFiltroMant>();
                    List<tblM_CatFiltroMant> lstObjFiltro = new List<tblM_CatFiltroMant>();
                    lstObjFiltro = _context.tblM_CatFiltroMant.Where(x => x.estado == true).ToList();
                    foreach (var obj in lstObjFiltro)
                    {
                        var temp = new
                        {
                            id = obj.id,
                            descripcion = obj.descripcion,
                            marca = obj.marca,
                            modelo = obj.modelo,
                            estado = obj.estado,
                            sintetico = obj.sintetico,
                            asignado = AsignadoMis(modeloEquipoID, idAct, idCompVis, idTipo, true, obj.id),
                            cantidad = CantidadMis(AsignadoMis(modeloEquipoID, idAct, idCompVis, idTipo, true, obj.id), modeloEquipoID, idAct, idCompVis, idTipo, true, obj.id)
                        };
                        lstResultado.Add(temp);
                    }
                }
                else if (2 == idTipo || 3 == idTipo)
                {
                    List<tblM_CatSuministros> objAceiteLubricante = new List<tblM_CatSuministros>();
                    List<tblM_CatSuministros> lstobjAceiteLubricante = new List<tblM_CatSuministros>();
                    if (idTipo == 2)
                    {
                        lstobjAceiteLubricante = _context.tblM_CatSuministros.Where(x => x.tipo == 1).ToList();//
                    }
                    else if (3 == idTipo)
                    {
                        lstobjAceiteLubricante = _context.tblM_CatSuministros.Where(x => x.tipo == 2).ToList();//anticongelante tipo3
                    }
                    //foreach (var obj in lstobjAceiteLubricante.Where(x=> x.modeloID== modeloEquipoID))//traer solo los aceites registrados en los componentes del motor
                    foreach (var obj in lstobjAceiteLubricante)//traer solo los aceites registrados en los componentes del motor
                    {
                        var temp = new
                        {
                            id = obj.id,
                            descripcion = obj.nomeclatura,
                            Componente = obj.descripcion,
                            asignado = AsignadoMis(modeloEquipoID, idAct, idCompVis, idTipo, true, obj.id),
                            cantidad = CantidadMis(AsignadoMis(modeloEquipoID, idAct, idCompVis, idTipo, true, obj.id), modeloEquipoID, idAct, idCompVis, idTipo, true, obj.id)

                        };
                        lstResultado.Add(temp);
                    }

                }
            }
            catch (Exception e)
            {
            }
            return lstResultado;
        }
        private object CantidadMis(object objAsignado, int modeloEquipoID, int idAct, int idComp, int idTipo, bool Bandera, int idMis)
        {

            var obj = new object();
            tblM_MiscelaneoMantenimiento objMisc = new tblM_MiscelaneoMantenimiento();
            try
            {
                if (Bandera == true && modeloEquipoID != 0 && idAct != 0 && idComp != 0)
                {
                    var res = _context.tblM_MiscelaneoMantenimiento.Where(x => x.estado == true && x.idAct == idAct && x.idMis == idMis && x.modeloEquipoID == modeloEquipoID && x.idCompVis == idComp && x.idTipo == idTipo);
                    obj = res.Select(x => new
                    {
                        id = x.id,
                        cantidad = x.cantidad
                    });
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return obj;
        }
        private object ComponeteVinculado(int subConjuntoID)
        {
            var result = new object();
            try
            {
                result = _context.tblM_CatSubConjunto.Where(x => x.id == subConjuntoID).FirstOrDefault();
            }
            catch (Exception)
            {
                throw;
            }
            return result;
        }
        private object AsignadoMis(int modeloEquipoID, int idAct, int idComp, int idTipo, bool Bandera, int idMis)
        {
            var result = new object();
            tblM_MiscelaneoMantenimiento objMisc = new tblM_MiscelaneoMantenimiento();
            try
            {
                if (modeloEquipoID == 0 && idAct == 0 && idComp == 0)
                    result = 0;

                if (Bandera == false && modeloEquipoID != 0 && idAct != 0 && idComp != 0)
                {
                    result = _context.tblM_MiscelaneoMantenimiento.Where(x => x.estado == true && x.idAct == idAct && x.modeloEquipoID == modeloEquipoID && x.idCompVis == idComp && idTipo == 0 ? true : x.idTipo == idTipo).OrderByDescending(i => i.id).FirstOrDefault();
                }
                else if (Bandera == true && modeloEquipoID != 0 && idAct != 0 && idComp != 0)
                {
                    result = _context.tblM_MiscelaneoMantenimiento.Where(x => x.estado == true && x.idAct == idAct && x.idMis == idMis && x.modeloEquipoID == modeloEquipoID && x.idCompVis == idComp && x.idTipo == idTipo).OrderByDescending(i => i.id).FirstOrDefault();
                }
            }
            catch (Exception)
            {


                throw;
            }
            return result;
        }
        public List<tblM_CatActividadPM> getCatActividad(string term, int idTipo)
        {
            var result = new List<tblM_CatActividadPM>();
            IObjectSet<tblM_CatActividadPM> _objectSetActividadPM = ((IObjectContextAdapter)_context).ObjectContext.CreateObjectSet<tblM_CatActividadPM>();
            try
            {
                if (idTipo == 1)
                {
                    result = _objectSetActividadPM.Where(x => x.descripcionActividad.Contains(term) && x.estado == true && x.idCatTipoActividad == 1).ToList().Take(10).ToList();
                }
                else if (idTipo == 2)
                {
                    result = _objectSetActividadPM.Where(x => x.descripcionActividad.Contains(term) && x.estado == true && x.idCatTipoActividad == 2).ToList().Take(10).ToList();
                }
                else if (idTipo == 3)
                {
                    result = _objectSetActividadPM.Where(x => x.descripcionActividad.Contains(term) && x.estado == true && x.idCatTipoActividad == 3).ToList().Take(10).ToList();
                }
                else if (idTipo == 4)
                {
                    result = _objectSetActividadPM.Where(x => x.descripcionActividad.Contains(term) && x.estado == true && x.idCatTipoActividad == 4).ToList().Take(10).ToList();
                }
                else if (idTipo == 5)
                {
                    result = _objectSetActividadPM.Where(x => x.descripcionActividad.Contains(term) && x.estado == true && x.idCatTipoActividad == 5).ToList().Take(10).ToList();
                }
                else if (idTipo == 0)//todos
                {
                    result = _objectSetActividadPM.Where(x => x.descripcionActividad.Contains(term) && x.estado == true).ToList().Take(10).ToList();
                }

            }
            catch (Exception e)
            {
            }
            return result;
        }
        public List<tblM_CatModeloEquipo> getCatModelo(string term)
        {
            var result = new List<tblM_CatModeloEquipo>();
            IObjectSet<tblM_CatModeloEquipo> _objectSetCarModelo = ((IObjectContextAdapter)_context).ObjectContext.CreateObjectSet<tblM_CatModeloEquipo>();
            IObjectSet<tblM_CatGrupoMaquinaria> _objectSetGrupoMaquinaria = ((IObjectContextAdapter)_context).ObjectContext.CreateObjectSet<tblM_CatGrupoMaquinaria>();
            try
            {
                var tipoMaquinaria =
                    from modelo in _objectSetCarModelo
                    join grupo in _objectSetGrupoMaquinaria on modelo.idGrupo equals grupo.id
                    where grupo.tipoEquipoID == 1
                    select new { modelo };
                result = tipoMaquinaria.Select(x => x.modelo).Where(x => x.descripcion.Contains(term)).ToList().Take(10).ToList();
            }
            catch (Exception e)
            {
            }
            return result;
        }
        public object FillGridParte(tblM_CatParteVidaUtil obj)
        {
            var result = new object();
            IObjectSet<tblM_CatParteVidaUtil> _objectSetPartvida = ((IObjectContextAdapter)_context).ObjectContext.CreateObjectSet<tblM_CatParteVidaUtil>();
            List<tblM_CatParteVidaUtil> objVidaUtil = new List<tblM_CatParteVidaUtil>();
            try
            {
                objVidaUtil = _objectSetPartvida.Where(x => (obj.id == 0 ? true : x.id == obj.id) && (obj.descripcion == null ? true : x.descripcion == obj.descripcion)).ToList();
            }
            catch (Exception e)
            {
            }
            return objVidaUtil;
        }
        public object FillGridComponenteVin(int modeloEquipoID, int idActs, int idTipoAct, int idpm)
        {
            var result = new object();
            List<tblM_CatComponentesViscosidades> _lstobjComponent = new List<tblM_CatComponentesViscosidades>();
            if (idActs == 20)
            {
                _lstobjComponent = _context.tblM_CatComponentesViscosidades.ToList();
            }
            else if (true)
            {
                _lstobjComponent = _context.tblM_CatComponentesViscosidades.Where(x => x.id != 16).ToList();
            }
            List<object> lstobjCompVin = new List<object>();
            try
            {
                foreach (var obj in _lstobjComponent)
                {
                    var temp = new
                    {
                        id = obj.id,
                        descripcion = obj.Descripcion,
                        Tipo = obj.Tipo,
                        asignado = VinCompoAct(modeloEquipoID, idActs, obj.id, idTipoAct, idpm), //realizar una consulta para buscar en una tabla  modelo y id componente
                        asignadoMis = AsignadoMis(modeloEquipoID, idActs, obj.id, 0, false, 0) //el cero representa al tipo
                    };
                    lstobjCompVin.Add(temp);
                }
            }
            catch (Exception e)
            {
            }
            return lstobjCompVin;
        }
        private object VinCompoAct(int modeloEquipoID, int idAct, int idComp, int idTipoAct, int idpm)
        {

            var result = new object();
            try
            {
                if (modeloEquipoID == 0 && idAct == 0)
                    result = 0;
                if (modeloEquipoID != 0 && idAct != 0)
                    result = _context.tblM_ComponenteMantenimiento.Where(x => x.idAct == idAct && x.modeloEquipoID == modeloEquipoID && x.idCompVis == idComp && x.estado == true && x.idCatTipoActividad == idTipoAct && x.idPM == idpm).FirstOrDefault();


            }
            catch (Exception)
            {

                throw;
            }
            return result;
        }
        public List<tblM_CatParteVidaUtil> getCatComponente(string term)
        {
            var result = new List<tblM_CatParteVidaUtil>();
            IObjectSet<tblM_CatParteVidaUtil> _objectSetComponente = ((IObjectContextAdapter)_context).ObjectContext.CreateObjectSet<tblM_CatParteVidaUtil>();
            try
            {
                result = _objectSetComponente.Where(x => x.descripcion.Contains(term)).ToList().Take(10).ToList();
            }


            catch (Exception e)
            {
            }
            return result;
        }
        public object GuardarNuevaActividad(tblM_CatActividadPM objCatActividadPM)
        {
            var result = new object();
            IObjectSet<tblM_CatActividadPM> _objectSetActividadPM = ((IObjectContextAdapter)_context).ObjectContext.CreateObjectSet<tblM_CatActividadPM>();
            try
            {
                if (objCatActividadPM.id == 0)
                {
                    objCatActividadPM.estado = true;
                    _objectSetActividadPM.AddObject(objCatActividadPM);
                    _context.SaveChanges();
                    result = "Guardado";
                }
                else
                {
                    tblM_CatActividadPM objModificacionActividad = new tblM_CatActividadPM();
                    objModificacionActividad = _objectSetActividadPM.Where(x => x.id == objCatActividadPM.id).FirstOrDefault();
                    objModificacionActividad.descripcionActividad = objCatActividadPM.descripcionActividad;
                    objModificacionActividad.idCatTipoActividad = objCatActividadPM.idCatTipoActividad;
                    _context.SaveChanges();
                    result = "Modificado";
                }
            }
            catch (Exception e)
            {
                result = "Error";
            }
            return result;
        }
        public object GuardarParte(tblM_CatParteVidaUtil objParte)
        {
            var result = new object();
            IObjectSet<tblM_CatParteVidaUtil> _objectSetParte = ((IObjectContextAdapter)_context).ObjectContext.CreateObjectSet<tblM_CatParteVidaUtil>();
            try
            {
                if (objParte.id == 0)
                {
                    _objectSetParte.AddObject(objParte);
                    _context.SaveChanges();
                    result = "Guardado";
                }
            }
            catch (Exception e)
            {
                result = "Error";
            }
            return result;
        }

        public tblM_DocumentosMaquinaria getDocumentosByID(int id)
        {
            return _context.tblM_DocumentosMaquinaria.Where(x => x.id == id).FirstOrDefault();
        }
        public object ELiminarVinc(tblM_CatPM_CatActividadPM obj)
        {
            var result = new object();
            IObjectSet<tblM_CatPM_CatActividadPM> _objectSetVinc = ((IObjectContextAdapter)_context).ObjectContext.CreateObjectSet<tblM_CatPM_CatActividadPM>();
            tblM_CatPM_CatActividadPM _objActividadPM = new tblM_CatPM_CatActividadPM();
            List<tblM_ComponenteMantenimiento> lstobjActividadComp = new List<tblM_ComponenteMantenimiento>();
            List<tblM_MiscelaneoMantenimiento> lstobjActividadMis = new List<tblM_MiscelaneoMantenimiento>();
            tblM_MiscelaneoMantenimiento _objActividadMis = new tblM_MiscelaneoMantenimiento();
            List<tblM_CatPM_CatActividadPM> _lstobjActividadPM = new List<tblM_CatPM_CatActividadPM>();
            //List<tblM_CatPM_CatActividadPM> _lstobjActividadPM1 = new List<tblM_CatPM_CatActividadPM>();//nuevol
            try
            {
                //eliminar Actividad Vinculada modelo
                int idCatTipoActividad = 0;
                _objActividadPM = _objectSetVinc.Where(x => x.id == obj.id).First();
                _objActividadPM.estado = false;
                int idAct = _objActividadPM.idAct;
                int modeloEquipoID = _objActividadPM.modeloEquipoID;
                lstobjActividadComp = _context.tblM_ComponenteMantenimiento.Where(x => x.idAct == idAct && x.modeloEquipoID == modeloEquipoID).ToList();//raguilar 22/05/18 seagrego el modelo
                if (lstobjActividadComp != null)
                {
                    foreach (var item in lstobjActividadComp)
                    {
                        item.estado = false;
                    }
                }
                lstobjActividadMis = _context.tblM_MiscelaneoMantenimiento.Where(x => x.idAct == idAct && x.modeloEquipoID == modeloEquipoID).ToList();
                if (lstobjActividadMis != null)
                {
                    foreach (var item in lstobjActividadMis)
                    {
                        item.estado = false;
                    }
                }
                //raguilar modificacion eliminado de vinculado de formatos 21/05/1/
                EliminarFormatoVinculado(idAct, modeloEquipoID);

                _lstobjActividadPM = _objectSetVinc.ToList();
                idCatTipoActividad = _context.tblM_CatPM_CatActividadPM.Where(x => x.id == obj.id).FirstOrDefault().idCatTipoActividad;
                //int idPM = _objActividadPM.idPM;
                int contador = 0;
                if (idCatTipoActividad == 1)//esquema pm
                {
                    foreach (var item in _lstobjActividadPM.Where(x => x.estado == true && x.idPM == obj.idPM && x.modeloEquipoID == modeloEquipoID).OrderBy(y => y.orden))
                    {
                        contador = contador + 1;
                        item.orden = contador;
                        _context.SaveChanges();
                    }
                }
                else if (idCatTipoActividad == 2)//jG
                {
                    foreach (var item in _lstobjActividadPM.Where(x => x.estado == true && x.idCatTipoActividad == idCatTipoActividad && x.modeloEquipoID == modeloEquipoID).OrderBy(y => y.orden))
                    {
                        contador = contador + 1;
                        item.orden = contador;
                        _context.SaveChanges();
                    }
                }
                else if (idCatTipoActividad == 3)//actividades extras
                {
                    foreach (var item in _lstobjActividadPM.Where(x => x.estado == true && x.idCatTipoActividad == idCatTipoActividad && x.modeloEquipoID == modeloEquipoID).OrderBy(y => y.orden))
                    {
                        contador = contador + 1;
                        item.orden = contador;
                        _context.SaveChanges();
                    }
                }
                else if (idCatTipoActividad == 4)//actividades extras
                {
                    foreach (var item in _lstobjActividadPM.Where(x => x.estado == true && x.idDN == obj.idDN && x.modeloEquipoID == modeloEquipoID).OrderBy(y => y.orden))
                    {
                        contador = contador + 1;
                        item.orden = contador;
                        _context.SaveChanges();
                    }
                }



                else
                {
                    //raguilr 24/05/18 
                    //    foreach (var item in _lstobjActividadPM.Where(x => x.estado == true && x.idAct == obj.idAct).OrderBy(y => y.orden))
                    //    {
                    //        contador = contador + 1;
                    //        item.orden = contador;
                    //        _context.SaveChanges();
                    //    }
                    //}
                    //foreach (var item in _lstobjActividadPM.Where(x => x.estado == true && x.idPM == obj.idAct).OrderBy(y => y.orden))
                    //{
                    //    contador = contador + 1;
                    //    item.orden = contador;
                    //    _context.SaveChanges();
                    //}
                }
                _lstobjActividadPM = _objectSetVinc.ToList();
                result = "Eliminado";
            }
            catch (Exception e)
            {
                result = "Error";
            }
            return result;
        }
        private void EliminarFormatoVinculado(int idAct, int modeloID)
        {
            List<tblM_FormatoManteniento> lstobjFormato = new List<tblM_FormatoManteniento>();
            tblM_FormatoManteniento _objActividadFor = new tblM_FormatoManteniento();
            lstobjFormato = _context.tblM_FormatoManteniento.Where(x => x.idAct == idAct && x.modeloEquipoID == modeloID).ToList();
            if (lstobjFormato != null)
            {
                foreach (var item in lstobjFormato)
                {
                    item.estado = false;
                }
            }
            _context.SaveChanges();
        }
        public object ELiminarActividad(tblM_CatActividadPM objCatActividadPM)
        {
            var result = new object();
            IObjectSet<tblM_CatActividadPM> _objectSetActividadPM = ((IObjectContextAdapter)_context).ObjectContext.CreateObjectSet<tblM_CatActividadPM>();
            tblM_CatActividadPM objActividadPM = new tblM_CatActividadPM();
            try
            {
                objActividadPM = _objectSetActividadPM.Where(x => x.id == objCatActividadPM.id).First();
                objActividadPM.estado = false;
                _context.SaveChanges();
                result = "Eliminado";
            }
            catch (Exception e)
            {
                result = "Error";
            }
            return result;
        }
        public tblM_CapRitmoHorometro CapRitmoHorometro(string obj)
        {
            var result = (from r in _context.tblM_CapRitmoHorometro
                          where r.economico.Equals(obj)
                          select r);
            return result.FirstOrDefault();
        }

        public List<getActividadModeloDTO> getActividadesModelos(int id)
        {
            var dataSet = (from pmac in _context.tblM_CatPM_CatActividadPM
                           join a in _context.tblM_CatActividadPM
                           on pmac.idAct equals a.id
                           join ta in _context.tblM_CatTipoActividad
                           on a.idCatTipoActividad equals ta.id
                           where pmac.modeloEquipoID == id
                           select new getActividadModeloDTO
                           {
                               id = pmac.id,
                               orden = pmac.orden,
                               descripcion = a.descripcionActividad,
                               idAct = a.id,
                               Tipo = ta.descripcion,
                               idTipo = pmac.idCatTipoActividad,
                               PM = pmac.idPM,
                               idformato = null,
                               leyenda = pmac.leyenda,
                               Componente = false,
                               DN = pmac.idDN
                           }).ToList();

            return dataSet;
        }
        public object getActividadModelo(int id)
        {
            var result = new object();
            List<tblM_CatPM_CatActividadPM> lstobjActividadModelo = new List<tblM_CatPM_CatActividadPM>();
            List<tblM_CatActividadPM> lstCatActividades = new List<tblM_CatActividadPM>();
            lstCatActividades = _context.tblM_CatActividadPM.ToList();
            lstobjActividadModelo = _context.tblM_CatPM_CatActividadPM.Where(x => (x.modeloEquipoID == id) && x.estado == true).ToList();
            var lstResultado = new List<object>();
            try
            {
                foreach (var obj in lstobjActividadModelo)
                {

                    var temp = new getActividadModeloDTO
                    {
                        id = obj.id,
                        orden = obj.orden,
                        descripcion = (lstCatActividades.Where(act => act.id == obj.idAct && act.estado == true).Select(act => act.descripcionActividad)).FirstOrDefault(),
                        idAct = (lstCatActividades.Where(act => act.id == obj.idAct && act.estado == true).Select(act => act.id)).FirstOrDefault(),
                        Tipo = _context.tblM_CatTipoActividad.Where(y => y.id == obj.idCatTipoActividad).Select(y => y.descripcion).FirstOrDefault(),
                        idTipo = obj.idCatTipoActividad,
                        PM = obj.idPM,
                        idformato = RutaFormato(obj.idAct, obj.modeloEquipoID, obj.estado, obj.idPM, obj.idCatTipoActividad, obj.idDN),
                        leyenda = obj.leyenda,
                        // Componente = VinCompo(obj.modeloEquipoID, obj.idAct, obj.idCatTipoActividad, obj.idPM),//obtiene el id del componente vinculado un listado
                        perioricidad = obj.perioricidad,//perioricidad con la cual se lleva la actividad 21/05/18S
                        DN = obj.idDN//control db 16/06/18
                    };
                    lstResultado.Add(temp);
                }
            }
            catch (Exception e)
            {
            }
            return lstResultado;
        }

        /// <summary>
        /// Se encarga de obtener las actividades del modelo para poder hacer el guardaro de la informacion segun el pm que se encuentre activo.
        /// 
        /// </summary>
        public void getActividadesPMByModelo(int modeloID, int pmID)
        {
            var getInfoActiviadesPM = from A in _context.tblM_CatPM_CatActividadPM
                                      join B in _context.tblM_CatActividadPM
                                      on A.idAct equals B.id
                                      where A.modeloEquipoID == modeloID && (A.idPM == pmID || A.idPM == 0)
                                      select new
                                      {

                                          A.idAct,
                                          A.idPM,
                                          A.perioricidad,
                                          A.idDN,
                                          B.descripcionActividad,
                                          B.idCatTipoActividad

                                      };
        }

        private List<tblM_ComponenteMantenimiento> VinCompo(int modeloEquipoID, int idAct, int idCatActividad, int idPm)
        {
            List<tblM_ComponenteMantenimiento> lstobjCompMant = new List<tblM_ComponenteMantenimiento>();
            try
            {
                lstobjCompMant = _context.tblM_ComponenteMantenimiento.Where(x => x.idAct == idAct && x.modeloEquipoID == modeloEquipoID && x.estado == true && x.idCatTipoActividad == idCatActividad && x.idPM == idPm).ToList();
            }
            catch (Exception)
            {

                throw;
            }
            return lstobjCompMant;
        }
        private tblM_DocumentosMaquinaria RutaFormato(int idAct, int modeloEquipoID, bool estado, int idpm, int idcatTipoAct, int idDN)
        {
            tblM_DocumentosMaquinaria objDocumentoRuta = new tblM_DocumentosMaquinaria();
            tblM_FormatoManteniento objFormatoDoc = new tblM_FormatoManteniento();

            idpm = idDN == 0 ? idpm : 0;
            try
            {
                objFormatoDoc = _context.tblM_FormatoManteniento.Where(x => x.idAct == idAct && x.modeloEquipoID == modeloEquipoID && x.estado == true && x.idPM == idpm && x.idCatTipoActividad == idcatTipoAct && x.idDN == idDN).FirstOrDefault();
                if (objFormatoDoc != null)
                {
                    objDocumentoRuta = _context.tblM_DocumentosMaquinaria.Where(x => x.id == objFormatoDoc.DocumentosMaquinariaID && x.tipoArchivo == 10).FirstOrDefault();
                }
                else
                {
                    objDocumentoRuta.nombreRuta = objDocumentoRuta.nombreRuta = "";
                }
            }
            catch (Exception)
            {

                throw;
            }


            return objDocumentoRuta;
        }
        public object tipoLeyenda(tblM_CatPM_CatActividadPM obj)
        {
            tblM_CatPM_CatActividadPM objCatPMActividad = new tblM_CatPM_CatActividadPM();
            IObjectSet<tblM_CatPM_CatActividadPM> _objectSetCatVincular = ((IObjectContextAdapter)_context).ObjectContext.CreateObjectSet<tblM_CatPM_CatActividadPM>();
            try
            {
                //tipoLeyenda
                objCatPMActividad = _objectSetCatVincular.Where(y => y.id == obj.id && y.estado == true).FirstOrDefault();
                objCatPMActividad.leyenda = obj.leyenda;
                _context.SaveChanges();
            }
            catch (Exception e)
            {
                //result = e.ToString();
            }
            return objCatPMActividad;
        }
        public object VinculaNuevaActividad(tblM_CatPM_CatActividadPM objActContPM)
        {
            var Resultado = new object();
            IObjectSet<tblM_CatPM_CatActividadPM> _objectSetCatVincular = ((IObjectContextAdapter)_context).ObjectContext.CreateObjectSet<tblM_CatPM_CatActividadPM>();
            try
            {
                var exist = _context.tblM_CatActividadPM.Where(X => X.id == objActContPM.idAct).FirstOrDefault();//validacion si existe en el catalogo
                if (exist != null)
                {
                    if (objActContPM.idPM != 0 && objActContPM.idDN == 0)
                    {
                        var Orden = _objectSetCatVincular.Where(y => y.idPM == objActContPM.idPM && y.estado == true && y.modeloEquipoID == objActContPM.modeloEquipoID).OrderByDescending(x => x.orden).Take(1).Select(x => x.orden).FirstOrDefault();
                        objActContPM.orden = (Orden + 1);
                    }
                    else if (objActContPM.idDN != 0)
                    {
                        var Orden = _objectSetCatVincular.Where(y => y.idDN == objActContPM.idDN && y.estado == true && y.modeloEquipoID == objActContPM.modeloEquipoID).OrderByDescending(x => x.orden).Take(1).Select(x => x.orden).FirstOrDefault();
                        objActContPM.orden = (Orden + 1);
                    }
                    else if (objActContPM.idCatTipoActividad != 0)
                    {
                        var Orden = _objectSetCatVincular.Where(y => y.idCatTipoActividad == objActContPM.idCatTipoActividad && y.estado == true && y.modeloEquipoID == objActContPM.modeloEquipoID).OrderByDescending(x => x.orden).Take(1).Select(x => x.orden).FirstOrDefault();
                        //var Orden = _objectSetCatVincular.Where(y => y.idCatTipoActividad == objActContPM.idCatTipoActividad && y.estado == true && y.modeloEquipoID == objActContPM.modeloEquipoID).OrderByDescending(x => x.orden).Take(1).FirstOrDefault();
                        objActContPM.orden = (Orden + 1);
                        //objActContPM.orden = (Orden + 1);
                    }
                    else
                    {
                        var Orden = _objectSetCatVincular.Where(y => y.idCatTipoActividad == objActContPM.idAct && y.estado == true && y.modeloEquipoID == objActContPM.modeloEquipoID).OrderByDescending(x => x.orden).Take(1).Select(x => x.orden).FirstOrDefault();
                        objActContPM.orden = (Orden + 1);
                    }
                    objActContPM.leyenda = false;
                    objActContPM.estado = true;
                    _objectSetCatVincular.AddObject(objActContPM);
                    _context.SaveChanges();

                    SaveBitacora((int)BitacoraEnum.AgrupacionEsquemas, (int)AccionEnum.AGREGAR, objActContPM.id, JsonUtils.convertNetObjectToJson(objActContPM));
                }
            }
            catch (Exception e)
            {
                //result = e.ToString();
            }
            return objActContPM;
        }
        public bool ActividadExistente(tblM_CatPM_CatActividadPM objActContPM)//valida si existe la actividad
        {
            bool ExisteActividadRel = false;
            int idAct = 0;
            try
            {
                if (objActContPM.idPM != 0)
                {
                    idAct = _context.tblM_CatPM_CatActividadPM.Where(y => y.idPM == objActContPM.idPM && y.estado == true && y.modeloEquipoID == objActContPM.modeloEquipoID && y.idAct == objActContPM.idAct).Take(1).Select(x => x.idAct).FirstOrDefault();
                }
                else if (objActContPM.idDN != 0)
                {
                    idAct = _context.tblM_CatPM_CatActividadPM.Where(y => y.idDN == objActContPM.idDN && y.estado == true && y.modeloEquipoID == objActContPM.modeloEquipoID && y.idAct == objActContPM.idAct).Take(1).Select(x => x.idAct).FirstOrDefault();
                }
                else if (objActContPM.idCatTipoActividad != 0)
                {
                    idAct = _context.tblM_CatPM_CatActividadPM.Where(y => y.idCatTipoActividad == objActContPM.idCatTipoActividad && y.estado == true && y.modeloEquipoID == objActContPM.modeloEquipoID && y.idAct == objActContPM.idAct).Take(1).Select(x => x.idAct).FirstOrDefault();
                }
            }
            catch (Exception e)
            {

            }
            if (idAct != 0)
            {
                ExisteActividadRel = true;
            }
            return ExisteActividadRel;
        }
        public object VincularMis(tblM_MiscelaneoMantenimiento obj)
        {
            var Resultado = new object();
            IObjectSet<tblM_MiscelaneoMantenimiento> _objectSetMis = ((IObjectContextAdapter)_context).ObjectContext.CreateObjectSet<tblM_MiscelaneoMantenimiento>();
            try
            {
                if (obj.id == 0)
                {
                    _objectSetMis.AddObject(obj);
                    _context.SaveChanges();

                }
                else
                {
                    tblM_MiscelaneoMantenimiento objMisc = new tblM_MiscelaneoMantenimiento();
                    objMisc = _context.tblM_MiscelaneoMantenimiento.Where(x => x.id == obj.id).FirstOrDefault();
                    objMisc.estado = false;
                    _context.SaveChanges();
                }
            }
            catch (Exception e)
            {
                //result = e.ToString();
            }
            return Resultado;
        }
        public object VincularEdadMis(int id, int edad)
        {
            var Resultado = new object();
            tblM_MiscelaneoMantenimiento objMisMant = new tblM_MiscelaneoMantenimiento();
            try
            {
                IObjectSet<tblM_MiscelaneoMantenimiento> _objectSetMis = ((IObjectContextAdapter)_context).ObjectContext.CreateObjectSet<tblM_MiscelaneoMantenimiento>();
                objMisMant = _context.tblM_MiscelaneoMantenimiento.Where(x => x.id == id).FirstOrDefault();
                objMisMant.vida = edad;
                _context.SaveChanges();
            }
            catch (Exception e)
            {
                //result = e.ToString();
            }
            return Resultado;
        }
        public object VincularCantidadMis(int id, int cantidad)
        {
            var Resultado = new object();
            tblM_MiscelaneoMantenimiento objMisMant = new tblM_MiscelaneoMantenimiento();
            try
            {
                IObjectSet<tblM_MiscelaneoMantenimiento> _objectSetMis = ((IObjectContextAdapter)_context).ObjectContext.CreateObjectSet<tblM_MiscelaneoMantenimiento>();
                objMisMant = _context.tblM_MiscelaneoMantenimiento.Where(x => x.id == id).FirstOrDefault();
                objMisMant.cantidad = cantidad;
                _context.SaveChanges();
            }
            catch (Exception e)
            {
                //result = e.ToString();
            }
            return Resultado;
        }
        public object VincularEdadAct(int id, int edad)
        {
            var Resultado = new object();
            tblM_CatPM_CatActividadPM objAct = new tblM_CatPM_CatActividadPM();
            try
            {
                IObjectSet<tblM_CatPM_CatActividadPM> _objectSetAct = ((IObjectContextAdapter)_context).ObjectContext.CreateObjectSet<tblM_CatPM_CatActividadPM>();
                objAct = _context.tblM_CatPM_CatActividadPM.Where(x => x.id == id).FirstOrDefault();
                objAct.perioricidad = edad;
                _context.SaveChanges();
            }
            catch (Exception e)
            {
            }
            return Resultado;
        }
        public List<tblM_CatTipoActividad> getTipoActividad(bool estatus)
        {
            return _context.tblM_CatTipoActividad.Where(X => X.estado == true).ToList();
        }
        public List<tblM_CatTipoMaquinaria> FillCboTipoMaquinaria(bool estatus)
        {
            return _context.tblM_CatTipoMaquinaria.Where(x => x.estatus == estatus/* && x.id == 1*/).ToList();
        }
        public List<tblM_CatModeloEquipo> FillCboModelo_Maquina(int idTipo)
        {
            return _context.tblM_CatModeloEquipo.Where(x => x.idGrupo == idTipo && x.estatus == true).ToList();
        }
        public List<tblM_CatMaquina> FillCboEconomicos(int grupo, int usuarioID)
        {
            var listadoCCbyUS = RetornoCentrCostoAcceso(usuarioID);
            List<tblM_CatMaquina> lstobjCatMaquina = new List<tblM_CatMaquina>();

            foreach (var ccAsignado in listadoCCbyUS)
            {

                var query = (from p in _context.tblM_CatMaquina.Where(x => x.centro_costos == ccAsignado && x.estatus != 0)
                             join c in _context.tblM_MatenimientoPm.Where(y => y.estatus == true)
                             on p.id equals c.idMaquina into g
                             from c in g.DefaultIfEmpty()
                             select new
                             {
                                 p.id,
                                 CID = c != null ? (int?)c.idMaquina : 0,
                                 p.noEconomico,
                                 e = c != null ? c.estatus : true,
                                 g = p != null ? p.grupoMaquinariaID : 0,
                             }).ToList().Where(x => x.g == grupo).ToList();

                foreach (var item in query)
                {

                    if (((item.CID == 0 && item.e == true) || (item.CID != 0 && item.e == false)) && grupo == item.g)
                    {
                        tblM_CatMaquina objCatMaquina = new tblM_CatMaquina();
                        objCatMaquina.noEconomico = item.noEconomico;
                        objCatMaquina.id = item.id;
                        lstobjCatMaquina.Add(objCatMaquina);
                    }

                }
            }
            return lstobjCatMaquina;

        }
        public List<string> RetornoCentrCostoAcceso(int usuarioID)
        {
            List<string> lstCcAcceso = _context.tblP_CC_Usuario.Where(y => y.usuarioID == usuarioID).Select(x => x.cc).ToList();
            return lstCcAcceso;
        }
        public int GuardarDoc(tblM_DocumentosMaquinaria obj)
        {
            IObjectSet<tblM_DocumentosMaquinaria> _objectSet = ((IObjectContextAdapter)_context).ObjectContext.CreateObjectSet<tblM_DocumentosMaquinaria>();
            _objectSet.AddObject(obj);
            _context.SaveChanges();
            return obj.id;
        }
        public object GuardarDocFormat(tblM_FormatoManteniento obj)
        {
            var result = new object();
            IObjectSet<tblM_FormatoManteniento> _objectSet = ((IObjectContextAdapter)_context).ObjectContext.CreateObjectSet<tblM_FormatoManteniento>();

            try
            {//realizar validacion si existe un formato relacionada a la actividad
                _objectSet.AddObject(obj);
                _context.SaveChanges();
            }
            catch (Exception)
            {

                throw;
            }
            return result;
        }
        public tblM_DocumentosMaquinaria GetObjRutaDocumentobyID(int objIdFormato)
        {
            return _context.tblM_DocumentosMaquinaria.FirstOrDefault(x => x.id.Equals(objIdFormato) && x.tipoArchivo == 10);
        }
        public object VincularComponete(tblM_ComponenteMantenimiento obj)
        {
            tblM_ComponenteMantenimiento objCat = new tblM_ComponenteMantenimiento();
            objCat = _context.tblM_ComponenteMantenimiento.Where(x => x.idAct == obj.idAct && x.modeloEquipoID == obj.modeloEquipoID && x.idCompVis == obj.idCompVis && x.estado == true).FirstOrDefault();
            if (objCat != null && obj.estado == false)//busca si el componente se encuentra ligado alguna actividad
            {
                List<tblM_MiscelaneoMantenimiento> lstobjMisc = new List<tblM_MiscelaneoMantenimiento>();
                lstobjMisc = _context.tblM_MiscelaneoMantenimiento.Where(x => x.estado == true && x.idAct == obj.idAct && x.idCompVis == obj.idCompVis).ToList();
                foreach (var item in lstobjMisc)
                {
                    item.estado = false;
                }
                objCat.estado = false;
                _context.SaveChanges();
            }
            else
            {
                IObjectSet<tblM_ComponenteMantenimiento> _objectSetCatVincular = ((IObjectContextAdapter)_context).ObjectContext.CreateObjectSet<tblM_ComponenteMantenimiento>();
                try
                {
                    //componente actividad

                    _objectSetCatVincular.AddObject(obj);
                    _context.SaveChanges();
                }
                catch (Exception e)
                {
                }
            }

            return objCat;
        }
        public List<JGHisDTO> ConsultarJGHis(int idMantenimiento)
        {
            List<tblM_BitacoraControlAceiteMant> lstobjBitAceite = new List<tblM_BitacoraControlAceiteMant>();
            JGHisDTO temp = new JGHisDTO();
            List<JGHisDTO> lstResult = new List<JGHisDTO>();
            try
            {
                tblM_CatMaquina objMaq = new tblM_CatMaquina();
                var matenimiento = _context.tblM_MatenimientoPm.FirstOrDefault(x => x.id == idMantenimiento);
                string EconomicoID = matenimiento.economicoID;
                var listaMatenimiento = _context.tblM_MatenimientoPm.Where(x => x.economicoID == EconomicoID).ToList();
                var listaMantenimientoID = listaMatenimiento.Select(x => x.id).ToList();

                lstobjBitAceite = _context.tblM_BitacoraControlAceiteMant.Where(x => x.idMant == idMantenimiento && x.estatus).GroupBy(x => x.idComp, (key, g) => g.OrderByDescending(e => e.idMant).FirstOrDefault()).ToList();

                //lstobjBitAceite = _context.tblM_BitacoraControlAceiteMant.Where(x => x.idMant == idMantenimiento).ToList();
                //string EconomicoID = _context.tblM_MatenimientoPm.Where(x => x.id == idMantenimiento).Select(y => y.economicoID).FirstOrDefault();

                objMaq = _context.tblM_CatMaquina.Where(x => x.noEconomico == EconomicoID).FirstOrDefault();
                int modeloEquipoID = objMaq.modeloEquipoID;
                foreach (var objAc in lstobjBitAceite)
                {
                    temp = new JGHisDTO
                    {
                        idhis = objAc.id,
                        componente = _context.tblM_CatComponentesViscosidades.Where(x => x.id == objAc.idComp).Select(y => y.Descripcion).ToList(),
                        AceiteVin = AceiteVinculado(objAc.idComp, modeloEquipoID, 0),
                        prueba = objAc.prueba,
                        vidaA = (objAc.Vigencia),
                        hrsAplico = (objAc.Hrsaplico),
                        aplico = objAc.Aplicado,
                        idComp = objAc.idComp
                        //revisar vigencia en busca de error
                    };
                    lstResult.Add(temp);
                }
            }
            catch (Exception)
            {

                throw;
            }
            return lstResult;
        }
        private List<lstAceiteDTO> AceiteVinculado1(int idComp, int modeloEquipoId, int idAct, int idmis)
        {
            List<int> lstIdAceite = new List<int>();
            List<lstAceiteDTO> lstAceite = new List<lstAceiteDTO>();

            var result = new object();
            try
            {

                //traer todos los idaceites Vinculados cuando sean idTipo2
                ////segun el tipo de actividad por la cual esta vinculada traer el orden
                lstIdAceite = _context.tblM_MiscelaneoMantenimiento.Where(x => x.idCompVis == idComp && x.idTipo == 2 && x.estado == true && x.idMis == idmis && x.modeloEquipoID == modeloEquipoId).Select(y => y.idMis).Distinct().ToList();
                if (lstIdAceite.Count() == 0)
                {
                    lstIdAceite = _context.tblM_MiscelaneoMantenimiento.Where(x => x.idCompVis == idComp && x.idTipo == 3 && x.estado == true).Select(y => y.idMis).Distinct().ToList();
                }
                foreach (var obj in lstIdAceite)
                {

                    if (obj == idmis)
                    {
                        var temp = new lstAceiteDTO
                        {
                            suministroID = _context.tblM_MiscelaneoMantenimiento.Where(x => x.idCompVis == idComp && x.estado == true && x.modeloEquipoID == modeloEquipoId && x.idMis == obj).FirstOrDefault().idMis,
                            descripcion = _context.tblM_CatSuministros.Where(x => x.id == obj).ToList(),
                            edadSuministro = _context.tblM_MiscelaneoMantenimiento.Where(x => x.idCompVis == idComp && x.estado == true && x.modeloEquipoID == modeloEquipoId && x.idMis == obj).ToList(),

                        };
                        if (temp != null)
                        {
                            lstAceite.Add(temp);
                        }
                    }

                }

            }
            catch (Exception)
            {
                throw;
            }
            return lstAceite;
        }
        public List<actividadesExtraHisDTO> ConsultarActividadesExtrashis(int idMantenimiento)
        {
            List<tblM_BitacoraControlActExt> lstobjBitActExt = new List<tblM_BitacoraControlActExt>();
            actividadesExtraHisDTO temp = new actividadesExtraHisDTO();
            List<actividadesExtraHisDTO> lstResult = new List<actividadesExtraHisDTO>();
            try
            {
                lstobjBitActExt = _context.tblM_BitacoraControlActExt.Where(x => x.idMant == idMantenimiento).ToList();
                foreach (var objAc in lstobjBitActExt)
                {
                    temp = new actividadesExtraHisDTO
                    {
                        actividad = _context.tblM_CatActividadPM.FirstOrDefault(x => x.id == objAc.idAct).descripcionActividad,
                        Hrsaplico = objAc.Hrsaplico,
                        perioricidad = 0,//_context.tblM_CatPM_CatActividadPM.FirstOrDefault(x => x.id == objAc.idPerioricidad).perioricidad,///Select(x => x.perioricidad).(),
                        aplico = objAc.Aplicado
                    };
                    lstResult.Add(temp);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return lstResult;
        }
        public List<JGEstructuraDTO> ConsultarJGEstructura(int modeloEquipoID)
        {
            object result = new object();
            List<JGEstructuraDTO> lstResult = new List<JGEstructuraDTO>();
            try
            {
                //recuerda roberto...¬¬ se tiene que tipiear cuando marca el error de que el ya hay un datareader abierto para el command "nada de var"
                List<int> lstObjCompMant = new List<int>();
                lstObjCompMant = _context.tblM_ComponenteMantenimiento.Where(x => x.modeloEquipoID == modeloEquipoID && x.estado == true && x.idCatTipoActividad == 2).Select(m => m.idCompVis).Distinct().ToList();
                List<tblM_ComponenteMantenimiento> lstObjComponente = new List<tblM_ComponenteMantenimiento>();
                tblM_ComponenteMantenimiento objComponente = new tblM_ComponenteMantenimiento();
                foreach (var item in lstObjCompMant)
                {
                    objComponente = _context.tblM_ComponenteMantenimiento.Where(x => x.modeloEquipoID == modeloEquipoID && x.estado == true && x.idCompVis == item).FirstOrDefault();
                    lstObjComponente.Add(objComponente);
                }
                //lstObjComponente = _context.tblM_ComponenteManteni miento.Where(x => x.modeloEquipoID == modeloEquipoID && x.estado == true).Distinct().ToList();
                //foreach (var idComp in lstObjCompMant)
                foreach (var idComp in lstObjComponente)
                {
                    var temp = new JGEstructuraDTO
                    {
                        idComponente = idComp,
                        componente = _context.tblM_CatComponentesViscosidades.Where(x => x.id == idComp.idCompVis).Select(y => y.Descripcion).ToList(),
                        AceiteVin = AceiteVinculado(idComp.idCompVis, modeloEquipoID, idComp.idAct),
                        Icon = _context.tblM_IconMantenimiento.Where(x => x.idComp == idComp.idCompVis).Select(y => y.icon).ToList()
                    };
                    lstResult.Add(temp);
                }

            }
            catch (Exception)
            {

                throw;
            }
            return lstResult;
        }

        public List<dataSetLubProxDTO> ConsultarJGEstructura2(int modeloEquipoID)
        {
            object result = new object();
            List<dataSetLubProxDTO> lstResult = new List<dataSetLubProxDTO>();
            try
            {
                //recuerda roberto...¬¬ se tiene que tipiear cuando marca el error de que el ya hay un datareader abierto para el command "nada de var"
                List<int> lstObjCompMant = new List<int>();
                //      lstObjCompMant = _context.tblM_ComponenteMantenimiento.Where(x => x.modeloEquipoID == modeloEquipoID && x.estado == true && x.idCatTipoActividad == 2).Select(m => m.idCompVis).Distinct().ToList();

                lstObjCompMant = _context.tblM_PMComponenteModelo.Where(x => x.modeloID == modeloEquipoID).Select(x => x.componenteID).Distinct().ToList();

                List<tblM_ComponenteMantenimiento> lstObjComponente = new List<tblM_ComponenteMantenimiento>();
                tblM_ComponenteMantenimiento objComponente = new tblM_ComponenteMantenimiento();

                var iconos = _context.tblM_IconMantenimiento.FirstOrDefault();
                foreach (var idComp in lstObjCompMant)
                {
                    var aceitesVinculados = AceiteVinculado(idComp, modeloEquipoID, idComp);
                    var componente = _context.tblM_CatComponentesViscosidades.FirstOrDefault(x => x.id == idComp);
                    var temp = new dataSetLubProxDTO
                    {
                        componenteMantenimiento = new componenteMantenimiento { idCompVis = idComp },
                        aceiteDTO = aceitesVinculados == null ? new List<lstAceiteDTO>() : aceitesVinculados.Where(a => a.componenteID == idComp).ToList(),
                        icono = iconos == null ? "" : iconos.icon,
                        descripcion = componente == null ? "" : componente.Descripcion
                    };
                    lstResult.Add(temp);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return lstResult;
        }

        public Dictionary<string, object> GetModeloEconomico(int idNoEconomico)
        {
            Dictionary<string, object> resultado = new Dictionary<string, object>();
            using (var _ctx = new MainContext(vSesiones.sesionEmpresaActual))
            {
                try
                {
                    if (idNoEconomico <= 0) { throw new Exception("Ocurrió un error al obtener el modelo del equipo"); }

                    int idModelo = _ctx.Select<int>(new DapperDTO
                    {
                        baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
                        consulta = @"SELECT t2.id AS idModelo
	                                        FROM tblM_CatMaquina AS t1 
	                                        INNER JOIN tblM_CatModeloEquipo AS t2 ON t2.id = t1.modeloEquipoID
		                                        WHERE t1.id = @idNoEconomico",
                        parametros = new { idNoEconomico = idNoEconomico }
                    }).FirstOrDefault();

                    resultado.Add(SUCCESS, true);
                    resultado.Add(ITEMS, idModelo);
                }
                catch (Exception e)
                {
                    var NOMBRE_FUNCION = System.Reflection.MethodBase.GetCurrentMethod().Name;
                    LogError(_SISTEMA, 0, _NOMBRE_CONTROLADOR, NOMBRE_FUNCION, e, AccionEnum.CONSULTA, 0, new { idNoEconomico = idNoEconomico });
                    resultado.Add(SUCCESS, false);
                    resultado.Add(MESSAGE, e.Message);
                }
            }
            return resultado;
        }

        public List<actividadesExtraDTO> ConsultarActividadesExtras(int modeloEquipoID)
        {
            object result = new object();
            List<tblM_CatPM_CatActividadPM> lstobjActividadModelo = new List<tblM_CatPM_CatActividadPM>();
            List<tblM_CatActividadPM> lstCatActividades = new List<tblM_CatActividadPM>();
            lstCatActividades = _context.tblM_CatActividadPM.ToList();
            lstobjActividadModelo = _context.tblM_CatPM_CatActividadPM.Where(x => (x.modeloEquipoID == modeloEquipoID) && x.estado == true && x.idCatTipoActividad == 3).ToList();//modificacion para obtener solo actividades extras
            var lstResultado = new List<actividadesExtraDTO>();
            try
            {
                foreach (var obj in lstobjActividadModelo)
                {
                    var temp = new actividadesExtraDTO
                    {
                        id = obj.id,
                        orden = obj.orden,
                        descripcion = (lstCatActividades.FirstOrDefault(act => act.id == obj.idAct && act.estado == true).descripcionActividad), //Select(act => act.descripcionActividad)).FirstOrDefault(),
                        idAct = (lstCatActividades.FirstOrDefault(act => act.id == obj.idAct && act.estado == true).id),
                        Tipo = _context.tblM_CatTipoActividad.FirstOrDefault(y => y.id == obj.idCatTipoActividad).descripcion,
                        idTipo = obj.idCatTipoActividad,
                        PM = obj.idPM,
                        idformato = RutaFormato(obj.idAct, obj.modeloEquipoID, obj.estado, obj.idPM, obj.idCatTipoActividad, obj.idDN),
                        leyenda = obj.leyenda,
                        Componente = VinCompo(obj.modeloEquipoID, obj.idAct, obj.idCatTipoActividad, obj.idPM),//obtiene el id del componente vinculado un listado
                        perioricidad = obj.perioricidad//perioricidad con la cual se lleva la actividad 21/05/18S
                    };
                    lstResultado.Add(temp);
                }
            }
            catch (Exception e)
            {
            }
            return lstResultado;


        }
        public List<ActividadesDNDTO> ConsultarActividadesDN(int modeloEquipoID, int idPM = 1)
        {
            ActividadesDNDTO result = new ActividadesDNDTO();
            List<tblM_CatPM_CatActividadPM> lstobjActividadModelo = new List<tblM_CatPM_CatActividadPM>();
            List<tblM_CatActividadPM> lstCatActividades = new List<tblM_CatActividadPM>();
            lstCatActividades = _context.tblM_CatActividadPM.ToList();
            lstobjActividadModelo = _context.tblM_CatPM_CatActividadPM.Where(x => (x.modeloEquipoID == modeloEquipoID) && x.estado == true && x.idCatTipoActividad == 4 && x.idPM == idPM - 1).ToList();//modificacion para obtener solo actividades extras
            var lstResultado = new List<ActividadesDNDTO>();
            try
            {
                foreach (var obj in lstobjActividadModelo)
                {
                    var temp = new ActividadesDNDTO
                    {
                        id = obj.id,
                        orden = obj.orden,
                        descripcion = (lstCatActividades.FirstOrDefault(act => act.id == obj.idAct && act.estado == true).descripcionActividad), //Select(act => act.descripcionActividad)).FirstOrDefault(),
                        idAct = (lstCatActividades.FirstOrDefault(act => act.id == obj.idAct && act.estado == true).id), //.Select(act => act.id)).FirstOrDefault(),
                        Tipo = _context.tblM_CatTipoActividad.FirstOrDefault(y => y.id == obj.idCatTipoActividad).descripcion, //.Select(y => y.descripcion).FirstOrDefault(),
                        idTipo = obj.idCatTipoActividad,
                        PM = obj.idPM,
                        idformato = RutaFormato(obj.idAct, obj.modeloEquipoID, obj.estado, obj.idPM, obj.idCatTipoActividad, obj.idDN),
                        leyenda = obj.leyenda,
                        Componente = VinCompo(obj.modeloEquipoID, obj.idAct, obj.idCatTipoActividad, obj.idPM),//obtiene el id del componente vinculado un listado
                        //perioricidad = obj.perioricidad//perioricidad con la cual se lleva la actividad 21/05/18S
                        perioricidad = obtenerperioricidadDn(obj.idAct, modeloEquipoID, obj.idPM)//trae las id de las actividades del dn2 y busca en las demas si se encuentra
                    };
                    lstResultado.Add(temp);
                }
            }
            catch (Exception e)
            {
            }
            return lstResultado;


        }
        public List<ActividadesDNHisDTO> ConsultarActividadesDNhis(int idMantenimiento)
        {
            ActividadesDNHisDTO temp = new ActividadesDNHisDTO();
            object result = new object();
            List<ActividadesDNHisDTO> lstresult = new List<ActividadesDNHisDTO>();
            List<tblM_BitacoraControlDN> lstobjBitDN = new List<tblM_BitacoraControlDN>();
            lstobjBitDN = _context.tblM_BitacoraControlDN.Where(x => x.idMant == idMantenimiento).ToList();
            try
            {
                string Economico = _context.tblM_MatenimientoPm.Where(x => x.id == idMantenimiento).Select(x => x.economicoID).FirstOrDefault();
                //var lstMantenimientos = _context.tblM_MatenimientoPm.Where(x => x.economicoID == Economico).Select(x => x.id).ToList();
                //var lstobjBitDNHis = _context.tblM_BitacoraControlDN.Where(x => lstMantenimientos.Contains(x.idMant)).OrderByDescending(x => x.fechaCaptura).GroupBy(x => x.idAct).Select(x => new {
                //    idAct = x.FirstOrDefault().idAct,
                //    Hrsaplico = x.FirstOrDefault().Hrsaplico,
                //    Aplicado = x.FirstOrDefault().Aplicado,
                //}).ToList();
                var lstobjBitDNHis = _context.tblM_BitacoraControlDN.Where(x => x.idMant == idMantenimiento).OrderByDescending(x => x.fechaCaptura).GroupBy(x => x.idAct).Select(x => new
                {
                    idAct = x.FirstOrDefault().idAct,
                    Hrsaplico = x.FirstOrDefault().Hrsaplico,
                    Aplicado = x.FirstOrDefault().Aplicado,
                }).ToList();
                //traer la perioricidad para cada idDN logica raguilar
                foreach (var obj in lstobjBitDNHis)
                {
                    temp = new ActividadesDNHisDTO
                    {
                        actividad = _context.tblM_CatActividadPM.FirstOrDefault(x => x.id == obj.idAct).descripcionActividad,
                        Hrsaplico = obj.Hrsaplico,
                        perioricidad = obtenerperioricidadDn(obj.idAct, ConsultarModelo(Economico), 1),
                        //perioricidad = _context.tblM_CatPM_CatActividadPM.Where(x => x.id == obj.idPerioricidad).Select(x => x.perioricidad).FirstOrDefault(),
                        aplico = obj.Aplicado
                    };
                    lstresult.Add(temp);
                }
            }
            catch (Exception e)
            {
            }
            return lstresult;
        }
        private int obtenerperioricidadDn(int idActividad, int modeloEquipoID, int idPM)
        {
            int Perioricidad = 0;
            try
            {
                var catActividadPM = _context.tblM_CatPM_CatActividadPM.Where(x => x.modeloEquipoID == modeloEquipoID && x.estado && x.idCatTipoActividad == 4 && x.idAct == idActividad/* && x.idPM == idPM*/).FirstOrDefault();
                if (catActividadPM != null && catActividadPM.perioricidad != 0) { Perioricidad = catActividadPM.perioricidad; }
                else
                {
                    switch (idPM)
                    {
                        case 1:
                            Perioricidad = 500;
                            break;
                        case 2:
                            Perioricidad = 1000;
                            break;
                        case 3:
                            Perioricidad = 2000;
                            break;
                        default:
                            Perioricidad = 250;
                            break;
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return Perioricidad;
        }
        private List<lstAceiteDTO> AceiteVinculado(int idComp, int modeloEquipoId, int idAct)
        {
            List<int> lstIdAceite = new List<int>();
            List<lstAceiteDTO> lstAceite = new List<lstAceiteDTO>();
            var result = new object();
            try
            {
                //traer todos los idaceites Vinculados cuando sean idTipo2
                ////segun el tipo de actividad por la cual esta vinculada traer el orden

                var lstAceiteRaw = _context.tblM_PMComponenteLubricante.Where(x => x.modeloID == modeloEquipoId && x.componenteID == idComp).ToList();

                foreach (var obj in lstAceiteRaw.Select(x => x.lubricanteID).ToList().Distinct())
                {

                    List<tblM_MiscelaneoMantenimiento> lstESuministros = new List<tblM_MiscelaneoMantenimiento>();
                    foreach (var objESuministro in lstAceiteRaw.Where(x => x.lubricanteID == obj))
                    {
                        tblM_MiscelaneoMantenimiento objRaw = new tblM_MiscelaneoMantenimiento();
                        objRaw.id = 0;
                        objRaw.modeloEquipoID = objESuministro.modeloID;
                        objRaw.idAct = 0;
                        objRaw.estado = objESuministro.estatus;
                        objRaw.idCompVis = objESuministro.componenteID;
                        objRaw.idTipo = 0;
                        objRaw.idMis = objESuministro.lubricanteID;
                        objRaw.vida = objESuministro.vidaLubricante;
                        objRaw.cantidad = objESuministro.cantidadLitros;
                        objRaw.UsuarioCap = objESuministro.usuarioID;
                        objRaw.fechaCaptura = objESuministro.fechaCaptura;
                        lstESuministros.Add(objRaw);
                    }

                    var temp = new lstAceiteDTO
                    {
                        suministroID = obj,
                        componenteID = idComp,
                        descripcion = _context.tblM_CatSuministros.Where(x => x.id == obj).ToList(),
                        edadSuministro = lstESuministros ///_context.tblM_MiscelaneoMantenimiento.Where(x => x.idCompVis == idComp && x.estado == true && x.modeloEquipoID == modeloEquipoId && x.idMis == obj).ToList()
                    };
                    if (temp != null)
                    {
                        lstAceite.Add(temp);
                    }
                }

            }
            catch (Exception)
            {
                throw;
            }
            return lstAceite;
        }
        public int ConsultarModelo(string noEconomico)
        {
            int result = new int();
            try
            {
                result = _context.tblM_CatMaquina.Where(x => x.noEconomico == noEconomico).Select(y => y.modeloEquipoID).FirstOrDefault();
            }
            catch (Exception)
            {
                throw;
            }
            return result;
        }
        public void ProgramaActividades(int idmantenimiento)
        {
            try
            {
                tblM_RenderFullCalendar objRenderCl = new tblM_RenderFullCalendar();
                objRenderCl = _context.tblM_RenderFullCalendar.Where(x => x.idMantenimiento == idmantenimiento).FirstOrDefault();
                objRenderCl.borderColor = "blue";
                _context.SaveChanges();
            }
            catch (Exception)
            {
                throw;
            }
        }
        public void EliminarVincDoc(int idActividad, int modelo)
        {
            try
            {
                EliminarFormatoVinculado(idActividad, modelo);//eliminar formato vinculado
            }
            catch (Exception)
            {
                throw;
            }

        }
        public List<object> ConsultarBitacora(string noEconomico)
        {
            List<object> result = new List<object>();
            //int result = new int();
            try
            {
                List<tblM_BitacoraControlAceiteMant> lstObjBitacoraCOntrolAceiteMant = new List<tblM_BitacoraControlAceiteMant>();


                object Filtros = new object();
                object ActividadesExtras = new object();
                //PRIMERO TRAER EL ULTIMO MANTENIMIENTO REALIZADO
                int IDmantenimiento = _context.tblM_MatenimientoPm.Where(X => X.economicoID == noEconomico && X.estatus == true).Select(y => y.id).FirstOrDefault();
                //traer la bitacora Aceite
                lstObjBitacoraCOntrolAceiteMant = _context.tblM_BitacoraControlAceiteMant.Where(x => x.idMant == IDmantenimiento && x.estatus).ToList();
                Filtros = _context.tblM_BitacoraControlAceiteMant.Where(x => x.idMant == IDmantenimiento && x.estatus).ToList();
                ///TRAE LA BITACORA DE ACTIVIDADES EXTRAS
                ActividadesExtras = _context.tblM_BitacoraControlActExt.Where(X => X.idMant == IDmantenimiento).FirstOrDefault();
                foreach (var item in lstObjBitacoraCOntrolAceiteMant)
                {
                    var temp = new
                    {
                        Hrsaplico = item.Hrsaplico,
                        VidaRestante = item.VidaRestante,
                        Vigencia = item.Vigencia,
                        Componente = _context.tblM_CatComponentesViscosidades.Where(y => y.id == item.idComp).Select(x => x.Descripcion),
                    };
                }
            }
            catch (Exception)
            {
                throw;
            }
            return result;
        }
        public List<tblM_BitacoraControlAceiteMantProy> CargaDeProyectado(int idmantenimiento)
        {
            List<tblM_BitacoraControlAceiteMantProy> result = new List<tblM_BitacoraControlAceiteMantProy>();
            try
            {
                //var maquinaID = _context.tblM_MatenimientoPm.FirstOrDefault(x => x.id == idmantenimiento).idMaquina;
                //var mantenimientosMaquina = _context.tblM_MatenimientoPm.Where(x => x.idMaquina == maquinaID).Select(x => x.id).ToList();
                //result = _context.tblM_BitacoraControlAceiteMantProy.Where(x => x.idComp != 0 && mantenimientosMaquina.Contains(x.idMant)).GroupBy(x => x.idComp, (key, g) => g.OrderByDescending(e => e.idMant).FirstOrDefault()).ToList();
                result = _context.tblM_BitacoraControlAceiteMantProy.Where(x => x.idMant == idmantenimiento && x.estatus && x.idComp != 0).Distinct().ToList();
            }
            catch (Exception)
            {
                throw;
            }
            return result;
        }
        public object CargaLubObs(int idMantProy)
        {
            object result = new object();

            try
            {
                result = _context.tblM_BitacoraControlAceiteMantProy.Where(x => x.id == idMantProy).Distinct();
            }
            catch (Exception)
            {
                throw;
            }
            return result;
        }

        public List<object> ConsultarGestorPM(int modeloEquipoID, int idPM, int idmantenimiento)
        {
            object result = new object();
            List<object> lstresult = new List<object>();
            try
            {
                List<tblM_CatPM_CatActividadPM> lstObjActividadPm = new List<tblM_CatPM_CatActividadPM>();
                if (modeloEquipoID == 0)
                {
                    var maquina = (from mp in _context.tblM_MatenimientoPm
                                   join e in _context.tblM_CatMaquina
                                   on mp.economicoID equals e.noEconomico
                                   where mp.id == idmantenimiento && e.estatus != 0
                                   select e).FirstOrDefault().modeloEquipoID;

                    modeloEquipoID = maquina;

                }


                lstObjActividadPm = _context.tblM_CatPM_CatActividadPM
                    .Where(x => /*x.idPM == idPM &&*/ x.modeloEquipoID == modeloEquipoID && x.idDN == 0 && x.estado == true && x.perioricidad <= 0 && x.idCatTipoActividad == 1).ToList();
                var idObjActividadPm = lstObjActividadPm.Select(x => x.idAct).ToList();
                List<tblM_BitacoraActividadesMantProy> lstObjBitActProy = new List<tblM_BitacoraActividadesMantProy>();
                //corregir logica deje el dn como para identificar por el idpm = iddn =1 SOLO lo identifica la idcatipode actividad
                var mantenimiento = _context.tblM_MatenimientoPm.FirstOrDefault(x => x.id == idmantenimiento);
                var mantenimientoInicial = _context.tblM_MatenimientoPm.Where(x => x.economicoID == mantenimiento.economicoID).Select(x => x.id).ToList();
                var listaActividades = _context.tblM_BitacoraActividadesMantProy.Where(y => mantenimientoInicial.Contains(y.idMant) && idObjActividadPm.Contains(y.idAct) && y.estatus).ToList();

                var listActividadesIDs = listaActividades.Select(x => x.idAct).ToList();

                var catalogoActividades = _context.tblM_CatPM_CatActividadPM.Where(x => listActividadesIDs.Contains(x.idAct) && x.modeloEquipoID == modeloEquipoID && x.estado).ToList();
                var BitActProy = _context.tblM_BitacoraActividadesMantProy.Where(x => listActividadesIDs.Contains(x.idAct) && x.estatus == true).ToList();
                var lstCarActvidadesPM = _context.tblM_CatActividadPM.Where(x => listActividadesIDs.Contains(x.id) && x.estado == true).ToList();

                //.Select(x => x.id).t;
                if (listaActividades != null && listaActividades.Count() > 0)
                {
                    foreach (var Actividades in listaActividades)
                    {
                        var obj = catalogoActividades.FirstOrDefault(x => x.idAct == Actividades.idAct);
                        bool banderaExisteDato = false;
                        var ObjBitActProy = BitActProy.FirstOrDefault(x => x.idAct == Actividades.idAct);
                        if (ObjBitActProy == null) { banderaExisteDato = true; }
                        else
                        {
                            if (ObjBitActProy.estatus == true && ObjBitActProy.aplicar == true) { banderaExisteDato = true; }
                            else if (ObjBitActProy.estatus == true && ObjBitActProy.aplicar == false) { banderaExisteDato = false; }
                        }
                        if (obj != null /*&& obj.idPM == Actividades.idPm*/)
                        {
                            var temp = new
                            {

                                id = obj.idAct,
                                orden = obj.orden,
                                descripcion = (lstCarActvidadesPM.Where(x => x.id == Actividades.idAct).Select(y => y.descripcionActividad)).FirstOrDefault(),
                                PM = obj.idPM,
                                aplicar = banderaExisteDato, //ConsultaidActProy(Actividades.idAct),
                                idobjProy = Actividades.id,
                                Observacion = BitActProy.Where(y => y.idMant == idmantenimiento).Select(x => x.Observaciones).FirstOrDefault()
                            };
                            lstresult.Add(temp);
                        }
                    }
                }
                else
                {
                    var actividadesDefault = _context.tblM_CatPM_CatActividadPM.Where(x => x.modeloEquipoID == modeloEquipoID && x.estado && x.idPM == idPM).ToList();
                    foreach (var Actividad in actividadesDefault)
                    {
                        var obj = catalogoActividades.FirstOrDefault(x => x.idAct == Actividad.idAct);
                        bool banderaExisteDato = false;
                        var ObjBitActProy = BitActProy.FirstOrDefault(x => x.idAct == Actividad.idAct);
                        if (ObjBitActProy == null) { banderaExisteDato = true; }
                        else
                        {
                            if (ObjBitActProy.estatus == true && ObjBitActProy.aplicar == true) { banderaExisteDato = true; }
                            else if (ObjBitActProy.estatus == true && ObjBitActProy.aplicar == false) { banderaExisteDato = false; }
                        }
                        var temp = new
                        {

                            id = Actividad.idAct,
                            orden = Actividad.orden,
                            descripcion = (lstCarActvidadesPM.Where(x => x.id == Actividad.idAct).Select(y => y.descripcionActividad)).FirstOrDefault(),
                            PM = Actividad.idPM,
                            aplicar = banderaExisteDato, //ConsultaidActProy(Actividad.idAct),
                            idobjProy = Actividad.id,
                            Observacion = BitActProy.Where(y => y.idMant == idmantenimiento).Select(x => x.Observaciones).FirstOrDefault()
                        };
                        lstresult.Add(temp);
                    }
                }
            }
            catch (Exception)
            {

            }
            return lstresult;
        }
        public bool ConsultaidActProy(int idAct)//20/07/18 ragular 11:01am 
        {
            bool banderaExisteDato = false;
            tblM_BitacoraActividadesMantProy ObjBitActProy = new tblM_BitacoraActividadesMantProy();
            object result = "";
            try
            {
                ObjBitActProy = _context.tblM_BitacoraActividadesMantProy.Where(x => x.idAct == idAct && x.estatus == true).FirstOrDefault();
                if (ObjBitActProy == null)
                {
                    banderaExisteDato = true;
                }
                else
                {
                    if (ObjBitActProy.estatus == true && ObjBitActProy.aplicar == true)
                    {
                        banderaExisteDato = true;
                    }
                    else if (ObjBitActProy.estatus == true && ObjBitActProy.aplicar == false)
                    {
                        banderaExisteDato = false;
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return banderaExisteDato;
        }
        public object GuardarActividadesMantProy(tblM_BitacoraActividadesMantProy objActvProy)
        {
            object result = new object();
            IObjectSet<tblM_BitacoraActividadesMantProy> _objectSet = ((IObjectContextAdapter)_context).ObjectContext.CreateObjectSet<tblM_BitacoraActividadesMantProy>();
            try
            {
                if (objActvProy.id != 0)
                {
                    //buscar y eliminar la restinccion del tipo de la actividad del mismo pm
                    tblM_BitacoraActividadesMantProy obj = new tblM_BitacoraActividadesMantProy();
                    obj = _context.tblM_BitacoraActividadesMantProy.Where(x => x.id == objActvProy.id && x.estatus == true).FirstOrDefault();
                    //if (obj != null)
                    //{
                    obj.estatus = objActvProy.estatus;
                    obj.aplicar = objActvProy.aplicar;

                    //}
                    //else
                    //{
                    //tblM_BitacoraActividadesMantProy objBitAct = new tblM_BitacoraActividadesMantProy();
                    //objBitAct = _context.tblM_BitacoraActividadesMantProy.Where(x => x.id == objActvProy.id && x.estatus == true).FirstOrDefault();
                    //objBitAct.Observaciones = objActvProy.Observaciones;
                    //}
                }
                else
                {
                    _objectSet.AddObject(objActvProy);
                }
                _context.SaveChanges();
            }
            catch (Exception)
            {
                throw;
            }
            return result;
        }
        public bool EliminarActividadProy(tblM_BitacoraActividadesMantProy ObjAct)
        {
            try
            {
                var actividad = getActividadesProByID(ObjAct.id);
                _context.tblM_BitacoraActividadesMantProy.Remove(actividad);
                _context.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public object GuardarActividadProy(tblM_BitacoraActividadesMantProy ObjAct)
        {
            object result = new object();
            IObjectSet<tblM_BitacoraActividadesMantProy> _objectSet = ((IObjectContextAdapter)_context).ObjectContext.CreateObjectSet<tblM_BitacoraActividadesMantProy>();
            try
            {
                _objectSet.AddObject(ObjAct);
                _context.SaveChanges();
            }
            catch (Exception)
            {
                throw;
            }
            return result;
        }
        public List<object> ActOtroPm(int idMant)
        {
            List<tblM_BitacoraActividadesMantProy> result = new List<tblM_BitacoraActividadesMantProy>();
            List<object> lstResultado = new List<object>();
            try
            {
                result = _context.tblM_BitacoraActividadesMantProy.Where(x => x.idMant == idMant && x.estatus == true && x.idPm == 0).ToList();
                foreach (var obj in result)
                {
                    var temp = new
                    {
                        id = obj.idAct,
                        //orden = obj.orden,
                        descripcion = (_context.tblM_CatActividadPM.Where(x => x.id == obj.idAct && x.estado == true).Select(y => y.descripcionActividad)).FirstOrDefault(),
                        PM = obj.idPm,
                        aplicar = ConsultaidActProy(obj.idAct),
                        Observacion = _context.tblM_BitacoraActividadesMantProy.Where(y => y.idAct == obj.idAct && y.estatus == true).Select(x => x.Observaciones).FirstOrDefault(),
                        idobjProy = _context.tblM_BitacoraActividadesMantProy.Where(y => y.idAct == obj.idAct && y.estatus == true && y.idMant == idMant).Select(x => x.id).FirstOrDefault(),
                    };
                    lstResultado.Add(temp);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return lstResultado;
        }
        public string ConsultarObservacionActividad(int idObjAct)
        {
            string Observaciones = "";
            try
            {
                Observaciones = _context.tblM_BitacoraActividadesMantProy.Where(x => x.id == idObjAct && x.estatus == true && x.aplicar == true).Select(x => x.Observaciones).FirstOrDefault();
            }
            catch (Exception)
            {
                throw;
            }
            return Observaciones;
        }
        public tblM_BitacoraControlAEMantProy GuardarBitProyAE(tblM_BitacoraControlAEMantProy ObjBitProyAE)
        {

            IObjectSet<tblM_BitacoraControlAEMantProy> _objAEProy = ((IObjectContextAdapter)_context).ObjectContext.CreateObjectSet<tblM_BitacoraControlAEMantProy>();
            try
            {
                if (ObjBitProyAE.id == 0)
                {
                    ObjBitProyAE.fechaCaptura = DateTime.Now;
                    ObjBitProyAE.FechaServicio = DateTime.Now;

                    _objAEProy.AddObject(ObjBitProyAE);

                }
                else
                {
                    tblM_BitacoraControlAEMantProy obj = new tblM_BitacoraControlAEMantProy();
                    obj = _context.tblM_BitacoraControlAEMantProy.Where(x => x.id == ObjBitProyAE.id).FirstOrDefault();
                    obj.Hrsaplico = ObjBitProyAE.Hrsaplico;
                    obj.Vigencia = ObjBitProyAE.Vigencia;
                    obj.programado = ObjBitProyAE.programado;
                    obj.Observaciones = ObjBitProyAE.Observaciones;
                    obj.FechaServicio = ObjBitProyAE.FechaServicio;

                }
                _context.SaveChanges();
            }
            catch (Exception e)
            {
            }
            return ObjBitProyAE;
        }
        public tblM_BitacoraControlDNMantProy GuardarBitProyDN(tblM_BitacoraControlDNMantProy ObjBitProyDN)
        {

            IObjectSet<tblM_BitacoraControlDNMantProy> _ObjBitProyDN = ((IObjectContextAdapter)_context).ObjectContext.CreateObjectSet<tblM_BitacoraControlDNMantProy>();
            try
            {
                tblM_BitacoraControlDNMantProy objDN = new tblM_BitacoraControlDNMantProy();
                if (ObjBitProyDN.id != 0)
                {

                    objDN = _context.tblM_BitacoraControlDNMantProy.Where(x => x.id == ObjBitProyDN.id && x.estatus == true).FirstOrDefault();
                    objDN.FechaServicio = ObjBitProyDN.FechaServicio;
                    objDN.Hrsaplico = ObjBitProyDN.Hrsaplico;
                    objDN.idAct = ObjBitProyDN.idAct;
                    objDN.idMant = ObjBitProyDN.idMant;
                    objDN.Observaciones = ObjBitProyDN.Observaciones;
                    objDN.programado = ObjBitProyDN.programado;
                    objDN.Vigencia = ObjBitProyDN.Vigencia;
                }
                else
                {
                    _ObjBitProyDN.AddObject(ObjBitProyDN);
                }
                _context.SaveChanges();
            }
            catch (Exception e)
            {
            }
            return ObjBitProyDN;
        }
        public List<tblM_BitacoraControlAEMantProy> CargaDeAEProyectado(int idmantenimiento)
        {
            List<tblM_BitacoraControlAEMantProy> result = new List<tblM_BitacoraControlAEMantProy>();

            try
            {
                result = _context.tblM_BitacoraControlAEMantProy.Where(x => x.idMant == idmantenimiento && x.estatus == true).Distinct().ToList();
            }
            catch (Exception)
            {
                throw;
            }
            return result;
        }
        public List<tblM_BitacoraControlDNMantProy> CargaDeDNProyectado(int idmantenimiento)
        {
            List<tblM_BitacoraControlDNMantProy> result = new List<tblM_BitacoraControlDNMantProy>();

            try
            {
                result = _context.tblM_BitacoraControlDNMantProy.Where(x => x.idMant == idmantenimiento && x.estatus == true).Distinct().ToList();
            }
            catch (Exception)
            {
                throw;
            }
            return result;
        }
        //DeshabilitarLubProy
        public int DeshabilitarLubProy(int idobjLub)
        {
            int modelo = 0;

            try
            {
                tblM_BitacoraControlAceiteMantProy objMant = new tblM_BitacoraControlAceiteMantProy();
                objMant = _context.tblM_BitacoraControlAceiteMantProy.Where(x => x.id == idobjLub && x.estatus).FirstOrDefault();
                objMant.estatus = false;
                int idMant = _context.tblM_BitacoraControlAceiteMantProy.Where(x => x.id == idobjLub && x.estatus).Select(x => x.idMant).FirstOrDefault();
                string economico = _context.tblM_MatenimientoPm.Where(x => x.id == idMant && x.estatus == true).Select(x => x.economicoID).FirstOrDefault();
                modelo = ConsultarModelo(economico);
                _context.SaveChanges();
            }
            catch (Exception)
            {

            }
            return modelo;
        }
        //DeshabilitarLubProy
        public int DeshabilitarACProy(int id)
        {
            int modelo = 0;

            try
            {
                tblM_BitacoraControlAEMantProy objMant = new tblM_BitacoraControlAEMantProy();
                objMant = _context.tblM_BitacoraControlAEMantProy.Where(x => x.id == id && x.estatus == true).FirstOrDefault();
                objMant.estatus = false;
                int idMant = _context.tblM_BitacoraControlAEMantProy.Where(x => x.id == id && x.estatus == true).Select(x => x.idMant).FirstOrDefault();
                string economico = _context.tblM_MatenimientoPm.Where(x => x.id == idMant && x.estatus == true).Select(x => x.economicoID).FirstOrDefault();
                modelo = ConsultarModelo(economico);
                _context.SaveChanges();
            }
            catch (Exception)
            {

            }
            return modelo;
        }
        public int DeshabilitarDNProy(int id)
        {
            int modelo = 0;

            try
            {
                tblM_BitacoraControlDNMantProy objMant = new tblM_BitacoraControlDNMantProy();
                objMant = _context.tblM_BitacoraControlDNMantProy.Where(x => x.id == id && x.estatus == true).FirstOrDefault();
                objMant.estatus = false;
                int idMant = _context.tblM_BitacoraControlDNMantProy.Where(x => x.id == id && x.estatus == true).Select(x => x.idMant).FirstOrDefault();
                string economico = _context.tblM_MatenimientoPm.Where(x => x.id == idMant && x.estatus == true).Select(x => x.economicoID).FirstOrDefault();
                modelo = ConsultarModelo(economico);
                _context.SaveChanges();
            }
            catch (Exception)
            {

            }
            return modelo;
        }
        public int ConsultaModelobyMantenimiento(int idMant)
        {
            int modelo = 0;

            try
            {
                string economico = _context.tblM_MatenimientoPm.Where(x => x.id == idMant).Select(x => x.economicoID).FirstOrDefault();
                modelo = ConsultarModelo(economico);
            }
            catch (Exception)
            {

            }
            return modelo;
        }
        //raguilar devuelve las actividades del pm 07/08/17
        public List<actividadesPMDTO> getFormato(int id, int idpm)
        {
            var result = new object();
            List<tblM_CatPM_CatActividadPM> lstobjActividadModelo = new List<tblM_CatPM_CatActividadPM>();
            List<tblM_CatActividadPM> lstCatActividades = new List<tblM_CatActividadPM>();
            lstCatActividades = _context.tblM_CatActividadPM.ToList();
            int modelo = ConsultaModelobyMantenimiento(id);
            lstobjActividadModelo = _context.tblM_CatPM_CatActividadPM.Where(x => (x.modeloEquipoID == modelo) && x.estado == true && x.idPM == idpm).ToList();
            var lstResultado = new List<actividadesPMDTO>();
            try
            {
                foreach (var obj in lstobjActividadModelo)
                {

                    var temp = new actividadesPMDTO
                    {
                        id = obj.id,
                        orden = obj.orden,
                        descripcion = (lstCatActividades.FirstOrDefault(act => act.id == obj.idAct && act.estado == true).descripcionActividad), ///Select(act => act.descripcionActividad)).FirstOrDefault(),
                        idAct = (lstCatActividades.FirstOrDefault(act => act.id == obj.idAct && act.estado == true).id),//(act => act.id)).FirstOrDefault(),
                        Tipo = _context.tblM_CatTipoActividad.FirstOrDefault(y => y.id == obj.idCatTipoActividad).descripcion,// Select(y => y.descripcion).FirstOrDefault(),
                        idTipo = obj.idCatTipoActividad,
                        PM = obj.idPM,
                        idformato = RutaFormato(obj.idAct, obj.modeloEquipoID, obj.estado, obj.idPM, obj.idCatTipoActividad, obj.idDN),
                        leyenda = obj.leyenda,
                        Componente = VinCompo(obj.modeloEquipoID, obj.idAct, obj.idCatTipoActividad, obj.idPM),//obtiene el id del componente vinculado un listado
                        perioricidad = obj.perioricidad,//perioricidad con la cual se lleva la actividad 21/05/18S
                        DN = obj.idDN//control db 16/06/18
                    };
                    if (temp.idformato.nombreRuta != null && temp.idformato.nombreRuta != "")
                    {
                        lstResultado.Add(temp);
                    }
                }

            }
            catch (Exception e)
            {
            }
            return lstResultado;
        }
        public tblM_ParamReport ConsultarMantenimientobyID(int idobjMatenimientoPm)// consulta preventivo menor
        {
            tblM_ParamReport result = new tblM_ParamReport();
            tblM_MatenimientoPm objMant = new tblM_MatenimientoPm();
            try
            {
                if (idobjMatenimientoPm != 0)
                {
                    objMant = _context.tblM_MatenimientoPm.Where(x => x.id == idobjMatenimientoPm && x.estatus == true).FirstOrDefault();
                    result.economico = objMant.economicoID;
                    result.tipopm = _context.tblM_catPM.FirstOrDefault(x => x.id == objMant.tipoMantenimientoProy).tipoMantenimiento.Substring(0, 3);
                    int modelo = ConsultaModelobyMantenimiento(objMant.id);
                    result.modelo = _context.tblM_CatModeloEquipo.Where(x => x.id == modelo).Select(x => x.descripcion).FirstOrDefault();
                    result.solicitador = _context.tblP_Usuario.Where(x => x.cveEmpleado == objMant.planeador.ToString()).Select(x => (x.nombre + " " + x.apellidoPaterno + " " + x.apellidoMaterno)).FirstOrDefault();
                }
            }
            catch (Exception e)
            {

            }
            return result;
        }
        public object ConsultarActPmbyModelo(int Modelo, int idact)// consulta preventivo menor
        {
            object result = new object();

            tblM_MatenimientoPm objMant = new tblM_MatenimientoPm();
            try
            {
                result = (_context.tblM_ComponenteMantenimiento.Where(x => x.modeloEquipoID == Modelo &&
                    x.estado == true && x.idPM != 0 && x.idCatTipoActividad == 1 && x.idAct == idact)
             .Select(x => x.idCompVis)).Distinct();
            }
            catch (Exception e)
            {

            }
            return result;
        }
        public object FillGridComponenteRestrinccion(int modeloEquipoID, int idActs, int idTipoAct, int idpm)
        {
            //idpm = 1;
            var result = new object();
            List<tblM_CatComponentesViscosidades> _lstobjComponent = new List<tblM_CatComponentesViscosidades>();
            if (idActs == 20)
            {
                _lstobjComponent = _context.tblM_CatComponentesViscosidades.ToList();
            }
            else if (true)
            {
                _lstobjComponent = _context.tblM_CatComponentesViscosidades.Where(x => x.id != 16).ToList();
            }
            List<object> lstobjCompVin = new List<object>();
            List<object> lstobjCompVinArmagedon = new List<object>();
            try
            {
                foreach (var obj in _lstobjComponent)//busca los compoenntes asignados a la actividad
                {
                    var temp = new
                    {
                        id = obj.id,
                        descripcion = obj.Descripcion,
                        Tipo = obj.Tipo,
                        asignado = _context.tblM_ComponenteMantenimiento.Where(x => x.idAct == idActs && x.modeloEquipoID == modeloEquipoID && x.idCompVis == obj.id && x.estado == true && x.idCatTipoActividad == idTipoAct && x.idPM == idpm).FirstOrDefault(), //realizar una consulta para buscar en una tabla  modelo y id componente
                        asignadoMis = AsignadoMis(modeloEquipoID, idActs, obj.id, 0, false, 0), //el cero representa al tipo
                        pmActual = true//indicacion que es un componente vinculado a este pm
                    };
                    if (temp.asignado != null)//si esta asignada agrega al listado
                    {
                        lstobjCompVinArmagedon.Add(temp);

                    }
                    var temp1 = new//busca en otros pm's si se encuentra vinculado para darle la opcion de agegarlo
                    {
                        id = obj.id,
                        descripcion = obj.Descripcion,
                        Tipo = obj.Tipo,
                        asignado = _context.tblM_ComponenteMantenimiento.Where(x => x.idAct == idActs && x.modeloEquipoID == modeloEquipoID && x.idCompVis == obj.id && x.estado == true && x.idCatTipoActividad == idTipoAct && x.idPM != idpm).FirstOrDefault(), //realizar una consulta para buscar en una tabla  modelo y id componente
                        asignadoMis = AsignadoMis(modeloEquipoID, idActs, obj.id, 0, false, 0), //el cero representa al tipo
                        pmActual = false//indicando que es un componente ajeno al pm
                    };
                    if ((temp1.asignado != null) && (temp.asignado == null))//si el componente no es el mismo lo agrega en caso de ser asignado
                    {
                        //realizar una consulta para buscar en una tabla  modelo y id componente
                        lstobjCompVinArmagedon.Add(temp1);//lista de compoentes del pmactual y de los demas asginados al modelo
                    }
                }
            }
            catch (Exception e)
            {
            }
            return lstobjCompVinArmagedon;
        }

        /*/*/

        public object ConsultarPMActivoByObra(string obra)
        {
            var lstResultado = new List<object>();
            try
            {
                //var maquinariasDisponibles = (from m in _context.tblM_CatMaquina
                //                              where m.grupoMaquinaria.tipoEquipoID == 1 && m.centro_costos.Equals(obra) && m.estatus != 0
                //                              select m.noEconomico).ToList();
                //--> Solo consultar Equipos Activos
                var maquinariasDisponibles = (from m in _context.tblM_CatMaquina
                                              where /*m.grupoMaquinaria.tipoEquipoID == 1 &&*/ m.centro_costos.Equals(obra) && m.estatus == 1
                                              select m.noEconomico).ToList();

                var lstPmActivo = _context.tblM_MatenimientoPm.Where(x => maquinariasDisponibles.Contains(x.economicoID)).GroupBy(x => x.economicoID).ToList();
                var lstCalActivo = _context.tblM_RenderFullCalendar.ToList();

                tblM_RenderFullCalendar objRenderFullCalendar = new tblM_RenderFullCalendar();

                List<string> economicosEquipos = lstPmActivo.SelectMany(x => x.Select(y => y.economicoID)).ToList();
                var fechaInicio = lstPmActivo.SelectMany(x => x.Select(y => y.fechaProy)).Min();

                var horometrosEquipos = _context.tblM_CapHorometro.Where(x => economicosEquipos.Contains(x.Economico) && x.Fecha >= fechaInicio).ToList();
                var ritmoMaquinas = CalculoHrsPromDiarioMaquinas(economicosEquipos);

                var auxTipoPM = lstPmActivo.SelectMany(x => x.Select(y => y.tipoMantenimientoProy)).ToList();
                var descripcionesTipoPM = _context.tblM_catPM.Where(x => auxTipoPM.Contains(x.id)).ToList();

                foreach (var item in lstPmActivo)
                {
                    var horometro = horometrosEquipos.Where(x => x.Economico == item.Key).OrderByDescending(x => x.id).FirstOrDefault();
                    var ritmo = ritmoMaquinas.FirstOrDefault(x => x.economico == item.Key);

                    decimal hrsPromDiaria = 0;
                    if (ritmo == null)
                    {
                        var auxHorometrosRitmo = horometrosEquipos.Where(x => x.Economico == item.Key).OrderByDescending(x => x.id).Take(20);
                        hrsPromDiaria = auxHorometrosRitmo.Average(x => x.HorasTrabajo);
                    }
                    else hrsPromDiaria = ritmo.horasDiarias;

                    foreach (var pmActivo in item)
                    {
                        //var horometroFechaParo = horometrosEquipos.Where(x => x.Economico == item.Key && x.Fecha <= pmActivo.fechaProy).OrderByDescending(x => x.id).FirstOrDefault();
                        var horometroFechaParo = horometrosEquipos.Where(x => x.Economico == item.Key && x.Fecha >= pmActivo.fechaProy).OrderBy(x => x.id).FirstOrDefault();

                        if (horometroFechaParo == null) horometroFechaParo = _context.tblM_CapHorometro.Where(x => x.Economico == item.Key && x.Fecha <= pmActivo.fechaProy).OrderByDescending(x => x.id).FirstOrDefault();

                        TimeSpan ts = pmActivo.fechaProy - pmActivo.fechaPM;
                        int differenceInDays = ts.Days;
                        TimeSpan t1 = pmActivo.fechaProy - pmActivo.fechaPM;
                        int diasFechaProyectada = t1.Days;

                        var FactorFechaProyectada = diasFechaProyectada * hrsPromDiaria;
                        double FactorHoy = (Convert.ToDouble(differenceInDays) * Convert.ToDouble((hrsPromDiaria)));//horas trabajadas al dia de hoy   

                        var horometroActual = Convert.ToDouble(pmActivo.horometroPM) + FactorHoy;
                        var obj = lstCalActivo.FirstOrDefault(x => x.idMantenimiento == pmActivo.id);

                        var DescripcionPM = descripcionesTipoPM.FirstOrDefault(x => x.id == pmActivo.tipoMantenimientoProy);
                        objRenderFullCalendar = new tblM_RenderFullCalendar();
                        objRenderFullCalendar.personalRealizo = pmActivo.personalRealizo;
                        objRenderFullCalendar.fechaMantenimientoActual = pmActivo.fechaCaptura.ToString("yyyy-MM-dd H:mm:ss");
                        objRenderFullCalendar.tipoMantenimientoActual = pmActivo.tipoPM;
                        objRenderFullCalendar.observaciones = pmActivo.observaciones;
                        objRenderFullCalendar.economicoID = pmActivo.economicoID;
                        objRenderFullCalendar.title = pmActivo.economicoID;
                        objRenderFullCalendar.start = pmActivo.estadoMantenimiento != 3 ? pmActivo.fechaProy.ToString("yyyy-MM-dd H:mm:ss") : pmActivo.fechaPM.ToString("yyyy-MM-dd H:mm:ss");
                        objRenderFullCalendar.idMantenimiento = pmActivo.id;
                        objRenderFullCalendar.UltimoHorometro = pmActivo.horometroUltCapturado;
                        objRenderFullCalendar.horometroProyectado = pmActivo.horometroProy;
                        objRenderFullCalendar.fechaProyectada = pmActivo.fechaProy.ToString("yyyy-MM-dd H:mm:ss");
                        objRenderFullCalendar.idMaquina = pmActivo.idMaquina;
                        objRenderFullCalendar.HorometroPm = pmActivo.estadoMantenimiento != 3 ? pmActivo.horometroProy : pmActivo.horometroPMEjecutado;
                        objRenderFullCalendar.borderColor = "blue";
                        objRenderFullCalendar.color = getColorStatus(pmActivo.economicoID, objRenderFullCalendar.horometroProyectado, pmActivo.estadoMantenimiento, horometro);
                        objRenderFullCalendar.description = DescripcionPM.tipoMantenimiento + "(" + objRenderFullCalendar.HorometroPm + ")" + " <br> Horometro Dia: " + GetHorometroActual(pmActivo.fechaProy, horometroFechaParo, Convert.ToDouble((hrsPromDiaria)));// (Convert.ToDecimal(pmActivo.horometroPM) + FactorHoy);// obj.description;
                        objRenderFullCalendar.id = pmActivo.id;
                        objRenderFullCalendar.estadoMantenimiento = pmActivo.estadoMantenimiento;
                        lstResultado.Add(objRenderFullCalendar);
                    }
                }
            }
            catch (Exception e)
            {
            }
            return lstResultado;
        }

        public double GetHorometroDia(DateTime dia, string economico, double FactorHoy)
        {
            dia = dia.AddHours(23).AddMinutes(59).AddSeconds(59);
            //var rawEconomicos = _context.tblM_CapHorometro.ToList().Where(x => x.Economico == economico && x.Fecha.Date <= dia).OrderByDescending(x => x.id).Take(10).ToList();
            var rawEconomicos2 = _context.tblM_CapHorometro.Where(x => x.Economico == economico && x.Fecha <= dia).OrderByDescending(x => x.id).FirstOrDefault();
            var topRawEconomico = rawEconomicos2;

            if (dia.Date != topRawEconomico.Fecha.Date)
            {
                TimeSpan ts = dia.Date - topRawEconomico.Fecha.Date;
                // Difference in days.
                int differenceInDays = ts.Days;
                double Factor = (Convert.ToDouble(differenceInDays) * Convert.ToDouble((FactorHoy)));



                return (double)topRawEconomico.Horometro + Factor;
            }
            else
            {
                return (double)topRawEconomico.Horometro;

            }
        }

        public double GetHorometroActual(DateTime fecha, tblM_CapHorometro horometro, double FactorDia)
        {
            if (fecha.Date != horometro.Fecha.Date)
            {
                TimeSpan ts = fecha.Date - horometro.Fecha.Date;
                int diferenciaDias = ts.Days;
                double diferenciaHoras = (Convert.ToDouble(diferenciaDias) * Convert.ToDouble((FactorDia)));
                return (double)horometro.Horometro - (double)horometro.HorasTrabajo + diferenciaHoras;
            }
            else
            {
                return (double)horometro.Horometro;
            }
        }

        public PuestosDTO ConsultaPersonalIdManteniminto(int numEmpleado)
        {
            PuestosDTO PuestosObj = new PuestosDTO();

            //string ConsutaEmpleado = "SELECT p.puesto,p.descripcion,e.clave_empleado AS personalID,";
            //ConsutaEmpleado += "e.nombre,ape_paterno,e.ape_materno ";
            //ConsutaEmpleado += "FROM DBA.sn_empleados AS e ";
            //ConsutaEmpleado += "INNER JOIN DBA.si_puestos as p on e.puesto=p.puesto ";
            ////ConsutaEmpleado += "INNER JOIN DBA.sn_tabulador_historial AS s ON e.clave_empleado=s.clave_empleado ";
            //// ConsutaEmpleado += "INNER JOIN (SELECT  MAX(id) AS id ,clave_empleado FROM DBA.sn_tabulador_historial GROUP BY (clave_empleado)) AS F ON F.id = s.id ";
            //ConsutaEmpleado += "WHERE e.clave_empleado =" + numEmpleado;

            try
            {
                //var listaEmpleados = (List<PuestosDTO>)ContextEnKontrolNominaArrendadora.Where(ConsutaEmpleado, 1).ToObject<List<PuestosDTO>>();

                var listaEmpleados = _context.Select<PuestosDTO>(new DapperDTO
                {
                    baseDatos = MainContextEnum.Construplan,
                    consulta = @"SELECT p.puesto,p.descripcion,e.clave_empleado AS personalID,
                                    e.nombre,ape_paterno,e.ape_materno 
                                FROM tblRH_EK_Empleados AS e 
                                INNER JOIN tblRH_EK_Puestos as p on e.puesto=p.puesto 

                                WHERE e.clave_empleado =" + numEmpleado,
                });

                var listaEmpleadosEmpresaActual = _context.Select<PuestosDTO>(new DapperDTO
                {
                    baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
                    consulta = @"SELECT p.puesto,p.descripcion,e.clave_empleado AS personalID,
                                    e.nombre,ape_paterno,e.ape_materno 
                                FROM tblRH_EK_Empleados AS e 
                                INNER JOIN tblRH_EK_Puestos as p on e.puesto=p.puesto 

                                WHERE e.clave_empleado =" + numEmpleado,
                });

                listaEmpleados.AddRange(listaEmpleadosEmpresaActual);

                return listaEmpleados.FirstOrDefault();
            }
            catch (Exception)
            {

            }
            try
            {
                //var listaEmpleados = (List<PuestosDTO>)ContextEnKontrolNominaArrendadora.Where(ConsutaEmpleado, 2).ToObject<List<PuestosDTO>>();

                var listaEmpleados = _context.Select<PuestosDTO>(new DapperDTO
                {
                    baseDatos = MainContextEnum.Arrendadora,
                    consulta = @"SELECT p.puesto,p.descripcion,e.clave_empleado AS personalID,
                                    e.nombre,ape_paterno,e.ape_materno 
                                FROM tblRH_EK_Empleados AS e 
                                INNER JOIN tblRH_EK_Puestos as p on e.puesto=p.puesto 

                                WHERE e.clave_empleado =" + numEmpleado,
                });

                return listaEmpleados.FirstOrDefault();

            }
            catch (Exception)
            {



            }

            return PuestosObj;
        }

        private string getColorStatus(string p, decimal hProyectado, int estadoPm, tblM_CapHorometro horometroEquipo)
        {
            //var horometroEquipo = _context.tblM_CapHorometro.Where(x => x.Economico == p).OrderByDescending(y => y.id).FirstOrDefault();

            if (horometroEquipo != null)
            {
                var hrasPM = hProyectado - horometroEquipo.Horometro;


                switch (estadoPm)
                {
                    case 0:
                        {
                            if (horometroEquipo.Horometro >= hProyectado)
                            {
                                return "#d9534f";
                            }
                            else if (hrasPM <= 25 && horometroEquipo.Horometro < hProyectado)
                            {
                                return "#f0ad4e";
                            }
                            else
                            {
                                return "#5cb85c";
                            }
                        }
                    case 1:
                        {
                            if (horometroEquipo.Horometro >= hProyectado)
                            {
                                return "#d9534f";
                            }
                            else if (hrasPM <= 25 && horometroEquipo.Horometro < hProyectado)
                            {
                                return "#f0ad4e";
                            }
                            else
                            {
                                return "#5cb85c";
                            }
                        }
                    case 2:
                        return "#337ab7";
                    case 3:
                        return "#6b6b6b";
                    default:

                        return "";
                }

            }
            else
            {
                return "";
            }
        }

        //Oscar
        public List<tblM_MatenimientoPm> GetMantenimientosProg(string cc = "")
        {
            List<tblM_MatenimientoPm> data = new List<tblM_MatenimientoPm>();
            if (cc == "")
            {
                return data;
                //return _context.tblM_MatenimientoPm.Where(x => x.estatus && x.estadoMantenimiento == (int)MantenimientoEstadoEnum.PROGRAMADO).ToList();
            }
            else
            {
                var maquinas = _context.tblM_CatMaquina.Where(x => x.centro_costos == cc).Select(x => x.id);
                data = _context.tblM_MatenimientoPm.Where(x => x.estatus && x.estadoMantenimiento == (int)MantenimientoEstadoEnum.PROGRAMADO && maquinas.Contains(x.idMaquina)).ToList();
                return data;
            }

        }
        public tblM_MatenimientoPm GuardarEjecutado(tblM_MatenimientoPm objGeneral, int idMantenimiento, List<tblM_BitacoraControlAceiteMantProy> tblGridLubProxTbl, List<tblM_BitacoraControlAEMantProy> tblGridActProxTbl, List<tblM_BitacoraControlDNMantProy> tblgridDNProxTbl, List<tblM_MantenimientoPm_Archivo> referencias)
        {
            //Se carga el mantenimiento actual
            var mantAnterior = ConsultarPMbyID(idMantenimiento);
            using (var dbContextTransaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var responsable = ConsultaPersonalIdManteniminto(mantAnterior.personalRealizo);
                    objGeneral.UsuarioCap = vSesiones.sesionUsuarioDTO.id;
                    objGeneral.personalRealizo = objGeneral.personalRealizo == 0 ? responsable.personalID : objGeneral.personalRealizo;
                    objGeneral.idMaquina = mantAnterior.idMaquina;
                    objGeneral.planeador = mantAnterior.planeador;

                    //Guardado de Tabla Padre
                    mantAnterior.fechaPM = objGeneral.fechaPM;
                    mantAnterior.estatus = false;
                    mantAnterior.estadoMantenimiento = 3;
                    mantAnterior.horometroPMEjecutado = objGeneral.horometroPMEjecutado;
                    _context.Entry(mantAnterior).State = System.Data.Entity.EntityState.Modified;
                    _context.SaveChanges();
                    var ultimoHorometro = _context.tblM_CapHorometro.Where(x => x.Economico == mantAnterior.economicoID).OrderByDescending(x => x.id).FirstOrDefault().Horometro;
                    int manttoActual = GetNextPM(mantAnterior.tipoPM);
                    tblM_MatenimientoPm objMant = new tblM_MatenimientoPm();
                    objMant.economicoID = objGeneral.economicoID;
                    objMant.horometroUltCapturado = ultimoHorometro; //objGeneral.horometroUltCapturado;
                    objMant.fechaUltCapturado = DateTime.Now;
                    objMant.tipoPM = manttoActual;
                    objMant.fechaPM = FechaProximoPM(objGeneral.economicoID, objGeneral.fechaPM).AddHours(5);
                    objMant.horometroPM = objGeneral.horometroPM; // objGeneral.horometroPM;
                    objMant.personalRealizo = objGeneral.personalRealizo;
                    objMant.observaciones = "";
                    objMant.horometroProy = objGeneral.horometroPM + 250;
                    objMant.fechaProy = FechaProximoPM(objGeneral.economicoID, objGeneral.fechaPM).AddHours(5); //FechaProximo(objGeneral.economicoID);
                    objMant.tipoMantenimientoProy = GetNextPM(manttoActual);
                    objMant.actual = true;
                    objMant.fechaProyFin = objMant.fechaProy.AddHours(1); //FechaProximo(objGeneral.economicoID);
                    objMant.fechaCaptura = DateTime.Now;
                    objMant.idMaquina = objGeneral.idMaquina;
                    objMant.estatus = true;
                    objMant.planeador = objGeneral.planeador;
                    objMant.UsuarioCap = objGeneral.UsuarioCap;
                    objMant.estadoMantenimiento = 1;
                    objMant.horometroPMEjecutado = objGeneral.horometroPM;
                    _context.tblM_MatenimientoPm.Add(objMant);
                    _context.SaveChanges();

                    //Cargado de datos
                    var maquinaria = _context.tblM_CatMaquina.FirstOrDefault(x => x.id == objMant.idMaquina);
                    var maquinaID = maquinaria.id;
                    var modeloEquipo = maquinaria.modeloEquipoID;

                    var tblProyectadoMant = _context.tblM_BitacoraControlAceiteMantProy.Where(x => x.idMant == idMantenimiento && x.estatus).ToList();
                    var lubricantesPorModelo = _context.tblM_PMComponenteLubricante.Where(x => x.modeloID == modeloEquipo && x.estatus).ToList();
                    var componentes = lubricantesPorModelo.Select(x => x.componenteID).Distinct().ToList();
                    var lstMantenimientos = _context.tblM_MatenimientoPm.Where(x => x.idMaquina == maquinaID).Select(x => x.id).ToList();
                    //Guardado de lubricaciones en base a los componentes que se lubrican para tal modelo
                    foreach (var componente in componentes)
                    {
                        tblM_BitacoraControlAceiteMant aplicaLubricante = new tblM_BitacoraControlAceiteMant();
                        tblM_BitacoraControlAceiteMantProy aplicaLubricanteProy = new tblM_BitacoraControlAceiteMantProy();
                        var auxAplicaLubricante = tblGridLubProxTbl.FirstOrDefault(x => x.idComp == componente);
                        var auxLubricantePorModelo = lubricantesPorModelo.FirstOrDefault(x => x.componenteID == componente);
                        var ultimoRegistroLubricante = _context.tblM_BitacoraControlAceiteMant.Where(x => lstMantenimientos.Contains(x.idMant) && x.idComp == componente && x.estatus).OrderByDescending(x => x.id).FirstOrDefault();
                        if (auxAplicaLubricante == null || !auxAplicaLubricante.aplicado)
                        {
                            aplicaLubricanteProy.id = 0;
                            aplicaLubricanteProy.aplicado = false;
                            aplicaLubricanteProy.estatus = true;
                            aplicaLubricanteProy.fechaCaptura = DateTime.Now;
                            aplicaLubricanteProy.FechaServicio = mantAnterior.fechaPM;
                            //aplicaLubricanteProy.Hrsaplico = objGeneral.horometroPM;
                            aplicaLubricanteProy.Hrsaplico = ultimoRegistroLubricante == null ? objGeneral.horometroPM : ultimoRegistroLubricante.Hrsaplico;
                            aplicaLubricanteProy.idAct = auxAplicaLubricante == null ? auxLubricantePorModelo.id : auxAplicaLubricante.idAct;
                            aplicaLubricanteProy.idComp = auxAplicaLubricante == null ? auxLubricantePorModelo.componenteID : auxAplicaLubricante.idComp;
                            aplicaLubricanteProy.idMant = objMant.id;
                            aplicaLubricanteProy.idMisc = auxAplicaLubricante == null ? auxLubricantePorModelo.lubricanteID : auxAplicaLubricante.idMisc;
                            aplicaLubricanteProy.Observaciones = auxAplicaLubricante == null ? "" : auxAplicaLubricante.Observaciones;
                            aplicaLubricanteProy.programado = false;
                            aplicaLubricanteProy.prueba = auxAplicaLubricante == null ? false : auxAplicaLubricante.prueba;
                            aplicaLubricanteProy.UsuarioCap = vSesiones.sesionUsuarioDTO.id;
                            aplicaLubricanteProy.Vigencia = auxAplicaLubricante == null ? auxLubricantePorModelo.vidaLubricante : auxAplicaLubricante.Vigencia;
                            _context.tblM_BitacoraControlAceiteMantProy.Add(aplicaLubricanteProy);

                            aplicaLubricante.alta = true;
                            aplicaLubricante.Aplicado = true;
                            aplicaLubricante.fechaCaptura = DateTime.Now;
                            aplicaLubricante.Hrsaplico = ultimoRegistroLubricante == null ? objGeneral.horometroPM : ultimoRegistroLubricante.Hrsaplico;
                            aplicaLubricante.id = 0;
                            aplicaLubricante.idAct = auxAplicaLubricante == null ? auxLubricantePorModelo.id : auxAplicaLubricante.idAct;
                            aplicaLubricante.idComp = auxAplicaLubricante == null ? auxLubricantePorModelo.componenteID : auxAplicaLubricante.idComp;
                            aplicaLubricante.idMant = objMant.id;
                            aplicaLubricante.idMisc = auxAplicaLubricante == null ? auxLubricantePorModelo.lubricanteID : auxAplicaLubricante.idMisc;
                            aplicaLubricante.prueba = auxAplicaLubricante == null ? false : auxAplicaLubricante.prueba;
                            aplicaLubricante.UsuarioCap = vSesiones.sesionUsuarioDTO.id;
                            aplicaLubricante.vidaActual = 0;
                            aplicaLubricante.Vigencia = auxAplicaLubricante == null ? auxLubricantePorModelo.vidaLubricante : auxAplicaLubricante.Vigencia;
                            aplicaLubricante.VidaRestante = aplicaLubricante.Vigencia - (objGeneral.horometroPM - aplicaLubricante.Hrsaplico);
                            aplicaLubricante.estatus = true;
                            _context.tblM_BitacoraControlAceiteMant.Add(aplicaLubricante);
                        }

                        else
                        {
                            aplicaLubricanteProy.id = 0;
                            aplicaLubricanteProy.aplicado = auxAplicaLubricante.aplicado;
                            aplicaLubricanteProy.estatus = auxAplicaLubricante.estatus;
                            aplicaLubricanteProy.fechaCaptura = objGeneral.fechaCaptura;
                            aplicaLubricanteProy.FechaServicio = auxAplicaLubricante.FechaServicio;
                            aplicaLubricanteProy.Hrsaplico = objGeneral.horometroPM;
                            aplicaLubricanteProy.idAct = auxAplicaLubricante.idAct;
                            aplicaLubricanteProy.idComp = auxAplicaLubricante.idComp;
                            aplicaLubricanteProy.idMant = objMant.id;
                            aplicaLubricanteProy.idMisc = auxAplicaLubricante.idMisc;
                            aplicaLubricanteProy.Observaciones = "";
                            aplicaLubricanteProy.programado = false;
                            aplicaLubricanteProy.prueba = auxAplicaLubricante.prueba;
                            aplicaLubricanteProy.UsuarioCap = vSesiones.sesionUsuarioDTO.id;
                            aplicaLubricanteProy.Vigencia = auxAplicaLubricante.Vigencia;
                            _context.tblM_BitacoraControlAceiteMantProy.Add(aplicaLubricanteProy);

                            if (ultimoRegistroLubricante == null)
                            {
                                aplicaLubricante.alta = false;
                                aplicaLubricante.Aplicado = false;
                                aplicaLubricante.fechaCaptura = DateTime.Now;
                                aplicaLubricante.Hrsaplico = objGeneral.horometroPM;
                                aplicaLubricante.id = 0;
                                aplicaLubricante.idAct = auxLubricantePorModelo.id;
                                aplicaLubricante.idComp = auxLubricantePorModelo.componenteID;
                                aplicaLubricante.idMant = objMant.id;
                                aplicaLubricante.idMisc = auxLubricantePorModelo.lubricanteID;
                                aplicaLubricante.prueba = false;
                                aplicaLubricante.UsuarioCap = vSesiones.sesionUsuarioDTO.id;
                                aplicaLubricante.vidaActual = 0;
                                aplicaLubricante.VidaRestante = auxLubricantePorModelo.vidaLubricante;
                                aplicaLubricante.Vigencia = auxLubricantePorModelo.vidaLubricante;
                                aplicaLubricante.estatus = true;
                                _context.tblM_BitacoraControlAceiteMant.Add(aplicaLubricante);
                            }
                            else
                            {
                                aplicaLubricante.alta = ultimoRegistroLubricante.alta;
                                aplicaLubricante.Aplicado = ultimoRegistroLubricante.Aplicado;
                                aplicaLubricante.fechaCaptura = objGeneral.fechaCaptura;
                                aplicaLubricante.Hrsaplico = objGeneral.horometroPM;
                                aplicaLubricante.id = 0;
                                aplicaLubricante.idAct = ultimoRegistroLubricante.idAct;
                                aplicaLubricante.idComp = ultimoRegistroLubricante.idComp;
                                aplicaLubricante.idMant = objMant.id;
                                aplicaLubricante.idMisc = ultimoRegistroLubricante.idMisc;
                                aplicaLubricante.prueba = ultimoRegistroLubricante.prueba;
                                aplicaLubricante.UsuarioCap = vSesiones.sesionUsuarioDTO.id;
                                aplicaLubricante.vidaActual = ultimoRegistroLubricante.vidaActual + 250;
                                aplicaLubricante.VidaRestante = ultimoRegistroLubricante.Vigencia;
                                aplicaLubricante.Vigencia = ultimoRegistroLubricante.Vigencia;
                                aplicaLubricante.estatus = true;
                                _context.tblM_BitacoraControlAceiteMant.Add(aplicaLubricante);
                            }
                        }
                    }

                    var ListaBitacoraControlActExt = _context.tblM_BitacoraControlActExt.Where(x => lstMantenimientos.Contains(x.idMant)).ToList();
                    var ListaBitacoraControlActExtProy = _context.tblM_BitacoraControlAEMantProy.Where(x => lstMantenimientos.Contains(x.idMant)).ToList();
                    var actividadesExtraPorModelo = _context.tblM_CatPM_CatActividadPM.Where(x => x.modeloEquipoID == modeloEquipo && x.idCatTipoActividad == 3 && x.estado).ToList();
                    var actividadesExtraID = actividadesExtraPorModelo.OrderBy(x => x.orden).Select(x => x.idAct).Distinct().ToList();

                    foreach (var actividadExtra in actividadesExtraID)
                    {
                        tblM_BitacoraControlActExt controlActividadExtra = new tblM_BitacoraControlActExt();
                        tblM_BitacoraControlAEMantProy controlActividadExtraProy = new tblM_BitacoraControlAEMantProy();
                        var auxActividadExtra = tblGridActProxTbl.FirstOrDefault(x => x.idAct == actividadExtra);
                        var ultimoRegistroActividadExtra = ListaBitacoraControlActExt.Where(x => x.idAct == actividadExtra).OrderByDescending(x => x.id).FirstOrDefault();
                        var ultimoRegistroActividadExtraProy = ListaBitacoraControlActExtProy.Where(x => x.idAct == actividadExtra).OrderByDescending(x => x.id).FirstOrDefault();

                        if (auxActividadExtra == null || !auxActividadExtra.aplicado)
                        {
                            controlActividadExtraProy.aplicado = false;
                            controlActividadExtraProy.estatus = true;
                            controlActividadExtraProy.fechaCaptura = DateTime.Now;
                            controlActividadExtraProy.FechaServicio = ultimoRegistroActividadExtraProy == null ? objGeneral.fechaCaptura : ultimoRegistroActividadExtraProy.FechaServicio;
                            controlActividadExtraProy.Hrsaplico = ultimoRegistroActividadExtra == null ? objGeneral.horometroPM : ultimoRegistroActividadExtra.Hrsaplico;
                            controlActividadExtraProy.id = 0;
                            controlActividadExtraProy.idAct = actividadExtra;
                            controlActividadExtraProy.idMant = objMant.id;
                            controlActividadExtraProy.Observaciones = "";
                            controlActividadExtraProy.programado = false;
                            controlActividadExtraProy.UsuarioCap = vSesiones.sesionUsuarioDTO.id;
                            controlActividadExtraProy.Vigencia = auxActividadExtra == null ? 0 : auxActividadExtra.Vigencia;
                            _context.tblM_BitacoraControlAEMantProy.Add(controlActividadExtraProy);

                            controlActividadExtra.alta = true;
                            controlActividadExtra.Aplicado = true;
                            controlActividadExtra.fechaCaptura = DateTime.Now;
                            controlActividadExtra.Hrsaplico = ultimoRegistroActividadExtra == null ? objGeneral.horometroPM : ultimoRegistroActividadExtra.Hrsaplico;
                            controlActividadExtra.id = 0;
                            controlActividadExtra.idAct = actividadExtra;
                            controlActividadExtra.idMant = objMant.id;
                            if (ultimoRegistroActividadExtra == null)
                            {
                                var periodicidad = _context.tblM_CatPM_CatActividadPM.FirstOrDefault(x => x.modeloEquipoID == modeloEquipo && x.idAct == actividadExtra);
                                if (periodicidad == null) controlActividadExtra.idPerioricidad = 11;
                                else controlActividadExtra.idPerioricidad = periodicidad.id;
                            }
                            else controlActividadExtra.idPerioricidad = ultimoRegistroActividadExtra.idPerioricidad;
                            controlActividadExtra.UsuarioCap = vSesiones.sesionUsuarioDTO.id;
                            controlActividadExtra.vidaRestante = controlActividadExtraProy.Vigencia - (objGeneral.horometroPM - controlActividadExtra.Hrsaplico);
                            controlActividadExtra.vidaActual = objGeneral.horometroPM - controlActividadExtra.Hrsaplico;
                            _context.tblM_BitacoraControlActExt.Add(controlActividadExtra);
                        }
                        else
                        {
                            controlActividadExtraProy.aplicado = false;
                            controlActividadExtraProy.estatus = true;
                            controlActividadExtraProy.fechaCaptura = DateTime.Now;
                            controlActividadExtraProy.FechaServicio = objMant.fechaPM;
                            controlActividadExtraProy.Hrsaplico = objMant.horometroPMEjecutado;
                            controlActividadExtraProy.id = 0;
                            controlActividadExtraProy.idAct = actividadExtra;
                            controlActividadExtraProy.idMant = objMant.id;
                            controlActividadExtraProy.Observaciones = "";
                            controlActividadExtraProy.programado = false;
                            controlActividadExtraProy.UsuarioCap = vSesiones.sesionUsuarioDTO.id;
                            controlActividadExtraProy.Vigencia = auxActividadExtra.Vigencia;
                            _context.tblM_BitacoraControlAEMantProy.Add(controlActividadExtraProy);

                            controlActividadExtra.alta = true;
                            controlActividadExtra.Aplicado = true;
                            controlActividadExtra.fechaCaptura = DateTime.Now;
                            controlActividadExtra.Hrsaplico = objMant.horometroPMEjecutado;
                            controlActividadExtra.id = 0;
                            controlActividadExtra.idAct = actividadExtra;
                            controlActividadExtra.idMant = objMant.id;
                            if (ultimoRegistroActividadExtra == null)
                            {
                                var periodicidad = _context.tblM_CatPM_CatActividadPM.FirstOrDefault(x => x.modeloEquipoID == modeloEquipo && x.idAct == actividadExtra);
                                if (periodicidad == null) controlActividadExtra.idPerioricidad = 1;
                                else controlActividadExtra.idPerioricidad = periodicidad.id;
                            }
                            controlActividadExtra.UsuarioCap = vSesiones.sesionUsuarioDTO.id;
                            controlActividadExtra.vidaRestante = controlActividadExtraProy.Vigencia - (objGeneral.horometroPM - controlActividadExtra.Hrsaplico);
                            controlActividadExtra.vidaActual = objGeneral.horometroPM - controlActividadExtra.Hrsaplico;
                            _context.tblM_BitacoraControlActExt.Add(controlActividadExtra);
                        }

                    }

                    var ListaBitacoraControlDN = _context.tblM_BitacoraControlDN.Where(x => lstMantenimientos.Contains(x.idMant)).ToList();
                    var ListaBitacoraControlDNProy = _context.tblM_BitacoraControlDNMantProy.Where(x => lstMantenimientos.Contains(x.idMant)).ToList();
                    var actividadesDNsPorModelo = _context.tblM_CatPM_CatActividadPM.Where(x => x.modeloEquipoID == modeloEquipo && x.idCatTipoActividad == 4 && x.estado).ToList();
                    var actividadesDNID = actividadesDNsPorModelo.OrderBy(x => x.orden).Select(x => x.idAct).Distinct().ToList();


                    foreach (var actividadDN in actividadesDNID)
                    {
                        tblM_BitacoraControlDN controlActividadDN = new tblM_BitacoraControlDN();
                        tblM_BitacoraControlDNMantProy controlActividadDNProy = new tblM_BitacoraControlDNMantProy();
                        var auxActividadDN = tblgridDNProxTbl.FirstOrDefault(x => x.idAct == actividadDN);
                        var ultimoRegistroActividadDN = ListaBitacoraControlDN.Where(x => x.idAct == actividadDN).OrderByDescending(x => x.id).FirstOrDefault();
                        var ultimoRegistroActividadDNProy = ListaBitacoraControlDNProy.Where(x => x.idAct == actividadDN).OrderByDescending(x => x.id).FirstOrDefault();
                        var auxActividadDNPorModelo = actividadesDNsPorModelo.FirstOrDefault(x => x.idAct == actividadDN);
                        var auxPeriodicidad = _context.tblM_CatPM_CatActividadPM.FirstOrDefault(x => x.idAct == actividadDN && x.estado && x.modeloEquipoID == modeloEquipo && x.idCatTipoActividad == 4 && x.idDN == auxActividadDNPorModelo.idDN);

                        if (auxActividadDN == null || !auxActividadDN.aplicado)
                        {
                            controlActividadDNProy.estatus = true;
                            controlActividadDNProy.aplicado = false;
                            controlActividadDNProy.fechaCaptura = DateTime.Now;
                            controlActividadDNProy.FechaServicio = ultimoRegistroActividadDN == null ? DateTime.Now : ultimoRegistroActividadDN.fechaCaptura;
                            controlActividadDNProy.Hrsaplico = ultimoRegistroActividadDN == null ? objGeneral.horometroPM : ultimoRegistroActividadDN.Hrsaplico;
                            controlActividadDNProy.id = 0;
                            controlActividadDNProy.idAct = actividadDN;
                            controlActividadDNProy.idMant = objMant.id;
                            controlActividadDNProy.Observaciones = "";
                            controlActividadDNProy.programado = false;
                            controlActividadDNProy.UsuarioCap = vSesiones.sesionUsuarioDTO.id;
                            controlActividadDNProy.Vigencia = ultimoRegistroActividadDNProy == null ? auxActividadDNPorModelo.perioricidad : ultimoRegistroActividadDNProy.Vigencia;

                            _context.tblM_BitacoraControlDNMantProy.Add(controlActividadDNProy);

                            controlActividadDN.alta = true;
                            controlActividadDN.Aplicado = true;
                            controlActividadDN.fechaCaptura = ultimoRegistroActividadDN == null ? objGeneral.fechaPM : ultimoRegistroActividadDN.fechaCaptura;
                            controlActividadDN.Hrsaplico = ultimoRegistroActividadDN == null ? objGeneral.horometroPM : ultimoRegistroActividadDN.Hrsaplico;
                            controlActividadDN.id = 0;
                            controlActividadDN.idAct = actividadDN;
                            controlActividadDN.idMant = objMant.id;
                            controlActividadDN.idPerioricidad = ultimoRegistroActividadDN == null ? (auxPeriodicidad == null ? 11 : auxPeriodicidad.id) : ultimoRegistroActividadDN.idPerioricidad;
                            controlActividadDN.UsuarioCap = vSesiones.sesionUsuarioDTO.id;
                            controlActividadDN.vidaRestante = controlActividadDNProy.Vigencia - (objGeneral.horometroPM - controlActividadDN.Hrsaplico);
                            controlActividadDN.vidaActual = objGeneral.horometroPM - controlActividadDN.Hrsaplico;
                            _context.tblM_BitacoraControlDN.Add(controlActividadDN);
                        }
                        else
                        {
                            controlActividadDNProy.estatus = true;
                            controlActividadDNProy.aplicado = false;
                            controlActividadDNProy.fechaCaptura = DateTime.Now;
                            controlActividadDNProy.FechaServicio = objMant.fechaPM;
                            controlActividadDNProy.Hrsaplico = objMant.horometroPMEjecutado;
                            controlActividadDNProy.id = 0;
                            controlActividadDNProy.idAct = actividadDN;
                            controlActividadDNProy.idMant = objMant.id;
                            controlActividadDNProy.Observaciones = "";
                            controlActividadDNProy.programado = false;
                            controlActividadDNProy.UsuarioCap = vSesiones.sesionUsuarioDTO.id;
                            controlActividadDNProy.Vigencia = ultimoRegistroActividadDNProy == null ? auxActividadDNPorModelo.perioricidad : ultimoRegistroActividadDNProy.Vigencia;
                            _context.tblM_BitacoraControlDNMantProy.Add(controlActividadDNProy);

                            controlActividadDN.alta = true;
                            controlActividadDN.Aplicado = true;
                            controlActividadDN.fechaCaptura = DateTime.Now;
                            controlActividadDN.Hrsaplico = objMant.horometroPMEjecutado;
                            controlActividadDN.id = 0;
                            controlActividadDN.idAct = actividadDN;
                            controlActividadDN.idMant = objMant.id;
                            controlActividadDN.idPerioricidad = ultimoRegistroActividadDN == null ? 11 : ultimoRegistroActividadDN.idPerioricidad;
                            _context.tblM_BitacoraControlDN.Add(controlActividadDN);
                        }

                    }

                    var getListaActividadesExtra = getActividadesByPM(modeloEquipo, objMant.tipoPM);

                    foreach (var actividadExtra in getListaActividadesExtra)
                    {
                        tblM_BitacoraActividadesMantProy objtblM_BitacoraActividadesMantProy = new tblM_BitacoraActividadesMantProy();
                        objtblM_BitacoraActividadesMantProy.id = 0;
                        objtblM_BitacoraActividadesMantProy.aplicar = true;
                        objtblM_BitacoraActividadesMantProy.estatus = true;
                        objtblM_BitacoraActividadesMantProy.fechaCaptura = DateTime.Now;
                        objtblM_BitacoraActividadesMantProy.idAct = actividadExtra.idAct;
                        objtblM_BitacoraActividadesMantProy.idMant = objMant.id;
                        objtblM_BitacoraActividadesMantProy.idPm = actividadExtra.idPM;
                        objtblM_BitacoraActividadesMantProy.Observaciones = "";
                        objtblM_BitacoraActividadesMantProy.UsuarioCap = vSesiones.sesionUsuarioDTO.id;
                        _context.tblM_BitacoraActividadesMantProy.Add(objtblM_BitacoraActividadesMantProy);
                    }

                    if (referencias != null)
                    {
                        referencias.ForEach(e => { _context.tblM_MantenimientoPm_Archivo.Add(e); });
                    }
                    _context.SaveChanges();
                    dbContextTransaction.Commit();
                    return objMant;
                }
                catch (Exception e)
                {
                    dbContextTransaction.Rollback();
                    LogError(0, 0, "MatenimientoPMController", "GuardarEjecutado", e, AccionEnum.AGREGAR, 0, new { mttoID = mantAnterior.id });
                    return null;
                }
            }
        }

        public void guardarDetAct(List<tblM_BitacoraControlAceiteMant> arrJG, tblM_BitacoraActividadesMantProy actividadExtra, int idMantenimiento, tblM_CatMaquina maquinaria)
        {

            try
            {

                foreach (var itemObjLubricantes in arrJG)
                {
                    List<tblM_MiscelaneoMantenimiento> listaCompontesActividad = _context.tblM_MiscelaneoMantenimiento
                        .Where(y => y.idAct == actividadExtra.idAct
                            && y.idCompVis == itemObjLubricantes.idComp
                            && y.estado == true
                            && y.modeloEquipoID == maquinaria.modeloEquipoID

                    ).ToList();

                    foreach (var misGuardar in listaCompontesActividad)
                    {
                        tblM_BitacoraDetActividadesMantProy tblM_BitacoraDetActividadesMantProyObj = new tblM_BitacoraDetActividadesMantProy();

                        tblM_BitacoraDetActividadesMantProyObj.aplicar = false;
                        tblM_BitacoraDetActividadesMantProyObj.id = 0;
                        tblM_BitacoraDetActividadesMantProyObj.idAct = actividadExtra.idAct;
                        tblM_BitacoraDetActividadesMantProyObj.idCompVis = itemObjLubricantes.idComp;
                        tblM_BitacoraDetActividadesMantProyObj.idMant = idMantenimiento;
                        tblM_BitacoraDetActividadesMantProyObj.modeloEquipoID = maquinaria.modeloEquipoID;
                        tblM_BitacoraDetActividadesMantProyObj.programado = false;

                        //   GuardarBitacoraDetActividadesMantProy(tblM_BitacoraDetActividadesMantProyObj);
                    }

                }
            }
            catch (Exception)
            {

                throw;
            }


        }


        public object GuardarBitacoraDetActividadesMantProy(tblM_BitacoraDetActividadesMantProy ObjAct)
        {
            object result = new object();
            IObjectSet<tblM_BitacoraDetActividadesMantProy> _objectSet = ((IObjectContextAdapter)_context).ObjectContext.CreateObjectSet<tblM_BitacoraDetActividadesMantProy>();
            try
            {
                _objectSet.AddObject(ObjAct);
                _context.SaveChanges();
            }
            catch (Exception)
            {
                throw;
            }
            return result;
        }


        public tblM_MatenimientoPm GuardarMantenimientoPM(int idMantenimiento, tblM_MatenimientoPm objGeneral, int tipo)
        {
            tblM_MatenimientoPm objMant = new tblM_MatenimientoPm();

            try
            {
                var anterior = _context.tblM_MatenimientoPm.FirstOrDefault(x => x.id == idMantenimiento);
                anterior.fechaPM = objGeneral.fechaPM;
                anterior.estatus = false;
                anterior.estadoMantenimiento = 3;
                anterior.horometroPMEjecutado = objGeneral.horometroPMEjecutado;

                _context.Entry(anterior).State = System.Data.Entity.EntityState.Modified;
                _context.SaveChanges();


                var ultimoHorometro = _context.tblM_CapHorometro.Where(x => x.Economico == anterior.economicoID).OrderByDescending(x => x.id).FirstOrDefault().Horometro;
                int manttoActual = GetNextPM(anterior.tipoPM);
                objMant.economicoID = objGeneral.economicoID;
                objMant.horometroUltCapturado = ultimoHorometro; //objGeneral.horometroUltCapturado;
                objMant.fechaUltCapturado = DateTime.Now;
                objMant.tipoPM = manttoActual;
                objMant.fechaPM = FechaProximoPM(objGeneral.economicoID, objGeneral.fechaPM).AddHours(5);
                objMant.horometroPM = objGeneral.horometroPM; // objGeneral.horometroPM;
                objMant.personalRealizo = objGeneral.personalRealizo;
                objMant.observaciones = "";
                objMant.horometroProy = objGeneral.horometroPM + 250;
                objMant.fechaProy = FechaProximoPM(objGeneral.economicoID, objGeneral.fechaPM).AddHours(5); //FechaProximo(objGeneral.economicoID);
                objMant.tipoMantenimientoProy = GetNextPM(manttoActual);
                objMant.actual = true;
                objMant.fechaProyFin = objMant.fechaProy.AddHours(1); //FechaProximo(objGeneral.economicoID);
                objMant.fechaCaptura = DateTime.Now;
                objMant.idMaquina = objGeneral.idMaquina;
                objMant.estatus = true;
                objMant.planeador = objGeneral.planeador;
                objMant.UsuarioCap = objGeneral.UsuarioCap;
                objMant.estadoMantenimiento = 1;
                objMant.horometroPMEjecutado = objGeneral.horometroPM;

                _context.tblM_MatenimientoPm.Add(objMant);
                _context.SaveChanges();
            }
            catch (Exception e)
            {
            }
            GuardarRenderizado(objMant.id);
            return objMant;
        }

        private DateTime FechaProximo(string economico)
        {
            var horasRitmoTrabajo = CalculoHrsPromDiario(economico);
            DateTime fechaActual = DateTime.Now;
            decimal count = 0;

            while (count >= 250)
            {
                fechaActual.AddDays(1);
                count += (decimal)horasRitmoTrabajo;
            }


            return fechaActual;
        }

        private DateTime FechaProximoPM(string economico, DateTime fechaActual)
        {

            TimeSpan ts = new TimeSpan(0, 0, 0);
            fechaActual = fechaActual.Date + ts;


            try
            {
                decimal horasRitmoTrabajo = (decimal)CalculoHrsPromDiario(economico);
                decimal count = 0;

                while (count <= 250)
                {
                    fechaActual = fechaActual.AddDays(1);
                    count += horasRitmoTrabajo;
                }


                return fechaActual;
            }
            catch (Exception)
            {

                int horasRitmoTrabajo = (int)CalculoHrsPromDiario(economico);
                int count = 0;

                while (count <= 250)
                {
                    fechaActual = fechaActual.AddDays(1);
                    count += horasRitmoTrabajo;
                }


                return fechaActual;
            }

        }

        private int GetNextPM(int tipoPM)
        {

            tipoPM += 1;
            var nextPM = _context.tblM_catPM.FirstOrDefault(y => y.id == tipoPM);

            if (nextPM != null)
            {
                return nextPM.id;
            }
            else
            {
                return _context.tblM_catPM.FirstOrDefault().id;
            }

        }


        public tblM_MantenimientoPm_Archivo GetUltimoArchivoMantenimiento()
        {
            return _context.tblM_MantenimientoPm_Archivo.ToList().LastOrDefault() != null ? _context.tblM_MantenimientoPm_Archivo.ToList().LastOrDefault() : null;
        }
        public ReporteProgramadoDTO GetReporteProgramado(int idMant)
        {
            ReporteProgramadoDTO rep = new ReporteProgramadoDTO();
            List<ReporteMiscelaneosDTO> listaMiscelaneos = new List<ReporteMiscelaneosDTO>();
            List<ReporteActExtDNsDTO> listaActExtDNs = new List<ReporteActExtDNsDTO>();
            List<tblM_BitacoraActividadesMantProy> listaActividadesMantProy = new List<tblM_BitacoraActividadesMantProy>();

            List<string> listaLeyendas = new List<string>();

            rep.miscelaneos = listaMiscelaneos;
            rep.leyendas = listaLeyendas;
            rep.actExtDNs = listaActExtDNs;

            rep.componentes1 = "";
            rep.componentes2 = "";
            rep.componentes3 = "";
            rep.componentes4 = "";
            rep.componentes5 = "";

            try
            {
                var mantenimiento = _context.tblM_MatenimientoPm.FirstOrDefault(x => x.estadoMantenimiento == 2 && x.id == idMant);
                var modeloEquipoID = ConsultarModelo(mantenimiento.economicoID);

                var perRealizo = ConsultaPersonalIdManteniminto(mantenimiento.personalRealizo);
                var perEnterado = ConsultaPersonalIdManteniminto(mantenimiento.planeador);

                rep.realizo = perRealizo != null ? string.Format("{0} {1} {2}", perRealizo.nombre, perRealizo.ape_paterno, perRealizo.ape_materno) : "";
                rep.enterado = perEnterado != null ? string.Format("{0} {1} {2}", perEnterado.nombre, perEnterado.ape_paterno, perEnterado.ape_materno) : "";
                rep.fechaFooter = DateTime.Today.ToShortDateString();
                rep.horometroActual = mantenimiento.horometroUltCapturado.ToString();

                rep.comentarios = mantenimiento.observaciones;

                var actividades = _context.tblM_CatActividadPM.ToList();

                if (mantenimiento != null)
                {
                    var aceites = _context.tblM_BitacoraControlAceiteMant.Where(x => x.idMant == idMant && x.estatus).Select(x => x.idAct).ToList();
                    var lubProg = _context.tblM_BitacoraControlAceiteMantProy.Where(x => x.idMant == idMant && x.programado && x.estatus).ToList();
                    var actExtProg = _context.tblM_BitacoraControlAEMantProy.Where(x => x.idMant == idMant && x.programado).ToList();
                    var dnProg = _context.tblM_BitacoraControlDNMantProy.Where(x => x.idMant == idMant && x.programado).ToList();
                    var idPM = _context.tblM_catPM.FirstOrDefault(x => x.id == mantenimiento.tipoPM).PM;
                    var lstObjActividadPm = _context.tblM_CatPM_CatActividadPM.Where(x => x.modeloEquipoID == modeloEquipoID && x.idDN == 0 && x.estado == true && x.perioricidad <= 0 && x.idCatTipoActividad == 1).ToList();
                    var idObjActividadPm = lstObjActividadPm.Select(x => x.idAct);
                    //var ActividadesMantProy = _context.tblM_BitacoraActividadesMantProy.Where(x => x.idMant == idMant && x.estatus && x.).ToList();
                    var ActividadesMantProy = _context.tblM_BitacoraActividadesMantProy.Where(y => y.idMant == idMant && idObjActividadPm.Contains(y.idAct) && y.estatus).ToList();
                    #region Sección "Refacciones y materiales requeridos:"
                    var miscelaneos = _context.tblM_CatFiltroMant.ToList().Where(x => lubProg.Where(w => w.idMisc != 0).Select(y => y.idMisc).Contains(x.id)).ToList();

                    rep.miscelaneos = _context.tblM_BitacoraDetActividadesMantProy.ToList().Where(x => x.idMant == idMant && x.programado).Select(q => new ReporteMiscelaneosDTO
                    {
                        check = "x",
                        piezas = "",
                        modelo = q.modelo + " " + q.cantidad + "pz",
                        codigo = "",
                        descripcion = ""
                    }).Distinct().ToList();
                    #endregion
                    #region Sección "Actividades a ejecutar:"
                    List<int> actLeyendasID = new List<int>();

                    actLeyendasID.AddRange(actExtProg.Select(x => x.idAct));
                    actLeyendasID.AddRange(dnProg.Select(x => x.idAct));
                    actLeyendasID.AddRange(ActividadesMantProy.Select(x => x.idAct));

                    var leyendasFiltradas = _context.tblM_CatPM_CatActividadPM.Where(x => x.leyenda && x.modeloEquipoID == modeloEquipoID).Select(w => w.idAct).ToList();

                    var inspeccionesaRealizarServicio = (from a in _context.tblM_BitacoraActividadesMantProy
                                                         join b in _context.tblM_CatActividadPM on a.idAct equals b.id
                                                         where a.idMant == idMant && (a.idPm == 0 || a.idPm == mantenimiento.tipoPM) && a.aplicar
                                                         select b).Select(x => x.descripcionActividad).ToList();

                    //   var leyendaLubricantes = _context.tblM_CatPM_CatActividadPM.Where(x => x.modeloEquipoID == modeloEquipoID && (x.idPM == 0 || x.idPM == idPM)).Select(x => x.idAct).Except(aceites).ToList();
                    var actividadesFiltradas = actLeyendasID.Where(x => leyendasFiltradas.Contains(x)).ToList();
                    var actividadesLeyendas = actividades.Where(x => actLeyendasID.Contains(x.id)).Select(w => w.descripcionActividad).ToList();
                    //var actividadesLeyendasLubricantes = actividades.Where(x => leyendaLubricantes.Contains(x.id)).Select(w => w.descripcionActividad).ToList();
                    List<string> actividadesLeyendasLubricantes = new List<string>();
                    foreach (var item in lubProg.Where(x => x.programado))
                    {
                        var objTempComponente = _context.tblM_PMComponenteModelo.FirstOrDefault(c => c.componenteID == item.idComp && c.modeloID == modeloEquipoID);
                        if (objTempComponente != null)
                        {
                            string infoDescripcion = "Cambiar Fluido de " + objTempComponente.Componente.Descripcion.ToLower();
                            actividadesLeyendasLubricantes.Add(infoDescripcion);
                        }
                    }
                    rep.leyendas.AddRange(actividadesLeyendasLubricantes);
                    rep.leyendas.AddRange(actividadesLeyendas);

                    rep.leyendas = rep.leyendas.Distinct().ToList();
                    #endregion

                    #region Sección "Inspecciones a realizar durante el servicio"
                    if (actExtProg.Count > 0)
                    {
                        rep.actExtDNs.AddRange(actividades.Where(x => actExtProg.Select(y => y.idAct).Contains(x.id)).Select(w => new ReporteActExtDNsDTO
                        {
                            check = "x",
                            descripcion = w.descripcionActividad
                        }));
                    }
                    if (dnProg.Count > 0)
                    {
                        rep.actExtDNs.AddRange(actividades.Where(x => dnProg.Select(y => y.idAct).Contains(x.id)).Select(w => new ReporteActExtDNsDTO
                        {
                            check = "x",
                            descripcion = w.descripcionActividad
                        }));
                    }
                    #endregion

                    #region Sección "Descripción de actividades realizadas y observaciones:"
                    if (lubProg.Count > 0)
                    {
                        //var componentesID = lubProg.Select(x => x.idComp).ToList();
                        //var componentes = _context.tblM_CatComponente.Where(x => componentesID.Contains(x.id)).Select(w => w.subConjunto.descripcion).ToList();

                        //var compString = string.Join(", ", componentes);

                        //rep.componentes1 = compString;
                        //rep.componentes2 = compString;
                    }

                    if (actExtProg.Count > 0)
                    {
                        // var componentesAct = _context.tblM_PMComponenteModelo.Where(x => x.modeloID == modeloEquipoID && actExtProg.Select(y => y.idAct).Contains(x.idac)).Select(q => q.idCompVis).ToList();
                        //var componentesDesc = _context.tblM_CatComponente.Where(x => componentesAct.Contains(x.id)).Select(w => w.descripcion).ToList();
                        /* var compString = string.Join(", ", componentesDesc);

                         rep.componentes3 = compString;
                         rep.componentes4 = compString;
                         rep.componentes5 = compString;*/
                    }
                    #endregion
                }
            }
            catch (Exception)
            {

            }

            return rep;
        }

        public List<tblM_CatFiltroMant> fillCboCatFiltroMant()
        {
            return _context.tblM_CatFiltroMant.ToList();
        }

        public List<tblM_CatMarcaMant> fillCboMarcaFiltro()
        {
            var data = _context.tblM_CatMarcaMant.ToList();
            return data;
        }

        public bool GuardarFiltro(tblM_CatFiltroMant obj)
        {
            var existeFiltro = _context.tblM_CatFiltroMant.Where(x => x.modelo == obj.modelo).ToList().Count;
            if (existeFiltro > 0) { return false; }
            else
            {
                _context.tblM_CatFiltroMant.Add(obj);
                _context.SaveChanges();
                return true;
            }
        }

        public byte[] GetZipDocumentosPM(List<tblM_DocumentosMaquinaria> documentos)
        {
            List<Tuple<byte[], string>> archivos = new List<Tuple<byte[], string>>();
            foreach (var item in documentos)
            {
                var auxDatos = System.IO.File.ReadAllBytes(item.nombreRuta);
                Tuple<byte[], string> auxArchivo = new Tuple<byte[], string>(auxDatos, item.nombreArchivo);
                archivos.Add(auxArchivo);
            }
            var data = GlobalUtils.GetZipVariosDocumentos(archivos);
            return data;
        }

        public List<tblM_DocumentosMaquinaria> getDocumentosByID(List<int> ids)
        {
            return _context.tblM_DocumentosMaquinaria.Where(x => ids.Contains(x.id)).ToList();
        }

        public MemoryStream DescargarExcelJOMAGALI(string areaCuenta, DateTime fechaInicio, DateTime fechaFin, string economico)
        {
            try
            {
                #region Información Inicial
                var data = _context.tblM_MatenimientoPm.Where(x =>
                    DbFunctions.TruncateTime(x.fechaPM) >= DbFunctions.TruncateTime(fechaInicio) &&
                    DbFunctions.TruncateTime(x.fechaPM) <= DbFunctions.TruncateTime(fechaFin)
                ).ToList().Join(
                    _context.tblM_CatMaquina.Where(x => x.centro_costos == areaCuenta).ToList().Where(x => (!string.IsNullOrEmpty(economico)) ? x.noEconomico == economico : true).ToList(),
                    man => man.economicoID,
                    maq => maq.noEconomico,
                    (man, maq) => new { man, maq }
                ).Join(
                    _context.tblM_BitacoraControlAceiteMantProy,
                    man_maq => man_maq.man.id,
                    detProy => detProy.idMant,
                    (man_maq, detProy) => new { man_maq, detProy }
                ).Join(
                    _context.tblM_BitacoraControlAceiteMant,
                    man_maq_detProy => new { man_maq_detProy.detProy.idMant, man_maq_detProy.detProy.idComp },
                    det => new { det.idMant, det.idComp },
                    (man_maq_detProy, det) => new { man_maq_detProy, det }
                ).ToList();

                var colorNaranja = Color.FromArgb(255, 226, 107, 10);
                var colorAzul = Color.FromArgb(255, 83, 141, 213);
                var colorAmarillo = Color.FromArgb(255, 255, 255, 0);

                var listaComponentes = _context.tblM_CatComponentesViscosidades.ToList();
                var listaSuministros = _context.tblM_CatSuministros.ToList();
                #endregion

                using (ExcelPackage excel = new ExcelPackage())
                {
                    var listaEconomicos = data.GroupBy(x => new { x.man_maq_detProy.man_maq.man.economicoID }).Select(x => new { economico = x.Key, lista = x.ToList() }).ToList();

                    foreach (var eco in listaEconomicos)
                    {
                        var hoja = excel.Workbook.Worksheets.Add(eco.economico.economicoID);

                        var cantidadMaximaComponentes = eco.lista.GroupBy(x => new
                        {
                            x.man_maq_detProy.man_maq.man.economicoID,
                            x.man_maq_detProy.man_maq.man.id
                        }).Select(x => new { economico = x.Key.economicoID, mantenimiento_id = x.Key.id, lista = x.ToList() }).ToList().Max(x => x.lista.Count()) * 2; //Se multiplica por dos la cantidad ya que cada componente abarca dos columnas.
                        var ultimaColumna = ExcelUtilities.GetExcelColumnName(cantidadMaximaComponentes);
                        var penultimaColumna = ExcelUtilities.GetExcelColumnName(cantidadMaximaComponentes - 1);

                        #region Encabezado Hoja
                        hoja.Cells[string.Format("A1:{0}1", ultimaColumna)].Value = "GRUPO CONSTRUCCIONES PLANIFICADAS S.A. DE C.V.";
                        hoja.Cells[string.Format("A1:{0}1", ultimaColumna)].Style.Font.Bold = true;
                        hoja.Cells[string.Format("A1:{0}1", ultimaColumna)].Style.Font.Size = 16;
                        hoja.Cells[string.Format("A1:{0}1", ultimaColumna)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        hoja.Cells[string.Format("A1:{0}1", ultimaColumna)].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        hoja.Cells[string.Format("A1:{0}1", ultimaColumna)].Merge = true;
                        hoja.Cells[string.Format("A1:{0}1", ultimaColumna)].Style.Border.Right.Style = ExcelBorderStyle.Medium;

                        hoja.Cells[string.Format("A2:{0}2", ultimaColumna)].Value = "GERENCIA DE MAQUINARIA Y EQUIPO";
                        hoja.Cells[string.Format("A2:{0}2", ultimaColumna)].Style.Font.Bold = true;
                        hoja.Cells[string.Format("A2:{0}2", ultimaColumna)].Style.Font.Size = 14;
                        hoja.Cells[string.Format("A2:{0}2", ultimaColumna)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        hoja.Cells[string.Format("A2:{0}2", ultimaColumna)].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        hoja.Cells[string.Format("A2:{0}2", ultimaColumna)].Merge = true;
                        hoja.Cells[string.Format("A2:{0}2", ultimaColumna)].Style.Border.Right.Style = ExcelBorderStyle.Medium;

                        hoja.Cells[string.Format("A3:{0}3", ultimaColumna)].Value = "ADMINISTRACIÓN DE MAQUINARIA";
                        hoja.Cells[string.Format("A3:{0}3", ultimaColumna)].Style.Font.Bold = true;
                        hoja.Cells[string.Format("A3:{0}3", ultimaColumna)].Style.Font.Size = 12;
                        hoja.Cells[string.Format("A3:{0}3", ultimaColumna)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        hoja.Cells[string.Format("A3:{0}3", ultimaColumna)].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        hoja.Cells[string.Format("A3:{0}3", ultimaColumna)].Merge = true;
                        hoja.Cells[string.Format("A3:{0}3", ultimaColumna)].Style.Border.Right.Style = ExcelBorderStyle.Medium;

                        hoja.Cells[string.Format("A4:{0}4", ultimaColumna)].Merge = true;
                        hoja.Cells[string.Format("A4:{0}4", ultimaColumna)].Style.Border.Right.Style = ExcelBorderStyle.Medium;

                        hoja.Cells[string.Format("A5:{0}5", ultimaColumna)].Value = "SERVICIOS DE MANTENIMIENTO EJECUTADOS";
                        hoja.Cells[string.Format("A5:{0}5", ultimaColumna)].Style.Font.Bold = true;
                        hoja.Cells[string.Format("A5:{0}5", ultimaColumna)].Style.Font.Size = 12;
                        hoja.Cells[string.Format("A5:{0}5", ultimaColumna)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        hoja.Cells[string.Format("A5:{0}5", ultimaColumna)].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        hoja.Cells[string.Format("A5:{0}5", ultimaColumna)].Merge = true;
                        hoja.Cells[string.Format("A5:{0}5", ultimaColumna)].Style.Border.Right.Style = ExcelBorderStyle.Medium;

                        hoja.Cells[string.Format("A6:{0}6", ultimaColumna)].Value = eco.economico.economicoID;
                        hoja.Cells[string.Format("A6:{0}6", ultimaColumna)].Style.Font.Bold = true;
                        hoja.Cells[string.Format("A6:{0}6", ultimaColumna)].Style.Font.Size = 13;
                        hoja.Cells[string.Format("A6:{0}6", ultimaColumna)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        hoja.Cells[string.Format("A6:{0}6", ultimaColumna)].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        hoja.Cells[string.Format("A6:{0}6", ultimaColumna)].Merge = true;
                        hoja.Cells[string.Format("A6:{0}6", ultimaColumna)].Style.Border.Right.Style = ExcelBorderStyle.Medium;
                        hoja.Cells[string.Format("A6:{0}6", ultimaColumna)].Style.Border.Bottom.Style = ExcelBorderStyle.Medium;

                        hoja.Cells[string.Format("A7:{0}7", ultimaColumna)].Merge = true;

                        #region Imagen Logo
                        var imgLogo = Image.FromFile(HttpContext.Current.Server.MapPath("\\Content\\img\\logo\\logo.jpg"));
                        var nuevoAncho = (int)(imgLogo.Width * 0.90);
                        var nuevoAlto = (int)(imgLogo.Height * 0.90);
                        var nuevaImgLogo = ResizeImage(imgLogo, nuevoAncho, nuevoAlto);
                        var logo = hoja.Drawings.AddPicture("logo", nuevaImgLogo);
                        logo.SetPosition(0, 0, 0, 0);
                        #endregion
                        #endregion

                        var listaMantenimientos = eco.lista.GroupBy(x => x.man_maq_detProy.man_maq.man.id).Select(x => new { mantenimiento_id = x.Key, lista = x.ToList() }).ToList();
                        var contadorRenglon = 8;

                        foreach (var mantenimiento in listaMantenimientos)
                        {
                            var registroMantenimiento = mantenimiento.lista[0].man_maq_detProy.man_maq.man;

                            if (cantidadMaximaComponentes >= 10)
                            {
                                #region Renglones Encabezado Mantenimiento
                                //Se asume que siempre habrá por lo menos cuatro componentes para el primer grupo de celdas del primer renglón del mantenimiento.
                                hoja.Cells[string.Format("A{0}:H{0}", contadorRenglon)].Value = "HORÓMETRO DE MUESTREO Y/O SERVICIO:";
                                hoja.Cells[string.Format("A{0}:H{0}", contadorRenglon)].Style.Font.Bold = true;
                                hoja.Cells[string.Format("A{0}:H{0}", contadorRenglon)].Style.Font.Size = 12;
                                hoja.Cells[string.Format("A{0}:H{0}", contadorRenglon)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                hoja.Cells[string.Format("A{0}:H{0}", contadorRenglon)].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                hoja.Cells[string.Format("A{0}:H{0}", contadorRenglon)].Merge = true;
                                hoja.Cells[string.Format("A{0}:H{0}", contadorRenglon)].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                hoja.Cells[string.Format("A{0}:H{0}", contadorRenglon)].Style.Fill.BackgroundColor.SetColor(colorNaranja);
                                hoja.Cells[string.Format("A{0}:H{0}", contadorRenglon)].Style.Border.Top.Style = ExcelBorderStyle.Medium;
                                hoja.Cells[string.Format("A{0}:H{0}", contadorRenglon)].Style.Border.Left.Style = ExcelBorderStyle.Medium;
                                hoja.Cells[string.Format("A{0}:H{0}", contadorRenglon)].Style.Border.Right.Style = ExcelBorderStyle.Medium;
                                hoja.Cells[string.Format("A{0}:H{0}", contadorRenglon)].Style.Border.Bottom.Style = ExcelBorderStyle.Medium;

                                hoja.Cells[string.Format("I{0}:{1}{0}", contadorRenglon, ultimaColumna)].Value = registroMantenimiento.horometroPM.ToString("N");
                                hoja.Cells[string.Format("I{0}:{1}{0}", contadorRenglon, ultimaColumna)].Style.Font.Bold = true;
                                hoja.Cells[string.Format("I{0}:{1}{0}", contadorRenglon, ultimaColumna)].Style.Font.Size = 12;
                                hoja.Cells[string.Format("I{0}:{1}{0}", contadorRenglon, ultimaColumna)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                hoja.Cells[string.Format("I{0}:{1}{0}", contadorRenglon, ultimaColumna)].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                hoja.Cells[string.Format("I{0}:{1}{0}", contadorRenglon, ultimaColumna)].Merge = true;
                                hoja.Cells[string.Format("I{0}:{1}{0}", contadorRenglon, ultimaColumna)].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                hoja.Cells[string.Format("I{0}:{1}{0}", contadorRenglon, ultimaColumna)].Style.Fill.BackgroundColor.SetColor(colorNaranja);
                                hoja.Cells[string.Format("I{0}:{1}{0}", contadorRenglon, ultimaColumna)].Style.Border.Top.Style = ExcelBorderStyle.Medium;
                                hoja.Cells[string.Format("I{0}:{1}{0}", contadorRenglon, ultimaColumna)].Style.Border.Left.Style = ExcelBorderStyle.Medium;
                                hoja.Cells[string.Format("I{0}:{1}{0}", contadorRenglon, ultimaColumna)].Style.Border.Right.Style = ExcelBorderStyle.Medium;
                                hoja.Cells[string.Format("I{0}:{1}{0}", contadorRenglon, ultimaColumna)].Style.Border.Bottom.Style = ExcelBorderStyle.Medium;

                                contadorRenglon++;

                                hoja.Cells[string.Format("A{0}", contadorRenglon)].Value = ((TipoMantenimientoPMEnum)registroMantenimiento.tipoPM).GetDescription();
                                hoja.Cells[string.Format("A{0}", contadorRenglon)].Style.Font.Bold = true;
                                hoja.Cells[string.Format("A{0}", contadorRenglon)].Style.Font.Size = 12;
                                hoja.Cells[string.Format("A{0}", contadorRenglon)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                hoja.Cells[string.Format("A{0}", contadorRenglon)].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                hoja.Cells[string.Format("A{0}", contadorRenglon)].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                hoja.Cells[string.Format("A{0}", contadorRenglon)].Style.Fill.BackgroundColor.SetColor(colorAzul);
                                hoja.Cells[string.Format("A{0}", contadorRenglon)].Style.Border.Top.Style = ExcelBorderStyle.Medium;
                                hoja.Cells[string.Format("A{0}", contadorRenglon)].Style.Border.Left.Style = ExcelBorderStyle.Medium;
                                hoja.Cells[string.Format("A{0}", contadorRenglon)].Style.Border.Right.Style = ExcelBorderStyle.Medium;
                                hoja.Cells[string.Format("A{0}", contadorRenglon)].Style.Border.Bottom.Style = ExcelBorderStyle.Medium;

                                hoja.Cells[string.Format("B{0}", contadorRenglon)].Value = registroMantenimiento.fechaPM.ToShortDateString();
                                hoja.Cells[string.Format("B{0}", contadorRenglon)].Style.Font.Bold = true;
                                hoja.Cells[string.Format("B{0}", contadorRenglon)].Style.Font.Size = 12;
                                hoja.Cells[string.Format("B{0}", contadorRenglon)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                                hoja.Cells[string.Format("B{0}", contadorRenglon)].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                hoja.Cells[string.Format("B{0}", contadorRenglon)].Style.Border.Top.Style = ExcelBorderStyle.Medium;
                                hoja.Cells[string.Format("B{0}", contadorRenglon)].Style.Border.Left.Style = ExcelBorderStyle.Medium;
                                hoja.Cells[string.Format("B{0}", contadorRenglon)].Style.Border.Right.Style = ExcelBorderStyle.Medium;
                                hoja.Cells[string.Format("B{0}", contadorRenglon)].Style.Border.Bottom.Style = ExcelBorderStyle.Medium;

                                hoja.Cells[string.Format("C{0}:{1}{0}", contadorRenglon, ultimaColumna)].Value = string.Join(", ", mantenimiento.lista.Where(x => !string.IsNullOrEmpty(x.man_maq_detProy.detProy.Observaciones)).Select(x => x.man_maq_detProy.detProy.Observaciones).ToList());
                                hoja.Cells[string.Format("C{0}:{1}{0}", contadorRenglon, ultimaColumna)].Style.Font.Bold = true;
                                hoja.Cells[string.Format("C{0}:{1}{0}", contadorRenglon, ultimaColumna)].Style.Font.Size = 12;
                                hoja.Cells[string.Format("C{0}:{1}{0}", contadorRenglon, ultimaColumna)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                hoja.Cells[string.Format("C{0}:{1}{0}", contadorRenglon, ultimaColumna)].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                hoja.Cells[string.Format("C{0}:{1}{0}", contadorRenglon, ultimaColumna)].Merge = true;
                                hoja.Cells[string.Format("C{0}:{1}{0}", contadorRenglon, ultimaColumna)].Style.Border.Top.Style = ExcelBorderStyle.Medium;
                                hoja.Cells[string.Format("C{0}:{1}{0}", contadorRenglon, ultimaColumna)].Style.Border.Left.Style = ExcelBorderStyle.Medium;
                                hoja.Cells[string.Format("C{0}:{1}{0}", contadorRenglon, ultimaColumna)].Style.Border.Right.Style = ExcelBorderStyle.Medium;
                                hoja.Cells[string.Format("C{0}:{1}{0}", contadorRenglon, ultimaColumna)].Style.Border.Bottom.Style = ExcelBorderStyle.Medium;

                                contadorRenglon++;
                                #endregion
                            }
                            else
                            {
                                #region Renglones Encabezado Mantenimiento
                                hoja.Cells[string.Format("A{0}:{1}{0}", contadorRenglon, penultimaColumna)].Value = "HORÓMETRO DE MUESTREO Y/O SERVICIO:";
                                hoja.Cells[string.Format("A{0}:{1}{0}", contadorRenglon, penultimaColumna)].Style.Font.Bold = true;
                                hoja.Cells[string.Format("A{0}:{1}{0}", contadorRenglon, penultimaColumna)].Style.Font.Size = 12;
                                hoja.Cells[string.Format("A{0}:{1}{0}", contadorRenglon, penultimaColumna)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                hoja.Cells[string.Format("A{0}:{1}{0}", contadorRenglon, penultimaColumna)].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                hoja.Cells[string.Format("A{0}:{1}{0}", contadorRenglon, penultimaColumna)].Merge = true;
                                hoja.Cells[string.Format("A{0}:{1}{0}", contadorRenglon, penultimaColumna)].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                hoja.Cells[string.Format("A{0}:{1}{0}", contadorRenglon, penultimaColumna)].Style.Fill.BackgroundColor.SetColor(colorNaranja);
                                hoja.Cells[string.Format("A{0}:{1}{0}", contadorRenglon, penultimaColumna)].Style.Border.Top.Style = ExcelBorderStyle.Medium;
                                hoja.Cells[string.Format("A{0}:{1}{0}", contadorRenglon, penultimaColumna)].Style.Border.Left.Style = ExcelBorderStyle.Medium;
                                hoja.Cells[string.Format("A{0}:{1}{0}", contadorRenglon, penultimaColumna)].Style.Border.Right.Style = ExcelBorderStyle.Medium;
                                hoja.Cells[string.Format("A{0}:{1}{0}", contadorRenglon, penultimaColumna)].Style.Border.Bottom.Style = ExcelBorderStyle.Medium;

                                hoja.Cells[string.Format("{1}{0}:{1}{0}", contadorRenglon, ultimaColumna)].Value = registroMantenimiento.horometroPM.ToString("N");
                                hoja.Cells[string.Format("{1}{0}:{1}{0}", contadorRenglon, ultimaColumna)].Style.Font.Bold = true;
                                hoja.Cells[string.Format("{1}{0}:{1}{0}", contadorRenglon, ultimaColumna)].Style.Font.Size = 12;
                                hoja.Cells[string.Format("{1}{0}:{1}{0}", contadorRenglon, ultimaColumna)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                hoja.Cells[string.Format("{1}{0}:{1}{0}", contadorRenglon, ultimaColumna)].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                hoja.Cells[string.Format("{1}{0}:{1}{0}", contadorRenglon, ultimaColumna)].Merge = true;
                                hoja.Cells[string.Format("{1}{0}:{1}{0}", contadorRenglon, ultimaColumna)].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                hoja.Cells[string.Format("{1}{0}:{1}{0}", contadorRenglon, ultimaColumna)].Style.Fill.BackgroundColor.SetColor(colorNaranja);
                                hoja.Cells[string.Format("{1}{0}:{1}{0}", contadorRenglon, ultimaColumna)].Style.Border.Top.Style = ExcelBorderStyle.Medium;
                                hoja.Cells[string.Format("{1}{0}:{1}{0}", contadorRenglon, ultimaColumna)].Style.Border.Left.Style = ExcelBorderStyle.Medium;
                                hoja.Cells[string.Format("{1}{0}:{1}{0}", contadorRenglon, ultimaColumna)].Style.Border.Right.Style = ExcelBorderStyle.Medium;
                                hoja.Cells[string.Format("{1}{0}:{1}{0}", contadorRenglon, ultimaColumna)].Style.Border.Bottom.Style = ExcelBorderStyle.Medium;

                                contadorRenglon++;

                                hoja.Cells[string.Format("A{0}", contadorRenglon)].Value = ((TipoMantenimientoPMEnum)registroMantenimiento.tipoPM).GetDescription();
                                hoja.Cells[string.Format("A{0}", contadorRenglon)].Style.Font.Bold = true;
                                hoja.Cells[string.Format("A{0}", contadorRenglon)].Style.Font.Size = 12;
                                hoja.Cells[string.Format("A{0}", contadorRenglon)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                hoja.Cells[string.Format("A{0}", contadorRenglon)].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                hoja.Cells[string.Format("A{0}", contadorRenglon)].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                hoja.Cells[string.Format("A{0}", contadorRenglon)].Style.Fill.BackgroundColor.SetColor(colorAzul);
                                hoja.Cells[string.Format("A{0}", contadorRenglon)].Style.Border.Top.Style = ExcelBorderStyle.Medium;
                                hoja.Cells[string.Format("A{0}", contadorRenglon)].Style.Border.Left.Style = ExcelBorderStyle.Medium;
                                hoja.Cells[string.Format("A{0}", contadorRenglon)].Style.Border.Right.Style = ExcelBorderStyle.Medium;
                                hoja.Cells[string.Format("A{0}", contadorRenglon)].Style.Border.Bottom.Style = ExcelBorderStyle.Medium;

                                hoja.Cells[string.Format("B{0}", contadorRenglon)].Value = registroMantenimiento.fechaPM.ToShortDateString();
                                hoja.Cells[string.Format("B{0}", contadorRenglon)].Style.Font.Bold = true;
                                hoja.Cells[string.Format("B{0}", contadorRenglon)].Style.Font.Size = 12;
                                hoja.Cells[string.Format("B{0}", contadorRenglon)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                                hoja.Cells[string.Format("B{0}", contadorRenglon)].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                hoja.Cells[string.Format("B{0}", contadorRenglon)].Style.Border.Top.Style = ExcelBorderStyle.Medium;
                                hoja.Cells[string.Format("B{0}", contadorRenglon)].Style.Border.Left.Style = ExcelBorderStyle.Medium;
                                hoja.Cells[string.Format("B{0}", contadorRenglon)].Style.Border.Right.Style = ExcelBorderStyle.Medium;
                                hoja.Cells[string.Format("B{0}", contadorRenglon)].Style.Border.Bottom.Style = ExcelBorderStyle.Medium;

                                hoja.Cells[string.Format("C{0}:{1}{0}", contadorRenglon, ultimaColumna)].Value = string.Join(", ", mantenimiento.lista.Where(x => !string.IsNullOrEmpty(x.man_maq_detProy.detProy.Observaciones)).Select(x => x.man_maq_detProy.detProy.Observaciones).ToList());
                                hoja.Cells[string.Format("C{0}:{1}{0}", contadorRenglon, ultimaColumna)].Style.Font.Bold = true;
                                hoja.Cells[string.Format("C{0}:{1}{0}", contadorRenglon, ultimaColumna)].Style.Font.Size = 12;
                                hoja.Cells[string.Format("C{0}:{1}{0}", contadorRenglon, ultimaColumna)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                hoja.Cells[string.Format("C{0}:{1}{0}", contadorRenglon, ultimaColumna)].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                hoja.Cells[string.Format("C{0}:{1}{0}", contadorRenglon, ultimaColumna)].Merge = true;
                                hoja.Cells[string.Format("C{0}:{1}{0}", contadorRenglon, ultimaColumna)].Style.Border.Top.Style = ExcelBorderStyle.Medium;
                                hoja.Cells[string.Format("C{0}:{1}{0}", contadorRenglon, ultimaColumna)].Style.Border.Left.Style = ExcelBorderStyle.Medium;
                                hoja.Cells[string.Format("C{0}:{1}{0}", contadorRenglon, ultimaColumna)].Style.Border.Right.Style = ExcelBorderStyle.Medium;
                                hoja.Cells[string.Format("C{0}:{1}{0}", contadorRenglon, ultimaColumna)].Style.Border.Bottom.Style = ExcelBorderStyle.Medium;

                                contadorRenglon++;
                                #endregion
                            }

                            var contadorRenglonComponente = contadorRenglon;
                            var contadorColumnaComponente = 1;

                            foreach (var componente in mantenimiento.lista.OrderBy(x => x.man_maq_detProy.detProy.idComp).ToList())
                            {
                                var componenteColumna1 = ExcelUtilities.GetExcelColumnName(contadorColumnaComponente);
                                var componenteColumna2 = ExcelUtilities.GetExcelColumnName(++contadorColumnaComponente);
                                var nombreComponente = listaComponentes.FirstOrDefault(x => x.id == componente.man_maq_detProy.detProy.idComp).Descripcion;

                                #region Renglones Componentes
                                hoja.Cells[string.Format("{1}{0}:{2}{0}", contadorRenglonComponente, componenteColumna1, componenteColumna2)].Value = nombreComponente;
                                hoja.Cells[string.Format("{1}{0}:{2}{0}", contadorRenglonComponente, componenteColumna1, componenteColumna2)].Style.Font.Bold = true;
                                hoja.Cells[string.Format("{1}{0}:{2}{0}", contadorRenglonComponente, componenteColumna1, componenteColumna2)].Style.Font.Size = 14;
                                hoja.Cells[string.Format("{1}{0}:{2}{0}", contadorRenglonComponente, componenteColumna1, componenteColumna2)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                hoja.Cells[string.Format("{1}{0}:{2}{0}", contadorRenglonComponente, componenteColumna1, componenteColumna2)].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                hoja.Cells[string.Format("{1}{0}:{2}{0}", contadorRenglonComponente, componenteColumna1, componenteColumna2)].Merge = true;
                                hoja.Cells[string.Format("{1}{0}:{2}{0}", contadorRenglonComponente, componenteColumna1, componenteColumna2)].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                hoja.Cells[string.Format("{1}{0}:{2}{0}", contadorRenglonComponente, componenteColumna1, componenteColumna2)].Style.Fill.BackgroundColor.SetColor(colorAmarillo);
                                hoja.Cells[string.Format("{1}{0}:{2}{0}", contadorRenglonComponente, componenteColumna1, componenteColumna2)].Style.Border.Top.Style = ExcelBorderStyle.Medium;
                                hoja.Cells[string.Format("{1}{0}:{2}{0}", contadorRenglonComponente, componenteColumna1, componenteColumna2)].Style.Border.Left.Style = ExcelBorderStyle.Medium;
                                hoja.Cells[string.Format("{1}{0}:{2}{0}", contadorRenglonComponente, componenteColumna1, componenteColumna2)].Style.Border.Right.Style = ExcelBorderStyle.Medium;
                                hoja.Cells[string.Format("{1}{0}:{2}{0}", contadorRenglonComponente, componenteColumna1, componenteColumna2)].Style.Border.Bottom.Style = ExcelBorderStyle.Medium;

                                contadorRenglonComponente++;

                                hoja.Cells[string.Format("{1}{0}", contadorRenglonComponente, componenteColumna1)].Value = "Cambio Aceite:";
                                hoja.Cells[string.Format("{1}{0}", contadorRenglonComponente, componenteColumna1)].Style.Font.Size = 11;
                                hoja.Cells[string.Format("{1}{0}", contadorRenglonComponente, componenteColumna1)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                                hoja.Cells[string.Format("{1}{0}", contadorRenglonComponente, componenteColumna1)].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                hoja.Cells[string.Format("{1}{0}", contadorRenglonComponente, componenteColumna1)].Style.Border.Left.Style = ExcelBorderStyle.Medium;

                                #region Formato Íconos
                                var formatoIconos = hoja.ConditionalFormatting.AddThreeIconSet(
                                    new ExcelAddress(hoja.Cells[string.Format("{1}{0}", contadorRenglonComponente, componenteColumna2)].Address), //Rango de celdas
                                    eExcelconditionalFormatting3IconsSetType.Symbols2 //Set de íconos
                                );

                                formatoIconos.ShowValue = false;

                                //El Icon1 no se setea en este caso, como en Excel.
                                formatoIconos.Icon2.Type = eExcelConditionalFormattingValueObjectType.Num;
                                formatoIconos.Icon2.Value = 0.5;
                                formatoIconos.Icon3.Type = eExcelConditionalFormattingValueObjectType.Num;
                                formatoIconos.Icon3.Value = 1;
                                #endregion

                                hoja.Cells[string.Format("{1}{0}", contadorRenglonComponente, componenteColumna2)].Value = Convert.ToInt16(componente.man_maq_detProy.detProy.aplicado);
                                hoja.Cells[string.Format("{1}{0}", contadorRenglonComponente, componenteColumna2)].Style.Font.Size = 11;
                                hoja.Cells[string.Format("{1}{0}", contadorRenglonComponente, componenteColumna2)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                hoja.Cells[string.Format("{1}{0}", contadorRenglonComponente, componenteColumna2)].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                hoja.Cells[string.Format("{1}{0}", contadorRenglonComponente, componenteColumna2)].Style.Border.Right.Style = ExcelBorderStyle.Medium;

                                contadorRenglonComponente++;

                                hoja.Cells[string.Format("{1}{0}", contadorRenglonComponente, componenteColumna1)].Value = "Tipo de Aceite:";
                                hoja.Cells[string.Format("{1}{0}", contadorRenglonComponente, componenteColumna1)].Style.Font.Size = 11;
                                hoja.Cells[string.Format("{1}{0}", contadorRenglonComponente, componenteColumna1)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                                hoja.Cells[string.Format("{1}{0}", contadorRenglonComponente, componenteColumna1)].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                hoja.Cells[string.Format("{1}{0}", contadorRenglonComponente, componenteColumna1)].Style.Border.Left.Style = ExcelBorderStyle.Medium;

                                hoja.Cells[string.Format("{1}{0}", contadorRenglonComponente, componenteColumna2)].Value = listaSuministros.FirstOrDefault(x => x.id == componente.man_maq_detProy.detProy.idMisc).nomeclatura;
                                hoja.Cells[string.Format("{1}{0}", contadorRenglonComponente, componenteColumna2)].Style.Font.Size = 11;
                                hoja.Cells[string.Format("{1}{0}", contadorRenglonComponente, componenteColumna2)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                hoja.Cells[string.Format("{1}{0}", contadorRenglonComponente, componenteColumna2)].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                hoja.Cells[string.Format("{1}{0}", contadorRenglonComponente, componenteColumna2)].Style.Border.Right.Style = ExcelBorderStyle.Medium;

                                contadorRenglonComponente++;

                                hoja.Cells[string.Format("{1}{0}", contadorRenglonComponente, componenteColumna1)].Value = "Edad de Aceite:";
                                hoja.Cells[string.Format("{1}{0}", contadorRenglonComponente, componenteColumna1)].Style.Font.Size = 11;
                                hoja.Cells[string.Format("{1}{0}", contadorRenglonComponente, componenteColumna1)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                                hoja.Cells[string.Format("{1}{0}", contadorRenglonComponente, componenteColumna1)].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                hoja.Cells[string.Format("{1}{0}", contadorRenglonComponente, componenteColumna1)].Style.Font.Bold = true;
                                hoja.Cells[string.Format("{1}{0}", contadorRenglonComponente, componenteColumna1)].Style.Border.Left.Style = ExcelBorderStyle.Medium;

                                hoja.Cells[string.Format("{1}{0}", contadorRenglonComponente, componenteColumna2)].Value = (int)componente.det.vidaActual;
                                hoja.Cells[string.Format("{1}{0}", contadorRenglonComponente, componenteColumna2)].Style.Font.Size = 11;
                                hoja.Cells[string.Format("{1}{0}", contadorRenglonComponente, componenteColumna2)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                hoja.Cells[string.Format("{1}{0}", contadorRenglonComponente, componenteColumna2)].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                hoja.Cells[string.Format("{1}{0}", contadorRenglonComponente, componenteColumna2)].Style.Font.Bold = true;
                                hoja.Cells[string.Format("{1}{0}", contadorRenglonComponente, componenteColumna2)].Style.Font.Color.SetColor(Color.FromArgb(255, 0, 176, 80));
                                hoja.Cells[string.Format("{1}{0}", contadorRenglonComponente, componenteColumna2)].Style.Border.Right.Style = ExcelBorderStyle.Medium;

                                contadorRenglonComponente++;

                                hoja.Cells[string.Format("{1}{0}:{2}{0}", contadorRenglonComponente, componenteColumna1, componenteColumna2)].Value = string.Format(@"CADA {0} HRS", ((int)componente.det.Vigencia).ToString());
                                hoja.Cells[string.Format("{1}{0}:{2}{0}", contadorRenglonComponente, componenteColumna1, componenteColumna2)].Style.Font.Size = 11;
                                hoja.Cells[string.Format("{1}{0}:{2}{0}", contadorRenglonComponente, componenteColumna1, componenteColumna2)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                hoja.Cells[string.Format("{1}{0}:{2}{0}", contadorRenglonComponente, componenteColumna1, componenteColumna2)].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                hoja.Cells[string.Format("{1}{0}:{2}{0}", contadorRenglonComponente, componenteColumna1, componenteColumna2)].Merge = true;
                                hoja.Cells[string.Format("{1}{0}:{2}{0}", contadorRenglonComponente, componenteColumna1, componenteColumna2)].Style.Border.Top.Style = ExcelBorderStyle.Medium;
                                hoja.Cells[string.Format("{1}{0}:{2}{0}", contadorRenglonComponente, componenteColumna1, componenteColumna2)].Style.Border.Left.Style = ExcelBorderStyle.Medium;
                                hoja.Cells[string.Format("{1}{0}:{2}{0}", contadorRenglonComponente, componenteColumna1, componenteColumna2)].Style.Border.Right.Style = ExcelBorderStyle.Medium;
                                hoja.Cells[string.Format("{1}{0}:{2}{0}", contadorRenglonComponente, componenteColumna1, componenteColumna2)].Style.Border.Bottom.Style = ExcelBorderStyle.Medium;
                                #endregion

                                contadorColumnaComponente++;
                                contadorRenglonComponente = contadorRenglon;
                            }

                            contadorRenglon += 5;
                        }

                        hoja.Cells[hoja.Dimension.Address].AutoFitColumns();
                        hoja.View.ZoomScale = 70;
                    }

                    excel.Compression = OfficeOpenXml.CompressionLevel.BestSpeed;

                    var bytes = new MemoryStream();

                    using (var exportData = new MemoryStream())
                    {
                        excel.SaveAs(exportData);
                        bytes = exportData;
                    }

                    return bytes;
                }
            }
            catch (Exception e)
            {
                LogError(0, 0, "MatenimientoPMController", "DescargarExcelJOMAGALI", e, AccionEnum.DESCARGAR, 0, new { areaCuenta = areaCuenta, fechaInicio = fechaInicio, fechaFin = fechaFin, economico = economico });
                return null;
            }
        }

        public Image ResizeImage(Image imagen, int ancho, int alto)
        {
            Image imagenNueva = null;

            using (var imgAntes = imagen)
            {
                Bitmap myBitmap;
                ImageCodecInfo myImageCodecInfo;
                System.Drawing.Imaging.Encoder myEncoder;
                EncoderParameter myEncoderParameter;
                EncoderParameters myEncoderParameters;

                myBitmap = new Bitmap(imgAntes, ancho, alto);
                myImageCodecInfo = GetEncoderInfo("image/jpeg");
                myEncoder = System.Drawing.Imaging.Encoder.Quality;
                myEncoderParameters = new EncoderParameters(1);
                myEncoderParameter = new EncoderParameter(myEncoder, 45L);
                myEncoderParameters.Param[0] = myEncoderParameter;

                using (MemoryStream memStream = new MemoryStream())
                {
                    myBitmap.Save(memStream, myImageCodecInfo, myEncoderParameters);
                    Image newImage = Image.FromStream(memStream);
                    ImageAttributes imageAttributes = new ImageAttributes();
                    using (Graphics g = Graphics.FromImage(newImage))
                    {
                        g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                        g.DrawImage(newImage, new Rectangle(Point.Empty, newImage.Size), 0, 0, newImage.Width, newImage.Height, GraphicsUnit.Pixel, imageAttributes);
                    }

                    imagenNueva = newImage;
                }
            }

            return imagenNueva;
        }

        private static ImageCodecInfo GetEncoderInfo(String mimeType)
        {
            int j;
            ImageCodecInfo[] encoders;
            encoders = ImageCodecInfo.GetImageEncoders();
            for (j = 0; j < encoders.Length; ++j)
            {
                if (encoders[j].MimeType == mimeType)
                    return encoders[j];
            }
            return null;
        }
    }
}
