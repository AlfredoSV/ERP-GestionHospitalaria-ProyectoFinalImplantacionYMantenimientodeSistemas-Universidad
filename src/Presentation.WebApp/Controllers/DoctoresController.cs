using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using Application.IServicios;


namespace Presentation.WebApp.Controllers
{
    [Authorize]
    public class DoctoresController : Controller
    {

        private readonly IServicioDoctor _servicioDoctor;
        private readonly IServicioCatalogos _servicioCatalogos;
        private readonly IFileConvertService _fileConvertService;
        public DoctoresController(IConfiguration configuration, IFileConvertService fileConvertService, IServicioDoctor servicioDoctor,IServicioCatalogos servicioCatalogos)
        {
            _servicioCatalogos = servicioCatalogos;
            _servicioDoctor = servicioDoctor;
            _fileConvertService = fileConvertService;
        }

        [Authorize(Roles = "Admin,Doctor,Enfermera")]
        public IActionResult Index()
        {

            return View(_servicioDoctor.ConsultarDoctores());
        }
        [HttpPost]
        public IActionResult ListarDoctores()
        {

            return Json(_servicioDoctor.ConsultarDoctores());
        }


        [HttpPost]
        public IActionResult Details(Guid id)
        {
            var doctor = _servicioDoctor.ConsultarDetalleDoctor(id);
            return Json(doctor);
        }

        [Authorize(Roles = "Admin,Doctor")]
        [HttpGet]
        public IActionResult Create()
        {
            Catalogos();
            return View();
        }
        [HttpPost]
        public IActionResult Create(Presentation.WebApp.Models.Doctor data)
        {
            if (ModelState.IsValid)
            {
                _servicioDoctor.RegistrarDoctor(Doctor.Create(data.Nombre, data.Apellidop, data.ApellidoM, data.Edad, data.Direccion, data.Telefono, data.Correo, data.Especialidad, _fileConvertService.ConvertToBase64(data.Foto.OpenReadStream()), data.IdArea, data.IdEstadoCivil));
                return RedirectToAction("Index");
            }
            return View("Create", data);
        }

        [Authorize(Roles = "Admin,Doctor")]
        [HttpGet]
        public IActionResult Edit(Guid id)
        {
            var doctores = _servicioDoctor.ConsultarDoctores();
            Catalogos();
            return View("Edit", doctores);
        }

        [HttpPost]
        public IActionResult Edit(Presentation.WebApp.Models.Doctor data)
        {
            var fotografia = data.Foto == null ? _servicioDoctor.ConsultarDetalleDoctor(data.Id).Foto : _fileConvertService.ConvertToBase64(data.Foto.OpenReadStream());

            var editarPro = _servicioDoctor.EditarDoctor(Domain.Doctor.Create(data.Id, data.Nombre, data.Apellidop, data.ApellidoM, data.Edad, data.Direccion, data.Telefono, data.Correo, data.Especialidad, fotografia, data.IdArea, data.IdEstadoCivil));
            return Json(new { editarPro });
        }

        public IActionResult Delete(Guid id)
        {
            var doctorEliminado = _servicioDoctor.EliminarDoctor(id);
            return Json(new { doctorEliminado });
        }

        private void Catalogos()
        {
            List<SelectListItem> listaEstadosCiviles = new List<SelectListItem>();
            List<SelectListItem> listaAreas = new List<SelectListItem>();
            foreach (var area in _servicioCatalogos.ConsultarCatalogoAreaMedica())
            {
                listaAreas.Add(new SelectListItem { Value = area.Id_Area.ToString(), Text = area.Nombre_Area });
            }
            ViewBag.areas = listaAreas;

            foreach (var estado in _servicioCatalogos.ConsultarCatalogoEstadoCivil())
            {
                listaEstadosCiviles.Add(new SelectListItem { Value = estado.IdEstado.ToString(), Text = estado.Nombre_Estado });
            }
            ViewBag.estadosCiviles = listaEstadosCiviles;
        }
    }
}
