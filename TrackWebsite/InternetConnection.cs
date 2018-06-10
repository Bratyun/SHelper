using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace StateInternetConnection
{
    public class InternetConnection
    {
        /// <summary>
        /// Состояние соединения
        /// </summary>
        [Flags]
        public enum InternetConnectionState : int
        {
            INTERNET_CONNECTION_MODEM = 0x1,
            INTERNET_CONNECTION_LAN = 0x2,
            INTERNET_CONNECTION_PROXY = 0x4,
            INTERNET_RAS_INSTALLED = 0x10,
            INTERNET_CONNECTION_OFFLINE = 0x20,
            INTERNET_CONNECTION_CONFIGURED = 0x40
        }

        private bool isInternetConnected;
        private bool isUsingModem;
        private bool isOffline;
        private bool isUsingLAN;
        private bool isUsingProxy;
        private bool isRasEnabled;

        #region Свойства
        public bool IsInternetConnected
        {
            get { return isInternetConnected; }
        }

        public bool IsUsingModem
        {
            get { return isUsingModem; }
        }

        public bool IsOffline
        {
            get { return isOffline; }
        }

        public bool IsUsingLAN
        {
            get { return isUsingLAN; }
        }

        public bool IsUsingProxy
        {
            get { return isUsingProxy; }
        }

        public bool IsRasEnabled
        {
            get { return isRasEnabled; }
        }
        #endregion

        [DllImport("WININET", CharSet = CharSet.Auto)]
        static extern bool InternetGetConnectedState(ref InternetConnectionState lpdwFlags, int dwReserved);

        public void Init()
        {
            InternetConnectionState flags = 0;
            isInternetConnected = InternetGetConnectedState(ref flags, 0);
            isUsingModem = (flags & InternetConnectionState.INTERNET_CONNECTION_MODEM) != 0;
            isUsingLAN = (flags & InternetConnectionState.INTERNET_CONNECTION_LAN) != 0;
            isOffline = (flags & InternetConnectionState.INTERNET_CONNECTION_OFFLINE) != 0;
            isUsingProxy = (flags & InternetConnectionState.INTERNET_CONNECTION_PROXY) != 0;
            isRasEnabled = (flags & InternetConnectionState.INTERNET_RAS_INSTALLED) != 0;
        }
    }
}
