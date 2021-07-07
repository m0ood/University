using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Университет
{
    class Program
    {
        static void Main(string[] args)
        {
            // connection string (описано выше: шаг №4)
            string connectionString = @"Data Source=DESKTOP-954URH2;Initial Catalog=Университет1;Integrated Security=True";
            Console.WriteLine("1.Сформировать список студентов.");
            Console.WriteLine("2.Сформировать список оценок.");
            Console.WriteLine("3.Сформировать список студентов допущенных к экзаменам.");
            Console.WriteLine("4.Сформировать список экзаменов для каждой группы.");
            Console.WriteLine("5.Сформировать список дисциплин, которые вел преподаватель у специальности(потока) на протяжении всего периода обучения.");
            Console.WriteLine("6.Сформировать список студентов, сдававших платные экзамены. ");
            Console.WriteLine("7.Сформировать список студентов, сдававших бесплатные пересдачи. ");
            Console.WriteLine("8.Изменить оценку студента, полученную на экзамене.");
            Console.WriteLine("9.Удалить студента из группы, которого отчислили. ");
            Console.WriteLine("10.Добавить нового студента в группу.");
            Console.WriteLine("0.Выйти.");
            while (true)
            {
                Console.Write("Введите команду:");
                int task = Int32.Parse(Console.ReadLine());
                if (task == 10)
                {
                    Console.Write("Введите ФИО:");
                    string name = Console.ReadLine();
                    Console.Write("Введите ID группы:");
                    int id = Int32.Parse(Console.ReadLine());
                    string sqlExpression = String.Format("INSERT INTO Cтудент (ФИО, ID_группа) VALUES ('{0}', {1})", name, id);
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        connection.Open();
                        SqlCommand command = new SqlCommand(sqlExpression, connection);
                        int number = command.ExecuteNonQuery();
                        Console.WriteLine("Добавлено объектов: {0}", number);
                        connection.Close();
                    }
                }
                else if (task == 9)
                {
                    Console.Write("Введите ID:");
                    int id = Int32.Parse(Console.ReadLine());
                    string sqlExpression = String.Format("DELETE FROM Cтудент WHERE ID_Студент = {0}", id);
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        connection.Open();
                        SqlCommand command = new SqlCommand(sqlExpression, connection);
                        int number = command.ExecuteNonQuery();
                        Console.WriteLine("Удалено объектов: {0}", number);
                        connection.Close();
                    }
                }
                else if (task == 8)
                {
                    Console.Write("Введите новую оценку:");
                    int rating = Int32.Parse(Console.ReadLine());
                    Console.Write("Введите ID оценки:");
                    int id = Int32.Parse(Console.ReadLine());
                    string sqlExpression = String.Format("UPDATE Оценка SET Оценка={0} WHERE ID_Оценка={1}", rating, id);
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        connection.Open();
                        SqlCommand command = new SqlCommand(sqlExpression, connection);
                        int number = command.ExecuteNonQuery();
                        Console.WriteLine("Обновлено объектов: {0}", number);
                        connection.Close();
                    }
                }
                else if (task == 3)
                {/*Select Студент.ФИО From Cтудент INNER JOIN Допуск ON Студент.ID_Студент=Допуск.ID_Студент WHERE Допущен = 'Да'*/
                    string sqlExpression = String.Format("Select Cтудент.ФИО From Cтудент INNER JOIN Допуск ON Cтудент.ID_Студент=Допуск.ID_Студент WHERE Допущен = 'Да'");
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        connection.Open();
                        SqlCommand command = new SqlCommand(sqlExpression, connection);
                        SqlDataReader reader = command.ExecuteReader();

                        if (reader.HasRows)
                        {
                            Console.WriteLine("{0}", reader.GetName(0));
                            while (reader.Read())
                            {
                                Console.WriteLine(reader.GetString(0));
                            }
                        }
                        else
                        {
                            Console.WriteLine("No rows found.");
                        }
                        connection.Close();
                    }
                }
                else if (task == 4)
                {
                    string sqlExpression = String.Format("SELECT Экзамен.Дата, Экзамен.Место, Группа.Название AS Группа_Название, Дисциплина.Название AS Дисциплина_Название" +
                    " FROM Дисциплина INNER JOIN(Группа INNER JOIN Экзамен ON Группа.[ID_Группа] = Экзамен.[ID_Группа]) ON Дисциплина.[ID_Дисциплина] = Экзамен.[ID_Дисциплина]" +
                    " WHERE(((Экзамен.Тип_оценивания) = 'Экзамен'));");
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        connection.Open();
                        SqlCommand command = new SqlCommand(sqlExpression, connection);
                        SqlDataReader reader = command.ExecuteReader();

                        if (reader.HasRows)
                        {
                            Console.WriteLine("{0}\t\t\t{1}\t\t{2}\t{3}", reader.GetName(0), reader.GetName(1), reader.GetName(2), reader.GetName(3));
                            while (reader.Read())
                            {
                                Console.WriteLine("{0}\t{1}\t{2}\t{3}", reader.GetDateTime(0),
                            reader.GetString(1), reader.GetString(2), reader.GetString(3));
                            }
                        }
                        else
                        {
                            Console.WriteLine("No rows found.");
                        }
                        connection.Close();
                    }
                }
                else if (task == 5)
                {
                    Console.Write("Введите ID Преподавателя:");
                    int id = Int32.Parse(Console.ReadLine());
                    string sqlExpression = String.Format("SELECT Дисциплина.Название, Дисциплина.Количество_часов" +
                    " FROM Преподаватель INNER JOIN Дисциплина ON Преподаватель.[ID_Преподаватель] = Дисциплина.[ID_Преподаватель]" +
                    " WHERE (((Дисциплина.ID_Преподаватель)={0}))", id);
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        connection.Open();
                        SqlCommand command = new SqlCommand(sqlExpression, connection);
                        SqlDataReader reader = command.ExecuteReader();

                        if (reader.HasRows)
                        {
                            Console.WriteLine("{0}\t{1}", reader.GetName(0), reader.GetName(1));
                            while (reader.Read())
                            {
                                Console.WriteLine("{0}\t{1}",reader.GetString(0), reader.GetInt32(1));
                            }
                        }
                        else
                        {
                            Console.WriteLine("No rows found.");
                        }
                        connection.Close();
                    }
                }
                else if (task == 6)
                {
                    string sqlExpression = String.Format("SELECT DISTINCT Cтудент.ФИО, Оценка.Стоимость" +
                    " FROM Cтудент INNER JOIN Оценка ON Cтудент.[ID_Студент] = Оценка.[ID_Студент]" +
                    " WHERE (((Оценка.Стоимость)>0) AND ([№попытки]>1));");
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        connection.Open();
                        SqlCommand command = new SqlCommand(sqlExpression, connection);
                        SqlDataReader reader = command.ExecuteReader();

                        if (reader.HasRows)
                        {
                            Console.WriteLine("{0}\t\t\t\t{1}", reader.GetName(0), reader.GetName(1));
                            while (reader.Read())
                            {
                                Console.WriteLine("{0}\t{1}", reader.GetString(0), reader.GetInt32(1));
                            }
                        }
                        else
                        {
                            Console.WriteLine("No rows found.");
                        }
                        connection.Close();
                    }
                }
                else if (task == 7)
                {
                    string sqlExpression = String.Format("SELECT DISTINCT Cтудент.ФИО, Оценка.Стоимость" +
                       " FROM Cтудент INNER JOIN Оценка ON Cтудент.[ID_Студент] = Оценка.[ID_Студент]" +
                       " WHERE (((Оценка.Стоимость)=0) AND ([№попытки]>1));");
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        connection.Open();
                        SqlCommand command = new SqlCommand(sqlExpression, connection);
                        SqlDataReader reader = command.ExecuteReader();

                        if (reader.HasRows)
                        {
                            Console.WriteLine("{0}\t\t\t\t{1}", reader.GetName(0), reader.GetName(1));
                            while (reader.Read())
                            {
                                Console.WriteLine("{0}\t{1}", reader.GetString(0), reader.GetInt32(1));
                            }
                        }
                        else
                        {
                            Console.WriteLine("No rows found.");
                        }
                        connection.Close();
                    }
                }
                else if (task == 0)
                    break;
                else if (task == 1)
                {
                    string sqlExpression = String.Format("SELECT *" +
                       " FROM Cтудент");
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        connection.Open();
                        SqlCommand command = new SqlCommand(sqlExpression, connection);
                        SqlDataReader reader = command.ExecuteReader();

                        if (reader.HasRows)
                        {
                            Console.WriteLine("{0}\t{1}", reader.GetName(0), reader.GetName(1));
                            while (reader.Read())
                            {
                                Console.WriteLine("{0}\t{1}", reader.GetInt32(0),reader.GetString(1));
                            }
                        }
                        else
                        {
                            Console.WriteLine("No rows found.");
                        }
                        connection.Close();
                    }
                }
                else if (task == 2)
                {
                    string sqlExpression = String.Format("SELECT *" +
                      " FROM Оценка");
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        connection.Open();
                        SqlCommand command = new SqlCommand(sqlExpression, connection);
                        SqlDataReader reader = command.ExecuteReader();

                        if (reader.HasRows)
                        {
                            Console.WriteLine("{0}\t{1}\t{2}\t{3}\t{4} {5}", reader.GetName(0), reader.GetName(1), reader.GetName(2), reader.GetName(3), reader.GetName(4), reader.GetName(7));
                            while (reader.Read())
                            {
                                Console.WriteLine("{0}\t{1}\t{2}\t{3}\t{4}\t{5}", reader.GetInt32(0), reader.GetString(1), reader.GetDateTime(2), reader.GetInt32(3), reader.GetInt32(4), reader.GetString(7));
                            }
                        }
                        else
                        {
                            Console.WriteLine("No rows found.");
                        }
                        connection.Close();
                    }
                }
            }
        }
    }
}
