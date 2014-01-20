using System;
using System.Runtime.InteropServices;
using CodeRefractor.RuntimeBase;
using CodeRefractor.RuntimeBase.MiddleEnd;
using Tao.OpenGl;
using Tao.Sdl;

namespace Game
{
    public class CrGlu
    {
    }

    public class CrGl
    {
    }
    public class CrSdl
    {
    }
    public class TypeResolver : CrTypeResolver
    {
        public TypeResolver()
        {
            MapType<CrGl>(typeof(Gl));
            MapType<CrGlu>(typeof(Glu));
            MapType<CrSdl>(typeof(Sdl));
        }
        public override bool Resolve(MethodInterpreter methodInterpreter)
        {
            var method = methodInterpreter.Method;

            if (method.DeclaringType == typeof (Gl))
            {
                ResolveAsPinvoke(methodInterpreter, "opengl32.dll", CallingConvention.StdCall);
                return true;
            }
            if (method.DeclaringType == typeof(Sdl))
            {
                ResolveAsPinvoke(methodInterpreter, "sdl.dll", CallingConvention.StdCall);
                return true;
            }
            return false;
        }

        public override Type ResolveType(Type type)
        {
            return MappedResolveType(type);
        }
    }
}