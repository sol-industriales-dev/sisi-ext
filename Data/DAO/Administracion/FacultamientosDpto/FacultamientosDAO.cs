using Core.DAO.Administracion.FacultamientosDpto;
using Core.DTO.Principal.Generales;
using System.Linq;
using Core.Entity.Administrativo.FacultamientosDpto;
using Data.EntityFramework.Generic;
using System;
using System.Collections.Generic;
using Core.DTO.Administracion.Facultamiento;
using Data.EntityFramework.Context;
using System.Data.Entity;
using Core.DTO;
using Core.Enum.Principal.Bitacoras;
using Infrastructure.Utils;
using Data.DAO.Principal.Menus;
using Core.Enum.Administracion.Facultamiento;
using Core.Entity.Principal.Alertas;
using Core.Enum.Principal.Alertas;
using Core.Entity.Principal.Multiempresa;
using Core.Enum.Principal;
using Core.Entity.Principal.Usuarios;
using Core.DTO.Utils.Data;
using Core.Enum.Multiempresa;
using Core.Entity.Administrativo.Contabilidad.Nomina;

namespace Data.DAO.Administracion.FacultamientosDpto
{
    public class FacultamientosDAO : GenericDAO<tblFA_Plantilla>, IFacultamientosDAO
    {
        #region variables
        // Variables a utilizar en los diccionarios de resultados.
        public readonly string SUCCESS = "success";
        public readonly string ERROR = "error";
        //Variables a utilizar al momento de loguear algún error.
        private readonly int SISTEMA_ID = 7;
        private readonly int MODULO_CATALOGO_ID = 6218;
        private readonly int MODULO_ASIGNACION_ID = 7218;
        private readonly int MODULO_AUTORIZACION_ID = 7222;
        private readonly int MODULO_HISTORICO_ID = 7223;
        private readonly int MODULO_EMPLEADO_ID = 7227;
        private readonly string NOMBRE_CONTROLADOR = "FacultamientosController";
        Dictionary<string, object> resultado = new Dictionary<string, object>();
        private string NombreControlador = "FacultamientosController";
        #endregion

        #region metodos catalogo
        public List<ComboDTO> ObtenerDepartamentos()
        {
            var listaDepartamentos = new List<ComboDTO>();
            try
            {
                listaDepartamentos = _context.tblFA_Grupos.Where(x => x.estatus).Select(x => new ComboDTO
                {
                    Value = x.id.ToString(),
                    Text = x.descripcion.Trim(),
                    Prefijo = ""
                }).OrderBy(x => x.Text).ToList();
            }
            catch (Exception e)
            {
                logErrorFacultamientos(MODULO_CATALOGO_ID, "ObtenerDepartamentos", e, AccionEnum.CONSULTA, 0, null);
                return new List<ComboDTO>();
            }

            return listaDepartamentos;
        }

        public Dictionary<string, object> GuardarPlantilla(string titulo, List<int> listaDepartamentos, List<ConceptoDTO> listaConceptos)
        {
            var resultado = new Dictionary<string, object>();

            var plantilla = new tblFA_Plantilla();
            var aut = new tblFA_ConceptoPlantilla();
            var plantillaDepartamento = new tblFA_PlantillatblFA_Grupos();

            using (var context = new MainContext())
            {
                using (DbContextTransaction dbContextTransaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        try
                        {
                            var count = context.tblFA_Plantilla.Count();
                            // Creación de la plantilla.
                            plantilla.titulo = titulo.Trim();
                            plantilla.fechaCreacion = DateTime.Now;
                            plantilla.esActiva = true;
                            plantilla.usuarioCreadorID = vSesiones.sesionUsuarioDTO.id;
                            plantilla.orden = count + 1;
                            context.tblFA_Plantilla.Add(plantilla);
                            context.SaveChanges();
                        }
                        catch (Exception e)
                        {
                            dbContextTransaction.Rollback();
                            resultado.Add(SUCCESS, false);
                            resultado.Add(ERROR, "Ocurrió un error interno al intentar guardar la plantilla seleccionada");
                            logErrorFacultamientos(MODULO_CATALOGO_ID, "GuardarPlantilla", e, AccionEnum.AGREGAR, 0, plantilla);
                            return resultado;
                        }
                        try
                        {
                            // Guardado de la lista de conceptos.
                            for (int i = 0; i < listaConceptos.ToArray().Count(); i++)
                            {
                                aut = new tblFA_ConceptoPlantilla();
                                aut.plantillaID = plantilla.id;
                                aut.concepto = listaConceptos.ToArray()[i].Concepto.Trim();
                                aut.esAutorizacion = listaConceptos.ToArray()[i].EsAutorizante;
                                aut.orden = i;
                                aut.esActivo = true;
                                context.tblFA_ConceptoPlantilla.Add(aut);
                                context.SaveChanges();
                                listaConceptos.ToArray()[i].ID = aut.id;
                            }
                        }
                        catch (Exception e)
                        {
                            dbContextTransaction.Rollback();
                            resultado.Add(SUCCESS, false);
                            resultado.Add(ERROR, "Ocurrió un error interno al intentar guardar la lista de autorización de la plantilla");
                            logErrorFacultamientos(MODULO_CATALOGO_ID, "GuardarPlantilla", e, AccionEnum.AGREGAR, 0, aut);
                            return resultado;
                        }
                        try
                        {
                            // Guardado de la relacion entre la plantilla y los departamentos.
                            foreach (int departamentoID in listaDepartamentos)
                            {
                                plantillaDepartamento = new tblFA_PlantillatblFA_Grupos();
                                plantillaDepartamento.plantillaID = plantilla.id;
                                plantillaDepartamento.grupoID = departamentoID;
                                context.tblFA_PlantillatblFA_Grupos.Add(plantillaDepartamento);
                            }
                        }
                        catch (Exception e)
                        {
                            dbContextTransaction.Rollback();
                            resultado.Add(SUCCESS, false);
                            resultado.Add(ERROR, "Ocurrió un error interno al intentar guardar la lista de departamentos de la plantilla");
                            logErrorFacultamientos(MODULO_CATALOGO_ID, "GuardarPlantilla", e, AccionEnum.AGREGAR, 0, plantillaDepartamento);
                            return resultado;
                        }

                        #region crear o actualizar paquetes existentes
                        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                        // Se buscan a todos los paquetes existentes que sean de un CC perteneciente a un departamento de los de la nueva plantilla
                        // y se les agrega registros vacíos de la nueva plantilla.
                        // Si algun paquete ya estaba en autorizado, se creará otra versión nueva con la nueva plantilla agregada.
                        try
                        {
                            var listaPaquetes = new List<tblFA_Paquete>();
                            listaDepartamentos.ForEach(departamentoID =>
                            {
                                List<tblFA_Paquete> paquete = new List<tblFA_Paquete>();

                                if (vSesiones.sesionEmpresaActual == (int)EmpresaEnum.Construplan || vSesiones.sesionEmpresaActual == (int)EmpresaEnum.GCPLAN)
                                {
                                    paquete = context.tblFA_Paquete.Where(y => y.esActivo != false && y.cc.grupoID == departamentoID).ToList();
                                }
                                else
                                {
                                    var listaRelacionCatalogoCCGrupo = context.tblC_Nom_CatalogoCCtblFA_Grupos.Where(x => x.registroActivo && x.grupo_id == departamentoID).Select(x => x.catalogoCC_id).ToList();

                                    paquete = context.tblFA_Paquete.Where(y => y.esActivo != false && listaRelacionCatalogoCCGrupo.Contains(y.ccID)).ToList();
                                }

                                listaPaquetes.AddRange(paquete);
                            });

                            listaPaquetes.ForEach(paquete =>
                            {
                                // Si el estado del paquete está en como null, significa que está editando o en proceso de autorización.
                                // Se vuelve a poner en editando y se le agregan los nuevos conceptos
                                if (paquete.esActivo == null)
                                {
                                    paquete.estado = (int)EstadoPaqueteFaEnum.Editando;

                                    // Se crea el nuevo facultamiento con los nuevos registros.
                                    var nuevoFacultamiento = new tblFA_Facultamiento();
                                    nuevoFacultamiento.paqueteID = paquete.id;
                                    nuevoFacultamiento.plantillaID = plantilla.id;
                                    nuevoFacultamiento.aplica = false;
                                    context.tblFA_Facultamiento.Add(nuevoFacultamiento);
                                    context.SaveChanges();

                                    // Por cada concepto en la plantilla, se agrega un nuevo registro de empleado a cada paquete.
                                    listaConceptos.ForEach(concepto =>
                                    {
                                        var empleado = new tblFA_Empleado
                                        {
                                            conceptoID = concepto.ID,
                                            editado = false,
                                            esActivo = true,
                                            facultamientoID = nuevoFacultamiento.id,
                                            aplica = false,
                                            nombreEmpleado = null,
                                            claveEmpleado = null
                                        };
                                        context.tblFA_Empleado.Add(empleado);
                                    });
                                    paquete.comentario = String.Format("Se añadió el facultamiento de {0} al Paquete de Facultamientos.", titulo);
                                }

                                // Si el estado del paquete está en activo, significa que ya está autorizado.
                                // Se procede a crear otro versión del paquete con los nuevos conceptos agregados.
                                else
                                {
                                    bool esPendiente = true;
                                    var listaPaquetesPendientes = listaPaquetes.Where(y => y.ccID.Equals(paquete.ccID) && y.id != paquete.id).ToList();
                                    listaPaquetesPendientes.ForEach(paquetePendiente =>
                                    {
                                        if (paquetePendiente.esActivo == null)
                                        {
                                            esPendiente = false;
                                        }
                                    });
                                    // Si el paquete ya está autorizado y no hay otra versión del mismo en editando o pendiente de autorizar, se crea otra versión.
                                    if ((paquete.estado.Equals((int)EstadoPaqueteFaEnum.Autorizado)) &&
                                        (paquete.esActivo ?? false) &&
                                        esPendiente)
                                    {
                                        // Se crea la nueva versión del paquete con los nuevos conceptos y se deja como editando.
                                        var nuevoPaquete = new tblFA_Paquete();
                                        nuevoPaquete.ccID = paquete.ccID;
                                        nuevoPaquete.estado = (int)EstadoPaqueteFaEnum.Editando;
                                        nuevoPaquete.fechaCreacion = DateTime.Now;
                                        nuevoPaquete.esActivo = null;
                                        nuevoPaquete.usuarioCreadorID = vSesiones.sesionUsuarioDTO.id;
                                        nuevoPaquete.comentario = String.Format("Se añadió el facultamiento de {0} al Paquete de Facultamientos.", titulo);
                                        context.tblFA_Paquete.Add(nuevoPaquete);
                                        context.SaveChanges();

                                        var listaFacultamientos = context.tblFA_Facultamiento.Where(y => y.paqueteID.Equals(paquete.id)).ToList();

                                        // Guardado de la lista de facultamientos.
                                        foreach (var facultamiento in listaFacultamientos)
                                        {
                                            var nuevoFacultamiento = new tblFA_Facultamiento();
                                            nuevoFacultamiento.paqueteID = nuevoPaquete.id;
                                            nuevoFacultamiento.plantillaID = facultamiento.plantillaID;
                                            nuevoFacultamiento.aplica = facultamiento.aplica;
                                            context.tblFA_Facultamiento.Add(nuevoFacultamiento);
                                            context.SaveChanges();

                                            // Se procede a guardar la lista de empleados de cada facultamiento.
                                            var listaEmpleados = context.tblFA_Empleado.Where(y => y.facultamientoID.Equals(facultamiento.id)).ToList();

                                            listaEmpleados.ForEach(x =>
                                            {
                                                var empleado = new tblFA_Empleado();
                                                empleado.conceptoID = x.conceptoID;
                                                empleado.editado = false;
                                                empleado.esActivo = true;
                                                empleado.facultamientoID = nuevoFacultamiento.id;
                                                empleado.aplica = x.aplica;
                                                if (x.claveEmpleado != null)
                                                {
                                                    empleado.nombreEmpleado = x.nombreEmpleado.Trim();
                                                    empleado.claveEmpleado = x.claveEmpleado;
                                                }
                                                else
                                                {
                                                    empleado.nombreEmpleado = null;
                                                    empleado.claveEmpleado = null;
                                                }
                                                context.tblFA_Empleado.Add(empleado);
                                            });
                                        }

                                        // Se agrega el nuevo facultamiento a la plantilla.
                                        var facultamientoAgregado = new tblFA_Facultamiento();
                                        facultamientoAgregado.plantillaID = plantilla.id;
                                        facultamientoAgregado.paqueteID = nuevoPaquete.id;
                                        facultamientoAgregado.aplica = false;
                                        context.tblFA_Facultamiento.Add(facultamientoAgregado);
                                        context.SaveChanges();

                                        // Se agregan los nuevos empleados(conceptos) de la plantilla.
                                        listaConceptos.ForEach(y =>
                                        {
                                            var empleado = new tblFA_Empleado
                                            {
                                                conceptoID = y.ID,
                                                editado = false,
                                                esActivo = true,
                                                facultamientoID = facultamientoAgregado.id,
                                                aplica = false,
                                                nombreEmpleado = null,
                                                claveEmpleado = null
                                            };
                                            context.tblFA_Empleado.Add(empleado);
                                        });

                                        // Se agregan los autorizantes del paquete anterior.
                                        int contadorOrden = 0;
                                        paquete.autorizantes.ForEach(y =>
                                        {
                                            var autorizante = new tblFA_Autorizante();
                                            autorizante.esAutorizante = (contadorOrden.Equals(0));
                                            autorizante.autorizado = null;
                                            autorizante.firma = null;
                                            autorizante.orden = ++contadorOrden;
                                            autorizante.paqueteID = nuevoPaquete.id;
                                            autorizante.usuarioID = (y.usuarioID != null) ? y.usuarioID : null;
                                            context.tblFA_Autorizante.Add(autorizante);
                                        });

                                        context.SaveChanges();
                                    }
                                }
                            });
                        }

                        catch (Exception e)
                        {
                            dbContextTransaction.Rollback();
                            resultado.Add(SUCCESS, false);
                            resultado.Add(ERROR, "Ocurrió un error interno al intentar actualizar los facultamientos ya existentes.");
                            logErrorFacultamientos(MODULO_CATALOGO_ID, "GuardarPlantilla", e, AccionEnum.AGREGAR, 0, plantillaDepartamento);
                            return resultado;
                        }
                        #endregion

                        context.SaveChanges();
                        dbContextTransaction.Commit();
                        //if (new MenuDAO().isLiberado(vSesiones.sesionCurrentView))
                        //{
                        //    string objeto;
                        //    try
                        //    {
                        //        objeto = JsonUtils.convertNetObjectToJson(plantilla);
                        //    }
                        //    catch (Exception)
                        //    {
                        //        objeto = "";
                        //    }
                        //    SaveBitacora((int)BitacoraEnum.PlantillaFacultamiento, (int)AccionEnum.AGREGAR, plantilla.id, objeto);
                        //}
                        resultado.Add(SUCCESS, true);
                    }
                    catch (Exception e)
                    {
                        dbContextTransaction.Rollback();
                        resultado.Add(SUCCESS, false);
                        resultado.Add(ERROR, "Ocurrió un error interno al intentar guardar la plantilla");
                        logErrorFacultamientos(MODULO_CATALOGO_ID, "GuardarPlantilla", e, AccionEnum.AGREGAR, 0, plantilla);
                        return resultado;
                    }
                }
            }

            return resultado;
        }

