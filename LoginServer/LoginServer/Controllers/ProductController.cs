using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using LoginServer.Models;
using System.Data.SqlClient;
using System.Diagnostics;

namespace LoginServer.Controllers
{
    [EnableCorsAttribute("*", "*", "*")]
    public class ProductController : ApiController
    {
        public string connectionString = @"Data Source=DESKTOP-VAB8R5D;Initial Catalog=login;Integrated Security=True";
        public List<Product> listProduct;
        SqlParameter sp1 = new SqlParameter();

        //[Route("api/Product/HandleGetProductData/{id:int?}")]
        [HttpGet]
        public List<Product> HandleGetProductData(int id = -1)
        {
            using (SqlConnection cnn = new SqlConnection(connectionString))
            {
                listProduct = new List<Product>();
                SqlCommand sqlcmd = new SqlCommand("SP_Product_GetRows", cnn);
                sqlcmd.CommandType = System.Data.CommandType.StoredProcedure;
                try
                {
                    if (id != -1)
                    {
                        sqlcmd.Parameters.Add("@_ProductID", System.Data.SqlDbType.Int).Value = id;
                    }
                    else
                    {
                        sqlcmd.Parameters.AddWithValue("@_ProductID", System.Data.SqlDbType.Int).Value = -1;
                    }


                    cnn.Open();
                    SqlDataReader sqldr = sqlcmd.ExecuteReader();

                    while (sqldr.Read())
                    {
                        listProduct.Add(
                            new Product()
                            {
                                Id = (int)sqldr["id"],
                                ProductName = (string)sqldr["product_name"],
                                Price = (double)sqldr["price"],
                                Qty = (int)sqldr["qty"]
                            }
                        );
                    }
                    cnn.Close();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            return listProduct;
        }
        [HttpPut]
        public string HandleUpdateProductData(int id,[FromBody]Product product)
        {
            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                try
                {
                    SqlCommand sqlcmd = new SqlCommand("SP_Product_UpdateRows", sqlConnection);
                    sqlcmd.CommandType = System.Data.CommandType.StoredProcedure;
                    sqlcmd.Parameters.Add("@_ID", System.Data.SqlDbType.Int).Value = id;
                    sqlcmd.Parameters.Add("@_ProductName", System.Data.SqlDbType.VarChar).Value = product.ProductName;
                    sqlcmd.Parameters.Add("@_Price", System.Data.SqlDbType.Float).Value = product.Price;
                    sqlcmd.Parameters.Add("@_Qty", System.Data.SqlDbType.Int).Value = product.Qty;
                    sqlConnection.Open();
                    sqlcmd.ExecuteReader();
                    sqlConnection.Close();
                }
                catch (Exception e)
                {
                    throw e;
                }

            }
            return "Cập nhật thành công";
        }
        [HttpPost]
        public string HandleCreateProductData([FromBody]Product product)
        {
            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                try
                {
                    SqlCommand sqlcmd = new SqlCommand("SP_Product_CreateRows", sqlConnection);
                    sqlcmd.CommandType = System.Data.CommandType.StoredProcedure;
                    sqlcmd.Parameters.Add("@_ProductName", System.Data.SqlDbType.VarChar).Value = product.ProductName;
                    sqlcmd.Parameters.Add("@_Price", System.Data.SqlDbType.Float).Value = product.Price;
                    sqlcmd.Parameters.Add("@_Qty", System.Data.SqlDbType.Int).Value = product.Qty;
                    sqlConnection.Open();
                    sqlcmd.ExecuteReader();
                    sqlConnection.Close();
                }
                catch (Exception e)
                {
                    throw e;
                }

            }
            return "Thêm mới thành công";
        }
        [HttpDelete]
        public string DeleteProduct(int id)
        {
            Debug.Write("ID: " + id);
            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                try
                {
                    SqlCommand sqlcmd = new SqlCommand("SP_Product_DeleteRows", sqlConnection);
                    sqlcmd.CommandType = System.Data.CommandType.StoredProcedure;
                    sqlcmd.Parameters.Add("@_Id", System.Data.SqlDbType.Int).Value = id;
                    sqlConnection.Open();
                    sqlcmd.ExecuteReader();
                    sqlConnection.Close();
                }
                catch (Exception e)
                {
                    throw e;
                }

            }
            return "Xóa thành công";
        }
    }
}
