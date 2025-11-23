using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Examinator
{

    internal class Questions {
    
    string question;
    List<string> answers;
    string correctAnswer;
    
    


    public Questions(string question, List<string> answers, string correctAnswer)
    {
        this.question = question;
        this.answers = answers;
        this.correctAnswer = correctAnswer;
    }


    public string Question { get => question; set => question = value; }
    public List<string> Answer { get => answers; set => answers = value; }
    public string CorrectAnswer { get => correctAnswer; set => correctAnswer = value; }

    public override string ToString()
    {
        string answerString = string.Join("; ", this.answers);
        
        return $"{this.question},{answerString},{this.correctAnswer}";
    }


    }
}
