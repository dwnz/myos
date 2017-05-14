using System.Collections.Generic;

namespace MyOS.Common
{
    public interface IProcessManager
    {
        void Register(IProcess process);
        void End(IProcess process);
        IReadOnlyList<IProcess> List();
    }
}