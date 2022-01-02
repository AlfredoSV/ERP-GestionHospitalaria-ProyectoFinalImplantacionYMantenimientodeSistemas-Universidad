using Domain;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Infrastructure
{

    public class ProductosDbContext
    {
        private readonly string _connectionString;

        public ProductosDbContext(string connectionString)
        {
            _connectionString = connectionString;
        }

        public List<Producto> List()
        {
            var data = new List<Producto>();

            var con = new SqlConnection(_connectionString);
            var cmd = new SqlCommand(@"SELECT pro.Id,pro.Nombre,pro.Descripcion,tp.nombre nombreTipo,pro.Precio
                                        ,pro.Cantidad, pro.Foto from Producto pro INNER JOIN TipoProducto tp on pro.Tipo = tp.idTipo", con);
            try
            {
                con.Open();
                var dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    data.Add(new Producto
                    {
                        Id = (Guid)dr["Id"],
                        Nombre = (string)dr["Nombre"],
                        Descripcion = (string)dr["Descripcion"],
                        NombreTipo = (string)(dr["nombreTipo"]),
                        Precio = Convert.ToSingle(dr["Precio"]),
                        Cantidad = (int)dr["Cantidad"],
                        Foto = (string)dr["Foto"]
                    });
                }
                return data;
            }
            catch (Exception e)
            {
                throw;
            }
            finally
            {
                con.Close();
            }
        }



        public Producto Details(Guid id)
        {
            var data = new Producto();

            var con = new SqlConnection(_connectionString);
            var cmd = new SqlCommand(@"SELECT pro.Id,pro.Nombre,pro.Descripcion,tp.nombre nombreTipo,pro.Precio
                                        ,pro.Cantidad, pro.Foto,tp.idTipo from Producto pro INNER JOIN TipoProducto tp on pro.Tipo = tp.idTipo where pro.Id = @id", con);
            cmd.Parameters.Add("@id", SqlDbType.UniqueIdentifier).Value = id;
            try
            {
                con.Open();
                var dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    data= new Producto
                    {
                        Id = (Guid)dr["Id"],
                        Nombre = (string)dr["Nombre"],
                        Descripcion = (string)dr["Descripcion"],
                        NombreTipo = (string)(dr["nombreTipo"]),
                        Precio = Convert.ToSingle(dr["Precio"]),
                        IdTipoProducto = (Guid)dr["idTipo"],
                        Cantidad = (int)dr["Cantidad"],
                        Foto = (string)dr["Foto"]
                    };
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

        public bool Create(Producto data)
        {
            var res = 0;
            var con = new SqlConnection(_connectionString);
            var cmd = new SqlCommand("INSERT INTO [Producto] VALUES (@id,@nombre,@desc,@tipo,@precio,@cantidad,@foto)", con);
            cmd.Parameters.Add("id", SqlDbType.UniqueIdentifier).Value = Guid.NewGuid();
            cmd.Parameters.Add("nombre", SqlDbType.NVarChar, 128).Value = data.Nombre;
            cmd.Parameters.Add("desc", SqlDbType.NVarChar, 512).Value = data.Descripcion;
            cmd.Parameters.Add("tipo", SqlDbType.UniqueIdentifier).Value = data.IdTipoProducto;
            cmd.Parameters.Add("precio", SqlDbType.Float).Value = data.Precio;
            cmd.Parameters.Add("cantidad", SqlDbType.Int).Value = data.Cantidad;
            cmd.Parameters.Add("foto", SqlDbType.VarChar).Value = data.Foto;


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

        public bool Edit(Producto data)
        {
            var res = 0;
            var con = new SqlConnection(_connectionString);
            var cmd = new SqlCommand("UPDATE [Producto] SET [Nombre] = @nombre, Descripcion = @desc, Tipo = @tipo, Precio = @precio, Cantidad = @cantidad, Foto = @foto WHERE [Id] = @id", con);
            cmd.Parameters.Add("id", SqlDbType.UniqueIdentifier).Value = data.Id;
            cmd.Parameters.Add("nombre", SqlDbType.NVarChar, 128).Value = data.Nombre;
            cmd.Parameters.Add("desc", SqlDbType.NVarChar, 512).Value = data.Descripcion;
            cmd.Parameters.Add("tipo", SqlDbType.UniqueIdentifier).Value = data.IdTipoProducto;
            cmd.Parameters.Add("precio", SqlDbType.Float).Value = data.Precio;
            cmd.Parameters.Add("cantidad", SqlDbType.Int).Value = data.Cantidad;
            cmd.Parameters.Add("foto", SqlDbType.VarChar).Value = data.Foto;
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
            var cmd = new SqlCommand("DELETE FROM [Producto] WHERE [Id] = @id", con);
            cmd.Parameters.Add("id", SqlDbType.UniqueIdentifier).Value = id;
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
    }
}
