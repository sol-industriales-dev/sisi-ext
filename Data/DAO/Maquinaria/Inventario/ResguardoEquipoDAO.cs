using Core.DAO.Maquinaria.Inventario;
using Core.DTO.RecursosHumanos;
using Core.Entity.Maquinaria.Inventario;
using Core.Entity.RecursosHumanos.Catalogo;
using Core.Enum.Principal.Bitacoras;
using Data.EntityFramework.Context;
using Data.EntityFramework.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Entity.Principal.Usuarios;
using Core.Entity.Maquinaria.Catalogo;
using Core.DTO.Utils.Data;
using Core.Enum.Multiempresa;
using Core.DTO;
using Core.Enum.Principal;
using Core.Entity.Principal.Multiempresa;
using Core.Enum.Administracion.Seguridad.Requerimientos;
using Core.Entity.Principal.Alertas;

namespace Data.DAO.Maquinaria.Inventario
{

    public class ResguardoEquipoDAO : GenericDAO<tblM_ResguardoVehiculosServicio>, IResguardoEquipoDAO
    {
        public List<tblM_CatPreguntaResguardoVehiculo> GetListaPreguntas()
        {
            return _context.tblM_CatPreguntaResguardoVehiculo.Where(x => x.Estatus.Equals(true)).ToList();
        }
        public List<tblRH_CatEmpleados> getCatEmpleados(string term, List<string> CentroCostos)
        {
            List<tblRH_CatEmpleados> lstCatEmpleado = new List<tblRH_CatEmpleados>();

            //var getCatEmpleado = "SELECT top 10 clave_empleado, (LTRIM(RTRIM(nombre))+' '+replace(ape_paterno, ' ', '')+' '+replace(ape_materno, ' ', '')) AS Nombre, puesto, cc_contable as CC FROM DBA.sn_empleados WHERE replace((nombre+' '+ape_paterno+' '+ape_materno), ' ', '') LIKE '%" + term.Replace(" ", "") + "%' AND CC_Contable in (" + GetListaCCString(CentroCostos) + ")";
            //MIGRADO ↓
            //var getCatEmpleado = "SELECT top 10 clave_empleado, (LTRIM(RTRIM(nombre))+' '+replace(ape_paterno, ' ', '')+' '+replace(ape_materno, ' ', '')) AS Nombre, puesto, cc_contable as CC FROM DBA.sn_empleados WHERE estatus_empleado='A' and replace((nombre+' '+ape_paterno+' '+ape_materno), ' ', '') LIKE '%" + term.Replace(" ", "") + "%'";

            try
            {
                //                try
                //                {
                //                //var resultado = (IList<tblRH_CatEmpleados>)ContextEnKontrolNominaArrendadora.Where(getCatEmpleado, 1).ToObject<IList<tblRH_CatEmpleados>>();

                //                    var resultado = _context.Select<tblRH_CatEmpleados>(new DapperDTO
                //                    {
                //                        baseDatos = MainContextEnum.Construplan,
                //                        consulta = @"SELECT top 10 clave_empleado, (LTRIM(RTRIM(nombre))+' '+replace(ape_paterno, ' ', '')+' '+replace(ape_materno, ' ', '')) AS Nombre, puesto, cc_contable as CC 
                //                                    FROM tblRH_EK_Empleados 
                //                                    WHERE estatus_empleado='A' and replace((nombre+' '+ape_paterno+' '+ape_materno), ' ', '') LIKE '%" + term.Replace(" ", "") + "%'",
                //                    });

                //                    foreach (var item in resultado)
                //                    {
                //                        lstCatEmpleado.Add(item);
                //                    }
                //                }
                //                catch{
                //                }
                //                try
                //                {
                //                    //var resultado2 = (IList<tblRH_CatEmpleados>)ContextEnKontrolNominaArrendadora.Where(getCatEmpleado, 2).ToObject<IList<tblRH_CatEmpleados>>();

                //                    var resultado2 = _context.Select<tblRH_CatEmpleados>(new DapperDTO
                //                    {
                //                        baseDatos = MainContextEnum.Arrendadora,
                //                        consulta = @"SELECT top 10 clave_empleado, (LTRIM(RTRIM(nombre))+' '+replace(ape_paterno, ' ', '')+' '+replace(ape_materno, ' ', '')) AS Nombre, puesto, cc_contable as CC 
                //                                    FROM tblRH_EK_Empleados 
                //                                    WHERE estatus_empleado='A' and replace((nombre+' '+ape_paterno+' '+ape_materno), ' ', '') LIKE '%" + term.Replace(" ", "") + "%'",
                //                    });

                //                    foreach (var item in resultado2.Where(x => !lstCatEmpleado.Select(y => y.clave_empleado).ToList().Contains(x.clave_empleado)))
                //                    {
                //                        lstCatEmpleado.Add(item);
                //                    }
                //                }
                //                catch { }
                //                lstCatEmpleado.Add(new tblRH_CatEmpleados { clave_empleado = 89898989, cc = "214", Nombre = "NARCIZO AGUILAR Y LUNA", Puesto = "GERENTE" });

                var empresas = _context.tblP_Empresas.Where(x => x.estatus).Select(x => x.id).OrderByDescending(x => x).First();

                for (int i = 1; i <= empresas; i++)
                {
                    var obtenerEmpleados = false;
                    switch ((MainContextEnum)i)
                    {
                        case MainContextEnum.Construplan:
                            obtenerEmpleados = true;
                            break;
                        case MainContextEnum.Arrendadora:
                            obtenerEmpleados = true;
                            break;
                        case MainContextEnum.Colombia:
                            obtenerEmpleados = true;
                            break;
                        case MainContextEnum.PERU:
                            obtenerEmpleados = true;
                            break;
                        case MainContextEnum.GCPLAN:
                            obtenerEmpleados = true;
                            break;
                        default:
                            obtenerEmpleados = false;
                            break;
                    }

                    if (obtenerEmpleados)
                    {
                        var resultado = _context.Select<tblRH_CatEmpleados>(new DapperDTO
                        {
                            baseDatos = (MainContextEnum)i,
                            consulta = @"SELECT top 10 clave_empleado, (LTRIM(RTRIM(nombre))+' '+replace(ape_paterno, ' ', '')+' '+replace(ape_materno, ' ', '')) AS Nombre, puesto, cc_contable as CC 
                                    FROM tblRH_EK_Empleados 
                                    WHERE estatus_empleado='A' and replace((nombre+' '+ape_paterno+' '+ape_materno), ' ', '') LIKE @parametro",
                            parametros = new { parametro = "%" + term.Replace(" ", "") + "%" }
                        });

                        foreach (var item in resultado)
                        {
                            lstCatEmpleado.Add(item);
                        }
                    }
                }

                var lstTemp = new List<tblRH_CatEmpleados>();
                foreach (var item in lstCatEmpleado.GroupBy(x => x.clave_empleado))
                {
                    lstTemp.Add(item.First());
                }

                lstCatEmpleado = lstTemp;
            }
            catch
            {
                return lstCatEmpleado;
            }

            return lstCatEmpleado;
        }


