namespace MyBlog.Api.DLL.Models.RequestModels
{
    public class MainViewModel
    {
        public MainViewModel() { Post = new ArticleCreateRequest(); }
        public ArticleCreateRequest Post { get; set; }

        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string Surname { get; set; }
        public string Login { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

        //public UserRole Role { get; set; }
        public DateTime? DateOfTheBirth { get; set; }
        public string Image { get; set; }
        public string Status { get; set; }
        public string About { get; set; }
        public string PhoneNumber { get; set; }

        public IEnumerable<ArticleEditRequest> Posts { get; set; }

        public string GetFullName()
        {
            return FirstName + " " + Surname;
        }
    }
}
