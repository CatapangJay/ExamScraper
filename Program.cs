using ExamScraper;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OpenQA.Selenium;
using OpenQA.Selenium.Edge;
using static System.Runtime.InteropServices.JavaScript.JSType;

IWebDriver driver = new EdgeDriver("C:\\Projects\\ExamScraper\\assets\\msedgedriver.exe");

// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");

List<QuestionInfo> qList = new();
int qId = 0;
// Loop through each pages
for (int i = 0; i < 10; i++)
{
    Console.WriteLine($"Scraping page {i + 1}...");
    driver.Url = $"https://www.examtopics.com/exams/google/associate-cloud-engineer/view/{i + 1}/";

    var questionCards = driver.FindElements(By.ClassName("exam-question-card"));

    while (questionCards.Count == 0)
    {
        Console.WriteLine("CAPTCHA detected. Please resolve it and press any key to continue...");

        Thread.Sleep(new Random().Next(1000, 5000));
        questionCards = driver.FindElements(By.ClassName("exam-question-card"));
    }
    //if (questionCards == null || questionCards.Count == 0) {
    //    Console.WriteLine("CAPTCHA detected. Please resolve it and press any key to continue...");
    //    Console.ReadKey(); // Pauses execution until a key is pressed

    //    questionCards = driver.FindElements(By.ClassName("exam-question-card"));
    //}

    foreach (var card in questionCards)
    {
        var paragraph = card.FindElement(By.TagName("p"));
        QuestionInfo questionInfo = new()
        {
            id = qId++,
            type = "SingleSelect",
            text = paragraph.Text,
        };

        var choices = card.FindElements(By.ClassName("multi-choice-item"));
        var hasMostVotedBadge = choices.Any(c => c.FindElements(By.CssSelector("span.most-voted-answer-badge")).Count > 0);

        foreach (var choice in choices)
        {
            var isCorrect = false;

            if (!hasMostVotedBadge)
            {
                isCorrect = choice.GetAttribute("class").Contains("correct-choice");
            }
            else
            {
                isCorrect = choice.FindElements(By.CssSelector("span.most-voted-answer-badge")).Count > 0;
            }

            questionInfo.answers.Add(new ChoiceInfo { text = choice.Text.Remove(0, 3), correct = isCorrect });
        }

        qList.Add(questionInfo);
    }
    Thread.Sleep(new Random().Next(1000, 5000));

    Console.WriteLine($"Finished scraping page {i + 1}");
}

string qJson = JsonConvert.SerializeObject(qList, Formatting.Indented);
Console.ReadKey();