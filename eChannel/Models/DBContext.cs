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
            command.Parameters.AddWithValue("@birthdate", model.Birthdate);
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

        public List<DoctorSchedule> FindAllDoctorScheduleBySpecializationID(int specializationID)
        {

            DbConnection.Open();
            MySqlCommand command = DbConnection.CreateCommand();
            command.CommandText = "SELECT * FROM doctor_specialization WHERE specialization_id=@value;";
            command.Parameters.AddWithValue("@value", specializationID);
            MySqlDataReader reader = command.ExecuteReader();
            List<int> doctorIDs = new List<int>();
            while (reader.Read())
            {
                doctorIDs.Add(reader.GetInt32("doctor_id"));
            }
            DbConnection.Close();

            List<DoctorSchedule> doctorSchedules = new List<DoctorSchedule>();
            for (int i = 0; i < doctorIDs.Count; i++)
            {
                doctorSchedules.AddRange(FindAllDoctorSchedule(doctorIDs[i]));
            }

            DbConnection.Close();
            return doctorSchedules;
        }

        public DoctorSchedule FindOneInDoctorSchedule(int workID)
        {

            DbConnection.Open();
            MySqlCommand command = DbConnection.CreateCommand();
            command.CommandText = "SELECT room_work.work_id,CONCAT(doctor.first_name,' ',doctor.last_name) AS doctor_name,hospital.name AS hospital_name,room.room_name,start_datetime,end_datetime,max_channels,COUNT(channel.work_id) AS applied_patients " +
                "FROM e_channel.room_work " +
                "LEFT JOIN e_channel.room ON room_work.room_id=room.room_id " +
                "JOIN e_channel.hospital ON room.hospital_id=hospital.hospital_id " +
                "JOIN e_channel.doctor ON room_work.doctor_id=doctor.doctor_id " +
                "LEFT JOIN e_channel.channel ON (room_work.work_id = channel.work_id) " +
                "WHERE room_work.work_id=@value GROUP BY room_work.work_id";
            command.Parameters.AddWithValue("@value", workID);
            MySqlDataReader reader = command.ExecuteReader();
            DoctorSchedule existing = null;
            if (reader.Read())
            {
                existing = new DoctorSchedule();
                existing.WorkID = reader.GetInt32("work_id");
                existing.DoctorName = reader.GetString("doctor_name");
                existing.HospitalName = reader.GetString("hospital_name");
                existing.RoomName = reader.GetString("room_name");
                existing.StartDateTime = reader.GetDateTime("start_datetime");
                existing.EndDateTime = reader.GetDateTime("end_datetime");
                existing.MaxChannels = reader.GetInt32("max_channels");
                existing.PatientApplied = reader.GetInt32("applied_patients");

            }

            DbConnection.Close();
            return existing;
        }

        #endregion

        #region Specialization

        public Specialization FindOneInSpecialization(string columnName, string value)
        {

            DbConnection.Open();
            MySqlCommand command = DbConnection.CreateCommand();
            command.CommandText = "SELECT * FROM specialization WHERE " + columnName + "=@value";
            command.Parameters.AddWithValue("@value", value);
            MySqlDataReader reader = command.ExecuteReader();
            Specialization existing = null;
            if (reader.Read())
            {
                existing = new Specialization();
                existing.SpecID = reader.GetInt32("specialization_id");
                existing.SpecType = reader.GetString("specialization_type");
                existing.SpecDegree = reader.GetString("degree");

            }

            DbConnection.Close();
            return existing;
        }

        public List<Specialization> FindAllInSpecialization(string columnName, string value)
        {

            DbConnection.Open();
            MySqlCommand command = DbConnection.CreateCommand();
            command.CommandText = "SELECT * FROM specialization WHERE " + columnName + "=@value";
            command.Parameters.AddWithValue("@value", value);
            MySqlDataReader reader = command.ExecuteReader();
            List<Specialization> specializations = new List<Specialization>();

            while (reader.Read())
            {
                Specialization existing = new Specialization();
                existing.SpecID = reader.GetInt32("specialization_id");
                existing.SpecType = reader.GetString("specialization_type");
                existing.SpecDegree = reader.GetString("degree");
                specializations.Add(existing);
            }

            DbConnection.Close();
            return specializations;
        }

        public List<Specialization> FindAllInSpecialization()
        {

            DbConnection.Open();
            MySqlCommand command = DbConnection.CreateCommand();
            command.CommandText = "SELECT * FROM specialization";
            MySqlDataReader reader = command.ExecuteReader();
            List<Specialization> specializations = new List<Specialization>();

            while (reader.Read())
            {
                Specialization existing = new Specialization();
                existing.SpecID = reader.GetInt32("specialization_id");
                existing.SpecType = reader.GetString("specialization_type");
                existing.SpecDegree = reader.GetString("degree");
                specializations.Add(existing);
            }

            DbConnection.Close();
            return specializations;
        }

        #endregion

        #region Service

        public Service FindOneInService(string columnName, string value)
        {

            DbConnection.Open();
            MySqlCommand command = DbConnection.CreateCommand();
            command.CommandText = "SELECT * FROM service WHERE " + columnName + "=@value";
            command.Parameters.AddWithValue("@value", value);
            MySqlDataReader reader = command.ExecuteReader();
            Service existing = null;
            if (reader.Read())
            {
                existing = new Service();
                existing.ServiceID = reader.GetInt32("service_id");
                existing.ServiceName = reader.GetString("service_name");
            }

            DbConnection.Close();
            return existing;
        }

        public List<Service> FindAllInService(string columnName, string value)
        {

            DbConnection.Open();
            MySqlCommand command = DbConnection.CreateCommand();
            command.CommandText = "SELECT * FROM service WHERE " + columnName + "=@value";
            command.Parameters.AddWithValue("@value", value);
            MySqlDataReader reader = command.ExecuteReader();
            List<Service> services = new List<Service>();

            while (reader.Read())
            {
                Service existing = new Service();
                existing.ServiceID = reader.GetInt32("service_id");
                existing.ServiceName = reader.GetString("service_name");
                services.Add(existing);
            }

            DbConnection.Close();
            return services;
        }

        public List<Service> FindAllInService()
        {

            DbConnection.Open();
            MySqlCommand command = DbConnection.CreateCommand();
            command.CommandText = "SELECT * FROM service";
            MySqlDataReader reader = command.ExecuteReader();
            List<Service> services = new List<Service>();
            while (reader.Read())
            {
                Service existing = new Service();
                existing.ServiceID = reader.GetInt32("service_id");
                existing.ServiceName = reader.GetString("service_name");
                services.Add(existing);
            }

            DbConnection.Close();
            return services;
        }

        #endregion


        #region Channel

        public void CreateChannel(Channel model)
        {

            DbConnection.Open();
            MySqlCommand command = DbConnection.CreateCommand();
            command.CommandText = "INSERT INTO channel(work_id, patient_id, specialization_id, service_id, channel_number, reason, channel_rating, channel_comments) VALUES(@work_id, @patient_id, @specialization_id, @service_id, @channel_number, @reason, @channel_rating, @channel_comments)";
            command.Parameters.AddWithValue("@work_id", model.WorkID);
            command.Parameters.AddWithValue("@patient_id", model.PatientID);
            command.Parameters.AddWithValue("@specialization_id", model.SpecID);
            command.Parameters.AddWithValue("@service_id", model.ServiceID);
            command.Parameters.AddWithValue("@channel_number", model.ChannelNumber);
            command.Parameters.AddWithValue("@reason", model.Reason);
            command.Parameters.AddWithValue("@channel_rating", model.ChannelRating);
            command.Parameters.AddWithValue("@channel_comments", model.ChannelComments);
            command.ExecuteNonQuery();
            DbConnection.Close();
        }

        public void UpdateChannel(Channel model)
        {

            DbConnection.Open();
            MySqlCommand command = DbConnection.CreateCommand();
            command.CommandText = "UPDATE channel SET work_id=@work_id, patient_id=@patient_id, specialization_id=@specialization_id, service_id=@service_id, channel_number=@channel_number, reason=@reason, channel_rating=@channel_rating, channel_comments=@channel_comments WHERE `channel_id`=@channel_id";
            command.Parameters.AddWithValue("@work_id", model.WorkID);
            command.Parameters.AddWithValue("@patient_id", model.PatientID);
            command.Parameters.AddWithValue("@specialization_id", model.SpecID);
            command.Parameters.AddWithValue("@service_id", model.ServiceID);
            command.Parameters.AddWithValue("@channel_number", model.ChannelNumber);
            command.Parameters.AddWithValue("@reason", model.Reason);
            command.Parameters.AddWithValue("@channel_rating", model.ChannelRating);
            command.Parameters.AddWithValue("@channel_comments", model.ChannelComments);
            command.Parameters.AddWithValue("@channel_id", model.ChannelID);
            command.ExecuteNonQuery();
            DbConnection.Close();
        }

        public Channel FindOneInChannel(string columnName, string value)
        {

            DbConnection.Open();
            MySqlCommand command = DbConnection.CreateCommand();
            command.CommandText = "SELECT * FROM channel WHERE " + columnName + "=@value";
            command.Parameters.AddWithValue("@value", value);
            MySqlDataReader reader = command.ExecuteReader();
            Channel existing = null;
            if (reader.Read())
            {
                existing = new Channel();
                existing.ChannelID = reader.GetInt32("channel_id");
                existing.WorkID = reader.GetInt32("work_id");
                existing.PatientID = reader.GetInt32("patient_id");
                existing.SpecID = reader.GetInt32("specialization_id");
                existing.ServiceID = reader.GetInt32("service_id");
                existing.ChannelNumber = reader.GetInt32("channel_number");
                existing.Reason = reader.GetString("reason");
                existing.ChannelRating = reader.GetInt32("channel_rating");
                existing.ChannelComments = reader.GetString("channel_comments");
            }

            DbConnection.Close();
            return existing;
        }

        public List<Channel> FindAllInChannel(string columnName, string value)
        {

            DbConnection.Open();
            MySqlCommand command = DbConnection.CreateCommand();
            command.CommandText = "SELECT * FROM channel WHERE " + columnName + "=@value";
            command.Parameters.AddWithValue("@value", value);
            MySqlDataReader reader = command.ExecuteReader();
            List<Channel> channels = new List<Channel>();
            while (reader.Read())
            {
                Channel existing = new Channel();
                existing.ChannelID = reader.GetInt32("channel_id");
                existing.WorkID = reader.GetInt32("work_id");
                existing.PatientID = reader.GetInt32("patient_id");
                existing.SpecID = reader.GetInt32("specialization_id");
                existing.ServiceID = reader.GetInt32("service_id");
                existing.ChannelNumber = reader.GetInt32("channel_number");
                existing.Reason = reader.GetString("reason");
                existing.ChannelRating = reader.GetInt32("channel_rating");
                existing.ChannelComments = reader.GetString("channel_comments");
                channels.Add(existing);
            }

            DbConnection.Close();
            return channels;
        }

        public List<Channel> FindAllInChannel()
        {

            DbConnection.Open();
            MySqlCommand command = DbConnection.CreateCommand();
            command.CommandText = "SELECT * FROM channel;";
            MySqlDataReader reader = command.ExecuteReader();
            List<Channel> channels = new List<Channel>();
            while (reader.Read())
            {
                Channel existing = new Channel();
                existing.ChannelID = reader.GetInt32("channel_id");
                existing.WorkID = reader.GetInt32("work_id");
                existing.PatientID = reader.GetInt32("patient_id");
                existing.SpecID = reader.GetInt32("specialization_id");
                existing.ServiceID = reader.GetInt32("service_id");
                existing.ChannelNumber = reader.GetInt32("channel_number");
                existing.Reason = reader.GetString("reason");
                existing.ChannelRating = reader.GetInt32("channel_rating");
                existing.ChannelComments = reader.GetString("channel_comments");
                channels.Add(existing);
            }

            DbConnection.Close();
            return channels;
        }

        #endregion

        #region DoctorChannel
        public List<DoctorChannel> FindAllInDoctorChannel(int doctorID)
        {

            DbConnection.Open();
            MySqlCommand command = DbConnection.CreateCommand();
            command.CommandText = "SELECT channel_id,work_id, CONCAT(first_name,' ',last_name) AS full_name,specialization_type,service_name,channel_number,reason,channel_rating,channel_comments FROM e_channel.channel " +
                "LEFT JOIN patient ON channel.patient_id=patient.patient_id " +
                "JOIN specialization ON channel.specialization_id=specialization.specialization_id " +
                "JOIN service ON channel.service_id=service.service_id " +
                "WHERE work_id IN (SELECT room_work.work_id FROM room_work WHERE doctor_id =@doctorID);";
            command.Parameters.AddWithValue("@doctorID", doctorID);
            MySqlDataReader reader = command.ExecuteReader();
            List<DoctorChannel> doctorChannels = new List<DoctorChannel>();
            while (reader.Read())
            {
                DoctorChannel existing = new DoctorChannel();
                existing.ChannelID = reader.GetInt32("channel_id");
                existing.WorkID = reader.GetInt32("work_id");
                existing.PatientFullName = reader.GetString("full_name");
                existing.Spec = reader.GetString("specialization_type");
                existing.Service = reader.GetString("service_name");
                existing.ChannelNumber = reader.GetInt32("channel_number");
                existing.Reason = reader.GetString("reason");
                existing.ChannelRating = reader.GetInt32("channel_rating");
                existing.ChannelComments = reader.GetString("channel_comments");
                doctorChannels.Add(existing);
            }

            DbConnection.Close();
            return doctorChannels;
        }

        public List<DoctorChannel> FindAllInDoctorChannel(int doctorID, string keyword, string column)
        {

            DbConnection.Open();
            MySqlCommand command = DbConnection.CreateCommand();
            command.CommandText = "SELECT channel_id,work_id, CONCAT(first_name,' ',last_name) AS full_name,specialization_type,service_name,channel_number,reason,channel_rating,channel_comments FROM e_channel.channel " +
                "LEFT JOIN patient ON channel.patient_id=patient.patient_id " +
                "JOIN specialization ON channel.specialization_id=specialization.specialization_id " +
                "JOIN service ON channel.service_id=service.service_id " +
                "WHERE work_id IN (SELECT room_work.work_id FROM room_work WHERE doctor_id =@doctorID) AND " +
                column + " LIKE @key;";
            command.Parameters.AddWithValue("@doctorID", doctorID);
            command.Parameters.AddWithValue("@key", "%" + keyword + "%");
            MySqlDataReader reader = command.ExecuteReader();
            List<DoctorChannel> doctorChannels = new List<DoctorChannel>();
            while (reader.Read())
            {
                DoctorChannel existing = new DoctorChannel();
                existing.ChannelID = reader.GetInt32("channel_id");
                existing.WorkID = reader.GetInt32("work_id");
                existing.PatientFullName = reader.GetString("full_name");
                existing.Spec = reader.GetString("specialization_type");
                existing.Service = reader.GetString("service_name");
                existing.ChannelNumber = reader.GetInt32("channel_number");
                existing.Reason = reader.GetString("reason");
                existing.ChannelRating = reader.GetInt32("channel_rating");
                existing.ChannelComments = reader.GetString("channel_comments");
                doctorChannels.Add(existing);
            }

            DbConnection.Close();
            return doctorChannels;
        }
        #endregion

        #region PatientChannel
        public List<PatientChannel> FindAllInPatientChannel(int patientID)
        {

            DbConnection.Open();
            MySqlCommand command = DbConnection.CreateCommand();
            command.CommandText = "SELECT channel_id,channel.work_id,specialization_type,service_name,channel_number,reason,channel_rating,channel_comments,doctor.doctor_id,CONCAT(doctor.first_name,' ',doctor.last_name) AS full_name,hospital.hospital_id,hospital.name,hospital.location FROM e_channel.channel " +
                "LEFT JOIN room_work ON channel.work_id=room_work.work_id " +
                "JOIN doctor ON doctor.doctor_id=room_work.doctor_id " +
                "JOIN room ON room.room_id=room_work.room_id " +
                "JOIN hospital ON hospital.hospital_id=room.hospital_id " +
                "JOIN specialization ON channel.specialization_id=specialization.specialization_id " +
                "JOIN service ON channel.service_id=service.service_id " +
                "WHERE channel.patient_id=@patientID ORDER BY channel_id DESC;";
            command.Parameters.AddWithValue("@patientID", patientID);
            MySqlDataReader reader = command.ExecuteReader();
            List<PatientChannel> patientChannels = new List<PatientChannel>();
            while (reader.Read())
            {
                PatientChannel existing = new PatientChannel();
                existing.ChannelID = reader.GetInt32("channel_id");
                existing.WorkID = reader.GetInt32("work_id");
                existing.Spec = reader.GetString("specialization_type");
                existing.Service = reader.GetString("service_name");
                existing.ChannelNumber = reader.GetInt32("channel_number");
                existing.Reason = reader.GetString("reason");
                existing.ChannelRating = reader.GetInt32("channel_rating");
                existing.ChannelComments = reader.GetString("channel_comments");
                existing.DoctorID = reader.GetInt32("doctor_id");
                existing.DoctorFullName = reader.GetString("full_name");
                existing.HospitalID = reader.GetInt32("hospital_id");
                existing.HospitalName = reader.GetString("name");
                existing.HospitalLocation = reader.GetString("location");
                patientChannels.Add(existing);
            }

            DbConnection.Close();
            return patientChannels;
        }


        public List<PatientChannel> FindAllInPatientChannel(int patientID, string keyword, string column)
        {

            DbConnection.Open();
            MySqlCommand command = DbConnection.CreateCommand();
            command.CommandText = "SELECT channel_id,channel.work_id,specialization_type,service_name,channel_number,reason,channel_rating,channel_comments,doctor.doctor_id,CONCAT(doctor.first_name,' ',doctor.last_name) AS full_name,hospital.hospital_id,hospital.name,hospital.location FROM e_channel.channel " +
                "LEFT JOIN room_work ON channel.work_id=room_work.work_id " +
                "JOIN doctor ON doctor.doctor_id=room_work.doctor_id " +
                "JOIN room ON room.room_id=room_work.room_id " +
                "JOIN hospital ON hospital.hospital_id=room.hospital_id " +
                "JOIN specialization ON channel.specialization_id=specialization.specialization_id " +
                "JOIN service ON channel.service_id=service.service_id " +
                "WHERE channel.patient_id=@patientID " +
                "AND " + column + " LIKE @key " +
                "ORDER BY channel_id DESC;";
            command.Parameters.AddWithValue("@patientID", patientID);
            command.Parameters.AddWithValue("@key", "%" + keyword + "%");
            MySqlDataReader reader = command.ExecuteReader();
            List<PatientChannel> patientChannels = new List<PatientChannel>();
            while (reader.Read())
            {
                PatientChannel existing = new PatientChannel();
                existing.ChannelID = reader.GetInt32("channel_id");
                existing.WorkID = reader.GetInt32("work_id");
                existing.Spec = reader.GetString("specialization_type");
                existing.Service = reader.GetString("service_name");
                existing.ChannelNumber = reader.GetInt32("channel_number");
                existing.Reason = reader.GetString("reason");
                existing.ChannelRating = reader.GetInt32("channel_rating");
                existing.ChannelComments = reader.GetString("channel_comments");
                existing.DoctorID = reader.GetInt32("doctor_id");
                existing.DoctorFullName = reader.GetString("full_name");
                existing.HospitalID = reader.GetInt32("hospital_id");
                existing.HospitalName = reader.GetString("name");
                existing.HospitalLocation = reader.GetString("location");
                patientChannels.Add(existing);
            }

            DbConnection.Close();
            return patientChannels;
        }
        #endregion

        #region DoctorDetails

        public List<DoctorDetail> FindAllInDoctorDetails()
        {

            DbConnection.Open();
            MySqlCommand command = DbConnection.CreateCommand();
            command.CommandText = "SELECT *,length(picture) as p_size FROM e_channel.doctor_details ORDER BY first_name,last_name;";
            MySqlDataReader reader = command.ExecuteReader();
            List<DoctorDetail> doctorChannels = new List<DoctorDetail>();
            while (reader.Read())
            {
                DoctorDetail existing = new DoctorDetail();
                existing.DoctorID = reader.GetInt32("doctor_id");
                existing.FirstName = reader.GetString("first_name");
                existing.LastName = reader.GetString("last_name");
                existing.PhoneNumber = reader.GetString("phone");
                existing.Gender = reader.GetString("gender");
                existing.Email = reader.GetString("email");
                existing.AverageRating = reader.GetFloat("avg_rating");

                byte[] picture = new byte[reader.GetUInt32("p_size")];
                reader.GetBytes(reader.GetOrdinal("picture"), 0, picture, 0, picture.Length);
                existing.Picture = picture;

                doctorChannels.Add(existing);
            }

            DbConnection.Close();
            return doctorChannels;
        }

        public List<DoctorDetail> FindAllInDoctorDetails(string keyword, string column)
        {

            DbConnection.Open();
            MySqlCommand command = DbConnection.CreateCommand();
            if (column.Equals("doctor_id"))
            {
                command.CommandText = "SELECT *,length(picture) as p_size FROM e_channel.doctor_details " +
                   "WHERE " + column + " = @key " +
                   "ORDER BY first_name,last_name;";
                command.Parameters.AddWithValue("@key", keyword);
            }
            else
            {
                command.CommandText = "SELECT *,length(picture) as p_size FROM e_channel.doctor_details " +
                    "WHERE " + column + " LIKE @key " +
                    "ORDER BY first_name,last_name;";
                command.Parameters.AddWithValue("@key", "%" + keyword + "%");
            }
            MySqlDataReader reader = command.ExecuteReader();
            List<DoctorDetail> doctorChannels = new List<DoctorDetail>();
            while (reader.Read())
            {
                DoctorDetail existing = new DoctorDetail();
                existing.DoctorID = reader.GetInt32("doctor_id");
                existing.FirstName = reader.GetString("first_name");
                existing.LastName = reader.GetString("last_name");
                existing.PhoneNumber = reader.GetString("phone");
                existing.Gender = reader.GetString("gender");
                existing.Email = reader.GetString("email");
                existing.AverageRating = reader.GetFloat("avg_rating");

                byte[] picture = new byte[reader.GetUInt32("p_size")];
                reader.GetBytes(reader.GetOrdinal("picture"), 0, picture, 0, picture.Length);
                existing.Picture = picture;

                doctorChannels.Add(existing);
            }

            DbConnection.Close();
            return doctorChannels;
        }

        #endregion

        #region DoctorSpecialization

        public List<DoctorSpecialization> FindAllInDoctorSpecialization(string keyword, string column)
        {

            DbConnection.Open();
            MySqlCommand command = DbConnection.CreateCommand();
            command.CommandText = "SELECT doctor_specialization.specialization_id,doctor_id,university,specialization_type,degree FROM doctor_specialization " +
                "LEFT JOIN specialization ON specialization.specialization_id=doctor_specialization.specialization_id " +
                "WHERE " + column + " LIKE @key;";
            command.Parameters.AddWithValue("@key", "%" + keyword + "%");
            MySqlDataReader reader = command.ExecuteReader();
            List<DoctorSpecialization> doctorChannels = new List<DoctorSpecialization>();
            while (reader.Read())
            {
                DoctorSpecialization existing = new DoctorSpecialization();
                existing.DoctorID = reader.GetInt32("doctor_id");
                existing.SpecID = reader.GetInt32("specialization_id");
                existing.University = reader.GetString("university");
                existing.SpecType = reader.GetString("specialization_type");
                existing.SpecDegree = reader.GetString("degree");
                doctorChannels.Add(existing);
            }

            DbConnection.Close();
            return doctorChannels;
        }

        #endregion
    }
}