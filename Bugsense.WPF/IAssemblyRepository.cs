using System.Reflection;

namespace Bugsense.WPF
{
    /// <summary>
    /// Should not be public but Castle.Dynamic proxies wont allow it.
    /// </summary>
    public interface IAssemblyRepository
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns>Assembly name</returns>
        Assembly GetEntryAssembly();
    }
}
