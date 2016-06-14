using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Database class for the venues within that link to the venue rooms.
/// </summary>
public class venues
{

    public double VENUE_ID { get; set; }
    public string VENUE_NAME { get; set; }
    public string POSTCODE { get; set; }
    public object DETAILS { get; set; }
    public string ADDRESS_LINE { get; set; }
    public string PHONE_NUMBER { get; set; }
    public string COUNTY { get; set; }
    public string COUNTRY { get; set; }
    public string CITY { get; set; }

    public venues()
    {
    }
}