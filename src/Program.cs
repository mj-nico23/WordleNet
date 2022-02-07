using System.Text.Json;

namespace WordleNet;

public class Program
{
    private static readonly string[] words;
    private static readonly string answer;
    private static int chances = 5;
    
    static Program()
    {
        words = LoadValidWords();
        answer = GetCorrectAnswer();
    }
    
    static int Main()
    {
        Console.WriteLine("Welcome to WordleNet");
        Console.WriteLine("Guess the 5 letters word");
        
        while (true)
        {
            if (chances == 0)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("No more chances left. You lost!!!");
                Console.ResetColor();
                Console.WriteLine($"Answer: {answer}");
                return 0;
            }
            
            var guess = (Console.ReadLine() ?? "").ToLower();

            if (string.IsNullOrEmpty(guess) || !words.Contains(guess))
            {
                Console.WriteLine("Word doesn't exists. Guess again.");
                continue;
            }

            if (CheckAnswer(guess))
            {
                return 0;
            }
            
            chances--;
        }
    }

    private static string[] LoadValidWords()
    {
        var jsonUtf8Bytes = File.ReadAllBytes("words.json");

        var readOnlySpan = new ReadOnlySpan<byte>(jsonUtf8Bytes);
        return JsonSerializer.Deserialize<string[]>(readOnlySpan)!;
    }

    private static string GetCorrectAnswer()
    {
        var random = new Random();
        return words[random.Next(words.Length - 1)];
    }

    private static bool CheckAnswer(string guess)
    {
        Console.SetCursorPosition(0, Console.CursorTop - 1);
        
        if (guess == answer)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(answer);
            Console.WriteLine();
            Console.WriteLine("Correct. You won!!!");
            return true;
        }

        WriteGuess(guess);
        //Console.WriteLine($"Incorrect. Guess again. {chances} left.");
        Console.ResetColor();
        return false;
    }
    
    private static void WriteGuess(string guess)
    {
        for (var i = 0; i < 5; i++)
        {
            if (guess[i] == answer[i]) WriteChar(guess[i], ConsoleColor.Green);
            else if (answer.Contains(guess[i])) WriteChar(guess[i], ConsoleColor.Yellow);
            else WriteChar(guess[i], ConsoleColor.Red);
        }

        Console.WriteLine();
    }

    private static void WriteChar(char c, ConsoleColor color)
    {
        Console.ForegroundColor = color;
        Console.Write(c);
        Console.ResetColor();
    }
}