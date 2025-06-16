using System;
using System.IO;
using Python.Runtime;
using UnityEngine;

public class PythonExecutionResult
{
    public string Output { get; set; } = "";
    public string Error { get; set; } = "";
    public bool Success => string.IsNullOrEmpty(Error);
}

public class PythonExecutor : MonoBehaviour
{
    private bool isInitialized = false;

    public void Initialize()
    {
        if (isInitialized)
            return;

        try
        {
            var pythonDll = Path.Combine(
                Application.streamingAssetsPath,
                "Python",
                "python313.dll"
            );

            if (!File.Exists(pythonDll))
            {
                Debug.LogError(
                    $"Python DLL не найдена по пути: {pythonDll}. Проверьте папку StreamingAssets и имя файла."
                );
                return;
            }

            Runtime.PythonDLL = pythonDll;

            PythonEngine.Initialize();
            isInitialized = true;
            Debug.Log("Python Engine Initialized Successfully!");
        }
        catch (Exception e)
        {
            Debug.LogError(
                "Failed to initialize Python Engine: " + e.Message + "\n" + e.StackTrace
            );
        }
    }

    public PythonExecutionResult Execute(string code)
    {
        if (!isInitialized)
        {
            Initialize();
            if (!isInitialized)
            {
                return new PythonExecutionResult { Error = "Python Engine не инициализирован." };
            }
        }

        var result = new PythonExecutionResult();

        using (Py.GIL())
        {
            using (var scope = Py.CreateScope())
            {
                var stringWriter = new StringWriter();
                var outputRedirector = new PyOutputStream(stringWriter);

                scope.Set("stdout_redirector", outputRedirector.ToPython());

                try
                {
                    scope.Exec(
                        @"
import sys
sys.stdout = stdout_redirector
sys.stderr = stdout_redirector
"
                    );
                    scope.Exec(code);
                    result.Output = stringWriter.ToString();
                }
                catch (PythonException ex)
                {
                    result.Error = ex.Message + "\n" + stringWriter.ToString();
                }
                catch (Exception ex)
                {
                    result.Error = "Критическая ошибка выполнения: " + ex.Message;
                }
            }
        }
        return result;
    }

    void OnApplicationQuit()
    {
        if (isInitialized)
        {
            PythonEngine.Shutdown();
            Debug.Log("Python Engine Shutdown.");
        }
    }
}

[PyExport(true)]
public class PyOutputStream
{
    private readonly StringWriter writer;

    public PyOutputStream(StringWriter writer)
    {
        this.writer = writer;
    }

    public void write(string message)
    {
        writer.Write(message);
    }

    public void flush() { }
}
