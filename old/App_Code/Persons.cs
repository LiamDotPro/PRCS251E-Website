using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;

/// <summary>
/// Class for persons within the website linked to the account table in page logic.
/// </summary>
public class Persons
{
    public double PERSON_ID { get; set; }
    public string FORENAME { get; set; }
    public string SURNAME { get; set; }
    public string EMAIL { get; set; }
    public string ADDRESS_LINE_ONE { get; set; }
    public object ADDRESS_LINE_TWO { get; set; }
    public string POSTCODE { get; set; }
    public string PHONE_NUMBER { get; set; }
    public string DOB { get; set; }
    public string COUNTY { get; set; }
    public string COUNTRY { get; set; }
    public string CITY { get; set; }

    public Persons()
    {
        //
        // TODO: Add constructor logic here
        //
    }

    /// <summary>
    /// checks if the person already exists
    /// </summary>
    /// <returns>boolean value</returns>
    public bool checkPerson() {

        List<Persons> PersonList = new List<Persons>();
        List<Accounts> AccountList = new List<Accounts>();

        using (WebClient wc = new WebClient())
        {
            var json = wc.DownloadString("http://xserve.uopnet.plymouth.ac.uk/modules/INTPROJ/PRCS251E/api/Persons");
            PersonList = JsonConvert.DeserializeObject<List<Persons>>(json);
            json = wc.DownloadString("http://xserve.uopnet.plymouth.ac.uk/modules/INTPROJ/PRCS251E/api/Accounts");
            AccountList = JsonConvert.DeserializeObject<List<Accounts>>(json);
        }

        foreach (var item in PersonList)
        {
            if (EMAIL == item.EMAIL)
            {
                foreach (var account in AccountList)
                {
                    if (item.PERSON_ID == account.PERSON_ID)
                    {
                        return true;
                    }
                }
            }
            
        }
        return false;
    }

    /// <summary>
    /// creates a person within the api and returns their firstname.
    /// </summary>
    /// <param name="password">The password to be associated in accounts.</param>
    /// <returns>new users forename</returns>
    public string createPerson(string password) {

        string postData = JsonConvert.SerializeObject(this);
        var bookingApiLink = new Uri("http://xserve.uopnet.plymouth.ac.uk/modules/INTPROJ/PRCS251E/api/Persons");

        string Response = "";

        using (var client = new WebClient())
        {
            client.Headers.Add(HttpRequestHeader.ContentType, "application/json");
            Response = client.UploadString(bookingApiLink, "POST", postData);
        }

        Persons responseUser = new Persons();

        responseUser = JsonConvert.DeserializeObject<Persons>(Response);

        Accounts newAccount = new Accounts();

        bookingApiLink = new Uri("http://xserve.uopnet.plymouth.ac.uk/modules/INTPROJ/PRCS251E/api/Accounts");

        newAccount.PERSON_ID = responseUser.PERSON_ID;
        newAccount.PASSWORD = password;

        postData = JsonConvert.SerializeObject(newAccount);

        using (var client = new WebClient())
        {
            client.Headers.Add(HttpRequestHeader.ContentType, "application/json");
            Response = client.UploadString(bookingApiLink, "POST", postData);
        }

        return responseUser.FORENAME;

    }
}