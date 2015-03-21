using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using MySql.Data.MySqlClient;
namespace eChannel.Models
{
    public class DBContext
    {
        private static DBContext instance = null;
        private MySqlConnection DbConnection;
        public static DBContext GetInstance()
        {
            if (instance == null)
                instance = new DBContext();
            return instance;
        }

        private DBContext()
        {
            string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["MySQLConnection"].ConnectionString;
            DbConnection = new MySqlConnection(connectionString);
        }



        public void CreateDoctorLogin(DoctorLogin model)
        {

            DbConnection.Open();
            MySqlCommand command = DbConnection.CreateCommand();
            command.CommandText = "INSERT INTO doctor_login(username,password,email) VALUES(@username, @password,@email)";
            command.Parameters.AddWithValue("@username", model.Username);
            command.Parameters.AddWithValue("@password", model.Password);
            command.Parameters.AddWithValue("@email", model.Email);
            command.ExecuteNonQuery();
            DbConnection.Close();
        }

        public DoctorLogin FindOneInDoctorLogin(string columnName, string value)
        {

            DbConnection.Open();
            MySqlCommand command = DbConnection.CreateCommand();
            command.CommandText = "SELECT * FROM doctor_login WHERE " + columnName + "=@value";
            command.Parameters.AddWithValue("@value", value);
            MySqlDataReader reader = command.ExecuteReader();
            DoctorLogin existing = null;
            if (reader.Read())
            {
                existing = new DoctorLogin();
                existing.DoctorID = reader.GetInt32("doctor_id");
                existing.Username = reader.GetString("username");
                existing.Password = reader.GetString("password");
                existing.Email = reader.GetString("email");
            }

            DbConnection.Close();
            return existing;
        }

       

    }
}