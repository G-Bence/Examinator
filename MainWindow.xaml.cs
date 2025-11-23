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
using System.IO;
using Microsoft.VisualBasic.FileIO;

namespace Examinator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        List<Questions> questions = new List<Questions>();
        List<Questions> usedQuestions = new List<Questions>();

        List<RadioButton> answerOptions;

        Random random = new Random();

        int questionCounter = 0;
        int scoreCounter = 0;

        string FileName = "QuestionsFile.txt";
        string ResultsFileName = "ResultsFile.txt";
        string FullName = "User";

        Questions choosenOne;

        public MainWindow()
        {
            InitializeComponent();
            ReadInFile(FileName);

            answerOptions = new List<RadioButton>() {AnswerOp_1, AnswerOp_2, AnswerOp_3, AnswerOp_4};

            GiveQuestion();
        }

        private void ReadInFile(string fileName)
        {
            foreach (var item in File.ReadAllLines(fileName))
            {
                var slice = item.Split(";");
                string question = slice[0];
                List<string> answers = new List<string>();

                for(int i = 1; i <  slice.Length-1; i++)
                {
                    answers.Add(slice[i]);
                }
                string correctAnswer = slice[slice.Length-1];

                Questions newQuestion = new Questions(question, answers, correctAnswer);
                questions.Add(newQuestion);

            }
        }


        private void GiveQuestion()
        {
            int qIndex;

            NextTask.Visibility = Visibility.Collapsed;
            SCorrectAnswer.Visibility = Visibility.Visible;

            foreach (RadioButton option in answerOptions)
            {
                option.Foreground = Brushes.Black;
                    option.IsChecked = false;
            }
            
            List<string> answerList = new List<string>();

            do
            {
                qIndex = random.Next(0, questions.Count);

                choosenOne = questions[qIndex];

            } while (usedQuestions.Contains(choosenOne));

            questionCounter++;
            
            QCounterTB.Text = $"Question {questionCounter}";
            SCounterTB.Text = $"- {scoreCounter} / 10 -";

            QuestionTB.Text = choosenOne.Question.ToString();

            AnswerOp_1.Content = $"A; {choosenOne.Answer[0]}";
            AnswerOp_2.Content = $"B; {choosenOne.Answer[1]}";
            AnswerOp_3.Content = $"C; {choosenOne.Answer[2]}";
            AnswerOp_4.Content = $"D; {choosenOne.Answer[3]}";
        }


        private void CheckResults()
        {

            foreach (RadioButton option in answerOptions)
            {
                if (option.Content.ToString().Contains(choosenOne.CorrectAnswer.ToString()))    
                {
                    //choosenAnswer = option; 
                    option.Foreground = Brushes.Green;
                    if (option.IsChecked == true)
                    {
                        scoreCounter++;
                    }

                }
                else if (option.IsChecked == true)
                {
                    option.Foreground = Brushes.Red;
                }
            }
        }



        
        private void ContinueButton_Click (object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(NameInput.Text.ToString()))
            {
                FullName = NameInput.Text;
                IntroBody.Visibility = Visibility.Collapsed;
                GameBody.Visibility = Visibility.Visible;
            }
            else
            {
                WarningText.Opacity = 1;
            }
        }

        private void NameInput_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (WarningText != null && WarningText.Opacity != 0)
            {
                WarningText.Opacity = 0;
            }
        }

        private void SCorrectAnswerButton_Click(object sender, RoutedEventArgs e)
        {
            bool isChoosen = false;
            foreach (RadioButton option in answerOptions)
            {
                if (option.IsChecked == true)
                {
                    isChoosen = true;
                }
            }

            if (isChoosen)
            {
                AnswerWarningText.Opacity = 0;
                CheckResults();
                SCounterTB.Text = $"- {scoreCounter} / 10 -";

                SCorrectAnswer.Visibility = Visibility.Collapsed;
                NextTask.Visibility = Visibility.Visible;
            }
            else
            {
                AnswerWarningText.Opacity = 1;
            }

        }

        private void NextTask_Click(object sender, RoutedEventArgs e)
        {
            if(questionCounter < 10)
            {
                GiveQuestion();
            }

            else {
                GameBody.Visibility = Visibility.Collapsed;
                ResultsBody.Visibility = Visibility.Visible;

                ResultsTB.Text = $"- {scoreCounter} / 10 -";
                try
                {
                    using (StreamWriter writer = new StreamWriter(ResultsFileName, true, System.Text.Encoding.UTF8))
                    {
                        writer.WriteLine($"{FullName}: \t{scoreCounter} / 10");
                    }
                }
                catch (Exception ex) {
                    FileWritenTB.Text = ex.Message;
                }
                
                FileWritenTB.Text = $"Results are succesfully saved to {FileName}";
            }
        }
    }
}