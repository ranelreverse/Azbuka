using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Azbuka
{
     
    public class Pricelist
    {
        public int price { get; set; }
        public string license_type { get; set; }
        public int license_period { get; set; }
        public string @for { get; set; }
    }

    public class CatalogItem
    {
        public string id { get; set; }
        public string name { get; set; }
        public int publication_year { get; set; }
        public string file_id { get; set; }
        public string publishing_house { get; set; }
        public string publishing_slug { get; set; }
        public int last_update_date { get; set; }
        public bool is_demo { get; set; }
        public string authors_short_str { get; set; }
        public List<Pricelist> pricelist { get; set; }
        public string cover_url { get; set; }
    }

    public class CatalogType {
        public string Id { get; set; }
        public string Name { get; set; }
    }

    public class CatalogContainer
    {
        public List<CatalogItem> catalog { get; set; }
        public int previous_cursor { get; set; }
        public int next_cursor { get; set; }
        public int total { get; set; }
    }

    public class Category
    {
        public string name { get; set; }
        public int qty { get; set; }
    }

    public class CategoryContainer
    {
        public List<Category> list { get; set; }
    }

    public class Pubhouse
    {
        public string id { get; set; }
        public string name { get; set; }
        public string slug { get; set; }
        public int qty { get; set; }
    }

    public class PubhouseContainer
    {
        public List<Pubhouse> list { get; set; }
    }

   

    public class PositionInformation
    {
        public string id { get; set; }
        public string name { get; set; }
        public int publication_year { get; set; }
        public string file_id { get; set; }
        public string publishing_house { get; set; }
        public string publishing_slug { get; set; }
        public int last_update_date { get; set; }
        public bool is_demo { get; set; }
        public string authors_short_str { get; set; }
        public List<Classificator> classificators { get; set; }
        public List<Pricelist> pricelist { get; set; }
        public string cover_url { get; set; }
    }

    public class Classificator
    {
        public string logical_name { get; set; }
        public string human_readable_name { get; set; }
        public string value { get; set; }
    }

    
}
