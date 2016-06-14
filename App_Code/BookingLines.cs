/// <summary>
/// Classes for all of the booking lines otherwise known as tickets.
/// </summary>
public class BookingLines
{
    public double BOOKING_LINE_ID { get; set; }
    public double BOOKING_ID { get; set; }
    public double COST_PAID { get; set; }
    public double DELIVERY_TYPE { get; set; }
    public double TICKET_ALLOCATION_ID { get; set; }

    TicketAllocation bookingLineTicketAllocation { get; set; }

    public BookingLines()
    {
  
    }

    /// <summary>
    /// sets the ticket allocation to the booking like
    /// </summary>
    /// <param name="newAllocation">allocation to add</param>
    public void setTicketAllocation(TicketAllocation newAllocation)
    {
        bookingLineTicketAllocation = newAllocation;
    }

    /// <summary>
    /// gets the ticket allocation object
    /// </summary>
    /// <returns></returns>
    public TicketAllocation getTicketAllocation()
    {
        return bookingLineTicketAllocation;
    }

    /// <summary>
    /// get the booking line associated ticket Allocation ID
    /// </summary>
    /// <returns>returns the ticket ID</returns>
    public double getTicketAllocationID()
    {
        return TICKET_ALLOCATION_ID;
    }

    /// <summary>
    /// Finds if a booking has associated booking lines
    /// </summary>
    /// <returns></returns>
    public bool findAssociatedBookingLines(double BookingID)
    {
        if (BookingID == BOOKING_ID)
        {
            return true;
        }

        return false;
    }
}