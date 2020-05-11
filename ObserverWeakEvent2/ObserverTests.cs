using NUnit.Framework;
using System;
using System.Diagnostics;
using System.Windows;

namespace ObserverWeakEvent2
{
    public class Button
    {
        public event EventHandler Clicked;

        public void Fire()
        {
            Clicked?.Invoke(this, EventArgs.Empty);
        }
    }

    public class Window
    {
        public Window(Button button)
        {
            WeakEventManager<Button, EventArgs>
                .AddHandler(button, "Clicked", OnButtonClicked);
        }

        private void OnButtonClicked(object sender, EventArgs e)
        {
            Debug.WriteLine("Button clicked (Window handler)");
        }

        ~Window()
        {
            Debug.WriteLine("Window finalized");
        }
    }

    public class ObserverTests
    {
        [Test]
        public void Test1()
        {
            var button = new Button();
            var window = new Window(button);
            var windowRef = new WeakReference(window);

            button.Fire();

            Debug.WriteLine("Setting window to null!");

            window = null;

            FireGC();

            // Window finalizer was never called until this point! --> memory was not free'ed

            Debug.WriteLine($"Is the window alive after GC? {windowRef.IsAlive}");
        }

        private void FireGC()
        {
            Debug.WriteLine("Starting GC");
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();

            Debug.WriteLine("GC done.");
        }
    }
}
