using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace LimonikOne.Tests.Schema;

public static class EfCoreModelReader
{
    public static DatabaseSchema Read(DbContext context, string schemaName)
    {
        var relationalModel = context.Model.GetRelationalModel();
        var tables = new List<DatabaseTable>();

        foreach (var table in relationalModel.Tables)
        {
            if (!string.Equals(table.Schema, schemaName, StringComparison.OrdinalIgnoreCase))
                continue;

            var columns = new List<DatabaseColumn>();

            foreach (var column in table.Columns)
            {
                var storeType = column.StoreType;
                ParseStoreType(
                    storeType,
                    out var normalizedType,
                    out var maxLength,
                    out var precision,
                    out var scale
                );

                columns.Add(
                    new DatabaseColumn(
                        Name: column.Name,
                        DataType: normalizedType,
                        IsNullable: column.IsNullable,
                        MaxLength: maxLength,
                        NumericPrecision: precision,
                        NumericScale: scale
                    )
                );
            }

            tables.Add(new DatabaseTable(schemaName, table.Name, columns));
        }

        return new DatabaseSchema(schemaName, tables);
    }

    /// <summary>
    /// Parses EF Core store type strings like "character varying(100)", "numeric(18,4)"
    /// into normalized components matching information_schema output.
    /// </summary>
    private static void ParseStoreType(
        string storeType,
        out string normalizedType,
        out int? maxLength,
        out int? precision,
        out int? scale
    )
    {
        maxLength = null;
        precision = null;
        scale = null;

        // Handle parameterized types: "character varying(100)", "numeric(18,4)"
        var parenIndex = storeType.IndexOf('(');
        if (parenIndex < 0)
        {
            normalizedType = NormalizeEfStoreType(storeType);
            return;
        }

        var baseType = storeType[..parenIndex].Trim();
        var paramsPart = storeType[(parenIndex + 1)..].TrimEnd(')');
        var parts = paramsPart.Split(',');

        if (
            baseType is "character varying" or "varchar"
            && parts.Length == 1
            && int.TryParse(parts[0].Trim(), out var len)
        )
        {
            maxLength = len;
            normalizedType = $"character varying({len})";
            return;
        }

        if (
            baseType is "numeric" or "decimal"
            && parts.Length == 2
            && int.TryParse(parts[0].Trim(), out var p)
            && int.TryParse(parts[1].Trim(), out var s)
        )
        {
            precision = p;
            scale = s;
            normalizedType = $"numeric({p},{s})";
            return;
        }

        normalizedType = NormalizeEfStoreType(storeType);
    }

    private static string NormalizeEfStoreType(string storeType)
    {
        return storeType switch
        {
            "int" or "int4" or "integer" => "integer",
            "bigint" or "int8" => "bigint",
            "smallint" or "int2" => "smallint",
            "bool" or "boolean" => "boolean",
            "text" => "text",
            "uuid" => "uuid",
            "jsonb" => "jsonb",
            "bytea" => "bytea",
            "float8" or "double precision" => "double precision",
            "float4" or "real" => "real",
            "timestamp without time zone" or "timestamp" => "timestamp without time zone",
            "timestamp with time zone" or "timestamptz" => "timestamp with time zone",
            _ => storeType,
        };
    }
}
