using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YnamarEditors.Interfaces;

namespace YnamarEditors.Services
{
    internal class CommandService
    {
        private readonly Stack<ICommand> _undoStack = new();
        private readonly Stack<ICommand> _redoStack = new();

        public void ExecuteCommand(ICommand command)
        {
            command.Execute();

            _undoStack.Push(command);
            _redoStack.Clear();
        }

        public void UndoCommand()
        {
            if (_undoStack.Count == 0) return;

            ICommand command = _undoStack.Pop();
            command.Undo();
            _redoStack.Push(command);
        }

        public void RedoCommand()
        {
            if (_redoStack.Count == 0) return;

            ICommand command = _redoStack.Pop();
            command.Execute();
            _undoStack.Push(command);
        }
    }
}
