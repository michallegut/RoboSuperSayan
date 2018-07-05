using Mono.Data.Sqlite;
using System;
using System.Data;
using UnityEngine;

public class LoginController
{
    IDbConnection connection;

    public LoginController()
    {
        string connectionString = "URI=file:" + Application.dataPath + "/Players.s3db";
        connection = new SqliteConnection(connectionString);
        IDbCommand command = connection.CreateCommand();
        connection.Open();
        string sql = "CREATE TABLE IF NOT EXISTS players(login Varchar(20) PRIMARY KEY, password Varchar(20))";
        command.CommandText = sql;
        command.ExecuteNonQuery();
        command.Dispose();
        connection.Close();
    }

    public bool login(string login, string password)
    {
        IDbCommand command = connection.CreateCommand();
        connection.Open();
        string sql = "SELECT * FROM players WHERE login = '" + login + "'";
        command.CommandText = sql;
        IDataReader dataReader = command.ExecuteReader();
        bool result = dataReader.Read();
        if (result)
        {
            result = password.Equals(dataReader["password"]);
        }
        dataReader.Close();
        command.Dispose();
        connection.Close();
        return result;
    }

    public String register(string login, string password)
    {
        String result = "";
        IDbCommand command = connection.CreateCommand();
        connection.Open();
        string sql = "SELECT * FROM players WHERE login = '" + login + "'";
        command.CommandText = sql;
        IDataReader dataReader = command.ExecuteReader();
        if (dataReader.Read())
        {
            result = "Player with this login already exists. ";
        }
        if (login.Length == 0)
        {
            result = result + "Login can't be empty. ";
        }
        if (password.Length == 0)
        {
            result = result + "Password can't be empty.";
        }
        dataReader.Close();
        if (result.Length == 0)
        {
            sql = "INSERT INTO players values('" + login + "', '" + password + "')";
            command.CommandText = sql;
            command.ExecuteNonQuery();
            result = "Player has been registered.";
        }
        command.Dispose();
        connection.Close();
        return result;
    }
}
