using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Oversteer.Enums
{
    public enum WeatherType
    {
        [Display(Name = "Please Select")]
        PleaseSelect = 0,
        [Display(Name = "Realistic Weather forecast of the Chosen Track location")]
        Realistic = 1,
        [Display(Name = "Set Weather Conditions manually")]
        SetManual = 2
    }
}
