using System;
using System.Configuration;

public class DatabaseConfig
{
    public static string GetDbUsername()
    {
         return ConfigurationManager.AppSettings["DbUsername"];
    }

    public static string GetDbPassword()
    {
        return ConfigurationManager.AppSettings["DbPassword"];
    }

    public static string GetDbServer()
    {
        return ConfigurationManager.AppSettings["DbServer"];
    }

    public static string GetConnectionString()
    {
        string username = GetDbUsername();
        string password = GetDbPassword();
        string server = GetDbServer();
        return $"Server={server};Database=Rubica;User Id={username};Password={password};";
    }

    
}