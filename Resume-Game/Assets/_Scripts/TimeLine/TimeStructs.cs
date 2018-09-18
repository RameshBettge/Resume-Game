using UnityEngine;

[System.Serializable]
public struct Date
{
    public int day;
    public Month month;
    public int year;

    public int Position
    {
        get { return year * 12 + (int)month; }
    }


    public string GetString()
    {
        string o = "";
        string sDay = day.ToString();

        if (day > 0)
        {
            if(day == 11)
            {
                o += "11th";
            }
            else if(sDay[sDay.Length -1] == '1')
            {
                o += sDay + "st";
            }
            else if (sDay[sDay.Length - 1] == '2')
            {
                o += sDay + "nd";
            }
            else if(sDay[sDay.Length - 1] == '3')
            {
                o += sDay + "rd";
            }
            else
            {
                o += sDay + "th";
            }

            o += " of ";
        }

        o += month.ToString() + " ";
        o += year;

        if (year.ToString().Length < 4 || month.ToString().Length < 1)
        {
            Debug.LogWarning("This date is not set properly! The standard format is: 'dd.mm.yyyy' OR 'mm.yyyy' . This date is set to: " + o);
        }

        return o;
    }
}

public enum Month
{
    January = 1, February, March, April, May, June, July, August, September, October, November, December
}

[System.Serializable]
public struct Phase
{
    public string title;

    public Date start;
    public bool inProgress;
    public Date end;
}

