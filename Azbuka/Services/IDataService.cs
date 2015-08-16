using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Web;
using System.Net;
 


namespace Azbuka 
{

    interface IDataService 
    {
        Task<List<CatalogItem>> GetContentAsync();
        List<Category> GetCategories();
        List<Pubhouse> GetPubhouses();
        List<CatalogType> GetCatalogTypes();
        void SetCategory(Category category);
        void SetPubhouse(Pubhouse pubhouse);
        void SetSearchTerm(string searchTerm);
        void SetCatalogType(CatalogType ct);
        PositionInformation GetPositionInformation(string id);
    }

    public class DataService : IDataService
    {
        private Uri _baseAddress = new Uri(@"http://api.e-azbuka.ru/");
        private Category _selectedCategory;
        private Pubhouse _selectedPubhouse;
        private CatalogType _selectedCatalogType;
        private List<CatalogType> _catalogType;
        private string _searchTerm;

        public DataService()
        {
            _catalogType = new List<CatalogType>()
            {
                new CatalogType(){ Name = "user" },
                new CatalogType(){ Name = "organization" }
            };
        }

        

        public async Task<List<CatalogItem>> GetContentAsync()
        {
            var list = new List<CatalogItem>();
            var uri = GetUriForContent();

            using (var client = new HttpClient())
            {
                client.BaseAddress = _baseAddress;
                var response = await client.GetAsync(uri);
                 list.AddRange(JsonConvert.DeserializeObject<CatalogContainer>(response.Content.ReadAsStringAsync().Result).catalog);
            }

            return list;
        }

        public PositionInformation GetPositionInformation(string id)
        {
            var info = new PositionInformation();
            var uri = GetUriForInfo(id);

            using (var client = new HttpClient())
            {
                client.BaseAddress = _baseAddress;
                var response = client.GetAsync(uri); 
                info = JsonConvert.DeserializeObject<PositionInformation>(response.Result.Content.ReadAsStringAsync().Result);
            }

            return info;
        }

        private string GetUriForInfo(string id)
        {
            var uri = string.Format("{0}/{1}.json", UriContainer.POSITION_INFO, id);

            if (_selectedCatalogType != null)
                uri += string.Format("?type=", _selectedCatalogType.Name);

            return uri;
        }

        private string GetUriForContent()
        {
            var u = UriContainer.CONTENT + "?";


            if (_selectedCatalogType != null)
            {
                u = u + string.Format("type={0}&", _selectedCatalogType.Name);
            }

            u = u + string.Format("search={0}&", _searchTerm);

            if (_selectedCategory != null && _selectedCategory.name != "Все") 
            {
                u = u + string.Format("categories={0}&", _selectedCategory.name);
            }

            if (_selectedPubhouse != null && _selectedPubhouse.id != "-1")
            {
                u = u + string.Format("pubhouses={0}&", _selectedPubhouse.id);
            }

            return u;
        }


        public List<Category> GetCategories()
        {
            var list = new List<Category>();
            list.Add(new Category() { name ="Все"});
            var uri = UriContainer.CATEGORIES; 

            using (var client = new HttpClient())
            {
                client.BaseAddress = _baseAddress;
                var response =  client.GetAsync(uri);
                list.AddRange(JsonConvert.DeserializeObject<CategoryContainer>(response.Result.Content.ReadAsStringAsync().Result).list);
            }

            return list;
        }

        public List<Pubhouse> GetPubhouses()
        {
            var list = new List<Pubhouse>();
            list.Add(new Pubhouse() { name = "Все", id = "-1" });
            var uri = UriContainer.PUBHOUSES;
            using (var client = new HttpClient())
            {
                client.BaseAddress = _baseAddress;
                var response = client.GetAsync(uri); 
                list.AddRange(JsonConvert.DeserializeObject<PubhouseContainer>(response.Result.Content.ReadAsStringAsync().Result).list);
            }

            return list;
        }

        struct UriContainer
        {
            public const string CONTENT = @"/1.0/catalog/user.json";
            public const string POSITION_INFO = @"/1.0/catalog/user/"; 
            public const string CATEGORIES = @"/1.0/catalog/user/categories.json";
            public const string PUBHOUSES = @"1.0/catalog/user/pubhouses.json";
        }

        

        public void SetCategory(Category category)
        {
            _selectedCategory = category;
        }

        public void SetPubhouse(Pubhouse pubhouse)
        {
            _selectedPubhouse = pubhouse;
        }

        public void SetSearchTerm(string searchTerm)
        {
            _searchTerm = searchTerm;
        }

        public List<CatalogType> GetCatalogTypes() {
            return _catalogType;
        }

        public void SetCatalogType( CatalogType ct)
        {
            _selectedCatalogType = ct;
        }
    }
}
