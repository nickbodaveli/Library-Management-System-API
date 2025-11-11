namespace BuildingBlocks.Exceptions
{
    public class NotActiveException : Exception
    {
        public NotActiveException(string message) : base(message)
        {
        }

        public NotActiveException(string name, object key) : base($"Entity \"{name}\" ({key}) is not Active.")
        {
        }
    }
}
