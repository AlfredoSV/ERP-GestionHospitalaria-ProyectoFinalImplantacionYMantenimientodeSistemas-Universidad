using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using Presentation.WebApp.Models;
using Infrastructure;

using System.Linq;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;

using Microsoft.AspNetCore.Identity;
using Application.IServicios;
using Rotativa.AspNetCore;

namespace Presentation.WebApp.Controllers
{
    [Authorize(Roles = "Admin,Doctor,Enfermera")]
    public class CitasController : Controller
    {
        private readonly IServicioCitas _servicioCitas;
        private readonly IServicioPaciente _servicioPaciente;
        private readonly IServicioCatalogos _servicioCatalogos;
        private readonly IServicioSmtpCorreo _servicioSmtpCorreo;
        private readonly IServicioReporte _servicioReporte;

        public CitasController(IConfiguration configuration, UserManager<IdentityUser> userManager,
            IServicioCitas servicioCitas, IServicioPaciente servicioPaciente, 
            IServicioCatalogos servicioCatalogos, IServicioSmtpCorreo servicioSmtpCorreo, 
            IServicioReporte servicioReporte)
        {
            _servicioCatalogos = servicioCatalogos;
            _servicioCitas = servicioCitas;
            _servicioPaciente = servicioPaciente;
            _servicioSmtpCorreo = servicioSmtpCorreo;
            _servicioReporte = servicioReporte;
        }

        [HttpGet]
        public IActionResult Index()
        {
            try
            {              
                Catalogos();
                return View();
            }
            catch (Exception ex)
            {
                return View("Error", new ErrorViewModel() { Message = ex.Message });
            }

        }

        [HttpPost]
        public IActionResult ListarCitas()
        {
            try
            {
                var draw = Request.Form["draw"].FirstOrDefault();
                var inicio = Request.Form["start"].FirstOrDefault();
                var length = Request.Form["length"].FirstOrDefault();
                var searchValue = Request.Form["search[value]"].FirstOrDefault();
                int tamanioPagina = length != null ? Convert.ToInt32(length) : 0;
                int pagina = inicio != null ? (int)(Convert.ToInt32(inicio)) : 1;
                int totalregistros = 0;

                totalregistros = _servicioCitas.ConsultarCitas().Count();

                if (inicio == "1")
                    pagina = 0;

                var data = _servicioCitas.ConsultarCitasPaginadas(pagina, tamanioPagina, searchValue, true);

                var resultado = new { draw = draw, recordsFiltered = totalregistros, recordsTotal = totalregistros, data = data };
                return Ok(resultado);
            }
            catch (Exception ex)
            {

                return StatusCode(500,ex);
            }
        }


        [HttpPost]
        public IActionResult DetalleCita(Guid id)
        {
            try { 
                return PartialView("_DetalleCita", _servicioCitas.ConsultarDetalleCita(id));
            }
            catch (Exception ex)
            {
                return StatusCode(500,ex);
            }           

        }

        [HttpPost]
        public IActionResult DetalleCitaEditar(Guid id)
        {
            try
            {
                Catalogos();

                Domain.Cita citaServicio = _servicioCitas.ConsultarDetalleCita(id);

                CitaModelViewEditar cita = new CitaModelViewEditar()
                {
                    Id = citaServicio.Id,
                    NombrePaciente = citaServicio.NombrePaciente,
                    Fecha = citaServicio.Fecha,
                    IdArea = citaServicio.IdArea,
                    IdEstaus = citaServicio.IdEstaus,
                    Observaciones = citaServicio.Observaciones

                };

                return PartialView("_EditarCita", cita);
            }
            catch (Exception ex)
            {

                return StatusCode(500,ex);
            }
     

        }

        [HttpGet]
        public IActionResult GenerarReporte()
        {
            try
            {
                ViewAsPdf archivo = new ViewAsPdf("ReporteCitas", _servicioReporte.ConsultarReporteCitas())
                { PageMargins = { Left = 10, Bottom = 10, Right = 10, Top = 10 } };
                return File((archivo.BuildFile(ControllerContext)).Result.ToArray(), "application/pdf", 
                    Guid.NewGuid().ToString() + ".pdf");

            }
            catch (Exception ex)
            {
                return View("Error",new ErrorViewModel() { Message = ex.Message});
            }
        }

        [HttpGet]
        public IActionResult CrearCita(Guid id)
        {
            try
            {
                Catalogos();
                return View();
            }
            catch (Exception ex)
            {

                return View("Error", new ErrorViewModel() { Message = ex.Message });
            }
            
        }


        [HttpPost]
        public IActionResult GuardarCita([FromForm] Cita cita, string pacienteId)
        {
            try
            {
                Catalogos();
                if (!ModelState.IsValid)
                    return View(cita);


                _servicioCitas.CrearNuevaCita(Domain.Cita.Create(cita.PacienteId, cita.Fecha, cita.idEstaus, cita.idArea, cita.Observaciones));
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {

                return View("Error", new ErrorViewModel() { Message = ex.Message });
            }
            
        }

        [HttpPost]
        public IActionResult EditarCita([FromForm] CitaModelViewEditar dataCita)
        {
            try
            {
                CitaModelViewEditar citaEdit = dataCita;
                if (ModelState.IsValid)
                {
                    _servicioCitas.EditarCita(Domain.Cita.Create(citaEdit.PacienteId, 
                        citaEdit.Fecha, citaEdit.IdEstaus, citaEdit.IdArea, citaEdit.Observaciones));

                    return StatusCode(204);
                }

                Catalogos();

                return PartialView("_EditarCita", dataCita);

            }
            catch (Exception ex)
            {
                return StatusCode(500,ex);
            }

        }

      


        [HttpDelete]
        public IActionResult CancelarCita(Guid id) {

            try
            {            
                _servicioCitas.EliminarCita(id);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500,ex);
            }  
        }
        

        private void Catalogos()
        {
            List<SelectListItem> listaNombres = new List<SelectListItem>();
            List<SelectListItem> listaEstatus = new List<SelectListItem>();
            List<SelectListItem> listaAreas = new List<SelectListItem>();

            foreach (var paciente in _servicioPaciente.ConsultarPacientesBD())
            {
                listaNombres.Add(new SelectListItem { Value = paciente.Id.ToString(), Text = ("Expediente:" + " " + paciente.Expediente + " - " + paciente.Nombre) });
            }
            ViewBag.pacientes = listaNombres;
            foreach (var estatus in _servicioCatalogos.ConsultarCatalogoEstatusCitas())
            {
                listaEstatus.Add(new SelectListItem { Value = estatus.IdEstatus.ToString(), Text = estatus.Nombre_Estatus });
            }
            ViewBag.estatus = listaEstatus;
            foreach (var area in _servicioCatalogos.ConsultarCatalogoAreaMedica())
            {
                listaAreas.Add(new SelectListItem { Value = area.Id_Area.ToString(), Text = area.Nombre_Area });
            }
            ViewBag.areas = listaAreas;
        }
    }
}
