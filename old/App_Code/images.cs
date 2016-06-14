using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for images
/// </summary>
public class images
{
    public double IMAGE_ID { get; set; }
    public string IMAGE_NAME { get; set; }
    public object IMAGE_ALT_TEXT { get; set; }
    public string IMAGE_DATA { get; set; }

    public images()
    {
        //
        // TODO: Add constructor logic here
        //
    }

    /// <summary>
    /// loads and image through a memory stream.
    /// </summary>
    /// <returns></returns>
    public Image LoadImage()
    {
        //data:image/gif;base64,
        byte[] bytes = Convert.FromBase64String(IMAGE_DATA);

        Image image;
        using (MemoryStream ms = new MemoryStream(bytes))
        {
            image = Image.FromStream(ms);
        }
        return image;
    }
}