using ConsoleDraw;

namespace Text
{
    class Program
    {
        static void Main(string[] args)
        {
            //Setup
            WindowManager.UpdateWindow(150, 40);
            WindowManager.SetWindowTitle("TEXT");

            //Start Program
            new MainWindow();   
        }
    }
}
