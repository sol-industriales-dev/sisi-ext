using Core.DAO.Maquinaria.Captura.HorasHombre;
using Core.DTO;
using Core.DTO.Captura;
using Core.DTO.Maquinaria.Captura.OT;
using Core.DTO.Maquinaria.Captura.OT.rptConcentradoHH;
using Core.DTO.Principal.Generales;
using Core.DTO.RecursosHumanos;
using Core.Entity.Maquinaria.Captura;
using Core.Entity.Principal.Multiempresa;
using Core.Enum.Principal.Bitacoras;
using Data.EntityFramework.Context;
using Data.EntityFramework.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Data.Entity.Migrations;
using Core.Enum.Multiempresa;
using Core.DTO.Utils.Data;
using Core.Enum.Principal;
using Core.DAO.Enkontrol.General.CC;
using Data.Factory.Enkontrol.General.CC;

namespace Data.DAO.Maquinaria.Captura
{
    public class CapHorasHombreDAO : GenericDAO<tblM_CapHorasHombre>, ICapHorasHombreDAO
    {
        #region Filtros.
        public List<tblM_CatCategoriasHH> getCategorias()
        {
            return _context.tblM_CatCategoriasHH.ToList();
        }

        public List<DistribucionHHPersonalDTO> rptGeneralDistribucion(List<string> cc, DateTime fechaInicio, DateTime fechaFin, List<int> puestos, List<int> empleados)
        {
            List<DistribucionHHPersonalDTO> result = new List<DistribucionHHPersonalDTO>();

            if (puestos == null)
            {
                puestos = new List<int>();
            }
            if (empleados == null)
            {
                empleados = new List<int>();
            }

            //var objRaw = _context.tblM_CapHorasHombre.ToList().Where(x => cc.Contains(x.centroCostos)
            //    && (fechaInicio >= x.fechaCaptura && x.fechaCaptura <= fechaFin) &&
            //    puestos != null ? puestos.Contains(x.puestoID) : x.id == x.id && empleados != null ? empleados.Contains(x.numEmpleado) : x.id == x.id).ToList();
            var objRaw = _context.tblM_CapHorasHombre
                .Where(x =>
                    cc.Contains(x.centroCostos) &&
                    fechaInicio >= x.fechaCaptura &&
                    x.fechaCaptura <= fechaFin &&
                    (puestos.Count > 0 ? puestos.Contains(x.puestoID) : true) &&
                    (empleados.Count > 0 ? empleados.Contains(x.numEmpleado) : true)
               ).ToList();

            ////var objRaw = _context.tblM_CapHorasHombre.Where(x => (cc.Contains(x.centroCostos))
            ////    && (fechaInicio >= x.fechaCaptura && x.fechaCaptura <= fechaFin) &&
            ////    (puestos != null ? puestos.Contains(x.puestoID) : true) && (empleados != null ? empleados.Contains(x.numEmpleado) : true)).ToList();

            //var objRaw = _context.tblM_CapHorasHombre.Where(x => (cc.Contains(x.centroCostos))
            //    && (fechaInicio >= x.fechaCaptura && x.fechaCaptura <= fechaFin));

            //var obj1 = objRaw.ToList();
            //if (puestos != null)
            //{

            //    objRaw = objRaw.Where(e => puestos.Contains(e.puestoID));
            //    var obj2 = objRaw.ToList();


            //}

            //if (empleados != null)
            //{
            //    objRaw = objRaw.Where(e => empleados.Contains(e.numEmpleado));
            //    var obj3 = objRaw.ToList();


            //}

            //var objCooked = objRaw.ToList();
            //    //(puestos != null ? puestos.Contains(x.puestoID) : true) && (empleados != null ? empleados.Contains(x.numEmpleado) : true)).ToList();

            //string Consutal = "SELECT p.puesto,p.descripcion,e.clave_empleado AS personalID, s.salario_base AS salarioBase, s.complemento AS salarioComplemento, ";
            //Consutal += "e.nombre,ape_paterno,e.ape_materno ";
            //Consutal += "FROM DBA.sn_empleados AS e ";
            //Consutal += "INNER JOIN DBA.si_puestos as p on e.puesto=p.puesto ";
            //Consutal += "INNER JOIN DBA.sn_tabulador_historial AS s ON e.clave_empleado=s.clave_empleado ";
            //Consutal += "INNER JOIN (SELECT  MAX(id) AS id ,clave_empleado FROM DBA.sn_tabulador_historial GROUP BY (clave_empleado)) AS F ON F.id = s.id ";
            //Consutal += "WHERE p.puesto IN (" + GetListaPuestos(puestos.Select(x => x.ToString()).ToList()) + ") AND estatus_empleado = 'A' AND cc_contable IN (" + GetListaPuestos(cc.ToList()) + ")";



            List<PuestosDTO> lstPuestosEnkontrol = new List<PuestosDTO>();
            try
            {
                //var lstPuestosEnkontrol1 = (List<PuestosDTO>)ContextEnKontrolNominaArrendadora.Where(Consutal, 1).ToObject<List<PuestosDTO>>();

                var lstPuestosEnkontrol1 = _context.Select<PuestosDTO>(new DapperDTO
                {
                    baseDatos = MainContextEnum.Construplan,
                    consulta = @"SELECT p.puesto,p.descripcion,e.clave_empleado AS personalID, s.salario_base AS salarioBase, s.complemento AS salarioComplemento, 
                                    e.nombre,ape_paterno,e.ape_materno 
                                FROM tblRH_EK_Empleados AS e 
                                INNER JOIN tblRH_EK_Puestos as p on e.puesto=p.puesto 
                                INNER JOIN tblRH_EK_Tabulador_Historial AS s ON e.clave_empleado=s.clave_empleado 
                                INNER JOIN (SELECT  MAX(id) AS id ,clave_empleado FROM tblRH_EK_Tabulador_Historial GROUP BY (clave_empleado)) AS F ON F.id = s.id 
                                WHERE p.puesto IN (" + GetListaPuestos(puestos.Select(x => x.ToString()).ToList()) + ") AND estatus_empleado = 'A' AND cc_contable IN (" + GetListaPuestos(cc.ToList()) + ")",
                });
                lstPuestosEnkontrol.AddRange(lstPuestosEnkontrol1);
            }
            catch (Exception)
            {


            }

            try
            {
                //var lstPuestosEnkontrol2 = (List<PuestosDTO>)ContextEnKontrolNominaArrendadora.Where(Consutal, 2).ToObject<List<PuestosDTO>>();

                var lstPuestosEnkontrol2 = _context.Select<PuestosDTO>(new DapperDTO
                {
                    baseDatos = MainContextEnum.Arrendadora,
                    consulta = @"SELECT p.puesto,p.descripcion,e.clave_empleado AS personalID, s.salario_base AS salarioBase, s.complemento AS salarioComplemento, 
                                    e.nombre,ape_paterno,e.ape_materno 
                                FROM tblRH_EK_Empleados AS e 
                                INNER JOIN tblRH_EK_Puestos as p on e.puesto=p.puesto 
                                INNER JOIN tblRH_EK_Tabulador_Historial AS s ON e.clave_empleado=s.clave_empleado 
                                INNER JOIN (SELECT  MAX(id) AS id ,clave_empleado FROM tblRH_EK_Tabulador_Historial GROUP BY (clave_empleado)) AS F ON F.id = s.id 
                                WHERE p.puesto IN (" + GetListaPuestos(puestos.Select(x => x.ToString()).ToList()) + ") AND estatus_empleado = 'A' AND cc_contable IN (" + GetListaPuestos(cc.ToList()) + ")",
                });

                lstPuestosEnkontrol.AddRange(lstPuestosEnkontrol2);
            }
            catch (Exception)
            {


            }

            var listaEmpleadosEnkontrol = lstPuestosEnkontrol.GroupBy(x => x.personalID).Where(x => empleados != null ? empleados.Contains(x.Key) : x.Key == x.Key).Select(x => x.Key).ToList();



            List<string> ccRaw = new List<string>();


            var ac = _context.tblP_CC.Where(x => cc.Contains(x.cc)).ToList();
            var ac2 = _context.tblP_CCRH.Where(x => cc.Contains(x.cc)).ToList();

            foreach (var item in ac)
            {
                ccRaw.Add(item.areaCuenta);
            }

            foreach (var item in ac2)
            {
                ccRaw.Add(item.areaCuenta);
            }



            var rawOT = (from dot in _context.tblM_DetOrdenTrabajo
                         where ccRaw.Distinct().Contains(dot.OrdenTrabajo.CC)
                         select dot).ToList().Where(x => listaEmpleadosEnkontrol.Contains(x.PersonalID) && x.HoraInicio >= fechaInicio && x.HoraFin <= fechaFin).ToList();


            var listaEmpleados = objRaw.GroupBy(x => new { x.numEmpleado }).Where(x => empleados != null ? empleados.Contains(x.Key.numEmpleado) : x.Key.numEmpleado == x.Key.numEmpleado).Select(x => x.Key.numEmpleado).ToList();

            listaEmpleados.AddRange(listaEmpleadosEnkontrol);


            foreach (var item in listaEmpleados.Distinct()) //objRaw.GroupBy(x => new { x.numEmpleado }).Select(x => x.Key.numEmpleado).ToList())
            {
                DistribucionHHPersonalDTO obj = new DistribucionHHPersonalDTO();
                var HorasOT = rawOT.Where(x => x.PersonalID == item);
                decimal TotalHorasOT = 0;

                foreach (var ot in HorasOT)
                {
                    var data = (decimal)(ot.HoraFin - ot.HoraInicio).TotalHours;
                    TotalHorasOT += data;
                }

                var rawData = _context.tblM_CapHorasHombre.Where(x => x.numEmpleado == item).ToList()
                    ;
                var objEmpleado = lstPuestosEnkontrol.FirstOrDefault(x => x.personalID == item);

                List<tblM_CapHorasHombre> RawData = new List<tblM_CapHorasHombre>();

                foreach (var items2 in rawData)
                {

                    DateTime fechaCompra = items2.fechaCaptura.Date;

                    if (fechaCompra >= fechaInicio.Date && fechaCompra <= fechaFin.Date)
                    {
                        RawData.Add(items2);
                    }

                }

                if (objEmpleado != null)
                {
                    obj.numEmpleado = objEmpleado.personalID;
                    obj.nombreEmpleado = objEmpleado.nombre + " " + objEmpleado.ape_paterno + " " + objEmpleado.ape_materno;
                    obj.puestoID = objEmpleado.puesto;
                    obj.puesto = objEmpleado.descripcion;


                    obj.trabajoMaquinariaOT = Math.Round(TotalHorasOT, 2);
                    obj.trabajosInstalaciones = Math.Round(RawData.Where(x => x.categoriaTrabajo == 2).Sum(x => x.tiempo), 2);
                    obj.limpieza = Math.Round(RawData.Where(x => x.categoriaTrabajo == 3).Sum(x => x.tiempo), 2);
                    obj.consultaInformacion = Math.Round(RawData.Where(x => x.categoriaTrabajo == 4).Sum(x => x.tiempo), 2);
                    obj.tiempoDescanso = Math.Round(RawData.Where(x => x.categoriaTrabajo == 5).Sum(x => x.tiempo), 2);
                    obj.cursosCapacitaciones = Math.Round(RawData.Where(x => x.categoriaTrabajo == 6).Sum(x => x.tiempo), 2);
                    obj.monitoreoDiario = Math.Round(RawData.Where(x => x.categoriaTrabajo == 7).Sum(x => x.tiempo), 2);
                    obj.totalHorashombre = Math.Round(obj.trabajoMaquinariaOT + obj.trabajosInstalaciones + obj.limpieza + obj.consultaInformacion + obj.tiempoDescanso + obj.cursosCapacitaciones + obj.monitoreoDiario, 2);

                    if (obj.totalHorashombre > 0)
                    {
                        result.Add(obj);
                    }
                }
            }
            return result.ToList();
        }

