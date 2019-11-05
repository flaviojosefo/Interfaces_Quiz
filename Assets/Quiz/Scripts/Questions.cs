using System.Collections.Generic;
using UnityEngine;

namespace MathQuiz {
    
    [System.Serializable]
    public class Questions {

        [TextArea(3, 5)]
        public string question;

        // if there is one
        public Sprite questionImage;

        // Answers for the specific question
        public List<Answers> answers;
    }
}
