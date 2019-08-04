using System;

namespace Jannesen.VisualStudioExtension.NBuildProject.Build
{
    [Serializable]
    public class BuildException: Exception
    {
        public          BuildException() : base()
        {
        }
        public          BuildException(string message): base(message)
        {
        }
        public          BuildException(string message, Exception innerException): base(message, innerException)
        {
        }
        protected       BuildException(System.Runtime.Serialization.SerializationInfo serializationInfo, System.Runtime.Serialization.StreamingContext streamingContext): base(serializationInfo, streamingContext)
        {
        }
    }

    [Serializable]
    public class StatusFileException: Exception
    {
        public          StatusFileException(): base()
        {
        }
        public          StatusFileException(string message): base(message)
        {
        }
        public          StatusFileException(string message, Exception innerException): base(message, innerException)
        {
        }
        protected       StatusFileException(System.Runtime.Serialization.SerializationInfo serializationInfo, System.Runtime.Serialization.StreamingContext streamingContext) : base(serializationInfo, streamingContext)
        {
        }
    }
}