        public List<DistribucionHHPersonalDTO> rptGeneralCCPorPuesto(List<string> cc, DateTime fechaInicio, DateTime fechaFin, List<int> puestos)
        {

            List<DistribucionHHPersonalDTO> result = new List<DistribucionHHPersonalDTO>();

            var cc2 = new List<string>();

            if (vSesiones.sesionEmpresaActual == 2)
            {

                cc2 = _context.tblP_CCRH.Where(x => cc.Contains(x.cc)).Select(x => x.areaCuenta).ToList();

            }

            if (cc == null)
            {
                cc = new List<string>();
            }
            if (puestos == null)
            {
                puestos = new List<int>();
            }

            //var objRaw = _context.tblM_CapHorasHombre.ToList().Where(x => cc.Contains(x.centroCostos) && x.fechaCaptura.Date >= fechaInicio.Date && x.fechaCaptura.Date <= fechaFin.Date && puestos != null ? puestos.Contains(x.puestoID) : x.id == x.id).ToList();
            var objRaw = _context.tblM_CapHorasHombre
                .Where(x =>
                    cc.Contains(x.centroCostos) &&
                    x.fechaCaptura >= fechaInicio.Date &&
                    x.fechaCaptura <= fechaFin &&
                    (puestos != null ? puestos.Contains(x.puestoID) : true)
                ).ToList();

            //string Consutal = "SELECT p.puesto,p.descripcion,e.clave_empleado AS personalID, s.salario_base AS salarioBase, s.complemento AS salarioComplemento, ";
            //Consutal += "e.nombre,ape_paterno,e.ape_materno ";
            //Consutal += "FROM DBA.sn_empleados AS e ";
            //Consutal += "INNER JOIN DBA.si_puestos as p on e.puesto=p.puesto ";
            //Consutal += "INNER JOIN DBA.sn_tabulador_historial AS s ON e.clave_empleado=s.clave_empleado ";
            //Consutal += "INNER JOIN (SELECT  MAX(id) AS id ,clave_empleado FROM DBA.sn_tabulador_historial GROUP BY (clave_empleado)) AS F ON F.id = s.id ";
            //Consutal += "WHERE p.puesto IN (" + GetListaPuestos(puestos.Select(x => x.ToString()).ToList()) + ") AND estatus_empleado = 'A' AND cc_contable IN (" + GetListaPuestos(cc.ToList()) + ")";

            //var lstPuestosEnkontrol = (List<PuestosDTO>)ContextEnKontrolNomina.Where(Consutal).ToObject<List<PuestosDTO>>();

            var lstPuestosEnkontrol = _context.Select<PuestosDTO>(new DapperDTO
            {
                baseDatos = (int)vSesiones.sesionEmpresaActual == (int)EmpresaEnum.Construplan ? MainContextEnum.Construplan : MainContextEnum.Arrendadora,
                consulta = @"SELECT p.puesto,p.descripcion,e.clave_empleado AS personalID, s.salario_base AS salarioBase, s.complemento AS salarioComplemento, 
                                    e.nombre,ape_paterno,e.ape_materno 
                                FROM tblRH_EK_Empleados AS e 
                                INNER JOIN tblRH_EK_Puestos as p on e.puesto=p.puesto 
                                INNER JOIN tblRH_EK_Tabulador_Historial AS s ON e.clave_empleado=s.clave_empleado 
                                INNER JOIN (SELECT  MAX(id) AS id ,clave_empleado FROM tblRH_EK_Tabulador_Historial GROUP BY (clave_empleado)) AS F ON F.id = s.id 
                                WHERE p.puesto IN (" + GetListaPuestos(puestos.Select(x => x.ToString()).ToList()) + ") AND estatus_empleado = 'A' AND cc_contable IN (" + GetListaPuestos(cc.ToList()) + ")",
            });

            var listaEmpleadosEnkontrol = lstPuestosEnkontrol.GroupBy(x => x.personalID).Select(x => x.Key).ToList();

            var rawOT = (from dot in _context.tblM_DetOrdenTrabajo
                         where cc2.Contains(dot.OrdenTrabajo.CC)
                         select dot).ToList().Where(x => listaEmpleadosEnkontrol.Contains(x.PersonalID) && x.HoraInicio >= fechaInicio && x.HoraFin <= fechaFin).ToList();


            var listaEmpleados = objRaw.GroupBy(x => new { x.numEmpleado }).Select(x => x.Key.numEmpleado).ToList();

            listaEmpleados.AddRange(listaEmpleadosEnkontrol);

            foreach (var centroCostos in cc)
            {
                DistribucionHHPersonalDTO obj = new DistribucionHHPersonalDTO();
                var HorasOT = rawOT.Where(x => x.OrdenTrabajo.CC == centroCostos);
                decimal TotalHorasOT = 0;

                foreach (var ot in HorasOT)
                {
                    var data = (decimal)(ot.HoraFin - ot.HoraInicio).TotalHours;
                    TotalHorasOT += data;
                }

                var RawData = objRaw.Where(x => x.centroCostos == centroCostos).ToList();

                obj.centrosCosto = centroCostos;

                obj.trabajoMaquinariaOT = Math.Round(TotalHorasOT, 2);
                obj.trabajosInstalaciones = Math.Round(RawData.Where(x => x.categoriaTrabajo == 2).Sum(x => x.tiempo), 2);
                obj.limpieza = Math.Round(RawData.Where(x => x.categoriaTrabajo == 3).Sum(x => x.tiempo), 2);
                obj.consultaInformacion = Math.Round(RawData.Where(x => x.categoriaTrabajo == 4).Sum(x => x.tiempo), 2);
                obj.tiempoDescanso = Math.Round(RawData.Where(x => x.categoriaTrabajo == 5).Sum(x => x.tiempo), 2);
                obj.cursosCapacitaciones = Math.Round(RawData.Where(x => x.categoriaTrabajo == 6).Sum(x => x.tiempo), 2);
                obj.monitoreoDiario = Math.Round(RawData.Where(x => x.categoriaTrabajo == 7).Sum(x => x.tiempo), 2);
                obj.totalHorashombre = Math.Round(obj.trabajoMaquinariaOT + obj.trabajosInstalaciones + obj.limpieza + obj.consultaInformacion + obj.tiempoDescanso + obj.cursosCapacitaciones + obj.monitoreoDiario, 2);

                if (obj.totalHorashombre > 0)
                {
                    result.Add(obj);
                }


            }
            return result.ToList();
        }

        public List<ComboDTO> getListaPersonalPuestos(List<int> puestos, List<string> ccs)
        {
            //if (vSesiones.sesionEmpresaActual == 2)
            //{
            //    // ccs = new List<string>();
            //    ccs = _context.tblP_CCRH.Where(x => ccs.Contains(x.areaCuenta)).Select(x => x.cc).ToList();
            //}

            List<string> listaCcs = new List<string>();
            List<tblP_CC> newLista = new List<tblP_CC>();

            var listaCcsAC = _context.tblP_CC.Where(x => ccs.Contains(x.areaCuenta)).ToList();
            var listaCCscc = _context.tblP_CC.Where(x => ccs.Contains(x.cc)).ToList();
            var listaCCNM = new List<string>();
            if (vSesiones.sesionEmpresaActual == 2)
            {
                listaCCNM = _context.tblP_CCRH.Where(x => ccs.Contains(x.cc)).Select(e => e.cc).ToList();

            }
            else
            {
                listaCCNM = _context.tblC_Nom_CatalogoCC.Where(e => ccs.Contains(e.cc)).Select(e => e.cc).ToList();
            }


            newLista.AddRange(listaCcsAC);
            newLista.AddRange(listaCCscc);


            foreach (var item in newLista.Distinct())
            {


                listaCcs.Add(item.cc);
            }

            foreach (var item in listaCCNM)
            {
                listaCcs.Add(item);
            }

            //string ConsutaEmpleado = "SELECT p.puesto,p.descripcion,e.clave_empleado AS personalID, s.salario_base AS salarioBase, s.complemento AS salarioComplemento, ";
            //ConsutaEmpleado += "e.nombre,ape_paterno,e.ape_materno ";
            //ConsutaEmpleado += "FROM DBA.sn_empleados AS e ";
            //ConsutaEmpleado += "INNER JOIN DBA.si_puestos as p on e.puesto=p.puesto ";
            //ConsutaEmpleado += "INNER JOIN DBA.sn_tabulador_historial AS s ON e.clave_empleado=s.clave_empleado ";
            //ConsutaEmpleado += "INNER JOIN (SELECT  MAX(id) AS id ,clave_empleado FROM DBA.sn_tabulador_historial GROUP BY (clave_empleado)) AS F ON F.id = s.id ";
            //ConsutaEmpleado += "WHERE p.puesto IN (" + GetListaPuestos(puestos.Select(x => x.ToString()).ToList()) + ") AND e.estatus_empleado = 'A' AND cc_contable IN (" + GetListaPuestos(listaCcs.Distinct().ToList()) + ")";

            List<PuestosDTO> listaEmpleados = new List<PuestosDTO>();
            try
            {

                //var listaEmpleados1 = (List<PuestosDTO>)ContextEnKontrolNominaArrendadora.Where(ConsutaEmpleado, 1).ToObject<List<PuestosDTO>>();

                var listaEmpleados1 = _context.Select<PuestosDTO>(new DapperDTO
                {
                    baseDatos = MainContextEnum.Construplan,
                    consulta = @"SELECT p.puesto,p.descripcion,e.clave_empleado AS personalID, s.salario_base AS salarioBase, s.complemento AS salarioComplemento, 
                                    e.nombre,ape_paterno,e.ape_materno 
                                FROM tblRH_EK_Empleados AS e 
                                INNER JOIN tblRH_EK_Puestos as p on e.puesto=p.puesto 
                                INNER JOIN tblRH_EK_Tabulador_Historial AS s ON e.clave_empleado=s.clave_empleado 
                                INNER JOIN (SELECT  MAX(id) AS id ,clave_empleado FROM tblRH_EK_Tabulador_Historial GROUP BY (clave_empleado)) AS F ON F.id = s.id 
                                WHERE p.puesto IN (" + GetListaPuestos(puestos.Select(x => x.ToString()).ToList()) + ") AND estatus_empleado = 'A' AND cc_contable IN (" + GetListaPuestos(listaCcs.Distinct().ToList()) + ")",
                });

                listaEmpleados.AddRange(listaEmpleados1);

            }
            catch (Exception)
            {


            }


            try
            {

                //var listaEmpleados2 = (List<PuestosDTO>)ContextEnKontrolNominaArrendadora.Where(ConsutaEmpleado, 2).ToObject<List<PuestosDTO>>();

                var listaEmpleados2 = _context.Select<PuestosDTO>(new DapperDTO
                {
                    baseDatos = MainContextEnum.Arrendadora,
                    consulta = @"SELECT p.puesto,p.descripcion,e.clave_empleado AS personalID, s.salario_base AS salarioBase, s.complemento AS salarioComplemento, 
                                    e.nombre,ape_paterno,e.ape_materno 
                                FROM tblRH_EK_Empleados AS e 
                                INNER JOIN tblRH_EK_Puestos as p on e.puesto=p.puesto 
                                INNER JOIN tblRH_EK_Tabulador_Historial AS s ON e.clave_empleado=s.clave_empleado 
                                INNER JOIN (SELECT  MAX(id) AS id ,clave_empleado FROM tblRH_EK_Tabulador_Historial GROUP BY (clave_empleado)) AS F ON F.id = s.id 
                                WHERE p.puesto IN (" + GetListaPuestos(puestos.Select(x => x.ToString()).ToList()) + ") AND estatus_empleado = 'A' AND cc_contable IN (" + GetListaPuestos(listaCcs.Distinct().ToList()) + ")",
                });

                listaEmpleados.AddRange(listaEmpleados2);

            }
            catch (Exception)
            {


            }

            var listaEmpleados3 = _context.Select<PuestosDTO>(new DapperDTO
            {
                baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
                consulta = @"SELECT p.puesto,p.descripcion,e.clave_empleado AS personalID, s.salario_base AS salarioBase, s.complemento AS salarioComplemento, 
                                    e.nombre,ape_paterno,e.ape_materno 
                                FROM tblRH_EK_Empleados AS e 
                                INNER JOIN tblRH_EK_Puestos as p on e.puesto=p.puesto 
                                INNER JOIN tblRH_EK_Tabulador_Historial AS s ON e.clave_empleado=s.clave_empleado 
                                INNER JOIN (SELECT  MAX(id) AS id ,clave_empleado FROM tblRH_EK_Tabulador_Historial GROUP BY (clave_empleado)) AS F ON F.id = s.id 
                                WHERE p.puesto IN (" + GetListaPuestos(puestos.Select(x => x.ToString()).ToList()) + ") AND estatus_empleado = 'A' AND cc_contable IN (" + GetListaPuestos(listaCcs.Distinct().ToList()) + ")",
            });

            listaEmpleados.AddRange(listaEmpleados3);

            return listaEmpleados.Select(x => new ComboDTO
            {
                Value = x.personalID.ToString(),
                Text = x.nombre + " " + x.ape_paterno + " " + x.ape_materno
            }).Distinct().OrderBy(x => x.Text).ToList();
        }

        #endregion

        public List<ComboDTO> fillCboCC()
        {
            switch ((EmpresaEnum)vSesiones.sesionEmpresaActual)
            {
                case EmpresaEnum.Peru:
                    {
                        var resultF = new List<ComboDTO>();

                        var ccs = _context.tblP_CC.Select(x => new ComboDTO
                        {
                            Value = x.cc,
                            Text = x.descripcion.Trim()
                        }).ToList();

                        resultF.AddRange(ccs);

                        resultF.Add(new ComboDTO
                        {
                            Text = "TALLER MECANICO CENTRAL",
                            Value = "1010"
                        });
                        resultF.Add(new ComboDTO
                        {
                            Text = "PATIO DE MAQUINARIA",
                            Value = "1015"
                        });
                        resultF.Add(new ComboDTO
                        {
                            Text = "TALLER OVEHAUL(VIRTUAL)",
                            Value = "1018"
                        });

                        return resultF.Distinct().ToList();
                    }
                    break;
                default:
                    {
                        List<ComboDTO> resultF = new List<ComboDTO>();
                        //string ConsultaCC = "SELECT CC AS Value , DESCRIPCION AS Text FROM CC WHERE st_ppto <> 'T'";
                        //List<ComboDTO> result = (List<ComboDTO>)ContextEnKontrolNominaArrendadora.Where(ConsultaCC, 1).ToObject<List<ComboDTO>>();
                        //List<ComboDTO> result2 = (List<ComboDTO>)ContextEnKontrolNominaArrendadora.Where(ConsultaCC, 2).ToObject<List<ComboDTO>>();
                        var query_cc = new OdbcConsultaDTO();

                        query_cc.consulta =
                            @"SELECT
                                cc AS Value,
                                descripcion AS TEXT
                            FROM
                                cc
                            WHERE st_ppto <> 'T'";

                        var ccs = _contextEnkontrol.Select<ComboDTO>(EnkontrolAmbienteEnum.ProdCPLAN, query_cc);
                        var ccsArre = _contextEnkontrol.Select<ComboDTO>(EnkontrolAmbienteEnum.ProdARREND, query_cc);

                        resultF.AddRange(ccs);
                        resultF.AddRange(ccsArre);

                        ComboDTO obj = new ComboDTO();
                        ComboDTO obj2 = new ComboDTO();



                        resultF.Add(new ComboDTO
                        {
                            Text = "TALLER MECANICO CENTRAL",
                            Value = "1010"
                        });
                        resultF.Add(new ComboDTO
                        {
                            Text = "PATIO DE MAQUINARIA",
                            Value = "1015"
                        });
                        resultF.Add(new ComboDTO
                        {
                            Text = "TALLER OVEHAUL(VIRTUAL)",
                            Value = "1018"
                        });

                        return resultF.Distinct().ToList();
                    }
                    break;
            }
        }

        public Dictionary<string, object> FillComboDepartamentos(string cc)
        {
            var resultado = new Dictionary<string, object>();

            try
            {
                using (var _contextConstruplan = new MainContext(EmpresaEnum.Construplan))
                {
                    var listaDepartamentos = _contextConstruplan.tblRH_EK_Departamentos.Where(x => x.cc == cc).ToList().Select(x => new ComboDTO
                    {
                        Value = x.clave_depto.ToString(),
                        Text = x.desc_depto
                    }).OrderBy(x => x.Text).ToList();

                    resultado.Add(ITEMS, listaDepartamentos);
                }

                resultado.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                LogError(0, 0, "HorasHombreController", "FillComboDepartamentos", e, AccionEnum.CONSULTA, 0, cc);
                resultado.Add(SUCCESS, false);
            }

            return resultado;
        }

