using Core.DAO.RecursosHumanos.Reportes;
using Core.DTO;
using Core.DTO.RecursosHumanos;
using Core.DTO.Utils.Data;
using Core.Entity.Principal.Usuarios;
using Core.Entity.RecursosHumanos.Catalogo;
using Core.Enum.Multiempresa;
using Core.Enum.Principal;
using Data.EntityFramework.Context;
using Data.EntityFramework.Generic;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.DAO.RecursosHumanos.Reportes
{
    public class IncidenciasDAO : GenericDAO<tblP_Usuario>, IIncidencias
    {

        public List<IncidenciasDTO> getLstIncidencias(IncidenciasDTO objBuscar)
        {
            List<IncidenciasDTO> lista = new List<IncidenciasDTO>();

            try {
                //string Query = "SELECT Det.clave_empleado,(Emp.Nombre+' '+Emp.ape_paterno+' '+Emp.ape_materno) AS Nombre, dia1,dia2, dia3,dia4, dia5,dia6, dia7,dia8, dia9,dia10, dia11,dia12, dia13,dia14, dia15,dia16, Glo.cc,CC.descripcion As ccNombre, anio, Glo.periodo, ap.fecha_inicial, ap.fecha_final";
                //Query += " FROM DBA.sn_incidencias_empl_det AS Det";
                //Query += " INNER JOIN DBA.sn_incidencias_empl AS Glo";
                //Query += " ON Glo.id_incidencia = Det.id_incidencia";
                //Query += " INNER JOIN DBA.cc AS CC";
                //Query += " ON CC.cc = Glo.cc";
                //Query += " INNER JOIN DBA.sn_empleados AS Emp";
                //Query += " ON Emp.clave_empleado= Det.clave_empleado";
                //Query += " INNER JOIN DBA.sn_periodos AS ap ON Glo.anio=ap.year AND Glo.periodo = ap.periodo AND Glo.tipo_nomina = ap.tipo_nomina";

                string Query = @"SELECT 
	                                Det.clave_empleado,(Emp.Nombre+' '+Emp.ape_paterno+' '+Emp.ape_materno) AS Nombre, dia1,dia2, dia3,dia4, dia5,dia6, dia7,dia8, dia9,dia10, dia11,dia12, dia13,dia14, dia15,dia16,
	                                Glo.cc,CC.ccDescripcion As ccNombre, anio, Glo.periodo, ap.fecha_inicial, ap.fecha_final
                                FROM tblRH_EK_Incidencias_Empl_Det AS Det
                                INNER JOIN tblRH_EK_Incidencias_Empl AS Glo ON Glo.id_incidencia = Det.id_incidencia
                                INNER JOIN tblC_Nom_CatalogoCC AS CC ON CC.cc = Glo.cc
                                INNER JOIN tblRH_EK_Empleados AS Emp ON Emp.clave_empleado= Det.clave_empleado
                                INNER JOIN tblRH_EK_Periodos AS ap ON Glo.anio=ap.year AND Glo.periodo = ap.periodo AND Glo.tipo_nomina = ap.tipo_nomina";

                if (objBuscar.clave_empleado > 0)
                {
                    Query += " WHERE Det.clave_empleado='" + objBuscar.clave_empleado + "' AND Glo.estatus = 'A'";

                    if (!string.IsNullOrEmpty(objBuscar.cc))
                    {
                        Query += " AND Glo.cc = '" + objBuscar.cc + "'";
                    }

                }

                else
                {
                    Query += " WHERE (Glo.cc = '" + objBuscar.cc + "' OR  Det.clave_empleado='" + objBuscar.clave_empleado + "') AND Glo.estatus = 'A'";
                }
            
            
            

                if(objBuscar.anio > 0)
                {
                    Query += " AND Glo.anio = '" + objBuscar.anio + "'";
                }

                if (objBuscar.periodo > 0)
                {
                    Query += " AND Glo.periodo = '" + objBuscar.periodo + "' AND Glo.tipo_nomina='"+objBuscar.nomina+"'";
                }

                var resultado = (IList<IncidenciasDTO>)ContextEnKontrolNomina.Where(Query).ToObject<IList<IncidenciasDTO>>();

                foreach (var item in resultado)
                {
                    lista.Add(item);

                }
            }

            catch{

            }
            return lista;
        }

        public List<IncidenciasDTO> getLstIncidencias2Fechas(IncidenciasDTO objBuscar)
        {
            List<IncidenciasDTO> lista = new List<IncidenciasDTO>();

            try
            {
                //string Query = "SELECT Det.clave_empleado,(Emp.Nombre+' '+Emp.ape_paterno+' '+Emp.ape_materno) AS Nombre, dia1,dia2, dia3,dia4, dia5,dia6, dia7,dia8, dia9,dia10, dia11,dia12, dia13,dia14, dia15,dia16, Glo.cc,CC.descripcion As ccNombre, anio, Glo.periodo, ap.fecha_inicial, ap.fecha_final";
                //Query += " FROM DBA.sn_incidencias_empl_det AS Det";
                //Query += " INNER JOIN DBA.sn_incidencias_empl AS Glo";
                //Query += " ON Glo.id_incidencia = Det.id_incidencia";
                //Query += " INNER JOIN DBA.cc AS CC";
                //Query += " ON CC.cc = Glo.cc";
                //Query += " INNER JOIN DBA.sn_empleados AS Emp";
                //Query += " ON Emp.clave_empleado= Det.clave_empleado";
                //Query += " INNER JOIN DBA.sn_periodos AS ap ON Glo.anio=ap.year AND Glo.periodo = ap.periodo AND Glo.tipo_nomina = ap.tipo_nomina";

                string Query = @"SELECT Det.clave_empleado,(Emp.Nombre+' '+Emp.ape_paterno+' '+Emp.ape_materno) AS Nombre, Det.dia1, Det.dia2, Det.dia3, Det.dia4, Det.dia5, Det.dia6, Det.dia7, Det.dia8, Det.dia9, Det.dia10, Det.dia11, Det.dia12, Det.dia13, Det.dia14, Det.dia15, Det.dia16, Glo.cc,CC.ccDescripcion As ccNombre, anio, Glo.periodo, ap.fecha_inicial, ap.fecha_final, Glo.tipo_nomina
                                 FROM tblRH_BN_Incidencia_det AS Det
                                 INNER JOIN tblRH_BN_Incidencia AS Glo
                                 ON Glo.id = Det.incidenciaID
                                 INNER JOIN tblC_Nom_CatalogoCC AS CC
                                 ON CC.cc = Glo.cc
                                 INNER JOIN tblRH_EK_Empleados AS Emp
                                 ON Emp.clave_empleado = Det.clave_empleado
                                 INNER JOIN tblRH_EK_Periodos AS ap ON Glo.anio=ap.year AND Glo.periodo = ap.periodo AND Glo.tipo_nomina = ap.tipo_nomina";


                if (objBuscar.clave_empleado > 0)
                {
                    Query += " WHERE Det.clave_empleado='" + objBuscar.clave_empleado + "' AND Glo.estatus = 'A'";

                    if (!string.IsNullOrEmpty(objBuscar.cc))
                    {
                        Query += " AND Glo.cc = '" + objBuscar.cc + "'";
                    }

                }

                else
                {
                    Query += " WHERE Glo.cc = '" + objBuscar.cc + "'  AND Glo.estatus = 'A' ";
                }


                Query += " AND (ap.fecha_inicial >='" + objBuscar.fecha_inicial.ToString("yyyyMMdd") + "' AND ap.fecha_final <='" + objBuscar.fecha_final.ToString("yyyyMMdd") + "')";

                //if (objBuscar.fecha_inicial.Year > 0 && objBuscar.fecha_final.Year > 0)
                //{
                    
                //}
                //var semanaInicio = numeroSemana(objBuscar.fecha_inicial);
                //var semanaFinal = numeroSemana(objBuscar.fecha_final);
                //if (semanaInicio > 0 && semanaFinal > 0)
                //{
                //    Query += " AND Glo.periodo >='" + semanaInicio + "' AND Glo.periodo <='" + semanaFinal + "' AND Glo.tipo_nomina='" + objBuscar.nomina + "'";
                //}

                //BIEJON AU
                  //if (vSesiones.sesionEmpresaActual == 1)
                  //{
                  //    var resultado = (IList<IncidenciasDTO>)ContextEnKontrolNomina.Where(Query).ToObject<IList<IncidenciasDTO>>();

                  //    foreach (var item in resultado)
                  //    {
                  //        lista.Add(item);

                  //    }
                  //}
                  //else
                  //{
                  //    var resultado = (IList<IncidenciasDTO>)ContextEnKontrolNominaArrendadora.Where(Query,2).ToObject<IList<IncidenciasDTO>>();

                  //    foreach (var item in resultado)
                  //    {
                  //        lista.Add(item);

                  //    }
                  //}

                var resultado = _context.Select<IncidenciasDTO>(new DapperDTO 
                {
                    baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
                    consulta = Query,            
                });

                lista.AddRange(resultado);

            }

            catch
            {
                
            }
            return lista;
        }

        public List<int> CatAnios()
        {
            string Query = "SELECT DISTINCT year FROM DBA.sn_periodos";

            var resultado = ContextEnKontrolNomina.Where(Query);

            List<int> lista = new List<int>();

            foreach (var item in resultado)
            {
                lista.Add(Convert.ToInt32(item["year"].Value));

            }

            return lista;

        }

        public List<CatIncidencias> CatIncidencia()
        {
            string Query = "SELECT id, concepto FROM DBA.sn_incidencias_conceptos";

            var resultado = (IList<CatIncidencias>)ContextEnKontrolNomina.Where(Query).ToObject<IList<CatIncidencias>>();

            List<CatIncidencias> lista = new List<CatIncidencias>();

            foreach (var item in resultado)
            {
                lista.Add(item);

            }

            return lista;

        }

        public List<string> CatPeriodo(int anio, string cc)
        {

            string Query = null;
            if (!string.IsNullOrEmpty(cc)) { Query = "SELECT periodo, tipo_nomina FROM DBA.sn_incidencias_empl where anio = '" + anio + "' and cc='" + cc + "' AND estatus = 'A' Order by periodo, tipo_nomina"; }
            else { Query = "SELECT DISTINCT periodo, tipo_nomina FROM DBA.sn_incidencias_empl where anio = '" + anio + "' AND estatus = 'A' Order by periodo, tipo_nomina"; }

            var resultado = ContextEnKontrolNomina.Where(Query);

            List<string> lstRs = new List<string>();


            foreach (var item in resultado)
            {
                var obj = item.tipo_nomina > 1 ? "" + item.periodo + "-Quincenal" : "" + item.periodo + "-Semanal" ;
                lstRs.Add(obj);
            }



            return lstRs;
        }

        public List<int> CatDias()
        {
 	        throw new NotImplementedException();
        }

        int numeroSemana(DateTime time)
        {
            DayOfWeek day = CultureInfo.InvariantCulture.Calendar.GetDayOfWeek(time);
            if (day >= DayOfWeek.Monday && day <= DayOfWeek.Wednesday)
            {
                time = time.AddDays(3);
            }
            return CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(time, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);

        }
    }
}
