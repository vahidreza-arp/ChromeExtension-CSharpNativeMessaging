using System;
using System.Threading;
using ExtensionApi;
using Logger;

namespace ChromeExtensionNativeMessagingHost
{
    internal class Program
    {
        static Extension extension;

        static void Main(string[] args)
        {
            // TODO: There must be pipe to which the applications and services connect and receive the extension messages
            // TODO: There must be a gRPC server to which all applications can connec to and receieve messages from extension
            // TODO: Create the installer that installes the messaging host then other apps can connect to its pipe

            extension = new Extension();
            extension.OnMessageReceived += OnMessageFromExtension;
            extension.OnListeningException += OnExtensionListeningException;
            extension.StartListening();

            while (true)
            {
                Thread.Sleep(1000);
            }    
        }

        static void OnMessageFromExtension(object sender, ExtensionMessageEventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(e.Message))
                {
                    Log.Info($"Message received from extension: {e.Message}");
                    if (e.Message.ToLower() == "ping")
                    {
                        extension.Write("ping");
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error($"Failure in processing the extension message. Error: {ex.Message}");
            }
        }

        static void OnExtensionListeningException(object sender, ExtensionListeningExceptionEventArgs e)
        {
            throw new Exception($"Extension listening failed. Error: {e.Message}");
        }
    }
}