        public PuestosDTO getinfoEmpleadoGeneral(int numEmpleado)
        {

            //string ConsutaEmpleado = "SELECT p.puesto,p.descripcion,e.clave_empleado AS personalID";//, s.salario_base AS salarioBase, s.complemento AS salarioComplemento, ";
            //ConsutaEmpleado += "e.nombre,ape_paterno,e.ape_materno ";
            //ConsutaEmpleado += "FROM DBA.sn_empleados AS e ";
            //ConsutaEmpleado += "INNER JOIN DBA.si_puestos as p on e.puesto=p.puesto ";
            ////ConsutaEmpleado += "INNER JOIN DBA.sn_tabulador_historial AS s ON e.clave_empleado=s.clave_empleado ";
            //// ConsutaEmpleado += "INNER JOIN (SELECT  MAX(id) AS id ,clave_empleado FROM DBA.sn_tabulador_historial GROUP BY (clave_empleado)) AS F ON F.id = s.id ";
            //ConsutaEmpleado += "WHERE e.clave_empleado =" + numEmpleado + "  AND estatus_empleado = 'A' ";

            //var listaEmpleados = (List<PuestosDTO>)ContextEnKontrolNomina.Where(ConsutaEmpleado).ToObject<List<PuestosDTO>>();

            var listaEmpleados = _context.Select<PuestosDTO>(new DapperDTO
            {
                baseDatos = (int)vSesiones.sesionEmpresaActual == (int)EmpresaEnum.Construplan ? MainContextEnum.Construplan : MainContextEnum.Arrendadora,
                consulta = @"SELECT p.puesto,p.descripcion,e.clave_empleado AS personalID,
                                    e.nombre,ape_paterno,e.ape_materno 
                                FROM tblRH_EK_Empleados AS e 
                                INNER JOIN tblRH_EK_Puestos as p on e.puesto=p.puesto 
                                WHERE e.clave_empleado =" + numEmpleado + "  AND estatus_empleado = 'A'",
            });

            return listaEmpleados.FirstOrDefault();
        }


        public List<CapDistribucionHHDTO> loadDataCapHorasHombre(string cc, DateTime fecha, int turno)
        {

            var lstPuestosEnkontrol = _context.tblM_CatPuestosMaquinaria.ToList();

            //string ConsutaEmpleado = "SELECT p.puesto,p.descripcion,e.clave_empleado AS personalID,";//, s.salario_base AS salarioBase, s.complemento AS salarioComplemento, ";
            //ConsutaEmpleado += "e.nombre,ape_paterno,e.ape_materno ";
            //ConsutaEmpleado += "FROM DBA.sn_empleados AS e ";
            //ConsutaEmpleado += "INNER JOIN DBA.si_puestos as p on e.puesto=p.puesto ";
            //// ConsutaEmpleado += "INNER JOIN DBA.sn_tabulador_historial AS s ON e.clave_empleado=s.clave_empleado ";
            //// ConsutaEmpleado += "INNER JOIN (SELECT  MAX(id) AS id ,clave_empleado FROM DBA.sn_tabulador_historial GROUP BY (clave_empleado)) AS F ON F.id = s.id ";
            //ConsutaEmpleado += "WHERE p.puesto IN (" + GetListaPuestos(lstPuestosEnkontrol.Select(y => y.puesto.ToString()).ToList()) + ") AND e.estatus_empleado = 'A' AND cc_contable ='" + cc + "'";

            //var listaEmpleados = (List<PuestosDTO>)ContextEnKontrolNomina.Where(ConsutaEmpleado).ToObject<List<PuestosDTO>>();

            var listaEmpleados = _context.Select<PuestosDTO>(new DapperDTO
            {
                baseDatos = MainContextEnum.Construplan,
                consulta = @"SELECT p.puesto,p.descripcion,e.clave_empleado AS personalID,
                                    e.nombre,ape_paterno,e.ape_materno 
                                FROM tblRH_EK_Empleados AS e 
                                INNER JOIN tblRH_EK_Puestos as p on e.puesto=p.puesto 
                                WHERE p.puesto IN (" + GetListaPuestos(lstPuestosEnkontrol.Select(y => y.puesto.ToString()).ToList()) + ") AND e.estatus_empleado = 'A' AND cc_contable ='" + cc + "'",
            });

            var dataDT = _context.tblM_CapHorasHombre.ToList().Where(x => x.fechaCaptura.Date == fecha.Date && x.centroCostos.Equals(cc) && x.turno == turno).ToList();
            var SubCategorias = _context.tblM_CatSubCategoriasHH.ToList();

            List<CapDistribucionHHDTO> CapHorasHombre = new List<CapDistribucionHHDTO>();

            foreach (var item in listaEmpleados)
            {
                CapDistribucionHHDTO objCaphorasHombre = new CapDistribucionHHDTO();

                var objRaw = dataDT.Where(x => x.numEmpleado == item.personalID).ToList();
                objCaphorasHombre.personalID = item.personalID;
                objCaphorasHombre.empleado = item.nombre + " " + item.ape_paterno + " " + item.ape_materno;
                objCaphorasHombre.puestoID = item.puesto;


                if (objRaw.Count > 0)
                {
                    objCaphorasHombre.valueTotalHorashombre = objRaw.Sum(x => x.tiempo);
                    foreach (var objHorasHombre in objRaw)
                    {
                        switch (objHorasHombre.categoriaTrabajo)
                        {
                            case 2:
                                {
                                    objCaphorasHombre.idcatTrabajosInstalaciones = objHorasHombre.id;
                                    objCaphorasHombre.catTrabajosInstalaciones = objHorasHombre.categoriaTrabajo;
                                    objCaphorasHombre.valueTrabajosInstalaciones = objHorasHombre.tiempo;
                                    objCaphorasHombre.subCatConsultaInformacion = objHorasHombre.subCategoria;
                                    objCaphorasHombre.descatTrabajosInstalaciones = SubCategorias.FirstOrDefault(x => x.id == objHorasHombre.subCategoria).descripcion;
                                }
                                break;
                            case 3:
                                {
                                    objCaphorasHombre.idcatLimpieza = objHorasHombre.id;
                                    objCaphorasHombre.catLimpieza = objHorasHombre.categoriaTrabajo;
                                    objCaphorasHombre.valueLimpieza = objHorasHombre.tiempo;
                                    objCaphorasHombre.subCatLimpieza = objHorasHombre.subCategoria;
                                    objCaphorasHombre.descatLimpieza = SubCategorias.FirstOrDefault(x => x.id == objHorasHombre.subCategoria).descripcion;
                                }
                                break;
                            case 4:
                                {
                                    objCaphorasHombre.idcatConsultaInformacion = objHorasHombre.id;
                                    objCaphorasHombre.catConsultaInformacion = objHorasHombre.categoriaTrabajo;
                                    objCaphorasHombre.valueConsultaInformacion = objHorasHombre.tiempo;
                                    objCaphorasHombre.subCatConsultaInformacion = objHorasHombre.subCategoria;
                                    objCaphorasHombre.descatConsultaInformacion = SubCategorias.FirstOrDefault(x => x.id == objHorasHombre.subCategoria).descripcion;
                                }
                                break;
                            case 5:
                                {
                                    objCaphorasHombre.idcatTiempoDescanso = objHorasHombre.id;
                                    objCaphorasHombre.catTiempoDescanso = objHorasHombre.categoriaTrabajo;
                                    objCaphorasHombre.valueTiempoDescanso = objHorasHombre.tiempo;
                                    objCaphorasHombre.subCatTiempoDescanso = objHorasHombre.subCategoria;
                                    objCaphorasHombre.descatTiempoDescanso = SubCategorias.FirstOrDefault(x => x.id == objHorasHombre.subCategoria).descripcion;
                                }
                                break;
                            case 6:
                                {
                                    objCaphorasHombre.idcatCursosCapacitaciones = objHorasHombre.id;
                                    objCaphorasHombre.catCursosCapacitaciones = objHorasHombre.categoriaTrabajo;
                                    objCaphorasHombre.valueCursosCapacitaciones = objHorasHombre.tiempo;
                                    objCaphorasHombre.subCatCursosCapacitaciones = objHorasHombre.subCategoria;
                                    objCaphorasHombre.descatCursosCapacitaciones = SubCategorias.FirstOrDefault(x => x.id == objHorasHombre.subCategoria).descripcion;
                                }
                                break;
                            case 7:
                                {
                                    objCaphorasHombre.idcatMonitoreoDiario = objHorasHombre.id;
                                    objCaphorasHombre.catMonitoreoDiario = objHorasHombre.categoriaTrabajo;
                                    objCaphorasHombre.valueMonitoreoDiario = objHorasHombre.tiempo;
                                    objCaphorasHombre.subCatMonitoreoDiario = objHorasHombre.subCategoria;
                                    objCaphorasHombre.descatMonitoreoDiario = SubCategorias.FirstOrDefault(x => x.id == objHorasHombre.subCategoria).descripcion;
                                }
                                break;
                            default:
                                break;
                        }
                    }
                    objCaphorasHombre.catTrabajosInstalaciones = 2;
                    objCaphorasHombre.catLimpieza = 3;
                    objCaphorasHombre.catConsultaInformacion = 4;
                    objCaphorasHombre.catTiempoDescanso = 5;
                    objCaphorasHombre.catCursosCapacitaciones = 6;
                    objCaphorasHombre.catMonitoreoDiario = 7;

                    CapHorasHombre.Add(objCaphorasHombre);
                }
                else
                {

                    objCaphorasHombre.catTrabajosInstalaciones = 2;
                    objCaphorasHombre.valueTrabajosInstalaciones = 0;
                    objCaphorasHombre.subCatConsultaInformacion = 0;
                    objCaphorasHombre.catLimpieza = 3;
                    objCaphorasHombre.valueLimpieza = 0;
                    objCaphorasHombre.subCatLimpieza = 0;
                    objCaphorasHombre.catConsultaInformacion = 4;
                    objCaphorasHombre.valueConsultaInformacion = 0;
                    objCaphorasHombre.subCatConsultaInformacion = 0;
                    objCaphorasHombre.catTiempoDescanso = 5;
                    objCaphorasHombre.valueTiempoDescanso = 0;
                    objCaphorasHombre.subCatTiempoDescanso = 0;
                    objCaphorasHombre.catCursosCapacitaciones = 6;
                    objCaphorasHombre.valueCursosCapacitaciones = 0;
                    objCaphorasHombre.subCatCursosCapacitaciones = 0;
                    objCaphorasHombre.catMonitoreoDiario = 7;
                    objCaphorasHombre.valueMonitoreoDiario = 0;
                    objCaphorasHombre.subCatMonitoreoDiario = 0;
                    CapHorasHombre.Add(objCaphorasHombre);
                }
            }




            return CapHorasHombre;
        }

