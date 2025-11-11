namespace BuildingBlocks.Exceptions
{
    public class ActiveItemsException : Exception
    {
        public ActiveItemsException(string message) : base(message)
        {
        }

        public ActiveItemsException(string message, string details) : base(message)
        {
            Details = details;
        }

        public string? Details { get; }
    }
}
