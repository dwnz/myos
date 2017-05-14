using System.Collections.Generic;
using System.Linq;
using MyOS.Common;

namespace MyOS
{
    public class ProcessManager : IProcessManager
    {
        private readonly List<IProcess> _processes = new List<IProcess>();

        public IReadOnlyList<IProcess> List()
        {
            IReadOnlyList<IProcess> processes = _processes;
            return processes;
        }

        public void Register(IProcess process)
        {
            _processes.Add(process);
        }

        public void End(IProcess process)
        {
            IProcess toEnd = _processes.First(x => x.ProcessId == process.ProcessId);
            _processes.Remove(toEnd);
        }
    }
}
