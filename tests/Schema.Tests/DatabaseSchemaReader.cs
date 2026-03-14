using Npgsql;

namespace LimonikOne.Tests.Schema;

public static class DatabaseSchemaReader
{
    public static async Task<DatabaseSchema> ReadAsync(
        string connectionString,
        string schemaName,
        CancellationToken ct = default
    )
    {
        await using var connection = new NpgsqlConnection(connectionString);
        await connection.OpenAsync(ct);

        var tables = await ReadTablesAsync(connection, schemaName, ct);
        return new DatabaseSchema(schemaName, tables);
    }

    private static async Task<List<DatabaseTable>> ReadTablesAsync(
        NpgsqlConnection connection,
        string schemaName,
        CancellationToken ct
    )
    {
        var tables = new List<DatabaseTable>();

        const string tableSql = """
            SELECT table_name
            FROM information_schema.tables
            WHERE table_schema = @schema
              AND table_type = 'BASE TABLE'
            ORDER BY table_name;
            """;

        await using var tableCmd = new NpgsqlCommand(tableSql, connection);
        tableCmd.Parameters.AddWithValue("schema", schemaName);

        await using var reader = await tableCmd.ExecuteReaderAsync(ct);
        var tableNames = new List<string>();
        while (await reader.ReadAsync(ct))
        {
            tableNames.Add(reader.GetString(0));
        }
        await reader.CloseAsync();

        foreach (var tableName in tableNames)
        {
            var columns = await ReadColumnsAsync(connection, schemaName, tableName, ct);
            tables.Add(new DatabaseTable(schemaName, tableName, columns));
        }

        return tables;
    }

    private static async Task<List<DatabaseColumn>> ReadColumnsAsync(
        NpgsqlConnection connection,
        string schemaName,
        string tableName,
        CancellationToken ct
    )
    {
        const string columnSql = """
            SELECT
                column_name,
                data_type,
                udt_name,
                is_nullable,
                character_maximum_length,
                numeric_precision,
                numeric_scale
            FROM information_schema.columns
            WHERE table_schema = @schema
              AND table_name = @table
            ORDER BY ordinal_position;
            """;

        await using var cmd = new NpgsqlCommand(columnSql, connection);
        cmd.Parameters.AddWithValue("schema", schemaName);
        cmd.Parameters.AddWithValue("table", tableName);

        await using var reader = await cmd.ExecuteReaderAsync(ct);
        var columns = new List<DatabaseColumn>();

        while (await reader.ReadAsync(ct))
        {
            var dataType = reader.GetString(1);
            var udtName = reader.GetString(2);

            // Use udt_name for user-defined types like uuid, or when data_type is "USER-DEFINED"
            var resolvedType = dataType == "USER-DEFINED" ? udtName : dataType;

            // Normalize PostgreSQL type names to match EF Core store types
            resolvedType = NormalizePostgresType(
                resolvedType,
                reader.IsDBNull(4) ? null : reader.GetInt32(4),
                reader.IsDBNull(5) ? null : reader.GetInt32(5),
                reader.IsDBNull(6) ? null : reader.GetInt32(6)
            );

            columns.Add(
                new DatabaseColumn(
                    Name: reader.GetString(0),
                    DataType: resolvedType,
                    IsNullable: reader.GetString(3) == "YES",
                    MaxLength: reader.IsDBNull(4) ? null : reader.GetInt32(4),
                    NumericPrecision: reader.IsDBNull(5) ? null : reader.GetInt32(5),
                    NumericScale: reader.IsDBNull(6) ? null : reader.GetInt32(6)
                )
            );
        }

        return columns;
    }

    private static string NormalizePostgresType(
        string dataType,
        int? maxLength,
        int? precision,
        int? scale
    )
    {
        return dataType switch
        {
            "character varying" when maxLength.HasValue => $"character varying({maxLength})",
            "character varying" => "character varying",
            "numeric" when precision.HasValue && scale.HasValue => $"numeric({precision},{scale})",
            "numeric" => "numeric",
            "timestamp without time zone" => "timestamp without time zone",
            "timestamp with time zone" => "timestamp with time zone",
            "integer" => "integer",
            "bigint" => "bigint",
            "smallint" => "smallint",
            "boolean" => "boolean",
            "text" => "text",
            "uuid" => "uuid",
            "jsonb" => "jsonb",
            "bytea" => "bytea",
            "double precision" => "double precision",
            "real" => "real",
            _ => dataType,
        };
    }
}