        public Dictionary<string, object> ObtenerCatalogo(int departamentoID)
        {
            #region VERSION 1 //COMENTADO
            //resultado = new Dictionary<string, object>();
            //var listaFacultamientos = new List<CatalogoPlantillaFaDTO>();

            //var lista = _context.tblFA_PlantillatblFA_Grupos
            //    .Where(x =>
            //        (departamentoID.Equals(0)) ? true : x.grupoID.Equals(departamentoID))
            //    .Select(x => x.plantillaID)
            //    .Distinct()
            //    .ToList();
            //var facultamientos = _context.tblFA_Plantilla.Where(x => (departamentoID == 0 ? true : lista.Contains(x.id)) && x.esActiva).OrderBy(x => x.orden).ToList();

            //foreach (var i in facultamientos)
            //{
            //    var cantidad = _context.tblFA_PlantillatblFA_Grupos.Where(x => x.plantillaID == i.id).ToList().Count();
            //    var o = new CatalogoPlantillaFaDTO();
            //    o.orden = i.orden;
            //    o.PlantillaID = i.id;
            //    o.Titulo = i.titulo;
            //    o.Departamento = cantidad == 0 ? i.titulo : (cantidad + " departamentos");
            //    o.Fecha = (i.fechaModificacion != null) ? i.fechaModificacion.ToString() : i.fechaCreacion.ToString();

            //    listaFacultamientos.Add(o);
            //}
            //if (listaFacultamientos.Count > 0)
            //{
            //    resultado.Add(SUCCESS, true);
            //    resultado.Add("listaFacultamientos", listaFacultamientos.OrderBy(x => x.orden).ToList());
            //}
            //else
            //{
            //    resultado.Add(SUCCESS, false);
            //    resultado.Add("EMPTY", true);
            //}

            //return resultado;
            #endregion

            resultado = new Dictionary<string, object>();
            try
            {
                List<CatalogoPlantillaFaDTO> lstFacultamientos = new List<CatalogoPlantillaFaDTO>();
                List<int> lista = _context.tblFA_PlantillatblFA_Grupos.Where(x => (departamentoID.Equals(0)) ? true : x.grupoID.Equals(departamentoID)).Select(x => x.plantillaID).Distinct().ToList();
                List<tblFA_Plantilla> facultamientos = _context.tblFA_Plantilla.Where(x => (departamentoID == 0 ? true : lista.Contains(x.id)) && x.esActiva).OrderBy(x => x.orden).ToList();

                foreach (var objFacultamiento in facultamientos)
                {
                    int cantidad = _context.tblFA_PlantillatblFA_Grupos.Where(x => x.plantillaID == objFacultamiento.id).ToList().Count();
                    CatalogoPlantillaFaDTO objPlantillaDTO = new CatalogoPlantillaFaDTO();
                    objPlantillaDTO.orden = objFacultamiento.orden;
                    objPlantillaDTO.PlantillaID = objFacultamiento.id;
                    objPlantillaDTO.Titulo = objFacultamiento.titulo;
                    objPlantillaDTO.Departamento = cantidad == 0 ? objFacultamiento.titulo : string.Format("{0} departamentos", cantidad);
                    objPlantillaDTO.Fecha = (objFacultamiento.fechaModificacion != null) ? objFacultamiento.fechaModificacion.ToString() : objFacultamiento.fechaCreacion.ToString();
                    lstFacultamientos.Add(objPlantillaDTO);
                }

                if (lstFacultamientos.Count > 0)
                {
                    resultado.Add(SUCCESS, true);
                    resultado.Add("listaFacultamientos", lstFacultamientos.OrderBy(x => x.orden).ToList());
                }
                else
                {
                    resultado.Add(SUCCESS, false);
                    resultado.Add("EMPTY", true);
                }
            }
            catch (Exception e)
            {
                LogError(0, 0, NombreControlador, "ObtenerCatalogo", e, AccionEnum.CONSULTA, 0, new { departamentoID = departamentoID });
                resultado.Add(MESSAGE, e.Message);
                resultado.Add(SUCCESS, false);
            }
            return resultado;
        }

        public Dictionary<string, object> ObtenerPlantilla(int plantillaID)
        {
            var resultado = new Dictionary<string, object>();
            var plantilla = new tblFA_Plantilla();
            var plantillaDTO = new PlantillaFacultamientoDTO();
            try
            {
                plantilla = _context.tblFA_Plantilla.FirstOrDefault(x => x.id.Equals(plantillaID));
                plantillaDTO.Titulo = plantilla.titulo;
                plantillaDTO.ListaConceptos = _context.tblFA_ConceptoPlantilla
                    .Where(x => x.plantillaID.Equals(plantilla.id) && x.esActivo)
                    .OrderBy(x => x.orden)
                    .Select(x => new ConceptoDTO
                    {
                        Concepto = x.concepto,
                        EsAutorizante = x.esAutorizacion,
                        ID = x.id
                    })
                    .ToList();
                plantillaDTO.ListaDepartamentos = _context.tblFA_PlantillatblFA_Grupos.Where(x => x.plantillaID.Equals(plantilla.id)).Select(x => x.grupoID).ToList();

                resultado.Add(SUCCESS, true);
                resultado.Add("plantilla", plantillaDTO);
            }
            catch (Exception e)
            {
                resultado.Add(SUCCESS, false);
                resultado.Add(ERROR, "Ocurrió un error interno al intentar obtener la plantilla");
                logErrorFacultamientos(MODULO_CATALOGO_ID, "ObtenerPlantilla", e, AccionEnum.CONSULTA, plantillaID, null);
                return resultado;
            }

            return resultado;
        }

