using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace Azbuka
{
    class MainViewModel : INotifyPropertyChanged
    {
        private IDataService _dataService;
        public MainViewModel()
        {
            _dataService = new DataService();

            Categories = _dataService.GetCategories();
            _selectedCategory = Categories.Count > 0 ? Categories[0] : new Category();

            Pubhouses = _dataService.GetPubhouses();
            _selectedPubhouse = Pubhouses.Count > 0 ? Pubhouses[0] : new Pubhouse();

            CatalogTypes = _dataService.GetCatalogTypes();
            _selectedCatalogType = CatalogTypes.Count > 0 ? CatalogTypes[0] : new CatalogType();

            UpdateContentAsyncCommand.Execute(null);
        }

        private List<CatalogItem> _content = new List<CatalogItem>();

        public List<CatalogItem> Content
        {
            get { return _content; }
            set
            {
                _content = value;
                RaisePropertyChanged();
            }
        }

        private CatalogItem _selectedContentItem;

        public CatalogItem SelectedContentItem
        {
            get { return _selectedContentItem; }
            set
            {
                _selectedContentItem = value;

                PositionInformation = _dataService.GetPositionInformation(value.id);

                
                RaisePropertyChanged();
            }
        }
         

        private PositionInformation _positionInformation;


        public PositionInformation PositionInformation
        {
            get
            {
                return _positionInformation;
            }
            set {
                _positionInformation = value;
                RaisePropertyChanged("PositionInformationString");
                RaisePropertyChanged();
            }
        }

        public string PositionInformationString
        {
            get { 
                    return _positionInformation == null ? "" :
                            string.Format(@"{0}, {1}, {2}, {3} ", _positionInformation.name
                                                 , _positionInformation.publication_year
                                                 , _positionInformation.publishing_house
                                                 , _positionInformation.authors_short_str ); 
            }
        }

        private List<CatalogType> _catalogTypes = new List<CatalogType>();

        public List<CatalogType> CatalogTypes
        {
            get { return _catalogTypes; }
            set
            {
                _catalogTypes = value;
                RaisePropertyChanged();
            }
        }

        private CatalogType _selectedCatalogType = new CatalogType();

        public CatalogType SelectedCatalogType
        {
            get { return _selectedCatalogType; }
            set
            {
                _selectedCatalogType = value;
                _dataService.SetCatalogType(value);
                 
                UpdateContentAsyncCommand.Execute(null);
                RaisePropertyChanged();
            }
        }

        private AsyncCommand _updateContentCommand;
        public ICommand UpdateContentAsyncCommand
        {
            get
            {
                if (_updateContentCommand == null)
                {
                    _updateContentCommand = new AsyncCommand(AsyncUpdateContent);
                }
                return _updateContentCommand;
            }
        }
        private async Task AsyncUpdateContent(object o)
        {
            PositionInformation = null;
            Content = await _dataService.GetContentAsync(); 
        }

        private List<Category> _categories = new List<Category>();

        public List<Category> Categories
        {
            get { return _categories; }
            set
            {
                _categories = value;
                RaisePropertyChanged();
            }
        }

        private Category _selectedCategory ;

        public Category SelectedCategory
        {
            get { return _selectedCategory; }
            set
            {
                _selectedCategory = value;
                _dataService.SetCategory(value);
                UpdateContentAsyncCommand.Execute(null);

                RaisePropertyChanged();
            }
        } 


         private List<Pubhouse> _pubhouses = new List<Pubhouse>();

        public List<Pubhouse> Pubhouses
        {
            get { return _pubhouses; }
            set
            {
                _pubhouses = value;
                RaisePropertyChanged();
            }
        }


        private Pubhouse _selectedPubhouse;

        public Pubhouse SelectedPubhouse
        {
            get { return _selectedPubhouse; }
            set
            {
                _selectedPubhouse = value;
                _dataService.SetPubhouse(value);
                UpdateContentAsyncCommand.Execute(null);

                RaisePropertyChanged();
            }
        } 
         
        private string _searchTerm = "";
        public string SearchTerm
        {
            get { return _searchTerm; }
            set
            {
                _searchTerm = value;

                _dataService.SetSearchTerm(_searchTerm);
                UpdateContentAsyncCommand.Execute(null);
                
                RaisePropertyChanged();
            }
        }

        private void RaisePropertyChanged([CallerMemberName]string propertyName = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
