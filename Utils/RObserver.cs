using System;
using System.Collections.Generic;
using System.Linq;
using System.Timers;
using _4RTools.Model;
using System.Diagnostics;

namespace _4RTools.Utils
{
    public interface IObserver
    {
        void Update(ISubject subject);
    }

    public interface ISubject
    {
        Message Message { get; }
        void Attach(IObserver observer);
        void Detach(IObserver observer);
        void Notify(Message message);
    }

    public enum MessageCode { 
        PROCESS_CHANGED,
        PROCESS_LIST_CHANGED,
        PROFILE_CHANGED,
        PROFILE_INPUT_CHANGE,
        TURN_ON,
        TURN_OFF,
        SHUTDOWN_APPLICATION,
        CLICK_ICON_TRAY,
        SERVER_LIST_CHANGED,
        TURN_HEAL_ON,
        TURN_HEAL_OFF,
        ADDED_NEW_AUTOBUFF_SKILL,
        CHANGED_AUTOSWITCH_SKILL,
        ADDED_NEW_AUTOSWITCH_PETS,
        LOAD_PROFILE_BY_NAME,
        CLIENT_DISCONNECTED
    }

    public class Message
    {
        public MessageCode code { get; }
        public object data { get; set; }
        public Message() { }

        public Message(MessageCode code, object data)
        {
            this.code = code;
            this.data = data;
        }
    }

    public class Subject : ISubject
    {
        public Message Message { get; private set; } = new Message();
        private List<IObserver> _observers = new List<IObserver>();
        public void Attach(IObserver observer)
        {
            Console.WriteLine("Subject: Attached an observer.");
            this._observers.Add(observer);
        }

        public void Detach(IObserver observer)
        {
            this._observers.Remove(observer);
            Console.WriteLine("Subject: Detached an observer.");
        }

        public void Notify(Message message)
        {
            Console.WriteLine("Subject: Notifying observers...");
            this.Message = message;
            foreach (var observer in _observers)
            {
                observer.Update(this);
            }
        }
    }

    public class ClientObserver : ISubject
    {
        private static ClientObserver instance;
        private readonly Timer timer;
        
        private readonly List<IObserver> _observers = new List<IObserver>();
        public Message Message { get; private set; } = new Message();

        private List<string> processList = new List<string>();
        private string selectedProcessName;
        private Client client;

        private ClientObserver()
        {
            timer = new Timer(2000);
            timer.Elapsed += OnTimerElapsed;
            timer.AutoReset = true;
            timer.Enabled = true;
        }

        public static ClientObserver Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new ClientObserver();
                }
                return instance;
            }
        }

        private void OnTimerElapsed(object source, ElapsedEventArgs e)
        {
            List<string> currentProcesses = new List<string>();
            foreach (Process p in Process.GetProcesses())
            {
                if (p.MainWindowTitle != "" && ClientListSingleton.ExistsByProcessName(p.ProcessName))
                {
                    currentProcesses.Add(string.Format("{0}.exe - {1}", p.ProcessName, p.Id));
                }
            }

            if (!processList.SequenceEqual(currentProcesses))
            {
                processList = currentProcesses;
                Notify(new Message(MessageCode.PROCESS_LIST_CHANGED, processList));
            }

            if (client != null && client.process != null && !client.process.HasExited)
            {
                string currentCharacterName = client.ReadCharacterName();
                if (!string.IsNullOrEmpty(currentCharacterName) && currentCharacterName != client.characterName)
                {
                    client.characterName = currentCharacterName;
                    Notify(new Message(MessageCode.CLIENT_DISCONNECTED, null)); // To reset manualProfileSelectionDone in Container

                    string profileName = CharacterProfileManager.GetProfileName(currentCharacterName);
                    if (profileName != null)
                    {
                        Notify(new Message(MessageCode.LOAD_PROFILE_BY_NAME, profileName));
                    }
                }
            }

            if (client != null && (client.process == null || client.process.HasExited))
            {
                client = null;
                selectedProcessName = null;
                ClientSingleton.Instance(null);
                Notify(new Message(MessageCode.CLIENT_DISCONNECTED, null));
            }
        }

        public void SelectProcess(string processName)
        {
            if (selectedProcessName != processName)
            {
                selectedProcessName = processName;
                if (selectedProcessName != null)
                {
                    client = new Client(selectedProcessName);
                    ClientSingleton.Instance(client);
                    Notify(new Message(MessageCode.PROCESS_CHANGED, client));

                    string characterName = client.ReadCharacterName();
                    if (!string.IsNullOrEmpty(characterName))
                    {
                        string profileName = CharacterProfileManager.GetProfileName(characterName);
                        if (profileName != null)
                        {
                            Notify(new Message(MessageCode.LOAD_PROFILE_BY_NAME, profileName));
                        }
                    }
                }
                else
                {
                    client = null;
                    ClientSingleton.Instance(null);
                    Notify(new Message(MessageCode.PROCESS_CHANGED, null));
                }
            }
        }
        
        public List<string> GetProcessList()
        {
            return new List<string>(processList);
        }

        public void Attach(IObserver observer)
        {
            this._observers.Add(observer);
        }

        public void Detach(IObserver observer)
        {
            this._observers.Remove(observer);
        }

        public void Notify(Message message)
        {
            this.Message = message;
            foreach (var observer in _observers)
            {
                observer.Update(this);
            }
        }
    }
}