
using System.Net.Sockets;
using System.Net;
using System.Text;

TcpListener listener = new TcpListener(IPAddress.Any, 8080);
listener.Start();
Console.WriteLine("Server started on port 8080...");

while (true)
{
    TcpClient client = listener.AcceptTcpClient();
    NetworkStream stream = client.GetStream();

    byte[] buffer = new byte[1024];
    int bytesRead = stream.Read(buffer, 0, buffer.Length);

    string request = Encoding.UTF8.GetString(buffer, 0, bytesRead);
    Console.WriteLine(request);

    string response = "";

    if (request.Contains("GET /hello"))
    {
        response = "HTTP/1.1 200 OK\r\nContent-Type: text/plain\r\n\r\nHello, World!";
    }
    else if (request.Contains("GET /time"))
    {
        response = $"HTTP/1.1 200 OK\r\nContent-Type: text/plain\r\n\r\n{DateTime.Now}";
    }
    
    byte[] responseBytes = Encoding.UTF8.GetBytes(response);
    stream.Write(responseBytes, 0, responseBytes.Length);

    client.Close();
}