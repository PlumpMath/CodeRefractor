#include "sloth.h"
#include "runtime_base.partcpp"
#include "math.h"
System::Double System_Math__Sin(System::Double a)
{ return sin(a); }
#include "stdio.h"
void System_Console__WriteLine(System::Double value)
{ printf("%lf\n", value); }
namespace  {
struct NBody;
}
namespace  {
struct NBody {
}; }
void _NBody__Main();
void _NBody__Main()
{
System::Double local_0;

local_0 = 0;
System_Console__WriteLine(0);
return;
}


int main(int argc, char**argv) {
auto argsAsList = System::getArgumentsAsList(argc, argv);
_NBody__Main();
return 0;
}
void mapLibs() {
}

void RuntimeHelpersBuildConstantTable() {
}

