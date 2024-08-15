using Data.Factory.Maquinaria.Catalogos;
using Data.Factory.Maquinaria.Inventario;
using Data.Factory.Principal.Alertas;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SIGOPLAN.Controllers.Maquinaria.Reportes
{
    public class RepGanttMaquinariaController : BaseController
    {
        private AsignacionEquiposFactoryServices AsignacionEquiposServices = new AsignacionEquiposFactoryServices();
        private AlertaMantenimientoFactoryServices alertaMantenimientoFactoryServices = new AlertaMantenimientoFactoryServices();
        private CentroCostosFactoryServices centroCostosFactoryServices = new CentroCostosFactoryServices();
        private MaquinaFactoryServices maquinaFactoryServices = new MaquinaFactoryServices();
        public struct stGanttTemp
        {
            public int id { get; set; }
            public string titulo { get; set; }
            public string fechaInicio { get; set; }
            public string fechaFin { get; set; }
            public double avance { get; set; }
            public string centrocosto { get; set; }
            public int grupoID { get; set; }
            public string grupoDes { get; set; }
        }
        public struct stGantt
        {
            public int id { get; set; }
            public string text { get; set; }
            public string inicio { get; set; }
            public double avance { get; set; }
            public string fin { get; set; }
            public string start_date { get; set; }
            public int parent { get; set; }
            public int grupoID { get; set; }
            public double progress { get; set; }
            public string cc { get; set; }
            public bool open { get; set; }
            public string color { get; set; }
            public string textColor { get; set; }
            public string progressColor { get; set; }
        }
        // GET: RepGanttMaquinaria
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult getMaquinas(List<string> ccs, List<int> grupos)
        {
            grupos = grupos ?? new List<int>();
            var data = AsignacionEquiposServices.getAsignacionEquiposFactoryServices().getAsignacionActivas(ccs);
            var dataTemp = data.Where(x => x.fechaFin > DateTime.Now);
            var result = new List<stGanttTemp>();
            var listGantt = new List<stGantt>();
            foreach (var x in dataTemp)
            {
                try
                {
                    var temp = new stGanttTemp
                    {
                        id = 0,
                        titulo = x.cc + " - " + x.Economico,
                        fechaInicio = x.fechaInicio.ToString("dd/MM/yyyy"),
                        fechaFin = x.fechaFin.ToString("dd/MM/yyyy"),
                        avance = getAvance(x.fechaInicio, x.fechaFin, DateTime.Now),
                        centrocosto = x.cc,
                        grupoID = maquinaFactoryServices.getMaquinaServices().GetMaquinaByNoEconomico(x.Economico).grupoMaquinariaID,
                        grupoDes = maquinaFactoryServices.getMaquinaServices().GetMaquinaByNoEconomico(x.Economico).grupoMaquinaria.descripcion
                    };
                    if (grupos.Count > 0 ? grupos.Contains(temp.grupoID) : true)
                        result.Add(temp);
                }
                catch (Exception ex) { }
            }


            if (result.Count > 0)
            {
                int count = 1;
                foreach (var i in result.Select(x => x.centrocosto).OrderBy(x => x).Distinct())
                {
                    count++;
                    var objcc = new stGantt();
                    objcc.id = count;
                    objcc.text = i.ToString();
                    objcc.inicio = result.Where(y => y.centrocosto == i).OrderBy(y => y.fechaInicio).FirstOrDefault().fechaInicio;
                    objcc.fin = result.Where(y => y.centrocosto == i).OrderByDescending(y => y.fechaFin).FirstOrDefault().fechaFin;
                    objcc.avance = getAvance(DateTime.Parse(result.Where(y => y.centrocosto == i).OrderBy(y => y.fechaInicio).FirstOrDefault().fechaInicio, new CultureInfo("es-MX", true)),
                            DateTime.Parse(result.Where(y => y.centrocosto == i).OrderByDescending(y => y.fechaFin).FirstOrDefault().fechaFin, new CultureInfo("es-MX", true)),
                            DateTime.Now);
                    objcc.start_date = result.Where(y => y.centrocosto == i).OrderBy(y => y.fechaInicio).FirstOrDefault().fechaInicio;
                    objcc.parent = 0;
                    objcc.grupoID = 0;
                    objcc.progress = objcc.avance;
                    objcc.cc = i;
                    objcc.open = ccs == null || ccs.Count > 1 ? false : true;
                    objcc.color = "#EEEEEE";
                    objcc.textColor = "black";
                    objcc.progressColor = "#5cb85c";
                    listGantt.Add(objcc);

                    foreach (var j in result.Where(x => x.centrocosto == i).OrderBy(x => x.centrocosto).Select(x => x.grupoID).Distinct())
                    {
                        count++;
                        var objg = new stGantt();
                        objg.id = count;
                        objg.text = result.FirstOrDefault(x => x.centrocosto == i && x.grupoID == j).grupoDes;
                        objg.inicio = result.Where(y => y.centrocosto == i && y.grupoID == j).OrderBy(y => y.fechaInicio).FirstOrDefault().fechaInicio;
                        objg.fin = result.Where(y => y.centrocosto == i && y.grupoID == j).OrderByDescending(y => y.fechaFin).FirstOrDefault().fechaFin;
                        objg.avance = getAvance(DateTime.Parse(result.Where(y => y.centrocosto == i && y.grupoID == j).OrderBy(y => y.fechaInicio).FirstOrDefault().fechaInicio, new CultureInfo("es-MX", true)),
                                DateTime.Parse(result.Where(y => y.centrocosto == i && y.grupoID == j).OrderByDescending(y => y.fechaFin).FirstOrDefault().fechaFin, new CultureInfo("es-MX", true)),
                                DateTime.Now);
                        objg.start_date = result.Where(y => y.centrocosto == i && y.grupoID == j).OrderBy(y => y.fechaInicio).FirstOrDefault().fechaInicio;
                        objg.parent = objcc.id;
                        objg.grupoID = objcc.id;
                        objg.progress = objg.avance;
                        objg.cc = j.ToString();
                        objg.open = false;
                        objg.color = "#EEEEEE";
                        objg.textColor = "black";
                        objg.progressColor = "#5cb85c";
                        listGantt.Add(objg);

                        foreach (var z in result.Where(x => x.centrocosto == i && x.grupoID == j).OrderBy(x => x.titulo))
                        {
                            count++;
                            var obje = new stGantt();
                            obje.id = count;
                            obje.text = z.titulo;
                            obje.inicio = z.fechaInicio;
                            obje.fin = z.fechaFin;
                            obje.avance = z.avance;
                            obje.start_date = z.fechaInicio;
                            obje.parent = objg.id;
                            obje.grupoID = objg.id;
                            obje.progress = z.avance;
                            obje.cc = z.centrocosto;
                            obje.open = false;
                            obje.color = "#EEEEEE";
                            obje.textColor = "black";
                            obje.progressColor = "#5cb85c";
                            listGantt.Add(obje);
                        }
                    }
                }
                //var aux = result.Select(x=> new {id = x.grupoID, descripcion= x.grupoDes}).Distinct();

                //var resultGrupo= aux.Select(x => new stGanttTemp
                //{
                //    id = x.id,
                //    titulo = x.descripcion,
                //    fechaInicio = result.Where(y => y.grupoID == x.id).OrderBy(y => y.fechaInicio).FirstOrDefault().fechaInicio,
                //    fechaFin = result.Where(y => y.grupoID == x.id).OrderByDescending(y => y.fechaFin).FirstOrDefault().fechaFin,
                //    avance = getAvance(DateTime.Parse(result.Where(y => y.grupoID == x.id).OrderBy(y => y.fechaInicio).FirstOrDefault().fechaInicio, new CultureInfo("es-MX", true)),
                //            DateTime.Parse(result.Where(y => y.grupoID == x.id).OrderByDescending(y => y.fechaFin).FirstOrDefault().fechaFin, new CultureInfo("es-MX", true)),
                //            alertaMantenimientoFactoryServices.getAlertaMantenimientoService().getHoraServer()),
                //    centrocosto = 0,
                //    grupoID = 0,
                //    grupoDes = ""
                //}).ToList();

                //foreach (var parent in resultGrupo)
                //{
                //    result.Add(parent);
                //}
            }


            return Json(listGantt, JsonRequestBehavior.AllowGet);
        }

        private double getAvance(DateTime inicio, DateTime fin, DateTime actual)
        {
            int fInicio = inicio.Year * 10000 + inicio.Month * 100 + inicio.Day;
            int fFin = fin.Year * 10000 + fin.Month * 100 + fin.Day;
            int fActual = DateTime.Now.Year * 10000 + DateTime.Now.Month * 100 + DateTime.Now.Day;

            int x = fFin - fInicio;
            int y = fActual - fInicio;

            if (fActual < fFin)
            {
                double avance = (y * 100 / x) * .01;
                return avance <= 0 ? 0.0 : avance;
            }

            else
            {
                return 1;
            }

        }
    }
}