        public List<newCapHorasHombreDTO> loadTblHorasHombre(string cc, int clave_depto, DateTime fecha, int turno, int usuarioActual, int puesto, int empleado)
        {
            List<PuestosDTO> rawData = new List<PuestosDTO>();
            List<PuestosDTO> listaEmpleados = new List<PuestosDTO>();
            List<newCapHorasHombreDTO> res = new List<newCapHorasHombreDTO>();

            var lstPuestosEnkontrol = _context.tblM_CatPuestosMaquinaria.Where(x => puesto != 0 ? puesto == x.puesto : x.id == x.id).ToList();

            switch ((EmpresaEnum)vSesiones.sesionEmpresaActual)
            {
                case EmpresaEnum.Peru:
                    {
                        #region PERÚ
                        try
                        {
                            var listaEmpleadosL1 = _context.Select<PuestosDTO>(new DapperDTO
                            {
                                baseDatos = MainContextEnum.PERU,
                                consulta = string.Format(@"
                                    SELECT
                                        p.puesto, p.descripcion, e.clave_empleado AS personalID, e.nombre, ape_paterno, e.ape_materno
                                    FROM tblRH_EK_Empleados AS e 
                                        INNER JOIN tblRH_EK_Puestos AS p on e.puesto = p.puesto 
                                    WHERE p.puesto IN ({0}) AND e.estatus_empleado = 'A' AND cc_contable = '{1}' {2} {3}",
                                    GetListaPuestos(lstPuestosEnkontrol.Select(y => y.puesto.ToString()).ToList()),
                                    cc,
                                    (empleado != 0 ? " AND e.clave_empleado = @empleado" : ""),
                                    (clave_depto > 0 ? " AND e.clave_depto = @clave_depto" : "")
                                ),
                                parametros = new { empleado, clave_depto }
                            });

                            // SI SELECCIONAN 020102, TAMBIEN SE BUSCA LOS EMPLEADOS DEL 020101
                            if (cc == "020102")
                            {
                                listaEmpleadosL1.AddRange(_context.Select<PuestosDTO>(new DapperDTO
                                {
                                    baseDatos = MainContextEnum.PERU,
                                    consulta = string.Format(@"
                                    SELECT
                                        p.puesto, p.descripcion, e.clave_empleado AS personalID, e.nombre, ape_paterno, e.ape_materno
                                    FROM tblRH_EK_Empleados AS e 
                                        INNER JOIN tblRH_EK_Puestos AS p on e.puesto = p.puesto 
                                    WHERE p.puesto IN ({0}) AND e.estatus_empleado = 'A' AND cc_contable = '{1}' {2} {3}",
                                        GetListaPuestos(lstPuestosEnkontrol.Select(y => y.puesto.ToString()).ToList()),
                                        "020101",
                                        (empleado != 0 ? " AND e.clave_empleado = @empleado" : ""),
                                        (clave_depto > 0 ? " AND e.clave_depto = @clave_depto" : "")
                                    ),
                                    parametros = new { empleado, clave_depto }
                                }));
                            }

                            rawData.AddRange(listaEmpleadosL1);

                            listaEmpleados.AddRange(rawData.Distinct());
                            var fechaDate = fecha.Date;
                            var dataDT = _context.tblM_CapHorasHombre.Where(x => x.fechaCaptura == fechaDate && x.centroCostos.Equals(cc) && x.turno == turno).GroupBy(g => g.numEmpleado);

                            if (dataDT.Count() == 0 && listaEmpleados.Count == 0)
                            {
                                return res;
                            }

                            if (dataDT.Count() == 0 && listaEmpleados.Count != 0)
                            {
                                foreach (var item in listaEmpleados)
                                {
                                    res.Add(setHorasHombres(item.personalID, item.nombre + " " + item.ape_paterno + " " + item.ape_materno, item.puesto));
                                }

                                return res;
                            }

                            if (dataDT.Count() != 0 && listaEmpleados.Count != 0)
                            {
                                foreach (var item in dataDT)
                                {
                                    var empleadoEnkontrol = listaEmpleados.FirstOrDefault(f => f.personalID == item.Key);

                                    if (empleadoEnkontrol != null)
                                    {
                                        listaEmpleados.Remove(empleadoEnkontrol);

                                        var objCaphorasHombre = new newCapHorasHombreDTO();
                                        objCaphorasHombre.numEmpleado = empleadoEnkontrol.personalID;
                                        objCaphorasHombre.nombreEmpleado = empleadoEnkontrol.nombre + " " + empleadoEnkontrol.ape_paterno + " " + empleadoEnkontrol.ape_materno;
                                        objCaphorasHombre.puestoID = empleadoEnkontrol.puesto;

                                        var trabajosInstalacion = item.Where(x => x.numEmpleado == empleadoEnkontrol.personalID && x.categoriaTrabajo == 2);
                                        var limpieza = item.Where(x => x.numEmpleado == empleadoEnkontrol.personalID && x.categoriaTrabajo == 3);
                                        var consultaInformacion = item.Where(x => x.numEmpleado == empleadoEnkontrol.personalID && x.categoriaTrabajo == 4);
                                        var tiempoDescanso = item.Where(x => x.numEmpleado == empleadoEnkontrol.personalID && x.categoriaTrabajo == 5);
                                        var cursoCapacitacion = item.Where(x => x.numEmpleado == empleadoEnkontrol.personalID && x.categoriaTrabajo == 6);
                                        var monitoreoDiario = item.Where(x => x.numEmpleado == empleadoEnkontrol.personalID && x.categoriaTrabajo == 7);

                                        var objNuevo = setHorasHombres(objCaphorasHombre.numEmpleado, objCaphorasHombre.nombreEmpleado, objCaphorasHombre.puestoID);

                                        objCaphorasHombre.total = item.Sum(s => s.tiempo);
                                        objCaphorasHombre.trabajosInstalacion = trabajosInstalacion.ToList();
                                        objCaphorasHombre.limpieza = limpieza.ToList();
                                        objCaphorasHombre.consultaInformacion = consultaInformacion.ToList();
                                        objCaphorasHombre.tiempoDescanso = tiempoDescanso.ToList();
                                        objCaphorasHombre.cursoCapacitacion = cursoCapacitacion.ToList();
                                        objCaphorasHombre.monitoreoDiario = monitoreoDiario.ToList();

                                        objCaphorasHombre.trabajosInstalacion.AddRange(objNuevo.trabajosInstalacion);
                                        objCaphorasHombre.limpieza.AddRange(objNuevo.limpieza);
                                        objCaphorasHombre.consultaInformacion.AddRange(objNuevo.consultaInformacion);
                                        objCaphorasHombre.tiempoDescanso.AddRange(objNuevo.tiempoDescanso);
                                        objCaphorasHombre.cursoCapacitacion.AddRange(objNuevo.cursoCapacitacion);
                                        objCaphorasHombre.monitoreoDiario.AddRange(objNuevo.monitoreoDiario);

                                        res.Add(objCaphorasHombre);
                                    }
                                }

                                foreach (var item in listaEmpleados)
                                {
                                    res.Add(setHorasHombres(item.personalID, item.nombre + " " + item.ape_paterno + " " + item.ape_materno, item.puesto));
                                }
                            }

                            return res;
                        }
                        catch (Exception ex)
                        {
                            return new List<newCapHorasHombreDTO>();
                        }
                        #endregion
                    }
                default:
                    {
                        #region DEMÁS EMPRESAS
                        try
                        {
                            var listaEmpleadosL1 = _context.Select<PuestosDTO>(new DapperDTO
                            {
                                baseDatos = MainContextEnum.Construplan,
                                consulta = string.Format(@"
                                    SELECT
                                        p.puesto, p.descripcion, e.clave_empleado AS personalID, e.nombre, ape_paterno, e.ape_materno 
                                    FROM tblRH_EK_Empleados AS e 
                                        INNER JOIN tblRH_EK_Puestos AS p ON e.puesto = p.puesto 
                                    WHERE p.puesto IN ({0}) AND e.estatus_empleado = 'A' AND cc_contable ='{1}' {2} {3}",
                                    GetListaPuestos(lstPuestosEnkontrol.Select(y => y.puesto.ToString()).ToList()),
                                    cc,
                                    (empleado != 0 ? " AND e.clave_empleado = @empleado" : ""),
                                    (clave_depto > 0 ? " AND e.clave_depto = @clave_depto" : "")
                                ),
                                parametros = new { empleado, clave_depto }
                            });

                            rawData.AddRange(listaEmpleadosL1);

                            var listaEmpleados2 = _context.Select<PuestosDTO>(new DapperDTO
                            {
                                baseDatos = MainContextEnum.Arrendadora,
                                consulta = string.Format(@"
                                    SELECT
                                        p.puesto, p.descripcion, e.clave_empleado AS personalID, e.nombre, ape_paterno, e.ape_materno 
                                    FROM tblRH_EK_Empleados AS e 
                                        INNER JOIN tblRH_EK_Puestos AS p ON e.puesto = p.puesto 
                                    WHERE p.puesto IN ({0}) AND e.estatus_empleado = 'A' AND cc_contable ='{1}' {2} {3}",
                                    GetListaPuestos(lstPuestosEnkontrol.Select(y => y.puesto.ToString()).ToList()),
                                    cc,
                                    (empleado != 0 ? " AND e.clave_empleado = @empleado" : ""),
                                    (clave_depto > 0 ? " AND e.clave_depto = @clave_depto" : "")
                                ),
                                parametros = new { empleado, clave_depto }
                            });

                            rawData.AddRange(listaEmpleados2);

                            listaEmpleados.AddRange(rawData.Distinct());
                            var fechaDate = fecha.Date;
                            var dataDT = _context.tblM_CapHorasHombre.Where(x => x.fechaCaptura == fechaDate && x.centroCostos.Equals(cc) && x.turno == turno).GroupBy(g => g.numEmpleado);

                            if (dataDT.Count() == 0 && listaEmpleados.Count == 0)
                            {
                                return res;
                            }

                            if (dataDT.Count() == 0 && listaEmpleados.Count != 0)
                            {
                                foreach (var item in listaEmpleados)
                                {
                                    res.Add(setHorasHombres(item.personalID, item.nombre + " " + item.ape_paterno + " " + item.ape_materno, item.puesto));
                                }

                                return res;
                            }

                            if (dataDT.Count() != 0 && listaEmpleados.Count != 0)
                            {
                                foreach (var item in dataDT)
                                {
                                    var empleadoEnkontrol = listaEmpleados.FirstOrDefault(f => f.personalID == item.Key);

                                    if (empleadoEnkontrol != null)
                                    {
                                        listaEmpleados.Remove(empleadoEnkontrol);

                                        var objCaphorasHombre = new newCapHorasHombreDTO();
                                        objCaphorasHombre.numEmpleado = empleadoEnkontrol.personalID;
                                        objCaphorasHombre.nombreEmpleado = empleadoEnkontrol.nombre + " " + empleadoEnkontrol.ape_paterno + " " + empleadoEnkontrol.ape_materno;
                                        objCaphorasHombre.puestoID = empleadoEnkontrol.puesto;

                                        var trabajosInstalacion = item.Where(x => x.numEmpleado == empleadoEnkontrol.personalID && x.categoriaTrabajo == 2);
                                        var limpieza = item.Where(x => x.numEmpleado == empleadoEnkontrol.personalID && x.categoriaTrabajo == 3);
                                        var consultaInformacion = item.Where(x => x.numEmpleado == empleadoEnkontrol.personalID && x.categoriaTrabajo == 4);
                                        var tiempoDescanso = item.Where(x => x.numEmpleado == empleadoEnkontrol.personalID && x.categoriaTrabajo == 5);
                                        var cursoCapacitacion = item.Where(x => x.numEmpleado == empleadoEnkontrol.personalID && x.categoriaTrabajo == 6);
                                        var monitoreoDiario = item.Where(x => x.numEmpleado == empleadoEnkontrol.personalID && x.categoriaTrabajo == 7);

                                        var objNuevo = setHorasHombres(objCaphorasHombre.numEmpleado, objCaphorasHombre.nombreEmpleado, objCaphorasHombre.puestoID);

                                        objCaphorasHombre.total = item.Sum(s => s.tiempo);
                                        objCaphorasHombre.trabajosInstalacion = trabajosInstalacion.ToList();
                                        objCaphorasHombre.limpieza = limpieza.ToList();
                                        objCaphorasHombre.consultaInformacion = consultaInformacion.ToList();
                                        objCaphorasHombre.tiempoDescanso = tiempoDescanso.ToList();
                                        objCaphorasHombre.cursoCapacitacion = cursoCapacitacion.ToList();
                                        objCaphorasHombre.monitoreoDiario = monitoreoDiario.ToList();

                                        objCaphorasHombre.trabajosInstalacion.AddRange(objNuevo.trabajosInstalacion);
                                        objCaphorasHombre.limpieza.AddRange(objNuevo.limpieza);
                                        objCaphorasHombre.consultaInformacion.AddRange(objNuevo.consultaInformacion);
                                        objCaphorasHombre.tiempoDescanso.AddRange(objNuevo.tiempoDescanso);
                                        objCaphorasHombre.cursoCapacitacion.AddRange(objNuevo.cursoCapacitacion);
                                        objCaphorasHombre.monitoreoDiario.AddRange(objNuevo.monitoreoDiario);

                                        res.Add(objCaphorasHombre);
                                    }
                                }

                                foreach (var item in listaEmpleados)
                                {
                                    res.Add(setHorasHombres(item.personalID, item.nombre + " " + item.ape_paterno + " " + item.ape_materno, item.puesto));
                                }
                            }

                            return res;
                        }
                        catch (Exception ex)
                        {
                            return new List<newCapHorasHombreDTO>();
                        }
                        #endregion
                    }
            }
        }

        private newCapHorasHombreDTO setHorasHombres(int numEmpleado, string nombreEmpleado, int puestoID)
        {
            newCapHorasHombreDTO objCaphorasHombre = new newCapHorasHombreDTO();

            objCaphorasHombre.numEmpleado = numEmpleado;
            objCaphorasHombre.nombreEmpleado = nombreEmpleado;
            objCaphorasHombre.puestoID = puestoID;

            var trabajosInstalacionObj = new tblM_CapHorasHombre();
            var limpiezaObj = new tblM_CapHorasHombre();
            var consultaInformacionObj = new tblM_CapHorasHombre();
            var tiempoDescansoObj = new tblM_CapHorasHombre();
            var cursoCapacitacionObj = new tblM_CapHorasHombre();
            var monitoreoDiarioObj = new tblM_CapHorasHombre();

            trabajosInstalacionObj.id = 0;
            trabajosInstalacionObj.centroCostos = "";
            trabajosInstalacionObj.fechaCaptura = DateTime.Now;
            trabajosInstalacionObj.nombreEmpleado = objCaphorasHombre.nombreEmpleado;
            trabajosInstalacionObj.puestoID = objCaphorasHombre.puestoID;
            trabajosInstalacionObj.subCategoria = 0;
            trabajosInstalacionObj.tiempo = 0;
            trabajosInstalacionObj.turno = 1;
            trabajosInstalacionObj.usuarioCapturaID = vSesiones.sesionUsuarioDTO.id;
            trabajosInstalacionObj.numEmpleado = objCaphorasHombre.numEmpleado;
            trabajosInstalacionObj.categoriaTrabajo = 2;

            limpiezaObj.id = 0;
            limpiezaObj.centroCostos = "";
            limpiezaObj.fechaCaptura = DateTime.Now;
            limpiezaObj.nombreEmpleado = objCaphorasHombre.nombreEmpleado;
            limpiezaObj.puestoID = objCaphorasHombre.puestoID;
            limpiezaObj.subCategoria = 0;
            limpiezaObj.tiempo = 0;
            limpiezaObj.turno = 1;
            limpiezaObj.usuarioCapturaID = vSesiones.sesionUsuarioDTO.id;
            limpiezaObj.numEmpleado = objCaphorasHombre.numEmpleado;
            limpiezaObj.categoriaTrabajo = 3;

            consultaInformacionObj.id = 0;
            consultaInformacionObj.centroCostos = "";
            consultaInformacionObj.fechaCaptura = DateTime.Now;
            consultaInformacionObj.nombreEmpleado = objCaphorasHombre.nombreEmpleado;
            consultaInformacionObj.puestoID = objCaphorasHombre.puestoID;
            consultaInformacionObj.subCategoria = 0;
            consultaInformacionObj.tiempo = 0;
            consultaInformacionObj.turno = 1;
            consultaInformacionObj.usuarioCapturaID = vSesiones.sesionUsuarioDTO.id;
            consultaInformacionObj.numEmpleado = objCaphorasHombre.numEmpleado;
            consultaInformacionObj.categoriaTrabajo = 4;

            tiempoDescansoObj.id = 0;
            tiempoDescansoObj.centroCostos = "";
            tiempoDescansoObj.fechaCaptura = DateTime.Now;
            tiempoDescansoObj.nombreEmpleado = objCaphorasHombre.nombreEmpleado;
            tiempoDescansoObj.puestoID = objCaphorasHombre.puestoID;
            tiempoDescansoObj.subCategoria = 0;
            tiempoDescansoObj.tiempo = 0;
            tiempoDescansoObj.turno = 1;
            tiempoDescansoObj.usuarioCapturaID = vSesiones.sesionUsuarioDTO.id;
            tiempoDescansoObj.numEmpleado = objCaphorasHombre.numEmpleado;
            tiempoDescansoObj.categoriaTrabajo = 5;

            cursoCapacitacionObj.id = 0;
            cursoCapacitacionObj.centroCostos = "";
            cursoCapacitacionObj.fechaCaptura = DateTime.Now;
            cursoCapacitacionObj.nombreEmpleado = objCaphorasHombre.nombreEmpleado;
            cursoCapacitacionObj.puestoID = objCaphorasHombre.puestoID;
            cursoCapacitacionObj.subCategoria = 0;
            cursoCapacitacionObj.tiempo = 0;
            cursoCapacitacionObj.turno = 1;
            cursoCapacitacionObj.usuarioCapturaID = vSesiones.sesionUsuarioDTO.id;
            cursoCapacitacionObj.numEmpleado = objCaphorasHombre.numEmpleado;
            cursoCapacitacionObj.categoriaTrabajo = 6;

            monitoreoDiarioObj.id = 0;
            monitoreoDiarioObj.centroCostos = "";
            monitoreoDiarioObj.fechaCaptura = DateTime.Now;
            monitoreoDiarioObj.nombreEmpleado = objCaphorasHombre.nombreEmpleado;
            monitoreoDiarioObj.puestoID = objCaphorasHombre.puestoID;
            monitoreoDiarioObj.subCategoria = 0;
            monitoreoDiarioObj.tiempo = 0;
            monitoreoDiarioObj.turno = 1;
            monitoreoDiarioObj.usuarioCapturaID = vSesiones.sesionUsuarioDTO.id;
            monitoreoDiarioObj.numEmpleado = objCaphorasHombre.numEmpleado;
            monitoreoDiarioObj.categoriaTrabajo = 7;

            objCaphorasHombre.trabajosInstalacion.Add(trabajosInstalacionObj);
            objCaphorasHombre.limpieza.Add(limpiezaObj);
            objCaphorasHombre.consultaInformacion.Add(consultaInformacionObj);
            objCaphorasHombre.tiempoDescanso.Add(tiempoDescansoObj);
            objCaphorasHombre.cursoCapacitacion.Add(cursoCapacitacionObj);
            objCaphorasHombre.monitoreoDiario.Add(monitoreoDiarioObj);

            return objCaphorasHombre;
        }


