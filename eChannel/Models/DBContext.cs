using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using MySql.Data.MySqlClient;
using System.IO;
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

            if (instance.DbConnection.State == System.Data.ConnectionState.Open)
                instance.DbConnection.Close();
            return instance;
        }

        private void CheckIfConeectionOpen()
        {
            if (instance.DbConnection.State == System.Data.ConnectionState.Open)
                instance.DbConnection.Close();

        }

        private DBContext()
        {
            string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["MySQLConnection"].ConnectionString;
            DbConnection = new MySqlConnection(connectionString);
        }

        #region DoctorLogin

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
        #endregion

        #region PatientLogin

        public void CreatePatientLogin(PatientLogin model)
        {

            DbConnection.Open();
            MySqlCommand command = DbConnection.CreateCommand();
            command.CommandText = "INSERT INTO patient_login(username,password,email) VALUES(@username, @password,@email)";
            command.Parameters.AddWithValue("@username", model.Username);
            command.Parameters.AddWithValue("@password", model.Password);
            command.Parameters.AddWithValue("@email", model.Email);
            command.ExecuteNonQuery();
            DbConnection.Close();
        }

        public PatientLogin FindOneInPatientLogin(string columnName, string value)
        {

            DbConnection.Open();
            MySqlCommand command = DbConnection.CreateCommand();
            command.CommandText = "SELECT * FROM patient_login WHERE " + columnName + "=@value";
            command.Parameters.AddWithValue("@value", value);
            MySqlDataReader reader = command.ExecuteReader();
            PatientLogin existing = null;
            if (reader.Read())
            {
                existing = new PatientLogin();
                existing.PatientID = reader.GetInt32("patient_id");
                existing.Username = reader.GetString("username");
                existing.Password = reader.GetString("password");
                existing.Email = reader.GetString("email");
            }

            DbConnection.Close();
            return existing;
        }
        #endregion


        #region Doctor

        public void UpdateDoctor(Doctor model)
        {

            DbConnection.Open();
            MySqlCommand command = DbConnection.CreateCommand();
            command.CommandText = "UPDATE doctor SET first_name=@firstName, last_name=@lastName, phone=@phone, gender=@gender, picture=@picture WHERE `doctor_id`=@doctorID";
            command.Parameters.AddWithValue("@firstName", model.FirstName);
            command.Parameters.AddWithValue("@lastName", model.LastName);
            command.Parameters.AddWithValue("@phone", model.PhoneNumber);
            command.Parameters.AddWithValue("@gender", model.Gender);
            command.Parameters.AddWithValue("@picture", model.Picture);
            command.Parameters.AddWithValue("@doctorID", model.DoctorLogin.DoctorID);
            command.ExecuteNonQuery();
            DbConnection.Close();
        }

        public Doctor FindOneInDoctor(string columnName, string value)
        {
            DbConnection.Open();
            MySqlCommand command = DbConnection.CreateCommand();
            command.CommandText = "SELECT *,length(picture) as p_size FROM doctor WHERE " + columnName + "=@value";
            command.Parameters.AddWithValue("@value", value);
            MySqlDataReader reader = command.ExecuteReader();
            Doctor existing = null;

            if (reader.Read())
            {
                int doctorID = reader.GetInt32("doctor_id");
                string firstName = reader.GetString("first_name");
                string lastName = reader.GetString("last_name");
                string phoneNumber = reader.GetString("phone");
                string gender = reader.GetString("gender");
                byte[] picture = new byte[reader.GetUInt32("p_size")];
                if (picture.Length > 8)
                    reader.GetBytes(reader.GetOrdinal("picture"), 0, picture, 0, picture.Length);
                else
                    picture = File.ReadAllBytes(HttpContext.Current.Server.MapPath("~/Content/empty_profile.gif"));
                DbConnection.Close();
                existing = new Doctor(FindOneInDoctorLogin("doctor_id", doctorID.ToString()));
                existing.FirstName = firstName;
                existing.LastName = lastName;
                existing.PhoneNumber = phoneNumber;
                existing.Gender = gender;
                existing.Picture = picture;
            }
            else
                DbConnection.Close();

            return existing;
        }
        #endregion

        #region Patient

        public void UpdatePatient(Patient model)
        {

            DbConnection.Open();
            MySqlCommand command = DbConnection.CreateCommand();
            command.CommandText = "UPDATE patient SET first_name=@firstName, last_name=@lastName, phone=@phone, birthdate=@birthdate, gender=@gender, picture=@picture WHERE `patient_id`=@patientID";
            command.Parameters.AddWithValue("@firstName", model.FirstName);
            command.Parameters.AddWithValue("@lastName", model.LastName);
            command.Parameters.AddWithValue("@phone", model.PhoneNumber);
            command.Parameters.AddWithValue("@birthdate",model.Birthdate);
            command.Parameters.AddWithValue("@gender", model.Gender);
            command.Parameters.AddWithValue("@picture", model.Picture);
            command.Parameters.AddWithValue("@patientID", model.PatientLogin.PatientID);
            command.ExecuteNonQuery();
            DbConnection.Close();
        }

        public Patient FindOneInPatient(string columnName, string value)
        {
            DbConnection.Open();
            MySqlCommand command = DbConnection.CreateCommand();
            command.CommandText = "SELECT *,length(picture) as p_size FROM patient WHERE " + columnName + "=@value";
            command.Parameters.AddWithValue("@value", value);
            MySqlDataReader reader = command.ExecuteReader();
            Patient existing = null;

            if (reader.Read())
            {
                int patiemtID = reader.GetInt32("patient_id");
                string firstName = reader.GetString("first_name");
                string lastName = reader.GetString("last_name");
                string phoneNumber = reader.GetString("phone");
                DateTime birthdate = reader.GetDateTime("birthdate");
                string gender = reader.GetString("gender");
                byte[] picture = new byte[reader.GetUInt32("p_size")];
                if (picture.Length > 8)
                    reader.GetBytes(reader.GetOrdinal("picture"), 0, picture, 0, picture.Length);
                else
                    picture = File.ReadAllBytes(HttpContext.Current.Server.MapPath("~/Content/empty_profile.gif"));
                DbConnection.Close();
                existing = new Patient(FindOneInPatientLogin("patient_id", patiemtID.ToString()));
                existing.FirstName = firstName;
                existing.LastName = lastName;
                existing.PhoneNumber = phoneNumber;
                existing.Birthdate = birthdate;
                existing.Gender = gender;
                existing.Picture = picture;
            }
            else
                DbConnection.Close();

            return existing;
        }
        #endregion
    }
}