using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class TestController : ControllerBase
{
    private readonly ILogger<TestController> _logger;
    
    // Some Variables
   // private const string PythonDirectory = @"C:\Python27\python.exe"; \\ python v2.7
    //private const string PythonDirectory = @"C:\Windows\py.exe"; // python v3.10
    private const string PythonDirectory = "python3"; // python v3.10
    private const string Arg1 = "2012-1-1";
    private const string Arg2 = "2019-1-22";
    
    //private readonly string _pythonSourceScript = Environment.CurrentDirectory + @"\PythonScripts\python_script.py";
    private readonly string _pythonSourceScript = Environment.CurrentDirectory + @"/PythonScripts/python_script.py";

    public TestController(ILogger<TestController> logger)
    {
        _logger = logger;
    }

    [HttpGet("/api/get")]
    public string Get()
    {
        _logger.LogInformation("call get method");
        return "Hello!!!!!";
    }

    [HttpPost("/api/post")]
    public string Post()
    {
        // 1) Create Process Info
        var psi = new ProcessStartInfo();

        // 2) Provide script and arguments
        var args = new List<string>
        {
            _pythonSourceScript, // script
            Arg1, // arg1
            Arg2 // arg2
        };

        foreach (var arg in args)
        {
            psi.ArgumentList.Add(arg);
        }
        
        // 3) Process configuration
        psi.FileName = PythonDirectory;
        psi.UseShellExecute = false;
        psi.CreateNoWindow = true;
        psi.RedirectStandardOutput = true;
        psi.RedirectStandardError = true;

        // 4) Execute process and get output
        var errors = "";
        var results = "";

        using(var process = Process.Start(psi))
        {
            errors = process.StandardError.ReadToEnd();
            results = process.StandardOutput.ReadToEnd();
        }

        // 5) Display output
        Console.WriteLine("ERRORS:");
        Console.WriteLine(errors);
        Console.WriteLine();
        Console.WriteLine("Results:");
        Console.WriteLine(results);
        
        return errors.Length > 0 ? errors : results;
    }
}