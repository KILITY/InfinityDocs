using System.Windows;
using InfinityDocs.Features.Helpers.Implementations;
using InfinityDocs.Features.Models;
using InfinityDocs.Features.Parser;
using TreeSitter;
using TreeSitterLanguagePack;

namespace InfinityDocs
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            //TestTreeSitter();
            Parse();
        }

        //private void TestTreeSitter()
        //{
        //    var parser = TreeSitterLanguagePackConverter.GetParser("csharp");

        //    var code = """
        //               class Test
        //               {
        //                   void Run()
        //                   {
        //                       Calculate(5);
        //                   }

        //                   void Calculate(int number)
        //                   {
        //                   }
        //               }
        //               """;

        //    var tree = parser.Parse(code);

        //    var result = tree.RootNode();

        //    var result2 = result.ToSexp();

        //    var one = 2;
        //}

        public void Parse()
        {
            var languageHelper = new LanguageHelper();
            var fileExtensionHelper = new FileExtensionHelper();
            var docParser = new DocParser(fileExtensionHelper, languageHelper);
            var result = docParser.Parse("C:\\Users\\Andrei\\Downloads\\ParserTestProject", Languages.csharp);
            var result2 = 1;
        }
    }
}