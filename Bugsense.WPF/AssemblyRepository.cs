using System.Reflection;

namespace Bugsense.WPF
{
    class AssemblyRepository: IAssemblyRepository
    {
        public System.Reflection.Assembly GetEntryAssembly()
        {
            return Assembly.GetEntryAssembly();
        }
    }
}
