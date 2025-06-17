using UnityEngine;

[System.Serializable]
public class TestCase
{
    [Tooltip(
        "Данные, которые будут поданы на стандартный ввод (input()). Каждая новая строка - отдельный вызов input()."
    )]
    [TextArea(2, 5)]
    public string InputData;

    [Tooltip("Ожидаемый результат в стандартном выводе (print()).")]
    [TextArea(2, 5)]
    public string ExpectedOutput;
}
