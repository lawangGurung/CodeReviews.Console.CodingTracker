using System.Configuration;
using System.Data;
using System.Globalization;
using Dapper;
using Microsoft.Data.Sqlite;

namespace Lawang.Coding_Tracker;

public class CodingController
{
    private readonly string _connectionString;
    public CodingController()
    {
        _connectionString = ConfigurationManager.ConnectionStrings["sqlite"].ConnectionString;
    }

    public List<CodingSession> GetAllData()
    {

        var codingSessions = new List<CodingSession>();
        try
        {
            using IDbConnection connection = new SqliteConnection(_connectionString);
            string getAllSQL =
                @"SELECT * FROM CodingSessions";

            using var dbReader = connection.ExecuteReader(getAllSQL);

            while (dbReader.Read())
            {
                codingSessions.Add(new CodingSession()
                {
                    Id = dbReader.GetInt32(0),
                    StartTime = DateTime.ParseExact(dbReader.GetString(1), "hh:mm tt", CultureInfo.InvariantCulture),
                    EndTime = DateTime.ParseExact(dbReader.GetString(2), "hh:mm tt", CultureInfo.InvariantCulture),
                    Duration = TimeSpan.Parse(dbReader.GetString(3)),
                    Date = DateTime.ParseExact(dbReader.GetString(4), "dd/MM/yyyy", CultureInfo.InvariantCulture)
                });
            }

            return codingSessions;
        }
        catch (SqliteException ex)
        {
            Console.WriteLine(ex.Message);
            Console.ReadLine();
        }

        return codingSessions;
    }
    public int Post(CodingSession codingSession)
    {
        try
        {
            using IDbConnection connection = new SqliteConnection(_connectionString);
            string insertSQL =
                @"INSERT INTO CodingSessions (StartTime, EndTime, Duration, Date)
                VALUES(@startTime, @endTime, @Duration, @Date)";

            var parameter = new
            {
                @startTime = codingSession.StartTime.ToString("hh:mm tt"),
                @endTime = codingSession.EndTime.ToString("hh:mm tt"),
                @Duration = codingSession.Duration.ToString("hh\\:mm\\:ss"),
                @Date = codingSession.Date.ToString("dd/MM/yyyy")
            };

            int affectedRow = connection.Execute(insertSQL, parameter);

            return affectedRow;
        }
        catch (SqliteException ex)
        {
            Console.WriteLine(ex.Message);
        }

        return -1;
    }

    public int Update(CodingSession codingSession)
    {
        try
        {
            using IDbConnection connection = new SqliteConnection(_connectionString);
            string updateSQL =
                @"UPDATE CodingSessions 
                SET StartTime = @startTime, EndTime = @endTime, Duration = @duration
                WHERE Id = @id";
            var param = new
            {
                @id = codingSession.Id,
                @startTime = codingSession.StartTime.ToString("hh:mm tt"),
                @endTime = codingSession.EndTime.ToString("hh:mm tt"),
                @duration = codingSession.Duration.ToString()

            };

            int affectedRow = connection.Execute(updateSQL, param);

            return affectedRow;
        }
        catch(SqliteException ex)
        {
            Console.WriteLine(ex.Message);
        }

        return -1;
    }

    public int Delete(CodingSession codingSession)
    {
        try
        {
            using var connection = new SqliteConnection(_connectionString);
            string deleteSQL = 
                @"DELETE FROM CodingSessions
                WHERE Id = @id";

            return connection.Execute(deleteSQL, new {@id = codingSession.Id});
        }
        catch (SqliteException ex)
        {
            Console.WriteLine(ex.Message);
        }
        return -1;
    }

}
