using System;

namespace Domain
{
    public class Paciente
    {
        public Paciente(){}
        public Paciente(string nombre, string apellidoP, string apellidoM, int edad, string correo, string telefonoM, string telefonoT, string telefonoD, string curp, string sexo, string comentarios, string calle, string colonia, string delegacion, string numInt, string numExt, string entreCalles, Guid profesion, Guid estadoCivil, Guid tipoSangre, string estado, string cp, string referencia1, string referencia2, string fotografia, string ine, string actaNacimiento)
        {
            Nombre = nombre;
            ApellidoP = apellidoP;
            ApellidoM = apellidoM;
            Edad = edad;
            Correo = correo;
            TelefonoM = telefonoM;
            TelefonoT = telefonoT;
            TelefonoD = telefonoD;
            Curp = curp;
            Sexo = sexo;
            Comentarios = comentarios;
            Calle = calle;
            Colonia = colonia;
            Delegacion = delegacion;
            NumInt = numInt;
            NumExt = numExt;
            EntreCalles = entreCalles;
            Profesion = profesion;
            EstadoCivil = estadoCivil;
            TipoSangre = tipoSangre;
            Estado = estado;
            Cp = cp;
            Referencia1 = referencia1;
            Referencia2 = referencia2;
            Fotografia = fotografia;
            Ine = ine;
            ActaNacimiento = actaNacimiento;
        }


        public static Paciente Create(string nombre, string apellidoP, string apellidoM, int edad, string correo, string telefonoM, string telefonoT, string telefonoD, string curp, string sexo, string comentarios, string calle, string colonia, string delegacion, string numInt, string numExt, string entreCalles, Guid profesion, Guid estadoCivil, Guid tipoSangre, string estado, string cp, string referencia1, string referencia2, string fotografia, string ine, string actaNacimiento)
        {
            return new Paciente( nombre,  apellidoP,  apellidoM,  edad,  correo,  telefonoM,  telefonoT,  telefonoD,  curp,  sexo,  comentarios,  calle,  colonia,  delegacion,  numInt,  numExt,  entreCalles,  profesion, estadoCivil, tipoSangre, estado, cp, referencia1, referencia2, fotografia, ine,actaNacimiento);
        }

        public string NombreProfesion { get; set; }
        public string TipoSangreN { get; set; }
        public string EstadoCivilN { get; set; }
        public Guid Id { get; set; }

        public string Nombre { get; set; }

       
        public string ApellidoP { get; set; }

        
        public string ApellidoM { get; set; }

        
        public int Edad { get; set; }

        public string Correo { get; set; }

   
        public string TelefonoM { get; set; }

       
        public string TelefonoT { get; set; }

       
        public string TelefonoD { get; set; }

        public string Curp { get; set; }

        public string Sexo { get; set; }

        public string Comentarios { get; set; }

       
        public string Calle { get; set; }

       
        public string Colonia { get; set; }
        public string Delegacion { get; set; }

        public string NumInt { get; set; }

        
        public string NumExt { get; set; }

        
        public string EntreCalles { get; set; }

        public Guid Profesion { get; set; }

        public Guid EstadoCivil { get; set; }

        public Guid TipoSangre { get; set; }
        public string Estado { get; set; }

        public string Cp { get; set; }

      
        public string Referencia1 { get; set; }


        public string Referencia2 { get; set; }


        
        public string Fotografia { get; set; }

        
        public string Ine { get; set; }

       
        public string ActaNacimiento { get; set; }

        public string Expediente { get; set; }


    }
}