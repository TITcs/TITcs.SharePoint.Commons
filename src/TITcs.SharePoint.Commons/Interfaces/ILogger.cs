using System;

namespace TITcs.SharePoint.Commons.Interfaces
{
    public interface ILogger
    {
        void Information(string source);
        void Information(string source, string message, params object[] parameters);
        void Debug(string source);
        void Debug(string source, string message, params object[] parameters);
        void Unexpected(string source, string message, params object[] parameters);
        void Unexpected(string source, Exception exceptions);
    }
}
