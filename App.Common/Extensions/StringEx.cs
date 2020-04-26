using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Common.Extentions
{
  public static class StringEx
  {
    public static string ToDebugString<T>(this T[] @obj, string separator = "; ", string start = "{", string finish = "}")
    {
      return start +  string.Join(" ", @obj) + finish;
    }
  }
}
