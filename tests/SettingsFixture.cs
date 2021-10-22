using System.IO;
using System.Text.Json;

namespace TestProject
{
    public class ConnectionString
    {
        public string ConnString { get; set; }
    }
    // singleton pattern will return the connection string once 
    // across any test run, can be called from multiple places and is thread safe
    // https://csharpindepth.com/Articles/Singleton
    public sealed class SettingsFile
    {
        private static SettingsFile instance = null;
        private static readonly object padlock = new object();

        public string ConnString = null;
        private SettingsFile()
        {
            ConnectionString Conn = JsonSerializer.Deserialize<ConnectionString>(File.ReadAllText("settings.json"));
            ConnString = Conn.ConnString;
        }
        public static SettingsFile Instance
        {
            get
            {
                // C# Lock keyword ensures that one thread is executing a piece of code at one time.
                // The lock keyword ensures that one thread does not enter a critical section of code while another thread is in that critical section.

                lock (padlock)
                {
                    if (instance == null)
                    {
                        instance = new SettingsFile();
                    }
                    return instance;
                }
            }
        }
    }
}
