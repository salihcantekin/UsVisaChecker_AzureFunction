using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UsVisaChecker.AF.Models;
public class NotificationRequestModel
{
    public DateTime CreateDate { get; set; }

    public int TotalAvailableDate { get; set; }

    public DateTime OriginalDate { get; set; }

    public DateTime EarliestDate { get; set; }

}
