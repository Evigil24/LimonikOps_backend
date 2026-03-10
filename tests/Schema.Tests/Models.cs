namespace LimonikOne.Tests.Schema;

public sealed record DatabaseSchema(string SchemaName, List<DatabaseTable> Tables);

public sealed record DatabaseTable(string Schema, string Name, List<DatabaseColumn> Columns);

public sealed record DatabaseColumn(
    string Name,
    string DataType,
    bool IsNullable,
    int? MaxLength,
    int? NumericPrecision,
    int? NumericScale
);

public sealed record SchemaDifference(string Category, string Description)
{
    public override string ToString() => $"[{Category}] {Description}";
}