        public tblRH_CatEmpleados getCatEmpleado(string id)
        {
            List<tblRH_CatEmpleados> lstCatEmpleado = new List<tblRH_CatEmpleados>();

            //var getCatEmpleado = "SELECT a.clave_empleado, (LTRIM(RTRIM(a.nombre))+' '+replace(a.ape_paterno, ' ', '')+' '+replace(a.ape_materno, ' ', '')) AS Nombre, p.descripcion as 'puesto', p.puesto as cvePuesto," +
            //                     " cc_contable as CC FROM DBA.sn_empleados a inner join DBA.si_puestos as p on a.puesto=p.puesto WHERE estatus_empleado='A' and clave_empleado='" + id + "'";



            try
            {
                //var resultado = (IList<tblRH_CatEmpleados>)ContextEnKontrolNomina.Where(getCatEmpleado).ToObject<IList<tblRH_CatEmpleados>>();
                try
                {
                    //var resultado = (IList<tblRH_CatEmpleados>)ContextEnKontrolNominaArrendadora.Where(getCatEmpleado, 1).ToObject<IList<tblRH_CatEmpleados>>();

                    var resultado = _context.Select<tblRH_CatEmpleados>(new DapperDTO
                    {
                        baseDatos = MainContextEnum.Arrendadora,
                        consulta = @"SELECT a.clave_empleado, (LTRIM(RTRIM(a.nombre))+' '+replace(a.ape_paterno, ' ', '')+' '+replace(a.ape_materno, ' ', '')) AS Nombre, p.descripcion as 'puesto', p.puesto as cvePuesto,
                                    cc_contable as CC 
                                    FROM tblRH_EK_Empleados a inner join tblRH_EK_Puestos as p on a.puesto=p.puesto WHERE estatus_empleado='A' and clave_empleado='" + id + "'",
                    });

                    if (resultado.Count == 0)
                    {
                        var resultado2 = _context.Select<tblRH_CatEmpleados>(new DapperDTO
                        {
                            baseDatos = MainContextEnum.Construplan,
                            consulta = @"SELECT a.clave_empleado, (LTRIM(RTRIM(a.nombre))+' '+replace(a.ape_paterno, ' ', '')+' '+replace(a.ape_materno, ' ', '')) AS Nombre, p.descripcion as 'puesto', p.puesto as cvePuesto,
                                    cc_contable as CC 
                                    FROM tblRH_EK_Empleados a inner join tblRH_EK_Puestos as p on a.puesto=p.puesto WHERE estatus_empleado='A' and clave_empleado='" + id + "'",
                        });
                        resultado.AddRange(resultado2);

                        var resultado4 = _context.Select<tblRH_CatEmpleados>(new DapperDTO
                        {
                            baseDatos = MainContextEnum.GCPLAN,
                            consulta = @"SELECT a.clave_empleado, (LTRIM(RTRIM(a.nombre))+' '+replace(a.ape_paterno, ' ', '')+' '+replace(a.ape_materno, ' ', '')) AS Nombre, p.descripcion as 'puesto', p.puesto as cvePuesto,
                                    cc_contable as CC 
                                    FROM tblRH_EK_Empleados a inner join tblRH_EK_Puestos as p on a.puesto=p.puesto WHERE estatus_empleado='A' and clave_empleado='" + id + "'",
                        });
                        resultado.AddRange(resultado4);

                        if (resultado2.Count == 0 && resultado4.Count == 0)
                        {
                            var resultado3 = _context.Select<tblRH_CatEmpleados>(new DapperDTO
                               {
                                   baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
                                   consulta = @"SELECT a.clave_empleado, (LTRIM(RTRIM(a.nombre))+' '+replace(a.ape_paterno, ' ', '')+' '+replace(a.ape_materno, ' ', '')) AS Nombre, p.descripcion as 'puesto', p.puesto as cvePuesto,
                                            cc_contable as CC 
                                            FROM tblRH_EK_Empleados a inner join tblRH_EK_Puestos as p on a.puesto=p.puesto WHERE estatus_empleado='A' and clave_empleado='" + id + "'",
                               });
                            resultado.AddRange(resultado3);
                        }
                    }


                    return resultado.FirstOrDefault();
                }
                catch (Exception ex) { }
                try
                {
                    //var resultado2 = (IList<tblRH_CatEmpleados>)ContextEnKontrolNominaArrendadora.Where(getCatEmpleado, 2).ToObject<IList<tblRH_CatEmpleados>>();

                    var resultado2 = _context.Select<tblRH_CatEmpleados>(new DapperDTO
                    {
                        baseDatos = MainContextEnum.Arrendadora,
                        consulta = @"SELECT a.clave_empleado, (LTRIM(RTRIM(a.nombre))+' '+replace(a.ape_paterno, ' ', '')+' '+replace(a.ape_materno, ' ', '')) AS Nombre, p.descripcion as 'puesto', p.puesto as cvePuesto,
                                    cc_contable as CC 
                                    FROM tblRH_EK_Empleados a inner join tblRH_EK_Puestos as p on a.puesto=p.puesto WHERE estatus_empleado='A' and clave_empleado='" + id + "'",
                    });

                    return resultado2.FirstOrDefault();
                }
                catch (Exception ex) { }


            }
            catch
            {
                return null;
            }

            return null;
        }



