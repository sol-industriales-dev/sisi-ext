using System;
using Core.DAO.Administracion.Contratistas;
using Core.Entity.Administrativo.Contratistas;
using Data.EntityFramework.Generic;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.DTO.Principal.Generales;
using Core.DTO.Administracion.Cotnratistas;
using Core.Enum.Principal.Bitacoras;
using System.Web;
using System.IO;
using OfficeOpenXml;

namespace Data.DAO.Administracion.Contratistas
{
    public class EmpleadosDAO : GenericDAO<tblS_IncidentesEmpleadoContratistas>, IEmpleadosDAO
    {
        string Controller = "EmpleadosController";
        private Dictionary<string, object> resultado;


        public List<ComboDTO> ObtenerPais()
        {
            List<ComboDTO> data = _context.tblP_Pais.Select(x => new ComboDTO
            {
                Value = x.idPais.ToString(),
                Text = x.Pais

            }).ToList();
            return data;
        }
        public List<ComboDTO> ObtenerEstado(int idPais)
        {
            List<ComboDTO> data = _context.tblP_Estado.Where(x => x.idPais == idPais).Select(x => new ComboDTO
            {
                Value = x.idEstado.ToString(),
                Text = x.Estado

            }).ToList();
            return data;
        }
        public List<ComboDTO> ObtenerMunicipio(int idEstado)
        {
            List<ComboDTO> data = _context.tblP_Municipio.Where(x => x.idEstado == idEstado).Select(x => new ComboDTO
            {
                Value = x.idMunicipio.ToString(),
                Text = x.Municipio

            }).ToList();
            return data;
        }
        public EmpleadosDTO CrearEditar(EmpleadosDTO _objEmpleados)
        {
            tblS_IncidentesEmpleadoContratistas objAddEdit = new tblS_IncidentesEmpleadoContratistas();
            EmpleadosDTO objRetornar = new EmpleadosDTO();
            try
            {
                DateTime FechaNull = Convert.ToDateTime("01/01/0001 00:00:00");

                //si es diferente de cero tiene que guardar
                if (_objEmpleados.id == 0)
                {
                    tblS_IncidentesEmpleadoContratistas objExite = _context.tblS_IncidentesEmpleadoContratistas.Where(x => x.nombre == _objEmpleados.nombre
                                                                                                     && x.apePaterno == _objEmpleados.apePaterno
                                                                                                     && x.apeMaterno == _objEmpleados.apeMaterno
                                                                                                     && x.colonia == _objEmpleados.colonia
                                                                                                     && x.domicilio == _objEmpleados.domicilio
                                                                                                    ).FirstOrDefault();
                    if (objExite == null)
                    {

                        objAddEdit.idEmpresaContratista = _objEmpleados.idEmpresaContratista;
                        objAddEdit.nombre = _objEmpleados.nombre;
                        objAddEdit.apePaterno = _objEmpleados.apePaterno;
                        objAddEdit.apeMaterno = _objEmpleados.apeMaterno;
                        objAddEdit.domicilio = _objEmpleados.domicilio;
                        objAddEdit.colonia = _objEmpleados.colonia;
                        objAddEdit.idPais = _objEmpleados.idPais;
                        objAddEdit.idEstado = _objEmpleados.idEstado;
                        objAddEdit.idCiudad = _objEmpleados.idCiudad;
                        objAddEdit.codigoPostal = _objEmpleados.codigoPostal;
                        objAddEdit.UMF = _objEmpleados.UMF;
                        objAddEdit.sexo = _objEmpleados.sexo;
                        objAddEdit.localidadNacimiento = _objEmpleados.localidadNacimiento;
                        objAddEdit.estadoNacimiento = _objEmpleados.estadoNacimiento;
                        objAddEdit.numeroSeguroSocial = _objEmpleados.numeroSeguroSocial;
                        objAddEdit.numeroDeIdentificacionOficial = _objEmpleados.numeroDeIdentificacionOficial;
                        objAddEdit.rfc = _objEmpleados.rfc;
                        objAddEdit.curp = _objEmpleados.curp;
                        objAddEdit.nombrePadre = _objEmpleados.nombrePadre;
                        objAddEdit.nombreMadre = _objEmpleados.nombreMadre;
                        objAddEdit.nombreEspo = _objEmpleados.nombreEspo;
                        objAddEdit.beneficiario = _objEmpleados.beneficiario;
                        objAddEdit.parentesco = _objEmpleados.parentesco;
                        objAddEdit.calzado = _objEmpleados.calzado;
                        objAddEdit.tipoSangre = _objEmpleados.tipoSangre;
                        objAddEdit.estadoCivil = _objEmpleados.estadoCivil;
                        objAddEdit.tallaCamisa = _objEmpleados.tallaCamisa;
                        objAddEdit.alergias = _objEmpleados.alergias;
                        objAddEdit.tipoVivienda = _objEmpleados.tipoVivienda;
                        objAddEdit.tallaPantalon = _objEmpleados.tallaPantalon;
                        objAddEdit.overoll = _objEmpleados.overoll;
                        objAddEdit.hijos = _objEmpleados.hijos;
                        objAddEdit.edades = _objEmpleados.edades;
                        objAddEdit.telefono = _objEmpleados.telefono;
                        objAddEdit.celular = _objEmpleados.celular;
                        objAddEdit.puesto = _objEmpleados.puesto;
                        objAddEdit.centroDeCostos = _objEmpleados.centroDeCostos;
                        objAddEdit.jefeInmediato = _objEmpleados.jefeInmediato;
                        objAddEdit.sueldoBase = _objEmpleados.sueldoBase;
                        objAddEdit.complento = _objEmpleados.complento;
                        objAddEdit.total = _objEmpleados.total;
                        objAddEdit.esActivo = true;

                        _context.tblS_IncidentesEmpleadoContratistas.Add(objAddEdit);
                        _context.SaveChanges();
                        objRetornar.status = 1;
                        objRetornar.msjExito = "Usuario Editado Con Exito";
                    }
                    else
                    {
                        objRetornar.status = 2;
                        objRetornar.msjExito = "Ya existe un usuario con estas caracteristicas reportese con TI";
                    }
                }
                else
                {
                    //bienvenido al update
                    objAddEdit = _context.tblS_IncidentesEmpleadoContratistas.Where(x => x.id == _objEmpleados.id).FirstOrDefault();

                    objAddEdit.idEmpresaContratista = _objEmpleados.idEmpresaContratista;
                    objAddEdit.nombre = _objEmpleados.nombre;
                    objAddEdit.apePaterno = _objEmpleados.apePaterno;
                    objAddEdit.apeMaterno = _objEmpleados.apeMaterno;
                    objAddEdit.domicilio = _objEmpleados.domicilio;
                    objAddEdit.colonia = _objEmpleados.colonia;
                    objAddEdit.idPais = _objEmpleados.idPais;
                    objAddEdit.idEstado = _objEmpleados.idEstado;
                    objAddEdit.idCiudad = _objEmpleados.idCiudad;
                    objAddEdit.codigoPostal = _objEmpleados.codigoPostal;
                    objAddEdit.UMF = _objEmpleados.UMF;
                    objAddEdit.sexo = _objEmpleados.sexo;
                    objAddEdit.localidadNacimiento = _objEmpleados.localidadNacimiento;
                    objAddEdit.estadoNacimiento = _objEmpleados.estadoNacimiento;
                    objAddEdit.numeroSeguroSocial = _objEmpleados.numeroSeguroSocial;
                    objAddEdit.numeroDeIdentificacionOficial = _objEmpleados.numeroDeIdentificacionOficial;
                    objAddEdit.rfc = _objEmpleados.rfc;
                    objAddEdit.curp = _objEmpleados.curp;
                    objAddEdit.nombrePadre = _objEmpleados.nombrePadre;
                    objAddEdit.nombreMadre = _objEmpleados.nombreMadre;
                    objAddEdit.nombreEspo = _objEmpleados.nombreEspo;
                    objAddEdit.beneficiario = _objEmpleados.beneficiario;
                    objAddEdit.parentesco = _objEmpleados.parentesco;
                    objAddEdit.calzado = _objEmpleados.calzado;
                    objAddEdit.tipoSangre = _objEmpleados.tipoSangre;
                    objAddEdit.estadoCivil = _objEmpleados.estadoCivil;
                    objAddEdit.tallaCamisa = _objEmpleados.tallaCamisa;
                    objAddEdit.alergias = _objEmpleados.alergias;
                    objAddEdit.tipoVivienda = _objEmpleados.tipoVivienda;
                    objAddEdit.tallaPantalon = _objEmpleados.tallaPantalon;
                    objAddEdit.overoll = _objEmpleados.overoll;
                    objAddEdit.hijos = _objEmpleados.hijos;
                    objAddEdit.edades = _objEmpleados.edades;
                    objAddEdit.telefono = _objEmpleados.telefono;
                    objAddEdit.celular = _objEmpleados.celular;
                    objAddEdit.puesto = _objEmpleados.puesto;
                    objAddEdit.centroDeCostos = _objEmpleados.centroDeCostos;
                    objAddEdit.jefeInmediato = _objEmpleados.jefeInmediato;
                    objAddEdit.sueldoBase = _objEmpleados.sueldoBase;
                    objAddEdit.complento = _objEmpleados.complento;
                    objAddEdit.total = _objEmpleados.total;
                    objAddEdit.esActivo = true;
                    _context.SaveChanges();
                    objRetornar.status = 1;
                    objRetornar.msjExito = "Usuario Editado Con Exito";
                }
            }
            catch (Exception e)
            {
                objRetornar.status = 2;
                objRetornar.msjExito = "algo ocurrio al guardar el registro";
                LogError(2, 0, Controller, "AtblC_EmpresasContratistas", e, AccionEnum.CONSULTA, 0, 0);
                return null;
            }

            return objRetornar;
        }
        public EmpleadosDTO ActivarDesactivar(int id, bool esActivo)
        {
            tblS_IncidentesEmpleadoContratistas objAddEdit = new tblS_IncidentesEmpleadoContratistas();
            EmpleadosDTO objRetornar = new EmpleadosDTO();
            try
            {
                //bienvenido al update
                objAddEdit = _context.tblS_IncidentesEmpleadoContratistas.Where(x => x.id == id).FirstOrDefault();

                objAddEdit.esActivo = esActivo;
                _context.SaveChanges();

                objRetornar.status = 1;
                objRetornar.msjExito = "Se edito correctamente el registro";
            }
            catch (Exception e)
            {
                objRetornar.status = 2;
                objRetornar.msjExito = "algo ocurrio al guardar el registro";
                LogError(2, 0, Controller, "tblS_IncidentesEmpleadoContratistas", e, AccionEnum.CONSULTA, 0, 0);
                return null;
            }

            return objRetornar;
        }

