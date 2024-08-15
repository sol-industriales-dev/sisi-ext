using Core.DAO.RecursosHumanos.Captura;
using Core.Entity.RecursosHumanos.Captura;
using Data.EntityFramework.Context;
using Data.EntityFramework.Generic;
using Core.DTO.Principal.Generales;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Entity.RecursosHumanos.Catalogo;
using Core.DTO.RecursosHumanos;
using Core.DTO;
using Core.Entity.Principal.Usuarios;
using Core.Entity.Principal.Alertas;
using Core.Enum.Principal.Alertas;
using Data.Factory.Principal.Alertas;
using Infrastructure.Utils;
using System.Globalization;
using Core.Enum.Principal;
using Core.DAO.Enkontrol.General.CC;
using Data.Factory.Enkontrol.General.CC;
using Core.DTO.Utils.Data;
using Core.Enum.Multiempresa;
using Core.Entity.Administrativo.RecursosHumanos.Enkontrol;

namespace Data.DAO.RecursosHumanos.Captura
{
    public class FiniquitoDAO : GenericDAO<tblRH_Finiquito>, IFiniquito
    {
        ICCDAO _ccDAO = new CCFactoryService().getCCService();

        public tblRH_Finiquito getEmpleadoForId(int idEmpleado)
        {
            tblRH_Finiquito resultado = new tblRH_Finiquito();

            List<tblRH_Finiquito> listaEmpleados = InfEmpleado(idEmpleado);

            foreach (tblRH_Finiquito Emp in listaEmpleados)
            {
                var valid = listaEmpleados.Exists(x => x.claveEmpleado.Equals(idEmpleado));

                if (valid)
                {
                    resultado.claveEmpleado = Emp.claveEmpleado;
                    resultado.nombre = Emp.nombre;
                    resultado.ape_paterno = Emp.ape_paterno;
                    resultado.ape_materno = Emp.ape_materno;
                    resultado.fechaAlta = Emp.fechaAlta;
                    resultado.fechaBaja = Emp.estatus == false ? Emp.fechaBaja : null;
                    resultado.puestoID = Emp.puestoID;
                    resultado.puesto = Emp.puesto;
                    resultado.tipoNominaID = Emp.tipoNominaID;
                    resultado.tipoNomina = Emp.tipoNomina;
                    resultado.ccID = Emp.ccID;
                    resultado.cc = Emp.cc;
                    resultado.salarioBase = Emp.salarioBase;
                    resultado.complemento = Emp.complemento;
                    resultado.bono = Emp.bono;
                    resultado.formuloID = Emp.formuloID;
                    resultado.formuloNombre = Emp.formuloNombre;
                    resultado.voboID = Emp.voboID;
                    resultado.voboNombre = Emp.voboNombre;
                    resultado.autorizoID = Emp.autorizoID;
                    resultado.autorizoNombre = Emp.autorizoNombre;
                    resultado.total = Emp.total;
                }
                else
                {
                    resultado.claveEmpleado = 0;
                    resultado.nombre = "";
                    resultado.ape_paterno = "";
                    resultado.ape_materno = "";
                    resultado.fechaAlta = DateTime.Now;
                    resultado.fechaBaja = DateTime.Now;
                    resultado.puestoID = 0;
                    resultado.puesto = "";
                    resultado.tipoNominaID = 0;
                    resultado.tipoNomina = "";
                    resultado.ccID = "0";
                    resultado.cc = "";
                    resultado.salarioBase = 0;
                    resultado.complemento = 0;
                    resultado.bono = 0;
                    resultado.formuloID = 0;
                    resultado.formuloNombre = "";
                    resultado.voboID = 0;
                    resultado.voboNombre = "";
                    resultado.autorizoID = 0;
                    resultado.autorizoNombre = "";
                    resultado.total = 0;
                }
            }

            return resultado;
        }

