using Core.DAO.RecursosHumanos.BajasPersonal;
using Core.DTO;
using Core.DTO.RecursosHumanos.BajasPersonal;
using Core.DTO.Utils.Data;
using Core.Entity.Principal.Usuarios;
using Core.Entity.RecursosHumanos.Bajas;
using Core.Entity.RecursosHumanos.Catalogo;
using Core.Enum.Multiempresa;
using Core.Enum.Principal;
using Core.Enum.Principal.Bitacoras;
using Data.EntityFramework.Context;
using Data.EntityFramework.Generic;
using Infrastructure.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Odbc;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.DAO.RecursosHumanos.BajasPersonal
{
    public class BajasPersonalEntrevistaDAO : GenericDAO<tblP_Usuario>, IBajasPersonalEntrevistaDAO
    {
        Dictionary<string,object> resultado = new Dictionary<string, object>();
        private const string _NOMBRE_CONTROLADOR = "BajasPersonalEntrevista";
        private const int _SISTEMA = (int)SistemasEnum.RH;

        #region ENTREVISTA

        public Dictionary<string, object> GetCapturada(int idRegistro, int empresa)
        {
            resultado = new Dictionary<string, object>();

            try
            {
                using(var context = new MainContext(empresa))
	            {
                    var entrevista = context.tblRH_Baja_Entrevista.Where(e => e.registroActivo && e.registroID == idRegistro).FirstOrDefault();

                    if (entrevista != null)
                    {
                        if (entrevista.estado_civil_clave != 0)
                        {
                            resultado.Add(SUCCESS, false);
                            resultado.Add(MESSAGE, "Esta entrevista ya fue capturada");
                        }
                        else
                        {
                            resultado.Add(SUCCESS, true);
                            vSesiones.sesionEmpresaActual = empresa;
                        }
                    }
                    else
                    {
                        resultado.Add(SUCCESS, true);
                    }
	            }
                
            }
            catch (Exception e)
            {
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, "Ocurrio un error");
                throw;
            }

            return resultado;
        }

        public Dictionary<string, object> GetBaja(int id, int empresa)
        {

            resultado = new Dictionary<string,object>();

            try
            {

                tblRH_Baja_Registro objBaja = new tblRH_Baja_Registro();


                using (var _context = new MainContext(empresa))
                {
                    objBaja = _context.tblRH_Baja_Registro.Where(e => e.id == id).FirstOrDefault();
                }

                var infoBaja = GetDatosPersona(objBaja.numeroEmpleado,"",empresa)["objDatosPersona"] as BajaPersonalDTO;

                DateTime fecha_ingreso = infoBaja.fechaIngreso;

                if (objBaja == null)
                    throw new Exception("Algo salio mal");

                resultado.Add(SUCCESS, true);
                resultado.Add(ITEMS, objBaja);
                resultado.Add("fecha_ingreso", fecha_ingreso);
            }
            catch (Exception e)
            {
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, e.Message);
            }
            return resultado;
        }

        public Dictionary<string, object> CrearEditarEntrevista(BajaPersonalDTO objDTO)
        {

            Dictionary<string, object> result = new Dictionary<string, object>();
            using (var _context = new MainContext(objDTO.empresa))
            {
                using (var dbContextTransaction = _context.Database.BeginTransaction())
                {
                    result = new Dictionary<string, object>();


                    #region SE OBTIENE CONCEPTOS
                    List<tblRH_Baja_Entrevista_Conceptos> lstConceptos = _context.tblRH_Baja_Entrevista_Conceptos.ToList();
                    #endregion

                    tblRH_Baja_Registro objCEBaja = new tblRH_Baja_Registro();
                    tblRH_Baja_Entrevista objCEEntrevista = new tblRH_Baja_Entrevista();

                    try
                    {

                        if (GetCapturada(objDTO.id, vSesiones.sesionEmpresaActual)[SUCCESS] as bool? == false)
                            throw new Exception("Esta entrevista ya fue capturada");


                        #region VALIDACIONES
                        bool errorValidacion = false;

                        if (objDTO.gerente_clave <= 0) { errorValidacion = true; }
                        if (string.IsNullOrEmpty(objDTO.nombreGerente)) { errorValidacion = true; }
                        if (objDTO.fecha_nacimiento == null) { errorValidacion = true; }
                        if (objDTO.estado_civil_clave <= 0) { errorValidacion = true; }
                        if (objDTO.escolaridad_clave <= 0) { errorValidacion = true; }
                        if (objDTO.p1_clave <= 0) { errorValidacion = true; }
                        if (objDTO.p2_clave <= 0) { errorValidacion = true; }
                        if (objDTO.p3_1_clave <= 0) { errorValidacion = true; }
                        if (objDTO.p3_2_clave <= 0) { errorValidacion = true; }
                        if (objDTO.p3_3_clave <= 0) { errorValidacion = true; }
                        if (objDTO.p3_4_clave <= 0) { errorValidacion = true; }
                        if (objDTO.p3_5_clave <= 0) { errorValidacion = true; }
                        if (objDTO.p3_6_clave <= 0) { errorValidacion = true; }
                        if (objDTO.p3_7_clave <= 0) { errorValidacion = true; }
                        if (objDTO.p3_8_clave <= 0) { errorValidacion = true; }
                        if (objDTO.p3_9_clave <= 0) { errorValidacion = true; }
                        if (objDTO.p3_10_clave <= 0) { errorValidacion = true; }
                        if (objDTO.p4_clave <= 0) { errorValidacion = true; }
                        if (objDTO.p5_clave <= 0) { errorValidacion = true; }
                        if (objDTO.p8_clave <= 0) { errorValidacion = true; }
                        if (string.IsNullOrEmpty(objDTO.p8_porque)) { errorValidacion = true; }
                        if (objDTO.p9_clave <= 0) { errorValidacion = true; }
                        if (string.IsNullOrEmpty(objDTO.p9_porque)) { errorValidacion = true; }
                        if (objDTO.p10_clave <= 0) { errorValidacion = true; }
                        if (string.IsNullOrEmpty(objDTO.p10_porque)) { errorValidacion = true; }
                        if (objDTO.p11_1_clave <= 0) { errorValidacion = true; }
                        if (objDTO.p11_2_clave <= 0) { errorValidacion = true; }
                        if (objDTO.p12_clave <= 0) { errorValidacion = true; }
                        if (string.IsNullOrEmpty(objDTO.p12_porque)) { errorValidacion = true; }
                        if (objDTO.p13_clave <= 0) { errorValidacion = true; }
                        //if (objDTO.p14_clave <= 0) { errorValidacion = true; }
                        //if (objDTO.p14_fecha == null) { errorValidacion = true; }
                        //if (string.IsNullOrEmpty(objDTO.p14_porque)) { errorValidacion = true; }

                        #endregion
                        if (errorValidacion == true)
                            throw new Exception("Es necesario llenar los campos obligatorios.");

                        objCEEntrevista = _context.tblRH_Baja_Entrevista.Where(e => e.registroID == objDTO.id && e.registroActivo == true).FirstOrDefault();
                        objCEBaja = _context.tblRH_Baja_Registro.Where(w => w.id == objDTO.id && w.registroActivo == true).OrderByDescending(e => e.id).FirstOrDefault();
                        if (objCEEntrevista != null)
                        {

                            if (objCEBaja == null)
                                throw new Exception("Ocurrió un error al actualizar la baja.");

                            #region SE OBTIENE EL CC Y PUESTO EN BASE A LA CLAVE DEL EMPLEADO
                            var dicDatosPersona = GetDatosPersona(objCEBaja.numeroEmpleado, objCEBaja.nombre, objDTO.empresa);
                            BajaPersonalDTO bajaPersonalDTO = dicDatosPersona["objDatosPersona"] as BajaPersonalDTO;
                            string numCC = bajaPersonalDTO.numCC;
                            string descripcionCC = bajaPersonalDTO.descripcionCC;
                            int idPuesto = bajaPersonalDTO.idPuesto;
                            string nombrePuesto = bajaPersonalDTO.nombrePuesto;
                            #endregion

                            #region ACTUALIZA BAJA PERSONAL
                            //objCEBaja.numeroEmpleado = objDTO.numeroEmpleado;
                            //objCEBaja.nombre = !string.IsNullOrEmpty(objDTO.nombre) ? objDTO.nombre.Trim() : null;
                            //objCEBaja.cc = !string.IsNullOrEmpty(numCC) ? numCC.Trim() : null;
                            //objCEBaja.descripcionCC = !string.IsNullOrEmpty(descripcionCC) ? descripcionCC.Trim() : null;
                            //objCEBaja.idPuesto = idPuesto;
                            //objCEBaja.nombrePuest0 = !string.IsNullOrEmpty(nombrePuesto) ? nombrePuesto.Trim() : null;
                            //objCEBaja.habilidadesConEquipo = !string.IsNullOrEmpty(objDTO.habilidadesConEquipo) ? objDTO.habilidadesConEquipo : null;
                            //objCEBaja.telPersonal = !string.IsNullOrEmpty(objDTO.telPersonal) ? objDTO.telPersonal.Trim() : null;
                            //objCEBaja.tieneWha = objDTO.strTieneWha == 1 ? true : false;
                            //objCEBaja.telCasa = !string.IsNullOrEmpty(objDTO.telCasa) ? objDTO.telCasa.Trim() : null;
                            //objCEBaja.contactoFamilia = !string.IsNullOrEmpty(objDTO.contactoFamilia) ? objDTO.contactoFamilia.Trim() : null;
                            //objCEBaja.idEstado = objDTO.idEstado;
                            //objCEBaja.idCiudad = objDTO.idCiudad;
                            //objCEBaja.idMunicipio = objDTO.idMunicipio;
                            //objCEBaja.direccion = !string.IsNullOrEmpty(objDTO.direccion) ? objDTO.direccion : null;
                            //objCEBaja.facebook = !string.IsNullOrEmpty(objDTO.facebook) ? objDTO.facebook.Trim() : null;
                            //objCEBaja.instagram = !string.IsNullOrEmpty(objDTO.instagram) ? objDTO.instagram.Trim() : null;
                            //objCEBaja.correo = !string.IsNullOrEmpty(objDTO.correo) ? objDTO.correo.Trim() : null;
                            //objCEBaja.fechaBaja = objDTO.fechaBaja;
                            //objCEBaja.motivoBajaDeSistema = !string.IsNullOrEmpty(objDTO.motivoBajaDeSistema) ? objDTO.motivoBajaDeSistema.Trim() : null;
                            //objCEBaja.motivoSeparacionDeEmpresa = !string.IsNullOrEmpty(objDTO.motivoSeparacionDeEmpresa) ? objDTO.motivoSeparacionDeEmpresa.Trim() : null;
                            //objCEBaja.regresariaALaEmpresa = objDTO.strRegresariaALaEmpresa == 1 ? true : false;
                            //objCEBaja.porqueRegresariaALaEmpresa = !string.IsNullOrEmpty(objDTO.porqueRegresariaALaEmpresa) ? objDTO.porqueRegresariaALaEmpresa.Trim() : null;
                            //objCEBaja.dispuestoCambioDeProyecto = !string.IsNullOrEmpty(objDTO.dispuestoCambioDeProyecto) ? objDTO.dispuestoCambioDeProyecto.Trim() : null;
                            //objCEBaja.experienciaEnCP = !string.IsNullOrEmpty(objDTO.experienciaEnCP) ? objDTO.experienciaEnCP.Trim() : null;
                            //objCEBaja.esContratable = objDTO.strContratable == 1 ? true : false;
                            //objCEBaja.prioridad = objDTO.prioridad;
                            //objCEBaja.idUsuarioModificacion = 0;
                            //objCEBaja.fechaModificacion = DateTime.Now;
                            //objCEBaja.curp = objDTO.curp;
                            //objCEBaja.rfc = objDTO.rfc;
                            //objCEBaja.nss = objDTO.nss;
                            //_context.SaveChanges();
                            #endregion


                            #region ACTUALIZAR ENTREVISTA
                            objCEEntrevista.fecha_ingreso = objDTO.fechaIngreso;
                            //objCEEntrevista.fecha_salida = objDTO.fechaBaja;
                            objCEEntrevista.cc = numCC;
                            objCEEntrevista.cc_nombre = descripcionCC;
                            objCEEntrevista.gerente_clave = objDTO.gerente_clave;
                            objCEEntrevista.nombreGerente = objDTO.nombreGerente;
                            objCEEntrevista.fecha_nacimiento = objDTO.fecha_nacimiento;
                            objCEEntrevista.estado_civil_clave = objDTO.estado_civil_clave;
                            objCEEntrevista.escolaridad_clave = objDTO.escolaridad_clave;
                            objCEEntrevista.p1_clave = objDTO.p1_clave;
                            objCEEntrevista.p1_concepto = lstConceptos.Where(w => w.id == objDTO.p1_clave).Select(s => s.concepto).FirstOrDefault();
                            objCEEntrevista.p2_clave = objDTO.p2_clave;
                            objCEEntrevista.p2_concepto = lstConceptos.Where(w => w.id == objDTO.p2_clave).Select(s => s.concepto).FirstOrDefault();
                            objCEEntrevista.p3_1_clave = objDTO.p3_1_clave;
                            objCEEntrevista.p3_1_concepto = lstConceptos.Where(w => w.id == objDTO.p3_1_clave).Select(s => s.concepto).FirstOrDefault();
                            objCEEntrevista.p3_2_clave = objDTO.p3_2_clave;
                            objCEEntrevista.p3_2_concepto = lstConceptos.Where(w => w.id == objDTO.p3_2_clave).Select(s => s.concepto).FirstOrDefault();
                            objCEEntrevista.p3_3_clave = objDTO.p3_3_clave;
                            objCEEntrevista.p3_3_concepto = lstConceptos.Where(w => w.id == objDTO.p3_3_clave).Select(s => s.concepto).FirstOrDefault();
                            objCEEntrevista.p3_4_clave = objDTO.p3_4_clave;
                            objCEEntrevista.p3_4_concepto = lstConceptos.Where(w => w.id == objDTO.p3_4_clave).Select(s => s.concepto).FirstOrDefault();
                            objCEEntrevista.p3_5_clave = objDTO.p3_5_clave;
                            objCEEntrevista.p3_5_concepto = lstConceptos.Where(w => w.id == objDTO.p3_5_clave).Select(s => s.concepto).FirstOrDefault();
                            objCEEntrevista.p3_6_clave = objDTO.p3_6_clave;
                            objCEEntrevista.p3_6_concepto = lstConceptos.Where(w => w.id == objDTO.p3_6_clave).Select(s => s.concepto).FirstOrDefault();
                            objCEEntrevista.p3_7_clave = objDTO.p3_7_clave;
                            objCEEntrevista.p3_7_concepto = lstConceptos.Where(w => w.id == objDTO.p3_7_clave).Select(s => s.concepto).FirstOrDefault();
                            objCEEntrevista.p3_8_clave = objDTO.p3_8_clave;
                            objCEEntrevista.p3_8_concepto = lstConceptos.Where(w => w.id == objDTO.p3_8_clave).Select(s => s.concepto).FirstOrDefault();
                            objCEEntrevista.p3_9_clave = objDTO.p3_9_clave;
                            objCEEntrevista.p3_9_concepto = lstConceptos.Where(w => w.id == objDTO.p3_9_clave).Select(s => s.concepto).FirstOrDefault();
                            objCEEntrevista.p3_10_clave = objDTO.p3_10_clave;
                            objCEEntrevista.p3_10_concepto = lstConceptos.Where(w => w.id == objDTO.p3_10_clave).Select(s => s.concepto).FirstOrDefault();
                            objCEEntrevista.p4_clave = objDTO.p4_clave;
                            objCEEntrevista.p4_concepto = lstConceptos.Where(w => w.id == objDTO.p4_clave).Select(s => s.concepto).FirstOrDefault();
                            objCEEntrevista.p5_clave = objDTO.p5_clave;
                            objCEEntrevista.p5_concepto = lstConceptos.Where(w => w.id == objDTO.p5_clave).Select(s => s.concepto).FirstOrDefault();
                            objCEEntrevista.p6_concepto = objDTO.p6_concepto;
                            objCEEntrevista.p7_concepto = objDTO.p7_concepto;
                            objCEEntrevista.p8_clave = objDTO.p8_clave;
                            objCEEntrevista.p8_concepto = lstConceptos.Where(w => w.id == objDTO.p8_clave).Select(s => s.concepto).FirstOrDefault();
                            objCEEntrevista.p8_porque = objDTO.p8_porque;
                            objCEEntrevista.p9_clave = objDTO.p9_clave;
                            objCEEntrevista.p9_concepto = lstConceptos.Where(w => w.id == objDTO.p9_clave).Select(s => s.concepto).FirstOrDefault();
                            objCEEntrevista.p9_porque = objDTO.p9_porque;
                            objCEEntrevista.p10_clave = objDTO.p10_clave;
                            objCEEntrevista.p10_concepto = lstConceptos.Where(w => w.id == objDTO.p10_clave).Select(s => s.concepto).FirstOrDefault();
                            objCEEntrevista.p10_porque = objDTO.p10_porque;
                            objCEEntrevista.p11_1_clave = objDTO.p11_1_clave;
                            objCEEntrevista.p11_1_concepto = lstConceptos.Where(w => w.id == objDTO.p11_1_clave).Select(s => s.concepto).FirstOrDefault();
                            objCEEntrevista.p11_2_clave = objDTO.p11_2_clave;
                            objCEEntrevista.p11_2_concepto = lstConceptos.Where(w => w.id == objDTO.p11_2_clave).Select(s => s.concepto).FirstOrDefault();
                            objCEEntrevista.p12_clave = objDTO.p12_clave;
                            objCEEntrevista.p12_concepto = lstConceptos.Where(w => w.id == objDTO.p12_clave).Select(s => s.concepto).FirstOrDefault();
                            objCEEntrevista.p12_porque = objDTO.p12_porque;
                            objCEEntrevista.p13_clave = objDTO.p13_clave;
                            objCEEntrevista.p13_concepto = lstConceptos.Where(w => w.id == objDTO.p13_clave).Select(s => s.concepto).FirstOrDefault();
                            objCEEntrevista.p14_clave = objDTO.p14_clave;
                            objCEEntrevista.p14_concepto = lstConceptos.Where(w => w.id == objDTO.p14_clave).Select(s => s.concepto).FirstOrDefault();
                            if (objDTO.p14_clave == 65)
                            {
                                objCEEntrevista.p14_fecha = objDTO.p14_fecha;
                                objCEEntrevista.p14_porque = objDTO.p14_porque;
                            }

                            _context.SaveChanges();
                            #endregion

                            result = new Dictionary<string, object>();
                            result.Add(MESSAGE, "Se ha actualizado con éxito.");
                        }
                        else
                        {
                            if (objCEBaja == null)
                                throw new Exception("Ocurrió un error al registrar la baja.");

                            objCEEntrevista = new tblRH_Baja_Entrevista();
                            #region SE OBTIENE EL CC Y PUESTO EN BASE A LA CLAVE DEL EMPLEADO
                            var dicDatosPersona = GetDatosPersona(objCEBaja.numeroEmpleado, objCEBaja.nombre, objDTO.empresa);
                            BajaPersonalDTO bajaPersonalDTO = dicDatosPersona["objDatosPersona"] as BajaPersonalDTO;
                            string numCC = bajaPersonalDTO.numCC;
                            string descripcionCC = bajaPersonalDTO.descripcionCC;
                            int idPuesto = bajaPersonalDTO.idPuesto;
                            string nombrePuesto = bajaPersonalDTO.nombrePuesto;
                            DateTime? fechaIngreso = bajaPersonalDTO.fechaIngreso;
                            DateTime? fechaSalida = bajaPersonalDTO.fechaBaja;
                            #endregion

                            #region REGISTRAR BAJA
                            //objCEBaja.numeroEmpleado = objDTO.numeroEmpleado;
                            //objCEBaja.nombre = !string.IsNullOrEmpty(objDTO.nombre) ? objDTO.nombre.Trim() : null;
                            //objCEBaja.cc = !string.IsNullOrEmpty(numCC) ? numCC.Trim() : null;
                            //objCEBaja.descripcionCC = !string.IsNullOrEmpty(descripcionCC) ? descripcionCC.Trim() : null;
                            //objCEBaja.idPuesto = idPuesto;
                            //objCEBaja.nombrePuesto = !string.IsNullOrEmpty(nombrePuesto) ? nombrePuesto.Trim() : null;
                            //objCEBaja.habilidadesConEquipo = !string.IsNullOrEmpty(objDTO.habilidadesConEquipo) ? objDTO.habilidadesConEquipo : null;
                            //objCEBaja.telPersonal = !string.IsNullOrEmpty(objDTO.telPersonal) ? objDTO.telPersonal.Trim() : null;
                            //objCEBaja.tieneWha = objDTO.strTieneWha == 1 ? true : false;
                            //objCEBaja.telCasa = !string.IsNullOrEmpty(objDTO.telCasa) ? objDTO.telCasa.Trim() : null;
                            //objCEBaja.contactoFamilia = !string.IsNullOrEmpty(objDTO.contactoFamilia) ? objDTO.contactoFamilia.Trim() : null;
                            //objCEBaja.idEstado = objDTO.idEstado;
                            //objCEBaja.idCiudad = objDTO.idCiudad;
                            //objCEBaja.idMunicipio = objDTO.idMunicipio;
                            //objCEBaja.direccion = !string.IsNullOrEmpty(objDTO.direccion) ? objDTO.direccion : null;
                            //objCEBaja.facebook = !string.IsNullOrEmpty(objDTO.facebook) ? objDTO.facebook.Trim() : null;
                            //objCEBaja.instagram = !string.IsNullOrEmpty(objDTO.instagram) ? objDTO.instagram.Trim() : null;
                            //objCEBaja.correo = !string.IsNullOrEmpty(objDTO.correo) ? objDTO.correo.Trim() : null;
                            //objCEBaja.fechaBaja = objDTO.fechaBaja;
                            //objCEBaja.motivoBajaDeSistema = !string.IsNullOrEmpty(objDTO.motivoBajaDeSistema) ? objDTO.motivoBajaDeSistema.Trim() : null;
                            //objCEBaja.motivoSeparacionDeEmpresa = !string.IsNullOrEmpty(objDTO.motivoSeparacionDeEmpresa) ? objDTO.motivoSeparacionDeEmpresa.Trim() : null;
                            //objCEBaja.regresariaALaEmpresa = objDTO.strRegresariaALaEmpresa == 1 ? true : false;
                            //objCEBaja.porqueRegresariaALaEmpresa = !string.IsNullOrEmpty(objDTO.porqueRegresariaALaEmpresa) ? objDTO.porqueRegresariaALaEmpresa.Trim() : null;
                            //objCEBaja.dispuestoCambioDeProyecto = !string.IsNullOrEmpty(objDTO.dispuestoCambioDeProyecto) ? objDTO.dispuestoCambioDeProyecto.Trim() : null;
                            //objCEBaja.experienciaEnCP = !string.IsNullOrEmpty(objDTO.experienciaEnCP) ? objDTO.experienciaEnCP.Trim() : null;
                            //objCEBaja.esContratable = objDTO.strContratable == 1 ? true : false;
                            //objCEBaja.prioridad = objDTO.prioridad;
                            //objCEBaja.idUsuarioCreacion = 0;
                            //objCEBaja.fechaCreacion = DateTime.Now;
                            //objCEBaja.registroActivo = true;
                            //objCEBaja.curp = objDTO.curp;
                            //objCEBaja.rfc = objDTO.rfc;
                            //objCEBaja.nss = objDTO.nss;
                            //_context.tblRH_Baja_Registro.Add(objCEBaja);
                            //_context.SaveChanges();
                            #endregion

                            #region SE OBTIENE ID DE LA BAJA RECIEN REGISTRADA
                            //int id = _context.tblRH_Baja_Registro.OrderByDescending(o => o.numeroEmpleado == bajaPersonalDTO).Select(s => s.id).FirstOrDefault();
                            //if (id <= 0)
                            //    throw new Exception("Ocurrió un error al registrar la baja.");
                            #endregion

                            #region REGISTRA ENTREVISTA
                            objCEEntrevista.registroID = objCEBaja.id;
                            objCEEntrevista.fecha_ingreso = objDTO.fechaIngreso;
                            objCEEntrevista.fecha_salida = null;
                            objCEEntrevista.cc = numCC;
                            objCEEntrevista.cc_nombre = descripcionCC;
                            objCEEntrevista.gerente_clave = objDTO.gerente_clave;
                            objCEEntrevista.nombreGerente = objDTO.nombreGerente;
                            objCEEntrevista.fecha_nacimiento = objDTO.fecha_nacimiento;
                            objCEEntrevista.estado_civil_clave = objDTO.estado_civil_clave;
                            objCEEntrevista.estado_civil_nombre = objDTO.estado_civil_clave > 0 ? _context.tblP_EstadoCivil.Where(w => w.id == objCEEntrevista.estado_civil_clave).Select(s => s.estadoCivil).FirstOrDefault() : string.Empty;
                            objCEEntrevista.escolaridad_clave = objDTO.escolaridad_clave;
                            objCEEntrevista.p1_clave = objDTO.p1_clave;
                            objCEEntrevista.p1_concepto = lstConceptos.Where(w => w.id == objDTO.p1_clave).Select(s => s.concepto).FirstOrDefault();
                            objCEEntrevista.p2_clave = objDTO.p2_clave;
                            objCEEntrevista.p2_concepto = lstConceptos.Where(w => w.id == objDTO.p2_clave).Select(s => s.concepto).FirstOrDefault();
                            objCEEntrevista.p3_1_clave = objDTO.p3_1_clave;
                            objCEEntrevista.p3_1_concepto = lstConceptos.Where(w => w.id == objDTO.p3_1_clave).Select(s => s.concepto).FirstOrDefault();
                            objCEEntrevista.p3_2_clave = objDTO.p3_2_clave;
                            objCEEntrevista.p3_2_concepto = lstConceptos.Where(w => w.id == objDTO.p3_2_clave).Select(s => s.concepto).FirstOrDefault();
                            objCEEntrevista.p3_3_clave = objDTO.p3_3_clave;
                            objCEEntrevista.p3_3_concepto = lstConceptos.Where(w => w.id == objDTO.p3_3_clave).Select(s => s.concepto).FirstOrDefault();
                            objCEEntrevista.p3_4_clave = objDTO.p3_4_clave;
                            objCEEntrevista.p3_4_concepto = lstConceptos.Where(w => w.id == objDTO.p3_4_clave).Select(s => s.concepto).FirstOrDefault();
                            objCEEntrevista.p3_5_clave = objDTO.p3_5_clave;
                            objCEEntrevista.p3_5_concepto = lstConceptos.Where(w => w.id == objDTO.p3_5_clave).Select(s => s.concepto).FirstOrDefault();
                            objCEEntrevista.p3_6_clave = objDTO.p3_6_clave;
                            objCEEntrevista.p3_6_concepto = lstConceptos.Where(w => w.id == objDTO.p3_6_clave).Select(s => s.concepto).FirstOrDefault();
                            objCEEntrevista.p3_7_clave = objDTO.p3_7_clave;
                            objCEEntrevista.p3_7_concepto = lstConceptos.Where(w => w.id == objDTO.p3_7_clave).Select(s => s.concepto).FirstOrDefault();
                            objCEEntrevista.p3_8_clave = objDTO.p3_8_clave;
                            objCEEntrevista.p3_8_concepto = lstConceptos.Where(w => w.id == objDTO.p3_8_clave).Select(s => s.concepto).FirstOrDefault();
                            objCEEntrevista.p3_9_clave = objDTO.p3_9_clave;
                            objCEEntrevista.p3_9_concepto = lstConceptos.Where(w => w.id == objDTO.p3_9_clave).Select(s => s.concepto).FirstOrDefault();
                            objCEEntrevista.p3_10_clave = objDTO.p3_10_clave;
                            objCEEntrevista.p3_10_concepto = lstConceptos.Where(w => w.id == objDTO.p3_10_clave).Select(s => s.concepto).FirstOrDefault();
                            objCEEntrevista.p4_clave = objDTO.p4_clave;
                            objCEEntrevista.p4_concepto = lstConceptos.Where(w => w.id == objDTO.p4_clave).Select(s => s.concepto).FirstOrDefault();
                            objCEEntrevista.p5_clave = objDTO.p5_clave;
                            objCEEntrevista.p5_concepto = lstConceptos.Where(w => w.id == objDTO.p5_clave).Select(s => s.concepto).FirstOrDefault();
                            objCEEntrevista.p6_concepto = objDTO.p6_concepto;
                            objCEEntrevista.p7_concepto = objDTO.p7_concepto;
                            objCEEntrevista.p8_clave = objDTO.p8_clave;
                            objCEEntrevista.p8_concepto = lstConceptos.Where(w => w.id == objDTO.p8_clave).Select(s => s.concepto).FirstOrDefault();
                            objCEEntrevista.p8_porque = objDTO.p8_porque;
                            objCEEntrevista.p9_clave = objDTO.p9_clave;
                            objCEEntrevista.p9_concepto = lstConceptos.Where(w => w.id == objDTO.p9_clave).Select(s => s.concepto).FirstOrDefault();
                            objCEEntrevista.p9_porque = objDTO.p9_porque;
                            objCEEntrevista.p10_clave = objDTO.p10_clave;
                            objCEEntrevista.p10_concepto = lstConceptos.Where(w => w.id == objDTO.p10_clave).Select(s => s.concepto).FirstOrDefault();
                            objCEEntrevista.p10_porque = objDTO.p10_porque;
                            objCEEntrevista.p11_1_clave = objDTO.p11_1_clave;
                            objCEEntrevista.p11_1_concepto = lstConceptos.Where(w => w.id == objDTO.p11_1_clave).Select(s => s.concepto).FirstOrDefault();
                            objCEEntrevista.p11_2_clave = objDTO.p11_2_clave;
                            objCEEntrevista.p11_2_concepto = lstConceptos.Where(w => w.id == objDTO.p11_2_clave).Select(s => s.concepto).FirstOrDefault();
                            objCEEntrevista.p12_clave = objDTO.p12_clave;
                            objCEEntrevista.p12_concepto = lstConceptos.Where(w => w.id == objDTO.p12_clave).Select(s => s.concepto).FirstOrDefault();
                            objCEEntrevista.p12_porque = objDTO.p12_porque;
                            objCEEntrevista.p13_clave = objDTO.p13_clave;
                            objCEEntrevista.p13_concepto = lstConceptos.Where(w => w.id == objDTO.p13_clave).Select(s => s.concepto).FirstOrDefault();
                            objCEEntrevista.p14_clave = objDTO.p14_clave;
                            objCEEntrevista.p14_concepto = lstConceptos.Where(w => w.id == objDTO.p14_clave).Select(s => s.concepto).FirstOrDefault();
                            objCEEntrevista.p14_fecha = objDTO.p14_fecha.HasValue ? objDTO.p14_fecha : DateTime.Now;
                            objCEEntrevista.p14_porque = objDTO.p14_porque;
                            objCEEntrevista.registroActivo = true;
                            _context.tblRH_Baja_Entrevista.Add(objCEEntrevista);
                            _context.SaveChanges();
                            #endregion

                            result = new Dictionary<string, object>();
                            result.Add(MESSAGE, "Se ha registrado con éxito.");
                        }

                        result.Add(SUCCESS, true);
                        dbContextTransaction.Commit();
                    }
                    catch (Exception e)
                    {
                        dbContextTransaction.Rollback();
                        LogError(0, 0, "BajasPersonalEntrevistaController", "CrearEditarBajaPersonal", e, AccionEnum.CONSULTA, 0, 0);
                        result.Add(MESSAGE, e.Message);
                        result.Add(SUCCESS, false);
                    }
                }
            }

            return result;

        }

        public Dictionary<string, object> GetDatosPersona(int claveEmpleado, string nombre, int empresa)
        {
            var resultado = new Dictionary<string, object>();
            try
            {
                #region SE OBTIENE CC, PUESTO Y FECHA DE INGRESO EN BASE AL CLAVE DEL EMPLEADO
//                var odbc = new OdbcConsultaDTO()
//                {
//                    consulta = @"SELECT CONVERT(VARCHAR(200), t1.ape_paterno) + ' ' + CONVERT(VARCHAR(200), t1.ape_materno) + ' ' + CONVERT(VARCHAR(200), t1.nombre) AS nombreCompleto, 
//                                        CONVERT(VARCHAR(200), t3.cc) + ' - ' + CONVERT(VARCHAR(200), t3.descripcion) AS cc, 
//                                        t2.descripcion AS nombrePuesto,  t1.fecha_alta AS fechaIngreso, t1.puesto, t3.cc AS numCC, t3.descripcion AS descripcionCC,t1.curp,t1.rfc,t1.nss
//                                            FROM sn_empleados AS t1
//                                            INNER JOIN si_puestos AS t2 ON t1.puesto = t2.puesto
//                                            INNER JOIN cc AS t3 ON t1.cc_contable = t3.cc
//                                                WHERE t1.clave_empleado = ?",
//                    parametros = new List<OdbcParameterDTO>() { new OdbcParameterDTO { nombre = "clave_empleado", tipo = OdbcType.Numeric, valor = claveEmpleado } }
//                }; //OMAR
//                BajaPersonalDTO objDatosPersona = new BajaPersonalDTO();
//                objDatosPersona = _contextEnkontrol.Select<BajaPersonalDTO>(EnkontrolEnum.CplanRh, odbc).FirstOrDefault();
//                if (objDatosPersona == null)
//                    objDatosPersona = _contextEnkontrol.Select<BajaPersonalDTO>(EnkontrolEnum.ArrenRh, odbc).FirstOrDefault();


                var objDatosPersona = _context.Select<BajaPersonalDTO>(new DapperDTO
                {
                    baseDatos = (MainContextEnum)empresa,
                    consulta = @"SELECT
                                    CONVERT(VARCHAR(200), t1.ape_paterno) + ' ' + CONVERT(VARCHAR(200), t1.ape_materno) + ' ' + CONVERT(VARCHAR(200), t1.nombre) AS nombreCompleto, 
                                    CONVERT(VARCHAR(200), t3.cc) + ' - ' + CONVERT(VARCHAR(200), t3.descripcion) AS cc, 
                                    t2.descripcion AS nombrePuesto,  t1.fecha_antiguedad AS fechaIngreso, t1.puesto, t3.cc AS numCC, t3.descripcion AS descripcionCC,t1.curp,t1.rfc,t1.nss,
                                    t1.clave_ciudad_nac AS idCiudad,
                                    t1.clave_estado_nac AS idEstado
                                FROM tblRH_EK_Empleados AS t1
                                    LEFT JOIN tblRH_EK_Puestos AS t2 ON t1.puesto = t2.puesto
                                    LEFT JOIN tblP_CC AS t3 ON t1.cc_contable = t3.cc
                                WHERE t1.clave_empleado = @claveEmpleado",
                    parametros = new { claveEmpleado }
                }).FirstOrDefault();
                #endregion

                resultado.Add("objDatosPersona", objDatosPersona);
                resultado.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                var NOMBRE_FUNCION = System.Reflection.MethodBase.GetCurrentMethod().Name;
                LogError(_SISTEMA, 0, _NOMBRE_CONTROLADOR, NOMBRE_FUNCION, e, AccionEnum.CONSULTA, 0, null);
                resultado.Add(MESSAGE, e.Message);
                resultado.Add(SUCCESS, false);
            }
            return resultado;
        }

        public Dictionary<string, object> FillCboPreguntas(int idPregunta)
        {
            try
            {
                #region FILL CBO PREGUNTA #1 ENTREVISTA
                List<ComboDTO> lstConceptos = _context.Select<ComboDTO>(new DapperDTO
                {
                    baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
                    consulta = @"SELECT id AS Value, concepto AS Text FROM tblRH_Baja_Entrevista_Conceptos WHERE preguntaID = @preguntaID AND estatus = @estatus ORDER BY orden",
                    parametros = new { preguntaID = idPregunta, estatus = true }
                });
                resultado.Add(ITEMS, lstConceptos);
                resultado.Add(SUCCESS, true);
                #endregion
            }
            catch (Exception e)
            {
                LogError(0, 0, _NOMBRE_CONTROLADOR, "FillCboPreguntas", e, AccionEnum.CONSULTA, 0, 0);
                resultado.Add(MESSAGE, e.Message);
                resultado.Add(SUCCESS, false);
            }
            return resultado;
        }

        #endregion

        #region FILL COMBOS
        public Dictionary<string, object> FillCboEstados()
        {

            resultado = new Dictionary<string, object>();
            try
            {
                #region SE OBTIENE LISTADO DE ESTADOS
                List<ComboDTO> lstEstados = _context.Select<ComboDTO>(new DapperDTO
                {
                    baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
                    consulta = @"SELECT idEstado AS Value, Estado AS Text FROM tblP_Estados ORDER BY Text"
                });

                resultado.Add(ITEMS, lstEstados);
                resultado.Add(SUCCESS, true);
                #endregion
            }
            catch (Exception e)
            {
                LogError(0, 0, _NOMBRE_CONTROLADOR, "FillCboEstados", e, AccionEnum.CONSULTA, 0, 0);
                resultado.Add(MESSAGE, e.Message);
                resultado.Add(SUCCESS, false);
            }
            
            return resultado;
        }

        public Dictionary<string, object> FillCboMunicipios(int idEstado)
        {

            resultado = new Dictionary<string, object>();
            try
            {
                #region SE OBTIENE LISTADO DE MUNICIPIOS
                List<ComboDTO> lstMunicipios = _context.Select<ComboDTO>(new DapperDTO
                {
                    baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
                    consulta = @"SELECT idMunicipio AS Value, Municipio AS Text FROM tblP_Municipios WHERE idEstado = @idEstado ORDER BY Text",
                    parametros = new { idEstado = idEstado }
                });

                resultado.Add(ITEMS, lstMunicipios);
                resultado.Add(SUCCESS, true);
                #endregion
            }
            catch (Exception e)
            {
                LogError(0, 0, _NOMBRE_CONTROLADOR, "FillMunicipios", e, AccionEnum.CONSULTA, 0, 0);
                resultado.Add(MESSAGE, e.Message);
                resultado.Add(SUCCESS, false);
            }
            
            return resultado;
        }

        public Dictionary<string, object> FillCboEstadosCiviles()
        {
            resultado = new Dictionary<string, object>();
            try
            {
                #region ES OBTIENE LISTADO DE ESTADO CIVIL
                List<ComboDTO> lstEstadoCivil = _context.Select<ComboDTO>(new DapperDTO
                {
                    baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
                    consulta = @"SELECT id AS Value, estadoCivil AS Text FROM tblP_EstadoCivil ORDER BY Text"
                });
                resultado.Add(ITEMS, lstEstadoCivil);
                resultado.Add(SUCCESS, true);
                #endregion
            }
            catch (Exception e)
            {
                LogError(0, 0, _NOMBRE_CONTROLADOR, "FillCboEstadosCiviles", e, AccionEnum.CONSULTA, 0, 0);
                resultado.Add(MESSAGE, e.Message);
                resultado.Add(SUCCESS, false);
            }
            return resultado;
        }

        public Dictionary<string, object> FillCboEscolaridades()
        {
            resultado = new Dictionary<string, object>();
            try
            {
                #region FILL CBO
                List<ComboDTO> lstEscolaridades = _context.Select<ComboDTO>(new DapperDTO
                {
                    baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
                    consulta = @"SELECT id AS Value, escolaridad AS Text FROM tblP_CatEscolaridades WHERE registroActivo = @registroActivo ORDER BY orden",
                    parametros = new { registroActivo = true }
                });
                resultado.Add(ITEMS, lstEscolaridades);
                resultado.Add(SUCCESS, true);
                #endregion
            }
            catch (Exception e)
            {
                LogError(0, 0, _NOMBRE_CONTROLADOR, "FillCboEscolaridades", e, AccionEnum.CONSULTA, 0, 0);
                resultado.Add(MESSAGE, e.Message);
                resultado.Add(SUCCESS, false);
            }
            
            return resultado;
        }
        #endregion

        #region GRALES
        public List<tblRH_CatEmpleados> getCatEmpleadosGeneral(string term, int empresa)
        {
            var palabraArr = term.Split(' ');
            var palabra = string.Join("%", palabraArr);
            //var getCatEmpleado = "SELECT TOP 10 clave_empleado, (LTRIM(RTRIM(nombre))+' '+replace(ape_paterno, ' ', '')+' '+replace(ape_materno, ' ', '')) AS Nombre, puesto FROM DBA.sn_empleados WHERE  (LTRIM(RTRIM(nombre))+' '+replace(ape_paterno, ' ', '')+' '+replace(ape_materno, ' ', '')) LIKE '%" + palabra + "%'";
            try
            {
                //var resultado = (List<tblRH_CatEmpleados>)ContextEnKontrolNominaArrendadora.Where(getCatEmpleado, 1).ToObject<List<tblRH_CatEmpleados>>();
                var resultado = _context.Select<tblRH_CatEmpleados>(new DapperDTO
                {
                    baseDatos = (MainContextEnum)empresa,
                    consulta = "SELECT TOP 10 clave_empleado, (LTRIM(RTRIM(nombre))+' '+replace(ape_paterno, ' ', '')+' '+replace(ape_materno, ' ', '')) AS Nombre, puesto FROM tblRH_EK_Empleados WHERE  (LTRIM(RTRIM(nombre))+' '+replace(ape_paterno, ' ', '')+' '+replace(ape_materno, ' ', '')) LIKE '%" + palabra + "%'"
                }).ToList();
                return resultado;
            }
            catch (Exception o_O)
            {
                return new List<tblRH_CatEmpleados>();
            }
        }
        #endregion
    }
}
