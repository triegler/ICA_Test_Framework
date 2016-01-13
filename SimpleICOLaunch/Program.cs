using System;
using System.Threading;
using WFICALib;

namespace SimpleICOLaunch
{
    /// <summary>
    /// This program demo's the basics of launching an ICO session from within an application.
    /// </summary>
    class Program
    {
        public static System.Threading.AutoResetEvent onLogonResetEvent = null;

        static void Main(string[] args)
        {
            ICAClientClass ica = new ICAClientClass();
            onLogonResetEvent = new AutoResetEvent(false);

            // Launch published Notepad if you comment this line, and uncommented
            // the one above you will launch a desktop session
            ica.Application = "#Standard Desktop XA76";
            // ica.InitialProgram = "#Notepad";

            // Launch a new session
            ica.Launch = true;

            // Set Server address
            ica.Address = "172.16.11.40";

            // No Password property is exposed (for security)
            // but can explicitly specify it by using the ICA File "password" property
            ica.Username = "tr";
            ica.Domain = "cherryhealth";
            ica.SetProp("Password", "Password78");

            // Let's have a "pretty" session
            ica.DesiredColor = ICAColorDepth.Color24Bit;

            // Reseach the output mode you want, depending on what your trying
            // to attempt to automate. The "Client Simulation APIs" are only available under certain modes
            // (i.e. things like sending keys to the session, enumerating windows, etc.)
            ica.OutputMode = OutputMode.OutputModeNormal;

            // Height and Width
            ica.DesiredHRes = 1024;
            ica.DesiredVRes = 786;

            // Register for the OnLogon event
            ica.OnLogon += new _IICAClientEvents_OnLogonEventHandler(ica_OnLogon);

            // Launch/Connect to the session
            ica.Connect();

            if (onLogonResetEvent.WaitOne(new TimeSpan(0, 2, 0)))
                Console.WriteLine("Session Logged on sucessfully! And OnLogon Event was Fired!");
            else
                Console.WriteLine("OnLogon event was NOT Fired! Logging off ...");

            // Do we have access to the client simulation APIs?
            if (ica.Session == null)
                Console.WriteLine("ICA.Session object is NULL! :(");

            Console.WriteLine("\nPress any key to log off");
            Console.Read();

            // Logoff our session
            Console.WriteLine("Logging off Session");
            ica.Logoff();
        }

        /// <summary>
        /// OnLogon event handler
        /// </summary>
        static void ica_OnLogon()
        {
            Console.WriteLine("OnLogon event was Handled!");
            onLogonResetEvent.Set();
        }
    }
}