        private List<tblRH_Finiquito> InfEmpleado(int idEmpleado)
        {
            string empleado = @"select top 1 
                                    e.clave_empleado as claveEmpleado
                                    , e.nombre as nombre
                                    , e.ape_paterno as ape_paterno
                                    , e.ape_materno as ape_materno
                                    , cast(case when e.estatus_empleado = 'A' then 1 else 0 end as bit) as estatus
                                    , e.fecha_antiguedad as fechaAlta
                                    , (select top 1 fecha_baja FROM tblRH_EK_Empl_Baja as b where b.clave_empleado = e.clave_empleado order by fecha_baja desc) as fechaBaja
                                    , e.puesto as puestoID
                                    , p.descripcion as puesto
                                    , e.tipo_nomina as tipoNominaID
                                    , tn.descripcion as tipoNomina
                                    , e.cc_contable as ccID
                                    , s.salario_base as salarioBase
                                    , s.complemento as complemento
                                    , s.bono_zona as bono 
                                from tblRH_EK_Empleados as e
                                    inner join tblRH_EK_Puestos as p on e.puesto = p.puesto
                                    inner join tblRH_EK_Tipos_Nomina as tn on e.tipo_nomina = tn.tipo_nomina
                                    inner join tblRH_EK_Tabulador_Historial as s on e.clave_empleado = s.clave_empleado
                                where e.estatus_empleado != 'C' and e.clave_empleado = " + idEmpleado +
                              @"order by s.id desc";

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

            //var resultado = (List<tblRH_Finiquito>)ContextEnKontrolNomina.Where(empleado).ToObject<List<tblRH_Finiquito>>();
            var resultado = _context.Select<tblRH_Finiquito>(new DapperDTO
            {
                baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
                consulta = empleado
            }).ToList();

            var ccs = _context.tblP_CC.ToList();
            //var ccs = _ccDAO.GetCCs();

            foreach (var item in resultado)
            {
                var cc = ccs.FirstOrDefault(x => x.cc == item.ccID);
                if (cc != null)
                {
                    item.cc = cc.descripcion;
                }
            }

            return resultado;
        }

        public List<ComboDTO> getListaConceptos()
        {
            var conceptos = _context.tblRH_FiniquitoConceptos.Where(x => x.estatus == true && x.id != 1).Select(y => new ComboDTO
            {
                Value = y.id.ToString(),
                Text = ((y.operador == true) ? "(+) " + y.concepto : "(-) " + y.concepto),
                Prefijo = y.operador.ToString()
            }).ToList();

            return conceptos;
        }

