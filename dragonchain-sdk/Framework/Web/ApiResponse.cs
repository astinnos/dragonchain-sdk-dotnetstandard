namespace dragonchain_sdk.Framework.Web
{
    public class ApiResponse<T>
    {
        /// <summary>
        /// Boolean result passed from the fetch library.
        /// This can be used to quickly determine if the status code is 2xx.
        /// </summary>
        public bool Ok { get; set; }

        /// <summary>
        /// HTTP Status Code        
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// Responses from Dragonchain will return here.
        /// Check the docs for the specific function you are calling to see what will appear here.        
        /// </summary>
        public T Response { get; set; }
    }    
}