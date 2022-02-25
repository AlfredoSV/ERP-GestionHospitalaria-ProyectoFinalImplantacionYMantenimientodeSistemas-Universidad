using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

using Domain;
using Infrastructure;
using Application;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using Application.IServicios;


namespace Presentation.WebApp.Controllers
{
    [Authorize]
    public class DoctoresController : Controller
    {

        private readonly DoctoresDbContext _DoctoresDbContext;
        private readonly CatalogosDbContext _catalogosDbContext;
        private readonly IFileConvertService _fileConvertService;
        public DoctoresController(IConfiguration configuration, IFileConvertService fileConvertService)
        {
            _catalogosDbContext = new CatalogosDbContext(configuration.GetConnectionString("DefaultConnection"));
            _DoctoresDbContext = new DoctoresDbContext(configuration.GetConnectionString("DefaultConnection"));
            _fileConvertService = fileConvertService;
        }

        [Authorize(Roles = "Admin,Doctor,Enfermera")]
        public IActionResult Index()
        {

            return View(_DoctoresDbContext.List());
        }
        [HttpPost]
        public IActionResult ListarDoctores()
        {

            return Json(_DoctoresDbContext.List());
        }


        [HttpPost]
        public IActionResult Details(Guid id)
        {
            var doctor = _DoctoresDbContext.Details(id);
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
                _DoctoresDbContext.Create(Doctor.Create(data.Nombre, data.Apellidop, data.ApellidoM, data.Edad, data.Direccion, data.Telefono, data.Correo, data.Especialidad, _fileConvertService.ConvertToBase64(data.Foto.OpenReadStream()), data.IdArea, data.IdEstadoCivil));
                return RedirectToAction("Index");
            }
            return View("Create", data);
        }

        [Authorize(Roles = "Admin,Doctor")]
        [HttpGet]
        public IActionResult Edit(Guid id)
        {
            var doctores = _DoctoresDbContext.List();
            Catalogos();
            return View("Edit", doctores);
        }

        [HttpPost]
        public IActionResult Edit(Presentation.WebApp.Models.Doctor data)
        {
            var fotografia = data.Foto == null ? _DoctoresDbContext.Details(data.Id).Foto : _fileConvertService.ConvertToBase64(data.Foto.OpenReadStream());

            var editarPro = _DoctoresDbContext.Edit(Domain.Doctor.Create(data.Id, data.Nombre, data.Apellidop, data.ApellidoM, data.Edad, data.Direccion, data.Telefono, data.Correo, data.Especialidad, fotografia, data.IdArea, data.IdEstadoCivil));
            return Json(new { editarPro });
        }

        public IActionResult Delete(Guid id)
        {
            var doctorEliminado = _DoctoresDbContext.Delete(id);
            return Json(new { doctorEliminado });
        }

        private void Catalogos()
        {
            List<SelectListItem> listaEstadosCiviles = new List<SelectListItem>();
            List<SelectListItem> listaAreas = new List<SelectListItem>();
            foreach (var area in _catalogosDbContext.ListCatMed())
            {
                listaAreas.Add(new SelectListItem { Value = area.Id_Area.ToString(), Text = area.Nombre_Area });
            }
            ViewBag.areas = listaAreas;

            foreach (var estado in _catalogosDbContext.ListCatEstadoCivil())
            {
                listaEstadosCiviles.Add(new SelectListItem { Value = estado.IdEstado.ToString(), Text = estado.Nombre_Estado });
            }
            ViewBag.estadosCiviles = listaEstadosCiviles;
        }
    }
}
