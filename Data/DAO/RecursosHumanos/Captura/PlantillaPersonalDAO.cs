
using Core.DAO.RecursosHumanos.Captura;
using Core.DTO;
using Core.DTO.Principal.Generales;
using Core.DTO.RecursosHumanos.Plantilla;
using Core.DTO.Utils.Data;
using Core.Entity.Principal.Alertas;
using Core.Entity.Principal.Usuarios;
using Core.Entity.RecursosHumanos.Captura;
using Core.Enum;
using Core.Enum.Multiempresa;
using Core.Enum.Principal;
using Core.Enum.Principal.Alertas;
using Core.Enum.Principal.Bitacoras;
using Core.Enum.RecursosHumanos;
using Data.EntityFramework;
using Data.EntityFramework.Context;
using Data.EntityFramework.Generic;
using Data.Factory.Principal.Usuarios;
using Infrastructure.Utils;
using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Core.Entity.FileManager;
using Data.DAO.Principal.Usuarios;
using Data.Factory.Enkontrol.General.CC;
using Core.DAO.Enkontrol.General.CC;
using Core.Entity.Administrativo.RecursosHumanos.Enkontrol;
using Data.DAO.Enkontrol.General.CC;
using Core.Enum.RecursosHumanos.Tabuladores;
using Core.Entity.Administrativo.RecursosHumanos.Tabuladores;

namespace Data.DAO.RecursosHumanos.Captura
{
    public class PlantillaPersonalDAO : GenericDAO<tblRH_PP_PlantillaPersonal>, IPlantillaPersonalDAO
    {
        #region INIT
        private const string _NOMBRE_CONTROLADOR = "PlantillaPersonalController";
        private const int _SISTEMA = (int)SistemasEnum.RH;
        private Dictionary<string, object> resultado = new Dictionary<string, object>();

        ICCDAO _ccFS_SP = new CCFactoryService().getCCServiceSP();
        #endregion

