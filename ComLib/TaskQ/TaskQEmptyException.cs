namespace ComLib.TaskQ
{
    internal class TaskQEmptyException : System.Exception
    {
        private readonly string _message="TaskQ is empty.";

        public override string Message
        {
            get { return _message; }
        }

        public TaskQEmptyException()
        {}

        public TaskQEmptyException(string message)
        {
            _message = message;
        }
    }
}
