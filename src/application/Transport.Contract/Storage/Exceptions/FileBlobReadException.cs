using System;

namespace Super.Paula.Application.Storage.Exceptions
{
    public class FileBlobReadException : Exception
    {
        public FileBlobReadException(FormattableString message)
            : base(message.ToString())
        {

        }

        public FileBlobReadException(FormattableString message, Exception innerException)
            : base(message.ToString(), innerException)
        {

        }
    }
}
