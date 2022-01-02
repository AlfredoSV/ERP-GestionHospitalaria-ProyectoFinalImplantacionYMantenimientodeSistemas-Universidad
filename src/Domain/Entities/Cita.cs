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
        public Guid idEstaus { get; set; }
        public Guid idArea { get; set; }      
        public string Observaciones { get; set; }

        public Cita(Guid pacienteId, DateTime fecha, Guid idEstaus, Guid idArea, string observaciones)
        {
            PacienteId = pacienteId;
            Fecha = fecha;
            this.idEstaus = idEstaus;
            this.idArea = idArea;
            Observaciones = observaciones;
        }

        public static Cita Create(Guid pacienteId, DateTime fecha, Guid idEstaus, Guid idArea, string observaciones)
        {
            return new Cita(pacienteId,fecha,idEstaus,idArea, observaciones);
        }

        public Cita(Guid id, Guid pacienteId, string nombrePaciente, string estatusCita, string areaAtencion, DateTime fecha,string observaciones)
        {
            Id = id;
            PacienteId = pacienteId;
            NombrePaciente = nombrePaciente;
            EstatusCita = estatusCita;
            AreaAtencion = areaAtencion;
            Fecha = fecha;
            Observaciones = observaciones;
        }

        public static Cita Create(Guid id, Guid pacienteId, string nombrePaciente, string estatusCita, string areaAtencion, DateTime fecha, string observaciones)
        {
            return new Cita(id, pacienteId, nombrePaciente, estatusCita,areaAtencion,fecha,observaciones);
        }

        
    }
}
