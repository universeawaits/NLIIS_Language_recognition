using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
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
using Microsoft.Win32;
using NLIIS_Language_recognizer.Models;
using NLIIS_Language_recognizer.Service;

namespace NLIIS_Language_recognizer
{
    public partial class MainWindow : Window
    {
        private ILanguageRecognizer FrequencyWordRecognizer { get; set; }
        private ILanguageRecognizer ShortWordRecognizer { get; set; }

        public IDictionary<string, int> RussianWordFrequency = new Dictionary<string, int>()
        {
            { "и", 7416716 },
            { "в", 5842670 },
            { "не", 3385161 },
            { "на", 2936096 },
            { "с", 2228350 },
            { "что", 2210373 },
            { "я", 1592127 },
            { "а", 1541398 },
            { "он", 1377314 },
            { "как", 1300577 },
            { "к", 1132463 },
            { "по", 1071698 },
            { "но", 1048321 },
            { "его", 983462 },
            { "это", 957828 },
            { "из", 836230 },
            { "все", 817619 },
            { "у", 798746 },
            { "за", 754712 },
            { "от", 747239 },
            { "то", 695763 },
            { "о", 685187 },
            { "же", 665139 },
            { "так", 663853 },
            { "для", 600197 },
            { "было", 592525 },
            { "она", 553635 },
            { "только", 516518 },
            { "мы", 501250 },
            { "бы", 485709 },
            { "мне", 449883 },
            { "был", 442198 },
            { "ее", 438349 },
            { "или", 434375 },
            { "еще", 432318 },
            { "меня", 422671 },
            { "их", 415977 },
            { "они", 412867 },
            { "до", 400385 },
            { "когда", 390040 },
            { "уже", 385992 },
            { "ты", 348216 },
            { "если", 347484 },
            { "да", 338405 },
            { "вы", 338350 },
            { "вот", 310419 },
            { "при", 305370 },
            { "ни", 305025 },
            { "ему", 302129 },
            { "чтобы", 286114 },
            { "нет", 269615 },
            { "есть", 267554 },
            { "даже", 264014 },
            { "может", 263199 },
            { "быть", 262913 },
            { "во", 259603 },
            { "время", 255317 },
            { "очень", 252939 },
            { "были", 249393 },
            { "была", 246499 },
            { "сказал", 233062 },
            { "ли", 231733 },
            { "под", 228843 },
            { "со", 222715 },
            { "себя", 220734 },
            { "нас", 218046 },
            { "где", 216726 },
            { "него", 216511 },
            { "чем", 213262 },
            { "того", 209534 },
            { "без", 205150 },
            { "будет", 204581 },
            { "этого", 202868 },
            { "теперь", 201329 },
            { "после", 195907 },
            { "там", 192639 },
            { "можно", 189774 },
            { "этом", 189405 },
            { "раз", 184146 },
            { "себе", 180956 },
            { "тем", 177179 },
            { "этот", 176597 },
            { "ну", 175961 },
            { "том", 174807 },
            { "потом", 173458 },
            { "более", 170327 },
            { "них", 168703 },
            { "которые", 167945 },
            { "всех", 167764 },
            { "человек", 166587 },
            { "ничего", 163311 },
            { "надо", 162849 },
            { "тут", 160363 },
            { "тогда", 159227 },
            { "здесь", 158961 },
            { "потому", 157741 },
            { "один", 157644 },
            { "кто", 156987 },
            { "через", 153712 },
            { "который", 151251 },
        };

