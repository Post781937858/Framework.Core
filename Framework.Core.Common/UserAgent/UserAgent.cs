namespace Framework.Core.Common
{
    /// <summary>
    ///
    /// </summary>
    public class UserAgent
    {
        private readonly string _userAgent;

        private ClientBrowser _browser;

        /// <summary>
        ///
        /// </summary>
        public ClientBrowser Browser
        {
            get
            {
                if (_browser == null)
                {
                    _browser = new ClientBrowser(_userAgent);
                }
                return _browser;
            }
        }

        private ClientOS _os;

        /// <summary>
        ///
        /// </summary>
        public ClientOS OS
        {
            get
            {
                if (_os == null)
                {
                    _os = new ClientOS(_userAgent);
                }
                return _os;
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="userAgent"></param>
        public UserAgent(string userAgent)
        {
            _userAgent = userAgent;
        }
    }
}