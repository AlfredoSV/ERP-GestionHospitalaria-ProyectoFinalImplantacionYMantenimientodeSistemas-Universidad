using System;

namespace Domain
{
    public class Cita
    {
        public Guid Id { get; set; }
        public Guid PacienteId { get; set; }
        public string NombrePaciente { get; set; }
        public string EstatusCita { get; set; }
        public string AreaAtencion { get; set; }
        public DateTime Fecha { get; set; }    
        public Guid IdEstaus { get; set; }
        public Guid IdArea { get; set; }      
        public string Observaciones { get; set; }

        public Cita(Guid pacienteId, DateTime fecha, Guid idEstaus, Guid idArea, string observaciones)
        {
            PacienteId = pacienteId;
            Fecha = fecha;
            IdEstaus = idEstaus;
            IdArea = idArea;
            Observaciones = observaciones;
        }

        public static Cita Create(Guid pacienteId, DateTime fecha, Guid idEstaus, Guid idArea, string observaciones)
        {
            return new Cita(pacienteId,fecha,idEstaus,idArea, observaciones);
        }

        public Cita(Guid id, Guid pacienteId, string nombrePaciente, string estatusCita, string areaAtencion, DateTime fecha,string observaciones, Guid idArea, Guid idEstatus)
        {
            Id = id;
            PacienteId = pacienteId;
            NombrePaciente = nombrePaciente;
            EstatusCita = estatusCita;
            AreaAtencion = areaAtencion;
            Fecha = fecha;
            Observaciones = observaciones;
            IdArea = idArea;
            IdEstaus = idEstatus;
        }

        public static Cita Create(Guid id, Guid pacienteId, string nombrePaciente, string estatusCita, string areaAtencion, DateTime fecha, string observaciones,Guid idArea,Guid idEstatus)
        {
            return new Cita(id, pacienteId, nombrePaciente, estatusCita,areaAtencion,fecha,observaciones,idArea,idEstatus);
        }

        
    }
}
