namespace TMS.Ticketing.Domain.Venues;

public enum SectionType
{
    /// <summary>
    /// Specific designated place in a row.
    /// </summary>
    Designated = 1,

    /// <summary>
    /// Patrons can choose to sit anywhere within a particular section (for example, the dance floor)
    /// </summary>
    GeneralAdmission = 2
}