namespace ScrTilla
{
    class json_st
    {
        public class Response
        {
            public Response()
            {
                filename = message = "";
            }
            public string filename { get; set; }
            public int code { get; set; }
            public string message { get; set; }
        }
    }
}
