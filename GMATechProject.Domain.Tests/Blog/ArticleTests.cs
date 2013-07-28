
using System;
using System.Collections.Generic;
using System.Configuration;
using GMATechProject.Domain.Blog;
using NUnit.Framework;

namespace GMATechProject.Domain.Tests.Blog
{
	/// <summary>
	/// Description of ArticleTests.
	/// </summary>
    [TestFixture]
    public class ArticleTests : TestBase
	{
		IArticleRepository _Repository = new ArticleRepository();
    	
		public ArticleTests()
		{
		}
		
        [Test]
		public void CanAddMinimalArticle()
		{
			var article = new Article();
			article.Title = "test Title";
			article.Description = "test Description";
			article.Content = "test Content";
			
			var dbArticle = _Repository.Add(article);
			
			Assert.AreEqual(article.Title, dbArticle.Title);
			Assert.AreEqual(article.Description, dbArticle.Description);
			Assert.AreEqual(article.Content, dbArticle.Content);
			Assert.IsNull(article.SeoTitle);
			Assert.IsNull(article.SeoDescription);
			Assert.IsNull(article.SeoKeywords);
			Assert.IsNull(article.PublicationDate);
			
			Assert.IsNotNull(dbArticle.Id);
			
            //_server.DropDatabase("test");
		}

        [Test]
		public void CanGetArticlePage()
		{
			var articles = new List<Article>();
			
			for (int i = 0; i < 49; i++) {
				var article = new Article();
				article.Title = "test Title " + i;
				article.Description = "test Description" + i;
				article.Content = "test Content" + i;
				article.PublicationDate = DateTime.Now.AddDays(-1);
				article.PublicationState = PublicationState.Published;
				
				var dbArticle = _Repository.Add(article);
			}
			
			for (int i = 0; i < 5; i++) {
				var page = _Repository.All().ToPaginatedList(i, 10);
			
				Assert.IsNotNull(page);
				Assert.AreEqual(i, page.PageIndex, "Page index is incorrect");
				Assert.AreEqual(10, page.PageSize, "Page size is incorrect");
				Assert.AreEqual(49, page.Total, "Total is incorrect");
				Assert.AreEqual(page.PageIndex < 4 ? 10 : 9, page.Items.Count, "Items count is incorrect");
			}
		}
	}
}
