using System;

namespace Domain
{
    public class Producto
    {
        public Producto() { }
        public Producto(string nombre, string descripcion, Guid idTipoProducto, float precio, int cantidad, string foto)
        {
            Nombre = nombre;
            Descripcion = descripcion;
            IdTipoProducto = idTipoProducto;
            Precio = precio;
            Cantidad = cantidad;
            Foto = foto;
        }

        public Producto(Guid id, string nombre, string descripcion, Guid idTipoProducto, float precio, int cantidad, string foto)
        {
            Id = id;
            Nombre = nombre;
            Descripcion = descripcion;
            IdTipoProducto = idTipoProducto;
            Precio = precio;
            Cantidad = cantidad;
            Foto = foto;
        }

        public Guid Id { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public int Tipo { get; set; }
        public string NombreTipo { get; set; }
        public Guid IdTipoProducto { get; set; }
        public float Precio { get; set; }
        public int Cantidad { get; set; }
        public string Foto { get; set; }


        public static Producto Create(string nombre, string descripcion, Guid idTipoProducto, float precio, int cantidad, string foto)
        {
            return new Producto( nombre,  descripcion,  idTipoProducto,  precio,  cantidad,  foto);
        }

        public static Producto Create(Guid id, string nombre, string descripcion, Guid idTipoProducto, float precio, int cantidad, string foto)
        {
            return new Producto( id,  nombre,  descripcion,  idTipoProducto,  precio,  cantidad,  foto);
        }


    }
}