        public tblRH_Finiquito GuardarFiniquito(tblRH_Finiquito general, List<tblRH_FiniquitoDetalle> detalle)
        {
            //var checkFiniquito = _context.tblRH_Finiquito.Where(x => x.claveEmpleado == general.claveEmpleado).FirstOrDefault();

            //if (checkFiniquito == null)
            //{
            var infoEmpleado = getEmpleadoForId(general.claveEmpleado);

            var voboNombre = general.voboNombre != null ? general.voboNombre.Replace(" ", "") : "";
            var voboUsuario = _context.tblP_Usuario.Where(x => x.estatus == true && (x.nombre.Replace(" ", "") + x.apellidoPaterno.Replace(" ", "") + x.apellidoMaterno.Replace(" ", "")) == voboNombre).FirstOrDefault();

            var autorizaNombre = general.autorizoNombre != null ? general.autorizoNombre.Replace(" ", "") : "";
            var autorizaUsuario = _context.tblP_Usuario.Where(x => x.estatus == true && (x.nombre.Replace(" ", "") + x.apellidoPaterno.Replace(" ", "") + x.apellidoMaterno.Replace(" ", "")) == autorizaNombre).FirstOrDefault();

            tblRH_Finiquito fin = new tblRH_Finiquito();

            fin.claveEmpleado = infoEmpleado.claveEmpleado;
            fin.nombre = infoEmpleado.nombre;
            fin.ape_paterno = infoEmpleado.ape_paterno;
            fin.ape_materno = infoEmpleado.ape_materno;
            fin.fechaAlta = infoEmpleado.fechaAlta;
            fin.fechaBaja = general.fechaBaja;
            fin.puestoID = infoEmpleado.puestoID;
            fin.puesto = infoEmpleado.puesto;
            fin.tipoNominaID = infoEmpleado.tipoNominaID;
            fin.tipoNomina = infoEmpleado.tipoNomina;
            fin.ccID = infoEmpleado.ccID;
            fin.cc = infoEmpleado.cc;
            fin.salarioBase = infoEmpleado.salarioBase;
            fin.complemento = infoEmpleado.complemento;
            fin.bono = infoEmpleado.bono;

            fin.formuloID = general.formuloID;
            fin.formuloNombre = general.formuloNombre;

            fin.voboID = voboUsuario != null ? voboUsuario.id : 0;
            fin.voboNombre = general.voboNombre != null ? general.voboNombre : "";

            fin.autorizoID = autorizaUsuario != null ? autorizaUsuario.id : 0;
            fin.autorizoNombre = general.autorizoNombre != null ? general.autorizoNombre : "";

            fin.total = general.total;
            fin.estatus = true;
            fin.autorizado = 1;

            _context.tblRH_Finiquito.Add(fin);
            _context.SaveChanges();

            //SaveEntity(general);

            foreach (var det in detalle)
            {
                tblRH_FiniquitoDetalle finDet = new tblRH_FiniquitoDetalle();

                finDet.conceptoID = det.conceptoID;
                finDet.conceptoInfo = det.conceptoInfo != null ? det.conceptoInfo : "";
                finDet.operacion1 = det.operacion1;
                finDet.operacion2 = det.operacion2;
                finDet.operacion3 = det.operacion3;
                finDet.operacion4 = det.operacion4;
                finDet.conceptoDetalle = det.conceptoDetalle != null ? det.conceptoDetalle : "";
                finDet.resultado = det.resultado;
                finDet.estatus = true;
                finDet.finiquitoID = fin.id;

                _context.tblRH_FiniquitoDetalle.Add(finDet);
                _context.SaveChanges();

                //SaveEntity(det);
            }

            var orden = 1;
            if (fin.voboID != 0)
            {
                var voboEmpleado = _context.tblP_Usuario.Where(x => x.id == fin.voboID).FirstOrDefault();

                tblRH_FiniquitoFirmas firma = new tblRH_FiniquitoFirmas();

                firma.finiquitoID = fin.id;
                firma.fecha = DateTime.Now.Date;
                firma.usuarioID = fin.voboID;
                firma.nombre = voboEmpleado.nombre;
                firma.ape_paterno = voboEmpleado.apellidoPaterno;
                firma.ape_materno = voboEmpleado.apellidoMaterno;
                firma.estatus = false;
                firma.autorizando = true;
                firma.rechazado = false;
                firma.orden = orden;

                _context.tblRH_FiniquitoFirmas.Add(firma);
                _context.SaveChanges();

                orden++;

                var objAlerta = new tblP_Alerta();
                objAlerta.msj = "Autorización de Finiquito para empleado: " + infoEmpleado.nombre + " " + infoEmpleado.ape_paterno + " " + infoEmpleado.ape_materno;
                objAlerta.sistemaID = (int)SistemasEnum.RH;
                objAlerta.tipoAlerta = (int)AlertasEnum.MENSAJE;
                objAlerta.url = "/Administrativo/Finiquito/GestionFiniquito?obj=" + fin.id + "";
                objAlerta.userEnviaID = general.formuloID;
                objAlerta.userRecibeID = fin.voboID;
                objAlerta.objID = fin.id;
                AlertaFactoryServices alertaFactoryServices = new AlertaFactoryServices();
                alertaFactoryServices.getAlertaService().saveAlerta(objAlerta);
            }

            if (fin.autorizoID != 0)
            {
                var autorizoEmpleado = _context.tblP_Usuario.Where(x => x.id == fin.autorizoID).FirstOrDefault();

                tblRH_FiniquitoFirmas firma = new tblRH_FiniquitoFirmas();

                firma.finiquitoID = fin.id;
                firma.fecha = DateTime.Now.Date;
                firma.usuarioID = fin.autorizoID;
                firma.nombre = autorizoEmpleado.nombre;
                firma.ape_paterno = autorizoEmpleado.apellidoPaterno;
                firma.ape_materno = autorizoEmpleado.apellidoMaterno;
                firma.estatus = false;
                firma.autorizando = false;
                firma.rechazado = false;
                firma.orden = orden;

                _context.tblRH_FiniquitoFirmas.Add(firma);
                _context.SaveChanges();

                orden++;
            }
            return fin;
            //}
            //else
            //{
            //    return null;
            //}
        }

