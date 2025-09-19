namespace ECommerceBackend.CommonApi
{
    public class CommonResponse<T>
    {
        public int StatusCode { get; set; }
        public string? Message { get; set; }
        public T  Data { get; set; }
        public CommonResponse(int Status,string msg,T data)
        {
            StatusCode = Status;
            Message = msg;
            Data = data;
        }
    }
}
