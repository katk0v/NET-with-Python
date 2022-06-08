using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class TestController : ControllerBase
{
    private readonly ILogger<TestController> _logger;
    
    // Some Variables
    private const string PythonDirectory = @"C:\Python27\python.exe";
    private const string PythonSourceScript = @"C:\Work\repos\NET-with-Python\project_snake\WebApi\PythonScripts\python_script.py";
    private const string Arg1 = "2012-1-1";
    private const string Arg2 = "2019-1-22";

    public TestController(ILogger<TestController> logger)
    {
        _logger = logger;
    }

    [HttpPost(Name = "RunPythonScript")]
    public string Post()
    {
        // 1) Create Process Info
        var psi = new ProcessStartInfo();

        // 2) Provide script and arguments
        var args = new List<string>
        {
            PythonSourceScript, // script
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