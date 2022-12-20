using ICSharpCode.AvalonEdit.Document;
using ICSharpCode.AvalonEdit.Highlighting;
using ICSharpCode.AvalonEdit.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.Metadata;
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

namespace TextRenPyFinder
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Finder _finder;

        public MainWindow()
        {
            InitializeComponent();
            this.avEditor.PreviewMouseWheel += new System.Windows.Input.MouseWheelEventHandler(textEditor_PreviewMouseWheel);
            this.KeyDown += AvEditorOnKeyDown;
            var path = @"x:\Game\Photo_Hunt\Rus\PhotoHunt-0.15.1-Rus+mod\game\script_loc\loc43_nurse_office.rpy";

            _finder = new Finder(@"x:/Game/Photo_Hunt/Rus/game");

            this.avEditor.FontSize = 30;

            //if (f.FindText("Я тоже волнуюсь".ToLowerInvariant(), out var finded))
            //{

            //    avEditor.Document = new TextDocument(finded.Text);
            //    avEditor.Loaded += (sender, args) =>
            //    {
            //        avEditor.SelectionStart = finded.Index;
            //        avEditor.SelectionLength =finded.Lenght;
            //        var select = avEditor.TextArea.Selection;
            //        avEditor.ScrollToLine(select.StartPosition.Line);
            //        var hlManager = HighlightingManager.Instance;
            //        var d = hlManager.GetDefinition("Json");
            //        avEditor.SyntaxHighlighting = d;
            //    };
            //}


        }

        private void AvEditorOnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.F3)
            {
                var text = _finder.FindNext(FindText.Text);
                if (text != null)
                {
                    Init(text);
                }
            }

            if (Keyboard.Modifiers == ModifierKeys.Control && e.Key == Key.F)
            {
                FindText.Focus();
                FindText.SelectAll();
            }
        }

        private void textEditor_PreviewMouseWheel(object sender, System.Windows.Input.MouseWheelEventArgs e)
        {
            if (Keyboard.Modifiers == ModifierKeys.Control)
            {
                double fontSize = this.avEditor.FontSize + e.Delta / 25.0;

                if (fontSize < 6)
                    this.avEditor.FontSize = 6;
                else
                {
                    if (fontSize > 200)
                        this.avEditor.FontSize = 200;
                    else
                        this.avEditor.FontSize = fontSize;
                }

                e.Handled = true;
            }
        }

        private void Init(FindedData finded)
        {
            FileName.Text = finded.Path;
            avEditor.Document = new TextDocument(finded.Text);
            avEditor.SelectionStart = finded.Index;
            avEditor.SelectionLength = finded.Lenght;
            var select = avEditor.TextArea.Selection;
            avEditor.ScrollToLine(select.StartPosition.Line);
            var hlManager = HighlightingManager.Instance;
            var d = hlManager.GetDefinition("Json");
            avEditor.SyntaxHighlighting = d;
        }

        private void AvEditor_OnMouseDown(object sender, MouseButtonEventArgs e)
        {

        }

        private void Find_OnClick(object sender, RoutedEventArgs e)
        {
            Status.Text = "";
            var text = _finder.Find(FindText.Text);
            if (text != null)
            {
                Init(text);
            }
            else
            {
                Status.Text = "Not found";
            }
        }

        private void FindNext_OnClick(object sender, RoutedEventArgs e)
        {
            Status.Text = "";
            var text = _finder.FindNext(FindText.Text);
            if (text != null)
            {
                Init(text);
            }
            else
            {
                Status.Text = "Not found";
            }
        }

        private void MainWindow_OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            avEditor.Width = e.NewSize.Width - 20;
        }

        private void PathFind_OnClick(object sender, RoutedEventArgs e)
        {
            using (var dialog = new System.Windows.Forms.FolderBrowserDialog())
            {
                if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    Path.Text = dialog.SelectedPath;
                    _finder = new Finder(dialog.SelectedPath);
                }
            }
        }
    }
}
