namespace Core.Libraries.Domain.Exceptions;

/// <summary>
/// Represents a base exception for violations of explicit business rules within the domain model.
/// </summary>
/// <remarks>
/// Use this exception to represent domain-level constraints or policies that must not be violated.
/// Subclasses should define specific rules (e.g., credit limits, user eligibility, etc.).
/// </remarks>
public abstract class BusinessRuleViolationException : DomainException
{
    /// <summary>
    /// Gets the name or code of the business rule that was violated.
    /// </summary>
    public string Rule { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="BusinessRuleViolationException"/> class.
    /// </summary>
    /// <param name="rule">The name or code of the business rule violated.</param>
    /// <param name="message">The exception message.</param>
    /// <param name="domainContext">The name of the entity or aggregate where the violation occurred.</param>
    /// <param name="details">Optional structured details for diagnostics.</param>
    protected BusinessRuleViolationException(string rule, string message, string domainContext, object? details = null)
        : base(message, "BUSINESS_RULE_VIOLATION", domainContext, details)
    {
        Rule = rule;
    }
}
