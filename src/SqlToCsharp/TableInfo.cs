using Microsoft.SqlServer.TransactSql.ScriptDom;
using System.Collections.Generic;
using System.Linq;

namespace SqlToCsharp
{
    class TableInfo
    {
        public string Name { get; }
        public IEnumerable<ColumnInfo> Columns { get; }

        public TableInfo(CreateTableStatement t)
        {
            Name = GetName(t.SchemaObjectName);
            Columns = t.Definition.ColumnDefinitions.Select(c => new ColumnInfo(c, t.Definition.TableConstraints)).ToArray();
        }

        private static string GetName(MultiPartIdentifier name)
        {
            var dboRemoved = name.Identifiers.Where(i => i.Value != "dbo");
            var joined = string.Join("", dboRemoved.Select(x => x.Value));
            var singularized = joined.Last() == 's' ? joined.Substring(0, joined.Length - 1) : joined;
            return singularized;
        }
    }
}
