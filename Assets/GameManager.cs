using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class GameManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI firstButtonText, secondButtonText, lifeText, scoreText, bestScoreText;
    [SerializeField] private Image firstButtonImage, secondButtonImage;

    private Image _correctAnswerImage, _incorrectAnswerImage;

    [SerializeField] private List<string> passwords;

    private int _firstAnswer, _secondAnswer, _correctAnswer, _lifes = 3, _score = 0;

    private bool _canAnswer = true;

    private void Start()
    {
        Restart();
    }

    private void Restart()
    {
        if (_score > PlayerPrefs.GetInt("best score", 0))
        {
            PlayerPrefs.SetInt("best score", _score);
            bestScoreText.DOColor(new Color(1f, 0.9259405f, 0.409434f), 0.75f).OnComplete(() =>
                bestScoreText.DOColor(Color.white, 0.75f));
        }

        _lifes = 3;
        _score = 0;

        lifeText.text = "lifes: " + _lifes.ToString();
        scoreText.text = "score: " + _score.ToString();
        bestScoreText.text = "best score: " + PlayerPrefs.GetInt("best score", 0).ToString();

        Generate();
    }

    private void Generate()
    {
        _canAnswer = true;
        firstButtonImage.DOColor(Color.gray, 0.5f);
        secondButtonImage.DOColor(Color.gray, 0.5f);

        _firstAnswer = Random.Range(0, passwords.Count - 1);
        _secondAnswer = Random.Range(0, passwords.Count - 1);
        while (_secondAnswer == _firstAnswer) _secondAnswer = Random.Range(0, passwords.Count - 1);

        firstButtonText.text = passwords[_firstAnswer];
        secondButtonText.text = passwords[_secondAnswer];

        _correctAnswerImage = firstButtonImage;
        _incorrectAnswerImage = secondButtonImage;
        _correctAnswer = _firstAnswer;

        if (_firstAnswer > _secondAnswer)
        {
            _correctAnswerImage = secondButtonImage;
            _incorrectAnswerImage = firstButtonImage;
            _correctAnswer = _secondAnswer;
        }
    }

    public void FirstAnswer()
    {
        Answer(_firstAnswer);
    }

    public void SecondAnswer()
    {
        Answer(_secondAnswer);
    }

    private void Answer(int answer)
    {
        if (!_canAnswer) return;
        _canAnswer = false;
        if (answer != _correctAnswer) _lifes -= 1;
        else _score += 1;

        lifeText.text = "lifes: " + _lifes.ToString();
        scoreText.text = "score: " + _score.ToString();

        _correctAnswerImage.DOColor(new Color(0.2612047f, 0.735849f, 0.2457458f), 0.5f);
        _incorrectAnswerImage.DOColor(new Color(0.8705882f, 0.3568628f, 0.3568628f), 0.5f);

        if (_lifes <= 0) Invoke("Restart", 1f);
        else Invoke("Generate", 1f);
    }
}
