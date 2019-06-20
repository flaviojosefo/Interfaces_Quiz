using UnityEngine;
using UnityEngine.EventSystems;
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

        [Header("Buttons")]
        public Button startButton;

        void Start() {

            SetSliderDefaultValues();
        }

        void Update() {

            UpdateSlider();
        }

        private void CheckInput() {

            if (Input.GetKeyDown(KeyCode.DownArrow)) {

                startButton.Select();
            }
        }

        private void SetSliderDefaultValues() {
            
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

                // END QUIZ
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

            // GetQuestion();
            // GetAnswers();
            SetSliderDefaultValues();
            quiz.SetActive(true);
        }

        // 'public' to use in button if the person wants to exit
        public void EndQuiz() {


        }

        public void Quit() {

            print("Yes, it quit.");
            Application.Quit();
        }
    }
}
