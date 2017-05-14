using System;

namespace MyOS.Common
{
    public interface IProcess
    {
        Guid ProcessId { get; }
        string Name { get; }
        ProcessType ProcessType { get; }

        void Run(string[] args);
        void End();
    }

    public enum ProcessType
    {
        KernelApplication,
        UserApplication,
        KernelService,
        UserService
    }
}
