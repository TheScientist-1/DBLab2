using draft;
using System.Text.Json;

public class ResultLogger
{
    private readonly string resultDirectory;
    private readonly string combinedResultsFile;

    public ResultLogger()
    {
        // Встановлюємо шлях до папки "Results" у базовій директорії
        string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
        resultDirectory = Path.Combine(baseDirectory, "Results");

        // Перевіряємо наявність папки "Results"
        if (!Directory.Exists(resultDirectory))
        {
            throw new DirectoryNotFoundException("Папка 'Results' не знайдена.");
        }

        // Встановлюємо шлях до файлу, в який будуть записуватися комбіновані результати
        string combinedResultsFileName = "combined_results.json";
        combinedResultsFile = Path.Combine(resultDirectory, combinedResultsFileName);
    }

    public void LogResults(string queryName, IEnumerable<object> results)
    {
        // Збереження окремого JSON-файлу для кожного запиту
        string jsonFileName = $"{queryName}_results.json";
        string jsonFilePath = Path.Combine(resultDirectory, jsonFileName);

        string json = JsonSerializer.Serialize(results, new JsonSerializerOptions
        {
            WriteIndented = true
        });
        File.WriteAllText(jsonFilePath, json);

        // Збереження окремого текстового файлу для кожного запиту
        string textFileName = $"{queryName}_results.txt";
        string textFilePath = Path.Combine(resultDirectory, textFileName);

        using (StreamWriter writer = new StreamWriter(textFilePath))
        {
            writer.WriteLine($"Results for Query: {queryName}");
            writer.WriteLine("--------------------------------");

            foreach (var result in results)
            {
                if (result is Student student)
                {
                    string formattedStudent = $"Student ID: {student.Id}, First Name: {student.FirstName}, Last Name: {student.LastName}, Enrollment Date: {student.EnrollmentDate.ToShortDateString()}";
                    writer.WriteLine(formattedStudent);
                }
                else if (result is Studying studying)
                {
                    string formattedStudying = $"Studying ID: {studying.StudyingId}, Student ID: {studying.StudentId}, Course ID: {studying.CourseId}";
                    writer.WriteLine(formattedStudying);
                }
                else if (result is CourseAssignment courseAssignment)
                {
                    string formattedCourseAssignment = $"Course Assignment ID: {courseAssignment.Id}, Course ID: {courseAssignment.CourseId}, Teacher ID: {courseAssignment.TeacherId}";
                    writer.WriteLine(formattedCourseAssignment);
                }
                else if (result is Course course)
                {
                    string formattedCourse = $"Course ID: {course.CourseId}, Title: {course.Title}, Department ID: {course.DepartmentId}, Credits: {course.CreditsNumber}";
                    writer.WriteLine(formattedCourse);
                }
                else if (result is Department department)
                {
                    string formattedDepartment = $"Department ID: {department.DepartmentId}, Name: {department.Name}, Start Date: {department.StartDate.ToShortDateString()}";
                    writer.WriteLine(formattedDepartment);
                }
                else
                {
                    writer.WriteLine(result.ToString());
                }
            }
        }

        Console.WriteLine($"Файл {jsonFileName} успішно збережено в папці {resultDirectory}");
        Console.WriteLine($"Файл {textFileName} успішно збережено в папці {resultDirectory}");

        // Збереження комбінованого файлу з усіма результатами
        AppendResultsToCombinedFile(queryName, results);
    }

    private void AppendResultsToCombinedFile(string queryName, IEnumerable<object> results)
    {
        // Перевірка наявності комбінованого файлу
        if (!File.Exists(combinedResultsFile))
        {
            // Створення нового комбінованого файлу з першим результатом
            var combinedResults = new Dictionary<string, List<object>>
            {
                { queryName, results.ToList() }
            };

            string combinedJson = JsonSerializer.Serialize(combinedResults, new JsonSerializerOptions
            {
                WriteIndented = true
            });
            File.WriteAllText(combinedResultsFile, combinedJson);
        }
        else
        {
            // Додавання результатів до існуючого комбінованого файлу
            string existingJson = File.ReadAllText(combinedResultsFile);
            var existingResults = JsonSerializer.Deserialize<Dictionary<string, List<object>>>(existingJson);

            // Додавання нових результатів або оновлення існуючих результатів
            existingResults[queryName] = results.ToList();

            // Запис оновленого комбінованого файлу
            string updatedJson = JsonSerializer.Serialize(existingResults, new JsonSerializerOptions
            {
                WriteIndented = true
            });
            File.WriteAllText(combinedResultsFile, updatedJson);
        }

        Console.WriteLine($"Результати додано до комбінованого файлу {combinedResultsFile}");
    }
}
