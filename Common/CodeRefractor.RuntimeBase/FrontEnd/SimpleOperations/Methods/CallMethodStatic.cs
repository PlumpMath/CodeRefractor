#region Uses

using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using CodeRefractor.FrontEnd;
using CodeRefractor.FrontEnd.SimpleOperations;
using CodeRefractor.FrontEnd.SimpleOperations.Identifiers;
using CodeRefractor.MiddleEnd.Interpreters;
using CodeRefractor.MiddleEnd.SimpleOperations.Identifiers;

#endregion

namespace CodeRefractor.MiddleEnd.SimpleOperations.Methods
{
    public class CallMethodStatic : LocalOperation
    {
        public LocalVariable Result;

        public CallMethodStatic(MethodInterpreter interpreter)
            : base(OperationKind.Call)
        {
            Parameters = new List<IdentifierValue>();

            Interpreter = interpreter;
            Info = interpreter.Method;
        }

        public bool IsVoid => Interpreter.Method.GetReturnType() == typeof (void);

        public List<IdentifierValue> Parameters { get; set; }
        public MethodInterpreter Interpreter { get; set; }
        public MethodBase Info { get; set; }

        public void ExtractNeededValuesFromStack(EvaluatorStack evaluatorStack)
        {
            var methodParams = Info.GetParameters();
            if (Info.IsConstructor)
            {
                Parameters.Insert(0, evaluatorStack.Pop());
                foreach (var t in methodParams)
                    Parameters.Insert(1, evaluatorStack.Pop());
                return;
            }
            foreach (var t in methodParams)
                Parameters.Insert(0, evaluatorStack.Pop());
            if (!Info.IsStatic)
                Parameters.Insert(0, evaluatorStack.Pop());
        }

        public override string ToString()
        {
            var paramData = string.Join(", ",
                Parameters.Select(
                    par =>
                        string.Format("{0}:{1}", par.Name, par.ComputedType().Name)));
            if (Result == null)
            {
                return string.Format("{0}({1})", Info.Name,
                    paramData);
            }
            return string.Format("{0} = {1}({2})", Result.Name, Info.Name, paramData);
        }
    }
}