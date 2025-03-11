using Grpc.Core;
using Genova.ContentService.Protos;

namespace Genova.ContentService.Services;

public class ContentService : Content.ContentBase
{

    public override Task<PingResponse> Ping(PingRequest request, ServerCallContext context)
    {
        return Task.FromResult(new PingResponse { Message = "ContentService is alive!" });
    }

    public override Task<ContentResponse> FetchContent
        (ContentRequest request, ServerCallContext context)
    {
        string jsonResponse = """
        {
            "id": "p-123",
            "type": "Page",
            "slug": "/about-us",
            "fields": [
                {
                    "name": "title",
                    "type": "TextField",
                    "value": "About Us"
                },
                {
                    "name": "metaDescription",
                    "type": "TextField",
                    "value": "Learn more about our company..."
                }
            ],
            "components": [
                {
                    "id": "comp-1",
                    "type": "HeroBanner",
                    "order": 1,
                    "fields": [
                        {
                            "name": "headline",
                            "type": "TextField",
                            "value": "Welcome to Our Company"
                        },
                        {
                            "name": "backgroundImage",
                            "type": "ImageField",
                            "value": "https://example.com/images/banner.jpg"
                        }
                    ]
                },
                {
                    "id": "comp-2",
                    "type": "TextBlock",
                    "order": 2,
                    "fields": [
                        {
                            "name": "body",
                            "type": "RichTextField",
                            "value": "<p>We are an <strong>award-winning</strong> company.</p>"
                        }
                    ]
                },
                {
                    "id": "comp-3",
                    "type": "Gallery",
                    "order": 3,
                    "fields": [
                        {
                            "name": "images",
                            "type": "ListField",
                            "value": [
                                {
                                    "type": "ImageField",
                                    "value": "https://example.com/images/photo1.jpg"
                                },
                                {
                                    "type": "ImageField",
                                    "value": "https://example.com/images/photo2.jpg"
                                },
                                {
                                    "type": "ImageField",
                                    "value": "https://example.com/images/photo3.jpg"
                                }
                            ]
                        }
                    ]
                }
            ]
        }
        """;

        return Task.FromResult(new ContentResponse { Content = jsonResponse });
    }
}