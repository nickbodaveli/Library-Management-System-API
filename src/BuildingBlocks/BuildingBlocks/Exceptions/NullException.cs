namespace BuildingBlocks.Exceptions
{
    public class NullException : Exception
    {
        public NullException(string message) : base(message)
        {
        }

        public NullException(string message, string details) : base(message)
        {
            Details = details;
        }

        public string? Details { get; }
    }
}