        public List<FiniquitoDTO> getFiniquitos(int clave, string nombre, string cc, int aut)
        {
            //var autorizacionesFiniquitoID = _context.tblRH_FiniquitoFirmas.Where(x => x.usuarioID == vSesiones.sesionUsuarioDTO.id).Select(y => y.finiquitoID).ToList();

            var nombreTrim = nombre.Trim().ToUpper();
            var ccTrim = cc.Trim().ToUpper();
            var data = _context.tblRH_Finiquito.ToList().Where(x =>
                x.estatus == true &&
                    //autorizacionesFiniquitoID.Contains(x.id) &&
                ((clave != 0) ? x.claveEmpleado == clave : true) &&
                (x.nombre.Contains(nombreTrim) || x.ape_paterno.Contains(nombreTrim) || x.ape_materno.Contains(nombreTrim)) &&
                x.cc.Contains(ccTrim) &&
                x.autorizado == aut).Select(y => new FiniquitoDTO
                {
                    id = y.id,
                    claveEmpleado = y.claveEmpleado,
                    nombre = y.nombre,
                    ape_paterno = y.ape_paterno,
                    ape_materno = y.ape_materno,
                    nombreCompleto = (y.ape_paterno != null && y.ape_materno != null) ? y.nombre + " " + y.ape_paterno + " " + y.ape_materno : (y.ape_paterno != null) ? y.nombre + " " + y.ape_paterno : y.nombre,
                    fechaAlta = y.fechaAlta,
                    fechaBaja = y.fechaBaja,
                    puestoID = y.puestoID,
                    puesto = y.puesto,
                    tipoNominaID = y.tipoNominaID,
                    tipoNomina = y.tipoNomina,
                    ccID = y.ccID,
                    cc = y.cc,
                    salarioBase = y.salarioBase,
                    complemento = y.complemento,
                    bono = y.bono,
                    formuloID = y.formuloID,
                    formuloNombre = y.formuloNombre,
                    voboID = y.voboID,
                    voboNombre = y.voboNombre,
                    autorizoID = y.autorizoID,
                    autorizoNombre = y.autorizoNombre,
                    total = y.total,
                    autorizado = y.autorizado
                }).ToList();

            return (List<FiniquitoDTO>)data;
        }

        public FiniquitoDTO GetDetalleFin(int id)
        {
            var detalle = _context.tblRH_FiniquitoDetalle.Where(x => x.estatus == true && x.finiquitoID == id).Select(y => new FiniquitoDetalleDTO
            {
                id = y.id,
                conceptoID = y.conceptoID,
                conceptoNombre = _context.tblRH_FiniquitoConceptos.Where(z => z.id == y.conceptoID).Select(w => w.concepto).FirstOrDefault(),
                conceptoInfo = y.conceptoInfo,
                operacion1 = y.operacion1,
                operacion2 = y.operacion2,
                operacion3 = y.operacion3,
                operacion4 = y.operacion4,
                conceptoDetalle = y.conceptoDetalle,
                resultado = y.resultado,
                estatus = y.estatus,
                finiquitoID = y.finiquitoID
            }).ToList();

            var finiquito = _context.tblRH_Finiquito.ToList().Where(x => x.estatus == true && x.id == id).Select(y => new FiniquitoDTO
            {
                id = y.id,
                claveEmpleado = y.claveEmpleado,
                nombre = y.nombre,
                ape_paterno = y.ape_paterno,
                ape_materno = y.ape_materno,
                fechaAlta = y.fechaAlta,
                fechaBaja = y.fechaBaja,
                puestoID = y.puestoID,
                puesto = y.puesto,
                tipoNominaID = y.tipoNominaID,
                tipoNomina = y.tipoNomina,
                ccID = y.ccID,
                cc = y.cc,
                salarioBase = y.salarioBase,
                complemento = y.complemento,
                bono = y.bono,
                formuloID = y.formuloID,
                formuloNombre = y.formuloNombre,
                voboID = y.voboID,
                voboNombre = y.voboNombre,
                autorizoID = y.autorizoID,
                autorizoNombre = y.autorizoNombre,
                total = y.total,
                autorizado = y.autorizado,
                detalle = detalle
            }).FirstOrDefault();

            return finiquito;
        }

