using System.IO;
using draft;
using Microsoft.AspNetCore.Hosting;
using System.Text.Json;


public class ResultLogger
{
    private readonly string resultDirectory;

    public ResultLogger(IWebHostEnvironment hostingEnvironment)
    {
        // Встановлюємо шлях до папки "Results" у базовій директорії додатка
        resultDirectory = Path.Combine(hostingEnvironment.ContentRootPath, "Results");

        // Переконатися, що папка "Results" існує
        if (!Directory.Exists(resultDirectory))
        {
            Directory.CreateDirectory(resultDirectory);
        }
    }

    public void LogResults(string queryName, IEnumerable<object> results)
    {
        string fileName = $"{queryName}_results.txt";
        string filePath = Path.Combine(resultDirectory, fileName);

        using (StreamWriter writer = new StreamWriter(filePath))
        {
            writer.WriteLine($"Results for Query: {queryName}");
            writer.WriteLine("--------------------------------");

            foreach (var result in results)
            {
                string serializedResult = JsonSerializer.Serialize(result);
                writer.WriteLine(serializedResult);
            }
        }

        Console.WriteLine($"Файл {fileName} успішно збережено в папці {resultDirectory}");
    }

}
