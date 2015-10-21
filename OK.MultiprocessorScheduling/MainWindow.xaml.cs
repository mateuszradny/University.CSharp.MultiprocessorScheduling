using OK.MultiprocessorScheduling.Logics;
using OK.MultiprocessorScheduling.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;

namespace OK.MultiprocessorScheduling
{
    public partial class MainWindow : Window
    {
        private readonly object locker = new object();
        private SchedulingProblemViewModel schedulingProblemViewModel = new SchedulingProblemViewModel();

        public MainWindow()
        {
            InitializeComponent();
            this.Algorithms.ItemsSource = ResolverManager.Resolvers;
            this.SchedulingProblemInput.DataContext = schedulingProblemViewModel;
        }

        private void Resolve_Click(object sender, RoutedEventArgs e)
        {
            this.ResolveButton.Content = "Trwa obliczanie...";
            this.ResolveButton.IsEnabled = false;
            this.Algorithms.IsEnabled = false;
            this.SchedulingProblemInput.IsEnabled = false;

            SchedulingProblem problem;
            if (this.InputMode.SelectedIndex == 0)
            {
                problem = SchedulingProblem.Generate(
                    schedulingProblemViewModel.ProcessorCount,
                    schedulingProblemViewModel.TaskCount,
                    schedulingProblemViewModel.MinDuration,
                    schedulingProblemViewModel.MaxDuration);
            }
            else
            {
                string text = new TextRange(this.TasksRichTextBox.Document.ContentStart, this.TasksRichTextBox.Document.ContentEnd).Text;
                problem = new SchedulingProblem(schedulingProblemViewModel.ProcessorCount, text.Split(new char[0], StringSplitOptions.RemoveEmptyEntries).Select(t => int.Parse(t)).ToArray());
            }

            var tasks = new List<System.Threading.Tasks.Task>();
            foreach (var resolver in this.Algorithms.SelectedItems.Cast<ISchedulingProblemResolver>())
                tasks.Add(System.Threading.Tasks.Task.Run(() => resolver.Resolve(problem)).ContinueWith(task => { lock (locker) this.Algorithms.Items.Refresh(); }, TaskScheduler.FromCurrentSynchronizationContext()));

            System.Threading.Tasks.Task.Factory.ContinueWhenAll(tasks.ToArray(), _tasks =>
            {
                this.ResolveButton.Content = "Uruchom!";
                this.ResolveButton.IsEnabled = true;
                this.Algorithms.IsEnabled = true;
                this.SchedulingProblemInput.IsEnabled = true;
            }, CancellationToken.None, TaskContinuationOptions.None, TaskScheduler.FromCurrentSynchronizationContext());
        }

        private void InputMode_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            bool isRandomMode = this.InputMode.SelectedIndex == 0;

            try
            {
                this.TaskCountLabel.Visibility = isRandomMode ? Visibility.Visible : Visibility.Hidden;
                this.TaskCountTextBox.Visibility = isRandomMode ? Visibility.Visible : Visibility.Hidden;
                this.MinDurationLabel.Visibility = isRandomMode ? Visibility.Visible : Visibility.Hidden;
                this.MinDurationTextBox.Visibility = isRandomMode ? Visibility.Visible : Visibility.Hidden;
                this.MaxDurationLabel.Visibility = isRandomMode ? Visibility.Visible : Visibility.Hidden;
                this.MaxDurationTextBox.Visibility = isRandomMode ? Visibility.Visible : Visibility.Hidden;
                this.TasksLabel.Visibility = !isRandomMode ? Visibility.Visible : Visibility.Hidden;
                this.TasksRichTextBox.Visibility = !isRandomMode ? Visibility.Visible : Visibility.Hidden;
            }
            catch
            { }
        }
    }

    internal class SchedulingProblemViewModel : INotifyPropertyChanged
    {
        private int maxDuration;
        private int minDuration;
        private int processorCount;
        private int taskCount;

        public event PropertyChangedEventHandler PropertyChanged;

        public int MaxDuration
        {
            get { return this.maxDuration; }
            set
            {
                this.maxDuration = value;
                this.OnPropertyChanged(nameof(MaxDuration));
            }
        }

        public int MinDuration
        {
            get { return this.minDuration; }
            set
            {
                this.minDuration = value;
                this.OnPropertyChanged(nameof(MinDuration));
            }
        }

        public int ProcessorCount
        {
            get { return this.processorCount; }
            set
            {
                this.processorCount = value;
                this.OnPropertyChanged(nameof(ProcessorCount));
            }
        }

        public int TaskCount
        {
            get { return this.taskCount; }
            set
            {
                this.taskCount = value;
                this.OnPropertyChanged(nameof(TaskCount));
            }
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            var eventHandler = this.PropertyChanged;
            if (eventHandler != null)
                eventHandler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}