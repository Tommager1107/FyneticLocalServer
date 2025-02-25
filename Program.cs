using System;
using System.IO;
using System.Net;
using System.Threading;

class Program
{
    static void Main(string[] args)
    {
        
        string version = "1.0.0";

        
        if (args.Length == 0)
        {
            
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine($"FCE version {version}");
            Console.ResetColor();
        }
       
        else if (args.Length >= 2 && args[0] == "server")
        {
            
            int port = 8080;
            if (args.Length == 3 && args[1] == "-p")
            {
                if (int.TryParse(args[2], out int specifiedPort))
                {
                    port = specifiedPort;
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.DarkBlue;
                    Console.WriteLine("Invalid port number. Using default port 8080.");
                    Console.ResetColor();
                }
            }

            
            StartWebServer(port);
        }
        else if (args.Length == 1 && args[0] == "setup")
        {
            
            RunSetup();
        }
        else
        {
           
            Console.ForegroundColor = ConsoleColor.DarkBlue;
            Console.WriteLine("Usage:");
            Console.WriteLine("fce          - Show FCE version");
            Console.WriteLine("fce server -p <port> - Start a simple web server");
            Console.WriteLine("fce setup    - Set up the web server directory");
            Console.ResetColor();
        }
    }

    static void RunSetup()
    {
        string currentDirectory = Directory.GetCurrentDirectory();

       
        if (Directory.GetFiles(currentDirectory).Length > 0 || Directory.GetDirectories(currentDirectory).Length > 0)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Error: The directory is not empty. Please run 'fce setup' in an empty folder.");
            Console.ResetColor();
            return;
        }

        
        string wwwPath = Path.Combine(currentDirectory, "www");

        string corePath = Path.Combine(currentDirectory, "core"); 

        string indexFilePath = Path.Combine(wwwPath, "index.html");
        string indexFileUrl = "https://raw.githubusercontent.com/Tommager1107/cdn-fynetic/refs/heads/main/local-server/index.html";

        string coreJsonUrl = "https://raw.githubusercontent.com/Tommager1107/cdn-fynetic/refs/heads/main/local-server/core/core.json";
        string coreJsonFilePath = Path.Combine(corePath, "core.json");

        try
        {
          
            Directory.CreateDirectory(wwwPath);

            Directory.CreateDirectory(corePath);

            using (WebClient client = new WebClient())
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("Downloading index.html...");
                client.DownloadFile(indexFileUrl, indexFilePath);
                Console.WriteLine("index.html download complete.");
                Console.ResetColor();
            }

            using (WebClient client = new WebClient())
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("Downloading core.json...");
                client.DownloadFile(coreJsonUrl, coreJsonFilePath);
                Console.WriteLine("core.json download complete.");
                Console.ResetColor();
            }

            Console.WriteLine("Setup complete! Web server files are ready.");
        }
        catch (Exception ex)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"Error during setup: {ex.Message}");
            Console.ResetColor();
        }
    }

  

    static void DrawProgressBar(int currentStep, int totalSteps)
    {
        double percentage = (double)currentStep / totalSteps * 100;
        int progress = (int)(percentage / 2);
        string bar = new string('#', progress) + new string('-', 50 - progress);

        Console.ForegroundColor = ConsoleColor.Blue;
        Console.Write("\r[" + bar + "] ");
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.Write(percentage.ToString("F2") + "%");
        Console.ResetColor();
    }

    static void StartWebServer(int port)
    {
        Console.Clear();

        Console.ForegroundColor = ConsoleColor.Blue;
        Console.WriteLine("--------------------------------------------------");
        Console.WriteLine("|               Fynetic Web Server               |");
        Console.WriteLine("|               Made by Tommager                 |");
        Console.WriteLine("|               Version 0.1                      |");
        Console.WriteLine("--------------------------------------------------");
        Console.ResetColor();

        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine($"| Your web server is running on                 |");
        Console.WriteLine($"| http://localhost:{port}                          |");
        Console.WriteLine();
        Console.WriteLine("| You can edit the page in www/index.html        |");
        Console.ResetColor();

        Console.WriteLine("--------------------------------------------------");
        Console.WriteLine();
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("╔════════════════════════════════════════════════╗");
        Console.WriteLine("║                IMPORTANT NOTE                  ║");
        Console.WriteLine("╚════════════════════════════════════════════════╝");
        Console.ResetColor();

        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("This is a development server intended for developers or small projects.");
        Console.WriteLine("It is not designed for large-scale or production-level use.");
        Console.WriteLine("Please do not use this server for high traffic or big projects.");
        Console.WriteLine();
        Console.ResetColor();






        string wwwDirectory = "www";
        if (!Directory.Exists(wwwDirectory))
        {
            Console.ForegroundColor = ConsoleColor.DarkBlue;
            Console.WriteLine("Error: The 'www' directory does not exist.");
            Console.ResetColor();
            return;
        }

        HttpListener listener = new HttpListener();
        listener.Prefixes.Add($"http://localhost:{port}/");
        listener.Start();
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.ResetColor();

        while (true)
        {
            HttpListenerContext context = listener.GetContext();
            HttpListenerRequest request = context.Request;
            HttpListenerResponse response = context.Response;

            string filePath = Path.Combine(wwwDirectory, "index.html");

            if (File.Exists(filePath))
            {
                string content = File.ReadAllText(filePath);
                byte[] buffer = System.Text.Encoding.UTF8.GetBytes(content);
                response.ContentType = "text/html";
                response.ContentLength64 = buffer.Length;
                response.StatusCode = (int)HttpStatusCode.OK;
                response.OutputStream.Write(buffer, 0, buffer.Length);
            }
            else
            {
                response.StatusCode = (int)HttpStatusCode.NotFound;
                byte[] errorMessage = System.Text.Encoding.UTF8.GetBytes("404 Not Found");
                response.OutputStream.Write(errorMessage, 0, errorMessage.Length);
            }

            response.OutputStream.Close();
        }
    }
}
