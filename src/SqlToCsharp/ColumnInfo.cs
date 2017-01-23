using Microsoft.SqlServer.TransactSql.ScriptDom;
using System;
using System.Linq;
using System.Collections.Generic;

namespace SqlToCsharp
{
    class ColumnInfo
    {
        public string Name { get; }
        public string TypeName { get; }
        public IEnumerable<string> Attributes => _attributes;
        public int? KeyColumnOrder { get; }

        private List<string> _attributes = new List<string>();

        public ColumnInfo(ColumnDefinition c, IList<ConstraintDefinition> tableConstraints)
        {
            Name = c.ColumnIdentifier.Value;
            var (tn, isRequired) = GetType(c.DataType, c.Constraints);
            TypeName = tn;
            if (isRequired) _attributes.Add("Required");
            KeyColumnOrder = FindPrimaryKeyConstraint(c, tableConstraints);
        }

        private static int? FindPrimaryKeyConstraint(ColumnDefinition column, IList<ConstraintDefinition> tableConstraints)
        {
            var cons =
                from UniqueConstraintDefinition uc in tableConstraints
                from col in uc.Columns.Indexed()
                where col.item.Column.MultiPartIdentifier.Identifiers.Last().Value == column.ColumnIdentifier.Value
                select (int?)col.index;

            return cons.FirstOrDefault();
        }

        private static (string typeName, bool isRequired) GetType(DataTypeReference t, IEnumerable<ConstraintDefinition> constraints)
        {
            var name = t.Name.Identifiers.Last().Value;

            if (IsNullable(constraints))
            {
                switch (name)
                {
                    case "INT": return ("int?", false);
                    case "BIGINT": return ("long?", false);
                    case "SMALLINT": return ("short?", false);
                    case "TINYINT": return ("byte?", false);
                    case "BIT": return ("bool?", false);
                    case "NVARCHAR":
                    case "VARCHAR": return ("string", false);
                    case "DATETIMEOFFSET": return ("DateTimeOffset?", false);
                    case "DATETIME2": return ("DateTime?", false);
                    default: throw new NotSupportedException(name);
                }
            }
            else
            {
                switch (name)
                {
                    case "INT": return ("int", false);
                    case "BIGINT": return ("long", false);
                    case "SMALLINT": return ("short", false);
                    case "TINYINT": return ("byte", false);
                    case "BIT": return ("bool", false);
                    case "NVARCHAR":
                    case "VARCHAR": return ("string", true);
                    case "DATETIMEOFFSET": return ("DateTimeOffset", false);
                    case "DATETIME2": return ("DateTime", false);
                    default: throw new NotSupportedException(name);
                }
            }
        }

        private static bool IsNullable(IEnumerable<ConstraintDefinition> constraints)
        {
            var nc = constraints.OfType<NullableConstraintDefinition>().FirstOrDefault();
            return nc?.Nullable ?? false;
        }
    }
}
