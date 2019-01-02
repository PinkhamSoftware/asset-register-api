namespace WebApi.Extensions
{
    public class ResponseData<T>
    {
        public T Data { get; }

        public ResponseData(T data)
        {
            Data = data;
        }
    }
}