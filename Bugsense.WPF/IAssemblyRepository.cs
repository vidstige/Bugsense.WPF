using System.Reflection;

namespace Bugsense.WPF
{
    interface IAssemblyRepository
    {
        Assembly GetEntryAssembly();
    }
}