        public List<EmpleadosDTO> getListadoDeEmpleados(int idEmpresa, DateTime FechaAlta, bool esActivo)
        {
            //.Where(x => x.idEmpresaContratista==idEmpresa)
            List<EmpleadosDTO> data = new List<EmpleadosDTO>();

            if (idEmpresa != 0)
            {
                data = _context.tblS_IncidentesEmpleadoContratistas.Where(x => x.idEmpresaContratista == idEmpresa && x.esActivo == esActivo).Select(x => new EmpleadosDTO
                {
                    id = x.id,
                    nombreEmpresa = _context.tblS_IncidentesEmpresasContratistas.Where(y => y.id == x.idEmpresaContratista).Select(i => i.nombreEmpresa).FirstOrDefault(),
                    nombreCompleto = x.nombre + " " + x.apePaterno + " " + x.apeMaterno,
                    cc = x.centroDeCostos,
                    idEmpresaContratista = x.idEmpresaContratista,
                    nombre = x.nombre,
                    apePaterno = x.apePaterno,
                    apeMaterno = x.apeMaterno,
                    domicilio = x.domicilio,
                    colonia = x.colonia,
                    idPais = x.idPais,
                    idEstado = x.idEstado,
                    idCiudad = x.idCiudad,
                    codigoPostal = x.codigoPostal,
                    UMF = x.UMF,
                    sexo = x.sexo,
                    localidadNacimiento = x.localidadNacimiento,
                    estadoNacimiento = x.estadoNacimiento,
                    numeroSeguroSocial = x.numeroSeguroSocial,
                    numeroDeIdentificacionOficial = x.numeroDeIdentificacionOficial,
                    rfc = x.rfc,
                    curp = x.curp,
                    nombrePadre = x.nombrePadre,
                    nombreMadre = x.nombreMadre,
                    nombreEspo = x.nombreEspo,
                    beneficiario = x.beneficiario,
                    parentesco = x.parentesco,
                    calzado = x.calzado,
                    tipoSangre = x.tipoSangre,
                    estadoCivil = x.estadoCivil,
                    tallaCamisa = x.tallaCamisa,
                    alergias = x.alergias,
                    tipoVivienda = x.tipoVivienda,
                    tallaPantalon = x.tallaPantalon,
                    overoll = x.overoll,
                    hijos = x.hijos,
                    edades = x.edades,
                    telefono = x.telefono,
                    celular = x.celular,
                    puesto = x.puesto,
                    centroDeCostos = x.centroDeCostos,
                    jefeInmediato = x.jefeInmediato,
                    sueldoBase = x.sueldoBase,
                    complento = x.complento,
                    total = x.total,
                    esActivo = x.esActivo

                }).ToList();

            }
            else
            {
                data = _context.tblS_IncidentesEmpleadoContratistas.Where(x => x.esActivo == esActivo).Select(x => new EmpleadosDTO
                {
                    id = x.id,
                    nombreEmpresa = _context.tblS_IncidentesEmpresasContratistas.Where(y => y.id == x.idEmpresaContratista).Select(i => i.nombreEmpresa).FirstOrDefault(),
                    nombreCompleto = x.nombre + " " + x.apePaterno + " " + x.apeMaterno,
                    cc = x.centroDeCostos,
                    idEmpresaContratista = x.idEmpresaContratista,
                    nombre = x.nombre,
                    apePaterno = x.apePaterno,
                    apeMaterno = x.apeMaterno,
                    domicilio = x.domicilio,
                    colonia = x.colonia,
                    idPais = x.idPais,
                    idEstado = x.idEstado,
                    idCiudad = x.idCiudad,
                    codigoPostal = x.codigoPostal,
                    UMF = x.UMF,
                    sexo = x.sexo,
                    localidadNacimiento = x.localidadNacimiento,
                    estadoNacimiento = x.estadoNacimiento,
                    numeroSeguroSocial = x.numeroSeguroSocial,
                    numeroDeIdentificacionOficial = x.numeroDeIdentificacionOficial,
                    rfc = x.rfc,
                    curp = x.curp,
                    nombrePadre = x.nombrePadre,
                    nombreMadre = x.nombreMadre,
                    nombreEspo = x.nombreEspo,
                    beneficiario = x.beneficiario,
                    parentesco = x.parentesco,
                    calzado = x.calzado,
                    tipoSangre = x.tipoSangre,
                    estadoCivil = x.estadoCivil,
                    tallaCamisa = x.tallaCamisa,
                    alergias = x.alergias,
                    tipoVivienda = x.tipoVivienda,
                    tallaPantalon = x.tallaPantalon,
                    overoll = x.overoll,
                    hijos = x.hijos,
                    edades = x.edades,
                    telefono = x.telefono,
                    celular = x.celular,
                    puesto = x.puesto,
                    centroDeCostos = x.centroDeCostos,
                    jefeInmediato = x.jefeInmediato,
                    sueldoBase = x.sueldoBase,
                    complento = x.complento,
                    total = x.total,
                    esActivo = x.esActivo

                }).ToList();
            }

            return data;
        }
        public EmpleadosDTO CargaMasivaContratistas(HttpPostedFileBase archivo, int idEmpresa)
        {
            EmpleadosDTO objRetornar = new EmpleadosDTO();
            try
            {
                ExcelPackage package = new ExcelPackage(archivo.InputStream);
                ExcelWorksheet worksheet = package.Workbook.Worksheets.FirstOrDefault();

                // get number of rows and columns in the sheet
                int rows = worksheet.Dimension.Rows; // 20
                int columns = worksheet.Dimension.Columns; // 7
                tblS_IncidentesEmpresasContratistas obj;
                tblS_IncidentesEmpleadoContratistas objEmpleado;
                tblS_IncidentesEmpleadoContratistas objEmpleadoExiste;
                int ultimoid = 0;
                // loop through the worksheet rows and columns
                for (int i = 4; i <= rows; i++)
                {
                    objEmpleado = new tblS_IncidentesEmpleadoContratistas();
                    objEmpleadoExiste = new tblS_IncidentesEmpleadoContratistas();
                    for (int j = 1; j <= columns; j++)
                    {
                        string content = "";
                        if (worksheet.Cells[i, j].Value != null)
                        {
                            content = worksheet.Cells[i, j].Value.ToString();
                        }

                        if (j == 1)
                        {
                            if (content != "")
                            {
                                obj = _context.tblS_IncidentesEmpresasContratistas.Where(r => r.nombreEmpresa == content && r.esActivo).FirstOrDefault();
                                if (obj == null)
                                {
                                    obj = new tblS_IncidentesEmpresasContratistas();
                                    obj.esActivo = true;
                                    obj.nombreEmpresa = content;
                                    _context.tblS_IncidentesEmpresasContratistas.Add(obj);
                                    _context.SaveChanges();
                                    ultimoid = _context.tblS_IncidentesEmpresasContratistas.OrderByDescending(r => r.id).FirstOrDefault().id;
                                }
                                else
                                {
                                    ultimoid = obj.id;
                                }
                            }
                        }

                        switch (j)
                        {
                            case 1:
                                objEmpleado.idEmpresaContratista = ultimoid;
                                break;
                            case 2:
                                objEmpleado.nombre = content;
                                break;
                            case 3:
                                objEmpleado.apePaterno = content;
                                break;
                            case 4:
                                objEmpleado.apeMaterno = content;
                                break;
                            case 5:
                                objEmpleado.domicilio = content;
                                break;
                            case 6:
                                objEmpleado.colonia = content;
                                objEmpleadoExiste = _context.tblS_IncidentesEmpleadoContratistas.Where(r => r.nombre == objEmpleado.nombre && r.apePaterno == objEmpleado.apePaterno && r.apeMaterno == objEmpleado.apeMaterno && r.domicilio == objEmpleado.domicilio && r.colonia == objEmpleado.colonia && r.esActivo).FirstOrDefault();
                                break;
                            case 7:
                                var obp = _context.tblP_Pais.Where(r => r.Pais == content).FirstOrDefault();
                                if (obp != null)
                                {
                                    objEmpleado.idPais = obp.idPais;
                                }
                                break;
                            case 8:
                                var obe = _context.tblP_Estado.Where(r => r.Estado == content).FirstOrDefault();
                                if (obe != null)
                                {
                                    objEmpleado.idEstado = obe.idEstado;
                                }
                                break;
                            case 9:
                                var obm = _context.tblP_Municipio.Where(r => r.Municipio == content).FirstOrDefault();
                                if (obm != null)
                                {
                                    objEmpleado.idCiudad = obm.idMunicipio;
                                }
                                break;
                            case 10:
                                objEmpleado.codigoPostal = content;
                                break;
                            case 11:
                                objEmpleado.UMF = content;
                                break;
                            case 12:
                                if (content.ToUpper().Trim() == "MASCULINO")
                                {
                                    objEmpleado.sexo = "M";
                                }
                                else
                                {
                                    objEmpleado.sexo = "F";
                                }
                                break;
                            case 13:
                                objEmpleado.numeroSeguroSocial = content;
                                break;
                            case 14:
                                objEmpleado.numeroDeIdentificacionOficial = content;
                                break;
                            case 15:
                                objEmpleado.rfc = content;
                                break;
                            case 16:
                                objEmpleado.curp = content;
                                break;
                            case 17:
                                objEmpleado.tipoSangre = content;
                                break;
                            case 18:
                                objEmpleado.alergias = content;
                                break;
                            case 19:
                                objEmpleado.puesto = content;
                                break;
                            case 20:
                                objEmpleado.centroDeCostos = content;
                                objEmpleado.esActivo = true;
                                objEmpleado.localidadNacimiento = "";
                                objEmpleado.estadoNacimiento = "";
                                objEmpleado.nombrePadre = "";
                                objEmpleado.nombreMadre = "";
                                objEmpleado.nombreEspo = "";
                                objEmpleado.beneficiario = "";
                                objEmpleado.parentesco = "";
                                objEmpleado.calzado = "";
                                objEmpleado.estadoCivil = "";
                                objEmpleado.tallaCamisa = "";
                                objEmpleado.tipoVivienda = "";
                                objEmpleado.tallaPantalon = "";
                                objEmpleado.overoll = "";
                                objEmpleado.hijos = "";
                                objEmpleado.edades = "";
                                objEmpleado.telefono = "";
                                objEmpleado.celular = "";
                                objEmpleado.jefeInmediato = "";
                                objEmpleado.sueldoBase = 0;
                                objEmpleado.complento = 0;
                                objEmpleado.total = 0;

                                if (objEmpleadoExiste == null)
                                {
                                    _context.tblS_IncidentesEmpleadoContratistas.Add(objEmpleado);
                                    _context.SaveChanges();
                                }
                                break;
                        }
                    }

                }


                objRetornar.status = 1;
                objRetornar.msjExito = "Guardado con exito";

            }
            catch (Exception e)
            {
                objRetornar.status = 2;
                objRetornar.msjExito = "algo ocurrio al guardar el registro";
                LogError(2, 0, Controller, "tblS_IncidentesEmpleadoContratistas", e, AccionEnum.CONSULTA, 0, 0);
                return null;
            }

            return objRetornar;
        }

    }
}
