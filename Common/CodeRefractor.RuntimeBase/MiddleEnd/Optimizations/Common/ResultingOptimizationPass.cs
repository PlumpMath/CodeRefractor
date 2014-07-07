#region Usings

using CodeRefractor.ClosureCompute;
using CodeRefractor.RuntimeBase.Optimizations;

#endregion

namespace CodeRefractor.MiddleEnd.Optimizations.Common
{
    public abstract class ResultingOptimizationPass
    {
        //returns true if it succeed to apply any optimizations
        //Try to return false by default
        //If the code succeeded to optimize something that other optimizations may benefit, return true

        private bool _result;

        public static ClosureEntities Closure { get; set; }
        protected ResultingOptimizationPass(OptimizationKind kind)
        {
            Kind = kind;
        }

        public bool Result
        {
            get { return _result; }
            set { _result = value; }
        }

        public abstract void OptimizeOperations(CilMethodInterpreter interpreter);

    


        public OptimizationKind Kind { get; set; }

        public virtual bool CheckPreconditions(CilMethodInterpreter midRepresentation)
        {
            return true;
        }
         //returns true if it succeed to apply any optimizations
        //Try to return false by default
        //If the code succeeded to optimize something that other optimizations may benefit, return true
        public bool Optimize(CilMethodInterpreter intermediateCode)
        {
            _result = false;
            try
            {
                OptimizeOperations(intermediateCode);
            }
            catch
            {
            }
            return _result;
        }
    }
}