        private string GetListaCCString(List<string> CentroCostos)
        {

            string cadenaString = "";

            foreach (string CC in CentroCostos)
            {
                cadenaString += "'" + CC + "',";
            }

            return cadenaString.TrimEnd(',');
        }

        public List<tblM_ResguardoVehiculosServicio> GetListaAutorizacionesPendientes(string cc, int obj)
        {

            switch ((EmpresaEnum)vSesiones.sesionEmpresaActual)
            {
                case EmpresaEnum.Colombia:
                    {
                        var ac = _context.tblP_CC.FirstOrDefault(x => x.cc == cc);
                        if (ac != null)
                        {
                            cc = ac.areaCuenta;
                        }
                    }
                    break;
            }
            var resguardo = _context.tblM_ResguardoVehiculosServicio.Where(x => x.Obra.Equals(cc) && (obj != 0 ? x.estado == obj : true)).ToList();

            return resguardo;
        }

        public string GetFechaVigenciaResguardo(int id)
        {
            var fechaVigencia = "";
            var registroResguardo = _context.tblM_ResguardoVehiculosServicio.FirstOrDefault(x => x.id == id);

            using (var _contextConstruplan = new MainContext(EmpresaEnum.Construplan))
            {
                var listaCursosManejo_id = _contextConstruplan.tblS_CapacitacionCursos.Where(x => x.isActivo && x.claveCurso == "SEG-PRT-044").Select(x => x.id).ToList(); //La clave SEG-PRT-044 es para los cursos de manejo seguro de vehículos.
                var listaControlesAsistencia = _contextConstruplan.tblS_CapacitacionControlAsistencia.Where(x =>
                    x.activo &&
                    listaCursosManejo_id.Contains(x.cursoID) &&
                    x.asistentes.Any(y => y.claveEmpleado == registroResguardo.noEmpleado)
                ).ToList();

                if (listaControlesAsistencia.Count() > 0)
                {
                    var ultimaCapacitacion = listaControlesAsistencia.OrderByDescending(x => x.fechaCapacitacion).FirstOrDefault();

                    fechaVigencia = ultimaCapacitacion.fechaCapacitacion.ToShortDateString() + " - " + ultimaCapacitacion.fechaCapacitacion.AddYears(1).ToShortDateString();
                }
            }

            return fechaVigencia;
        }

