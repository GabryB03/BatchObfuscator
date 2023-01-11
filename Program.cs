using System;
using System.Text;

public class Program
{
    private static string _alphaCharacters = "abcdefghijklmnopqrstuvwxyz";

    public static void Main()
    {
        ProtoRandom random = new ProtoRandom(5);
        Console.Title = "BatchObfuscator";
        string path = "";

        while (!System.IO.File.Exists(path) && !System.IO.Path.GetExtension(path).ToLower().Equals(".bat"))
        {
            Console.WriteLine("Please, insert the path of the Batch file to obfuscate: ");
            path = Console.ReadLine().Replace("\"", "").Replace("'", "").Replace('\t'.ToString(), "");

            if (!System.IO.File.Exists(path) && !System.IO.Path.GetExtension(path).ToLower().Equals(".bat"))
            {
                Console.WriteLine("The specified file does not exist or has an invalid extension.");
            }
        }

        string newPath = path.Substring(0, path.Length - 4) + "-obfuscated.bat";
        string codeToObfuscate = System.IO.File.ReadAllText(path);
        StringBuilder obfuscatedCode = new StringBuilder();
        obfuscatedCode.Append("cls\r\n@echo off\r\ncls\r\n");
        string[] alphaCharactersDefinitions = new string[27];

        for (int i = 0; i < 26; i++)
        {
            alphaCharactersDefinitions[i] = random.GetRandomString(_alphaCharacters, 6);
            obfuscatedCode.Append($"set {alphaCharactersDefinitions[i]}={_alphaCharacters[i]}\r\n");
        }

        string theStr = "";

        for (int i = 0; i < codeToObfuscate.Length; i++)
        {
            char theCharacter = codeToObfuscate[i];
            bool exists = false;

            for (int j = 0; j < _alphaCharacters.Length; j++)
            {
                if (theCharacter.Equals(_alphaCharacters[j]))
                {
                    exists = true;
                    theStr = alphaCharactersDefinitions[j];
                }
            }

            if (exists)
            {
                obfuscatedCode.Append($"%{theStr}%");
            }
            else
            {
                obfuscatedCode.Append(theCharacter);
            }
        }

        System.IO.File.WriteAllBytes(newPath, Combine(new byte[4] { 0xFF, 0xFE, 0x0D, 0x0A }, Encoding.UTF8.GetBytes(obfuscatedCode.ToString())));
        Console.WriteLine("Succesfully obfuscated your Batch file. Press ENTER to exit.");
        Console.ReadLine();
    }

    private static byte[] Combine(byte[] first, byte[] second)
    {
        byte[] ret = new byte[first.Length + second.Length];

        Buffer.BlockCopy(first, 0, ret, 0, first.Length);
        Buffer.BlockCopy(second, 0, ret, first.Length, second.Length);

        return ret;
    }
}