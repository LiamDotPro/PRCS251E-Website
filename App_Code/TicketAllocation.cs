using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Ticket allocation class for the tickets within the database structure.
/// </summary>
public class TicketAllocation
{

    public double TICKET_ALLOCATION_ID { get; set; }
    public double TICKET_TYPE_ID { get; set; }
    public double QUANTITY { get; set; }
    public double PRICE { get; set; }
    public double EVENT_DAY_ID { get; set; }
    public double AVAILABLE { get; set; }

    public TicketAllocation()
    {
        //
        // TODO: Add constructor logic here
        //
    }

    /// <summary>
    /// gets the ticket allocation ID.
    /// </summary>
    /// <returns></returns>
    public double getEventDayID() {
        return EVENT_DAY_ID;
    }
}