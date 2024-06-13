using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Windows;

namespace ApplicationManagementSystem
{
    public class DatabaseManager
    {
        private const string ConnectionString = "Data Source=DatabaseOfCollege.db;Version=3;";

        public void CreateUser(string username, string password, string role)
        {
            using (SQLiteConnection connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();
                string checkIfExistsSql = "SELECT COUNT(*) FROM Users WHERE username = @username";
                using (SQLiteCommand checkCommand = new SQLiteCommand(checkIfExistsSql, connection))
                {
                    checkCommand.Parameters.AddWithValue("@username", username);
                    int count = Convert.ToInt32(checkCommand.ExecuteScalar());
                    if (count > 0)
                    {
                        throw new Exception("Пользователь с таким username уже существует");
                    }
                }

                string insertSql = "INSERT INTO Users (username, password, role) VALUES (@username, @password, @role)";
                using (SQLiteCommand insertCommand = new SQLiteCommand(insertSql, connection))
                {
                    insertCommand.Parameters.AddWithValue("@username", username);
                    insertCommand.Parameters.AddWithValue("@password", password);
                    insertCommand.Parameters.AddWithValue("@role", role);
                    insertCommand.ExecuteNonQuery();
                }
            }
        }

        public void AddAbiturent(string fullname, string passportnumber, string phonenumber, string averagegrade, string specialty, string status, string examsStatus)
        {
            using (SQLiteConnection connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();
                string insertSql = "INSERT INTO Applications (FullName, PassportNumber, PhoneNumber, AverageGrade, Specialty, Status, ExamsStatus) VALUES (@FullName, @PassportNumber, @PhoneNumber, @AverageGrade, @Specialty, @Status, @ExamsStatus)";
                using (SQLiteCommand insertCommand = new SQLiteCommand(insertSql, connection))
                {
                    insertCommand.Parameters.AddWithValue("@FullName", fullname);
                    insertCommand.Parameters.AddWithValue("@PassportNumber", passportnumber);
                    insertCommand.Parameters.AddWithValue("@PhoneNumber", phonenumber);
                    insertCommand.Parameters.AddWithValue("@AverageGrade", averagegrade);
                    insertCommand.Parameters.AddWithValue("@Specialty", specialty);
                    insertCommand.Parameters.AddWithValue("@Status", status);
                    insertCommand.Parameters.AddWithValue("@ExamsStatus", examsStatus);
                    insertCommand.ExecuteNonQuery();
                }
            }
        }


        public bool AuthenticateUser(string username, string password)
        {
            using (SQLiteConnection connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();
                string sql = "SELECT COUNT(*) FROM Users WHERE username = @username AND password = @password";
                using (SQLiteCommand command = new SQLiteCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@username", username);
                    command.Parameters.AddWithValue("@password", password);
                    int count = Convert.ToInt32(command.ExecuteScalar());
                    return count > 0;
                }
            }
        }