        public int GuardarPlantilla(tblRH_PP_PlantillaPersonal plantilla, List<tblRH_PP_PlantillaPersonal_Det> Dets, List<tblRH_PP_PlantillaPersonal_Aut> Auts)
        {
            int folio = 0;
            using (var dbContextTransaction = _context.Database.BeginTransaction())
            {
                try
                {
                    if (plantilla.id == 0)
                    {
                        plantilla.usuarioID = vSesiones.sesionUsuarioDTO.id;
                        plantilla.fechaMod = DateTime.Now;
                        _context.tblRH_PP_PlantillaPersonal.Add(plantilla);
                        _context.SaveChanges();
                        Dets.ForEach(x => x.plantillaID = plantilla.id);
                        Dets.ForEach(x => x.departamento = EnumExtensions.GetDescription((Tipo_DepartamentoEnum)x.departamentoNumero));
                        Auts.ForEach(x => x.plantillaID = plantilla.id);
                        _context.tblRH_PP_PlantillaPersonal_Det.AddRange(Dets);
                        _context.SaveChanges();
                        _context.tblRH_PP_PlantillaPersonal_Aut.AddRange(Auts);
                        _context.SaveChanges();
                        var primero = _context.tblRH_PP_PlantillaPersonal_Aut.FirstOrDefault(x => x.plantillaID == plantilla.id);
                        primero.autorizando = true;
                        _context.SaveChanges();
                        dbContextTransaction.Commit();
                        folio = plantilla.id;
                        //EnviarCorreo(plantilla.id, 0,(int)EstatusRegEnum.PENDIENTE);
                    }
                    else
                    {
                        _context.Entry(plantilla).State = System.Data.Entity.EntityState.Modified;
                        _context.Entry(Dets).State = System.Data.Entity.EntityState.Modified;
                        _context.Entry(Auts).State = System.Data.Entity.EntityState.Modified;
                        _context.SaveChanges();
                        dbContextTransaction.Commit();
                        folio = plantilla.id;
                    }
                }
                catch (Exception e)
                {
                    dbContextTransaction.Rollback();
                    folio = 0;
                }
            }
            return folio;
        }
        public tblRH_PP_PlantillaPersonal GetPlantillaSP(int id)
        {
            var data = new tblRH_PP_PlantillaPersonal();
            data = _context.tblRH_PP_PlantillaPersonal.FirstOrDefault(x => x.id == id);
            return data;
        }
        public List<PlantillaPersonal2DTO> GetPlantillaEK(string cc)
        {
            var data = new List<PlantillaPersonal2DTO>();
            var usuario = vSesiones.sesionUsuarioDTO.id;
            bool permisoSueldos = (_context.tblP_AccionesVistatblP_Usuario.Where(x => x.tblP_Usuario_id == usuario && x.tblP_AccionesVista_id == 4032).Count()) > 0;

            if (!cc.Equals(""))
            {
                var query = @"select
                            ppe.id_plantilla as plantillaID,
                            convert(varchar(10), ppe.fecha_inicio, 103) as fechaInicio,
                            convert(varchar(10), ppe.fecha_fin, 103) as fechaFin,
                            pu.puesto as id,
                            pu.descripcion as puesto,
                            '' as departamento,
                            tn.descripcion as nomina,
                            tn.tipo_nomina as nominaID,
                            ppu.cantidad as personalOriginal,
                            ISNULL((select SUM(a.cantidad) from tblRH_EK_Plantilla_Aditiva as a where a.id_plantilla=ppe.id_plantilla and a.puesto=pu.puesto ),0) as personalActual,
                            ISNULL((select top 1 t.salario_base from tblRH_EK_Tabulador_Puesto as t where t.puesto = pu.puesto and t.tabulador = tb.id order by t.tabulador desc),0) as sueldoBase,
                            ISNULL((select top 1 t.complemento from tblRH_EK_Tabulador_Puesto as t where t.puesto = pu.puesto and t.tabulador = tb.id order by t.tabulador desc),0) as sueldoComplemento,
                            ISNULL(((select top 1 t.salario_base from tblRH_EK_Tabulador_Puesto as t where t.puesto = pu.puesto and t.tabulador = tb.id order by t.tabulador desc) + (select top 1 t.complemento from tblRH_EK_Tabulador_Puesto as t where t.puesto = pu.puesto order by t.tabulador desc)),0) as sueldoTotal
                        from tblRH_EK_Plantilla_Puesto as ppu 
                        inner join tblRH_EK_Plantilla_Personal as ppe on ppu.id_plantilla = ppe.id_plantilla
                        inner join tblRH_EK_Puestos as pu on ppu.puesto=pu.puesto
                        inner join tblRH_EK_Tipos_Nomina as tn on pu.FK_TipoNomina=tn.tipo_nomina
                        inner join tblRH_EK_Tabuladores as tb on ppe.cc=tb.cc
                        where pu.descripcion not like '%NO USA%' and ppe.cc='" + cc + "'";
                //data = (List<PlantillaPersonal2DTO>)ContextEnKontrolNomina.Where(query).ToObject<List<PlantillaPersonal2DTO>>();
                data = _context.Select<PlantillaPersonal2DTO>(new DapperDTO
                {
                    baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
                    consulta = query
                }).ToList();
                var pID = data.FirstOrDefault().plantillaID;
                var plantilla = _context.tblRH_PP_PlantillaPersonal.FirstOrDefault(x=>x.plantillaEKID == pID);
                foreach (var i in data)
                {
                    if (plantilla != null) {
                        var depto = plantilla.listDetalle.FirstOrDefault(x => x.puestoNumero == i.id);
                        if (depto != null)
                        {
                            i.departamento = depto.departamento;
                        }
                    }
                    i.fechaInicio = Convert.ToDateTime(i.fechaInicio).ToShortDateString();
                    i.fechaFin = i.fechaFin == null ? "" : Convert.ToDateTime(i.fechaFin).ToShortDateString();
                    i.personalActual = i.personalOriginal + i.personalActual;
                    decimal m = permisoSueldos ? (i.nominaID == (int)Tipo_NominaEnum.QUINCENAL ? ((Decimal.Parse(i.sueldoBase) + Decimal.Parse(i.sueldoComplemento)) * 2) : ((((Decimal.Parse(i.sueldoBase) + Decimal.Parse(i.sueldoComplemento)))/7) * (decimal)30.4)) : 0;
                    i.sueldoMensual = permisoSueldos ? m.ToString("C2") : "--";
                    i.sueldoBase = permisoSueldos ? Decimal.Parse(i.sueldoBase).ToString("C2") : "--";
                    i.sueldoComplemento = permisoSueldos ? Decimal.Parse(i.sueldoComplemento).ToString("C2") : "--";
                    i.sueldoTotal = permisoSueldos ? Decimal.Parse(i.sueldoTotal).ToString("C2") : "--";
                    
                }
            }

            return data;
        }
        public bool AutorizarPlantilla(int plantillaID, int autorizacion, int estatus)
        {
            bool flag = false;
            using (var dbContextTransaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var data = _context.tblRH_PP_PlantillaPersonal_Aut.FirstOrDefault(x => x.id == autorizacion);
                    data.estatus = estatus;
                    data.fecha = DateTime.Now;
                    data.autorizando = false;
                    data.firma = "--" + plantillaID.ToString() + "|" + DateTime.Now.ToString("ddMMyyyy|HHmm") + "|" + (int)BitacoraEnum.PlantillaPersonal + "|" + data.aprobadorClave + "--";
                    _context.SaveChanges();
                    if (estatus == (int)EstatusRegEnum.RECHAZADO)
                    {
                        var p = _context.tblRH_PP_PlantillaPersonal.FirstOrDefault(x => x.id == plantillaID);
                        p.estatus = (int)EstatusRegEnum.RECHAZADO;
                        data.firma = "--" + plantillaID.ToString() + "|" + DateTime.Now.ToString("ddMMyyyy|HHmm") + "|" + (int)BitacoraEnum.PlantillaPersonal + "|" + data.aprobadorClave + "--";
                        p.fechaMod = (DateTime)data.fecha;
                        _context.SaveChanges();
                    }
                    else
                    {
                        var siguiente = _context.tblRH_PP_PlantillaPersonal_Aut.Where(x => x.plantillaID == plantillaID && x.estatus == (int)EstatusRegEnum.PENDIENTE).OrderBy(x => x.orden).FirstOrDefault();
                        if (siguiente != null)
                        {
                            if (siguiente.aprobadorClave == data.aprobadorClave)
                            {
                                siguiente.estatus = (int)EstatusRegEnum.AUTORIZADO;
                                siguiente.fecha = DateTime.Now;
                                siguiente.firma = "--" + plantillaID.ToString() + "|" + DateTime.Now.ToString("ddMMyyyy|HHmm") + "|" + (int)BitacoraEnum.PlantillaPersonal + "|" + data.aprobadorClave + "--";
                                _context.SaveChanges();
                                var siguiente2 = _context.tblRH_PP_PlantillaPersonal_Aut.Where(x => x.plantillaID == plantillaID && x.estatus == (int)EstatusRegEnum.PENDIENTE).OrderBy(x => x.orden).FirstOrDefault();
                                if (siguiente2 != null)
                                {
                                    if (siguiente2.aprobadorClave == data.aprobadorClave)
                                    {
                                        siguiente2.estatus = (int)EstatusRegEnum.AUTORIZADO;
                                        siguiente2.fecha = DateTime.Now;
                                        siguiente2.firma = "--" + plantillaID.ToString() + "|" + DateTime.Now.ToString("ddMMyyyy|HHmm") + "|" + (int)BitacoraEnum.PlantillaPersonal + "|" + data.aprobadorClave + "--";
                                        _context.SaveChanges();
                                        var siguiente3 = _context.tblRH_PP_PlantillaPersonal_Aut.Where(x => x.plantillaID == plantillaID && x.estatus == (int)EstatusRegEnum.PENDIENTE).OrderBy(x => x.orden).FirstOrDefault();
                                        if (siguiente3 != null)
                                        {
                                            if (siguiente3.aprobadorClave == data.aprobadorClave)
                                            {
                                                siguiente3.estatus = (int)EstatusRegEnum.AUTORIZADO;
                                                siguiente3.fecha = DateTime.Now;
                                                siguiente3.firma = "--" + plantillaID.ToString() + "|" + DateTime.Now.ToString("ddMMyyyy|HHmm") + "|" + (int)BitacoraEnum.PlantillaPersonal + "|" + data.aprobadorClave + "--";
                                                _context.SaveChanges();
                                                var siguiente4 = _context.tblRH_PP_PlantillaPersonal_Aut.Where(x => x.plantillaID == plantillaID && x.estatus == (int)EstatusRegEnum.PENDIENTE).OrderBy(x => x.orden).FirstOrDefault();
                                                if (siguiente4 == null)
                                                {
                                                    var p = _context.tblRH_PP_PlantillaPersonal.FirstOrDefault(x => x.id == plantillaID);
                                                    p.estatus = (int)EstatusRegEnum.AUTORIZADO;
                                                    p.fechaMod = (DateTime)data.fecha;
                                                    _context.SaveChanges();
                                                    insertEnkontrol(p.id);
                                                    insertEnvioGestor(p.id);
                                                }
                                            }
                                            else
                                            {
                                                siguiente3.autorizando = true;
                                                _context.SaveChanges();
                                            }
                                        }
                                        else
                                        {
                                            var p = _context.tblRH_PP_PlantillaPersonal.FirstOrDefault(x => x.id == plantillaID);
                                            p.estatus = (int)EstatusRegEnum.AUTORIZADO;
                                            p.fechaMod = (DateTime)data.fecha;
                                            _context.SaveChanges();
                                            insertEnkontrol(p.id);
                                            insertEnvioGestor(p.id);

                                        }
                                    }
                                    else
                                    {
                                        siguiente2.autorizando = true;
                                        _context.SaveChanges();
                                    }
                                }
                                else
                                {
                                    var p = _context.tblRH_PP_PlantillaPersonal.FirstOrDefault(x => x.id == plantillaID);
                                    p.estatus = (int)EstatusRegEnum.AUTORIZADO;
                                    p.fechaMod = (DateTime)data.fecha;
                                    _context.SaveChanges();
                                    insertEnkontrol(p.id);
                                    insertEnvioGestor(p.id);
                                }
                            }
                            else
                            {
                                siguiente.autorizando = true;
                                _context.SaveChanges();
                            }

                        }
                        else
                        {
                            var p = _context.tblRH_PP_PlantillaPersonal.FirstOrDefault(x => x.id == plantillaID);
                            p.estatus = (int)EstatusRegEnum.AUTORIZADO;
                            p.fechaMod = (DateTime)data.fecha;
                            _context.SaveChanges();
                            insertEnkontrol(p.id);
                            insertEnvioGestor(p.id);
                        }
                    }
                    dbContextTransaction.Commit();
                    //EnviarCorreo(plantillaID, autorizacion,estatus);
                    flag = true;
                }
                catch (Exception e)
                {
                    dbContextTransaction.Rollback();
                    flag = false;
                }
            }
            return flag;
        }
        public bool insertEnkontrol(int plantillaID)
        {
            bool result = false;
            try
            {
                var def = 0;
                var plantilla = _context.tblRH_PP_PlantillaPersonal.FirstOrDefault(x => x.id == plantillaID);

                //Plantilla
                //var odbcPlantilla = new OdbcConsultaDTO()
                //{
                //    consulta = queryPlantilla(plantilla),
                //    parametros = parametrosPlantilla(plantilla)
                //};
                //def = _contextEnkontrol.Save(EnkontrolAmbienteEnum.Rh, odbcPlantilla);
                var plantillaP = new tblRH_EK_Plantilla_Personal();
                plantillaP.cc = plantilla.ccID;
                plantillaP.solicita = 71;
                plantillaP.autoriza = 71;
                plantillaP.vistobueno = 113;
                plantillaP.observaciones = "";
                plantillaP.fecha = plantilla.fechaInicio;
                plantillaP.fecha_inicio = plantilla.fechaInicio;
                plantillaP.fecha_fin = plantilla.fechaFin;
                plantillaP.estatus = "A";
                plantillaP.check_vobo = true;
                plantillaP.check_autoriza = true;
                plantillaP.idUsuarioCreacion = vSesiones.sesionUsuarioDTO.id;
                plantillaP.fechaCreacion = DateTime.Now;
                plantillaP.esActivo = true;
                _context.tblRH_EK_Plantilla_Personal.Add(plantillaP);
                _context.SaveChanges();


                //var dtoPlantilla = _contextEnkontrol.Select<EscalarDTO>(EnkontrolAmbienteEnum.Rh, "SELECT max(id_plantilla) as valor FROM sn_plantilla_personal").FirstOrDefault();
                //plantilla.plantillaEKID = Convert.ToInt32(dtoPlantilla.valor);
                plantilla.plantillaEKID = plantillaP.id_plantilla;
                _context.SaveChanges();
                //Tabuladores
                //var odbcTabulador = new OdbcConsultaDTO()
                //{
                //    consulta = queryTabuladores(),
                //    parametros = parametrosTabuladores(plantilla)
                //};
                //def = _contextEnkontrol.Save(EnkontrolAmbienteEnum.Rh, odbcTabulador);
                var tabulador = new tblRH_EK_Tabuladores();
                tabulador.cc = plantilla.ccID;
                tabulador.fecha = plantilla.fechaInicio;
                tabulador.nomina = plantilla.listDetalle.FirstOrDefault().tipoNomina;
                tabulador.libre = "N";
                tabulador.idUsuarioCreacion = vSesiones.sesionUsuarioDTO.id;
                tabulador.fechaCreacion = DateTime.Now;
                tabulador.esActivo = true;
                _context.tblRH_EK_Tabuladores.Add(tabulador);
                _context.SaveChanges();
                //var dtoTabulador = _contextEnkontrol.Select<EscalarDTO>(EnkontrolAmbienteEnum.Rh, "(select max(id) as valor from sn_tabuladores)").FirstOrDefault();
                //plantilla.tabuladorEKID = Convert.ToInt32(dtoTabulador.valor);
                plantilla.tabuladorEKID = tabulador.id;
                _context.SaveChanges();
                foreach (var i in plantilla.listDetalle)
                {
                    //Puestos
                    var det = _context.tblRH_PP_PlantillaPersonal_Det.FirstOrDefault(x => x.id == i.id);
                    if (det.puestoNumero == 0)
                    {
                        //var odbcPuesto = new OdbcConsultaDTO()
                        //{
                        //    consulta = queryPuestos(),
                        //    parametros = parametrosPuestos(i)
                        //};
                        //def = _contextEnkontrol.Save(EnkontrolAmbienteEnum.Rh, odbcPuesto);
                        //var dtoPuesto = _contextEnkontrol.Select<EscalarDTO>(EnkontrolAmbienteEnum.Rh, "(select max(puesto) as valor from si_puestos)").FirstOrDefault();
                        //det.puestoNumero = Convert.ToInt32(dtoPuesto.valor);
                        var puesto = new tblRH_EK_Puestos();
                        puesto.descripcion = i.puesto;
                        puesto.descripcion_puesto = i.puesto;
                        puesto.FK_TipoNomina = i.tipoNomina;
                        puesto.FK_Sindicato = _context.tblRH_TAB_CatSindicato.Where(w => w.id == 2 && w.registroActivo).Select(s => s.id).FirstOrDefault(); // NO SINDICALIZADO
                        puesto.FK_UsuarioCreacion = vSesiones.sesionUsuarioDTO.id;
                        puesto.fechaCreacion = DateTime.Now;
                        puesto.registroActivo = true;
                        _context.tblRH_EK_Puestos.Add(puesto);
                        _context.SaveChanges();
                        det.puestoNumero = puesto.puesto;
                    }
                    //Plantilla_Puesto

                    //var odbcPlantilla_Puesto = new OdbcConsultaDTO()
                    //{
                    //    consulta = queryPlantilla_Puestos(),
                    //    parametros = parametrosPlantilla_Puestos(i)
                    //};
                    //def = _contextEnkontrol.Save(EnkontrolAmbienteEnum.Rh, odbcPlantilla_Puesto);
                    var plantillaPuesto = new tblRH_EK_Plantilla_Puesto();
                    plantillaPuesto.id_plantilla = plantillaP.id_plantilla;
                    plantillaPuesto.puesto = i.puestoNumero;
                    plantillaPuesto.cantidad = i.personalNecesario;
                    plantillaPuesto.check_bobo = true;
                    plantillaPuesto.check_autoriza = true;
                    plantillaPuesto.estatus = "A";
                    plantillaPuesto.altas = 0;
                    plantillaPuesto.bajas = 0;
                    plantillaPuesto.cc = "N";
                    plantillaPuesto.fechaCreacion = DateTime.Now;
                    plantillaPuesto.idUsuarioCreacion = vSesiones.sesionUsuarioDTO.id;
                    plantillaPuesto.esActivo = true;
                    _context.tblRH_EK_Plantilla_Puesto.Add(plantillaPuesto);
                    _context.SaveChanges();

                    //Tabulador_Puesto
                    //var odbcTabulador_Puesto = new OdbcConsultaDTO()
                    //{
                    //    consulta = queryTabulador_Puestos(),
                    //    parametros = parametrosTabulador_Puestos(i)
                    //};
                    //def = _contextEnkontrol.Save(EnkontrolAmbienteEnum.Rh, odbcTabulador_Puesto);
                    var tabuladorPuesto = new tblRH_EK_Tabulador_Puesto();
                    tabuladorPuesto.tabulador = i.plantilla.tabuladorEKID.Value;
                    tabuladorPuesto.puesto = i.puestoNumero;
                    tabuladorPuesto.salario_base = i.sueldoBase;
                    tabuladorPuesto.complemento = i.sueldoComplemento;
                    tabuladorPuesto.bono_de_zona = 0;
                    tabuladorPuesto.year = i.plantilla.fechaInicio.Year;
                    _context.tblRH_EK_Tabulador_Puesto.Add(tabuladorPuesto);
                    _context.SaveChanges();
                }

                result = true;
            }
            catch (Exception ex)
            {

                result = false;
            }
            
            
            return result;
        }

