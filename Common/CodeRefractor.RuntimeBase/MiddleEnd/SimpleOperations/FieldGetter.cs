using CodeRefractor.RuntimeBase.MiddleEnd.SimpleOperations.Identifiers;

namespace CodeRefractor.RuntimeBase.MiddleEnd.SimpleOperations
{
    public class FieldGetter : IdentifierValue
    {
        public IdentifierValue Instance;
        public string FieldName;
    }
}