        public string GetUserRoleByUsername(string username)
        {
            string role = null;
            using (SQLiteConnection connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();
                string sql = "SELECT role FROM Users WHERE username = @username";
                using (SQLiteCommand command = new SQLiteCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@username", username);
                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            role = reader["role"].ToString();
                        }
                    }
                }
            }
            return role;
        }

        public List<Abiturent> GetAllApplications()
        {
            List<Abiturent> abiturents = new List<Abiturent>();

            using (SQLiteConnection connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();
                string sql = "SELECT * FROM Applications";
                using (SQLiteCommand command = new SQLiteCommand(sql, connection))
                {
                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Abiturent abiturent = new Abiturent
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("id")),
                                FullName = reader.GetString(reader.GetOrdinal("FullName")),
                                PassportNumber = reader.GetString(reader.GetOrdinal("PassportNumber")),
                                PhoneNumber = reader.GetString(reader.GetOrdinal("PhoneNumber")),
                                AverageGrade = reader.GetDouble(reader.GetOrdinal("AverageGrade")),
                                Specialty = reader.GetString(reader.GetOrdinal("Specialty")),
                                Status = reader.GetString(reader.GetOrdinal("Status")),
                                ExamsStatus = reader.GetString(reader.GetOrdinal("ExamsStatus"))
                            };
                            abiturents.Add(abiturent);
                        }
                    }
                }
            }
            return abiturents;
        }

        public List<Examination> GetAllExams()
        {
            List<Examination> exams = new List<Examination>();

            using (SQLiteConnection connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();
                string sql = "SELECT * FROM Exams";
                using (SQLiteCommand command = new SQLiteCommand(sql, connection))
                {
                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Examination exam = new Examination
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("id")),
                                SpecialtyName = reader.GetString(reader.GetOrdinal("SpecialtyName")),
                                FullName = reader.GetString(reader.GetOrdinal("FullName")),
                                ExamDate = reader.GetDateTime(reader.GetOrdinal("ExamDate")),
                                Observer = reader.GetString(reader.GetOrdinal("Observer")),
                                Results = reader.GetInt32(reader.GetOrdinal("Results"))
                            };
                            exams.Add(exam);
                        }
                    }
                }
            }
            return exams;
        }


        public void UpdateStatus(int userId, string newStatus)
        {
            using (SQLiteConnection connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();

                string updateSql = "UPDATE Applications SET Status = @NewStatus WHERE id = @UserId";

                using (SQLiteCommand updateCommand = new SQLiteCommand(updateSql, connection))
                {
                    updateCommand.Parameters.AddWithValue("@NewStatus", newStatus);
                    updateCommand.Parameters.AddWithValue("@UserId", userId);
                    updateCommand.ExecuteNonQuery();
                }
            }
        }

        public void ReductionQuantityBudgetPlaces(string specialtyName)
        {
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();
                string selectQuery = "SELECT * FROM Specialties WHERE Name=@Name";
                using (var command = new SQLiteCommand(selectQuery, connection))
                {
                    command.Parameters.AddWithValue("@Name", specialtyName);
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            int budgetPlaces = reader.GetInt32(2);
                            if (budgetPlaces > 0)
                            {
                                int updatedBudgetPlaces = budgetPlaces - 1;
                                string updateQuery = "UPDATE Specialties SET BudgetPlaces=@BudgetPlaces WHERE id=@Id";
                                using (var updateCommand = new SQLiteCommand(updateQuery, connection))
                                {
                                    updateCommand.Parameters.AddWithValue("@BudgetPlaces", updatedBudgetPlaces);
                                    updateCommand.Parameters.AddWithValue("@Id", reader.GetInt32(0));
                                    updateCommand.ExecuteNonQuery();
                                }
                            }
                            else
                            {
                                MessageBox.Show("Нет доступных бюджетных мест для данной специальности.");
                            }
                        }
                    }
                }
            }
        }

        public void CreateOrder(int id)
        {
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();

                string query = "SELECT * FROM Applications WHERE id = @id";
                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@id", id);

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            string fullName = reader["FullName"].ToString();
                            string passportNumber = reader["PassportNumber"].ToString();
                            string phoneNumber = reader["PhoneNumber"].ToString();
                            double averageGrade = Convert.ToDouble(reader["AverageGrade"]);
                            string specialty = reader["Specialty"].ToString();
                            string status = reader["Status"].ToString();
                            string examsStatus = reader["ExamsStatus"].ToString();

                            string orderText = CreateAdmissionOrder(fullName, passportNumber, phoneNumber, averageGrade, specialty, status, examsStatus);

                            string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                            string filePath = Path.Combine(desktopPath, "AdmissionOrder.txt");

                            File.WriteAllText(filePath, orderText);

                            Console.WriteLine("Приказ о зачислении успешно создан: " + filePath);
                        }
                        else
                        {
                            Console.WriteLine("Абитуриент с указанным ID не найден.");
                        }
                    }
                }
            }
        }
        public string CreateAdmissionOrder(string fullName, string passportNumber, string phoneNumber, double averageGrade, string specialty, string status, string examsStatus)
        {
            return $@"
            ПРИКАЗ О ЗАЧИСЛЕНИИ

            ФИО: {fullName}
            Номер паспорта: {passportNumber}
            Номер телефона: {phoneNumber}
            Средний балл: {averageGrade}
            Специальность: {specialty}
            Статус: {status}
            Вступительные экзамены: {examsStatus}

            Настоящим приказом абитуриент {fullName} зачисляется в учебное заведение.

            Дата: {DateTime.Now.ToShortDateString()}

            Подпись директора: ______________________
        ";
        }

        public void AddExam(string specialtyName, string fullName, DateTime examDate, string observer, string results)
        {
            using (SQLiteConnection connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();

                string sql = "INSERT INTO Exams (SpecialtyName, FullName, ExamDate, Observer, Results) VALUES (@SpecialtyName, @FullName, @ExamDate, @Observer, @Results)";

                using (SQLiteCommand command = new SQLiteCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@SpecialtyName", specialtyName);
                    command.Parameters.AddWithValue("@FullName", fullName);
                    command.Parameters.AddWithValue("@ExamDate", examDate);
                    command.Parameters.AddWithValue("@Observer", observer);
                    command.Parameters.AddWithValue("@Results", results);

                    command.ExecuteNonQuery();
                }
            }
        }

        public void UpdateExamResults(int id, int newResults)
        {
            using (SQLiteConnection connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();

                string sql = "UPDATE Exams SET Results = @NewResults WHERE id = @Id";

                using (SQLiteCommand command = new SQLiteCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@NewResults", newResults);
                    command.Parameters.AddWithValue("@Id", id);

                    int rowsAffected = command.ExecuteNonQuery();
                    if (rowsAffected == 1)
                    {
                        Console.WriteLine("Результаты успешно обновлены");
                    }
                }
            }
        }

        public void UpdateExamObserver(int id, string observer)
        {
            using (SQLiteConnection connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();

                string sql = "UPDATE Exams SET Observer = @NewObserver WHERE id = @Id";

                using (SQLiteCommand command = new SQLiteCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@NewObserver", observer);
                    command.Parameters.AddWithValue("@Id", id);

                    int rowsAffected = command.ExecuteNonQuery();
                    if (rowsAffected == 1)
                    {
                        Console.WriteLine("Наблюдатель назначен");
                    }
                }
            }
        }

        public string CalculateTotalScore()
        {
            string totalscore = "";
            using (SQLiteConnection connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();
                string query = "SELECT SUM(Results) FROM Exams";
                using (SQLiteCommand command = new SQLiteCommand(query, connection))
                {
                    object result = command.ExecuteScalar();
                    if (result != DBNull.Value)
                    {
                        int totalScore = Convert.ToInt32(result);
                        totalscore = $"Количество суммарных баллов: {totalScore}";
                    }
                }
            }
            return totalscore;
        }

    }
}
