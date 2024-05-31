using SortingAlgo;
using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using System.IO;

namespace AlgorithmUI
{

    public partial class MainWindow : Window
    {
        private int[] array;
        private int[] arrayForSave;
        private Sort currentSort;

        #region Variables

        const int minElements = 100;
        const int maxElements = 50000;
        const int maxBars = 200;
        private int maxDiapason;
        private int minDiapason;
        private int sizeArray;
        private bool descending = false;
        private bool applyLimits = false;
        private bool sizeErrorCheck = false;
        private bool minErrorCheck = false;
        private bool maxErrorCheck = false;
        private bool enterArrTooSmallErrorCheck = false;
        enum AlgorithmType { Блочне, Підрахунком, Порозрядне }

        #endregion

        public MainWindow()
        {
            InitializeComponent();
        }

        private void SortingAlgorithmComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedItem = (SortingAlgorithmComboBox.SelectedItem as System.Windows.Controls.ComboBoxItem).Content.ToString();
            switch (selectedItem)
            {
                case "Впорядкований":
                    SizeArrayLabel.Visibility = Visibility.Visible;
                    SizeArrLabel.Visibility = Visibility.Visible;
                    SizeTextBox.Visibility = Visibility.Visible;
                    SizeErrorBtn.Visibility = Visibility.Visible;
                    DiapasonLabel.Visibility = Visibility.Visible;
                    MinDiapasonLabel.Visibility = Visibility.Visible;
                    MaxDiapasonLabel.Visibility = Visibility.Visible;
                    MinDiapasonTextBox.Visibility = Visibility.Visible;
                    MaxDiapasonTextBox.Visibility = Visibility.Visible;
                    GenerateBttn.Visibility = Visibility.Visible;
                    VisualiseCheckBox.Visibility = Visibility.Visible;
                    ArrayLabel.Visibility = Visibility.Collapsed;
                    TextBoxInput.Visibility = Visibility.Collapsed;
                    DiapasonErrorLbl.Visibility = Visibility.Collapsed;
                    addEnteredArr.Visibility = Visibility.Collapsed;
                    break;
                case "Зворотно-впорядкований":
                    SizeArrayLabel.Visibility = Visibility.Visible;
                    SizeArrLabel.Visibility = Visibility.Visible;
                    SizeTextBox.Visibility = Visibility.Visible;
                    SizeErrorBtn.Visibility = Visibility.Visible;
                    DiapasonLabel.Visibility = Visibility.Visible;
                    MinDiapasonLabel.Visibility = Visibility.Visible;
                    MaxDiapasonLabel.Visibility = Visibility.Visible;
                    MinDiapasonTextBox.Visibility = Visibility.Visible;
                    MaxDiapasonTextBox.Visibility = Visibility.Visible;
                    GenerateBttn.Visibility = Visibility.Visible;
                    VisualiseCheckBox.Visibility = Visibility.Visible;
                    ArrayLabel.Visibility = Visibility.Collapsed;
                    TextBoxInput.Visibility = Visibility.Collapsed;
                    DiapasonErrorLbl.Visibility = Visibility.Collapsed;
                    addEnteredArr.Visibility = Visibility.Collapsed;
                    break;
                case "Невпорядкований":
                    SizeArrayLabel.Visibility = Visibility.Visible;
                    SizeArrLabel.Visibility = Visibility.Visible;
                    SizeTextBox.Visibility = Visibility.Visible;
                    SizeErrorBtn.Visibility = Visibility.Visible;
                    DiapasonLabel.Visibility = Visibility.Visible;
                    MinDiapasonLabel.Visibility = Visibility.Visible;
                    MaxDiapasonLabel.Visibility = Visibility.Visible;
                    MinDiapasonTextBox.Visibility = Visibility.Visible;
                    MaxDiapasonTextBox.Visibility = Visibility.Visible;
                    GenerateBttn.Visibility = Visibility.Visible;
                    VisualiseCheckBox.Visibility = Visibility.Visible;
                    ArrayLabel.Visibility = Visibility.Collapsed;
                    TextBoxInput.Visibility = Visibility.Collapsed;
                    DiapasonErrorLbl.Visibility = Visibility.Collapsed;
                    addEnteredArr.Visibility = Visibility.Collapsed;
                    break;
                case "Вручну":
                    SizeArrayLabel.Visibility = Visibility.Collapsed;
                    SizeArrLabel.Visibility = Visibility.Collapsed;
                    SizeTextBox.Visibility = Visibility.Collapsed;
                    SizeErrorBtn.Visibility = Visibility.Collapsed;
                    DiapasonLabel.Visibility = Visibility.Collapsed;
                    MinDiapasonLabel.Visibility = Visibility.Collapsed;
                    MaxDiapasonLabel.Visibility = Visibility.Collapsed;
                    MinDiapasonTextBox.Visibility = Visibility.Collapsed;
                    MaxDiapasonTextBox.Visibility = Visibility.Collapsed;
                    GenerateBttn.Visibility = Visibility.Collapsed;
                    VisualiseCheckBox.Visibility = Visibility.Visible;
                    ArrayLabel.Visibility = Visibility.Visible;
                    TextBoxInput.Visibility = Visibility.Visible;
                    DiapasonErrorLbl.Visibility = Visibility.Collapsed;
                    SortBtn.Visibility = Visibility.Visible;
                    ComplexityGroupBox.Visibility = Visibility.Visible;
                    addEnteredArr.Visibility = Visibility.Visible;
                    addEnteredArr.Visibility = Visibility.Visible;
                    ArrayLabel.Content = "Введіть масив через кому та пробіл:";
                    break;
                default:
                    break;
            }
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            applyLimits = true;
            SizeArrLabel.Content = "Від 100 до 200";
            ArrayLabel1.Visibility = Visibility.Collapsed;
            ArrayTextBlock.Visibility = Visibility.Collapsed;
            SortedArrayLabel.Visibility = Visibility.Collapsed;
            SortedArrTextBlock1.Visibility = Visibility.Collapsed;
            DescendingCheckBox.Visibility = Visibility.Collapsed;
            ValidateInput();
        }

        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            applyLimits = false;
            SizeArrLabel.Content = "Від 100 до 50000";
            DiapasonErrorLbl.Visibility = Visibility.Collapsed;
            Canvas.Visibility = Visibility.Collapsed;
            DescendingCheckBox.Visibility = Visibility.Visible;
        }

        private void ValidateInput()
        {
            int maxDiapason;

            if (applyLimits)
            {
                bool isMinDiapasonValid = int.TryParse(MinDiapasonTextBox.Text, out minDiapason);
                bool isMaxDiapasonValid = int.TryParse(MaxDiapasonTextBox.Text, out maxDiapason);

                if (isMinDiapasonValid && isMaxDiapasonValid)
                {
                    if (minDiapason < 0 || maxDiapason < 0)
                    {
                        DiapasonErrorLbl.Visibility = Visibility.Visible;
                        DiapasonErrorLbl.Content = "Неможливо візуалізувати негативні значення.";
                    }
                }
                else
                {
                    DiapasonErrorLbl.Visibility = Visibility.Collapsed;

                }
            }

        }
        private void MinDiapasonTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

            bool isMinDiapasonValid = int.TryParse(MinDiapasonTextBox.Text, out int minDiapason);
            bool isMaxDiapasonValid = int.TryParse(MaxDiapasonTextBox.Text, out int maxDiapason);

            if (!isMinDiapasonValid)
            {
                DiapasonErrorLbl.Visibility = Visibility.Visible;
                DiapasonErrorLbl.Content = "Введене значення мінімального діапазону не є числом.";
                minErrorCheck = true;
            }
            else if (minDiapason < 0 && applyLimits)
            {
                DiapasonErrorLbl.Visibility = Visibility.Visible;
                DiapasonErrorLbl.Content = "Неможливо візуалізувати негативні значення для мінімального діапазону.";
                minErrorCheck = true;
            }
            else
            {
                minErrorCheck = false;
                DiapasonErrorLbl.Visibility = Visibility.Collapsed;
            }

            if (!isMaxDiapasonValid)
            {
                DiapasonErrorLbl.Visibility = Visibility.Visible;
                DiapasonErrorLbl.Content = "Введене значення максимального діапазону не є числом.";
                maxErrorCheck = true;
            }
            else if (maxDiapason < 0)
            {
                DiapasonErrorLbl.Visibility = Visibility.Visible;
                DiapasonErrorLbl.Content = "Неможливо візуалізувати негативні значення для максимального діапазону.";
                maxErrorCheck = true;
            }
            else
            {
                maxErrorCheck = false;
                DiapasonErrorLbl.Visibility = Visibility.Collapsed;
            }

        }

        private void MaxDiapasonTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            bool isMinDiapasonValid = int.TryParse(MinDiapasonTextBox.Text, out int minDiapason);
            bool isMaxDiapasonValid = int.TryParse(MaxDiapasonTextBox.Text, out int maxDiapason);

            if (!isMinDiapasonValid)
            {
                DiapasonErrorLbl.Visibility = Visibility.Visible;
                DiapasonErrorLbl.Content = "Введене значення мінімального діапазону не є числом.";
                minErrorCheck = true;
            }
            else if (minDiapason < 0 && applyLimits)
            {
                DiapasonErrorLbl.Visibility = Visibility.Visible;
                DiapasonErrorLbl.Content = "Неможливо візуалізувати негативні значення для мінімального діапазону.";
                minErrorCheck = true;
            }
            else
            {
                minErrorCheck = false;
                DiapasonErrorLbl.Visibility = Visibility.Collapsed;
            }

            if (!isMaxDiapasonValid)
            {
                DiapasonErrorLbl.Visibility = Visibility.Visible;
                DiapasonErrorLbl.Content = "Введене значення максимального діапазону не є числом.";
                maxErrorCheck = true;
            }
            else if (maxDiapason < 0)
            {
                DiapasonErrorLbl.Visibility = Visibility.Visible;
                DiapasonErrorLbl.Content = "Неможливо візуалізувати негативні значення для максимального діапазону.";
                maxErrorCheck = true;
            }
            else
            {
                maxErrorCheck = false;
                DiapasonErrorLbl.Visibility = Visibility.Collapsed;
            }
           
        }

        private void SizeErrorBtn_Click_1(object sender, RoutedEventArgs e)
        {
            string input = SizeTextBox.Text;
            try
            {
                int size = int.Parse(input);
                if (applyLimits)
                {
                    if (size < minElements || size > maxBars)
                    {
                        GenerateEnterErrorLbl.Content = "Некоректно введено дані.";
                        GenerateEnterErrorLbl.Visibility = Visibility.Visible;
                        sizeErrorCheck = true;

                    }
                    else
                    {
                        GenerateEnterErrorLbl.Visibility = Visibility.Collapsed;
                        sizeErrorCheck = false;
                        array = new int[size];
                        sizeArray = size;
                    }
                }
                else
                {
                    if (size < minElements || size > maxElements)
                    {
                        GenerateEnterErrorLbl.Content = "Некоректно введено дані.";
                        GenerateEnterErrorLbl.Visibility = Visibility.Visible;
                        sizeErrorCheck = true;
                    }
                    else
                    {
                        GenerateEnterErrorLbl.Visibility = Visibility.Collapsed;
                        sizeErrorCheck = false;
                        array = new int[size];
                        sizeArray = size;
                    }
                }
            }
            catch (FormatException)
            {
                MessageBox.Show("Некоректний ввід.");
            }
        }

        private void ShowValidationMessage(string message)
        {
            MessageBox.Show(message, "Помилка валідації", MessageBoxButton.OK, MessageBoxImage.Warning);
        }

        private void addEnteredArr_Click(object sender, RoutedEventArgs e)
        {
            if (!applyLimits)
            {
                ArrayLabel1.Visibility = Visibility.Visible;
                ArrayTextBlock.Visibility = Visibility.Visible;
                SortedArrayLabel.Visibility = Visibility.Visible;
                SortedArrTextBlock1.Visibility = Visibility.Visible;
                int size = array.Length;
                ArrayTextBlock.Text = $"{string.Join(", ", array.Take(size))}";
            }
            if (applyLimits)
            {
                Canvas.Visibility = Visibility.Visible;
                DisplayArrayVisual(array);
            }
        }

        private void DescendingCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            descending = true;
        }

        private void DescendingCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            descending = false;
        }

        private void GenerateBttn_Click(object sender, RoutedEventArgs e)
        {
            var selectedItem = (SortingAlgorithmComboBox.SelectedItem as System.Windows.Controls.ComboBoxItem).Content.ToString();
            string size = SizeTextBox.Text;
            sizeArray = int.Parse(size);
            try
            {
                if (sizeErrorCheck || maxErrorCheck || minErrorCheck)
                {
                    ShowValidationMessage("Некоректні дані.");
                    return;
                }
                else
                {
                    string maximumDiapason = MaxDiapasonTextBox.Text;
                    maxDiapason = int.Parse(maximumDiapason);
                    string minimumDiapason = MinDiapasonTextBox.Text;
                    minDiapason = int.Parse(minimumDiapason);
                    SortBtn.Visibility = Visibility.Visible;
                    if (!applyLimits)
                    {

                        ArrayLabel1.Visibility = Visibility.Visible;
                        ArrayTextBlock.Visibility = Visibility.Visible;
                        SortedArrayLabel.Visibility = Visibility.Visible;
                        SortedArrTextBlock1.Visibility = Visibility.Visible;

                    }

                    ComplexityGroupBox.Visibility = Visibility.Visible;

                    switch (selectedItem)
                    {
                        case "Впорядкований":
                            GenerateSortedArray(sizeArray);
                            break;
                        case "Зворотно-впорядкований":
                            GenerateSortedArrayDescending(sizeArray);
                            break;
                        case "Невпорядкований":
                            GenerateRandomArray(sizeArray);
                            break;
                        default:
                            break;
                    }
                }

                if (applyLimits)
                {
                    Canvas.Visibility = Visibility.Visible;
                    DisplayArrayVisual(array);
                }
                else
                {
                    ArrayTextBlock.Text = $"{string.Join(", ", array.Take(200))}";
                    return;
                }
            }

            catch (Exception ex)
            {
                MessageBox.Show($"Виникла непередбачувана помилка: {ex.Message}");
            }

        }
        private void GenerateRandomArray(int size)
        {
            array = new int[size];
            Random rand = new Random();

            for (int i = 0; i < size; i++)
            {
                array[i] = rand.Next(minDiapason, maxDiapason);
            }
        }

        private void GenerateSortedArray(int size)
        {
            array = new int[size];
            GenerateRandomArray(size);
            Array.Sort(array);

        }

        private void GenerateSortedArrayDescending(int size)
        {
            array = new int[size];
            GenerateSortedArray(size);
            Array.Reverse(array);
        }

        private async void SortButton_Click(object Sender, RoutedEventArgs e)
        {
            try
            {
                ComboBoxItem selectedAlgorithm = (ComboBoxItem)SortingAlgoComboBox.SelectedItem;

                if (selectedAlgorithm != null)
                {
                    int Swaps = 0;
                    string algorithmName = selectedAlgorithm.Content.ToString();
                    switch (algorithmName)
                    {
                        case "Блочне":
                            currentSort = new BucketSort(array);
                            break;
                        case "Підрахунком":                           
                            currentSort = new CountingSort(array);
                            break;
                        case "Порозрядне":                           
                            currentSort = new RadixSort(array); 
                            break;
                        default:
                            MessageBox.Show("Please select a valid sorting algorithm.");
                            return;
                    }
                    
                    if (applyLimits)
                    {
                        currentSort.OnUpdate = DisplayArrayVisual;
                        await currentSort.SortArrayAsync();
                        
                        arrayForSave = currentSort.GetArray();
                        DisplayArray(currentSort.GetArray());
                    }
                    else
                    {                    
                        Canvas.Visibility = Visibility.Collapsed;
                        ArrayLabel1.Visibility = Visibility.Visible;
                        SortedArrayLabel.Visibility = Visibility.Visible;
                        ArrayTextBlock.Visibility = Visibility.Visible;
                        SortedArrTextBlock1.Visibility = Visibility.Visible;
                        currentSort.SortArray();
                        switch (algorithmName)
                        {
                            case "Блочне":
                                Swaps = ((BucketSort)currentSort).SwapCount;
                                break;
                            case "Підрахунком":
                                Swaps = ((CountingSort)currentSort).SwapCount;
                                break;
                            case "Порозрядне":
                                Swaps = ((RadixSort)currentSort).SwapCount;
                                break;
                            default:
                                MessageBox.Show("Please select a valid sorting algorithm.");
                                return;
                        }         
                        SwapsLabel.Content = Swaps;
                        array = currentSort.GetArray();
                        if (descending)
                        {
                            Sort.ReverseArray(array);

                        }
                        arrayForSave = currentSort.GetArray();                      
                        SortedArrTextBlock1.Text = $"{string.Join(", ", array.Take(200))}";
                    }
                    SaveArrButton.Visibility = Visibility.Visible;

                }
                else
                {
                    MessageBox.Show("Please select a sorting algorithm.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}");
            }

        }

        public void GenerateArrayValuesOfType(string selectedType)
        {
            switch (selectedType)
            {
                case "Блочне":
                    currentSort = new BucketSort(array);

                    break;
                case "Підрахунком":
                    currentSort = new CountingSort(array);
                    break;
                case "Порозрядне":
                    currentSort = new RadixSort(array);
                    break;
                default:
                    return;
            }
        }

        private void SizeTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !IsTextAllowed(e.Text);
        }

        private static bool IsTextAllowed(string text)
        {
            return !new Regex("[^0-9]").IsMatch(text);
        }

        private void DisplayArray(int[] sortedArray)
        {           
            Canvas.Visibility = Visibility.Visible;
            DisplayArrayVisual(sortedArray);
        }

        private void DisplayArrayVisual(int[] array)
        {
            Canvas.Children.Clear();
            double canvasWidth = Canvas.ActualWidth;
            double canvasHeight = Canvas.ActualHeight;
            double barWidth = canvasWidth / array.Length;
            double maxValue = array.Max();

            for (int i = 0; i < Math.Min(array.Length, maxBars); i++)
            {
                double barHeight = (array[i] / maxValue) * canvasHeight;
                Rectangle bar = new Rectangle
                {
                    Width = barWidth,
                    Height = barHeight,
                    Fill = Brushes.Blue,
                    Stroke = Brushes.Black,
                    Margin = new Thickness(i * barWidth, canvasHeight - barHeight, 0, 0)
                };
                Canvas.Children.Add(bar);
            }
        }

        private void TextBoxInput_TextChanged(object sender, TextChangedEventArgs e)
        {
            string input = TextBoxInput.Text.Trim();

            if (string.IsNullOrEmpty(input))
            {
                MessageBox.Show("Введіть масив чисел через кому.");
                return;
            }

            string[] stringArray = input.Split(new char[] { ' ', ',' }, StringSplitOptions.RemoveEmptyEntries);

            List<int> tempArray = new List<int>();
            foreach (var item in stringArray)
            {
                if (int.TryParse(item.Trim(), out int parsedforSizeValue))
                {
                    if (applyLimits && parsedforSizeValue > 200)
                    {
                        enterArrTooSmallErrorCheck = true;
                        ArrayErrorLabel.Visibility = Visibility.Visible;
                        ArrayErrorLabel.Content = "Занадто великий масив.";
                        return;
                    }
                    else
                    {
                        tempArray.Add(parsedforSizeValue);
                        enterArrTooSmallErrorCheck = false;
                        ArrayErrorLabel.Visibility = Visibility.Collapsed;
                    }
                }
            }

           array = tempArray.ToArray();
        }

        private void SaveArrButton_Click(object sender, RoutedEventArgs e)
        {
            if (arrayForSave != null && arrayForSave.Length > 0)
            {
                string fileName = $"sorted_array_{arrayForSave.Length}.txt"; //  ім'я файлу

                int count = 1;
                while (File.Exists(fileName))
                {
                    fileName = $"sorted_array_{arrayForSave.Length}_{count}.txt"; // Якщо файл уже існує,  суфікс
                    count++;
                }

                // Збереження відсортованого масиву у файл
                File.WriteAllText(fileName, string.Join(", ", arrayForSave));

                MessageBox.Show($"Відсортований масив збережено у файл: {fileName}", "Успішно");
            }
            else
            {
                MessageBox.Show("Відсортований масив не існує або порожній.", "Помилка");
            }
        }

       

        
    }
}
