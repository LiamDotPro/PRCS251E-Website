using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Net;
using System.Linq;
using System.Web;
using System.Text;

/// <summary>
/// Summary description for order
/// </summary>
public class order
{
    private Events eventOrder;
    private List<EventDays> SelectedEventDaysOrder;

    public string Forename { get; set; }
    public string Surname { get; set; }
    public string Email { get; set; }
    public string Addrline1 { get; set; }
    public string Addrline2 { get; set; }
    public string Postcode { get; set; }
    public string PhoneNumber { get; set; }
    public string DOB { get; set; }
    public string County { get; set; }
    public string Country { get; set; }
    public string City { get; set; }
    public double TicketPrintOff { get; set; }

    private bool OrderSent { get; set; }

    private bool guest;
    private Persons member;

    public order()
    {
        SelectedEventDaysOrder = new List<EventDays>();
        eventOrder = new Events();
        guest = false;
        TicketPrintOff = 1;
    }

    /// <summary>
    /// Sets the event object to the order
    /// </summary>
    /// <param name="newEvent">the event to be added.</param>
    public void setEventOrder(Events newEvent)
    {
        eventOrder = newEvent;
    }

    /// <summary>
    /// Checks to see if the order is for a member or guest.
    /// </summary>
    /// <returns></returns>
    public bool checkMember()
    {
        if (member == null)
        {
            return false;
        }
        else {
            return true;
        }
    }

    /// <summary>
    /// gets the member associated with the order.
    /// </summary>
    /// <returns>the orders member object.</returns>
    public Persons getMember()
    {
        return member;
    }

    /// <summary>
    /// adds the eventday for the order.
    /// </summary>
    /// <param name="eventDay">eventday for purchase</param>
    public void setEventDays(EventDays eventDay)
    {
        SelectedEventDaysOrder.Add(eventDay);
    }


    /// <summary>
    /// sets a member too the order.
    /// </summary>
    /// <param name="newMember">Member object</param>
    public void setMember(Persons newMember)
    {
        member = newMember;
    }

    /// <summary>
    /// sets the order as a guest order.
    /// </summary>
    public void setGuest()
    {
        guest = true;
    }

    /// <summary>
    /// Returns all of the orders event days.
    /// </summary>
    /// <returns></returns>
    public List<EventDays> getEventDays()
    {
        return SelectedEventDaysOrder;
    }

    /// <summary>
    /// returns the associated events object.
    /// </summary>
    /// <returns></returns>
    public Events getEventObj()
    {
        return eventOrder;
    }

   /// <summary>
   /// Returns the orders event name.
   /// </summary>
   /// <returns></returns>
    public string getEventName()
    {
        return eventOrder.EVENT_NAME;
    }

    /// <summary>
    /// Decides if a person is a guest or member and creates an order.
    /// </summary>
    /// <returns>returns true if the order goes through.</returns>
    public bool createOrder()
    {
        if (guest == true)
        {
            createBookingLines(createBooking(createPerson()));
        }
        else {
            createBookingLines(createBooking(member.PERSON_ID));
        }
        OrderSent = true;
        return true;
    }


