
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
    // Extract the first line of the request
    string requestLine = request.Split("\r\n")[0];

    // Initialize the response
    string response = "";

    if (!string.IsNullOrEmpty(requestLine))
    {
        // Split the request line
        string[] parts = requestLine.Split(' ');

        // Validate the split result
        if (parts.Length >= 2)
        {
            string method = parts[0];
            string path = parts[1];

            // Match the exact path
            if (method == "GET" && path == "/")
            {
                response = "HTTP/1.1 200 OK\r\nContent-Type: text/plain\r\n\r\nWelcome";
            }
            else if (method == "GET" && path == "/hello")
            {
                response = "HTTP/1.1 200 OK\r\nContent-Type: text/plain\r\n\r\nHello, World!";
            }
            else if (method == "GET" && path == "/time")
            {
                response = $"HTTP/1.1 200 OK\r\nContent-Type: text/plain\r\n\r\n{DateTime.Now}";
            }
            else if (method == "GET" && path == "/html")
            {
                response = "HTTP/1.1 200 OK\r\nContent-Type: text/html\r\n\r\n<!DOCTYPE html><html><head><title>HTML Page</title></head><body><h1>Hello, HTML!</h1></body></html>";
            }
            else
            {
                response = "HTTP/1.1 404 Not Found\r\nContent-Type: text/plain\r\n\r\n404 Not Found";
            }
        }
        else
        {
            // Handle malformed requests
            response = "HTTP/1.1 400 Bad Request\r\nContent-Type: text/plain\r\n\r\n400 Bad Request";
        }
    }
    else
    {
        // Handle empty requests
        response = "HTTP/1.1 400 Bad Request\r\nContent-Type: text/plain\r\n\r\n400 Bad Request";
    }



    byte[] responseBytes = Encoding.UTF8.GetBytes(response);
    stream.Write(responseBytes, 0, responseBytes.Length);

    client.Close();
}