using Microsoft.AspNetCore.Http.HttpResults;
using notesBE.Models;
using notesBE.Services;
using Microsoft.AspNetCore.Mvc;

namespace notesBE.Controller;

[ApiController]
[Route("api/[controller]")]
public class ArticlesController : ControllerBase
{
    private readonly ArticlesService _articlesService;

    public ArticlesController(ArticlesService articlesService) => 
        _articlesService = articlesService;

    [HttpGet]
    public async Task<List<Article>> Get()=>
        await _articlesService.GetAsync();
    

    [HttpGet("{id:length(24)}")]
    public async Task<ActionResult<Article>> Get(string id)
    {
        var article = await _articlesService.GetAsync(id);

        if (article is null)
            return NotFound();
        
        Console.WriteLine("get");
        return article;
    }

    [HttpPost]
    public async Task<IActionResult> Post(Article newArticle)
    {
        await _articlesService.CreateAsync(newArticle);

        return CreatedAtAction(nameof(Get), new { id = newArticle.Id }, newArticle);
    }

    [HttpPut("{id:length(24)}")]
    public async Task<IActionResult> Update(string id, Article updateArticle)
    {
        var article = await _articlesService.GetAsync(id);

        if (article is null)
            return NotFound();

        updateArticle.Id = id;

        await _articlesService.UpdateAsync(updateArticle);

        return NoContent();
    }

    [HttpDelete("{id:length(24)}")]
    public async Task<IActionResult> Delete(string id)
    {
        if (!await _articlesService.IsExistArticleAsync(id))
            return NotFound();
        
        await _articlesService.RemoveAsync(id);

        return NoContent();
    }
}