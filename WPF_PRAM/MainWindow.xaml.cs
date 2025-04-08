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
            this.DataContext = this.pram;
            TextBox_Code.Document.Blocks.Clear();
            TextBox_Code.Document.Blocks.Add(new Paragraph(new Run("S0 := 0")));
            TextBox_Code.Document.Blocks.Add(new Paragraph(new Run(":cycle")));
            TextBox_Code.Document.Blocks.Add(new Paragraph(new Run("S0 := S0 + 1")));
            TextBox_Code.Document.Blocks.Add(new Paragraph(new Run("if S0 == 3 goto :end")));
            TextBox_Code.Document.Blocks.Add(new Paragraph(new Run("goto :cycle")));
            TextBox_Code.Document.Blocks.Add(new Paragraph(new Run(":end")));
            TextBox_Code.Document.Blocks.Add(new Paragraph(new Run("halt")));
            HighlightLine(0, Brushes.LightGreen);
            //TextBox_Code.Text = ":cycle\r\nS0 := S0 + 1\r\nif S0 == 3 goto :end\r\ngoto :cycle\r\n:end\r\nhalt";
        }
        private void Button_Next_Click(object sender, RoutedEventArgs e)
        {
            if(this.pram.Compiled)
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
                int lineNumber = pram.ParallelExecution ? pram.Processors[0].Program.instructions[pram.Processors[0].InstructionPointer].CodeLineIndex : pram.MainProgram.instructions[pram.InstructionPointer].CodeLineIndex;
                HighlightLine(lineNumber, Brushes.LightGreen);
            }
            else
            {
                MessageBox.Show("code not compiled");
            }
        }

        private void Button_Add_Click(object sender, RoutedEventArgs e)
        {
            pram.AddToSM();
        }
        private void Button_Remove_Click(object sender, RoutedEventArgs e)
        {
            pram.RemoveFromSM();
        }

        private void Button_Compile_Click(object sender, RoutedEventArgs e)
        {
            TextRange textRange = new TextRange(TextBox_Code.Document.ContentStart, TextBox_Code.Document.ContentEnd.GetPositionAtOffset(-1));
            string fullText = textRange.Text;
            string text = fullText.TrimEnd();
            pram.CodeCompiler.Compile(pram, text);
            MessageBox.Show("Compilation successful");
            pram.Compiled = true;
        }
        void HighlightLine(int lineNumber, Brush background)
        {
            var document = TextBox_Code.Document;
            var lines = new List<string>();

            TextRange fullText = new TextRange(document.ContentStart, document.ContentEnd);
            string[] allLines = fullText.Text.Split(new[] { "\r\n", "\n" }, StringSplitOptions.None);

            document.Blocks.Clear();

            for (int i = 0; i < allLines.Length; i++)
            {
                var paragraph = new Paragraph(new Run(allLines[i]));
                if (i == lineNumber)
                {
                    paragraph.Background = background;
                }
                document.Blocks.Add(paragraph);
            }
        }
    }
}
