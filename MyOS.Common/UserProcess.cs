namespace MyOS.Common
{
    public abstract class UserProcess : Process
    {
        public override ProcessType ProcessType => ProcessType.UserApplication;
        private IProcessManager _processManager;

        public void Initialize(IProcessManager processManager, IMyLib myLib)
        {
            _processManager = processManager;
            MyLib = myLib;
        }

        public override void End()
        {
            _processManager.End(this);
        }

        public override void Exit(int code = 0)
        {
            End();
        }
    }
}
