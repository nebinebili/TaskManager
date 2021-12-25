using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using WpfApp7.Command;
using WpfApp7.Views;

namespace WpfApp7.ViewModels
{
    public class MainViewModel
    {
        public ObservableCollection<Process> Processes { get; set; }
        public ObservableCollection<Process> BlackList { get; set; } = new ObservableCollection<Process>();
        DispatcherTimer timer;
        MainView mainView;

        public RelayCommand CreateTaskCommand { get; set; }
        public RelayCommand EndTaskCommand { get; set; }
        public RelayCommand AddBlackListCommand { get; set; }

        public MainViewModel(MainView _mainView)
        {
           

            mainView = _mainView;

            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(3);
            timer.Tick += timer_Tick;
            timer.Start();

            CreateTaskCommand = new RelayCommand
                (
                  act => BtnCreateTask_Click(),
                  pre => true
                );

            EndTaskCommand = new RelayCommand
                (
                  act => BtnEndTask_Click(),
                  pre => true
                );
            AddBlackListCommand = new RelayCommand
                (
                  act => BtnAddBlackList_Click(),
                  pre => true
                );
        }

        public void BtnAddBlackList_Click()
        {
            try
            {
                if (mainView.listview_process.SelectedIndex == -1)
                {
                    MessageBox.Show("Task not selected!");
                }
                else
                {
                    Process process = mainView.listview_process.SelectedItem as Process;
                    BlackList.Add(process);
                    Thread.Sleep(2000);
                    process.Kill();
                    Processes.Remove(process);
                    mainView.txb_process.Text = null;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            
        }

        public void timer_Tick(object sender, EventArgs e)
        {
            Processes = new ObservableCollection<Process>(Process.GetProcesses());
            mainView.listview_process.ItemsSource = Processes;
        }

        public void BtnEndTask_Click()
        {
            try
            {
                if (mainView.listview_process.SelectedIndex == -1)
                {
                    MessageBox.Show("Task not selected!");
                }
                else
                {
                    var p = mainView.listview_process.SelectedItem as Process;
                    p.Kill();
                    Process process = mainView.listview_process.SelectedItem as Process;
                    Processes.Remove(process);
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            
        }

        public void BtnCreateTask_Click()
        {

            try
            {

                if (!string.IsNullOrEmpty(mainView. txb_process.Text))
                {
                    if (!BlackList.Any(pro => pro.ProcessName == mainView.txb_process.Text))
                    {
                        Process p = new Process();
                        p.StartInfo.FileName = $"{mainView.txb_process.Text}";
                        p.StartInfo.Arguments = " ";
                        p.Start();
                        Processes.Add(p);
                        mainView.txb_process.Text = null;
                    }
                    else
                    {
                        MessageBox.Show("Task in BlackList !");
                    }
                }
                else
                {
                    MessageBox.Show("Task Name is Empty");
                }


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            
        }

    }
}
