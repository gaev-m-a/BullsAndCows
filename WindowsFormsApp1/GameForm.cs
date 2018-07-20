using System;
using System.Linq;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class GameForm : Form
    {
        private Random rand = new Random();
        private char[] generatedNumber; // загаданное число
        private char[] userGuess; // число, введённое пользователем
        private int bulls;
        private int cows;
        private const string separator = "\r\n~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~\r\n";
        private const string rules =                 
                "Ваша задача: отгадать четырёхзначное число, задуманное компьютером.\r\n" +
                "Цифры в числе не повторяются.Если цифра из названного числа есть в\r\n" +
                "отгадываемом числе, то эта ситуация называется «корова». Если цифра\r\n" +
                "из названного числа есть в отгадываемом числе и стоит в том же месте,\r\n" +
                "то эта ситуация называется «бык».\r\n";

        public GameForm()
        {
            InitializeComponent();
            outputTextBox.Text = rules;
            generateNewNumber();
        }
        private void generateNewNumber()
        {
            int[] digits = new int[4];
            bool arrayContainsThisDigit;
            for (int i = 0; i < 4; i++)
            {
                // случайная генерация каждой цифры и проверка того, что она ещё не использовалась в числе
                do
                {
                    arrayContainsThisDigit = false;
                    digits[i] = rand.Next(10);
                    for (int k = 0; k < i; k++)
                        if (digits[k] == digits[i])
                            arrayContainsThisDigit = true;
                } while (arrayContainsThisDigit || digits[0] == 0); // проверка того, что в числе разные цифры и оно не начинается с нуля
            }
            generatedNumber = (digits[0].ToString() + digits[1] + digits[2] + digits[3]).ToCharArray(); // приведение числа к массиву символов
        }
        private void compareNumbers()
        {
            bulls = 0;
            cows = 0;

            for (int i = 0; i < 4; i++)
            {
                if (generatedNumber.Contains(userGuess[i]))
                {
                    if (generatedNumber[i] == userGuess[i])
                        bulls++;
                    else
                        cows++;
                }
            }
        }
        private bool inputIsValid()
        {
            if (userGuessTextBox.Text.Length != 4)
            {
                outputTextBox.Text += separator + "Некорректный запрос. Введенное число должно быть четырёхзначным.";
                return false;
            }
            else if (!userGuessTextBox.Text.All(char.IsDigit))
            {
                outputTextBox.Text += separator + "Некорректный запрос. Допускается вводить только цифры.";
                return false;
            }
            else
            {
                userGuess = userGuessTextBox.Text.ToCharArray();
                if (userGuess[0] == '0')
                {
                    outputTextBox.Text += separator + "Некорректный запрос. Число не должно начинаться с нуля.";
                    return false;
                }
                else
                {
                    for (int i = 0; i < 4; i++)
                        for (int k = 0; k < i; k++)
                            if (userGuess[k] == userGuess[i])
                            {
                                outputTextBox.Text += separator + "Некорректный запрос. Цифры в числе не должны повторяться.";
                                return false;
                            }
                    return true;
                }
            }
        }
        private void guessButton_Click(object sender, EventArgs e)
        {
            if (inputIsValid())
            {
                compareNumbers();
                if (bulls == 4)
                {
                    // блокировка возможности ввода новых значений в случае победы
                    guessButton.Enabled = false;
                    userGuessTextBox.Enabled = false;
                    outputTextBox.Text += separator + "Вы ввели: " + userGuessTextBox.Text + "\r\n\r\nПобеда! Вы отгадали число!";
                }
                else
                {
                    outputTextBox.Text += separator + "Вы ввели: " + userGuessTextBox.Text + "\r\n\r\nКоровы: " + cows + "\r\nБыки: " + bulls;
                }
            }
            userGuessTextBox.Text = "";
        }
        private void newGameButton_Click(object sender, EventArgs e)
        {
            // разблокировка возможности ввода новых значений
            guessButton.Enabled = true;
            userGuessTextBox.Enabled = true;
            userGuessTextBox.Focus();
            // очишение выведенной ранее информации и вывод правил игры
            outputTextBox.Text = rules;
            // генерация нового числа
            generateNewNumber();
        }
        
        private void outputTextBox_TextChanged(object sender, EventArgs e)
        {
            // автоматическое пролистывание textbox'а вывода вниз
            outputTextBox.SelectionStart = outputTextBox.Text.Length;
            outputTextBox.ScrollToCaret();
            outputTextBox.Refresh();
        }
    }
}