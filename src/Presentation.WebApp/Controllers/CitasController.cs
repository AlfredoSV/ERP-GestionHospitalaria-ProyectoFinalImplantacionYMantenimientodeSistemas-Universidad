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

namespace Presentation.WebApp.Controllers
{
    [Authorize(Roles = "Admin,Doctor,Enfermera")]
    public class CitasController : Controller
    {
        private readonly IServicioCitas _servicioCitas;
        private readonly PacientesDbContext _pacientesDbContext;
        private readonly CatalogosDbContext _catalogosDbContext;
        private readonly SmtpClientEmailService _email;

        public CitasController(IConfiguration configuration, UserManager<IdentityUser> userManager, IServicioCitas servicioCitas)
        {
            _catalogosDbContext = new CatalogosDbContext(configuration.GetConnectionString("DefaultConnection"));
            _servicioCitas = servicioCitas;
            _pacientesDbContext = new PacientesDbContext(configuration.GetConnectionString("DefaultConnection"));
            var config = configuration.GetSection("Smtp");
            _email = new SmtpClientEmailService(config["Displayname"], config["Address"], config["Host"], int.Parse(config["Port"]), config["Username"], config["Password"]);

        }

        [HttpGet]
        public IActionResult Index() => View();



        [HttpPost]
        public IActionResult ListarCitas() => Json(_servicioCitas.ConsultarCitas());


        [HttpPost]
        public IActionResult Details(Guid id) => Json(_servicioCitas.ConsultarDetalleCita(id));



        public IActionResult Create(Guid id)
        {

            Catalogos();

            return View();
        }


        [HttpPost]
        public IActionResult Create([FromForm] Cita data, string pacienteId)
        {
            Catalogos();
            if (!ModelState.IsValid)
                return View();


            _servicioCitas.CrearNuevaCita(Domain.Cita.Create(data.PacienteId, data.Fecha, data.idEstaus, data.idArea, data.Observaciones));
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult EditarCita(Guid id, Cita dataCita)
        {
            var citaEdit = dataCita;

            _servicioCitas.EditarCita(id, Domain.Cita.Create(citaEdit.PacienteId, citaEdit.Fecha, citaEdit.idEstaus, citaEdit.idArea, citaEdit.Observaciones));

            return View(citaEdit);
        }


        [HttpPost]
        public IActionResult CancelarCita(Guid id) => Json(new { seCancelo = _servicioCitas.EliminarCita(id) });


        [HttpGet]
        public IActionResult CatalogoEstatus() => Json(_catalogosDbContext.ListCatEst());


        [HttpGet]
        public IActionResult CatalogoAreas() => Json(_catalogosDbContext.ListCatMed());

        private void Catalogos()
        {
            List<SelectListItem> listaNombres = new List<SelectListItem>();
            List<SelectListItem> listaEstatus = new List<SelectListItem>();
            List<SelectListItem> listaAreas = new List<SelectListItem>();
            foreach (var paciente in _pacientesDbContext.List())
            {
                listaNombres.Add(new SelectListItem { Value = paciente.Id.ToString(), Text = ("Expediente:" + " " + paciente.Expediente + " - " + paciente.Nombre) });
            }
            ViewBag.pacientes = listaNombres;
            foreach (var estatus in _catalogosDbContext.ListCatEst())
            {
                listaEstatus.Add(new SelectListItem { Value = estatus.IdEstatus.ToString(), Text = estatus.Nombre_Estatus });
            }
            ViewBag.estatus = listaEstatus;
            foreach (var area in _catalogosDbContext.ListCatMed())
            {
                listaAreas.Add(new SelectListItem { Value = area.Id_Area.ToString(), Text = area.Nombre_Area });
            }
            ViewBag.areas = listaAreas;
        }
    }
}
