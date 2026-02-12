 class TodoListApp {
    private TodoList _tasks;
    private bool _showHelp = true;
    private bool _insertMode = true;
    private bool _quit = false;

    public TodoListApp(TodoList tasks) {
        _tasks = tasks;
    }

    public void Run() {
        while (!_quit) {
            Console.Clear();
            Display();
            ProcessUserInput();
        }
    }

    public void Display() {
        DisplayTasks();
        if (_showHelp) {
            DisplayHelp();
        }
    }

    public void DisplayBar() {
        Console.WriteLine("----------------------------");
    }

    public string MakeRow(int i) {
        Task task = _tasks.GetTask(i);
        string arrow = "  ";
        if (task == _tasks.CurrentTask) arrow = "->";
        string check = " ";
        if (task.Status == CompletionStatus.Done) check = "X";
        return $"{arrow} [{check}] {task.Title}";
    }

    public void DisplayTasks() {
        DisplayBar();
        Console.WriteLine("Tasks:");
        for (int i = 0; i < _tasks.Length; i++) {
            Console.WriteLine(MakeRow(i));
        }
        DisplayBar();
    }

    public void DisplayHelp() {
        Console.WriteLine(
@"Instructions:
   h: show/hide instructions
   ↕: select previous or next task (wrapping around at the top and bottom)
   ↔: reorder task (swap selected task with previous or next task)
   space: toggle completion of selected task
   e: edit title
   i: insert new tasks
   delete/backspace: delete task");
        DisplayBar();
    }

    private string GetTitle() {
        Console.WriteLine("Please enter task title (or [enter] for none): ");
        return Console.ReadLine()!;
    }

    public void ProcessUserInput() {
        if (_insertMode) {
            string taskTitle = GetTitle();
            if (taskTitle.Length == 0) {
                _insertMode = false;
            } else {
                _tasks.Insert(taskTitle);
                _insertMode = false;
            }
        } else {
            switch (Console.ReadKey(true).Key) {
                case ConsoleKey.Escape:
                    _quit = true;
                    break;
                case ConsoleKey.UpArrow:
                    _tasks.SelectPrevious();
                    break;
                case ConsoleKey.DownArrow:
                    _tasks.SelectNext();
                    break;
                case ConsoleKey.LeftArrow:
                    _tasks.SwapWithPrevious();
                    break;
                case ConsoleKey.RightArrow:
                    _tasks.SwapWithNext();
                    break;
                case ConsoleKey.I:
                    _insertMode = true;
                    break;
                case ConsoleKey.E:
                    _tasks.CurrentTask.Title = GetTitle();
                    break;
                case ConsoleKey.H:
                    _showHelp = !_showHelp;
                    break;
                case ConsoleKey.Enter:
                case ConsoleKey.Spacebar:
                    _tasks.CurrentTask.ToggleStatus();
                    break;
                case ConsoleKey.Delete:
                case ConsoleKey.Backspace:
                    _tasks.DeleteSelected();
                    break;
                default:
                    break;
            }
        }
    }
  }

  class Program {
    static void Main() {
        new TodoListApp(new TodoList()).Run();
    }
  }

class TodoList
{
    
    public int Length {get => TaskList.Count();}
    Dictionary<int, Task> TaskList;
    public Task CurrentTask {get; set;}
    public int Number;

    public TodoList()
    {
        TaskList = new Dictionary<int, Task> ();
        CurrentTask = new Task("");
        Number = 0;
    }

    public Task GetTask(int i)
    {
        return TaskList[i];
    }
    public void Insert(string Title)
    {
        TaskList.Add(TaskList.Count(), new Task(Title));
        Number ++;
    }
    public void SelectNext()
    {
        if (!(TaskList.Count() >= Number ++)) Number ++; 
        CurrentTask = TaskList[Number];
    }
    public void SelectPrevious()
    {
        if (!(TaskList.Count() >= Number ++)) Number --;
        CurrentTask = TaskList[Number];
    }
    public void SwapWithNext()
    {
        if (TaskList.Count() > Number++) 
        {
            (TaskList[Number], TaskList[Number+1]) = (TaskList[Number+1], TaskList[Number]);
        }
    }   
    public void SwapWithPrevious()
    {
     if (TaskList.Count() > Number--) (TaskList[Number], TaskList[Number-1]) = (TaskList[Number-1], TaskList[Number]);   
    }
    public void DeleteSelected()
    {
        TaskList.Remove(Number);
        for (int i = Number+1; i < TaskList.Count(); i++)
        {
            TaskList[i].Equals(Number--);
        }
    }
}
class Task
{
    public string Title {get; set;}
    public CompletionStatus Status {get; set;}

    public Task(string _title)
    {
        Title = _title;
    }
    public void ToggleStatus()
    {
        if (Status == CompletionStatus.Done) Status = CompletionStatus.InProgress;
        if (Status == CompletionStatus.InProgress) Status = CompletionStatus.Done;
    }
}
enum CompletionStatus {Done, InProgress}