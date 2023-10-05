namespace XapauServer.Helpers
{
    public class CustomResponse<T>
    {
        public bool status { get; set; }
        public string message { get; set; } = string.Empty;
        public T? data { get; set; }
    }
}