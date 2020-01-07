using System;
using UnityEngine;

namespace MathQuiz {

    [Serializable]
    public class Answers {

        public bool isCorrect;

        [TextArea(3, 5)]
        public string answer;
    }
}