        public List<FiniquitoFirmasDTO> GetAutorizaciones(int finiquitoID)
        {
            var checkAutorizando = _context.tblRH_FiniquitoFirmas.ToList().Where(x => x.finiquitoID == finiquitoID && x.autorizando == true).FirstOrDefault();
            var flagAutorizando = false;
            if (checkAutorizando != null)
            {
                flagAutorizando = true;
            }
            else
            {
                flagAutorizando = false;
            }

            var data = _context.tblRH_FiniquitoFirmas.ToList().Where(x => x.finiquitoID == finiquitoID).Select(y => new FiniquitoFirmasDTO
            {
                id = y.id,
                finiquitoID = y.finiquitoID,
                usuarioID = y.usuarioID,
                nombre = y.nombre,
                ape_paterno = y.ape_paterno,
                ape_materno = y.ape_materno,
                puesto = _context.tblP_Usuario.ToList().Where(z => z.id == y.usuarioID).Select(w => w.puesto.descripcion).FirstOrDefault(),
                estatus = y.estatus,
                autorizando = y.autorizando,
                rechazado = y.rechazado,

                estadoAut = (y.estatus == true) ? "AUTORIZADO" : (y.autorizando == true) ? "AUTORIZANDO" : (y.rechazado == true) ? "RECHAZADO" : (flagAutorizando == true) ? "PENDIENTE" : "",

                orden = y.orden,
                usuarioActual = vSesiones.sesionUsuarioDTO.id,
                checkUsuario = (y.usuarioID == vSesiones.sesionUsuarioDTO.id && y.autorizando == true) ? true : false
            }).OrderBy(z => z.orden).ToList();

            return data;
        }

