using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// database class for all of the current bookings.
/// </summary>
public class Bookings
{

    public double BOOKING_ID { get; set; }
    public double PERSON_ID { get; set; }
    public string BOOKING_DATE { get; set; }
    public double DELIVERY_TYPE_ID { get; set; }



    public Bookings()
    {
        //
        // TODO: Add constructor logic here
        //
    }
}