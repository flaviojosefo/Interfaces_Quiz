using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

namespace MathQuiz {

    public class Quiz : MonoBehaviour {

        [Header("UI Elements")]
        public Button startButton;
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

        [Header("Exit Window Elements")]
        public GameObject exitWindow;
        public GameObject noButton;

        [Header("Endscreen Elements")]
        public Color goodColor;
        public Color okColor;
        public Color badColor;
        public Image background;
        public TMP_Text grade;
        public TMP_Text report;

        [Header("Question Groups")]
        public List<Group> groups;

        [Header("Help Buttons")]
        public List<GameObject> aidButtons;

        [Header("Navigation System")]
        public EventSystem eventSys;

        private List<Question> questions;
        private List<int> wrongQuestions;

        private int currentQuestion;

        private int totalAnswers;
        private int totalCorrectAnwers;

        private Button middleButton;
        private GameObject lastButton;

        void Start() {

            totalAnswers = 0;
            totalCorrectAnwers = 0;

            ResetTimer();
        }

        void Update() {

            EnableExit();

            UpdateSlider();
        }

        private void ResetTimer() {

            _currentSliderTime = originalSliderTime;
            slider.fillAmount = 0;
            sliderTime.text = originalSliderTime.ToString();
        }

        private void UpdateSlider() {

            if (!exitWindow.activeSelf) {

                if (_currentSliderTime > 0 && quiz.activeSelf) {

                    _currentSliderTime -= Time.deltaTime;
                    slider.fillAmount += Time.deltaTime / originalSliderTime;
                    sliderTime.text = ((int)_currentSliderTime).ToString();

                } else {

                    EndQuiz(false); // for now?
                }
            }
        }

        private void EnableExit() {

            if (Input.GetButtonDown("Cancel")) {

                if (!exitWindow.activeSelf) {

                    EnableExitWindow(true);

                } else {

                    EnableExitWindow(false);
                }
            }
        }


        // Also used on a button
        public void EnableExitWindow(bool on) {

            if (on) {

                lastButton = eventSys.currentSelectedGameObject;
                exitWindow.SetActive(true);
                eventSys.SetSelectedGameObject(noButton);

            } else {

                exitWindow.SetActive(false);
                eventSys.SetSelectedGameObject(lastButton);
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

            currentQuestion = 0;

            GetRandomGroup();

            GetAnswers(GetQuestion(currentQuestion));
            ActivateHelp();
            ResetTimer();
            quiz.SetActive(true);
        }

        // 'public' to use in button if the person wants to exit
        public void EndQuiz(bool toMenu) {

            endGame.SetActive(false);
            quiz.SetActive(false);
            exitWindow.SetActive(false);

            mainMenu.SetActive(true);

            if (toMenu) {

                startButton.animator.SetTrigger("Normal");
                eventSys.SetSelectedGameObject(startButton.gameObject);
            }
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

        private void GetRandomGroup() {

            questions = groups[Random.Range(0, 3)].questions;
        }

        public int GetQuestion(int next) {

            if (next < questions.Count) {

                question.text = questions[next].question;
                questionImg.sprite = questions[next].questionImage;

                currentQuestion = next;

                return next;

            } else {

                ShowEndScreen();
                return 0;
            }
        }

        private void GetAnswers(int questionID) {

            DeleteAllButtons();
            
            for (int i = 1; i <= 3; i++) {

                Button b = Instantiate(answerButton, answersParent);
                b.transform.GetChild(0).GetComponent<TMP_Text>().text = 
                    (string)questions[questionID].GetType().GetField($"answer{i}").GetValue(questions[questionID]);

                int tempI = i;
                b.onClick.AddListener(delegate { VerifyAnswer(questionID, tempI); GetAnswers(GetQuestion(currentQuestion += 1)); ResetTimer(); });

                if (i == 1) {

                    middleButton = b;
                }
            }

            StartCoroutine(SelectButton());
            eventSys.SetSelectedGameObject(middleButton.gameObject);
        }

        // Play button animation 1 frame after its creation
        // since it's inactive when created
        private IEnumerator SelectButton() {

            // wait 1 frame
            yield return 0;

            middleButton.animator.Play("Selected");
        }

        private void VerifyAnswer(int questionID, int answerID) {

            if (questions[questionID].answer == answerID) {

                totalAnswers += 1;
                totalCorrectAnwers += 1;
                return;
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

            if (totalCorrectAnwers <= 3) {

                grade.text = "MAU";
                background.color = badColor;

            } else if (totalCorrectAnwers <= 7) {

                grade.text = "BOM";
                background.color = okColor;

            } else {

                grade.text = "MUITO BOM";
                background.color = goodColor;
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

                    report.text += $"Erraste a pergunta {i + 1}. \n";
                }
            }
        }

        private void FiftyFifty() {

            int n;

            do {

                n = Random.Range(1, 4);

            } while (questions[currentQuestion].answer == n);

            Destroy(answersParent.GetChild(n - 1).gameObject);
        }

        public void UseHelp(int helpType) {

            switch (helpType) {

                case 0:

                    FiftyFifty();
                    aidButtons[helpType].SetActive(false);
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

        private void ActivateHelp() {

            foreach (GameObject g in aidButtons) {

                g.SetActive(true);
            }
        }
    }
}
