using Core.DAO.Maquinaria.Captura;
using Core.DTO;
using Core.DTO.Maquinaria.Captura.aceites;
using Core.DTO.Utils.Data;
using Core.Entity.Maquinaria.Captura;
using Core.Entity.Maquinaria.Catalogo;
using Core.Entity.Maquinaria.StandBy;
using Core.Enum.Maquinaria.StandBy;
using Core.Enum.Multiempresa;
using Core.Enum.Principal;
using Core.Enum.Principal.Bitacoras;
using Data.EntityFramework.Context;
using Data.EntityFramework.Generic;
using Data.Factory.Maquinaria;
using Infrastructure.Utils;
using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Data.DAO.Maquinaria.Captura
{
    public class MaquinariaAceitesLubricantesDAO : GenericDAO<tblM_MaquinariaAceitesLubricantes>, IMaquinariaAceitesLubricantesDAO
    {
        public tblM_MaquinariaAceitesLubricantes GuardarMaqAceiteLubricante(tblM_MaquinariaAceitesLubricantes obj)
        {
            try
            {


                if (obj.id == 0)
                {
                    var horometro = _context.tblM_CapHorometro.Where(x => x.Economico == obj.Economico).ToList().Where(x => x.Fecha <= obj.Fecha).OrderByDescending(x => x.id);



                    if (horometro.Count() > 0)
                    {
                        obj.Horometro = horometro.FirstOrDefault().Horometro;
                    }
                    SaveEntity(obj, (int)BitacoraEnum.MaquinaAceiteLubricante);

                    #region ACTIVACION DE EQUIPO DE STANDBY
                    ActivarEconomicoPorAccionRealizada
                        (
                            obj.Economico,
                            null,
                            AccionActivacionEconomicoEnum.CAPTURA_ACEITE,
                            new { Economico = obj.Economico, esActualizacion = false, turno = obj.Turno, fecha = obj.Fecha.ToShortDateString(), cc = obj.CC }, false);
                    #endregion
                }

                else
                {
                    Update(obj, obj.id, (int)BitacoraEnum.MaquinaAceiteLubricante);

                    #region ACTIVACION DE EQUIPO DE STANDBY
                    ActivarEconomicoPorAccionRealizada
                        (
                            obj.Economico,
                            null,
                            AccionActivacionEconomicoEnum.CAPTURA_ACEITE,
                            new { Economico = obj.Economico, esActualizacion = true, turno = obj.Turno, fecha = obj.Fecha.ToShortDateString(), cc = obj.CC }, false);
                    #endregion
                }
            }
            catch (Exception e)
            {
                return new tblM_MaquinariaAceitesLubricantes();
            }
            return obj;
        }

        private bool ActivarEconomicoPorAccionRealizada(string numeroEconomico, int? idEconomico, AccionActivacionEconomicoEnum accion, object objeto, bool buscarEnEnkontrol = false)
        {
            if (vSesiones.sesionEmpresaActual != (int)EmpresaEnum.Arrendadora)
            {
                return false;
            }

            tblM_CatMaquina maquina = null;

            if (buscarEnEnkontrol)
            {
                if (!string.IsNullOrEmpty(numeroEconomico))
                {
                    var queryEk = new OdbcConsultaDTO();
                    queryEk.consulta = "SELECT * FROM cc WHERE cc = ?";
                    queryEk.parametros.Add(new OdbcParameterDTO
                    {
                        nombre = "cc",
                        tipo = OdbcType.NVarChar,
                        valor = numeroEconomico
                    });
                    var ccDescripcion = _contextEnkontrol.Select<dynamic>(vSesiones.sesionAmbienteEnkontrolAdm, queryEk).FirstOrDefault();

                    if (ccDescripcion != null)
                    {
                        numeroEconomico = (string)ccDescripcion.descripcion;
                    }
                }
                else
                {
                    throw new Exception("Se tiene que indicar un CC");
                }
            }

            if (!string.IsNullOrEmpty(numeroEconomico))
            {
                maquina = _context.tblM_CatMaquina.FirstOrDefault(x => x.noEconomico == numeroEconomico && x.estatus == 2); //x.estatus == 2 significa que esta en StandBy
            }
            else if (idEconomico.HasValue)
            {
                maquina = _context.tblM_CatMaquina.FirstOrDefault(x => x.id == idEconomico.Value && x.estatus == 2); //x.estatus == 2 significa que esta en StandBy
            }

            if (maquina != null)
            {
                if (_context.tblM_STB_EconomicoBloqueado.Any(x => x.noEconomico == maquina.noEconomico && x.registroActivo))
                {
                    throw new Exception("No es posible realizar la acción puesto que el equipo referenciado se encuentra bloqueado por estatus StandBy");
                }

                var standBy = _context.tblM_STB_CapturaStandBy
                    .FirstOrDefault(x =>
                        x.noEconomicoID == maquina.id &&
                        x.estatus == 2 //Autorizado
                    );

                if (standBy != null)
                {
                    string motivoLiberacion = "";
                    maquina.estatus = 1;
                    standBy.estatus = 4; //Liberado
                    standBy.usuarioLiberaID = vSesiones.sesionUsuarioDTO.id;
                    standBy.fechaLibera = DateTime.Now;
                    standBy.comentarioLiberacion = "Se liberó por sistema - ";
                    switch (accion)
                    {
                        case AccionActivacionEconomicoEnum.ELABORACION_REQUISICION:
                            standBy.comentarioLiberacion += "Se realizó una requisición";
                            motivoLiberacion = "elaboración de requisición";
                            break;
                        case AccionActivacionEconomicoEnum.ELABORACION_ORDEN_COMPRA:
                            standBy.comentarioLiberacion += "Se realizó una orden de compra";
                            motivoLiberacion = "elaboración de orden de compra";
                            break;
                        case AccionActivacionEconomicoEnum.CAPTURA_HOROMETROS:
                            standBy.comentarioLiberacion += "Se capturó horómetros";
                            motivoLiberacion = "captura de horómetros";
                            break;
                        case AccionActivacionEconomicoEnum.CAPTURA_COMBUSTIBLE:
                            standBy.comentarioLiberacion += "Se capturó combustible";
                            motivoLiberacion = "captura de combustible";
                            break;
                        case AccionActivacionEconomicoEnum.CAPTURA_ACEITE:
                            standBy.comentarioLiberacion += "Se capturó aceite";
                            motivoLiberacion = "captura de aceite";
                            break;
                        case AccionActivacionEconomicoEnum.RECEPCION_FACTURA:
                            standBy.comentarioLiberacion += "Por recepción de factura";
                            motivoLiberacion = "recepción de factura";
                            break;
                        case AccionActivacionEconomicoEnum.SALIDA_ALMACEN:
                            standBy.comentarioLiberacion += "Por salida de almacén";
                            motivoLiberacion = "salida de almacén";
                            break;
                    }

                    var bitacora = new tblM_STB_BitacoraActivacionEconomico();
                    bitacora.economicoId = maquina.id;
                    bitacora.fechaAccion = DateTime.Now;
                    bitacora.motivoActivacionId = (int)accion;
                    bitacora.usuarioAccionId = vSesiones.sesionUsuarioDTO.id;
                    bitacora.objeto = JsonUtils.convertNetObjectToJson(objeto);
                    _context.tblM_STB_BitacoraActivacionEconomico.Add(bitacora);
                    _context.SaveChanges();

                    var correos = new List<string>();
                    var correosCC = new List<string>();

                    var adminsGerentes = _context.Select<Core.DTO.Maquinaria.StandBy.AutorizanteDTO>(new DapperDTO
                    {
                        baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
                        consulta = @"SELECT
                                                u.id,
                                                u.nombre,
                                                u.apellidoPaterno,
                                                u.apellidoMaterno,
                                                u.correo,
                                                c.cc as ac,
                                                a.perfilAutorizaID
                                            FROM
                                                tblP_Autoriza AS a
                                            INNER JOIN
                                                tblP_Usuario AS u ON u.id = a.usuarioID
                                            INNER JOIN
                                                tblP_CC_Usuario AS c ON c.id = a.cc_usuario_ID
                                            WHERE
                                                u.estatus = 1 AND
                                                a.perfilAutorizaID in (5, 1) AND /*5 == Admin, 1 == Gerente*/
                                                c.cc = @paramCC",
                        parametros = new { paramCC = standBy.ccActual }
                    });

                    correosCC.AddRange(adminsGerentes.Select(x => x.correo).Distinct().ToList());

                    correos.Add("oscar.roman@construplan.com.mx");
                    correosCC.Add("g.reina@construplan.com.mx");
                    correosCC.Add("e.encinas@construplan.com.mx");
                    correosCC.Add("luis.fortino@construplan.com.mx");
                    correosCC.Add("martin.valle@construplan.com.mx");
                    correosCC.Add("alan.palomera@construplan.com.mx");
                    correosCC.Add("diego.gonzalez@construplan.com.mx");
#if DEBUG
                    correos = new List<string> { "martin.zayas@construplan.com.mx" };
                    correosCC = new List<string> { "martin.zayas@construplan.com.mx" };
#endif
                    var cc = _context.tblP_CC.FirstOrDefault(x => x.areaCuenta == maquina.centro_costos);
                    var ccDescripcion = cc != null ? cc.descripcion.Trim() : maquina.centro_costos;
                    string asunto = "El equipo " + maquina.noEconomico + " ha sido liberado de StandBy por " + motivoLiberacion;
                    string contenido = string.Format(@"
                        <p>Buen día.</p>
                        <p>El equipo <strong>{0}</strong> ha sido liberado de StandBy por {1} </p>
                        <p>El equipo se encuentra en <strong>{2}.</strong>", maquina.noEconomico, motivoLiberacion, ccDescripcion);

                    var envioCorrecto = EnviarCorreo(new Infrastructure.DTO.CorreoDTO
                    {
                        asunto = asunto,
                        cuerpo = contenido,
                        correos = correos,
                        correosCC = correosCC
                    });

                    if (!envioCorrecto)
                    {
                        throw new Exception("Error al enviar correo de liberación de StandBy");
                    }

                    return true;
                }
                else
                {
                    throw new Exception("El económico esta en StandBy pero no se encuentra su registro autorizado");
                }
            }

            return false;
        }

        private bool EnviarCorreo(Infrastructure.DTO.CorreoDTO correo)
        {
            if (correo.correos == null || correo.correos.Count == 0 || string.IsNullOrEmpty(correo.asunto) || string.IsNullOrEmpty(correo.cuerpo))
            {
                return false;
            }

            MailMessage mailMessage = new MailMessage();

            correo.correos.ForEach(c => mailMessage.To.Add(new MailAddress(c)));
            correo.correosCC.ForEach(c => mailMessage.CC.Add(new MailAddress(c)));
            correo.archivos.ForEach(archivo => mailMessage.Attachments.Add(archivo));

            mailMessage.IsBodyHtml = true;
            mailMessage.From = new MailAddress("alertas.sigoplan@construplan.com.mx");
            mailMessage.Subject = correo.asunto;
            mailMessage.Body = string.Format(@"
                {0} 
                <p><o:p>&nbsp;</o:p></p>
                <p><o:p>&nbsp;</o:p></p>
                <p>Se informa que esta es un correo autogenerado por el sistema SIGOPLAN (<a href='http://sigoplan.construplan.com.mx/'>http://sigoplan.construplan.com.mx</a>). No es necesario dar una respuesta. Gracias.</p>
            ", correo.cuerpo);

            SmtpClient smptConfig = new SmtpClient();
            smptConfig.Send(mailMessage);
            smptConfig.Dispose();

            return true;
        }

        public List<MaquinariaAceitesLubricantesDTO> GetLstMaqAceiteLubricante(string cc, string consumo, int turno, DateTime fecha, int tipo)
        {
            // Ajuste del centro de costos según la empresa
            if ((EmpresaEnum)vSesiones.sesionEmpresaActual == EmpresaEnum.Peru)
            {
                var areaCuenta = _context.tblP_CC.FirstOrDefault(x => x.cc == cc);
                if (areaCuenta != null)
                {
                    cc = areaCuenta.areaCuenta;
                }
            }

            string TrimCC = cc.TrimStart('0');
            List<MaquinariaAceitesLubricantesDTO> resultado = new List<MaquinariaAceitesLubricantesDTO>();

            // Obtener la lista de máquinas filtradas directamente desde la base de datos
            var lstMaq = _context.tblM_CatMaquina
                .Where(x => !string.IsNullOrEmpty(x.centro_costos) &&
                            (tipo == 0 || x.grupoMaquinaria.tipoEquipoID == tipo) &&
                            x.centro_costos == TrimCC &&
                            x.TipoCaptura != 0 &&
                            x.estatus != 0)
                .ToList();

            // Obtener la lista de aceites filtrados directamente desde la base de datos
            var lstAceite = _context.tblM_MaquinariaAceitesLubricantes
                .Where(x => x.CC == cc && x.Turno == turno && x.Fecha == fecha)
                .ToList();

            // Crear un diccionario para acceso rápido a los aceites por económico
            var aceiteLookup = lstAceite.GroupBy(x => x.Economico).ToDictionary(g => g.Key, g => g.ToList());

            foreach (var item in lstMaq)
            {
                string economico = item.noEconomico;
                MaquinariaAceitesLubricantesDTO dto = new MaquinariaAceitesLubricantesDTO();
                List<tblM_MaquinariaAceitesLubricantes> aceites;
                aceiteLookup.TryGetValue(economico, out aceites);

                tblM_MaquinariaAceitesLubricantes aceite = null;
                if (aceites != null)
                {
                    aceite = aceites.FirstOrDefault(x => x.Fecha == fecha && x.Turno == turno);
                }

                if (aceite == null)
                {
                    // Asignar valores por defecto cuando no existe registro de aceite
                    dto.id = 0;
                    dto.Economico = economico;
                    dto.CC = item.centro_costos != null ? item.centro_costos : string.Empty;
                    dto.Turno = turno;
                    dto.Horometro = 0;
                    dto.Rotacion = false;
                    dto.Sopleteo = false;
                    dto.AK = false;
                    dto.Lubricacion = false;
                    dto.Antifreeze = 0;
                    dto.MotorId = 1;
                    dto.MotorVal = 0;
                    dto.TransmisionID = 1;
                    dto.TransmisionVal = 0;
                    dto.HidraulicoID = 1;
                    dto.HidraulicoVal = 0;
                    dto.DiferencialId = 1;
                    dto.DiferencialVal = 0;
                    dto.MFTIzqId = 1;
                    dto.MFTDerId = 1;
                    dto.MFTDerVal = 0;
                    dto.MFTIzqVal = 0;
                    dto.MDDerID = 1;
                    dto.MDIzqID = 1;
                    dto.MDDerVal = 0;
                    dto.MDIzqVal = 0;
                    dto.DirId = 1;
                    dto.DirVal = 0;
                    dto.Grasa = 0;
                    dto.Firma = consumo;
                    dto.Fecha = fecha;
                }
                else
                {
                    // Asignar valores desde el registro existente
                    dto.id = aceite.id;
                    dto.Economico = economico;
                    dto.CC = item.centro_costos;
                    dto.Turno = aceite.Turno;
                    dto.Horometro = aceite.Horometro;
                    dto.Rotacion = aceite.Rotacion;
                    dto.Sopleteo = aceite.Sopleteo;
                    dto.AK = aceite.AK;
                    dto.Lubricacion = aceite.Lubricacion;
                    dto.Antifreeze = aceite.Antifreeze;
                    dto.MotorId = aceite.MotorId;
                    dto.MotorVal = aceite.MotorVal;
                    dto.TransmisionID = aceite.TransmisionID;
                    dto.TransmisionVal = aceite.TransmisionVal;
                    dto.HidraulicoID = aceite.HidraulicoID;
                    dto.HidraulicoVal = aceite.HidraulicoVal;
                    dto.DiferencialId = aceite.DiferencialId;
                    dto.DiferencialVal = aceite.DiferencialVal;
                    dto.MFTIzqId = aceite.MFTIzqId;
                    dto.MFTDerId = aceite.MFTDerId;
                    dto.MFTDerVal = aceite.MFTDerVal;
                    dto.MFTIzqVal = aceite.MFTIzqVal;
                    dto.MDDerID = aceite.MDDerID;
                    dto.MDIzqID = aceite.MDIzqID;
                    dto.MDDerVal = aceite.MDDerVal;
                    dto.MDIzqVal = aceite.MDIzqVal;
                    dto.DirId = aceite.DirId;
                    dto.DirVal = aceite.DirVal;
                    dto.Grasa = aceite.Grasa;
                    dto.Firma = aceite.Firma;
                    dto.Fecha = aceite.Fecha;
                    dto.otroId1 = aceite.otroId1;
                    dto.otros1 = aceite.otros1;
                    dto.otroId2 = aceite.otroId2;
                    dto.otros2 = aceite.otros2;
                    dto.otroId3 = aceite.otroId3;
                    dto.otros3 = aceite.otros3;
                    dto.otroId4 = aceite.otroId4;
                    dto.otros4 = aceite.otros4;
                }

                // Asignar valores comunes
                dto.CboDiferencial = DataCbos(item.modeloEquipoID, 4, "cboAddDiferencial");
                dto.CboMotor = DataCbos(item.modeloEquipoID, 1, "cboAddMotor");
                dto.CboHidraulico = DataCbos(item.modeloEquipoID, 3, "cboAddHidraulico");
                dto.CboTransmision = DataCbos(item.modeloEquipoID, 2, "cboAddTransmision");
                dto.CboMandoFinal = DataCbos(item.modeloEquipoID, 5, "cboAddMFTIzq");
                dto.CboDireccion = DataCbos(item.modeloEquipoID, 9, "cboAddDir");
                dto.CboGrasa = DataCbos(item.modeloEquipoID, 14, "cboAddGrasa");
                dto.CboOtros1 = DataCbos(item.modeloEquipoID, 10, "CboOtros1");
                dto.CboOtros2 = DataCbos(item.modeloEquipoID, 11, "CboOtros2");
                dto.CboOtros3 = DataCbos(item.modeloEquipoID, 12, "CboOtros3");
                dto.CboOtros4 = DataCbos(item.modeloEquipoID, 13, "CboOtros4");

                resultado.Add(dto);
            }

            return resultado.OrderBy(x => x.Economico).ToList();
        }

        public List<MaquinariaAceitesLubricantesDTO> GetLstMaqAceiteLubricante_old(string cc, string consumo, int turno, DateTime fecha, int tipo)
        {
            switch ((EmpresaEnum)vSesiones.sesionEmpresaActual)
            {
                case EmpresaEnum.Peru:
                    {
                        var areaCuenta = _context.tblP_CC.FirstOrDefault(x => x.cc == cc);
                        cc = areaCuenta != null ? areaCuenta.areaCuenta : cc;
                    }
                    break;
            }

            var TrimCC = cc.TrimStart(new Char[] { '0' });
            var resultado = new List<MaquinariaAceitesLubricantesDTO>();
            //var lstMaquinas = _context.tblM_CatMaquina.ToList().Where(x => !string.IsNullOrEmpty(x.centro_costos) && x.noEconomico == "MC-61");
            var lstMaquinas = _context.tblM_CatMaquina.ToList().Where(x => !string.IsNullOrEmpty(x.centro_costos));
            var lstMaq = lstMaquinas.Where(x =>
                 (tipo == 0 ? true : x.grupoMaquinaria.tipoEquipoID.Equals(tipo))
                && x.centro_costos == TrimCC
                && x.TipoCaptura != 0
                && x.estatus != 0
                //&& consumo.Equals("TALLER") ? !x.noEconomico.Contains("OR") : x.noEconomico.Equals(consumo)
                ).ToList();


            var lstAceite = _context.tblM_MaquinariaAceitesLubricantes
                .Where(x =>
                    x.CC.Equals(cc)
                    && x.Turno == turno
                    && x.Fecha == fecha)
                    .ToList();

            foreach (var item in lstMaq)
            {
                var economico = item.noEconomico;
                //var aceite = lstAceite.FirstOrDefault(x => x.Economico.Equals(economico) && x.Fecha == fecha && x.Turno == turno&& x.Economico=="MC-61");
                var aceite = lstAceite.FirstOrDefault(x => x.Economico.Equals(economico) && x.Fecha == fecha && x.Turno == turno);
                if (aceite == null)
                {

                    resultado.Add(new MaquinariaAceitesLubricantesDTO()
                    {
                        id = 0,
                        Economico = economico,
                        CC = item.centro_costos ?? string.Empty,
                        Turno = turno,
                        Horometro = 0,
                        Rotacion = false,
                        Sopleteo = false,
                        AK = false,
                        Lubricacion = false,
                        Antifreeze = 0,
                        MotorId = 1,
                        MotorVal = 0,
                        TransmisionID = 1,
                        TransmisionVal = 0,
                        HidraulicoID = 1,
                        HidraulicoVal = 0,
                        DiferencialId = 1,
                        DiferencialVal = 0,
                        MFTIzqId = 1,
                        MFTDerId = 1,
                        MFTDerVal = 0,
                        MFTIzqVal = 0,
                        MDDerID = 1,
                        MDIzqID = 1,
                        MDDerVal = 0,
                        MDIzqVal = 0,
                        DirId = 1,
                        DirVal = 0,
                        Grasa = 0,
                        Firma = consumo,
                        Fecha = fecha,
                        CboDiferencial = DataCbos(item.modeloEquipoID, 4, "cboAddDiferencial"),
                        CboMotor = DataCbos(item.modeloEquipoID, 1, "cboAddMotor"),
                        CboHidraulico = DataCbos(item.modeloEquipoID, 3, "cboAddHidraulico"),
                        CboTransmision = DataCbos(item.modeloEquipoID, 2, "cboAddTransmision"),
                        CboMandoFinal = DataCbos(item.modeloEquipoID, 5, "cboAddMFTIzq"),
                        CboDireccion = DataCbos(item.modeloEquipoID, 9, "cboAddDir"),
                        CboGrasa = DataCbos(item.modeloEquipoID, 14, "cboAddGrasa"),
                        CboOtros1 = DataCbos(item.modeloEquipoID, 10, "CboOtros1"),
                        CboOtros2 = DataCbos(item.modeloEquipoID, 11, "CboOtros2"),
                        CboOtros3 = DataCbos(item.modeloEquipoID, 12, "CboOtros3"),
                        CboOtros4 = DataCbos(item.modeloEquipoID, 13, "CboOtros4")

                    });
                }
                else
                {
                    resultado.Add(new MaquinariaAceitesLubricantesDTO()
                    {
                        id = aceite.id,
                        Economico = economico,
                        CC = item.centro_costos,
                        Turno = aceite.Turno,
                        Horometro = aceite.Horometro,
                        Rotacion = aceite.Rotacion,
                        Sopleteo = aceite.Sopleteo,
                        AK = aceite.AK,
                        Lubricacion = aceite.Lubricacion,
                        Antifreeze = aceite.Antifreeze,
                        MotorId = aceite.MotorId,
                        MotorVal = aceite.MotorVal,
                        TransmisionID = aceite.TransmisionID,
                        TransmisionVal = aceite.TransmisionVal,
                        HidraulicoID = aceite.HidraulicoID,
                        HidraulicoVal = aceite.HidraulicoVal,
                        DiferencialId = aceite.DiferencialId,
                        DiferencialVal = aceite.DiferencialVal,
                        MFTIzqId = aceite.MFTIzqId,
                        MFTDerId = aceite.MFTDerId,
                        MFTDerVal = aceite.MFTDerVal,
                        MFTIzqVal = aceite.MFTIzqVal,
                        MDDerID = aceite.MDDerID,
                        MDIzqID = aceite.MDIzqID,
                        MDDerVal = aceite.MDDerVal,
                        MDIzqVal = aceite.MDIzqVal,
                        DirId = aceite.DirId,
                        DirVal = aceite.DirVal,
                        Grasa = aceite.Grasa,
                        Firma = aceite.Firma,
                        Fecha = aceite.Fecha,
                        CboDiferencial = DataCbos(item.modeloEquipoID, 4, "cboAddDiferencial"),
                        CboMotor = DataCbos(item.modeloEquipoID, 1, "cboAddMotor"),
                        CboHidraulico = DataCbos(item.modeloEquipoID, 3, "cboAddHidraulico"),
                        CboTransmision = DataCbos(item.modeloEquipoID, 2, "cboAddTransmision"),
                        CboMandoFinal = DataCbos(item.modeloEquipoID, 5, "cboAddMFTIzq"),
                        CboDireccion = DataCbos(item.modeloEquipoID, 9, "cboAddDir"),
                        CboGrasa = DataCbos(item.modeloEquipoID, 14, "cboAddGrasa"),
                        CboOtros1 = DataCbos(item.modeloEquipoID, 10, "CboOtros1"),
                        CboOtros2 = DataCbos(item.modeloEquipoID, 11, "CboOtros2"),
                        CboOtros3 = DataCbos(item.modeloEquipoID, 12, "CboOtros3"),
                        CboOtros4 = DataCbos(item.modeloEquipoID, 13, "CboOtros4"),
                        otroId1 = aceite.otroId1,
                        otros1 = aceite.otros1,

                        otroId2 = aceite.otroId2,
                        otros2 = aceite.otros2,
                        otroId3 = aceite.otroId3,
                        otros3 = aceite.otros3,
                        otroId4 = aceite.otroId4,
                        otros4 = aceite.otros4


                    });
                }

            }
            return resultado.OrderBy(x => x.Economico).ToList();
        }

        public List<tblM_MaquinariaAceitesLubricantes> GetRepMaqAceiteLubricante(string cc, int turno, DateTime inicio, DateTime fin, string economico)
        {


            var lstAceite = _context.tblM_MaquinariaAceitesLubricantes.Where(x => x.CC == cc).ToList();
            var lst = lstAceite
                    .Where(x =>
                        x.Fecha >= inicio
                        && x.Fecha <= fin
                        && (string.IsNullOrEmpty(economico) ? x.Economico.Equals(x.Economico) : x.Economico.Equals(economico))
                        && (turno == 0 ? true: x.Turno.Equals(turno)))
                       
                .ToList();


            return lst;
        }

        private string DataCbos(int ModeloID, int subConjuntoID, string cboTipo)
        {
            var data = _context.tblM_CatAceitesLubricantes.Where(x => x.modeloID == ModeloID && x.subConjuntoID == subConjuntoID).ToList();
            if (data.Count > 0)
            {
                var select = "<select class='form-control " + cboTipo + "'>";
                foreach (var item in data)
                {
                    select += "<option value='" + item.id + "'>" + item.Descripcion + "</option>";
                }
                select += "</select>";

                return select;
            }
            else
            {
                return "";
            }
        }

    }
}

