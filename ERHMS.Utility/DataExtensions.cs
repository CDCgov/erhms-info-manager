﻿using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;

namespace ERHMS.Utility
{
    public static class DataExtensions
    {
        public const string AccessProvider = "Microsoft.Jet.OLEDB.4.0";

        private static bool IsCensorable(KeyValuePair<string, object> property)
        {
            return property.Key.ContainsIgnoreCase("Password") || property.Key.EqualsIgnoreCase("Pwd");
        }

        private static string FormatCensored(KeyValuePair<string, object> property)
        {
            return string.Format("{0}={1}", property.Key, IsCensorable(property) ? "?" : property.Value);
        }

        public static string GetCensoredConnectionString(this DbConnectionStringBuilder @this)
        {
            return string.Join(";", @this.Cast<KeyValuePair<string, object>>().Select(property => FormatCensored(property)));
        }

        public static bool IsEditable(this DataColumn @this)
        {
            return !@this.AutoIncrement && !@this.ReadOnly;
        }
    }
}
