using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;

/// <summary>
/// class for event information within the website.
/// This class makes use of newton soft JSON libary, which you can find at http://www.newtonsoft.com/json.
/// </summary>
public class Events
{
    public double EVENT_ID { get; set; }
    public string EVENT_NAME { get; set; }
    public double EVENT_TYPE_ID { get; set; }
    public string START_DATE { get; set; }
    public string END_DATE { get; set; }
    public string EVENT_DETAILS { get; set; }
    public object NORMAL_IMAGE { get; set; }
    public object BANNER_IMAGE { get; set; }

    private List<String> eventTypes;

    public Events()
    {
        eventTypes = new List<String>();
    }

    public double getEventID()
    {
        return EVENT_ID;
    }

    public void addEventTypes(string eventType)
    {
        eventTypes.Add(eventType);
    }

    public List<String> getEventTypes()
    {
        return eventTypes;
    }

    /// <summary>
    /// gets the banner image for teh event.
    /// </summary>
    /// <returns>Api encoded image as string</returns>
    public string getBannerImg() {
        List<images> imgData = new List<images>();
        using (WebClient wc = new WebClient())
        {
            var json = wc.DownloadString("http://xserve.uopnet.plymouth.ac.uk/modules/INTPROJ/PRCS251E/api/images");
            imgData = JsonConvert.DeserializeObject<List<images>>(json);
        }

        IConvertible convert = BANNER_IMAGE as IConvertible;

        foreach (var item in imgData)
        {
            if (convert != null)
            {
                if (convert.ToDouble(null) == item.IMAGE_ID)
                {
                    return "data:image/jpg;base64," + item.IMAGE_DATA;
                }
            }
        }


        return getDefaultBannerImage();

    }

    /// <summary>
    /// Gets the default banner image from the API.
    /// </summary>
    /// <param name="imgData"></param>
    /// <returns>Api encoded image as string</returns>
    private string getDefaultBannerImage()
    {
        return "img/banner.jpg";
    }

    /// <summary>
    /// Gets the default pop icon from the api.
    /// </summary>
    /// <param name="imgData"></param>
    /// <returns>Api encoded image as string</returns>
    private string getDefaultImage()
    {
        return "img/noImg.jpg";
    }

    /// <summary>
    /// Gets dynamic image data from the api.
    /// </summary>
    /// <returns>Api encoded image as string</returns>
    public string getImageData()
    {
        List<images> imgData = new List<images>();
        using (WebClient wc = new WebClient())
        {
            var json = wc.DownloadString("http://xserve.uopnet.plymouth.ac.uk/modules/INTPROJ/PRCS251E/api/images");
            imgData = JsonConvert.DeserializeObject<List<images>>(json);
        }

        IConvertible convert = NORMAL_IMAGE as IConvertible;

        foreach (var item in imgData)
        {
            if (convert != null)
            {
                if (convert.ToDouble(null) == item.IMAGE_ID)
                {
                    return "data:image/jpg;base64, " + item.IMAGE_DATA;
                }
            }
        }


        return getDefaultImage();
    }
}