        public tblRH_Finiquito AutorizaFiniquito(int aut, int finiquitoID)
        {
            var autList = _context.tblRH_FiniquitoFirmas.Where(x => x.finiquitoID == finiquitoID).OrderBy(y => y.orden).ToList();
            var lastrow = autList.Last();
            var rowTemp = autList.FirstOrDefault(x => x.usuarioID == vSesiones.sesionUsuarioDTO.id && x.autorizando == true);
            var flagLast = false;
            if (rowTemp == lastrow)
            {
                flagLast = true;
            }
            else
            {
                flagLast = false;
            }

            var alertaVisto = _context.tblP_Alerta.Where(x => x.sistemaID == 7 && x.userRecibeID == vSesiones.sesionUsuarioDTO.id && x.objID == finiquitoID).FirstOrDefault();
            var finiquito = _context.tblRH_Finiquito.Where(x => x.id == finiquitoID).FirstOrDefault();

            switch (aut)
            {
                case 1:
                    rowTemp.estatus = true;
                    rowTemp.autorizando = false;
                    rowTemp.rechazado = false;

                    _context.Entry(rowTemp).State = System.Data.Entity.EntityState.Modified;
                    _context.SaveChanges();

                    alertaVisto.visto = true;
                    _context.SaveChanges();

                    if (flagLast == false)
                    {
                        var nextRow = autList.Where(x => x.id == rowTemp.id + 1).FirstOrDefault();

                        nextRow.autorizando = true;
                        _context.Entry(nextRow).State = System.Data.Entity.EntityState.Modified;
                        _context.SaveChanges();

                        var objAlerta = new tblP_Alerta();
                        objAlerta.msj = "Autorización de Finiquito para empleado: " + finiquito.nombre + " " + finiquito.ape_paterno + " " + finiquito.ape_materno;
                        objAlerta.sistemaID = (int)SistemasEnum.RH;
                        objAlerta.tipoAlerta = (int)AlertasEnum.MENSAJE;
                        objAlerta.url = "/Administrativo/Finiquito/GestionFiniquito?obj=" + finiquito.id + "";
                        objAlerta.userEnviaID = rowTemp.usuarioID;
                        objAlerta.userRecibeID = nextRow.usuarioID;
                        objAlerta.objID = finiquito.id;
                        AlertaFactoryServices alertaFactoryServices = new AlertaFactoryServices();
                        alertaFactoryServices.getAlertaService().saveAlerta(objAlerta);

                        //try
                        //{
                        //    var c = _context.tblP_Usuario.FirstOrDefault(x => x.id == finiquito.voboID);
                        //    var subject = "Autorización de Finiquito para empleado: " + finiquito.nombre + " " + finiquito.ape_paterno + " " + finiquito.ape_materno;

                        //    #region imagen
                        //    //var body = @"<img src='data:image/png;base64,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'/><br/><br/>";
                        //    #endregion

                        //    var body = @"Estimado(a): " + c.nombre.ToUpper() + " " + c.apellidoPaterno.ToUpper() + " " + c.apellidoMaterno.ToUpper() + "<br/>Se requiere su autorización para el finiquito del empleado: " + finiquito.nombre + " " + finiquito.ape_paterno + " " + finiquito.ape_materno + " .<br/>Link: http://sigoplan.construplan.com.mx/Administrativo/Finiquito/GestionFiniquito" + "<br/><br/>";
                        //    body += @"Grupo Construcciones Planificadas S.A. de C.V.<br/>Por favor no responda a esta dirección de correo, si necesita contactarnos puede llamar al (52) 662-108-0500.";
                        //    List<string> contactos = new List<string>();

                        //    contactos.Add(c.correo);
                        //    GlobalUtils.sendEmail(subject, body, contactos);
                        //}
                        //catch (Exception e)
                        //{
                        //}

                        return finiquito;
                    }
                    else
                    {
                        finiquito.autorizado = 2;

                        _context.Entry(finiquito).State = System.Data.Entity.EntityState.Modified;
                        _context.SaveChanges();

                        //var usuariosAlerta = db.tbl_Usuario.Where(x => x.estatus == true && (x.perfilID == 1 || x.perfilID == 9 || x.perfilID == 25)).ToList();

                        //foreach (var usu in usuariosAlerta)
                        //{
                        //    tbl_Alerta ale = new tbl_Alerta();

                        //    ale.userEnviaID = null;
                        //    ale.userRecibeID = usu.id;
                        //    ale.perfilRecibeID = db.tbl_Usuario.Where(x => x.id == usu.id).Select(y => y.perfilID).FirstOrDefault();
                        //    ale.enviado = true;
                        //    ale.visto = false;
                        //    ale.categoriaAlerta = 7;
                        //    ale.objetoID = compraID;
                        //    ale.mensaje = "Se ha autorizado la compra: '" + db.tbl_Compra.Where(x => x.id == compraID).Select(y => y.asunto).FirstOrDefault() + "'";

                        //    db.tbl_Alerta.Add(ale);
                        //    db.SaveChanges();
                        //}

                        return null;
                    }

                    break;
                case 2:
                    rowTemp.estatus = false;
                    rowTemp.autorizando = false;
                    rowTemp.rechazado = true;

                    _context.Entry(rowTemp).State = System.Data.Entity.EntityState.Modified;
                    _context.SaveChanges();

                    alertaVisto.visto = true;
                    _context.SaveChanges();

                    finiquito.autorizado = 3;

                    _context.Entry(finiquito).State = System.Data.Entity.EntityState.Modified;
                    _context.SaveChanges();
                    return null;
                    break;
                default:
                    rowTemp.estatus = false;
                    rowTemp.autorizando = true;
                    rowTemp.rechazado = false;

                    _context.Entry(rowTemp).State = System.Data.Entity.EntityState.Modified;
                    _context.SaveChanges();
                    return null;
                    break;
            }
        }

        public tblP_Usuario getUsuario(int id)
        {
            return _context.tblP_Usuario.FirstOrDefault(x => x.id == id);
        }

        public List<tblRH_FiniquitoConceptos> getConceptos()
        {
            var data = _context.tblRH_FiniquitoConceptos.Where(x => x.estatus == true).ToList();

            return data;
        }

