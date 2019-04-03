using System;

namespace TITcs.SharePoint.Commons
{
    public static class Guards
    {
        public static void GuardAgainstNull(object obj)
        {
            if (obj == null) throw new ArgumentNullException("obj");
        }
    }
}