        public List<dynamic> GetDocumentosResguardos()
        {
            List<dynamic> lstDocumentos = new List<dynamic>(); ;
            try
            {
                lstDocumentos = _context.Select<dynamic>(new DapperDTO
                {
                    baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual /*== (int)EmpresaEnum.Construplan ? MainContextEnum.Construplan : MainContextEnum.Arrendadora*/,
                    consulta = @"SELECT id, ResguardoID, nombreRuta FROM tblM_DocumentosResguardos WHERE tipoArchivo = @tipoArchivo ORDER BY id DESC",
                    parametros = new { tipoArchivo = 7 }
                }).ToList();
            }
            catch (Exception e)
            {
                LogError(0, 0, "ResguardoEquipoDAO", "GetListaAutorizacionesPendientes", e, AccionEnum.CONSULTA, 0, 0);
                return lstDocumentos.ToList();
            }
            return lstDocumentos.ToList();
        }

        public void GuardarResguardoVehiculos(tblM_ResguardoVehiculosServicio obj)
        {
            if (true)
            {
                if (obj.id == 0)
                    SaveEntity(obj, (int)BitacoraEnum.RESGUARDOEQUIPO);
                else
                    Update(obj, obj.id, (int)BitacoraEnum.RESGUARDOEQUIPO);
            }
            else
            {
                throw new Exception("Ya se genero un folio con ese consecutivo consultar con sistemas...");
            }
        }

        public tblM_ResguardoVehiculosServicio getResguardoBYID(int id)
        {
            return _context.tblM_ResguardoVehiculosServicio.FirstOrDefault(x => x.id.Equals(id));
        }

        public List<int> GetEmpleadosResguardo()
        {
            List<int> result = _context.tblM_ResguardoVehiculosServicio.Where(x => x.estado == 0).Select(x => x.noEmpleado).ToList();

            return result;
        }
        public List<int> GetMaquinariaAsignada()
        {
            List<int> result = _context.tblM_ResguardoVehiculosServicio.Where(x => x.estado == 0).Select(x => x.MaquinariaID).ToList();

            return result;
        }
        public List<tblM_ResguardoVehiculosServicio> getListaResguardosPendientesAutorizacion(string cc, int economicoID)
        {
            return _context.tblM_ResguardoVehiculosServicio.Where(x => x.MaquinariaID == economicoID && x.estado != 3).ToList();
        }
        public List<tblM_ResguardoVehiculosServicio> getListaResguardosPendientesLicencia(List<tblP_CC_Usuario> listObj)
        {
            var listCC = listObj.Select(x => x.cc).ToList();
            var aux = DateTime.Today;
            return _context.tblM_ResguardoVehiculosServicio.Where(x => listCC.Contains(x.Obra.ToString()) && x.fechaVencimiento < aux && x.estado != 3).ToList();
        }
        public List<tblM_ResguardoVehiculosServicio> GetCursosManejoVencidos()
        {
            return _context.tblM_ResguardoVehiculosServicio.Where(x => x.fechaVigenciaCurso != null).ToList().Where(x =>
                ((DateTime)x.fechaVigenciaCurso).Date < DateTime.Now.Date &&
                x.noEmpleado == Int32.Parse(vSesiones.sesionUsuarioDTO.cveEmpleado)
            ).ToList();
        }
        public List<tblM_ResguardoVehiculosServicio> getListaResguardosPendientesPoliza(List<tblP_CC_Usuario> listObj)
        {
            var listCC = listObj.Select(x => x.cc).ToList();
            var aux = DateTime.Today;
            return _context.tblM_ResguardoVehiculosServicio.Where(x => listCC.Contains(x.Obra.ToString()) && x.fechaVencimientoPoliza < aux && x.estado == 2).ToList();
        }

