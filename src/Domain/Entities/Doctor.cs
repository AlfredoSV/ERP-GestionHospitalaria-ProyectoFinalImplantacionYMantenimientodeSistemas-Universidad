using System;

namespace Domain
{
    public class Doctor
    {

        public  Doctor(){}
        public Guid Id { get; set; }

        public string Nombre { get; set; }

        public string Apellidop { get; set; }

        public string ApellidoM { get; set; }
        public int Edad { get; set; }

        public string Direccion { get; set; }

        public string Telefono { get; set; }

        public string Correo { get; set; }

        public string Especialidad { get; set; }

        public string Foto { get; set; }

        public Guid IdArea { get; set; }
        public string Nombre_Area { get; set; }
    
        public Guid IdEstadoCivil { get; set; }
        public string Nombre_Estado { get; set; }

        public Doctor(string nombre, string apellidop, string apellidoM, int edad, string direccion, string telefono, string correo, string especialidad, string foto, Guid idArea, Guid idEstadoCivil)
        {
            Nombre = nombre;
            Apellidop = apellidop;
            ApellidoM = apellidoM;
            Edad = edad;
            Direccion = direccion;
            Telefono = telefono;
            Correo = correo;
            Especialidad = especialidad;
            Foto = foto;
            IdArea = idArea;
            IdEstadoCivil = idEstadoCivil;
        }

        public Doctor(Guid id, string nombre, string apellidop, string apellidoM, int edad, string direccion, string telefono, string correo, string especialidad, string foto, Guid idArea, Guid idEstadoCivil)
        {
            Id = id;
            Nombre = nombre;
            Apellidop = apellidop;
            ApellidoM = apellidoM;
            Edad = edad;
            Direccion = direccion;
            Telefono = telefono;
            Correo = correo;
            Especialidad = especialidad;
            Foto = foto;
            IdArea = idArea;
            IdEstadoCivil = idEstadoCivil;
        }

        public static Doctor Create(Guid id, string nombre, string apellidop, string apellidoM, int edad, string direccion, string telefono, string correo, string especialidad, string foto, Guid idArea, Guid idEstadoCivil)
        {
            return new Doctor( id,  nombre,  apellidop,  apellidoM,  edad,  direccion,  telefono,  correo,  especialidad,  foto,  idArea,  idEstadoCivil);
        }
        public static Doctor Create(string nombre, string apellidop, string apellidoM, int edad, string direccion, string telefono, string correo, string especialidad, string foto, Guid idArea, Guid idEstadoCivil)
        {
            return new Doctor( nombre,  apellidop,  apellidoM,  edad,  direccion,  telefono,  correo,  especialidad,  foto,  idArea,  idEstadoCivil);
        }
    }
}
