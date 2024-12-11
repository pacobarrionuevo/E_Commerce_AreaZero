﻿namespace E_Commerce_VS.Extensions
{
    public static class HttpExtensions
    {
        //Jose
        public static string GetBaseUrl(this HttpRequest request)
        {
            return $"{request.Scheme}://{request.Host}/";
        }

        public static string GetAbsoluteUrl(this HttpRequest request, string relativeUrl)
        {
            Uri baseUrl = new Uri(request.GetBaseUrl());

            return new Uri(baseUrl, relativeUrl).ToString();
        }
    }
}