        public string getCCByArea(string area)
        {
            try
            {
                return _context.tblP_CC.FirstOrDefault(x => x.areaCuenta == area).descripcion;
            }
            catch (Exception)
            {
                return area.ToString();
            }
        }

        public string getMaquinaByID(int id)
        {
            return _context.tblM_CatMaquina.FirstOrDefault(x => x.id == id).descripcion;
        }

        public string getNoEconomicoMaquinaByID(int id)
        {
            return _context.tblM_CatMaquina.FirstOrDefault(x => x.id == id).noEconomico;
        }

        public string getModeloByID(int id)
        {
            return _context.tblM_CatModeloEquipo.FirstOrDefault(x => x.id == id).descripcion;
        }
        public List<tblM_CatMaquina> getEquipoSinResguardo(string ac)
        {
            switch ((EmpresaEnum)vSesiones.sesionEmpresaActual)
            {
                
                case EmpresaEnum.Colombia:
                    {
                        var cc = _context.tblP_CC.FirstOrDefault(x => x.cc == ac);
                        if (cc != null)
                        {
                            ac = cc.areaCuenta;
                        }
                    }
                    break;
            }
            List<int> grupos = new List<int> { 142, 160, 161, 162, 163, 164, 165, 167, 180, 217, 222, 223, 224, 225, 268, 273, 278, 312 };
            var res = _context.tblM_ResguardoVehiculosServicio.Where(x => x.estado != 3).Select(x => x.MaquinariaID).ToList();
            var data = _context.tblM_CatMaquina.Where(x => grupos.Contains(x.grupoMaquinaria.id) && x.estatus != 0 && !res.Contains(x.id) && (string.IsNullOrEmpty(ac) ? true : x.centro_costos.Equals(ac))).ToList();


            return data;
        }

        public List<tblP_CC> GetCentrosCostos()
        {
            return _context.tblP_CC.Where(x => x.estatus).ToList();
        }

        public void NotificarCoordinadorSSOMA(string cc, int resguardoId, string economico)
        {
            try
            {
                var usuariosSeguridadANotificar = _context.tblM_CordinadorSeguridadAreaCuenta.Where(x => x.areaCuenta == cc && x.registroActivo).ToList();
                foreach (var item in usuariosSeguridadANotificar.Where(x => x.usuario.estatus))
                {
                    var alerta = new tblP_Alerta();
                    alerta.userEnviaID = vSesiones.sesionUsuarioDTO.id;
                    alerta.userRecibeID = item.id;
                    alerta.tipoAlerta = 2;
                    alerta.sistemaID = 1;
                    alerta.visto = false;
                    alerta.url = "/ResguardoEquipo/AutorizacionResguardo?id=" + resguardoId;
                    alerta.objID = resguardoId;
                    alerta.msj = "Resguardo equipo: " + economico;
                    alerta.moduloID = 1;
                    _context.tblP_Alerta.Add(alerta);
                    _context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                LogError(1, 1, "ResguardoEquipoController", "NotificarCoordinadorSSOMA", ex, AccionEnum.AGREGAR, resguardoId, 0);
            }
        }

        public void QuitarNotificacionSSOMA(int resguardoId)
        {
            var alerta = _context.tblP_Alerta.Where(x => x.objID == resguardoId && x.msj.Contains("Resguardo equipo:") && !x.visto && x.sistemaID == 1 && x.moduloID == 1).ToList();
            foreach (var item in alerta)
            {
                item.visto = true;
                _context.SaveChanges();
            }
        }
    }
}
