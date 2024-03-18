using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

class Server
{
    static TcpListener listener;
    static Socket client;
    static Socket client2;
    static void Main(string[] args)
    {
        Console.WriteLine("Server started...");
        // Start listening on port
        listener = new TcpListener(IPAddress.Any, 888);
        listener.Start();
        // Accept client connections
        client = listener.AcceptSocket();
        client2 = listener.AcceptSocket();
        // Start a new thread for receiving messages from the client
        Thread receiverThread = new Thread(ReceiveMessage1);
        Thread receiverThread2 = new Thread(ReceiveMessage2);
        receiverThread.Start();
        receiverThread2.Start();
        // Loop to send messages to the client
        string message;
        while (true)
        {
            message = Console.ReadLine();
            SendMessage("Server: "+message, client);
            SendMessage("Server: "+message, client2);
        }
    }

   static void ReceiveMessage1()
    {
        byte[] buffer = new byte[1024];
        int bytesRead;
        while (true)
        {
            bytesRead = client.Receive(buffer);
            Console.WriteLine(Encoding.ASCII.GetString(buffer, 0, bytesRead));
            SendMessage(Encoding.ASCII.GetString(buffer, 0, bytesRead), client2);
        }
    }
    static void ReceiveMessage2()
    {
        byte[] buffer = new byte[1024];
        int bytesRead;
        while (true)
        {
            bytesRead = client2.Receive(buffer);
            Console.WriteLine(Encoding.ASCII.GetString(buffer, 0, bytesRead));
            SendMessage(Encoding.ASCII.GetString(buffer, 0, bytesRead), client);
        }
    }
    static void SendMessage(string message, Socket client)
    {
        byte[] buffer = Encoding.ASCII.GetBytes(message);
        client.Send(buffer);
    }
}
