﻿using Application.IServicios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.IRepositorios;
using Domain;
using Application.Dtos;

namespace Application.Servicios
{

    public class ServicioCita : IServicioCitas
    {
        private readonly IRepositorioCitas _repositorioCitas;

        public ServicioCita(IRepositorioCitas repositorioCitas)
        {
            _repositorioCitas = repositorioCitas;
        }

        public Cita ConsultarDetalleCita(Guid id) => _repositorioCitas.ConsultarDetalleCita(id);
  
        public bool CrearNuevaCita(Cita cita) => _repositorioCitas.CrearNuevaCita(cita);

        public bool EditarCita( Cita cita)=> _repositorioCitas.EditarCita(cita);

        public bool EliminarCita(Guid id) => _repositorioCitas.EliminarCita(id);
  
        public List<Cita> ConsultarCitasPaginadas(int pagina, int tamanioPag,string busqueda,bool ordernado)
        {
            List<Cita> listaCitas = _repositorioCitas.ListarCitasPaginadas(pagina,tamanioPag);

            if(!busqueda.Equals(string.Empty))
                listaCitas = listaCitas.Where(c => c.NombrePaciente.Contains(busqueda)).ToList();
            
            if(ordernado)
                listaCitas = listaCitas.OrderBy(c => c.NombrePaciente).ToList();
            
            return listaCitas;

        }

        public List<Cita> ConsultarCitas() => _repositorioCitas.ListarCitas();
    
        public IEnumerable<DtoGrafica> ConsultarCitasGraficas()=>

            _repositorioCitas.ListarCitasGraficas().GroupBy(info => info.EstatusCita)
                        .Select(group => new DtoGrafica
                        {
                            Metrica = group.Key,
                            Total = group.Count()
                        })
                        .OrderBy(x => x.Metrica); 
        

    }
}
