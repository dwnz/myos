using System.Threading;
using System.Threading.Tasks;

namespace MyOS.Common
{
    public abstract class KernelService : KernelProcess
    {
        public override ProcessType ProcessType => ProcessType.KernelService;

        private readonly CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();

        private Task _processThread;
        private CancellationToken _cancellationToken;

        public virtual void Start(string[] args, CancellationToken cancellationToken)
        {
        }

        public abstract void Loop(string[] args);

        public override void Run(string[] args)
        {
            _cancellationToken = _cancellationTokenSource.Token;

            _processThread = new Task(
                () =>
                {
                    while (!_cancellationToken.IsCancellationRequested)
                    {
                        Loop(args);
                    }
                },
                _cancellationToken);

            _processThread.Start();
        }

        public override void Exit(int code = 0)
        {
            _cancellationTokenSource.Cancel();
            base.Exit(code);
        }
    }
}