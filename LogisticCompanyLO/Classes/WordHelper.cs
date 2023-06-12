using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using Word = Microsoft.Office.Interop.Word;

namespace LogisticCompanyLO
{
    internal class WordHelper
    {
        private FileInfo _fileInfo;
        public WordHelper(string fileName)
        {
            if (File.Exists(fileName))
            {
                _fileInfo = new FileInfo(fileName);
            }
            else
            {
                MessageBox.Show("Файла не существует!");
            }
        }

        internal void CreateWordDocument(Dictionary<string, string> items, string routeOrder)
        {
            Word.Application app = null;
            try
            {
                app = new Word.Application();

                object file = _fileInfo.FullName;

                app.Documents.Open(file);

                foreach (var item in items)
                {
                    Word.Find find = app.Selection.Find;

                    find.Text = item.Key;

                    find.Replacement.Text = item.Value;

                    Object wrap = Word.WdFindWrap.wdFindContinue;
                    Object replace = Word.WdReplace.wdReplaceAll;


                    find.Execute(FindText: Type.Missing,
                        MatchCase: false,
                        MatchWholeWord: false,
                        MatchWildcards: false,
                        MatchSoundsLike: Type.Missing,
                        MatchAllWordForms: false,
                        Forward: true,
                        Wrap: wrap,
                        Format: false,
                        ReplaceWith: Type.Missing,
                        Replace: replace);
                }

                string newPath = _fileInfo.DirectoryName + "\\Orders";

                Object newFileName = Path.Combine(newPath, DateTime.Now.ToString("dd.MM.yyyy HH.mm.ss") + " " + routeOrder + ".doc");
                app.ActiveDocument.SaveAs2(newFileName);
                app.ActiveDocument.Close();

                MessageBox.Show("Операция прошла успешно", "Создание Word-документа", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                if (app != null)
                {
                    app.Quit();
                }
            }
        }
    }
}
