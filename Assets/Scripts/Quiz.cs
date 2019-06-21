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
        
        [Header("Slider")]
        public float originalSliderTime;
        public Image slider;
        public TMP_Text sliderTime;
        private float _currentSliderTime;

        [Header("Q&A Elements")]
        public TMP_Text question;
        public Button answerButton;
        public Transform answersParent;

        [Header("Questions")]
        public List<Questions> questions;

        // Already asked questions
        private List<int> aaQuestions;

        void Start() {

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

            GetQuestion();
            GetAnswers(0);
            ResetTimer();
            quiz.SetActive(true);
        }

        // 'public' to use in button if the person wants to exit
        public void EndQuiz() {

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

            if (!aaQuestions.Contains(n)) {

                question.text = questions[n].question;
                aaQuestions.Add(n);
                return n;
            }

            return n;
        }

        private void GetAnswers(int questionID) {

            DeleteAllButtons();

            // always 4 buttons?
            for (int i = 0; i < 4; i++) {

                Button b = Instantiate(answerButton, answersParent);
                b.onClick.AddListener(delegate { GetAnswers(GetQuestion()); ResetTimer(); });
            }
        }

        private void DeleteAllButtons() {

            foreach (Transform button in answersParent) {
                
                Destroy(button.gameObject);
            }
        }

        public void UseAid(int helpType) {

            switch (helpType) {

                case 0:

                    // remove 2 wrong answers
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
