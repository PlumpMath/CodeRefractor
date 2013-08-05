#region Usings

using CodeRefractor.CompilerBackend.Optimizations.Common;
using CodeRefractor.RuntimeBase.MiddleEnd;
using CodeRefractor.RuntimeBase.MiddleEnd.SimpleOperations;
using CodeRefractor.RuntimeBase.MiddleEnd.SimpleOperations.Identifiers;

#endregion

namespace CodeRefractor.CompilerBackend.Optimizations.ConstantFoldingAndPropagation.ComplexAssignments
{
    internal class DeleteVregAsLocalAssignedAndUsedPreviousLine : ResultingOptimizationPass
    {
        private int _currentRow;
        private MetaMidRepresentation _intermediateCode;

        public override void OptimizeOperations(MetaMidRepresentation intermediateCode)
        {
            _intermediateCode = intermediateCode;
            var operations = intermediateCode.LocalOperations;
            for (var i = 1; i < operations.Count - 1; i++)
            {
                var srcOperation = operations[i];
                if (srcOperation.Kind != LocalOperation.Kinds.Assignment)
                    continue;
                var srcAssign = srcOperation.GetAssignment();
                var leftLocal = srcAssign.Left;
                if (leftLocal.Kind == VariableKind.Vreg) continue;
                var rightVreg = srcAssign.Right as LocalVariable;
                if (rightVreg == null) continue;

                var destOperation = operations[i - 1];
                _currentRow = i;
                var destAssignment = destOperation.Value as Assignment;
                if (destAssignment == null)
                    continue;
                if (destAssignment.Left.Kind != VariableKind.Vreg || rightVreg.Id != destAssignment.Left.Id) continue;
                destAssignment.Left = leftLocal;
                DeletVreg();
            }
        }

        private void DeletVreg()
        {
            Result = true;
            _intermediateCode.LocalOperations.RemoveAt(_currentRow);
        }
    }
}