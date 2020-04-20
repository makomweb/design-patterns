using Autofac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Console;

namespace AdapterDependencyInjection
{
    public interface ICommand
    {
        void Execute();
    }

    public class SaveCommand : ICommand
    {
        public void Execute()
        {
            WriteLine("Saving file.");
        }
    }

    public class OpenCommand : ICommand
    {
        public void Execute()
        {
            WriteLine("Opening file.");
        }
    }

    public class Button
    {
        private readonly ICommand _command;

        public Button(ICommand command)
        {
            if (command == null) throw new ArgumentNullException(paramName: nameof(command));

            _command = command;
        }

        public void Click()
        {
            _command.Execute();
        }
    }

    public class Editor
    {
        private IEnumerable<Button> _buttons;

        public Editor(IEnumerable<Button> buttons)
        {
            if (buttons == null) throw new ArgumentNullException(paramName: nameof(buttons));
            _buttons = buttons;
        }

        public void ClickAll()
        {
            foreach (var b in _buttons)
            {
                b.Click();
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var b = new ContainerBuilder();
            b.RegisterType<SaveCommand>().As<ICommand>();
            b.RegisterType<OpenCommand>().As<ICommand>();
            //b.RegisterType<Button>();
            b.RegisterAdapter<ICommand, Button>(cmd => new Button(cmd));
            b.RegisterType<Editor>();

            using (var c = b.Build())
            {
                var e = c.Resolve<Editor>();
                e.ClickAll();
            }
        }
    }
}
