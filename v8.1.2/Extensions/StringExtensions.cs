﻿namespace Microsoft.Pfe.Xrm
{
    public static class StringExtensions
    {
        public static string Fmt(this string format, params object[] args)
        {
            return string.Format(format, args);
        }
    }
}