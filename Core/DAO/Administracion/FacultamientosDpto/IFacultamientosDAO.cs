using Core.DTO.Administracion.Facultamiento;
using Core.DTO.Principal.Generales;
using System;
using System.Collections.Generic;

namespace Core.DAO.Administracion.FacultamientosDpto
{
    public interface IFacultamientosDAO
    {
        #region metodos catalogo
        /// <summary>
        /// Obtiene todos los departamentos que tenga el atributo esDepartamento activo.
        /// </summary>
        /// <returns>Retorna una lista de los departamentos encontrados.</returns>
        List<ComboDTO> ObtenerDepartamentos();

        /// <summary>
        /// Crea un registro de una nueva plantilla de facultamientos.
        /// </summary>
        /// <param name="titulo">Título de la plantilla.</param>
        /// <param name="listaDepartamentos">Lista de departamentos a la que pertenecerá la plantilla.</param>
        /// <param name="listaConceptos">Lista de conceptos que tendrá la plantilla.</param>
        /// <returns>Retorna un diccionario con la variable SUCCESS true en caso de haber realizado la operación con éxito.</returns>
        Dictionary<string, object> GuardarPlantilla(string titulo, List<int> listaDepartamentos, List<ConceptoDTO> listaConceptos);

        /// <summary>
        /// Obtiene toda la lista de plantillas de facultamientos que pertenezcan al departamento indicado.
        /// </summary>
        /// <param name="departamentoID">Identificador del departamento.</param>
        /// <returns>Retorna una lista de objetos tipo CatalogoPlantillaFaDTO dentro de un diccionario.</returns>
        Dictionary<string, object> ObtenerCatalogo(int departamentoID);

        /// <summary>
        /// Obtiene una plantilla de facultamientos.
        /// </summary>
        /// <param name="plantillaID">Identificador de la plantilla.</param>
        /// <returns>Retorna una lista de objetos tipo PlantillaFacultamientoDTO dentro de un diccionario.</returns>
        Dictionary<string, object> ObtenerPlantilla(int plantillaID);

        /// <summary>
        /// Actualiza el registro de la plantilla especificada.
        /// </summary>
        /// <param name="nuevoTitulo">Nuevo título de la plantilla.</param>
        /// <param name="nuevosDepartamentos">Departamentos agregados a la plantilla.</param>
        /// <param name="nuevosConceptos">Nueva lista de conceptos de la plantilla.</param>
        /// <param name="plantillaID">Identificador de la plantilla.</param>
        /// <returns>Retorna un diccionario con la variable SUCCESS true en caso de haber realizado la operación con éxito.</returns>
        Dictionary<string, object> ActualizarPlantilla(string nuevoTitulo, List<int> nuevosDepartamentos, List<ConceptoDTO> nuevosConceptos, int plantillaID, bool esActualizar);
        #endregion

        #region metodos asignacion
        /// <summary>
        /// Obtiene todos los Centros de Costos que tengan asigando algún departamento, 
        /// que a su vez ese departamento tengo alguna plantilla de facultamiento
        /// y que no tenga asignado algún paquete de facultamientos.
        /// </summary>
        /// <returns>Retorna una lista con los Centro de Costos específicados.</returns>
        List<ComboDTO> ObtenerCentrosCostos();

        /// <summary>
        /// Obtiene todos los Centros de Costos que tengan algún paquete de facultamientos asignado.
        /// </summary>
        /// <returns>Retorna una lista con los Centros de Costos encontrados.</returns>
        List<ComboDTO> ObtenerObras();

        /// <summary>
        /// Obtiene todas las plantillas de facultamientos para el Centro de Costos especificado.
        /// </summary>
        /// <param name="centroCostosID">Identificador del Centro de Costos.</param>
        /// <returns>Retorna una lista de PlantillaFacultamientoDTO en caso de encontrar alguno.</returns>
        Dictionary<string, object> CargarPlantillasCC(int centroCostosID);

