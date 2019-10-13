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