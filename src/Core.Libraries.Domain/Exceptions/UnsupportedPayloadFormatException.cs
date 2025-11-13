using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Intrinsics.X86;
using System.Text;
using System.Threading.Tasks;

namespace Core.Libraries.Domain.Exceptions;


/// <summary>
/// Represents an exception thrown when a payload format is not supported by the domain or application.
/// </summary>
/// <remarks>
/// This exception is typically used when attempting to process, deserialize, or respond with
/// a format that is outside the supported set of formats.
/// </remarks>
public class UnsupportedPayloadFormatException : DomainException
{
    /// <summary>
    /// Gets the unsupported payload format value, if known.
    /// </summary>
    public string? ProvidedFormat { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="UnsupportedPayloadFormatException"/> class.
    /// </summary>
    /// <param name="providedFormat">The format that was rejected (if identifiable).</param>
    /// <param name="context">The name of the domain or feature area where the failure occurred.</param>
    /// <param name="details">Optional extra data for diagnostics.</param>
    public UnsupportedPayloadFormatException(
        string? providedFormat,
        string context,
        object? details = null)
        : base(
            message: $"The payload format '{providedFormat}' is not supported in context '{context}'.",
            errorCode: "UNSUPPORTED_PAYLOAD_FORMAT",
            domainContext: context,
            details: details ?? new { ProvidedFormat = providedFormat })
    {
        ProvidedFormat = providedFormat;
    }
}
