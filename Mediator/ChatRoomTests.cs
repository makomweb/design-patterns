using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Mediator
{
    public class Person
    {
        public string Name;
        public ChatRoom Room;
        public List<string> chatLog = new List<string>();

        public Person(string name)
        {
            Name = name;
        }

        public void Say(string message)
        {
            Room.Broadcast(Name, message);
        }

        public void PrivateMessage(string who, string message)
        {
            Room.Message(Name, who, message);
        }

        public void Receive(string sender, string message)
        {
            string s = $"{sender}: {message}";
            chatLog.Add(s);
            Debug.WriteLine($"[{Name}'s chat session] {s}");
        }
    }

    public class ChatRoom
    {
        private List<Person> _people = new List<Person>();

        public void Join(Person p)
        {
            p.Room = this;

            var joinMsg = $"{p.Name} joins the chat";

            Broadcast("room", joinMsg);
            _people.Add(p);
        }

        public void Broadcast(string source, string message)
        {
            foreach (var p in _people)
            {
                if (p.Name != source)
                {
                    p.Receive(source, message);
                }
            }
        }

        public void Message(string source, string destination, string message)
        {
            _people.FirstOrDefault(p => p.Name == destination)
                ?.Receive(source, message);
        }
    }

    public class ChatRoomTests
    {
        [Test]
        public void Test1()
        {
            var room = new ChatRoom();

            var john = new Person("John");
            var jane = new Person("Jane");

            room.Join(john);
            room.Join(jane);

            john.Say("Hi!");

            jane.Say("oh hai dare!");

            var simon = new Person("Simon");
            room.Join(simon);

            simon.Say("hi everyone!");

            jane.PrivateMessage("Simon", "glad you could join us!");
        }
    }
}