        public Dictionary<string, object> ActualizarPlantilla(string nuevoTitulo, List<int> nuevosDepartamentos, List<ConceptoDTO> nuevosConceptos, int plantillaID, bool esActualizar = true)
        {
            var resultado = new Dictionary<string, object>();
            tblFA_Plantilla plantilla = null;
            tblFA_ConceptoPlantilla concepto = null;
            tblFA_PlantillatblFA_Grupos plantillaDepartamento = null;

            using (var context = new MainContext())
            {
                using (DbContextTransaction dbContextTransaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        if (esActualizar)
                        {
                            #region SE ACTUALIZA EL TITULO DE LA PLANTILLA
                            try
                            {
                                plantilla = context.tblFA_Plantilla.FirstOrDefault(x => x.id.Equals(plantillaID) && x.esActiva);
                                plantilla.titulo = nuevoTitulo.Trim();
                                plantilla.fechaModificacion = DateTime.Now;
                                plantilla.usuarioCreadorID = vSesiones.sesionUsuarioDTO.id;
                                context.SaveChanges();
                            }
                            catch (Exception e)
                            {
                                dbContextTransaction.Rollback();
                                resultado.Add(SUCCESS, false);
                                resultado.Add(ERROR, "Ocurrió un error interno al intentar actualizar la plantilla seleccionada");
                                logErrorFacultamientos(MODULO_CATALOGO_ID, "ActualizarPlantilla", e, AccionEnum.ACTUALIZAR, plantillaID, plantilla);
                                return resultado;
                            }
                            #endregion

                            #region SE REGISTRA EL NUEVO CONCEPTO EN LA PLANTILLA (CUANDO ES ACTUALIZAR)
                            int idPlantilla = plantillaID;

                            // SE OBTIENE LISTADO DE PAQUETES EN BASE A LA PLANTILLA ID
                            List<tblFA_Facultamiento> lstFacultamientosConRegistrosDuplicados = context.tblFA_Facultamiento.Where(w => w.plantillaID == idPlantilla).ToList();
                            List<tblFA_Facultamiento> lstFacultamientosSinRegistrosDuplicados = new List<tblFA_Facultamiento>();

                            foreach (var item in lstFacultamientosConRegistrosDuplicados)
                            {
                                tblFA_Facultamiento objFacultamiento = lstFacultamientosSinRegistrosDuplicados.Where(w => w.paqueteID == item.paqueteID).FirstOrDefault();
                                if (objFacultamiento == null)
                                {
                                    objFacultamiento = lstFacultamientosConRegistrosDuplicados.Where(w => w.paqueteID == item.paqueteID).FirstOrDefault();
                                    lstFacultamientosSinRegistrosDuplicados.Add(objFacultamiento);
                                }
                            }

                            List<int> lstConceptosNuevosID = new List<int>();
                            foreach (var item in nuevosConceptos)
                            {
                                // SE VERIFICA SI EL CONCEPTO YA SE ENCUENTRA EN LA BASE DE DATOS
                                tblFA_ConceptoPlantilla objCEConcepto = context.tblFA_ConceptoPlantilla.Where(w => w.plantillaID == idPlantilla && w.id == item.ID && w.esActivo).FirstOrDefault();
                                if (objCEConcepto == null)
                                {
                                    // SE OBTIENE EL ULTIMO ORDEN DEL CONCEPTO
                                    int numOrden = context.tblFA_ConceptoPlantilla.Where(w => w.plantillaID == idPlantilla).Select(s => s.orden).OrderByDescending(o => o).FirstOrDefault();

                                    // SE REGISTRA EL CONCEPTO
                                    objCEConcepto = new tblFA_ConceptoPlantilla();
                                    objCEConcepto.plantillaID = idPlantilla;
                                    objCEConcepto.concepto = item.Concepto;
                                    objCEConcepto.esAutorizacion = item.EsAutorizante;
                                    objCEConcepto.orden = numOrden + 1;
                                    objCEConcepto.esActivo = true;
                                    context.tblFA_ConceptoPlantilla.Add(objCEConcepto);
                                    context.SaveChanges();

                                    // SE OBTIENE EL CONCEPTO_ID RECIEN REGISTRADOS
                                    int idConcepto = context.tblFA_ConceptoPlantilla.Select(s => s.id).OrderByDescending(o => o).FirstOrDefault();
                                    lstConceptosNuevosID.Add(idConcepto);
                                }
                                else
                                {
                                    // SE ACTUALIZA EL CONCEPTO
                                    objCEConcepto.concepto = item.Concepto;
                                    objCEConcepto.esAutorizacion = item.EsAutorizante;
                                    context.SaveChanges();
                                }
                            }

                            foreach (var item in lstFacultamientosSinRegistrosDuplicados)
                            {
                                // SE REGISTRA RELACIÓN CONCEPTO CON EMPLEADO
                                int idFacultamiento = item.id;
                                foreach (var itemConcepto in lstConceptosNuevosID)
                                {
                                    tblFA_Empleado objCE = new tblFA_Empleado();
                                    objCE.conceptoID = itemConcepto;
                                    objCE.facultamientoID = idFacultamiento;
                                    objCE.editado = false;
                                    objCE.esActivo = true;
                                    objCE.aplica = false;
                                    context.tblFA_Empleado.Add(objCE);
                                    context.SaveChanges();
                                }
                            }
                            #endregion

                            #region DEPARTAMENTOS
                            if (nuevosDepartamentos != null)
                            {
                                var depsPlantilla = context.tblFA_PlantillatblFA_Grupos.Where(x => x.plantillaID == plantillaID).ToList();
                                foreach (var item in depsPlantilla)
                                {
                                    if (!nuevosDepartamentos.Contains(item.grupoID))
                                    {
                                        context.tblFA_PlantillatblFA_Grupos.Remove(item);
                                        context.SaveChanges();
                                    }
                                }

                                foreach (var item in nuevosDepartamentos)
                                {
                                    var existeGrupo = context.tblFA_PlantillatblFA_Grupos.FirstOrDefault(x => x.plantillaID == plantillaID && x.grupoID == item);
                                    if (existeGrupo == null)
                                    {
                                        var nuevoGrupo = new tblFA_PlantillatblFA_Grupos();
                                        nuevoGrupo.plantillaID = plantillaID;
                                        nuevoGrupo.grupoID = item;
                                        context.tblFA_PlantillatblFA_Grupos.Add(nuevoGrupo);
                                        context.SaveChanges();
                                    }
                                }
                            }
                            #endregion

                            #region SE RELACIONA EL GRUPO DE CC CON EL FACULTAMIENTO
                            if (nuevosDepartamentos.Count() > 0)
                            {
                                List<string> lstDepartamentosID = new List<string>();
                                foreach (var item in nuevosDepartamentos)
                                {
                                    string idDepartamento = item.ToString();
                                    lstDepartamentosID.Add(idDepartamento);
                                }

                                // SE OBTIENE LOS CC_ID QUE TENGAN EL GRUPO_ID (DEPARTAMENTO)
                                //string strQuery = string.Empty;
                                //strQuery = string.Format("SELECT id FROM tblP_CC WHERE grupoID IN ({0})", string.Join(",", lstDepartamentosID));
                                //List<tblP_CC> lstCC = _context.Select<tblP_CC>(new DapperDTO
                                //{
                                //    baseDatos = (int)vSesiones.sesionEmpresaActual == (int)EmpresaEnum.Construplan ? MainContextEnum.Construplan : MainContextEnum.Arrendadora,
                                //    consulta = strQuery
                                //}).ToList();

                                var lstDeptos = string.Join(",", lstDepartamentosID);
                                var lstCC = context.tblP_CC.Where(e => lstDeptos.Contains(e.grupoID.ToString())).ToList();

                                List<int> lstCC_ID = new List<int>();
                                foreach (var item in lstCC)
                                {
                                    int idCC = Convert.ToInt32(item.id);
                                    lstCC_ID.Add(idCC);
                                }

                                // SE OBTIENE LISTADO PAQUETE_ID EN BASE A LOS CC_ID
                                List<tblFA_Paquete> lstPaquetes = context.tblFA_Paquete.Where(w => lstCC_ID.Contains(w.ccID)).ToList();
                                List<int> lstPaquetes_ID = new List<int>();
                                foreach (var item in lstPaquetes)
                                {
                                    int idPaquete = Convert.ToInt32(item.id);
                                    lstPaquetes_ID.Add(idPaquete);
                                }

                                // PAQUETES YA CON FACULTAMIENTOS
                                List<tblFA_Facultamiento> lstPaquetesEnFacultamientos = context.tblFA_Facultamiento.Where(w => w.plantillaID == plantillaID && lstPaquetes_ID.Contains(w.paqueteID)).ToList();

                                // PAQUETES SIN FACULTAMIENTOS
                                List<int> lstPaquetesSinFacultamientos = lstPaquetes_ID.Where(w => !lstPaquetesEnFacultamientos.Select(s => s.paqueteID).Contains(w)).Select(s => s).ToList();

                                // SE REGISTRA EL PAQUETE RELACIONANDOLO CON EL FACULTAMIENTO
                                List<tblFA_Facultamiento> lstFacultamientosNuevos = new List<tblFA_Facultamiento>();
                                tblFA_Facultamiento objFacultamientoNuevo = new tblFA_Facultamiento();
                                foreach (var item in lstPaquetesSinFacultamientos)
                                {
                                    objFacultamientoNuevo = new tblFA_Facultamiento();
                                    objFacultamientoNuevo.plantillaID = plantillaID;
                                    objFacultamientoNuevo.paqueteID = item;
                                    objFacultamientoNuevo.aplica = false;
                                    lstFacultamientosNuevos.Add(objFacultamientoNuevo);
                                }
                                context.tblFA_Facultamiento.AddRange(lstFacultamientosNuevos);
                                context.SaveChanges();

                                #region SE RELACIONA LOS CONCEPTOS Y EL FACULTAMIENTO

                                // SE OBTIENE LOS CONCEPTOS RELACIONADOS A FACULTAMIENTO (CONCEPTOS)
                                List<tblFA_ConceptoPlantilla> lstConceptos = context.tblFA_ConceptoPlantilla.Where(w => w.plantillaID == plantillaID && w.esActivo).OrderBy(o => o.orden).ToList();
                                List<int> lstConceptos_ID = new List<int>();
                                foreach (var item in lstConceptos)
                                {
                                    lstConceptos_ID.Add(item.id);
                                }

                                // SE OBTIENE LOS FACULTAMIENTOS_ID DE LA PLANTILLA
                                List<tblFA_Facultamiento> lstFacultamientos = context.tblFA_Facultamiento.Where(w => w.plantillaID == plantillaID).OrderBy(o => o.paqueteID).ToList();
                                List<int> lstFacultamientos_ID = new List<int>();
                                foreach (var item in lstFacultamientos)
                                {
                                    lstFacultamientos_ID.Add(item.id);
                                }

                                // SE REGISTRA EL FACULTAMIENTO CON TODOS LOS CONCEPTOS ENCONTRADOS
                                List<tblFA_Empleado> lstFaEmpleadosGuardar = new List<tblFA_Empleado>();
                                tblFA_Empleado objFaEmpleadoGuardar = new tblFA_Empleado();
                                foreach (var item in lstFacultamientos_ID)
                                {
                                    int idFacultamiento = item;
                                    if (idFacultamiento > 0)
                                    {
                                        int idConcepto = 0;
                                        foreach (var itemConcepto in lstConceptos_ID)
                                        {
                                            idConcepto = itemConcepto;

                                            // SE VERIFICA SI EL FACULTAMIENTO CON EL CONCEPTO YA EXISTEN
                                            //tblFA_Empleado objFaEmpleado = _context.Select<tblFA_Empleado>(new DapperDTO
                                            //{
                                            //    baseDatos = (int)vSesiones.sesionEmpresaActual == (int)EmpresaEnum.Construplan ? MainContextEnum.Construplan : MainContextEnum.Arrendadora,
                                            //    consulta = @"SELECT * FROM tblFA_Empleado WHERE conceptoID = @conceptoID AND facultamientoID = @facultamientoID AND esActivo = @esActivo",
                                            //    parametros = new { conceptoID = idConcepto, facultamientoID = idFacultamiento, esActivo = true }
                                            //}).FirstOrDefault();

                                            var objFaEmpleado = context.tblFA_Empleado.FirstOrDefault(e => e.conceptoID == idConcepto && e.facultamientoID == idFacultamiento && e.esActivo);

                                            if (objFaEmpleado == null)
                                            {
                                                #region SE REGISTRA EL FACULTAMIENTO CON EL CONCEPTO
                                                objFaEmpleadoGuardar = new tblFA_Empleado();
                                                objFaEmpleadoGuardar.nombreEmpleado = null;
                                                objFaEmpleadoGuardar.claveEmpleado = null;
                                                objFaEmpleadoGuardar.conceptoID = idConcepto;
                                                objFaEmpleadoGuardar.facultamientoID = idFacultamiento;
                                                objFaEmpleadoGuardar.editado = false;
                                                objFaEmpleadoGuardar.esActivo = true;
                                                objFaEmpleadoGuardar.aplica = false;
                                                lstFaEmpleadosGuardar.Add(objFaEmpleadoGuardar);
                                                #endregion
                                            }
                                        }
                                    }
                                }
                                context.tblFA_Empleado.AddRange(lstFaEmpleadosGuardar);
                                context.SaveChanges();
                                #endregion
                            }
                            #endregion
                        }
                        else
                        {
                            #region
                            try
                            {
                                // Actualización de la plantilla.
                                plantilla = context.tblFA_Plantilla.FirstOrDefault(x => x.id.Equals(plantillaID) && x.esActiva);
                                plantilla.titulo = nuevoTitulo.Trim();
                                plantilla.fechaModificacion = DateTime.Now;
                                plantilla.usuarioCreadorID = vSesiones.sesionUsuarioDTO.id;
                                context.SaveChanges();
                            }
                            catch (Exception e)
                            {
                                dbContextTransaction.Rollback();
                                resultado.Add(SUCCESS, false);
                                resultado.Add(ERROR, "Ocurrió un error interno al intentar actualizar la plantilla seleccionada");
                                logErrorFacultamientos(MODULO_CATALOGO_ID, "ActualizarPlantilla", e, AccionEnum.ACTUALIZAR, plantillaID, plantilla);
                                return resultado;
                            }

                            try
                            {
                                // Se actualizan los conceptos
                                nuevosConceptos.ForEach(x =>
                                {
                                    var conceptoActual = context.tblFA_ConceptoPlantilla.FirstOrDefault(y => y.id == x.ID);
                                    if (conceptoActual != null)
                                    {
                                        conceptoActual.concepto = x.Concepto;
                                        conceptoActual.esAutorizacion = x.EsAutorizante;
                                        x.PlantillaID = 1; // Se marca con uno para saber que concepto se editó (ya existente) y así poder sacar los nuevos conceptos.
                                    }
                                });

                                // Se agregan los nuevos conceptos
                                int contadorOrden = nuevosConceptos.Where(x => x.PlantillaID > 0).ToList().Count - 1;
                                nuevosConceptos.Where(x => x.PlantillaID < 1).ToList().ForEach(x =>
                                {
                                    concepto = new tblFA_ConceptoPlantilla();
                                    concepto.plantillaID = plantilla.id;
                                    concepto.concepto = x.Concepto.Trim();
                                    concepto.esAutorizacion = x.EsAutorizante;
                                    concepto.orden = ++contadorOrden;
                                    concepto.esActivo = true;
                                    context.tblFA_ConceptoPlantilla.Add(concepto);
                                    context.SaveChanges();
                                    x.ID = concepto.id;
                                });
                            }
                            catch (Exception e)
                            {
                                dbContextTransaction.Rollback();
                                resultado.Add(SUCCESS, false);
                                resultado.Add(ERROR, "Ocurrió un error interno al actualizar la lista de conceptos de la plantilla");
                                logErrorFacultamientos(MODULO_CATALOGO_ID, "ActualizarPlantilla", e, AccionEnum.CONSULTA, plantillaID, null);
                                return resultado;
                            }

                            try
                            {
                                // Guardado de la relacion entre la plantilla y los departamentos.
                                if (nuevosDepartamentos != null)
                                {
                                    foreach (int departamentoID in nuevosDepartamentos)
                                    {
                                        var fac = context.tblFA_PlantillatblFA_Grupos.FirstOrDefault(x => x.plantillaID == plantilla.id && x.grupoID == departamentoID);
                                        if (fac == null)
                                        {
                                            plantillaDepartamento = new tblFA_PlantillatblFA_Grupos();
                                            plantillaDepartamento.plantillaID = plantilla.id;
                                            plantillaDepartamento.grupoID = departamentoID;
                                            context.tblFA_PlantillatblFA_Grupos.Add(plantillaDepartamento);
                                            context.SaveChanges();
                                        }
                                    }
                                }
                            }
                            catch (Exception e)
                            {
                                dbContextTransaction.Rollback();
                                resultado.Add(SUCCESS, false);
                                resultado.Add(ERROR, "Ocurrió un error interno al intentar actualizar la lista de departamentos de la plantilla");
                                logErrorFacultamientos(MODULO_CATALOGO_ID, "ActualizarPlantilla", e, AccionEnum.ACTUALIZAR, plantillaID, plantillaDepartamento);
                                return resultado;
                            }
                            #endregion

                            #region crear nueva versión de paquetes si ya estaba autorizado
                            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                            // Se buscan a todos los paquetes existentes que sean de un CC perteneciente a un departamento de los de la nueva plantilla
                            // y se les agrega registros vacíos de la nueva plantilla.
                            // Si algun paquete ya estaba en autorizado, se creará otra versión nueva con la nueva plantilla agregada.
                            try
                            {
                                var listaDepartamentos = context.tblFA_PlantillatblFA_Grupos.Where(x => x.plantillaID == plantillaID).Select(x => x.grupoID).ToList();
                                var listaPaquetes = new List<tblFA_Paquete>();
                                listaDepartamentos.ForEach(departamentoID =>
                                {
                                    List<tblFA_Paquete> paquete = new List<tblFA_Paquete>();

                                    if (vSesiones.sesionEmpresaActual == (int)EmpresaEnum.Construplan || vSesiones.sesionEmpresaActual == (int)EmpresaEnum.GCPLAN)
                                    {
                                        paquete = context.tblFA_Paquete.Where(y => y.esActivo != false && y.cc.grupoID == departamentoID).ToList();
                                    }
                                    else
                                    {
                                        var listaRelacionCatalogoCCGrupo = context.tblC_Nom_CatalogoCCtblFA_Grupos.Where(x => x.registroActivo && x.grupo_id == departamentoID).Select(x => x.catalogoCC_id).ToList();

                                        paquete = context.tblFA_Paquete.Where(y => y.esActivo != false && listaRelacionCatalogoCCGrupo.Contains(y.ccID)).ToList();
                                    }

                                    listaPaquetes.AddRange(paquete);
                                });
                                var facTemp = context.tblFA_Facultamiento.Where(x => x.plantillaID == plantillaID);
                                foreach (var paquete in listaPaquetes)
                                {
                                    // Si el estado del paquete está en como null, significa que está editando o en proceso de autorización.
                                    // Se vuelve a poner en editando y se le agregan los nuevos conceptos.
                                    if (paquete.esActivo == null)
                                    {
                                        paquete.estado = (int)EstadoPaqueteFaEnum.Editando;

                                        // Se crea el nuevo facultamiento con los nuevos registros.
                                        var nuevoFacultamiento = new tblFA_Facultamiento();
                                        nuevoFacultamiento.paqueteID = paquete.id;
                                        nuevoFacultamiento.plantillaID = plantilla.id;
                                        nuevoFacultamiento.aplica = false;
                                        context.tblFA_Facultamiento.Add(nuevoFacultamiento);
                                        context.SaveChanges();

                                        // Por cada nuevo concepto en la plantilla, se agrega un nuevo registro de empleado a cada paquete.
                                        //var fac = facTemp.FirstOrDefault(x => x.plantillaID == plantillaID && x.paqueteID == paquete.id);
                                        foreach (var y in nuevosConceptos.Where(x => x.PlantillaID == 0).ToList())
                                        {

                                            var empleado = new tblFA_Empleado
                                            {
                                                conceptoID = y.ID,
                                                editado = false,
                                                esActivo = true,
                                                facultamientoID = nuevoFacultamiento.id,
                                                aplica = false,
                                                nombreEmpleado = null,
                                                claveEmpleado = null
                                            };
                                            context.tblFA_Empleado.Add(empleado);
                                            context.SaveChanges();
                                        }
                                        int cantidadNuevosConceptos = nuevosConceptos.Where(y => y.PlantillaID < 1).ToList().Count;
                                        if (cantidadNuevosConceptos > 0)
                                        {
                                            paquete.comentario = String.Format("Se agregaron conceptos al facultamiento de {0}.", plantilla.titulo);
                                        }
                                        else
                                        {
                                            paquete.comentario = String.Format("Se modificaron conceptos en el facultamiento de {0}.", plantilla.titulo);
                                        }

                                    }

                                    // Si el estado del paquete está en activo, significa que ya está autorizado.
                                    // Se procede a crear otro versión del paquete con los nuevos conceptos agregados.
                                    else
                                    {
                                        bool esPendiente = true;
                                        var listaPaquetesPendientes = listaPaquetes
                                            .Where(y =>
                                                y.ccID.Equals(paquete.ccID) &&
                                                y.id != paquete.id &&
                                                y.facultamientos.Any(z => z.plantillaID.Equals(plantillaID)))
                                            .ToList();
                                        listaPaquetesPendientes.ForEach(paquetePendiente =>
                                        {
                                            if (paquetePendiente.esActivo == null)
                                            {
                                                esPendiente = false;
                                            }
                                        });
                                        // Si el paquete ya está autorizado y no hay otra versión del mismo en editando o pendiente de autorizar, se crea otra versión y 
                                        // si el paquete tiene al facultamiento como aplica de la plantilla editada, se crea otra versión.
                                        if ((paquete.estado.Equals((int)EstadoPaqueteFaEnum.Autorizado)) &&
                                            (paquete.esActivo ?? false) &&
                                            esPendiente &&
                                            paquete.facultamientos.Any(y => y.plantillaID == plantillaID && y.aplica))
                                        {
                                            // Se crea la nueva versión del paquete con los nuevos conceptos y se deja como editando.
                                            var nuevoPaquete = new tblFA_Paquete();
                                            nuevoPaquete.ccID = paquete.ccID;
                                            nuevoPaquete.estado = (int)EstadoPaqueteFaEnum.Editando;
                                            nuevoPaquete.fechaCreacion = DateTime.Now;
                                            nuevoPaquete.esActivo = null;
                                            nuevoPaquete.usuarioCreadorID = vSesiones.sesionUsuarioDTO.id;
                                            nuevoPaquete.comentario = String.Format("Se modificó el facultamiento de {0}.", plantilla.titulo);
                                            context.tblFA_Paquete.Add(nuevoPaquete);
                                            context.SaveChanges();

                                            var listaFacultamientos = context.tblFA_Facultamiento.Where(y => y.paqueteID.Equals(paquete.id)).ToList();

                                            // Guardado de la lista de facultamientos.
                                            foreach (var facultamiento in listaFacultamientos)
                                            {
                                                var nuevoFacultamiento = new tblFA_Facultamiento();
                                                nuevoFacultamiento.paqueteID = nuevoPaquete.id;
                                                nuevoFacultamiento.plantillaID = facultamiento.plantillaID;
                                                nuevoFacultamiento.aplica = facultamiento.aplica;
                                                context.tblFA_Facultamiento.Add(nuevoFacultamiento);
                                                context.SaveChanges();

                                                // Se procede a guardar la lista de empleados de cada facultamiento.
                                                var listaEmpleados = context.tblFA_Empleado.Where(y => y.facultamientoID.Equals(facultamiento.id)).ToList();

                                                listaEmpleados.ForEach(x =>
                                                {
                                                    var empleado = new tblFA_Empleado();
                                                    empleado.conceptoID = x.conceptoID;
                                                    empleado.editado = false;
                                                    empleado.esActivo = true;
                                                    empleado.facultamientoID = nuevoFacultamiento.id;
                                                    empleado.aplica = x.aplica;
                                                    if (x.claveEmpleado != null)
                                                    {
                                                        empleado.nombreEmpleado = x.nombreEmpleado.Trim();
                                                        empleado.claveEmpleado = x.claveEmpleado;
                                                    }
                                                    else
                                                    {
                                                        empleado.nombreEmpleado = null;
                                                        empleado.claveEmpleado = null;
                                                    }
                                                    context.tblFA_Empleado.Add(empleado);
                                                });

                                                // Si el facultamiento pertence a la misma plantilla que se editó, se agregan los nuevos conceptos.
                                                if (facultamiento.plantillaID == plantillaID)
                                                {
                                                    // Se agregan los nuevos empleados(conceptos) de la plantilla.
                                                    nuevosConceptos.Where(y => y.PlantillaID < 1).ToList().ForEach(y =>
                                                    {
                                                        var empleado = new tblFA_Empleado
                                                        {
                                                            conceptoID = y.ID,
                                                            editado = false,
                                                            esActivo = true,
                                                            facultamientoID = nuevoFacultamiento.id,
                                                            aplica = false,
                                                            nombreEmpleado = null,
                                                            claveEmpleado = null
                                                        };
                                                        context.tblFA_Empleado.Add(empleado);
                                                    });
                                                }
                                            }

                                            // Se agregan los autorizantes del paquete anterior.
                                            int contadorOrden2 = 0;
                                            paquete.autorizantes.ForEach(y =>
                                            {
                                                var autorizante = new tblFA_Autorizante();
                                                autorizante.esAutorizante = (contadorOrden2 == 0);
                                                autorizante.autorizado = null;
                                                autorizante.firma = null;
                                                autorizante.orden = ++contadorOrden2;
                                                autorizante.paqueteID = nuevoPaquete.id;
                                                autorizante.usuarioID = (y.usuarioID != null) ? y.usuarioID : null;
                                                context.tblFA_Autorizante.Add(autorizante);
                                            });

                                            context.SaveChanges();
                                        }
                                    }
                                }
                            }

                            catch (Exception e)
                            {
                                dbContextTransaction.Rollback();
                                resultado.Add(SUCCESS, false);
                                resultado.Add(ERROR, "Ocurrió un error interno al intentar actualizar los facultamientos ya existentes.");
                                logErrorFacultamientos(MODULO_CATALOGO_ID, "ActualizarPlantilla", e, AccionEnum.ACTUALIZAR, 0, plantillaID);
                                return resultado;
                            }
                            #endregion
                        }

                        context.SaveChanges();
                        dbContextTransaction.Commit();
                        if (new MenuDAO().isLiberado(vSesiones.sesionCurrentView))
                        {
                            //SaveBitacora((int)BitacoraEnum.PlantillaFacultamiento, (int)AccionEnum.ACTUALIZAR, plantilla.id, JsonUtils.convertNetObjectToJson(plantilla));
                        }
                        resultado.Add(SUCCESS, true);
                    }
                    catch (Exception e)
                    {
                        dbContextTransaction.Rollback();
                        resultado.Add(SUCCESS, false);
                        resultado.Add(ERROR, "Ocurrió un error interno al intentar actualizar la plantilla");
                        logErrorFacultamientos(MODULO_CATALOGO_ID, "ActualizarPlantilla", e, AccionEnum.ACTUALIZAR, plantillaID, plantilla);
                        return resultado;
                    }
                }
            }

