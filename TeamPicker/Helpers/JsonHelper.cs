using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using ServiceStack.Text;

namespace TeamPicker.Helpers
{
    internal static class JsonHelper
    {
        static public string ToJSON(this object item)
        {
            return JsonSerializer.SerializeToString(item);
        }
        static public T FromJSON<T>(string code)
        {
            return JsonSerializer.DeserializeFromString<T>(code);
        }
    }
}