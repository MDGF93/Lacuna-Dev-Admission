﻿using System.Data.SQLite;
using Lacuna_Dev_Admission.Entities;

namespace Lacuna_Dev_Admission.Repositories;

public class JobRepository

{
    private static readonly string currentDirectory = Directory.GetCurrentDirectory();
    private static readonly string databasePath = currentDirectory + "\\sqlite.db";
    private static readonly string _connectionString =
        $"Data Source={databasePath}";

    public void Save(JobEntity job, string code)
    {
        Console.WriteLine("CURRENT DIRECTORY " + currentDirectory);
        using (var connection = new SQLiteConnection(_connectionString))
        {
            // If table doesn't exist, create it
            connection.Open();
            var command = new SQLiteCommand(connection);
            command.CommandText =
                "CREATE TABLE IF NOT EXISTS jobs (id INTEGER PRIMARY KEY, job_id TEXT, job_type TEXT, job_code TEXT, job_strand TEXT, job_strand_encoded TEXT, job_gene_encoded TEXT)";
            command.ExecuteNonQuery();

            // Check if the column exists and add it if it doesn't
            command.CommandText = "PRAGMA table_info(jobs)";
            var reader = command.ExecuteReader();
            var columnExists = false;
            while (reader.Read())
            {
                if (reader["name"].ToString().Equals("job_code"))
                {
                    columnExists = true;
                    break;
                }
            }

            reader.Close();
            if (!columnExists)
            {
                command.CommandText = "ALTER TABLE jobs ADD COLUMN job_code TEXT";
                command.ExecuteNonQuery();
            }

            command.CommandText =
                "INSERT INTO jobs (job_id, job_type, job_strand, job_strand_encoded, job_gene_encoded, job_code) VALUES (@job_id, @job_type, @job_strand, @job_strand_encoded, @job_gene_encoded, @job_code)";
            command.Parameters.AddWithValue("@job_id", job.Id);
            command.Parameters.AddWithValue("@job_type", job.Type);
            command.Parameters.AddWithValue("@job_strand", job.Strand);
            command.Parameters.AddWithValue("@job_strand_encoded", job.StrandEncoded);
            command.Parameters.AddWithValue("@job_gene_encoded", job.GeneEncoded);
            command.Parameters.AddWithValue("@job_code", code);
            command.ExecuteNonQuery();
        }
    }

}