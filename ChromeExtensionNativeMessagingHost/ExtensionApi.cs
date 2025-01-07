using System;
using System.Text;
using System.Threading.Tasks;

namespace ExtensionApi
{
    public class Extension
    {
        public EventHandler<ExtensionMessageEventArgs> OnMessageReceived;
        public EventHandler<ExtensionListeningExceptionEventArgs> OnListeningException;

        public void StartListening()
        {
            Task.Run(() =>
            {
                while (true)
                {
                    try
                    {
                        var message = Read();
                        if (message != null)
                        {
                            OnMessageReceived?.Invoke(null, new ExtensionMessageEventArgs(message));
                        }
                    }
                    catch (Exception ex)
                    {
                        OnListeningException?.Invoke(this, new ExtensionListeningExceptionEventArgs(ex.Message));
                        return;
                    }

                    Task.Delay(1000).Wait();
                }
            });
        }

        public string Read()
        {
            byte[] lengthBytes = new byte[4];
            Console.OpenStandardInput().Read(lengthBytes, 0, 4);
            int length = BitConverter.ToInt32(lengthBytes, 0);
            byte[] messageBytes = new byte[length];
            Console.OpenStandardInput().Read(messageBytes, 0, length);

            return Encoding.UTF8.GetString(messageBytes);
        }

        public void Write(string message)
        {
            var bytes = Encoding.UTF8.GetBytes(message);

            Console.OpenStandardOutput().Write(BitConverter.GetBytes(bytes.Length), 0, 4);
            Console.OpenStandardOutput().Write(bytes, 0, bytes.Length);
            Console.OpenStandardOutput().Flush();
        }
    }

    public class ExtensionMessageEventArgs : EventArgs
    {
        public string Message { get; }

        public ExtensionMessageEventArgs(string message)
        {
            Message = message;
        }
    }

    public class ExtensionListeningExceptionEventArgs : EventArgs
    {
        public string Message { get; }

        public ExtensionListeningExceptionEventArgs(string message)
        {
            Message = message;
        }
    }
}
