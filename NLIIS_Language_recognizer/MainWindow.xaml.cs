using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using Microsoft.Win32;
using NLIIS_Language_recognizer.Models;
using NLIIS_Language_recognizer.Service;

namespace NLIIS_Language_recognizer
{
    public partial class MainWindow : Window
    {
        private ILanguageRecognizer FrequencyWordRecognizer { get; set; }
        private ILanguageRecognizer ShortWordRecognizer { get; set; }
        private ILanguageRecognizer OwnRecognizer { get; set; }
        
        private OpenFileDialog FileDialog { get; set; }
        
        public MainWindow()
        {
            FrequencyWordRecognizer = new FrequencyWordLanguageRecognizer();
            ShortWordRecognizer = new ShortWordLanguageRecognizer();
            OwnRecognizer = new OwnLanguageRecognizer();
            FileDialog = new OpenFileDialog { Multiselect = true };

            InitializeComponent();
        }
        
        public void Upload(IEnumerable<string> paths, string language, string method)
        {
            foreach (var path in paths)
            {
                var termsFromFile = DocumentService.FromPDF(path);
                IDictionary<string, double> termsProbability = null;
            
                if (method == FrequencyWordRecognizer.MethodName)
                {
                    termsProbability = FrequencyWordRecognizer.GetWords(termsFromFile);
                }
                else if (method == ShortWordRecognizer.MethodName)
                {
                    termsProbability = ShortWordRecognizer.GetWords(termsFromFile);
                }

                foreach (var (word, probability) in termsProbability)
                {
                    var newWord = new LanguageWord
                    {
                        Language = language,
                        Method = method,
                        Probability = probability,
                        Word = word
                    };

                    if (!LanguageWord.Words.Contains(newWord, new LanguageWord.Comparer()))
                    {
                        LanguageWord.Words.Add(newWord);
                    }
                }
            }

            ButtonRecognize.IsEnabled = true;
        }
        
        private void Help_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(
                "Load documents to fill the base probabilities for each language and method" +
                "\nThen you can choose a document to recognize language in");
        }
        
        private void Authors_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(
                "Group 721701:\nSemenikhin Nikita,\nStryzhych Angelika",
                "Authors");
        }
        
        private void ButtonFile_OnClick(object sender, RoutedEventArgs e)
        {
            if (FileDialog.ShowDialog() == true)
            {
                UploadPath.Text = string.Empty;
                
                foreach (var filename in FileDialog.FileNames)
                {
                    UploadPath.Text += filename + "*";
                }

                if (UploadPath.Text.Length > 0)
                {
                    UploadPath.Text = UploadPath.Text.Substring(0, UploadPath.Text.Length - 1);
                    ButtonUpload.IsEnabled = true;
                }
            }
        }
        
        private void ButtonUpload_OnClick(object sender, RoutedEventArgs e)
        {
            Upload(UploadPath.Text.Split("*"), UploadLanguage.Text, UploadMethod.Text);
        }
        
        private void ButtonRecognize_OnClick(object sender, RoutedEventArgs e)
        {
            var index = UploadPath.Text.IndexOf("*", StringComparison.Ordinal);
            var path = UploadPath.Text.Substring(0, index <= 0 ? UploadPath.Text.Length : index);
            var text = DocumentService.FromPDF(path);

            LangsLabel.Content = "Frequency M: " + FrequencyWordRecognizer.Recognize(text) +
                                 ", Short M: " + ShortWordRecognizer.Recognize(text) +
                                 ", Own M: " + OwnRecognizer.Recognize(text);
        }
    }
}