using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Class directly for the word cloud at the bottom of the page.
/// </summary>
public class Tags
{

    private double tagName;
    private double tagCount;

  
    public Tags(double startTagName)
    {
        tagName = startTagName;
        tagCount = 0;
    }

    /// <summary>
    /// gets the name of the tag
    /// </summary>
    /// <returns></returns>
    public double getTagName() {
        return tagName;
    }

    /// <summary>
    /// checks the tag count.
    /// </summary>
    /// <returns></returns>
    public double getTagCount() {
        return tagCount;
    }

    /// <summary>
    /// increases the tag count.
    /// </summary>
    public void increaseCount() {
        tagCount++;
    }

    /// <summary>
    /// sets the name of the tag.
    /// </summary>
    /// <param name="NewTagName">the value of the tag</param>
    public void setTagName(int NewTagName) {
        tagName = NewTagName;
    }

}