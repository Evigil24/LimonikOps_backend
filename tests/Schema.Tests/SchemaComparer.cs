namespace LimonikOne.Tests.Schema;

public static class SchemaComparer
{
    public static List<SchemaDifference> Compare(DatabaseSchema efModel, DatabaseSchema database)
    {
        var differences = new List<SchemaDifference>();

        var efTables = efModel.Tables.ToDictionary(t => t.Name, StringComparer.OrdinalIgnoreCase);
        var dbTables = database.Tables.ToDictionary(t => t.Name, StringComparer.OrdinalIgnoreCase);

        // Tables in EF model but missing from database
        foreach (var (name, table) in efTables)
        {
            if (!dbTables.ContainsKey(name))
            {
                differences.Add(
                    new SchemaDifference("MISSING TABLE IN DB", $"{table.Schema}.{name}")
                );
            }
        }

        // Tables in database but missing from EF model
        foreach (var (name, table) in dbTables)
        {
            if (!efTables.ContainsKey(name))
            {
                differences.Add(
                    new SchemaDifference("EXTRA TABLE IN DB", $"{table.Schema}.{name}")
                );
            }
        }

        // Compare columns for matching tables
        foreach (var (tableName, efTable) in efTables)
        {
            if (!dbTables.TryGetValue(tableName, out var dbTable))
                continue;

            CompareColumns(efTable, dbTable, differences);
        }

        return differences;
    }

    private static void CompareColumns(
        DatabaseTable efTable,
        DatabaseTable dbTable,
        List<SchemaDifference> differences
    )
    {
        var qualifiedTable = $"{efTable.Schema}.{efTable.Name}";

        var efColumns = efTable.Columns.ToDictionary(c => c.Name, StringComparer.OrdinalIgnoreCase);
        var dbColumns = dbTable.Columns.ToDictionary(c => c.Name, StringComparer.OrdinalIgnoreCase);

        // Columns in EF model but missing from database
        foreach (var (colName, col) in efColumns)
        {
            if (!dbColumns.ContainsKey(colName))
            {
                var nullable = col.IsNullable ? "NULL" : "NOT NULL";
                differences.Add(
                    new SchemaDifference(
                        "MISSING COLUMN IN DB",
                        $"{qualifiedTable}.{colName} ({col.DataType}, {nullable})"
                    )
                );
            }
        }

        // Columns in database but missing from EF model
        foreach (var (colName, col) in dbColumns)
        {
            if (!efColumns.ContainsKey(colName))
            {
                var nullable = col.IsNullable ? "NULL" : "NOT NULL";
                differences.Add(
                    new SchemaDifference(
                        "EXTRA COLUMN IN DB",
                        $"{qualifiedTable}.{colName} ({col.DataType}, {nullable})"
                    )
                );
            }
        }

        // Compare matching columns
        foreach (var (colName, efCol) in efColumns)
        {
            if (!dbColumns.TryGetValue(colName, out var dbCol))
                continue;

            if (!string.Equals(efCol.DataType, dbCol.DataType, StringComparison.OrdinalIgnoreCase))
            {
                differences.Add(
                    new SchemaDifference(
                        "TYPE MISMATCH",
                        $"{qualifiedTable}.{colName} — Model: {efCol.DataType}, DB: {dbCol.DataType}"
                    )
                );
            }

            if (efCol.IsNullable != dbCol.IsNullable)
            {
                var efNullable = efCol.IsNullable ? "NULL" : "NOT NULL";
                var dbNullable = dbCol.IsNullable ? "NULL" : "NOT NULL";
                differences.Add(
                    new SchemaDifference(
                        "NULLABILITY MISMATCH",
                        $"{qualifiedTable}.{colName} — Model: {efNullable}, DB: {dbNullable}"
                    )
                );
            }
        }
    }
}
