namespace BuildingBlocks.Exceptions
{
    public class AlreadyExistsException : Exception
    {
        public AlreadyExistsException(string message) : base(message)
        {
        }

        public AlreadyExistsException(string message, string details) : base(message)
        {
            Details = details;
        }

        public string? Details { get; }
    }
}
