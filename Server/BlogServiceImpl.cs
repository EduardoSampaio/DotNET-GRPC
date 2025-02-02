using Blog;
using Grpc.Core;
using MongoDB.Bson;
using MongoDB.Driver;
using static Blog.BlogService;

namespace server;
public class BlogServiceImpl : BlogServiceBase
{
    private static MongoClient mongoClient = new("mongodb://192.168.1.100:27017");
    private static IMongoDatabase database = mongoClient.GetDatabase("mydb");
    private static IMongoCollection<BsonDocument> collection = database.GetCollection<BsonDocument>("blogs");

    public override Task<CreateBlogResponse> CreateBlog(CreateBlogRequest request, ServerCallContext context)
    {
        var blog = request.Blog;
        var document = new BsonDocument
         {
              {"author_id", blog.AuthorId},
              {"title", blog.Title},
              {"content", blog.Content}
         };
        collection.InsertOne(document);
        var id = document.GetValue("_id").ToString();
        return Task.FromResult(new CreateBlogResponse { Blog = new Blog.Blog { Id = id, AuthorId = blog.AuthorId, Title = blog.Title, Content = blog.Content } });
    }

    public override Task<ReadBlogResponse> ReadBlog(ReadBlogRequest request, ServerCallContext context)
    {
        var filter = Builders<BsonDocument>.Filter.Eq("_id", new ObjectId(request.Id));
        var document = collection.Find(filter).FirstOrDefault();
        if (document == null)
        {
            throw new RpcException(new Status(StatusCode.NotFound, $"Blog with id {request.Id} not found"));
        }
        var blog = new Blog.Blog
        {
            Id = document.GetValue("_id").ToString(),
            AuthorId = document.GetValue("author_id").AsString,
            Title = document.GetValue("title").AsString,
            Content = document.GetValue("content").AsString
        };
        return Task.FromResult(new ReadBlogResponse { Blog = blog });
    }

    public override Task<UpdateBlogResponse> UpdateBlog(UpdateBlogRequest request, ServerCallContext context)
    {
        var blog = request.Blog;
        var filter = Builders<BsonDocument>.Filter.Eq("_id", new ObjectId(blog.Id));
        var update = Builders<BsonDocument>.Update
            .Set("author_id", blog.AuthorId)
            .Set("title", blog.Title)
            .Set("content", blog.Content);
        var result = collection.UpdateOne(filter, update);
        if (result.MatchedCount == 0)
        {
            throw new RpcException(new Status(StatusCode.NotFound, $"Blog with id {blog.Id} not found"));
        }
        return Task.FromResult(new UpdateBlogResponse { Blog = blog });
    }

    public override Task<DeleteBlogResponse> DeleteBlog(DeleteBlogRequest request, ServerCallContext context)
    {
        var filter = Builders<BsonDocument>.Filter.Eq("_id", new ObjectId(request.Id));
        var result = collection.DeleteOne(filter);
        if (result.DeletedCount == 0)
        {
            throw new RpcException(new Status(StatusCode.NotFound, $"Blog with id {request.Id} not found"));
        }
        return Task.FromResult(new DeleteBlogResponse { Id = request.Id });
    }

    public override async Task ListBlog(ListBlogRequest request, IServerStreamWriter<ListBlogResponse> responseStream, ServerCallContext context)
    {
        var filter = Builders<BsonDocument>.Filter.Empty;
        var documents = collection.Find(filter).ToList();
        foreach (var document in documents)
        {
            var blog = new Blog.Blog
            {
                Id = document.GetValue("_id").ToString(),
                AuthorId = document.GetValue("author_id").AsString,
                Title = document.GetValue("title").AsString,
                Content = document.GetValue("content").AsString
            };
            await responseStream.WriteAsync(new ListBlogResponse { Blog = blog });
        }
    }
}
