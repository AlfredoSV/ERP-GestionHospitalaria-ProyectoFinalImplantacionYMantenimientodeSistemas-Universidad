using Domain;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Infrastructure
{
    public class PacientesDbContext
    {
        private readonly string _connectionString;

        public PacientesDbContext(string connectionString)
        {
            _connectionString = connectionString;
        }

        public List<Paciente> List()
        {
            var data = new List<Paciente>();

            var con = new SqlConnection(_connectionString);
            var cmd = new SqlCommand("SELECT * FROM [Paciente]", con);
            try
            {
                con.Open();
                var dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    data.Add(new Paciente()
                    {
                        Id = (Guid)dr["Id"],
                        Nombre = ((string)dr["Nombre"] + " " + (string)dr["ApellidoP"] + " " + (string)dr["ApellidoM"])
                        ,
                        Expediente = (string)dr["Expediente"],
                        Correo = (string)dr["Correo"],
                        Colonia = ((string)dr["Colonia"] + " " + (string)dr["Calle"]),
                        TelefonoM = (string)dr["TelefonoMovil"],
                        Edad = (int)dr["Edad"],
                        EstadoCivil = (Guid)dr["IdEstadoCivil"]

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

        public List<Paciente> ListSangre()
        {
            var data = new List<Paciente>();

            var con = new SqlConnection(_connectionString);
            var cmd = new SqlCommand("select p.id, tp.nombre_profesion from Paciente p inner join profesion tp on p.idprofesion = tp.idprofesion", con);
            try
            {
                con.Open();
                var dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    data.Add(new Paciente()
                    {
                        Id = (Guid)dr["Id"],
                        NombreProfesion = (string)dr["nombre_profesion"]

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
        public Paciente Details(Guid id)
        {
            var data = new Paciente();

            var con = new SqlConnection(_connectionString);
            var cmd = new SqlCommand(@"SELECT concat(Nombre,' ',ApellidoP,' ', ApellidoM) nombre,
                                    edad, TelefonoMovil, Curp, Correo, Sexo, FotoPaciente, IdTipoSangre,
                                    pro.nombre_profesion, tp.nombre_Tipo, es.nombre_EstadoC FROM Paciente pa
                                    inner join Profesion pro on pa.IdProfesion = pro.idProfesion
                                    inner join TipoSangre tp on pa.IdTipoSangre = tp.idTipo inner join EstadoCivil es
                                    on pa.IdEstadoCivil = es.idEstadoC where id = @id", con);
            cmd.Parameters.Add("@id", SqlDbType.UniqueIdentifier).Value = id;
            try
            {
                con.Open();
                var dr = cmd.ExecuteReader();
                if (dr.Read())
                {

                    data.Nombre = (string)dr["nombre"];
                    data.Edad = (int)dr["edad"];
                    data.TelefonoM = (string)dr["TelefonoMovil"];
                    data.Curp = (string)dr["Curp"];
                    data.Correo = (string)dr["Correo"];
                    data.Sexo = (string)dr["Sexo"];
                    data.Fotografia = (string)dr["FotoPaciente"];
                    data.NombreProfesion = (string)dr["nombre_profesion"];
                    data.TipoSangreN = (string)dr["nombre_tipo"];
                    data.EstadoCivilN = (string)dr["nombre_EstadoC"];
                }

            }
            catch (Exception)
            {

                data = null;
            }
            finally
            {
                con.Close();
            }

            return data;
        }

        public Paciente DetailsGenPaciente(Guid id)
        {
            var data = new Paciente();

            var con = new SqlConnection(_connectionString);
            var cmd = new SqlCommand(@"SELECT * FROM [Paciente] where id = @id", con);
            cmd.Parameters.Add("@id", SqlDbType.UniqueIdentifier).Value = id;
            try
            {
                con.Open();
                var dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    data.Id = (Guid)dr["Id"];
                    data.Nombre = (string)dr["nombre"];
                    data.ApellidoP = (string)dr["ApellidoP"];
                    data.ApellidoM = (string)dr["ApellidoM"];
                    data.Edad = (int)dr["edad"];
                    data.TelefonoM = (string)dr["TelefonoMovil"];
                    data.TelefonoT = (string)dr["TelefonoTraba"];
                    data.TelefonoD = (string)dr["TelefonoDomi"];
                    data.Curp = (string)dr["Curp"];
                    data.Correo = (string)dr["Correo"];
                    data.Comentarios = (string)dr["Comentarios"];
                    data.Profesion = (Guid)dr["IdProfesion"];
                    data.EstadoCivil = (Guid)dr["IdEstadoCivil"];
                    data.TipoSangre = (Guid)dr["IdTipoSangre"];
                    data.Sexo = (string)dr["Sexo"];
                    data.Fotografia = (string)dr["FotoPaciente"];
                    data.Delegacion = (string)dr["Delegacion"];
                    data.Calle = (string)dr["Calle"];
                    data.Colonia = (string)dr["Colonia"];
                    data.NumInt = (string)dr["NumInt"];
                    data.NumExt = (string)dr["NumExt"];
                    data.EntreCalles = (string)dr["EntreCalles"];
                    data.Referencia1 = (string)dr["Referencia1"];
                    data.Referencia2 = (string)dr["Referencia2"];
                    data.Estado = (string)dr["Estado"];
                    data.Cp = (string)dr["Cp"];
                    data.Expediente = (string)dr["Expediente"];
                }

            }
            catch (Exception)
            {

                data = null;
            }
            finally
            {
                con.Close();
            }

            return data;
        }

        public bool Create(Paciente data)
        {
            var res = 0;
            var con = new SqlConnection(_connectionString);
            var cmd = new SqlCommand(@"
            INSERT INTO [dbo].[Paciente] VALUES
           (@Id
           ,@Nombre
           ,@ApellidoP
           ,@ApellidoM
           ,@Edad
           ,@TelefonoMovil
           ,@TelefonoTraba
           ,@TelefonoDomi
           ,@Curp
           ,@Correo
           ,@Comentarios
           ,@IdProfesion
           ,@IdEstadoCivil
           ,@IdTipoSangre
           ,@Sexo
           ,@FotoPaciente
           ,@PdfIne
           ,@PdfActaNaci
           ,@Delegacion
           ,@Calle
           ,@Colonia
           ,@NumInt
           ,@NumExt
           ,@EntreCalles
           ,@Referencia1
           ,@Referencia2
           ,@Estado
           ,@Cp
           ,@Expediente)", con);
            cmd.Parameters.Add("Id", SqlDbType.UniqueIdentifier).Value = Guid.NewGuid();
            cmd.Parameters.Add("Nombre", SqlDbType.NVarChar, 256).Value = data.Nombre;
            cmd.Parameters.Add("ApellidoP", SqlDbType.NVarChar, 256).Value = data.ApellidoP;
            cmd.Parameters.Add("ApellidoM", SqlDbType.NVarChar, 256).Value = data.ApellidoM;
            cmd.Parameters.Add("Edad", SqlDbType.Int).Value = data.Edad;
            cmd.Parameters.Add("TelefonoMovil", SqlDbType.NVarChar, 128).Value = data.TelefonoM;
            cmd.Parameters.Add("TelefonoTraba", SqlDbType.NVarChar, 128).Value = data.TelefonoT;
            cmd.Parameters.Add("TelefonoDomi", SqlDbType.NVarChar, 128).Value = data.TelefonoD;
            cmd.Parameters.Add("Curp", SqlDbType.NVarChar, 128).Value = data.Curp;
            cmd.Parameters.Add("Correo", SqlDbType.NVarChar, 256).Value = data.Correo;
            cmd.Parameters.Add("Comentarios", SqlDbType.VarChar).Value = data.Comentarios == null ? string.Empty : data.Comentarios;
            cmd.Parameters.Add("IdProfesion", SqlDbType.UniqueIdentifier).Value = data.Profesion;
            cmd.Parameters.Add("IdEstadoCivil", SqlDbType.UniqueIdentifier).Value = data.EstadoCivil;
            cmd.Parameters.Add("IdTipoSangre", SqlDbType.UniqueIdentifier).Value = data.TipoSangre;
            cmd.Parameters.Add("Sexo", SqlDbType.VarChar).Value = data.Sexo;
            cmd.Parameters.Add("FotoPaciente", SqlDbType.VarChar).Value = data.Fotografia;
            cmd.Parameters.Add("PdfIne", SqlDbType.VarChar).Value = data.Ine;
            cmd.Parameters.Add("PdfActaNaci", SqlDbType.VarChar).Value = data.ActaNacimiento;
            cmd.Parameters.Add("Delegacion", SqlDbType.VarChar).Value = data.Delegacion;
            cmd.Parameters.Add("Calle", SqlDbType.VarChar).Value = data.Calle;
            cmd.Parameters.Add("Colonia", SqlDbType.VarChar).Value = data.Colonia;
            cmd.Parameters.Add("NumInt", SqlDbType.VarChar).Value = data.NumInt == null ? string.Empty : data.NumInt;
            cmd.Parameters.Add("NumExt", SqlDbType.VarChar).Value = data.NumExt;
            cmd.Parameters.Add("EntreCalles", SqlDbType.VarChar).Value = data.EntreCalles;
            cmd.Parameters.Add("Referencia1", SqlDbType.VarChar).Value = data.Referencia1;
            cmd.Parameters.Add("Referencia2", SqlDbType.VarChar).Value = data.Referencia2 == null ? string.Empty : data.Referencia2;
            cmd.Parameters.Add("Estado", SqlDbType.VarChar).Value = data.Estado;
            cmd.Parameters.Add("Cp", SqlDbType.VarChar, 6).Value = data.Cp;
            cmd.Parameters.Add("Expediente", SqlDbType.VarChar).Value = Guid.NewGuid().ToString().Substring(0, 5) + "-" + DateTime.Now.Day;

            try
            {
                con.Open();
                res = cmd.ExecuteNonQuery();
            }
            catch (Exception)
            {
                throw;
            }

            finally
            {
                con.Close();
            }
            return res > 0;
        }

        public bool Edit(Guid id, Paciente data)
        {
            var res = 0;
            var con = new SqlConnection(_connectionString);
            var cmd = new SqlCommand(@"UPDATE [dbo].[Paciente]
   SET 
       [Nombre] = @Nombre
      ,[ApellidoP] = @ApellidoP
      ,[ApellidoM] = @ApellidoM
      ,[Edad] = @Edad
      ,[TelefonoMovil] = @TelefonoMovil
      ,[TelefonoTraba] = @TelefonoTraba
      ,[TelefonoDomi] = @TelefonoDomi
      ,[Curp] = @Curp
      ,[Correo] = @Correo
      ,[Comentarios] = @Comentarios
      ,[IdProfesion] = @IdProfesion
      ,[IdEstadoCivil] = @IdEstadoCivil
      ,[IdTipoSangre] = @IdTipoSangre
      ,[Sexo] = @Sexo
      ,[FotoPaciente] = @FotoPaciente
      
      ,[Delegacion] = @Delegacion
      ,[Calle] = @Calle
      ,[Colonia] = @Colonia
      ,[NumInt] = @NumInt
      ,[NumExt] = @NumExt
      ,[EntreCalles] = @EntreCalles
      ,[Referencia1] = @Referencia1
      ,[Referencia2] = @Referencia2
      ,[Estado] = @Estado
      ,[Cp] = @Cp
      
 WHERE id = @id
", con);
            cmd.Parameters.Add("id", SqlDbType.UniqueIdentifier).Value = id;
            cmd.Parameters.Add("nombre", SqlDbType.NVarChar, 256).Value = data.Nombre;
            cmd.Parameters.Add("ApellidoP", SqlDbType.NVarChar, 256).Value = data.ApellidoP;
            cmd.Parameters.Add("ApellidoM", SqlDbType.NVarChar, 256).Value = data.ApellidoM;
            cmd.Parameters.Add("edad", SqlDbType.Int).Value = data.Edad;
            cmd.Parameters.Add("TelefonoMovil", SqlDbType.NVarChar, 128).Value = data.TelefonoM;
            cmd.Parameters.Add("TelefonoTraba", SqlDbType.NVarChar, 128).Value = data.TelefonoT;
            cmd.Parameters.Add("TelefonoDomi", SqlDbType.NVarChar, 128).Value = data.TelefonoD;
            cmd.Parameters.Add("Curp", SqlDbType.NVarChar, 128).Value = data.Curp;
            cmd.Parameters.Add("Correo", SqlDbType.NVarChar, 245).Value = data.Correo;
            cmd.Parameters.Add("Comentarios", SqlDbType.VarChar).Value = data.Comentarios == null ? "" : data.Comentarios;
            cmd.Parameters.Add("IdProfesion", SqlDbType.UniqueIdentifier).Value = data.Profesion;
            cmd.Parameters.Add("IdEstadoCivil", SqlDbType.UniqueIdentifier).Value = data.EstadoCivil;
            cmd.Parameters.Add("IdTipoSangre", SqlDbType.UniqueIdentifier).Value = data.TipoSangre;
            cmd.Parameters.Add("Sexo", SqlDbType.NVarChar, 15).Value = data.Sexo;
            cmd.Parameters.Add("FotoPaciente", SqlDbType.VarChar).Value = data.Fotografia;
            cmd.Parameters.Add("Delegacion", SqlDbType.VarChar).Value = data.Delegacion;
            cmd.Parameters.Add("Calle", SqlDbType.VarChar).Value = data.Calle;
            cmd.Parameters.Add("Colonia", SqlDbType.VarChar).Value = data.Colonia;
            cmd.Parameters.Add("NumInt", SqlDbType.VarChar).Value = data.NumInt == null ? "" : data.NumInt;
            cmd.Parameters.Add("NumExt", SqlDbType.VarChar).Value = data.NumExt;
            cmd.Parameters.Add("EntreCalles", SqlDbType.VarChar).Value = data.EntreCalles;
            cmd.Parameters.Add("Referencia1", SqlDbType.VarChar).Value = data.Referencia1;
            cmd.Parameters.Add("Referencia2", SqlDbType.VarChar).Value = data.Referencia2 == null ? "" : data.Referencia2;
            cmd.Parameters.Add("Estado", SqlDbType.VarChar).Value = data.Estado;
            cmd.Parameters.Add("Cp", SqlDbType.VarChar).Value = data.Cp;



            try
            {
                con.Open();
                res = cmd.ExecuteNonQuery();
            }
            catch (Exception)
            {
                throw;
            }

            finally
            {
                con.Close();
            }
            return res > 0;
        }

        public bool Delete(Guid id)
        {
            var res = 0;
            var con = new SqlConnection(_connectionString);

            var cmd2 = new SqlCommand("DELETE FROM [Cita] WHERE [PacienteId] = @id", con);
            var cmd = new SqlCommand("DELETE FROM [Paciente] WHERE [Id] = @id", con);

            cmd.Parameters.Add("id", SqlDbType.UniqueIdentifier).Value = id;
            cmd2.Parameters.Add("id", SqlDbType.UniqueIdentifier).Value = id;
            try
            {
                con.Open();

                res = cmd2.ExecuteNonQuery();
                res += cmd.ExecuteNonQuery();


            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                con.Close();
            }


            return res > 0;
        }
    }
}