        public List<tblM_CatSubCategoriasHH> getSubCategorias(List<int> list)
        {
            return _context.tblM_CatSubCategoriasHH.Where(x => (list.Count > 0 ? list.Contains(x.categoriaID) : x.id == x.id) && x.estatus == true).ToList();
        }

        public Dictionary<string, object> getSubCategorias()
        {
            var r = new Dictionary<string, object>();

            var categorias = new Dictionary<string, int>();
            categorias.Add("catTrabajosInstalacionesLista", 2);
            categorias.Add("catLimpiezaLista", 3);
            categorias.Add("catConsultaInformacionLista", 4);
            categorias.Add("catTiempoDescansoLista", 5);
            categorias.Add("catCursosCapacitacionesLista", 6);
            categorias.Add("catMonitoreoDiario", 7);

            var subCat = _context.tblM_CatSubCategoriasHH.Where(x => x.estatus).GroupBy(x => new { x.categoriaID, x.id, x.descripcion });

            foreach (var item in categorias)
            {
                r.Add(item.Key, subCat.Where(x => x.Key.categoriaID == item.Value).Select(m => new
                {
                    Value = m.Key.id,
                    Text = m.Key.descripcion,
                    categoria = m.Key.categoriaID
                }));
            }

            return r;
        }

        public string GetListaPuestos(List<string> listaPuestos)
        {
            string concat = "";

            foreach (var item in listaPuestos)
            {
                concat += "'" + item.Trim() + "',";
            }
            return concat.TrimEnd(',');
        }

