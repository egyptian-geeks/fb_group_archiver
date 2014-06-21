using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GroupArchiver
{
   public static class EX
    {
       public static long ToEpoch(this DateTime value)
       {
           long epoch = (value.Ticks - 621355968000000000) / 10000000;
           return epoch;
       }
    }
}
