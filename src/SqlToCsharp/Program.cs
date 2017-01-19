using System;
using System.Linq;

namespace SqlToCsharp
{
    class Program
    {
        static void Main(string[] args)
        {
            if(args.Length < 3)
            {
                Console.WriteLine(@"[Usage]
SqlToCsharp [sql path] [c# path] [namespace]

sql path : A folder path for source SQL files
C# path  : A folder path for target C# files

[Description]
Generates C# classes from SQL files.

Source files   : SQL files that contain CREATE TABLE statements.
                 primarily used for the SSDT Database Project.

Generated files: C# classes whose properties correspond to the SQL table columns.
                 used for O/R mappers such as Entity Framework and Dapper.
");
                return;
            }

            var sourcePath = args[0];
            var targetPath = args[1];
            var ns = args[2];

            Console.WriteLine($@"generate C# classes from CREATE TABLE statements
source: {sourcePath}
target: {targetPath}
namespace: {ns}
");

            var tables = Parser.FromFolder(sourcePath).ToArray();

            Generator.Save(ns, tables, targetPath);
        }
    }
}
