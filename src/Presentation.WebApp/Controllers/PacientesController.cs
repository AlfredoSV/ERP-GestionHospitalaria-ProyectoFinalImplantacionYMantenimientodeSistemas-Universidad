using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Http;

using Presentation.WebApp.Models;
using Infrastructure;
using Application;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using Domain;
using Application.IServicios;

namespace Presentation.WebApp.Controllers
{
    [Authorize]
    public class PacientesController : Controller
    {

        private readonly PacientesDbContext _pacientesDbContext;
        private readonly CitasDbContext _citasDbContext;
        private readonly CatalogosDbContext _catalogosDbContext;
        private readonly IFileConvertService _fileConvertService;

        public PacientesController(IConfiguration configuration, IFileConvertService fileConvertService)
        {
            _catalogosDbContext = new CatalogosDbContext(configuration.GetConnectionString("DefaultConnection"));
            _citasDbContext = new CitasDbContext(configuration.GetConnectionString("DefaultConnection"));
            _pacientesDbContext = new PacientesDbContext(configuration.GetConnectionString("DefaultConnection"));
            _fileConvertService = fileConvertService;
        }

        [Authorize(Roles = "Admin,Doctor,Enfermera")]
        [HttpGet]
        public IActionResult Index()
        {
            var pacientes = _pacientesDbContext.List();
            return View(pacientes);
        }

        [HttpPost]
        public IActionResult ListarPacientes()
        {
            var pacientes = _pacientesDbContext.List();
            return Json(pacientes);
        }

        [HttpPost]
        public IActionResult Details(Guid id)
        {
            var paciente = _pacientesDbContext.Details(id);
            return Json(paciente);
        }

        [Authorize(Roles = "Admin,Doctor,Enfermera")]
        public IActionResult Create()
        {
            Catalogos();
            return View();
        }


        [HttpPost]
        public IActionResult Create(Models.Paciente data)
        {


            if (!ModelState.IsValid)
            {
                Catalogos();
                return View();
            }


            _pacientesDbContext.Create(Domain.Paciente.Create(data.Nombre, data.ApellidoP, data.ApellidoM, data.Edad, data.Correo, data.TelefonoM,
                data.TelefonoT, data.TelefonoD, data.Curp, data.Sexo, data.Comentarios, data.Calle, data.Colonia, data.Delegacion, data.NumInt, data.NumExt,
                data.EntreCalles, data.Profesion, data.EstadoCivil, data.TipoSangre, data.Estado, data.Cp, data.Referencia1, data.Referencia2,
                _fileConvertService.ConvertToBase64(data.Fotografia.OpenReadStream()),
                "", ""));

            return RedirectToAction("Index");
        }



        [HttpGet("Pacientes/Edit/{id?}")]
        public IActionResult Edit(Guid id)
        {
            Catalogos();
            var pacienteRes = _pacientesDbContext.DetailsGenPaciente(id);
            var paciente = PacienteEdit.Create(pacienteRes.Id, pacienteRes.Nombre, pacienteRes.ApellidoP, pacienteRes.ApellidoM, pacienteRes.Edad, pacienteRes.Correo, pacienteRes.TelefonoM,
                pacienteRes.TelefonoT, pacienteRes.TelefonoD, pacienteRes.Curp, pacienteRes.Sexo, pacienteRes.Comentarios, pacienteRes.Calle, pacienteRes.Colonia, pacienteRes.Delegacion,
                pacienteRes.NumInt, pacienteRes.NumExt, pacienteRes.EntreCalles, pacienteRes.Profesion, pacienteRes.EstadoCivil, pacienteRes.TipoSangre, pacienteRes.Estado, pacienteRes.Cp,
                pacienteRes.Referencia1, pacienteRes.Referencia2, null);
            return View(paciente);

        }

        [HttpPost("Pacientes/Edit/{id?}")]
        public IActionResult Edit(Guid id, PacienteEdit data)
        {
            string fotografia;
            if (ModelState.IsValid)
            {
                if (data.Fotografia == null)
                {
                    fotografia = _pacientesDbContext.Details(id).Fotografia;
                }
                else
                {
                    fotografia = _fileConvertService.ConvertToBase64(data.Fotografia.OpenReadStream());
                }
                _pacientesDbContext.Edit(id, Domain.Paciente.Create(data.Nombre, data.ApellidoP, data.ApellidoM, data.Edad, data.Correo, data.TelefonoM,
                data.TelefonoT, data.TelefonoD, data.Curp, data.Sexo, data.Comentarios, data.Calle, data.Colonia, data.Delegacion, data.NumInt, data.NumExt,
                data.EntreCalles, data.Profesion, data.EstadoCivil, data.TipoSangre, data.Estado, data.Cp, data.Referencia1, data.Referencia2,
                fotografia,
                "", ""));
                Catalogos();
                return RedirectToAction("Delete");
            }

            return View("Edit", data);

        }

        [Authorize(Roles = "Admin,Doctor,Enfermera")]
        [HttpGet]
        public IActionResult Delete()
        {

            var pacientes = _pacientesDbContext.List();
            return View(pacientes);

        }
        [HttpPost]
        public IActionResult Delete(Guid id)
        {

            _pacientesDbContext.Delete(id);
            var pacientes = _pacientesDbContext.List();
            return Json(pacientes);

        }
        private void Catalogos()
        {
            List<SelectListItem> listaEstadosCiviles = new List<SelectListItem>();
            List<SelectListItem> listaProfesiones = new List<SelectListItem>();
            List<SelectListItem> listaTipos = new List<SelectListItem>();
            foreach (var estado in _catalogosDbContext.ListCatEstadoCivil())
            {
                listaEstadosCiviles.Add(new SelectListItem { Value = estado.IdEstado.ToString(), Text = estado.Nombre_Estado });
            }
            ViewBag.estadosCiviles = listaEstadosCiviles;

            foreach (var profesion in _catalogosDbContext.ListCatProfesion())
            {
                listaProfesiones.Add(new SelectListItem { Value = profesion.IdProfesion.ToString(), Text = profesion.Nombre_profesion });
            }
            ViewBag.profesiones = listaProfesiones;

            foreach (var tipo in _catalogosDbContext.ListCatTiposSangre())
            {
                listaTipos.Add(new SelectListItem { Value = tipo.IdTipo.ToString(), Text = tipo.nombre_Tipo });
            }
            ViewBag.tipos = listaTipos;

        }
    }
}
