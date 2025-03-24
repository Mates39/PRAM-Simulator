using System;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Bakalarka;

namespace WPF_PRAM
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public PRAM pram { get; set; }
        public MainWindow()
        {
            InitializeComponent();
            this.pram = new PRAM();
            //pram.Program_Sum();
            pram.Run();
            this.DataContext = this.pram;
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (pram.ParallelExecution)
            {
                int i = pram.ExecuteNextParallelStep();
                if (i == -1)
                    MessageBox.Show("Access violation");
            }
            else
            {
                int i = pram.ExecuteNextInstruction();
                if (i == -1)
                    MessageBox.Show("Access violation");
                if (i == -2)
                    MessageBox.Show("Program has ended");
            }
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            pram.AddToSM();
        }

    }
}
