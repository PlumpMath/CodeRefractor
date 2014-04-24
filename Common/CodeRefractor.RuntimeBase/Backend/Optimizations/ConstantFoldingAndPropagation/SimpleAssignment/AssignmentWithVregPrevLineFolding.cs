#region Usings

using CodeRefractor.CompilerBackend.Optimizations.Common;
using CodeRefractor.RuntimeBase.Analyze;
using CodeRefractor.RuntimeBase.MiddleEnd;
using CodeRefractor.RuntimeBase.MiddleEnd.SimpleOperations;
using CodeRefractor.RuntimeBase.MiddleEnd.SimpleOperations.Identifiers;
using CodeRefractor.RuntimeBase.MiddleEnd.SimpleOperations.Operators;

#endregion

namespace CodeRefractor.CompilerBackend.Optimizations.ConstantFoldingAndPropagation.SimpleAssignment
{
    public class AssignmentWithVregPrevLineFolding : ResultingInFunctionOptimizationPass
    {
        public override void OptimizeOperations(MethodInterpreter interpreter)
        {
            var operations = interpreter.MidRepresentation.LocalOperations;

            for (var index = operations.Count - 1; index >= 1; index--)
            {
                var localOperation = operations[index];
                if (localOperation.Kind != OperationKind.Assignment)
                    continue;

                var assignment = localOperation.GetAssignment();
                var vregAssignment = assignment.Right as LocalVariable;

                if (vregAssignment == null || vregAssignment.Kind != VariableKind.Vreg) continue;

                var destOperation = operations[index - 1];
                var destOperationDefiniton = destOperation.GetDefinition();
                if (destOperationDefiniton == null || !destOperationDefiniton.Equals(vregAssignment)) continue;
                switch (destOperation.Kind)
                {
                    case OperationKind.UnaryOperator:
                    case OperationKind.BinaryOperator:
                        var operatorData = (OperatorBase) destOperation.Value;
                        operatorData.AssignedTo = assignment.AssignedTo;
                        break;
                    default:
                        continue;
                }
                Result = true;
                operations.RemoveAt(index);
                return;
            }
        }
    }
}