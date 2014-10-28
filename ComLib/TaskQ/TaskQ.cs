using System.Collections.Generic;
using System.Linq;

namespace ComLib.TaskQ
{
    /// <summary>
    /// Provides task/message queue functionality
    /// </summary>
    public class TaskQ
    {
        private readonly List<Task> _tasks;
        private readonly string _name;
        private readonly object _lockHelper = new object();

        /// <summary>
        /// Gets the total number of enqueued tasks.
        /// </summary>
        public long TotalEnQ { get; private set; }

        /// <summary>
        /// Gets the total number of dequeued tasks.
        /// </summary>
        public long TotalDeQ { get; private set; }

        /// <summary>
        /// Initializes a TaskQ.
        /// </summary>
        public TaskQ()
        {
            _tasks = new List<Task>();
        }

        /// <summary>
        /// DO NOT USE THIS INITIALIZER!
        /// </summary>
        /// <param name="capacity">OOPS!</param>
        public TaskQ(int capacity)
        {
            _tasks=new List<Task>(capacity);
        }

        /// <summary>
        /// Initializes a named TaskQ.
        /// </summary>
        /// <param name="name">The name of the TaskQ.</param>
        public TaskQ(string name)
            : this()
        {
            _name = name;
        }

        /// <summary>
        /// Gets the number of the tasks in queue.
        /// </summary>
        public int Count
        {
            get { return _tasks.Count; }
        }

        /// <summary>
        /// Gets a value indicating whether there is any task in the queue.
        /// </summary>
        public bool HasTask
        {
            get { return _tasks.Any(); }
        }

        /// <summary>
        /// Gets the capacity of the task queue.
        /// </summary>
        public int Capacity
        {
            get { return _tasks.Capacity; }
        }

        /// <summary>
        /// Adds a task to the end of the queue.
        /// </summary>
        /// <param name="task">The task to add.</param>
        /// <returns><code>true</code> on success, <code>false</code> on failure.</returns>
        public bool EnQ(Task task)
        {
            lock (_lockHelper)
            {
                const int lowerBound = 0;
                int upperBound = _tasks.Count;
                int position = Position(task.Priority, lowerBound, upperBound);
                _tasks.Insert(position, task);
                unchecked
                {
                    ++TotalEnQ;
                }
                return true;
            }
        }

        /// <summary>
        /// Removes and returns the first task in the queue.
        /// 
        /// Exceptions:
        /// TaskQEmptyException:
        ///     The queue is empty.
        /// </summary>
        /// <returns>The first task in the queue.</returns>
        public Task DeQ()
        {
            lock (_lockHelper)
            {
                if (_tasks.Count == 0)
                {
                    throw new TaskQEmptyException();
                }
                Task task = _tasks[0];
                _tasks.RemoveAt(0);
                unchecked
                {
                    ++TotalDeQ;
                }
                return task;
            }
        }

        /// <summary>
        /// Gets a value indicating whether the queue is empty.
        /// </summary>
        public bool IsEmpty
        {
            get { return !_tasks.Any(); }
        }

        private int Position(int priority, int lowerBound, int upperBound)
        {
            if (_tasks.Count == 0)
                return 0;
            if (lowerBound == upperBound)
                return lowerBound;

            int splitPos = (lowerBound + upperBound)/2;
            if (_tasks[splitPos].Priority > priority)
            {
                return Position(priority, lowerBound, splitPos);
            }
            return Position(priority, splitPos + 1, upperBound);
        }
    }
}
