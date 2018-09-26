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
        public IEnumerable<string> Attributes { get; }
        public int? KeyColumnOrder { get; }

        private List<string> _attributes = new List<string>();

        public ColumnInfo(ColumnDefinition c, IList<ConstraintDefinition> tableConstraints)
        {
            Name = c.ColumnIdentifier.Value;
            (TypeName, Attributes) = GetType(c.DataType, c.Constraints);
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

        private const string Required = "Required";
        private const string Timestamp = "Timestamp";
        private static string[] None => Array.Empty<string>();
        private static string ArrayLength(DataTypeReference t)
        {
            if (!(t is ParameterizedDataTypeReference p)) return null;
            return $"MaxLength({p.Parameters[0].Value})";
        }

        private static (string typeName, string[] attributes) GetType(DataTypeReference t, IEnumerable<ConstraintDefinition> constraints)
        {
            var name = t.Name.Identifiers.Last().Value;

            if (IsNullable(constraints))
            {
                switch (name)
                {
                    case "INT": return ("int?", None);
                    case "BIGINT": return ("long?", None);
                    case "SMALLINT": return ("short?", None);
                    case "TINYINT": return ("byte?", None);
                    case "BIT": return ("bool?", None);
                    case "NVARCHAR":
                    case "VARCHAR": return ("string", None);
                    case "DATETIMEOFFSET": return ("DateTimeOffset?", None);
                    case "DATETIME2": return ("DateTime?", None);
                    case "timestamp":
                    case "ROWVERSION": return ("byte[]", new[] { Timestamp });
                    case "BINARY":
                    case "VARBINARY": return ("byte[]", new[] { ArrayLength(t) });
                    default: throw new NotSupportedException(name);
                }
            }
            else
            {
                switch (name)
                {
                    case "INT": return ("int", None);
                    case "BIGINT": return ("long", None);
                    case "SMALLINT": return ("short", None);
                    case "TINYINT": return ("byte", None);
                    case "BIT": return ("bool", None);
                    case "NVARCHAR":
                    case "VARCHAR": return ("string", new[] { Required });
                    case "DATETIMEOFFSET": return ("DateTimeOffset", None);
                    case "DATETIME2": return ("DateTime", None);
                    case "ROWVERSION": return ("byte[]", new[] { Timestamp });
                    case "BINARY":
                    case "VARBINARY": return ("byte[]", new[] { Required, ArrayLength(t) });
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
