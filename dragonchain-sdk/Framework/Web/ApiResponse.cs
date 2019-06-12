namespace dragonchain_sdk.Framework.Web
{
    /// <summary>
    /// Response returned from a `DragonchainClient` call to a dragonchain
    /// </summary>    
    public class ApiResponse<T>
    {
        /// <summary>
        /// Boolean result if the response from the dragonchain was a 2XX status code, indicating a successful call        
        /// </summary>
        public bool Ok { get; set; }

        /// <summary>
        /// HTTP status code returned from this call
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// Responses from Dragonchain will return here.
        /// Check the docs for the specific function you are calling to see what will appear here.        
        /// </summary>
        public T Response { get; set; }
    }    
}