        public void guardarInformacion(List<tblM_CapHorasHombre> array)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    foreach (var item in array)
                    {
                        _context.tblM_CapHorasHombre.AddOrUpdate(x => x.id, item);
                    }
                    _context.SaveChanges();
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw new Exception(ex.Message, ex);
                }
            }
            //foreach (tblM_CapHorasHombre obj in array)
            //{
            //    if (true)
            //    {
            //        if (obj.id == 0)
            //            SaveEntity(obj, (int)BitacoraEnum.capHorasHombre);
            //        else
            //            Update(obj, obj.id, (int)BitacoraEnum.capHorasHombre);
            //    }
            //    else
            //    {
            //        throw new Exception("Error ocurrio un error al insertar un registro");
            //    }
            //}
        }

        public List<GeneralConcentradoHHDTO> getConcentradoGeneral(List<string> cc, DateTime fechaInicio, DateTime fechaFin, List<int> listaCategorias, List<int> subCategoriasHH)
        {

            var listaPuestos = _context.tblM_CatPuestosMaquinaria.ToList();

            List<string> auxCC = new List<string>();
            if (cc != null) auxCC.AddRange(cc);
            List<int> auxListaCategorias = new List<int>();
            if (listaCategorias != null) auxListaCategorias.AddRange(listaCategorias);
            List<int> auxSubCategoriasHH = new List<int>();
            if (subCategoriasHH != null) auxSubCategoriasHH.AddRange(subCategoriasHH);


            var listaEmpladoHO = _context.tblM_CapHorasHombre.Where(x =>
                                auxCC.Contains(x.centroCostos)
                                && (x.fechaCaptura >= fechaInicio && x.fechaCaptura <= fechaFin) &&
                                   (auxListaCategorias.Count() == 0 ? x.id == x.id : auxListaCategorias.Contains(x.categoriaTrabajo)) &&
                                   (auxSubCategoriasHH.Count() == 0 ? x.id == x.id : auxSubCategoriasHH.Contains(x.subCategoria))
                                ).ToList();

            listaEmpladoHO = listaEmpladoHO.Where(x => x.fechaCaptura.Date >= fechaInicio && x.fechaCaptura.Date <= fechaFin.Date).ToList();
            var TotalDeRegistrosXPuesto = listaEmpladoHO.ToList().Count();


            string concatPuestos = "(";
            foreach (var item in listaPuestos)
            {
                concatPuestos += item.puesto + ",";
            }
            concatPuestos = concatPuestos.TrimEnd(',') + ")";


            string concatCentroCostos = "(";
            foreach (var item in cc)
            {
                concatCentroCostos += "'" + item + "',";
            }
            concatCentroCostos = concatCentroCostos.TrimEnd(',') + ")";

            //string Consutal = "SELECT p.puesto,p.descripcion,e.clave_empleado AS personalID, s.salario_base AS salarioBase, s.complemento AS salarioComplemento, ";
            //Consutal += "e.nombre,ape_paterno,e.ape_materno ";
            //Consutal += "FROM DBA.sn_empleados AS e ";
            //Consutal += "INNER JOIN DBA.si_puestos as p on e.puesto=p.puesto ";
            //Consutal += "INNER JOIN DBA.sn_tabulador_historial AS s ON e.clave_empleado=s.clave_empleado ";
            //Consutal += "INNER JOIN (SELECT  MAX(id) AS id ,clave_empleado FROM DBA.sn_tabulador_historial GROUP BY (clave_empleado)) AS F ON F.id = s.id ";
            //Consutal += "WHERE p.puesto IN " + concatPuestos + " AND cc_contable IN " + concatCentroCostos + " AND estatus_empleado = 'A'";

            List<PuestosDTO> lstPuestosEnkontrol = new List<PuestosDTO>();
            try
            {
                //var lstPuestosEnkontrol1 = (List<PuestosDTO>)ContextEnKontrolNominaArrendadora.Where(Consutal, 1).ToObject<List<PuestosDTO>>();
                var lstPuestosEnkontrol1 = _context.Select<PuestosDTO>(new DapperDTO
                {
                    baseDatos = MainContextEnum.Construplan,
                    consulta = @"SELECT p.puesto,p.descripcion,e.clave_empleado AS personalID, s.salario_base AS salarioBase, s.complemento AS salarioComplemento, 
                                    e.nombre,ape_paterno,e.ape_materno 
                                FROM tblRH_EK_Empleados AS e 
                                INNER JOIN tblRH_EK_Puestos as p on e.puesto=p.puesto 
                                INNER JOIN tblRH_EK_Tabulador_Historial AS s ON e.clave_empleado=s.clave_empleado 
                                INNER JOIN (SELECT  MAX(id) AS id ,clave_empleado FROM tblRH_EK_Tabulador_Historial GROUP BY (clave_empleado)) AS F ON F.id = s.id 
                                WHERE p.puesto IN " + concatPuestos + " AND cc_contable IN " + concatCentroCostos + " AND estatus_empleado = 'A'",
                });

                lstPuestosEnkontrol.AddRange(lstPuestosEnkontrol1.ToList());
            }
            catch (Exception)
            {
            }
            try
            {
                //var lstPuestosEnkontrol2 = (List<PuestosDTO>)ContextEnKontrolNominaArrendadora.Where(Consutal, 2).ToObject<List<PuestosDTO>>();

                var lstPuestosEnkontrol2 = _context.Select<PuestosDTO>(new DapperDTO
                {
                    baseDatos = MainContextEnum.Arrendadora,
                    consulta = @"SELECT p.puesto,p.descripcion,e.clave_empleado AS personalID, s.salario_base AS salarioBase, s.complemento AS salarioComplemento, 
                                    e.nombre,ape_paterno,e.ape_materno 
                                FROM tblRH_EK_Empleados AS e 
                                INNER JOIN tblRH_EK_Puestos as p on e.puesto=p.puesto 
                                INNER JOIN tblRH_EK_Tabulador_Historial AS s ON e.clave_empleado=s.clave_empleado 
                                INNER JOIN (SELECT  MAX(id) AS id ,clave_empleado FROM tblRH_EK_Tabulador_Historial GROUP BY (clave_empleado)) AS F ON F.id = s.id 
                                WHERE p.puesto IN " + concatPuestos + " AND cc_contable IN " + concatCentroCostos + " AND estatus_empleado = 'A'",
                });

                lstPuestosEnkontrol.AddRange(lstPuestosEnkontrol2.ToList());
            }
            catch (Exception)
            {


            }


            var puestosAgrupados = lstPuestosEnkontrol.Distinct().GroupBy(x => new { x.puesto, x }).ToList();

            FiltrosRtpHorasHombre filtrosRtpHorasHombre = new FiltrosRtpHorasHombre();

            filtrosRtpHorasHombre.CC = cc;
            filtrosRtpHorasHombre.FechaInicio = fechaInicio;
            filtrosRtpHorasHombre.FechaFin = fechaFin;

            var horasOT = ConsultaFiltrosOT(filtrosRtpHorasHombre);
            List<tblHorasHombreConcentradoDTO> tblHorasHombreObj = new List<tblHorasHombreConcentradoDTO>();


            foreach (var item in listaPuestos)
            {
                tblHorasHombreConcentradoDTO objHH = new tblHorasHombreConcentradoDTO();
                var CountData = lstPuestosEnkontrol.Where(x => x.puesto == item.puesto).Count();
                decimal horasOTPuesto = horasOT.Where(x => x.puestoID == item.puesto).Sum(x => x.totalHorasHombre);
                var infoData = listaEmpladoHO.Where(x => x.puestoID == item.puesto && x.categoriaTrabajo != 7).ToList();
                var monitoreoDiario = listaEmpladoHO.Where(x => x.puestoID == item.puesto && x.categoriaTrabajo == 7).Sum(x => x.tiempo);
                var horasDisponibles = Convert.ToDecimal(((fechaFin - fechaInicio).TotalDays) * 6.09) * (CountData == 0 ? 1 : CountData);

                objHH.puestoID = item.puesto;
                objHH.descripcionPuesto = item.descripcion;
                objHH.totalHorasHombre = infoData.Sum(x => x.tiempo) + horasOTPuesto + monitoreoDiario;

                objHH.porRegistro = objHH != null ? Math.Round(objHH.totalHorasHombre / horasDisponibles, 2) * 100 : 0;

                objHH.horasEfectivas = Math.Round(horasOTPuesto + monitoreoDiario / horasDisponibles, 2) * 100;


                var SueldoBase = CountData != 0 ? lstPuestosEnkontrol.Where(x => x.puesto == item.puesto).Select(x => x.salarioBase).Sum(r => r) / CountData : 0;
                var Complemento = CountData != 0 ? lstPuestosEnkontrol.Where(x => x.puesto == item.puesto).Select(x => x.salarioComplemento).Sum(r => r) / CountData : 0;

                var CostoHora = Math.Round((SueldoBase + Complemento) / 55, 2);
                var PrecioHHS = Math.Round(objHH.totalHorasHombre * CostoHora);

                objHH.costoHorasHombre = CostoHora;// PrecioHHS;
                tblHorasHombreObj.Add(objHH);
            }
            var returnInfo = tblHorasHombreObj.GroupBy(x => new { x.puestoID, x.descripcionPuesto }).Select(y => new GeneralConcentradoHHDTO
            {
                puestoID = y.Key.puestoID,
                puesto = y.Key.descripcionPuesto,
                horashombre = Math.Round(tblHorasHombreObj.Where(x => y.Key.puestoID == x.puestoID).Sum(r => r.totalHorasHombre), 2),
                costohorashombre = Math.Round(tblHorasHombreObj.Where(x => y.Key.puestoID == x.puestoID).Sum(r => r.costoHorasHombre) / tblHorasHombreObj.Where(x => y.Key.puestoID == x.puestoID).Count(), 2),
                CostoTotalHorasHombre = Math.Round(tblHorasHombreObj.FirstOrDefault(p => p.puestoID == y.Key.puestoID).totalHorasHombre * tblHorasHombreObj.Where(x => y.Key.puestoID == x.puestoID).Sum(r => r.costoHorasHombre) / tblHorasHombreObj.Where(x => y.Key.puestoID == x.puestoID).Count(), 2),
                porHorasEfectivas = Math.Round(tblHorasHombreObj.FirstOrDefault(x => x.puestoID == y.Key.puestoID).horasEfectivas, 2),
                porRegistro = tblHorasHombreObj.FirstOrDefault(x => x.puestoID == y.Key.puestoID).porRegistro,
                btn = ""

            }).ToList();

            return returnInfo;
        }

        public List<DistribucionHHDTO> getDistribucionGeneral(List<string> cc, DateTime fechaInicio, DateTime fechaFin, List<int> listaCategorias, List<int> subCategoriasHH)
        {

            var listaPuestos = _context.tblM_CatPuestosMaquinaria.ToList();
            var listaEmpladoHO = _context.tblM_CapHorasHombre.Where(x =>
                                cc.Contains(x.centroCostos)
                &&
                (x.fechaCaptura >= fechaInicio && x.fechaCaptura <= fechaFin)
                //listaCategorias.Contains(x.categoriaTrabajo) &&
                //subCategoriasHH.Contains(x.subCategoria)
                                ).ToList();
            listaEmpladoHO = listaEmpladoHO.Where(x => x.fechaCaptura.Date >= fechaInicio.Date && x.fechaCaptura.Date <= fechaFin.Date).ToList();

            List<DistribucionHHDTO> listDistribucionHHDTO = new List<DistribucionHHDTO>();

            FiltrosRtpHorasHombre obj = new FiltrosRtpHorasHombre();
            obj.CC = cc;
            obj.FechaInicio = fechaInicio;
            obj.FechaFin = fechaFin;

            var Lista = ConsultaFiltrosOT(obj);

            foreach (var item in listaPuestos)
            {
                DistribucionHHDTO objHH = new DistribucionHHDTO();

                var RawData = listaEmpladoHO.Where(x => x.puestoID == item.puesto).ToList();
                var total = Lista.Where(x => x.puestoID == item.puesto).Sum(y => y.totalHorasHombre);
                objHH.puestoID = item.puesto;
                objHH.puesto = item.descripcion;
                objHH.trabajosInstalaciones = Math.Round(RawData.Where(x => x.categoriaTrabajo == 2).Sum(x => x.tiempo), 2);
                objHH.limpieza = Math.Round(RawData.Where(x => x.categoriaTrabajo == 3).Sum(x => x.tiempo), 2);
                objHH.consultaInformacion = Math.Round(RawData.Where(x => x.categoriaTrabajo == 4).Sum(x => x.tiempo));
                objHH.tiempoDescanso = Math.Round(RawData.Where(x => x.categoriaTrabajo == 5).Sum(x => x.tiempo), 2);
                objHH.cursosCapacitaciones = Math.Round(RawData.Where(x => x.categoriaTrabajo == 6).Sum(x => x.tiempo), 2);
                objHH.monitoreoDiario = Math.Round(RawData.Where(x => x.categoriaTrabajo == 7).Sum(x => x.tiempo), 2);
                objHH.totalHorashombre = Math.Round(objHH.trabajosInstalaciones + objHH.limpieza + objHH.consultaInformacion + objHH.tiempoDescanso + objHH.cursosCapacitaciones + objHH.monitoreoDiario + total, 2);
                objHH.trabajoMaquinariaOT = Math.Round(total, 2);

                listDistribucionHHDTO.Add(objHH);
            }



            return listDistribucionHHDTO;
        }


        public List<DistribucionHHPersonalDTO> DetalleDistribucionGeneral(List<string> cc, DateTime fechaInicio, DateTime fechaFin, int puestoID)
        {
            List<DistribucionHHPersonalDTO> result = new List<DistribucionHHPersonalDTO>();

            var objRaw = _context.tblM_CapHorasHombre.ToList().Where(x => cc.Contains(x.centroCostos) && x.fechaCaptura.Date >= fechaInicio.Date && x.fechaCaptura.Date <= fechaFin.Date && x.puestoID == puestoID).ToList();



            string Consutal = "SELECT p.puesto,p.descripcion,e.clave_empleado AS personalID,";//, s.salario_base AS salarioBase, s.complemento AS salarioComplemento, ";
            Consutal += "e.nombre,ape_paterno,e.ape_materno ";
            Consutal += "FROM DBA.sn_empleados AS e ";
            Consutal += "INNER JOIN DBA.si_puestos as p on e.puesto=p.puesto ";
            //   Consutal += "INNER JOIN DBA.sn_tabulador_historial AS s ON e.clave_empleado=s.clave_empleado ";
            //    Consutal += "INNER JOIN (SELECT  MAX(id) AS id ,clave_empleado FROM DBA.sn_tabulador_historial GROUP BY (clave_empleado)) AS F ON F.id = s.id ";
            Consutal += "WHERE p.puesto IN (" + (puestoID) + ") AND estatus_empleado = 'A'";

            //var lstPuestosEnkontrol = (List<PuestosDTO>)ContextEnKontrolNomina.Where(Consutal).ToObject<List<PuestosDTO>>();

            var lstPuestosEnkontrol = _context.Select<PuestosDTO>(new DapperDTO
            {
                baseDatos = (int)vSesiones.sesionEmpresaActual == (int)EmpresaEnum.Construplan ? MainContextEnum.Construplan : MainContextEnum.Arrendadora,
                consulta = @"SELECT p.puesto,p.descripcion,e.clave_empleado AS personalID,
                                    e.nombre,ape_paterno,e.ape_materno 
                                FROM tblRH_EK_Empleados AS e 
                                INNER JOIN tblRH_EK_Puestos as p on e.puesto=p.puesto 
                                WHERE p.puesto IN (" + (puestoID) + ") AND estatus_empleado = 'A'",
            });

            var listaEmpleadosEnkontrol = lstPuestosEnkontrol.GroupBy(x => x.personalID).Select(x => x.Key).ToList();

            var rawOT = (from dot in _context.tblM_DetOrdenTrabajo
                         where cc.Contains(dot.OrdenTrabajo.CC)
                         select dot).ToList().Where(x => listaEmpleadosEnkontrol.Contains(x.PersonalID) && x.HoraInicio >= fechaInicio && x.HoraFin <= fechaFin).ToList();


            var listaEmpleados = objRaw.GroupBy(x => new { x.numEmpleado }).Select(x => x.Key.numEmpleado).ToList();

            listaEmpleados.AddRange(rawOT.Select(x => x.PersonalID).ToList());


            foreach (var item in listaEmpleados.Distinct()) //objRaw.GroupBy(x => new { x.numEmpleado }).Select(x => x.Key.numEmpleado).ToList())
            {
                DistribucionHHPersonalDTO obj = new DistribucionHHPersonalDTO();
                var HorasOT = rawOT.Where(x => x.PersonalID == item);
                decimal TotalHorasOT = 0;



                foreach (var ot in HorasOT)
                {
                    var data = (decimal)(ot.HoraFin - ot.HoraInicio).TotalHours;
                    TotalHorasOT += data;
                }

                var RawData = objRaw.Where(x => x.numEmpleado == item).ToList();
                var objEmpleado = lstPuestosEnkontrol.FirstOrDefault(x => x.personalID == item);

                if (objEmpleado != null)
                {


                    obj.numEmpleado = objEmpleado.personalID;
                    obj.nombreEmpleado = objEmpleado.nombre + " " + objEmpleado.ape_paterno + " " + objEmpleado.ape_materno;
                    obj.puestoID = objEmpleado.puesto;
                    obj.trabajoMaquinariaOT = Math.Round(TotalHorasOT, 2);
                    obj.trabajosInstalaciones = Math.Round(RawData.Where(x => x.categoriaTrabajo == 2).Sum(x => x.tiempo), 2);
                    obj.limpieza = Math.Round(RawData.Where(x => x.categoriaTrabajo == 3).Sum(x => x.tiempo), 2);
                    obj.consultaInformacion = Math.Round(RawData.Where(x => x.categoriaTrabajo == 4).Sum(x => x.tiempo), 2);
                    obj.tiempoDescanso = Math.Round(RawData.Where(x => x.categoriaTrabajo == 5).Sum(x => x.tiempo), 2);
                    obj.cursosCapacitaciones = Math.Round(RawData.Where(x => x.categoriaTrabajo == 6).Sum(x => x.tiempo), 2);
                    obj.monitoreoDiario = Math.Round(RawData.Where(x => x.categoriaTrabajo == 7).Sum(x => x.tiempo), 2);
                    obj.totalHorashombre = Math.Round(obj.trabajoMaquinariaOT + obj.trabajosInstalaciones + obj.limpieza + obj.consultaInformacion + obj.tiempoDescanso + obj.cursosCapacitaciones + obj.monitoreoDiario, 2);

                    result.Add(obj);
                }
            }
            return result;
        }

        private string getListaUsuarios(IEnumerable<int> enumerable)
        {
            string cadena = "";

            foreach (var item in enumerable)
            {
                cadena += "'" + item + "',";
            }
            return cadena.TrimEnd(',');
        }

        private List<tblHorasHombreDTO> ConsultaFiltrosOT(FiltrosRtpHorasHombre obj)
        {

            List<string> CCs = obj.CC.ToList();
            obj.FechaFin = obj.FechaFin.AddHours(23).AddMinutes(59).AddSeconds(60);

            var c = CCs.Count();
            if (vSesiones.sesionEmpresaActual == 2)
            {
                CCs = new List<string>();
                CCs = _context.tblP_CCRH.Where(x => obj.CC.ToList().Contains(x.cc)).Select(x => x.areaCuenta).ToList();

                //if (CCs.Count == 0)
                //{
                var ccsTemp = _context.tblP_CC.Where(x => obj.CC.ToList().Contains(x.cc)).Select(x => x.areaCuenta).ToList();
                CCs.AddRange(ccsTemp);
                //}

            }

            CCs = CCs.Distinct().ToList();

            var rawOT = (from dot in _context.tblM_DetOrdenTrabajo
                         where CCs.Contains(dot.OrdenTrabajo.CC) &&
                         dot.OrdenTrabajo.FechaEntrada >= obj.FechaInicio && dot.OrdenTrabajo.FechaSalida <= obj.FechaFin
                         select dot).ToList();

            List<tblHorasHombreDTO> tblHorasHombreObj = new List<tblHorasHombreDTO>();

            var rawPersonal = rawOT.GroupBy(x => x.PersonalID).Select(y => new
            {
                id = y.FirstOrDefault().PersonalID
            }).ToList();

            if (rawPersonal.Count > 0)
            {
                //string Consutal = "SELECT p.puesto,p.descripcion,e.clave_empleado AS personalID, s.salario_base AS salarioBase, s.complemento AS salarioComplemento, ";
                //Consutal += "e.nombre,ape_paterno,e.ape_materno ";
                //Consutal += "FROM DBA.sn_empleados AS e ";
                //Consutal += "INNER JOIN DBA.si_puestos as p on e.puesto=p.puesto ";
                //Consutal += "INNER JOIN DBA.sn_tabulador_historial AS s ON e.clave_empleado=s.clave_empleado ";
                //Consutal += "INNER JOIN (SELECT  MAX(id) AS id ,clave_empleado FROM DBA.sn_tabulador_historial GROUP BY (clave_empleado)) AS F ON F.id = s.id ";
                //Consutal += "WHERE e.clave_empleado IN (" + getListaUsuarios(rawPersonal.Select(x => x.id)) + ")  AND estatus_empleado = 'A'";
                List<PuestosDTO> lstPuestosEnkontrol = new List<PuestosDTO>();

                try
                {
                    //var rawDataConstruplan = (List<PuestosDTO>)ContextEnKontrolNominaArrendadora.Where(Consutal, 1).ToObject<List<PuestosDTO>>();

                    var rawDataConstruplan = _context.Select<PuestosDTO>(new DapperDTO
                    {
                        baseDatos = MainContextEnum.Construplan,
                        consulta = @"SELECT p.puesto,p.descripcion,e.clave_empleado AS personalID, s.salario_base AS salarioBase, s.complemento AS salarioComplemento, 
                                    e.nombre,ape_paterno,e.ape_materno 
                                FROM tblRH_EK_Empleados AS e 
                                INNER JOIN tblRH_EK_Puestos as p on e.puesto=p.puesto 
                                INNER JOIN tblRH_EK_Tabulador_Historial AS s ON e.clave_empleado=s.clave_empleado 
                                INNER JOIN (SELECT  MAX(id) AS id ,clave_empleado FROM tblRH_EK_Tabulador_Historial GROUP BY (clave_empleado)) AS F ON F.id = s.id 
                                WHERE e.clave_empleado IN (" + getListaUsuarios(rawPersonal.Select(x => x.id)) + ")  AND estatus_empleado = 'A'",
                    });

                    lstPuestosEnkontrol.AddRange(rawDataConstruplan);
                }
                catch (Exception)
                {


                }

                try
                {
                    //var rawDataArrendadora = (List<PuestosDTO>)ContextEnKontrolNominaArrendadora.Where(Consutal, 2).ToObject<List<PuestosDTO>>();

                    var rawDataArrendadora = _context.Select<PuestosDTO>(new DapperDTO
                    {
                        baseDatos = MainContextEnum.Construplan,
                        consulta = @"SELECT p.puesto,p.descripcion,e.clave_empleado AS personalID, s.salario_base AS salarioBase, s.complemento AS salarioComplemento, 
                                    e.nombre,ape_paterno,e.ape_materno 
                                FROM tblRH_EK_Empleados AS e 
                                INNER JOIN tblRH_EK_Puestos as p on e.puesto=p.puesto 
                                INNER JOIN tblRH_EK_Tabulador_Historial AS s ON e.clave_empleado=s.clave_empleado 
                                INNER JOIN (SELECT  MAX(id) AS id ,clave_empleado FROM tblRH_EK_Tabulador_Historial GROUP BY (clave_empleado)) AS F ON F.id = s.id 
                                WHERE e.clave_empleado IN (" + getListaUsuarios(rawPersonal.Select(x => x.id)) + ")  AND estatus_empleado = 'A'",
                    });

                    lstPuestosEnkontrol.AddRange(rawDataArrendadora);
                }
                catch (Exception)
                {


                }


                foreach (var item in lstPuestosEnkontrol.OrderBy(x => x.puesto))
                {

                    tblHorasHombreDTO objHH = new tblHorasHombreDTO();

                    var inforData = rawOT.Where(x => x.PersonalID == item.personalID).ToList();

                    objHH.puestoID = item.puesto;
                    objHH.descripcionPuesto = item.descripcion;
                    objHH.totalHorasHombre = inforData.Sum(x => GetTotalHoras(x.HoraInicio, x.HoraFin));

                    var SueldoBase = item.salarioBase;
                    var Complemento = item.salarioComplemento;
                    var CostoHora = (SueldoBase + Complemento) / 55;
                    var PrecioHHS = objHH.totalHorasHombre * CostoHora;

                    objHH.costoHorasHombre = CostoHora;// PrecioHHS;
                    tblHorasHombreObj.Add(objHH);
                }
            }
            return tblHorasHombreObj;

        }

        private decimal GetTotalHoras(DateTime dateTime1, DateTime? dateTime2)
        {
            var FechaEntrada = dateTime1;
            var FechaSalida = dateTime2;

            if (dateTime2 != null)
            {
                var horas = (decimal)((DateTime)FechaSalida - FechaEntrada).TotalHours;
                return horas;
            }
            else
            {
                return 0;
            }
        }

        public List<ComboDTO> fillCboPuestos()
        {
            return _context.tblM_CatPuestosMaquinaria.Select(x => new ComboDTO { Value = x.puesto.ToString(), Text = x.descripcion }).ToList();

        }

        public List<ComboDTO> getNombreEmpleado(string term, int puesto)
        {
            List<ComboDTO> lstProveedores = new List<ComboDTO>();

            string parametro = "";
            if (puesto != 0)
                parametro = " AND puesto = " + puesto;
            else
                parametro = "";

            //            var getCatEmpleado = @"SELECT TOP 5 clave_empleado AS Value, (LTRIM(RTRIM(nombre))+' '+replace(ape_paterno, ' ', '')+' '+replace(ape_materno, ' ', '')) AS Text 
            //                                   FROM DBA.sn_empleados 
            //                                   WHERE replace((nombre+' '+ape_paterno+' '+ape_materno), ' ', '') LIKE '%" + term.Replace(" ", "") + "%' " + parametro + " AND estatus_empleado = 'A'";

            var getCatEmpleado = @"SELECT TOP 5 clave_empleado AS Value, (LTRIM(RTRIM(nombre))+' '+replace(ape_paterno, ' ', '')+' '+replace(ape_materno, ' ', '')) AS Text 
                                   FROM tblRH_EK_Empleados 
                                   WHERE replace((nombre+' '+ape_paterno+' '+ape_materno), ' ', '') LIKE '%" + term.Replace(" ", "") + "%' " + parametro + " AND estatus_empleado = 'A'";
            try
            {
                //var resultado = (List<ComboDTO>)ContextEnKontrolNominaArrendadora.Where(getCatEmpleado, 1).ToObject<List<ComboDTO>>();

                switch ((EmpresaEnum)vSesiones.sesionEmpresaActual)
                {
                    case EmpresaEnum.Peru:
                        {
                            var resultado = _context.Select<ComboDTO>(new DapperDTO
                                {
                                    baseDatos = MainContextEnum.PERU,
                                    consulta = getCatEmpleado
                                });

                            lstProveedores.AddRange(resultado);

                            return lstProveedores;
                        }
                        break;
                    default:
                        {
                            var resultado = _context.Select<ComboDTO>(new DapperDTO
                            {
                                baseDatos = MainContextEnum.Construplan,
                                consulta = getCatEmpleado,
                            });

                            lstProveedores.AddRange(resultado);
                            //var resultado2 = (List<ComboDTO>)ContextEnKontrolNominaArrendadora.Where(getCatEmpleado, 2).ToObject<List<ComboDTO>>();

                            var resultado2 = _context.Select<ComboDTO>(new DapperDTO
                            {
                                baseDatos = MainContextEnum.Arrendadora,
                                consulta = getCatEmpleado,
                            });

                            lstProveedores.AddRange(resultado2);
                            return lstProveedores;
                        }
                        break;
                }
            }
            catch
            {
                return lstProveedores;
            }
        }

        public ComboDTO searchNumEmpleado(string term, int puesto, int numEmpleado)
        {
            ComboDTO lstProveedores = new ComboDTO();

            string parametro = "";
            if (puesto != 0)
                parametro = " AND puesto = " + puesto;
            else
                parametro = "";

            //            var getCatEmpleado = @"SELECT TOP 1 clave_empleado AS Value, (LTRIM(RTRIM(nombre))+' '+replace(ape_paterno, ' ', '')+' '+replace(ape_materno, ' ', '')) AS Text , sn_empleados.puesto as Id ,descripcion as Prefijo
            //                                   FROM sn_empleados 
            //                                        INNER JOIN si_puestos
            //                                        ON sn_empleados.puesto = si_puestos.puesto
            //                                   WHERE clave_empleado = '" + numEmpleado + "' AND estatus_empleado = 'A'";

            var getCatEmpleado = @"SELECT TOP 1 clave_empleado AS Value, (LTRIM(RTRIM(nombre))+' '+replace(ape_paterno, ' ', '')+' '+replace(ape_materno, ' ', '')) AS Text , tblRH_EK_Empleados.puesto as Id ,descripcion as Prefijo
                                   FROM tblRH_EK_Empleados 
                                        INNER JOIN tblRH_EK_Puestos
                                        ON tblRH_EK_Empleados.puesto = tblRH_EK_Puestos.puesto
                                   WHERE clave_empleado = '" + numEmpleado + "' AND estatus_empleado = 'A'";

            try
            {
                //var resultado = (List<ComboDTO>)ContextEnKontrolNominaArrendadora.Where(getCatEmpleado, 1).ToObject<List<ComboDTO>>();

                if (vSesiones.sesionEmpresaActual == 1 || vSesiones.sesionEmpresaActual == 2)
                {
                    var resultado = _context.Select<ComboDTO>(new DapperDTO
                    {
                        baseDatos = MainContextEnum.Construplan,
                        consulta = getCatEmpleado,
                    });
                    lstProveedores = resultado[0];
                }
                else
                {
                    var empleadosEmpresas = _context.Select<ComboDTO>(new DapperDTO
                    {
                        baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
                        consulta = @"SELECT TOP 1 clave_empleado AS Value, (LTRIM(RTRIM(nombre))+' '+replace(ape_paterno, ' ', '')+' '+replace(ape_materno, ' ', '')) AS Text , tblRH_EK_Empleados.puesto as Id ,descripcion as Prefijo
                                   FROM tblRH_EK_Empleados 
                                        INNER JOIN tblRH_EK_Puestos
                                        ON tblRH_EK_Empleados.puesto = tblRH_EK_Puestos.puesto
                                   WHERE clave_empleado = '" + numEmpleado + "' AND estatus_empleado = 'A'",
                    });
                    lstProveedores = empleadosEmpresas[0];
                }

                return lstProveedores;
            }
            catch (Exception ex)
            {
                return lstProveedores;
            }
        }


        public List<EmpleadoConcentradoGeneralDTO> getInfoByEmpleado(List<string> cc, DateTime fechaInicio, DateTime fechaFin, int numEmpleado)
        {
            List<EmpleadoConcentradoGeneralDTO> result = new List<EmpleadoConcentradoGeneralDTO>();

            var objRaw = _context.tblM_CapHorasHombre.ToList().Where(x => cc.Contains(x.centroCostos) && x.fechaCaptura.Date >= fechaInicio.Date && x.fechaCaptura.Date <= fechaFin.Date && x.numEmpleado == numEmpleado).ToList();
            var subCategorias = _context.tblM_CatSubCategoriasHH.ToList();
            var categorias = _context.tblM_CatCategoriasHH.ToList();

            //string ConsutaEmpleado = "SELECT p.puesto,p.descripcion,e.clave_empleado AS personalID, s.salario_base AS salarioBase, s.complemento AS salarioComplemento, ";
            //ConsutaEmpleado += "e.nombre,ape_paterno,e.ape_materno ";
            //ConsutaEmpleado += "FROM DBA.sn_empleados AS e ";
            //ConsutaEmpleado += "INNER JOIN DBA.si_puestos as p on e.puesto=p.puesto ";
            //ConsutaEmpleado += "INNER JOIN DBA.sn_tabulador_historial AS s ON e.clave_empleado=s.clave_empleado ";
            //ConsutaEmpleado += "INNER JOIN (SELECT  MAX(id) AS id ,clave_empleado FROM DBA.sn_tabulador_historial GROUP BY (clave_empleado)) AS F ON F.id = s.id ";
            //ConsutaEmpleado += "WHERE e.clave_empleado IN (" + numEmpleado + ")  AND estatus_empleado = 'A'";

            //var listaEmpleados = (List<PuestosDTO>)ContextEnKontrolNomina.Where(ConsutaEmpleado).ToObject<List<PuestosDTO>>();

            var listaEmpleados = _context.Select<PuestosDTO>(new DapperDTO
            {
                baseDatos = (int)vSesiones.sesionEmpresaActual == (int)EmpresaEnum.Construplan ? MainContextEnum.Construplan : MainContextEnum.Arrendadora,
                consulta = @"SELECT p.puesto,p.descripcion,e.clave_empleado AS personalID, s.salario_base AS salarioBase, s.complemento AS salarioComplemento, 
                                    e.nombre,ape_paterno,e.ape_materno 
                                FROM tblRH_EK_Empleados AS e 
                                INNER JOIN tblRH_EK_Puestos as p on e.puesto=p.puesto 
                                INNER JOIN tblRH_EK_Tabulador_Historial AS s ON e.clave_empleado=s.clave_empleado 
                                INNER JOIN (SELECT  MAX(id) AS id ,clave_empleado FROM tblRH_EK_Tabulador_Historial GROUP BY (clave_empleado)) AS F ON F.id = s.id 
                                WHERE e.clave_empleado IN (" + numEmpleado + ")  AND estatus_empleado = 'A'",
            });

            var datosEmpleadoEnkontrol = listaEmpleados.FirstOrDefault();

            var OTData = _context.tblM_DetOrdenTrabajo.Where(x => x.PersonalID == numEmpleado && x.HoraInicio >= fechaInicio && x.HoraFin <= fechaFin).ToList();
            var TotalOT = regresarTotalHoras(OTData);

            foreach (var item in subCategorias)
            {
                EmpleadoConcentradoGeneralDTO objData = new EmpleadoConcentradoGeneralDTO();
                if (item.categoriaID != 1)
                {
                    var horaRAw = objRaw.Where(x => x.subCategoria == item.id).ToList();

                    objData.categoria = categorias.FirstOrDefault(x => x.id == item.categoriaID).descripcion;
                    objData.subCategoria = item.descripcion;
                    objData.horasHombre = horaRAw.Where(x => x.subCategoria == item.id).Sum(x => x.tiempo);


                    var SueldoBase = datosEmpleadoEnkontrol.salarioBase;
                    var Complemento = datosEmpleadoEnkontrol.salarioComplemento;
                    var CostoHora = Math.Round((SueldoBase + Complemento) / 55, 2);
                    var PrecioHHS = objData.horasHombre * CostoHora;
                    objData.costoHH = CostoHora;
                    objData.costoTotal = Math.Round(PrecioHHS, 2);
                    //objData.horasHombre = CostoHora;// PrecioHHS;


                }
                else
                {


                    objData.categoria = categorias.FirstOrDefault(x => x.id == item.categoriaID).descripcion;
                    objData.subCategoria = item.descripcion;
                    objData.horasHombre = Math.Round(TotalOT, 2);
                    var SueldoBase = datosEmpleadoEnkontrol.salarioBase;
                    var Complemento = datosEmpleadoEnkontrol.salarioComplemento;
                    var CostoHora = Math.Round((SueldoBase + Complemento) / 55, 2);
                    var PrecioHHS = objData.horasHombre * CostoHora;
                    objData.costoHH = CostoHora;
                    objData.costoTotal = Math.Round(PrecioHHS, 2);

                }
                result.Add(objData);


            }

            return result;
        }

        public List<rptUtilizacionDTO> getDetalleConcentradoGeneral(List<string> cc, DateTime fechaInicio, DateTime fechaFin, int puestoID)
        {
            List<rptUtilizacionDTO> listaUtilizacion = new List<rptUtilizacionDTO>();

            var listaEmpladoHO = _context.tblM_CapHorasHombre.ToList().Where(x =>
                    cc.Contains(x.centroCostos)
                    && (x.fechaCaptura.Date >= fechaInicio && x.fechaCaptura.Date <= fechaFin.Date) &&
                    x.puestoID == puestoID
                    ).ToList();

            string concat = "(";
            foreach (var item in cc)
            {
                concat += "'" + item + "',";
            }
            concat = concat.TrimEnd(',') + ")";


            //string Consutal = "SELECT p.puesto,p.descripcion,e.clave_empleado AS personalID, s.salario_base AS salarioBase, s.complemento AS salarioComplemento, ";
            //Consutal += "e.nombre,ape_paterno,e.ape_materno, e.cc_contable  as centroCostos ";
            //Consutal += "FROM DBA.sn_empleados AS e ";
            //Consutal += "INNER JOIN DBA.si_puestos as p on e.puesto=p.puesto ";
            //Consutal += "INNER JOIN DBA.sn_tabulador_historial AS s ON e.clave_empleado=s.clave_empleado ";
            //Consutal += "INNER JOIN (SELECT  MAX(id) AS id ,clave_empleado FROM DBA.sn_tabulador_historial GROUP BY (clave_empleado)) AS F ON F.id = s.id ";
            //Consutal += "WHERE p.puesto IN (" + (puestoID) + ") AND estatus_empleado = 'A' AND e.cc_contable IN " + concat;
            List<PuestosDTO> consultaConstruplan = new List<PuestosDTO>();
            List<PuestosDTO> consultaArrendadora = new List<PuestosDTO>();

            List<PuestosDTO> lstPuestosEnkontrol = new List<PuestosDTO>();
            try
            {
                //consultaConstruplan = (List<PuestosDTO>)ContextEnKontrolNominaArrendadora.Where(Consutal, 1).ToObject<List<PuestosDTO>>();

                consultaConstruplan = _context.Select<PuestosDTO>(new DapperDTO
                {
                    baseDatos = MainContextEnum.Construplan,
                    consulta = @"SELECT p.puesto,p.descripcion,e.clave_empleado AS personalID, s.salario_base AS salarioBase, s.complemento AS salarioComplemento, 
                                    e.nombre,ape_paterno,e.ape_materno 
                                FROM tblRH_EK_Empleados AS e 
                                INNER JOIN tblRH_EK_Puestos as p on e.puesto=p.puesto 
                                INNER JOIN tblRH_EK_Tabulador_Historial AS s ON e.clave_empleado=s.clave_empleado 
                                INNER JOIN (SELECT  MAX(id) AS id ,clave_empleado FROM tblRH_EK_Tabulador_Historial GROUP BY (clave_empleado)) AS F ON F.id = s.id 
                                WHERE p.puesto IN (" + (puestoID) + ") AND estatus_empleado = 'A' AND e.cc_contable IN " + concat,
                });

                lstPuestosEnkontrol.AddRange(consultaConstruplan);
            }
            catch (Exception)
            {

            }

            try
            {
                //consultaArrendadora = (List<PuestosDTO>)ContextEnKontrolNominaArrendadora.Where(Consutal, 2).ToObject<List<PuestosDTO>>();

                consultaArrendadora = _context.Select<PuestosDTO>(new DapperDTO
                {
                    baseDatos = MainContextEnum.Arrendadora,
                    consulta = @"SELECT p.puesto,p.descripcion,e.clave_empleado AS personalID, s.salario_base AS salarioBase, s.complemento AS salarioComplemento, 
                                    e.nombre,ape_paterno,e.ape_materno 
                                FROM tblRH_EK_Empleados AS e 
                                INNER JOIN tblRH_EK_Puestos as p on e.puesto=p.puesto 
                                INNER JOIN tblRH_EK_Tabulador_Historial AS s ON e.clave_empleado=s.clave_empleado 
                                INNER JOIN (SELECT  MAX(id) AS id ,clave_empleado FROM tblRH_EK_Tabulador_Historial GROUP BY (clave_empleado)) AS F ON F.id = s.id 
                                WHERE p.puesto IN (" + (puestoID) + ") AND estatus_empleado = 'A' AND e.cc_contable IN " + concat,
                });

                lstPuestosEnkontrol.AddRange(consultaArrendadora);
            }
            catch (Exception)
            {

            }


            var listaEmpleadosOT = lstPuestosEnkontrol.Select(x => x.personalID).ToList();

            var ListaOT = _context.tblM_DetOrdenTrabajo.Where(x => listaEmpleadosOT.Contains(x.PersonalID) && x.HoraInicio >= fechaInicio && x.HoraFin <= fechaFin).ToList();

            var listaPersonas = ListaOT.GroupBy(x => x.PersonalID).Select(x => x.Key).Distinct().ToList();

            var listaPersonalOH = listaEmpladoHO.GroupBy(x => x.puestoID).Select(x => x.Key).Distinct().ToList();

            listaPersonas.AddRange(listaPersonalOH);

            var TotalDias = (fechaFin - fechaInicio).TotalDays;

            var HorasPeriodo = _context.tblM_CatJornadasPersonalCC.ToList();

            foreach (var item in listaPersonas)
            {

                rptUtilizacionDTO rptUtilizacionObj = new rptUtilizacionDTO();
                var EmpleadoData = lstPuestosEnkontrol.FirstOrDefault(x => x.personalID == item);

                if (EmpleadoData != null)
                {

                    var setAC = _context.tblP_CC.Where(c => c.cc == EmpleadoData.centroCostos).FirstOrDefault();
                    if (setAC != null)
                    {
                        var ac = setAC.areaCuenta;
                        var datosPeriodo = HorasPeriodo.FirstOrDefault(x => x.cc == ac);

                        decimal didasSemana = datosPeriodo.diasSemana;
                        decimal horasDiarias = datosPeriodo.hrsTrabajadasDias;
                        var horasOTTRabajadas = ListaOT.Where(x => x.PersonalID == item).ToList();
                        rptUtilizacionObj.hrsProyectadas = Convert.ToDecimal(((fechaFin - fechaInicio).TotalDays) * 6.09);

                        rptUtilizacionObj.Nombre = EmpleadoData.nombre + " " + EmpleadoData.ape_paterno + " " + EmpleadoData.ape_materno;
                        rptUtilizacionObj.hrsRegistradasTrabajo = Math.Round(Convert.ToDecimal(listaEmpladoHO.Where(x => x.numEmpleado == item).Sum(y => y.tiempo)) + regresarTotalHoras(horasOTTRabajadas), 2);
                        rptUtilizacionObj.hrsFaltantesRegistro = Math.Round(rptUtilizacionObj.hrsProyectadas - rptUtilizacionObj.hrsRegistradasTrabajo, 2);
                        rptUtilizacionObj.porRegistro = Math.Round(rptUtilizacionObj.hrsRegistradasTrabajo / rptUtilizacionObj.hrsProyectadas * 100, 2);
                        rptUtilizacionObj.porHorasEfectivas = Math.Round(regresarTotalHoras(horasOTTRabajadas) / rptUtilizacionObj.hrsProyectadas * 100, 2);


                        listaUtilizacion.Add(rptUtilizacionObj);

                    }

                }

            }

            return listaUtilizacion;
        }

        private decimal regresarTotalHoras(List<tblM_DetOrdenTrabajo> obj)
        {
            decimal dato = 0;


            foreach (var item in obj)
            {
                var totalHorasOT = Convert.ToDecimal((item.HoraFin - item.HoraInicio).TotalHours);

                dato += totalHorasOT;
            }

            return dato;
        }

        public List<ComboDTO> getListaPuestos()
        {

            return _context.tblM_CatPuestosMaquinaria.Select(x => new ComboDTO
            {
                Value = x.puesto.ToString(),
                Text = x.descripcion
            }).ToList();
        }

        public GraficaParetoDTO getParetoCategorias(List<string> ccs, DateTime fechaInicio, DateTime fechaFin, List<int> listaCategorias, List<int> listaSubCategoria)
        {
            List<ParetoCategoriasDTO> paretoCategorias = new List<ParetoCategoriasDTO>();
            List<ParetoCategoriasDTO> paretoSubCategorias = new List<ParetoCategoriasDTO>();

            var listaC = _context.tblM_CatCategoriasHH.ToList();
            var listaSC = _context.tblM_CatSubCategoriasHH.ToList();


            var listaEmpladoHO = _context.tblM_CapHorasHombre.ToList().Where(x =>
                     ccs.Contains(x.centroCostos)
                     && (x.fechaCaptura.Date >= fechaInicio && x.fechaCaptura.Date <= fechaFin.Date)
                     ).ToList();

            var listaEmpleadosOT = _context.tblM_DetOrdenTrabajo.Where(x => ccs.Contains(x.OrdenTrabajo.CC) && fechaInicio >= x.HoraInicio && x.HoraFin <= fechaFin).ToList();

            var sumaHrsOT = regresarTotalHoras(listaEmpleadosOT);

            foreach (var item in listaC)
            {
                ParetoCategoriasDTO obj = new ParetoCategoriasDTO();
                var DataPareto = listaEmpladoHO.Where(x => x.categoriaTrabajo == item.id).ToList();

                obj.Descripcion = item.descripcion;
                if (item.id != 1)
                {
                    obj.valuePareto = DataPareto.Sum(x => x.tiempo);
                }
                else
                {
                    obj.valuePareto = sumaHrsOT;
                }



                paretoCategorias.Add(obj);
            }
            foreach (var item in listaSC)
            {
                ParetoCategoriasDTO obj = new ParetoCategoriasDTO();
                var DataPareto = listaEmpladoHO.Where(x => x.subCategoria == item.id).ToList();

                obj.Descripcion = item.descripcion;


                if (item.categoriaID != 1)
                {
                    obj.valuePareto = DataPareto.Sum(x => x.tiempo);
                }
                else
                {
                    obj.valuePareto = sumaHrsOT;
                }
                paretoSubCategorias.Add(obj);
            }


            GraficaParetoDTO dato = new GraficaParetoDTO();

            dato.listaCategorias = paretoCategorias;
            dato.listaSubCategorias = paretoSubCategorias;

            return dato;


        }

        ////////////////////////////////////////////////////////

        public void setRptMaquinariaHorasHombre(List<int> economicos, DateTime fechaInicio, DateTime fechaFin, List<int> Puestos, List<int> Empleados, string cc)
        {

            fechaFin = fechaFin.AddHours(23).AddMinutes(59).AddSeconds(59);
            var Maquinaria = _context.tblM_CatMaquina.Where(x => economicos.Contains(x.id)).ToList();

            string centro_costosObj = "";
            int tipoNominda = 0;
            if (vSesiones.sesionEmpresaActual == 1)
            {
                centro_costosObj = cc;
                tipoNominda = 1;
                // centro_costosObj = _context.tblP_CC.Where(x => x.areaCuenta == cc).FirstOrDefault().cc;
            }
            else
            {

                try
                {
                    centro_costosObj = _context.tblP_CCRH.Where(x => x.areaCuenta == cc).FirstOrDefault().cc;
                    tipoNominda = 2;
                }
                catch (Exception)
                {

                    try
                    {
                        centro_costosObj = _context.tblP_CC.Where(x => x.areaCuenta == cc).FirstOrDefault().cc;
                        tipoNominda = 1;
                    }
                    catch (Exception)
                    {

                    }
                }


            }

            var rawDatOT = _context.tblM_DetOrdenTrabajo.Where(x => economicos.Contains(x.OrdenTrabajo.EconomicoID) && x.HoraInicio >= fechaInicio && x.HoraInicio <= fechaFin).ToList();
            var rawPersonal = rawDatOT.GroupBy(x => x.PersonalID).Select(y => new
            {
                id = y.FirstOrDefault().PersonalID
            }).ToList();

            //string Consutal = "SELECT p.puesto,p.descripcion,e.clave_empleado AS personalID, s.salario_base AS salarioBase, s.complemento AS salarioComplemento, ";
            //Consutal += "e.nombre,ape_paterno,e.ape_materno ";
            //Consutal += "FROM DBA.sn_empleados AS e ";
            //Consutal += "INNER JOIN DBA.si_puestos as p on e.puesto=p.puesto ";
            //Consutal += "INNER JOIN DBA.sn_tabulador_historial AS s ON e.clave_empleado=s.clave_empleado ";
            //Consutal += "INNER JOIN (SELECT  MAX(id) AS id ,clave_empleado FROM DBA.sn_tabulador_historial GROUP BY (clave_empleado)) AS F ON F.id = s.id ";
            //Consutal += "WHERE e.clave_empleado IN (" + getListaUsuarios(rawPersonal.Select(x => x.id)) + ");// AND  e.cc_contable IN ('" + centro_costosObj + "')";
            //string centro_costos = "SELECT descripcion FROM cc WHERE cc = '" + centro_costosObj + "';";

            //var resultado = (IList<economicoDTO>)ContextEnKontrolNominaArrendadora.Where(centro_costos, tipoNominda).ToObject<IList<economicoDTO>>();

            //var lstPuestosEnkontrol = (List<PuestosDTO>)ContextEnKontrolNominaArrendadora.Where(Consutal, tipoNominda).ToObject<List<PuestosDTO>>();

            var resultado = _context.Select<PuestosDTO>(new DapperDTO
            {
                baseDatos = (int)vSesiones.sesionEmpresaActual == (int)EmpresaEnum.Construplan ? MainContextEnum.Construplan : MainContextEnum.Arrendadora,
                consulta = @"SELECT descripcion FROM cc WHERE cc = '" + centro_costosObj + "';",
            });

            var lstPuestosEnkontrol = _context.Select<PuestosDTO>(new DapperDTO
            {
                baseDatos = (int)vSesiones.sesionEmpresaActual == (int)EmpresaEnum.Construplan ? MainContextEnum.Construplan : MainContextEnum.Arrendadora,
                consulta = @"SELECT p.puesto,p.descripcion,e.clave_empleado AS personalID, s.salario_base AS salarioBase, s.complemento AS salarioComplemento, 
                                    e.nombre,ape_paterno,e.ape_materno 
                                FROM tblRH_EK_Empleados AS e 
                                INNER JOIN tblRH_EK_Puestos as p on e.puesto=p.puesto 
                                INNER JOIN tblRH_EK_Tabulador_Historial AS s ON e.clave_empleado=s.clave_empleado 
                                INNER JOIN (SELECT  MAX(id) AS id ,clave_empleado FROM tblRH_EK_Tabulador_Historial GROUP BY (clave_empleado)) AS F ON F.id = s.id 
                                WHERE e.clave_empleado IN (" + getListaUsuarios(rawPersonal.Select(x => x.id)) + ");// AND  e.cc_contable IN ('" + centro_costosObj + "')",
            });

            List<rptMaquinariaHorasHombre> listaMaquinas = new List<rptMaquinariaHorasHombre>();
            foreach (var maquina in Maquinaria)
            {


                var ListaOTMAquinaria = rawDatOT.Where(x => x.OrdenTrabajo.EconomicoID == maquina.id).ToList();

                var PersonalMaquina = ListaOTMAquinaria.GroupBy(x => x.PersonalID).Select(y => new
                {
                    id = y.FirstOrDefault().PersonalID
                }).ToList();
                var PersontalString = PersonalMaquina.Select(y => y.id).ToList();

                var FiltroPersonal = lstPuestosEnkontrol.Where(x => PersontalString.Contains(x.personalID)).ToList();
                var ListaPuestosOT = FiltroPersonal.Select(x => new { x.puesto, x.descripcion }).Distinct().ToList();



                foreach (var puesto in ListaPuestosOT)
                {
                    rptMaquinariaHorasHombre objResult = new rptMaquinariaHorasHombre();
                    var inforPersonal = FiltroPersonal.Where(x => x.puesto == puesto.puesto).ToList();
                    var listaPuestosID = inforPersonal.Select(x => x.personalID).ToList();

                    var objOT = ListaOTMAquinaria.Where(x => listaPuestosID.Contains(x.PersonalID)).Distinct().ToList();

                    var otRawPuesto = ListaOTMAquinaria.Select(x => x.OrdenTrabajo).ToList();
                    var horasXPuesto = regresarTotalHoras(objOT);
                    var cantidadOT = otRawPuesto.Count;
                    var costoxHora = (inforPersonal.Sum(x => x.salarioBase) / inforPersonal.Count) / 55;
                    var CostoTotal = Math.Round(costoxHora * horasXPuesto, 2);
                    objResult.Puesto = puesto.descripcion;
                    objResult.puestoID = puesto.puesto;
                    objResult.cantidadOT = objOT.Count;
                    objResult.totalCostoXhora = Math.Round(CostoTotal, 2);
                    objResult.economico = maquina.noEconomico;
                    objResult.centroCostos = cc;
                    objResult.nombreCC = resultado.Select(x => x.descripcion).FirstOrDefault();
                    objResult.costoXhora = costoxHora;
                    objResult.horasXPuesto = horasXPuesto;

                    listaMaquinas.Add(objResult);

                }

            }
            HttpContext.Current.Session["HoraHombreENmaquinaria"] = listaMaquinas.ToList();
        }
    }
}
