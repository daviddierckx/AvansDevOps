using System;
using AvansDevOps.Domain.Models;

namespace AvansDevOps.Domain.States
{
    // State Pattern - ToDoState
    public class ToDoState : IBacklogItemState
    {
        public void ToDo(BacklogItem item)
        {
            throw new InvalidOperationException("Item staat al in ToDo.");
        }
        public void Doing(BacklogItem item)
        {
            item.SetState(new DoingState());
        }
        public void ReadyForTesting(BacklogItem item)
        {
            throw new InvalidOperationException("Ongeldige overgang vanuit ToDo naar ReadyForTesting.");
        }
        public void Testing(BacklogItem item)
        {
            throw new InvalidOperationException("Ongeldige overgang vanuit ToDo naar Testing.");
        }
        public void Tested(BacklogItem item)
        {
            throw new InvalidOperationException("Ongeldige overgang vanuit ToDo naar Tested.");
        }
        public void Done(BacklogItem item)
        {
            throw new InvalidOperationException("Ongeldige overgang vanuit ToDo naar Done.");
        }
        public string GetName() { return "ToDo"; }
    }

    // State Pattern - DoingState
    public class DoingState : IBacklogItemState
    {
        public void ToDo(BacklogItem item)
        {
            throw new InvalidOperationException("Terug naar ToDo kan niet vanuit Doing.");
        }
        public void Doing(BacklogItem item)
        {
            throw new InvalidOperationException("Item staat al in Doing.");
        }
        public void ReadyForTesting(BacklogItem item)
        {
            if (!item.AllActivitiesDone())
                throw new InvalidOperationException("Niet alle activiteiten zijn afgerond.");
            item.SetState(new ReadyForTestingState());
            item.NotifyObservers("ready_for_testing");
        }
        public void Testing(BacklogItem item)
        {
            throw new InvalidOperationException("Ongeldige overgang vanuit Doing naar Testing.");
        }
        public void Tested(BacklogItem item)
        {
            throw new InvalidOperationException("Ongeldige overgang vanuit Doing naar Tested.");
        }
        public void Done(BacklogItem item)
        {
            throw new InvalidOperationException("Ongeldige overgang vanuit Doing naar Done.");
        }
        public string GetName() { return "Doing"; }
    }

    // State Pattern - ReadyForTestingState
    public class ReadyForTestingState : IBacklogItemState
    {
        public void ToDo(BacklogItem item)
        {
            item.SetState(new ToDoState());
            item.NotifyObservers("returned_to_todo");
        }
        public void Doing(BacklogItem item)
        {
            throw new InvalidOperationException("Terug naar Doing kan niet vanuit ReadyForTesting.");
        }
        public void ReadyForTesting(BacklogItem item)
        {
            throw new InvalidOperationException("Item staat al in ReadyForTesting.");
        }
        public void Testing(BacklogItem item)
        {
            item.SetState(new TestingState());
        }
        public void Tested(BacklogItem item)
        {
            throw new InvalidOperationException("Ongeldige overgang vanuit ReadyForTesting naar Tested.");
        }
        public void Done(BacklogItem item)
        {
            throw new InvalidOperationException("Ongeldige overgang vanuit ReadyForTesting naar Done.");
        }
        public string GetName() { return "ReadyForTesting"; }
    }

    // State Pattern - TestingState
    public class TestingState : IBacklogItemState
    {
        public void ToDo(BacklogItem item)
        {
            throw new InvalidOperationException("Ongeldige overgang vanuit Testing naar ToDo.");
        }
        public void Doing(BacklogItem item)
        {
            throw new InvalidOperationException("Ongeldige overgang vanuit Testing naar Doing.");
        }
        public void ReadyForTesting(BacklogItem item)
        {
            item.SetState(new ReadyForTestingState());
        }
        public void Testing(BacklogItem item)
        {
            throw new InvalidOperationException("Item staat al in Testing.");
        }
        public void Tested(BacklogItem item)
        {
            item.SetState(new TestedState());
        }
        public void Done(BacklogItem item)
        {
            throw new InvalidOperationException("Ongeldige overgang vanuit Testing naar Done.");
        }
        public string GetName() { return "Testing"; }
    }

    // State Pattern - TestedState
    public class TestedState : IBacklogItemState
    {
        public void ToDo(BacklogItem item)
        {
            item.SetState(new ToDoState());
            item.NotifyObservers("returned_to_todo");
        }
        public void Doing(BacklogItem item)
        {
            throw new InvalidOperationException("Ongeldige overgang vanuit Tested naar Doing.");
        }
        public void ReadyForTesting(BacklogItem item)
        {
            item.SetState(new ReadyForTestingState());
        }
        public void Testing(BacklogItem item)
        {
            throw new InvalidOperationException("Ongeldige overgang vanuit Tested naar Testing.");
        }
        public void Tested(BacklogItem item)
        {
            throw new InvalidOperationException("Item staat al in Tested.");
        }
        public void Done(BacklogItem item)
        {
            item.SetState(new DoneState());
            item.LockThreads();
        }
        public string GetName() { return "Tested"; }
    }

    // State Pattern - DoneState
    public class DoneState : IBacklogItemState
    {
        public void ToDo(BacklogItem item)
        {
            item.SetState(new ToDoState());
            item.UnlockThreads();
        }
        public void Doing(BacklogItem item)
        {
            throw new InvalidOperationException("Ongeldige overgang vanuit Done naar Doing.");
        }
        public void ReadyForTesting(BacklogItem item)
        {
            throw new InvalidOperationException("Ongeldige overgang vanuit Done naar ReadyForTesting.");
        }
        public void Testing(BacklogItem item)
        {
            throw new InvalidOperationException("Ongeldige overgang vanuit Done naar Testing.");
        }
        public void Tested(BacklogItem item)
        {
            throw new InvalidOperationException("Ongeldige overgang vanuit Done naar Tested.");
        }
        public void Done(BacklogItem item)
        {
            throw new InvalidOperationException("Item staat al in Done.");
        }
        public string GetName() { return "Done"; }
    }
}