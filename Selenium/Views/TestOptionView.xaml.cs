using WindowViewBase;
using System;
using System.Windows;
using Selenium.Models;
using System.IO;
using Newtonsoft.Json;

namespace Selenium.Views
{
    /// <summary>
    /// Interaction logic for TestOptionView.xaml
    /// </summary>
    public partial class TestOptionView : WindowViewFunctionBase
    {
        public TestOptionView()
        {
            InitializeComponent();
            this.Loaded += TestOptionView_Loaded;
        }
        private void TestOptionView_Loaded(object sender, RoutedEventArgs e)
        {
            SelectTestModel SelectedOption = new SelectTestModel();
            StackPanel_SelectOptions.DataContext = SelectedOption;
        }
        public void TestOption_Click(object sender, RoutedEventArgs e)
        {
            var testOption = (TestOptionModel)ComboBox_TestOption.SelectedItem;
            if (testOption.Id == 1)
            {
                OpenLinkInFile();
            }
            else if (testOption.Id == 2)
            {
                OpenLinkInMenu();
            }
            else if (testOption.Id == 3)
            {
                OpenLinkInProject();
            }
        }
        public void SaveSelectTestOption_Click(object sender, RoutedEventArgs e)
        {
            var SelectedOption = (SelectTestModel)StackPanel_SelectOptions.DataContext;
            var testOption = (TestOptionModel)ComboBox_TestOption.SelectedItem;
            var Path = @"D:\Selenium\Selenium\SaveTestSelect\";
            if (testOption.Id == 1)
            {
                Path = Path + testOption.SaveName;
                Directory.CreateDirectory(Path);
            }
            else if (testOption.Id == 2)
            {
                Path = Path + testOption.SaveName;
                Directory.CreateDirectory(Path);
            }
            else if (testOption.Id == 3)
            {
                Path = Path + testOption.SaveName;
                Directory.CreateDirectory(Path);
            }
            Path = Path + @"\json.txt";
            string json = JsonConvert.SerializeObject(SelectedOption, Formatting.Indented);
            File.WriteAllText(Path, json);
            MessageBox.Show("Lưu mẫu thành công!");
        }
        public void UploadFileJsonToSelectTest_Click(object sender, RoutedEventArgs e)
        {
            // Create OpenFileDialog
            Microsoft.Win32.OpenFileDialog openFileDlg = new Microsoft.Win32.OpenFileDialog();

            // Launch OpenFileDialog by calling ShowDialog method
            Nullable<bool> result = openFileDlg.ShowDialog();
            // Get the selected file name and display in a TextBox.
            // Load content of file in a TextBlock
            if (result == true)
            {
                var Json = File.ReadAllText(openFileDlg.FileName);
                SelectTestModel SelectedOption = JsonConvert.DeserializeObject<SelectTestModel>(Json);
                StackPanel_SelectOptions.DataContext = SelectedOption;
                MessageBox.Show("Tải lên thành công!");
            }
        }
        public void UnSelectAll_Click(object sender, RoutedEventArgs e)
        {
            SelectTestModel SelectedOption = new SelectTestModel();
            StackPanel_SelectOptions.DataContext = SelectedOption;
        }
    }
}
