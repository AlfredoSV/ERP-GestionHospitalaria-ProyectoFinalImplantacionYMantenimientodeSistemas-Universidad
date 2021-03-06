using Domain;
using Domain.IRepositorios;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Infrastructure
{
    public class RepositorioDoctores : IRepositorioDoctores
    {
        private readonly string _connectionString;
        public RepositorioDoctores(string connectionString)
        {
            _connectionString = connectionString;
        }
        public List<Doctor> ConsultarDoctoresDb()
        {
            var data = new List<Doctor>();

            var con = new SqlConnection(_connectionString);
            var cmd = new SqlCommand(@"select *  from doctor doc inner join areaMedica am on doc.idArea = am.idArea 
                                       inner join EstadoCivil ec on doc.idEstadoCivil = ec.idEstadoC ", con);
            try
            {
                con.Open();
                var dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    data.Add(new Doctor()
                    {
                        Id = (Guid)dr["Id"],
                        Nombre = (string)dr["Nombre"],
                        Apellidop = (string)dr["ApellidoP"],
                        ApellidoM = (string)dr["ApellidoM"],
                        Edad = (int)dr["Edad"],
                        Telefono = (string)dr["TelefonoM"],
                        Correo = (string)dr["Correo"],
                        Especialidad = (string)dr["Especialidad"],
                        Foto = (string)dr["Foto"],
                        IdArea = (Guid)dr["idArea"],
                        Nombre_Area = (string)dr["nombre_area"],
                        IdEstadoCivil = (Guid)dr["idEstadoC"],
                        Nombre_Estado = (string)dr["nombre_EstadoC"]
                    });



                }

            }
            catch (Exception e)
            {
                data = null;
            }
            finally
            {
                con.Close();

            }

            return data;
        }

        public Doctor ConsultarDetalleDoctorDb(Guid id)
        {
            var data = new Doctor();

            var con = new SqlConnection(_connectionString);
            var cmd = new SqlCommand(@"select *  from doctor doc inner join areaMedica am on doc.idArea = am.idArea
                                        inner join EstadoCivil ec on doc.idEstadoCivil = ec.idEstadoC where id = @id ", con);

            cmd.Parameters.Add("@id", SqlDbType.UniqueIdentifier).Value = id;
            try
            {
                con.Open();
                var dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    data = new Doctor()
                    {
                        Id = (Guid)dr["Id"],
                        Nombre = (string)dr["Nombre"],
                        Apellidop = (string)dr["ApellidoP"],
                        ApellidoM = (string)dr["ApellidoM"],
                        Edad = (int)dr["Edad"],
                        Telefono = (string)dr["TelefonoM"],
                        Correo = (string)dr["Correo"],
                        Especialidad = (string)dr["Especialidad"],
                        Foto = (string)dr["Foto"],
                        IdArea = (Guid)dr["idArea"],
                        Nombre_Area = (string)dr["nombre_area"],
                        IdEstadoCivil = (Guid)dr["idEstadoC"],
                        Nombre_Estado = (string)dr["nombre_EstadoC"]
                    };



                }

            }
            catch (Exception e)
            {
                throw;
                data = null;
            }
            finally
            {
                con.Close();

            }

            return data;
        }

        public bool GuardarDoctorBd(Doctor data)
        {
            var res = 0;
            var con = new SqlConnection(_connectionString);
            var cmd = new SqlCommand(@"INSERT INTO [dbo].[Doctor]
           ([Id]
           ,[Nombre]
           ,[ApellidoP]
           ,[ApellidoM]
           ,[Edad]
           ,[TelefonoM]
           ,[Correo]
           ,[Especialidad]
           ,[Foto]
           ,[idArea]
           ,[idEstadoCivil])
     VALUES
           (@Id
           ,@Nombre
           ,@ApellidoP
           ,@ApellidoM
           ,@Edad
           ,@TelefonoM
           ,@Correo
           ,@Especialidad
           ,@Foto
           ,@idArea
           ,@idEstadoCivil)", con);
            cmd.Parameters.Add("id", SqlDbType.UniqueIdentifier).Value = Guid.NewGuid();
            cmd.Parameters.Add("Nombre", SqlDbType.VarChar).Value = data.Nombre;
            cmd.Parameters.Add("ApellidoP", SqlDbType.VarChar).Value = data.Apellidop;
            cmd.Parameters.Add("ApellidoM", SqlDbType.VarChar).Value = data.ApellidoM;
            cmd.Parameters.Add("Edad", SqlDbType.Int).Value = data.Edad;
            cmd.Parameters.Add("TelefonoM", SqlDbType.VarChar).Value = data.Telefono;

            cmd.Parameters.Add("Correo", SqlDbType.VarChar).Value = data.Correo;
            cmd.Parameters.Add("Especialidad", SqlDbType.VarChar).Value = data.Especialidad;
            cmd.Parameters.Add("Foto", SqlDbType.VarChar).Value = data.Foto;

            cmd.Parameters.Add("idArea", SqlDbType.UniqueIdentifier).Value = data.IdArea;
            cmd.Parameters.Add("idEstadoCivil", SqlDbType.UniqueIdentifier).Value = data.IdEstadoCivil;
            try
            {
                con.Open();
                res = cmd.ExecuteNonQuery();
            }
            catch (Exception)
            {
                throw;
                res = 0;
            }
            finally
            {
                con.Close();
            }
            return res > 0;
        }

        public bool EditarDoctorBd(Doctor data)
        {
            var res = 0;
            var con = new SqlConnection(_connectionString);
            var cmd = new SqlCommand(@"UPDATE [dbo].[Doctor]
   SET 
       [Nombre] = @Nombre
      ,[ApellidoP] = @ApellidoP
      ,[ApellidoM] = @ApellidoM
      ,[Edad] = @Edad
      ,[TelefonoM] = @TelefonoM
      ,[Correo] = @Correo
      ,[Especialidad] = @Especialidad
      ,[Foto] = @Foto
      ,[idArea] = @idArea
      ,[idEstadoCivil] = @idEstadoCivil  where id = @id", con);
            cmd.Parameters.Add("id", SqlDbType.UniqueIdentifier).Value = data.Id;
            cmd.Parameters.Add("Nombre", SqlDbType.VarChar).Value = data.Nombre;
            cmd.Parameters.Add("ApellidoP", SqlDbType.VarChar).Value = data.Apellidop;
            cmd.Parameters.Add("ApellidoM", SqlDbType.VarChar).Value = data.ApellidoM;
            cmd.Parameters.Add("Edad", SqlDbType.Int).Value = data.Edad;
            cmd.Parameters.Add("TelefonoM", SqlDbType.VarChar).Value = data.Telefono;

            cmd.Parameters.Add("Correo", SqlDbType.VarChar).Value = data.Correo;
            cmd.Parameters.Add("Especialidad", SqlDbType.VarChar).Value = data.Especialidad;
            cmd.Parameters.Add("Foto", SqlDbType.VarChar).Value = data.Foto;

            cmd.Parameters.Add("idArea", SqlDbType.UniqueIdentifier).Value = data.IdArea;
            cmd.Parameters.Add("idEstadoCivil", SqlDbType.UniqueIdentifier).Value = data.IdEstadoCivil;
            try
            {
                con.Open();
                res = cmd.ExecuteNonQuery();
            }
            catch (Exception)
            {
                throw;
                res = 0;
            }
            finally
            {
                con.Close();
            }
            return res > 0;
        }

        public bool EliminarDoctorBd(Guid id)
        {
            var res = 0;
            var con = new SqlConnection(_connectionString);
            var cmd = new SqlCommand(@"delete from Doctor where Id = @id", con);
            cmd.Parameters.Add("id", SqlDbType.UniqueIdentifier).Value = id;

            try
            {
                con.Open();
                res = cmd.ExecuteNonQuery();
            }
            catch (Exception)
            {
                throw;
                res = 0;
            }
            finally
            {
                con.Close();
            }
            return res > 0;
        }
    }
}
