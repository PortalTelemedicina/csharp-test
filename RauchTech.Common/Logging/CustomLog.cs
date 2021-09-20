using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Demo.Common.Logging
{
    public class CustomLog : CustomLogFactory, ICustomLog
    {
        public ILogger ILogger { get; set; }

        public CustomLog(ILoggerFactory loggerFactory) : base(loggerFactory)
        {
            ILoggerFactory = loggerFactory;
        }
        public CustomLog(CustomLogFactory customLogFactory) : base(customLogFactory.ILoggerFactory)
        {
            IDs = customLogFactory.IDs;
            LogHistory = customLogFactory.LogHistory;
            ILoggerFactory = customLogFactory.ILoggerFactory;
        }

        public void LogCustom(LogLevel logLevel,
                                EventId? eventId = null,
                                Exception exception = null,
                                string message = null,
                                [CallerMemberName] string memberName = "",
                                [CallerLineNumber] int sourceLineNumber = 0,
                                params ValueTuple<string, object>[] args)
        {
            if (ILogger.IsEnabled(logLevel))
            {
                AddLogHistory(logger: ILogger,
                            logLevel: logLevel,
                            eventId: eventId,
                            exception: exception,
                            message: message,
                            memberName: memberName,
                            sourceLineNumber: sourceLineNumber,
                            args: args
                            );
            }
        }
    }

    public class CustomLogFactory : IDisposable, ICustomLogFactory
    {
        #region Log Memory

        public class LogItem
        {
            public ILogger Logger { get; }
            public long CurrentStep { get; }
            public DateTime CurrentTime { get; }

            public LogLevel LogLevel { get; }
            public EventId? EventId { get; }
            public Exception Exception { get; }
            public string Message { get; set; }
            public string MemberName { get; }
            public int SourceLineNumber { get; }

            public ValueTuple<string, object>[] Args { get; }

            public LogItem(ILogger logger,
                            long currentStep,
                            LogLevel logLevel,
                            EventId? eventId = null,
                            Exception exception = null,
                            string message = null,
                            string memberName = "",
                            int sourceLineNumber = 0,
                            params ValueTuple<string, object>[] args)
            {
                Logger = logger;
                CurrentStep = currentStep;
                CurrentTime = DateTime.Now;

                LogLevel = logLevel;
                EventId = eventId;
                Exception = exception;
                Message = message;
                MemberName = memberName;
                SourceLineNumber = sourceLineNumber;
                Args = args;
            }
        }
        public class CustomLogHistory
        {
            public List<LogItem> LogItems { get; set; } = new List<LogItem>();
        }
        public class CustomLogVault
        {
            public List<(string, object)> Keys { get; set; } = new List<(string, object)>();
        }

        #endregion

        #region Properties

        public CustomLogVault IDs { get; set; }
        public CustomLogHistory LogHistory { get; set; }
        public ILoggerFactory ILoggerFactory { get; set; }
        #endregion


        #region Constructor

        public CustomLogFactory(ILoggerFactory loggerFactory)
        {
            ILoggerFactory = loggerFactory;
            IDs = new CustomLogVault();
            LogHistory = new CustomLogHistory();
        }
        #endregion


        public CustomLog CreateLogger<T>() where T : class
        {
            CustomLog log = new CustomLog(this);
            log.ILogger = ILoggerFactory.CreateLogger<T>();
            return log;
        }

        //Add Scoped Identifiers, that will be preserved in all Logs (prevents duplication)
        public void AddID(string key, object value)
        {
            if (!IDs.Keys.Any(x => x.Item1 == key))
            {
                IDs.Keys.Add((key, value));
            }
        }

        public void AddLogHistory(ILogger logger,
                                    LogLevel logLevel,
                                    EventId? eventId = null,
                                    Exception exception = null,
                                    string message = null,
                                    string memberName = "",
                                    int sourceLineNumber = 0,
                                    params ValueTuple<string, object>[] args)
        {
            long currentStep = (LogHistory.LogItems.Count == 0) ? 1 : (LogHistory.LogItems.Max(x => x.CurrentStep) + 1);
            LogHistory.LogItems.Add(new LogItem
                                    (
                                        logger: logger,
                                        currentStep: currentStep,
                                        logLevel: logLevel,
                                        eventId: eventId,
                                        exception: exception,
                                        message: message,
                                        memberName: memberName,
                                        sourceLineNumber: sourceLineNumber,
                                        args: args
                                    ));
        }

        private void FinishLogging(LogItem logItem)
        {
            logItem.Message ??= ICustomLog.LineMarker;

            List<(string, object)> temp = logItem.Args.ToList();
            foreach ((string, object) item in IDs.Keys)
            {
                if (!temp.Any(x => x.Item1 == item.Item1))
                {
                    temp.Add((item.Item1, item.Item2));
                }
            }

            CustomLogData customLogData = new CustomLogData(logItem.Message, logItem.MemberName, logItem.SourceLineNumber, temp.ToArray());

            string data = JsonConvert.SerializeObject(customLogData);

            if (logItem.Exception is null)
            {
                if (logItem.EventId is null)
                {
                    logItem.Logger.Log(logItem.LogLevel, "{Timestamp}{data}{CurrentStep}", logItem.CurrentTime, data, logItem.CurrentStep);
                }
                else
                {
                    logItem.Logger.Log(logItem.LogLevel, logItem.EventId.Value, "{Timestamp}{data}{CurrentStep}", logItem.CurrentTime, data, logItem.CurrentStep);
                }
            }
            else
            {
                if (logItem.EventId is null)
                {
                    logItem.Logger.Log(logItem.LogLevel, logItem.Exception, "{Timestamp}{data}{CurrentStep}", logItem.CurrentTime, data, logItem.CurrentStep);
                }
                else
                {
                    logItem.Logger.Log(logItem.LogLevel, logItem.EventId.Value, logItem.Exception, "{Timestamp}{data}{CurrentStep}", logItem.CurrentTime, data, logItem.CurrentStep);
                }
            }
        }

        public void Dispose()
        {
            LogHistory.LogItems.ForEach(h => FinishLogging(h));
        }

        #region Helper

        protected class CustomLogData
        {
            public CustomLogData(string message,
                                 string method,
                                 int line,
                                 params ValueTuple<string, object>[] values)
            {
                Method = method;
                Line = line;
                Message = message;

                if (values.Length > 0)
                {
                    Values = GetDynamicObject(values.ToDictionary(x => x.Item1, x => x.Item2));
                }
            }

            public string Method { get; }
            public int Line { get; }
            public string Message { get; }

            public dynamic Values { get; }
        }


        private static dynamic GetDynamicObject(Dictionary<string, object> properties)
        {
            return new MyDynObject(properties);
        }

        private sealed class MyDynObject : DynamicObject
        {
            private readonly Dictionary<string, object> _properties;

            public MyDynObject(Dictionary<string, object> properties)
            {
                _properties = properties;
            }

            public override IEnumerable<string> GetDynamicMemberNames()
            {
                return _properties.Keys;
            }

            public override bool TryGetMember(GetMemberBinder binder, out object result)
            {
                if (_properties.ContainsKey(binder.Name))
                {
                    result = _properties[binder.Name];
                    return true;
                }
                else
                {
                    result = null;
                    return false;
                }
            }

            public override bool TrySetMember(SetMemberBinder binder, object value)
            {
                if (_properties.ContainsKey(binder.Name))
                {
                    _properties[binder.Name] = value;
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        #endregion
    }
}
