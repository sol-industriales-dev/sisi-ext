using Core.DAO.Encuestas;
using Core.DTO.Captura;
using Core.DTO.Encuestas.Proveedores.Reportes;
using Core.DTO.Encuestas.SubContratista;
using Core.DTO.Utils.Data;
using Core.Entity.Encuestas;
using Core.Enum.Encuesta;
using Core.Enum.Multiempresa;
using Data.EntityFramework.Context;
using Data.EntityFramework.Generic;
using Infrastructure.DTO;
using Infrastructure.Utils;
using OfficeOpenXml;
using OfficeOpenXml.Table;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Data.Odbc;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;


namespace Data.DAO.Encuestas
{
    public class EncuestasSubContratistasDAO : GenericDAO<tblEN_ResultadoSubContratistas>, IEncuestasSubContratistasDAO
    {
        public int saveEncuesta(tblEN_EncuestaSubContratista obj, List<tblEN_PreguntasSubContratistas> listObj)
        {
            try
            {

                IObjectSet<tblEN_EncuestaSubContratista> _objectSet = ((IObjectContextAdapter)_context).ObjectContext.CreateObjectSet<tblEN_EncuestaSubContratista>();

                if (obj == null) { throw new ArgumentNullException("Entity"); }
                _objectSet.AddObject(obj);
                _context.SaveChanges();
                int encuestasID = obj.id;

                foreach (var item in listObj)
                {
                    item.encuestaID = encuestasID;
                    if (item.id == 0)
                    {
                        saveNewEncuesta(item);
                    }
                }
                _context.tblEN_PreguntasSubContratistas.AddRange(listObj);
                _context.SaveChanges();
                return encuestasID;
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public List<tblEN_EncuestaSubContratista> fillCboEncuestas(int tipoEncuesta)
        {
            var result = _context.tblEN_EncuestaSubContratista.Where(x => x.tipoEncuesta == tipoEncuesta && x.estatus == true).ToList();
            return result;
        }

        public void saveNewEncuesta(tblEN_PreguntasSubContratistas obj)
        {
            IObjectSet<tblEN_PreguntasSubContratistas> _objectSet = ((IObjectContextAdapter)_context).ObjectContext.CreateObjectSet<tblEN_PreguntasSubContratistas>();

            if (obj == null) { throw new ArgumentNullException("Entity"); }
            _objectSet.AddObject(obj);
            _context.SaveChanges();
        }

        public void saveEncuestaResult(List<tblEN_ResultadoSubContratistas> obj, tblEN_ResultadoSubContratistasDet objSingle, string comentario)
        {
            //objSingle.fec = DateTime.Now;
            IObjectSet<tblEN_ResultadoSubContratistasDet> _objectSet = ((IObjectContextAdapter)_context).ObjectContext.CreateObjectSet<tblEN_ResultadoSubContratistasDet>();

            var estrellas = _context.tblEN_Estrellas.ToList();
            obj.ForEach(e =>
            {
                e.porcentaje = estrellas.FirstOrDefault(x => x.estrellas == e.calificacion) != null ? estrellas.FirstOrDefault(x => x.estrellas == e.calificacion).maximo : 0;
            });

            if (objSingle == null) { throw new ArgumentNullException("Entity"); }

            objSingle.calificacion = (decimal)Math.Truncate(100 * (double)(obj.Where(y => y.porcentaje != null).Select(x => x.porcentaje).Sum() / obj.Count).Value) / 100;

            _objectSet.AddObject(objSingle);
            _context.SaveChanges();
            foreach (var item in obj)
            {
                item.encuestaFolioID = objSingle.id;
            }

            _context.tblEN_ResultadoSubContratistas.AddRange(obj);
            _context.SaveChanges();

        }

        public int saveEncuestaUpdate(tblEN_EncuestaSubContratista obj, List<tblEN_PreguntasSubContratistas> listObj)
        {
            try
            {
                IObjectSet<tblEN_EncuestaSubContratista> _objectSet = ((IObjectContextAdapter)_context).ObjectContext.CreateObjectSet<tblEN_EncuestaSubContratista>();


                tblEN_EncuestaSubContratista existe = _context.tblEN_EncuestaSubContratista.Find(obj.id);

                if (existe != null)
                {
                    existe.descripcion = obj.descripcion;
                    existe.estatus = existe.estatus;
                    existe.tipoEncuesta = obj.tipoEncuesta;
                    existe.titulo = obj.titulo;
                    _context.SaveChanges();
                }
                int encuestasID = obj.id;
                foreach (var item in listObj)
                {

                    if (item.id != 0)
                    {
                        tblEN_PreguntasSubContratistas Pregunta = _context.tblEN_PreguntasSubContratistas.Find(item.id);
                        Pregunta.pregunta = item.pregunta;
                        Pregunta.estatus = item.estatus;
                        Pregunta.tipo = item.tipo;
                    }
                    else
                    {
                        item.encuestaID = encuestasID;
                        _context.tblEN_PreguntasSubContratistas.Add(item);
                    }
                }
                _context.SaveChanges();
                return encuestasID;
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public tblEN_EncuestaSubContratista getEncuestaID(int encuestaID)
        {
            var resultados = _context.tblEN_EncuestaSubContratista.FirstOrDefault(x => x.id == encuestaID && x.estatus == true);

            return resultados;
        }

        public List<tblEN_PreguntasSubContratistas> getListaPreguntasByIDEncuesta(int encuestaID)
        {

            var resultados = _context.tblEN_PreguntasSubContratistas.Where(x => x.encuestaID == encuestaID).ToList();
            return resultados;
        }

        public List<RespuestasEncuestasDTO> GetEncuestaContestada(int idEncuestaDEtalle)
        {
            var estrellas = _context.tblEN_Estrellas.ToList();

            var Respuestas = _context.tblEN_ResultadoSubContratistas.Where(x => x.encuestaFolioID == idEncuestaDEtalle).ToList();

            List<RespuestasEncuestasDTO> lstObjRespuestasDTO = new List<RespuestasEncuestasDTO>();

            foreach (var item in Respuestas)
            {
                RespuestasEncuestasDTO ObjRespuestasDTO = new RespuestasEncuestasDTO();

                ObjRespuestasDTO.Pregunta = item.pregunta.pregunta;
                ObjRespuestasDTO.tipoPregunta = item.pregunta.tipo;
                ObjRespuestasDTO.Calificacion = item.calificacion;
                ObjRespuestasDTO.DescripcionTipo = EnumExtensions.GetDescription((TiposPreguntasEnum)item.pregunta.tipo);
                ObjRespuestasDTO.fecha = item.fecha;
                ObjRespuestasDTO.CalificacionDescripcion = estrellas.FirstOrDefault(x => x.estrellas == item.calificacion) != null ? estrellas.FirstOrDefault(x => x.estrellas == item.calificacion).descripcion : "";
                lstObjRespuestasDTO.Add(ObjRespuestasDTO);
            }

            return lstObjRespuestasDTO;
        }

        public Dictionary<string, object> getExcel(List<dataSubContratistaDTO> lstSubContratistas) //TODO
        {
            var result = new Dictionary<string, object>();

            #region CONSULTA PARA LA HOJA 2
            DateTime fechaActual = DateTime.Now;
            var odbc = new OdbcConsultaDTO()
            {
                consulta = string.Format(@"SELECT  A.numPro AS numProveedor, A.DSContratista AS nombreSubContratista, 
                                                   B.obra AS centroCostos, D.descripcion AS centroCostosNombre, 
                                                   C.observaciones AS nombreProyecto, B.fecha_inicio_contrato AS fechaInicio, 
                                                   B.fecha_termino_contrato AS fechaFin, B.id_convenio  as convenio
                                FROM {0}su_contratistas A 
                                INNER JOIN {0}su_contrato_contratista B ON A.numpro = B.numpro
                                INNER JOIN {0}SU_CONVENIO_CONTRATISTAS C ON C.numpro = B.numpro AND B.obra = C.obra AND B.id_convenio = C.id_convenio
                                INNER JOIN {0}CC D ON D.CC = C.obra
                                    WHERE fecha_termino_contrato >= ?", "dba."),
                parametros = new List<OdbcParameterDTO>() 
                    {
                        new OdbcParameterDTO() { nombre = "fecha_termino_contrato", tipo = OdbcType.Date, valor = fechaActual },
                    }
            };
            var res2 = _contextEnkontrol.Select<dataSubContratistaDTO>(EnkontrolAmbienteEnum.Prod, odbc);
            #endregion

            #region SE CREA EL ARCHIVO EXCEL
            using (ExcelPackage excel = new ExcelPackage())
            {
                var excelDetalles = excel.Workbook.Worksheets.Add("Evaluaciones");
                var header = new List<string> { "CC", "Fecha de inicio", "Fecha de termino", "Servicio", "Subcontratista", 
                                                    "Evaluador", "Fecha evaluación", "Calificación" };

                var excelDetalles2 = excel.Workbook.Worksheets.Add("Activos");
                var header2 = new List<string> { "Núm. Proveedor", "SubContratista", "CC", "CC Nombre", "Nombre Proyecto", 
                                                    "Fecha inicio", "Fecha fin", "Convenio" };

                for (int i = 1; i <= header.Count; i++)
                {
                    excelDetalles.Cells[1, i].Value = header[i - 1];
                    excelDetalles2.Cells[1, i].Value = header2[i - 1];
                }

                var cellData = new List<object[]>();
                foreach (var item in lstSubContratistas)
                {
                    string fechaEvaluacion = string.Empty;
                    if (item.fechaEvaluacion != null)
                    {
                        for (int i = 0; i < 1; i++)
                        {
                            fechaEvaluacion = item.fechaEvaluacion[i];
                        }
                    }

                    cellData.Add(new object[]{
                        item.centroCostos,
                        item.fechaInicio.ToShortDateString(),
                        item.fechaFin.ToShortDateString(),
                        item.nombreProyecto,
                        item.nombreSubContratista,
                        item.evaluador,
                        fechaEvaluacion,
                        item.calificacion
                    });
                }

                var cellData2 = new List<object[]>();
                foreach (var item in res2)
                {
                    cellData2.Add(new object[]{
                        item.numProveedor,
                        item.nombreSubContratista,
                        item.centroCostos,
                        item.centroCostosNombre,
                        item.nombreProyecto,
                        item.fechaInicio.ToShortDateString(),
                        item.fechaFin.ToShortDateString(),
                        item.convenio
                    });
                }

                excelDetalles.Cells[2, 1].LoadFromArrays(cellData);
                excelDetalles2.Cells[2, 1].LoadFromArrays(cellData2);

                ExcelRange range = excelDetalles.Cells[1, 1, excelDetalles.Dimension.End.Row, excelDetalles.Dimension.End.Column];
                ExcelRange range2 = excelDetalles2.Cells[1, 1, excelDetalles2.Dimension.End.Row, excelDetalles2.Dimension.End.Column];

                ExcelTable tab = excelDetalles.Tables.Add(range, "Consulta1");
                ExcelTable tab2 = excelDetalles2.Tables.Add(range2, "Consulta2");

                excelDetalles.Cells[1, 2, excelDetalles.Dimension.End.Row, 2].Style.Numberformat.Format = "dd-mm-yyyy";
                excelDetalles.Cells[1, 3, excelDetalles.Dimension.End.Row, 3].Style.Numberformat.Format = "dd-mm-yyyy";
                excelDetalles.Cells[1, 7, excelDetalles.Dimension.End.Row, 7].Style.Numberformat.Format = "dd-mm-yyyy";

                excelDetalles2.Cells[1, 6, excelDetalles2.Dimension.End.Row, 6].Style.Numberformat.Format = "dd-mm-yyyy";
                excelDetalles2.Cells[1, 7, excelDetalles2.Dimension.End.Row, 7].Style.Numberformat.Format = "dd-mm-yyyy";

                int cont = 0;
                foreach (var item in excelDetalles.Cells["B" + excelDetalles.Dimension.Start.Row + ":" + "B" + excelDetalles.Dimension.End.Row])
                {
                    if (cont > 0 && item.Value != null)
                    {
                        item.Formula = "=DATEVALUE(\"" + Convert.ToDateTime(item.Value).ToString("dd/MM/yyyy") + "\")";
                    }
                    cont++;
                }

                cont = 0;
                foreach (var item in excelDetalles.Cells["C" + excelDetalles.Dimension.Start.Row + ":" + "C" + excelDetalles.Dimension.End.Row])
                {
                    if (cont > 0 && item.Value != null)
                    {
                        item.Formula = "=DATEVALUE(\"" + Convert.ToDateTime(item.Value).ToString("dd/MM/yyyy") + "\")";
                    }
                    cont++;
                }

                cont = 0;
                foreach (var item in excelDetalles.Cells["G" + excelDetalles.Dimension.Start.Row + ":" + "G" + excelDetalles.Dimension.End.Row])
                {
                    if (cont > 0 && item.Value != null)
                    {
                        item.Formula = "=DATEVALUE(\"" + Convert.ToDateTime(item.Value).ToString("dd/MM/yyyy") + "\")";
                    }
                    cont++;
                }

                cont = 0;
                foreach (var item in excelDetalles2.Cells["F" + excelDetalles2.Dimension.Start.Row + ":" + "F" + excelDetalles2.Dimension.End.Row])
                {
                    if (cont > 0 && item.Value != null)
                    {
                        item.Formula = "=DATEVALUE(\"" + Convert.ToDateTime(item.Value).ToString("dd/MM/yyyy") + "\")";
                    }
                    cont++;
                }

                cont = 0;
                foreach (var item in excelDetalles2.Cells["G" + excelDetalles2.Dimension.Start.Row + ":" + "G" + excelDetalles2.Dimension.End.Row])
                {
                    if (cont > 0 && item.Value != null)
                    {
                        item.Formula = "=DATEVALUE(\"" + Convert.ToDateTime(item.Value).ToString("dd/MM/yyyy") + "\")";
                    }
                    cont++;
                }

                excelDetalles.Cells[excelDetalles.Dimension.Address].AutoFitColumns();
                excelDetalles2.Cells[excelDetalles2.Dimension.Address].AutoFitColumns();

                var bytes = new MemoryStream();
                using (var stream = new MemoryStream())
                {
                    excel.SaveAs(stream);
                    bytes = stream;
                }

                result.Add(SUCCESS, true);
                result.Add(ITEMS, bytes);
            }
            #endregion

            return result;
        }

        public List<dataSubContratistaDTO> getListaSubContratistas(DateTime fechaInicio, DateTime fechaFin, int encuestaID)
        {
            List<dataSubContratistaDTO> res = new List<dataSubContratistaDTO>();
            try
            {
                var datosEnKontrol = getLstContrato(fechaInicio, fechaFin);
                var usuarios = _context.tblP_Usuario.ToList();
                if (datosEnKontrol.Count > 0)
                {
                    var listaContratosNoVisibles = new List<dataSubContratistaDTO>();

                    datosEnKontrol.ForEach(enKontrol =>
                    {
                        var nombreProyecto = string.IsNullOrEmpty(enKontrol.nombreProyecto) ? "" : enKontrol.nombreProyecto.Replace("\n", "");
                        nombreProyecto = nombreProyecto.Replace("\r", "");

                        var encuestaExistente = _context.tblEN_ResultadoSubContratistasDet.Where
                            (sigoplan =>
                                sigoplan.numSubContratista == enKontrol.numProveedor &&
                                sigoplan.centroCostos.Equals(enKontrol.centroCostos) &&
                                sigoplan.descripcionServicio.Equals(nombreProyecto)
                            ).ToList();

                        if (encuestaExistente.Count() > 0)
                        {
                            if (encuestaID > 0)
                                encuestaExistente = encuestaExistente.Where(w => w.encuestaID == encuestaID).ToList();
                        }

                        if (encuestaExistente.Count > 0)
                        {
                            #region
                            var diasDuracionContrato = DatetimeUtils.DiasDiferencia(enKontrol.fechaInicio, enKontrol.fechaFin);

                            if (diasDuracionContrato < 270 || (encuestaExistente.First().descripcionServicio != null && (
                                    encuestaExistente.First().descripcionServicio.ToUpper().Contains("ACARREO") ||
                                    encuestaExistente.First().descripcionServicio.ToUpper().Contains("PIPA") ||
                                    encuestaExistente.First().descripcionServicio.ToUpper().Contains("CALIDAD") ||
                                    encuestaExistente.First().descripcionServicio.ToUpper().Contains("LABORATORIO") ||
                                    encuestaExistente.First().descripcionServicio.ToUpper().Contains("TOPOGRAFIA"))))
                            {
                                #region
                                //EL CONTRATO DEBE SER MENOR A 270 DÍAS Y SE DEBE VERIFICAR QUE HAYAN TRANSCURRIDO 30 DÍAS DESDE SU FECHA DE INICIO.
                                if (diasDuracionContrato < 270 && diasDuracionContrato > 90 && DateTime.Now > enKontrol.fechaFin && encuestaExistente.Count == 1 && enKontrol.fechaInicio >= new DateTime(2020, 11, 25))
                                    enKontrol.estatus = true;
                                else
                                    enKontrol.estatus = false;

                                //enKontrol.id.Add(encuestaExistente.First().id);
                                //enKontrol.evaluador.Add(encuestaExistente.First().usuario.nombre + " " + encuestaExistente.First().usuario.apellidoPaterno + " " + encuestaExistente.First().usuario.apellidoMaterno);
                                //enKontrol.fechaEvaluacion.Add(encuestaExistente.First().detalles.First().fecha.ToShortDateString());
                                #endregion
                            }
                            else
                            {
                                #region
                                //VALIDACION DE CONTRATOS DE MAYOR O IGUAL A 3 MESES Y MENOR O IGUAL A 9 MESES
                                if (diasDuracionContrato > 90 && diasDuracionContrato < 270)
                                {
                                    if (enKontrol.fechaFin > DateTime.Now && encuestaExistente.Count == 1 && enKontrol.fechaInicio >= new DateTime(2020, 11, 25))
                                    {
                                        enKontrol.estatus = true;
                                    }
                                    else
                                    {
                                        enKontrol.estatus = false;
                                    }
                                }
                                else
                                {
                                    var evaluacionesRealizadas = encuestaExistente.Count;
                                    var evaluacionesMaximas = Decimal.ToInt32(((diasDuracionContrato / 30) / 6));
                                    var sobrante = ((diasDuracionContrato / 30M) / 6M) - Math.Truncate((diasDuracionContrato / 30M) / 6M);

                                    if (evaluacionesRealizadas <= evaluacionesMaximas)
                                    {
                                        var diaComienzoNuevaEvaluacion = 0;

                                        if (
                                            (evaluacionesRealizadas + 1 == evaluacionesMaximas && sobrante < 0.5M) ||
                                            (evaluacionesRealizadas == evaluacionesMaximas && sobrante > 0.5M)
                                           )
                                        {
                                            diaComienzoNuevaEvaluacion = diasDuracionContrato;
                                        }
                                        if (
                                            (evaluacionesRealizadas + 1 == evaluacionesMaximas && sobrante >= 0.5M) ||
                                            (evaluacionesRealizadas + 1 < evaluacionesMaximas)
                                           )
                                        {
                                            diaComienzoNuevaEvaluacion = (evaluacionesRealizadas + 1) * 180;
                                        }

                                        var diasActuales = DatetimeUtils.DiasDiferencia(enKontrol.fechaInicio, DateTime.Now);

                                        if (diasActuales >= diaComienzoNuevaEvaluacion)
                                        {
                                            enKontrol.estatus = true;
                                        }
                                        else
                                        {
                                            enKontrol.estatus = false;
                                        }
                                    }
                                    else
                                    {
                                        enKontrol.estatus = false;
                                    }
                                }
                                #endregion
                            }

                            if (encuestaExistente.Count >= 1 && (enKontrol.fechaInicio < new DateTime(2020, 01, 01)))
                            {
                                enKontrol.estatus = false;
                                var temp = encuestaExistente.Last();
                                encuestaExistente.RemoveRange(0, encuestaExistente.Count);
                                encuestaExistente.Add(temp);
                            }

                            foreach (var item in encuestaExistente)
                            {
                                enKontrol.id.Add(item.id);
                                enKontrol.evaluador.Add(item.usuario.nombre + " " + item.usuario.apellidoPaterno + " " + item.usuario.apellidoMaterno);
                                enKontrol.fechaEvaluacion.Add(item.detalles.First().fecha.ToShortDateString());
                                enKontrol.calificacion = item.calificacion;
                            }
                            #region COMENTADO
                            //var mesesDuracionContrato = DatetimeUtils.MesesDiferencia(enKontrol.fechaFin, enKontrol.fechaInicio);

                            //if (mesesDuracionContrato < 8 || encuestaExistente.Count > 1)
                            //{
                            //    enKontrol.estatus = false;
                            //}
                            //else
                            //{
                            //    enKontrol.estatus = true;

                            //    if (mesesDuracionContrato >= 8 && encuestaExistente.Count >= 1 && (enKontrol.fechaInicio < new DateTime(2020, 01, 01) || enKontrol.fechaFin <= new DateTime(2020, 10, 31)))
                            //    {
                            //        enKontrol.estatus = false;
                            //    }
                            //}

                            //if (encuestaExistente.Count > 1 && (enKontrol.fechaInicio < new DateTime(2020, 01, 01) || enKontrol.fechaFin <= new DateTime(2020, 10, 31)))
                            //{
                            //    var temp = encuestaExistente.Last();
                            //    encuestaExistente.RemoveRange(0, encuestaExistente.Count);
                            //    encuestaExistente.Add(temp);
                            //}

                            //foreach (var item in encuestaExistente)
                            //{
                            //    enKontrol.id.Add(item.id);
                            //    enKontrol.evaluador.Add(item.usuario.nombre + " " + item.usuario.apellidoPaterno + " " + item.usuario.apellidoMaterno);
                            //    enKontrol.fechaEvaluacion.Add(_context.tblEN_ResultadoSubContratistas.First(x => x.encuestaFolioID == item.id).fecha.ToShortDateString());
                            //}
                            #endregion
                            #endregion
                        }
                        else
                        {
                            #region FUNCIONAL PERO COMENTADO
                            var diasDuracionContrato = DatetimeUtils.DiasDiferencia(enKontrol.fechaInicio, enKontrol.fechaFin);
                            var diasDuracionActual = DatetimeUtils.DiasDiferencia(enKontrol.fechaInicio, DateTime.Now);
                            TimeSpan tSpan = DateTime.Now - enKontrol.fechaInicio;
                            int diasTranscurridos = tSpan.Days;

                            if (
                                (
                                    diasDuracionContrato >= 270 &&
                                    diasDuracionActual >= 181 &&
                                    !string.IsNullOrEmpty(enKontrol.nombreProyecto) &&
                                    !enKontrol.nombreProyecto.ToUpper().Contains("ACARREO") &&
                                    !enKontrol.nombreProyecto.ToUpper().Contains("PIPA") &&
                                    !enKontrol.nombreProyecto.ToUpper().Contains("CALIDAD") &&
                                    !enKontrol.nombreProyecto.ToUpper().Contains("LABORATORIO") &&
                                    !enKontrol.nombreProyecto.ToUpper().Contains("TOPOGRAFIA") &&
                                    enKontrol.fechaInicio >= new DateTime(2020, 01, 01)
                                ) ||
                                (
                                    diasDuracionContrato < 270 && diasTranscurridos > 30 && diasDuracionContrato > 90 && enKontrol.fechaFin > new DateTime(2020, 11, 25)
                                ) ||
                                (
                                    (
                                        DateTime.Now > enKontrol.fechaFin
                                    )
                                )
                               )
                            {
                                enKontrol.estatus = true;
                                enKontrol.id.Add(0);
                                enKontrol.evaluador.Add("");
                                enKontrol.fechaEvaluacion.Add("");
                                enKontrol.calificacion = null;
                            }
                            else
                            {
                                listaContratosNoVisibles.Add(enKontrol);
                            }
                            #endregion

                            #region COMENTADO
                            //enKontrol.estatus = true;
                            //enKontrol.id.Add(0);
                            //enKontrol.evaluador.Add("");
                            //enKontrol.fechaEvaluacion.Add("");
                            #endregion
                        }

                        //var evaluador = encuestaExistente.Count() > 0 ? null : usuarios.FirstOrDefault(x => x.id == encuestaExistente.evaluador);
                        ////var encuesta = encuestaExistente == null ? null : _context.tblEN_EncuestaSubContratista.FirstOrDefault(x => x.id == encuestaExistente.encuestaID);
                        //var fechaEncuesta = encuestaExistente == null || evaluador == null ? "" : _context.tblEN_ResultadoSubContratistas.Where(x => x.encuestaFolioID == encuestaExistente.id && x.usuarioRespondioID == evaluador.id).OrderBy(x => x.fecha).FirstOrDefault().fecha.ToString("dd/MM/yyyy");

                        //enKontrol.estatus = encuestaExistente == null;
                        //enKontrol.id = encuestaExistente != null ? encuestaExistente.id : 0;
                        //enKontrol.evaluador = evaluador == null ? "" : evaluador.nombre + " " + evaluador.apellidoPaterno + " " + evaluador.apellidoMaterno;
                        //enKontrol.fechaEvaluacion = fechaEncuesta;
                    });
                    res.AddRange(datosEnKontrol);
                    foreach (var item in listaContratosNoVisibles)
                    {
                        res.Remove(item);
                    }
                }

                return res;
            }
            catch
            {
                return new List<dataSubContratistaDTO>();
            }
        }

        List<dataSubContratistaDTO> getLstContrato(DateTime fechaInicio, DateTime fechaFin)
        {
            var odbc = new OdbcConsultaDTO()
            {
                consulta = string.Format(@"SELECT  A.numPro AS numProveedor, A.DSContratista AS nombreSubContratista, B.obra AS centroCostos, D.descripcion AS centroCostosNombre, C.observaciones AS nombreProyecto, B.fecha_inicio_contrato AS fechaInicio, B.fecha_termino_contrato AS fechaFin, B.id_convenio  as convenio
                                FROM {0}su_contratistas A 
                                INNER JOIN {0}su_contrato_contratista B ON A.numpro = B.numpro
                                INNER JOIN {0}SU_CONVENIO_CONTRATISTAS C ON C.numpro = B.numpro AND B.obra = C.obra AND B.id_convenio = C.id_convenio
                                INNER JOIN {0}CC D ON D.CC = C.obra
                                    WHERE fecha_termino_contrato BETWEEN ? AND ?", "dba."),
                parametros = new List<OdbcParameterDTO>() 
                    {
                        new OdbcParameterDTO() { nombre = "fecha_termino_contrato", tipo = OdbcType.Date, valor = fechaInicio },
                        new OdbcParameterDTO() { nombre = "fecha_termino_contrato", tipo = OdbcType.Date, valor = fechaFin }
                    }
            };
            return _contextEnkontrol.Select<dataSubContratistaDTO>(EnkontrolAmbienteEnum.Prod, odbc);
        }

        public dataSubContratistaDTO getInfoSubContratista(string cc, int numPro, int convenio)
        {
            string Consulta = @"SELECT A.numPro AS numProveedor, A.DSContratista AS nombreSubContratista, B.obra AS centroCostos, D.descripcion AS centroCostosNombre,C.observaciones AS nombreProyecto
                                FROM su_contratistas A 
                                INNER JOIN su_contrato_contratista B
                                ON A.numpro = B.numpro
                                INNER JOIN SU_CONVENIO_CONTRATISTAS C
                                ON  C.numpro = B.numpro AND B.obra = C.obra AND B.id_convenio = C.id_convenio
                                INNER JOIN DBA.CC D 
                                ON D.CC = C.obra
                                WHERE C.obra = '" + cc + "' and C.numpro = '" + numPro + "' AND " + " C.id_convenio = " + convenio;

            try
            {
                var rawDataEnkontrol = (List<dataSubContratistaDTO>)_contextEnkontrol.Where(Consulta).ToObject<List<dataSubContratistaDTO>>();

                return rawDataEnkontrol.FirstOrDefault();
            }
            catch
            {

                return new dataSubContratistaDTO();
            }
        }

        public tblEN_ResultadoSubContratistasDet GetDetalleEncuesta(int id)
        {
            return _context.tblEN_ResultadoSubContratistasDet.FirstOrDefault(x => x.id == id);
        }

        public List<subContratistasDTO> getNombreSubContratistas(string term)
        {
            string Consulta = @"SELECT A.numPro AS numSubContratista, A.DSContratista AS nombreSubContratista 
                                FROM su_contratistas A where A.DSContratista like '%" + term + "%'";

            var rawDataEnkontrol = (List<subContratistasDTO>)_contextEnkontrol.Where(Consulta).ToObject<List<subContratistasDTO>>();

            return rawDataEnkontrol;
        }

        public List<dataSubContratistaDTO> getListaSubContratistasbySubContratista(int contratistaID)
        {
            List<dataSubContratistaDTO> res = new List<dataSubContratistaDTO>();
            string Consulta = @"SELECT D.cc,A.numPro AS numProveedor, 
                                A.DSContratista AS nombreSubContratista, B.obra AS centroCostos, D.descripcion AS centroCostosNombre,
                                C.observaciones AS nombreProyecto,B.fecha_inicio_contrato AS fechaInicio, 
                                B.fecha_termino_contrato AS fechaFin
                                FROM su_contratistas A 
                                INNER JOIN su_contrato_contratista B
                                ON A.numpro = B.numpro
                                INNER JOIN SU_CONVENIO_CONTRATISTAS C
                                ON  C.numpro = B.numpro AND B.obra = C.obra AND B.id_convenio = C.id_convenio
                                INNER JOIN DBA.CC D 
                                ON D.CC = C.obra
                                WHERE  A.numPro ='" + contratistaID + "'";
            try
            {
                var rawDataEnkontrol = (List<dataSubContratistaDTO>)_contextEnkontrol.Where(Consulta).ToObject<List<dataSubContratistaDTO>>();

                var rawCompare = from e in rawDataEnkontrol
                                 join sub in _context.tblEN_ResultadoSubContratistasDet on e.numProveedor equals sub.numSubContratista
                                 into encDet
                                 from sub in encDet.DefaultIfEmpty()
                                 select new { e, sub };

                foreach (var item in rawCompare)
                {
                    var datosEnkontrol = item.e;
                    var datosSigoplan = item.sub;

                    if (datosSigoplan != null)
                    {
                        item.e.estatus = false;
                        item.e.id.Add(datosSigoplan.id);

                    }
                    else
                    {
                        item.e.estatus = true;
                        item.e.id.Add(0);
                    }

                    res.Add(item.e);
                }
                return res.ToList();
            }
            catch
            {

                return new List<dataSubContratistaDTO>();
            }
        }

        List<tblEN_ResultadoSubContratistasDet> getLstResultadoSubcontratistaDet(DateTime fechaInicio, DateTime fechaFin, List<string> cc)
        {
            var lstContrato = getLstContrato(fechaInicio, fechaFin).Where(w => cc.Contains(w.centroCostos)).ToList();
            return _context.tblEN_ResultadoSubContratistasDet.ToList().Where(x => lstContrato.Any(a => a.numProveedor == x.numSubContratista && a.centroCostos == x.centroCostos)).ToList();
        }

        public DataTable GetMatrizEvaluacionSubContratista(DateTime fechaInicio, DateTime fechaFin, List<string> cc)
        {
            var lstResDet = getLstResultadoSubcontratistaDet(fechaInicio, fechaFin, cc);
            var ListaSubContratistas = lstResDet.GroupBy(x => new { x.numSubContratista, x.nombreSubContratista }).Select(y => y.Key).ToList();
            var listaCentroCostos = lstResDet.GroupBy(x => new { x.centroCostos, x.centroCostosNombre }).ToList();
            DataTable tableEncabezado = new DataTable();
            tableEncabezado.Columns.Add("CC", System.Type.GetType("System.String"));
            tableEncabezado.Columns.Add("Proyecto", System.Type.GetType("System.String"));
            foreach (var item in ListaSubContratistas)
            {
                tableEncabezado.Columns.Add(item.nombreSubContratista.ToString(), System.Type.GetType("System.String"));
            }
            foreach (var centroCostos in listaCentroCostos)
            {
                DataRow dr = tableEncabezado.NewRow();
                dr[0] = centroCostos.Key.centroCostos;
                string centro_costosName = "";
                try
                {
                    centro_costosName = _context.tblP_CC.FirstOrDefault(x => x.cc == centroCostos.Key.centroCostos).descripcion;
                    dr[1] = centro_costosName.TrimEnd().Replace("\"", "''");
                }
                catch (Exception)
                {
                    dr[1] = "";
                }
                int i = 2;
                foreach (var subContratistas in ListaSubContratistas)
                {
                    decimal calificacion = 0;
                    var foliosID = centroCostos.Where(x => x.numSubContratista == subContratistas.numSubContratista).ToList();
                    foreach (var foliosCC in foliosID)
                    {
                        var GetRespuestas = _context.tblEN_ResultadoSubContratistas.Where(x => x.encuestaFolioID == foliosCC.id);
                        foreach (var item in GetRespuestas)
                        {
                            if (item.calificacion != 0)
                            {
                                decimal dato = Convert.ToDecimal(item.calificacion);
                                switch (dato.ToString())
                                {
                                    case "1.00":
                                    case "2.00":
                                    case "1,00":
                                    case "2,00":
                                        calificacion += 1;
                                        break;
                                    case "3.00":
                                    case "4.00":
                                    case "3,00":
                                    case "4,00":
                                        calificacion += 2;
                                        break;
                                    case "5.00":
                                    case "5,00":
                                        calificacion += 3;
                                        break;
                                    default:
                                        break;
                                }
                            }
                        }
                    }

                    dr[i] = foliosID.Count > 0 ? Math.Floor(calificacion / foliosID.Count) : 0;
                    i++;
                }
                tableEncabezado.Rows.Add(dr);
            }
            return tableEncabezado;

        }

        public DataTable GetMatrizEvaluacionSubContratistaEstrellas(DateTime fechaInicio, DateTime fechaFin, List<string> cc)
        {
            var lstResDet = getLstResultadoSubcontratistaDet(fechaInicio, fechaFin, cc);
            var ListaSubContratistas = lstResDet.GroupBy(x => new { x.numSubContratista, x.nombreSubContratista }).Select(y => y.Key).ToList();
            var listaCentroCostos = lstResDet.GroupBy(x => new { x.centroCostos, x.centroCostosNombre }).ToList();
            DataTable tableEncabezado = new DataTable();
            tableEncabezado.Columns.Add("CC", System.Type.GetType("System.String"));
            tableEncabezado.Columns.Add("Proyecto", System.Type.GetType("System.String"));
            foreach (var item in ListaSubContratistas)
            {
                tableEncabezado.Columns.Add(item.nombreSubContratista.ToString(), System.Type.GetType("System.String"));
            }
            foreach (var centroCostos in listaCentroCostos)
            {
                DataRow dr = tableEncabezado.NewRow();
                dr[0] = centroCostos.Key.centroCostos;
                string centro_costosName = "";
                try
                {
                    centro_costosName = _context.tblP_CC.FirstOrDefault(x => x.cc == centroCostos.Key.centroCostos).descripcion;
                    dr[1] = centro_costosName.TrimEnd().Replace("\"", "''");
                }
                catch (Exception)
                {
                    dr[1] = "";
                }
                int i = 2;
                foreach (var subContratistas in ListaSubContratistas)
                {

                    decimal calificacion = 0;
                    var foliosID = centroCostos.Where(x => x.numSubContratista == subContratistas.numSubContratista).ToList();

                    foreach (var foliosCC in foliosID)
                    {
                        var GetRespuestas = _context.tblEN_ResultadoSubContratistas.Where(x => x.encuestaFolioID == foliosCC.id);
                        foreach (var item in GetRespuestas)
                        {
                            if (item.calificacion != 0)
                            {
                                calificacion = Convert.ToDecimal(item.calificacion);
                            }
                        }
                    }
                    dr[i] = foliosID.Count > 0 ? Math.Floor(calificacion / foliosID.Count) : 0;
                    i++;
                }
                tableEncabezado.Rows.Add(dr);
            }
            return tableEncabezado;
        }

        public List<GraficaEvaluacionSubContratistaCCDTO> GetGraficaEvaluacionSubContratistaCC(DateTime fechaInicio, DateTime fechaFin, List<string> cc, int subContratista)
        {
            List<GraficaEvaluacionSubContratistaCCDTO> listGraficaEvaluacionSubContratistaCCDTO = new List<GraficaEvaluacionSubContratistaCCDTO>();
            var lstResDet = getLstResultadoSubcontratistaDet(fechaInicio, fechaFin, cc);
            var ListaSubContratistas = lstResDet.GroupBy(x => new { x.numSubContratista, x.nombreSubContratista }).Select(y => y.Key).ToList();
            var listaCentroCostos = lstResDet.Where(w => w.numSubContratista == subContratista).GroupBy(x => new { x.centroCostos, x.centroCostosNombre }).Select(y => y.Key).ToList();
            foreach (var centroCostos in listaCentroCostos)
            {
                GraficaEvaluacionSubContratistaCCDTO GraficaEvaluacionSubContratistaCCDTOobj = new GraficaEvaluacionSubContratistaCCDTO();
                var getID = lstResDet.Where(x => x.numSubContratista == subContratista).ToList();
                var listaIDS = getID.Select(x => x.id);
                var GetRespuestas = _context.tblEN_ResultadoSubContratistas.Where(x => listaIDS.Contains(x.encuestaFolioID)).ToList();
                GraficaEvaluacionSubContratistaCCDTOobj.CCname = centroCostos.centroCostosNombre;
                decimal calificacion = 0;
                foreach (var item in GetRespuestas)
                {
                    if (item.calificacion != 0)
                    {
                        string dato = item.calificacion.ToString();

                        switch (item.calificacion.ToString())
                        {
                            case "1.00":
                            case "2.00":
                                calificacion += 1;
                                break;
                            case "3.00":
                            case "4.00":
                                calificacion += 2;
                                break;
                            case "5.00":
                                calificacion += 3;
                                break;
                            default:
                                break;
                        }
                    }
                }
                GraficaEvaluacionSubContratistaCCDTOobj.CC = centroCostos.centroCostos;
                GraficaEvaluacionSubContratistaCCDTOobj.CCname = centroCostos.centroCostosNombre;
                GraficaEvaluacionSubContratistaCCDTOobj.value = calificacion;
                listGraficaEvaluacionSubContratistaCCDTO.Add(GraficaEvaluacionSubContratistaCCDTOobj);
            }
            return listGraficaEvaluacionSubContratistaCCDTO;
        }

        public List<GraficaEvaluacionSubContratistaCCDTO> GetGraficaSubContratistaCCEstrellas(DateTime fechaInicio, DateTime fechaFin, List<string> cc, int subContratista)
        {
            List<GraficaEvaluacionSubContratistaCCDTO> listGraficaEvaluacionSubContratistaCCDTO = new List<GraficaEvaluacionSubContratistaCCDTO>();

            var Listacontestadas = _context.tblEN_ResultadoSubContratistas.Where(x => x.fecha >= fechaInicio && x.fecha <= fechaFin).ToList();
            var listaFolios = Listacontestadas.GroupBy(x => x.encuestaFolioID).Select(y => y.Key).ToList();
            var listaCentroCostos = _context.tblEN_ResultadoSubContratistasDet.Where(x => x.numSubContratista == subContratista && listaFolios.Contains(x.id) && cc.Contains(x.centroCostos)).GroupBy(x => new { x.centroCostos, x.centroCostosNombre }).Select(y => y.Key).ToList();

            foreach (var centroCostos in listaCentroCostos)
            {
                GraficaEvaluacionSubContratistaCCDTO GraficaEvaluacionSubContratistaCCDTOobj = new GraficaEvaluacionSubContratistaCCDTO();
                var getID = _context.tblEN_ResultadoSubContratistasDet.Where(x => listaFolios.Contains(x.id) && x.centroCostos == centroCostos.centroCostos && x.numSubContratista == subContratista).ToList();
                var listaIDS = getID.Select(x => x.id);
                var GetRespuestas = _context.tblEN_ResultadoSubContratistas.Where(x => listaIDS.Contains(x.encuestaFolioID)).ToList();

                GraficaEvaluacionSubContratistaCCDTOobj.CCname = centroCostos.centroCostosNombre;
                decimal calificacion = 0;

                foreach (var item in GetRespuestas)
                {
                    if (item.calificacion != 0)
                    {
                        string dato = item.calificacion.ToString();
                        calificacion = item.calificacion;

                        //switch (item.calificacion.ToString())
                        //{
                        //    case "1.00":
                        //    case "2.00":
                        //        calificacion += 1;
                        //        break;
                        //    case "3.00":
                        //    case "4.00":
                        //        calificacion += 2;
                        //        break;
                        //    case "5.00":
                        //        calificacion += 3;
                        //        break;
                        //    default:
                        //        break;
                        //}
                    }
                }

                GraficaEvaluacionSubContratistaCCDTOobj.CC = centroCostos.centroCostos;
                GraficaEvaluacionSubContratistaCCDTOobj.CCname = centroCostos.centroCostosNombre;
                GraficaEvaluacionSubContratistaCCDTOobj.value = calificacion;
                listGraficaEvaluacionSubContratistaCCDTO.Add(GraficaEvaluacionSubContratistaCCDTOobj);

            }


            return listGraficaEvaluacionSubContratistaCCDTO;
        }

        public List<GraficaEvaluacionSubContratistaDTO> GetGraficaEvaluacionSubContratista(DateTime fechaInicio, DateTime fechaFin, List<string> cc)
        {

            List<GraficaEvaluacionSubContratistaDTO> objList = new List<GraficaEvaluacionSubContratistaDTO>();
            var lstResDet = getLstResultadoSubcontratistaDet(fechaInicio, fechaFin, cc);
            var ListaSubContratistas = lstResDet.GroupBy(x => new { x.numSubContratista, x.nombreSubContratista }).Select(y => y.Key).ToList();
            var listaCentroCostos = lstResDet.GroupBy(x => new { x.centroCostos, x.centroCostosNombre }).Select(y => y.Key).ToList();

            List<ComboDTO> comboDTO = new List<ComboDTO>();

            int i = 2;
            foreach (var subContratistas in ListaSubContratistas)
            {


                ComboDTO objCombo = new ComboDTO();
                GraficaEvaluacionSubContratistaDTO objData = new GraficaEvaluacionSubContratistaDTO();
                var getID = lstResDet.Where(x => x.numSubContratista == subContratistas.numSubContratista).ToList();
                var listaIDS = getID.Select(x => x.id);
                var GetRespuestas = _context.tblEN_ResultadoSubContratistas.Where(x => listaIDS.Contains(x.encuestaFolioID)).ToList();
                decimal calificacion = 0;
                foreach (var item in GetRespuestas)
                {
                    if (item.calificacion != 0)
                    {
                        if (item.calificacion == 5)
                        {
                            calificacion += 3;
                        }
                        else if (item.calificacion >= 3 && item.calificacion <= 4)
                        {
                            calificacion += 2;
                        }
                        else if (item.calificacion >= 0 && item.calificacion <= 2)
                        {
                            calificacion += 1;
                        }
                    }
                }
                objCombo.Text = subContratistas.nombreSubContratista;
                objCombo.Value = subContratistas.numSubContratista;

                objData.value = calificacion / getID.Count();
                objData.nombreContratista = subContratistas.nombreSubContratista;

                comboDTO.Add(objCombo);
                objList.Add(objData);
            }

            HttpContext.Current.Session["cboSubContratistas"] = comboDTO;
            return objList;
        }

        public List<GraficaEvaluacionSubContratistaDTO> GetGraficaEvaluacionSubContratistaEstrellas(DateTime fechaInicio, DateTime fechaFin, List<string> cc)
        {

            List<GraficaEvaluacionSubContratistaDTO> objList = new List<GraficaEvaluacionSubContratistaDTO>();
            var Listacontestadas = _context.tblEN_ResultadoSubContratistas.Where(x => x.fecha >= fechaInicio && x.fecha <= fechaFin).ToList();
            var listaFolios = Listacontestadas.GroupBy(x => x.encuestaFolioID).Select(y => y.Key).ToList();
            var ListaSubContratistas = _context.tblEN_ResultadoSubContratistasDet.Where(x => listaFolios.Contains(x.id) && cc.Contains(x.centroCostos)).GroupBy(x => new { x.numSubContratista, x.nombreSubContratista }).Select(y => y.Key).ToList();
            var listaCentroCostos = _context.tblEN_ResultadoSubContratistasDet.Where(x => listaFolios.Contains(x.id) && cc.Contains(x.centroCostos)).GroupBy(x => new { x.centroCostos, x.centroCostosNombre }).ToList();

            List<ComboDTO> comboDTO = new List<ComboDTO>();

            int i = 2;
            foreach (var subContratistas in ListaSubContratistas)
            {


                ComboDTO objCombo = new ComboDTO();
                GraficaEvaluacionSubContratistaDTO objData = new GraficaEvaluacionSubContratistaDTO();
                var getID = _context.tblEN_ResultadoSubContratistasDet.Where(x => listaFolios.Contains(x.id) && cc.Contains(x.centroCostos) && x.numSubContratista == subContratistas.numSubContratista).ToList();
                var listaIDS = getID.Select(x => x.id);
                var GetRespuestas = _context.tblEN_ResultadoSubContratistas.Where(x => listaIDS.Contains(x.encuestaFolioID)).ToList();
                decimal calificacion = 0;

                foreach (var item in GetRespuestas)
                {
                    if (item.calificacion != 0)
                    {
                        //if (item.calificacion == 5)
                        //{
                        //    calificacion += 3;
                        //}
                        //else if (item.calificacion >= 3 && item.calificacion <= 4)
                        //{
                        //    calificacion += 2;
                        //}
                        //else if (item.calificacion >= 0 && item.calificacion <= 2)
                        //{
                        //    calificacion += 1;
                        //}

                        calificacion = Convert.ToDecimal(item.calificacion);
                    }
                }

                objCombo.Text = subContratistas.nombreSubContratista;
                objCombo.Value = subContratistas.numSubContratista;

                objData.value = calificacion / getID.Count();
                objData.nombreContratista = subContratistas.nombreSubContratista;

                comboDTO.Add(objCombo);
                objList.Add(objData);
            }

            HttpContext.Current.Session["cboSubContratistas"] = comboDTO;
            return objList;
        }
    }
}