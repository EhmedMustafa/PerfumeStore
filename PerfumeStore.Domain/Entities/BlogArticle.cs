using System;

namespace PerfumeStore.Domain.Entities
{
    public class BlogArticle
    {
        public int Id { get; set; }
        public string Slug { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Excerpt { get; set; } = string.Empty;
        public string Tag { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty;

        // JSON array — hər element ya paraqraf, ya "## başlıq"
        public string BodyJson { get; set; } = "[]";

        public DateTime PublishedAt { get; set; } = DateTime.UtcNow;
        public int ViewCount { get; set; } = 0;
        public bool IsPublished { get; set; } = true;
    }
}
