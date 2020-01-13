using UnityEngine;

namespace MathQuiz {

    [CreateAssetMenu(fileName = "Question", menuName = "ScriptableObject/Question")]
    public class Question : ScriptableObject {

        [Header("Question Settings")]
        [TextArea(3, 5)]
        public string question;

        public Sprite questionImage;

        [Header("Answers")]
        public string answer1;
        public string answer2;
        public string answer3;

        [Header("Correct Answer")]
        public int answer;
    }
}
