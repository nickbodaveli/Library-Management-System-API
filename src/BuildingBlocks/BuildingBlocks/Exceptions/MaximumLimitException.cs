namespace BuildingBlocks.Exceptions
{
    public class MaximumLimitException : Exception
    {
        public MaximumLimitException(string message) : base(message)
        {
        }

        public MaximumLimitException(string message, string details) : base(message)
        {
            Details = details;
        }

        public string? Details { get; }
    }
}