        public List<tblRH_CatEmpleados> getEmpleadosTodos(string term)
        {
            var getCatEmpleado = "SELECT TOP 10 clave_empleado, (LTRIM(RTRIM(nombre))+' '+replace(ape_paterno, ' ', '')+' '+replace(ape_materno, ' ', '')) AS Nombre, puesto FROM tblRH_EK_Empleados WHERE estatus_empleado != 'C' AND replace((nombre+' '+ape_paterno+' '+ape_materno), ' ', '') LIKE '%" + term.Replace(" ", "") + "%'";
            try
            {
                //var resultado = (List<tblRH_CatEmpleados>)ContextEnKontrolNomina.Where(getCatEmpleado).ToObject<List<tblRH_CatEmpleados>>();
                var resultado = _context.Select<tblRH_CatEmpleados>(new DapperDTO
                {
                    //baseDatos = vSesiones.sesionEmpresaActual == (int)EmpresaEnum.Construplan ? MainContextEnum.Construplan : MainContextEnum.Arrendadora,
                    baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
                    consulta = getCatEmpleado
                }).ToList();
                return resultado;
            }
            catch
            {
                return new List<tblRH_CatEmpleados>();
            }
        }

        public tblRH_FiniquitoConceptos GetDetalleConcepto(int id)
        {
            var concepto = _context.tblRH_FiniquitoConceptos.Where(x => x.estatus == true && x.id == id).FirstOrDefault();

            return concepto;
        }

        public void GuardarConcepto(string concepto, string detalle, bool operador)
        {
            tblRH_FiniquitoConceptos con = new tblRH_FiniquitoConceptos();

            con.concepto = concepto;
            con.detalle = detalle;
            con.operador = operador;
            con.estatus = true;

            _context.tblRH_FiniquitoConceptos.Add(con);
            _context.SaveChanges();
        }

        public void UpdateConcepto(int id, string concepto, string detalle, bool operador)
        {
            var rowTemp = _context.tblRH_FiniquitoConceptos.Where(x => x.estatus == true && x.id == id).FirstOrDefault();

            if (rowTemp != null)
            {
                rowTemp.concepto = concepto;
                rowTemp.detalle = detalle;
                rowTemp.operador = operador;

                _context.Entry(rowTemp).State = System.Data.Entity.EntityState.Modified;
                _context.SaveChanges();
            }
        }

        public void RemoveConcepto(int id)
        {
            var rowTemp = _context.tblRH_FiniquitoConceptos.Where(x => x.estatus == true && x.id == id).FirstOrDefault();

            if (rowTemp != null)
            {
                rowTemp.estatus = false;

                _context.Entry(rowTemp).State = System.Data.Entity.EntityState.Modified;
                _context.SaveChanges();
            }
        }

        public decimal GetPrimaAntiguedad(string ingreso, string egreso)
        {
            decimal resultado = 0;

            var ingresoDate = DateTime.Parse(ingreso);
            var egresoDate = DateTime.Parse(egreso);

            decimal anios = 0;
            decimal diasTotales = egresoDate.Subtract(ingresoDate).Days;
            anios = (diasTotales / 365);
            var anios2 = anios.ToString("0.##");
            var anios3 = Convert.ToDecimal(anios2, CultureInfo.InvariantCulture);

            //var aniosEntero = (int)Math.Floor(anios3);

            var salariominX2 = _context.tblRH_FiniquitoSalarioMin.Where(x => x.estatus == true).Select(y => y.salarioMinimo).FirstOrDefault() * 2;

            //var mesIngreso = ingresoDate.Month;
            //var anioIngreso = ingresoDate.Year;
            //var mesEgreso = egresoDate.Month;
            //var anioEgreso = egresoDate.Year;

            //var ultimoAnioCompleto = anioIngreso + aniosEntero;
            //DateTime ingresoTemp = new DateTime(ultimoAnioCompleto, mesIngreso, 1);
            //DateTime egresoTemp = new DateTime(egresoDate.Year, egresoDate.Month, 1);
            //decimal diasRestantes = egresoTemp.Subtract(ingresoTemp).Days;

            //var diasEnteros = 12 * aniosEntero;
            //decimal diaPrima = 12m / 365m;
            //decimal diasProp = diaPrima * diasRestantes;

            //resultado = salariominX2 * (diasEnteros + diasProp);

            resultado = (anios * 12) * salariominX2;

            return resultado;
        }
    }
}
