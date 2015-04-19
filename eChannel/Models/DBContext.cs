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

        #region Hospital

        public Hospital FindOneInHospital(string columnName, string value)
        {

            DbConnection.Open();
            MySqlCommand command = DbConnection.CreateCommand();
            command.CommandText = "SELECT * FROM hospital WHERE " + columnName + "=@value";
            command.Parameters.AddWithValue("@value", value);
            MySqlDataReader reader = command.ExecuteReader();
            Hospital existing = null;
            if (reader.Read())
            {
                existing = new Hospital();
                existing.HospitalID = reader.GetInt32("hospital_id");
                existing.Name = reader.GetString("name");
                existing.Location = reader.GetString("location");
                existing.Phone = reader.GetString("phone");
            }

            DbConnection.Close();
            return existing;
        }

        public List<Hospital> FindAllInHospital(string columnName, string value)
        {

            DbConnection.Open();
            MySqlCommand command = DbConnection.CreateCommand();
            command.CommandText = "SELECT * FROM hospital WHERE " + columnName + "=@value";
            command.Parameters.AddWithValue("@value", value);
            MySqlDataReader reader = command.ExecuteReader();
            List<Hospital> hospitals = new List<Hospital>();
            
            while (reader.Read())
            {             
                Hospital existing = new Hospital();
                existing.HospitalID = reader.GetInt32("hospital_id");
                existing.Name = reader.GetString("name");
                existing.Location = reader.GetString("location");
                existing.Phone = reader.GetString("phone");
                hospitals.Add(existing);
            }

            DbConnection.Close();
            return hospitals;
        }

        public List<Hospital> FindAllInHospital()
        {

            DbConnection.Open();
            MySqlCommand command = DbConnection.CreateCommand();
            command.CommandText = "SELECT * FROM hospital;";
            MySqlDataReader reader = command.ExecuteReader();
            List<Hospital> hospitals = new List<Hospital>();

            while (reader.Read())
            {
                Hospital existing = new Hospital();
                existing.HospitalID = reader.GetInt32("hospital_id");
                existing.Name = reader.GetString("name");
                existing.Location = reader.GetString("location");
                existing.Phone = reader.GetString("phone");
                hospitals.Add(existing);
            }

            DbConnection.Close();
            return hospitals;
        }

        #endregion

        #region Room

        public Room FindOneInRoom(string columnName, string value)
        {

            DbConnection.Open();
            MySqlCommand command = DbConnection.CreateCommand();
            command.CommandText = "SELECT * FROM room WHERE " + columnName + "=@value";
            command.Parameters.AddWithValue("@value", value);
            MySqlDataReader reader = command.ExecuteReader();
            Room existing = null;
            if (reader.Read())
            {
                existing = new Room();
                existing.RoomID = reader.GetInt32("room_id");
                existing.HospitalID = reader.GetInt32("hospital_id");
                existing.RoomName = reader.GetString("room_name");
                existing.Capacity = reader.GetInt32("capacity");
            }

            DbConnection.Close();
            return existing;
        }

        public List<Room> FindAllInRoom(string columnName, string value)
        {

            DbConnection.Open();
            MySqlCommand command = DbConnection.CreateCommand();
            command.CommandText = "SELECT * FROM room WHERE " + columnName + "=@value";
            command.Parameters.AddWithValue("@value", value);
            MySqlDataReader reader = command.ExecuteReader();
            List<Room> rooms = new List<Room>();

            while (reader.Read())
            {            
                Room existing = new Room();
                existing.RoomID = reader.GetInt32("room_id");
                existing.HospitalID = reader.GetInt32("hospital_id");
                existing.RoomName = reader.GetString("room_name");
                existing.Capacity = reader.GetInt32("capacity");
                rooms.Add(existing);
            }

            DbConnection.Close();
            return rooms;
        }

        public List<Room> FindAllInRoom()
        {

            DbConnection.Open();
            MySqlCommand command = DbConnection.CreateCommand();
            command.CommandText = "SELECT * FROM room;";
            
            MySqlDataReader reader = command.ExecuteReader();
            List<Room> rooms = new List<Room>();

            while (reader.Read())
            {
                Room existing = new Room();
                existing.RoomID = reader.GetInt32("room_id");
                existing.HospitalID = reader.GetInt32("hospital_id");
                existing.RoomName = reader.GetString("room_name");
                existing.Capacity = reader.GetInt32("capacity");
                rooms.Add(existing);
            }

            DbConnection.Close();
            return rooms;
        }

        #endregion

        #region RoomWork

        public void CreateRoomWork(RoomWork model)
        {

            DbConnection.Open();
            MySqlCommand command = DbConnection.CreateCommand();
            command.CommandText = "INSERT INTO room_work(doctor_id,room_id,start_datetime,end_datetime,max_channels) VALUES(@doctor_id, @room_id,@start_datetime,@end_datetime,@max_channels)";
            command.Parameters.AddWithValue("@doctor_id", model.DoctorID);
            command.Parameters.AddWithValue("@room_id", model.RoomID);
            command.Parameters.AddWithValue("@start_datetime", model.StartDateTime);
            command.Parameters.AddWithValue("@end_datetime", model.EndDateTime);
            command.Parameters.AddWithValue("@max_channels", model.MaxChannels);
            command.ExecuteNonQuery();
            DbConnection.Close();
        }

        public RoomWork FindOneInRoomWork(string columnName, string value)
        {

            DbConnection.Open();
            MySqlCommand command = DbConnection.CreateCommand();
            command.CommandText = "SELECT * FROM room_work WHERE " + columnName + "=@value";
            command.Parameters.AddWithValue("@value", value);
            MySqlDataReader reader = command.ExecuteReader();
            RoomWork existing = null;
            if (reader.Read())
            {
                existing = new RoomWork();
                existing.DoctorID = reader.GetInt32("doctor_id");
                existing.RoomID = reader.GetInt32("room_id");
                existing.StartDateTime = reader.GetDateTime("start_datetime");
                existing.EndDateTime = reader.GetDateTime("end_datetime");
                existing.MaxChannels = reader.GetInt32("max_channels");
            }

            DbConnection.Close();
            return existing;
        }

        public List<RoomWork> FindAllInRoomWork(string columnName, string value)
        {

            DbConnection.Open();
            MySqlCommand command = DbConnection.CreateCommand();
            command.CommandText = "SELECT * FROM room_work WHERE " + columnName + "=@value";
            command.Parameters.AddWithValue("@value", value);
            MySqlDataReader reader = command.ExecuteReader();
            List<RoomWork> roomWorks = new List<RoomWork>();
            while (reader.Read())
            {
                RoomWork existing = new RoomWork();
                existing.DoctorID = reader.GetInt32("doctor_id");
                existing.RoomID = reader.GetInt32("room_id");
                existing.StartDateTime = reader.GetDateTime("start_datetime");
                existing.EndDateTime = reader.GetDateTime("end_datetime");
                existing.MaxChannels = reader.GetInt32("max_channels");
                roomWorks.Add(existing);
            }

            DbConnection.Close();
            return roomWorks;
        }

        #endregion

        #region DoctorSchedule
        public List<DoctorSchedule> FindAllDoctorSchedule(int doctorID)
        {

            DbConnection.Open();
            MySqlCommand command = DbConnection.CreateCommand();
            command.CommandText = "SELECT room_work.work_id,CONCAT(doctor.first_name,' ',doctor.last_name) AS doctor_name,hospital.name AS hospital_name,room.room_name,start_datetime,end_datetime,max_channels,COUNT(channel.work_id) AS applied_patients " +
                "FROM e_channel.room_work " +
                "LEFT JOIN e_channel.room ON room_work.room_id=room.room_id " +
                "JOIN e_channel.hospital ON room.hospital_id=hospital.hospital_id " +
                "JOIN e_channel.doctor ON room_work.doctor_id=doctor.doctor_id " +
                "LEFT JOIN e_channel.channel ON (room_work.work_id = channel.work_id) " +
                "WHERE room_work.doctor_id=@value GROUP BY room_work.work_id";
            command.Parameters.AddWithValue("@value", doctorID);
            MySqlDataReader reader = command.ExecuteReader();
            List<DoctorSchedule> doctorSchedules = new List<DoctorSchedule>();
            while (reader.Read())
            {
                DoctorSchedule existing = new DoctorSchedule();
                existing.WorkID = reader.GetInt32("work_id");
                existing.DoctorName = reader.GetString("doctor_name");
                existing.HospitalName = reader.GetString("hospital_name");
                existing.RoomName = reader.GetString("room_name");
                existing.StartDateTime = reader.GetDateTime("start_datetime");
                existing.EndDateTime = reader.GetDateTime("end_datetime");
                existing.MaxChannels = reader.GetInt32("max_channels");
                existing.PatientApplied = reader.GetInt32("applied_patients");
                doctorSchedules.Add(existing);
            }

            DbConnection.Close();
            return doctorSchedules;
        }

        #endregion
    }
}