using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Services;

/// <summary>
/// Class for event days.
/// This class makes use of newton soft JSON libary, which you can find at http://www.newtonsoft.com/json.
/// </summary>
public class EventDays
{

    public double EVENT_DAY_ID { get; set; }
    public string START_TIMESTAMP { get; set; }
    public double EVENT_ID { get; set; }
    public object AGE_LIMIT { get; set; }
    public string EVENT_DAY_DATE { get; set; }
    public string EVENT_DAY_DESCRIPTION { get; set; }
    public double VENUE_ROOM_ID { get; set; }
    public double DURATION_HOURS { get; set; }

    private List<VenueRooms> EventDayVenue { get; set; }
    private List<TicketAllocation> EventDayAllocation { get; set; }
    private int numberOfTickets;

    public EventDays()
    {
        EventDayVenue = new List<VenueRooms>();
        EventDayAllocation = new List<TicketAllocation>();
    }

    /// <summary>
    /// Sets the ticket allocation for this eventday.
    /// </summary>
    public void setTicketAllocation() {

        List<TicketAllocation> ticketAllocations = new List<TicketAllocation>();
        using (WebClient wc = new WebClient())
        {
            var json = wc.DownloadString("http://xserve.uopnet.plymouth.ac.uk/Modules/INTPROJ/PRCS251E/Api/TicketAllocations");
            ticketAllocations = JsonConvert.DeserializeObject<List<TicketAllocation>>(json);
        }

        foreach (var item in ticketAllocations)
        {
            if (item.EVENT_DAY_ID == EVENT_DAY_ID) {
                EventDayAllocation.Add(item);
            }
        }

    }

    /// <summary>
    /// gets the events days ticket allocations.
    /// </summary>
    /// <returns></returns>
    public List<TicketAllocation> getTicketAllocation() {
        return EventDayAllocation;
    }

    /// <summary>
    /// add venue rooms to the days
    /// </summary>
    /// <param name="aRoom">a room to be added</param>
    public void addVenueRooms(VenueRooms aRoom)
    {
        EventDayVenue.Add(aRoom);
    }

    /// <summary>
    /// gets the number of tickets associated to the day, used in the order.
    /// </summary>
    /// <returns> integer number of tickets</returns>
    public int getNumberOfTicets() {
        return numberOfTickets;
    }

    /// <summary>
    /// gets the list of venue rooms.
    /// </summary>
    /// <returns>The venue room list</returns>
    public List<VenueRooms> getVenueRooms()
    {
        return EventDayVenue;
    }

    /// <summary>
    /// Gets the venue room for the day.
    /// </summary>
    /// <returns>The venue room</returns>
    public VenueRooms getVenueRoom() {
        return EventDayVenue[0];
    }

    public double getEventDayPrice() {
        if (EventDayAllocation.Count > 0) {
            return EventDayAllocation[0].PRICE;
        }
        else
        {
            return 0;
        }

        
    }
    
    /// <summary>
    /// Sets the number of tickets being ordered.
    /// </summary>
    /// <param name="numOfTickets">The number of tickets</param>
    public void setNumberOfTickets(int numOfTickets) {
        numberOfTickets = numOfTickets;
    }

    /// <summary>
    /// Gets the quanitiy of tickets avaliable.
    /// </summary>
    /// <returns></returns>
    public string getTicketQuantity()
    {
        if (EventDayAllocation.Count > 0) {
            return EventDayAllocation[0].AVAILABLE.ToString();
        }
        else
        {
            return "Ticket count unavaliable";
        }

    }
}