namespace Core.Libraries.Domain.Tests.Enums;

/// <summary>
/// Defines the supported payload formats for structured and semi-structured data exchange.
/// </summary>
public enum ContentFormat : int
{
    /// <summary>
    /// JavaScript Object Notation format.
    /// MIME type: application/json
    /// </summary>
    Json = 1,

    /// <summary>
    /// Comma-Separated Values format.
    /// MIME type: text/csv
    /// </summary>
    Csv = 2,

    /// <summary>
    /// XML (eXtensible Markup Language) format.
    /// MIME type: application/xml
    /// </summary>
    Xml = 3,

    /// <summary>
    /// YAML (YAML Ain't Markup Language) format.
    /// MIME type: application/x-yaml
    /// </summary>
    Yaml = 4,

    /// <summary>
    /// Apache Parquet columnar storage format.
    /// MIME type: application/parquet
    /// </summary>
    Parquet = 5,

    /// <summary>
    /// Apache Avro serialization format.
    /// MIME type: application/avro
    /// </summary>
    Avro = 6,

    /// <summary>
    /// Binary-encoded format for raw or custom data payloads.
    /// MIME type: application/octet-stream
    /// </summary>
    Binary = 7
}