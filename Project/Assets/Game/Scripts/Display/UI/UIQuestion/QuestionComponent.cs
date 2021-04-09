using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestionComponent : MonoBehaviour
{
    [SerializeField]private Text subject_text;
    [SerializeField]private Text question_text;
    // Start is called before the first frame update
    public void SetQuestion(string subject,string content)
    {
        subject_text.text = subject;
        question_text.text = content;
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
