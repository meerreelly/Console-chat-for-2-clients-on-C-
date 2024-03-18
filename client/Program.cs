using System;
using System.Net.Sockets;
using System.Text;

class Client
{
    static TcpClient client;
    
    static void Main(string[] args)
    {
        Console.WriteLine("Client started...");
        // Connect to the server on localhost and port 8888
        client = new TcpClient("localhost", 888);
        // Start a new thread for receiving messages from the server
        System.Threading.Thread receiverThread = new System.Threading.Thread(ReceiveMessage);
        receiverThread.Start();
        Console.WriteLine("Enter your name:");
        string name = Console.ReadLine();
        // Loop to send messages to the server
        string message;
        while (true)
        {
            
            message = Console.ReadLine();
            SendMessage(name+": "+message);
        }
    }
    static void ReceiveMessage()
    {
        NetworkStream nwStream = client.GetStream();
        byte[] buffer = new byte[client.ReceiveBufferSize];
        while (true)
        {
            int bytesRead = nwStream.Read(buffer, 0, client.ReceiveBufferSize);
            Console.WriteLine(Encoding.ASCII.GetString(buffer, 0, bytesRead));
        }
    }

    static void SendMessage(string message)
    {
        NetworkStream nwStream = client.GetStream();
        byte[] buffer = Encoding.ASCII.GetBytes(message);
        nwStream.Write(buffer, 0, buffer.Length);
        nwStream.Flush();
    }
}
