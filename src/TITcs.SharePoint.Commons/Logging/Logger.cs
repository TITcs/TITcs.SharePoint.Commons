using Microsoft.SharePoint.Administration;
using System;
using System.Collections.Generic;
using System.Text;
using TITcs.SharePoint.Commons.Interfaces;

namespace TITcs.SharePoint.Commons.Logging
{
    public class Logger : SPDiagnosticsServiceBase, ILogger
    {
        #region fields and properties

        public static string DiagnosticAreaName = "TIT Framework";
        private static ILogger instance;

        /// <summary>
        /// Singleton instance that provides access to the logger infrastructure.
        /// </summary>
        public static ILogger Instance
        {
            get { return instance ?? (instance = new Logger()); }
        }

        #endregion

        #region constructors

        public Logger()
            : base("TITcs Logging", SPFarm.Local)
        {
        }

        #endregion

        #region events and methods

        /// <summary>
        /// Logs informational messages to the ULS.
        /// </summary>
        /// <param name="source">Source to which messages will be associated.</param>
        public void Information(string source)
        {
            Information(source, string.Empty);
        }

        /// <summary>
        /// Logs informational messages to the ULS.
        /// </summary>
        /// <param name="source">Source to which messages will be associated.</param>
        /// <param name="message">Message to log.</param>
        public void Information(string source, string message)
        {
            WriteLog(LoggerCategory.Information, source, message);
        }

        /// <summary>
        /// Logs informational messages to the ULS.
        /// </summary>
        /// <param name="source">Source to which messages will be associated.</param>
        /// <param name="message">Message to log.</param>
        /// <param name="parameters">Parameters to interpolate in the message content.</param>
        public void Information(string source, string message, params object[] parameters)
        {
            WriteLog(LoggerCategory.Information, source, string.Format(message, parameters));
        }

        /// <summary>
        /// Logs debug messages to the ULS.
        /// </summary>
        /// <param name="source">Source to which messages will be associated.</param>
        public void Debug(string source)
        {
            Debug(source, string.Empty);
        }

        /// <summary>
        /// Logs debug messages to the ULS.
        /// </summary>
        /// <param name="source">Source to which messages will be associated.</param>
        /// <param name="message">Message to log.</param>
        /// <param name="parameters">Parameters to interpolate in the message content.</param>
        public void Debug(string source, string message, params object[] parameters)
        {
#if DEBUG
            WriteLog(LoggerCategory.Debug, source, message != null ? string.Format(message, parameters) : string.Empty);
#endif
        }

        /// <summary>
        /// Logs errors to the ULS.
        /// </summary>
        /// <param name="source">Source to which messages will be associated.</param>
        /// <param name="message">Message to log.</param>
        public void Unexpected(string source, string message)
        {
            WriteLog(LoggerCategory.Unexpected, source, message);
        }

        /// <summary>
        /// Logs errors to the ULS.
        /// </summary>
        /// <param name="source">Source to which messages will be associated.</param>
        /// <param name="exception">Exception associated with the error.</param>
        public void Unexpected(string source, Exception exception)
        {
            var message = new StringBuilder();

            message.Append(exception.Message);

            if (exception.InnerException != null)
            {
                message.AppendLine(exception.InnerException.Message);
            }

            WriteLog(LoggerCategory.Unexpected, source, message.ToString());
        }

        /// <summary>
        /// Logs errors to the ULS.
        /// </summary>
        /// <param name="source">Source to which messages will be associated.</param>
        /// <param name="message">Message to log.</param>
        /// <param name="parameters">Parameters to interpolate in the message content.</param>
        public void Unexpected(string source, string message, params object[] parameters)
        {
            WriteLog(LoggerCategory.Unexpected, source, string.Format(message, parameters));
        }

        protected override IEnumerable<SPDiagnosticsArea> ProvideAreas()
        {
            return new List<SPDiagnosticsArea>
            {
                new SPDiagnosticsArea(DiagnosticAreaName, new List<SPDiagnosticsCategory>
                {
                    new SPDiagnosticsCategory("Unexpected", TraceSeverity.Unexpected, EventSeverity.Error),
                    new SPDiagnosticsCategory("High", TraceSeverity.High, EventSeverity.Warning),
                    new SPDiagnosticsCategory("Medium", TraceSeverity.Medium, EventSeverity.Information),
                    new SPDiagnosticsCategory("Information", TraceSeverity.Verbose, EventSeverity.Information),
                    new SPDiagnosticsCategory("Debug", TraceSeverity.Verbose, EventSeverity.Information)
                })
            };
        }
        private void WriteLog(LoggerCategory categoryName, string source, string errorMessage)
        {
            var category = this.Areas[DiagnosticAreaName].Categories[categoryName.ToString()];
            WriteTrace(0, category, category.TraceSeverity, string.Concat(string.Format("[{0}]", source), " ", errorMessage));
        }

        #endregion
    }
}
