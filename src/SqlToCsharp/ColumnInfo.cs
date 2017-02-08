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
            var (tn, isRequired, isRowversion) = GetType(c.DataType, c.Constraints);
            TypeName = tn;
            if (isRequired) _attributes.Add("Required");
            if (isRowversion) _attributes.Add("Timestamp");
            KeyColumnOrder = FindPrimaryKeyConstraint(c, tableConstraints);
        }

        private static int? FindPrimaryKeyConstraint(ColumnDefinition column, IList<ConstraintDefinition> tableConstraints)
        {
            if (column.Constraints.Any(c => (c as UniqueConstraintDefinition)?.IsPrimaryKey ?? false))
                return 0;

            var cons =
                from uc in tableConstraints.OfType<UniqueConstraintDefinition>()
                where uc.IsPrimaryKey
                from col in uc.Columns.Indexed()
                where col.item.Column.MultiPartIdentifier.Identifiers.Last().Value == column.ColumnIdentifier.Value
                select (int?)col.index;

            return cons.FirstOrDefault();
        }

        private static (string typeName, bool isRequired, bool isRowversion) GetType(DataTypeReference t, IEnumerable<ConstraintDefinition> constraints)
        {
            var name = t.Name.Identifiers.Last().Value;

            if (IsNullable(constraints))
            {
                switch (name)
                {
                    case "INT": return ("int?", false, false);
                    case "BIGINT": return ("long?", false, false);
                    case "SMALLINT": return ("short?", false, false);
                    case "TINYINT": return ("byte?", false, false);
                    case "BIT": return ("bool?", false, false);
                    case "NVARCHAR":
                    case "VARCHAR": return ("string", false, false);
                    case "DATETIMEOFFSET": return ("DateTimeOffset?", false, false);
                    case "DATETIME2": return ("DateTime?", false, false);
                    case "timestamp":
                    case "ROWVERSION": return ("byte[]", false, true);
                    default: throw new NotSupportedException(name);
                }
            }
            else
            {
                switch (name)
                {
                    case "INT": return ("int", false, false);
                    case "BIGINT": return ("long", false, false);
                    case "SMALLINT": return ("short", false, false);
                    case "TINYINT": return ("byte", false, false);
                    case "BIT": return ("bool", false, false);
                    case "NVARCHAR":
                    case "VARCHAR": return ("string", true, false);
                    case "DATETIMEOFFSET": return ("DateTimeOffset", false, false);
                    case "DATETIME2": return ("DateTime", false, false);
                    case "ROWVERSION": return ("byte[]", false, true);
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
