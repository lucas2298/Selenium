using Caliburn.Micro;
using Selenium.Models;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using System.Data;
using System.Linq;

namespace Selenium.ViewModels
{
    public class TestOptionViewModel
    {
        public BindableCollection<TestOptionModel> TestOption { get; set; }
        public TestOptionViewModel()
        {
            var JsonString = File.ReadAllText(@"D:\Selenium\Selenium\TestOption\Category.json");
            DataSet dataSet = JsonConvert.DeserializeObject<DataSet>(JsonString);
            DataTable dataTable = dataSet.Tables[0];
            var TestOptionSelect = dataSet.Tables[0].AsEnumerable().Select(dataRow => new TestOptionModel
            {
                Id = dataRow.Field<long>("Id"),
                OptionName = dataRow.Field<string>("OptionName"),
                Description = dataRow.Field<string>("Description"),
            }).ToList();
            TestOption = new BindableCollection<TestOptionModel>(TestOptionSelect);
        }
    }
}
