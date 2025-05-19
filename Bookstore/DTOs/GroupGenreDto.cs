namespace Bookstore.DTOs
{
    public class GroupGenreDto
    {
        public int Id { get; set; }
        public string GenreType { get; set; }
        public ICollection<GroupBookDto> Books { get; set; }
        public ICollection<string> BookTitle { get; set; }
    }
}