        public IDictionary<string, int> ItalianWordFrequency = new Dictionary<string, int>()
        {
            { "non", 25757 },
            { "di", 22868 },
            { "che", 22738 },
            { "è", 18624 },
            { "e", 17600 },
            { "la", 16404 },
            { "il", 14765 },
            { "un", 14460 },
            { "a", 13915 },
            { "per", 10501 },
            { "in", 8583 },
            { "una", 8529 },
            { "mi", 8303 },
            { "sono", 8020 },
            { "ho", 6908 },
            { "ma", 6403 },
            { "l'", 6343 },
            { "lo", 6212 },
            { "ha", 6152 },
            { "le", 6145 },
            { "si", 5879 },
            { "ti", 5662 },
            { "i", 5626 },
            { "con", 5537 },
            { "cosa", 5524 },
            { "se", 5394 },
            { "io", 5286 },
            { "come", 5211 },
            { "da", 5195 },
            { "ci", 4765 },
            { "no", 4755 },
            { "questo", 3830 },
            { "qui", 3745 },
            { "e'", 3739 },
            { "hai", 3727 },
            { "sei", 3550 },
            { "del", 3466 },
            { "bene", 3457 },
            { "tu", 3447 },
            { "sì", 3295 },
            { "me", 3202 },
            { "più", 3199 },
            { "al", 3029 },
            { "mio", 2986 },
            { "c'", 2884 },
            { "perché", 2818 },
            { "lei", 2756 },
            { "solo", 2727 },
            { "te", 2672 },
            { "era", 2622 },
            { "gli", 2611 },
            { "tutto", 2593 },
            { "della", 2560 },
            { "così", 2529 },
            { "mia", 2361 },
            { "ne", 2316 },
            { "questa", 2225 },
            { "fare", 2188 },
            { "quando", 2179 },
            { "ora", 2124 },
            { "fatto", 2049 },
            { "essere", 2042 },
            { "so", 2031 },
            { "mai", 1981 },
            { "chi", 1932 },
            { "o", 1879 },
            { "alla", 1871 },
            { "tutti", 1857 },
            { "molto", 1848 },
            { "dei", 1826 },
            { "anche", 1820 },
            { "detto", 1802 },
            { "quello", 1767 },
            { "va", 1765 },
            { "niente", 1754 },
            { "grazie", 1725 },
            { "lui", 1694 },
            { "voglio", 1655 },
            { "abbiamo", 1588 },
            { "stato", 1578 },
            { "nel", 1565 },
            { "suo", 1548 },
            { "dove", 1545 },
            { "posso", 1531 },
            { "oh", 1530 },
            { "prima", 1516 },
            { "allora", 1476 },
            { "siamo", 1474 },
            { "d'", 1472 },
            { "uno", 1466 },
            { "un'", 1456 },
            { "sua", 1456 },
            { "tuo", 1451 },
            { "hanno", 1450 },
            { "noi", 1448 },
            { "sta", 1447 },
            { "fa", 1436 },
            { "due", 1406 },
            { "vuoi", 1405 },
            { "ancora", 1398 }
        };
        
        public MainWindow()
        {
            FrequencyWordRecognizer = new FrequencyWordLanguageRecognizer();
            ShortWordRecognizer = new ShortWordLanguageRecognizer();
            
            InitializeComponent();
        }
        
        public void Upload(string path, string language, string method)
        {
            var termsFromFile = DocumentService.FromPDF(path);
            IDictionary<string, double> termsProbability = null;
            
            if (method == FrequencyWordLanguageRecognizer.MethodName)
            {
                termsProbability = FrequencyWordRecognizer.GetWords(termsFromFile);
            }
            else if (method == ShortWordLanguageRecognizer.MethodName)
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
        
        private void Help_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(
                "Type words in search box, then press Search\n" +
                "button to find proper documents.\n" +
                "Boolean operators can be used: &, |, and brackets. Example query:\n" +
                @"we | (are & ""the champions"")",
                "Help");
        }
        
        private void Authors_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(
                "Group 721701:\nSemenikhin Nirita,\nStryzhych Angelika",
                "Authors");
        }
        
        private void ButtonFile_OnClick(object sender, RoutedEventArgs e)
        {
            var fileDialog = new OpenFileDialog();
            
            if (fileDialog.ShowDialog() == true)
            {
                UploadPath.Text = fileDialog.FileName;
            }
        }
        
        public void ButtonUpload_OnClick(object sender, RoutedEventArgs e)
        {
            Upload(UploadPath.Text, UploadLanguage.Text, UploadMethod.Text);
        }
    }
}