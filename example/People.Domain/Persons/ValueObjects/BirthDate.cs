namespace People.Domain.Persons.ValueObjects;

using System;

/// <summary>
/// Represents a person's birth date with age calculation and birthday checking capabilities.
/// </summary>
public record BirthDate
{
    /// <summary>
    /// Gets the birth date value.
    /// </summary>
    public DateOnly Value { get; init; }

    /// <summary>
    /// Protected constructor for EF Core and deserialization.
    /// </summary>
    protected BirthDate() => Value = default;

    /// <summary>
    /// Initializes a new instance of <see cref="BirthDate"/>.
    /// </summary>
    /// <param name="date">The birth date. Cannot be in the future.</param>
    /// <exception cref="ArgumentException">Thrown when the date is invalid or in the future.</exception>
    public BirthDate(DateOnly date)
    {
        var today = DateOnly.FromDateTime(DateTime.UtcNow);

        if (date > today)
            throw new ArgumentException("Birth date cannot be in the future.", nameof(date));

        // Validate reasonable minimum year (e.g., 100 years ago)
        var minDate = today.AddYears(-150);
        if (date < minDate)
            throw new ArgumentException("Birth date is too far in the past (more than 150 years ago).", nameof(date));

        Value = date;
    }

    /// <summary>
    /// Creates a BirthDate from year, month, and day.
    /// </summary>
    /// <param name="year">The birth year.</param>
    /// <param name="month">The birth month (1-12).</param>
    /// <param name="day">The birth day.</param>
    /// <returns>A new BirthDate instance.</returns>
    /// <exception cref="ArgumentException">Thrown when the date is invalid.</exception>
    public static BirthDate From(int year, int month, int day)
        => new(new DateOnly(year, month, day));

    /// <summary>
    /// Creates a BirthDate from a DateTime.
    /// </summary>
    /// <param name="dateTime">The birth date and time. Only the date part is used.</param>
    /// <returns>A new BirthDate instance.</returns>
    /// <exception cref="ArgumentException">Thrown when the date is invalid.</exception>
    public static BirthDate From(DateTime dateTime)
        => new(DateOnly.FromDateTime(dateTime));

    /// <summary>
    /// Gets the person's age in years, calculated from the birth date.
    /// </summary>
    /// <param name="referenceDate">The reference date to calculate age from. Defaults to today.</param>
    /// <returns>The age in years.</returns>
    public int GetAge(DateOnly? referenceDate = null)
    {
        var reference = referenceDate ?? DateOnly.FromDateTime(DateTime.UtcNow);
        var age = reference.Year - Value.Year;

        // Adjust if birthday hasn't occurred yet this year
        if (reference.Month < Value.Month ||
            (reference.Month == Value.Month && reference.Day < Value.Day))
        {
            age--;
        }

        return age;
    }

    /// <summary>
    /// Gets the person's age in years as of today.
    /// </summary>
    /// <returns>The current age in years.</returns>
    public int GetCurrentAge()
        => GetAge(DateOnly.FromDateTime(DateTime.UtcNow));

    /// <summary>
    /// Checks if the birthday falls on the specified date.
    /// </summary>
    /// <param name="date">The date to check.</param>
    /// <returns><c>true</c> if the birthday is on the specified date; otherwise, <c>false</c>.</returns>
    public bool IsBirthday(DateOnly date)
        => date.Month == Value.Month && date.Day == Value.Day;

    /// <summary>
    /// Checks if today is the person's birthday.
    /// </summary>
    /// <returns><c>true</c> if today is the birthday; otherwise, <c>false</c>.</returns>
    public bool IsBirthdayToday()
        => IsBirthday(DateOnly.FromDateTime(DateTime.UtcNow));

    /// <summary>
    /// Gets the next birthday date from the reference date.
    /// </summary>
    /// <param name="referenceDate">The reference date. Defaults to today.</param>
    /// <returns>The next birthday date.</returns>
    public DateOnly GetNextBirthday(DateOnly? referenceDate = null)
    {
        var reference = referenceDate ?? DateOnly.FromDateTime(DateTime.UtcNow);
        var thisYearBirthday = new DateOnly(reference.Year, Value.Month, Value.Day);

        // If birthday already passed this year, next birthday is next year
        if (thisYearBirthday < reference)
        {
            return new DateOnly(reference.Year + 1, Value.Month, Value.Day);
        }

        return thisYearBirthday;
    }

    /// <summary>
    /// Gets the number of days until the next birthday from the reference date.
    /// </summary>
    /// <param name="referenceDate">The reference date. Defaults to today.</param>
    /// <returns>The number of days until the next birthday.</returns>
    public int DaysUntilNextBirthday(DateOnly? referenceDate = null)
    {
        var reference = referenceDate ?? DateOnly.FromDateTime(DateTime.UtcNow);
        var nextBirthday = GetNextBirthday(reference);
        return (nextBirthday.ToDateTime(TimeOnly.MinValue) - reference.ToDateTime(TimeOnly.MinValue)).Days;
    }

    /// <summary>
    /// Returns the birth date as a string in ISO format (yyyy-MM-dd).
    /// </summary>
    public override string ToString()
        => Value.ToString("yyyy-MM-dd");

    /// <summary>
    /// Implicitly converts a BirthDate to a DateOnly.
    /// </summary>
    /// <param name="birthDate">The birth date.</param>
    public static implicit operator DateOnly(BirthDate birthDate)
        => birthDate.Value;

    /// <summary>
    /// Implicitly converts a DateOnly to a BirthDate.
    /// </summary>
    /// <param name="date">The date.</param>
    public static implicit operator BirthDate(DateOnly date)
        => new(date);
}

