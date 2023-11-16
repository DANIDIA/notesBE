using notesBE.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace notesBE.Services;

public class ArticlesService
{
    private readonly IMongoCollection<Article> _articleCollection;

    public ArticlesService(IOptions<NotesDatabaseSettings> notesDatabaseSetting)
    {
        var mongoClient = new MongoClient(notesDatabaseSetting.Value.ConnectionString);
        var mongoDatabase = mongoClient.GetDatabase(notesDatabaseSetting.Value.DatabaseName);
        _articleCollection = mongoDatabase.GetCollection<Article>(notesDatabaseSetting.Value.ArticlesCollectionName);
    }

    public async Task<List<Article>> GetAsync() => 
        await _articleCollection.Find(article => true).ToListAsync();

    public async Task<Article?> GetAsync(string id) =>
         await _articleCollection.Find(article => article.Id == id).FirstOrDefaultAsync();
    
    public async Task CreateAsync(Article article) =>
        await _articleCollection.InsertOneAsync(article);

    public async Task UpdateAsync(Article updateArticle) =>
        await _articleCollection.ReplaceOneAsync(article => article.Id == updateArticle.Id, updateArticle);

    public async Task RemoveAsync(string id) =>
        await _articleCollection.DeleteOneAsync(article => article.Id == id);

    public async Task<bool> IsExistArticleAsync(string id) =>
        await _articleCollection.Find(article => article.Id == id).FirstOrDefaultAsync() != null;
}