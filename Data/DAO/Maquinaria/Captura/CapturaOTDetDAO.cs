using Core.DTO;
using Core.DTO.Maquinaria.Reporte;
using Core.DTO.Utils.Data;
using Core.Entity.Maquinaria.Captura;
using Core.Entity.RecursosHumanos.Captura;
using Core.Entity.RecursosHumanos.Catalogo;
using Core.Enum.Multiempresa;
using Core.Enum.Principal;
using Core.Enum.Principal.Bitacoras;
using Data.EntityFramework.Context;
using Data.EntityFramework.Generic;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.DAO.Maquinaria.Captura
{
    public class CapturaOTDetDAO : GenericDAO<tblM_DetOrdenTrabajo>, ICapturaOTDetDAO
    {
        public void Guardar(List<tblM_DetOrdenTrabajo> obj, int idOT)
        {
            foreach (var dato in obj)
            {
                dato.OrdenTrabajoID = idOT;

                if (dato.id == 0)
                    SaveEntity(dato, (int)BitacoraEnum.CAPTURAOTDET);
                else
                    Update(obj, dato.id, (int)BitacoraEnum.CAPTURAOTDET);
            }
        }

        public List<tblM_DetOrdenTrabajo> getListaOTDet(int id)
        {
            return _context.tblM_DetOrdenTrabajo.Where(x => x.OrdenTrabajoID.Equals(id)).ToList();
        }

        public tblRH_CatEmpleados getCatEmpleados(string term)
        {
            List<tblRH_CatEmpleados> lstCatEmpleado = new List<tblRH_CatEmpleados>();

            //var getCatEmpleado = "SELECT clave_empleado, (LTRIM(RTRIM(emp.nombre))+' '+replace(emp.ape_paterno, ' ', '')+' '+replace(emp.ape_materno, ' ', '')) AS " +
            //    "Nombre,pu.descripcion AS puesto, emp.cc_contable as CC FROM DBA.sn_empleados as emp " +
            //    " inner join si_puestos as pu on emp.puesto = pu.puesto " +
            //    "WHERE clave_empleado = '" + term + "';";

            try
            {
                //var resultado = (IList<tblRH_CatEmpleados>)ContextEnKontrolNominaArrendadora.Where(getCatEmpleado, 1).ToObject<IList<tblRH_CatEmpleados>>();

                var resultado = _context.Select<tblRH_CatEmpleados>(new DapperDTO
                {
                    baseDatos = MainContextEnum.Construplan,
                    consulta = @"SELECT clave_empleado, (LTRIM(RTRIM(emp.nombre))+' '+replace(emp.ape_paterno, ' ', '')+' '+replace(emp.ape_materno, ' ', '')) AS Nombre,pu.descripcion AS puesto, emp.cc_contable as CC 
                                FROM tblRH_EK_Empleados as emp 
                                inner join tblRH_EK_Puestos as pu on emp.puesto = pu.puesto
                                WHERE clave_empleado = '" + term + "';",
                });

                return resultado.FirstOrDefault();

            }
            catch
            {

            }
            try
            {
                //var resultado = (IList<tblRH_CatEmpleados>)ContextEnKontrolNominaArrendadora.Where(getCatEmpleado, 2).ToObject<IList<tblRH_CatEmpleados>>();

                var resultado = _context.Select<tblRH_CatEmpleados>(new DapperDTO
                {
                    baseDatos = MainContextEnum.Arrendadora,
                    consulta = @"SELECT clave_empleado, (LTRIM(RTRIM(emp.nombre))+' '+replace(emp.ape_paterno, ' ', '')+' '+replace(emp.ape_materno, ' ', '')) AS Nombre,pu.descripcion AS puesto, emp.cc_contable as CC 
                                FROM tblRH_EK_Empleados as emp 
                                inner join tblRH_EK_Puestos as pu on emp.puesto = pu.puesto
                                WHERE clave_empleado = '" + term + "';",
                });

                return resultado.FirstOrDefault();

            }
            catch
            {
                return null;
            }
        }

        public void delete(int id)
        {

            try
            {
                tblM_DetOrdenTrabajo entidad = _context.tblM_DetOrdenTrabajo.FirstOrDefault(x => x.id.Equals(id));

                Delete(entidad, 33);
            }
            catch (Exception)
            {
                throw new Exception("Error al eliminar el registro");
            }

        }

        public List<RepGastosMaquinariaGrid> GetCostosHoraHombre(int EconomicoID, DateTime FechaInicio, DateTime FechaFin)
        {
            var OrdenTrabajo = _context.tblM_CapOrdenTrabajo.Where(c => c.EconomicoID == EconomicoID && (c.FechaEntrada >= FechaInicio && c.FechaEntrada <= FechaFin));
            RepGastosMaquinariaGrid repGastosMaquinariaGrid = new RepGastosMaquinariaGrid();
            List<RepGastosMaquinariaGrid> result = new List<RepGastosMaquinariaGrid>();

            repGastosMaquinariaGrid.importe = totalCosto(OrdenTrabajo.Select(c => c.id).ToList()).ToString();
            repGastosMaquinariaGrid.descripcion = "COSTOS MANO DE OBRA.";
            repGastosMaquinariaGrid.tipoInsumo = 16;

            result.Add(repGastosMaquinariaGrid);

            return result;
        }

        public List<RepGastosMaquinariaGrid> FillMotivosParo(RepGastosFiltrosDTO obj)
        {

            int EconomicoID = _context.tblM_CatMaquina.FirstOrDefault(m => m.noEconomico == obj.maq).id;
            DateTime FechaInicio = Convert.ToDateTime(obj.fechaInicio, CultureInfo.InvariantCulture);
            DateTime FechaFin = Convert.ToDateTime(obj.fechaFin, CultureInfo.InvariantCulture);


            FechaInicio = new DateTime(FechaInicio.Year, (obj.mesID), FechaInicio.Day);
            FechaFin = new DateTime(FechaFin.Year, (obj.mesID), DateTime.DaysInMonth(FechaInicio.Year, FechaInicio.Month));

            var OrdenTrabajo = _context.tblM_CapOrdenTrabajo.Where(c => c.EconomicoID == EconomicoID && (c.FechaEntrada >= FechaInicio && c.FechaEntrada <= FechaFin)).OrderBy(x => x.MotivoParo).ToList();

            List<RepGastosMaquinariaGrid> result = new List<RepGastosMaquinariaGrid>();

            var GruposOT = OrdenTrabajo.GroupBy(x => x.MotivoParo).Select(c => new { motivoParo = c.Key, id = c.Select(y => y.id) });


            foreach (var grupo in GruposOT)
            {
                RepGastosMaquinariaGrid repGastosMaquinariaGrid = new RepGastosMaquinariaGrid();
                decimal PrecioHH = 0;
                foreach (var item2 in grupo.id)
                {
                    var otDet = _context.tblM_DetOrdenTrabajo.Where(x => x.OrdenTrabajoID == item2).ToList();
                    foreach (var item in otDet)
                    {
                        var infoEmpleadoObj = InfEmpleado(item.PersonalID).FirstOrDefault();

                        if (infoEmpleadoObj != null)
                        {
                            var SueldoBase = infoEmpleadoObj.Salario_Base;
                            var Complemento = infoEmpleadoObj.Complemento;
                            var CostoHora = (SueldoBase + Complemento) / 55;
                            var restaHoras = item.HoraFin - item.HoraInicio;
                            var TotalHoras = Convert.ToDecimal(restaHoras.TotalHours);
                            var PrecioHHS = TotalHoras * CostoHora;

                            PrecioHH += PrecioHHS;
                        }
                    }
                }


                var Descripcion = _context.tblM_CatCriteriosCausaParo.FirstOrDefault(tp => tp.id == grupo.motivoParo);
                if (Descripcion != null)
                {
                    repGastosMaquinariaGrid.importe = PrecioHH.ToString();
                    repGastosMaquinariaGrid.descripcion = Descripcion.CausaParo;
                    repGastosMaquinariaGrid.tipoInsumo = grupo.motivoParo;
                    result.Add(repGastosMaquinariaGrid);
                }

            }



            return result;
        }

        public List<RepGastosMaquinariaGrid> FillUsuario(RepGastosFiltrosDTO obj)
        {
            int EconomicoID = _context.tblM_CatMaquina.FirstOrDefault(m => m.noEconomico == obj.maq).id;
            DateTime FechaInicio = Convert.ToDateTime(obj.fechaInicio, CultureInfo.InvariantCulture);
            DateTime FechaFin = Convert.ToDateTime(obj.fechaFin, CultureInfo.InvariantCulture);

            FechaInicio = new DateTime(FechaInicio.Year, (obj.mesID), FechaInicio.Day);
            FechaFin = new DateTime(FechaFin.Year, (obj.mesID), DateTime.DaysInMonth(FechaInicio.Year, FechaInicio.Month));

            var Lista = _context.tblM_DetOrdenTrabajo.Where(c => c.OrdenTrabajo.MotivoParo == obj.idGrupo && c.OrdenTrabajo.EconomicoID == EconomicoID && (c.OrdenTrabajo.FechaEntrada >= FechaInicio && c.OrdenTrabajo.FechaEntrada <= FechaFin)).ToList();

            List<RepGastosMaquinariaGrid> result = new List<RepGastosMaquinariaGrid>();

            foreach (var item in Lista)
            {
                RepGastosMaquinariaGrid repGastosMaquinariaGrid = new RepGastosMaquinariaGrid();
                decimal PrecioHH = 0;

                var infoEmpleadoObj = InfEmpleado(item.PersonalID).FirstOrDefault();

                if (infoEmpleadoObj != null)
                {
                    var SueldoBase = infoEmpleadoObj.Salario_Base;
                    var Complemento = infoEmpleadoObj.Complemento;
                    var CostoHora = (SueldoBase + Complemento) / 55;
                    var restaHoras = item.HoraFin - item.HoraInicio;
                    var TotalHoras = Convert.ToDecimal(restaHoras.TotalHours);
                    var PrecioHHS = TotalHoras * CostoHora;

                    PrecioHH += PrecioHHS;

                    repGastosMaquinariaGrid.importe = PrecioHH.ToString();
                    repGastosMaquinariaGrid.descripcion = infoEmpleadoObj.Nombre;
                    repGastosMaquinariaGrid.tipoInsumo = obj.idTipo;

                    var Fecha = _context.tblM_CapOrdenTrabajo.Where(x => x.id == item.OrdenTrabajoID).FirstOrDefault();
                    if (Fecha != null)
                    {
                        repGastosMaquinariaGrid.fecha = Fecha.FechaEntrada.ToShortDateString();
                    }
                    else
                    {
                        repGastosMaquinariaGrid.fecha = "";
                    }
                    result.Add(repGastosMaquinariaGrid);
                }

            }

            return result;
        }



        private decimal totalCosto(List<int> listaOrdenTrabajoID)
        {
            var detOrdenTrabajo = _context.tblM_DetOrdenTrabajo.Where(x => listaOrdenTrabajoID.Contains(x.OrdenTrabajoID));

            decimal PrecioHH = 0;
            foreach (var item in detOrdenTrabajo)
            {

                var infoEmpleadoObj = InfEmpleado(item.PersonalID).FirstOrDefault();

                if (infoEmpleadoObj != null)
                {
                    var SueldoBase = infoEmpleadoObj.Salario_Base;
                    var Complemento = infoEmpleadoObj.Complemento;

                    var CostoHora = (SueldoBase + Complemento) / 55;

                    var restaHoras = item.HoraFin - item.HoraInicio;
                    var TotalHoras = Convert.ToDecimal(restaHoras.TotalHours);

                    var PrecioHHS = TotalHoras * CostoHora;

                    PrecioHH += PrecioHHS;
                }

            }

            return PrecioHH;
        }

        private List<tblRH_FormatoCambio> InfEmpleado(int idEmpleado)
        {

            try
            {
                //string inf_Empleado = "SELECT Top 1 s.bono_zona As Bono, ";
                //inf_Empleado += "e.clave_empleado,e.nombre,ape_paterno,e.ape_materno,e.fecha_antiguedad as fecha_alta,e.puesto as 'puestoID',p.descripcion as 'puesto',";
                //inf_Empleado += "e.tipo_nomina as 'tipoNominaID',";
                //inf_Empleado += "tn.descripcion as 'tipoNomina',";
                //inf_Empleado += "e.cc_contable as 'ccID',";
                //inf_Empleado += "c.descripcion as 'cc',";
                //inf_Empleado += "e.id_regpat as 'registroPatronalID',";
                //inf_Empleado += "r.nombre_corto as 'registroPatronal',";
                //inf_Empleado += "e.jefe_inmediato as 'clave_jefe_inmediato',";
                //inf_Empleado += "(Select (e2.nombre+' '+e2.ape_paterno+' '+e2.ape_materno) from DBA.sn_empleados as e2 where e2.clave_empleado = e.jefe_inmediato)as 'nombre_jefe_inmediato',";
                //inf_Empleado += "s.salario_base,";
                //inf_Empleado += "s.complemento ";
                //inf_Empleado += "FROM DBA.sn_empleados as e";
                //inf_Empleado += " inner join DBA.si_puestos as p on e.puesto=p.puesto";
                //inf_Empleado += " inner join DBA.sn_tipos_nomina as tn on e.tipo_nomina=tn.tipo_nomina";
                //inf_Empleado += " inner join DBA.cc as c on e.cc_contable=c.cc";
                //inf_Empleado += " inner join DBA.sn_registros_patronales as r on e.id_regpat=r.clave_reg_pat";
                //inf_Empleado += " inner join DBA.sn_tabulador_historial as s on e.clave_empleado=s.clave_empleado";
                //inf_Empleado += " where e.clave_empleado=" + idEmpleado + "AND e.estatus_empleado ='A' ";
                //inf_Empleado += " order by s.id DESC";

                //var resultado = (List<tblRH_FormatoCambio>)ContextEnKontrolNomina.Where(inf_Empleado).ToObject<List<tblRH_FormatoCambio>>();

                var resultado = _context.Select<tblRH_FormatoCambio>(new DapperDTO
                {
                    baseDatos = (int)vSesiones.sesionEmpresaActual == (int)EmpresaEnum.Construplan ? MainContextEnum.Construplan : MainContextEnum.Arrendadora,
                    consulta = @"SELECT Top 1 s.bono_zona As Bono, 
                                    e.clave_empleado,e.nombre,ape_paterno,e.ape_materno,e.fecha_antiguedad as fecha_alta,e.puesto as 'puestoID',p.descripcion as 'puesto',
                                    e.tipo_nomina as 'tipoNominaID',
                                    tn.descripcion as 'tipoNomina',
                                    e.cc_contable as 'ccID',
                                    c.descripcion as 'cc',
                                    e.id_regpat as 'registroPatronalID',
                                    r.nombre_corto as 'registroPatronal',
                                    e.jefe_inmediato as 'clave_jefe_inmediato',
                                    (Select (e2.nombre+' '+e2.ape_paterno+' '+e2.ape_materno) from tblRH_EK_Empleados as e2 where e2.clave_empleado = e.jefe_inmediato)as 'nombre_jefe_inmediato',
                                    s.salario_base,
                                    s.complemento 
                                FROM tblRH_EK_Empleados as e
                                inner join tblRH_EK_Puestos as p on e.puesto=p.puesto
                                inner join tblRH_EK_Tipos_Nomina as tn on e.tipo_nomina=tn.tipo_nomina
                                inner join tblP_CC as c on e.cc_contable=c.cc
                                inner join tblRH_EK_Registros_Patronales as r on e.id_regpat=r.clave_reg_pat
                                inner join tblRH_EK_Tabulador_Historial as s on e.clave_empleado=s.clave_empleado
                                where e.clave_empleado=" + idEmpleado + "AND e.estatus_empleado ='A' order by s.id DESC",
                });

                return resultado;
            }
            catch (Exception)
            {
                List<tblRH_FormatoCambio> tblRH_FormatoCambioD = new List<tblRH_FormatoCambio>();

                return tblRH_FormatoCambioD;
            }
        }
        public byte[] obtenerImagen(int id, int tipoEvidencia)
        {
            try
            {
                byte[] obtenerByte = new byte[0];
                var obj = _context.tblBL_Evidencias.Where(r => r.idBL == id && r.tipoEvidencia == tipoEvidencia && r.esActivo).FirstOrDefault();
                if (obj != null)
                {
                    var rutaImagen = _context.tblBL_Evidencias.Where(r => r.idBL == id && r.tipoEvidencia == tipoEvidencia && r.esActivo).Select(y => y.rutaArchivo).FirstOrDefault();
                    Image imagen = Image.FromFile(rutaImagen);
                    obtenerByte = ImageToByteArray(imagen);
                    return obtenerByte;
                }
                else
                {
                    var rutaImagen = _context.tblBL_Evidencias.Where(x => x.idBL == 0).Select(s => s.rutaArchivo).FirstOrDefault();
                    Image imagen = Image.FromFile(rutaImagen);
                    obtenerByte = ImageToByteArray(imagen);
                    return obtenerByte;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public List<byte[]> obtenerImagenLista(int id, int tipoEvidencia)
        {
            byte[] obtenerByte = new byte[0];
            List<byte[]> lstObtenerByte = new List<byte[]>();
            try
            {
                var obj = _context.tblBL_Evidencias.Where(r => r.idBL == id && r.tipoEvidencia == tipoEvidencia && r.esActivo).FirstOrDefault();
                if (obj != null)
                {
                    int contador = 0;
                    List<string> lstEvidencias = _context.tblBL_Evidencias.Where(r => r.idBL == id && r.tipoEvidencia == tipoEvidencia && r.esActivo).Select(y => y.rutaArchivo).ToList();
                    foreach (var item in lstEvidencias)
                    {
                        if (!string.IsNullOrEmpty(item) && contador > 0)
                        {
                            string rutaImagen = item;
                            Image imagen = Image.FromFile(rutaImagen);
                            obtenerByte = new byte[0];
                            obtenerByte = ImageToByteArray(imagen);
                            lstObtenerByte.Add(obtenerByte);
                        }
                        contador++;
                    }
                }
                else
                {
                    var rutaImagen = _context.tblBL_Evidencias.Where(x => x.idBL == 0).Select(s => s.rutaArchivo).FirstOrDefault();
                    Image imagen = Image.FromFile(rutaImagen);
                    obtenerByte = new byte[0];
                    obtenerByte = ImageToByteArray(imagen);
                    lstObtenerByte.Add(obtenerByte);
                }
            }
            catch (Exception ex)
            {
                LogError(0, 0, "CapturaOTDetDAO", "obtenerImagenLista", ex, AccionEnum.CONSULTA, 0, new { id = id, tipoEvidencia = tipoEvidencia });
                return lstObtenerByte;
            }
            return lstObtenerByte;
        }

        public byte[] ImageToByteArray(Image imageIn)
        {
            try
            {
                using (var ms = new MemoryStream())
                {
                    imageIn.Save(ms, imageIn.RawFormat);
                    return ms.ToArray();
                }
            }
            catch (Exception ex)
            {

                throw;
            }
        }
    }
}