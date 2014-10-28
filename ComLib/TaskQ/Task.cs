using System;

namespace ComLib.TaskQ
{
    public enum TaskState
    {
        Success,
        Failure
    }

    public abstract class Task
    {
        public string Name { get; protected set; }
        public int Priority { get; protected set; }
        public int Retries { get; protected set; }

        //TODO: Try to specialize the EventHandlers with environmental parameters
        public event EventHandler OnStart;
        public event EventHandler OnExecuting;
        public event EventHandler OnExecuted;
        public event EventHandler OnFailure;

        public TaskState Do()
        {
            // Event OnStart
            if (OnStart != null)
                OnStart(this, new EventArgs());
            do
            {
                // Event OnExecuting
                if (OnExecuting != null)
                    OnExecuting(this, new EventArgs());

                bool res = Execute();

                // Event OnExecuted
                if (OnExecuted != null)
                    OnExecuted(this, new EventArgs());

                if(res)
                    return TaskState.Success;

                // Event OnFailure
                if(OnFailure!=null)
                    OnFailure(this, new EventArgs());
                Fail();
            } while (Retries-- > 0);
            return TaskState.Failure;
        }

        protected Task(int priority = 0, int retries = 0)
        {
            Priority = priority;
            Retries = retries;
        }

        protected Task(string name, int priority = 0, int retries = 0)
            : this(priority, retries)
        {
            Name = name;
        }

        protected abstract bool Execute();

        protected abstract void Fail();
    }
}
