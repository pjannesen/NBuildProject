using System;
using System.Collections.Generic;
using System.Text;

namespace Jannesen.VisualStudioExtension.NBuildProject.Build.Library
{
    [Serializable]
    public class ConfigException: Exception
    {
        public              ConfigException(string message) : base(message)
        {
        }
        public              ConfigException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