        public List<tblRH_PP_PlantillaPersonal_Aut> GetAutorizadores(int plantillaID)
        {
            var data = new List<tblRH_PP_PlantillaPersonal_Aut>();
            data = _context.tblRH_PP_PlantillaPersonal_Aut.Where(x => x.plantillaID == plantillaID).OrderBy(x => x.orden).ToList();
            return data;
        }
        public List<tblRH_PP_PlantillaPersonal> GetPlantillas(string cc, int estatus)
        {
            var ud = new UsuarioDAO();
            var rh = ud.getViewAction(vSesiones.sesionCurrentView, "VerTodoFormato");
            var data = new List<tblRH_PP_PlantillaPersonal>();
            var ccUsuario = !string.IsNullOrEmpty(vSesiones.sesionUsuarioDTO.cc) ? vSesiones.sesionUsuarioDTO.cc.Equals("012") : false;
            var esAdmin = vSesiones.sesionUsuarioDTO.perfil.Equals("Administrador") && ccUsuario;
            if (esAdmin || rh)
            {
                data = _context.tblRH_PP_PlantillaPersonal.Where(x => x.cc.StartsWith(cc) && x.estatus == estatus).ToList();
            }
            else
            {
                data = _context.tblRH_PP_PlantillaPersonal.Where(x => 
                    x.cc.StartsWith(cc) && 
                    ((x.estatus == (int)EstatusRegEnum.PENDIENTE && x.estatus == estatus && x.listAutorizadores.Any(y => y.aprobadorClave == vSesiones.sesionUsuarioDTO.id && y.autorizando)) || 
                    (x.estatus != (int)EstatusRegEnum.PENDIENTE && x.estatus == estatus && (x.usuarioID == vSesiones.sesionUsuarioDTO.id || x.listAutorizadores.Any(y => y.aprobadorClave == vSesiones.sesionUsuarioDTO.id))))
                    ).ToList();
            }
            return data;
        }
        public bool EnviarCorreo(int plantillaID, int autorizacion, int estatus)
        {
            UsuarioFactoryServices usuarioFactoryServices = new UsuarioFactoryServices();
            bool enviado = false;
            try
            {
                var plantilla = GetPlantillaSP(plantillaID);
                var aut = _context.tblRH_PP_PlantillaPersonal_Aut.FirstOrDefault(x => x.id == autorizacion);
                var usuarioSolicito = _context.tblP_Usuario.FirstOrDefault(x => x.id == plantilla.usuarioID);
                var usuarioEnvia = vSesiones.sesionUsuarioDTO;
                var downloadPDF = vSesiones.downloadPDF;
                var usuariosFormatoCambios = GetAutorizadores(plantillaID);
                var sig = _context.tblRH_PP_PlantillaPersonal_Aut.FirstOrDefault(x => x.plantillaID == plantillaID && x.autorizando);
                if (sig != null)
                {
                    var a = _context.tblP_Alerta.Where(x => x.sistemaID == (int)SistemasEnum.RH && x.moduloID == (int)BitacoraEnum.PlantillaPersonal && x.objID == plantillaID).ToList();
                    if (a.Count > 0)
                    {
                        foreach (var i in a)
                        {
                            i.visto = true;
                        }
                        _context.SaveChanges();
                        var b = new tblP_Alerta();
                        b.userEnviaID = 1;
                        b.userRecibeID = sig.aprobadorClave;
                        b.tipoAlerta = (int)AlertasEnum.REDIRECCION;
                        b.visto = false;
                        b.url = "/Administrativo/PlantillaPersonal/Gestion/?autID=" + plantillaID;
                        b.objID = plantillaID;
                        b.obj = "";
                        b.msj = "Plantilla Personal (" + plantilla.cc + ")";
                        b.documentoID = 0;
                        b.sistemaID = (int)SistemasEnum.RH;
                        b.moduloID = (int)BitacoraEnum.PlantillaPersonal;
                        _context.tblP_Alerta.Add(b);
                        _context.SaveChanges();
                    }
                    else
                    {
                        var b = new tblP_Alerta();
                        b.userEnviaID = 1;
                        b.userRecibeID = sig.aprobadorClave;
                        b.tipoAlerta = (int)AlertasEnum.REDIRECCION;
                        b.visto = false;
                        b.url = "/Administrativo/PlantillaPersonal/Gestion/?autID=" + plantillaID;
                        b.objID = plantillaID;
                        b.obj = "";
                        b.msj = "Plantilla Personal (" + plantilla.cc + ")";
                        b.documentoID = 0;
                        b.sistemaID = (int)SistemasEnum.RH;
                        b.moduloID = (int)BitacoraEnum.PlantillaPersonal;
                        _context.tblP_Alerta.Add(b);
                        _context.SaveChanges();
                    }
                }
                else
                {
                    var a = _context.tblP_Alerta.Where(x => x.sistemaID == (int)SistemasEnum.RH && x.moduloID == (int)BitacoraEnum.PlantillaPersonal && x.objID == plantillaID).ToList();
                    a.ForEach(x => x.visto = true);
                    _context.SaveChanges();
                }

                List<string> CorreoEnviar = new List<string>();
                string AsuntoCorreo = @"<html>
                                            <head>
                                                <style>
                                                    table {
                                                        font-family: arial, sans-serif;
                                                        border-collapse: collapse;
                                                        width: 100%;
                                                    }

                                                    td, th {
                                                        border: 1px solid #dddddd;
                                                        text-align: left;
                                                        padding: 8px;
                                                    }

                                                    tr:nth-child(even) {
                                                        background-color: #dddddd;
                                                    }
                                                </style>
                                            </head>
                                            <body lang=ES-MX link='#0563C1' vlink='#954F72'>
                                                <div class=WordSection1>
                                                    <p class=MsoNormal>
                                                        Buen día <o:p></o:p>
                                                    </p>
                                                    <p class=MsoNormal>
                                                        <o:p>&nbsp;</o:p>
                                                    </p>";
                var folioID = plantillaID;
                var folio = plantillaID.ToString().PadLeft(6, '0');
                if (plantilla.estatus != (int)EstatusRegEnum.PENDIENTE)
                {
                    if (plantilla.estatus == (int)EstatusRegEnum.AUTORIZADO)
                    {
                        AsuntoCorreo += @" <p class=MsoNormal>
                                                        Se informa que se finalizo correctamente el proceso de autorización en la plantilla de personal con Folio: &#8220;" + folio + @"&#8221 para el CC " + plantilla.cc + " por el usuario (" + usuarioEnvia.nombre + " " + usuarioEnvia.apellidoPaterno + " " + usuarioEnvia.apellidoMaterno + @").<o:p></o:p>
                                                    </p>";
                    }
                    else if (plantilla.estatus == (int)EstatusRegEnum.RECHAZADO)
                    {
                        AsuntoCorreo += @" <p class=MsoNormal>
                                                Se informa que la plantilla de personal con Folio: &#8220;" + folio + @"&#8221 para el CC " + plantilla.cc + " fue rechazado por el usuario (" + usuarioEnvia.nombre + " " + usuarioEnvia.apellidoPaterno + " " + usuarioEnvia.apellidoMaterno + @").<o:p></o:p>
                                            </p>";
                        AsuntoCorreo += @" <p class=MsoNormal>
                                                    <strong>La razón del rechazo fue: </strong> " + HttpUtility.HtmlEncode((aut.comentario ?? "")) + @"<o:p></o:p>
                                                </p>";
                    }
                }
                else if (autorizacion == 0)
                {
                    AsuntoCorreo += @" <p class=MsoNormal>
                                                        Se informa que se registro una nueva plantilla de persona con Folio: &#8220;" + folio + @"&#8221 para el CC " + plantilla.cc + " por el usuario (" + usuarioEnvia.nombre + " " + usuarioEnvia.apellidoPaterno + " " + usuarioEnvia.apellidoMaterno + @").<o:p></o:p>
                                                    </p>";
                }
                else if (estatus == (int)EstatusRegEnum.AUTORIZADO)
                {
                    AsuntoCorreo += @" <p class=MsoNormal>
                                                        Se informa que fue realizada una autorización en la plantilla de personal con Folio: &#8220;" + folio + @"&#8221 para el CC " + plantilla.cc + " por el usuario (" + usuarioEnvia.nombre + " " + usuarioEnvia.apellidoPaterno + " " + usuarioEnvia.apellidoMaterno + @").<o:p></o:p>
                                                    </p>";
                }
                else if (estatus == (int)EstatusRegEnum.RECHAZADO)
                {
                    AsuntoCorreo += @" <p class=MsoNormal>
                                            Se informa que la plantilla de personal con Folio: &#8220;" + folio + @"&#8221 para el CC " + plantilla.cc + " fue rechazado por el usuario (" + usuarioEnvia.nombre + " " + usuarioEnvia.apellidoPaterno + " " + usuarioEnvia.apellidoMaterno + @").<o:p></o:p>
                                        </p>";
                    AsuntoCorreo += @" <p class=MsoNormal>
                                                <strong>La razón del rechazo fue: </strong> " + HttpUtility.HtmlEncode((aut.comentario ?? "")) + @"<o:p></o:p>
                                            </p>";
                }
                AsuntoCorreo += @" <p class=MsoNormal>
                                                        <o:p>&nbsp;</o:p>
                                                    </p><br/><br/>
                                                        <table>
                                                            <thead>
                                                              <tr>
                                                                <th>Nombre Autorizador </th>
                                                                <th>Tipo</th>
                                                                <th>Autorizó</th>
                                                              </tr></thead>
                                                            <tbody>";

                var excepcionesCorreo = usuarioFactoryServices.getUsuarioService().getPermisosAutorizaCorreo(1);

                List<int> excepcionesCorreoIDs = new List<int>();

                if (excepcionesCorreo.Count > 0)
                {
                    excepcionesCorreoIDs.AddRange(excepcionesCorreo.Select(x => x.usuarioID));
                }

                int cont = 0;
                int lonusuariosFormatoCambios = usuariosFormatoCambios.Count();
                foreach (var i in usuariosFormatoCambios)
                {

                    AsuntoCorreo += @"<tr>
                                <td>" + i.aprobadorNombre + "</td>" +
                                    "<td>" + i.aprobadorPuesto + "</td>" +
                                        getEstatus(i.estatus, i.autorizando) +
                                    "</tr>";


                    var usuarioCorreo = usuarioFactoryServices.getUsuarioService().ListUsersById(i.aprobadorClave).FirstOrDefault();

                    if (i.autorizando)
                    {
                        CorreoEnviar.Add(usuarioCorreo.correo);
                    }
                    else
                    {
                        if (!excepcionesCorreoIDs.Contains(i.aprobadorClave))
                        {
                            CorreoEnviar.Add(usuarioCorreo.correo);
                        }
                    }
                }

                AsuntoCorreo += @"</tbody>" +
                            @"</table>
                                                    <p class=MsoNormal>
                                                        <o:p>&nbsp;</o:p>
                                                    </p>
                                                    <p class=MsoNormal>
                                                        Favor de ingresar al sistema SIGOPLAN (<a href='http://sigoplan.construplan.com.mx/'>http://sigoplan.construplan.com.mx/</a>), en el apartado de ADMINISTRACION, menú RH en la opción Plantilla de Personal<o:p></o:p>
                                                    </p>
                                                    <p class=MsoNormal>
                                                        <o:p>&nbsp;</o:p>
                                                    </p>
                                                    <p class=MsoNormal>
                                                        <o:p>&nbsp;</o:p>
                                                    </p>
                                                    <p class=MsoNormal>
                                                        PD. Se informa que esta es una notificación autogenerada por el sistema SIGOPLAN no es necesario dar una respuesta <o:p></o:p>
                                                    </p>
                                                    <p class=MsoNormal>
                                                        <o:p>&nbsp;</o:p>
                                                    </p>
                                                    <p class=MsoNormal>
                                                        Gracias.<o:p></o:p>
                                                    </p>
                                                </div>
                                            </body>
                                        </html>";

                CorreoEnviar.Add(usuarioEnvia.correo);
                if (usuarioEnvia.id != usuarioSolicito.id)
                {
                    CorreoEnviar.Add(usuarioSolicito.correo);
                }
                CorreoEnviar.Add(_context.tblP_Usuario.FirstOrDefault(u => u.id == 1019).correo);
                CorreoEnviar.Add(_context.tblP_Usuario.FirstOrDefault(u => u.id == 79552).correo);

                try
                {
                    var d = _context.tblP_Autoriza.Where(x => x.perfilAutorizaID == 8 && x.usuario.cc.Equals(plantilla.ccID) && (x.usuario.correo!=null && !x.usuario.correo.Equals(""))).Select(x=>x.usuario.correo).Distinct().ToList();
                    CorreoEnviar.AddRange(d);
                }
                catch (Exception)
                {
                    
                }
                var tipoFormato = "PlantillaPersonal.pdf";
                 #region Remover_Gerardo Reina de seguimiento una ves autorizado
                try
                {
                    if (CorreoEnviar.Contains("g.reina@construplan.com.mx"))
                    {
                        var autorizadores = _context.tblRH_PP_PlantillaPersonal_Aut.Where(x => x.plantillaID == plantillaID);
                        var greina = autorizadores.FirstOrDefault(x => x.aprobadorClave == 1164);
                        if (greina != null)
                        {
                            if(greina.estatus==2)
                            {
                                CorreoEnviar.Remove("g.reina@construplan.com.mx");
                            }
                        }
                        else
                        {

                            CorreoEnviar.Remove("g.reina@construplan.com.mx");
                        }
                    }
                }
                catch{}
                #endregion
                GlobalUtils.sendEmailAdjuntoInMemory2(string.Format("{0}: {1}", PersonalUtilities.GetNombreEmpresa(), "Alerta de Autorizaciones 'Plantilla Personal (" + plantilla.cc + ")'"), AsuntoCorreo, CorreoEnviar.Distinct().ToList(), downloadPDF, tipoFormato);
                vSesiones.downloadPDF = null;
                enviado = true;
            }
            catch (Exception e)
            {
                enviado = false;
            }
            return enviado;
        }
        private string getEstatus(int est, bool aut)
        {
            if ((int)EstatusRegEnum.PENDIENTE == (est) && aut)
                return "<td style='background-color: yellow;'>AUTORIZANDO</td>";
            else if ((int)EstatusRegEnum.AUTORIZADO == (est))
                return "<td style='background-color: #82E0AA;'>AUTORIZADO</td>";
            else
                if ((int)EstatusRegEnum.RECHAZADO == (est))
                    return "<td style='background-color: #EC7063;'>RECHAZADO</td>";
                else
                    return "<td style='background-color: #FAE5D3;'>PENDIENTE</td>";
        }
        public List<ComboDTO> GetDepartamentos(string cc)
        {
            List<ComboDTO> lista = new List<ComboDTO>();
            lista.AddRange(EnumExtensions.ToCombo<Tipo_DepartamentoEnum>().Select(x => new ComboDTO { Value = x.Value.ToString(), Text = x.Text, Prefijo = x.Prefijo }));
            return lista;
        }
        public List<ComboDTO> GetPuestos()
        {
            List<ComboDTO> lista = new List<ComboDTO>();
            //lista = (List<ComboDTO>)ContextEnKontrolNomina.Where("select puesto as Value,descripcion as Text,tipo_nomina as prefijo from si_puestos where descripcion not like '%(NO USAR)%'").ToObject<List<ComboDTO>>();
            lista = _context.Select<ComboDTO>(new DapperDTO
            {
                baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
                consulta = "SELECT puesto AS Value, descripcion AS Text, FK_TipoNomina AS prefijo FROM tblRH_EK_Puestos WHERE descripcion NOT LIKE '%(NO USA)%' and registroActivo = 1"
            }).ToList();
            return lista;
        }
        public List<ComboDTO> GetTipoNomina()
        {
            List<ComboDTO> lista = new List<ComboDTO>();
            lista.AddRange(EnumExtensions.ToCombo<Tipo_NominaEnum>().Select(x => new ComboDTO { Value = x.Value.ToString(), Text = x.Text, Prefijo = x.Prefijo }));
            return lista;
        }
        public PlantillaReporteDTO GetReporte(int id, int empresa = 0)
        {
            if (empresa == 0) { empresa = vSesiones.sesionEmpresaActual; }
            var plantilla = new PlantillaReporteDTO();

            using (var ctx = new MainContext(empresa))
            {
                var data = ctx.tblRH_PP_PlantillaPersonal.FirstOrDefault(x => x.id == id);
                plantilla.cc = data.cc;
                plantilla.fechaInicio = data.fechaInicio == null ? "" : data.fechaInicio.ToShortDateString();
                plantilla.fechaFin = data.fechaFin == null ? "" : data.fechaFin.Value.ToShortDateString();

                var datos = new List<PlantillaPersonalDTO>();
                foreach (var item in data.listDetalle)
                {
                    var o = new PlantillaPersonalDTO();
                    o.id = item.puestoNumero.ToString();
                    o.puesto = item.puesto;
                    o.departamento = item.departamento;
                    o.nomina = EnumExtensions.GetDescription((Tipo_NominaEnum)item.tipoNomina);
                    o.personal = item.personalNecesario.ToString();
                    o.sueldoBase = item.sueldoBase.ToString("C2");
                    o.sueldoComplemento = item.sueldoComplemento.ToString("C2");
                    o.sueldoTotal = item.sueldoTotal.ToString("C2");
                    o.sueldoMensual = (item.tipoNomina == (int)Tipo_NominaEnum.QUINCENAL ? ((item.sueldoBase + item.sueldoComplemento) * 2) : ((((item.sueldoBase + item.sueldoComplemento)) / 7) * (decimal)30.4)).ToString("C2");
                    datos.Add(o);
                }

                var autorizantes = new List<PlantillaAutorizanteDTO>();
                foreach (var item in data.listAutorizadores)
                {
                    var o = new PlantillaAutorizanteDTO();
                    o.nombre = item.aprobadorNombre;
                    o.tipo = item.tipo;
                    o.puesto = item.aprobadorPuesto;
                    o.firma = item.firma;
                    autorizantes.Add(o);
                }
                plantilla.data = datos;
                plantilla.autorizantes = autorizantes;
            }

            return plantilla;
        }

        //PARA DOCUMENTO DE MODULO DE TABULADORES
        public PlantillaReporteDTO GetReportePlantilla(string cc, int empresa = 0)
        {
            try
            {
                #region CATALOGOS
                List<tblRH_TAB_CatAreaDepartamento> lstAreasDepartamentos = _context.tblRH_TAB_CatAreaDepartamento.Where(w => w.registroActivo).ToList();
                List<tblRH_EK_Tipos_Nomina> lstTipoNominas = _context.tblRH_EK_Tipos_Nomina.Where(w => w.esActivo == true).ToList();
                List<tblRH_TAB_CatCategorias> lstCategorias = _context.tblRH_TAB_CatCategorias.Where(w => w.registroActivo).ToList();
                List<tblRH_EK_Puestos> lstPuestos = _context.tblRH_EK_Puestos.Where(w => w.registroActivo).ToList();
                List<tblRH_TAB_Tabuladores> lstTabuladores = _context.tblRH_TAB_Tabuladores.Where(w => w.tabuladorAutorizado == EstatusGestionAutorizacionEnum.AUTORIZADO && w.registroActivo).ToList();
                List<tblRH_TAB_TabuladoresDet> lstTabuladoresDet = _context.tblRH_TAB_TabuladoresDet.Where(w => w.tabuladorDetAutorizado == EstatusGestionAutorizacionEnum.AUTORIZADO && w.registroActivo).ToList();
                #endregion

                PlantillaReporteDTO plantilla = new PlantillaReporteDTO();
                List<PlantillaPersonalDTO> datos = new List<PlantillaPersonalDTO>();
                if (HttpContext.Current.Session["lstTabuladoresDTO"] != null)
                {
                    List<Core.DTO.RecursosHumanos.Tabuladores.TabuladorDetDTO> lstTabuladoresDTO = (List<Core.DTO.RecursosHumanos.Tabuladores.TabuladorDetDTO>)HttpContext.Current.Session["lstTabuladoresDTO"];
                    if (lstTabuladoresDTO == null || lstTabuladoresDTO.Count() <= 0)
                        throw new Exception("Es necesario realizar una búsqueda para poder generar el archivo excel.");

                    foreach (var item in lstTabuladoresDTO)
                    {
                        #region FILTROS
                        if (item.FK_LineaNegocio > 0)
                            lstTabuladoresDet = lstTabuladoresDet.Where(w => w.FK_LineaNegocio == item.FK_LineaNegocio).ToList();
                        #endregion

                        // SE OBTIENE LA INFORMACIÓN DEL PUESTO
                        tblRH_EK_Puestos objPuesto = lstPuestos.Where(w => w.puesto == item.idPuesto).FirstOrDefault();
                        tblRH_TAB_CatCategorias objCategoria = lstCategorias.Where(w => w.id == item.FK_Categoria).FirstOrDefault();

                        for (int i = 0; i < item.lstCategorias.Count(); i++)
                        {
                            PlantillaPersonalDTO obj = new PlantillaPersonalDTO();
                            obj.id = item.idPuesto.ToString();
                            obj.puesto = string.Format("{0} {1}", item.puestoDesc, item.lstCategorias[i]);
                            obj.departamento = "S/N";
                            obj.personal = "0";
                            obj.nomina = EnumExtensions.GetDescription((Tipo_NominaEnum)objPuesto.FK_TipoNomina);
                            obj.sueldoBase = vSesiones.sesionEmpresaActual == (int)EmpresaEnum.Peru ? ("S/ " + item.lstSueldosBases[i].ToString()) : item.lstSueldosBases[i].ToString();
                            obj.sueldoComplemento = vSesiones.sesionEmpresaActual == (int)EmpresaEnum.Peru ? ("S/ " + item.lstComplementos[i].ToString()) : item.lstSueldosBases[i].ToString();
                            obj.sueldoTotal = vSesiones.sesionEmpresaActual == (int)EmpresaEnum.Peru ? ("S/ " + item.lstTotalNominal[i].ToString()) : item.lstSueldosBases[i].ToString();
                            obj.sueldoMensual = vSesiones.sesionEmpresaActual == (int)EmpresaEnum.Peru ? ("S/ " + item.lstTotalNominal[i].ToString()) : item.lstSueldosBases[i].ToString();
                            datos.Add(obj);
                        }
                    }
                }
                else
                {
                    #region MIKE
                    var ccDescripcion = _context.tblC_Nom_CatalogoCC.FirstOrDefault(x => x.cc == cc);
                    var FK_Plantilla = _context.tblRH_TAB_PlantillasPersonal.FirstOrDefault(w => w.cc == cc && w.registroActivo);
                    plantilla.cc = FK_Plantilla.cc + (ccDescripcion != null ? " " + ccDescripcion.ccDescripcion : "");
                    plantilla.fechaInicio = FK_Plantilla.fechaInicio == null ? "" : Convert.ToDateTime(FK_Plantilla.fechaInicio).ToShortDateString();
                    plantilla.fechaFin = FK_Plantilla.fechaFin == null ? "" : Convert.ToDateTime(FK_Plantilla.fechaFin).ToShortDateString();

                    List<tblRH_TAB_PlantillasPersonalDet> lstPlantillasPersonalDet = _context.tblRH_TAB_PlantillasPersonalDet.Where(w => w.FK_Plantilla == FK_Plantilla.id && w.registroActivo).ToList();
                    List<int> lstFK_Puestos = lstPlantillasPersonalDet.Where(w => w.FK_Plantilla == FK_Plantilla.id && w.registroActivo).Select(s => s.FK_Puesto).ToList();
                    if (lstFK_Puestos.Count() <= 0)
                        throw new Exception("Ocurrió un error al obtener el detalle de la plantilla.");

                    lstPuestos = _context.tblRH_EK_Puestos.Where(w => lstFK_Puestos.Contains(w.puesto) && w.registroActivo).ToList();

                    lstTabuladores = _context.tblRH_TAB_Tabuladores.Where(w => w.tabuladorAutorizado == EstatusGestionAutorizacionEnum.AUTORIZADO && lstFK_Puestos.Contains(w.FK_Puesto) && w.registroActivo).ToList();
                    List<int> lstFK_Tabuladores = lstTabuladores.Select(s => s.id).ToList();

                    var lineaNegocioDet = _context.tblRH_TAB_CatLineaNegocioDet.FirstOrDefault(x => x.cc == cc && x.registroActivo);

                    List<PlantillaPersonalDTO> lstDetalleDTO = new List<PlantillaPersonalDTO>();
                    PlantillaPersonalDTO objDetalleDTO = new PlantillaPersonalDTO();

                    var lstAutorizadores = _context.tblRH_TAB_GestionAutorizantes.Where(x => x.registroActivo && x.vistaAutorizacion == VistaAutorizacionEnum.PLANTILLAS_PERSONAL && x.FK_Registro == FK_Plantilla.id).ToList();

                    foreach (var item in lstPlantillasPersonalDet)
                    {
                        tblRH_EK_Puestos objPuesto = lstPuestos.Where(w => w.puesto == item.FK_Puesto).FirstOrDefault();
                        tblRH_TAB_CatAreaDepartamento objAreaDepartamento = lstAreasDepartamentos.Where(w => w.id == objPuesto.FK_AreaDepartamento).FirstOrDefault();
                        tblRH_TAB_PlantillasPersonalDet objPersonalNecesario = lstPlantillasPersonalDet.Where(w => w.FK_Puesto == item.FK_Puesto).FirstOrDefault();
                        tblRH_EK_Tipos_Nomina objTipoNomina = lstTipoNominas.Where(w => w.tipo_nomina == objPuesto.FK_TipoNomina).FirstOrDefault();

                        var objFKPuestoTab = lstTabuladores.FirstOrDefault(e => e.FK_Puesto == item.FK_Puesto);

                        if (objFKPuestoTab != null)
                        {
                            string descCategoria = "";
                            var lstTabDet = _context.tblRH_TAB_TabuladoresDet
                                .Where(e => e.FK_Tabulador == objFKPuestoTab.id && e.FK_LineaNegocio == lineaNegocioDet.FK_LineaNegocio && 
                                       e.tabuladorDetAutorizado == EstatusGestionAutorizacionEnum.AUTORIZADO && e.registroActivo);

                            foreach (var objTabDet in lstTabDet)
                            {
                                var objCategoria = lstCategorias.FirstOrDefault(e => e.id == objTabDet.FK_Categoria);
                                if (objCategoria != null)
                                    descCategoria = lstCategorias.FirstOrDefault(e => e.id == objTabDet.FK_Categoria).concepto;

                                var obj = new PlantillaPersonalDTO();
                                obj.id = objFKPuestoTab.FK_Puesto.ToString();
                                obj.puesto = objPuesto.descripcion;
                                obj.categoria = descCategoria;
                                obj.departamento = objAreaDepartamento != null ? objAreaDepartamento.concepto : "";
                                obj.nomina = EnumExtensions.GetDescription((Tipo_NominaEnum)objPuesto.FK_TipoNomina) ?? "";
                                obj.personal = item.personalNecesario.ToString() ?? "";
                                obj.sueldoBase = vSesiones.sesionEmpresaActual == 6 ? ("S/ " + objTabDet.sueldoBase.ToString("C2")) : objTabDet.sueldoBase.ToString("C2");
                                obj.sueldoComplemento = vSesiones.sesionEmpresaActual == 6 ? ("S/ " + objTabDet.complemento.ToString("C2")) : objTabDet.complemento.ToString("C2");
                                obj.sueldoTotal = vSesiones.sesionEmpresaActual == 6 ? ("S/ " + objTabDet.totalNominal.ToString("C2")) : objTabDet.totalNominal.ToString("C2");
                                obj.sueldoMensual = vSesiones.sesionEmpresaActual == 6 ? ("S/ " + objTabDet.sueldoMensual.ToString("C2")) : objTabDet.sueldoMensual.ToString("C2");
                                datos.Add(obj);
                            }
                        }
                        else
                        {
                            var o = new PlantillaPersonalDTO();
                            o.id = item.FK_Puesto.ToString();
                            o.puesto = objPuesto.descripcion;
                            o.departamento = "S/N";
                            o.nomina = EnumExtensions.GetDescription((Tipo_NominaEnum)objPuesto.FK_TipoNomina);
                            o.personal = item.personalNecesario.ToString();
                            o.sueldoBase = "0";
                            o.sueldoComplemento = "0";
                            o.sueldoTotal = "0";
                            o.sueldoMensual = "0";
                            datos.Add(o);
                        }
                    }

                    var autorizantes = new List<PlantillaAutorizanteDTO>();
                    var lstAutorizantesOrdered = new List<tblRH_TAB_GestionAutorizantes>();

                    //INSERTAR EN EL ORDEN CORRECTO LOS AUTORIZANTES
                    lstAutorizantesOrdered.AddRange(lstAutorizadores.Where(e => e.nivelAutorizante == NivelAutorizanteEnum.RESPONSABLE_CC));
                    lstAutorizantesOrdered.AddRange(lstAutorizadores.Where(e => e.nivelAutorizante == NivelAutorizanteEnum.DIRECTOR_LINEA_NEGOCIOS));
                    lstAutorizantesOrdered.AddRange(lstAutorizadores.Where(e => e.nivelAutorizante == NivelAutorizanteEnum.ALTA_DIRECCION));
                    lstAutorizantesOrdered.AddRange(lstAutorizadores.Where(e => e.nivelAutorizante == NivelAutorizanteEnum.CAPITAL_HUMANO));

                    foreach (var item in lstAutorizantesOrdered)
                    {
                        tblP_Usuario objUsuario = _context.tblP_Usuario.Where(w => w.id == item.FK_UsuarioAutorizacion).FirstOrDefault();

                        string puesto = "";
                        string tipo = "";

                        switch (item.nivelAutorizante)
                        {
                            case NivelAutorizanteEnum.CAPITAL_HUMANO:
                                puesto = "Capital humano";
                                tipo = "VoBo";
                                break;
                            case NivelAutorizanteEnum.DIRECTOR_LINEA_NEGOCIOS:
                                puesto = "Director de linea de negocios";
                                tipo = "Autoriza 1";
                                break;
                            case NivelAutorizanteEnum.ALTA_DIRECCION:
                                puesto = "Alta dirección";
                                tipo = "Autoriza 2";
                                break;
                            case NivelAutorizanteEnum.RESPONSABLE_CC:
                                puesto = "Responsable del CC";
                                tipo = "Solicita";
                                break;
                            default:
                                break;
                        }

                        var o = new PlantillaAutorizanteDTO();
                        o.nombre = objUsuario.nombre + " " + objUsuario.apellidoPaterno + " " + objUsuario.apellidoMaterno;
                        o.tipo = tipo;
                        o.puesto = puesto;
                        o.firma = item.autorizado == EstatusGestionAutorizacionEnum.AUTORIZADO ? GlobalUtils.CrearFirmaDigitalConFecha(FK_Plantilla.id, DocumentosEnum.PlantillaPersonal, objUsuario.id, item.fechaModificacion ?? item.fechaCreacion, TipoFirmaEnum.Autorizacion) : "-";
                        autorizantes.Add(o);
                    }
                    plantilla.autorizantes = autorizantes;
                    #endregion
                }

                plantilla.data = datos;
                return plantilla;
            }
            catch (Exception e)
            {
                var NOMBRE_FUNCION = System.Reflection.MethodBase.GetCurrentMethod().Name;
                LogError(_SISTEMA, 0, _NOMBRE_CONTROLADOR, NOMBRE_FUNCION, e, AccionEnum.CONSULTA, 0, null);
                return null;
            }
        }

        public List<ComboDTO> FillComboCC(bool plantilla)
        {
            var txt = plantilla ? "in" : "not in";
            //return (List<ComboDTO>)ContextEnKontrolNomina.Where("SELECT cc as Value, (cc+'-' +descripcion) as Text FROM cc where st_ppto!='T' and cc " + txt + " ( SELECT cc FROM sn_plantilla_personal)").ToObject<List<ComboDTO>>();

//            var query_cc = new OdbcConsultaDTO();

//            query_cc.consulta =
//                @"SELECT
//                    cc as Value,
//                    (cc + '-' + descripcion) as Text
//                FROM
//                    cc
//                WHERE
//                    cc" + txt + " (SELECT cc FROM tblRH_EK_Plantilla_Personal)";

            //return _contextEnkontrol.Select<ComboDTO>(vSesiones.sesionAmbienteEnkontrolAdm, query_cc);

            //var ccdao = new ;
            //var ccs = ccdao.GetCCs();
            var ccs = _ccFS_SP.GetCCsNomina(null);
            var ccPlantilla = _context.tblRH_EK_Plantilla_Personal.Select(x => x.cc).Distinct().ToList();

            return ccs.Where(x => (plantilla ? ccPlantilla.Contains(x.cc) : !ccPlantilla.Contains(x.cc))).Select(x => new ComboDTO
            {
                Value = x.cc,
                Text = x.cc + "-" + x.descripcion
            }).ToList();
            //return _context.Select<ComboDTO>(new DapperDTO
            //{
            //    baseDatos = vSesiones.sesionEmpresaActual == (int)EmpresaEnum.Construplan ? MainContextEnum.Construplan : MainContextEnum.Arrendadora,
            //    consulta = query_cc.consulta
            //}).ToList();
        }
        string queryPlantilla(tblRH_PP_PlantillaPersonal o)
        {
            if (o.fechaFin != null)
            {
                return
                    string.Format(@"
                insert into sn_plantilla_personal
                (
                    id_plantilla,
                    cc,
                    solicita,
                    autoriza,
                    vistobueno,
                    observaciones,
                    fecha,
                    fecha_inicio,
                    fecha_fin,
                    estatus,
                    check_vobo,
                    check_autoriza
                )
                values(?,?,?,?,?,?,?,?,?,?,?,?)");
            }
            else {
                return
                    string.Format(@"
                insert into sn_plantilla_personal
                (
                    id_plantilla,
                    cc,
                    solicita,
                    autoriza,
                    vistobueno,
                    observaciones,
                    fecha,
                    fecha_inicio,
                    estatus,
                    check_vobo,
                    check_autoriza
                )
                values(?,?,?,?,?,?,?,?,?,?,?)");
            }
        }
        List<OdbcParameterDTO> parametrosPlantilla(tblRH_PP_PlantillaPersonal o)
        {
            var parameters = new List<OdbcParameterDTO>();
            var valor = _contextEnkontrol.Select<EscalarDTO>(EnkontrolAmbienteEnum.Rh, "SELECT (max(id_plantilla)+1) as valor FROM sn_plantilla_personal").FirstOrDefault();
            parameters.Add(new OdbcParameterDTO() { nombre = "id_plantilla", tipo = OdbcType.Decimal, valor = valor.valor });
            parameters.Add(new OdbcParameterDTO() { nombre = "cc", tipo = OdbcType.Char, valor = o.ccID });
            parameters.Add(new OdbcParameterDTO() { nombre = "solicita", tipo = OdbcType.Decimal, valor = 71 });
            parameters.Add(new OdbcParameterDTO() { nombre = "autoriza", tipo = OdbcType.Decimal, valor = 71 });
            parameters.Add(new OdbcParameterDTO() { nombre = "vistobueno", tipo = OdbcType.Decimal, valor = 113 });
            parameters.Add(new OdbcParameterDTO() { nombre = "observaciones", tipo = OdbcType.VarChar, valor = "" });
            parameters.Add(new OdbcParameterDTO() { nombre = "fecha", tipo = OdbcType.Date, valor = o.fechaInicio });
            parameters.Add(new OdbcParameterDTO() { nombre = "fecha_inicio", tipo = OdbcType.Date, valor = o.fechaInicio });
            if (o.fechaFin != null)
            {
                parameters.Add(new OdbcParameterDTO() { nombre = "fecha_fin", tipo = OdbcType.Date, valor = o.fechaFin.Value });
            }
            parameters.Add(new OdbcParameterDTO() { nombre = "estatus", tipo = OdbcType.Char, valor = "A" });
            parameters.Add(new OdbcParameterDTO() { nombre = "check_vobo", tipo = OdbcType.Numeric, valor = 1 });
            parameters.Add(new OdbcParameterDTO() { nombre = "check_autoriza", tipo = OdbcType.Numeric, valor = 1 });
            return parameters;
        }
        string queryTabuladores()
        {
            return
                string.Format(@"
                insert into sn_tabuladores
                (
                    id,
                    cc,
                    fecha,
                    nomina,
                    libre
                )
                values(?,?,?,?,?)");
        }
        List<OdbcParameterDTO> parametrosTabuladores(tblRH_PP_PlantillaPersonal o)
        {
            var parameters = new List<OdbcParameterDTO>();
            var valor = _contextEnkontrol.Select<EscalarDTO>(EnkontrolAmbienteEnum.Rh, "(select (max(id)+1) as valor from sn_tabuladores)").FirstOrDefault();
            parameters.Add(new OdbcParameterDTO() { nombre = "id", tipo = OdbcType.Decimal, valor = valor.valor });
            parameters.Add(new OdbcParameterDTO() { nombre = "cc", tipo = OdbcType.Char, valor = o.ccID });
            parameters.Add(new OdbcParameterDTO() { nombre = "fecha", tipo = OdbcType.Date, valor = o.fechaInicio });
            parameters.Add(new OdbcParameterDTO() { nombre = "nomina", tipo = OdbcType.Decimal, valor = o.listDetalle.FirstOrDefault().tipoNomina });
            parameters.Add(new OdbcParameterDTO() { nombre = "libre", tipo = OdbcType.Char, valor = "N" });
            return parameters;
        }
        string queryPuestos()
        {
            return
                string.Format(@"
                insert into si_puestos
                (
                    puesto,
                    descripcion,
                    descripcion_puesto,
                    tipo_nomina,
                    sindicalizado
                ) 
                values (?,?,?,?,?)");
        }
        List<OdbcParameterDTO> parametrosPuestos(tblRH_PP_PlantillaPersonal_Det o)
        {
            var parameters = new List<OdbcParameterDTO>();
            var valor = _contextEnkontrol.Select<EscalarDTO>(EnkontrolAmbienteEnum.Rh, "(select (max(puesto)+1) as valor from si_puestos)").FirstOrDefault();
            parameters.Add(new OdbcParameterDTO() { nombre = "puesto", tipo = OdbcType.Numeric, valor = valor.valor });
            parameters.Add(new OdbcParameterDTO() { nombre = "descripcion", tipo = OdbcType.VarChar, valor = o.puesto });
            parameters.Add(new OdbcParameterDTO() { nombre = "descripcion_puesto", tipo = OdbcType.VarChar, valor = o.puesto });
            parameters.Add(new OdbcParameterDTO() { nombre = "tipo_nomina", tipo = OdbcType.Numeric, valor = o.tipoNomina });
            parameters.Add(new OdbcParameterDTO() { nombre = "sindicalizado", tipo = OdbcType.Char, valor = "N" });
            return parameters;
        }
        string queryPlantilla_Puestos()
        {
            return
                string.Format(@"
                insert into sn_plantilla_puesto
                (
                    id_plantilla,
                    puesto,
                    cantidad,
                    check_vobo,
                    check_autoriza,
                    estatus,
                    altas,
                    bajas,
                    cc
                )
                values(?,?,?,?,?,?,?,?,?)");
        }
        List<OdbcParameterDTO> parametrosPlantilla_Puestos(tblRH_PP_PlantillaPersonal_Det o)
        {
            var parameters = new List<OdbcParameterDTO>();
            parameters.Add(new OdbcParameterDTO() { nombre = "id_plantilla", tipo = OdbcType.Decimal, valor = o.plantilla.plantillaEKID });
            parameters.Add(new OdbcParameterDTO() { nombre = "puesto", tipo = OdbcType.Decimal, valor = o.puestoNumero });
            parameters.Add(new OdbcParameterDTO() { nombre = "cantidad", tipo = OdbcType.Numeric, valor = o.personalNecesario });
            parameters.Add(new OdbcParameterDTO() { nombre = "check_vobo", tipo = OdbcType.Numeric, valor = 1 });
            parameters.Add(new OdbcParameterDTO() { nombre = "check_autoriza", tipo = OdbcType.Numeric, valor = 1 });
            parameters.Add(new OdbcParameterDTO() { nombre = "estatus", tipo = OdbcType.Char, valor = "A" });
            parameters.Add(new OdbcParameterDTO() { nombre = "altas", tipo = OdbcType.Numeric, valor = 0 });
            parameters.Add(new OdbcParameterDTO() { nombre = "bajas", tipo = OdbcType.Numeric, valor = 0 });
            parameters.Add(new OdbcParameterDTO() { nombre = "cc", tipo = OdbcType.Char, valor = "N" });
            return parameters;
        }
        string queryTabulador_Puestos()
        {
            return
                string.Format(@"
                insert into sn_tabulador_puesto
                (
                    tabulador,
                    puesto,
                    salario_base,
                    complemento,
                    bono_de_zona,
                    year
                )
                values(?,?,?,?,?,?)");
        }
        List<OdbcParameterDTO> parametrosTabulador_Puestos(tblRH_PP_PlantillaPersonal_Det o)
        {
            var parameters = new List<OdbcParameterDTO>();
            parameters.Add(new OdbcParameterDTO() { nombre = "tabulador", tipo = OdbcType.Decimal, valor = o.plantilla.tabuladorEKID });
            parameters.Add(new OdbcParameterDTO() { nombre = "puesto", tipo = OdbcType.Decimal, valor = o.puestoNumero });
            parameters.Add(new OdbcParameterDTO() { nombre = "salario_base", tipo = OdbcType.Decimal, valor = o.sueldoBase });
            parameters.Add(new OdbcParameterDTO() { nombre = "complemento", tipo = OdbcType.Decimal, valor = o.sueldoComplemento });
            parameters.Add(new OdbcParameterDTO() { nombre = "bono_de_zona", tipo = OdbcType.Decimal, valor = 0 });
            parameters.Add(new OdbcParameterDTO() { nombre = "year", tipo = OdbcType.Numeric, valor = o.plantilla.fechaInicio.Year });
            return parameters;
        }

        private bool insertEnvioGestor(int plantillaID)
        {
            try
            {
                var empresa = vSesiones.sesionEmpresaActual;
                var plantilla = _context.tblRH_PP_PlantillaPersonal.FirstOrDefault(x => x.id == plantillaID);
                using (var ctx = new MainContext((int)EmpresaEnum.Construplan))
                {                    
                    tblFM_EnvioDocumento documento = new tblFM_EnvioDocumento();
                    documento.id = 0;
                    documento.documentoID = plantillaID;
                    documento.descripcion = plantilla.cc;
                    documento.tipoDocumento = 16;
                    documento.usuarioID = plantilla.usuarioID;
                    documento.estatus = 0;
                    documento.empresa = empresa;
                    documento.fecha = DateTime.Today;
                    ctx.tblFM_EnvioDocumento.Add(documento);
                    ctx.SaveChanges();
                    return true;
                }
            }
            catch (Exception e)
            {
                return false;
            }
        }

    }
}
