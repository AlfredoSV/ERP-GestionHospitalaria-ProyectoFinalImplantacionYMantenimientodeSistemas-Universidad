using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Http;

using Infrastructure;
using Application;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using Presentation.WebApp.Models;
using Application.IServicios;

namespace Presentation.WebApp.Controllers
{
    [Authorize]
    public class ProductosController : Controller
    {
        private readonly ProductosDbContext _productosDbContext;
        private readonly IServicioCatalogos _servicioCatalogos;
        private readonly IFileConvertService _fileConvertService;
        public ProductosController(IConfiguration configuration, IFileConvertService fileConvertService,IServicioCatalogos servicioCatalogos)
        {
            _productosDbContext = new ProductosDbContext(configuration.GetConnectionString("DefaultConnection"));
            _servicioCatalogos = servicioCatalogos;
            _fileConvertService = fileConvertService;
        }

        [Authorize(Roles = "Admin,Doctor")]
        public IActionResult Index()
        {
            CargarTipoProductos();
            return View(_productosDbContext.List());
        }

        [HttpPost]
        public IActionResult ListarProductos()
        {
            return Json(_productosDbContext.List());
        }
        [Authorize(Roles = "Admin,Doctor,Enfermera")]
        [HttpGet]
        public IActionResult Details()
        {
            return View("Details", _productosDbContext.List());
        }

        [HttpPost]
        public IActionResult Details(Guid id)
        {

            return Json(_productosDbContext.Details(id));
        }

        [HttpPost]
        public IActionResult DetailsEdit(Guid id)
        {

            return Json(_productosDbContext.Details(id));
        }

        [Authorize(Roles = "Admin,Doctor")]
        public IActionResult Create()
        {
            CargarTipoProductos();
            return View();
        }
        [HttpPost]
        public IActionResult Create(Producto data)
        {
            CargarTipoProductos();
            _productosDbContext.Create(Domain.Producto.Create(data.Nombre, data.Descripcion,
                data.IdTipoProducto, data.Precio, data.Cantidad, _fileConvertService.ConvertirABase64(data.Foto.OpenReadStream())));
            return RedirectToAction("Index");
        }


        [HttpPost]
        public IActionResult Edit(Producto data)
        {

            var fotografia = data.Foto == null ? _productosDbContext.Details(data.Id).Foto : _fileConvertService.ConvertirABase64(data.Foto.OpenReadStream());

            var editarPro = _productosDbContext.Edit(Domain.Producto.Create(data.Id, data.Nombre, data.Descripcion, data.IdTipoProducto, data.Precio, data.Cantidad, fotografia));

            return Json(new { sal = true });

        }

        [HttpGet]
        public IActionResult Delete()
        {
            return View(_productosDbContext.List());
        }
        [HttpPost]
        public IActionResult Delete(Guid id)
        {
            var seElimino = _productosDbContext.Delete(id);
            return Json(new { seElimino });
        }

        private void CargarTipoProductos()
        {
            List<SelectListItem> listaTiposProductos = new List<SelectListItem>();
            foreach (var estado in _servicioCatalogos.ConsultarTiposProductos())
            {
                listaTiposProductos.Add(new SelectListItem { Value = estado.IdTipo.ToString(), Text = estado.Nombre });
            }
            ViewBag.tiposProductos = listaTiposProductos;
        }
    }
}
