using Autofac;
using Autofac.Features.Metadata;
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
        private string _name;

        public Button(ICommand command, string name)
        {
            if (command == null) throw new ArgumentNullException(paramName: nameof(command));

            _command = command;
            _name = name;
        }

        public void Click()
        {
            _command.Execute();
        }

        public void PrintMe()
        {
            WriteLine($"I am a button called {_name}.");
        }
    }

    public class Editor
    {
        public IEnumerable<Button> Buttons { get; private set; }

        public Editor(IEnumerable<Button> buttons)
        {
            if (buttons == null) throw new ArgumentNullException(paramName: nameof(buttons));
            Buttons = buttons;
        }

        public void ClickAll()
        {
            foreach (var b in Buttons)
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
            b.RegisterType<SaveCommand>().As<ICommand>()
                .WithMetadata("Name", "Save");
            b.RegisterType<OpenCommand>().As<ICommand>()
                .WithMetadata("Name", "Open");

            //b.RegisterType<Button>();
            //b.RegisterAdapter<ICommand, Button>(cmd => new Button(cmd));
            b.RegisterAdapter<Meta<ICommand>, Button>(cmd =>
                new Button(cmd.Value, (string)cmd.Metadata["Name"]));

            b.RegisterType<Editor>();

            using (var c = b.Build())
            {
                var e = c.Resolve<Editor>();
                //e.ClickAll();

                foreach (var btn in e.Buttons)
                {
                    btn.PrintMe();
                }
            }
        }
    }
}