        /// <summary>
        /// Guarda un registro sobre las asignaciones del paquete de facultamientos indicado.
        /// </summary>
        /// <param name="centroCostosID">Identificador del Centro de Costos al que pertenece el paquete de facultamientos.</param>
        /// <param name="listaEmpleados">Lista de empleados específicados en la asignación del paquete.</param>
        /// <param name="listaAutorizantes">Lista de empleados que autorizarán este paquete de facultamientos.</param>
        /// <param name="todoCompleto">Variable booleana para saber si el paquete quedará como Editando o Pendiente por Autorizar.</param>
        /// <returns></returns>
        Dictionary<string, object> AsignarFacultamientos(int centroCostosID, List<FacultamientoDTO> listaFacultamientos, List<EmpleadoAutorizanteDTO> listaAutorizantes, bool todoCompleto);

        /// <summary>
        /// Obtiene todos los paquetes de facultamientos que cumplan con los parámetros específicados.
        /// </summary>
        /// <param name="departamentoID">Identificador del departamento al que pertenece el paquete.</param>
        /// <param name="centroCostosID">Identificador del Centro de Costos al que pertenece el paquete.</param>
        /// <param name="estado">Estado de autorización del paquete.</param>
        /// <returns>Retorna una diccionario con la lista de paquetes encontrados. En caso de error, retornará un mensaje </returns>
        Dictionary<string, object> ObtenerPaquetes(int departamentoID, int centroCostosID, int estado);

        /// <summary>
        /// Obtiene el paquete de facultamientos y toda su información.
        /// </summary>
        /// <param name="paqueteID">Identificador del paquete.</param>
        /// <param name="esReporte">Sí el método es llamado para cargar información de un reporte, mandar True</param>
        /// <returns>Retorna una instancia de tipo PaqueteFaDTO en caso de encontrar alguno.</returns>
        Dictionary<string, object> ObtenerPaqueteActualizar(int paqueteID, bool esReporte = false);

        /// <summary>
        /// Actualiza la información de los facultamientos del algún paquete especificado.
        /// </summary>
        /// <param name="paqueteID">Identificador del paquete por actualizar.</param>
        /// <param name="listaEmpleados">Lista de empleados pertenecientes a los facultamientos.</param>
        /// <param name="listaAutorizantes">Lista de empleados que autorizarán el paquete.</param>
        /// <param name="todoCompleto">Variable booleana para saber si el paquete quedará como Editando o Pendiente por Autorizar.</param>
        /// <returns>Retorna un diccionario con la variable SUCCESS true en caso de haber realizado la operación con éxito.</returns>
        Dictionary<string, object> ActualizarFacultamientos(int paqueteID, List<FacultamientoDTO> listaFacultamientos, List<EmpleadoAutorizanteDTO> listaAutorizantes, bool todoCompleto);
        #endregion

        #region metodos autorizacion
        /// <summary>
        /// Obtiene toda la lista de paquetes donde el usuario logueado tenga pendiente alguna autorización.
        /// </summary>
        /// <returns>Retorna un diccionario con la lista de paquetes. En caso de error, retornará un mensaje de la causa.</returns>
        Dictionary<string, object> ObtenerPaquetesPorAutorizar();

        /// <summary>
        /// Obtiene la lista de autorizantes del paquete seleccionado.
        /// </summary>
        /// <param name="paqueteID">Identificador del paquete.</param>
        /// <returns>Retorna un diccionario con la lista de autorizantes. En caso de error, retornará un mensaje de la causa.</returns>
        Dictionary<string, object> ObtenerAutorizantes(int paqueteID);

        /// <summary>
        /// Autoriza el paquete seleccionado del usuario logueado.
        /// </summary>
        /// <param name="paqueteID">Identificador del paquete.</param>
        /// <returns>Retorna un diccionario con la variable SUCCESS true en caso de haber realizado la operación con éxito.</returns>
        Dictionary<string, object> AutorizarPaquete(int paqueteID);

        /// <summary>
        /// Rechaza el paquete seleccionado del usuario logueado.
        /// </summary>
        /// <param name="paqueteID">Identificador del paquete.</param>
        /// <param name="comentario">Mensaje de justificación del rechazo del facultamiento.</param>
        /// <returns>Retorna un diccionario con la variable SUCCESS true en caso de haber realizado la operación con éxito.</returns>
        Dictionary<string, object> RechazarPaquete(int paqueteID, string comentario);

