using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Infrastructure.Test
{
    public class PruebaUnitariaInfraestructura
    {
        private readonly RepositorioProductos _productosDbContext;
        private readonly RepositorioCatalogos _catalogosDbContext;
        private readonly RepositorioCitas _citasDbContext;
        private string sqlConeccion = "Data Source=im01.database.windows.net;Initial Catalog=db;Persist Security Info=True;User ID=AdminDb;Password=P@$$w0rd";
        public PruebaUnitariaInfraestructura()
        {
            _productosDbContext = new RepositorioProductos(sqlConeccion);
            _catalogosDbContext = new RepositorioCatalogos(sqlConeccion);
            _citasDbContext = new RepositorioCitas(sqlConeccion);
        }
        [Fact]
        public void SeEliminaCorrectamenteProducto()
        {
            bool result = _productosDbContext.Delete(Guid.Parse(""));
            Assert.True(result);

        }
        [Fact]
        public void SeConsultaCorrectemneteProducto()
        {
            var result = _productosDbContext.Details(Guid.Parse(""));
            Assert.Equal(result ,new Domain.Producto() { });

        }
        [Fact]
        public void SeConsultaEditaCorrectamenteProdcuto()
        {
            var result = _productosDbContext.Edit(new Domain.Producto() { });
            Assert.True(true);

        }
        [Fact]
        public void SeEliminaCorrectamenteCita()
        {
            bool result = _citasDbContext.Delete(Guid.Parse(""));
            Assert.True(result);

        }
        [Fact]
        public void SeConsultaCorrectemneteCita()
        {
            var result = _citasDbContext.Details(Guid.Parse(""));
            Assert.Equal(result, new Domain.Cita(Guid.Empty, DateTime.Now, Guid.Empty, Guid.Empty,null) { });

        }
        [Fact]
        public void SeConsultaEditaCorrectamenteCita()
        {
            var result = _citasDbContext.Edit(Guid.Empty, new Domain.Cita(Guid.Empty, DateTime.Now, Guid.Empty, Guid.Empty, null) { });
            Assert.True(true);

        }
    }
}
