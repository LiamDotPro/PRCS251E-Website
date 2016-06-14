using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Linq;
using System.Web;
using System.Net;
using System.Web.Services;

/// <summary>
/// class for CurrentBookings within the website.
/// This class makes use of newton soft JSON libary, which you can find at http://www.newtonsoft.com/json.
/// </summary>
public class CurrentBookings
{
    string BookingDate { get; set; }
    double bookingID { get; set; }
    double ticketAllocationID { get; set; }
    List<BookingLines> orderBookingLines { get; set; }
    List<TicketAllocation> associatedTicketAllocation { get; set; }
    List<EventDays> EventDays { get; set; }
    Events EventBooking { get; set; }

    public CurrentBookings(string NewBookingDate, double newBookingID)
    {
        BookingDate = NewBookingDate;
        bookingID = newBookingID;
        EventDays = new List<EventDays>();
        orderBookingLines = new List<BookingLines>();
    }

    /// <summary>
    /// gets the associated ticket allocation ID.
    /// </summary>
    /// <returns></returns>
    public double getTicketAllocationID()
    {
        return ticketAllocationID;
    }

    /// <summary>
    /// sets and event to the booking.
    /// </summary>
    /// <param name="thisEvent">the event object</param>
    public void setEvent(Events thisEvent)
    {
        EventBooking = thisEvent;
    }


    /// <summary>
    /// Adds all of the eventday venues to the venues room list
    /// </summary>
    public void addEventDayVenues()
    {
        List<VenueRooms> VenueRoomsList = new List<VenueRooms>();
        using (WebClient wc = new WebClient())
        {
            var json = wc.DownloadString("http://xserve.uopnet.plymouth.ac.uk/modules/INTPROJ/PRCS251E/api/venuerooms");
            VenueRoomsList = JsonConvert.DeserializeObject<List<VenueRooms>>(json);
        }

        foreach (var item in VenueRoomsList)
        {
            foreach (var day in EventDays)
            {
                if (day.VENUE_ROOM_ID == item.VENUE_ROOM_ID)
                {
                    day.addVenueRooms(item);
                }
            }
        }
    }

    /// <summary>
    /// gets the current venue
    /// </summary>
    /// <param name="bookingLineID">the booklingID</param>
    /// <returns>the venues room name</returns>
    public string getVenue(double bookingLineID) {

        if (EventDays.Count > 0) {
            foreach (var item in orderBookingLines)
            {
                if (bookingLineID == item.BOOKING_LINE_ID)
                {
                    foreach (var days in EventDays)
                    {
                        if (item.getTicketAllocation().EVENT_DAY_ID == days.EVENT_DAY_ID )
                        {
                            return days.getVenueRoom().ROOM_NAME;
                        }
                    }
                  
                }
            }
        }
        else
        {
            return "Unavaliable";
        }

        return "Unavaliable";
    }

    /// <summary>
    /// gets the start and end date for the current booking
    /// </summary>
    /// <param name="bookingLineID">the booking line ID</param>
    /// <returns>the end and start date formatted and a string</returns>
    public string getStartAndEndDateTime(double bookingLineID) {

        if (EventDays.Count > 0)
        {
            foreach (var item in orderBookingLines)
            {
                if (bookingLineID == item.BOOKING_LINE_ID)
                {
                    foreach (var days in EventDays)
                    {
                        if (item.getTicketAllocation().EVENT_DAY_ID == days.EVENT_DAY_ID)
                        {
                            DateTime returnTime = DateTime.Parse(days.EVENT_DAY_DATE);
                            string outputString = "";

                            outputString += returnTime.ToShortDateString();
                            outputString += " - ";
                            outputString += returnTime.TimeOfDay.ToString();
                                 
                            return outputString;
                        }
                    }

                }
            }
        }
        else
        {
            return "Unavaliable";
        }

        return "Unavaliable";
    }

    /// <summary>
    /// adds a event day to the curent booking.
    /// </summary>
    /// <param name="eventDay">an event day</param>
    public void addEventDays(EventDays eventDay)
    {
        EventDays.Add(eventDay);
    }

    /// <summary>
    /// Gets the current booking events name.
    /// </summary>
    /// <returns> the events booking name </returns>
    public string getEventName()
    {
        return EventBooking.EVENT_NAME;
    }

    /// <summary>
    /// Gets the events staring date.
    /// </summary>
    /// <returns></returns>
    public string getEventStartDate()
    {
        return EventBooking.START_DATE;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public string getEventEndDate()
    {
        return EventBooking.END_DATE;
    }

    /// <summary>
    /// gets the current booking event object
    /// </summary>
    /// <returns>an event object</returns>
    public Events getEventObj() {
        return EventBooking;
    }

    /// <summary>
    /// gets the eventDays count
    /// </summary>
    /// <returns></returns>
    public int getEventDaysCount() {
        return EventDays.Count();
    }

    /// <summary>
    /// gets the event ID
    /// </summary>
    /// <returns>returns the event ID</returns>
    public double getEventID()
    {
            return EventDays[0].EVENT_ID;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="thisTicketAllocation"></param>
    public void setAssociatedTicketAllocation(TicketAllocation thisTicketAllocation)
    {
        associatedTicketAllocation.Add(thisTicketAllocation);
    }

    /// <summary>
    /// Gets the associated ticket allocation
    /// </summary>
    /// <param name="thisLine">the associated bookinline</param>
    /// <returns>the ticket allocation</returns>
    public TicketAllocation getAssociatedTicketAllocation(BookingLines thisLine)
    {
        foreach (var item in orderBookingLines)
        {
            if (item.BOOKING_LINE_ID == thisLine.BOOKING_LINE_ID) {
                return item.getTicketAllocation();
            }

        }

        return null;
    }

    /// <summary>
    /// adds and booking like to the orderlines
    /// </summary>
    /// <param name="thisBookingLine"></param>
    public void addOrderLine(BookingLines thisBookingLine)
    {
        orderBookingLines.Add(thisBookingLine);
    }

    /// <summary>
    /// Gets the toal price of the booking
    /// </summary>
    /// <returns>the overall price of the booking.</returns>
    public double getTotalPrice()
    {
        double totalPrice = 0;

        foreach (var item in orderBookingLines)
        {
            totalPrice += item.COST_PAID;
        }
        return totalPrice;

    }

    /// <summary>
    /// get the orderlines for the booking.
    /// </summary>
    /// <returns>the associated bookinglines</returns>
    public List<BookingLines> getOrderLine()
    {
        return orderBookingLines;
    }

    /// <summary>
    /// sets the current bookings ticket allocation.
    /// </summary>
    /// <param name="allocationID">takes the allocation ID</param>
    public void setTicketAllocationID(double allocationID)
    {
        ticketAllocationID = allocationID;
    }

    /// <summary>
    /// gets the current bookings, booking ID
    /// </summary>
    /// <returns></returns>
    public double getBookingID()
    {
        return bookingID;
    }

}