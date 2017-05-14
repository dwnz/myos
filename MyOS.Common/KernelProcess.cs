namespace MyOS.Common
{
    public abstract class KernelProcess : Process
    {
        protected IKernel Kernel;
        protected IProcessManager ProcessManager;

        public override ProcessType ProcessType => ProcessType.KernelApplication;

        public void Initialize(IKernel kernel, IProcessManager processManager, IMyLib myLib)
        {
            Kernel = kernel;
            ProcessManager = processManager;
            MyLib = myLib;
        }

        public override void End()
        {
            ProcessManager.End(this);
        }

        public override void Exit(int code = 0)
        {
            End();
        }
    }
}