    /// <summary>
    /// Creates a booking lines for all of the associated tickets
    /// </summary>
    /// <param name="bookingID">The associated booking ID</param>
    private void createBookingLines(double bookingID)
    {
        List<BookingLines> NewBookingLines = new List<BookingLines>();

        //get all days selected and
        foreach (var item in SelectedEventDaysOrder)
        {
            for (int i = 0; i < item.getNumberOfTicets(); i++)
            {
                BookingLines Ticket = new BookingLines();

                Ticket.BOOKING_ID = bookingID;
                Ticket.DELIVERY_TYPE = TicketPrintOff;
                Ticket.COST_PAID = item.getEventDayPrice();
                Ticket.TICKET_ALLOCATION_ID = item.getTicketAllocation()[0].TICKET_ALLOCATION_ID;

                NewBookingLines.Add(Ticket);
            }

        }

        //update ticket allocations
        foreach (var item in SelectedEventDaysOrder)
        {
            if (item.getTicketAllocation().Count > 0)
            {
                TicketAllocation ticketAllocations = new TicketAllocation();
                ticketAllocations = item.getTicketAllocation()[0];

                ticketAllocations.AVAILABLE = ticketAllocations.AVAILABLE - item.getNumberOfTicets();


                string postData = JsonConvert.SerializeObject(ticketAllocations);
                var bookingApiLink = new Uri("http://xserve.uopnet.plymouth.ac.uk/Modules/INTPROJ/PRCS251E/Api/TicketAllocations/" + ticketAllocations.TICKET_ALLOCATION_ID.ToString());

                using (var client = new WebClient())
                {
                    client.Headers.Add(HttpRequestHeader.ContentType, "application/json");
                    client.UploadString(bookingApiLink, "PUT", postData);
                }
            }
        }

        //Post each individual Ticket to bookinglines
        foreach (var item in NewBookingLines)
        {

            string postData = JsonConvert.SerializeObject(item);
            var bookingApiLink = new Uri("http://xserve.uopnet.plymouth.ac.uk/Modules/INTPROJ/PRCS251E/Api/BookingLines");

            string response = "";

            using (var client = new WebClient())
            {
                client.Headers.Add(HttpRequestHeader.ContentType, "application/json");
                response = client.UploadString(bookingApiLink, "POST", postData);
            }


            BookingLines responseBookingLine = new BookingLines();

            responseBookingLine = JsonConvert.DeserializeObject<BookingLines>(response);

        }

    }

    /// <summary>
    /// If the person ordering a ticket is a guest this creates a person.
    /// </summary>
    /// <returns></returns>
    private double createPerson()
    {
        Persons newUser = new Persons();

        newUser.ADDRESS_LINE_ONE = Addrline1;
        newUser.ADDRESS_LINE_TWO = Addrline2;
        newUser.CITY = City;
        newUser.COUNTRY = Country;
        newUser.COUNTY = County;
        newUser.EMAIL = Email;
        newUser.FORENAME = Forename;
        newUser.SURNAME = Surname;
        newUser.POSTCODE = Postcode;
        newUser.PHONE_NUMBER = PhoneNumber;
        newUser.DOB = DateTime.Parse(DOB).ToString("yyyy/MM/dd HH:mm:ss");

        string postData = JsonConvert.SerializeObject(newUser);
        var bookingApiLink = new Uri("http://xserve.uopnet.plymouth.ac.uk/modules/INTPROJ/PRCS251E/api/persons");

        string Response = "";

        using (var client = new WebClient())
        {
            client.Headers.Add(HttpRequestHeader.ContentType, "application/json");
            Response = client.UploadString(bookingApiLink, "POST", postData);
        }

        Persons responseUser = new Persons();

        responseUser = JsonConvert.DeserializeObject<Persons>(Response);

        return responseUser.PERSON_ID;
    }


    /// <summary>
    /// Creates a booking.
    /// </summary>
    /// <param name="personID">The person the booking is associated too</param>
    /// <returns></returns>
    private double createBooking(double personID)
    {
        Bookings thisBooking = new Bookings();

        thisBooking.BOOKING_DATE = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
        thisBooking.DELIVERY_TYPE_ID = TicketPrintOff;
        thisBooking.PERSON_ID = personID;

        string postData = JsonConvert.SerializeObject(thisBooking);
        var bookingApiLink = new Uri("http://xserve.uopnet.plymouth.ac.uk/Modules/INTPROJ/PRCS251E/Api/Bookings");
        string response = "";


        using (var client = new WebClient())
        {
            client.Headers.Add(HttpRequestHeader.ContentType, "application/json");
            response = client.UploadString(bookingApiLink, "POST", postData);
        }

        Bookings responseBooking = new Bookings();

        responseBooking = JsonConvert.DeserializeObject<Bookings>(response);

        return responseBooking.BOOKING_ID;
    }
}