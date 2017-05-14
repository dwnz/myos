using ConsoleDraw;
using MyOS.Common;
using Text;

namespace MyOS.Apps.Text
{
    public class Application : UserProcess
    {
        public override string Name => "text";

        public override void Run(string[] args)
        {
            //Setup
            WindowManager.UpdateWindow(150, 40);
            WindowManager.SetWindowTitle("TEXT");

            //Start Program
            new MainWindow();
        }
    }
}
