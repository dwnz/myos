namespace MyOS.Common
{
    public interface IMyOS
    {
        void Boot(IKernel kernel, IProcessManager processManager);
    }
}
