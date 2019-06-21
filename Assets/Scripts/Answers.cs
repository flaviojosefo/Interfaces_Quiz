using UnityEngine;

namespace MathQuiz {

    [System.Serializable]
    public class Answers {

        public bool isCorrect;

        [TextArea(3, 5)]
        public string answer;
    }
}
