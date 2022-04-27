namespace NUnit.Runner.Messages {
    public class ErrorMessage {
        public const string Name = nameof(ErrorMessage);

        public ErrorMessage(string message) {
            Message = message;
        }

        public string Message { get; set; }
    }
}
