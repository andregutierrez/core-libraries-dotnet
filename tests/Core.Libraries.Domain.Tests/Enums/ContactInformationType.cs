namespace Core.LibrariesDomain.Enums
{
    /// <summary>
    /// Enumerates the different kinds of contact information that can be stored for an individual.
    /// </summary>
    public enum ContactInformationType : int
    {
        /// <summary>
        /// A landline telephone number.
        /// </summary>
        Phone = 1,

        /// <summary>
        /// A cellular (mobile) phone number.
        /// </summary>
        Mobile = 2,

        /// <summary>
        /// An email address.
        /// </summary>
        Email = 4,

    }
}
