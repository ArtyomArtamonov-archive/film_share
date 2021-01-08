using System;
using System.Linq;

namespace FilmShare.Classes.Helpers{
    public static class RequestHelpers
    {
        public static string GetBoundary(string contentType)
        {
            var elements = contentType.Split(' ');
            var element = elements.First(e => e.StartsWith("boundary="));
            var boundary = element.Substring("boundary=".Length);
            if (boundary.Length >= 2 && boundary[0] == '"' && boundary[^1] == '"')
                boundary = boundary[1..^1];
            return boundary;
        }
        public static bool IsMultipartContentType(string contentType) => !string.IsNullOrEmpty(contentType) && contentType.IndexOf("multipart/", StringComparison.OrdinalIgnoreCase) >= 0;
    }
}