        /// <summary>
        /// Envia un correo de autorización de facultamiento al autorizante indicado.
        /// </summary>
        /// <param name="paqueteID">Identificador del paquete de facultamientos.</param>
        /// <param name="ordenVoBo">Orden del autorizante al que se le enviará el correo</param>
        /// <param name="pdf">Archivo PDF donde viene el reporte del facultamiento</param>
        /// <returns>Retorna un diccionario con la variable SUCCESS true en caso de haber realizado la operación con éxito.</returns>
        Dictionary<string, object> EnviarCorreoAutorizacion(int paqueteID, int ordenVoBo, List<Byte[]> pdf);

        /// <summary>
        /// Envía un correo a todos los autorizantes del paquete de facultamientos indicando que el paquete quedó autorizado.
        /// </summary>
        /// <param name="paqueteID">Identificador del paquete.</param>
        /// <param name="pdf">Archivo PDF donde viene el reporte del facultamiento.</param>
        /// <returns>Retorna un diccionario con la variable SUCCESS true en caso de haber realizado la operación con éxito.</returns>
        Dictionary<string, object> EnviarCorreoAutorizacionCompleta(int paqueteID, List<Byte[]> pdf);

        /// <summary>
        /// Envía un correo a todos los autorizantes del paquete de facultamientos indicando que se rechazó y su causa.
        /// </summary>
        /// <param name="paqueteID">Identificador del paquete.</param>
        /// <param name="pdf">Archivo PDF donde viene el reporte del facultamiento.</param>
        /// <param name="comentario">Mensaje de rechazo del paquete.</param>
        /// <returns>Retorna un diccionario con la variable SUCCESS true en caso de haber realizado la operación con éxito.</returns>
        Dictionary<string, object> EnviarCorreoRechazo(int paqueteID, string comentario, List<Byte[]> pdf);
        #endregion

        #region metodos historico
        /// <summary>
        /// Obtiene toda la lista de paquetes dado un Centro de Costos.
        /// </summary>
        /// <param name="ccID">Identificador del Centro ce Costos.</param>
        /// <returns>Retorna un diccionario con la variable SUCCESS true en caso de haber realizado la operación con éxito.</returns>
        Dictionary<string, object> ObtenerHistorico(int ccID);
        #endregion

        #region metodos por empleado
        /// <summary>
        /// Obtiene todos los facultamientos en los que participa algún empleado.
        /// </summary>
        /// <param name="claveEmpleado">Clave del empleado a buscar.</param>
        /// <param name="centroCostosID">Identificador del Centro de Costos.</param>
        /// <returns>Retorna una diccionario con la lista de facultamientos encontrados. En caso de error, retornará un mensaje </returns>
        Dictionary<string, object> ObtenerFacultamientosEmpleado(int claveEmpleado, int centroCostosID);

        /// <summary>
        /// Obtiene un facultamiento específico en base a su ID.
        /// </summary>
        /// <param name="facultamientoID">Identificador del facultamiento.</param>
        /// <returns>Retorna una diccionario con el facultamiento encontrado. En caso de error, retornará un mensaje</returns>
        Dictionary<string, object> ObtenerFacultamiento(int facultamientoID);

        /// <summary>
        /// Obtiene el nombre completo de un empleado en la bd EnKontrol en base a su clave de empleado.
        /// </summary>
        /// <param name="claveEmpleado">Identificador del empleado.</param>
        /// <returns>Retorna el nombre completo en caso de éxito. Si no, retorna una cadena vacía.</returns>
        string ObtenerNombreEmpleadoPorClave(int claveEmpleado);
        #endregion
        #region metodos catalogo grupos
        /// <summary>
        /// Obtiene todos los CC y su grupo asignado.
        /// </summary>
        /// <param name="grupoID">Clave del cc por grupo.</param>

        /// <returns>Retorna una diccionario con la lista de cc y el grupo que tiene asignado </returns>
        Dictionary<string, object> ObtenerCCGrupo(int grupoID);
        Dictionary<string, object> GuardarCCGrupo(int ccID,int? grupoID);
        Dictionary<string, object> getTblGrupo();
        Dictionary<string, object> delGrupo(int id);
        Dictionary<string, object> GuardarGrupo(string grupo);
        Dictionary<string, object> delPuesto(int id);
        
        #endregion
    }
}
