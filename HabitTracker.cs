using Microsoft.Data.Sqlite;

namespace Habit_Tracker;

static class HabitTracker
{
    static readonly string ConnectionString = @"Data Source=habit-Tracker.db";

    private static void CreateDatabase()
    {
        /*Creating a connection passing the connection string as an argument
        This will create the database for you, there's no need to manually create it.
        And no need to use File.Create().*/
        using (var connection = new SqliteConnection(ConnectionString))
        {
            //Creating the command that will be sent to the database
            using (var tableCmd = connection.CreateCommand())
            {
                connection.Open();
                //Declaring what is that command (in SQL syntax)
                tableCmd.CommandText =
                    @"CREATE TABLE IF NOT EXISTS yourHabit (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    Date TEXT,
                    Quantity INTEGER
                    )";

                // Executing the command, which isn't a query, it's not asking to return data from the database.
                tableCmd.ExecuteNonQuery();
            }
            // We don't need to close the connection or the command. The 'using statement' does that for us.
        }

        /* Once we check if the database exists and create it (or not),
        we will call the next method, which will handle the user's input. Your next step is to create this method*/
        bool keepGoing = true;

        while (keepGoing)
        {
            keepGoing = GetUserInput();
        }
    }

    private static bool GetUserInput()
    {
        int userChoice;
        
        Console.WriteLine("----------------");
        Console.WriteLine("0 - Quit");
        Console.WriteLine("1 - View All");
        Console.WriteLine("2 - Insert");
        Console.WriteLine("3 - Delete");
        Console.WriteLine("4 - Update");
        Console.WriteLine("----------------");
        
        do
        {
            Console.Write("> ");
        } while (!int.TryParse(Console.ReadLine(), out userChoice));

        if (userChoice == 0)
        {
            Console.WriteLine("Quitting...");
            return false;
        }

        
        RunCommand(userChoice);
        return true;
    }

    private static void RunCommand(int commandChoice)
    {
        using var connection = new SqliteConnection("Data Source=yourHabit.db");
        connection.Open();

        var command = connection.CreateCommand();

        switch (commandChoice)
        {
            case 1:
                command.CommandText =
                    @"SELECT *
                    FROM yourHabit
                    ";
                ViewAll(command);
                break;
            case 2:
                Console.Write("Enter date: ");
                string? date = Console.ReadLine();
                Console.Write("Enter quantity: ");
                int quantity = Convert.ToInt32(Console.ReadLine());
                command.CommandText =
                    @"INSERT INTO yourHabit (Date,Quantity)
                    VALUES (@date,@qty)
                    ";
                command.Parameters.AddWithValue("@date", date);
                command.Parameters.AddWithValue("@qty", quantity);
                using (var _ = command.ExecuteReader())
                {
                    Console.WriteLine("Insert successful.");
                }
                break;
            case 3:
                command.CommandText =
                    @"SELECT *
                    FROM yourHabit
                    ";
                ViewAll(command);
                Console.Write("Enter Id of record to delete: ");
                int id = Convert.ToInt32(Console.ReadLine());
                command.CommandText =
                    @"DELETE FROM yourHabit
                    WHERE Id =@id
                    ";
                command.Parameters.AddWithValue("@id", id);
                using (var _ = command.ExecuteReader())
                {
                    Console.WriteLine("Delete successful.");
                }
                break;
            case 4:
                command.CommandText =
                    @"
                        SELECT *
                        FROM yourHabit
                        ";
                ViewAll(command);
                Console.Write("Enter Id of record to update: ");
                int id2 = Convert.ToInt32(Console.ReadLine());
                Console.Write("Enter new quantity: ");
                int qty = Convert.ToInt32(Console.ReadLine());
                command.CommandText =
                    @"UPDATE yourHabit
                    SET Quantity = @qty
                    WHERE Id = @id;
                    ";
                command.Parameters.AddWithValue("@id", id2);
                command.Parameters.AddWithValue("@qty", qty);
                using (var _ = command.ExecuteReader())
                {
                    Console.WriteLine("Update successful.");
                }
                break;
            default:
                command.CommandText =
                    @"
                        SELECT *
                        FROM yourHabit
                        ";
                //command.Parameters.AddWithValue("$id", id);
                break;
        }
    }

    private static void ViewAll(SqliteCommand command)
    {
        using var reader = command.ExecuteReader();
        Console.WriteLine($"{"ID",-4}{"DATE",-11}{"QUANTITY",-14}");
        while (reader.Read())
        {
            var id = reader.GetString(0);
            var date = reader.GetString(1);
            var quantity = reader.GetString(2);
                    
            Console.WriteLine($"{id,-4}{date,-11} {quantity,-14}");
        }
        Console.WriteLine();
    }

    private static void Main()
    {
        HabitTracker.CreateDatabase();
    }
}



