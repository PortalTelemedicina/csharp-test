using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace Demo.Common.Logging
{
    public interface ICustomLog
    {
        const string Begin = "Begin";
        const string LineMarker = "Line";
        const string Finish = "Finish";

        void AddID(string key, object value);

        void LogCustom(LogLevel logLevel,
                        EventId? eventId = null,
                        Exception exception = null,
                        string message = null,
                        [CallerMemberName] string memberName = "",
                        [CallerLineNumber] int sourceLineNumber = 0,
                        params ValueTuple<string, object>[] args);
    }

    public interface ICustomLogFactory
    {
        CustomLog CreateLogger<T>() where T : class;
    }
}