            return resultado;
        }
        #endregion

        #region metodos asignacion
        public Dictionary<string, object> ObtenerPaquetes(int departamentoID, int centroCostosID, int estado)
        {
            var resultado = new Dictionary<string, object>();
            var listaPaquetesFa = new List<CatalogoPaquetesDTO>();

            if (vSesiones.sesionEmpresaActual == (int)EmpresaEnum.Construplan || vSesiones.sesionEmpresaActual == (int)EmpresaEnum.GCPLAN)
            {
                listaPaquetesFa = _context.tblFA_Paquete.Where(x =>
                    ((estado.Equals(0)) ? x.estado != 0 : x.estado.Equals(estado)) &&
                    ((centroCostosID.Equals(0)) ? x.ccID != 0 : x.ccID.Equals(centroCostosID)) &&
                    ((departamentoID.Equals(0)) ? x.cc.grupoID != null : x.cc.grupoID == departamentoID)).ToList().Select(x => new CatalogoPaquetesDTO
                {
                    ID = x.id,
                    CentroCostos = x.cc.cc,
                    ccID = x.ccID,
                    Descripcion = x.cc.descripcion,
                    Fecha = (x.fechaModificacion != null) ? String.Format("{0:dd-MM-yyyy}", x.fechaModificacion) : x.fechaCreacion.ToString("dd-MM-yyyy"),
                    Estatus = EnumExtensions.GetDescription((EstadoPaqueteFaEnum)(Int32.Parse(x.estado.ToString()))),
                    Departamento = x.cc.grupo.descripcion.Trim()
                }).OrderByDescending(x => x.ID).ToList();
            }
            else
            {
                var listaCatalogoCC = _context.tblC_Nom_CatalogoCC.ToList();

                listaPaquetesFa = _context.tblFA_Paquete.Where(x =>
                    ((estado.Equals(0)) ? x.estado != 0 : x.estado.Equals(estado)) &&
                    ((centroCostosID.Equals(0)) ? x.ccID != 0 : x.ccID.Equals(centroCostosID))
                    //((departamentoID.Equals(0)) ? x.cc.grupoID != null : x.cc.grupoID == departamentoID)
                ).ToList().Select(x =>
                {
                    return new CatalogoPaquetesDTO
                    {
                        ID = x.id,
                        CentroCostos = listaCatalogoCC.Where(y => y.id == x.ccID).Select(z => z.cc).FirstOrDefault(),
                        ccID = x.ccID,
                        Descripcion = listaCatalogoCC.Where(y => y.id == x.ccID).Select(z => z.ccDescripcion).FirstOrDefault(),
                        Fecha = (x.fechaModificacion != null) ? String.Format("{0:dd-MM-yyyy}", x.fechaModificacion) : x.fechaCreacion.ToString("dd-MM-yyyy"),
                        Estatus = EnumExtensions.GetDescription((EstadoPaqueteFaEnum)(Int32.Parse(x.estado.ToString()))),
                        Departamento = "",
                    };
                }).OrderByDescending(x => x.ID).ToList();
            }

            // Se verifica cuantas versiones hay de cada paquete para ver si se podrá actualizar o no.
            listaPaquetesFa.ForEach(x =>
            {
                List<tblFA_Paquete> cantidadVersiones = _context.tblFA_Paquete.Where(y => y.ccID.Equals(x.ccID)).OrderBy(y => y.id).ToList();

                // Si sólo hay una versión, es editable.
                if (cantidadVersiones != null && cantidadVersiones.Count == 1)
                {
                    x.Editable = true;
                }

                // Si hay dos versiones o más, se pone editable a la más reciente.
                else if (cantidadVersiones.Count >= 2)
                {
                    int recienteID = cantidadVersiones.Where(y => y.ccID.Equals(x.ccID)).OrderBy(y => (y.id)).ToList().LastOrDefault().id;

                    if (x.ID.Equals(recienteID))
                    {
                        x.Editable = true;
                    }
                }

            });

            if (listaPaquetesFa.Count > 0)
            {
                resultado.Add(SUCCESS, true);
                resultado.Add("listaPaquetesFa", listaPaquetesFa.OrderBy(x => x.CentroCostos));
            }
            else
            {
                resultado.Add(SUCCESS, false);
                resultado.Add("EMPTY", true);
            }

            return resultado;
        }

        public List<ComboDTO> ObtenerCentrosCostos()
        {
            var listaCC = new List<ComboDTO>();
            try
            {
                if (vSesiones.sesionEmpresaActual == (int)EmpresaEnum.Construplan || vSesiones.sesionEmpresaActual == (int)EmpresaEnum.GCPLAN)
                {
                    listaCC = _context.tblP_CC.Where(x =>
                        (x.grupoID != null) &&
                        (x.grupo.plantillasFaDepartamento.Where(y => y.grupoID == (int)x.grupoID).ToList().Count > 0) &&
                        (x.paquetes.FirstOrDefault(z => z.ccID == x.id) == null)
                    ).Select(x => new ComboDTO
                    {
                        Value = x.id.ToString(),
                        Text = x.cc + " - " + x.descripcion.Trim() + " (" + x.grupo.descripcion + ")",
                        Prefijo = ""
                    }).OrderBy(x => x.Text).ToList();
                }
                else
                {
                    listaCC = _context.tblC_Nom_CatalogoCC.Where(x => x.estatus).Select(x => new ComboDTO
                    {
                        Value = x.id.ToString(),
                        Text = x.cc + " - " + x.ccDescripcion.Trim(),
                        Prefijo = ""
                    }).OrderBy(x => x.Text).ToList();
                }

            }
            catch (Exception e)
            {
                logErrorFacultamientos(MODULO_ASIGNACION_ID, "ObtenerCentrosCostos", e, AccionEnum.CONSULTA, 0, null);
                return new List<ComboDTO>();
            }

            return listaCC;
        }

        public List<ComboDTO> ObtenerObras()
        {
            var listaObras = new List<ComboDTO>();
            try
            {
                if (vSesiones.sesionEmpresaActual == (int)EmpresaEnum.Construplan || vSesiones.sesionEmpresaActual == (int)EmpresaEnum.GCPLAN)
                {
                    listaObras = _context.tblP_CC.Where(x => x.paquetes.FirstOrDefault(y => y.ccID.Equals(x.id)) != null).Select(x => new ComboDTO
                    {
                        Value = x.id.ToString(),
                        Text = x.cc + " - " + x.descripcion.Trim() + " (" + x.grupo.descripcion + ")",
                        Prefijo = ""
                    }).OrderBy(x => x.Text).ToList();
                }
                else
                {
                    listaObras = _context.tblC_Nom_CatalogoCC.Where(x => x.estatus).Select(x => new ComboDTO
                    {
                        Value = x.id.ToString(),
                        Text = x.cc + " - " + x.ccDescripcion.Trim(),
                        Prefijo = ""
                    }).OrderBy(x => x.Text).ToList();
                }
            }
            catch (Exception e)
            {
                logErrorFacultamientos(MODULO_ASIGNACION_ID, "ObtenerObras", e, AccionEnum.CONSULTA, 0, null);
                return new List<ComboDTO>();
            }

            return listaObras;
        }

        public Dictionary<string, object> CargarPlantillasCC(int centroCostosID)
        {
            var resultado = new Dictionary<string, object>();
            var listaPlantillas = new List<PlantillaFacultamientoDTO>();
            try
            {
                int? departamentoID = null;

                if (vSesiones.sesionEmpresaActual == (int)EmpresaEnum.Construplan || vSesiones.sesionEmpresaActual == (int)EmpresaEnum.GCPLAN)
                {
                    departamentoID = _context.tblP_CC.Where(x => x.id.Equals(centroCostosID)).Select(x => x.grupoID).FirstOrDefault();
                }
                else
                {
                    var relacionCatalogoCCGrupo = _context.tblC_Nom_CatalogoCCtblFA_Grupos.FirstOrDefault(x => x.registroActivo && x.catalogoCC_id == centroCostosID);

                    if (relacionCatalogoCCGrupo != null)
                    {
                        departamentoID = relacionCatalogoCCGrupo.grupo_id;
                    }
                }

                if (departamentoID != null)
                {
                    List<int> listaPlantillasID = _context.tblFA_PlantillatblFA_Grupos.Where(x => x.grupoID == departamentoID && x.plantilla.esActiva).OrderBy(x => x.plantilla.orden).Select(x => x.plantillaID).ToList();
                    if (listaPlantillasID.Count > 0)
                    {
                        foreach (var plantillaID in listaPlantillasID)
                        {
                            var plantillaDTO = new PlantillaFacultamientoDTO();
                            var plantilla = _context.tblFA_Plantilla.FirstOrDefault(x => x.id.Equals(plantillaID));
                            plantillaDTO.orden = plantilla.orden;
                            plantillaDTO.Titulo = plantilla.titulo.Trim();
                            plantillaDTO.PlantillaID = plantilla.id;
                            plantillaDTO.ListaConceptos = _context.tblFA_ConceptoPlantilla.Where(x => x.plantillaID.Equals(plantillaID) && x.esActivo).OrderBy(x => x.orden)
                                .Select(x => new ConceptoDTO
                                {
                                    ID = x.id,
                                    Concepto = x.concepto,
                                    EsAutorizante = x.esAutorizacion,
                                    PlantillaID = x.plantillaID
                                })
                                .ToList();
                            listaPlantillas.Add(plantillaDTO);
                        }
                        resultado.Add(SUCCESS, true);
                        resultado.Add("listaPlantillas", listaPlantillas.OrderBy(x => x.orden));
                    }
                    else
                    {
                        resultado.Add(SUCCESS, false);
                        resultado.Add(ERROR, "No hay plantillas asigandas para este Centro de Costos.");
                        return resultado;
                    }
                }
                else
                {
                    resultado.Add(SUCCESS, false);
                    resultado.Add(ERROR, "No hay departamento asigando para este Centro de Costos.");
                    return resultado;
                }
            }
            catch (Exception e)
            {
                resultado.Add(SUCCESS, false);
                resultado.Add(ERROR, "Ocurrió un error interno al intentar cargar las plantillas de este Centro de Costos.");
                logErrorFacultamientos(MODULO_ASIGNACION_ID, "CargarPlantillasCC", e, AccionEnum.CONSULTA, centroCostosID, listaPlantillas);
                return resultado;
            }

            return resultado;
        }

        public Dictionary<string, object> AsignarFacultamientos(int centroCostosID, List<FacultamientoDTO> listaFacultamientos, List<EmpleadoAutorizanteDTO> listaAutorizantes, bool todoCompleto)
        {
            var resultado = new Dictionary<string, object>();
            var paqueteFacultamientos = new tblFA_Paquete();
            using (var context = new MainContext())
            {
                using (DbContextTransaction dbContextTransaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        try
                        {
                            // Se guarda el paquete de facultamientos.
                            paqueteFacultamientos.ccID = centroCostosID;
                            paqueteFacultamientos.estado = (todoCompleto) ? (int)EstadoPaqueteFaEnum.PendienteAutorizacion : (int)EstadoPaqueteFaEnum.Editando;
                            paqueteFacultamientos.fechaCreacion = DateTime.Now;
                            paqueteFacultamientos.esActivo = null;
                            paqueteFacultamientos.usuarioCreadorID = vSesiones.sesionUsuarioDTO.id;
                            context.tblFA_Paquete.Add(paqueteFacultamientos);
                            context.SaveChanges();
                        }
                        catch (Exception e)
                        {
                            dbContextTransaction.Rollback();
                            resultado.Add(SUCCESS, false);
                            resultado.Add(ERROR, "Ocurrió un error interno al intentar asignar el paquete de facultamientos");
                            logErrorFacultamientos(MODULO_ASIGNACION_ID, "AsignarFacultamientos", e, AccionEnum.AGREGAR, 0, null);
                            return resultado;
                        }

                        try
                        {
                            // Guardado de la lista de facultamientos.
                            foreach (var facultamiento in listaFacultamientos)
                            {
                                var nuevoFacultamiento = new tblFA_Facultamiento();
                                nuevoFacultamiento.paqueteID = paqueteFacultamientos.id;
                                nuevoFacultamiento.plantillaID = facultamiento.PlantillaID;
                                nuevoFacultamiento.aplica = facultamiento.Aplica;


                                context.tblFA_Facultamiento.Add(nuevoFacultamiento);
                                context.SaveChanges();

                                // Se procede a guardar la lista de empleados de cada facultamiento.
                                foreach (var x in facultamiento.ListaEmpleados)
                                {
                                    var empleado = new tblFA_Empleado();
                                    empleado.conceptoID = x.ConceptoID;
                                    empleado.editado = false;
                                    empleado.esActivo = true;
                                    empleado.facultamientoID = nuevoFacultamiento.id;
                                    empleado.aplica = x.Aplica;
                                    if (x.ClaveEmpleado != null)
                                    {
                                        empleado.nombreEmpleado = x.NombreEmpleado.Trim();
                                        empleado.claveEmpleado = x.ClaveEmpleado;
                                    }
                                    else
                                    {
                                        empleado.nombreEmpleado = null;
                                        empleado.claveEmpleado = null;
                                    }
                                    context.tblFA_Empleado.Add(empleado);
                                }
                            }
                        }
                        catch (Exception e)
                        {
                            dbContextTransaction.Rollback();
                            resultado.Add(SUCCESS, false);
                            resultado.Add(ERROR, "Ocurrió un error interno al intentar guardar la lista de facultamientos del paquete de facultamientos");
                            logErrorFacultamientos(MODULO_ASIGNACION_ID, "AsignarFacultamientos", e, AccionEnum.AGREGAR, paqueteFacultamientos.id, null);
                            return resultado;
                        }

                        try
                        {
                            // Guardado de la lista de autorizantes.
                            int contadorOrden = 0;
                            foreach (var empleadoAutorizante in listaAutorizantes)
                            {
                                var autorizante = new tblFA_Autorizante();
                                autorizante.esAutorizante = (contadorOrden.Equals(0));
                                autorizante.autorizado = null;
                                autorizante.firma = null;
                                autorizante.orden = ++contadorOrden;
                                autorizante.paqueteID = paqueteFacultamientos.id;
                                autorizante.usuarioID = (empleadoAutorizante.ClaveEmpleado != 0) ? empleadoAutorizante.ClaveEmpleado : null;
                                context.tblFA_Autorizante.Add(autorizante);
                                empleadoAutorizante.Orden = contadorOrden;
                            }
                        }
                        catch (Exception e)
                        {
                            dbContextTransaction.Rollback();
                            resultado.Add(SUCCESS, false);
                            resultado.Add(ERROR, "Ocurrió un error interno al intentar guardar la lista de autorizantes del paquete de facultamientos");
                            logErrorFacultamientos(MODULO_ASIGNACION_ID, "AsignarFacultamientos", e, AccionEnum.AGREGAR, 0, null);
                            return resultado;
                        }

                        // Si se llenaron todos los campos, el paquete como PendienteAutorizacion y envía la primer alerta.
                        if (paqueteFacultamientos.estado == (int)EstadoPaqueteFaEnum.PendienteAutorizacion)
                        {
                            string cc = "";

                            if (vSesiones.sesionEmpresaActual == (int)EmpresaEnum.Construplan || vSesiones.sesionEmpresaActual == (int)EmpresaEnum.GCPLAN)
                            {
                                cc = context.tblP_CC.FirstOrDefault(x => x.id.Equals(centroCostosID)).cc;
                            }
                            else
                            {
                                cc = context.tblC_Nom_CatalogoCC.FirstOrDefault(x => x.id.Equals(centroCostosID)).cc;
                            }

                            EmpleadoAutorizanteDTO empleadoRecibe = listaAutorizantes.FirstOrDefault(x => x.Orden.Equals(2));
                            if (empleadoRecibe.NombreEmpleado == null)
                            {
                                empleadoRecibe = listaAutorizantes.FirstOrDefault();
                            }

                            resultado.Add("ordenVoBo", empleadoRecibe.Orden);
                            resultado.Add("paqueteID", paqueteFacultamientos.id);

                            // Enviado de alerta al primer autorizante.
                            try
                            {
                                crearNotifacionFacultamiento(
                                    "Autorización de Facultamiento: " + cc,
                                    empleadoRecibe.ClaveEmpleado ?? 0,
                                    paqueteFacultamientos.id,
                                    context);
                            }
                            catch (Exception e)
                            {
                                dbContextTransaction.Rollback();
                                resultado.Add(SUCCESS, false);
                                resultado.Add(ERROR, "Ocurrió un error interno al intentar enviar una notificacicón al primer autorizante del paquete de facultamientos");
                                logErrorFacultamientos(MODULO_ASIGNACION_ID, "AsignarFacultamientos", e, AccionEnum.AGREGAR, 0, null);
                                return resultado;
                            }
                        }

                        context.SaveChanges();
                        dbContextTransaction.Commit();

                        //if (new MenuDAO().isLiberado(vSesiones.sesionCurrentView))
                        //{
                        //    string objeto;
                        //    try
                        //    {
                        //        objeto = JsonUtils.convertNetObjectToJson(paqueteFacultamientos);
                        //    }
                        //    catch (Exception)
                        //    {
                        //        objeto = "";
                        //    }
                        //    SaveBitacora((int)BitacoraEnum.PaqueteFacultamientos, (int)AccionEnum.AGREGAR, paqueteFacultamientos.id, objeto);
                        //}
                        resultado.Add(SUCCESS, true);
                    }
                    catch (Exception e)
                    {
                        dbContextTransaction.Rollback();
                        resultado.Add(SUCCESS, false);
                        resultado.Add(ERROR, "Ocurrió un error interno al intentar guardar la el paquete de facultamientos");
                        logErrorFacultamientos(MODULO_ASIGNACION_ID, "AsignarFacultamientos", e, AccionEnum.AGREGAR, 0, null);
                        return resultado;
                    }
                }
            }

            return resultado;
        }

        public Dictionary<string, object> ObtenerPaqueteActualizar(int paqueteID, bool esReporte = false)
        {
            var resultado = new Dictionary<string, object>();
            var paqueteFacultamientos = new PaqueteFaDTO();
            try
            {
                // Se verifica si el paquete de facultamientos tiene algún comentario.
                var paquete = _context.tblFA_Paquete.FirstOrDefault(x => x.id.Equals(paqueteID));
                if (paquete != null && paquete.estado.Equals((int)EstadoPaqueteFaEnum.Editando) && !String.IsNullOrEmpty(paquete.comentario))
                {
                    paqueteFacultamientos.Comentario = paquete.comentario.Trim();
                    paqueteFacultamientos.Estado = EnumExtensions.GetDescription((EstadoPaqueteFaEnum)paquete.estado);
                }

                if (esReporte)
                {
                    if (vSesiones.sesionEmpresaActual == (int)EmpresaEnum.Construplan || vSesiones.sesionEmpresaActual == (int)EmpresaEnum.GCPLAN)
                    {
                        // Se agregan campos necesarios para reporte.            
                        paqueteFacultamientos.Fecha = (paquete.fechaModificacion != null) ? paquete.fechaModificacion.ToString() : paquete.fechaCreacion.ToString();
                        paqueteFacultamientos.Obra = paquete.cc.descripcion;
                        paqueteFacultamientos.CentroCostos = paquete.cc.cc;
                        paqueteFacultamientos.Departamento = paquete.cc.departamento.descripcion;
                        paqueteFacultamientos.EsActivo = paquete.esActivo;
                    }
                    else
                    {
                        var centroCostoCatalogo = _context.tblC_Nom_CatalogoCC.FirstOrDefault(x => x.id == paquete.ccID);

                        // Se agregan campos necesarios para reporte.            
                        paqueteFacultamientos.Fecha = (paquete.fechaModificacion != null) ? paquete.fechaModificacion.ToString() : paquete.fechaCreacion.ToString();
                        paqueteFacultamientos.Obra = centroCostoCatalogo != null ? centroCostoCatalogo.ccDescripcion : "";
                        paqueteFacultamientos.CentroCostos = centroCostoCatalogo != null ? centroCostoCatalogo.cc : "";
                        paqueteFacultamientos.Departamento = "";
                        paqueteFacultamientos.EsActivo = paquete.esActivo;
                    }
                }

                // Se agrega la lista de facultamientos al paquete.
                paqueteFacultamientos.listaFacultamientos = new List<FacultamientoDTO>();

                foreach (var facultamiento in paquete.facultamientos.Where(w => w.plantillaID > 0).OrderBy(x => x.plantilla.orden))
                {
                    var facultamientoDTO = new FacultamientoDTO();
                    facultamientoDTO.orden = facultamiento.plantilla.orden;
                    facultamientoDTO.Titulo = facultamiento.plantilla.titulo;
                    facultamientoDTO.FacultamientoID = facultamiento.id;
                    facultamientoDTO.Aplica = facultamiento.aplica;
                    facultamientoDTO.PlantillaID = facultamiento.plantillaID;

                    // Se agrega la lista de empleados al facultamiento.
                    facultamientoDTO.ListaEmpleados = _context.tblFA_Empleado
                        .Where(x => x.facultamientoID.Equals(facultamiento.id) && x.esActivo)
                        .OrderBy(x => x.concepto.orden)
                        .Select(x => new EmpleadoFaDTO
                        {
                            EmpleadoID = x.id,
                            NombreEmpleado = x.nombreEmpleado.Trim(),
                            ClaveEmpleado = x.claveEmpleado,
                            ConceptoID = x.conceptoID,
                            Editado = x.editado,
                            Concepto = x.concepto.concepto.Trim(),
                            EsAutorizante = x.concepto.esAutorizacion,
                            Aplica = x.aplica
                        })
                        .ToList();

                    paqueteFacultamientos.listaFacultamientos.Add(facultamientoDTO);
                }

                // Se agrega la lista de autorizantes al paquete de facultamientos.
                paqueteFacultamientos.ListaAutorizantes = new List<AutorizanteFaDTO>();
                paqueteFacultamientos.ListaAutorizantes = _context.tblFA_Autorizante
                    .ToList()
                    .Where(x => x.paqueteID.Equals(paqueteID))
                    .Select(x => new AutorizanteFaDTO
                    {
                        AutorizanteID = x.id,
                        Nombre = (x.usuarioID != null || x.usuarioID > 0) ? (x.usuario.nombre + " " + x.usuario.apellidoPaterno + "" + x.usuario.apellidoMaterno) : String.Empty,
                        UsuarioID = x.usuarioID,
                        EsAutorizante = x.esAutorizante,
                        Autorizado = x.autorizado,
                        Orden = x.orden,
                        Firma = x.firma
                    })
                    .OrderBy(x => x.Orden)
                    .ToList();

                resultado.Add(SUCCESS, true);
                resultado.Add("paqueteFacultamientos", paqueteFacultamientos);

                #region SANDBOX PARA DETECTAR QUE PLANTILLAS - PAQUETE, NO CONTIENE CONCEPTOS REGISTRADOS
                /*string lstSinEmpleados = string.Empty;
                string lstConEmpleados = string.Empty;

                string strQuery = string.Empty;
                bool seActualizo = false;
                foreach (var item in paquete.facultamientos.Where(w => w.plantillaID > 0).OrderBy(o => o.paqueteID).ToList())
                {
                    int plantillaID = item.plantillaID;
                    tblFA_Empleado objEmpleado = _context.tblFA_Empleado.Where(w => w.facultamientoID == item.id).FirstOrDefault();

                    if (objEmpleado == null)
                    {
                        seActualizo = true;
                        lstSinEmpleados += string.Format("{0},", item.id);
                        strQuery += string.Format("UPDATE tblFA_Facultamiento SET plantillaID = -{0} WHERE id = {1}", item.plantillaID, item.id);
                        tblFA_Facultamiento objActualizar = _context.tblFA_Facultamiento.Where(w => w.id == item.id).FirstOrDefault();
                        if (objActualizar != null)
                        {
                            string plantillaNegativa = "-" + item.plantillaID;
                            objActualizar.plantillaID = Convert.ToInt32(plantillaNegativa);
                            _context.SaveChanges();
                        }
                    }
                    else if (objEmpleado != null)
                        lstConEmpleados += string.Format("{0},", item.id);
                }

                if (seActualizo)
                {
                    string stop = string.Empty;
                }*/
                #endregion
            }
            catch (Exception e)
            {
                resultado.Add(SUCCESS, false);
                resultado.Add(ERROR, "Ocurrió un error interno al intentar cargar los facultamientos de la obra seleccionada.");
                logErrorFacultamientos(MODULO_ASIGNACION_ID, "ObtenerPaqueteActualizar", e, AccionEnum.CONSULTA, paqueteID, null);
                return resultado;
            }

            return resultado;
        }

        public Dictionary<string, object> ActualizarFacultamientos(int paqueteID, List<FacultamientoDTO> listaFacultamientos, List<EmpleadoAutorizanteDTO> listaAutorizantes, bool todoCompleto)
        {
            var resultado = new Dictionary<string, object>();
            var paquete = new tblFA_Paquete();
            using (var context = new MainContext())
            {
                using (DbContextTransaction dbContextTransaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        try
                        {
                            paquete = context.tblFA_Paquete.FirstOrDefault(x => x.id.Equals(paqueteID));

                            // Si el paquete ya está autorizado, se crea otra versión.
                            if ((paquete.estado.Equals((int)EstadoPaqueteFaEnum.Autorizado)) && (paquete.esActivo ?? false))
                            {
                                return AsignarFacultamientos(paquete.ccID, listaFacultamientos, listaAutorizantes, todoCompleto);
                            }

                            // Se actualiza el paquete de facultamientos.
                            paquete.estado = (todoCompleto) ? (int)EstadoPaqueteFaEnum.PendienteAutorizacion : (int)EstadoPaqueteFaEnum.Editando;
                            paquete.fechaModificacion = DateTime.Now;
                            paquete.usuarioCreadorID = vSesiones.sesionUsuarioDTO.id;

                            // Si el paquete de facultamientos tiene algún comentario, este se quitará.
                            if (!String.IsNullOrEmpty(paquete.comentario))
                            {
                                paquete.comentario = null;
                            }

                        }
                        catch (Exception e)
                        {
                            dbContextTransaction.Rollback();
                            resultado.Add(SUCCESS, false);
                            resultado.Add(ERROR, "Ocurrió un error interno al intentar actualizar el paquete de facultamientos");
                            logErrorFacultamientos(MODULO_ASIGNACION_ID, "ActualizarFacultamientos", e, AccionEnum.ACTUALIZAR, paqueteID, paquete);
                            return resultado;
                        }

                        try
                        {
                            // Se actualizan los facultamientos.
                            listaFacultamientos.ForEach(x =>
                            {
                                var facultamiento = context.tblFA_Facultamiento.FirstOrDefault(y => y.id.Equals(x.FacultamientoID));
                                facultamiento.aplica = x.Aplica;

                                // Se actualiza la lista de empleados de cada facultamiento.
                                if (x.ListaEmpleados == null || x.ListaEmpleados.Count == 0)
                                {
                                    var empleadosPorActualizar = context.tblFA_Empleado.Where(w => w.esActivo && w.facultamientoID == x.FacultamientoID && w.facultamiento.plantillaID == x.PlantillaID);
                                    foreach (var item in empleadosPorActualizar)
                                    {
                                        item.esActivo = false;
                                    }
                                }
                                else
                                {
                                    x.ListaEmpleados.ForEach(y =>
                                    {
                                        var empleadoPorActualizar = context.tblFA_Empleado.FirstOrDefault(z => z.id.Equals(y.EmpleadoID) && z.esActivo);
                                        if (empleadoPorActualizar != null)
                                        {
                                            if (!x.Aplica)
                                            {
                                                empleadoPorActualizar.aplica = false;
                                                empleadoPorActualizar.nombreEmpleado = null;
                                                empleadoPorActualizar.claveEmpleado = null;
                                            }
                                            else
                                            {
                                                empleadoPorActualizar.aplica = y.Aplica;
                                                if (y.ClaveEmpleado != empleadoPorActualizar.claveEmpleado)
                                                {
                                                    empleadoPorActualizar.nombreEmpleado = y.NombreEmpleado;
                                                    empleadoPorActualizar.claveEmpleado = y.ClaveEmpleado;
                                                }
                                            }
                                        }
                                    });
                                }
                            });
                        }
                        catch (Exception e)
                        {
                            dbContextTransaction.Rollback();
                            resultado.Add(SUCCESS, false);
                            resultado.Add(ERROR, "Ocurrió un error interno al intentar actualizar la lista de facultamientos del paquete de facultamientos");
                            logErrorFacultamientos(MODULO_ASIGNACION_ID, "ActualizarFacultamientos", e, AccionEnum.ACTUALIZAR, paqueteID, null);
                            return resultado;
                        }

                        try
                        {
                            // Se actualiza de la lista de autorizantes.
                            int contadorOrden = 1;
                            foreach (var empleadoAutorizante in listaAutorizantes)
                            {
                                var autorizante = context.tblFA_Autorizante.FirstOrDefault(x => x.id.Equals(empleadoAutorizante.EmpleadoID));
                                if (autorizante != null && empleadoAutorizante.ClaveEmpleado != autorizante.usuarioID)
                                {
                                    // Si ya había un usuario vinculado a ese registro y se actualizará a uno null, se
                                    // elimina el registro y se crea otro igual con la relación en null.
                                    if (autorizante.usuarioID > 0 && empleadoAutorizante.ClaveEmpleado == null)
                                    {
                                        tblFA_Autorizante nuevoRegistroAutorizante = new tblFA_Autorizante
                                        {
                                            esAutorizante = autorizante.esAutorizante,
                                            usuarioID = null,
                                            autorizado = autorizante.autorizado,
                                            orden = autorizante.orden,
                                            paqueteID = autorizante.paqueteID
                                        };
                                        context.tblFA_Autorizante.Add(nuevoRegistroAutorizante);
                                        context.tblFA_Autorizante.Remove(autorizante);
                                        context.SaveChanges();
                                    }
                                    else
                                    {
                                        autorizante.usuarioID = empleadoAutorizante.ClaveEmpleado;
                                    }

                                }
                                empleadoAutorizante.Orden = contadorOrden++;
                            }
                        }
                        catch (Exception e)
                        {
                            dbContextTransaction.Rollback();
                            resultado.Add(SUCCESS, false);
                            resultado.Add(ERROR, "Ocurrió un error interno al intentar actualizar la lista de autorizantes del paquete de facultamientos");
                            logErrorFacultamientos(MODULO_ASIGNACION_ID, "ActualizarFacultamientos", e, AccionEnum.ACTUALIZAR, paqueteID, null);
                            return resultado;
                        }

                        // Si ya había alertas activas para otros usuarios sobre el mismo paquete, se marcan como vistas.
                        context.tblP_Alerta
                            .Where(x => x.objID.Equals(paquete.id) && !x.visto)
                            .ToList()
                            .ForEach(x => x.visto = true);

                        // Se cambian todas las autorizaciones a null, para reiniciar el proceso.
                        context.tblFA_Autorizante
                            .Where(x => x.paqueteID.Equals(paqueteID))
                            .ToList()
                            .ForEach(x =>
                            {
                                x.autorizado = null;
                                x.firma = null;
                            });

                        // Si se llenaron todos los campos, el paquete como PendienteAutorizacion y envía la primer alerta y correo correspondiente.
                        if (paquete.estado == (int)EstadoPaqueteFaEnum.PendienteAutorizacion)
                        {

                            string cc = "";

                            if (vSesiones.sesionEmpresaActual == (int)EmpresaEnum.Construplan || vSesiones.sesionEmpresaActual == (int)EmpresaEnum.GCPLAN)
                            {
                                cc = context.tblP_CC.FirstOrDefault(x => x.id.Equals(paquete.ccID)).cc;
                            }
                            else
                            {
                                cc = context.tblC_Nom_CatalogoCC.FirstOrDefault(x => x.id.Equals(paquete.ccID)).cc;
                            }

                            EmpleadoAutorizanteDTO empleadoRecibe = listaAutorizantes.FirstOrDefault(x => x.Orden.Equals(2));
                            if (empleadoRecibe.NombreEmpleado == null)
                            {
                                empleadoRecibe = listaAutorizantes.FirstOrDefault();
                            }

                            resultado.Add("ordenVoBo", empleadoRecibe.Orden);
                            resultado.Add("paqueteID", paquete.id);

                            // Enviado de alerta al primer autorizante.
                            try
                            {
                                crearNotifacionFacultamiento(
                                "Autorización de Facultamiento: " + cc,
                                empleadoRecibe.ClaveEmpleado ?? 0,
                                 paquete.id,
                                context);

                            }
                            catch (Exception e)
                            {
                                dbContextTransaction.Rollback();
                                resultado.Add(SUCCESS, false);
                                resultado.Add(ERROR, "Ocurrió un error interno al intentar enviar una notificacicón al primer autorizante del paquete de facultamientos");
                                logErrorFacultamientos(MODULO_ASIGNACION_ID, "AsignarFacultamientos", e, AccionEnum.AGREGAR, 0, null);
                                return resultado;
                            }
                        }

                        context.SaveChanges();
                        dbContextTransaction.Commit();
                        //if (new MenuDAO().isLiberado(vSesiones.sesionCurrentView))
                        //{
                        //    string objeto;
                        //    try
                        //    {
                        //        objeto = JsonUtils.convertNetObjectToJson(paquete);
                        //    }
                        //    catch (Exception)
                        //    {
                        //        objeto = "";
                        //    }
                        //    SaveBitacora((int)BitacoraEnum.PaqueteFacultamientos, (int)AccionEnum.ACTUALIZAR, paqueteID, objeto);
                        //}
                        resultado.Add(SUCCESS, true);
                    }
                    catch (Exception e)
                    {
                        dbContextTransaction.Rollback();
                        resultado.Add(SUCCESS, false);
                        resultado.Add(ERROR, "Ocurrió un error interno al intentar guardar la el paquete de facultamientos");
                        logErrorFacultamientos(MODULO_ASIGNACION_ID, "AsignarFacultamientos", e, AccionEnum.AGREGAR, 0, null);
                        return resultado;
                    }
                }
            }

            return resultado;
        }
        #endregion

        #region metodos autorizacion
        public Dictionary<string, object> ObtenerPaquetesPorAutorizar()
        {
            var resultado = new Dictionary<string, object>();
            var listaPaquetesFa = new List<CatalogoPaquetesDTO>();
            try
            {
                var usuarioID = vSesiones.sesionUsuarioDTO.id;

                // Se obtiene toda la lista de paquetes en 
                // donde el usuario logueado esté dentro de la lista de autorizantes.
                List<int> listaPaquetesID = _context.tblFA_Paquete
                    .Where(x => x.autorizantes.FirstOrDefault(y => y.usuarioID == usuarioID) != null)
                    .Select(x => x.id)
                    .ToList();

                // Se analiza Si es el empleado en turno por autorizar algún paquete.
                foreach (var paqueteID in listaPaquetesID)
                {
                    if (esUsuarioPorAutorizar(paqueteID, usuarioID, false))
                    {
                        CatalogoPaquetesDTO catalogo = null;

                        if (vSesiones.sesionEmpresaActual == (int)EmpresaEnum.Construplan || vSesiones.sesionEmpresaActual == (int)EmpresaEnum.GCPLAN)
                        {
                            catalogo = _context.tblFA_Paquete.Where(x => x.id == paqueteID).ToList().Select(x => new CatalogoPaquetesDTO
                            {
                                ID = x.id,
                                CentroCostos = x.cc.cc,
                                Descripcion = x.cc.descripcion.Trim(),
                                Fecha = (x.fechaModificacion != null) ? String.Format("{0:dd-MM-yyyy}", x.fechaModificacion) : x.fechaCreacion.ToString("dd-MM-yyyy"),
                                Departamento = x.cc.departamento.descripcion.Trim()
                            }).OrderByDescending(x => x.Fecha).FirstOrDefault();
                        }
                        else
                        {
                            var listaCatalogoCC = _context.tblC_Nom_CatalogoCC.ToList();

                            catalogo = _context.tblFA_Paquete.Where(x => x.id == paqueteID).ToList().Select(x => new CatalogoPaquetesDTO
                            {
                                ID = x.id,
                                CentroCostos = listaCatalogoCC.Where(y => y.id == x.ccID).Select(z => z.cc).FirstOrDefault(),
                                Descripcion = listaCatalogoCC.Where(y => y.id == x.ccID).Select(z => z.ccDescripcion).FirstOrDefault(),
                                Fecha = (x.fechaModificacion != null) ? String.Format("{0:dd-MM-yyyy}", x.fechaModificacion) : x.fechaCreacion.ToString("dd-MM-yyyy"),
                                Departamento = ""
                            }).OrderByDescending(x => x.Fecha).FirstOrDefault();
                        }

                        // Los paquetes en donde el usuario siga por autorizar, se van agregando a la lista.
                        listaPaquetesFa.Add(catalogo);
                    }
                }

                if (listaPaquetesFa.Count > 0)
                {
                    resultado.Add(SUCCESS, true);
                    resultado.Add("listaPaquetesFa", listaPaquetesFa);
                }
                else
                {
                    resultado.Add(SUCCESS, false);
                    resultado.Add("EMPTY", true);
                }
            }
            catch (Exception e)
            {
                resultado.Add(SUCCESS, false);
                resultado.Add(ERROR, "Ocurrió un error interno al intentar obtener los paquetes de facultamientos por autorizar.");
                logErrorFacultamientos(MODULO_AUTORIZACION_ID, "ObtenerPaquetesPorAutorizar", e, AccionEnum.CONSULTA, 0, null);
            }

            return resultado;
        }

        public Dictionary<string, object> ObtenerAutorizantes(int paqueteID)
        {
            var resultado = new Dictionary<string, object>();
            var listaAutorizantes = new List<AutorizanteFaDTO>();

            try
            {
                listaAutorizantes = _context.tblFA_Autorizante
                    .Where(x =>
                        (x.paqueteID.Equals(paqueteID)) &&
                        (x.usuarioID > 0))
                    .ToList()
                    .Select(x => new AutorizanteFaDTO
                    {
                        AutorizanteID = x.id,
                        EsAutorizante = x.esAutorizante,
                        Nombre = x.usuario.nombre + " " + x.usuario.apellidoPaterno + " " + x.usuario.apellidoMaterno,
                        UsuarioID = (esUsuarioPorAutorizar(paqueteID, x.usuarioID ?? 0, true, x.orden)) ? 1 : 0,
                        Autorizado = x.autorizado,
                        Orden = x.orden
                    })
                    .OrderBy(x => x.Orden)
                    .ToList();

                if ((listaAutorizantes != null) && (listaAutorizantes.Count > 0))
                {
                    resultado.Add(SUCCESS, true);
                    resultado.Add("listaAutorizantes", listaAutorizantes);
                }
                else
                {
                    resultado.Add(SUCCESS, false);
                    resultado.Add(ERROR, "Aún no hay autorizantes asigandos para este paquete de facultamientos.");
                }
            }
            catch (Exception e)
            {
                resultado.Add(SUCCESS, false);
                resultado.Add(ERROR, "Ocurrió un error interno al intentar obtener la lista de autorizantes del paquete de facultamientos.");
                logErrorFacultamientos(MODULO_AUTORIZACION_ID, "ObtenerAutorizantes", e, AccionEnum.CONSULTA, paqueteID, null);
            }

            return resultado;
        }

        // Función recursiva para autorizar varias veces si se repite el mismo usuario dentro de la lista de autorizantes.
        private Dictionary<string, object> Autorizar(int usuarioID, int paqueteID, tblFA_Autorizante autorizante, string cc, MainContext context, DbContextTransaction dbContextTransaction, Dictionary<string, object> resultado)
        {

            //Se vacía el diccionario.
            resultado.Clear();

            // Se marca como autorizado y se registra la firma digital.
            autorizante.autorizado = true;
            autorizante.firma = GlobalUtils.CrearFirmaDigital(paqueteID, DocumentosEnum.Facultamiento_General, usuarioID);

            // Se carga al siguiente VoBo.
            int ordenSiguiente = autorizante.orden;
            ordenSiguiente++;
            tblFA_Autorizante siguienteAutorizante = context.tblFA_Autorizante
                .FirstOrDefault(x =>
                    x.paqueteID.Equals(paqueteID) &&
                    x.usuarioID > 0 &&
                    x.autorizado != true &&
                    x.orden.Equals(ordenSiguiente));

            if (autorizante.orden.Equals(1))
            {
                siguienteAutorizante = null;
            }

            // Si el siguiente es nulo, se intenta con el autorizante
            if (siguienteAutorizante == null)
            {
                if (!autorizante.orden.Equals(1))
                {
                    siguienteAutorizante = context.tblFA_Autorizante
                        .FirstOrDefault(x =>
                            x.paqueteID.Equals(paqueteID) &&
                            x.autorizado != true &&
                            x.orden.Equals(1));
                    ordenSiguiente = 2;
                }
            }

            // Si el primero también es nulo, significa que ya autorizaron todos.
            if (siguienteAutorizante == null)
            {
                // Se actualiza el estado del paquete a Autorizado.
                var paquete = context.tblFA_Paquete.FirstOrDefault(x => x.id.Equals(paqueteID));
                paquete.estado = (int)EstadoPaqueteFaEnum.Autorizado;

                // Se marca al paquete como activo
                paquete.esActivo = true;

                // Verifica si ya había otro paquete activo, y en caso de encontrar uno, lo marca como inactivo.
                context.tblFA_Paquete.Where(x => x.ccID.Equals(paquete.ccID) && (x.esActivo == true) && (x.id != paqueteID)).ToList().ForEach(x => x.esActivo = false);

                resultado.Add("autCompleta", true);

                context.SaveChanges();
                dbContextTransaction.Commit();
                resultado.Add(SUCCESS, true);
                return resultado;
            }

            // Si el siguiente VoBo es el mismo usuario, se vuelve a llamar la misma función.
            if (siguienteAutorizante.usuarioID == usuarioID)
            {
                return this.Autorizar(usuarioID, paqueteID, siguienteAutorizante, cc, context, dbContextTransaction, resultado);
            }
            else
            {
                // Se procede a crear una notficación para el siguiente autorizante.
                crearNotifacionFacultamiento(
                    "Autorización de Facultamiento: " + cc,
                    siguienteAutorizante.usuarioID ?? 0,
                    paqueteID,
                    context);

                // Variables necesarias para enviar el correo.
                resultado.Add("ordenVoBo", siguienteAutorizante.orden);

                context.SaveChanges();
                dbContextTransaction.Commit();
                resultado.Add(SUCCESS, true);

                return resultado;
            }

        }

        public Dictionary<string, object> AutorizarPaquete(int paqueteID)
        {
            var resultado = new Dictionary<string, object>();
            using (var context = new MainContext())
            {
                using (DbContextTransaction dbContextTransaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        int usuarioID = vSesiones.sesionUsuarioDTO.id;
                        tblFA_Autorizante autorizante = context.tblFA_Autorizante
                            .Where(x =>
                                (x.usuarioID == usuarioID) &&
                                (x.paqueteID.Equals(paqueteID)) &&
                                (x.orden != 1) &&
                                (x.autorizado != true))
                            .OrderBy(x => x.orden)
                            .FirstOrDefault();

                        // Si el autorizante es nulo, se intenta con el autorizante.
                        if (autorizante == null)
                        {
                            autorizante = context.tblFA_Autorizante
                                .FirstOrDefault(x =>
                                (x.usuarioID == usuarioID) &&
                                (x.paqueteID.Equals(paqueteID)) &&
                                (x.orden.Equals(1)));
                        }

                        string cc = "";

                        if (vSesiones.sesionEmpresaActual == (int)EmpresaEnum.Construplan || vSesiones.sesionEmpresaActual == (int)EmpresaEnum.GCPLAN)
                        {
                            cc = context.tblFA_Paquete.FirstOrDefault(x => x.id.Equals(paqueteID)).cc.cc;
                        }
                        else
                        {
                            var registroPaquete = context.tblFA_Paquete.FirstOrDefault(x => x.id.Equals(paqueteID));
                            var centroCosto = context.tblC_Nom_CatalogoCC.FirstOrDefault(x => x.id == registroPaquete.ccID);

                            cc = centroCosto.cc;
                        }

                        try
                        {
                            //Se desactiva la alerta al usuario actual.
                            context.tblP_Alerta.FirstOrDefault(x =>
                                x.objID.Equals(paqueteID) &&
                                x.url.Contains("/Administrativo/Facultamiento/AutorizacionFA") &&
                                !x.visto &&
                                x.userRecibeID.Equals(usuarioID)).visto = true;
                        }
                        catch { }

                        // Función recursiva por si se repite el mismo usuario entre los autorizantes.
                        return this.Autorizar(usuarioID, paqueteID, autorizante, cc, context, dbContextTransaction, resultado);
                    }
                    catch (Exception e)
                    {
                        dbContextTransaction.Rollback();
                        resultado.Add(SUCCESS, false);
                        resultado.Add(ERROR, "Ocurrió un error interno al intentar autorizar el paquete de facultamientos");
                        logErrorFacultamientos(MODULO_AUTORIZACION_ID, "AutorizarPaquete", e, AccionEnum.ACTUALIZAR, paqueteID, null);
                        return resultado;
                    }
                }
            }
        }

        public Dictionary<string, object> RechazarPaquete(int paqueteID, string comentario)
        {
            var resultado = new Dictionary<string, object>();
            using (var context = new MainContext())
            {
                using (DbContextTransaction dbContextTransaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        int usuarioID = vSesiones.sesionUsuarioDTO.id;

                        // Se obtiene toda la lista de autorizantes y se cambia el estado de autorizado a null (pendiente) y se eliminan las firmas.
                        context.tblFA_Autorizante
                            .Where(x => x.paqueteID.Equals(paqueteID))
                            .ToList()
                            .ForEach(x =>
                            {
                                x.autorizado = null;
                                x.firma = null;
                            });

                        // Se desactiva la alerta al usuario actual.
                        context.tblP_Alerta.FirstOrDefault(x =>
                            x.objID.Equals(paqueteID) &&
                            x.url.Contains("/Administrativo/Facultamiento/AutorizacionFA") &&
                            !x.visto &&
                            x.userRecibeID.Equals(usuarioID)).visto = true;

                        // Se cambia el estado del paquete a editando y se le agrega el comentario.
                        tblFA_Paquete paquete = context.tblFA_Paquete.FirstOrDefault(x => x.id.Equals(paqueteID));
                        paquete.comentario = comentario.Trim();
                        paquete.estado = (int)EstadoPaqueteFaEnum.Editando;

                        context.SaveChanges();
                        dbContextTransaction.Commit();
                        resultado.Add(SUCCESS, true);
                    }
                    catch (Exception e)
                    {
                        dbContextTransaction.Rollback();
                        resultado.Add(SUCCESS, false);
                        resultado.Add(ERROR, "Ocurrió un error interno al intentar rechazar el paquete de facultamientos");
                        logErrorFacultamientos(MODULO_AUTORIZACION_ID, "RechazarPaquete", e, AccionEnum.ACTUALIZAR, paqueteID, null);
                        return resultado;
                    }
                }
            }

            return resultado;
        }

        public Dictionary<string, object> EnviarCorreoAutorizacion(int paqueteID, int ordenVoBo, List<Byte[]> pdf)
        {
            var resultado = new Dictionary<string, object>();

            try
            {
                var usuarioEnvia = vSesiones.sesionUsuarioDTO;
                tblFA_Paquete paquete = _context.tblFA_Paquete.FirstOrDefault(x => x.id.Equals(paqueteID));
                tblFA_Autorizante empleadoRecibe = paquete.autorizantes.FirstOrDefault(x => x.orden.Equals(ordenVoBo));
                string correo = empleadoRecibe.usuario.correo;

                string cc = "";

                if (vSesiones.sesionEmpresaActual == (int)EmpresaEnum.Construplan || vSesiones.sesionEmpresaActual == (int)EmpresaEnum.GCPLAN)
                {
                    cc = paquete.cc.cc;
                }
                else
                {
                    cc = _context.tblC_Nom_CatalogoCC.FirstOrDefault(x => x.id == paquete.ccID).cc;
                }

                string asunto = string.Format("{0}: {1}", vSesiones.sesionEmpresaActual.Equals(1) ? "CC" : "AC", cc);
                string cuerpoCorreo = @"<html>
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
                                                            <div class=WordSection1>";
                cuerpoCorreo += @" <p class=MsoNormal>Se registró una autorización de Facultamientos por el empleado "
                 + usuarioEnvia.nombre + " " + usuarioEnvia.apellidoPaterno + " " + usuarioEnvia.apellidoMaterno + ".<o:p></o:p></p>";
                cuerpoCorreo += @"</tbody></table>
                                                                <p class=MsoNormal>
                                                                    Favor de ingresar al sistema <a href='http://sigoplan.construplan.com.mx/'>SIGOPLAN</a>,
                                                                    en el apartado de Administración, menú Facultamiento, submenú Generales, en el apartado de Autorización.<o:p></o:p>
                                                                </p>
                                                                <p class=MsoNormal>
                                                                    También puede acceder ingresando normalmente al sistema y dando clic en la notificación correspondiente.<o:p></o:p>
                                                                </p>
                                                                <p class=MsoNormal>
                                                                    <o:p>" + string.Format("{0}: {1}.", vSesiones.sesionEmpresaActual.Equals(1) ? "Centro Costos" : "Area Cuenta", cc) + @"</o:p>
                                                                </p>
                                                                <p class=MsoNormal>
                                                                    PD. Se informa que esta notificación es autogenerada por el sistema SIGOPLAN y no es necesario dar una respuesta.<o:p></o:p>
                                                                </p>
                                                                <p class=MsoNormal>
                                                                    Gracias.<o:p></o:p>
                                                                </p>
                                                            </div>
                                                        </body>
                                                    </html>";
                string tipoFormato = "Facultamiento.pdf";
                if (!GlobalUtils.sendEmailAdjuntoInMemory2(string.Format("{0}: {1}", PersonalUtilities.GetNombreEmpresa(), "Alerta de Facultamientos " + asunto), cuerpoCorreo, new List<string> { correo }, pdf, tipoFormato))
                {
                    resultado.Add(SUCCESS, false);
                    resultado.Add(ERROR, "Ocurrió un error interno al intentar enviar el correo de autorización");
                }
                else
                {
                    resultado.Add(SUCCESS, true);
                }
            }
            catch (Exception e)
            {
                resultado.Add(SUCCESS, false);
                resultado.Add(ERROR, "Ocurrió un error interno al intentar enviar el correo de autorización");
                logErrorFacultamientos(MODULO_AUTORIZACION_ID, "enviarCorreoAutorizacion", e, AccionEnum.CONSULTA, paqueteID, null);
            }
            return resultado;
        }

        public Dictionary<string, object> EnviarCorreoAutorizacionCompleta(int paqueteID, List<Byte[]> pdf)
        {
            var resultado = new Dictionary<string, object>();

            try
            {
                tblFA_Paquete paquete = _context.tblFA_Paquete.FirstOrDefault(x => x.id.Equals(paqueteID));
                string cc = "";

                if (vSesiones.sesionEmpresaActual == (int)EmpresaEnum.Construplan || vSesiones.sesionEmpresaActual == (int)EmpresaEnum.GCPLAN)
                {
                    cc = paquete.cc.cc;
                }
                else
                {
                    cc = _context.tblC_Nom_CatalogoCC.FirstOrDefault(x => x.id == paquete.ccID).cc;
                }

                List<string> listaCorreos = _context.tblFA_Autorizante
                    .Where(x => x.paqueteID.Equals(paqueteID) && x.usuarioID > 0)
                    .Select(x => x.usuario.correo).ToList();

                // También se le envía correo a Antontio Castro(11) y Luis Castro(12).
                new List<int> { 11, 12 }.ForEach(x =>
                {
                    var usuarioDefault = _context.tblP_Usuario.FirstOrDefault(y => y.id.Equals(x));
                    listaCorreos.Add(usuarioDefault.correo);
                });

                var usuarioEnvia = vSesiones.sesionUsuarioDTO;
                string asunto = string.Format("{0}: {1}", vSesiones.sesionEmpresaActual.Equals(1) ? "CC" : "AC", cc);
                string cuerpoCorreo = @"<html>
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
                                            <div class=WordSection1>";
                cuerpoCorreo += @" <p class=MsoNormal>Se <strong>autorizó</strong> completamente el Paquete de Facultamientos -  "
        + string.Format("{0}: {1}.", vSesiones.sesionEmpresaActual.Equals(1) ? "Centro Costos" : "Area Cuenta", cc) + "<o:p></o:p></p>";
                cuerpoCorreo += @"</tbody></table>
                                                <p class=MsoNormal>
                                                    PD. Se informa que esta notificación es autogenerada por el sistema SIGOPLAN y no es necesario dar una respuesta.<o:p></o:p>
                                                </p>
                                                <p class=MsoNormal>
                                                    Gracias.<o:p></o:p>
                                                </p>
                                            </div>
                                        </body>
                                    </html>";
                string tipoFormato = "Facultamiento.pdf";
                if (!GlobalUtils.sendEmailAdjuntoInMemory2(string.Format("{0}: {1}", PersonalUtilities.GetNombreEmpresa(), "Paquete de Facultamientos Autorizado. " + asunto), cuerpoCorreo, listaCorreos, pdf, tipoFormato))
                {
                    resultado.Add(SUCCESS, false);
                    resultado.Add(ERROR, "Ocurrió un error interno al intentar enviar el correo de autorización completa");
                }
                else
                {
                    resultado.Add(SUCCESS, true);
                }
            }
            catch (Exception e)
            {
                resultado.Add(SUCCESS, false);
                resultado.Add(ERROR, "Ocurrió un error interno al intentar enviar el correo de autorización completa");
                logErrorFacultamientos(MODULO_AUTORIZACION_ID, "enviarCorreoAutorizacionCompleta", e, AccionEnum.CONSULTA, paqueteID, null);
            }
            return resultado;
        }

        public Dictionary<string, object> EnviarCorreoRechazo(int paqueteID, string comentario, List<Byte[]> pdf)
        {
            var resultado = new Dictionary<string, object>();
            try
            {

                tblFA_Paquete paquete = _context.tblFA_Paquete.FirstOrDefault(x => x.id.Equals(paqueteID));
                string cc = "";

                if (vSesiones.sesionEmpresaActual == (int)EmpresaEnum.Construplan || vSesiones.sesionEmpresaActual == (int)EmpresaEnum.GCPLAN)
                {
                    cc = paquete.cc.cc;
                }
                else
                {
                    cc = _context.tblC_Nom_CatalogoCC.FirstOrDefault(x => x.id == paquete.ccID).cc;
                }

                List<string> listaCorreos = _context.tblFA_Autorizante
                    .Where(x => x.paqueteID.Equals(paqueteID) && x.usuarioID > 0)
                    .Select(x => x.usuario.correo).ToList();

                var usuarioEnvia = vSesiones.sesionUsuarioDTO;
                string asunto = string.Format("{0}: {1}", vSesiones.sesionEmpresaActual.Equals(1) ? "CC" : "AC", cc);
                string cuerpoCorreo = @"<html>
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
                                            <div class=WordSection1>";
                cuerpoCorreo += @" <p class=MsoNormal>Se <strong>rechazó</strong> el paquete de facultamientos "
                    + string.Format("{0}: {1}", vSesiones.sesionEmpresaActual.Equals(1) ? "Centro Costos" : "Area Cuenta", cc);
                cuerpoCorreo += @" por el empleado " + usuarioEnvia.nombre + " " + usuarioEnvia.apellidoPaterno + " " + usuarioEnvia.apellidoMaterno + ".<o:p></o:p></p>";
                cuerpoCorreo += @"</tbody></table>";
                cuerpoCorreo += @"<p class=MsoNormal>Razón del rechazo: " + comentario + "<o:p></o:p></p>";
                cuerpoCorreo += @"<p class=MsoNormal>
                                                    PD. Se informa que esta notificación es autogenerada por el sistema SIGOPLAN y no es necesario dar una respuesta.<o:p></o:p>
                                                </p>
                                                <p class=MsoNormal>
                                                    Gracias.<o:p></o:p>
                                                </p>
                                            </div>
                                        </body>
                                    </html>";
                string tipoFormato = "Facultamiento.pdf";
                if (!GlobalUtils.sendEmailAdjuntoInMemory2(string.Format("{0}: {1}", PersonalUtilities.GetNombreEmpresa(), "Paquete de Facultamientos Rechazado. " + asunto), cuerpoCorreo, listaCorreos, pdf, tipoFormato))
                {
                    resultado.Add(SUCCESS, false);
                    resultado.Add(ERROR, "Ocurrió un error interno al intentar enviar el correo de rechazo");
                }
                else
                {
                    resultado.Add(SUCCESS, true);
                }
            }
            catch (Exception e)
            {
                resultado.Add(SUCCESS, false);
                resultado.Add(ERROR, "Ocurrió un error interno al intentar enviar el correo de rechazo");
                logErrorFacultamientos(MODULO_AUTORIZACION_ID, "enviarCorreoRechazo", e, AccionEnum.CONSULTA, paqueteID, null);
            }
            return resultado;
        }
        #endregion

        #region metodos historico
        public Dictionary<string, object> ObtenerHistorico(int ccID)
        {
            var resultado = new Dictionary<string, object>();
            var listaPaquetesFa = new List<CatalogoPaquetesDTO>();
            try
            {
                var listaCC = _context.tblFA_Paquete.Where(x => (ccID.Equals(0)) ? x.ccID != 0 : x.ccID.Equals(ccID)).Select(x => x.ccID).Distinct().ToList();

                listaCC.ForEach(x =>
                    {
                        var paquetesCC = _context.tblFA_Paquete.Where(y => y.ccID.Equals(x)).OrderBy(y => y.id).ToList();
                        int version = 1;
                        paquetesCC.ForEach(y =>
                        {
                            CatalogoPaquetesDTO paquete = null;

                            if (vSesiones.sesionEmpresaActual == (int)EmpresaEnum.Construplan || vSesiones.sesionEmpresaActual == (int)EmpresaEnum.GCPLAN)
                            {
                                paquete = new CatalogoPaquetesDTO
                                {
                                    ID = y.id,
                                    CentroCostos = y.cc.cc,
                                    ccID = y.ccID,
                                    Descripcion = y.cc.descripcion,
                                    Fecha = (y.fechaModificacion != null) ? String.Format("{0:dd-MM-yyyy}", y.fechaModificacion) : y.fechaCreacion.ToString("dd-MM-yyyy"),
                                    Estatus = EnumExtensions.GetDescription((EstadoPaqueteFaEnum)(Int32.Parse(y.estado.ToString()))),
                                    Departamento = y.cc.departamento.descripcion,
                                    Version = version++
                                };
                            }
                            else
                            {
                                var listaCatalogoCC = _context.tblC_Nom_CatalogoCC.ToList();

                                paquete = new CatalogoPaquetesDTO
                                {
                                    ID = y.id,
                                    CentroCostos = listaCatalogoCC.Where(z => z.id == y.ccID).Select(w => w.cc).FirstOrDefault(),
                                    ccID = y.ccID,
                                    Descripcion = listaCatalogoCC.Where(z => z.id == y.ccID).Select(w => w.ccDescripcion).FirstOrDefault(),
                                    Fecha = (y.fechaModificacion != null) ? String.Format("{0:dd-MM-yyyy}", y.fechaModificacion) : y.fechaCreacion.ToString("dd-MM-yyyy"),
                                    Estatus = EnumExtensions.GetDescription((EstadoPaqueteFaEnum)(Int32.Parse(y.estado.ToString()))),
                                    Departamento = "",
                                    Version = version++
                                };
                            }

                            listaPaquetesFa.Add(paquete);
                        }
                        );
                        listaPaquetesFa = listaPaquetesFa.OrderByDescending(y => y.ID).ToList();
                    });

                if (listaPaquetesFa.Count > 0)
                {
                    resultado.Add(SUCCESS, true);
                    resultado.Add("listaPaquetesFa", listaPaquetesFa);
                }
                else
                {
                    resultado.Add(SUCCESS, false);
                    resultado.Add("EMPTY", true);
                }
            }
            catch (Exception e)
            {
                resultado.Add(SUCCESS, false);
                resultado.Add(ERROR, "Ocurrió un error interno al intentar obtener el histórico de paquetes de facultamientos.");
                logErrorFacultamientos(MODULO_HISTORICO_ID, "ObtenerHistorico", e, AccionEnum.CONSULTA, 0, null);
            }

            return resultado;
        }
        #endregion

        #region metodos por empleado
        public Dictionary<string, object> ObtenerFacultamientosEmpleado(int claveEmpleado, int centroCostosID)
        {
            var resultado = new Dictionary<string, object>();
            var listaFacultamientos = new List<CatalogoEmpleadoDTO>();
            try
            {
                var facultamientos = _context.tblFA_Facultamiento.Where(x => x.empleados.Any(y => y.claveEmpleado == claveEmpleado)).ToList();
                facultamientos.ForEach(facultamiento =>
                {
                    facultamiento.empleados.Where(x => x.claveEmpleado == claveEmpleado).ToList().ForEach(empleado =>
                    {
                        CatalogoEmpleadoDTO catalogoEmpleado = null;

                        if (vSesiones.sesionEmpresaActual == (int)EmpresaEnum.Construplan || vSesiones.sesionEmpresaActual == (int)EmpresaEnum.GCPLAN)
                        {
                            catalogoEmpleado = new CatalogoEmpleadoDTO
                            {
                                FacultamientoID = facultamiento.id,
                                CentroCostos = facultamiento.paquete.cc.cc,
                                ccID = facultamiento.paquete.ccID,
                                Descripcion = facultamiento.paquete.cc.descripcion.Trim(),
                                Titulo = facultamiento.plantilla.titulo,
                                Puesto = empleado.concepto.concepto,
                                Fecha = (facultamiento.paquete.fechaModificacion != null) ?
                                String.Format("{0:dd-MM-yyyy}", facultamiento.paquete.fechaModificacion) :
                                facultamiento.paquete.fechaCreacion.ToString("dd-MM-yyyy"),
                                Estatus = EnumExtensions.GetDescription((EstadoPaqueteFaEnum)(Int32.Parse(facultamiento.paquete.estado.ToString()))),
                                TipoAutorizacion = empleado.concepto.esAutorizacion ? "Autorizante" : "VoBo"
                            };
                        }
                        else
                        {
                            var listaCatalogoCC = _context.tblC_Nom_CatalogoCC.ToList();

                            catalogoEmpleado = new CatalogoEmpleadoDTO
                            {
                                FacultamientoID = facultamiento.id,
                                CentroCostos = listaCatalogoCC.Where(x => x.id == facultamiento.paquete.ccID).Select(y => y.cc).FirstOrDefault(),
                                ccID = facultamiento.paquete.ccID,
                                Descripcion = listaCatalogoCC.Where(x => x.id == facultamiento.paquete.ccID).Select(y => y.ccDescripcion).FirstOrDefault(),
                                Titulo = facultamiento.plantilla.titulo,
                                Puesto = empleado.concepto.concepto,
                                Fecha = (facultamiento.paquete.fechaModificacion != null) ?
                                String.Format("{0:dd-MM-yyyy}", facultamiento.paquete.fechaModificacion) :
                                facultamiento.paquete.fechaCreacion.ToString("dd-MM-yyyy"),
                                Estatus = EnumExtensions.GetDescription((EstadoPaqueteFaEnum)(Int32.Parse(facultamiento.paquete.estado.ToString()))),
                                TipoAutorizacion = empleado.concepto.esAutorizacion ? "Autorizante" : "VoBo"
                            };
                        }

                        listaFacultamientos.Add(catalogoEmpleado);
                    });
                });

                if (listaFacultamientos.Count > 0)
                {
                    resultado.Add(SUCCESS, true);
                    resultado.Add("listaFacultamientos", listaFacultamientos);
                }
                else
                {
                    resultado.Add(SUCCESS, false);
                    resultado.Add("EMPTY", true);
                }
            }
            catch (Exception e)
            {
                resultado.Add(SUCCESS, false);
                resultado.Add(ERROR, "Ocurrió un error interno al intentar obtener el catálogo de paquetes de facultamientos.");
                logErrorFacultamientos(MODULO_EMPLEADO_ID, "ObtenerFacultamientosEmpleado", e, AccionEnum.CONSULTA, claveEmpleado, null);
            }

            return resultado;
        }

        public Dictionary<string, object> ObtenerFacultamiento(int facultamientoID)
        {
            var resultado = new Dictionary<string, object>();
            var facultamientoDTO = new FacultamientoDTO();
            try
            {
                // Se obtiene el facultamiento y se crea su DTO.
                if (vSesiones.sesionEmpresaActual == (int)EmpresaEnum.Construplan || vSesiones.sesionEmpresaActual == (int)EmpresaEnum.GCPLAN)
                {
                    facultamientoDTO = _context.tblFA_Facultamiento.Where(x => x.id.Equals(facultamientoID)).ToList().Select(x => new FacultamientoDTO
                    {
                        orden = x.plantilla.orden,
                        Aplica = x.aplica,
                        CentroCostos = x.paquete.cc.cc,
                        Departamento = x.paquete.cc.departamento.descripcion.Trim(),
                        Fecha = (x.paquete.fechaModificacion != null) ? x.paquete.fechaModificacion.ToString() : x.paquete.fechaCreacion.ToString(),
                        Obra = x.paquete.cc.descripcion.Trim(),
                        Titulo = x.plantilla.titulo,
                        FacultamientoID = x.id
                    }).FirstOrDefault();
                }
                else
                {
                    var listaCatalogoCC = _context.tblC_Nom_CatalogoCC.ToList();

                    facultamientoDTO = _context.tblFA_Facultamiento.Where(x => x.id.Equals(facultamientoID)).ToList().Select(x => new FacultamientoDTO
                    {
                        orden = x.plantilla.orden,
                        Aplica = x.aplica,
                        CentroCostos = listaCatalogoCC.Where(y => y.id == x.paquete.ccID).Select(z => z.cc).FirstOrDefault(),
                        Departamento = "",
                        Fecha = (x.paquete.fechaModificacion != null) ? x.paquete.fechaModificacion.ToString() : x.paquete.fechaCreacion.ToString(),
                        Obra = listaCatalogoCC.Where(y => y.id == x.paquete.ccID).Select(z => z.ccDescripcion).FirstOrDefault(),
                        Titulo = x.plantilla.titulo,
                        FacultamientoID = x.id
                    }).FirstOrDefault();
                }

                // Se agrega la lista de empleados al facultamiento.
                facultamientoDTO.ListaEmpleados = _context.tblFA_Empleado
                    .Where(x => x.facultamientoID.Equals(facultamientoDTO.FacultamientoID) && x.esActivo && x.aplica)
                    .OrderBy(x => x.concepto.orden)
                    .Select(x => new EmpleadoFaDTO
                    {
                        EmpleadoID = x.id,
                        NombreEmpleado = x.nombreEmpleado.Trim(),
                        ClaveEmpleado = x.claveEmpleado,
                        ConceptoID = x.conceptoID,
                        Editado = x.editado,
                        Concepto = x.concepto.concepto.Trim(),
                        EsAutorizante = x.concepto.esAutorizacion,
                        Aplica = x.aplica
                    })
                    .ToList();

                resultado.Add(SUCCESS, true);
                resultado.Add("facultamiento", facultamientoDTO);
            }
            catch (Exception e)
            {
                resultado.Add(SUCCESS, false);
                resultado.Add(ERROR, "Ocurrió un error interno al intentar cargar los facultamientos de la obra seleccionada.");
                logErrorFacultamientos(MODULO_EMPLEADO_ID, "ObtenerFacultamiento", e, AccionEnum.CONSULTA, facultamientoID, null);
                return resultado;
            }

            return resultado;
        }

        public string ObtenerNombreEmpleadoPorClave(int claveEmpleado)
        {
            try
            {
                if (claveEmpleado == 0)
                {
                    return "";
                }

                //var consulta = string.Format(@"SELECT TRIM(nombre) + ' ' + TRIM(ape_paterno)+ ' ' + TRIM(ape_materno) as nombreEmpleado FROM sn_empleados WHERE clave_empleado ={0}", claveEmpleado);
                //var nombreEmpleado = (List<dynamic>)ContextEnKontrolNomina.Where(consulta).ToObject<List<dynamic>>();

                var nombreEmpleado = _context.Select<dynamic>(new DapperDTO
                {
                    baseDatos = (int)vSesiones.sesionEmpresaActual == (int)EmpresaEnum.Construplan ? MainContextEnum.Construplan : MainContextEnum.Arrendadora,
                    consulta = @"SELECT TRIM(nombre) + ' ' + TRIM(ape_paterno)+ ' ' + TRIM(ape_materno) as nombreEmpleado FROM tblRH_EK_Empleados WHERE clave_empleado = @claveEmpleado",
                    parametros = new { claveEmpleado }
                });

                if (nombreEmpleado.Count() == 0) 
                {
                    nombreEmpleado = _context.Select<dynamic>(new DapperDTO
                    {
                        baseDatos = MainContextEnum.GCPLAN,
                        consulta = @"SELECT TRIM(nombre) + ' ' + TRIM(ape_paterno)+ ' ' + TRIM(ape_materno) as nombreEmpleado FROM tblRH_EK_Empleados WHERE clave_empleado = @claveEmpleado",
                        parametros = new { claveEmpleado }
                    });
                }


                try
                {
                    return nombreEmpleado.First().nombreEmpleado.ToString();
                }
                catch (Exception e)
                {
                    return "";
                }
            }
            catch
            {
                return "";
            }
        }
        #endregion

        #region HelperMethods
        /// <summary>
        /// Permite loguear un error con ciertos parámetros default para el módulo de facultamientos.
        /// </summary>
        /// <param name="moduloID">Identificador del módulo al que pertenece la acción.</param>
        /// <param name="accion">Nombre de la acción del controlador que invocó el método.</param>
        /// <param name="excepcion">Ínstancia de algún objeto Excepcion o derivado.</param>
        /// <param name="tipo">Tipo de acción que se estaba intentando realizar al momento del error.</param>
        /// <param name="registroID">Identificador del registro en caso de haber intentado eliminar o actualiazr algún registro existente.</param>
        /// <param name="objeto">Si se estaba trabajando sobre algún objeto existente, se puede intentar serializar.</param>
        private void logErrorFacultamientos(int moduloID, string accion, Exception excepcion, AccionEnum tipo, int registroID, object objeto)
        {
            LogError(SISTEMA_ID, moduloID, NOMBRE_CONTROLADOR, accion, excepcion, tipo, registroID, objeto);
        }

        /// <summary>
        /// Verifica Si el usuario es el próximo en autorizar el paquete de facultamientos indicado.
        /// </summary>
        /// <param name="paqueteID">Identificador del paquete de facultamientos</param>
        /// <param name="usuarioID">Identificador del Usuario</param>
        /// <returns>Retorna verdadero si el usuario es el siguiente por autorizar, de lo contrario, falso.</returns>
        private bool esUsuarioPorAutorizar(int paqueteID, int usuarioID, bool esAutorizacion, int orden = 0)
        {
            int voBo = 0;
            List<tblFA_Autorizante> listaAutorizantes = _context.tblFA_Autorizante
                .Where(x =>
                    (x.paqueteID.Equals(paqueteID)) &&
                    (x.usuarioID > 0) &&
                    (x.autorizado != true) &&
                    (x.paquete.estado.Equals((int)EstadoPaqueteFaEnum.PendienteAutorizacion))
                )
                .OrderBy(x => x.orden)
                .ToList();
            if (listaAutorizantes == null || listaAutorizantes.Count.Equals(0))
            {
                return false;
            }
            else if (listaAutorizantes.FirstOrDefault().autorizado == true)
            {
                return false;
            }
            voBo = listaAutorizantes.Max(x => x.orden);
            if (voBo > 1)
            {
                voBo = listaAutorizantes.Where(x => x.orden > 1 && x.autorizado != true).Min(x => x.orden);
            }
            if (esAutorizacion)
            {
                return listaAutorizantes.FirstOrDefault(x => x.orden.Equals(voBo)).usuarioID == usuarioID &&
                listaAutorizantes.FirstOrDefault(x => x.orden.Equals(voBo)).orden == orden;
            }
            else
            {
                return listaAutorizantes.FirstOrDefault(x => x.orden.Equals(voBo)).usuarioID == usuarioID;
            }
        }

        private void crearNotifacionFacultamiento(string mensaje, int usuarioRecibeID, int objetoID, MainContext context)
        {
            var alertaFacultamiento = new tblP_Alerta
            {
                msj = mensaje,
                sistemaID = SISTEMA_ID,
                tipoAlerta = (int)AlertasEnum.REDIRECCION,
                url = "/Administrativo/Facultamiento/AutorizacionFA",
                userEnviaID = vSesiones.sesionUsuarioDTO.id,
                userRecibeID = usuarioRecibeID,
                objID = objetoID
            };
            context.tblP_Alerta.Add(alertaFacultamiento);
        }
        #endregion

        #region metodo cargado solicitud de maquinaria
        // Método para carga manual de solicitud de maquinaria.
        private void cargarSolicitudMaquinaria()
        {
            using (var context = new MainContext())
            {
                using (DbContextTransaction dbContextTransaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        // Se sacan los diferentes CC presentes en las autorizaciones
                        var diferentesCentroCostos = (from aut in _context.tblP_Autoriza
                                                      from ccUsuario in _context.tblP_CC_Usuario
                                                      where aut.cc_usuario_ID == ccUsuario.id
                                                      select new
                                                      {
                                                          cc = ccUsuario.cc
                                                      }).OrderBy(x => x.cc).Distinct().ToList();

                        var listaCCs = new List<tblP_CC>();

                        // Se crea una lista con los CC existentes.
                        diferentesCentroCostos.ForEach(x =>
                        {
                            var cc = _context.tblP_CC.FirstOrDefault(y => y.cc.Trim() == x.cc.Trim());
                            if (cc != null)
                            {
                                listaCCs.Add(cc);
                            }
                        });

                        // Se itera entre los CC existentes y se busca si ya se tiene a los 4 autorizantes.
                        foreach (var cc in listaCCs)
                        {
                            var listaEmpleados = (from aut in _context.tblP_Autoriza
                                                  from ccUsuario in _context.tblP_CC_Usuario
                                                  from perfilAut in _context.tblP_PerfilAutoriza
                                                  where aut.cc_usuario_ID == ccUsuario.id
                                                  where perfilAut.id == aut.perfilAutorizaID
                                                  where ccUsuario.cc == cc.cc
                                                  where (aut.perfilAutorizaID == 1 || aut.perfilAutorizaID == 2 || aut.perfilAutorizaID == 3 || aut.perfilAutorizaID == 4)
                                                  select new
                                                  {
                                                      usuarioID = aut.usuarioID,
                                                      nombre = aut.usuario.nombre + " " + aut.usuario.apellidoPaterno + " " + aut.usuario.apellidoMaterno,
                                                      perfilID = perfilAut.id,
                                                      perfil = perfilAut.descripcion,
                                                      cc = ccUsuario.cc
                                                  }).OrderBy(x => x.cc).ToList();

                            // Si ya tiene los 4 autorizantes, se procede a crear la asignacion de facultamientos.
                            if (listaEmpleados.Count == 4)
                            {

                                // Se guarda el paquete de facultamientos.
                                var nuevoPaquete = new tblFA_Paquete();
                                nuevoPaquete.ccID = cc.id;
                                nuevoPaquete.estado = (int)EstadoPaqueteFaEnum.Editando;
                                nuevoPaquete.fechaCreacion = DateTime.Now;
                                nuevoPaquete.esActivo = null;
                                nuevoPaquete.usuarioCreadorID = vSesiones.sesionUsuarioDTO.id;
                                context.tblFA_Paquete.Add(nuevoPaquete);
                                context.SaveChanges();

                                // Se crea el facultamiento
                                var nuevoFacultamiento = new tblFA_Facultamiento();
                                nuevoFacultamiento.plantillaID = 1111111; // ID PENDIENTE
                                nuevoFacultamiento.paqueteID = nuevoPaquete.id;
                                nuevoFacultamiento.aplica = true;


                                // Guardado de la lista de empleados.
                                foreach (var empleadoAutorizante in listaEmpleados)
                                {
                                    var empleado = new tblFA_Empleado
                                    {
                                        editado = false,
                                        esActivo = true,
                                        facultamientoID = nuevoFacultamiento.id,
                                        nombreEmpleado = empleadoAutorizante.nombre.Trim(),
                                        claveEmpleado = obtenerClaveEmpleadoPorNombre(empleadoAutorizante.nombre.Trim()),
                                        aplica = true
                                    };

                                    switch (empleadoAutorizante.perfilID)
                                    {
                                        case 1:
                                            empleado.conceptoID = 115; // ID PENDIENTE
                                            break;
                                        case 2:
                                            empleado.conceptoID = 116; // ID PENDIENTE
                                            break;
                                        case 3:
                                            empleado.conceptoID = 114; // ID PENDIENTE
                                            break;
                                        case 4:
                                            empleado.conceptoID = 117; // ID PENDIENTE
                                            break;
                                    }

                                    context.tblFA_Empleado.Add(empleado);
                                    context.SaveChanges();
                                }

                                // Se crean los autorizantes manualmente
                                var autorizanteReina = new tblFA_Autorizante
                                {
                                    esAutorizante = true,
                                    autorizado = null,
                                    orden = 1,
                                    paqueteID = nuevoPaquete.id,
                                    usuarioID = 1164,
                                };

                                var autorizanteElia = new tblFA_Autorizante
                                {
                                    esAutorizante = false,
                                    autorizado = null,
                                    orden = 2,
                                    paqueteID = nuevoPaquete.id,
                                    usuarioID = 1123,
                                };

                                var autorizanteGaytan = new tblFA_Autorizante
                                {
                                    esAutorizante = false,
                                    autorizado = null,
                                    orden = 3,
                                    paqueteID = nuevoPaquete.id,
                                    usuarioID = 4,
                                };

                                var autorizanteVacio4 = new tblFA_Autorizante
                                {
                                    esAutorizante = false,
                                    autorizado = null,
                                    orden = 4,
                                    paqueteID = nuevoPaquete.id,
                                    usuarioID = null
                                };

                                var autorizanteVacio5 = new tblFA_Autorizante
                                {
                                    esAutorizante = false,
                                    autorizado = null,
                                    orden = 5,
                                    paqueteID = nuevoPaquete.id,
                                    usuarioID = null
                                };

                                context.tblFA_Autorizante.Add(autorizanteReina);
                                context.tblFA_Autorizante.Add(autorizanteElia);
                                context.tblFA_Autorizante.Add(autorizanteGaytan);
                                context.tblFA_Autorizante.Add(autorizanteVacio4);
                                context.tblFA_Autorizante.Add(autorizanteVacio5);
                            }
                        }
                        context.SaveChanges();
                        dbContextTransaction.Commit();
                    }
                    catch (Exception)
                    {
                        dbContextTransaction.Rollback();
                    }
                }
            }
        }

        // Busca la clave de un empleado en la bd EnKontrol por el nombre completo.
        private int? obtenerClaveEmpleadoPorNombre(string nombre)
        {
            try
            {
                if (string.IsNullOrEmpty(nombre))
                {
                    return null;
                }
                nombre = nombre.Replace(" ", String.Empty);
                //var consulta = string.Format(@"SELECT clave_empleado FROM sn_empleados WHERE REPLACE(nombre + ape_paterno + ape_materno, ' ', '') like '{0}%'", nombre);
                //var claveEmpleado = (List<dynamic>)ContextEnKontrolNomina.Where(consulta).ToObject<List<dynamic>>();
                int? clave;

                var claveEmpleado = _context.Select<dynamic>(new DapperDTO
                {
                    baseDatos = (int)vSesiones.sesionEmpresaActual == (int)EmpresaEnum.Construplan ? MainContextEnum.Construplan : MainContextEnum.Arrendadora,
                    consulta = @"SELECT clave_empleado FROM sn_empleados WHERE REPLACE(nombre + ape_paterno + ape_materno, ' ', '') like '@nombre%'",
                    parametros = new { nombre }
                });
                try
                {
                    clave = Int32.Parse(claveEmpleado.First().clave_empleado.ToString());
                }
                catch (Exception e)
                {
                    clave = null;
                }

                return clave;
            }
            catch
            {
                return null;
            }
        }
        #endregion
        #region metodos catalogo grupos

        public Dictionary<string, object> ObtenerCCGrupo(int grupoID)
        {
            var resultado = new Dictionary<string, object>();
            var data = new List<dynamic>();
            try
            {
                if (vSesiones.sesionEmpresaActual == (int)EmpresaEnum.Construplan || vSesiones.sesionEmpresaActual == (int)EmpresaEnum.GCPLAN)
                {
                    var temp = _context.tblP_CC.Where(x => (grupoID == 0 ? true : (int)x.grupoID == grupoID) && x.estatus).Select(x => new
                    {
                        id = x.id,
                        cc = x.cc,
                        descripcion = x.descripcion,
                        grupoID = x.grupoID == null ? 0 : (int)x.grupoID
                    }).ToList();

                    data.AddRange(temp);
                }
                else
                {
                    var ccs = _context.tblC_Nom_CatalogoCC.Where(x => x.estatus).Select(x => new CCAgrupacionDTO
                    {
                        id = x.id,
                        cc = x.cc,
                        descripcion = x.ccDescripcion
                    }).ToList();

                    var ccGrupos = _context.tblC_Nom_CatalogoCCtblFA_Grupos.Where(x => x.registroActivo && (grupoID > 0 ? x.grupo_id == grupoID : true)).ToList();

                    foreach (var item in ccs)
                    {
                        var ccGrupo = ccGrupos.FirstOrDefault(x => x.catalogoCC_id == item.id);
                        if (ccGrupo != null)
                        {
                            item.grupoID = ccGrupo.grupo_id;
                        }
                    }

                    data.AddRange(ccs);
                }

                resultado.Add(SUCCESS, true);
                resultado.Add("data", data);
            }
            catch (Exception e)
            {
                resultado.Add(SUCCESS, false);
                resultado.Add(ERROR, "Ocurrió un error interno al intentar cargar los cc del grupo seleccionado.");
                return resultado;
            }

            return resultado;
        }
        public Dictionary<string, object> GuardarCCGrupo(int ccID, int? grupoID)
        {
            var resultado = new Dictionary<string, object>();
            try
            {
                if (vSesiones.sesionEmpresaActual == (int)EmpresaEnum.Construplan || vSesiones.sesionEmpresaActual == (int)EmpresaEnum.GCPLAN)
                {
                    var obj = _context.tblP_CC.FirstOrDefault(x => x.id == ccID);
                    obj.grupoID = grupoID;
                    _context.SaveChanges();
                }
                else
                {
                    var listaRegistrosAnteriores = _context.tblC_Nom_CatalogoCCtblFA_Grupos.Where(x => x.catalogoCC_id == ccID).ToList();

                    foreach (var reg in listaRegistrosAnteriores)
                    {
                        reg.registroActivo = false;
                        _context.SaveChanges();
                    }

                    _context.tblC_Nom_CatalogoCCtblFA_Grupos.Add(new tblC_Nom_CatalogoCCtblFA_Grupos
                    {
                        catalogoCC_id = ccID,
                        grupo_id = (int)grupoID,
                        registroActivo = true
                    });
                    _context.SaveChanges();
                }

                resultado.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                resultado.Add(SUCCESS, false);
                resultado.Add(ERROR, "Ocurrió un error interno al intentar cargar los cc del grupo seleccionado.");
                return resultado;
            }

            return resultado;
        }
        public Dictionary<string, object> getTblGrupo()
        {
            var resultado = new Dictionary<string, object>();
            var data = new List<dynamic>();
            try
            {
                if (vSesiones.sesionEmpresaActual == (int)EmpresaEnum.Construplan || vSesiones.sesionEmpresaActual == (int)EmpresaEnum.GCPLAN)
                {
                    var temp = _context.tblFA_Grupos.Where(x => x.estatus).Select(x => new
                    {
                        id = x.id,
                        cantidad = (_context.tblP_CC.Where(y => y.grupoID == x.id).Count()),
                        grupo = x.descripcion
                    }).ToList();

                    data.AddRange(temp);
                }
                else
                {
                    var temp = _context.tblFA_Grupos.Where(x => x.estatus).Select(x => new
                    {
                        id = x.id,
                        cantidad = _context.tblC_Nom_CatalogoCCtblFA_Grupos.Where(y => y.registroActivo && y.grupo_id == x.id).Count(),
                        grupo = x.descripcion
                    }).ToList();

                    data.AddRange(temp);
                }

                resultado.Add("data", data);
                resultado.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                resultado.Add(SUCCESS, false);
                resultado.Add(ERROR, "Ocurrió un error interno al intentar cargar los cc del grupo seleccionado.");
                return resultado;
            }

            return resultado;
        }
        public Dictionary<string, object> delGrupo(int id)
        {
            var resultado = new Dictionary<string, object>();
            try
            {
                var obj = _context.tblFA_Grupos.FirstOrDefault(x => x.id == id);
                obj.estatus = false;
                _context.SaveChanges();

                resultado.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                resultado.Add(SUCCESS, false);
                resultado.Add(ERROR, "Ocurrió un error interno al intentar desactivar el grupo indicado.");
                return resultado;
            }

            return resultado;
        }
        public Dictionary<string, object> GuardarGrupo(string grupo)
        {
            var resultado = new Dictionary<string, object>();
            try
            {
                var obj = new tblFA_Grupos();
                obj.descripcion = grupo;
                obj.estatus = true;
                _context.tblFA_Grupos.Add(obj);
                _context.SaveChanges();

                resultado.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                resultado.Add(SUCCESS, false);
                resultado.Add(ERROR, "Ocurrió un error interno al intentar guardar el grupo indicado.");
                return resultado;
            }

            return resultado;
        }
        public Dictionary<string, object> delPuesto(int id)
        {
            var resultado = new Dictionary<string, object>();
            try
            {

                var obj2 = _context.tblFA_Empleado.Where(x => x.conceptoID == id);
                _context.tblFA_Empleado.RemoveRange(obj2);
                _context.SaveChanges();

                var obj = _context.tblFA_ConceptoPlantilla.FirstOrDefault(x => x.id == id);
                _context.tblFA_ConceptoPlantilla.Remove(obj);
                _context.SaveChanges();


                resultado.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                resultado.Add(SUCCESS, false);
                resultado.Add(ERROR, "Ocurrió un error interno al intentar desactivar el grupo indicado.");
                return resultado;
            }

            return resultado;
        }
        #endregion
    }
}