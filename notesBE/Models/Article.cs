using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace notesBE.Models;

public class Article
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; } = null;

    [BsonElement("title")]
    public string Title { get; set; } = null!;
    [BsonElement("lessonDate")]
    public DateTime LessonDate { get; set; }
    [BsonElement("content")]
    public string Content { get; set; } = null!;
}