using System;
using System.Globalization;

public class DateModifier
{
    private DateTime firstDate;
    private DateTime secondDate;

    public DateModifier(DateTime firstDate, DateTime secondDate)
    {
        this.firstDate = firstDate;
        this.secondDate = secondDate;
    }

    public double CalculateDiferance()
    {

        var result = Math.Abs((this.firstDate - this.secondDate).TotalDays);

        return result;
    }
}