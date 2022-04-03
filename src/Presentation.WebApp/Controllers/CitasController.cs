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

        public CitasController(IConfiguration configuration, UserManager<IdentityUser> userManager, IServicioCitas servicioCitas, IServicioPaciente servicioPaciente, IServicioCatalogos servicioCatalogos, IServicioSmtpCorreo servicioSmtpCorreo, IServicioReporte servicioReporte)
        {
            _servicioCatalogos = servicioCatalogos;
            _servicioCitas = servicioCitas;
            _servicioPaciente = servicioPaciente;
            _servicioSmtpCorreo = servicioSmtpCorreo;
            _servicioReporte = servicioReporte;

            var u = userManager.Users.ToList();
        }

        [HttpGet]
        public IActionResult Index()
        {
            try
            {
                Catalogos();
                return View();
            }
            catch (Exception e)
            {

                return View("Error",new ErrorViewModel());
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
                //var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();
                //var sortColumnDirection = Request.Form["order[0][dir]"].FirstOrDefault();
                var searchValue = Request.Form["search[value]"].FirstOrDefault();
                int tamanioPagina = length != null ? Convert.ToInt32(length) : 0;
                int pagina = inicio != null ? (int)(Convert.ToInt32(inicio)) : 1;
                int totalregistros = 0;

                totalregistros = _servicioCitas.ConsultarCitas().Count();

                if (inicio == "1")
                    pagina =0;

                var data = _servicioCitas.ConsultarCitasPaginadas(pagina, tamanioPagina, searchValue,true);

                var resultado = new { draw = draw, recordsFiltered = totalregistros, recordsTotal = totalregistros, data = data };
                return Ok(resultado);
            }
            catch (Exception ex)
            {

                return StatusCode(500);
            }
        }

      
        [HttpPost]
        public IActionResult DetalleCita(Guid id) => Json(_servicioCitas.ConsultarDetalleCita(id));

        [HttpGet]
        public IActionResult GenerarReporte()
        {
            var archivo = new ViewAsPdf("ReporteCitas", _servicioReporte.ConsultarReporteCitas()) { PageMargins = { Left = 10, Bottom = 10, Right = 10, Top = 10 } };
            return File((archivo.BuildFile(ControllerContext)).Result.ToArray(), "application/pdf", Guid.NewGuid().ToString() + ".pdf");
        }

        public IActionResult CrearCita(Guid id)
        {
            Catalogos();
            return View();
        }


        [HttpPost]
        public IActionResult GuardarCita([FromForm] Cita cita, string pacienteId)
        {
            Catalogos();
            if (!ModelState.IsValid)
                return View();


            _servicioCitas.CrearNuevaCita(Domain.Cita.Create(cita.PacienteId, cita.Fecha, cita.idEstaus, cita.idArea, cita.Observaciones));
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult EditarCita(Guid id, Cita dataCita)
        {
            try
            {
                var citaEdit = dataCita;

                _servicioCitas.EditarCita(id, Domain.Cita.Create(citaEdit.PacienteId, citaEdit.Fecha, citaEdit.idEstaus, citaEdit.idArea, citaEdit.Observaciones));

                return View(citaEdit);
            }
            catch (Exception)
            {

                return StatusCode(500);
            }
            
        }

        [HttpGet]
        public IActionResult Error()
        {
            return View("Error");
        }


        [HttpPost]
        public IActionResult CancelarCita(Guid id) => Json(new { seCancelo = _servicioCitas.EliminarCita(id) });


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
