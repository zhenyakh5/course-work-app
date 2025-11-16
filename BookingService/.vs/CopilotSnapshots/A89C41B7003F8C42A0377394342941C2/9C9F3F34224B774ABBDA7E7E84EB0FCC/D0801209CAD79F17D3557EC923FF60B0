using System.Windows;
using System.Windows.Input;

namespace BookingService
{
    public partial class AdminPanel : Window
    {
        private readonly AppDbContext _context = new AppDbContext();

        public static RoutedCommand AddRoomCommand { get; } = new RoutedCommand();
        public static RoutedCommand EditRoomCommand { get; } = new RoutedCommand();
        public static RoutedCommand DeleteRoomCommand { get; } = new RoutedCommand();
        public static RoutedCommand UndoCommand { get; } = new RoutedCommand();
        public static RoutedCommand RedoCommand { get; } = new RoutedCommand();

        public AdminPanel()
        {
            InitializeComponent();

            CommandBindings.Add(new CommandBinding(AddRoomCommand, ExecuteAddRoomCommand));
            CommandBindings.Add(new CommandBinding(EditRoomCommand, ExecuteEditRoomCommand));
            CommandBindings.Add(new CommandBinding(DeleteRoomCommand, ExecuteDeleteRoomCommand));
            CommandBindings.Add(new CommandBinding(UndoCommand, ExecuteUndoCommand, CanExecuteUndoRedo));
            CommandBindings.Add(new CommandBinding(RedoCommand, ExecuteRedoCommand, CanExecuteUndoRedo));
        }

        private void CanExecuteUndoRedo(object sender, CanExecuteRoutedEventArgs e)
        {
            if (e.Command == UndoCommand)
                e.CanExecute = _context.CanUndo;
            else if (e.Command == RedoCommand)
                e.CanExecute = _context.CanRedo;
        }

        private void ExecuteAddRoomCommand(object sender, ExecutedRoutedEventArgs e)
        {
            var addRoomWindow = new AddRoom(_context);
            addRoomWindow.Show();
        }

        private void ExecuteEditRoomCommand(object sender, ExecutedRoutedEventArgs e)
        {
            var editRoomWindow = new EditRoom(_context);
            editRoomWindow.Show();
        }

        private void ExecuteDeleteRoomCommand(object sender, ExecutedRoutedEventArgs e)
        {
            var deleteRoomWindow = new DeleteRoom(_context);
            deleteRoomWindow.Show();
        }

        private void ExecuteUndoCommand(object sender, ExecutedRoutedEventArgs e)
        {
            _context.Undo();
        }

        private void ExecuteRedoCommand(object sender, ExecutedRoutedEventArgs e)
        {
            _context.Redo();
        }
    }
}