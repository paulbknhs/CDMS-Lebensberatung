﻿using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace CDMS_Lebensberatung.Resources.classes;

public class DatabaseHelper
{
    private SqlConnection _connection;
    private readonly string _connectionString;

    public DatabaseHelper(string connectionString)
    {
        _connectionString = connectionString;
    }

    public void Connect()
    {
        _connection = new SqlConnection(_connectionString);
        _connection.Open();
    }

    public void Disconnect()
    {
        _connection.Close();
    }

    public void InsertStringDict(string tableName, Dictionary<string, string> data)
    {
        if (data.Count == 0)
            throw new ArgumentException("Dictionary enthält keine Einträge", nameof(data));

        var columns = "";
        var values = "";

        foreach (var item in data)
        {
            columns += $"[{item.Key}],";
            values += $"'{item.Value}',";
        }

        columns = columns.Remove(columns.Length - 1);
        values = values.Remove(values.Length - 1);

        var command = new SqlCommand(
            $"INSERT INTO [dbo].[{tableName}] ({columns}) VALUES ({values})",
            _connection
        );
        command.ExecuteNonQuery();
    }

    public int GetNumberOfRows(string tableName)
    {
        using var command = new SqlCommand($"SELECT * FROM [{tableName}]", _connection);
        var count = command.ExecuteNonQuery();

        return count;
    }

    public DataTable GetFullTable(string tableName)
    {
        var dataTable = new DataTable();

        using var command = new SqlCommand($"SELECT * FROM [{tableName}]", _connection);
        using var adapter = new SqlDataAdapter(command);
        adapter.Fill(dataTable);
        return dataTable;
    }

    public DataTable GetColumn(string tableName, string columnName)
    {
        var columnTable = new DataTable();

        using var command = new SqlCommand($"SELECT [{columnName}] FROM [{tableName}]", _connection);
        using var adapter = new SqlDataAdapter(command);
        adapter.Fill(columnTable);
        return columnTable;
    }

    public DataTable SendQuery(string query)
    {
        var dataTable = new DataTable();

        using var command = new SqlCommand(query, _connection);
        using var adapter = new SqlDataAdapter(command);
        adapter.Fill(dataTable);
        return dataTable;
    }

    public DataTable GetDataFiltered(string tableName, Dictionary<string, string> filters)
    {
        var dataTable = new DataTable();
        var filter = "";

        foreach (var item in filters) filter += $"[{item.Key}] LIKE '%{item.Value}%' AND ";

        filter = filter.Remove(filter.Length - 5);
        using var command = new SqlCommand(
            $"SELECT * FROM [dbo].[{tableName}] WHERE {filter}",
            _connection
        );
        using var reader = command.ExecuteReader();
        dataTable.Load(reader);
        return dataTable;
    }


    public bool DeleteData(string tableName, Dictionary<string, string> toDelete)
    {
        var remove = "";

        foreach (var item in toDelete) remove += $"[{item.Key}]= '{item.Value}' AND ";

        remove = remove.Remove(remove.Length - 5);

        using var command = new SqlCommand(
            $"DELETE FROM [dbo].[{tableName}] WHERE {remove}",
            _connection
        );
        using var reader = command.ExecuteReader();

        return true;
    }
}