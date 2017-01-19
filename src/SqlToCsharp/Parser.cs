using System.Collections.Generic;
using System.Linq;
using Microsoft.SqlServer.TransactSql.ScriptDom;
using System.IO;

namespace SqlToCsharp
{
    class Parser
    {
        public static IEnumerable<TableInfo> FromFolder(string folderPath)
            => FromFolder(new TSql90Parser(false), folderPath);

        public static IEnumerable<TableInfo> FromFolder(TSqlParser parser, string folderPath)
        {
            foreach (var file in Directory.GetFiles(folderPath, "*.sql", SearchOption.AllDirectories))
            {
                foreach (var info in FromFile(parser, file))
                {
                    yield return info;
                }
            }
        }

        public static IEnumerable<TableInfo> FromFile(TSqlParser parser, string sqlFilePath)
        {
            using (var reader = new StreamReader(sqlFilePath))
            {
                IList<ParseError> errors;
                var result = parser.Parse(reader, out errors);
                var script = result as TSqlScript;

                foreach (var ts in script.Batches)
                    foreach (var st in ts.Statements.OfType<CreateTableStatement>())
                    {
                        yield return new TableInfo(st);
                    }
            }
        }
    }
}
