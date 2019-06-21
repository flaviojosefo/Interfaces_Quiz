using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

namespace MathQuiz {

    public class Quiz : MonoBehaviour {

        [Header("UI Elements")]
        public GameObject mainMenu;
        public GameObject quiz;
        public GameObject endGame;

        [Header("Slider")]
        public float originalSliderTime;
        public Image slider;
        public TMP_Text sliderTime;
        private float _currentSliderTime;

        [Header("Q&A Elements")]
        public TMP_Text question;
        public Image questionImg;
        public Button answerButton;
        public Transform answersParent;

        [Header("Endscreen Elements")]
        public Image background;
        public TMP_Text grade;
        public TMP_Text report;

        [Header("Questions")]
        public List<Questions> questions;

        [Header("Aid Buttons")]
        public List<GameObject> aidButtons;

        // Already asked questions
        private List<int> aaQuestions;

        private List<int> wrongQuestions;

        private int currentQuestion;

        private int totalAnswers;
        private int totalCorrectAnwers;

        void Start() {

            totalAnswers = 0;
            totalCorrectAnwers = 0;

            ResetTimer();
        }

        void Update() {

            UpdateSlider();
        }

        private void ResetTimer() {

            _currentSliderTime = originalSliderTime;
            slider.fillAmount = 0;
            sliderTime.text = originalSliderTime.ToString();
        }

        private void UpdateSlider() {

            if (_currentSliderTime > 0 && quiz.activeSelf) {

                _currentSliderTime -= Time.deltaTime;
                slider.fillAmount += Time.deltaTime / originalSliderTime;
                sliderTime.text = ((int)_currentSliderTime).ToString();

            } else {

                EndQuiz(); // for now?
            }
        }

        public void EnableMenu() {

            mainMenu.SetActive(true);
        }

        public void DisableMenu() {

            mainMenu.SetActive(false);
            StartQuiz();
        }

        private void StartQuiz() {

            wrongQuestions = new List<int>();

            totalAnswers = 0;
            totalCorrectAnwers = 0;

            GetAnswers(GetQuestion());
            ResetTimer();
            quiz.SetActive(true);
        }

        // 'public' to use in button if the person wants to exit
        public void EndQuiz() {

            aaQuestions = new List<int>();

            endGame.SetActive(false);
            quiz.SetActive(false);
            mainMenu.SetActive(true);
        }

        public void Quit() {

            if (SceneManager.sceneCount > 1) {

                print("Yes, it went back.");
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);

            } else {

                print("Yes, it quit.");
                Application.Quit();
            }
        }

        public int GetQuestion() {

            if (aaQuestions == null) aaQuestions = new List<int>();

            int n = Random.Range(0, questions.Count);

            int i = 0;

            while (aaQuestions.Contains(n) && i < 1000) {

                n = Random.Range(0, questions.Count);
                i++;
            }

            if (i >= 1000) ShowEndScreen();

            question.text = questions[n].question;
            questionImg.sprite = questions[n].questionImage;
            aaQuestions.Add(n);

            currentQuestion = n;

            return n;
        }

        private void GetAnswers(int questionID) {

            DeleteAllButtons();
            
            for (int i = 0; i < questions[questionID].answers.Count; i++) {

                Button b = Instantiate(answerButton, answersParent);
                b.transform.GetChild(0).GetComponent<TMP_Text>().text = questions[questionID].answers[i].answer;

                int tempI = i;
                b.onClick.AddListener(delegate { VerifyAnswer(questionID, tempI); GetAnswers(GetQuestion()); ResetTimer(); });
            }
        }

        private void VerifyAnswer(int questionID, int answerID) {

            for (int i = 0; i < questions[questionID].answers.Count; i++) {

                if (i == answerID && questions[questionID].answers[answerID].isCorrect) {
                    
                    totalAnswers += 1;
                    totalCorrectAnwers += 1;
                    return;
                }
            }

            wrongQuestions.Add(questionID);
            totalAnswers += 1;
        }

        private void DeleteAllButtons() {

            foreach (Transform button in answersParent) {

                Destroy(button.gameObject);
            }
        }

        private void ShowEndScreen() {

            ShowIncorrectAnswers();

            if (totalCorrectAnwers <= 2) {

                grade.text = "MAU";
                background.color = Color.red;

            } else if (totalCorrectAnwers <= 5) {

                grade.text = "BOM";
                background.color = Color.yellow;

            } else {

                grade.text = "MUITO BOM";
                background.color = Color.green;
            }

            endGame.SetActive(true);
        }

        private void ShowIncorrectAnswers() {

            if (totalCorrectAnwers == questions.Count) {

                report.text = "Acertaste todas as perguntas! Parabéns!";
                return;

            } else {

                report.text = $"Acertaste {totalCorrectAnwers} em {totalAnswers} perguntas! \n\n";

                foreach (int i in wrongQuestions) {

                    report.text += $"Erraste a pergunta {i+1}. \n";
                }
            }
        }

        private void FiftyFifty() {

            int n = 0;

            for (int i = 0; i < questions[currentQuestion].answers.Count; i++) {

                if (!questions[currentQuestion].answers[i].isCorrect && n < questions[currentQuestion].answers.Count/2) {

                    Destroy(answersParent.GetChild(i).gameObject);
                    n++;
                }
            }
        }

        public void UseAid(int helpType) {

            switch (helpType) {

                case 0:

                    FiftyFifty();
                    Destroy(aidButtons[0]);
                    break;

                case 1:

                    // show percentage of most likely question
                    // 'high percentage to give a higher percentage to the correct answer' and distribute the rest with the others
                    break;

                case 2:

                    // phone a friend
                    // high percentage of showing most likely answer (>= 95%)
                    // ...or just give the answer?
                    break;

                case 3:

                    // change answer
                    break;
            }
        }
    }
}
